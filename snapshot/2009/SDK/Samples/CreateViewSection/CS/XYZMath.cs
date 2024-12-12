//
// (C) Copyright 2003-2008 by Autodesk, Inc.
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
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC.
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

using Autodesk.Revit.Geometry;

namespace Revit.SDK.Samples.CreateViewSection.CS
{
    /// <summary>
    /// The helper class which give some operation about point and vector.
    /// The point and vector are both presented by XYZ structure.
    /// </summary>
    public class XYZMath
    {
        // Private Members
        const Double PRECISION = 0.0000000001;  // Define a precision of double data


        // Methods
        /// <summary>
        /// Find the middle point of the line.
        /// </summary>
        /// <param name="first">the start point of the line</param>
        /// <param name="second">the end point of the line</param>
        /// <returns>the middle point of the line</returns>
        public static XYZ FindMidPoint(XYZ first, XYZ second)
        {
            double x = (first.X + second.X) / 2;
            double y = (first.Y + second.Y) / 2;
            double z = (first.Z + second.Z) / 2;
            XYZ midPoint = new XYZ(x, y, z);
            return midPoint;
        }

        /// <summary>
        /// Find the distance between two points
        /// </summary>
        /// <param name="first">the first point</param>
        /// <param name="second">the first point</param>
        /// <returns>the distance of two points</returns>
        public static double FindDistance(XYZ first, XYZ second)
        {
            double x = first.X - second.X;
            double y = first.Y - second.Y;
            double z = first.Z - second.Z;
            return Math.Sqrt(x * x + y * y + z * z);
        }

        /// <summary>
        /// Find the direction vector from first point to second point
        /// </summary>
        /// <param name="first">the first point</param>
        /// <param name="second">the second point</param>
        /// <returns>the direction vector</returns>
        public static XYZ FindDirection(XYZ first, XYZ second)
        {
            double x = second.X - first.X;
            double y = second.Y - first.Y;
            double z = second.Z - first.Z;
            double distance = FindDistance(first, second);
            XYZ direction = new XYZ(x / distance, y / distance, z / distance);
            return direction;
        }

        /// <summary>
        /// Find the right direction vector, 
        /// which is the same meaning of RightDirection property in View class
        /// </summary>
        /// <param name="viewDirection">the view direction vector</param>
        /// <returns>the right direction vector</returns>
        public static XYZ FindRightDirection(XYZ viewDirection)
        {
            // Because this example only allow the beam to be horizontal,
            // the created viewSection should be vertical, 
            // the same thing can also be found when the user select wall or floor.
            // So only need to turn 90 degree around Z axes will get Right Direction.  

            double x = -viewDirection.Y;
            double y = viewDirection.X;
            double z = viewDirection.Z;
            XYZ direction = new XYZ(x, y, z);
            return direction;         
        }

        /// <summary>
        /// Find the up direction vector, 
        /// which is the same meaning of UpDirection property in View class
        /// </summary>
        /// <param name="viewDirection">the view direction vector</param>
        /// <returns>the up direction vector</returns>
        public static XYZ FindUpDirection(XYZ viewDirection)
        {
            // Because this example only allow the beam to be horizontal,
            // the created viewSection should be vertical, 
            // the same thing can also be found when the user select wall or floor.
            // So UpDirection should be z axes.
            XYZ direction = new XYZ(0, 0, 1);
            return direction;
        }

        /// <summary>
        /// Find the middle point of a profile.
        /// This method is used to find out middle point of the selected wall or floor.
        /// </summary>
        /// <param name="curveArray">the array of curve which form the profile</param>
        /// <returns>the middle point of the profile</returns>
        public static XYZ FindMiddlePoint(CurveArray curveArray)
        {
            // First form a point array which include all the end points of the curves
            XYZArray array = new XYZArray();
            foreach (Curve curve in curveArray)
            {
                XYZ first = curve.get_EndPoint(0);
                XYZ second = curve.get_EndPoint(1);
                array.Append(first);
                array.Append(second);
            }

            // Second find the max and min value of three coordinate
            double maxX = array.get_Item(0).X;  // the max x coordinate in the array
            double minX = array.get_Item(0).X;  // the min x coordinate in the array
            double maxY = array.get_Item(0).Y;  // the max y coordinate in the array
            double minY = array.get_Item(0).Y;  // the min y coordinate in the array
            double maxZ = array.get_Item(0).Z;  // the max z coordinate in the array
            double minZ = array.get_Item(0).Z;  // the min z coordinate in the array

            foreach (XYZ curve in array)
            {
                if (maxX < curve.X)
                {
                    maxX = curve.X;
                }
                if (minX > curve.X)
                {
                    minX = curve.X;
                }
                if (maxY < curve.Y)
                {
                    maxY = curve.Y;
                }
                if (minY > curve.Y)
                {
                    minY = curve.Y;
                }
                if (maxZ < curve.Z)
                {
                    maxZ = curve.Z;
                }
                if (minZ > curve.Z)
                {
                    minZ = curve.Z;
                }
            }

            // Third form the middle point using the average of max and min values
            double x = (maxX + minX) / 2;
            double y = (maxY + minY) / 2;
            double z = (maxZ + minZ) / 2;
            XYZ midPoint = new XYZ(x, y, z);
            return midPoint;
        }

        /// <summary>
        /// Find the view direction vector, 
        /// which is the same meaning of ViewDirection property in View class
        /// </summary>
        /// <param name="curveArray">the curve array which form wall's AnalyticalModel</param>
        /// <returns>the view direction vector</returns>
        public static XYZ FindWallViewDirection(CurveArray curveArray)
        {
            XYZ direction = new XYZ();
            foreach (Curve curve in curveArray)
            {
                XYZ startPoint = curve.get_EndPoint(0);
                XYZ endPoint = curve.get_EndPoint(1);
                double distanceX = startPoint.X - endPoint.X;
                double distanceY = startPoint.Y - endPoint.Y;
                if(-PRECISION > distanceX || PRECISION < distanceX
                    || -PRECISION > distanceY || PRECISION < distanceY)
                {
                    XYZ first = new XYZ(startPoint.X, startPoint.Y, 0);
                    XYZ second = new XYZ(endPoint.X, endPoint.Y, 0);
                    direction = FindDirection(first, second);
                    break;
                }
            }
            return direction;
        }

        /// <summary>
        /// Find the view direction vector, 
        /// which is the same meaning of ViewDirection property in View class
        /// </summary>
        /// <param name="curveArray">the curve array which form floor's AnalyticalModel</param>
        /// <returns>the view direction vector</returns>
        public static XYZ FindFloorViewDirection(CurveArray curveArray)
        {
            // Because the floor is always on the level,
            // so each curve can give the direction information.
            Curve curve = curveArray.get_Item(0);
            XYZ first = curve.get_EndPoint(0);
            XYZ second = curve.get_EndPoint(1);
            return FindDirection(first, second);
        }
    }
}
