//
// (C) Copyright 2003-2012 by Autodesk, Inc.
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


namespace Revit.SDK.Samples.CreateBeamSystem.CS
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Drawing;

    using Autodesk.Revit.DB;

    /// <summary>
    /// utility class contains some methods deal with 3D arithmetic
    /// </summary>
    public class GeometryUtil
    {
        /// <summary>
        /// The Application Creation object is used to create new instances of utility objects. 
        /// </summary>
        public static Autodesk.Revit.Creation.Application CreApp;

        /// <summary>
        /// judge whether two XYZs are equal
        /// </summary>
        /// <param name="pnt1">first XYZ</param>
        /// <param name="pnt2">second XYZ</param>
        /// <returns>is equal</returns>
        public static bool CompareXYZ(Autodesk.Revit.DB.XYZ  pnt1, Autodesk.Revit.DB.XYZ  pnt2)
        {
            return (MathUtil.CompareDouble(pnt1.X, pnt2.X) &&
                    MathUtil.CompareDouble(pnt1.Y, pnt2.Y) &&
                    MathUtil.CompareDouble(pnt1.Z, pnt2.Z));
        }

        /// <summary>
        /// sorted lines end to end to make a closed loop profile
        /// if input lines can't make a profile, null will return
        /// </summary>
        /// <param name="originLines">lines to be sorted</param>
        /// <returns>sorted lines which can make a closed loop profile</returns>
        public static List<Line> SortLines(List<Line> originLines)
        {
            // at least 3 lines to form the profile
            if (originLines.Count < 3)
            {
                return null;
            }

            List<Line> lines  = new List<Line>(originLines);
            List<Line> result = new List<Line>();

            // sorted line end to end in order
            result.Add(lines[0]);
            Autodesk.Revit.DB.XYZ  intersectPnt = lines[0].get_EndPoint(1);
            lines[0]         = null;

            for (int i = 0; i < lines.Count; i++)
            {    
                for (int j = 1; j < lines.Count; j++)
                {
                    if (null == lines[j])
                    {
                        continue;
                    }

                    if (CompareXYZ(lines[j].get_EndPoint(0), intersectPnt))
                    {
                        result.Add(lines[j]);
                        intersectPnt = lines[j].get_EndPoint(1);
                        lines[j] = null;
                        break;
                    }
                    else if (CompareXYZ(lines[j].get_EndPoint(1), intersectPnt))
                    {
                        Autodesk.Revit.DB.XYZ  startPnt = lines[j].get_EndPoint(1);
                        Autodesk.Revit.DB.XYZ  endPnt = lines[j].get_EndPoint(0);
                        lines[j] = null;
                        Line inversedLine = CreApp.NewLine(startPnt, endPnt, true);
                        result.Add(inversedLine);
                        intersectPnt = inversedLine.get_EndPoint(1);
                        break;
                    }
                }
            }

            // there is line doesn't included in the closed loop
            if (result.Count != lines.Count)
            {
                return null;
            }

            // the last point in the sorted loop is same to the firs point
            if (!CompareXYZ(intersectPnt, result[0].get_EndPoint(0)))
            {
                return null;
            }

            // make sure there is only one closed region enclosed by the closed loop
            for (int i = 0; i < result.Count - 2; i++)
            {
                for (int j = i + 2; j < result.Count; j++)
                {
                    if (i == 0 && j == (result.Count - 1))
                    {
                        continue;
                    }

                    Line2D line1 = ConvertTo2DLine(result[i]);
                    Line2D line2 = ConvertTo2DLine(result[j]);
                    int count = Line2D.FindIntersection(line1, line2);
                    // line shouldn't intersect with lines which not adjoin to it
                    if (count > 0)
                    {
                        return null;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// judge whether the lines are in the same horizontal plane
        /// </summary>
        /// <param name="lines">lines to be judged</param>
        /// <returns>is in the same horizontal plane</returns>
        public static bool InSameHorizontalPlane(List<Line> lines)
        {
            // all the Z coordinate of lines' start point and end point should be equal
            Autodesk.Revit.DB.XYZ  firstPnt = lines[0].get_EndPoint(0);
            for (int i = 0; i < lines.Count; i++)
            {
                if (!MathUtil.CompareDouble(lines[i].get_EndPoint(0).Z, firstPnt.Z) ||
                    !MathUtil.CompareDouble(lines[i].get_EndPoint(1).Z, firstPnt.Z))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// use the X and Y coordinate of 3D Line to new a Line2D instance
        /// </summary>
        /// <param name="line">3D Line</param>
        /// <returns>2D Line</returns>
        private static Line2D ConvertTo2DLine(Line line)
        {
            PointF pnt1 = new PointF((float)line.get_EndPoint(0).X, (float)line.get_EndPoint(0).Y);
            PointF pnt2 = new PointF((float)line.get_EndPoint(1).X, (float)line.get_EndPoint(1).Y);
            return new Line2D(pnt1, pnt2);
        }
    }

    /// <summary>
    /// utility class contains some methods deal with some general arithmetic
    /// </summary>
    public class MathUtil
    {
        /// <summary>
        /// the minimum double used to compare
        /// </summary>
        public const double Double_Epsilon = 0.00001;

        /// <summary>
        /// the minimum positive float used to compare to as zero
        /// </summary>
        public const float Float_Epsilon = 0.00001f;

        /// <summary>
        /// forbidden default constructor
        /// </summary>
        private MathUtil()
        {
        }

        /// <summary>
        /// compare whether 2 double is equal using internal precision
        /// </summary>
        /// <param name="d1">first value</param>
        /// <param name="d2">second value</param>
        /// <returns>is Equal</returns>
        public static bool CompareDouble(double d1, double d2)
        {
            return Math.Abs(d1 - d2) < Double_Epsilon;
        }

        /// <summary>
        /// dot multiply two vector
        /// </summary>
        /// <param name="pnt1">first vector</param>
        /// <param name="pnt2">second vector</param>
        /// <returns>result</returns>
        public static float Dot(PointF pnt1, PointF pnt2)
        {
            return pnt1.X * pnt2.X + pnt1.Y * pnt2.Y;
        }

        /// <summary>
        /// multiply a float with a vector
        /// </summary>
        /// <param name="f">float value</param>
        /// <param name="pnt">vector</param>
        /// <returns>result</returns>
        public static PointF Multiply(float f, PointF pnt)
        {
            return new PointF(f * pnt.X, f * pnt.Y);
        }

        /// <summary>
        /// add 2 vector
        /// </summary>
        /// <param name="f1">first vector</param>
        /// <param name="f2">second vector</param>
        /// <returns>result</returns>
        public static PointF Add(PointF f1, PointF f2)
        {
            return new PointF(f1.X + f2.X, f1.Y + f2.Y);
        }

        /// <summary>
        /// subtract 2 vector
        /// </summary>
        /// <param name="f1">first vector</param>
        /// <param name="f2">second vector</param>
        /// <returns>result</returns>
        public static PointF Subtract(PointF f1, PointF f2)
        {
            return new PointF(f1.X - f2.X, f1.Y - f2.Y);
        }

        /// <summary>
        /// find and calculate the intersection of two interval [u0, u1] and [v0, v1]
        /// </summary>
        /// <param name="u0">first interval</param>
        /// <param name="u1">first interval</param>
        /// <param name="v0">second interval</param>
        /// <param name="v1">second interval</param>
        /// <param name="w">2 intersections</param>
        /// <returns>number of intersection</returns>
        public static int FindIntersection(float u0, float u1, float v0, float v1, ref float[] w)
        {
            if (u1 < v0 || u0 > v1)
            {
                return 0;
            }

            if (u1 == v0)
            {
                w[0] = u1;
                return 1;
            }

            if (u0 == v1)
            {
                w[0] = u0;
                return 1;
            }

            if (u1 > v0)
            {
                if (u0 < v1)
                {
                    if (u0 < v0)
                    {
                        w[0] = v0;
                    }
                    else
                    {
                        w[0] = u0;
                    }

                    if (u1 > v1)
                    {
                        w[1] = v1;
                    }
                    else
                    {
                        w[1] = u1;
                    }

                    return 2;
                }
                else
                {
                    w[0] = u0;
                    return 1;
                }
            }
            else
            {
                w[0] = u1;
                return 1;
            }
        }

        /// <summary>
        /// get the minimum value of 2 float
        /// </summary>
        /// <param name="f1">first float</param>
        /// <param name="f2">second float</param>
        /// <returns>minimum float</returns>
        public static float GetMin(float f1, float f2)
        {
            if (f1 < f2)
            {
                return f1;
            }

            return f2;
        }

        /// <summary>
        /// get the maximum value of 2 float
        /// </summary>
        /// <param name="f1">first float</param>
        /// <param name="f2">second float</param>
        /// <returns>maximum float</returns>
        public static float GetMax(float f1, float f2)
        {
            if (f1 > f2)
            {
                return f1;
            }

            return f2;
        }
    }
}
