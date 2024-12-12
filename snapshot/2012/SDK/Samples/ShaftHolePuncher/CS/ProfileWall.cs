//
// (C) Copyright 2003-2011 by Autodesk, Inc.
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
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit;
using System.Drawing;
using System.Windows.Forms;

namespace Revit.SDK.Samples.ShaftHolePuncher.CS
{
    /// <summary>
    /// ProfileWall class contains the information about profile of a wall,
    /// and contains method to create Opening on a wall
    /// </summary>
    public class ProfileWall : Profile
    {
        private Wall m_data;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="wall">wall to create Opening on</param>
        /// <param name="commandData">object which contains reference of Revit Application</param>
        public ProfileWall(Wall wall, ExternalCommandData commandData)
            : base(commandData)
        {
            m_data = wall;
            List<List<Edge>> faces = GetFaces(m_data);
            m_points = GetNeedPoints(faces);
            m_to2DMatrix = GetTo2DMatrix();
            m_moveToCenterMatrix = ToCenterMatrix();
        }

        /// <summary>
        /// Get points of first face
        /// </summary>
        /// <param name="faces">edges in all faces</param>
        /// <returns>points of first face</returns>
        public override List<List<XYZ>> GetNeedPoints(List<List<Edge>> faces)
        {
            List<Edge> needFace = new List<Edge>();
            List<List<XYZ>> needPoints = new List<List<XYZ>>();
            LocationCurve location = m_data.Location as LocationCurve;
            Curve curve = location.Curve;
            List<XYZ> xyzs = curve.Tessellate() as List<XYZ>;
            Vector4 zAxis = new Vector4(0, 0, 1);

            //if Location curve of wall is line, then return first face
            if (xyzs.Count == 2)
            {
                needFace = faces[0];
            }

            //else we return the face whose normal is Z axis
            foreach (List<Edge> face in faces)
            {
                foreach (Edge edge in face)
                {
                    List<XYZ> edgexyzs = edge.Tessellate() as List<XYZ>;
                    if (xyzs.Count == edgexyzs.Count)
                    {
                        //get the normal of face
                        Vector4 normal = GetFaceNormal(face);
                        Vector4 cross = Vector4.CrossProduct(zAxis, normal);
                        cross.Normalize();
                        if (cross.Length() == 1)
                        {
                            needFace = face;
                        }
                    }
                }
            }
            needFace = faces[0];

            //get points array in edges 
            foreach (Edge edge in needFace)
            {
                List<XYZ> edgexyzs = edge.Tessellate() as List<XYZ>;
                needPoints.Add(edgexyzs);
            }
            return needPoints;
        }

        /// <summary>
        /// Get a matrix which can transform points to 2D
        /// </summary>
        /// <returns>matrix which can transform points to 2D</returns>
        public override Matrix4 GetTo2DMatrix()
        {
            //get the location curve
            LocationCurve location = m_data.Location as LocationCurve;
            Vector4 xAxis = new Vector4(1, 0, 0);
            Vector4 yAxis = new Vector4(0, 1, 0);
            Vector4 zAxis = new Vector4(0, 0, 1);
            Vector4 origin = new Vector4(0, 0, 0);
            if (location != null)
            {
                Curve curve = location.Curve;

                if (!(curve is Autodesk.Revit.DB.Line))
                {
                    throw new Exception("Opening cannot build on this Wall");
                }

                Autodesk.Revit.DB.XYZ start = curve.get_EndPoint(0);
                Autodesk.Revit.DB.XYZ end = curve.get_EndPoint(1);

                xAxis = new Vector4((float)(end.X - start.X),
                    (float)(end.Y - start.Y), (float)(end.Z - start.Z));
                xAxis.Normalize();

                //because in the windows UI, Y axis is downward
                yAxis = new Vector4(0, 0, -1);

                zAxis = Vector4.CrossProduct(xAxis, yAxis);
                zAxis.Normalize();

                origin = new Vector4((float)(end.X + start.X) / 2,
                    (float)(end.Y + start.Y) / 2, (float)(end.Z + start.Z) / 2);
            }
            return new Matrix4(xAxis, yAxis, zAxis, origin);
        }

        /// <summary>
        /// create Opening on wall
        /// </summary>
        /// <param name="points">points used to create Opening</param>
        /// <returns>newly created Opening</returns>
        public override Opening CreateOpening(List<Vector4> points)
        {
            //create Opening on wall
            Autodesk.Revit.DB.XYZ p1 = new Autodesk.Revit.DB.XYZ (points[0].X, points[0].Y, points[0].Z);
            Autodesk.Revit.DB.XYZ p2 = new Autodesk.Revit.DB.XYZ (points[1].X, points[1].Y, points[1].Z);
            return m_docCreator.NewOpening(m_data, p1, p2);
        }
    }
}
