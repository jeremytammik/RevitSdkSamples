//
// (C) Copyright 2003-2007 by Autodesk, Inc.
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
    public class LineTool : ITool
    {
        /// <summary>
        /// draw a line from end point of tool to the location where mouse moved
        /// </summary>
        /// <param name="graphic">graphic object,used to draw geometry</param>
        /// <param name="e">mouse event args</param>
        public override void OnMouseMove(System.Drawing.Graphics graphic, 
            System.Windows.Forms.MouseEventArgs e)
        {
            if(m_points.Count != 0 && !m_finished)
            {                
                graphic.DrawLine(m_backGroundPen, m_points[m_points.Count - 1], m_preMovePoint);
                m_preMovePoint = e.Location;
                graphic.DrawLine(m_foreGroundPen, m_points[m_points.Count - 1], e.Location);
            }
        }

        /// <summary>
        /// record the location point where mouse clicked
        /// </summary>
        /// <param name="e">mouse event args</param>
        public override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            //when user click right button of mouse,
            //finish the curve if the number of points is more than 2
            if (MouseButtons.Right == e.Button && m_points.Count > 2)
            {
                m_finished = true;
            }

            if (MouseButtons.Left == e.Button && !m_finished
                && GetDistance(m_preDownPoint, e.Location) > 2)
            {
                m_preDownPoint = e.Location;
                m_points.Add(e.Location);
            }
        }

        /// <summary>
        /// draw lines recorded in the tool
        /// </summary>
        /// <param name="graphic">Graphics object, use to draw geometry</param>
        public override void Draw(Graphics graphic)
        {
            for (int i = 0; i < m_points.Count - 1; i++)
            {
                graphic.DrawLine(m_foreGroundPen, m_points[i], m_points[i + 1]);
            }

            //if user finished draw (clicked the right button), then close the curve
            if (m_finished)
            {
                graphic.DrawLine(m_foreGroundPen, m_points[0], m_points[m_points.Count - 1]);
            }
        }
    }
}
