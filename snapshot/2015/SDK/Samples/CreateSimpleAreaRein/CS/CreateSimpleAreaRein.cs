//
// (C) Copyright 2003-2014 by Autodesk, Inc.
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

using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.CreateSimpleAreaRein.CS
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows.Forms;

    using Autodesk.Revit;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.DB.Structure;

    using DocCreator = Autodesk.Revit.Creation.Document;


    /// <summary>
    /// main class to create simple AreaReinforcement on selected wall or floor
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Automatic)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class Command : IExternalCommand
    {
        private UIDocument m_currentDoc;
        private static ExternalCommandData m_revit;

        ///<summary>
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
        public Autodesk.Revit.UI.Result Execute(ExternalCommandData revit,
            ref string message, Autodesk.Revit.DB.ElementSet elements)
        {
            //initialize necessary data
            m_revit = revit;
            m_currentDoc = revit.Application.ActiveUIDocument;

            //create AreaReinforcement
            try
            {
                if (Create())
                {
                    return Autodesk.Revit.UI.Result.Succeeded;
                }
            }
            catch (ApplicationException appEx)
            {
                TaskDialog.Show("Revit", appEx.Message);
                return Autodesk.Revit.UI.Result.Failed;
            }
            catch
            {
                TaskDialog.Show("Revit", "Unknow Errors.");
                return Autodesk.Revit.UI.Result.Failed;
            }
            return Autodesk.Revit.UI.Result.Cancelled;
        }

        /// <summary>
        /// ExternalCommandData
        /// </summary>
        public static ExternalCommandData CommandData
        {
            get
            {
                return m_revit;
            }
        }

        /// <summary>
        /// create simple AreaReinforcement on selected wall or floor
        /// </summary>
        /// <returns></returns>
        private bool Create()
        {
           ElementSet elems = new ElementSet();
            foreach (ElementId elementId in m_currentDoc.Selection.GetElementIds())
            {
               elems.Insert(m_currentDoc.Document.GetElement(elementId));
            }

            //selected 0 or more than 1 element
            if (elems.Size != 1)
            {
                TaskDialog.Show("Error", "Please select exactly one wall or floor.");
                return false;
            }
            foreach (object o in elems)
            {
                //create on floor
                Floor floor = o as Floor;
                if (null != floor)
                {
                    bool flag = CreateAreaReinOnFloor(floor);
                    return flag;
                }

                //create on wall
                Wall wall = o as Wall;
                if (null != wall)
                {
                    bool flag = CreateAreaReinOnWall(wall);
                    return flag;
                }

                //selected element is neither wall nor floor
                TaskDialog.Show("Error", "Please select exactly one wall or floor.");
            }
            return false;
        }

        /// <summary>
        /// create simple AreaReinforcement on horizontal floor
        /// </summary>
        /// <param name="floor"></param>
        /// <returns>is successful</returns>
        private bool CreateAreaReinOnFloor(Floor floor)
        {
            GeomHelper helper = new GeomHelper();
            Reference refer = null;
            IList<Curve> curves = new List<Curve>();

            //check whether floor is horizontal rectangular 
            //and prepare necessary to create AreaReinforcement
            if (!helper.GetFloorGeom(floor, ref refer, ref curves))
            {
                ApplicationException appEx = new ApplicationException(
                    "Your selection is not a horizontal rectangular slab.");
                throw appEx;
            }

            AreaReinDataOnFloor dataOnFloor = new AreaReinDataOnFloor();
            CreateSimpleAreaReinForm createForm =
                new CreateSimpleAreaReinForm(dataOnFloor);

            //allow use select parameters to create
            if (createForm.ShowDialog() == DialogResult.OK)
            {
                //define the Major Direction of AreaReinforcement,
                //we get direction of first Line on the Floor as the Major Direction
                Line firstLine = (Line)(curves[0]);
                Autodesk.Revit.DB.XYZ majorDirection = new Autodesk.Revit.DB.XYZ(
                    firstLine.GetEndPoint(1).X - firstLine.GetEndPoint(0).X,
                    firstLine.GetEndPoint(1).Y - firstLine.GetEndPoint(0).Y,
                    firstLine.GetEndPoint(1).Z - firstLine.GetEndPoint(0).Z);

                //Create AreaReinforcement
                ElementId areaReinforcementTypeId = AreaReinforcementType.CreateDefaultAreaReinforcementType(m_revit.Application.ActiveUIDocument.Document);
                ElementId rebarBarTypeId = RebarBarType.CreateDefaultRebarBarType(m_revit.Application.ActiveUIDocument.Document);
                ElementId rebarHookTypeId = RebarHookType.CreateDefaultRebarHookType(m_revit.Application.ActiveUIDocument.Document);
                AreaReinforcement areaRein = AreaReinforcement.Create(m_revit.Application.ActiveUIDocument.Document, floor, curves, majorDirection, areaReinforcementTypeId, rebarBarTypeId, rebarHookTypeId);

                //set AreaReinforcement and it's AreaReinforcementCurves parameters
                dataOnFloor.FillIn(areaRein);
                return true;
            }
            return false;
        }

        /// <summary>
        /// create simple AreaReinforcement on vertical straight rectangular wall
        /// </summary>
        /// <param name="wall"></param>
        /// <returns>is successful</returns>
        private bool CreateAreaReinOnWall(Wall wall)
        {
            //make sure selected is basic wall
            if (wall.WallType.Kind != WallKind.Basic)
            {
                TaskDialog.Show("Revit", "Selected wall is not a basic wall.");
                return false;
            }

            GeomHelper helper = new GeomHelper();
            Reference refer = null;
            IList<Curve> curves = new List<Curve>();
            //check whether wall is vertical rectangular and analytical model shape is line
            if (!helper.GetWallGeom(wall, ref refer, ref curves))
            {
                ApplicationException appEx = new ApplicationException(
                    "Your selection is not a structural straight rectangular wall.");
                throw appEx;
            }

            AreaReinDataOnWall dataOnWall = new AreaReinDataOnWall();
            CreateSimpleAreaReinForm createForm = new
                CreateSimpleAreaReinForm(dataOnWall);

            //allow use select parameters to create
            if (createForm.ShowDialog() == DialogResult.OK)
            {
                DocCreator creator = m_revit.Application.ActiveUIDocument.Document.Create;

                //define the Major Direction of AreaReinforcement,
                //we get direction of first Line on the Floor as the Major Direction
                Line firstLine = (Line)(curves[0]);
                Autodesk.Revit.DB.XYZ majorDirection = new Autodesk.Revit.DB.XYZ(
                    firstLine.GetEndPoint(1).X - firstLine.GetEndPoint(0).X,
                    firstLine.GetEndPoint(1).Y - firstLine.GetEndPoint(0).Y,
                    firstLine.GetEndPoint(1).Z - firstLine.GetEndPoint(0).Z);

                //create AreaReinforcement
                IList<Curve> curveList = new List<Curve>();
                foreach (Curve curve in curves)
                {
                    curveList.Add(curve);
                }
                ElementId areaReinforcementTypeId = AreaReinforcementType.CreateDefaultAreaReinforcementType(m_revit.Application.ActiveUIDocument.Document);
                ElementId rebarBarTypeId = RebarBarType.CreateDefaultRebarBarType(m_revit.Application.ActiveUIDocument.Document);
                ElementId rebarHookTypeId = RebarHookType.CreateDefaultRebarHookType(m_revit.Application.ActiveUIDocument.Document);
                AreaReinforcement areaRein = AreaReinforcement.Create(m_revit.Application.ActiveUIDocument.Document, wall, curveList, majorDirection, areaReinforcementTypeId, rebarBarTypeId, rebarHookTypeId);
                dataOnWall.FillIn(areaRein);
                return true;
            }
            return false;
        }
    }
}
