//
// (C) Copyright 2003-2019 by Autodesk, Inc.
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
    /// Utility verifies the input parameters, like curves from Revit document.
    /// It also adapts the input data for winder stairs creation.
    /// </summary>
    class WinderUtil
    {
        /// <summary>
        /// Return the control points from the connected curve elements.
        /// </summary>
        /// <param name="rvtDoc">Revit Document</param>
        /// <param name="crvElements">Connected curve element</param>
        /// <returns>Control points</returns>
        public static IList<XYZ> CalculateControlPoints(Document rvtDoc, IList<ElementId> crvElements)
        {
            double maxOffset = 0.0;
            // All the elements should be line base.
            for (int i = 0; i < crvElements.Count; i++)
            {
                Element curve = rvtDoc.GetElement(crvElements[i]);
                LocationCurve locationCrv = curve.Location as LocationCurve;
                if (locationCrv == null || !(locationCrv.Curve is Line))
                {
                    throw new ArgumentException("The input elements are not Line base.");
                }

                if (Math.Abs(locationCrv.Curve.GetEndPoint(0).Z 
                    - locationCrv.Curve.GetEndPoint(1).Z) > 1.0e-9)
                {
                    throw new AggregateException(
                        "The input curve elements are not in the same elevation plane.");
                }
                if (curve is Wall)
                {
                    maxOffset = Math.Max(maxOffset, ((Wall)curve).Width * 0.5);
                }
            }

            IList<XYZ> controlPoints = CalculateControlPoints2(rvtDoc, crvElements);
            if (controlPoints.Count == 0)
            {
                throw new ArgumentException("The input curve elements are not continues.");
            }
            if (!CheckOrientation(controlPoints))
            {
                throw new ArgumentException(
                    "The input curve elements should have the same orientation: CW or CCW.");
            }
            if (maxOffset > 0.0)
            {
                controlPoints = CalculateOffset(controlPoints, maxOffset);
            }
            return controlPoints;
        }

        /// <summary>
        /// Calculate the max straight steps determined by the control points.
        /// </summary>
        /// <param name="controlPoints">Control points of the stairs</param>
        /// <param name="runWidth">Stairs Run width</param>
        /// <param name="treadDepth">Stairs tread depth</param>
        /// <returns></returns>
        public static IList<uint> CalculateMaxStepsCount(IList<XYZ> controlPoints,
            double runWidth, double treadDepth)
        {
            IList<XYZ> innerPnts = CalculateOffset(controlPoints, runWidth);
            IList<uint> counts = new List<uint>();
            for (int i = 1; i < innerPnts.Count; i++)
            {
                double dist = innerPnts[i].DistanceTo(innerPnts[i - 1]);
                uint count = (uint)(dist / treadDepth);
                counts.Add(count);
            }
            return counts;
        }

        /// <summary>
        /// Check to see if the control points bend to the same direction(CW or CCW).
        /// </summary>
        /// <param name="controlPoints">Control points to test</param>
        /// <returns>true if the controls points bend to the same direction</returns>
        static bool CheckOrientation(IList<XYZ> controlPoints)
        {
            XYZ previousDir = null;
            for (int i = 1; i < controlPoints.Count - 1; i++)
            {
                XYZ dir1 = controlPoints[i] - controlPoints[i - 1];
                XYZ dir2 = controlPoints[i + 1] - controlPoints[i];
                if (previousDir == null)
                {
                    previousDir = dir1.CrossProduct(dir2).Normalize();
                }
                else if (!previousDir.IsAlmostEqualTo(dir1.CrossProduct(dir2).Normalize()))
                {
                    return false;
                }
            }
            return true;
        } 

        /// <summary>
        /// Offset the control points by specified distance.
        /// </summary>
        /// <param name="controlPoints">Control point to offset</param>
        /// <param name="offset">offset distance</param>
        /// <returns>Control points after offsetting</returns>
        static IList<XYZ> CalculateOffset(IList<XYZ> controlPoints, double offset)
        {
            IList<XYZ> innerPnts = new List<XYZ>();

            for (int i = 1; i < controlPoints.Count - 1; i++)
            {
                XYZ dir1 = (controlPoints[i] - controlPoints[i - 1]).Normalize();
                XYZ dir2 = (controlPoints[i + 1] - controlPoints[i]).Normalize();
                // Calculate the bisect direction of the corner.
                XYZ bisectDir = (dir2 - dir1).Normalize();

                // Calculate the step direction of the fist line.
                XYZ stepInside1stDir = new XYZ(-dir1.Y, dir1.X, 0.0);
                if (stepInside1stDir.DotProduct(bisectDir) < 0.0)
                    stepInside1stDir = stepInside1stDir.Negate();

                if (i == 1)
                {
                    innerPnts.Add(controlPoints[i - 1] + stepInside1stDir * offset);
                }

                // Calculate the step direction of the second line.
                XYZ stepInside2ndDir = new XYZ(-dir2.Y, dir2.X, 0.0);
                if (stepInside2ndDir.DotProduct(bisectDir) < 0.0)
                    stepInside2ndDir = stepInside2ndDir.Negate();

                double semiAngle = bisectDir.AngleTo(dir2);
                double slopDist = offset / Math.Sin(semiAngle);

                // Calculate the corner points
                XYZ innerCornerPnt = controlPoints[i] + bisectDir * slopDist;

                innerPnts.Add(innerCornerPnt);

                if (i == controlPoints.Count - 2)
                {
                    innerPnts.Add(controlPoints[i + 1] + stepInside2ndDir * offset);
                }
            }

            return innerPnts;
        }

        /// <summary>
        /// Calculate the control points from the connected curve elements.
        /// </summary>
        /// <param name="rvtDoc">Revit Document</param>
        /// <param name="elements">Connected Curve Elements</param>
        /// <returns>Control points</returns>
        static IList<XYZ> CalculateControlPoints2(Document rvtDoc, IList<ElementId> elements)
        {
            IList<Curve> curves = new List<Curve>();
            for (int i = 0; i < elements.Count; i++)
            {
                Element curve = rvtDoc.GetElement(elements[i]);
                LocationCurve locationCrv = curve.Location as LocationCurve;
                curves.Add(locationCrv.Curve);
            }

            IList<XYZ> controlPoints = new List<XYZ>();
            if (curves.Count == 2)
            {
                Curve curve1 = curves[0];
                Curve curve2 = curves[1];
                XYZ commonPnt = null;
                int index1 = -1, index2 = -1;
                if (HasCommonEndPoint(curve1, curve2, out commonPnt, out index1, out index2))
                {
                    XYZ start = curve1.GetEndPoint(1 - index1);
                    XYZ end = curve2.GetEndPoint(1 - index2);
                    controlPoints.Add(start);
                    controlPoints.Add(commonPnt);
                    controlPoints.Add(end);
                }
            }
            else if (curves.Count == 3)
            {
                Curve curve1 = curves[0];
                Curve curve2 = curves[1];
                Curve curve3 = curves[2];

                XYZ start = null, commonPnt1 = null, commonPnt2 = null, end = null;
                int index1 = -1, index2 = -1, index3 = -1, index4 = -1;

                if (HasCommonEndPoint(curve1, curve2, out commonPnt1, out index1, out index2))
                {
                    if (HasCommonEndPoint(curve1, curve3, out commonPnt2, out index3, out index4))
                    {
                        // common curve is curve1
                        start = curve2.GetEndPoint(1 - index2);
                        end = curve3.GetEndPoint(1 - index4);

                    }
                    else if (HasCommonEndPoint(curve2, curve3, out commonPnt2, out index3, out index4))
                    {
                        // common curve is curve2
                        start = curve1.GetEndPoint(1 - index1);
                        end = curve3.GetEndPoint(1 - index4);
                    }
                }
                else
                {
                    if (HasCommonEndPoint(curve1, curve3, out commonPnt1, out index1, out index2) &&
                        HasCommonEndPoint(curve2, curve3, out commonPnt2, out index3, out index4))
                    {
                        // common curve is curve3
                        start = curve1.GetEndPoint(1 - index1);
                        end = curve2.GetEndPoint(1 - index3);
                    }
                }
                if (start != null)
                {
                    controlPoints.Add(start);
                    controlPoints.Add(commonPnt1);
                    controlPoints.Add(commonPnt2);
                    controlPoints.Add(end);
                }
            }
            return controlPoints;
        }

        /// <summary>
        /// Calculate the common end point of two curves.
        /// </summary>
        /// <param name="crv1">Curve 1</param>
        /// <param name="crv2">Curve 2</param>
        /// <param name="common">Comment point of the two curves</param>
        /// <param name="index1">index of the common point in curve 1</param>
        /// <param name="index2">index of the common point in curve 2</param>
        /// <returns>true if there is one common point, false otherwise</returns>
        static bool HasCommonEndPoint(Curve crv1, Curve crv2, out XYZ common,
            out int index1, out int index2)
        {
            XYZ pnt1 = crv1.GetEndPoint(0);
            XYZ pnt2 = crv1.GetEndPoint(1);

            XYZ pnt3 = crv2.GetEndPoint(0);
            XYZ pnt4 = crv2.GetEndPoint(1);

            if (pnt1.IsAlmostEqualTo(pnt3))
            {
                index1 = 0; index2 = 0;
                common = pnt1;
                return true;
            }
            else if (pnt1.IsAlmostEqualTo(pnt4))
            {
                index1 = 0; index2 = 1;
                common = pnt1;
                return true;
            }
            else if (pnt2.IsAlmostEqualTo(pnt3))
            {
                index1 = 1; index2 = 0;
                common = pnt2;
                return true;
            }
            else if (pnt2.IsAlmostEqualTo(pnt4))
            {
                index1 = 1; index2 = 1;
                common = pnt2;
                return true;
            }
            common = null;
            index1 = index2 = -1;
            return false;
        }
    }
}
