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
using System.IO;

using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.ImportExport.CS
{
    /// <summary>
    /// It contains a dialog which provides the options of common information for export
    /// </summary>
    public partial class ExportWithViewsForm : System.Windows.Forms.Form
    {
        // Data class object of ExportDataWithViews
        private ExportDataWithViews m_exportData;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="data"></param>
        public ExportWithViewsForm(ExportDataWithViews data)
        {
            m_exportData = data;
            InitializeComponent();
            InitializeControls();
        }

        /// <summary>
        /// Initialize values and status of controls
        /// </summary>
        private void InitializeControls()
        {
            textBoxSaveAs.Text = m_exportData.ExportFolder + "\\" + m_exportData.ExportFileName;

            radioButtonCurrentView.Checked = true;

            // Initialize the title
            this.Text = m_exportData.Title;
            if (m_exportData.ExportFormat == ExportFormat.SAT)
            {
                buttonOptions.Visible = false;
            }
            else if (m_exportData.ExportFormat == ExportFormat.Image)
            {
                this.Hide();
                ExportIMGData exportIMGData = m_exportData as ExportIMGData;
                using (ExportIMGOptionsForm exportOptionsForm = new ExportIMGOptionsForm(m_exportData))
                {
                    exportOptionsForm.ShowDialog();
                }

            }
        }

        /// <summary>
        /// Provide the export option dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOptions_Click(object sender, EventArgs e)
        {
            // Export dwg
            if (m_exportData.ExportFormat == ExportFormat.DWG)
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

                ExportDWGData exportDWGData = m_exportData as ExportDWGData;
                using (ExportBaseOptionsForm exportOptionsForm = new ExportBaseOptionsForm(exportDWGData.ExportOptionsData,
                    contain3DView, "DWG"))
                {
                    exportOptionsForm.ShowDialog();
                }
            }
            //Export dxf
            else if (m_exportData.ExportFormat == ExportFormat.DXF)
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

                ExportDXFData exportDXFData = m_exportData as ExportDXFData;

                using (ExportBaseOptionsForm exportOptionsForm = new ExportBaseOptionsForm(exportDXFData.ExportOptionsData,
                    contain3DView, "DXF"))
                {
                    exportOptionsForm.ShowDialog();
                }

            }
            // Export dgn
            else if (m_exportData.ExportFormat == ExportFormat.DGN)
            {
                ExportDGNData exportDGNData = m_exportData as ExportDGNData;
                using (ExportDGNOptionsForm exportOptionsForm = new ExportDGNOptionsForm(exportDGNData))
                {
                    exportOptionsForm.ShowDialog();
                }
            }

            // Export DWF
            else
            {
                ExportDWFData exportDWFData = m_exportData as ExportDWFData;
                using (ExportDWFOptionForm exportOptionsForm = new ExportDWFOptionForm(exportDWFData))
                {
                    exportOptionsForm.ShowDialog();
                }
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
                if (m_exportData.SelectViewsData.SelectedViews.Size == 0)
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
            String fileName = String.Empty;
            int filterIndex = -1;

            DialogResult result = MainData.ShowSaveDialog(m_exportData, ref fileName, ref filterIndex);
            if (result != DialogResult.Cancel)
            {
                textBoxSaveAs.Text = fileName;
                if (m_exportData.ExportFormat == ExportFormat.DWG)
                {
                    ExportDWGData exportDWGData = m_exportData as ExportDWGData;
                    exportDWGData.ExportFileVersion = exportDWGData.EnumFileVersion[filterIndex - 1];
                }
                else if (m_exportData.ExportFormat == ExportFormat.DXF)
                {
                    ExportDXFData exportDXFData = m_exportData as ExportDXFData;
                    exportDXFData.ExportFileVersion = exportDXFData.EnumFileVersion[filterIndex - 1];

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
                m_exportData.CurrentViewOnly = !radioButtonSelectView.Checked;

                try
                {
                    bool exported = m_exportData.Export();
                    if (!exported)
                    {
                        TaskDialog.Show("Export", "This project cannot be exported to " + m_exportData.ExportFileName +
                        " in current settings.", TaskDialogCommonButtons.Ok);
                    }
                }
                catch (Exception ex)
                {
                    TaskDialog.Show("Export Failed", ex.ToString(), TaskDialogCommonButtons.Ok);
                }

                this.DialogResult = DialogResult.OK;
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
                TaskDialog.Show("Information", "Please specify the folder and file name!", TaskDialogCommonButtons.Ok);
                textBoxSaveAs.Focus();
                return false;
            }

            //If has file name
            if (!fileNameFull.Contains("\\"))
            {
                TaskDialog.Show("Information", "Please specify file name!", TaskDialogCommonButtons.Ok);
                textBoxSaveAs.Focus();
                return false;
            }

            //Whether the folder exists
            String folder = Path.GetDirectoryName(fileNameFull);
            if (!Directory.Exists(folder))
            {
                TaskDialog.Show("Information", "The specified folder does not exist!", TaskDialogCommonButtons.Ok);
                textBoxSaveAs.Focus();
                return false;
            }

            m_exportData.ExportFileName = Path.GetFileNameWithoutExtension(fileNameFull);
            m_exportData.ExportFolder = folder;

            return true;
        }

    }
}
