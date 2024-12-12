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
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Diagnostics;

namespace Revit.SDK.Samples.ImportExport.CS
{
    /// <summary>
    /// Provide a dialog which provides the options for exporting dgn format
    /// </summary>
    public partial class ExportDGNOptionsForm : System.Windows.Forms.Form
    {
        /// <summary>
        /// data class
        /// </summary>
        private ExportDGNData m_data;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="data">Data class object</param>
        public ExportDGNOptionsForm(ExportDGNData data)
        {
            m_data = data;
            InitializeComponent();
            InitializeControl();
        }

        /// <summary>
        /// Initialize values and status of controls
        /// </summary>
        private void InitializeControl()
        {
            comboBoxLayerSettings.DataSource = m_data.LayerMapping;
            comboBoxLayerSettings.SelectedIndex = 0;
            comboBoxExportFileFormat.DataSource = m_data.ExportFileVersions;
            comboBoxExportFileFormat.SelectedIndex = 0;
            checkBoxHideReferencePlanes.Checked = false;
            checkBoxHideScopeBoxes.Checked = false;
            checkBoxHideUnrefereceViewTag.Checked = false;
        }

        /// <summary>
        /// OK button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBox2DSeedFile.Text))
            {
                MessageBox.Show("Please specify the 2D seed file.", "Specify 2D seed file",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                button2DSeedFile.Focus();
                return;
            }

            if (!File.Exists(textBox2DSeedFile.Text))
            {
                MessageBox.Show("The 2D seed file does not exist.", "Specify 2D seed file",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                button2DSeedFile.Focus();
                return;
            }

            if (String.IsNullOrEmpty(textBox3DSeedFile.Text))
            {
                MessageBox.Show("Please specify the 3D seed file.", "Specify 3D seed file",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                button3DSeedFile.Focus();
                return;
            }

            if (!File.Exists(textBox3DSeedFile.Text))
            {
                MessageBox.Show("The 3D seed file does not exist.", "Specify 3D seed file",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                button3DSeedFile.Focus();
                return;
            }

            m_data.ExportLayerMapping =
                m_data.EnumLayerMapping[comboBoxLayerSettings.SelectedIndex];
            m_data.ExportFileVersion = m_data.ExportFileVersions[comboBoxExportFileFormat.SelectedIndex];
            m_data.HideScopeBox = checkBoxHideScopeBoxes.Checked;
            m_data.HideReferencePlane = checkBoxHideReferencePlanes.Checked;
            m_data.HideUnreferenceViewTags = checkBoxHideUnrefereceViewTag.Checked;
            this.Close();
        }

        /// <summary>
        /// button2DSeedFile click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2DSeedFile_Click(object sender, EventArgs e)
        {
            String fileName = String.Empty;
            DialogResult result = ShowOpenDialog(ref fileName);
            if (result != DialogResult.Cancel)
            {
                textBox2DSeedFile.Text = fileName;
            }
        }

        /// <summary>
        /// Show open file dialog
        /// </summary>
        /// <param name="returnFileName"></param>
        /// <returns></returns>
        public static DialogResult ShowOpenDialog(ref String returnFileName)
        {
            ProcessModule mainModule = Process.GetCurrentProcess().MainModule;
            String folderRevit = Path.GetDirectoryName(mainModule.FileName);
            String folderACADInterop = Path.Combine(folderRevit, "ACADInterop");
            String initialDirectory = folderRevit;
            if (Directory.Exists(folderACADInterop))
            {
                initialDirectory = folderACADInterop;
            }

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Specify seed file";
                openFileDialog.InitialDirectory = initialDirectory;
                openFileDialog.Filter = "DGN Files |*.dgn";
                openFileDialog.RestoreDirectory = true;

                DialogResult result = openFileDialog.ShowDialog();
                if (result != DialogResult.Cancel)
                {
                    returnFileName = openFileDialog.FileName;
                }

                return result;
            }
        }

        private void button3DSeedFile_Click(object sender, EventArgs e)
        {
            String fileName = String.Empty;
            DialogResult result = ShowOpenDialog(ref fileName);
            if (result != DialogResult.Cancel)
            {
                textBox3DSeedFile.Text = fileName;
            }
        }
    }
}
