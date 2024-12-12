//
// (C) Copyright 2003-2007 by Autodesk, Inc.
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
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Autodesk.Revit.Geometry;

namespace Revit.SDK.Samples.FamilyExplorer.CS
{
    /// <summary>
    /// the user interface class.
    /// </summary>
    public partial class FamilyExplorerForm : Form
    {
        private FamilyMgr m_Families;   // stores all families in the current project.
        private double m_defaultScale = 1.2;  // the default scale
        private double m_defaultTranslate;    // the default translation distance.
        private double m_defaultAngle = Math.PI / 12;   // the default rotate angle

        private int m_mouseX;   // the x of mouse location.
        private int m_mouseY;    // the y of mouse location.
        private bool m_isRotate;    // the flag show is ready to rotate.
        private bool m_isTranslate;    // the flag show is ready to translate.

        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="families">the wrapper of all family profiles</param>
        public FamilyExplorerForm(FamilyMgr families)
        {
            m_Families = families;
            InitializeComponent();
            familiesListBox.DataSource = m_Families.AllFamilies;
            familiesListBox.DisplayMember = "Name";
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            FitLayout();
        }

        /// <summary>
        /// display the selected family.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void familiesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            AutoFit();
        }

        /// <summary>
        /// dynamic view the family wire frame
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void previewPictureBox_KeyDown(object sender, KeyEventArgs e)
        {
            FamilyWireFrame rfa = familiesListBox.SelectedItem as FamilyWireFrame;
            if (null == rfa)
            {
                throw new InvalidOperationException();
            }

            if (rfa.IsEmpty)
            {
                return;
            }

            Graphics graphics = previewPictureBox.CreateGraphics();

            switch (e.KeyCode)
            {
                case Keys.ShiftKey://ready to rotate
                    m_isRotate = true;
                    m_isTranslate = false;
                    this.Cursor = Cursors.Cross;
                    break;
                case Keys.NumPad8:
                    if (e.Alt)//rotate around right direction axis
                    {
                        rfa.Rotate(graphics, 1, m_defaultAngle);
                    }
                    else// move up
                    {
                        XYZ move = new XYZ(0, -m_defaultTranslate, 0);
                        rfa.Translate(graphics, move);
                    }
                    break;
                case Keys.NumPad2:
                    if (e.Alt)//rotate around right direction axis
                    {
                        rfa.Rotate(graphics, 1, -m_defaultAngle);
                    }
                    else // move down
                    {
                        XYZ move = new XYZ(0, m_defaultTranslate, 0);
                        rfa.Translate(graphics, move);
                    }
                    break;
                case Keys.NumPad4:
                    if (e.Alt)//rotate around up direction axis
                    {
                        rfa.Rotate(graphics, 0, -m_defaultAngle);
                    }
                    else//move left
                    {
                        XYZ move = new XYZ(-m_defaultTranslate, 0, 0);
                        rfa.Translate(graphics, move);
                    }
                    break;
                case Keys.NumPad6:
                    if (e.Alt)//rotate around up direction axis
                    {
                        rfa.Rotate(graphics, 0, m_defaultAngle);
                    }
                    else//move right
                    {
                        XYZ move = new XYZ(m_defaultTranslate, 0, 0);
                        rfa.Translate(graphics, move);
                    }
                    break;
                case Keys.Add:
                    if (e.Alt)//rotate around view direction axis
                    {
                        rfa.Rotate(graphics, 2, m_defaultAngle);
                    }
                    else//zoom in
                    {
                        rfa.Zoom(graphics, m_defaultScale);
                    }
                    break;
                case Keys.Subtract:
                    if (e.Alt)//rotate around view direction axis
                    {
                        rfa.Rotate(graphics, 2, -m_defaultAngle);
                    }
                    else//zoom out
                    {
                        rfa.Zoom(graphics, 1 / m_defaultScale);
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// stop dynamic view by mouse
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void previewPictureBox_KeyUp(object sender, KeyEventArgs e)
        {
            m_isRotate = false;
            this.Cursor = Cursors.IBeam;
        }

        /// <summary>
        /// change view window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FamilyExplorerForm_SizeChanged(object sender, EventArgs e)
        {
            FitLayout();
            AutoFit();
        }

        /// <summary>
        ///  Justify the graphics such as Move, Rotate, Zoom when mouse move
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void previewPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (!previewPictureBox.Focused)
                {
                    this.Cursor = Cursors.Default;
                    return;
                }

                if (!(m_isTranslate | m_isRotate))
                {
                    return;
                }

                // get the family to preview
                FamilyWireFrame rfa = familiesListBox.SelectedItem as FamilyWireFrame;
                if (null == rfa)
                {
                    throw new InvalidOperationException();
                }

                if (rfa.IsEmpty)
                {
                    return;
                }
                //data assert finished

                Graphics graphics = previewPictureBox.CreateGraphics();
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;

                int xMoved = e.X - m_mouseX;
                int yMoved = e.Y - m_mouseY;
                if (0 == xMoved && 0 == yMoved)
                {
                    m_isRotate = false;
                    return;
                }

                if (m_isTranslate)// translate the family
                {
                    double x = Convert.ToDouble(xMoved);
                    double y = Convert.ToDouble(yMoved);
                    XYZ move = new XYZ(x, y, 0);
                    rfa.Translate(graphics, move);
                    m_mouseX = e.X;
                    m_mouseY = e.Y;
                }
                else if (m_isRotate && e.Button == MouseButtons.Left)//rotate the family
                {
                    if (Math.Abs(xMoved) > Math.Abs(yMoved))
                    {
                        rfa.Rotate(graphics, 0, m_defaultAngle * (xMoved / Math.Abs(xMoved)));
                    }
                    else
                    {
                        rfa.Rotate(graphics, 1, m_defaultAngle * (yMoved / Math.Abs(yMoved)));
                    }

                    m_isRotate = false;
                }
            }
            catch (Exception /*ex*/)
            {
                AutoFit();
            }
        }

        /// <summary>
        /// Set cursor and preserve position of the mouse
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void previewPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (!previewPictureBox.Focused)
            {
                this.Cursor = Cursors.Default;
                return;
            }

            //flag set to true
            m_isTranslate = true;
            this.Cursor = Cursors.SizeAll;
            //record the location of mouse
            m_mouseX = e.X;
            m_mouseY = e.Y;
        }

        /// <summary>
        /// stop change view the family wire frame .
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void previewPictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            m_isTranslate = false;
            this.Cursor = Cursors.IBeam;
        }

        /// <summary>
        /// Occurs when the mouse wheel moves while the control has focus.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void previewPictureBox_MouseWheel(object sender, MouseEventArgs e)
        {
            Graphics graphics = previewPictureBox.CreateGraphics();
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;

            FamilyWireFrame rfa = familiesListBox.SelectedItem as FamilyWireFrame;
            if (null != rfa && !rfa.IsEmpty)
            {
                if (0 < e.Delta)
                {
                    rfa.Zoom(graphics, 1 / m_defaultScale);
                }
                else if (0 > e.Delta)
                {
                    rfa.Zoom(graphics, m_defaultScale);
                }

            }
        }

        /// <summary>
        /// Occurs when the mouse pointer leaves the control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void previewPictureBox_MouseLeave(object sender, EventArgs e)
        {
            familiesListBox.Focus();
            this.Cursor = Cursors.Default;
        }

        /// <summary>
        /// Occurs when the mouse pointer rests on the control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void previewPictureBox_MouseHover(object sender, EventArgs e)
        {
            previewPictureBox.Focus();
            this.Cursor = Cursors.IBeam;
        }

        /// <summary>
        /// auto fit the family wire frame
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void previewPictureBox_DoubleClick(object sender, EventArgs e)
        {
            AutoFit();
        }

        /// <summary>
        /// adjust the layout of the UI
        /// </summary>
        private void FitLayout()
        {
            int xDived = Convert.ToInt32(Width * 0.615);
            previewPictureBox.Width = xDived - 24;
            previewPictureBox.Height = Height - 96;

            familiesListBox.Location = new Point(xDived, 12);
            familiesListBox.Width = Width - xDived - 24;
            familiesListBox.Height = Height - 96;

            m_defaultTranslate = previewPictureBox.Width / 20;
        }

        /// <summary>
        /// auto fit the child controls in the UI
        /// </summary>
        private void AutoFit()
        {
            Graphics graphics = previewPictureBox.CreateGraphics();
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;

            FamilyWireFrame rfa = familiesListBox.SelectedItem as FamilyWireFrame;
            if (null != rfa)
            {
                rfa.AutoFit(graphics, previewPictureBox.Width, previewPictureBox.Height);
            }
        }

        /// <summary>
        /// Paint the family profile and make it fit the picture box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void previewPictureBox_Paint(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;

            FamilyWireFrame rfa = familiesListBox.SelectedItem as FamilyWireFrame;
            if (null != rfa)
            {
                rfa.AutoFit(graphics, previewPictureBox.Width, previewPictureBox.Height);
            }
        }

        /// <summary>
        /// list all parameters of this family
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void familiesListBox_DoubleClick(object sender, EventArgs e)
        {
            FamilyWireFrame rfa = familiesListBox.SelectedItem as FamilyWireFrame;
            if (null != rfa)
            {
                MessageBox.Show(rfa.Parameters, rfa.Name + " Parameters");
            }
        }
    }
}