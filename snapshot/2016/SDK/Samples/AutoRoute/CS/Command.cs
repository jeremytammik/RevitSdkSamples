//
// (C) Copyright 2003-2015 by Autodesk, Inc.
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted,
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE. AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
//


using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using Autodesk.Revit;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Mechanical;
using GXYZ = Autodesk.Revit.DB.XYZ;

namespace Revit.SDK.Samples.AutoRoute.CS
{
    /// <summary>
    /// route a set of ducts and fittings between a base air supply equipment and 2 terminals.
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class Command : IExternalCommand
    {
        /// <summary>
        /// The revit application
        /// </summary>
        private static Application m_application;

        /// <summary>
        /// The current document of the application
        /// </summary>
        private static Document m_document;

        /// <summary>
        /// The mechanical system that will be created
        /// </summary>
        private MechanicalSystem m_mechanicalSystem;

        /// <summary>
        /// The type of the ducts in the system
        /// </summary>
        DuctType dtRectangle;

        /// <summary>
        /// Minimum length of a fitting
        /// </summary>
        private const double min1FittingLength = 1;

        /// <summary>
        /// Minimum length of a duct
        /// </summary>
        private const double minDuctLength = 0.5;

        /// <summary>
        /// The vertical offset of the highest connector to the trunk ducts
        /// </summary>
        private const double verticalTrunkOffset = 15;

        /// <summary>
        /// Optional distance from trunk to the boundary of system bounding box 
        /// used when failed to lay out ducts in the region of bounding box
        /// </summary>
        private const double horizontalOptionalTrunkOffset = 5;

        /// <summary>
        /// Minimum length of 2 fittings
        /// </summary>
        private const double min2FittingsLength = min1FittingLength * 2;

        /// <summary>
        /// The minimum length of 1 duct with 2 fittings
        /// </summary>
        private const double min1Duct2FittingsLength = min1FittingLength * 2 + minDuctLength;

        #region IExternalCommand Members
        /// <summary>
        /// Implement this method as an external command for Revit.
        /// </summary>
        /// <param name="commandData">An object that is passed to the external application 
        /// which contains data related to the command, 
        /// such as the application object and active view.</param>
        /// <param name="message">A message that can be set by the external application 
        /// which will be displayed if a failure or cancellation is returned by 
        /// the external command.</param>
        /// <param name="elements">A set of elements to which the external application 
        /// can add elements that are to be highlighted in case of failure or cancellation.</param>
        /// <returns>Return the status of the external command. 
        /// A result of Succeeded means that the API external method functioned as expected. 
        /// Cancelled can be used to signify that the user cancelled the external operation 
        /// at some point. Failure should be returned if the application is unable to proceed with 
        /// the operation.</returns>
        public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData, ref string message,
            ElementSet elements)
        {
            // set out default result to Failed.
            Autodesk.Revit.UI.Result retRes = Autodesk.Revit.UI.Result.Failed;

            m_application = commandData.Application.Application;
            m_document = commandData.Application.ActiveUIDocument.Document;
            Trace.Listeners.Clear();
            Trace.AutoFlush = true;

            string outputFileName = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "AutoRoute.log");
            if (File.Exists(outputFileName))
            {
                File.Delete(outputFileName);
            }
            TextWriterTraceListener listener = new TextWriterTraceListener(outputFileName);
            Trace.Listeners.Add(listener);

            Transaction transaction = new Transaction(m_document, "Sample_AutoRoute");
            try
            {
                transaction.Start();

                //Lists to temporarily record the created elements
                List<Duct> ducts = new List<Duct>();
                List<GXYZ> points = new List<GXYZ>();
                List<Connector> connectors = new List<Connector>();
                List<Connector> baseConnectors = new List<Connector>();

                //Get the connectors and bounding boxes
                List<Autodesk.Revit.DB.ElementId> ids = new List<ElementId>();
                ids.Add(new ElementId(378728));
                ids.Add(new ElementId(378707));
                ids.Add(new ElementId(378716));

                FamilyInstance[] instances = new FamilyInstance[3];
                Autodesk.Revit.DB.BoundingBoxXYZ[] boxes = new Autodesk.Revit.DB.BoundingBoxXYZ[3];
                Connector[] conns = new Connector[3];
                ConnectorSetIterator csi = null;
                for (int i = 0; i < ids.Count; ++i)
                {
                    Element element = m_document.GetElement(ids[i]);
                    if (null == element)
                    {
                        message = "Element " + ids[i].IntegerValue + " can't be found.";
                        return Autodesk.Revit.UI.Result.Failed;
                    }
                    instances[i] = element as FamilyInstance;
                    csi = ConnectorInfo.GetConnectors(ids[i].IntegerValue).ForwardIterator();
                    csi.MoveNext();
                    conns[i] = csi.Current as Connector;
                    boxes[i] = instances[i].get_BoundingBox(m_document.ActiveView);
                }

                //Find the "Out" and "SupplyAir" connector on the base equipment
                csi = ConnectorInfo.GetConnectors(ids[0].IntegerValue).ForwardIterator();
                while (csi.MoveNext())
                {
                    Connector conn = csi.Current as Connector;
                    if (conn.Direction == FlowDirectionType.Out && conn.DuctSystemType == DuctSystemType.SupplyAir)
                    {
                        conns[0] = conn;
                    }
                }

                //Create a mechanical system with a base air supply equipment and 2 terminals.
                m_mechanicalSystem = CreateMechanicalSystem(
                    //[378728][SupplyAir][Out][RectProfile][OST_MechanicalEquipment]
                        new ConnectorInfo(378728, conns[0].Origin.X, conns[0].Origin.Y, conns[0].Origin.Z),
                    new ConnectorInfo[]{
                        //[378707][SupplyAir][In][RectProfile]
                        new ConnectorInfo(378707, conns[1].Origin.X, conns[1].Origin.Y, conns[1].Origin.Z),
                        //[378716][SupplyAir][In][RectProfile]
                        new ConnectorInfo(378716, conns[2].Origin.X, conns[2].Origin.Y, conns[2].Origin.Z)
                    },
                    DuctSystemType.SupplyAir
                );

                //Get the boundary of the system
                double minX = conns[0].Origin.X;
                double minY = conns[0].Origin.Y;
                double maxX = conns[0].Origin.X;
                double maxY = conns[0].Origin.Y;
                double maxZ = conns[0].Origin.Z;
                for (int i = 1; i < boxes.Length; ++i)
                {
                    if (conns[i].Origin.X < minX)
                        minX = conns[i].Origin.X;
                    if (conns[i].Origin.Y < minY)
                        minY = conns[i].Origin.Y;
                    if (conns[i].Origin.X > maxX)
                        maxX = conns[i].Origin.X;
                    if (conns[i].Origin.Y > maxY)
                        maxY = conns[i].Origin.Y;
                    if (conns[i].Origin.Z > maxZ)
                        maxZ = conns[i].Origin.Z;
                }

                //Calculate the optional values for the trunk ducts
                double midX = (minX + maxX) / 2;
                double midY = (minY + maxY) / 2;
                double[] baseXValues = new double[3] { midX, (minX + midX) / 2, (maxX + midX) / 2 };
                double[] baseYValues = new double[3] { midY, (minY + midY) / 2, (maxY + midY) / 2 };

                //Get the duct type for the ducts to be created
                Autodesk.Revit.DB.ElementId ductTypeId = new Autodesk.Revit.DB.ElementId(139191);
                dtRectangle = m_document.GetElement(ductTypeId) as DuctType;


                //Create the ducts and elbows that connect the base mechanical equipment
                GXYZ connectorDirection = conns[0].CoordinateSystem.BasisZ;

                if (0 == connectorDirection.DistanceTo(new GXYZ(-1, 0, 0)))
                {
                    points.Add(new GXYZ(conns[0].Origin.X - min1FittingLength, conns[0].Origin.Y, conns[0].Origin.Z));
                    points.Add(new GXYZ(conns[0].Origin.X - min2FittingsLength, conns[0].Origin.Y, conns[0].Origin.Z + min1FittingLength));
                    points.Add(new GXYZ(conns[0].Origin.X - min2FittingsLength, conns[0].Origin.Y, maxZ + verticalTrunkOffset - min1FittingLength));
                }
                else if (0 == connectorDirection.DistanceTo(new GXYZ(1, 0, 0)))
                {
                    points.Add(new GXYZ(conns[0].Origin.X + min1FittingLength, conns[0].Origin.Y, conns[0].Origin.Z));
                    points.Add(new GXYZ(conns[0].Origin.X + min2FittingsLength, conns[0].Origin.Y, conns[0].Origin.Z + min1FittingLength));
                    points.Add(new GXYZ(conns[0].Origin.X + min2FittingsLength, conns[0].Origin.Y, maxZ + verticalTrunkOffset - min1FittingLength));
                }
                else if (0 == connectorDirection.DistanceTo(new GXYZ(0, -1, 0)))
                {
                    points.Add(new GXYZ(conns[0].Origin.X, conns[0].Origin.Y - min1FittingLength, conns[0].Origin.Z));
                    points.Add(new GXYZ(conns[0].Origin.X, conns[0].Origin.Y - min2FittingsLength, conns[0].Origin.Z + min1FittingLength));
                    points.Add(new GXYZ(conns[0].Origin.X, conns[0].Origin.Y - min2FittingsLength, maxZ + verticalTrunkOffset - min1FittingLength));
                }
                else if (0 == connectorDirection.DistanceTo(new GXYZ(0, 1, 0)))
                {
                    points.Add(new GXYZ(conns[0].Origin.X, conns[0].Origin.Y + min1FittingLength, conns[0].Origin.Z));
                    points.Add(new GXYZ(conns[0].Origin.X, conns[0].Origin.Y + min2FittingsLength, conns[0].Origin.Z + min1FittingLength));
                    points.Add(new GXYZ(conns[0].Origin.X, conns[0].Origin.Y + min2FittingsLength, maxZ + verticalTrunkOffset - min1FittingLength));
                }
                ducts.Add(m_document.Create.NewDuct(points[0], conns[0], dtRectangle));
                ducts.Add(m_document.Create.NewDuct(points[1], points[2], dtRectangle));
                connectors.Add(ConnectorInfo.GetConnector(ducts[0].Id.IntegerValue, points[0]));
                connectors.Add(ConnectorInfo.GetConnector(ducts[1].Id.IntegerValue, points[1]));
                connectors.Add(ConnectorInfo.GetConnector(ducts[1].Id.IntegerValue, points[2]));
                connectors[0].ConnectTo(connectors[1]);
                m_document.Create.NewElbowFitting(connectors[0], connectors[1]);
                baseConnectors.Add(connectors[2]);

                //Create the vertical ducts for terminals
                points.Clear();
                ducts.Clear();

                points.Add(new GXYZ(conns[1].Origin.X, conns[1].Origin.Y, maxZ + verticalTrunkOffset - min1FittingLength));
                points.Add(new GXYZ(conns[2].Origin.X, conns[2].Origin.Y, maxZ + verticalTrunkOffset - min1FittingLength));
                ducts.Add(m_document.Create.NewDuct(points[0], conns[1], dtRectangle));
                ducts.Add(m_document.Create.NewDuct(points[1], conns[2], dtRectangle));
                baseConnectors.Add(ConnectorInfo.GetConnector(ducts[0].Id.IntegerValue, points[0]));
                baseConnectors.Add(ConnectorInfo.GetConnector(ducts[1].Id.IntegerValue, points[1]));

                //Connect the system by creating the trunk line of ducts and connect them to the base connectors
                SortConnectorsByX(baseConnectors);
                for (int i = 0; i < baseYValues.Length; ++i)
                {
                    if (ConnectSystemOnXAxis(baseConnectors, baseYValues[i]))
                    {
                        LogUtility.WriteMechanicalSystem(m_mechanicalSystem);
                        return Autodesk.Revit.UI.Result.Succeeded;
                    }
                }

                SortConnectorsByY(baseConnectors);
                for (int i = 0; i < baseXValues.Length; ++i)
                {
                    if (ConnectSystemOnYAxis(baseConnectors, baseXValues[i]))
                    {
                        LogUtility.WriteMechanicalSystem(m_mechanicalSystem);
                        return Autodesk.Revit.UI.Result.Succeeded;
                    }
                }

                //If all the cases fail to route the system, try the trunks out of the bounding box
                SortConnectorsByX(baseConnectors);
                if (ConnectSystemOnXAxis(baseConnectors, maxY + horizontalOptionalTrunkOffset))
                {
                    LogUtility.WriteMechanicalSystem(m_mechanicalSystem);
                    return Autodesk.Revit.UI.Result.Succeeded;
                }

                SortConnectorsByY(baseConnectors);
                if (ConnectSystemOnYAxis(baseConnectors, maxX + horizontalOptionalTrunkOffset))
                {
                    LogUtility.WriteMechanicalSystem(m_mechanicalSystem);
                    return Autodesk.Revit.UI.Result.Succeeded;
                }

                //If there's no path for the connection, choose one path and let Revit report the error
                connectors.Clear();
                SortConnectorsByX(baseConnectors);
                connectors.AddRange(CreateDuct(new GXYZ(baseConnectors[0].Origin.X + min1FittingLength, baseYValues[0], maxZ + verticalTrunkOffset), new GXYZ(baseConnectors[1].Origin.X - min1FittingLength, baseYValues[0], maxZ + verticalTrunkOffset)));
                connectors.AddRange(CreateDuct(new GXYZ(baseConnectors[1].Origin.X + min1FittingLength, baseYValues[0], maxZ + verticalTrunkOffset), new GXYZ(baseConnectors[2].Origin.X - min1FittingLength, baseYValues[0], maxZ + verticalTrunkOffset)));
                ConnectWithElbowFittingOnXAxis(baseConnectors[0], connectors[0]);
                ConnectWithElbowFittingOnXAxis(baseConnectors[2], connectors[3]);
                ConnectWithTeeFittingOnXAxis(baseConnectors[1], connectors[1], connectors[2], false);

            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
                message = ex.Message;
                retRes = Autodesk.Revit.UI.Result.Failed;
            }
            finally
            {
                transaction.Commit();
                Trace.Flush();
                listener.Close();
                Trace.Close();
                Trace.Listeners.Remove(listener);
            }
            return retRes;
        }
        #endregion

        /// <summary>
        /// Connect the system with a trunk line of ducts on X axis
        /// </summary>
        /// <param name="baseConnectors">the upper connectors of the vertical ducts that derived from the terminals and the base equipment</param>
        /// <param name="baseY">the y value of the trunk line</param>
        /// <returns>
        /// true if the system can be connected
        /// false if the system cannot be connected
        /// </returns>
        private bool ConnectSystemOnXAxis(List<Connector> baseConnectors, double baseY)
        {
            //Check the count of the base connectors
            if (null == baseConnectors || 3 != baseConnectors.Count)
            {
                return false;
            }
            for (int i = 0; i < baseConnectors.Count; ++i)
            {
                //Check the distance of the connector from the trunk
                if (baseConnectors[i].Origin.Y != baseY && Math.Abs(baseConnectors[i].Origin.Y - baseY) < min1Duct2FittingsLength)
                {
                    return false;
                }
                //Check the distance of the connectors on X axis
                for (int j = i + 1; j < baseConnectors.Count; ++j)
                {
                    if (baseConnectors[j].Origin.X != baseConnectors[i].Origin.X && baseConnectors[j].Origin.X - baseConnectors[i].Origin.X < min2FittingsLength)
                    {
                        return false;
                    }
                }
            }
            try
            {
                double baseZ = baseConnectors[0].Origin.Z + min1FittingLength;
                //Create the ducts and elbow fittings to connect the vertical ducts and the trunk ducts
                List<Connector> connectors = new List<Connector>();

                if (baseConnectors[0].Origin.X == baseConnectors[1].Origin.X)
                {
                    //All 3 connectors are with the same X value
                    if (baseConnectors[1].Origin.X == baseConnectors[2].Origin.X)
                    {
                        return false;
                    }
                    else
                    {
                        //The 1st and 2nd base connectors are on the same side of the trunk
                        if (Math.Sign(baseConnectors[0].Origin.Y - baseY) * Math.Sign(baseConnectors[1].Origin.Y - baseY) == 1)
                        {
                            return false;
                        }

                        //Create the trunk
                        connectors = CreateDuct(new GXYZ(baseConnectors[0].Origin.X + min1FittingLength, baseY, baseZ), new GXYZ(baseConnectors[2].Origin.X - min1FittingLength, baseY, baseZ));

                        //Create a tee fitting connecting the 1st and 2nd base connectors to the trunk
                        ConnectWithTeeFittingOnXAxis(baseConnectors[0], baseConnectors[1], connectors[0], true);

                        //Create an elbow fitting connection the 3rd base connector to the trunk
                        ConnectWithElbowFittingOnXAxis(baseConnectors[2], connectors[1]);
                    }
                }
                else
                {
                    //Create the segment of duct on the trunk to be connected to the 1st base connector
                    connectors = CreateDuct(new GXYZ(baseConnectors[0].Origin.X + min1FittingLength, baseY, baseZ), new GXYZ(baseConnectors[1].Origin.X - min1FittingLength, baseY, baseZ));

                    //Create an elbow fitting connection the 1st base connector with the trunk
                    ConnectWithElbowFittingOnXAxis(baseConnectors[0], connectors[0]);

                    if (baseConnectors[1].Origin.X == baseConnectors[2].Origin.X)
                    {
                        //The 2nd and 3rd connectors are on the same side of the trunk
                        if (Math.Sign(baseConnectors[1].Origin.Y - baseY) * Math.Sign(baseConnectors[2].Origin.Y - baseY) == 1)
                        {
                            return false;
                        }
                        //Create a tee fitting connecting the 2nd and 3rd base connectors to the trunk
                        ConnectWithTeeFittingOnXAxis(baseConnectors[1], baseConnectors[2], connectors[1], true);
                    }
                    else
                    {
                        connectors.AddRange(CreateDuct(new GXYZ(baseConnectors[1].Origin.X + min1FittingLength, baseY, baseZ), new GXYZ(baseConnectors[2].Origin.X - min1FittingLength, baseY, baseZ)));
                        //Create a tee fitting connecting the 2nd base connector to the trunk
                        ConnectWithTeeFittingOnXAxis(baseConnectors[1], connectors[1], connectors[2], false);
                        //Create an elbow fitting connection the 3rd base connector to the trunk
                        ConnectWithElbowFittingOnXAxis(baseConnectors[2], connectors[3]);
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Connect a base connector to a connector on the trunk with an elbow fitting
        /// </summary>
        /// <param name="baseConn">the upper connector of the vertical duct that derived from a terminal or the base equipment</param>
        /// <param name="conn">the connector of a duct on the trunk</param>
        private void ConnectWithElbowFittingOnXAxis(Connector baseConn, Connector conn)
        {
            double baseY = conn.Origin.Y;
            double baseZ = conn.Origin.Z;
            List<Connector> connectors = new List<Connector>();

            //If the distance of the two connectors on the Y axis is greater than 2, create a duct on the Y axis and then connect it to the 2 connectors with elbow fittings
            if (Math.Abs(baseConn.Origin.Y - baseY) > min1Duct2FittingsLength)
            {
                connectors.AddRange(CreateDuct(new GXYZ(baseConn.Origin.X, baseConn.Origin.Y - Math.Sign(baseConn.Origin.Y - baseY), baseZ), new GXYZ(baseConn.Origin.X, baseY + Math.Sign(baseConn.Origin.Y - baseY), baseZ)));
                connectors[0].ConnectTo(baseConn);
                m_document.Create.NewElbowFitting(connectors[0], baseConn);
                connectors[1].ConnectTo(conn);
                m_document.Create.NewElbowFitting(connectors[1], conn);
            }
            //If the distance of the two connectors on the Y axis is less than 2, connect them with an elbow fitting
            else
            {
                baseConn.ConnectTo(conn);
                m_document.Create.NewElbowFitting(baseConn, conn);
            }
        }

        /// <summary>
        /// Connect 3 connectors on the trunk with a tee fitting
        /// </summary>
        /// <param name="conn1">the first connector</param>
        /// <param name="conn2">the second connector</param>
        /// <param name="conn3">the third connector</param>
        /// <param name="flag">a flag to indicate whether there are 2 base connectors or 1 base connector</param>
        private void ConnectWithTeeFittingOnXAxis(Connector conn1, Connector conn2, Connector conn3, bool flag)
        {
            double baseY = conn3.Origin.Y;
            double baseZ = conn3.Origin.Z;

            List<GXYZ> points = new List<GXYZ>();
            List<Duct> ducts = new List<Duct>();
            List<Connector> connectors = new List<Connector>();

            //Connect two base connectors to a connector on the trunk
            if (true == flag)
            {

                Connector baseConn1 = conn1;
                Connector baseConn2 = conn2;
                Connector conn = conn3;

                connectors.AddRange(CreateDuct(new GXYZ(baseConn1.Origin.X, baseConn1.Origin.Y - Math.Sign(baseConn1.Origin.Y - baseY), baseZ), new GXYZ(baseConn1.Origin.X, baseY + Math.Sign(baseConn1.Origin.Y - baseY), baseZ)));
                connectors.AddRange(CreateDuct(new GXYZ(baseConn2.Origin.X, baseConn2.Origin.Y - Math.Sign(baseConn2.Origin.Y - baseY), baseZ), new GXYZ(baseConn2.Origin.X, baseY + Math.Sign(baseConn2.Origin.Y - baseY), baseZ)));

                connectors[0].ConnectTo(baseConn1);
                connectors[2].ConnectTo(baseConn2);
                m_document.Create.NewElbowFitting(connectors[0], baseConn1);
                m_document.Create.NewElbowFitting(connectors[2], baseConn2);

                connectors[1].ConnectTo(connectors[3]);
                connectors[1].ConnectTo(conn);
                connectors[3].ConnectTo(conn);
                m_document.Create.NewTeeFitting(connectors[1], connectors[3], conn);
            }
            //Connect a base connector to two connectors on the trunk
            else
            {
                Connector baseConn = conn1;

                if (Math.Abs(baseConn.Origin.Y - baseY) > min1Duct2FittingsLength)
                {
                    connectors.AddRange(CreateDuct(new GXYZ(baseConn.Origin.X, baseConn.Origin.Y - Math.Sign(baseConn.Origin.Y - baseY), baseZ), new GXYZ(baseConn.Origin.X, baseY + Math.Sign(baseConn.Origin.Y - baseY), baseZ)));
                    baseConn.ConnectTo(connectors[0]);
                    m_document.Create.NewElbowFitting(connectors[0], baseConn);

                    connectors[1].ConnectTo(conn2);
                    connectors[1].ConnectTo(conn3);
                    conn2.ConnectTo(conn3);
                    m_document.Create.NewTeeFitting(conn2, conn3, connectors[1]);
                }
                else
                {
                    baseConn.ConnectTo(conn2);
                    baseConn.ConnectTo(conn3);
                    conn2.ConnectTo(conn3);
                    m_document.Create.NewTeeFitting(conn2, conn3, baseConn);
                }
            }
        }

        /// <summary>
        /// Sort the base connectors by their x values
        /// </summary>
        /// <param name="connectors">the connectors to be sorted</param>
        private void SortConnectorsByX(List<Connector> connectors)
        {
            for (int i = 0; i < connectors.Count; ++i)
            {
                double min = connectors[i].Origin.X;
                int minIndex = i;
                for (int j = i; j < connectors.Count; ++j)
                {
                    if (connectors[j].Origin.X < min)
                    {
                        min = connectors[j].Origin.X;
                        minIndex = j;
                    }
                }
                Connector t = connectors[i];
                connectors[i] = connectors[minIndex];
                connectors[minIndex] = t;
            }
        }

        /// <summary>
        /// Connect the system with a trunk line of ducts on Y axis
        /// </summary>
        /// <param name="baseConnectors">the upper connectors of the vertical ducts that derived from the terminals and the base equipment</param>
        /// <param name="baseX">the x value of the trunk line</param>
        /// <returns>
        /// true if the system can be connected
        /// false if the system cannot be connected
        /// </returns>
        private bool ConnectSystemOnYAxis(List<Connector> baseConnectors, double baseX)
        {
            //Check the count of the base connectors
            if (null == baseConnectors || 3 != baseConnectors.Count)
            {
                return false;
            }
            for (int i = 0; i < baseConnectors.Count; ++i)
            {
                //Check the distance of the connector from the trunk
                if (baseConnectors[i].Origin.X != baseX && Math.Abs(baseConnectors[i].Origin.X - baseX) < min1Duct2FittingsLength)
                {
                    return false;
                }
                //Check the distance of the connectors on Y axis
                for (int j = i + 1; j < baseConnectors.Count; ++j)
                {
                    if (baseConnectors[j].Origin.Y != baseConnectors[i].Origin.Y && baseConnectors[j].Origin.Y - baseConnectors[i].Origin.Y < min2FittingsLength)
                    {
                        return false;
                    }
                }
            }
            try
            {
                double baseZ = baseConnectors[0].Origin.Z + min1FittingLength;
                //Create the ducts and elbow fittings to connect the vertical ducts and the trunk ducts
                List<Connector> connectors = new List<Connector>();

                if (baseConnectors[0].Origin.Y == baseConnectors[1].Origin.Y)
                {
                    //All 3 connectors are with the same Y value
                    if (baseConnectors[1].Origin.Y == baseConnectors[2].Origin.Y)
                    {
                        return false;
                    }
                    else
                    {
                        //The 1st and 2nd base connectors are on the same side of the trunk
                        if (Math.Sign(baseConnectors[0].Origin.X - baseX) * Math.Sign(baseConnectors[1].Origin.X - baseX) == 1)
                        {
                            return false;
                        }

                        //Create the trunk
                        connectors = CreateDuct(new GXYZ(baseX, baseConnectors[0].Origin.Y + min1FittingLength, baseZ), new GXYZ(baseX, baseConnectors[2].Origin.Y - min1FittingLength, baseZ));

                        //Create a tee fitting connecting the 1st and 2nd base connectors to the trunk
                        ConnectWithTeeFittingOnYAxis(baseConnectors[0], baseConnectors[1], connectors[0], true);

                        //Create an elbow fitting connection the 3rd base connector to the trunk
                        ConnectWithElbowFittingOnYAxis(baseConnectors[2], connectors[1]);
                    }
                }
                else
                {
                    //Create the segment of duct on the trunk to be connected to the 1st base connector
                    connectors = CreateDuct(new GXYZ(baseX, baseConnectors[0].Origin.Y + min1FittingLength, baseZ), new GXYZ(baseX, baseConnectors[1].Origin.Y - min1FittingLength, baseZ));

                    //Create an elbow fitting connection the 1st base connector with the trunk
                    ConnectWithElbowFittingOnYAxis(baseConnectors[0], connectors[0]);

                    if (baseConnectors[1].Origin.Y == baseConnectors[2].Origin.Y)
                    {
                        //The 2nd and 3rd connectors are on the same side of the trunk
                        if (Math.Sign(baseConnectors[1].Origin.X - baseX) * Math.Sign(baseConnectors[2].Origin.X - baseX) == 1)
                        {
                            return false;
                        }
                        //Create a tee fitting connecting the 2nd and 3rd base connectors to the trunk
                        ConnectWithTeeFittingOnYAxis(baseConnectors[1], baseConnectors[2], connectors[1], true);
                    }
                    else
                    {
                        connectors.AddRange(CreateDuct(new GXYZ(baseX, baseConnectors[1].Origin.Y + min1FittingLength, baseZ), new GXYZ(baseX, baseConnectors[2].Origin.Y - min1FittingLength, baseZ)));
                        //Create a tee fitting connecting the 2nd base connector to the trunk
                        ConnectWithTeeFittingOnYAxis(baseConnectors[1], connectors[1], connectors[2], false);
                        //Create an elbow fitting connection the 3rd base connector to the trunk
                        ConnectWithElbowFittingOnYAxis(baseConnectors[2], connectors[3]);
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Connect a base connector to a connector on the trunk with an elbow fitting
        /// </summary>
        /// <param name="baseConn">the upper connector of the vertical duct that derived from a terminal or the base equipment</param>
        /// <param name="conn">the connector of a duct on the trunk</param>
        private void ConnectWithElbowFittingOnYAxis(Connector baseConn, Connector conn)
        {
            double baseX = conn.Origin.X;
            double baseZ = conn.Origin.Z;
            List<Connector> connectors = new List<Connector>();

            //If the distance of the two connectors on the X axis is greater than 2, create a duct on the X axis and then connect it to the 2 connectors with elbow fittings
            if (Math.Abs(baseConn.Origin.X - baseX) > min1Duct2FittingsLength)
            {
                connectors.AddRange(CreateDuct(new GXYZ(baseConn.Origin.X - Math.Sign(baseConn.Origin.X - baseX), baseConn.Origin.Y, baseZ), new GXYZ(baseX + Math.Sign(baseConn.Origin.X - baseX), baseConn.Origin.Y, baseZ)));
                connectors[0].ConnectTo(baseConn);
                m_document.Create.NewElbowFitting(connectors[0], baseConn);
                connectors[1].ConnectTo(conn);
                m_document.Create.NewElbowFitting(connectors[1], conn);
            }
            //If the distance of the two connectors on the X axis is less than 2, connect them with an elbow fitting
            else
            {
                baseConn.ConnectTo(conn);
                m_document.Create.NewElbowFitting(baseConn, conn);
            }
        }

        /// <summary>
        /// Connect 3 connectors on the trunk with a tee fitting
        /// </summary>
        /// <param name="conn1">the first connector</param>
        /// <param name="conn2">the second connector</param>
        /// <param name="conn3">the third connector</param>
        /// <param name="flag">a flag to indicate whether there are 2 base connectors or 1 base connector</param>
        private void ConnectWithTeeFittingOnYAxis(Connector conn1, Connector conn2, Connector conn3, bool flag)
        {
            double baseX = conn3.Origin.X;
            double baseZ = conn3.Origin.Z;

            List<GXYZ> points = new List<GXYZ>();
            List<Duct> ducts = new List<Duct>();
            List<Connector> connectors = new List<Connector>();

            //Connect two base connectors to a connector on the trunk
            if (true == flag)
            {

                Connector baseConn1 = conn1;
                Connector baseConn2 = conn2;
                Connector conn = conn3;

                connectors.AddRange(CreateDuct(new GXYZ(baseConn1.Origin.X - Math.Sign(baseConn1.Origin.X - baseX), baseConn1.Origin.Y, baseZ), new GXYZ(baseX + Math.Sign(baseConn1.Origin.X - baseX), baseConn1.Origin.Y, baseZ)));
                connectors.AddRange(CreateDuct(new GXYZ(baseConn2.Origin.X - Math.Sign(baseConn2.Origin.X - baseX), baseConn2.Origin.Y, baseZ), new GXYZ(baseX + Math.Sign(baseConn2.Origin.X - baseX), baseConn2.Origin.Y, baseZ)));

                connectors[0].ConnectTo(baseConn1);
                connectors[2].ConnectTo(baseConn2);
                m_document.Create.NewElbowFitting(connectors[0], baseConn1);
                m_document.Create.NewElbowFitting(connectors[2], baseConn2);

                connectors[1].ConnectTo(connectors[3]);
                connectors[1].ConnectTo(conn);
                connectors[3].ConnectTo(conn);
                m_document.Create.NewTeeFitting(connectors[1], connectors[3], conn);
            }
            //Connect a base connector to two connectors on the trunk
            else
            {
                Connector baseConn = conn1;

                if (Math.Abs(baseConn.Origin.X - baseX) > min1Duct2FittingsLength)
                {
                    connectors.AddRange(CreateDuct(new GXYZ(baseConn.Origin.X - Math.Sign(baseConn.Origin.X - baseX), baseConn.Origin.Y, baseZ), new GXYZ(baseX + Math.Sign(baseConn.Origin.X - baseX), baseConn.Origin.Y, baseZ)));
                    baseConn.ConnectTo(connectors[0]);
                    m_document.Create.NewElbowFitting(connectors[0], baseConn);

                    connectors[1].ConnectTo(conn2);
                    connectors[1].ConnectTo(conn3);
                    conn2.ConnectTo(conn3);
                    m_document.Create.NewTeeFitting(conn2, conn3, connectors[1]);
                }
                else
                {
                    baseConn.ConnectTo(conn2);
                    baseConn.ConnectTo(conn3);
                    conn2.ConnectTo(conn3);
                    m_document.Create.NewTeeFitting(conn2, conn3, baseConn);
                }
            }
        }

        /// <summary>
        /// Sort the base connectors by their y values
        /// </summary>
        /// <param name="connectors">the connectors to be sorted</param>
        private void SortConnectorsByY(List<Connector> connectors)
        {
            for (int i = 0; i < connectors.Count; ++i)
            {
                double min = connectors[i].Origin.Y;
                int minIndex = i;
                for (int j = i; j < connectors.Count; ++j)
                {
                    if (connectors[j].Origin.Y < min)
                    {
                        min = connectors[j].Origin.Y;
                        minIndex = j;
                    }
                }
                Connector t = connectors[i];
                connectors[i] = connectors[minIndex];
                connectors[minIndex] = t;
            }
        }

        /// <summary>
        /// Create a duct with two points
        /// </summary>
        /// <param name="point1">the first point</param>
        /// <param name="point2">the second point</param>
        /// <returns></returns>
        private List<Connector> CreateDuct(GXYZ point1, GXYZ point2)
        {
            List<Connector> connectors = new List<Connector>();

            Duct duct = m_document.Create.NewDuct(point1, point2, dtRectangle);

            connectors.Add(ConnectorInfo.GetConnector(duct.Id.IntegerValue, point1));
            connectors.Add(ConnectorInfo.GetConnector(duct.Id.IntegerValue, point2));

            return connectors;
        }

        /// <summary>
        /// Create a mechanical system
        /// </summary>
        /// <param name="baseConnector">the base connector of the mechanical system</param>
        /// <param name="connectors">the connectors of the mechanical system</param>
        /// <param name="systemType">the system type of the mechanical system</param>
        /// <returns>the created mechanical system</returns>
        private MechanicalSystem CreateMechanicalSystem(ConnectorInfo baseConnector, ConnectorInfo[] connectors, DuctSystemType systemType)
        {
            ConnectorSet cset = null;
            if (connectors != null)
            {
                cset = new ConnectorSet();
                foreach (ConnectorInfo ci in connectors)
                {
                    cset.Insert(ci.Connector);
                }
            }
            MechanicalSystem mechanicalSystem = m_document.Create.NewMechanicalSystem(baseConnector == null ? null : baseConnector.Connector, cset, systemType);
            return mechanicalSystem;
        }

        /// <summary>
        /// information of a connector
        /// </summary>
        public class ConnectorInfo
        {
            /// <summary>
            /// The owner's element ID
            /// </summary>
            int m_ownerId;

            /// <summary>
            /// The origin of the connector
            /// </summary>
            GXYZ m_origin;

            /// <summary>
            /// The Connector object
            /// </summary>
            Connector m_connector;

            /// <summary>
            /// The connector this object represents
            /// </summary>
            public Connector Connector
            {
                get { return m_connector; }
                set { m_connector = value; }
            }

            /// <summary>
            /// The owner ID of the connector
            /// </summary>
            public int OwnerId
            {
                get { return m_ownerId; }
                set { m_ownerId = value; }
            }

            /// <summary>
            /// The origin of the connector
            /// </summary>
            public GXYZ Origin
            {
                get { return m_origin; }
                set { m_origin = value; }
            }

            /// <summary>
            /// The constructor that finds the connector with the owner ID and origin
            /// </summary>
            /// <param name="ownerId">the ownerID of the connector</param>
            /// <param name="origin">the origin of the connector</param>
            public ConnectorInfo(int ownerId, GXYZ origin)
            {
                m_ownerId = ownerId;
                m_origin = origin;
                m_connector = ConnectorInfo.GetConnector(m_ownerId, origin);
            }

            /// <summary>
            /// The constructor that finds the connector with the owner ID and the values of the origin
            /// </summary>
            /// <param name="ownerId">the ownerID of the connector</param>
            /// <param name="x">the X value of the connector</param>
            /// <param name="y">the Y value of the connector</param>
            /// <param name="z">the Z value of the connector</param>
            public ConnectorInfo(int ownerId, double x, double y, double z)
                : this(ownerId, new GXYZ(x, y, z))
            {
            }

            /// <summary>
            /// Get the connector of the owner at the specific origin
            /// </summary>
            /// <param name="ownerId">the owner ID of the connector</param>
            /// <param name="connectorOrigin">the origin of the connector</param>
            /// <returns>if found, return the connector, or else return null</returns>
            public static Connector GetConnector(int ownerId, GXYZ connectorOrigin)
            {
                ConnectorSet connectors = GetConnectors(ownerId);
                foreach (Connector conn in connectors)
                {
                    if (conn.ConnectorType == ConnectorType.Logical)
                        continue;
                    if (conn.Origin.IsAlmostEqualTo(connectorOrigin))
                        return conn;
                }
                return null;
            }

            /// <summary>
            /// Get all the connectors of an element with a specific ID
            /// </summary>
            /// <param name="ownerId">the owner ID of the connector</param>
            /// <returns>the connector set which includes all the connectors found</returns>
            public static ConnectorSet GetConnectors(int ownerId)
            {
                Autodesk.Revit.DB.ElementId elementId = new ElementId(ownerId);
                Element element = m_document.GetElement(elementId);
                return GetConnectors(element);
            }

            /// <summary>
            /// Get all the connectors of a specific element
            /// </summary>
            /// <param name="element">the owner of the connector</param>
            /// <returns>if found, return all the connectors found, or else return null</returns>
            public static ConnectorSet GetConnectors(Autodesk.Revit.DB.Element element)
            {
                if (element == null) return null;
                FamilyInstance fi = element as FamilyInstance;
                if (fi != null && fi.MEPModel != null)
                {
                    return fi.MEPModel.ConnectorManager.Connectors;
                }
                MEPSystem system = element as MEPSystem;
                if (system != null)
                {
                    return system.ConnectorManager.Connectors;
                }

                MEPCurve duct = element as MEPCurve;
                if (duct != null)
                {
                    return duct.ConnectorManager.Connectors;
                }
                return null;
            }

            /// <summary>
            /// Find the two connectors of the specific ConnectorManager at the two locations
            /// </summary>
            /// <param name="connMgr">The ConnectorManager of the connectors to be found</param>
            /// <param name="ptn1">the location of the first connector</param>
            /// <param name="ptn2">the location of the second connector</param>
            /// <returns>The two connectors found</returns>
            public static Connector[] FindConnectors(ConnectorManager connMgr, GXYZ pnt1, GXYZ pnt2)
            {
                Connector[] result = new Connector[2];
                ConnectorSet conns = connMgr.Connectors;
                foreach (Connector conn in conns)
                {
                    if (conn.Origin.IsAlmostEqualTo(pnt1))
                    {
                        result[0] = conn;
                    }
                    else if (conn.Origin.IsAlmostEqualTo(pnt2))
                    {
                        result[1] = conn;
                    }
                }
                return result;
            }

        };
    }

    public class LogUtility
    {
        /// <summary>
        /// Invalid string.
        /// </summary>
        public const string InvalidString = "[!]";

        /// <summary>
        /// Write the information of an element to the log file
        /// </summary>
        /// <param name="element">the element whose information is to be written</param>
        public static void WriteElement(Element element)
        {
            WriteElement(element, true);
        }
        /// <summary>
        /// Write the information of an element to the log file
        /// </summary>
        /// <param name="element">the element whose information is to be written</param>
        /// <param name="writeId">whether the id will be outputted</param>
        public static void WriteElement(Element element, bool writeId)
        {
            if (element == null)
            {
                Trace.WriteLine("null"); return;
            }
            int elementId = element.Id.IntegerValue;
            int familyId = element.get_Parameter(BuiltInParameter.ELEM_FAMILY_PARAM).AsElementId().IntegerValue;
            string familyName = LogUtility.InvalidString;
            Element objectType = GetElement<Element>(element.Document, familyId);
            string elementName = LogUtility.InvalidString;
            try { elementName = element.Name; }
            catch { }
            if (objectType != null)
            {
                Parameter familyNameParameter = objectType.get_Parameter(BuiltInParameter.ALL_MODEL_FAMILY_NAME);
                if (familyNameParameter != null)
                    familyName = familyNameParameter.AsString();
            }
            BuiltInCategory category = (BuiltInCategory)(element.get_Parameter(BuiltInParameter.ELEM_CATEGORY_PARAM).AsElementId().IntegerValue);

            Trace.WriteLine("Type: " + element.GetType().FullName);
            Trace.WriteLine("Name: " + familyName + ":" + elementName);
            if (writeId) Trace.WriteLine("Id: " + elementId);
            Trace.WriteLine("Category: " + category);
            Trace.WriteLine("FamilyId: " + familyId);
        }

        /// <summary>
        /// Write the information of a mechanical system to the log file
        /// </summary>
        /// <param name="system">the mechanical system whose information is to be written</param>
        public static void WriteMechanicalSystem(MechanicalSystem system)
        {
            string flow = InvalidString;
            try { flow = system.Flow.ToString(); }
            catch (Exception) { }
            //string staticPressure = InvalidString;
            //try { staticPressure = system.StaticPressure.ToString(); }
            //catch (Exception) { }

            Trace.WriteLine("Flow: " + flow);
            Trace.WriteLine("IsWellConnected: " + system.IsWellConnected);
            //Trace.WriteLine("StaticPressure: " + staticPressure);
            Trace.WriteLine("SystemType: " + system.SystemType);
            Trace.WriteLine("+DuctNetwork");
            Trace.Indent();
            foreach (Element element in system.DuctNetwork)
            {
                LogUtility.WriteElement(element, false);
                Trace.WriteLine("");
            }
            Trace.Unindent();
            WriteMEPSystem(system);
        }

        /// <summary>
        /// Get element by its id and cast it to the specified type
        /// </summary>
        /// <param name="document">the owner document of the element</param>
        /// <param name="eid">the id of the element</param>
        /// <returns>the element of the specified type</returns>
        public static T GetElement<T>(Document document, int eid) where T : Autodesk.Revit.DB.Element
        {
            Autodesk.Revit.DB.ElementId elementId = new ElementId(eid);
            return document.GetElement(elementId) as T;
        }

        /// <summary>
        /// Write the information of a MEPSystem to the log file.
        /// </summary>
        /// <param name="system">the MEP system whose information is to be written</param>
        public static void WriteMEPSystem(MEPSystem system)
        {
            WriteElement(system.BaseEquipment);
            Trace.Unindent();
            Trace.WriteLine("+BaseEquipmentConnector");
            Trace.Indent();
            WriteConnector(system.BaseEquipmentConnector);
            Trace.Unindent();
            Trace.WriteLine("+Elements");
            Trace.Indent();
            foreach (Element element in system.Elements)
            {
                WriteElement(element);
                Trace.WriteLine("");
            }
            Trace.Unindent();
            Trace.WriteLine("+ConnectorManager");
            Trace.Indent();
            WriteConnectorManager(system.ConnectorManager);
            Trace.Unindent();
        }

        /// <summary>
        /// Write the information of a connector to the log file
        /// </summary>
        /// <param name="connector">the connector whose information is to be written</param>
        public static void WriteConnector(Connector connector)
        {
            if (connector == null)
            {
                Trace.WriteLine("null"); return;
            }
            object connType = InvalidString;
            object connDirection = InvalidString;
            object connShape = InvalidString;
            try { connShape = connector.Shape; }
            catch { }
            object connSize = InvalidString;
            try { connSize = GetShapeInfo(connector); }
            catch { }
            object connLocation = GetLocation(connector);
            object connAType = connector.ConnectorType;
            object connIsConnected = InvalidString;
            switch (connector.Domain)
            {
                case Domain.DomainElectrical:
                    connType = connector.ElectricalSystemType;
                    break;
                case Domain.DomainHvac:
                    connType = connector.DuctSystemType;
                    connDirection = connector.Direction;
                    connIsConnected = connector.IsConnected;
                    break;
                case Domain.DomainPiping:
                    connType = connector.PipeSystemType;
                    connDirection = connector.Direction;
                    connIsConnected = connector.IsConnected;
                    break;
                case Domain.DomainUndefined:
                default:
                    connType = Domain.DomainUndefined;
                    break;
            }
            Trace.WriteLine("Type: " + connAType);
            Trace.WriteLine("SystemType: " + connType);
            Trace.WriteLine("Direction: " + connDirection);
            Trace.WriteLine("Shape: " + connShape);
            Trace.WriteLine("Size: " + connSize);
            Trace.WriteLine("Location: " + connLocation);
            Trace.WriteLine("IsConnected: " + connIsConnected);
        }

        /// <summary>
        /// Write the information of a ConnectorManager to the log file
        /// </summary>
        /// <param name="connectorManager">the ConnectorManager whose information is to be written</param>
        public static void WriteConnectorManager(ConnectorManager connectorManager)
        {
            Trace.WriteLine("+Connectors");
            Trace.Indent();
            WriteConnectorSet2(connectorManager.Connectors);
            Trace.Unindent();
            Trace.WriteLine("+UnusedConnectors");
            Trace.Indent();
            WriteConnectorSet2(connectorManager.UnusedConnectors);
            Trace.Unindent();
        }

        /// <summary>
        /// Get the information string of a connector
        /// </summary>
        /// <param name="connector">the connector to be read</param>
        /// <returns>the information string of the connector</returns>
        public static string GetConnectorId(Connector connector)
        {
            if (connector == null)
            {
                return "null";
            }
            int ownerId = connector.Owner.Id.IntegerValue;
            string systemId = InvalidString;
            try { systemId = connector.MEPSystem.Id.IntegerValue.ToString(); }
            catch { }
            object connType = InvalidString;
            object connDirection = InvalidString;
            object connShape = InvalidString;
            try { connShape = connector.Shape; }
            catch { }
            object connSize = InvalidString;
            try { connSize = GetShapeInfo(connector); }
            catch { }
            object connLocation = GetLocation(connector);
            object connAType = connector.ConnectorType;
            object connIsConnected = InvalidString;
            switch (connector.Domain)
            {
                case Domain.DomainElectrical:
                    connType = connector.ElectricalSystemType;
                    break;
                case Domain.DomainHvac:
                    connType = connector.DuctSystemType;
                    connDirection = connector.Direction;
                    connIsConnected = connector.IsConnected;
                    break;
                case Domain.DomainPiping:
                    connType = connector.PipeSystemType;
                    connDirection = connector.Direction;
                    connIsConnected = connector.IsConnected;
                    break;
                case Domain.DomainUndefined:
                default:
                    connType = Domain.DomainUndefined;
                    break;
            }
            return string.Format("[{0}]\t[{1}]\t[{2}]\t[{3}]\t[{4}]\t[{5}]\t[{6}]\t[{7}]\t[{8}]\t",
                ownerId, connType, connDirection, connShape, connSize, connLocation,
                connAType, connIsConnected, systemId);
        }

        /// <summary>
        /// Get the shape information string of a connector
        /// </summary>
        /// <param name="conn">the element to be read</param>
        /// <returns>the shape information string of the connector</returns>
        private static string GetShapeInfo(Connector conn)
        {
            switch (conn.Shape)
            {
                case ConnectorProfileType.Invalid:
                    break;
                case ConnectorProfileType.Oval:
                    break;
                case ConnectorProfileType.Rectangular:
                    return string.Format("{0}\" x {1}\"", conn.Width, conn.Height);
                case ConnectorProfileType.Round:
                    return string.Format("{0}\"", conn.Radius);
                default:
                    break;
            }
            return InvalidString;
        }

        /// <summary>
        /// Get the location string of a connector
        /// </summary>
        /// <param name="conn">the connector to be read</param>
        /// <returns>the location information string of the connector</returns>
        private static object GetLocation(Connector conn)
        {
            if (conn.ConnectorType == ConnectorType.Logical)
            {
                return InvalidString;
            }
            Autodesk.Revit.DB.XYZ origin = conn.Origin;
            return string.Format("{0},{1},{2}", origin.X, origin.Y, origin.Z);
        }

        /// <summary>
        /// Write the information of a ConnectorSet to the log file.
        /// </summary>
        /// <param name="connectorSet">the ConnectorSet whose information is to be written</param>
        private static void WriteConnectorSet2(ConnectorSet connectorSet)
        {
            SortedDictionary<string, List<Connector>> connectors = new SortedDictionary<string, List<Connector>>();
            foreach (Connector conn in connectorSet)
            {
                string connId = GetConnectorId(conn);
                if (conn.ConnectorType == ConnectorType.Logical)
                {
                    foreach (Connector logLinkConn in conn.AllRefs)
                    {
                        connId += GetConnectorId(logLinkConn);
                    }
                }
                if (!connectors.ContainsKey(connId))
                {
                    connectors.Add(connId, new List<Connector>());
                }

                connectors[connId].Add(conn);
            }
            foreach (string key in connectors.Keys)
            {
                foreach (Connector conn in connectors[key])
                {
                    WriteConnector(conn);
                    Trace.WriteLine("+AllRefs");
                    Trace.Indent();
                    WriteConnectorSet(conn.AllRefs);
                    Trace.Unindent();
                    Trace.WriteLine("");
                }
            }
        }

        /// <summary>
        /// Write the information of a ConnectorSet to the log file.
        /// </summary>
        /// <param name="connectorSet">the mechanical system whose information is to be written</param>
        private static void WriteConnectorSet(ConnectorSet connectorSet)
        {
            SortedDictionary<string, List<Connector>> connectors = new SortedDictionary<string, List<Connector>>();
            foreach (Connector conn in connectorSet)
            {
                string connId = GetConnectorId(conn);
                if (conn.ConnectorType == ConnectorType.Logical)
                {
                    foreach (Connector logLinkConn in conn.AllRefs)
                    {
                        connId += GetConnectorId(logLinkConn);
                    }
                }
                if (!connectors.ContainsKey(connId))
                {
                    connectors.Add(connId, new List<Connector>());
                }

                connectors[connId].Add(conn);
            }

            foreach (string key in connectors.Keys)
            {
                foreach (Connector conn in connectors[key])
                {
                    WriteConnector(conn);
                    Trace.WriteLine("");
                }
            }
        }
    }
}
