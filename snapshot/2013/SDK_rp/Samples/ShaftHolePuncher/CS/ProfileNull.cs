//
// (C) Copyright 2003-2012 by Autodesk, Inc.
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
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit;
using System.Drawing;
using System.Drawing.Drawing2D;
using Point = System.Drawing.Point;

namespace Revit.SDK.Samples.ShaftHolePuncher.CS
{
    /// <summary>
    /// ProfileNull class contains method to draw a coordinate system,
    /// and contains method used to create Shaft Opening
    /// </summary>
    public class ProfileNull : Profile
    {
        Level level1 = null; //level 1 used to create Shaft Opening
        Level level2 = null; //level 2 used to create Shaft Opening
        float m_scale = 1; //scale of shaft opening

        /// <summary>
        /// Scale property to get/set scale of shaft opening
        /// </summary>
        public float Scale
        {
            get
            {
                return m_scale;
            }
            set
            {
                m_scale = value;
            }
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="commandData">object which contains reference of Revit Application</param>
        public ProfileNull(ExternalCommandData commandData)
            : base(commandData)
        {
            GetLevels();
            m_to2DMatrix = new Matrix4();
            m_moveToCenterMatrix = new Matrix4();
        }

        /// <summary>
        /// get level1 and level2 used to create shaft opening
        /// </summary>
        private void GetLevels()
        {
            IList<Element> levelList = (new FilteredElementCollector(m_commandData.Application.ActiveUIDocument.Document)).OfClass(typeof(Level)).ToElements();
            IEnumerable<Level> levels = from elem in levelList
                                         let level = elem as Level
                                         where level != null && "Level 1" == level.Name
                                         select level;
            if (levels.Count()>0)
            {
                level1 = levels.First();
            }

            levels = from elem in levelList
                     let level = elem as Level
                     where level != null && "Level 2" == level.Name
                     select level;
            if (levels.Count() > 0)
            {
                level2 = levels.First();
            }
            
        }

        /// <summary>
        /// calculate the matrix for scale
        /// </summary>
        /// <param name="size">pictureBox size</param>
        /// <returns>maxtrix to scale the opening curve</returns>
        public override Matrix4 ComputeScaleMatrix(Size size)
        {
            m_scaleMatrix = new Matrix4(m_scale);
            return m_scaleMatrix;
        }

        /// <summary>
        /// calculate the matrix used to transform 3D to 2D.
        /// because profile of shaft opening in Revit is 2d too,
        /// so we need do nothing but new a matrix
        /// </summary>
        /// <returns>maxtrix is use to transform 3d points to 2d</returns>
        public override Matrix4 Compute3DTo2DMatrix()
        {
            m_transformMatrix = new Matrix4();
            return m_transformMatrix;
        }

        /// <summary>
        /// draw the coordinate system
        /// </summary>
        /// <param name="graphics">form graphic</param>
        /// <param name="pen">pen used to draw line in pictureBox</param>
        /// <param name="matrix4">Matrix used to transform 3d to 2d 
        /// and make picture in right scale </param>
        public override void Draw2D(Graphics graphics, Pen pen, Matrix4 matrix4)
        {
            graphics.Transform = new System.Drawing.Drawing2D.Matrix( 
                1, 0, 0, 1, 0, 0);
            //draw X axis
            graphics.DrawLine(pen, new Point(20, 280), new Point(400, 280));
            graphics.DrawPie(pen, 400, 265, 30, 30, 165, 30);
            //draw Y axis
            graphics.DrawLine(pen, new Point(20, 280), new Point(20, 50));
            graphics.DrawPie(pen, 5, 20, 30, 30, 75, 30);
            //draw scale
            graphics.DrawLine(pen, new Point(120, 275), new Point(120, 285));
            graphics.DrawLine(pen, new Point(220, 275), new Point(220, 285));
            graphics.DrawLine(pen, new Point(320, 275), new Point(320, 285));
            graphics.DrawLine(pen, new Point(15, 80), new Point(25, 80));
            graphics.DrawLine(pen, new Point(15, 180), new Point(25, 180));
            //dimension
            Font font = new Font("Verdana", 10, FontStyle.Regular);
            graphics.DrawString("100'", font, Brushes.Blue, new PointF(122, 266));
            graphics.DrawString("200'", font, Brushes.Blue, new PointF(222, 266));
            graphics.DrawString("300'", font, Brushes.Blue, new PointF(322, 266));
            graphics.DrawString("100'", font, Brushes.Blue, new PointF(22, 181));
            graphics.DrawString("200'", font, Brushes.Blue, new PointF(22, 81));
            graphics.DrawString("(0,0)", font, Brushes.Blue, new PointF(10, 280));
        }

        /// <summary>
        /// move the points to the center and scale as user selected.
        /// profile of shaft opening in Revit is 2d too, so don't need transform points to 2d
        /// </summary>
        /// <param name="ps">contain the points to be transformed</param>
        /// <returns>Vector list contains points have been transformed</returns>
        public override List<Vector4> Transform2DTo3D(Point[] ps)
        {
            List<Vector4> result = new List<Vector4>();
            foreach (Point point in ps)
            {
                //because our coordinate system is different with window UI
                //so we should change what we got from UI coordinate
                Vector4 v = new Vector4((point.X - 20), - (point.Y - 280), 0);
                v = m_scaleMatrix.Transform(v);
                result.Add(v);
            }

            return result;
        }

        /// <summary>
        /// Create Shaft Opening
        /// </summary>
        /// <param name="points">points used to create Opening</param>
        /// <returns>newly created Opening</returns>
        public override Opening CreateOpening(List<Vector4> points)
        {
            Autodesk.Revit.DB.XYZ p1, p2; Line curve;
            CurveArray curves = m_appCreator.NewCurveArray();
            for (int i = 0; i < points.Count - 1; i++)
            {
                p1 = new Autodesk.Revit.DB.XYZ (points[i].X, points[i].Y, points[i].Z);
                p2 = new Autodesk.Revit.DB.XYZ (points[i + 1].X, points[i + 1].Y, points[i + 1].Z);
                curve = m_appCreator.NewLine(p1, p2, true);
                curves.Append(curve);
            }

            //close the curve
            p1 = new Autodesk.Revit.DB.XYZ (points[0].X, points[0].Y, points[0].Z);
            p2 = new Autodesk.Revit.DB.XYZ (points[points.Count - 1].X, 
                points[points.Count - 1].Y, points[points.Count - 1].Z);
            curve = m_appCreator.NewLine(p1, p2, true);
            curves.Append(curve);

            return m_docCreator.NewOpening(level1, level2, curves);
        }
    }
}
