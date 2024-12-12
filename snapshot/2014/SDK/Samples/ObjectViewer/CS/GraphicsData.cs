//
// (C) Copyright 2003-2013 by Autodesk, Inc. All rights reserved.
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.

//
// AUTODESK PROVIDES THIS PROGRAM 'AS IS' AND WITH ALL ITS FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE. AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.


namespace Revit.SDK.Samples.ObjectViewer.CS
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Drawing;
    using System.Drawing.Drawing2D;

    using Autodesk.Revit.Structural;
    using Autodesk.Revit.Geometry;

    /// <summary>
    /// interface as the datasource of view, include nececessary members
    /// </summary>
    public interface IGraphicsData
    {
        /// <summary>
        /// 2D lines
        /// </summary>
        /// <returns></returns>
        List<PointF[]> PointCurves();

        /// <summary>
        /// view data update
        /// </summary>
        event UpdateViewDelegate UpdateViewEvent;

        /// <summary>
        /// region of 3D view data
        /// </summary>
        RectangleF Region
        {
            get;
        }
    }

    /// <summary>
    /// abstrace class include general members
    /// </summary>
    public abstract class GraphicsDataBase : IGraphicsData
    {
        protected XYZ m_transferedMax;  //3D max point after transfered
        protected XYZ m_transferedMin;  //3D min point after transfered
        protected XYZ m_originMax = new XYZ(double.MinValue, double.MinValue, double.MinValue); //3D max point
        protected XYZ m_originMin = new XYZ(double.MaxValue, double.MaxValue, double.MaxValue); //3d min point
        protected double[,] m_origin = { { 1.0, 0.0, 0.0 }, { 0.0, 1.0, 0.0 }, { 0.0, 0.0, 1.0 } }; //tranform matrix between origin matrix and view matrix

        public const float MINEDGElENGTH = 1.0f;    //minimum value of the region box's edge
        public const double ROTATEANGLE = System.Math.PI / 90;  //default angle when rotate around X,Y,Z axis

        public virtual event UpdateViewDelegate UpdateViewEvent;

        /// <summary>
        /// constructor
        /// </summary>
        public GraphicsDataBase()
        {
            Initialize();
        }

        public double[,] Origin
        {
            get
            {
                return m_origin;
            }
            set
            {
                m_origin = value;
            }
        }

        /// <summary>
        /// initialize some member data
        /// </summary>
        protected virtual void Initialize()
        {
            m_transferedMax = new XYZ(double.MinValue, double.MinValue, double.MinValue);
            m_transferedMin = new XYZ(double.MaxValue, double.MaxValue, double.MaxValue);
        }

        /// <summary>
        /// trig updata view event
        /// </summary>
        public virtual void UpdataData()
        {
            UpdateViewEvent();
        }

        public abstract List<PointF[]> PointCurves();

        /// <summary>
        /// rectangular region of 3D view data
        /// </summary>
        public RectangleF Region
        {
            get
            {
                float width = (float)Math.Abs(m_transferedMax.X - m_transferedMin.X);
                float height = (float)Math.Abs(m_transferedMax.Y - m_transferedMin.Y);

                if (width < 1f)
                {
                    width = 1f;
                }
                if (height < 1f)
                {
                    height = 1f;
                }

                float maxX = (width / 2);
                float maxY = (height / 2);
                float minX = -(width / 2);
                float minY = -(height / 2);

                RectangleF rec = new RectangleF(minX, minY, width, height);
                return rec;
            }
        }

        /// <summary>
        /// rotate around Z axis with default angle
        /// </summary>
        /// <param name="direction">minus or positive angle</param>
        public void RotateZ(bool direction)
        {
            double angle = ROTATEANGLE;
            if (!direction)
            {
                angle = -ROTATEANGLE;
            }

            RotateZ(ref m_origin, angle);
            UpdataData();
        }

        /// <summary>
        /// rotate 3*3 matrix around Z axis
        /// </summary>
        /// <param name="origin">matrix to rotate</param>
        /// <param name="angle"></param>
        private void RotateZ(ref double[,] origin, double angle)
        {
            double sin = Math.Sin(angle);
            double cos = Math.Cos(angle);

            double[,] rotate = { { cos, sin, 0.0 }, { -sin, cos, 0.0 }, { 0.0, 0.0, 1.0 } };
            origin = MathUtil.MultiCross(m_origin, rotate);
        }

        /// <summary>
        /// rotate around Y axis with default angle
        /// </summary>
        /// <param name="direction">minus or positive angle</param>
        public void RotateY(bool direction)
        {
            double angle = ROTATEANGLE;
            if (!direction)
            {
                angle = -ROTATEANGLE;
            }

            RotateY(ref m_origin, angle);
            UpdataData();
        }

        /// <summary>
        /// rotate 3*3 matrix around Y axis
        /// </summary>
        /// <param name="origin">matrix to rotate</param>
        /// <param name="angle"></param>
        private void RotateY(ref double[,] origin, double angle)
        {
            double sin = Math.Sin(angle);
            double cos = Math.Cos(angle);

            double[,] rotate = { { cos, 0.0, -sin }, { 0.0, 1.0, 0.0 }, { sin, 0.0, cos } };
            origin = MathUtil.MultiCross(m_origin, rotate);
        }

        /// <summary>
        /// rotate around X axis with default angle
        /// </summary>
        /// <param name="direction">minus or positive angle</param>
        public void RotateX(bool direction)
        {
            double angle = ROTATEANGLE;
            if (!direction)
            {
                angle = -ROTATEANGLE;
            }

            RotateX(ref m_origin, angle);
            UpdataData();
        }

        /// <summary>
        /// rotate 3*3 matrix around X axis
        /// </summary>
        /// <param name="origin">matrix to rotate</param>
        /// <param name="angle"></param>
        private void RotateX(ref double[,] origin, double angle)
        {
            double sin = Math.Sin(angle);
            double cos = Math.Cos(angle);

            double[,] rotate = { { 1.0, 0.0, 0.0 }, { 0.0, cos, sin }, { 0.0, -sin, cos } };
            origin = MathUtil.MultiCross(m_origin, rotate);
        }
    }

    /// <summary>
    /// datasource that can bind to PictureBox3D
    /// </summary>
    public class GraphicsData : GraphicsDataBase
    {
        List<XYZArray> m_originCurves;
        List<XYZArray> m_transferedCurves;
        List<PointF[]> m_curves2D;

        public override event UpdateViewDelegate UpdateViewEvent;

        /// <summary>
        /// constructor to initialize member data
        /// </summary>
        public GraphicsData()
            : base()
        {
            m_originCurves = new List<XYZArray>();
            m_transferedCurves = new List<XYZArray>();
            m_curves2D = new List<PointF[]>();
        }

        /// <summary>
        /// curves array
        /// </summary>
        /// <returns></returns>
        public override List<PointF[]> PointCurves()
        {
            return m_curves2D;
        }

        /// <summary>
        /// insert curves array into datasource
        /// </summary>
        /// <param name="points">points compose the curve</param>
        public void InsertCurve(XYZArray points)
        {
            m_originCurves.Add(points);
            foreach (XYZ point in points)
            {
                XYZ temp = TransferRotate(point);
                UpdateRange(point, ref m_originMin, ref m_originMax);
                UpdateRange(temp, ref m_transferedMin, ref m_transferedMax);
            }
        }

        /// <summary>
        /// update 3D view data
        /// </summary>
        public override void UpdataData()
        {
            m_transferedCurves.Clear();
            m_curves2D.Clear();

            foreach (XYZArray points in m_originCurves)
            {
                SynChroData(points);
            }

            if (null != UpdateViewEvent)
            {
                UpdateViewEvent();
            }
        }

        /// <summary>
        /// update 3D view curve data with origin data and transfer matrix
        /// </summary>
        /// <param name="points"></param>
        private void SynChroData(XYZArray points)
        {
            int size = points.Size;
            PointF[] points2D = new PointF[size];
            XYZArray transferedPoints = new XYZArray();
            for (int i = 0; i < size; i++)
            {
                XYZ point = points.get_Item(i);
                XYZ temp = TransferMove(point);
                XYZ transferedPoint = TransferRotate(temp);

                points2D[i] = new PointF((float)transferedPoint.X, (float)transferedPoint.Y);
                transferedPoints.Append(ref transferedPoint);
            }
            m_transferedCurves.Add(transferedPoints);
            m_curves2D.Add(points2D);
        }

        /// <summary>
        /// compare to get the max and min value
        /// </summary>
        /// <param name="point">point to be compared</param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        private void UpdateRange(XYZ point, ref XYZ min, ref XYZ max)
        {
            if (point.X > max.X)
            {
                max.X = point.X;
            }
            if (point.Y > max.Y)
            {
                max.Y = point.Y;
            }
            if (point.Z > max.Z)
            {
                max.Z = point.Z;
            }
            if (point.X < min.X)
            {
                min.X = point.X;
            }
            if (point.Y < min.Y)
            {
                min.Y = point.Y;
            }
            if (point.Z < min.Z)
            {
                min.Z = point.Z;
            }
        }

        /// <summary>
        /// rotate points with origion matrix
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        private XYZ TransferRotate(XYZ point)
        {
            XYZ newPoint = new XYZ();
            double x = point.X;
            double y = point.Y;
            double z = point.Z;

            //transform the origin of the old coordinate system in the new coordinate system
            newPoint.X = x * m_origin[0, 0] + y * m_origin[0, 1] + z * m_origin[0, 2];
            newPoint.Y = x * m_origin[1, 0] + y * m_origin[1, 1] + z * m_origin[1, 2];
            newPoint.Z = x * m_origin[2, 0] + y * m_origin[2, 1] + z * m_origin[2, 2];

            return newPoint;
        }

        /// <summary>
        /// move the point so that the center of the curves in 3D view is origin
        /// </summary>
        /// <param name="point">points to be moved</param>
        /// <returns>moved result</returns>
        private XYZ TransferMove(XYZ point)
        {
            XYZ newPoint = new XYZ();

            //transform the origin of the old coordinate system in the new coordinate system
            newPoint.X = point.X - (m_transferedMax.X + m_transferedMin.X) / 2;
            newPoint.Y = point.Y - (m_transferedMax.Y + m_transferedMin.Y) / 2;
            newPoint.Z = point.Z - (m_transferedMax.Z + m_transferedMin.Z) / 2;

            return newPoint;
        }
    }
}
