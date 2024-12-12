//
// (C) Copyright 2003-2013 by Autodesk, Inc.
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
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using XYZ = Autodesk.Revit.DB.XYZ;

namespace SectionPropertiesExplorer
{
    /// <summary>
    /// Geometry utility
    /// </summary>
    internal class Geometry
    {
        public static bool IsEqual(double val1, double val2, double eps)
        {
            return (System.Math.Abs(val1 - val2) < eps);
        }

        public static bool PointsAreAlmostEqual(double[] pt1, double[] pt2)
        {
            double EpsilonLength = 5.0e-03;
            return IsEqual(pt1[0], pt2[0], EpsilonLength) && IsEqual(pt1[1], pt2[1], EpsilonLength) && IsEqual(pt1[2], pt2[2], EpsilonLength);
        }

        public static double Distance2D(double[] p1, double[] p2)
        {
            double x = p1[0] - p2[0];
            double y = p1[1] - p2[1];
            return System.Math.Sqrt(x * x + y * y);
        }

        public static double[] Subtract(double[] pEnd, double[] pStart)
        {
            double EpsilonLength = 5.0e-03;

            double x = pEnd[0] - pStart[0];
            double y = pEnd[1] - pStart[1];
            double z = pEnd[2] - pStart[2];

            if (System.Math.Abs(x) < EpsilonLength) x = 0;
            if (System.Math.Abs(y) < EpsilonLength) y = 0;
            if (System.Math.Abs(z) < EpsilonLength) z = 0;

            return new double[3] { x, y, z };
        }

        public static double[] GetParamLine(double[] pt1, double[] pt2)
        {
            double EpsilonLength = 5.0e-03;

            double a = 0;
            double b = 0;

            double xa = pt2[0] - pt1[0];
            double ya = pt2[1] - pt1[1];

            if (System.Math.Abs(xa) > EpsilonLength)
            {
                a = ya / xa;
                b = pt1[1] - a * pt1[0];
            }
            return new double[2] { a, b };
        }

        public static bool IsPointOnLine(double[] pt1Line, double[] pt2Line, double[] pt)
        {
            double EpsilonLength = 5.0e-03;

            double ParamT = -1;
            double[] Vect = Subtract(pt2Line, pt1Line);

            if (System.Math.Abs(Vect[0]) > EpsilonLength)
            { ParamT = (pt[0] - pt1Line[0]) / Vect[0]; }
            else if (System.Math.Abs(Vect[1]) > EpsilonLength)
            { ParamT = (pt[1] - pt1Line[1]) / Vect[1]; }
            else if (System.Math.Abs(Vect[2]) > EpsilonLength)
            { ParamT = (pt[2] - pt1Line[2]) / Vect[2]; }

            if ((ParamT > 0 || System.Math.Abs(ParamT) < EpsilonLength) &&
                (ParamT < 1 || System.Math.Abs(ParamT - 1) < EpsilonLength))
            {
                if (ParamT < 0) ParamT = 0;
                if (ParamT > 1) ParamT = 1;

                Vect[0] = pt1Line[0] + ParamT * Vect[0];
                Vect[1] = pt1Line[1] + ParamT * Vect[1];
                Vect[2] = pt1Line[2] + ParamT * Vect[2];

                return PointsAreAlmostEqual(Vect, pt);
            }
            return false;
        }

        public static bool IsIntersectionFirstLine2D(double[] pt11, double[] pt12, double[] pt21, double[] pt22, out double[] pt)
        {
            double EpsilonLength = 5.0e-03;

            double x = 0, y = 0;
            double[] Vers1 = Subtract(pt12, pt11);
            double[] Vers2 = Subtract(pt22, pt21);
 
            pt = new double[3];

            if (System.Math.Abs(System.Math.Abs(DotProduct(Vers1, Vers2)) - 1) < EpsilonLength)
            { return false; }

            double[] W1 = GetParamLine(pt11, pt12);
            double[] W2 = GetParamLine(pt21, pt22);

            if ((W1[0] != 0 || W1[1] != 0) && (W2[0] != 0 || W2[1] != 0))
            {
                x = (W2[1] - W1[1]) / (W1[0] - W2[0]);
                y = W1[0] * x + W1[1];
            }
            else if (W1[0] == 0 && W1[1] == 0 && (W2[0] == 0 && W2[1] == 0))
            {
                if (System.Math.Abs(System.Math.Abs(Vers1[0]) - 1) < EpsilonLength)
                { x = pt21[0]; y = pt11[1]; }
                else
                { x = pt11[0]; y = pt21[1]; }
            }
            else if (W1[0] == 0 && W1[1] == 0)
            {
                if (System.Math.Abs(System.Math.Abs(Vers1[0]) - 1) < EpsilonLength)
                { y = pt11[1]; if (W2[0] != 0) x = (y - W2[1]) / W2[0]; }
                else
                { x = pt11[0]; y = W2[0] * x + W2[1]; }
            }
            else
            {
                if (System.Math.Abs(System.Math.Abs(Vers2[0]) - 1) < EpsilonLength)
                { y = pt21[1]; if (W1[0] != 0) x = (y - W1[1]) / W1[0]; }
                else
                { x = pt21[0]; y = W1[0] * x + W1[1]; }
            }

            pt[0] = x;
            pt[1] = y;
            pt[2] = pt11[2];

            if (!IsPointOnLine(pt11, pt12, pt))
            {
                return false;
            }

            return true;
        }

        public static double DotProduct(double[] vector1, double[] vector2)
        {
            return vector1[0] * vector2[0] + vector1[1] * vector2[1] + vector1[2] * vector2[2];
        }

        public static double AngleBetweenX(double[] p1, double[] p2)
        {
            double EpsilonLength = 5.0e-03;

            double Lx = p2[0] - p1[0];
            double Ly = p2[1] - p1[1];
            double Lxy = System.Math.Sqrt(Lx * Lx + Ly * Ly);
            double Ang = 0;

            if (Lxy == 0) return Ang;

            if (Lx != 0)
            {
                Ang = Lx / Lxy;
                double len = System.Math.Sqrt(-Ang * Ang + 1);
                if (System.Math.Abs(len) > EpsilonLength)
                { Ang = System.Math.Atan(-Ang / len) + 2 * System.Math.Atan(1); }
                else
                { Ang = (Lx < -EpsilonLength) ? -System.Math.PI : 0; }

                if (Ly < -EpsilonLength) Ang = -Ang;
            }
            else if (Ly != 0)
            {
                Ang = Ly / Lxy;
                double len = System.Math.Sqrt(-Ang * Ang + 1);

                if (System.Math.Abs(len) > EpsilonLength)
                { Ang = System.Math.Atan(-Ang / len); }
                else
                { Ang = System.Math.PI / 2 * Ly / System.Math.Abs(Ly); }

                if (Lx < -EpsilonLength) Ang = -Ang;
            }

            return Ang;
        }
    }
    
    internal class BoundaryBox
    {
        #region constructor
        //--------------------------------------------------------------

        public BoundaryBox()
        {
            Children = new List<BoundaryBox>();
        }

        //--------------------------------------------------------------
        #endregion

        #region properties

        public XYZ Min
        {
            get;
            set;
        }

        public XYZ Max
        {
            get;
            set;
        }

        public List<BoundaryBox> Children
        {
            get;
            set;
        }

        #endregion

        #region methods
        //--------------------------------------------------------------

        public void AddPoint(XYZ pt)
        {
            if (Min == null)
            {
                Min = new XYZ(pt.X, pt.Y, pt.Z);
                Max = new XYZ(pt.X, pt.Y, pt.Z);
                return;
            }

            Min = new XYZ(System.Math.Min(Min.X, pt.X), System.Math.Min(Min.Y, pt.Y), System.Math.Min(Min.Z, pt.Z));

            Max = new XYZ(System.Math.Max(Max.X, pt.X), System.Math.Max(Max.Y, pt.Y), System.Math.Max(Max.Z, pt.Z));
        }

        public void AddBoundingBox(BoundaryBox bb)
        {
            AddPoint(bb.Min);
            AddPoint(bb.Max);
        }

        public void Reset()
        {
            Min = null;
            Max = null;
        }

        public XYZ GetCenter()
        {
            if (Min == null || Max == null)
                return new XYZ();

            return new XYZ((Min.X + Max.X) / 2, (Min.Y + Max.Y) / 2, (Min.Z + Max.Z) / 2);
        }

        /// <summary>
        /// Determines whether bounding box intersects with current one
        /// </summary>
        /// <param name="bb">bounding box</param>
        /// <param name="Epsilon">epsilon</param>
        /// <returns>true if given bounding box intersects the current one, else false.</returns>
        public bool Intersects(BoundaryBox bb, double Epsilon)
        {
            if (Min == null || Max == null)
                return false;

            if (Min.X - Epsilon > bb.Max.X || Min.Y - Epsilon > bb.Max.Y || Min.Z - Epsilon > bb.Max.Z)
                return false;

            if (bb.Min.X - Epsilon > Max.X || bb.Min.Y - Epsilon > Max.Y || bb.Min.Z - Epsilon > Max.Z)
                return false;

            return true;
        }

        /// <summary>
        /// Determines whether bounding box contains the other bounding box
        /// </summary>
        /// <param name="bb">bounding box</param>
        /// <param name="Epsilon">epsilon</param>
        /// <returns>true if given bounding box intersects the current one, else false.</returns>
        public bool Contains(BoundaryBox bb, double epsilon)
        {
            if (Min == null || Max == null)
                return false;

            if (Max.X + epsilon < bb.Max.X || Max.Y + epsilon < bb.Max.Y || Max.Z + epsilon < bb.Max.Z)
                return false;

            if (bb.Min.X + epsilon < Min.X || bb.Min.Y + epsilon < Min.Y || bb.Min.Z + epsilon < Min.Z)
                return false;

            return true;
        }

        //--------------------------------------------------------------
        #endregion
    }

    /// <summary>
    /// This is extension for Revit's XYZ class.    
    /// </summary>
    internal static class XYZ_Extension
    {
        public static XYZ Clone(this XYZ pt)
        {
            return new XYZ(pt.X, pt.Y, pt.Z);
        }

        public static double[] getDblArray(this XYZ pt)
        {
            return new double[] { pt.X, pt.Y, pt.Z };
        }

        public static double[] getDblArray(this IList<XYZ> list)
        {
            double[] ar = new double[list.Count * 3];

            for (int i = 0; i < list.Count; i++)
            {
                XYZ item = list[i];

                ar[i * 3] = item.X;
                ar[i * 3 + 1] = item.Y;
                ar[i * 3 + 2] = item.Z;
            }

            return ar;
        }
    }
}
