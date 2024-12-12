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


namespace Revit.SDK.Samples.Openings.CS
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Drawing;

    /// <summary>
    /// represent a geometry segment line
    /// </summary>
    public class Line2D
    {
        private PointF m_startPnt = new PointF();            // start point
        private PointF m_endPnt = new PointF();                // end point
        private float m_length;                                // length of the line
        // normal of the line; start point to end point
        private PointF m_normal = new PointF();                
        private RectangleF m_boundingBox = new RectangleF();// rectangle box contains the line

        /// <summary>
        /// rectangle box contains the line
        /// </summary>
        public RectangleF BoundingBox
        {
            get 
            {
                return m_boundingBox; 
            }
        }

        /// <summary>
        /// start point of the line; if it is set to new value,
        /// EndPoint is changeless; Length, Normal and BoundingBox will updated
        /// </summary>
        public PointF StartPnt
        {
            get
            {
                return m_startPnt;
            }
            set
            {
                if (m_startPnt == value)
                {
                    return;
                }
                m_startPnt = value;
                CalculateDirection();
                CalculateBoundingBox();
            }
        }

        /// <summary>
        /// end point of the line; if it is set to new value,
        /// StartPoint is changeless; Length, Normal and BoundingBox will updated
        /// </summary>
        public PointF EndPnt
        {
            get
            {
                return m_endPnt;
            }
            set
            {
                if (m_endPnt == value)
                {
                    return;
                }
                m_endPnt = value;
                CalculateDirection();
                CalculateBoundingBox();
            }
        }

        /// <summary>
        /// Length of the line; if it is set to new value,
        /// StartPoint and Normal is changeless; EndPoint and BoundingBox will updated
        /// </summary>
        public float Length
        {
            get
            {
                return m_length;

            }
            set
            {
                if (m_length == value)
                {
                    return;
                }
                m_length = value;
                CalculateEndPoint();
                CalculateBoundingBox();
            }
        }

        /// <summary>
        /// Normal of the line; if it is set to new value,
        /// StartPoint is changeless; EndPoint and BoundingBox will updated
        /// </summary>
        public PointF Normal
        {
            get
            {
                return m_normal;

            }
            set
            {
                if (m_normal == value)
                {
                    return;
                }
                m_normal = value;
                CalculateEndPoint();
                CalculateBoundingBox();
            }
        }

        /// <summary>
        /// constructor
        /// default StartPoint = (0.0, 0.0), EndPoint = (1.0, 0.0)
        /// </summary>
        public Line2D()
        {
            m_startPnt.X = 0.0f;
            m_startPnt.Y = 0.0f;
            m_endPnt.X = 1.0f;
            m_endPnt.Y = 0.0f;
            CalculateDirection();
            CalculateBoundingBox();
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="startPnt">StartPoint</param>
        /// <param name="endPnt">EndPoint</param>
        public Line2D(PointF startPnt, PointF endPnt)
        {
            m_startPnt = startPnt;
            m_endPnt = endPnt;
            CalculateDirection();
            CalculateBoundingBox();
        }

        /// <summary>
        /// calculate BoundingBox according to StartPoint and EndPoint
        /// </summary>
        private void CalculateBoundingBox()
        {
            float x1 = m_endPnt.X;
            float x2 = m_startPnt.X;
            float y1 = m_endPnt.Y;
            float y2 = m_startPnt.Y;

            float width = Math.Abs(x1 - x2);
            float height = Math.Abs(y1 - y2);

            if (x1 > x2)
            {
                x1 = x2;
            }
            if (y1 > y2)
            {
                y1 = y2;
            }
            m_boundingBox = new RectangleF(x1, y1, width, height);
        }

        /// <summary>
        /// calculate length by StartPoint and EndPoint
        /// </summary>
        private void CalculateLength()
        {
            m_length =
                (float)Math.Sqrt(Math.Pow((m_startPnt.X - m_endPnt.X), 2) + 
                Math.Pow((m_startPnt.Y - m_endPnt.Y), 2));

        }

        /// <summary>
        /// calculate Direction by StartPoint and EndPoint
        /// </summary>
        private void CalculateDirection()
        {
            CalculateLength();
            m_normal.X = (m_endPnt.X - m_startPnt.X) / m_length;
            m_normal.Y = (m_endPnt.Y - m_startPnt.Y) / m_length;
        }

        /// <summary>
        /// calculate EndPoint by StartPoint, Length and Direction
        /// </summary>
        private void CalculateEndPoint()
        {
            m_endPnt.X = m_startPnt.X + m_length * m_normal.X;
            m_endPnt.Y = m_startPnt.Y + m_length * m_normal.Y;
        }
    }
}
