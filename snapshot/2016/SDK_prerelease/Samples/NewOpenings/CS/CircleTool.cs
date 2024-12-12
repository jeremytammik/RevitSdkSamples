//
// (C) Copyright 2003-2014 by Autodesk, Inc.
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
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace Revit.SDK.Samples.NewOpenings.CS
{
    /// <summary>
    /// Tool used to draw circle
    /// </summary>
    class CircleTool:ITool
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public CircleTool()
        {
            m_type = ToolType.Circle;
        }

        /// <summary>
        /// Draw circles contained in the tool
        /// </summary>
        /// <param name="graphic"></param>
        public override void Draw(Graphics graphic)
        {
            foreach (List<Point> line in m_lines)
            {
                DrawCircle(graphic, m_foreGroundPen, line[0], line[1]);
            }            
        }

        /// <summary>
        /// Mouse down event handler
        /// </summary>
        /// <param name="graphic">Graphics object, used to draw geometry</param>
        /// <param name="e">Mouse event argument</param>
        public override void OnMouseDown(Graphics graphic, MouseEventArgs e)
        {
            base.OnMouseDown(graphic, e);
            if (MouseButtons.Left == e.Button)
            {
                m_preMovePoint = e.Location;
                m_points.Add(e.Location);

                if (2 == m_points.Count)
                {
                    DrawCircle(graphic, m_foreGroundPen, m_points[0], m_points[1]);
                }
            }
        }

        /// <summary>
        /// Mouse move event handler
        /// </summary>
        /// <param name="graphic">Graphics object, used to draw geometry</param>
        /// <param name="e">Mouse event argument</param>
        public override void OnMouseMove(Graphics graphic, MouseEventArgs e)
        {
            base.OnMouseMove(graphic, e);
            
            if (1 == m_points.Count)
            {
                DrawCircle(graphic, m_backGroundPen, m_points[0], m_preMovePoint);
                m_preMovePoint = e.Location;
                DrawCircle(graphic, m_foreGroundPen, m_points[0], e.Location);
            }
        }

        /// <summary>
        /// Mouse up event handler
        /// </summary>
        /// <param name="graphic">Graphics object, used to draw geometry</param>
        /// <param name="e">Mouse event argument</param>
        public override void OnMouseUp(Graphics graphic, MouseEventArgs e)
        {
            base.OnMouseUp(graphic, e);

            if (2 == m_points.Count)
            {
                List<Point> line = new List<Point>(m_points);
                m_lines.Add(line);
                m_points.Clear();
            }
        }

        /// <summary>
        /// Draw circle with center and one point on circle
        /// </summary>
        /// <param name="graphics">Graphics object, used  to draw geometry</param>
        /// <param name="pen">Pen used to set drawing color</param>
        /// <param name="pCenter">Circle center</param>
        /// <param name="pBound">One point on circle</param>
        private void DrawCircle(Graphics graphics,Pen pen, Point pCenter, Point pBound)
        {
            int radius = (int)Math.Sqrt((pBound.X - pCenter.X) * (pBound.X - pCenter.X)
                + (pBound.Y - pCenter.Y) * (pBound.Y - pCenter.Y));
            Size radiusSize = new Size(radius, radius);
            Point uperLeft = pCenter - radiusSize;
            graphics.DrawEllipse(pen, uperLeft.X, uperLeft.Y, 2 * radius, 2 * radius);
        }        
    }
}
