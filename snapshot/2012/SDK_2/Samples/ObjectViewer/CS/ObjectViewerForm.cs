//
// (C) Copyright 2003-2011 by Autodesk, Inc.
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


namespace Revit.SDK.Samples.ObjectViewer.CS
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;

    using Autodesk.Revit.DB;

    /// <summary>
    /// a class inherit from Form is used to
    /// display an element and its parameters
    /// </summary>
    public partial class ObjectViewerForm : System.Windows.Forms.Form, IMessageFilter
    {
        private ObjectViewer m_viewer;
        private SortableBindingList<Para> m_paras;
        private Sketch3D m_currentSketch;

        // key down code
        private const int WM_KEYDOWN = 0X0100;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="viewer"></param>
        public ObjectViewerForm(ObjectViewer viewer)
        {
            InitializeComponent();

            m_viewer = viewer;
            m_paras = viewer.Params;
            m_currentSketch = viewer.CurrentSketch3D;

            Application.AddMessageFilter(this);
        }

        /// <summary>
        /// initialize the controller
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ObjectViewerForm_Load(object sender, EventArgs e)
        {
            // detailLevelComboBox
            detailLevelComboBox.SelectedIndex = (int)m_viewer.DetailLevel;

            // viewListBox
            viewListBox.DataSource = m_viewer.AllViews;
            viewListBox.DisplayMember = "Name";
            viewListBox.SelectedItem = m_viewer.CurrentView;

            // physicalModelRadioButton and analyticalModelRadioButton
            physicalModelRadioButton.Checked =
                (m_viewer.DisplayKind == ObjectViewer.DisplayKinds.GeometryModel);
            analyticalModelRadioButton.Checked =
                (m_viewer.DisplayKind == ObjectViewer.DisplayKinds.AnalyticalModel);

            // viewDirectionComboBox
            viewDirectionComboBox.SelectedIndex = (int)Graphics3DData.ViewDirections.IsoMetric;

            // m_currentSketch
            m_currentSketch = m_viewer.CurrentSketch3D;
            m_currentSketch.DisplayBBox = new RectangleF(new PointF(0.0f, 0.0f), previewBox.Size);
            m_currentSketch.UpdateViewEvent += delegate
                    {
                        previewBox.Invalidate();
                    };

            // parametersDataGridView
            parametersDataGridView.DataSource = m_paras;
            parametersDataGridView.Columns[0].Width = parametersDataGridView.Width / 2;
            parametersDataGridView.Columns[0].HeaderText = "Parameter Name";
            parametersDataGridView.Columns[1].Width = parametersDataGridView.Width / 2;
            parametersDataGridView.Columns[1].HeaderText = "Value";
        }

        /// <summary>
        /// Prefilter the message, action zoom, transform, rotate etc.
        /// </summary>
        /// <param name="m">The message captured</param>
        /// <returns>Informs if this message is used. </returns>
        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == WM_KEYDOWN)
            {
                // mouse is out of previewBox
                if (Cursor.Current != Cursors.Cross)
                {
                    return false;
                }
                // deal with key down event
                System.Windows.Forms.Keys k = (System.Windows.Forms.Keys)(int)m.WParam;
                KeyEventArgs e = new KeyEventArgs(k);
                switch (e.KeyCode)
                {
                    case Keys.Left:
                        m_currentSketch.Data3D.RotateY(true);
                        break;
                    case Keys.Right:
                        m_currentSketch.Data3D.RotateY(false);
                        break;
                    case Keys.Up:
                        m_currentSketch.Data3D.RotateX(true);
                        break;
                    case Keys.Down:
                        m_currentSketch.Data3D.RotateX(false);
                        break;
                    case Keys.PageUp:
                        m_currentSketch.Data3D.RotateZ(true);
                        break;
                    case Keys.PageDown:
                        m_currentSketch.Data3D.RotateZ(false);
                        break;
                    case Keys.S:
                        m_currentSketch.MoveY(true);
                        break;
                    case Keys.W:
                        m_currentSketch.MoveY(false);
                        break;
                    case Keys.A:
                        m_currentSketch.MoveX(true);
                        break;
                    case Keys.D:
                        m_currentSketch.MoveX(false);
                        break;
                    case Keys.Home:
                        m_currentSketch.Zoom(true);
                        break;
                    case Keys.End:
                        m_currentSketch.Zoom(false);
                        break;
                    case Keys.Escape:
                        this.Close();
                        break;
                    default:
                        return false;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// remove message map
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ObjectViewerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.RemoveMessageFilter(this);
        }

        /// <summary>
        /// previewBox redraw
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void previewBox_Paint(object sender, PaintEventArgs e)
        {
            m_currentSketch.Draw(e.Graphics);
        }

        /// <summary>
        /// display physicalModel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void physicalModelRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (physicalModelRadioButton.Checked)
            {
                m_viewer.DisplayKind = ObjectViewer.DisplayKinds.GeometryModel;
            }
        }

        /// <summary>
        /// display analyticalModel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void analyticalModelRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (analyticalModelRadioButton.Checked)
            {
                m_viewer.DisplayKind = ObjectViewer.DisplayKinds.AnalyticalModel;
            }
        }

        /// <summary>
        /// Change the View to show the element
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void viewListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(!viewListBox.Focused)
            {
                return;
            }
            m_viewer.CurrentView = viewListBox.SelectedItem as Autodesk.Revit.DB.View;
            detailLevelComboBox.SelectedIndex = (int)m_viewer.DetailLevel;
        }

        /// <summary>
        /// Change the camera direction to the view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void viewDirectionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_currentSketch.Data3D.SetViewDirection(
                (Graphics3DData.ViewDirections)viewDirectionComboBox.SelectedIndex);
        }

        /// <summary>
        /// Change the detail level to show the element
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void detailLevelComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(!detailLevelComboBox.Focused)
            {
                return;
            }
            m_viewer.DetailLevel = (DetailLevels)detailLevelComboBox.SelectedIndex;
            viewListBox.SelectedItem = m_viewer.CurrentView;
        }

        /// <summary>
        /// Submit the edition of parameters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OKButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// Cancel all edit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
