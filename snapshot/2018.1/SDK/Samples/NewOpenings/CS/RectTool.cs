//
// (C) Copyright 2003-2017 by Autodesk, Inc.
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
    /// Tool used to draw rectangle
    /// </summary>
    class RectTool:ITool
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public RectTool()
        {
            m_type = ToolType.Rectangle;
        }

        /// <summary>
        /// Mouse move event handler
        /// </summary>
        /// <param name="graphic">Graphics object,used to draw geometry</param>
        /// <param name="e">Mouse event argument</param>
        public override void OnMouseMove(Graphics graphic, MouseEventArgs e)
        {
            if(m_points.Count == 1)
            {
                DrawRect(graphic, m_backGroundPen, m_points[0], m_preMovePoint);
                m_preMovePoint = e.Location;
                DrawRect(graphic, m_foreGroundPen, m_points[0], m_preMovePoint);
            }
        }

        /// <summary>
        /// Mouse down event handler
        /// </summary>
        /// <param name="graphic">Graphics object,used to draw geometry</param>
        /// <param name="e">Mouse event argument</param>
        public override void OnMouseDown(Graphics graphic, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                m_preMovePoint = e.Location;
                m_points.Add(e.Location);

                if(m_points.Count == 2)
                {
                    DrawRect(graphic, m_foreGroundPen, m_points[0], m_points[1]);
                }
            };
        }

        /// <summary>
        /// Mouse up event handler
        /// </summary>
        /// <param name="graphic">Graphics object,used to draw geometry</param>
        /// <param name="e">Mouse event argument</param>
        public override void OnMouseUp(Graphics graphic, MouseEventArgs e)
        {
            if(m_points.Count == 2  )
            {
                List<Point> line = new List<Point>(m_points);
                m_lines.Add(line);
                m_points.Clear();
            }
        }

        /// <summary>
        /// Draw rectangles 
        /// </summary>
        /// <param name="graphic">Graphics object,used to draw geometry </param>
        public override void Draw(Graphics graphic)
        {
            foreach (List<Point> line in m_lines)
            {
                DrawRect(graphic, m_foreGroundPen, line[0], line[1]);
            }
        }

        /// <summary>
        /// Draw rectangle use the given two opposite point p1 and p2
        /// </summary>
        /// <param name="graphic">Graphics object,used to draw geometry</param>
        /// <param name="pen">Pen used to set color</param>
        /// <param name="p1">Rectangle one corner</param>
        /// <param name="p2">Opposite corner of p1</param>
        private void DrawRect(Graphics graphic,Pen pen,Point p1, Point p2)
        {
            Size p = new Size(p2.X - p1.X, p2.Y - p1.Y);
            if(p.Width >= 0 && p.Height >= 0)
            {
                graphic.DrawRectangle(pen, p1.X, p1.Y, p.Width, p.Height);
            }
            //draw four lines
            else
            {
                Point[] points = new Point[5]{p1, new Point(p1.X, p2.Y), 
                    p2, new Point(p2.X, p1.Y), p1};
                graphic.DrawLines(pen, points);
            }
        }
    }
}
