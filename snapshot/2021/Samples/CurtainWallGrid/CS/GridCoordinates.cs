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

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.CurtainWallGrid.CS
{
    /// <summary>
    /// Maintain the matrixes needed by 3D & 2D operations: pan, zoom, 2D->3D, 3D->2D
    /// </summary>
    public class GridCoordinates
    {
        #region Fields
        // scale the size of the image to let it shown in the canvas
        private float m_scaleFactor = 0.85f;

        // the document of this sample
        private MyDocument m_myDocument;

        // stores the current GridDrawing data
        private GridDrawing m_drawing;

        // stores the client rectangle of the canvas of the curtain grid
        // will be used in the scale matrix and move-to-center matrix
        private System.Drawing.Rectangle m_boundary;

        // stores the midpoint of the client rectangle 
        // will be used in the scale matrix and move-to-center matrix
        private System.Drawing.Point m_center;

        // store the Matrix used to transform 3D points to 2D
        Matrix4 m_to2DMatrix = null;

        // store the Matrix used to move points to center
        Matrix4 m_moveToCenterMatrix = null;

        // store the Matrix used to scale profile fit to pictureBox
        Matrix4 m_scaleMatrix = null;

        // store the Matrix used to transform Revit coordinate to window UI
        Matrix4 m_transformMatrix = null;

        // store the Matrix used to transform window UI coordinate to Revit
        Matrix4 m_restoreMatrix = null;

        // stores the boundary of the curtain grid
        List<PointF> m_boundPoints;

        #endregion

        #region Properties
        /// <summary>
        /// stores the GridDrawing data used in the current dialog
        /// </summary>
        public GridDrawing Drawing
        {
            get
            {
                return m_drawing;
            }
            set
            {
                m_drawing = value;
            }
        }

        /// <summary>
        /// store the Matrix used to transform 3D points to 2D
        /// </summary>
        public Matrix4 To2DMatrix
        {
            get
            {
                return m_to2DMatrix;
            }
        }

        /// <summary>
        /// store the Matrix used to transform Revit coordinate to window UI
        /// </summary>
        public Matrix4 TransformMatrix
        {
            get
            {
                return m_transformMatrix;
            }
        }

        /// <summary>
        /// store the Matrix used to transform window UI coordinate to Revit
        /// </summary>
        public Matrix4 RestoreMatrix
        {
            get
            {
                return m_restoreMatrix;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="myDoc">
        /// the document of this sample
        /// </param>
        /// <param name="drawing">
        /// the GridDrawing data used in the dialog
        /// </param>
        public GridCoordinates(MyDocument myDoc, GridDrawing drawing)
        {
            m_myDocument = myDoc;

            if (null == drawing)
            {
                TaskDialog.Show("Revit", "Error! There's no grid information in the curtain wall.");
            }
            m_drawing = drawing;
            drawing.Coordinates = this;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// obtain the matrixes used in this dialog
        /// </summary>
        public void GetMatrix()
        {
            // initialize the class members, obtain the location information of the canvas
            m_boundary = m_drawing.Boundary;
            m_center = m_drawing.Center;

            // Get a matrix which can transform points to 2D
            m_to2DMatrix = GetTo2DMatrix();

            // get the vertexes of the canvas (in Point/PointF format)
            m_boundPoints = GetBoundsPoints();

            // get a matrix which can keep all the points in the center of the canvas
            m_moveToCenterMatrix = GetMoveToCenterMatrix();

            // get a matrix for scaling all the points and lines within the canvas
            m_scaleMatrix = GetScaleMatrix();

            // transform 3D points to 2D
            m_transformMatrix = Get3DTo2DMatrix();

            // transform from 2D to 3D
            m_restoreMatrix = Get2DTo3DMatrix();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// calculate the matrix used to transform 2D to 3D
        /// </summary>
        /// <returns>
        /// matrix used to transform 2d points to 3d
        /// </returns>
        private Matrix4 Get2DTo3DMatrix()
        {
            Matrix4 matrix = Matrix4.Multiply(
                new Matrix4(new Vector4(-m_center.X, -m_center.Y, 0)), m_scaleMatrix.Inverse());
            matrix = Matrix4.Multiply(
                matrix, m_moveToCenterMatrix);
            return Matrix4.Multiply(matrix, m_to2DMatrix);
        }

        /// <summary>
        /// calculate the matrix used to transform 3D to 2D
        /// </summary>
        /// <returns>
        /// matrix used to transform 3d points to 2d
        /// </returns>
        private Matrix4 Get3DTo2DMatrix()
        {
            Matrix4 result = Matrix4.Multiply(
               m_to2DMatrix.Inverse(), m_moveToCenterMatrix.Inverse());
            result = Matrix4.Multiply(result, m_scaleMatrix);
            return Matrix4.Multiply(result, new Matrix4(new Vector4(m_center.X, m_center.Y, 0)));
        }

        /// <summary>
        /// calculate the matrix used to scale
        /// </summary>
        /// <returns>
        /// matrix used to scale the to-be-painted image
        /// </returns>
        private Matrix4 GetScaleMatrix()
        {
            float xScale = m_boundary.Width / (m_boundPoints[1].X - m_boundPoints[0].X);
            float yScale = m_boundary.Height / (m_boundPoints[1].Y - m_boundPoints[0].Y);
            float factor = xScale <= yScale ? xScale : yScale;
            return new Matrix4((float)(factor * m_scaleFactor));
        }

        /// <summary>
        /// Get a matrix which can move points to center
        /// </summary>
        /// <returns>
        /// matrix used to move point to center of canvas
        /// </returns>
        private Matrix4 GetMoveToCenterMatrix()
        {
            //translate the origin to bound center
            PointF min = m_boundPoints[0];
            PointF max = m_boundPoints[1];
            PointF center = new PointF((min.X + max.X) / 2, (min.Y + max.Y) / 2);
            return new Matrix4(new Vector4(center.X, center.Y, 0));
        }

        /// <summary>
        /// Get max and min coordinates of all points
        /// </summary>
        /// <returns>
        /// points array stores the bounding box of all points
        /// </returns>
        private List<PointF> GetBoundsPoints()
        {
            Matrix4 matrix = m_to2DMatrix;
            Matrix4 inverseMatrix = matrix.Inverse();
            float minX = 0, maxX = 0, minY = 0, maxY = 0;
            bool bFirstPoint = true;
            List<PointF> resultPoints = new List<PointF>();

            //get the max and min point on the face
            foreach (Autodesk.Revit.DB.XYZ point in m_drawing.Geometry.GridVertexesXYZ)
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
            resultPoints.Add(new PointF(minX, minY));
            resultPoints.Add(new PointF(maxX, maxY));
            return resultPoints;
        }

        /// <summary>
        /// Get a matrix which can transform points to 2D
        /// </summary>
        /// <returns>
        /// matrix which can transform points to 2D
        /// </returns>
        private Matrix4 GetTo2DMatrix()
        {
            Autodesk.Revit.DB.XYZ startXYZ = m_myDocument.WallGeometry.StartXYZ;
            Autodesk.Revit.DB.XYZ endXYZ = m_myDocument.WallGeometry.EndXYZ;
            XYZ sub = endXYZ - startXYZ;
            Vector4 xAxis = new Vector4(new Autodesk.Revit.DB.XYZ(sub.X, sub.Y, sub.Z));
            xAxis.Normalize();
            //because in the windows UI, Y axis is downward
            Vector4 yAxis = new Vector4(new Autodesk.Revit.DB.XYZ(0, 0, -1));
            yAxis.Normalize();
            Vector4 zAxis = Vector4.CrossProduct(xAxis, yAxis);
            zAxis.Normalize();
            Vector4 origin = new Vector4(m_drawing.Geometry.GridVertexesXYZ[0]);

            return new Matrix4(xAxis, yAxis, zAxis, origin);
        }
        #endregion
    } // end of class
}
