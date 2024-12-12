//
// (C) Copyright 2003-2009 by Autodesk, Inc.
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
using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Geometry;
using Autodesk.Revit.Parameters;

using Element = Autodesk.Revit.Element;
using GElement = Autodesk.Revit.Geometry.Element; 
    
namespace Revit.SDK.Samples.ReferencePlane.CS
{
    /// <summary>
    /// A object to help locating with geometry data.
    /// </summary>
    public class GeoHelper
    {
        //Defined the precision.
        private const double Precision = 0.0001;

        /// <summary>
        /// Find the bottom face of a face array.
        /// </summary>
        /// <param name="faces">A face array.</param>
        /// <returns>The bottom face of a face array.</returns>
        static public Face GetBottomFace(FaceArray faces)
        {
            Face face = null;
            double elevation = 0;
            double tempElevation = 0;
            Mesh mesh = null;

            foreach (Face f in faces)
            {
                if (IsVerticalFace(f))
                {
                    // If this is a vertical face, it can not be a bottom face to a certainty.
                    continue;
                }

                tempElevation = 0;
                mesh = f.Triangulate();

                foreach (XYZ xyz in mesh.Vertices)
                {
                    tempElevation = tempElevation + xyz.Z;
                }

                tempElevation = tempElevation / mesh.Vertices.Size;

                if (elevation > tempElevation || null == face)
                {
                    // Update the bottom face to which's elevation is the lowest.
                    face = f;
                    elevation = tempElevation;
                }
            }

            // The bottom face is consider as which's average elevation is the lowest, except vertical
            // face.
            return face;
        }

        /// <summary>
        /// Find out the three points which made of a plane.
        /// </summary>
        /// <param name="mesh">A mesh contains many points.</param>
        /// <param name="startPoint">Create a new instance of ReferencePlane.</param>
        /// <param name="endPoint">The free end apply to reference plane.</param>
        /// <param name="thirdPnt">A third point needed to define the reference plane.</param>
        static public void Distribute(Mesh mesh, ref XYZ startPoint, ref XYZ endPoint, ref XYZ thirdPnt)
        {
            int count = mesh.Vertices.Size;
            startPoint = mesh.Vertices.get_Item(0);
            endPoint = mesh.Vertices.get_Item((int)(count / 3));
            thirdPnt = mesh.Vertices.get_Item((int)(count / 3 * 2));
        }

        /// <summary>
        /// Calculate the length between two points.
        /// </summary>
        /// <param name="startPoint">The start point.</param>
        /// <param name="endPoint">The end point.</param>
        /// <returns>The length between two points.</returns>
        static public double GetLength(XYZ startPoint, XYZ endPoint)
        {
            return Math.Sqrt(Math.Pow((endPoint.X - startPoint.X), 2) +
                Math.Pow((endPoint.Y - startPoint.Y), 2) +
                Math.Pow((endPoint.Z - startPoint.Z), 2));
        }

        /// <summary>
        /// The distance between two value in a same axis.
        /// </summary>
        /// <param name="start">start value.</param>
        /// <param name="end">end value.</param>
        /// <returns>The distance between two value.</returns>
        static public double GetDistance(double start, double end)
        {
            return Math.Abs(start - end);
        }

        /// <summary>
        /// Get the vector between two points.
        /// </summary>
        /// <param name="startPoint">The start point.</param>
        /// <param name="endPoint">The end point.</param>
        /// <returns>The vector between two points.</returns>
        static public XYZ GetVector(XYZ startPoint, XYZ endPoint)
        {
            return new XYZ(endPoint.X - startPoint.X,
                endPoint.Y - startPoint.Y, endPoint.Z - startPoint.Z);
        }

        /// <summary>
        /// Determines whether a face is vertical.
        /// </summary>
        /// <param name="face">The face to be determined.</param>
        /// <returns>Return true if this face is vertical, or else return false.</returns>
        static private bool IsVerticalFace(Face face)
        {
            foreach (EdgeArray ea in face.EdgeLoops)
            {
                foreach (Edge e in ea)
                {
                    if (IsVerticalEdge(e))
                    {
                        return true;
                    }
                }
            }

            return false;            
        }

        /// <summary>
        /// Determines whether a edge is vertical.
        /// </summary>
        /// <param name="edge">The edge to be determined.</param>
        /// <returns>Return true if this edge is vertical, or else return false.</returns>
        static private bool IsVerticalEdge(Edge edge)
        {
            XYZArray polyline = edge.Tessellate();
            XYZ verticalVct = new XYZ(0, 0, 1);
            XYZ pointBuffer = polyline.get_Item(0);

            for (int i = 1; i < polyline.Size; i = i + 1)
            {
                XYZ temp = polyline.get_Item(i);
                XYZ vector = GetVector(pointBuffer, temp);
                if (Equal(vector, verticalVct))
                {
                    return true;
                }
                else
                {
                    continue;                    
                }
            }

            return false;
        }

        /// <summary>
        /// Determines whether two vector are equal in x and y axis.
        /// </summary>
        /// <param name="vectorA">The vector A.</param>
        /// <param name="vectorB">The vector B.</param>
        /// <returns>Return true if two vector are equals, or else return false.</returns>
        static private bool Equal(XYZ vectorA, XYZ vectorB)
        {
            bool isNotEqual = (Precision < Math.Abs(vectorA.X - vectorB.X)) ||
                (Precision < Math.Abs(vectorA.Y - vectorB.Y));

            return isNotEqual ? false : true;
        }
    }
}
