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
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;

namespace Revit.SDK.Samples.Site.CS
{
    /// <summary>
    /// A command that moves a subregion and the points it contains to a new location on the host surface.
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class SiteMoveRegionAndPointsCommand : IExternalCommand
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
            MoveSubregionAndPoints(commandData.Application.ActiveUIDocument);
            return Result.Succeeded;
        }

        #endregion

        /// <summary>
        /// Moves a subregion and the associated topography to a new user-selected location.
        /// </summary>
        /// <param name="uiDoc">The document.</param>
        private void MoveSubregionAndPoints(UIDocument uiDoc)
        {
            Document doc = uiDoc.Document;

            // Pick subregion
            TopographySurface subregion = SiteUIUtils.PickSubregion(uiDoc);
            TopographySurface toposurface = SiteEditingUtils.GetTopographySurfaceHost(subregion);
            IList<XYZ> points = SiteEditingUtils.GetPointsFromSubregionExact(subregion);
            XYZ sourceLocation = SiteEditingUtils.GetCenterOf(subregion);

            // Pick target location
            XYZ targetPoint = SiteUIUtils.PickPointNearToposurface(uiDoc, toposurface, "Pick point to move to");

            // Delta for the move
            XYZ delta = targetPoint - sourceLocation;

            // All changes are added to one transaction group - will create one undo item
            using (TransactionGroup moveGroup = new TransactionGroup(doc, "Move subregion and points"))
            {
                moveGroup.Start();

                // Get elevation of region in current location
                IList<XYZ> existingPointsInCurrentLocation = subregion.GetPoints();

                double existingElevation = SiteEditingUtils.GetAverageElevation(existingPointsInCurrentLocation);

                // Move subregion first - allows the command delete existing points and adjust elevation to surroundings
                using (Transaction t2 = new Transaction(doc, "Move subregion"))
                {
                    t2.Start();
                    ElementTransformUtils.MoveElement(doc, subregion.Id, delta);
                    t2.Commit();
                }

                // The boundary points for the subregion cannot be deleted, since they are generated
                // to represent the subregion boundary rather than representing real points in the host.
                // Get non-boundary points only to be deleted.
                IList<XYZ> existingPointsInNewLocation = SiteEditingUtils.GetNonBoundaryPoints(subregion);

                // Average elevation of all points in the subregion.
                double newElevation = SiteEditingUtils.GetAverageElevation(subregion.GetPoints());

                // Adjust delta for elevation based on calculated values
                delta = SiteEditingUtils.MoveXYZToElevation(delta, newElevation - existingElevation);

                // Edit scope for points changes
                using (TopographyEditScope editScope = new TopographyEditScope(doc, "Edit TS"))
                {
                    editScope.Start(toposurface.Id);

                    using (Transaction t = new Transaction(doc, "Move points"))
                    {
                        t.Start();
                        // Delete existing points from target region
                        if (existingPointsInNewLocation.Count > 0)
                        {
                            toposurface.DeletePoints(existingPointsInNewLocation);
                        }

                        // Move points from source region
                        toposurface.MovePoints(points, delta);
                        t.Commit();
                    }

                    editScope.Commit(new TopographyEditFailuresPreprocessor());
                }

                moveGroup.Assimilate();
            }
        }
    }
}
