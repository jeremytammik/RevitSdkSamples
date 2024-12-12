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
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Text;
using System.Windows.Forms;

using Autodesk.Revit.DB;

namespace Revit.SDK.Samples.NewRoof.RoofForms.CS
{
    /// <summary>
    /// The GraphicsControl is used to display the footprint roof lines with GDI.
    /// </summary>
    public partial class GraphicsControl : UserControl
    {
        // A reference to FootPrintRoofWrapper, It constrains the DrawFootPrint() method to 
        // draw footprint roof lines in the PictureBox control.
        private FootPrintRoofWrapper m_footPrintRoofWrapper;
        // To store a highlight pen to highlight the specified footprint roof line.
        private Pen m_highLightPen;
        // To store a display pen to draw the footprint roof lines.
        private Pen m_displayPen;

        // To store the draw center location of the PictureBox control, it is the origin of the drawing.
        public PointF m_drawCenter;
        // To store a value to decide the scale of the drawing.
        private float m_scale;

        /// <summary>
        /// The private construct
        /// </summary>
        private GraphicsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The construct of the GraphicsControl.
        /// </summary>
        /// <param name="footPrintRoofWrapper">A reference to FootPrintRoofWrapper which will be displayed in 
        /// the picture box control.</param>
        public  GraphicsControl(FootPrintRoofWrapper footPrintRoofWrapper)
        {
            InitializeComponent();
            this.Load += new EventHandler(GraphicsControl_Load);
            
            m_displayPen = new Pen(System.Drawing.Color.Green, 0);
            m_highLightPen = new Pen(System.Drawing.Color.Red, 0);
            m_footPrintRoofWrapper = footPrintRoofWrapper;                      
        }

        /// <summary>
        /// When the GraphicsControl was loaded, then add the picture box control to it 
        /// and initialize the draw center and scale value.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void GraphicsControl_Load(object sender, EventArgs e)
        {
            PictureBox picturebox = new PictureBox();
            picturebox.Dock = DockStyle.Fill;
            this.Controls.Add(picturebox);
            picturebox.Paint += new PaintEventHandler(picturebox_Paint);

            // initialize the draw center and scale value
            m_drawCenter = new PointF(picturebox.Size.Width / 2, picturebox.Size.Height / 2);
            
            Autodesk.Revit.DB.XYZ size = m_footPrintRoofWrapper.Boundingbox.Max - m_footPrintRoofWrapper.Boundingbox.Min;
            float tempscale1 = (float)((0.9 * picturebox.Width) / size.X);
            float tempscale2 = (float)((0.9 * picturebox.Height) / size.Y);

            if (tempscale1 > tempscale2)
            {
                m_scale = tempscale2;
            }
            else
            {
                m_scale = tempscale1;
            }

            // Book the OnFootPrintRoofLineChanged event to refresh the picture box
            m_footPrintRoofWrapper.OnFootPrintRoofLineChanged += new EventHandler(m_footPrintRoofWrapper_OnFootPrintRoofLineChanged);
        }

        /// <summary>
        /// When the current selected FootPrintRoofLine changed in the PropertyGrid, then update the drawing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_footPrintRoofWrapper_OnFootPrintRoofLineChanged(object sender, EventArgs e)
        {
            this.Refresh();
        }

        /// <summary>
        /// Display the footprint roof lines in the picture box control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void picturebox_Paint(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            graphics.Clear(System.Drawing.Color.White);
            graphics.TranslateTransform(m_drawCenter.X, m_drawCenter.Y);
            graphics.ScaleTransform(m_scale, m_scale);
            graphics.PageUnit = GraphicsUnit.Pixel;
            m_footPrintRoofWrapper.DrawFootPrint(graphics, m_displayPen, m_highLightPen);
        }
    }
}
