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
using System.Drawing.Drawing2D;
using System.Collections.ObjectModel;

using Autodesk.Revit.Geometry;

namespace Revit.SDK.Samples.ObjectViewer.CS
{
    /// <summary>
    /// provide methods to draw objects
    /// </summary>
    public class Sketch3D
    {
        // ratio of margin to canvas width
        private const float MarginRatio = 0.1f;
        // every frame distance to move the drawing
        private const float MoveDistance = 2.0f;
        // every frame ratio to zoom the drawing
        private const float ZoomRatio = 0.05f;
        // scale ratio of region showing the 2D geometry to the whole display region
        private const float Drawing2DScale2D = 0.5f;

        private List<PointF[]> m_curve3Ds = new List<PointF[]>();   // all 3D points
        private List<PointF[]> m_curve2Ds = new List<PointF[]>();   // all 2D points
        private Graphics3DData m_data3D;        // 3D graphic data
        private Graphics2DData m_data2D;        // 2D graphic data
        // defines a local geometric transform
        private Matrix m_transform3D;
        private Matrix m_transform2D;
        // displayed BoundingBox in canvas
        private RectangleF m_displayBBox = new RectangleF(0.0f, 0.0f, 10.0f, 10.0f);

        /// <summary>
        /// Displayed BoundingBox in canvas
        /// </summary>
        public RectangleF DisplayBBox
        {
            get
            {
                return m_displayBBox;
            }
            set
            {
                m_displayBBox = value;
                Calculate2DTransform();
                Calculate3DTransform();
                UpdateDataAndEvent();
                if (null != UpdateViewEvent)
                {
                    UpdateViewEvent();
                }
            }
        }

        /// <summary>
        /// 2D graphic data
        /// </summary>
        public Graphics2DData Data2D
        {
            get
            {
                return m_data2D;
            }
            set
            {
                m_data2D = value;
                Calculate2DTransform();
                UpdateDataAndEvent();
                if (null != UpdateViewEvent)
                {
                    UpdateViewEvent();
                }
            }
        }

        /// <summary>
        /// 3D graphic data
        /// </summary>
        public Graphics3DData Data3D
        {
            get
            {
                return m_data3D;
            }
            set
            {
                m_data3D = value;
                UpdateDataAndEvent();
                if (null != UpdateViewEvent)
                {
                    UpdateViewEvent();
                }
            }
        }

        /// <summary>
        /// view data update
        /// </summary>
        public event UpdateViewDelegate UpdateViewEvent;

        /// <summary>
        /// The default constructor
        /// </summary>
        /// <param name="data3D">a list contain all the 3d data</param>
        /// <param name="data2D">a list contain all the 3d data</param>
        public Sketch3D(Graphics3DData data3D, Graphics2DData data2D)
        {
            m_data3D = data3D;
            m_data2D = data2D;
            Calculate3DTransform();
            Calculate2DTransform();
            UpdateDataAndEvent();
        }

        /// <summary>
        /// draw the line contain in m_lines in 2d Preview
        /// </summary>
        /// <param name="graphics">Graphics to draw</param>
        /// <returns></returns>
        public void Draw(Graphics graphics)
        {
            if (m_data3D.NumPoints == 0 && m_data2D.NumPoints == 0)
            {
                graphics.Clear(Color.LightGray);
                return;
            }

            graphics.Clear(Color.White);
            DrawCurves(graphics, m_curve3Ds, m_transform3D, new Pen(Color.DarkGreen));
            DrawCurves(graphics, m_curve2Ds, m_transform2D, new Pen(Color.DarkBlue));
            if (m_data2D.NumPoints > 0)
            {
                GraphicsPath gPath = new GraphicsPath();
                gPath.AddRectangle(m_data2D.BBox);
                gPath.Transform(m_transform2D);
                SolidBrush brush = new SolidBrush(Color.FromArgb(70, Color.LightSkyBlue));
                graphics.FillPath(brush, gPath);
            }
        }

        /// <summary>
        /// zoom the displayed drawing in canvas
        /// </summary>
        /// <param name="zoomIn">true for zoom in, false for zoom out</param>
        public void Zoom(bool zoomIn)
        {
            if (zoomIn)
            {
                m_transform3D.Scale(1f - ZoomRatio, 1f - ZoomRatio, MatrixOrder.Append);
            }
            else
            {
                m_transform3D.Scale(1f + ZoomRatio, 1f + ZoomRatio, MatrixOrder.Append);
            }
            if (null != UpdateViewEvent)
            {
                UpdateViewEvent();
            }
        }

        /// <summary>
        /// move view in horizontal direction
        /// </summary>
        /// <param name="left">left or right</param>
        public void MoveX(bool left)
        {
            if (left)
            {
                m_transform3D.Translate(-MoveDistance, 0, MatrixOrder.Append);
            }
            else
            {
                m_transform3D.Translate(MoveDistance, 0, MatrixOrder.Append);
            }
            if (null != UpdateViewEvent)
            {
                UpdateViewEvent();
            }
        }

        /// <summary>
        /// move view in vertical direction
        /// </summary>
        /// <param name="up">up or down</param>
        public void MoveY(bool up)
        {
            if (up)
            {
                m_transform3D.Translate(0, MoveDistance, MatrixOrder.Append);
            }
            else
            {
                m_transform3D.Translate(0, -MoveDistance, MatrixOrder.Append);
            }
            if (null != UpdateViewEvent)
            {
                UpdateViewEvent();
            }
        }

        /// <summary>
        /// update drawing data according and related Event
        /// </summary>
        private void UpdateDataAndEvent()
        {
            Initialize3DData();
            Initialize2DData();
            m_data3D.UpdateViewEvent += new UpdateViewDelegate(DataUpdateViewEvent);
        }

        /// <summary>
        /// initialize 3D drawing data
        /// </summary>
        private void Initialize3DData()
        {
            m_curve3Ds.Clear();
            foreach (List<Vector> vectors in m_data3D.ConstCurves)
            {
                PointF[] pnts = new PointF[vectors.Count];
                m_curve3Ds.Add(pnts);
                for (int i = 0; i < vectors.Count; i++)
                {
                    pnts[i] = new PointF((float)vectors[i].X, (float)vectors[i].Y);
                }
            }
        }

        /// <summary>
        /// initialize 2D drawing data
        /// </summary>
        private void Initialize2DData()
        {
            m_curve2Ds.Clear();
            m_curve2Ds.AddRange(m_data2D.ConstCurves);
        }

        /// <summary>
        /// Draw curves
        /// </summary>
        /// <param name="graphics">graphics handle</param>
        /// <param name="curves">curves to be drawn</param>
        /// <param name="transform">transform between canvas and graphic data</param>
        /// <param name="pen">pen to draw</param>
        private void DrawCurves(Graphics graphics, List<PointF[]> curves, Matrix transform, Pen pen)
        {
            foreach (PointF[] curve in curves)
            {
                GraphicsPath gPath = new GraphicsPath();
                if (curve.Length == 0)
                {
                    break;
                }
                if (curve.Length == 1)
                {
                    gPath.AddArc(new RectangleF(curve[0], new SizeF(0.5f, 0.5f)), 0.0f, (float)Math.PI);
                }
                else
                {
                    gPath.AddLines(curve);
                }
                gPath.Transform(transform);
                graphics.DrawPath(pen, gPath);
            }
        }

        /// <summary>
        /// update according data when 3D data is updated
        /// </summary>
        private void DataUpdateViewEvent()
        {
            Initialize3DData();
            if (null != UpdateViewEvent)
            {
                UpdateViewEvent();
            }
        }

        /// <summary>
        /// calculate the transform between canvas and 3D geometry objects
        /// </summary>
        private void Calculate3DTransform()
        {
            float previewWidth = m_displayBBox.Width;
            float previewHeight = m_displayBBox.Height;
            RectangleF bbox3D = ToBoundingBox2D(m_data3D.BBox);
            PointF[] plgpts3D = CalculateCanvasRegion(previewWidth, previewHeight, bbox3D);
            m_transform3D = new Matrix(PreTreatBBox(bbox3D), plgpts3D);
        }

        /// <summary>
        /// calculate the transform between canvas and 2D geometry objects
        /// </summary>
        private void Calculate2DTransform()
        {
            float previewWidth = m_displayBBox.Width;
            float previewHeight = m_displayBBox.Height;
            float previewWidth2D = previewWidth * Drawing2DScale2D;
            float previewHeight2D = previewHeight * Drawing2DScale2D;
            PointF[] plgpts2D = CalculateCanvasRegion(previewWidth2D, previewHeight2D, m_data2D.BBox);
            m_transform2D = new Matrix(PreTreatBBox(m_data2D.BBox), plgpts2D);
        }

        /// <summary>
        /// pretreat BBoundingBox so that its height or width will be bigger than zero
        /// </summary>
        /// <param name="bbox"></param>
        /// <returns></returns>
        private RectangleF PreTreatBBox(RectangleF bbox)
        {
            const float EpsilonLen = 0.001f;
            if (bbox.Height < EpsilonLen)
            {
                bbox.Height = 0.001f;
            }
            if (bbox.Width < EpsilonLen)
            {
                bbox.Width = 0.001f;
            }
            return bbox;
        }

        /// <summary>
        /// get the display region, adjust the proportion and location
        /// </summary>
        /// <returns>upper-left, upper-right, and lower-left corners of the rectangle </returns>
        private PointF[] CalculateCanvasRegion(float previewWidth, float previewHeigh, RectangleF bbox)
        {
            // get the area without margin
            float realWidth = previewWidth * (1 - 2 * MarginRatio);
            float realHeight = previewHeigh * (1 - 2 * MarginRatio);
            float minX = previewWidth * MarginRatio;
            float minY = previewHeigh * MarginRatio;
            // ratio of width to height
            float originRate = bbox.Width / bbox.Height;
            float displayRate = realWidth / realHeight;

            if (originRate > displayRate)
            {
                // display area in canvas need move to center in height
                float goalHeight = realWidth / originRate;
                minY = minY + (realHeight - goalHeight) / 2;
                realHeight = goalHeight;
            }
            else
            {
                // display area in canvas need move to center in width
                float goalWidth = realHeight * originRate;
                minX = minX + (realWidth - goalWidth) / 2;
                realWidth = goalWidth;
            }

            PointF[] plgpts = new PointF[3];
            plgpts[0] = new PointF(minX, realHeight + minY);                // upper-left point
            plgpts[1] = new PointF(realWidth + minX, realHeight + minY);    // upper-right point
            plgpts[2] = new PointF(minX, minY);                             // lower-left point

            return plgpts;
        }

        /// <summary>
        /// Get X and Y data of 3D BoundingBox to creat a 2D BoundingBox
        /// </summary>
        /// <param name="bbox3D"></param>
        /// <returns></returns>
        private RectangleF ToBoundingBox2D(BoundingBoxXYZ bbox3D)
        {
            float left = (float)(bbox3D.Min.X);
            float up = (float)(bbox3D.Min.Y);
            float right = (float)(bbox3D.Max.X);
            float down = (float)(bbox3D.Max.Y);
            RectangleF bbox2D = new RectangleF(left, up, (right - left), (down - up));
            return bbox2D;
        }
    }
}
