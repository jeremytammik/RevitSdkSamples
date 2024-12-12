//
// (C) Copyright 2003-2017 by Autodesk, Inc.
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

namespace Revit.SDK.Samples.NewOpenings.CS
{
    /// <summary>
    /// Stand for the draw tool type
    /// </summary>
    public enum ToolType
    {
        /// <summary>
        /// Draw nothing
        /// </summary>
        None,

        /// <summary>
        /// Draw polygon
        /// </summary>
        Line,

        /// <summary>
        /// Draw rectangle
        /// </summary>
        Rectangle,

        /// <summary>
        /// Draw circle
        /// </summary>
        Circle,

        /// <summary>
        /// Draw arc
        /// </summary>
        Arc
    }

    /// <summary>
    /// Abstract class use as base class of all draw tool class
    /// </summary>
    public abstract class ITool
    {
        # region members
        /// <summary>
        /// ToolType is enum type indicate draw tools.
        /// </summary>
        protected ToolType m_type;

        /// <summary>
        /// Field used to store points of a line
        /// </summary>
        protected List<Point> m_points = new List<Point>();
        
        /// <summary>
        /// Field used to store lines
        /// </summary>
        protected List<List<Point>> m_lines = new List<List<Point>>(); 

        /// <summary>
        /// Background pen used to erase the preview line
        /// </summary>
        protected Pen m_backGroundPen;

        /// <summary>
        /// Foreground pen used to draw lines
        /// </summary>
        protected Pen m_foreGroundPen;
  
        /// <summary>
        /// Store the mouse position when mouse move in pictureBox
        /// </summary>        
        protected Point m_preMovePoint;
        #endregion

        /// <summary>
        /// Default constructor
        /// </summary>
        public ITool()
        {
            m_backGroundPen = Pens.White;
            m_foreGroundPen = Pens.Red;
        }

        /// <summary>
        /// Get all lines drawn in pictureBox
        /// </summary>
        public List<List<Point>> GetLines()
        {
            return m_lines;
        }

        /// <summary>
        /// Get the tool type
        /// </summary>
        public virtual ToolType ToolType
        {
            get
            {
                return m_type;
            }
        }

        /// <summary>
        /// Right mouse click event handler  
        /// </summary>
        /// <param name="graphic">Graphics object, used to draw geometry</param>
        /// <param name="e">Mouse event argument</param>
        public virtual void OnRightMouseClick(Graphics graphic, MouseEventArgs e) { }

        /// <summary>
        /// Mouse move event handler
        /// </summary>
        /// <param name="graphic">Graphics object, used to draw geometry</param>
        /// <param name="e">Mouse event argument</param>
        public virtual void OnMouseMove(Graphics graphic, MouseEventArgs e) { }

        /// <summary>
        /// Mouse down event handler
        /// </summary>
        /// <param name="graphic">Graphics object, used to draw geometry</param>
        /// <param name="e">Mouse event argument</param>
        public virtual void OnMouseDown(Graphics graphic, MouseEventArgs e) { }

        /// <summary>
        /// Mouse up event handler
        /// </summary>
        /// <param name="graphic">Graphics object, used to draw geometry</param>
        /// <param name="e">Mouse event argument</param>
        public virtual void OnMouseUp(Graphics graphic, MouseEventArgs e) { }

        /// <summary>
        /// Mouse middle key down event handler
        /// </summary>
        /// <param name="graphic">Graphics object, used to draw geometry</param>
        /// <param name="e">Mouse event argument</param>
        public virtual void OnMidMouseDown(Graphics graphic, MouseEventArgs e)
        {
            this.m_points.Clear();
        }

        /// <summary>
        /// Draw geometries contained in the tool. which class derived from this class
        /// must implement this abstract method
        /// </summary>
        /// <param name="graphic">Graphics object, used to draw geometry</param>
        public abstract void Draw(Graphics graphic);
    }
}
