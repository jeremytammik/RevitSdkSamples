//
// (C) Copyright 2003-2023 by Autodesk, Inc.
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
using System.Collections;

namespace Revit.SDK.Samples.SlabShapeEditing.CS
{
    /// <summary>
    /// tool used to draw line
    /// </summary>
    class LineTool
    {
        #region class member variables
        ArrayList m_Points; //record all the points draw by this tool
        PointF m_movePoint; //record the coordinate of location where mouse just moved to. 
        #endregion 

        /// <summary>
        /// Get all the points of this tool
        /// </summary>
        public ArrayList Points
        {
            get
            {
                return m_Points;
            }
            set
            {
                m_Points = value;
            }
        }

        /// <summary>
        ///Get coordinate of location where mouse just moved to.
        /// </summary>
        public PointF MovePoint
        {
            get
            {
                return m_movePoint;
            }
            set
            {
                m_movePoint = value;
            }
        }

        /// <summary>
        /// default constructor
        /// </summary>
        public LineTool() 
        {
            m_Points = new ArrayList();
            m_movePoint = Point.Empty;
        }

        /// <summary>
        /// draw the stored lines
        /// </summary>
        /// <param name="graphics">Graphics object, used to draw geometry</param>
        /// <param name="pen">Pen which used to draw lines</param>
        public void Draw2D(Graphics graphics, Pen pen)
        {
            for (int i = 0; i < m_Points.Count - 1; i+=2)
            {
                graphics.DrawLine(pen, (PointF)m_Points[i], (PointF)m_Points[i+1]);
            }

            //draw the moving point
            if (!m_movePoint.IsEmpty)
            {
                if (m_Points.Count >= 1)
                {
                    graphics.DrawLine(pen, (PointF)m_Points[m_Points.Count - 1], m_movePoint);
                }
            }
        }

        /// <summary>
        /// draw rectangle with specific graphics and pen
        /// </summary>
        /// <param name="graphics">Graphics object, used to draw geometry</param>
        /// <param name="pen">Pen which used to draw lines</param>
        public void DrawRectangle(Graphics graphics, Pen pen)
        {
            for (int i = 0; i < m_Points.Count - 1; i += 2)
            {
                PointF pointF = (PointF)m_Points[i];
                graphics.DrawRectangle(pen, pointF.X-2, pointF.Y-2, 4, 4);
            }
        }
    }
}
