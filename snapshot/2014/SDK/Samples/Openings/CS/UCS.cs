//
// (C) Copyright 2003-2013 by Autodesk, Inc.
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
    /// This class stand for user coordinate system
    /// </summary>
    public class UCS
    {
        Vector m_origin = new Vector(0.0, 0.0, 0.0);
        Vector m_xAxis = new Vector(1.0, 0.0, 0.0);
        Vector m_yAxis = new Vector(0.0, 1.0, 0.0);
        Vector m_zAxis = new Vector(0.0, 0.0, 1.0);

        /// <summary>
        /// Property to get origin of user coordinate system
        /// </summary>
        public Vector Origin
        {
            get
            {
                return m_origin;
            }
        }

        /// <summary>
        /// Property to get X Axis of user coordinate system
        /// </summary>
        public Vector XAxis
        {
            get
            {
                return m_xAxis;
            }
        }

        /// <summary>
        /// Property to get Y Axis of user coordinate system
        /// </summary>
        public Vector YAxis
        {
            get
            {
                return m_yAxis;
            }
        }

        /// <summary>
        /// Property to get Z Axis of user coordinate system
        /// </summary>
        public Vector ZAxis
        {
            get
            {
                return m_zAxis;
            }
        }

        /// <summary>
        /// The default constructor, 
        /// </summary>
        public UCS(Vector origin, Vector xAxis, Vector yAxis)
            : this(origin, xAxis, yAxis, true)
        {
        }

        /// <summary>
        /// constructor, 
        /// get a user coordinate system
        /// </summary>
        /// <param name="origin">origin of user coordinate system</param>
        /// <param name="xAxis">xAxis of user coordinate system</param>
        /// <param name="yAxis">yAxis of user coordinate system</param>
        /// <param name="flag">select left handness or right handness</param>
        public UCS(Vector origin, Vector xAxis, Vector yAxis, bool flag)
        {
            Vector x2 = xAxis / ~xAxis;
            Vector y2 = yAxis / ~yAxis;
            Vector z2 = x2 & y2;
            if (~z2 < double.Epsilon)
            {
                throw new InvalidOperationException();
            }

            if (!flag)
            {
                z2 = -z2;
            }

            m_origin = origin;
            m_xAxis = x2;
            m_yAxis = y2;
            m_zAxis = z2;
        }

        /// <summary>
        /// Transform local coordinate to global coordinate
        /// </summary>
        /// <param name="arg">a vector which need to transform</param>
        public Vector LC2GC(Vector arg)
        {
            Vector result = new Vector();
            result.X =
                arg.X * m_xAxis.X + arg.Y * m_yAxis.X + arg.Z * m_zAxis.X + m_origin.X;
            result.Y =
                arg.X * m_xAxis.Y + arg.Y * m_yAxis.Y + arg.Z * m_zAxis.Y + m_origin.Y;
            result.Z =
                arg.X * m_xAxis.Z + arg.Y * m_yAxis.Z + arg.Z * m_zAxis.Z + m_origin.Z;
            return result;
        }

        /// <summary>
        /// Transform global coordinate to local coordinate
        /// </summary>
        /// <param name="line">a line which need to transform</param>
        public Line3D GC2LC(Line3D line)
        {
            Vector startPnt = GC2LC(line.StartPoint);
            Vector endPnt = GC2LC(line.EndPoint);
            return new Line3D(startPnt, endPnt);
        }

        /// <summary>
        /// Transform global coordinate to local coordinate
        /// </summary>
        /// <param name="arg">a vector which need to transform</param>
        public Vector GC2LC(Vector arg)
        {
            Vector result = new Vector();
            arg = arg - m_origin;
            result.X = m_xAxis * arg;
            result.Y = m_yAxis * arg;
            result.Z = m_zAxis * arg;
            return result;
        }
    }
}
