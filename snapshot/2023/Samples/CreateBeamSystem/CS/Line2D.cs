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


namespace Revit.SDK.Samples.CreateBeamSystem.CS
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Drawing;

    using Autodesk.Revit.DB;

    /// <summary>
    /// represent a geometry segment line
    /// </summary>
    public class Line2D
    {
        private PointF m_startPnt = new PointF();            // start point
        private PointF m_endPnt   = new PointF();                // end point
        private float m_length;                                // length of the line
        private PointF m_normal   = new PointF();                // normal of the line; start point to end point
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
            m_endPnt.X   = 1.0f;
            m_endPnt.Y   = 0.0f;
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
            m_endPnt   = endPnt;
            CalculateDirection();
            CalculateBoundingBox();
        }

        /// <summary>
        /// get an interval point on the line of the segment
        /// </summary>
        /// <param name="rate">rate of length from interval to StartPoint 
        /// to length from EndPoint to interval</param>
        /// <returns>interval point</returns>
        public PointF GetIntervalPoint(float rate)
        {
            PointF result = new PointF();
            result.X      = m_startPnt.X + (m_endPnt.X - m_startPnt.X) * rate;
            result.Y      = m_startPnt.Y + (m_endPnt.Y - m_startPnt.Y) * rate;
            return result;
        }

        /// <summary>
        /// scale the segment according to the center of the segment
        /// </summary>
        /// <param name="rate">rate to scale</param>
        public void Scale(float rate)
        {
            PointF startPnt = GetIntervalPoint((1.0f - rate) / 2.0f);
            PointF endPnt   = GetIntervalPoint((1.0f + rate) / 2.0f);
            m_startPnt      = startPnt;
            m_endPnt        = endPnt;
            CalculateLength();
        }

        /// <summary>
        /// parallelly shift the line
        /// </summary>
        /// <param name="distance">distance</param>
        public void Shift(float distance)
        {
            SizeF moveSize = new SizeF(-distance * m_normal.Y, distance * m_normal.X);
            m_startPnt     = m_startPnt + moveSize;
            m_endPnt       = m_endPnt + moveSize;
        }

        /// <summary>
        /// creates an instance of the GeometryLine class that is identical to the current GeometryLine
        /// </summary>
        /// <returns>created GeometryLine</returns>
        public Line2D Clone()
        {
            Line2D cloned = new Line2D(m_startPnt, m_endPnt);
            return cloned;
        }

        /// <summary>
        /// find the number of intersection points for two segments
        /// </summary>
        /// <param name="line0">first line</param>
        /// <param name="line1">second line</param>
        /// <returns>number of intersection points; 0, 1, or 2</returns>
        public static int FindIntersection(Line2D line0, Line2D line1)
        {
            PointF[] intersectPnt = new PointF[2];
            return FindIntersection(line0, line1, ref intersectPnt);
        }

        /// <summary>
        /// find the intersection points of two segments
        /// </summary>
        /// <param name="line0">first line</param>
        /// <param name="line1">second line</param>
        /// <param name="intersectPnt">0, 1 or 2 intersection points</param>
        /// <returns>number of intersection points; 0, 1, or 2</returns>
        public static int FindIntersection(Line2D line0, Line2D line1, ref PointF[] intersectPnt)
        {
            // segments p0 + s * d0 for s in [0, 0], p1 + t * d1 for t in [0, 1]
            PointF p0 = line0.StartPnt;
            PointF d0 = MathUtil.Multiply(line0.Length, line0.Normal);
            PointF p1 = line1.StartPnt;
            PointF d1 = MathUtil.Multiply(line1.Length, line1.Normal);

            PointF E       = MathUtil.Subtract(p1, p0);
            float kross    = d0.X * d1.Y - d0.Y * d1.X;
            float sqrKross = kross * kross;
            float sqrLen0  = d0.X * d0.X + d0.Y * d0.Y;
            float sqrLen1  = d1.X * d1.X + d1.Y * d1.Y;

            // lines of the segments are not parallel
            if (sqrKross > MathUtil.Float_Epsilon * sqrLen0 * sqrLen1)
            {
                float s = (E.X * d1.Y - E.Y * d1.X) / kross;
                if (s < 0 || s > 1)
                {
                    // intersection of lines is not point on segment p0 + s * d0
                    return 0;
                }

                float t = (E.X * d0.Y - E.Y * d0.X) / kross;
                if (t < 0 || t > 1)
                {
                    // intersection of lines is not a point on segment p1 + t * d1
                    return 0;
                }
                // intersection of lines is a point on each segment
                intersectPnt[0] = MathUtil.Add(p0, MathUtil.Multiply(s, d0));
                return 1;
            }
            // lines of the segments are paralled
            float sqrLenE   = E.X * E.X + E.Y * E.Y;
            float kross2    = E.X * d0.Y - E.Y * d0.X;
            float sqrKross2 = kross2 * kross2;
            if (sqrKross2 > MathUtil.Float_Epsilon * sqrLen0 * sqrLenE)
            {
                // lines of the segments are different
                return 0;
            }

            // lines of the segments are the same. need to test for overlap of segments
            float s0   = MathUtil.Dot(d0, E) / sqrLen0;
            float s1   = s0 + MathUtil.Dot(d0, d1) / sqrLen0;
            float smin = MathUtil.GetMin(s0, s1);
            float smax = MathUtil.GetMax(s0, s1);
            float[] w  = new float[2];

            int imax   = MathUtil.FindIntersection(0.0f, 1.0f, smin, smax, ref w);
            for (int i = 0; i < imax; i++)
            {
                intersectPnt[i] = MathUtil.Add(p0, MathUtil.Multiply(w[i], d0));
            }

            return imax;
        }

        /// <summary>
        /// calculate BoundingBox according to StartPoint and EndPoint
        /// </summary>
        private void CalculateBoundingBox()
        {
            float x1     = m_endPnt.X;
            float x2     = m_startPnt.X;
            float y1     = m_endPnt.Y;
            float y2     = m_startPnt.Y;
            float width  = Math.Abs(x1 - x2);
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
                (float)Math.Sqrt(Math.Pow((m_startPnt.X - m_endPnt.X), 2) + Math.Pow((m_startPnt.Y - m_endPnt.Y), 2));

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

        /// <summary>
        /// calculate StartPoint by EndPoint, Length and Direction
        /// </summary>
        private void CalculateStartPoint()
        {
            m_startPnt.X = m_endPnt.X - m_length * m_normal.X;
            m_startPnt.Y = m_endPnt.Y - m_length * m_normal.Y;
        }
    }
}
