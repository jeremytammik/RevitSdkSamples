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
    /// Tool used to draw arc.
    /// </summary>
    class ArcTool : ITool
    {
        private bool m_isFinished = false;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ArcTool()
        {
            m_type = ToolType.Arc;
        }

        /// <summary>
        /// Draw Arcs
        /// </summary>
        /// <param name="graphic">Graphics object</param>
        public override void Draw(System.Drawing.Graphics graphic)
        {
            foreach (List<Point> line in m_lines)
            {
                int count = line.Count;
                if (count == 3)
                {
                    DrawArc(graphic, m_foreGroundPen, line[0], line[1], line[3]);
                }
                else if (count > 3)
                {
                    DrawArc(graphic, m_foreGroundPen, line[0], line[1], line[2]);
                    for (int i = 1; i < count - 3; i += 2)
                    {
                        DrawArc(graphic, m_foreGroundPen, line[i], line[i + 2], line[i + 3]);
                    }
                }
            }
        }

        /// <summary>
        /// Mouse down event handler
        /// </summary>
        /// <param name="graphic">Graphics object, used to draw geometry</param>
        /// <param name="e">Mouse event argument</param>
        public override void OnMouseDown(Graphics graphic, MouseEventArgs e)
        {
            if (MouseButtons.Left == e.Button)
            {
                m_points.Add(e.Location);
                m_preMovePoint = e.Location;

                if (m_points.Count >= 4 && m_points.Count % 2 == 0)
                {
                    graphic.DrawLine(m_backGroundPen, 
                        m_points[m_points.Count - 3], m_preMovePoint);
                }
                Draw(graphic);

                if (m_isFinished)
                {
                    m_isFinished = false;
                    List<Point> line = new List<Point>(m_points);
                    m_lines.Add(line);
                    m_points.Clear();
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
            if (2 == m_points.Count)
            {
                DrawArc(graphic, m_backGroundPen, m_points[0], m_points[1], m_preMovePoint);
                m_preMovePoint = e.Location;
                DrawArc(graphic, m_foreGroundPen, m_points[0], m_points[1], e.Location);

            }
            else if (m_points.Count > 2 && m_points.Count % 2 == 0)
            {
                DrawArc(graphic, m_backGroundPen, m_points[m_points.Count - 3], 
                    m_points[m_points.Count - 1], m_preMovePoint);
                m_preMovePoint = e.Location;
                DrawArc(graphic, m_foreGroundPen, m_points[m_points.Count - 3], 
                    m_points[m_points.Count - 1], e.Location);
            }
            else if (!m_isFinished && m_points.Count > 2 && m_points.Count % 2 == 1)
            {
                graphic.DrawLine(m_backGroundPen, m_points[m_points.Count - 2], m_preMovePoint);
                m_preMovePoint = e.Location;
                graphic.DrawLine(m_foreGroundPen, m_points[m_points.Count - 2], e.Location);
            }
        }

        /// <summary>
        /// Mouse right key click
        /// </summary>
        /// <param name="graphic">Graphics object, used to draw geometry</param>
        /// <param name="e">Mouse event argument</param>
        public override void OnRightMouseClick(Graphics graphic, MouseEventArgs e)
        {
            if (!m_isFinished && e.Button == MouseButtons.Right && m_points.Count > 0)
            {
                m_isFinished = true;
                m_points.Add(m_points[0]);
                graphic.DrawLine(m_backGroundPen, m_points[m_points.Count - 3], e.Location);
            }
        }

        /// <summary>
        /// Mouse middle key down event handler
        /// </summary>
        /// <param name="graphic">Graphics object, used to draw geometry</param>
        /// <param name="e">Mouse event argument</param>
        public override void OnMidMouseDown(Graphics graphic, MouseEventArgs e)
        {
            base.OnMidMouseDown(graphic, e);
            if (m_isFinished)
            {
                m_isFinished = false;
            }
        }

        /// <summary>
        /// Calculate the arc center
        /// </summary>
        /// <param name="p1">Point on arc</param>
        /// <param name="p2">Point on arc</param>
        /// <param name="p3">Point on arc</param>
        /// <returns></returns>
        private PointF ComputeCenter(PointF p1, PointF p2, PointF p3)
        {
            float deta = 4 * (p2.X - p1.X) * (p3.Y - p1.Y) - 4 * (p2.Y - p1.Y) * (p3.X - p1.X);

            if (deta == 0)
            {
                throw new Exception("Divided by Zero!");
            }
            float constD1 = p2.X * p2.X + p2.Y * p2.Y - (p1.X * p1.X + p1.Y * p1.Y);
            float constD2 = p3.X * p3.X + p3.Y * p3.Y - (p1.X * p1.X + p1.Y * p1.Y);

            float centerX = (constD1 * 2 * (p3.Y - p1.Y) - constD2 * 2 * (p2.Y - p1.Y)) / deta;
            float centerY = (constD2 * 2 * (p2.X - p1.X) - constD1 * 2 * (p3.X - p1.X)) / deta;

            return new PointF(centerX, centerY);
        }

        /// <summary>
        /// Draw arc
        /// </summary>
        /// <param name="graphic">Graphics object, used to draw geometry</param>
        /// <param name="pen">Used to set drawing color</param>
        /// <param name="p1">Point on arc</param>
        /// <param name="p2">Point on arc</param>
        /// <param name="p3">Point on arc</param>
        private void DrawArc(Graphics graphic, Pen pen, PointF p1, PointF p2, PointF p3)
        {
            try
            {
                PointF pCenter = ComputeCenter(p1, p2, p3);
                
                //computer the arc rectangle
                float radius = (float)Math.Sqrt((p1.X - pCenter.X) * (p1.X - pCenter.X)
                    + (p1.Y - pCenter.Y) * (p1.Y - pCenter.Y));
                SizeF size = new SizeF(radius, radius);
                PointF upLeft = pCenter - size;
                SizeF sizeRect = new SizeF(2 * radius, 2 * radius);
                RectangleF rectF = new RectangleF(upLeft, sizeRect);

                double startCos = (p1.X - pCenter.X) / radius;
                double startSin = (p1.Y - pCenter.Y) / radius;

                double endCos = (p2.X - pCenter.X) / radius;
                double endSin = (p2.Y - pCenter.Y) / radius;

                double midCos = (p3.X - pCenter.X) / radius;
                double midSin = (p3.Y - pCenter.Y) / radius;

                double startAngle = 0, endAngle = 0, midAngle = 0;

                //computer the angle between [0, 360]
                startAngle = GetAngle(startSin, startCos);
                endAngle   = GetAngle(endSin, endCos);
                midAngle   = GetAngle(midSin, midCos);

                //get the min angle and sweep angle
                double minAngle = Math.Min(startAngle, endAngle);
                double maxAngle = Math.Max(startAngle, endAngle);
                double sweepAngle = Math.Abs(endAngle - startAngle);
                if (midAngle < minAngle || midAngle > maxAngle)
                {
                    minAngle = maxAngle;
                    sweepAngle = 360 - sweepAngle;
                }
                graphic.DrawArc(pen, rectF, (float)minAngle, (float)sweepAngle);
            }
            //catch divided by zero exception
            catch (Exception)
            {
                return;
            }
        }

        /// <summary>
        /// Get angle between [0,360]
        /// </summary>
        /// <param name="sin">Sin(Angle) value</param>
        /// <param name="cos">Cos(Angle) value</param>
        /// <returns></returns>
        private double GetAngle(double sin, double cos)
        {
            double result = 0;
            if (sin > 0)
            {
                result = (180 / Math.PI) * Math.Acos(cos);
            }
            else if (cos < 0)
            {
                result = 180 + (180 / Math.PI) * Math.Acos(Math.Abs(cos));
            }
            else if (cos > 0)
            {
                result = 360 - (180 / Math.PI) * Math.Acos(Math.Abs(cos));
            }
            return result;
        }
    }
}
