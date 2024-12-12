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
using System.Drawing.Drawing2D;

using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.DB;

namespace Revit.SDK.Samples.InPlaceMembers.CS
{
    /// <summary>
    /// trigger when view data updated
    /// </summary>
    public delegate void UpdateViewDelegate();

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
    /// abstract class include general members
    /// </summary>
    public abstract class GraphicsDataBase:IGraphicsData
    {
        /// <summary>
        ///3D max point after transfered 
        /// </summary>
        protected Autodesk.Revit.DB.XYZ m_transferedMax;    

        /// <summary>
        ///3D min point after transfered 
        /// </summary>
        protected Autodesk.Revit.DB.XYZ m_transferedMin;    

        /// <summary>
        /// origin max define
        /// </summary>
        protected Autodesk.Revit.DB.XYZ m_originMax = new Autodesk.Revit.DB.XYZ (double.MinValue, double.MinValue, double.MinValue);    

        /// <summary>
        /// origin min define
        /// </summary>
        protected Autodesk.Revit.DB.XYZ m_originMin = new Autodesk.Revit.DB.XYZ (double.MaxValue, double.MaxValue, double.MaxValue);

        /// <summary>
        /// origin define
        /// </summary>
        protected double[,] m_origin = {{ 1.0, 0.0, 0.0 }, { 0.0, 1.0, 0.0 }, { 0.0, 0.0, 1.0 } };

        /// <summary>
        ///minimum value of the region box's edge 
        /// </summary>
        public const float MINEDGElENGTH = 1.0f;

        /// <summary>
        /// default angle when rotate around X,Y,Z axis
        /// </summary>
        public const double ROTATEANGLE = System.Math.PI / 90;    

        /// <summary>
        /// update view event
        /// </summary>
        public virtual event UpdateViewDelegate UpdateViewEvent;
        
        /// <summary>
        /// constructor
        /// </summary>
        public GraphicsDataBase()
        {
            Initialize();
        }

        /// <summary>
        /// initialize some member data
        /// </summary>
        protected virtual void Initialize()
        {    
            double INITANGLE = Math.PI / 4;

            RotateX(ref m_origin, INITANGLE);
            RotateY(ref m_origin, INITANGLE);
            m_transferedMax = new Autodesk.Revit.DB.XYZ (double.MinValue, double.MinValue, double.MinValue);
            m_transferedMin = new Autodesk.Revit.DB.XYZ (double.MaxValue, double.MaxValue, double.MaxValue);
        }

        /// <summary>
        /// trigger updata view event
        /// </summary>
        public virtual void UpdataData()
        {
            UpdateViewEvent();
        }

        /// <summary>
        /// point curves function
        /// </summary>
        /// <returns>the points list of curves</returns>
        public abstract List<PointF[]> PointCurves();

        /// <summary>
        /// rectangular region of 3D view data
        /// </summary>
        public RectangleF Region
        {
            get
            {
                float width = (float)(m_transferedMax.X - m_transferedMin.X);
                float height = (float)(m_transferedMax.Y - m_transferedMin.Y);

                float maxX = (width / 2);
                float maxY = (height / 2);
                float minX = -(width / 2);
                float minY = -(height / 2);

                if (width < 1)
                {
                    width = 1;
                }
                if (height < 1)
                {
                    height = 1;
                }

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
            origin = MatrixArith.MultiCross(m_origin, rotate);
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
            origin = MatrixArith.MultiCross(m_origin, rotate);
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

            RotateX(ref m_origin ,angle);
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
            origin = MatrixArith.MultiCross(m_origin, rotate);
        }
    }

    /// <summary>
    /// datasource that can bind to PictureBox3D
    /// </summary>
    public class GraphicsData : GraphicsDataBase
    {
        List<List<XYZ>> m_originCurves;
        List<List<XYZ>> m_transferedCurves;
        List<PointF[]> m_curves2D;

        /// <summary>
        /// update view event
        /// </summary>
        public override event UpdateViewDelegate UpdateViewEvent;

        /// <summary>
        /// constructor to initialize member data
        /// </summary>
        public GraphicsData() : base()
        {
            m_originCurves = new List<List<XYZ>>();
            m_transferedCurves = new List<List<XYZ>>();
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
        public void InsertCurve(List<XYZ> points)
        {
            m_originCurves.Add(points);
            foreach (Autodesk.Revit.DB.XYZ point in points)
            {
                m_originMin = UpdateMinRange(point);
                m_originMax = UpdateMaxRange(point);
            }
        }

        /// <summary>
        /// update 3D view data
        /// </summary>
        public override void UpdataData()
        {
            m_transferedMin = TransferRotate(m_originMin);
            m_transferedMax = TransferRotate(m_originMax);    
        
            m_transferedCurves.Clear();
            m_curves2D.Clear();

            foreach (List<XYZ> points in m_originCurves)
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
        private void SynChroData(List<XYZ> points)
        {
            int size = points.Count;
            PointF[] points2D = new PointF[size];
            List<XYZ> transferedPoints = new List<XYZ>();
            for(int i = 0; i<size;i++)
            {
                Autodesk.Revit.DB.XYZ point = points[i];
                Autodesk.Revit.DB.XYZ temp = TransferRotate(point);
                Autodesk.Revit.DB.XYZ transferedPoint = TransferMove(temp);
                points2D[i] = new PointF((float)transferedPoint.X, (float)transferedPoint.Y);
            }
            m_transferedCurves.Add(transferedPoints);
            m_curves2D.Add(points2D);
        }

        private XYZ UpdateMaxRange(XYZ pnt)
        {
            return new XYZ(
                pnt.X > m_originMax.X ? pnt.X : m_originMax.X,
                pnt.Y > m_originMax.Y ? pnt.Y : m_originMax.Y,
                pnt.Z > m_originMax.Z ? pnt.Z : m_originMax.Z);
        }

        private XYZ UpdateMinRange(XYZ pnt)
        {
            return new XYZ(
                pnt.X < m_originMin.X ? pnt.X : m_originMin.X,
                pnt.Y < m_originMin.Y ? pnt.Y : m_originMin.Y,
                pnt.Z < m_originMin.Z ? pnt.Z : m_originMin.Z);
        }


        /// <summary>
        /// rotate points with origion matrix
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        private Autodesk.Revit.DB.XYZ TransferRotate(Autodesk.Revit.DB.XYZ point)
        {
            double x = point.X;
            double y = point.Y;
            double z = point.Z;

            return new XYZ(
                x * m_origin[0, 0] + y * m_origin[0, 1] + z * m_origin[0, 2],
                x * m_origin[1, 0] + y * m_origin[1, 1] + z * m_origin[1, 2],
                x * m_origin[2, 0] + y * m_origin[2, 1] + z * m_origin[2, 2]);

        }

        /// <summary>
        /// move the point so that the center of the curves in 3D view is origin
        /// </summary>
        /// <param name="point">points to be moved</param>
        /// <returns>moved result</returns>
        private Autodesk.Revit.DB.XYZ TransferMove(Autodesk.Revit.DB.XYZ point)
        {
            //transform the origin of the old coordinate system in the new coordinate system
            
            return new XYZ(
                point.X - (m_transferedMax.X + m_transferedMin.X) / 2,
                point.Y - (m_transferedMax.Y + m_transferedMin.Y) / 2,
                point.Z - (m_transferedMax.Z + m_transferedMin.Z) / 2);
        }
    }

    /// <summary>
    /// utility class provide arithmetic of matrix
    /// </summary>
    public class MatrixArith
    {
        /// <summary>
        /// multiply cross two matrix
        /// </summary>
        /// <param name="m1">left matrix</param>
        /// <param name="m2">right matrix</param>
        /// <returns>result matrix</returns>
        public static double[,] MultiCross(double[,] m1, double[,] m2)
        {
            double[,] result = new double[3, 3];

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        result[i, j] += m1[i, k] * m2[k, j];
                    }                    
                }
            }

            return result;
        }
    }
}
