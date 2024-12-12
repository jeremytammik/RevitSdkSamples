//
// (C) Copyright 2003-2023 by Autodesk, Inc.
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
using System.IO;
using System.Linq;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.MeasureHeight.CS
{
    /// <summary>
    /// Calculate the height above the ground floor of a selected skylight.
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class Command : IExternalCommand
    {
        #region Class Members
        /// <summary>
        /// Revit application
        /// </summary>
        private Autodesk.Revit.ApplicationServices.Application m_app;

        /// <summary>
        /// Revit active document
        /// </summary>
        private Document m_doc;

        /// <summary>
        /// a 3d view
        /// </summary>
        private View3D m_view3D;

        /// <summary>
        /// skylight family instance
        /// </summary>
        private FamilyInstance m_skylight;

        /// <summary>
        /// Floor element
        /// </summary>
        private Floor m_floor;
        #endregion

        #region Class Interface Implementation
        /// <summary>
        /// The top level command.
        /// </summary>
        /// <param name="revit">An object that is passed to the external application 
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
        public Result Execute(ExternalCommandData revit, ref string message, ElementSet elements)
        {
            m_app = revit.Application.Application;
            m_doc = revit.Application.ActiveUIDocument.Document;

            Transaction trans = new Transaction(m_doc, "Revit.SDK.Samples.MeasureHeight");
            trans.Start();
            // Find a 3D view to use for the ray tracing operation
            FilteredElementCollector collector = new FilteredElementCollector(m_doc);
            Func<View3D, bool> isNotTemplate = v3 => !(v3.IsTemplate);
            m_view3D = collector.OfClass(typeof(View3D)).Cast<View3D>().First<View3D>(isNotTemplate);

            Autodesk.Revit.UI.Selection.Selection selection = revit.Application.ActiveUIDocument.Selection;

            // If skylight is selected, process it.
            m_skylight = null;
            if (selection.GetElementIds().Count == 1)
            {
                foreach (ElementId eId in selection.GetElementIds())
                {
                   Element e = revit.Application.ActiveUIDocument.Document.GetElement(eId);
                    if (e is FamilyInstance)
                    {
                        FamilyInstance instance = e as FamilyInstance;
                        bool isWindow = (instance.Category.BuiltInCategory == BuiltInCategory.OST_Windows);
                        bool isHostedByRoof = (instance.Host.Category.BuiltInCategory == BuiltInCategory.OST_Roofs);

                        if (isWindow && isHostedByRoof)
                            m_skylight = instance;
                    }
                }
            }

            if (m_skylight == null)
            {
                message = "This tool requires exactly one skylight to be selected.";
                trans.RollBack();
                return Result.Cancelled;
            }

            // Find the floor to use for the measurement (hardcoded)
            ElementId id = new ElementId(150314L);
            m_floor = m_doc.GetElement(id) as Floor;

            // Calculate the height
            Line line = CalculateLineAboveFloor();

            // Create a model curve to show the distance
            Plane plane = Plane.CreateByNormalAndOrigin(new XYZ(1, 0, 0), line.GetEndPoint(0));
            SketchPlane sketchPlane = SketchPlane.Create(m_doc, plane);

            ModelCurve curve = m_doc.Create.NewModelCurve(line, sketchPlane);

            // Show a message with the length value
            TaskDialog.Show("Distance", "Distance to floor: " + String.Format("{0:f2}", line.Length));

            trans.Commit();
            return Result.Succeeded;
        }
        #endregion

        #region Class Implementations
        /// <summary>
        /// Determines the line segment that connects the skylight to the already obtained floor.
        /// </summary>
        /// <returns>The line segment.</returns>
        private Line CalculateLineAboveFloor()
        {
            // Use the center of the skylight bounding box as the start point.
            BoundingBoxXYZ box = m_skylight.get_BoundingBox(m_view3D);
            XYZ center = box.Min.Add(box.Max).Multiply(0.5);

            // Project in the negative Z direction down to the floor.
            XYZ rayDirection = new XYZ(0, 0, -1);

            // Look for references to faces where the element is the floor element id.
            ReferenceIntersector referenceIntersector = new ReferenceIntersector(m_floor.Id, FindReferenceTarget.Face, m_view3D);
            IList<ReferenceWithContext> references = referenceIntersector.Find(center, rayDirection);

            double distance = Double.PositiveInfinity;
            XYZ intersection = null;
            foreach (ReferenceWithContext referenceWithContext in references)
            {
                Reference reference = referenceWithContext.GetReference();
                // Keep the closest matching reference (using the proximity parameter to determine closeness).
                double proximity = referenceWithContext.Proximity;
                if (proximity < distance)
                {
                    distance = proximity;
                    intersection = reference.GlobalPoint;
                }
            }

            // Create line segment from the start point and intersection point.
            Line result = Line.CreateBound(center, intersection);
            return result;
        }
        #endregion
    }
}
