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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Revit.SDK.Samples.InPlaceMembers.CS
{
    /// <summary>
    /// Main form class, use to diaplay the preview picture box and property grid.
    /// </summary>
    public partial class InPlaceMembersForm : System.Windows.Forms.Form,IMessageFilter
    {
        /// <summary>
        /// Properties instance
        /// </summary>
        private Properties m_instanceProperties;

        /// <summary>
        /// Graphics data
        /// </summary>
        private GraphicsData m_graphicsData;

        /// <summary>
        /// window message key number
        /// </summary>
        private const int WM_KEYDOWN = 0X0100;

        /// <summary>
        /// constructor 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="graphicsData"></param>
        public InPlaceMembersForm(Properties p, GraphicsData graphicsData)
        {
            m_instanceProperties = p;
            m_graphicsData = graphicsData;
            Application.AddMessageFilter(this);
            InitializeComponent();
        }

        /// <summary>
        /// load event handle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InPlaceMembersForm_Load(object sender, EventArgs e)
        {
            instancePropertyGrid.SelectedObject = m_instanceProperties;
            modelPictureBox.DataSource = m_graphicsData;
        }

        /// <summary>
        /// implement IMessageFilter interface
        /// </summary>
        /// <param name="m">The message to be dispatched.</param>
        /// <returns>true to filter the message and stop it from being dispatched; 
        /// false to allow the message to continue to the next filter or control.</returns>
        public bool PreFilterMessage(ref Message m)
        {
            if (!modelPictureBox.Focused)
            {
                return false;
            }

            if (m.Msg == WM_KEYDOWN)
            {

                System.Windows.Forms.Keys k = (System.Windows.Forms.Keys)(int)m.WParam;
                KeyEventArgs e = new KeyEventArgs(k);
                switch (e.KeyCode)
                {
                    case Keys.Left:
                        m_graphicsData.RotateY(true);
                        break;
                    case Keys.Right:
                        m_graphicsData.RotateY(false);
                        break;
                    case Keys.Up:
                        m_graphicsData.RotateX(true);
                        break;
                    case Keys.Down:
                        m_graphicsData.RotateX(false);
                        break;
                    case Keys.PageUp:
                        m_graphicsData.RotateZ(true);
                        break;
                    case Keys.PageDown:
                        m_graphicsData.RotateZ(false);
                        break;
                    case Keys.S:
                        modelPictureBox.MoveY(true);
                        break;
                    case Keys.W:
                        modelPictureBox.MoveY(false);
                        break;
                    case Keys.A:
                        modelPictureBox.MoveX(true);
                        break;
                    case Keys.D:
                        modelPictureBox.MoveX(false);
                        break;
                    case Keys.Home:
                        modelPictureBox.Scale(true);
                        break;
                    case Keys.End:
                        modelPictureBox.Scale(false);
                        break;
                    default:
                        break;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// ok button event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OKbutton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Cancel button event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}