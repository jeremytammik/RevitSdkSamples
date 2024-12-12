//
// (C) Copyright 2003-2019 by Autodesk, Inc.
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

namespace Revit.SDK.Samples.ShaftHolePuncher.CS
{
    /// <summary>
    /// tool used to draw line
    /// </summary>
    public class RectangleTool : ITool
    {
        /// <summary>
        /// draw a line from end point of tool to the location where mouse move
        /// </summary>
        /// <param name="graphic">graphic object,used to draw geometry</param>
        /// <param name="e">mouse event args</param>
        public override void OnMouseMove(System.Drawing.Graphics graphic, 
            System.Windows.Forms.MouseEventArgs e)
        {
            if (1 == m_points.Count)
            {
                DrawRect(graphic, m_backGroundPen, m_points[0], m_preMovePoint);
                m_preMovePoint = e.Location;
                DrawRect(graphic, m_foreGroundPen, m_points[0], m_preMovePoint);
            }
        }

        /// <summary>
        /// record the location point where mouse clicked
        /// </summary>
        /// <param name="e">mouse event args</param>
        public override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            if (MouseButtons.Left == e.Button && !m_finished 
                && GetDistance(m_preDownPoint, e.Location) > 2)
            {
                m_preDownPoint = e.Location;
                m_points.Add(e.Location);
                if (2 == m_points.Count)
                {
                    m_finished = true;
                }
            }
        }

        /// <summary>
        /// draw a rectangle
        /// </summary>
        /// <param name="graphic">Graphics object, use to draw geometry</param>
        public override void Draw(Graphics graphic)
        {
            if (2 == m_points.Count)
            {
                DrawRect(graphic, m_foreGroundPen, m_points[0], m_points[1]);
            }
        }

        /// <summary>
        /// draw rectangle use the given two points p1 and p2
        /// </summary>
        /// <param name="graphic">Graphics object,used to draw geometry</param>
        /// <param name="pen">Pen used to set color</param>
        /// <param name="p1">rectangle one corner</param>
        /// <param name="p2">opposite corner of p1</param>
        private void DrawRect(Graphics graphic, Pen pen, Point p1, Point p2)
        {
            Point[] points = new Point[5] { p1, new Point(p1.X, p2.Y), 
                p2, new Point(p2.X, p1.Y), p1 };
            graphic.DrawLines(pen, points);
        }
    }
}
