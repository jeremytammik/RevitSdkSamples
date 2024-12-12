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
using Autodesk.Revit.Elements;
using Autodesk.Revit.Structural;
using Autodesk.Revit.Geometry;
using Autodesk.Revit;
using System.Drawing;

namespace Revit.SDK.Samples.NewOpenings.CS
{
    /// <summary>
    /// base class of ProfileFloor and ProfileWall
    /// contain the profile infomation and can make matrix to transform point to 2D plane
    /// </summary>
    public abstract class Profile
    {
        #region members
        /// <summary>
        ///Wall or Floor element 
        /// </summary>
        protected Autodesk.Revit.Element m_dataProfile; 

        /// <summary>
        /// geometry object [face]
        /// </summary>
        protected List<Edge> m_face ;

        /// <summary>
        ///  command data
        /// </summary>
        protected Autodesk.Revit.ExternalCommandData m_commandData;

        /// <summary>
        /// Application creator
        /// </summary>
        protected Autodesk.Revit.Creation.Application m_appCreator;

        /// <summary>
        /// Document creator
        /// </summary>
        protected Autodesk.Revit.Creation.Document m_docCreator;
        #endregion

        /// <summary>
        /// abstract method to create Opening
        /// </summary>
        public abstract void DrawOpening(List<Vector4> points, ToolType type);

        /// <summary>
        /// draw profile of wall or floor in 2D
        /// </summary>
        /// <param name="graphics">form graphic</param>
        /// <param name="pen">pen use to draw line in pictureBox</param>
        /// <param name="matrix4">matrix used to transform points between 3d and 2d.</param>>
        public void Draw2D(Graphics graphics, Pen pen, Matrix4 matrix4)
        {
            foreach (Edge edge in m_face)
            {
                XYZArray points = edge.Tessellate();
                for (int i = 0; i < points.Size - 1; i++)
                {
                    XYZ point1 = points.get_Item(i);
                    XYZ point2 = points.get_Item(i + 1);

                    Vector4 v1 = new Vector4(point1);
                    Vector4 v2 = new Vector4(point2);

                    v1 = matrix4.TransForm(v1);
                    v2 = matrix4.TransForm(v2);
                    graphics.DrawLine(pen, new Point((int)v1.X, (int)v1.Y), 
                        new Point((int)v2.X, (int)v2.Y));
                }
            }
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="elem">selected element</param>
        /// <param name="commandData">ExternalCommandData</param>
        public Profile(Autodesk.Revit.Element elem, ExternalCommandData commandData)
        {
            m_dataProfile = elem;
            m_commandData = commandData;
            m_appCreator = m_commandData.Application.Create;
            m_docCreator = m_commandData.Application.ActiveDocument.Create;

            List<List<Edge>> faces = GetFaces(m_dataProfile);
            m_face = GetNeedFace(faces);
        }

        /// <summary>
        /// Get edges of element's profile
        /// </summary>
        /// <param name="elem">selected element</param>
        public List<List<Edge>> GetFaces(Autodesk.Revit.Element elem)
        {
            List<List<Edge>> faceEdges = new List<List<Edge>>();
            Options options = m_appCreator.NewGeometryOptions();
            options.DetailLevel = Options.DetailLevels.Medium;
            options.ComputeReferences = true;
            Autodesk.Revit.Geometry.Element geoElem = elem.get_Geometry(options);

            GeometryObjectArray gObjects = geoElem.Objects;
            foreach (GeometryObject geo in gObjects)
            {
                Solid solid = geo as Solid;
                if (solid != null)
                {
                    EdgeArray edges = solid.Edges;
                    FaceArray faces = solid.Faces;
                    foreach (Face face in faces)
                    {
                        EdgeArrayArray edgeArrarr = face.EdgeLoops;
                        foreach (EdgeArray edgeArr in edgeArrarr)
                        {
                            List<Edge> edgesList = new List<Edge>();
                            foreach (Edge edge in edgeArr)
                            {
                                edgesList.Add(edge);
                            }
                            faceEdges.Add(edgesList);
                        }
                    }
                }
            }
            return faceEdges;
        }

        /// <summary>
        /// Get Face Normal
        /// </summary>
        /// <param name="face">edges in a face</param>
        private Vector4 GetFaceNormal(List<Edge> face)
        {
            Edge eg0 = face[0];
            Edge eg1 = face[1];

            XYZArray points = eg0.Tessellate();
            XYZ start = points.get_Item(0);
            XYZ end = points.get_Item(1);

            Vector4 vStart = new Vector4((float)start.X, (float)start.Y, (float)start.Z);
            Vector4 vEnd = new Vector4((float)end.X, (float)end.Y, (float)end.Z);
            Vector4 vSub = vEnd - vStart;

            points = eg1.Tessellate();
            start = points.get_Item(0);
            end = points.get_Item(1);

            vStart = new Vector4((float)start.X, (float)start.Y, (float)start.Z);
            vEnd = new Vector4((float)end.X, (float)end.Y, (float)end.Z);
            Vector4 vSub2 = vEnd - vStart;

            Vector4 result = vSub.CrossProduct(vSub2);
            result.Normalize();
            return result;
        }

        /// <summary>
        /// Get First Face
        /// </summary>
        /// <param name="faces">edges in all faces</param>
        private List<Edge> GetNeedFace(List<List<Edge>> faces)
        {
            if(m_dataProfile is Wall)
            {
                return GetWallFace(faces);
            }
            return faces[0];
        }

        /// <summary>
        /// Get a matrix which can transform points to 2D
        /// </summary>
        public Matrix4 To2DMatrix()
        {
            if(m_dataProfile is Wall)
            {
                return WallMatrix();
            }
            XYZArray eg0 = m_face[0].Tessellate();
            XYZArray eg1 = m_face[1].Tessellate();

            Vector4 v1 = new Vector4((float)eg0.get_Item(0).X, 
                (float)eg0.get_Item(0).Y, (float)eg0.get_Item(0).Z);

            Vector4 v2 = new Vector4((float)eg0.get_Item(1).X, 
                (float)eg0.get_Item(1).Y, (float)eg0.get_Item(1).Z);
            Vector4 v21 = v1 - v2;
            v21.Normalize();

            Vector4 v3 = new Vector4((float)eg1.get_Item(0).X, 
                (float)eg1.get_Item(0).Y, (float)eg1.get_Item(0).Z);

            Vector4 v4 = new Vector4((float)eg1.get_Item(1).X, 
                (float)eg1.get_Item(1).Y, (float)eg1.get_Item(1).Z);
            Vector4 v43 = v4 - v3;
            v43.Normalize();

            Vector4 vZAxis = Vector4.CrossProduct(v43, v21);
            Vector4 vYAxis = Vector4.CrossProduct(vZAxis, v43);
            vYAxis.Normalize();
            vZAxis.Normalize();
            Vector4 vOrigin = (v4 + v1) / 2;

            Matrix4 result = new Matrix4(v43, vYAxis, vZAxis, vOrigin);
            return result;
        }

        /// <summary>
        /// wall matrix
        /// </summary>
        /// <returns></returns>
        public Matrix4 WallMatrix()
        {
            //get the location curve
            LocationCurve location = m_dataProfile.Location as LocationCurve;
            Vector4 xAxis = new Vector4(1,0,0);
            Vector4 yAxis = new Vector4(0, 1, 0);
            Vector4 zAxis = new Vector4(0, 0, 1);
            Vector4 origin = new Vector4(0, 0, 0);
            if(location != null)
            {
                Curve curve = location.Curve;
                XYZ start = curve.get_EndPoint(0);
                XYZ end = curve.get_EndPoint(1);

                xAxis = new Vector4((float)(end.X - start.X), 
                    (float)(end.Y - start.Y), (float)(end.Z- start.Z));
                xAxis.Normalize();

                yAxis = new Vector4(0 ,0, 1);

                zAxis = Vector4.CrossProduct(xAxis, yAxis);
                zAxis.Normalize();

                origin = new Vector4((float)(end.X + start.X) / 2, 
                    (float)(end.Y + start.Y) / 2, (float)(end.Z + start.Z) / 2);
            }
            return new Matrix4(xAxis, yAxis, zAxis, origin);
        }

        /// <summary>
        /// get wall face
        /// </summary>
        /// <param name="faces"></param>
        /// <returns></returns>
        private List<Edge> GetWallFace(List<List<Edge>> faces)
        {
            LocationCurve location = m_dataProfile.Location as LocationCurve;
            Curve curve = location.Curve;
            XYZArray xyzs = curve.Tessellate();
            Vector4 zAxis = new Vector4(0, 0, 1);

            if(xyzs.Size == 2)
            {
                return faces[0];
            }

            foreach(List<Edge> face in faces)
            {
                foreach(Edge edge in face)
                {
                    XYZArray edgexyzs = edge.Tessellate();
                    if(xyzs.Size == edgexyzs.Size)
                    {                   
                        Vector4 normal = GetFaceNormal(face);
                        Vector4 cross = Vector4.CrossProduct(zAxis,normal);
                        cross.Normalize();
                        if(cross.Length() == 1)
                        {
                            return face;
                        }             
                    }
                }
            }
            return faces[0];
        }

        /// <summary>
        /// Get a matrix which can move points to origin
        /// </summary>
        public Matrix4 ToCenterMatrix()
        {
            //translate the origin to bound center
            PointF[] bounds = GetFaceBounds();
            PointF min = bounds[0];
            PointF max = bounds[1];
            PointF center = new PointF((min.X + max.X) / 2, (min.Y + max.Y) / 2);
            return new Matrix4(new Vector4(center.X, center.Y, 0)); 
        }

        /// <summary>
        /// Get Face Bounds
        /// </summary>
        public PointF[] GetFaceBounds()
        {
            Matrix4 matrix = To2DMatrix();
            Matrix4 inverseMatrix = matrix.Inverse();
            float minX = 0, maxX = 0, minY = 0, maxY = 0;
            bool bFirstPoint = true;
            foreach (Edge edge in m_face)
            {
                XYZArray points = edge.Tessellate();

                foreach (XYZ point in points)
                {
                    Vector4 v = new Vector4(point);
                    Vector4 v1 = inverseMatrix.TransForm(v);

                    if (bFirstPoint)
                    {
                        minX = maxX = v1.X;
                        minY = maxY = v1.Y;
                        bFirstPoint = false;
                    }
                    else
                    {
                        if (v1.X < minX)
                        {
                            minX = v1.X;
                        }
                        else if (v1.X > maxX)
                        {
                            maxX = v1.X;
                        }

                        if (v1.Y < minY)
                        {
                            minY = v1.Y;
                        }
                        else if (v1.Y > maxY)
                        {
                            maxY = v1.Y;
                        }
                    }
                }
            }
            PointF[] resultPoints = new PointF[2] { 
                new PointF(minX, minY), new PointF(maxX, maxY) };
            return resultPoints;
        }
    }
}
