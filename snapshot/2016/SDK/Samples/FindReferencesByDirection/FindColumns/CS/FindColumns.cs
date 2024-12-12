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
using System.Diagnostics;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace Revit.SDK.Samples.FindColumns.CS
{
    /// <summary>
    /// Find all walls that have embedded columns in them, and the ids of those embedded columns.
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.ReadOnly)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class Command : IExternalCommand
    {
        #region Class Members
        /// <summary>
        /// This is the increment by which the code checks for embedded columns on a curved wall.
        /// </summary>
        private static double WallIncrement = 0.5;  // Check every 1/2'

        /// <summary>
        /// This is a slight offset to ensure that the ray-trace occurs just outside the extents of the wall.
        /// </summary>
        private static double WALL_EPSILON = (1.0 / 8.0) / 12.0;  // 1/8"

        /// <summary>
        /// Dictionary of columns and walls
        /// </summary>
        private Dictionary<ElementId, List<ElementId>> m_columnsOnWall = new Dictionary<ElementId, List<ElementId>>();

        /// <summary>
        /// ElementId list for columns which are on walls
        /// </summary>
        private List<ElementId> m_allColumnsOnWalls = new List<ElementId>();

        /// <summary>
        /// revit application
        /// </summary>
        private Autodesk.Revit.UI.UIApplication m_app;

        /// <summary>
        /// Revit active document
        /// </summary>
        private Autodesk.Revit.DB.Document m_doc;

        /// <summary>
        /// A 3d view 
        /// </summary>
        private View3D m_view3D;
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
        public Autodesk.Revit.UI.Result Execute(ExternalCommandData revit,
                                                             ref string message,
                                                             Autodesk.Revit.DB.ElementSet elements)
        {
            // Initialization 
            m_app = revit.Application;
            m_doc = revit.Application.ActiveUIDocument.Document;

            // Find a 3D view to use for the ray tracing operation
            Get3DView("{3D}");

            Selection selection = revit.Application.ActiveUIDocument.Selection;
            List<Wall> wallsToCheck = new List<Wall>();

            // If wall(s) are selected, process them.
            if (selection.GetElementIds().Count > 0)
            {
                foreach (Autodesk.Revit.DB.ElementId eId in selection.GetElementIds())
                {
                   Autodesk.Revit.DB.Element e = revit.Application.ActiveUIDocument.Document.GetElement(eId);
                    if (e is Wall)
                    {
                        wallsToCheck.Add((Wall)e);
                    }
                }

                if (wallsToCheck.Count <= 0)
                {
                    message = "No walls were found in the active document selection";
                    return Result.Cancelled;
                }
            }
            // Find all walls in the document and process them.
            else
            {
                FilteredElementCollector collector = new FilteredElementCollector(m_doc);
                FilteredElementIterator iter = collector.OfClass(typeof(Wall)).GetElementIterator();
                iter.Reset();
                while (iter.MoveNext())
                {
                    wallsToCheck.Add((Wall)iter.Current);
                }
            }

            // Execute the check for embedded columns
            CheckWallsForEmbeddedColumns(wallsToCheck);

            // Process the results, in this case set the active selection to contain all embedded columns
            ICollection<ElementId> toSelected = new List<ElementId>(); 
            if (m_allColumnsOnWalls.Count > 0)
            {
                foreach (ElementId id in m_allColumnsOnWalls)
                {
                    ElementId familyInstanceId = id;
                    Autodesk.Revit.DB.Element familyInstance = m_doc.GetElement(familyInstanceId);
                    toSelected.Add(familyInstance.Id);
                }
                selection.SetElementIds(toSelected); 
            }
            return Result.Succeeded;
        }
        #endregion

        #region Class Implementation
        /// <summary>
        /// Check a list of walls for embedded columns.
        /// </summary>
        /// <param name="wallsToCheck">The list of walls to check.</param>
        private void CheckWallsForEmbeddedColumns(List<Wall> wallsToCheck)
        {
            foreach (Wall wall in wallsToCheck)
            {
                CheckWallForEmbeddedColumns(wall);
            }
        }

        /// <summary>
        /// Checks a single wall for embedded columns.
        /// </summary>
        /// <param name="wall">The wall to check.</param>
        private void CheckWallForEmbeddedColumns(Wall wall)
        {
            LocationCurve locationCurve = wall.Location as LocationCurve;
            Curve wallCurve = locationCurve.Curve;
            if (wallCurve is Line)
            {
                LogWallCurve((Line)wallCurve);
                CheckLinearWallForEmbeddedColumns(wall, locationCurve, (Line)wallCurve);
            }
            else
            {
                CheckProfiledWallForEmbeddedColumns(wall, locationCurve, wallCurve);
            }
        }

        /// <summary>
        /// Checks a single linear wall for embedded columns.
        /// </summary>
        /// <param name="wall">The wall to check.</param>
        /// <param name="locationCurve">The location curve extracted from this wall.</param>
        /// <param name="wallCurve">The profile of the wall.</param>
        private void CheckLinearWallForEmbeddedColumns(Wall wall, LocationCurve locationCurve, Curve wallCurve)
        {
            double bottomHeight = GetElevationForRay(wall);

            FindColumnsOnEitherSideOfWall(wall, locationCurve, wallCurve, 0, bottomHeight, wallCurve.Length);
        }

        /// <summary>
        /// Finds columns on either side of the given wall.
        /// </summary>
        /// <param name="wall">The wall.</param>
        /// <param name="locationCurve">The location curve of the wall.</param>
        /// <param name="wallCurve">The profile of the wall.</param>
        /// <param name="parameter">The normalized parameter along the wall profile which is being evaluated.</param>
        /// <param name="elevation">The elevation at which the rays are cast.</param>
        /// <param name="within">The maximum distance away that columns may be found.</param>
        private void FindColumnsOnEitherSideOfWall(Wall wall, LocationCurve locationCurve, Curve wallCurve, double parameter, double elevation, double within)
        {
            XYZ rayDirection = GetTangentAt(wallCurve, parameter);
            XYZ wallLocation = wallCurve.Evaluate(parameter, true);

            XYZ wallDelta = GetWallDeltaAt(wall, locationCurve, parameter);

            XYZ rayStart = new XYZ(wallLocation.X + wallDelta.X, wallLocation.Y + wallDelta.Y, elevation);
            FindColumnsByDirection(rayStart, rayDirection, within, wall);

            rayStart = new XYZ(wallLocation.X - wallDelta.X, wallLocation.Y - wallDelta.Y, elevation);
            FindColumnsByDirection(rayStart, rayDirection, within, wall);
        }

        /// <summary>
        /// Finds columns by projecting rays along a given direction.
        /// </summary>
        /// <param name="rayStart">The origin of the ray.</param>
        /// <param name="rayDirection">The direction of the ray.</param>
        /// <param name="within">The maximum distance away that columns may be found.</param>
        /// <param name="wall">The wall that this search is associated with.</param>
        private void FindColumnsByDirection(XYZ rayStart, XYZ rayDirection, double within, Wall wall)
        {
            ReferenceIntersector referenceIntersector = new ReferenceIntersector(m_view3D);
            IList<ReferenceWithContext> intersectedReferences = referenceIntersector.Find(rayStart, rayDirection);
            FindColumnsWithin(intersectedReferences, within, wall);
        }

        /// <summary>
        /// Checks a single curved/profiled wall for embedded columns.
        /// </summary>
        /// <param name="wall">The wall to check.</param>
        /// <param name="locationCurve">The location curve extracted from this wall.</param>
        /// <param name="wallCurve">The profile of the wall.</param>
        private void CheckProfiledWallForEmbeddedColumns(Wall wall, LocationCurve locationCurve, Curve wallCurve)
        {
            double bottomHeight = GetElevationForRay(wall);

            // Figure out the increment for the normalized parameter based on how long the wall is.  
            double parameterIncrement = WallIncrement / wallCurve.Length;

            // Find columns within 2' of the start of the ray.  Any smaller, and you run the risk of not finding a boundary
            // face of the column within the target range.
            double findColumnWithin = 2;

            // check for columns along every WallIncrement fraction of the wall
            for (double parameter = 0; parameter < 1.0; parameter += parameterIncrement)
            {
                FindColumnsOnEitherSideOfWall(wall, locationCurve, wallCurve, parameter, bottomHeight, findColumnWithin);
            }

        }

        /// <summary>
        /// Obtains the elevation for ray casting evaluation for a given wall.
        /// </summary>
        /// <param name="wall">The wall.</param>
        /// <returns>The elevation.</returns>
        private double GetElevationForRay(Wall wall)
        {
            Level level = m_doc.GetElement(wall.LevelId) as Level;

            // Start at 1 foot above the bottom level
            double bottomHeight = level.Elevation + 1.0;

            return bottomHeight;
        }

        /// <summary>
        /// Obtains the offset to the wall at a given location along the wall's profile.
        /// </summary>
        /// <param name="wall">The wall.</param>
        /// <param name="locationCurve">The location curve of the wall.</param>
        /// <param name="parameter">The normalized parameter along the location curve of the wall.</param>
        /// <returns>An XY vector representing the offset from the wall centerline.</returns>
        private XYZ GetWallDeltaAt(Wall wall, LocationCurve locationCurve, double parameter)
        {
            XYZ wallNormal = GetNormalToWallAt(wall, locationCurve, parameter);
            double wallWidth = wall.Width;

            // The LocationCurve is always the wall centerline, regardless of the setting for the wall Location Line.
            // So the delta to place the ray just outside the wall extents is always 1/2 the wall width + a little extra.
            XYZ wallDelta = new XYZ(wallNormal.X * wallWidth / 2 + WALL_EPSILON, wallNormal.Y * wallWidth / 2 + WALL_EPSILON, 0);

            return wallDelta;
        }

        /// <summary>
        /// Finds column elements which occur within a given set of references within the designated proximity, and stores them to the results. 
        /// </summary>
        /// <param name="references">The references obtained from FindReferencesByDirection()</param> 
        /// <param name="proximity">The maximum proximity.</param>
        /// <param name="wall">The wall from which these references were found.</param>
        private void FindColumnsWithin(IList<ReferenceWithContext> references, double proximity, Wall wall)
        {
            foreach (ReferenceWithContext reference in references)
            {
                // Exclude items too far from the start point.
                if (reference.Proximity < proximity)
                {
                    Autodesk.Revit.DB.Element referenceElement = wall.Document.GetElement(reference.GetReference());
                    if (referenceElement is FamilyInstance)
                    {
                        FamilyInstance familyInstance = (FamilyInstance)referenceElement;
                        ElementId familyInstanceId = familyInstance.Id;
                        ElementId wallId = wall.Id;
                        int categoryIdValue = referenceElement.Category.Id.IntegerValue;
                        if (categoryIdValue == (int)BuiltInCategory.OST_Columns || categoryIdValue == (int)BuiltInCategory.OST_StructuralColumns)
                        {
                            // Add the column to the map of wall->columns
                            if (m_columnsOnWall.ContainsKey(wallId))
                            {
                                List<ElementId> columnsOnWall = m_columnsOnWall[wallId];
                                if (!columnsOnWall.Contains(familyInstanceId))
                                    columnsOnWall.Add(familyInstanceId);
                            }
                            else
                            {
                                List<ElementId> columnsOnWall = new List<ElementId>();
                                columnsOnWall.Add(familyInstanceId);
                                m_columnsOnWall.Add(wallId, columnsOnWall);
                            }
                            // Add the column to the complete list of all embedded columns
                            if (!m_allColumnsOnWalls.Contains(familyInstanceId))
                                m_allColumnsOnWalls.Add(familyInstanceId);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Obtains the tangent of the given curve at the given parameter.
        /// </summary>
        /// <param name="curve">The curve.</param>
        /// <param name="parameter">The normalized parameter.</param>
        /// <returns>The normalized tangent vector.</returns>
        private XYZ GetTangentAt(Curve curve, double parameter)
        {
            Transform t = curve.ComputeDerivatives(parameter, true);
            // BasisX is the tangent vector of the curve.
            return t.BasisX.Normalize();
        }

        /// <summary>
        /// Finds the normal to the wall centerline at the given parameter.
        /// </summary>
        /// <param name="wall">The wall.</param>
        /// <param name="curve">The location curve of the wall.</param>
        /// <param name="parameter">The normalized parameter.</param>
        /// <returns>The normalized normal vector.</returns>
        private XYZ GetNormalToWallAt(Wall wall, LocationCurve curve, double parameter)
        {
            Curve wallCurve = curve.Curve;

            // There is no normal at a given point for a line.  We need to get the normal based on the tangent of the wall location curve.
            if (wallCurve is Line)
            {
                XYZ wallDirection = GetTangentAt(wallCurve, 0);
                XYZ wallNormal = new XYZ(wallDirection.Y, wallDirection.X, 0);
                return wallNormal;
            }
            else
            {
                Transform t = wallCurve.ComputeDerivatives(parameter, true);
                // For non-linear curves, BasisY is the normal vector to the curve.
                return t.BasisY.Normalize();
            }
        }

        /// <summary>
        /// Dump wall's curve(end points) to log
        /// </summary>
        /// <param name="wallCurve">Wall curve to be dumped.</param>
        private void LogWallCurve(Line wallCurve)
        {
            Debug.WriteLine("Wall curve is line: ");

            Debug.WriteLine("Start point: " + XYZToString(wallCurve.GetEndPoint(0)));
            Debug.WriteLine("End point: " + XYZToString(wallCurve.GetEndPoint(1)));
        }

        /// <summary>
        /// Format XYZ to string 
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        private String XYZToString(XYZ point)
        {
            return "( " + point.X + ", " + point.Y + ", " + point.Z + ")";
        }

        /// <summary>
        /// Get a 3D view from active document
        /// </summary>
        private void Get3DView(string viewName)
        {
            FilteredElementCollector collector = new FilteredElementCollector(m_app.ActiveUIDocument.Document);
            foreach (Autodesk.Revit.DB.View3D v in collector.OfClass(typeof(View3D)).ToElements())
            {
                // skip view template here because view templates are invisible in project browsers
                if (v != null && !v.IsTemplate && v.Name == viewName)
                {
                    m_view3D = v as Autodesk.Revit.DB.View3D;
                    break;
                }
            }
        }
        #endregion
    }
}
