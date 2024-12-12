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
namespace Revit.SDK.Samples.AreaReinCurve.CS
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows.Forms;

    using Autodesk.Revit;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.DB.Structure;

    using GeoElement = Autodesk.Revit.DB.GeometryElement;
    using Element = Autodesk.Revit.DB.Element;

    /// <summary>
    /// provide some common geometry judgement and calculate method
    /// </summary>
    class GeomUtil
    {
        const double PRECISION = 0.00001;    //precision when judge whether two doubles are equal

        /// <summary>
        /// judge whether given 4 lines can form a rectangular
        /// </summary>
        /// <param name="lines"></param>
        /// <returns>is rectangular</returns>
        /// <summary>
        /// judge whether given 4 lines can form a rectangular
        /// </summary>
        /// <param name="lines"></param>
        /// <returns>is rectangular</returns>
        public static bool IsRectangular(CurveArray curves)
        {
            if (curves.Size != 4)
            {
                return false;
            }

            Line[] lines = new Line[4];
            for (int i = 0; i < 4; i++)
            {
                lines[i] = curves.get_Item(i) as Line;
                if (null == lines[i])
                {
                    return false;
                }
            }

            Line iniLine = lines[0];
            Line[] verticalLines = new Line[2];
            Line paraLine = null;
            int index = 0;
            for (int i = 1; i < 4; i++)
            {
                if (IsVertical(lines[0], lines[i]))
                {
                    verticalLines[index] = lines[i];
                    index++;
                }
                else
                {
                    paraLine = lines[i];
                }
            }
            if (index != 2)
            {
                return false;
            }
            bool flag = IsVertical(paraLine, verticalLines[0]);
            return flag;
        }

        /// <summary>
        /// judge whether two lines are vertical
        /// </summary>
        /// <param name="line1"></param>
        /// <param name="line2"></param>
        /// <returns></returns>
        public static bool IsVertical(Line line1, Line line2)
        {
            Autodesk.Revit.DB.XYZ  vector1 = SubXYZ(line1.get_EndPoint(0), line1.get_EndPoint(1));
            Autodesk.Revit.DB.XYZ  vector2 = SubXYZ(line2.get_EndPoint(0), line2.get_EndPoint(1));

            double result = DotMatrix(vector1, vector2);

            if (Math.Abs(result) < PRECISION)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// subtraction of two Autodesk.Revit.DB.XYZ as Matrix
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        private static Autodesk.Revit.DB.XYZ  SubXYZ (Autodesk.Revit.DB.XYZ  p1, Autodesk.Revit.DB.XYZ  p2)
        {
            double x = p1.X - p2.X;
            double y = p1.Y - p2.Y;
            double z = p1.Z - p2.Z;

            Autodesk.Revit.DB.XYZ  result = new Autodesk.Revit.DB.XYZ (x, y, z);
            return result;
        }

        /// <summary>
        /// dot product of two Autodesk.Revit.DB.XYZ as Matrix
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        private static double DotMatrix(Autodesk.Revit.DB.XYZ  p1, Autodesk.Revit.DB.XYZ  p2)
        {
            double v1 = p1.X;
            double v2 = p1.Y;
            double v3 = p1.Z;

            double u1 = p2.X;
            double u2 = p2.Y;
            double u3 = p2.Z;

            double result = v1 * u1 + v2 * u2 + v3 * u3;

            return result;
        }

        /// <summary>
        /// judge whether the subtraction of two doubles is less than the internal decided precision
        /// </summary>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        /// <returns></returns>
        private static bool IsEqual(double d1, double d2)
        {
            double diff = Math.Abs(d1 - d2);
            if (diff < PRECISION)
            {
                return true;
            }
            return false;
        }
    }
}
