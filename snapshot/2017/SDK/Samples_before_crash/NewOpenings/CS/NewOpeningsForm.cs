//
// (C) Copyright 2003-2016 by Autodesk, Inc.
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
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using Autodesk.Revit.DB;
using Point = System.Drawing.Point;

namespace Revit.SDK.Samples.NewOpenings.CS
{
    /// <summary>
    /// Main form used to display the profile of Wall or Floor and draw the opening profiles.
    /// </summary>
    public partial class NewOpeningsForm : System.Windows.Forms.Form
    {
        #region class members
        private Profile m_profile;  //save the profile date (ProfileFloor or ProfileWall)
        private Matrix4 m_to2DMatrix; //save the matrix use to transform 3D to 2D
        private Matrix4 m_moveToCenterMatrix;  //save the matrix use to move point to origin
        private Matrix4 m_scaleMatrix; //save the matrix use to scale
        private ITool m_tool = null; //current using tool
        private Queue<ITool> m_tools = new Queue<ITool>(); //all tool can use in pictureBox       
        #endregion
        /// <summary>
        /// default constructor
        /// </summary>
        public NewOpeningsForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="profile">ProfileWall or ProfileFloor</param>
        public NewOpeningsForm(Profile profile)
            :this()
        {
            m_profile = profile;
            m_to2DMatrix = m_profile.To2DMatrix();
            m_moveToCenterMatrix = m_profile.ToCenterMatrix();
            InitTools();
        }

        /// <summary>
        /// add tools, then use can draw by these tools in picture box
        /// </summary>
        private void InitTools()
        {
            //wall
            if(m_profile is ProfileWall)
            {
                m_tool = new RectTool();
                m_tools.Enqueue(m_tool);
                m_tools.Enqueue(new EmptyTool());
            }
            //floor
            else
            {
                m_tool = new LineTool();
                m_tools.Enqueue(m_tool);
                m_tools.Enqueue(new RectTool());
                m_tools.Enqueue(new CircleTool());
                m_tools.Enqueue(new ArcTool());
                m_tools.Enqueue(new EmptyTool());
            }
        }

        /// <summary>
        /// use matrix to transform point
        /// </summary>
        /// <param name="pts">contain the points to be transform</param>
        private void TransFormPoints(Point[] pts)
        {
            System.Drawing.Drawing2D.Matrix matrix = new System.Drawing.Drawing2D.Matrix(
                1, 0, 0, 1, this.openingPictureBox.Width / 2, this.openingPictureBox.Height / 2);
            matrix.Invert();
            matrix.TransformPoints(pts);            
        }

        /// <summary>
        /// get four points on circle by center and one point on circle
        /// </summary>
        /// <param name="points">contain the center and one point on circle</param>
        private List<Vector4> GenerateCircle4Point(List<Point> points)
        {
            Matrix rotation = new Matrix(); 

            //get the circle center and bound point
            Point center = points[0];
            Point bound = points[1];
            rotation.RotateAt(90, (PointF)center);
            Point[] circle = new Point[4];
            circle[0] = points[1];
            for(int i = 1; i < 4; i++)
            {
                Point[] ps = new Point[1] { bound };
                rotation.TransformPoints(ps);
                circle[i] = ps[0];
                bound = ps[0];
            }
            return TransForm2DTo3D(circle);
        }

        /// <summary>
        /// Transform the point on Form to  3d world coordinate of Revit
        /// </summary>
        /// <param name="ps">contain the points to be transform</param>
        private List<Vector4> TransForm2DTo3D(Point[] ps)
        {
            List<Vector4> result = new List<Vector4>();
            TransFormPoints(ps);
            Matrix4 transFormMatrix = Matrix4.Multiply(
                m_scaleMatrix.Inverse(),m_moveToCenterMatrix);
            transFormMatrix = Matrix4.Multiply(transFormMatrix, m_to2DMatrix);
            foreach (Point point in ps)
            {
                Vector4 v = new Vector4(point.X, point.Y, 0);
                v = transFormMatrix.TransForm(v); 
                result.Add(v);
            }
            return result;
        }

        /// <summary>
        /// calculate the matrix use to scale
        /// </summary>
        /// <param name="size">pictureBox size</param>
        private Matrix4 ComputerScaleMatrix(Size size)
        {
            PointF[] boundPoints = m_profile.GetFaceBounds();
            float width = ((float)size.Width) / (boundPoints[1].X - boundPoints[0].X);
            float hight = ((float)size.Height) / (boundPoints[1].Y - boundPoints[0].Y);
            float factor = width <= hight ? width : hight;
            return new Matrix4(factor);
        }

        /// <summary>
        /// Calculate the matrix use to transform 3D to 2D
        /// </summary>
        private Matrix4 Comuter3DTo2DMatrix()
        {
            Matrix4 result = Matrix4.Multiply(
                m_to2DMatrix.Inverse(), m_moveToCenterMatrix.Inverse());
            result = Matrix4.Multiply(result, m_scaleMatrix);
            return result;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            foreach (ITool tool in m_tools)
            {
                List<List<Point>> curcves = tool.GetLines();
                foreach (List<Point> curve in curcves)
                {
                    List<Vector4> ps3D;

                    if (tool.ToolType == ToolType.Circle)
                    {
                        ps3D = GenerateCircle4Point(curve);
                    }
                    else if (tool.ToolType == ToolType.Rectangle)
                    {
                        Point[] ps = new Point[4] { curve[0], new Point(curve[0].X, curve[1].Y),
                            curve[1], new Point(curve[1].X, curve[0].Y) };
                        ps3D = TransForm2DTo3D(ps);
                    }
                    else
                    {
                        ps3D = TransForm2DTo3D(curve.ToArray());
                    }
                    m_profile.DrawOpening(ps3D, tool.ToolType);
                }
            }
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void openingPictureBox_Paint(object sender, PaintEventArgs e)
        {
            //Draw the pictures in the m_tools list
            foreach (ITool tool in m_tools)
            {
                tool.Draw(e.Graphics);
            }

            //draw the tips string
            e.Graphics.DrawString(m_tool.ToolType.ToString(), 
                SystemFonts.DefaultFont, SystemBrushes.Highlight, 2, 5);
            
            //move the origin to the picture center
            Size size = this.openingPictureBox.Size;
            e.Graphics.Transform = new System.Drawing.Drawing2D.Matrix(
                1, 0, 0, 1, size.Width / 2, size.Height / 2);

            //draw profile
            Size scaleSize = new Size((int)(0.85 * size.Width), (int)(0.85 * size.Height));
            m_scaleMatrix = ComputerScaleMatrix(scaleSize);
            Matrix4 trans = Comuter3DTo2DMatrix();
            m_profile.Draw2D(e.Graphics, Pens.Blue, trans);
        }

        /// <summary>
        /// mouse event handle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openingPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (MouseButtons.Left == e.Button || MouseButtons.Right == e.Button)
            {
                Graphics g = openingPictureBox.CreateGraphics();

                m_tool.OnMouseDown(g, e);
                m_tool.OnRightMouseClick(g, e);
            }
            else if (MouseButtons.Middle == e.Button)
            {
                m_tool.OnMidMouseDown(null, null);
                m_tool = m_tools.Peek();
                m_tools.Enqueue(m_tool);
                m_tools.Dequeue();
                Graphics graphic = openingPictureBox.CreateGraphics();
                graphic.DrawString(m_tool.ToolType.ToString(), 
                    SystemFonts.DefaultFont, SystemBrushes.Highlight, 2, 5);
                this.Refresh();
            }
        }

        /// <summary>
        /// Mouse event handle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openingPictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            Graphics g = openingPictureBox.CreateGraphics();
            m_tool.OnMouseUp(g, e);
        }

        /// <summary>
        /// Mouse event handle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openingPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            Graphics graphics = openingPictureBox.CreateGraphics();
            m_tool.OnMouseMove(graphics, e);
            PaintEventArgs paintArg = new PaintEventArgs(graphics, new System.Drawing.Rectangle());
            openingPictureBox_Paint(null, paintArg);
        }
    }
}