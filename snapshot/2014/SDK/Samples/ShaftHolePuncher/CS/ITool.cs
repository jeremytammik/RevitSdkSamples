//
// (C) Copyright 2003-2013 by Autodesk, Inc.
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
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace Revit.SDK.Samples.ShaftHolePuncher.CS
{
    /// <summary>
    /// Abstract class used as a base class of all drawing tool class
    /// </summary>
    public abstract class ITool
    {
        # region members
        protected List<Point> m_points = new List<Point>();  // Field used to store points of a line
        protected Pen m_backGroundPen;  // background pen used to Erase the preview line
        protected Pen m_foreGroundPen;  // foreground pen used to draw lines
        protected Point m_preMovePoint;  // store the mouse position when mouse move in pictureBox
        protected Point m_preDownPoint; // store the mouse position when right mouse button clicked in pictureBox
        protected bool m_finished;    // indicate whether user have finished drawing
        #endregion

        /// <summary>
        /// Finished property to define whether curve was finished
        /// </summary>
        public virtual bool Finished
        {
            get
            {
                return m_finished;
            }
            set
            {
                m_finished = value;
            }
        }

        /// <summary>
        /// get all lines drawn in pictureBox
        /// </summary>
        public virtual List<Point> Points
        {
            get
            {
                return m_points;
            }
        }

        /// <summary>
        /// default constructor
        /// </summary>
        public ITool()
        {
            m_backGroundPen = new Pen(System.Drawing.Color.White);
            m_backGroundPen.Width *= 2;
            m_foreGroundPen = new Pen(System.Drawing.Color.Black);
            m_foreGroundPen.Width *= 2;
            m_finished = false;
        }

        /// <summary>
        /// calculate the distance between two points
        /// </summary>
        /// <param name="p1">first point</param>
        /// <param name="p2">second point</param>
        /// <returns>distance between two points</returns>
        public double GetDistance(Point p1, Point p2)
        {
            double distance = Math.Sqrt(
                (p2.X - p1.X) * (p2.X - p1.X) + (p2.Y - p1.Y) * (p2.Y - p1.Y));
            return distance;
        }

        /// <summary>
        /// clear all the points in the tool
        /// </summary>
        public virtual void Clear() 
        {
            m_points.Clear();
        }

        /// <summary>
        /// draw a line from end point to the location where mouse moved
        /// </summary>
        /// <param name="graphic">Graphics object,used to draw geometry</param>
        /// <param name="e">mouse event args</param>
        public virtual void OnMouseMove(System.Drawing.Graphics graphic, 
            System.Windows.Forms.MouseEventArgs e) { }

        /// <summary>
        /// record the location point where mouse clicked
        /// </summary>
        /// <param name="e">mouse event args</param>
        public virtual void OnMouseDown(System.Windows.Forms.MouseEventArgs e) { }

        /// <summary>
        /// draw the stored lines
        /// </summary>
        /// <param name="graphic">Graphics object, used to draw geometry</param>
        public virtual void Draw(Graphics graphic) { }
    }
}
