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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Revit.SDK.Samples.ImportExport.CS
{
    /// <summary>
    /// Provide a dialog which provides the options for exporting dgn format
    /// </summary>
    public partial class ExportDGNOptionsForm : Form
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
            checkBoxEnableTemplateFile.Checked = false;
            textBoxTemplateFile.Enabled = false;
            buttonBrowserTemplate.Enabled = false;
        }

        /// <summary>
        /// Whether to use dgn template file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxEnableTemplateFile_CheckedChanged(object sender, EventArgs e)
        {
            textBoxTemplateFile.Enabled = checkBoxEnableTemplateFile.Checked;
            buttonBrowserTemplate.Enabled = checkBoxEnableTemplateFile.Checked;
        }

        /// <summary>
        /// Select dgn template
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonBrowserTemplate_Click(object sender, EventArgs e)
        {
            String fileName = String.Empty;

            using (OpenFileDialog openDialog = new OpenFileDialog())
            {
                openDialog.CheckPathExists = true;
                openDialog.DefaultExt = ".dgn";
                openDialog.Title = m_data.Title;
                openDialog.InitialDirectory = m_data.ExportFolder;

                DialogResult result = openDialog.ShowDialog();
                if (result != DialogResult.Cancel)
                {
                    textBoxTemplateFile.Text = openDialog.FileName;
                }
            }
        }

        /// <summary>
        /// OK button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOK_Click(object sender, EventArgs e)
        {
            m_data.ExportLayerMapping =
                m_data.EnumLayerMapping[comboBoxLayerSettings.SelectedIndex];

            if (checkBoxEnableTemplateFile.Checked)
            {
                if (ValidateTemplateFile())
                {
                    this.Close();
                }
            }
            else
            {
                this.Close();
            }
        }

        /// <summary>
        /// Check whether the template file is valid
        /// </summary>
        /// <returns></returns>
        private Boolean ValidateTemplateFile()
        {
            String fileNameFull = textBoxTemplateFile.Text;
            //If the textBoxTemplateFile is empty
            if (String.IsNullOrEmpty(fileNameFull))
            {
                MessageBox.Show("Please specify the template file name!", "Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBoxTemplateFile.Focus();
                return false;
            }

            //Whether the template file exists
            if (!File.Exists(fileNameFull))
            {
                MessageBox.Show("The specified template file does not exist!", "Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBoxTemplateFile.Focus();
                return false;
            }

            m_data.TemplateFile = fileNameFull;
            return true;
        }
    }
}