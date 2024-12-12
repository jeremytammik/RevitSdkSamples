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
using Autodesk.Revit.DB;
using System.Windows.Forms;
using Point = System.Drawing.Point;

namespace Revit.SDK.Samples.NewHostedSweep.CS
{
    /// <summary>
    /// This class is intent to convenience the geometry transformations.
    /// It can produce rotation and scale transformations. 
    /// </summary>
    public class TrackBall
    {
        /// <summary>
        /// Canvas width.
        /// </summary>
        private float m_canvasWidth;

        /// <summary>
        /// Canvas height.
        /// </summary>
        private float m_canvasHeight;

        /// <summary>
        /// Previous position in 2D.
        /// </summary>
        private Point m_previousPosition2D;

        /// <summary>
        /// Previous position in 3D.
        /// </summary>
        private XYZ m_previousPosition3D;

        /// <summary>
        /// Current rotation transform.
        /// </summary>
        private Transform m_rotation = Transform.Identity;

        /// <summary>
        /// Current scale transform.
        /// </summary>
        private double m_scale;

        /// <summary>
        /// Current rotation transform.
        /// </summary>
        public Transform Rotation
        {
            get { return m_rotation; }
            set { m_rotation = value; }
        }

        /// <summary>
        /// Current scale transform.
        /// </summary>
        public double Scale
        {
            get { return m_scale; }
            set { m_scale = value; }
        }

        /// <summary>
        /// Project canvas 2D point to the track ball.
        /// </summary>
        /// <param name="width">Canvas width</param>
        /// <param name="height">Canvas height</param>
        /// <param name="point">2D point</param>
        /// <returns>Projected point in track ball</returns>
        private XYZ ProjectToTrackball(double width, double height, Point point)
        {
            double x = point.X / (width / 2);    // Scale so bounds map to [0,0] - [2,2]
            double y = point.Y / (height / 2);

            x = x - 1;                           // Translate 0,0 to the center
            y = 1 - y;                           // Flip so +Y is up instead of down

            double d, t, z;

            d = Math.Sqrt(x * x + y * y);
            if (d < 0.70710678118654752440)
            {    /* Inside sphere */
                z = Math.Sqrt(1 - d * d);
            }
            else
            {           /* On hyperbola */
                t = 1 / 1.41421356237309504880;
                z = t * t / d;
            }
            return new XYZ(x, y, z);
        }

        /// <summary>
        /// Yield the rotation transform according to current 2D point in canvas.
        /// </summary>
        /// <param name="currentPosition">2D point in canvas</param>
        private void Track(Point currentPosition)
        {
            XYZ currentPosition3D = ProjectToTrackball(
                m_canvasWidth, m_canvasHeight, currentPosition);

            XYZ axis = m_previousPosition3D.CrossProduct(currentPosition3D);
            if (axis.GetLength() == 0) return;

            double angle = m_previousPosition3D.AngleTo(currentPosition3D);
            m_rotation = Transform.CreateRotation(axis, -angle);
            m_previousPosition3D = currentPosition3D;
        }

        /// <summary>
        /// Yield the scale transform according to current 2D point in canvas.
        /// </summary>
        /// <param name="currentPosition">2D point in canvas</param>
        private void Zoom(Point currentPosition)
        {
            double yDelta = currentPosition.Y - m_previousPosition2D.Y;

            double scale = Math.Exp(yDelta / 100);    // e^(yDelta/100) is fairly arbitrary.

            m_scale = scale;
        }

        /// <summary>
        /// Mouse down, initialize the transformation to identity.
        /// </summary>
        /// <param name="width">Canvas width</param>
        /// <param name="height">Canvas height</param>
        /// <param name="e"></param>
        public void OnMouseDown(float width, float height, MouseEventArgs e)
        {
            m_rotation = Transform.Identity;
            m_scale = 1.0;
            m_canvasWidth = width;
            m_canvasHeight = height;
            m_previousPosition2D = e.Location;
            m_previousPosition3D = ProjectToTrackball(m_canvasWidth,
                                                     m_canvasHeight,
                                                     m_previousPosition2D);
        }

        /// <summary>
        /// Mouse move with left button press will yield the rotation transform,
        /// with right button press will yield scale transform.
        /// </summary>
        /// <param name="e"></param>
        public void OnMouseMove(MouseEventArgs e)
        {
            Point currentPosition = e.Location;

            // avoid any zero axis conditions
            if (currentPosition == m_previousPosition2D) return;

            // Prefer tracking to zooming if both buttons are pressed.
            if (e.Button == MouseButtons.Left)
            {
                Track(currentPosition);
            }
            else if (e.Button == MouseButtons.Right)
            {
                Zoom(currentPosition);
            }

            m_previousPosition2D = currentPosition;
        }

        /// <summary>
        /// Arrows key down will also yield the rotation transform.
        /// </summary>
        /// <param name="e"></param>
        public void OnKeyDown(KeyEventArgs e)
        {
            XYZ axis = new XYZ(1.0, 0, 0);
            double angle = 0.1;
            switch (e.KeyCode)
            {
                case Keys.Down: break;
                case Keys.Up:
                    angle = -angle;
                    break;
                case Keys.Left:
                    axis = new XYZ(0, 1.0, 0);
                    angle = -angle;
                    break;
                case Keys.Right:
                    axis = new XYZ(0, 1.0, 0);
                    break;
                default: break;
            }
            m_rotation = Transform.CreateRotation(axis, angle);
        }
    }
}
