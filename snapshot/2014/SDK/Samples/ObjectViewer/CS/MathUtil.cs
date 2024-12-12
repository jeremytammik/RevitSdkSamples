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
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.DB;

namespace Revit.SDK.Samples.ObjectViewer.CS
{
    /// <summary>
    /// utility class provide arithmetic of matrix
    /// </summary>
    public static class MathUtil
    {
        /// <summary>
        /// multiply cross two matrix
        /// </summary>
        /// <param name="m1">left matrix</param>
        /// <param name="m2">right matrix</param>
        /// <returns>result matrix</returns>
        public static Autodesk.Revit.DB.XYZ[] MultiCross(Autodesk.Revit.DB.XYZ[] m1, Autodesk.Revit.DB.XYZ[] m2)
        {
            Autodesk.Revit.DB.XYZ[] result = new Autodesk.Revit.DB.XYZ[3];

            for (int i = 0; i < 3; i++)
            {
                result[i] = new Autodesk.Revit.DB.XYZ();
            }

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        switch (j)
                        {
                            case 0:
                                result[i] = new XYZ(
                                    result[i].X + ((m1[i])[k] * (m2[k])[j]),
                                    result[i].Y,
                                    result[i].Z);
                                break;
                            case 1:
                                result[i] = new XYZ(
                                    result[i].X,
                                    result[i].Y + (m1[i])[k] * (m2[k])[j],
                                    result[i].Z);
                                break;
                            case 2:
                                result[i] = new XYZ(
                                    result[i].X,
                                    result[i].Y,
                                    result[i].Z + (m1[i])[k] * (m2[k])[j]);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// subtract two Autodesk.Revit.DB.XYZ as Matrix
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static Autodesk.Revit.DB.XYZ SubXYZ(Autodesk.Revit.DB.XYZ lhs, Autodesk.Revit.DB.XYZ rhs)
        {
            double x = lhs.X - rhs.X;
            double y = lhs.Y - rhs.Y;
            double z = lhs.Z - rhs.Z;

            Autodesk.Revit.DB.XYZ result = new Autodesk.Revit.DB.XYZ (x, y, z);
            return result;
        }

        /// <summary>
        /// Add two Autodesk.Revit.DB.XYZ as Matrix
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static Autodesk.Revit.DB.XYZ AddXYZ(Autodesk.Revit.DB.XYZ lhs, Autodesk.Revit.DB.XYZ rhs)
        {
            double x = lhs.X + rhs.X;
            double y = lhs.Y + rhs.Y;
            double z = lhs.Z + rhs.Z;

            Autodesk.Revit.DB.XYZ result = new Autodesk.Revit.DB.XYZ (x, y, z);
            return result;
        }

        /// <summary>
        /// divide a Autodesk.Revit.DB.XYZ by a number
        /// </summary>
        /// <param name="lhs">divided XYZ</param>
        /// <param name="rhs">number</param>
        /// <returns>result</returns>
        public static Autodesk.Revit.DB.XYZ DivideXYZ(Autodesk.Revit.DB.XYZ lhs, double rhs)
        {
            return new Autodesk.Revit.DB.XYZ (lhs.X / rhs, lhs.Y / rhs, lhs.Z / rhs);
        }

        /// <summary>
        /// get the coordinates from old coordinate system in the new coordinate system
        /// </summary>
        /// <param name="point"></param>
        /// <param name="transform"></param>
        /// <returns></returns>
        public static Autodesk.Revit.DB.XYZ GetBasis(Autodesk.Revit.DB.XYZ point, Transform transform)
        {
            double x = point.X;
            double y = point.Y;
            double z = point.Z;
            double x2;
            double y2;
            double z2;

            //transform basis of the old coordinate system in the new coordinate system
            Autodesk.Revit.DB.XYZ b0 = transform.get_Basis(0);
            Autodesk.Revit.DB.XYZ b1 = transform.get_Basis(1);
            Autodesk.Revit.DB.XYZ b2 = transform.get_Basis(2);
            Autodesk.Revit.DB.XYZ origin = transform.Origin;

            //transform the origin of the old coordinate system in the new coordinate system
            x2 = x * b0.X + y * b1.X + z * b2.X + origin.X;
            y2 = x * b0.Y + y * b1.Y + z * b2.Y + origin.Y;
            z2 = x * b0.Z + y * b1.Z + z * b2.Z + origin.Z;

            Autodesk.Revit.DB.XYZ newPoint = new Autodesk.Revit.DB.XYZ (x2, y2, z2);
            return newPoint;
        }

        /// <summary>
        /// transform a Vector instance to a Autodesk.Revit.DB.XYZ instance
        /// </summary>
        /// <param name="v">transformed Vector</param>
        /// <returns>result XYZ</returns>
        public static Autodesk.Revit.DB.XYZ Vector2XYZ(Vector v)
        {
            return new Autodesk.Revit.DB.XYZ (v.X, v.Y, v.Z);
        }

        /// <summary>
        /// transform a Autodesk.Revit.DB.XYZ instance to a Vector instance
        /// </summary>
        /// <param name="pnt">transformed XYZ</param>
        /// <returns>result Vector</returns>
        public static Vector XYZ2Vector(Autodesk.Revit.DB.XYZ pnt)
        {
            return new Vector(pnt.X, pnt.Y, pnt.Z);
        }
    }
}
