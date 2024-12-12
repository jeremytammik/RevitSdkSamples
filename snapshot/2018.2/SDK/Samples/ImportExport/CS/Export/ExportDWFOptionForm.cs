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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Revit.SDK.Samples.ImportExport.CS
{
    /// <summary>
    /// Data class which stores information of lower priority for exporting DWF(x) format.
    /// </summary>
    public partial class ExportDWFOptionForm : System.Windows.Forms.Form
    {
        /// <summary>
        /// ExportDWFData object
        /// </summary>
        private ExportDWFData m_data;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="data">Data class object</param>
        public ExportDWFOptionForm(ExportDWFData data)
        {
            m_data = data;
            InitializeComponent();
            Initialize();
        }

        /// <summary>
        /// Initialize controls
        /// </summary>
        private void Initialize()
        {
            checkBoxModelElements.Checked = m_data.ExportObjectData;
            checkBoxRoomsAndAreas.Checked = m_data.ExportAreas;
            radioButtonStandardFormat.Checked = true;
            radioButtonCompressedFormat.Checked = false;
            labelImageQuality.Enabled = false;
            comboBoxImageQuality.Enabled = false;
            comboBoxImageQuality.DataSource = m_data.ImageQualities;
            checkBoxMergeViews.Enabled = !m_data.CurrentViewOnly;
            checkBoxMergeViews.Checked = !m_data.CurrentViewOnly;
            checkBoxMergeViews.Text = "Combine selected views and sheets into a single file";
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            m_data.ExportObjectData = checkBoxModelElements.Checked;
            m_data.ExportAreas = checkBoxRoomsAndAreas.Enabled ? checkBoxRoomsAndAreas.Checked : false;
            m_data.ExportMergeFiles = checkBoxMergeViews.Checked;
            if (radioButtonStandardFormat.Checked)
            {
                m_data.DwfImageFormat = Autodesk.Revit.DB.DWFImageFormat.Lossless;
                m_data.DwfImageQuality = Autodesk.Revit.DB.DWFImageQuality.Default;
            }
            else
            {
                m_data.DwfImageFormat = Autodesk.Revit.DB.DWFImageFormat.Lossy;
                m_data.DwfImageQuality = m_data.ImageQualities[comboBoxImageQuality.SelectedIndex];
            }
        }

        private void checkBoxModelElements_CheckedChanged(object sender, EventArgs e)
        {
            checkBoxRoomsAndAreas.Enabled = checkBoxModelElements.Checked ? true : false;
        }

        private void radioButtonCompressedFormat_CheckedChanged(object sender, EventArgs e)
        {
            labelImageQuality.Enabled = radioButtonCompressedFormat.Checked;
            comboBoxImageQuality.Enabled = radioButtonCompressedFormat.Checked;
        }
    }
}
