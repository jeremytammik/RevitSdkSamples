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
using System.Collections.Generic;
using Autodesk.Revit.DB;

namespace Revit.SDK.Samples.WinderStairs.CS
{
    /// <summary>
    /// This represents a single point winder corner.
    /// </summary>
    class WinderSinglePoint : WinderCorner
    {
        /*   
        *   Direction1
        *    |      
        *    |
        *    V    *(CenterPoint)
        *    |
        *    |
        *    |----->---- Direction2
        * CornerPoint
        */

        /// <summary>
        /// Calculated point: All the riser lines are pass through this point.
        /// </summary>
        public XYZ CenterPoint { get; private set; }

        /// <summary>
        /// Constructor to initialize the basic fields of the winder region.
        /// </summary>
        /// <param name="cornerPnt">Corner Point</param>
        /// <param name="dir1">Enter direction</param>
        /// <param name="dir2">Leave direction</param>
        /// <param name="steps">Number of steps</param>
        public WinderSinglePoint(XYZ cornerPnt, XYZ dir1, XYZ dir2, uint steps)
            : base(cornerPnt, dir1, dir2, steps)
        {
        }

        /// <summary>
        /// Calculate the CenterPoint base on Australia single-point layout algorithm.
        /// </summary>
        /// <param name="runWidth">Stairs Runwidth</param>
        /// <param name="offset1">CenterPoint Offset from the first inner boundary</param>
        /// <param name="offset2">CenterPoint Offset from the second inner boundary</param>
        public void Construct(double runWidth, double offset1, double offset2)
        {
            XYZ bisectDir = (Direction2 - Direction1).Normalize();
            XYZ perpendicularDir1 = new XYZ(-Direction1.Y, Direction1.X, 0);
            XYZ perpendicularDir2 = new XYZ(-Direction2.Y, Direction2.X, 0);
            if (bisectDir.DotProduct(perpendicularDir1) < 0)
            {
                perpendicularDir1 = perpendicularDir1.Negate();
                perpendicularDir2 = perpendicularDir2.Negate();
            }

            Line line1Offset = Line.CreateUnbound(
                CornerPoint + perpendicularDir1 * (runWidth + offset1), Direction1);
            Line line2Offset = Line.CreateUnbound(
                CornerPoint + perpendicularDir2 * (runWidth + offset2), Direction2);

            IntersectionResultArray xsect;
            line1Offset.Intersect(line2Offset, out xsect);
            CenterPoint = xsect.get_Item(0).XYZPoint;

            Line line1 = Line.CreateUnbound(CornerPoint, Direction1.Negate());
            Line line2 = Line.CreateUnbound(CornerPoint, Direction2);

            var proj1 = line1.Project(CenterPoint);
            var proj2 = line2.Project(CenterPoint);

            Distance1 = proj1.Parameter;
            Distance2 = proj2.Parameter;
        }

        /// <summary>
        /// Generate the sketch base on the calculated CenterPoint:
        /// Divide the corner into NumSteps evenly.
        /// </summary>
        public override void GenerateSketch(double runWidth,
            IList<Curve> outerBoundary, IList<Curve> walkPath,
            IList<Curve> innerBoundary, IList<Curve> riserLines)
        {
            // Calculate the bisect direction in the corner.
            XYZ bisectDir = (Direction2 - Direction1).Normalize();
            XYZ perpendicularDir1 = new XYZ(-Direction1.Y, Direction1.X, 0);
            XYZ perpendicularDir2 = new XYZ(-Direction2.Y, Direction2.X, 0);
            if (bisectDir.DotProduct(perpendicularDir1) < 0)
            {
                perpendicularDir1 = perpendicularDir1.Negate();
                perpendicularDir2 = perpendicularDir2.Negate();
            }

            // Determine the rotation axis and the angle span in the corner.
            double runwidth_2 = runWidth * 0.5;
            XYZ zDir = XYZ.BasisZ;
            double winderAngleSpan = perpendicularDir1.AngleOnPlaneTo(perpendicularDir2, zDir);
            if (winderAngleSpan > Math.PI)
            {
                zDir = zDir.Negate();
                winderAngleSpan = Math.PI * 2.0 - winderAngleSpan;
            }

            // Calculate the corner points
            double diagonalDist = runwidth_2 / Math.Cos(winderAngleSpan * 0.5);
            XYZ middleCornerPnt = CornerPoint + bisectDir * diagonalDist;
            XYZ innerCornerPnt = middleCornerPnt + bisectDir * diagonalDist;
            XYZ middleStart = StartPoint + perpendicularDir1 * runwidth_2;
            XYZ innerStart = StartPoint + perpendicularDir1 * runWidth;
            XYZ middleEnd = EndPoint + perpendicularDir2 * runwidth_2;
            XYZ innerEnd = EndPoint + perpendicularDir2 * runWidth;
            XYZ middleEnd0 = innerCornerPnt - perpendicularDir1 * runwidth_2;
            XYZ middleEnd1 = innerCornerPnt - perpendicularDir2 * runwidth_2;

            // Generate the two outer boundary lines
            Line outerLine1 = Line.CreateBound(StartPoint, CornerPoint);
            outerBoundary.Add(outerLine1);
            Line outerLine2 = Line.CreateBound(CornerPoint, EndPoint);
            outerBoundary.Add(outerLine2);

            // Generate the first inner line
            Line innerLine1 = null;
            if (!innerStart.IsAlmostEqualTo(innerCornerPnt))
            {
                innerLine1 = Line.CreateBound(innerStart, innerCornerPnt);
                innerBoundary.Add(innerLine1);
            }

            // Generate the second inner line
            Line innerLine2 = null;
            if (!innerCornerPnt.IsAlmostEqualTo(innerEnd))
            {
                innerLine2 = Line.CreateBound(innerCornerPnt, innerEnd);
                innerBoundary.Add(innerLine2);
            }

            // Generate the first middle line
            Line middleLine1 = null;
            if (!middleStart.IsAlmostEqualTo(middleEnd0))
            {
                middleLine1 = Line.CreateBound(middleStart, middleEnd0);
                walkPath.Add(middleLine1);
            }

            // Generate the middle fillet arc
            XYZ middleCenter = innerCornerPnt - bisectDir * runwidth_2;
            Arc middleArc = Arc.Create(middleEnd0, middleEnd1, middleCenter);
            walkPath.Add(middleArc);

            // Genera the second middle line
            Line middleLine2 = null;
            if (!middleEnd1.IsAlmostEqualTo(middleEnd))
            {
                middleLine2 = Line.CreateBound(middleEnd1, middleEnd);
                walkPath.Add(middleLine2);
            }

            //
            // Generate the riser lines by dividing the corner into NumSteps evenly.
            //
            double winderAnglePerStep = winderAngleSpan / NumSteps;
            Transform tsf = Transform.CreateRotation(zDir, winderAnglePerStep);
            XYZ currentDir = perpendicularDir1;
            for (int i = 0; i < NumSteps - 1; ++i)
            {
                XYZ rayDir = tsf.OfVector(currentDir); currentDir = rayDir;
                Line ray = Line.CreateUnbound(CenterPoint, rayDir);

                IntersectionResultArray xsect;
                if (ray.Intersect(outerLine1, out xsect) == SetComparisonResult.Overlap ||
                    ray.Intersect(outerLine2, out xsect) == SetComparisonResult.Overlap)
                {
                    XYZ xOuter = xsect.get_Item(0).XYZPoint;
                    XYZ xInner = null;
                    if (innerLine1 == null || innerLine2 == null)
                    {
                        if (ray.Distance(innerCornerPnt) < 1.0e-9)
                        {
                            xInner = innerCornerPnt;
                        }
                    }
                    if (xInner == null)
                    {
                        if (innerLine1 != null && ray.Intersect(innerLine1, out xsect) == SetComparisonResult.Overlap)
                        {
                            xInner = xsect.get_Item(0).XYZPoint;
                        }
                        else if (innerLine2 != null && ray.Intersect(innerLine2, out xsect) == SetComparisonResult.Overlap)
                        {
                            xInner = xsect.get_Item(0).XYZPoint;
                        }
                    }

                    if (xInner == null)
                    {
                        throw new ArgumentException("Bad Input.");
                    }
                    riserLines.Add(Line.CreateBound(xOuter, xInner));
                }
                else
                {
                    throw new ArgumentException("Bad Input.");
                }
            }
        }

        /// <summary>
        /// Move the center point
        /// </summary>
        public override void Move(XYZ vector)
        {
            base.Move(vector);
            CenterPoint += vector;
        }
    }
}
