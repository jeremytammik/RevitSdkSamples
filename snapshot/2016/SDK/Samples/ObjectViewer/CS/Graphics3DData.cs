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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.ObjectModel;

using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.DB;

namespace Revit.SDK.Samples.ObjectViewer.CS
{
    /// <summary>
    /// trigger when view data updated
    /// </summary>
    public delegate void UpdateViewDelegate();

    /// <summary>
    /// data class for graphics 3D
    /// </summary>
    public class Graphics3DData
    {
        /// <summary>
        /// Chooses the camera direction to the view
        /// </summary>
        public enum ViewDirections
        {
            /// <summary>
            /// top direction
            /// </summary>
            Top,

            /// <summary>
            /// front view
            /// </summary>
            Front,

            /// <summary>
            /// left view
            /// </summary>
            Left,

            /// <summary>
            /// right view
            /// </summary>
            Right,

            /// <summary>
            /// bottom view
            /// </summary>
            Bottom,

            /// <summary>
            /// back view
            /// </summary>
            Back,

            /// <summary>
            /// iso view diection
            /// </summary>
            IsoMetric
        }

        // default angle when rotate around X,Y,Z axis
        private const double RotateAngle = System.Math.PI / 90;
        // initial curves data before UCS transform
        private readonly List<List<Vector>> m_iniCurves = new List<List<Vector>>();
        // curves data after UCS transform
        private readonly List<List<Vector>> m_curves = new List<List<Vector>>();
        // number of points representing the 3D geometry
        private readonly int m_numPoints;

        // BoundingBox of the 3D geometry
        private BoundingBoxXYZ m_bbox = new BoundingBoxXYZ();
        // current UCS
        private UCS m_currentUCS = new UCS();

        /// <summary>
        /// Current UCS used to transform
        /// </summary>
        public UCS CurrentUCS
        {
            get
            {
                return m_currentUCS;
            }
            set
            {
                m_currentUCS = value;
                UpdateDisplayData(m_currentUCS);
                if (null != UpdateViewEvent)
                {
                    UpdateViewEvent();
                }
            }
        }

        /// <summary>
        /// Number of points representing the 3D geometry
        /// </summary>
        public int NumPoints
        {
            get
            {
                return m_numPoints;
            }
        }

        // 7 UCS according to camera directions in ViewDirections
        private static readonly UCS[] SpecialUCSs = new UCS[7];
        // small angle used to Rotate UCS around X, Y, Z axis
        private static readonly UCS RotateXUCS;
        private static readonly UCS RotateAntiXUCS;
        private static readonly UCS RotateYUCS;
        private static readonly UCS RotateAntiYUCS;
        private static readonly UCS RotateZUCS;
        private static readonly UCS RotateAntiZUCS;

        /// <summary>
        /// static constructor used to initialize static members
        /// </summary>
        static Graphics3DData()
        {
            // initialize small angle
            double angle = RotateAngle;
            double antiAngle = -RotateAngle;
            double sin = Math.Sin(angle);
            double cos = Math.Cos(angle);
            double antiSin = Math.Sin(antiAngle);
            double antiCos = Math.Cos(antiAngle);
            // initialize 6 Rotate UCS
            Vector origin = new Vector();
            RotateXUCS =
                new UCS(origin, new Vector(1.0, 0.0, 0.0), new Vector(0.0, cos, sin));
            RotateAntiXUCS =
                new UCS(origin, new Vector(1.0, 0.0, 0.0), new Vector(0.0, antiCos, antiSin));
            RotateYUCS =
                new UCS(origin, new Vector(cos, 0.0, -sin), new Vector(0.0, 1.0, 0.0));
            RotateAntiYUCS =
                new UCS(origin, new Vector(antiCos, 0.0, -antiSin), new Vector(0.0, 1.0, 0.0));
            RotateZUCS =
                new UCS(origin, new Vector(cos, sin, 0.0), new Vector(-sin, cos, 0.0));
            RotateAntiZUCS =
                new UCS(origin, new Vector(antiCos, antiSin, 0.0), new Vector(-antiSin, antiCos, 0.0));

            // initialize 7 special UCS
            SpecialUCSs[(int)ViewDirections.IsoMetric] = new UCS(origin,
                new Vector(-0.408248290463863, 0.408248290463863, 0.816496580927726),
                new Vector(0.707106781186548, 0.707106781186548, 0.0));
            SpecialUCSs[(int)ViewDirections.Top] =
                new UCS(origin, new Vector(1.0, 0.0, 0.0), new Vector(0.0, 1.0, 0.0));
            SpecialUCSs[(int)ViewDirections.Front] =
                new UCS(origin, new Vector(-1.0, 0.0, 0.0), new Vector(0.0, 0.0, 1.0));
            SpecialUCSs[(int)ViewDirections.Left] =
                new UCS(origin, new Vector(0.0, -1.0, 0.0), new Vector(0.0, 0.0, 1.0));
            SpecialUCSs[(int)ViewDirections.Right] =
                new UCS(origin, new Vector(0.0, 1.0, 0.0), new Vector(0.0, 0.0, 1.0));
            SpecialUCSs[(int)ViewDirections.Bottom] =
                new UCS(origin, new Vector(-1.0, 0.0, 0.0), new Vector(0.0, 1.0, 0.0));
            SpecialUCSs[(int)ViewDirections.Back] =
                new UCS(origin, new Vector(1.0, 0.0, 0.0), new Vector(0.0, 0.0, 1.0));
        }

        /// <summary>
        /// view data update
        /// </summary>
        public event UpdateViewDelegate UpdateViewEvent;

        /// <summary>
        /// Curves represent the wireframe of the 3D geometry
        /// </summary>
        public List<List<Vector>> ConstCurves
        {
            get
            {
                return m_curves;
            }
        }

        /// <summary>
        /// BoundingBox of the 3D geometry
        /// </summary>
        public BoundingBoxXYZ BBox
        {
            get
            {
                return m_bbox;
            }
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="curves">curves represent the wireframe of the 3D geometry</param>
        /// <param name="bbox">oundingBox of the 3D geometry</param>
        public Graphics3DData(List<List<XYZ>> curves, BoundingBoxXYZ bbox)
        {
            if (null == bbox)
            {
                return;
            }

            foreach (UCS ucs in SpecialUCSs)
            {
                ucs.Origin = FindBBoxCenter(bbox);
            }

            foreach (List<XYZ> pnts in curves)
            {
                List<Vector> vectors = new List<Vector>();
                m_iniCurves.Add(vectors);
                foreach (Autodesk.Revit.DB.XYZ pnt in pnts)
                {
                    vectors.Add(new Vector(pnt.X, pnt.Y, pnt.Z));
                    m_numPoints++;
                }
            }
            SetViewDirection(ViewDirections.IsoMetric);
            InitializeBBox(bbox);
        }

        /// <summary>
        /// change the camera direction to the geometry
        /// </summary>
        /// <param name="viewDirection">camera direction</param>
        public void SetViewDirection(ViewDirections viewDirection)
        {
            m_currentUCS = SpecialUCSs[(int)viewDirection];
            UpdateDisplayData(m_currentUCS);
        }

        /// <summary>
        /// rotate around Z axis with default angle
        /// </summary>
        /// <param name="direction">minus or positive angle</param>
        public void RotateZ(bool direction)
        {
            if (direction)
            {
                m_currentUCS = m_currentUCS + RotateZUCS;
                UpdateDisplayData(m_currentUCS);
            }
            else
            {
                m_currentUCS = m_currentUCS + RotateAntiZUCS;
                UpdateDisplayData(m_currentUCS);
            }
        }

        /// <summary>
        /// rotate around Y axis with default angle
        /// </summary>
        /// <param name="direction">minus or positive angle</param>
        public void RotateY(bool direction)
        {
            if (direction)
            {
                m_currentUCS = m_currentUCS + RotateYUCS;
                UpdateDisplayData(m_currentUCS);
            }
            else
            {
                m_currentUCS = m_currentUCS + RotateAntiYUCS;
                UpdateDisplayData(m_currentUCS);
            }
        }

        /// <summary>
        /// rotate around X axis with default angle
        /// </summary>
        /// <param name="direction">minus or positive angle</param>
        public void RotateX(bool direction)
        {
            if (direction)
            {
                m_currentUCS = m_currentUCS + RotateXUCS;
                UpdateDisplayData(m_currentUCS);
            }
            else
            {
                m_currentUCS = m_currentUCS + RotateAntiXUCS;
                UpdateDisplayData(m_currentUCS);
            }
        }

        /// <summary>
        /// update data according to current UCS
        /// </summary>
        /// <param name="lc"></param>
        private void UpdateDisplayData(UCS lc)
        {
            m_curves.Clear();
            foreach (List<Vector> iniVectors in m_iniCurves)
            {
                List<Vector> vectors = new List<Vector>();
                m_curves.Add(vectors);
                for (int i = 0; i < iniVectors.Count; i++)
                {
                    // transform points to local coordinate system
                    vectors.Add(lc.GC2LC(iniVectors[i]));
                }
            }
            // trigger update view event
            if (null != UpdateViewEvent)
            {
                UpdateViewEvent();
            }
        }

        /// <summary>
        /// move the BoundingBox to the center of the Coordinate System,
        /// modify its size to the size of Geometry's BoundingBox
        /// </summary>
        /// <param name="bbox"></param>
        private void InitializeBBox(BoundingBoxXYZ bbox)
        {
            Autodesk.Revit.DB.XYZ size = MathUtil.SubXYZ(bbox.Max, bbox.Min);
            m_bbox.Max = MathUtil.DivideXYZ(size, 2.0);
            m_bbox.Min = MathUtil.DivideXYZ(size, -2.0);
        }

        /// <summary>
        /// find the center of the BoundingBox
        /// </summary>
        /// <param name="bbox">BoundingBox</param>
        /// <returns>center Point</returns>
        private Vector FindBBoxCenter(BoundingBoxXYZ bbox)
        {
            Autodesk.Revit.DB.XYZ center = MathUtil.DivideXYZ(MathUtil.AddXYZ(bbox.Max, bbox.Min), 2.0);
            return MathUtil.XYZ2Vector(center);
        }
    }
}
