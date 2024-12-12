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
using System.IO;

namespace Revit.SDK.Samples.ImportAndExportForDWG.CS
{
    /// <summary>
    /// It contains a dialog which provides the options of common information for export
    /// </summary>
    public partial class ExportForm : Form
    {
        private ExportData m_exportData;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="data"></param>
        public ExportForm(ExportData data)
        {
            InitializeComponent();
            m_exportData = data;
            InitializeControls();
        }

        /// <summary>
        /// Initialize values and status of controls
        /// </summary>
        private void InitializeControls()
        {
            //Directory
            textBoxSaveAs.Text = m_exportData.ExportFolder + "\\" + m_exportData.ExportFileName;
        }

        /// <summary>
        /// Provide the export option dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOptions_Click(object sender, EventArgs e)
        {
            bool contain3DView = false;

            if (radioButtonCurrentView.Checked)
            {
                if (m_exportData.Is3DView)
                {
                    contain3DView = true;
                }                
            }
            else
            {
                if (m_exportData.SelectViewsData.Contain3DView)
                {
                    contain3DView = true;
                }
            }

            using (ExportOptionsForm exportOptionsForm = new ExportOptionsForm(m_exportData.ExportOptionsData,
                contain3DView))
            {
                exportOptionsForm.ShowDialog();
            }
        }

        /// <summary>
        /// Provide the views selecting dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSelectViews_Click(object sender, EventArgs e)
        {
            using (SelectViewsForm selectViewsForm = new SelectViewsForm(m_exportData.SelectViewsData))
            {
                m_exportData.SelectViewsData.SelectedViews.Clear();
                selectViewsForm.ShowDialog();
                if(m_exportData.SelectViewsData.SelectedViews.Size == 0)
                {
                    radioButtonCurrentView.Checked = true;
                }
                else
                {
                    radioButtonCurrentView.Checked = false;
                }
            }
        }

        /// <summary>
        /// Specify a file to export into
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonBrowser_Click(object sender, EventArgs e)
        {
            StringBuilder tmp = new StringBuilder();
            foreach(String version in m_exportData.FileVersion)
            {
                tmp.Append(version + "|*.dwg|");
            }
            String filter = tmp.ToString().TrimEnd('|');

            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.InitialDirectory = m_exportData.ExportFolder;
                saveDialog.FileName = m_exportData.ExportFileName;
                saveDialog.Filter = filter;
                saveDialog.FilterIndex = 1;
                saveDialog.RestoreDirectory = true;

                if (saveDialog.ShowDialog() != DialogResult.Cancel)
                {
                    m_exportData.ExportFileVersion = m_exportData.EnumFileVersion[saveDialog.FilterIndex];
                    textBoxSaveAs.Text = saveDialog.FileName;
                }
            }
        }

        /// <summary>
        /// Change the status of buttonSelectViews according to the selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButtonSelectView_CheckedChanged(object sender, EventArgs e)
        {
            buttonSelectViews.Enabled = radioButtonSelectView.Checked;

            if (radioButtonSelectView.Checked)
            {
                checkBoxMergeViews.Checked = true;
            }
        }

        /// <summary>
        /// Transfer information back to ExportData class and execute EXPORT operation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (ValidateExportFolder())
            {
                m_exportData.ExportMergeFiles = checkBoxMergeViews.Checked;
                m_exportData.CurrentViewOnly = !radioButtonSelectView.Checked;

                try
                {
                    m_exportData.Export();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Export Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
                this.Close();
            }            
        }

        /// <summary>
        /// Check whether the folder specified is valid
        /// </summary>
        /// <returns></returns>
        private Boolean ValidateExportFolder()
        {
            String fileNameFull = textBoxSaveAs.Text;
            //If the textBoxSaveAs is empty
            if (String.IsNullOrEmpty(fileNameFull))
            {
                MessageBox.Show("Please specify the folder and file name!", "Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBoxSaveAs.Focus();
                return false;
            }

            //If has file name
            if(!fileNameFull.Contains("\\"))
            {
                MessageBox.Show("Please specify file name!", "Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBoxSaveAs.Focus();
                return false;
            }

            //Whether the folder exists
            String folder = fileNameFull.Substring(0, fileNameFull.LastIndexOf("\\"));
            if(!Directory.Exists(folder))
            {
                MessageBox.Show("The specified folder does not exist!", "Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBoxSaveAs.Focus();
                return false;
            }

            //Whether file name has extension 
            String fileName = fileNameFull.Substring(fileNameFull.LastIndexOf("\\") + 1);
            if (fileName.EndsWith(".dwg"))
            {
                m_exportData.ExportFileName = fileName.Substring(0, fileName.LastIndexOf(".dwg"));
            }
            else
            {
                m_exportData.ExportFileName = fileName;
            }

            m_exportData.ExportFolder = folder;

            return true;
        }
    }
}