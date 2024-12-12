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
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
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
           ElementSet selections = new ElementSet();
            foreach (ElementId elementId in m_revit.Application.ActiveUIDocument.Selection.GetElementIds())
            {
               selections.Insert(m_revit.Application.ActiveUIDocument.Document.GetElement(elementId));
            }
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
        /// <returns>if add successfully, true will be returned, else false will be returned</returns>
        public bool AddDimension()
        {
            if (!initialize())
            {
                return false;
            }

            Transaction transaction = new Transaction(m_revit.Application.ActiveUIDocument.Document, "Add Dimensions");
            transaction.Start();
            //get out all the walls in this array, and create a dimension from its start to its end
            for (int i = 0; i < m_walls.Count; i++)
            {
                Wall wallTemp = m_walls[i] as Wall;
                if (null == wallTemp)
                {
                    continue;
                }

                //get location curve
                Location location = wallTemp.Location;
                LocationCurve locationline = location as LocationCurve;
                if (null == locationline)
                {
                    continue;
                }

                //New Line

                Line newLine = null;

                //get reference
                ReferenceArray referenceArray = new ReferenceArray();

                AnalyticalModel analyticalModel = wallTemp.GetAnalyticalModel();
                IList<Curve> activeCurveList = analyticalModel.GetCurves(AnalyticalCurveType.ActiveCurves);
                foreach (Curve aCurve in activeCurveList)
                {
                    // find non-vertical curve from analytical model
                    if (aCurve.GetEndPoint(0).Z == aCurve.GetEndPoint(1).Z)
                        newLine = aCurve as Line;
                    if (aCurve.GetEndPoint(0).Z != aCurve.GetEndPoint(1).Z)
                    {
                        AnalyticalModelSelector amSelector = new AnalyticalModelSelector(aCurve);
                        amSelector.CurveSelector = AnalyticalCurveSelector.StartPoint;
                        referenceArray.Append(analyticalModel.GetReference(amSelector));
                    }
                    if (2 == referenceArray.Size)
                        break;
                }
                if (referenceArray.Size != 2)
                {
                    m_errorMessage += "Did not find two references";
                    return false;
                }
                try
                {
                    //try to add new a dimension
                    Autodesk.Revit.UI.UIApplication app = m_revit.Application;
                    Document doc = app.ActiveUIDocument.Document;

                    Autodesk.Revit.DB.XYZ p1 = new XYZ(
                        newLine.GetEndPoint(0).X + 5,
                        newLine.GetEndPoint(0).Y + 5,
                        newLine.GetEndPoint(0).Z);
                    Autodesk.Revit.DB.XYZ p2 = new XYZ(
                        newLine.GetEndPoint(1).X + 5,
                        newLine.GetEndPoint(1).Y + 5,
                        newLine.GetEndPoint(1).Z);

                    Line newLine2 = Line.CreateBound(p1, p2);
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
            transaction.Commit();
            return true;
        }

    }
}
