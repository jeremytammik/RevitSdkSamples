//
// (C) Copyright 2003-2014 by Autodesk, Inc.
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

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.ImportExport.CS
{
    /// <summary>
    /// Provide a dialog which provides the options for exporting Civil3D format
    /// </summary>
    public partial class ExportCivil3DForm : System.Windows.Forms.Form
    {
        /// <summary>
        /// data class
        /// </summary>
        private ExportCivil3DData m_data;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="exportCivil3DData"></param>
        public ExportCivil3DForm(ExportCivil3DData exportCivil3DData)
        {
            m_data = exportCivil3DData;
            InitializeComponent();
            InitializeControl();
        }

        /// <summary>
        /// Initialize values and status of controls
        /// </summary>
        private void InitializeControl()
        {
            String unit = Properties.Resources.ResourceManager.GetString(m_data.Dut.ToString());
            labelUnit.Text = unit;

            textBoxSaveAs.Text = m_data.ExportFolder + "\\" + m_data.ExportFileName;
            foreach (View3D view in m_data.View3DList)
            {
                comboBox3DView.Items.Add(view.Name);
            }
            comboBox3DView.SelectedIndex = 0;

            foreach (ViewPlan vp in m_data.GrossAreaPlanList)
            {
                comboBoxGrossBuildPlan.Items.Add(vp.Name);
            }
            comboBoxGrossBuildPlan.SelectedIndex = 0;

            if (m_data.PropertyLineList.Count != 0)
            {
                foreach (PropertyLine pl in m_data.PropertyLineList)
                {
                    comboBoxPropertyLine.Items.Add("Property [" + pl.Id.IntegerValue + "]");
                }
            }
            else
            {
                comboBoxPropertyLine.Items.Add("None");
            }
            comboBoxPropertyLine.SelectedIndex = 0;

            textBoxPropertyLineOffset.Text = "0";
        }

        /// <summary>
        /// Show save dialog when button Browser is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonBrowser_Click(object sender, EventArgs e)
        {
            String fileName = String.Empty;
            int filterIndex = -1;

            DialogResult result = MainData.ShowSaveDialog(m_data, ref fileName, ref filterIndex);
            if (result != DialogResult.Cancel)
            {
                textBoxSaveAs.Text = fileName;
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

            m_data.ExportFileName = Path.GetFileNameWithoutExtension(fileNameFull);
            m_data.ExportFolder = folder;

            return true;
        }

        /// <summary>
        /// OK button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (ValidateExportFolder())
            {
                m_data.View3D = m_data.View3DList[comboBox3DView.SelectedIndex] as View3D;
                m_data.GrossAreaPlan = m_data.GrossAreaPlanList[comboBoxGrossBuildPlan.SelectedIndex];
                m_data.PropertyLine = (m_data.PropertyLineList.Count != 0 ?
                    m_data.PropertyLine = m_data.PropertyLineList[comboBoxPropertyLine.SelectedIndex] as PropertyLine : null);
                m_data.PropertyLineOffset = Unit.CovertToAPI(Convert.ToDouble(textBoxPropertyLineOffset.Text), m_data.Dut);

                try
                {
                    bool exported = m_data.Export();
                    if (!exported)
                    {
                        TaskDialog.Show("Export", "This project cannot be exported to " + m_data.ExportFileName +
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
    }
}
