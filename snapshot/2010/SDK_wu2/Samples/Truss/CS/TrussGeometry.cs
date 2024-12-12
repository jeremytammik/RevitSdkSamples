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
using System;
using System.Collections.Generic;
using System.Text;
using Autodesk.Revit.Geometry;
using System.Drawing;
using Autodesk.Revit;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Collections;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Structural;
using Point = System.Drawing.Point;

namespace Revit.SDK.Samples.Truss.CS
{
    /// <summary>
    /// TrussGeometry class contains Geometry information of new created Truss,
    /// and contains methods used to Edit profile of truss.
    /// </summary>
    class TrussGeometry
    {
        #region class member variables

        Autodesk.Revit.Elements.Truss m_truss; //object of truss in Revit

        LineTool m_topChord; //line tool used to draw top chord

        LineTool m_bottomChord; //line tool used to draw top chord

        ArrayList m_graphicsPaths; //store all the GraphicsPath objects of each curve in truss.

        int m_selectMemberIndex = -1; // index of selected truss member (beam), -1 when nothing selected.

        int m_clickMemberIndex = -1; // index of clicked truss member (beam), -1 when nothing clicked.

        XYZArray m_points; // store all the points on the needed face

        XYZ[] m_boundPoints; // store array store bound point of truss

        Matrix4 m_to2DMatrix = null; // store the Matrix used to transform 3D points to 2D

        Matrix4 m_moveToCenterMatrix = null; // store the Matrix used to move points to center

        Matrix4 m_scaleMatrix = null; // store the Matrix used to scale profile fit to pictureBox

        Matrix4 m_transformMatrix = null; // store the Matrix used to transform Revit coordinate to window UI

        Matrix4 m_restoreMatrix = null; // store the Matrix used to transform window UI coordinate to Revit

        Matrix4 m_2DToTrussProfileMatrix = null; //store matrix use to transform point on pictureBox to truss (profile) plane

        Vector4 m_origin = null; //base point of truss

        ExternalCommandData m_commandData; //object which contains reference of Revit Application

        XYZ startLocation = null; //store the start point of truss location

        XYZ endLocation = null; //store the end point of truss location

        #endregion

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="truss">new created truss object in Revit</param>
        public TrussGeometry(Autodesk.Revit.Elements.Truss truss, ExternalCommandData commandData)
        {
            m_commandData = commandData;
            m_topChord = new LineTool();
            m_bottomChord = new LineTool();
            m_truss = truss;
            m_graphicsPaths = new ArrayList();
            GetTrussGeometryInfo();
        }

        /// <summary>
        /// Calculate geometry info for truss
        /// </summary>
        private void GetTrussGeometryInfo()
        {
            // get the start and end point of the basic line of the truss
            m_points = GetTrussPoints();
            // Get a matrix which can transform points to 2D
            m_to2DMatrix = GetTo2DMatrix();
            // get the boundary of all the points
            m_boundPoints = GetBoundsPoints();
            // get a matrix which can keep all the points in the center of the canvas
            m_moveToCenterMatrix = GetMoveToCenterMatrix();
            // get a matrix for scaling all the points and lines within the canvas
            m_scaleMatrix = GetScaleMatrix();
            // transform 3D points to 2D
            m_transformMatrix = Get3DTo2DMatrix();
            // transform from 2D to 3D
            m_restoreMatrix = Get2DTo3DMatrix();
            // transform from 2D (on picture box) to truss profile plane
            m_2DToTrussProfileMatrix = Get2DToTrussProfileMatrix();
            // create the graphics path which contains all the lines
            CreateGraphicsPath();
        }

        /// <summary>
        /// Get points of the truss
        /// </summary>
        /// <returns>points array stores all the points on truss</returns>
        public XYZArray GetTrussPoints()
        {
            XYZArray xyzArray = new XYZArray();
            try
            {
                IEnumerator iter = m_truss.Members.GetEnumerator();
                iter.Reset();
                while (iter.MoveNext())
                {
                    ElementId id = (ElementId)(iter.Current);
                    Autodesk.Revit.Element elem =
                        m_commandData.Application.ActiveDocument.get_Element(ref id);
                    FamilyInstance familyInstace = (FamilyInstance)(elem);
                    AnalyticalModelFrame frame = (AnalyticalModelFrame)familyInstace.AnalyticalModel;
                    Line line = (Line)(frame.Curve);
                    xyzArray.Append(line.get_EndPoint(0));
                    xyzArray.Append(line.get_EndPoint(1));
                }
            }
            catch (System.ArgumentException)
            {
                MessageBox.Show("The start point and the end point of the line are too close, please re-draw it.");
            }
            return xyzArray;
        }

        /// <summary>
        /// Get a matrix which can transform points to 2D
        /// </summary>
        /// <returns>matrix which can transform points to 2D</returns>
        public Matrix4 GetTo2DMatrix() 
        {
            Line trussLocation = (m_truss.Location as LocationCurve).Curve as Line;
            startLocation = trussLocation.get_EndPoint(0);
            endLocation = trussLocation.get_EndPoint(1);
            //use baseline of truss as the X axis
            Vector4 xAxis = new Vector4(new XYZ(endLocation - startLocation));
            xAxis.Normalize();
            //get Z Axis
            Vector4 zAxis = Vector4.CrossProduct(xAxis, new Vector4(new XYZ(0, 0, 1)));
            zAxis.Normalize();
            //get Y Axis, downward
            Vector4 yAxis = Vector4.CrossProduct(xAxis, zAxis);
            yAxis.Normalize();
            //get original point, first point
            m_origin = new Vector4(m_points.get_Item(0));

            return new Matrix4(xAxis, yAxis, zAxis, m_origin);
        }

        /// <summary>
        /// calculate the matrix use to scale
        /// </summary>
        /// <returns>maxtrix is use to scale the profile</returns>
        public Matrix4 GetScaleMatrix() 
        {
            double xScale = 384 / (m_boundPoints[1].X - m_boundPoints[0].X);
            double yScale = 275 / (m_boundPoints[1].Y - m_boundPoints[0].Y);
            double factor = xScale <= yScale ? xScale : yScale;
            return new Matrix4((double)(factor * 0.85));
        }

        /// <summary>
        /// Get a matrix which can move points to center
        /// </summary>
        /// <returns>matrix used to move point to center of graphics</returns>
        public Matrix4 GetMoveToCenterMatrix()
        {
            //translate the origin to bound center
            XYZ[] bounds = GetBoundsPoints();
            XYZ min = bounds[0];
            XYZ max = bounds[1];
            XYZ center = new XYZ((min.X + max.X) / 2, (min.Y + max.Y) / 2, 0);
            return new Matrix4(new Vector4(center.X, center.Y, 0));
        }

        /// <summary>
        /// calculate the matrix used to transform 3D to 2D
        /// </summary>
        /// <returns>maxtrix is use to transform 3d points to 2d</returns>
        public Matrix4 Get3DTo2DMatrix()
        {
            Matrix4 result = Matrix4.Multiply(
                m_to2DMatrix.Inverse(), m_moveToCenterMatrix.Inverse());
            result = Matrix4.Multiply(result, m_scaleMatrix);
            return Matrix4.Multiply(result, new Matrix4(new Vector4(192, 137, 0)));
        }

        /// <summary>
        /// calculate the matrix used to transform 2D to 3D
        /// </summary>
        /// <returns>maxtrix is use to transform 2d points to 3d</returns>
        public Matrix4 Get2DTo3DMatrix()
        {
            Matrix4 matrix = Matrix4.Multiply(
                new Matrix4(new Vector4(-192, -137, 0)), m_scaleMatrix.Inverse());
            matrix = Matrix4.Multiply(matrix, m_moveToCenterMatrix);
            return Matrix4.Multiply(matrix, m_to2DMatrix);
        }

        /// <summary>
        /// calculate the matrix used to transform 2d points (on pictureBox) to the plane of truss
        /// which use to set profile
        /// </summary>
        /// <returns>maxtrix is use to transform 2d points to the plane of truss</returns>
        public Matrix4 Get2DToTrussProfileMatrix()
        {
            Matrix4 matrix = Matrix4.Multiply(
                new Matrix4(new Vector4(-192, -137, 0)), m_scaleMatrix.Inverse());
            return Matrix4.Multiply(matrix, m_moveToCenterMatrix);
            ////downward in picturebox, so rotate upward here, y = -y
            //Matrix4 upward =  new Matrix4(new Vector4(new XYZ(1, 0, 0)),
            //    new Vector4(new XYZ(0, -1, 0)), new Vector4(new XYZ(0, 0, 1)));
            //return Matrix4.Multiply(matrix, upward);
        }


        /// <summary>
        /// Get max and min coordinates of all points
        /// </summary>
        /// <returns>points array stores the bound of all points</returns>
        public XYZ[] GetBoundsPoints()
        {
            Matrix4 matrix = m_to2DMatrix;
            Matrix4 inverseMatrix = matrix.Inverse();
            double minX = 0, maxX = 0, minY = 0, maxY = 0;
            bool bFirstPoint = true;

            //get the max and min point on the face
            foreach (XYZ point in m_points)
            {
                Vector4 v = new Vector4(point);
                Vector4 v1 = inverseMatrix.Transform(v);

                if (bFirstPoint)
                {
                    minX = maxX = v1.X;
                    minY = maxY = v1.Y;
                    bFirstPoint = false;
                }
                else
                {
                    if (v1.X < minX) { minX = v1.X; }
                    else if (v1.X > maxX) { maxX = v1.X; }

                    if (v1.Y < minY) { minY = v1.Y; }
                    else if (v1.Y > maxY) { maxY = v1.Y; }
                }
            }
            //return an array with max and min value of all points
            XYZ[] resultPoints = new XYZ[2] { 
                new XYZ(minX, minY, 0), new XYZ(maxX, maxY, 0) };
            return resultPoints;
        }

        /// <summary>
        /// draw profile of truss in pictureBox
        /// </summary>
        /// <param name="graphics">form graphic</param>
        /// <param name="pen">pen used to draw line in pictureBox</param>
        public void Draw2D(Graphics graphics, Pen pen)
        {
            //draw truss curves
            for (int i = 0; i < m_points.Size - 1; i += 2)
            {
                XYZ point1 = m_points.get_Item(i);
                XYZ point2 = m_points.get_Item(i + 1);

                Vector4 v1 = new Vector4(point1);
                Vector4 v2 = new Vector4(point2);

                v1 = m_transformMatrix.Transform(v1);
                v2 = m_transformMatrix.Transform(v2);
                graphics.DrawLine(pen, new Point((int)v1.X, (int)v1.Y),
                    new Point((int)v2.X, (int)v2.Y));
            }
            //draw selected beam (line) by red pen
            DrawSelectedLineRed(graphics);

            //draw top chord and bottom chord
            m_topChord.Draw2D(graphics, Pens.Red);
            m_bottomChord.Draw2D(graphics, Pens.Black);
        }

        /// <summary>
        /// Set profile of truss
        /// </summary>
        /// <param name="commandData">object which contains reference of Revit Application</param>
        public void SetProfile(ExternalCommandData commandData)
        {
            if (m_topChord.Points.Count < 2)
            { MessageBox.Show("Haven't drawn top chord", "Truss API"); return; }
            else if (m_bottomChord.Points.Count < 2)
            { MessageBox.Show("Haven't drawn bottom chord", "Truss API"); return; }

            Autodesk.Revit.Creation.Document createDoc = commandData.Application.ActiveDocument.Create;
            Autodesk.Revit.Creation.Application createApp = commandData.Application.Create;
            CurveArray curvesTop = createApp.NewCurveArray();
            CurveArray curvesBottom = createApp.NewCurveArray();
            //get coordinates of top (bottom) chord from lineTool
            GetChordPoints(m_topChord, curvesTop, createApp);
            GetChordPoints(m_bottomChord, curvesBottom, createApp);
            try
            {
                //set profile by top curve and bottom curve drawn by user in picture box
                m_truss.SetProfile(curvesTop, curvesBottom);
            }
            catch (Exception ex)
            { 
                MessageBox.Show(ex.Message, "Truss API");
            }

            //re-calculate geometry info after truss profile changed
            GetTrussGeometryInfo();
            ClearChords();
        }

        private void GetChordPoints(LineTool chord, CurveArray curves, Autodesk.Revit.Creation.Application createApp)
        {            
            //get coordinates of top chord from lineTool
            for (int i = 0; i < chord.Points.Count - 1; i++)
            {
                Point point = (Point)chord.Points[i];
                Point point2 = (Point)chord.Points[i + 1];

                XYZ xyz = new XYZ(point.X, point.Y, 0);
                XYZ xyz2 = new XYZ(point2.X, point2.Y, 0);

                Vector4 v1 = new Vector4(xyz);
                Vector4 v2 = new Vector4(xyz2);

                v1 = m_restoreMatrix.Transform(v1);
                v2 = m_restoreMatrix.Transform(v2);

                try
                {
                    Line line = createApp.NewLineBound(
                        new XYZ(v1.X, v1.Y, v1.Z), new XYZ(v2.X, v2.Y, v2.Z));
                    curves.Append(line);
                }
                catch (System.ArgumentException)
                {
                    MessageBox.Show(
                        "The start point and the end point of the line are too close, please re-draw it.");
                    ClearChords();
                }
            }
        }

        /// <summary>
        /// restores truss profile to original
        /// </summary>
        public void RemoveProfile() 
        {
            m_truss.RemoveProfile();
            GetTrussGeometryInfo();
            ClearChords();
        }

        /// <summary>
        /// add new point to line tool which used to draw top chord
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        public void AddTopChordPoint(int x, int y)
        {
            // doesn't allow to add 2 points near-by
            if (m_topChord.Points.Count > 0)
            {
                Point lastPoint = (Point)m_topChord.Points[m_topChord.Points.Count - 1];
                if (Math.Abs(lastPoint.X - x) < 1 || 
                    Math.Abs(lastPoint.Y - y) < 1)
                {
                    return;
                }
            }

            m_topChord.Points.Add(new Point(x, y));
            
        }

        /// <summary>
        /// add new point to line tool which used to draw bottom chord
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        public void AddBottomChordPoint(int x, int y)
        {
            // doesn't allow to add 2 points near-by
            if (m_topChord.Points.Count > 0)
            {
                Point lastPoint = (Point)m_topChord.Points[m_topChord.Points.Count - 1];
                if (Math.Abs(lastPoint.X - x) < 1 ||
                    Math.Abs(lastPoint.Y - y) < 1)
                {
                    return;
                }
            }

            m_bottomChord.Points.Add(new Point(x, y));
        }

        /// <summary>
        /// add move point to line tool of top chord
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        public void AddTopChordMovePoint(int x, int y)
        {
            m_topChord.MovePoint = new Point(x, y);
            m_bottomChord.MovePoint = Point.Empty;
        }

        /// <summary>
        /// add move point to line tool of bottom chord
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        public void AddBottomChordMovePoint(int x, int y)
        {
            m_bottomChord.MovePoint = new Point(x, y);
            m_topChord.MovePoint = Point.Empty;
        }

        public void ClearMovePoint()
        {
            m_topChord.MovePoint = Point.Empty;
            m_bottomChord.MovePoint = Point.Empty;
        }

        /// <summary>
        /// clear points of top chord and bottom chord
        /// </summary>
        public void ClearChords()
        {
            m_topChord.Points.Clear();
            m_bottomChord.Points.Clear();
        }

        /// <summary>
        /// Create GraphicsPath object for each curves of truss
        /// </summary>
        public void CreateGraphicsPath()
        {
            m_graphicsPaths.Clear();
            //create path for all the curves of Truss
            for (int i = 0; i < m_points.Size - 1; i += 2)
            {
                XYZ point1 = m_points.get_Item(i);
                XYZ point2 = m_points.get_Item(i + 1);

                Vector4 v1 = new Vector4(point1);
                Vector4 v2 = new Vector4(point2);

                v1 = m_transformMatrix.Transform(v1);
                v2 = m_transformMatrix.Transform(v2);

                GraphicsPath path = new GraphicsPath();
                path.AddLine(new Point((int)v1.X, (int)v1.Y), new Point((int)v2.X, (int)v2.Y));
                m_graphicsPaths.Add(path);
            }
        }

        /// <summary>
        /// Judge which truss member has been selected via location of mouse
        /// </summary>
        /// <param name="x">X coordinate of mouse location</param>
        /// <param name="y">Y coordinate of mouse location</param>
        /// <returns>index of selected member</returns>
        public int SelectTrussMember(int x, int y)
        {
            Point point = new Point(x, y);
            for (int i = 0; i < m_graphicsPaths.Count; i++)
            {
                GraphicsPath path = (GraphicsPath)m_graphicsPaths[i];
                if (path.IsOutlineVisible(point, Pens.Blue))
                {
                    m_selectMemberIndex = i;
                    return m_selectMemberIndex;
                }
            }
            m_selectMemberIndex = -1;
            return m_selectMemberIndex;
        }

        /// <summary>
        /// Draw selected line (beam) by red pen
        /// </summary>
        /// <param name="graphics">graphics of picture box</param>
        public void DrawSelectedLineRed(Graphics graphics)
        {
            Pen redPen = new Pen(System.Drawing.Color.Red, (float)2.0);
            //draw the selected beam as red line
            if (m_selectMemberIndex != -1)
            {
                GraphicsPath selectPath = (GraphicsPath)(m_graphicsPaths[m_selectMemberIndex]);
                PointF startPointOfSelectedLine = (PointF)(selectPath.PathPoints.GetValue(0));
                PointF endPointOfSelectedLine = (PointF)(selectPath.PathPoints.GetValue(1));
                graphics.DrawLine(redPen, startPointOfSelectedLine, endPointOfSelectedLine);
            }
            //draw clicked beam red
            if (m_clickMemberIndex != -1)
            {
                GraphicsPath selectPath = (GraphicsPath)(m_graphicsPaths[m_clickMemberIndex]);
                PointF startPointOfSelectedLine = (PointF)(selectPath.PathPoints.GetValue(0));
                PointF endPointOfSelectedLine = (PointF)(selectPath.PathPoints.GetValue(1));
                graphics.DrawLine(redPen, startPointOfSelectedLine, endPointOfSelectedLine);
            }
        }

        /// <summary>
        /// Get selected beam (truss member) by select index
        /// </summary>
        /// <param name="commandData">object which contains reference of Revit Application</param>
        /// <returns>index of selected member</returns>
        public FamilyInstance GetSelectedBeam(ExternalCommandData commandData)
        {
            m_clickMemberIndex = m_selectMemberIndex;
            ElementId id = new ElementId();
            ElementIdSet idSet = m_truss.Members;
            IEnumerator iter = idSet.GetEnumerator();
            iter.Reset();
            int i = 0;
            while (iter.MoveNext())
            {
                if (i == m_selectMemberIndex)
                {
                    id = (ElementId)iter.Current;
                    break;
                }
                i++;
            }
            return (FamilyInstance)commandData.Application.ActiveDocument.get_Element(ref id);
        }

        /// <summary>
        /// Reset index and clear line tool
        /// </summary>
        public void Reset()
        {
            m_clickMemberIndex = -1;
            m_selectMemberIndex = -1;
            m_topChord.Points.Clear();
            m_bottomChord.Points.Clear();
        }
    }
}
