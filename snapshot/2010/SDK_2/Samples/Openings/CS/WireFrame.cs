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
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.ObjectModel;

namespace Revit.SDK.Samples.Openings.CS
{
    /// <summary>
    /// WireFrame class for generate the model lines and fit the picture box's size to display
    /// </summary>
    public class WireFrame : ObjectSketch
    {
        /// <summary>
        /// ratio of margin to canvas width
        /// </summary>
        private const float MARGINRATIO = 0.1f;

        //construct function
        /// <summary>
        /// The default constructor
        /// </summary>
        /// <param name="line3Ds">a list contain all the line in WireFrame</param>
        public WireFrame(ReadOnlyCollection<Line3D> line3Ds)
        {
            Frame3DTo2D(line3Ds);
        }

        /// <summary>
        /// draw the line contain in m_lines in 2d Preview
        /// </summary>
        /// <param name="previewWidth">Width of Preview</param>
        /// <param name="previewHeigh">Heigh of Preview</param>
        /// /// <param name="graphics">Graphics to draw</param>
        /// <returns></returns>
        public void Draw2D(float previewWidth, float previewHeigh, Graphics graphics)
        {
            graphics.Clear(Color.Black);
            CalculateTransform(previewWidth, previewHeigh);
            foreach (ObjectSketch sketch in m_objects)
            {
                sketch.Draw(graphics, m_transform);
            }
        }

        /// <summary>
        /// draw override method
        /// </summary>
        /// <param name="g">graphics object</param>
        /// <param name="translate">matrix use to transform points or vectors</param>
        public override void Draw(Graphics g, Matrix translate)
        {
        }

        /// <summary>
        /// calculate the transform between canvas and geometry objects
        /// </summary>
        private void CalculateTransform(float previewWidth, float previewHeigh)
        {
            PointF[] plgpts = CalculateCanvasRegion(previewWidth, previewHeigh);
            m_transform = new Matrix(BoundingBox, plgpts);
        }

        /// <summary>
        /// get the display region, adjust the proportion and location
        /// </summary>
        /// <returns>upper-left, upper-right, and lower-left corners of the rectangle </returns>
        private PointF[] CalculateCanvasRegion(float previewWidth, float previewHeigh)
        {
            // get the area without margin
            float realWidth = previewWidth * (1 - 2 * MARGINRATIO);
            float realHeight = previewHeigh * (1 - 2 * MARGINRATIO);
            float minX = previewWidth * MARGINRATIO;
            float minY = previewHeigh * MARGINRATIO;
            // ratio of width to height
            float originRate = m_boundingBox.Width / m_boundingBox.Height;
            float displayRate = realWidth / realHeight;

            if (originRate > displayRate)
            {
                // display area in canvas need move to center in height
                float goalHeight = realWidth / originRate;
                minY = minY + (realHeight - goalHeight) / 2;
                realHeight = goalHeight;
            }
            else
            {
                // display area in canvas need move to center in width
                float goalWidth = realHeight * originRate;
                minX = minX + (realWidth - goalWidth) / 2;
                realWidth = goalWidth;
            }

            PointF[] plgpts = new PointF[3];
            plgpts[0] = new PointF(minX, realHeight + minY);                // upper-left point    
            plgpts[1] = new PointF(realWidth + minX, realHeight + minY);    // upper-right point
            plgpts[2] = new PointF(minX, minY);                                // lower-left point

            return plgpts;
        }

        /// <summary>
        /// transform 3d point to 2d (if all points in the same plane)
        /// </summary>
        private void Frame3DTo2D(ReadOnlyCollection<Line3D> line3Ds)
        {
            const double LengthEpsilon = 0.01;
            const double AngleEpsilon = 0.1;
            // find 3 points to form 2 lines whose length is bigger than LengthEpsilon 
            // and angle between them should be bigger than AngleEpsilon 
            Line3D line0 = line3Ds[0];
            Vector vector0 = new Vector();
            Vector vector1 = new Vector();
            // to find the first 2 points to form first line
            int index = 0;
            for (int i = 1; i < line3Ds.Count; i++)
            {
                vector0 = line3Ds[i].StartPoint - line0.StartPoint;
                if (vector0.GetLength() > LengthEpsilon)
                {
                    index = i;
                    break;
                }
            }
            if (index == 0)
            {
                return;
            }
            // to find the last points to form the second line
            for (int j = index + 1; j < line3Ds.Count; j++)
            {
                vector1 = line3Ds[j].StartPoint - line3Ds[index].StartPoint;
                double angle = Vector.GetAngleOf2Vectors(vector0, vector1, true);
                if (vector1.GetLength() > LengthEpsilon && angle > AngleEpsilon)
                {
                    break;
                }
            }

            // find the local coordinate system in which the profile of opening is horizontal
            Vector zAxis = (vector0 & vector1).GetNormal();
            Vector xAxis = zAxis & (new Vector(0.0, 1.0, 0.0));
            Vector yAxis = zAxis & xAxis;
            Vector origin = new Vector(0.0, 0.0, 0.0);
            UCS ucs = new UCS(origin, xAxis, yAxis);

            // transform all the 3D lines to UCS and create accordingly 2D lines
            bool isFirst = true;
            foreach (Line3D line in line3Ds)
            {
                Line3D tmp = ucs.GC2LC(line);
                PointF startPnt = new PointF((float)tmp.StartPoint.X, (float)tmp.StartPoint.Y);
                PointF endPnt = new PointF((float)tmp.EndPoint.X, (float)tmp.EndPoint.Y);
                Line2D line2D = new Line2D(startPnt, endPnt);
                LineSketch aLineSketch = new LineSketch(line2D);
                if (isFirst)
                {
                    m_boundingBox = aLineSketch.BoundingBox;
                    isFirst = false;
                }
                else
                {
                    m_boundingBox = RectangleF.Union(m_boundingBox, aLineSketch.BoundingBox);
                }
                m_objects.Add(aLineSketch);
            }
        }
    }
}