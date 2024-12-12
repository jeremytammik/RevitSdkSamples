//
// (C) Copyright 2003-2019 by Autodesk, Inc.
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
using System.Reflection;
using System.IO;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.ImportExport.CS
{
    /// <summary>
    /// It contains a dialog which provides the options of importing dwg format
    /// </summary>
    public partial class ImportDWGForm : System.Windows.Forms.Form
    {
        /// <summary>
        /// Data class
        /// </summary>
        ImportDWGData m_importData;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="importData"></param>
        public ImportDWGForm(ImportDWGData importData)
        {
            InitializeComponent();
            m_importData = importData;
            InitializeControls();
        }

        /// <summary>
        /// Change the status of checkBoxOrient2View and comboBoxLevel according to the selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxCurrentViewOnly_CheckedChanged(object sender, EventArgs e)
        {
            bool currentViewOnly = checkBoxCurrentViewOnly.Checked;
            checkBoxOrient2View.Enabled = !currentViewOnly;
            comboBoxLevel.Enabled = !currentViewOnly;
        }

        /// <summary>
        /// Initialize values and status of controls
        /// </summary>
        private void InitializeControls()
        {
            //Layers
            foreach (String layer in m_importData.VisibleLayersOnly)
            {
                comboBoxLayers.Items.Add(layer);
            }
            comboBoxLayers.SelectedIndex = 0;

            //unit
            foreach (String unit in m_importData.Unit)
            {
                comboBoxUnits.Items.Add(unit);
            }
            comboBoxUnits.SelectedIndex = 0;

            //Level
            foreach (Autodesk.Revit.DB.View view in m_importData.Views)
            {
                comboBoxLevel.Items.Add(view.Name);
            }
            comboBoxLevel.SelectedIndex = 0;

            //If active view is 3D
            if (m_importData.Is3DView)
            {
                checkBoxCurrentViewOnly.Checked = false;
                checkBoxCurrentViewOnly.Enabled = false;
            }

            this.Text = m_importData.Title;
        }

        /// <summary>
        /// Specify a file to export from
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonBrowser_Click(object sender, EventArgs e)
        {
            String returnFileFullName = String.Empty;
            if (MainData.ShowOpenDialog(m_importData, ref returnFileFullName) != DialogResult.Cancel)
            {
                textBoxFileSource.Text = returnFileFullName;
            }
        }

        /// <summary>
        /// Change the status of textBoxScale according to the selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxUnits_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool custom = comboBoxUnits.SelectedIndex == 7;
            textBoxScale.Enabled = custom;
        }

        /// <summary>
        /// Transfer information back to ImportData class and execute IMPORT operation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOpen_Click(object sender, EventArgs e)
        {
            if (ValidateFileName())
            {
                //Color Mode
                SetImportColorMode();
                //Placement
                SetImportPlacement();
                //Current view only, Orient to view, view to import into
                SetImportViewsRelated();
                //Visible layers only
                SetImportLayers();
                //Unit, scaling
                SetImportUnitsAndScaling();

                //Import
                try
                {
                    if (!m_importData.Import())
                    {
                        TaskDialog.Show("Import", "Cannot import " + Path.GetFileName(m_importData.ImportFileFullName) +
                        " in current settings.", TaskDialogCommonButtons.Ok);
                    }
                }
                catch (Exception ex)
                {
                    TaskDialog.Show("Import Failed", ex.ToString(), TaskDialogCommonButtons.Ok);
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        /// <summary>
        /// Validate the file to import
        /// </summary>
        /// <returns></returns>
        private Boolean ValidateFileName()
        {
            String fileNameFull = textBoxFileSource.Text;
            //If the textBoxFileSource is empty
            if (String.IsNullOrEmpty(fileNameFull))
            {
                TaskDialog.Show("Information", "Please specify the folder and file name!", TaskDialogCommonButtons.Ok);
                textBoxFileSource.Focus();
                return false;
            }

            if (!File.Exists(fileNameFull))
            {
                TaskDialog.Show("Information", "Please specify an existing file!", TaskDialogCommonButtons.Ok);
                textBoxFileSource.Focus();
                return false;
            }

            m_importData.ImportFileFullName = fileNameFull;

            return true;
        }

        /// <summary>
        /// Set the value of color mode
        /// </summary>
        private void SetImportColorMode()
        {
            String colorMode;
            if (radioButtonBlackWhite.Checked)
            {
                colorMode = radioButtonBlackWhite.Text;
            }
            else if (radioButtonPreserve.Checked)
            {
                colorMode = radioButtonPreserve.Text;
            }
            else
            {
                colorMode = radioButtonInvertColor.Text;
            }

            int indexColor = 0;
            for (int i = 0; i < m_importData.ColorMode.Count; ++i)
            {
                if (colorMode == m_importData.ColorMode[i])
                {
                    indexColor = i;
                    break;
                }
            }

            m_importData.ImportColorMode = m_importData.EnumColorMode[indexColor];
        }

        /// <summary>
        /// Set the value of placement
        /// </summary>
        private void SetImportPlacement()
        {
            String placement;
            if (radioButtonCenter2Center.Checked)
            {
                placement = radioButtonCenter2Center.Text;
            }
            else
            {
                placement = radioButtonOrigin2Origin.Text;
            }

            int indexPlacement = 0;
            for (int i = 0; i < m_importData.Placement.Count; ++i)
            {
                if (placement == m_importData.Placement[i])
                {
                    indexPlacement = i;
                    break;
                }
            }

            m_importData.ImportPlacement = m_importData.EnumPlacement[indexPlacement];
        }

        /// <summary>
        /// Set the value of ImportThisViewOnly, ImportOrientToView, ImportView
        /// </summary>
        private void SetImportViewsRelated()
        {
            if (checkBoxCurrentViewOnly.Checked)
            {
                m_importData.ImportThisViewOnly = true;
            }
            else
            {
                m_importData.ImportThisViewOnly = false;
                m_importData.ImportOrientToView = checkBoxOrient2View.Checked;
                String viewName = comboBoxLevel.SelectedItem.ToString();
                foreach (Autodesk.Revit.DB.View view in m_importData.Views)
                {
                    if (viewName == view.Name)
                    {
                        m_importData.ImportView = view;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Set the value of ImportVisibleLayersOnly
        /// </summary>
        private void SetImportLayers()
        {
            //comboBoxLayers
            m_importData.ImportVisibleLayersOnly = m_importData.EnumVisibleLayersOnly[comboBoxLayers.SelectedIndex];
        }

        /// <summary>
        /// Set the value of unit, scaling
        /// </summary>
        private void SetImportUnitsAndScaling()
        {
            bool custom = comboBoxUnits.SelectedIndex == 7;

            //Custom is selected
            if (custom)
            {
                //Set the scaling
                m_importData.ImportCustomScale = Double.Parse(textBoxScale.Text);
            }
            else
            {
                //Do not change the scaling
                //Set unit
                m_importData.ImportUnit = m_importData.EnumUnit[comboBoxUnits.SelectedIndex];
            }
        }

        /// <summary>
        /// Only numbers and '.' permitted
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxScale_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < (char)46 || e.KeyChar > (char)57) && (e.KeyChar != (char)8 || e.KeyChar == (char)47))
            {
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }

        /// <summary>
        /// Only one '.' permitted
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxScale_TextChanged(object sender, EventArgs e)
        {
            bool newPoint = false;
            char tmp = '0';
            string text = textBoxScale.Text.Trim();
            StringBuilder textAfter = new StringBuilder();
            for (int i = 0; i < text.Length; i++)
            {
                tmp = char.Parse(text.Substring(i, 1).ToString());
                if (!newPoint && (tmp == '.'))
                {
                    textAfter.Append(tmp);
                    newPoint = true;
                }
                else
                {
                    if (char.IsDigit(tmp))
                    {
                        textAfter.Append(tmp);
                    }
                }

            }
            textBoxScale.Text = textAfter.ToString();
        }
    }
}
