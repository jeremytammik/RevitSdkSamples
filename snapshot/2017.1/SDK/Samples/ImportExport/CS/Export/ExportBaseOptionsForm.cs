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

namespace Revit.SDK.Samples.ImportExport.CS
{
    /// <summary>
    /// Provide a dialog which provides the options of lower priority information for exporting dwg format
    /// </summary>
    public partial class ExportBaseOptionsForm : System.Windows.Forms.Form
    {
        /// <summary>
        /// data class
        /// </summary>
        private ExportBaseOptionsData m_exportOptionsData;
        

        /// <summary>
        /// Whether export the current view only
        /// </summary>
        private bool m_contain3DView;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="exportOptionsData">Data class object</param>
        /// <param name="contain3DView">If views to export contain 3D views</param>
        /// <param name="exportFormat">export format</param>
        public ExportBaseOptionsForm(ExportBaseOptionsData exportOptionsData, bool contain3DView, String exportFormat)
        {
            InitializeComponent();
            m_exportOptionsData = exportOptionsData;
            m_contain3DView = contain3DView;
            this.Text = "Export " + exportFormat + " Options";
            InitializeControl();
        }

        /// <summary>
        /// Initialize values and status of controls
        /// </summary>
        private void InitializeControl()
        {
            comboBoxLayersAndProperties.DataSource = m_exportOptionsData.LayersAndProperties;
            comboBoxLayersAndProperties.SelectedIndex = 0;
            comboBoxLayerSettings.DataSource = m_exportOptionsData.LayerMapping;
            comboBoxLayerSettings.SelectedIndex = 0;
            comboBoxLinetypeScaling.DataSource = m_exportOptionsData.LineScaling;
            comboBoxLinetypeScaling.SelectedIndex = 2;
            comboBoxCoorSystem.DataSource = m_exportOptionsData.CoorSystem;
            comboBoxCoorSystem.SelectedIndex = 0;
            comboBoxDWGUnit.DataSource = m_exportOptionsData.Units;
            comboBoxDWGUnit.SelectedIndex = 1;
            comboBoxSolids.DataSource = m_exportOptionsData.Solids;
            comboBoxSolids.SelectedIndex = 0;
            checkBoxMergeViews.Checked = m_exportOptionsData.ExportMergeFiles;
            checkBoxMergeViews.Text = "Merge all views in one file (via XRefs).";
            if (m_contain3DView)
            {
                comboBoxSolids.Enabled = true;
            }
            else
            {
                comboBoxSolids.Enabled = false;
            }
        }

        /// <summary>
        /// Transfer information back to ExportOptionData class
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOK_Click(object sender, EventArgs e)
        {
            m_exportOptionsData.ExportLayersAndProperties = 
                m_exportOptionsData.EnumLayersAndProperties[comboBoxLayersAndProperties.SelectedIndex];
            m_exportOptionsData.ExportLayerMapping = 
                m_exportOptionsData.EnumLayerMapping[comboBoxLayerSettings.SelectedIndex];
            m_exportOptionsData.ExportLineScaling = 
                m_exportOptionsData.EnumLineScaling[comboBoxLinetypeScaling.SelectedIndex];
            m_exportOptionsData.ExportCoorSystem = 
                m_exportOptionsData.EnumCoorSystem[comboBoxCoorSystem.SelectedIndex];
            m_exportOptionsData.ExportUnit = m_exportOptionsData.EnumUnits[comboBoxDWGUnit.SelectedIndex];
            m_exportOptionsData.ExportSolid = m_exportOptionsData.EnumSolids[comboBoxSolids.SelectedIndex];
            m_exportOptionsData.ExportAreas = checkBoxExportingAreas.Checked;
            m_exportOptionsData.ExportMergeFiles = checkBoxMergeViews.Checked;

            this.Close();
        }
    }
}