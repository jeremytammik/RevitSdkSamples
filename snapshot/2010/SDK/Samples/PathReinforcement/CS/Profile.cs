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
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Autodesk.Revit;
using Autodesk.Revit.Geometry;
using Autodesk.Revit.Elements;
using System.Drawing.Drawing2D;

namespace Revit.SDK.Samples.PathReinforcement.CS
{
    /// <summary>
    /// This class stores the geometry information of path reinforcement.
    /// </summary>
    class Profile
    {
        /// <summary>
        /// field used to store path reinforcement.
        /// </summary>
        private Autodesk.Revit.Elements.PathReinforcement m_pathRein;

        /// <summary>
        /// field used to store external command data.
        /// </summary>
        private Autodesk.Revit.ExternalCommandData m_commandData;

        /// <summary>
        /// field used to store the geometry curves of path reinforcement.
        /// 3d data.
        /// </summary>
        private List<XYZArray> m_curves = new List<XYZArray>();

        /// <summary>
        /// store path 3D.
        /// </summary>
        private List<XYZArray> m_path = new List<XYZArray>();

        /// <summary>
        /// field used to store the bound of the curves of path reinforcement.
        /// 2d data.
        /// </summary>
        private BoundingBoxUV m_box = new BoundingBoxUV();

        /// <summary>
        /// field used to store the geometry data of curves of path reinforcement.
        /// 2d data.
        /// </summary>
        private List<UVArray> m_point2d = new List<UVArray>();

        /// <summary>
        /// store path 2D.
        /// </summary>
        private List<UVArray> m_path2d = new List<UVArray>();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pathRein">selected path reinforcement element.</param>
        /// <param name="commandData">External command data</param>
        public Profile(Autodesk.Revit.Elements.PathReinforcement pathRein, ExternalCommandData commandData)
        {
            m_pathRein = pathRein;
            m_commandData = commandData;
            Tessellate();
            ComputeBound();
            ComputePathTo2D();
        }

        /// <summary>
        /// Draw the curves of path reinforcement.
        /// </summary>
        /// <param name="graphics">Gdi object, used to draw curves of path reinforcement.</param>
        /// <param name="size">Bound to limit the size of the whole picture</param>
        /// <param name="pen">Gdi object,determine the color of the line.</param>
        public void Draw(Graphics graphics, Size size, Pen pen)
        {
            UV delta = m_box.Max - m_box.Min;
            float scaleX = size.Width / (float)delta.U;
            float scaleY = size.Width / (float)delta.V;
            float scale = scaleY > scaleX ? scaleX : scaleY;
            scale *= 0.90f;

            GraphicsContainer contain = graphics.BeginContainer();
            {
                //set graphics coordinate system to picture center
                //and  flip the yAxis.
                graphics.Transform = new Matrix(1, 0, 0, -1, size.Width / 2, size.Height / 2);

                //construct a matrix to transform the origin point to Bound center.
                UV center = (m_box.Min + m_box.Max) / 2;
                Matrix toCenter = new Matrix(1, 0, 0, 1, -(float)center.U, -(float)center.V);

                bool isDrawFinished = false;
                List<UVArray> point2d = m_point2d;
                Pen tmpPen = pen;

                while (!isDrawFinished)
                {
                    foreach (UVArray arr in point2d)
                    {
                        for (int i = 0; i < arr.Size - 1; i++)
                        {
                            //get the two connection points to draw a line between them.
                            UV uv1 = arr.get_Item(i);
                            UV uv2 = arr.get_Item(i + 1);
                            PointF[] points = new PointF[] { 
                            new PointF((float)uv1.U, (float)uv1.V), 
                            new PointF((float)uv2.U, (float)uv2.V) };

                            //transform points to bound center.
                            toCenter.TransformPoints(points);

                            //Zoom(Scale) the points to fit the picture box.
                            PointF pf1 = new PointF(points[0].X * scale, points[0].Y * scale);
                            PointF pf2 = new PointF(points[1].X * scale, points[1].Y * scale);

                            //draw a line between pf1 and pf2.
                            graphics.DrawLine(tmpPen, pf1, pf2);
                        }
                    }
                    if (point2d == m_path2d)
                    {
                        isDrawFinished = true;
                    }
                    point2d = m_path2d;
                    tmpPen = Pens.Blue;
                }
            }
            graphics.EndContainer(contain);
        }

        /// <summary>
        /// Transform 3d path to 2d path.
        /// </summary>
        /// <returns></returns>
        private void ComputePathTo2D()
        {
            Matrix4 transform = GetActiveViewMatrix().Inverse();
            foreach (XYZArray arr in m_path)
            {
                UVArray uvarr = new UVArray();
                foreach (XYZ xyz in arr)
                {
                    Vector4 tmpVector = transform.Transform(new Vector4(xyz));
                    UV tmpUv = new UV();
                    tmpUv.U = tmpVector.X;
                    tmpUv.V = tmpVector.Y;
                    uvarr.Append(tmpUv);
                }
                m_path2d.Add(uvarr);
            }
        }

        /// <summary>
        /// Compute the bound of the curves of path reinforcement.
        /// </summary>
        private void ComputeBound()
        {
            //make the bound
            UV min = m_box.get_Bounds(0);
            UV max = m_box.get_Bounds(1);

            Matrix4 transform = GetActiveViewMatrix().Inverse();

            bool isFirst = true;
            foreach (XYZArray arr in m_curves)
            {
                UVArray uvarr = new UVArray();
                foreach (XYZ xyz in arr)
                {
                    Vector4 tmpVector = transform.Transform(new Vector4(xyz));
                    UV tmpUv = new UV();
                    tmpUv.U = tmpVector.X;
                    tmpUv.V = tmpVector.Y;
                    uvarr.Append(tmpUv);

                    if (isFirst)
                    {
                        isFirst = false;
                        min.U = tmpUv.U;
                        min.V = tmpUv.V;
                        max.U = tmpUv.U;
                        max.V = tmpUv.V;
                    }
                    if (tmpUv.U < min.U)
                    {
                        min.U = tmpUv.U;
                    }
                    else if (tmpUv.U > max.U)
                    {
                        max.U = tmpUv.U;
                    }
                    if (tmpUv.V < min.V)
                    {
                        min.V = tmpUv.V;
                    }
                    else if (tmpUv.V > max.V)
                    {
                        max.V = tmpUv.V;
                    }
                }
                m_point2d.Add(uvarr);
            }
            m_box.Min = min;
            m_box.Max = max;
        }

        /// <summary>
        /// Tessellate the curves of path reinforcement.
        /// </summary>
        private void Tessellate()
        {
            Options option = new Options();
            option.DetailLevel = Options.DetailLevels.Fine;
            Autodesk.Revit.Geometry.Element geoElem = m_pathRein.get_Geometry(option);
            GeometryObjectArray geoArray = geoElem.Objects;

            foreach (GeometryObject geo in geoArray)
            {
                if (geo is Curve)
                {
                    Curve curve = geo as Curve;
                    m_curves.Add(curve.Tessellate());
                }
            }

            foreach (ModelCurve modelCurve in m_pathRein.Curves)
            {
                m_path.Add(modelCurve.GeometryCurve.Tessellate());
            }
        }

        /// <summary>
        /// Get view matrix from active view.
        /// </summary>
        /// <returns>view matrix</returns>
        private Matrix4 GetActiveViewMatrix()
        {
            View activeView = m_commandData.Application.ActiveDocument.ActiveView;
            XYZ vZAxis = activeView.ViewDirection;
            XYZ vXAxis = activeView.RightDirection;
            XYZ vYAxis = activeView.UpDirection;

            return new Matrix4(new Vector4(vXAxis), new Vector4(vYAxis), new Vector4(vZAxis));
        }
    }
}
