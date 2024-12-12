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
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Revit.SDK.Samples.NewHostedSweep.CS
{
    /// <summary>
    /// This class is intent to display element's wire-frame with C# GDI.
    /// It contains a solid and a bounding box of an element.
    /// It also contains transformation (translation, rotation and scale) to
    /// transform the geometry edges.
    /// </summary>
    public class ElementGeometry
    {
        /// <summary>
        /// Element's Solid
        /// </summary>
        private Solid m_solid;

        /// <summary>
        /// Solid bounding box minimal corner.
        /// </summary>
        private XYZ m_bBoxMin;

        /// <summary>
        /// Solid bounding box maximal corner.
        /// </summary>
        private XYZ m_bBoxMax;

        /// <summary>
        /// Translation transform, it is intent to translate the solid.
        /// It is actually the center of Bounding box.
        /// </summary>
        private XYZ m_translation;

        /// <summary>
        /// Scale transform, it is intent to scale the solid.
        /// </summary>
        private double m_scale;

        /// <summary>
        /// Rotation transform, it is intent to rotate the solid.
        /// </summary>
        private Transform m_rotation;

        /// <summary>
        /// If the solid is transformed (includes translation, scale and rotation)
        /// this flag should be true, otherwise false.
        /// </summary>
        private bool m_isDirty;

        /// <summary>
        /// Solid's Edge to EdgeBinding dictionary. It is intent to store all the edges
        /// of solid.
        /// </summary>
        private Dictionary<Edge, EdgeBinding> m_edgeBindinDic;

        /// <summary>
        /// Translation transform, it is intent to translate the solid.
        /// It is actually the center of Bounding box.
        /// </summary>
        public XYZ Translation
        {
            get { return m_translation; }
            set
            {
                m_isDirty = true;
                m_translation = value;
            }
        }

        /// <summary>
        /// Scale transform, it is intent to scale the solid.
        /// </summary>
        public double Scale
        {
            get { return m_scale; }
            set
            {
                m_isDirty = true;
                m_scale = value;
            }
        }

        /// <summary>
        /// Rotation transform, it is intent to rotate the solid.
        /// </summary>
        public Transform Rotation
        {
            get { return m_rotation; }
            set
            {
                m_isDirty = true;
                m_rotation = value;
            }
        }

        /// <summary>
        /// Element's Solid
        /// </summary>
        public Solid Solid
        {
            get { return m_solid; }
        }

        /// <summary>
        /// Solid's Edge to EdgeBinding dictionary. It is intent to store all the edges
        /// of solid.
        /// </summary>
        public Dictionary<Edge, EdgeBinding> EdgeBindingDic
        {
            get { return m_edgeBindinDic; }
        }

        /// <summary>
        /// Constructor, Construct a new object with an element's geometry Solid,
        /// and its corresponding bounding box.
        /// </summary>
        /// <param name="solid">Element's geometry Solid</param>
        /// <param name="box">Element's geometry bounding box</param>
        public ElementGeometry(Solid solid, BoundingBoxXYZ box)
        {
            m_solid = solid;
            m_bBoxMin = box.Min;
            m_bBoxMax = box.Max;
            m_isDirty = true;

            // Initialize edge binding
            m_edgeBindinDic = new Dictionary<Edge, EdgeBinding>();
            foreach (Edge edge in m_solid.Edges)
            {
                EdgeBinding edgeBingding = new EdgeBinding(edge);
                m_edgeBindinDic.Add(edge, edgeBingding);
            }
        }

        /// <summary>
        /// Initialize the transform (includes translation, scale, and rotation).
        /// </summary>
        /// <param name="width">Width of the view</param>
        /// <param name="height">Height of the view</param>
        public void InitializeTransform(double width, double height)
        {
            // Initialize translation and rotation transform
            XYZ bBoxCenter = (m_bBoxMax + m_bBoxMin) / 2.0;
            m_translation = -bBoxCenter;
            m_rotation = Transform.Identity;
            
            // Initialize scale factor
            double bBoxWidth = m_bBoxMax.X - m_bBoxMin.X;
            double bBoxHeight = m_bBoxMax.Y - m_bBoxMin.Y;
            double widthScale = width / bBoxWidth;
            double heigthScale = height / bBoxHeight;
            m_scale = Math.Min(widthScale, heigthScale);

            // Set dirty flag
            m_isDirty = true;
        }

        /// <summary>
        /// Reset all the edges' status to their original status.
        /// </summary>
        public void ResetEdgeStates()
        {
            foreach (KeyValuePair<Edge, EdgeBinding> pair in m_edgeBindinDic)
            {
                pair.Value.Reset();
            }
        }

        /// <summary>
        /// Update all the edges' transform (include translation, scale, and rotation),
        /// reconstruct the edge's geometry info.
        /// </summary>
        private void Update()
        {
            if (!m_isDirty) return;

            foreach (KeyValuePair<Edge, EdgeBinding> pair in m_edgeBindinDic)
            {
                pair.Value.Update(m_rotation, m_translation, m_scale);
            }
            m_isDirty = false;
        }

        /// <summary>
        /// Draw all the edges of solid in Graphics.
        /// </summary>
        /// <param name="g">Graphics, edges will be draw in it</param>
        public void Draw(Graphics g)
        {
            Update();
            foreach (KeyValuePair<Edge, EdgeBinding> pair in m_edgeBindinDic)
            {
                pair.Value.Draw(g);
            }
        }
    }

    /// <summary>
    /// Binds an edge with some properties which contains its geometry information 
    /// and indicates whether the edge is selected or highlighted.
    /// </summary>
    public class EdgeBinding : IDisposable
    {
        /// <summary>
        /// Edge points in world coordinate system of Revit.
        /// </summary>
        private IList<XYZ> m_points;

        /// <summary>
        /// Edge geometry presentation in C# GDI.
        /// </summary>
        private GraphicsPath m_gdiEdge;

        /// <summary>
        /// Edge bounding Region used to hit testing.
        /// </summary>
        private Region m_region;

        /// <summary>
        /// Pen for edge display.
        /// </summary>
        private Pen m_pen;

        /// <summary>
        /// A flag to indicate the edge is highlighted or not.
        /// </summary>
        private bool m_isHighLighted;

        /// <summary>
        /// A flag to indicate the edge is selected or not.
        /// </summary>
        private bool m_isSelected;

        /// <summary>
        /// Gets whether the edge is highlighted or not.
        /// </summary>
        public bool IsHighLighted
        {
            get { return m_isHighLighted; }
            set { m_isHighLighted = value; }
        }

        /// <summary>
        /// Gets whether the edge is selected or not.
        /// </summary>
        public bool IsSelected
        {
            get { return m_isSelected; }
            set { m_isSelected = value; }
        }

        /// <summary>
        /// Constructor takes Edge as parameter.
        /// </summary>
        /// <param name="edge">Edge</param>
        public EdgeBinding(Edge edge)
        {
            m_points = edge.Tessellate();
            m_pen = new Pen(System.Drawing.Color.White);
            Reset();
        }

        /// <summary>
        /// Reset the status of the edge: un-highlighted, un-selected
        /// </summary>
        public void Reset()
        {
            m_isHighLighted = false;
            m_isSelected = false;
            m_region = null;
        }

        /// <summary>
        /// Update the edge's geometry according to the transformation.
        /// </summary>
        /// <param name="rotation">Rotation transform</param>
        /// <param name="translation">Translation transform</param>
        /// <param name="scale">Scale transform</param>
        public void Update(Transform rotation, XYZ translation, double scale)
        {
            rotation = rotation.Inverse;
            PointF[] points = new PointF[m_points.Count];
            for (int i = 0; i < m_points.Count; i++)
            {
                XYZ tmpPt = m_points[i];
                tmpPt = rotation.OfPoint((tmpPt + translation) * scale);
                points[i] = new PointF((float)tmpPt.X, (float)tmpPt.Y);
            }
            if (m_gdiEdge != null) m_gdiEdge.Dispose();
            m_gdiEdge = new GraphicsPath();
            m_gdiEdge.AddLines(points);

            if (m_region != null) m_region.Dispose();
            m_region = null;
        }

        /// <summary>
        /// Draw the edge in Graphics.
        /// </summary>
        /// <param name="g">Graphics</param>
        public void Draw(Graphics g)
        {
            m_pen.Width = 2.0f;
            if (m_isHighLighted)
            {
                m_pen.Color = System.Drawing.Color.Yellow;                
            }
            else if (m_isSelected)
            {
                m_pen.Color = System.Drawing.Color.Red;
            }
            else
            {
                m_pen.Color = System.Drawing.Color.Green;
            }
            g.DrawPath(m_pen, m_gdiEdge);
        }

        /// <summary>
        /// Return the Edge Region.
        /// </summary>
        /// <returns>Region of the edge</returns>
        private Region GetRegion()
        {
            if (m_region == null)
            {
                GraphicsPath tmpPath = new GraphicsPath();
                tmpPath.AddLines(m_gdiEdge.PathPoints);
                Pen tmpPen = new Pen(System.Drawing.Color.White, 3.0f);
                tmpPath.Widen(tmpPen);
                m_region = new Region(tmpPath);
            }
            return m_region;
        }

        /// <summary>
        /// Test whether or not the edge is under a specified location.
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <returns></returns>
        private bool HitTest(float x, float y)
        {
            return this.GetRegion().IsVisible(x, y);
        }

        /// <summary>
        /// If the edge under the location (x, y), set the highlight flag to true,
        /// otherwise false.
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <returns></returns>
        public bool HighLight(float x, float y)
        {
            m_isHighLighted = HitTest(x, y);
            return m_isHighLighted;
        }

        #region IDisposable Members

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            m_gdiEdge.Dispose();
            m_pen.Dispose();
            m_region.Dispose();
        }

        #endregion
    }
}
