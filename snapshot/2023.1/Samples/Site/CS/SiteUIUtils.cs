//
// (C) Copyright 2003-2019 by Autodesk, Inc. All rights reserved.
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
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;

namespace Revit.SDK.Samples.Site.CS
{
    /// <summary>
    /// User interface utilities for site editing commands.
    /// </summary>
    class SiteUIUtils
    {
        /// <summary>
        /// Changes the elevation of all points in the subregion by a designated delta.
        /// </summary>
        /// <param name="uiDoc">The document.</param>
        /// <param name="elevationDelta">The change in elevation.</param>
        public static void ChangeSubregionAndPointsElevation(UIDocument uiDoc, double elevationDelta)
        {
            Document doc = uiDoc.Document;

            // Pick subregion
            TopographySurface subregion = PickSubregion(uiDoc);
            TopographySurface toposurface = SiteEditingUtils.GetTopographySurfaceHost(subregion);

            // Get points
            IList<XYZ> points = SiteEditingUtils.GetNonBoundaryPoints(subregion);
            if (points.Count == 0)
                return;

            // Change in elevation
            XYZ delta = elevationDelta * XYZ.BasisZ;

            // Edit scope for all changes
            using (TopographyEditScope editScope = new TopographyEditScope(doc, "Raise/lower terrain"))
            {
                editScope.Start(toposurface.Id);

                using (Transaction t = new Transaction(doc, "Raise/lower terrain"))
                {
                    t.Start();

                    // Use MovePoints to change all points relative to their current position
                    toposurface.MovePoints(points, delta);
                    t.Commit();
                }

                editScope.Commit(new TopographyEditFailuresPreprocessor());
            }
        }

        /// <summary>
        /// Prompts the user to pick a subregion.
        /// </summary>
        /// <param name="uiDoc">The document.</param>
        /// <returns>The selected subregion.</returns>
        public static TopographySurface PickSubregion(UIDocument uiDoc)
        {
            Reference subregRef = uiDoc.Selection.PickObject(ObjectType.Element,
                                                             new SubRegionSelectionFilter(),
                                                             "Select subregion");

            TopographySurface subregion = uiDoc.Document.GetElement(subregRef) as TopographySurface;
            return subregion;
        }

        /// <summary>
        /// Prompts the user to pick a TopographySurface (non-subregion).
        /// </summary>
        /// <param name="uiDoc">The document.</param>
        /// <returns>The selected TopographySurface.</returns>
        public static TopographySurface PickTopographySurface(UIDocument uiDoc)
        {
            Reference toposurfRef = uiDoc.Selection.PickObject(ObjectType.Element,
                                                             new TopographySurfaceSelectionFilter(),
                                                             "Select topography surface");

            TopographySurface toposurface = uiDoc.Document.GetElement(toposurfRef) as TopographySurface;
            return toposurface;
        }

        /// <summary>
        /// Prompts the user to select a point, and returns a point near to the associated TopographySurface.
        /// </summary>
        /// <param name="uiDoc">The document.</param>
        /// <param name="toposurface">The target topography surface.</param>
        /// <param name="message">The selection message.</param>
        /// <returns>The point.</returns>
        public static XYZ PickPointNearToposurface(UIDocument uiDoc, TopographySurface toposurface, String message)
        {
            // Pick the point
            XYZ point = uiDoc.Selection.PickPoint(message);

            // Get the average elevation for the host topography surface
            double elevation = SiteEditingUtils.GetAverageElevation(toposurface.GetPoints());

            // Project the point onto the Z = average elevation plane
            XYZ viewDirection = uiDoc.ActiveView.ViewDirection.Normalize();

            double elevationDelta = (elevation - point.Z) / viewDirection.Z;
            point = point + viewDirection * elevationDelta;

            return point;
        }

        /// <summary>
        /// A selection filter that passes subregions.
        /// </summary>
        class SubRegionSelectionFilter : ISelectionFilter
        {
            /// <summary>
            /// Implementation of the filter method.
            /// </summary>
            /// <param name="element"></param>
            /// <returns></returns>
            public bool AllowElement(Element element)
            {
                TopographySurface ts = element as TopographySurface;

                return ts != null && ts.IsSiteSubRegion;
            }

            /// <summary>
            /// Implementation of the filter method.  
            /// </summary>
            /// <param name="refer"></param>
            /// <param name="point"></param>
            /// <returns></returns>
            public bool AllowReference(Reference refer, XYZ point)
            {
                return false;
            }
        }

        /// <summary>
        /// A selection filter to pass topography surfaces which don't represent subregions.
        /// </summary>
        class TopographySurfaceSelectionFilter : ISelectionFilter
        {
            /// <summary>
            /// Implementation of the filter method.
            /// </summary>
            /// <param name="element"></param>
            /// <returns></returns>
            public bool AllowElement(Element element)
            {
                TopographySurface ts = element as TopographySurface;

                return ts != null && !ts.IsSiteSubRegion;
            }

            /// <summary>
            /// Implementation of the filter method.
            /// </summary>
            /// <param name="refer"></param>
            /// <param name="point"></param>
            /// <returns></returns>
            public bool AllowReference(Reference refer, XYZ point)
            {
                return false;
            }
        }
    }
}
