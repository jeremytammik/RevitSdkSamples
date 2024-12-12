//
// (C) Copyright 2003-2015 by Autodesk, Inc.
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

namespace Revit.SDK.Samples.Openings.CS
{
    /// <summary>
    /// Line class use to store information about line(include startPoint and endPoint)
    /// and get the value via (startPoint, endPoint)property
    /// </summary>
    public class Line3D
    {
        Vector m_startPnt; //start point
        Vector m_endPnt; //end point
        Vector m_normal; //normal
        double m_length; //length of line

        //property
        /// <summary>
        /// Property to get and set length of line
        /// </summary>
        public double Length
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
            }
        }

        /// <summary>
        /// Property to get and set Start Point of line
        /// </summary>
        public Vector StartPoint
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
            }
        }

        /// <summary>
        /// Property to get and set End Point of line
        /// </summary>
        public Vector EndPoint
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
            }
        }

        /// <summary>
        /// Property to get and set Normal of line
        /// </summary>
        public Vector Normal
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
            }
        }

        /// <summary>
        /// The default constructor
        /// </summary>
        public Line3D()
        {
            m_startPnt = new Vector(0.0, 0.0, 0.0);
            m_endPnt = new Vector(1.0, 0.0, 0.0);
            m_length = 1.0;
            m_normal = new Vector(1.0, 0.0, 0.0);
        }

        /// <summary>
        /// The default constructor
        /// </summary>
        /// <param name="startPnt">start point of line</param>
        /// <param name="endPnt">enn point of line</param>
        public Line3D(Vector startPnt, Vector endPnt)
        {
            m_startPnt = startPnt;
            m_endPnt = endPnt;
            CalculateDirection();
        }

        /// <summary>
        /// calculate length by StartPoint and EndPoint
        /// </summary>
        private void CalculateLength()
        {
            m_length = ~(m_startPnt - m_endPnt); 
        }

        /// <summary>
        /// calculate Direction by StartPoint and EndPoint
        /// </summary>
        private void CalculateDirection()
        {
            CalculateLength();
            m_normal = (m_endPnt - m_startPnt) / m_length;
        }

        /// <summary>
        /// calculate EndPoint by StartPoint, Length and Direction
        /// </summary>
        private void CalculateEndPoint()
        {
            m_endPnt = m_startPnt + m_normal * m_length;
        }
    }
}
