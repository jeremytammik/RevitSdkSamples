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

namespace Revit.SDK.Samples.NewOpenings.CS
{
    /// <summary>
    /// Tool used to draw line
    /// </summary>
    public class LineTool:ITool
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public LineTool()
        {
            m_type = ToolType.Line;
        }

        /// <summary>
        /// Mouse move event handle
        /// </summary>
        /// <param name="graphic">Graphics object, used to draw geometry</param>
        /// <param name="e">Mouse event argument</param>
        public override void OnMouseMove(System.Drawing.Graphics graphic, MouseEventArgs e)
        {
            if(m_points.Count != 0)
            {                
                graphic.DrawLine(m_backGroundPen, m_points[m_points.Count - 1], m_preMovePoint);
                m_preMovePoint = e.Location;
                graphic.DrawLine(m_foreGroundPen, m_points[m_points.Count - 1], e.Location);
            }
        }

        /// <summary>
        /// Mouse down event handler
        /// </summary>
        /// <param name="graphic">Graphics object, used to draw geometry</param>
        /// <param name="e">Mouse event argument</param>
        public override void OnMouseDown(System.Drawing.Graphics graphic, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                m_preMovePoint = e.Location;
                m_points.Add(e.Location);
          
                if (m_points.Count >= 2)
                {
                    graphic.DrawLine(m_foreGroundPen, m_points[m_points.Count - 2], 
                        m_points[m_points.Count - 1]);
                } 
            }
        }

        /// <summary>
        /// Right mouse click handler
        /// </summary>
        /// <param name="graphic">Graphics object, used to drawing geometry</param>
        /// <param name="e">Mouse event argument</param>
        public override void OnRightMouseClick(Graphics graphic, MouseEventArgs e)
        {
            if(MouseButtons.Right == e.Button && m_points.Count > 2)
            {
                List<Point> line = new List<Point>(m_points);
                m_lines.Add(line);
                
                graphic.DrawLine(m_foreGroundPen, m_points[m_points.Count - 1], m_points[0]);
                graphic.DrawLine(m_backGroundPen, m_points[m_points.Count - 1], m_preMovePoint);
                m_points.Clear();
            }
        }

        /// <summary>
        /// Draw lines
        /// </summary>
        /// <param name="graphic">Graphics object, used to draw geometry</param>
        public override void Draw(Graphics graphic)
        {
            foreach(List<Point> line in m_lines)
            {
                for (int i = 0; i < line.Count - 1;i++ )
                {
                    graphic.DrawLine(m_foreGroundPen, line[i], line[i + 1]);
                }                
                //close the line
                graphic.DrawLine(m_foreGroundPen, line[line.Count - 1], line[0]);
            }
        }
    }
}
