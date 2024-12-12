//
// (C) Copyright 2003-2011 by Autodesk, Inc.
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
using System.Collections.ObjectModel;

using Autodesk.Revit.DB;

namespace Revit.SDK.Samples.ObjectViewer.CS
{
    /// <summary>
    /// data class for graphics 2D
    /// </summary>
    public class Graphics2DData
    {
        // curves represent the wireframe of the 2D geometry
        private readonly List<PointF[]> m_curves = new List<PointF[]>();
        // the number of points consists of m_curves
        private readonly int m_numPoints;
        private static EmptyGraphics2DData m_empty = new EmptyGraphics2DData();

        // boundingbox of the 2D geometry
        private RectangleF m_bbox = new RectangleF(0.0f, 0.0f, 1.0f, 1.0f);

        /// <summary>
        /// Represents a new instance of the Graphics2DData class with member data left uninitialized
        /// </summary>
        public static Graphics2DData Empty
        {
            get
            {
                return m_empty;
            }
        }

        /// <summary>
        /// Number of points consists of 2D geometry's wireframe
        /// </summary>
        public int NumPoints
        {
            get
            {
                return m_numPoints;
            }
        }

        /// <summary>
        /// BoundingBox of the 2D geometry
        /// </summary>
        public RectangleF BBox
        {
            get
            {
                return m_bbox;
            }
        }

        /// <summary>
        /// Curves represent the wireframe of the 2D geometry
        /// </summary>
        public List<PointF[]> ConstCurves
        {
            get
            {
                return m_curves;
            }
        }

        /// <summary>
        /// only for Empty class
        /// </summary>
        protected Graphics2DData()
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="curves">curves represent the wireframe of the 2D geometry</param>
        public Graphics2DData(List<List<UV>> curves)
        {
            // initialize m_curves and m_bbox
            bool isFirst = true;    // is the first List<UV> instance in curves
            foreach (List<UV> uvs in curves)
            {
                PointF[] pnts = new PointF[uvs.Count];
                m_curves.Add(pnts);
                for (int i = 0; i < uvs.Count; i++)
                {
                    Autodesk.Revit.DB.UV uv = uvs[i];
                    pnts[i] = new PointF((float)uv.U, (float)uv.V);
                    RectangleF tmpBBox = new RectangleF(pnts[i], new SizeF(0.0f, 0.0f));
                    m_numPoints++;
                    if (!isFirst)
                    {
                        // union the m_bbox with next curve's BBox
                        m_bbox = RectangleF.Union(m_bbox, tmpBBox);
                    }
                    else
                    {
                        m_bbox = tmpBBox;
                        isFirst = false;
                    }
                }
            }
        }

        /// <summary>
        /// Represents a PointF class with member data left uninitialized
        /// </summary>
        class EmptyGraphics2DData : Graphics2DData
        {
        }
    }
}
