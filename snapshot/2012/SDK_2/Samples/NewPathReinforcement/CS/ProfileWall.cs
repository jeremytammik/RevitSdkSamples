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
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit;
using System.Drawing;
using System.Windows.Forms;

namespace Revit.SDK.Samples.NewPathReinforcement.CS
{
    /// <summary>
    /// ProfileWall class contains the information about profile of wall,
    /// and contains method to create PathReinforcement on wall
    /// </summary>
    public class ProfileWall : Profile
    {
        private Wall m_data;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="wall">wall to create reinforcement on</param>
        /// <param name="commandData">object which contains reference to Revit Application</param>
        public ProfileWall(Wall wall, ExternalCommandData commandData)
            : base(commandData)
        {
            m_data = wall;
            List<List<Edge>> faces = GetFaces(m_data);
            m_points = GetNeedPoints(faces);
            m_to2DMatrix = GetTo2DMatrix();
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
                        Vector4 normal = GetFaceNormal(face); //get the normal of face
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
                    throw new Exception("Path Reinforcement cannot build on this Wall");
                }

                Autodesk.Revit.DB.XYZ start = curve.get_EndPoint(0);
                Autodesk.Revit.DB.XYZ end = curve.get_EndPoint(1);

                //because we create PathReinforcement on the back of wall
                //so we need make X axis reverse
                xAxis = new Vector4(-(float)(end.X - start.X),
                    -(float)(end.Y - start.Y), -(float)(end.Z - start.Z));
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
        /// create PathReinforcement on wall
        /// </summary>
        /// <param name="points">points used to create PathReinforcement</param>
        /// <param name="flip">used to specify whether new PathReinforcement is Filp</param>
        /// <returns>new created PathReinforcement</returns>
        public override PathReinforcement CreatePathReinforcement(List<Vector4> points, bool flip)
        {
            Autodesk.Revit.DB.XYZ p1, p2; Line curve;
            CurveArray curves = m_appCreator.NewCurveArray();
            for (int i = 0; i < points.Count - 1; i++)
            {
                p1 = new Autodesk.Revit.DB.XYZ (points[i].X, points[i].Y, points[i].Z);
                p2 = new Autodesk.Revit.DB.XYZ (points[i + 1].X, points[i + 1].Y, points[i + 1].Z);
                curve = m_appCreator.NewLine(p1, p2, true);
                curves.Append(curve);
            }

            //draw PathReinforcement on wall
            return m_docCreator.NewPathReinforcement(m_data, curves, flip);
        }
    }
}
