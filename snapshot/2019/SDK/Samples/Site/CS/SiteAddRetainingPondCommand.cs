//
// (C) Copyright 2003-2017 by Autodesk, Inc. All rights reserved.
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.

//
// AUTODESK PROVIDES THIS PROGRAM 'AS IS' AND WITH ALL ITS FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE. AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.DB.Architecture;

namespace Revit.SDK.Samples.Site.CS
{
    /// <summary>
    /// A command that adds a new circular retaining pond to a TopographySurface where the user selects.
    /// </summary>
    /// <remarks>This command demonstrates how the Site API supports creation of standard topography "families" representing
    /// commonly used landscape structures.</remarks>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class SiteAddRetainingPondCommand : IExternalCommand
    {
        #region IExternalCommand Members

        /// <summary>
        /// Implementation of the external command.
        /// </summary>
        /// <param name="commandData"></param>
        /// <param name="message"></param>
        /// <param name="elements"></param>
        /// <returns></returns>
        public Result Execute(ExternalCommandData commandData, ref string message, Autodesk.Revit.DB.ElementSet elements)
        {
            AddNewRetainingPond(commandData.Application.ActiveUIDocument, 32);

            return Result.Succeeded;
        }

        /// <summary>
        /// Adds a new retaining pond.
        /// </summary>
        /// <param name="uiDoc">The document.</param>
        /// <param name="pondRadius">The radius of the pond.</param>
        private void AddNewRetainingPond(UIDocument uiDoc, double pondRadius)
        {
            Document doc = uiDoc.Document;

            // Find toposurfaces
            FilteredElementCollector tsCollector = new FilteredElementCollector(doc);
            tsCollector.OfClass(typeof(TopographySurface));
            IEnumerable<TopographySurface> tsEnumerable = tsCollector.Cast<TopographySurface>().Where<TopographySurface>(ts => !ts.IsSiteSubRegion);
            int count = tsEnumerable.Count<TopographySurface>();

            // If there is only on surface, use it.  If there is more than one, let the user select the target.
            TopographySurface targetSurface = null;
            if (count > 1) // tmp
            {
                targetSurface = SiteUIUtils.PickTopographySurface(uiDoc);
            }
            else
            {
                targetSurface = tsEnumerable.First<TopographySurface>();
            }

            // Pick point and project to plane at toposurface average elevation
            XYZ point = SiteUIUtils.PickPointNearToposurface(uiDoc, targetSurface, "Pick point for center of pond.");
            double elevation = point.Z;

            // Add subregion first, so that any previously existing points can be removed to avoid distorting the new region

            // Find material "Water"
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(Material));
            Material mat = collector.Cast<Material>().FirstOrDefault<Material>(m => m.Name == "Water");

            // Create subregion curves
            List<Curve> curves = new List<Curve>();
            curves.Add(Arc.Create(point, pondRadius, 0, Math.PI, XYZ.BasisX, XYZ.BasisY));
            curves.Add(Arc.Create(point, pondRadius, Math.PI, 2 * Math.PI, XYZ.BasisX, XYZ.BasisY));

            CurveLoop curveLoop = CurveLoop.Create(curves);
            List<CurveLoop> curveLoops = new List<CurveLoop>();
            curveLoops.Add(curveLoop);

            // All changes are added to one transaction group - will create one undo item
            using (TransactionGroup addGroup = new TransactionGroup(doc, "Add pond group"))
            {
                addGroup.Start();

                IList<XYZ> existingPoints = null;
                // Transacton for adding subregion.
                using (Transaction t2 = new Transaction(doc, "Add subregion"))
                {
                    t2.Start();
                    SiteSubRegion region = SiteSubRegion.Create(doc, curveLoops, targetSurface.Id);
                    if (mat != null)
                    {
                        region.TopographySurface.MaterialId = mat.Id;
                    }
                    t2.Commit();

                    // The boundary points for the subregion cannot be deleted, since they are generated
                    // to represent the subregion boundary rather than representing real points in the host.
                    // Get non-boundary points only to be deleted.
                    existingPoints = SiteEditingUtils.GetNonBoundaryPoints(region.TopographySurface);

                    // Average elevation of all points in the subregion to use as base elevation for the pond topography
                    elevation = SiteEditingUtils.GetAverageElevation(region.TopographySurface.GetPoints());
                }

                // Add the topography points to the target surface via edit scope.
                using (TopographyEditScope editScope = new TopographyEditScope(doc, "Edit TS"))
                {
                    editScope.Start(targetSurface.Id);

                    // Transaction for points changes
                    using (Transaction t = new Transaction(doc, "Add points"))
                    {
                        t.Start();

                        // Delete existing points first to avoid conflict
                        if (existingPoints.Count > 0)
                        {
                            targetSurface.DeletePoints(existingPoints);
                        }

                        // Generate list of points to add
                        IList<XYZ> points = SiteEditingUtils.GeneratePondPointsSurrounding(new XYZ(point.X, point.Y, elevation - 3), pondRadius);
                        targetSurface.AddPoints(points);
                        t.Commit();
                    }

                    editScope.Commit(new TopographyEditFailuresPreprocessor());
                }
                addGroup.Assimilate();
            }
        }

        #endregion
    }
}
