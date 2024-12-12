//
// (C) Copyright 2003-2015 by Autodesk, Inc.
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
    /// Provide a dialog which lets user choose the operation(export or import)
    /// </summary>
    public partial class MainForm : System.Windows.Forms.Form
    {
        /// <summary>
        /// Data class
        /// </summary>
        MainData m_mainData;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mainData"></param>
        public MainForm(MainData mainData)
        {
            m_mainData = mainData;
            InitializeComponent();
            radioButtonExport.Checked = true;
            comboBoxExport.Enabled = true;
            comboBoxImport.Enabled = false;
            //Append formats to be exported or imported
            InitializeFormats();
        }

        /// <summary>
        /// Append formats to be exported or imported
        /// </summary>
        private void InitializeFormats()
        {
            // Append formats to be exported
            comboBoxExport.Items.Add("DWG");
            comboBoxExport.Items.Add("DXF");
            comboBoxExport.Items.Add("SAT");
            comboBoxExport.Items.Add("DWF");
            comboBoxExport.Items.Add("DWFx");
            comboBoxExport.Items.Add("GBXML");
            comboBoxExport.Items.Add("DGN");
            if (m_mainData.Is3DView)
            {
                comboBoxExport.Items.Add("FBX");
            }
            comboBoxExport.Items.Add("Civil3D");
            comboBoxExport.Items.Add("IMAGE");
            comboBoxExport.SelectedIndex = 0;

            // Append formats to be imported
            comboBoxImport.Items.Add("DWG");
            if (!m_mainData.Is3DView)
            {
                comboBoxImport.Items.Add("IMAGE");
            }

            if (m_mainData.CommandData.Application.Application.Product == Autodesk.Revit.ApplicationServices.ProductType.MEP)
            {
                comboBoxImport.Items.Add("GBXML");
            }
            comboBoxImport.Items.Add("Inventor");
            
            comboBoxImport.SelectedIndex = 0;
        }

        /// <summary>
        /// Show the export/import dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOK_Click(object sender, EventArgs e)
        {
            String selectedFormat = String.Empty;
            DialogResult result = DialogResult.OK;

            if (radioButtonExport.Checked == true)
            {
                selectedFormat = comboBoxExport.SelectedItem.ToString();
                result = m_mainData.Export(selectedFormat);
            }
            else
            {
                selectedFormat = comboBoxImport.SelectedItem.ToString();
                result = m_mainData.Import(selectedFormat);
            }

            this.DialogResult = (result != DialogResult.Cancel ? DialogResult.OK : DialogResult.None);
        }

        private void radioButtonExport_CheckedChanged(object sender, EventArgs e)
        {
            comboBoxImport.Enabled = !radioButtonExport.Checked;
            comboBoxExport.Enabled = radioButtonExport.Checked;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
      
    }
}
