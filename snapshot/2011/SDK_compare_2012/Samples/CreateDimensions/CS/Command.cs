//
// (C) Copyright 2003-2010 by Autodesk, Inc.
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
using System.IO;
using System.Collections;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;

using Autodesk;
using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Structure;


namespace Revit.SDK.Samples.CreateDimensions.CS
{
    /// <summary>
    /// Implements the Revit add-in interface IExternalCommand
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Automatic)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Automatic)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class Command : IExternalCommand
    {
        ExternalCommandData m_revit = null;    //store external command
        string m_errorMessage = " ";           // store error message
        ArrayList m_walls = new ArrayList();   //store the wall of selected
        const double precision = 0.0000001;         //store the precision   

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
        public Autodesk.Revit.UI.Result Execute(ExternalCommandData revit, ref string message, Autodesk.Revit.DB.ElementSet elements)
        {
            try
            {
                m_revit = revit;
                Autodesk.Revit.DB.View view = m_revit.Application.ActiveUIDocument.Document.ActiveView;
                View3D view3D = view as View3D;
                if (null != view3D)
                {
                    message += "Only create dimensions in 2D";
                    return Autodesk.Revit.UI.Result.Failed;
                }

                ViewSheet viewSheet = view as ViewSheet;
                if (null != viewSheet)
                {
                    message += "Only create dimensions in 2D";
                    return Autodesk.Revit.UI.Result.Failed;
                }

                //try too adds a dimension from the start of the wall to the end of the wall into the project
                if (!AddDimension())
                {
                    message = m_errorMessage;
                    return Autodesk.Revit.UI.Result.Failed;
                }
                return Autodesk.Revit.UI.Result.Succeeded;
            }
            catch (Exception e)
            {
                message = e.Message;
                return Autodesk.Revit.UI.Result.Failed;
            }            
        }

        /// <summary>
        /// find out the wall, insert it into a array list
        /// </summary>
        bool initialize()
        {
            ElementSet selections = m_revit.Application.ActiveUIDocument.Selection.Elements;
            //nothing was selected
            if (0 == selections.Size)
            {
                m_errorMessage += "Please select Basic walls";
                return false;
            }

            //find out wall
            foreach (Autodesk.Revit.DB.Element e in selections)
            {
                Wall wall = e as Wall;
                if (null != wall)
                {
                    if ("Basic" != wall.WallType.Kind.ToString())
                    {
                        continue;
                    }
                    m_walls.Add(wall);
                }
            }

            //no wall was selected
            if (0 == m_walls.Count)
            {
                m_errorMessage += "Please select Basic walls";
                return false;
            }
            return true;
        }

        /// <summary>
        /// find out every wall in the selection and add a dimension from the start of the wall to its end
        /// </summary>
        /// <returns>if add successfully, true will be retured, else false will be returned</returns>
        public bool AddDimension()
        {
            if (!initialize())
            {
                return false;
            }

            //get out all the walls in this array, and create a dimension from its start to its end
            for (int i = 0; i < m_walls.Count; i++)
            {
                Wall wallTemp = m_walls[i] as Wall;
                if (null == wallTemp)
                {
                    continue;
                }

                //get loction curve
                Location location = wallTemp.Location;
                LocationCurve locationline = location as LocationCurve;
                if (null == locationline)
                {
                    continue;
                }

                //New Line
                Autodesk.Revit.DB.XYZ  locationEndPoint = locationline.Curve.get_EndPoint(1);
                Autodesk.Revit.DB.XYZ  locationStartPoint = locationline.Curve.get_EndPoint(0);
                Line newLine = m_revit.Application.Application.Create.NewLine(locationStartPoint,
                                                                  locationEndPoint,
                                                                  true);

                //get reference
                ReferenceArray referenceArray = new ReferenceArray();

                Options options = m_revit.Application.Application.Create.NewGeometryOptions();
                options.ComputeReferences = true;
                options.View = m_revit.Application.ActiveUIDocument.Document.ActiveView;
                Autodesk.Revit.DB.GeometryElement element = wallTemp.get_Geometry(options);
                GeometryObjectArray geoObjectArray = element.Objects;
                //enum the geometry element
                for (int j = 0; j < geoObjectArray.Size; j++)
                {
                    GeometryObject geoObject = geoObjectArray.get_Item(j);
                    Curve curve = geoObject as Curve;
                    if (null != curve)
                    {
                        //find the two upright lines beside the line
                        if (Validata(newLine, curve as Line))
                        {
                            referenceArray.Append(curve.Reference);
                        }

                        if (2 == referenceArray.Size)
                        {
                            break;
                        }
                    }
                }
                try
                {
                    //try to new a dimension
                    Autodesk.Revit.UI.UIApplication app = m_revit.Application;
                    Document doc = app.ActiveUIDocument.Document;

                    Autodesk.Revit.DB.XYZ p1 = new XYZ(
                        newLine.get_EndPoint(0).X + 5,
                        newLine.get_EndPoint(0).Y + 5,
                        newLine.get_EndPoint(0).Z);
                    Autodesk.Revit.DB.XYZ p2 = new XYZ(
                        newLine.get_EndPoint(1).X + 5,
                        newLine.get_EndPoint(1).Y + 5,
                        newLine.get_EndPoint(1).Z);

                    Line newLine2 = app.Application.Create.NewLine(p1, p2, true);
                    Dimension newDimension = doc.Create.NewDimension(
                      doc.ActiveView, newLine2, referenceArray);
                }
                // catch the exceptions
                catch (Exception ex)
                {
                    m_errorMessage += ex.ToString();
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// make sure the line we get from the its geometry is upright to the location line 
        /// </summary>
        /// <param name="line1">the first line</param>
        /// <param name="line2">the second line</param>
        /// <returns>if the two line is upright, true will be retured</returns>
        bool Validata(Line line1, Line line2)
        {
            //if it is not a linear line
            if (null == line1 || null == line2)
            {
                return false;
            }

            //get the first line's length
            Autodesk.Revit.DB.XYZ newLine = new Autodesk.Revit.DB.XYZ (line1.get_EndPoint(1).X - line1.get_EndPoint(0).X,
                                 line1.get_EndPoint(1).Y - line1.get_EndPoint(0).Y,
                                 line1.get_EndPoint(1).Z - line1.get_EndPoint(0).Z);
            double x1 = newLine.X * newLine.X;
            double y1 = newLine.Y * newLine.Y;
            double z1 = newLine.Z * newLine.Z;
            double sqrt1 = Math.Sqrt(x1 + y1 + z1);

            //get the second line's length
            Autodesk.Revit.DB.XYZ vTemp = new Autodesk.Revit.DB.XYZ (line2.get_EndPoint(1).X - line2.get_EndPoint(0).X,
                                line2.get_EndPoint(1).Y - line2.get_EndPoint(0).Y,
                                line2.get_EndPoint(1).Z - line2.get_EndPoint(0).Z);
            double x2 = vTemp.X * vTemp.X;
            double y2 = vTemp.Y * vTemp.Y;
            double z2 = vTemp.Z * vTemp.Z;
            double sqrt2 = Math.Sqrt(x1 + y1 + z1);

            double VP = newLine.X * vTemp.X + newLine.Y * vTemp.Y + newLine.Z * vTemp.Z;
            double compare = VP / (sqrt1 * sqrt2);
            if ((-precision <= compare) && (precision >= compare))
            {
                return true;
            }
            return false;
        }
    }
}
