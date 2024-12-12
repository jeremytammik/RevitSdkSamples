//
// (C) Copyright 2003-2013 by Autodesk, Inc. All rights reserved.
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
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;

namespace Revit.SDK.Samples.Site.CS
{
    /// <summary>
    /// Database level utilities for the site editing commands.
    /// </summary>
    class SiteEditingUtils
    {
        /// <summary>
        /// Moves an XYZ point to the elevation specified.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="elevation">The elevation.</param>
        /// <returns>The new point.</returns>
        public static XYZ MoveXYZToElevation(XYZ input, double elevation)
        {
            return input + XYZ.BasisZ * (elevation - input.Z);
        }

        /// <summary>
        /// Gets the average elevation (z) from a collection of points.
        /// </summary>
        /// <param name="existingPoints">The points.</param>
        /// <returns>The average elevation.</returns>
        public static double GetAverageElevation(IList<XYZ> existingPoints)
        {
            if (existingPoints.Count == 0)
            {
                return 0;
            }
            return existingPoints.Average<XYZ>(xyz => xyz.Z);
        }

        /// <summary>
        /// Gets the boundary points for the circular retaining pond surrounding the input point.
        /// </summary>
        /// <param name="center">The point.  The elevation of this point is assumed to be at the bottom of the pond.</param>
        /// <param name="maxRadius">The outer radius for the pond.</param>
        /// <returns>The collection of points representing the pond. </returns>
        public static IList<XYZ> GeneratePondPointsSurrounding(XYZ center, double maxRadius)
        {
            if (maxRadius <= 8)
                throw new Exception("Pond radius must be greater than 8");

            List<XYZ> points = new List<XYZ>();
            points.Add(center);

            GenerateCircleSurrounding(points, center, 1, maxRadius - 8);
            GenerateCircleSurrounding(points, center, 2, maxRadius - 5);
            GenerateCircleSurrounding(points, center, 3, maxRadius - 1);

            return points;
        }

        /// <summary>
        /// Generates the points representing a circular region around the center.
        /// </summary>
        /// <param name="points">The collection of points to which the results are added.</param>
        /// <param name="center">The center.</param>
        /// <param name="deltaElevation">The change in elevation relative to the center.</param>
        /// <param name="radius">The radius of the circular region.</param>
        private static void GenerateCircleSurrounding(IList<XYZ> points, XYZ center, double deltaElevation, double radius)
        {
            for (double theta = 0; theta < 2 * Math.PI; theta += Math.PI / 6.0)
            {
                XYZ targetPoint = center + new XYZ(radius * Math.Cos(theta), radius * Math.Sin(theta), deltaElevation);
                points.Add(targetPoint);
            }
        }

        /// <summary>
        /// Gets the center of the element's bounding box.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>The center of the bounding box.</returns>
        public static XYZ GetCenterOf(Element element)
        {
            BoundingBoxXYZ bbox = element.get_BoundingBox(null);

            return (bbox.Min + bbox.Max) / 2.0;
        }

        /// <summary>
        /// Gets the points from a rectangular outline surrounding the subregion bounding box.
        /// </summary>
        /// <param name="subregion">The subregion.</param>
        /// <returns>The points.</returns>
        public static IList<XYZ> GetPointsFromSubregionRough(TopographySurface subregion)
        {
            BoundingBoxXYZ bbox = subregion.get_BoundingBox(null);

            // Get toposurface points
            TopographySurface toposurface = GetTopographySurfaceHost(subregion);

            Outline outline = new Outline(bbox.Min, bbox.Max);
            IList<XYZ> points = toposurface.FindPoints(outline);

            return points;
        }

        /// <summary>
        /// Gets the points stored by a subregion.
        /// </summary>
        /// <param name="subregion">The subregion.</param>
        /// <returns>The points</returns>
        public static IList<XYZ> GetPointsFromSubregionExact(TopographySurface subregion)
        {
            return subregion.GetPoints();
        }

        /// <summary>
        /// Gets the TopographySurface host for a subregion.
        /// </summary>
        /// <param name="subregion">The subregion.</param>
        /// <returns>The host TopographySurface.</returns>
        public static TopographySurface GetTopographySurfaceHost(TopographySurface subregion)
        {
            TopographySurface toposurface = subregion.Document.GetElement(subregion.AsSiteSubRegion().HostId) as TopographySurface;
            return toposurface;
        }

        /// <summary>
        /// Gets all non-boundary points from a TopographySurface or Subregion.
        /// </summary>
        /// <param name="toposurface">The TopographySurface or Subregion.</param>
        /// <returns>The non-boundary points.</returns>
        public static IList<XYZ> GetNonBoundaryPoints(TopographySurface toposurface)
        {
            IList<XYZ> existingPoints = toposurface.GetPoints();

            // Remove boundary points from the list.
            IList<XYZ> boundaryPoints = toposurface.GetBoundaryPoints();

            foreach (XYZ boundaryPoint in boundaryPoints)
            {
                foreach (XYZ existingPoint in existingPoints)
                {
                    if (boundaryPoint.IsAlmostEqualTo(existingPoint, 1e-05))
                    {
                        existingPoints.Remove(existingPoint);
                        break;
                    }
                }
            }

            return existingPoints;
        }
    }
}
