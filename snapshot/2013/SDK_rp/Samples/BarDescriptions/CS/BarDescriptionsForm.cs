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

namespace Revit.SDK.Samples.BarDescriptions.CS
{
    /// <summary>
    /// UI which displays the BarDescriptions
    /// </summary>
    public partial class BarDescriptionsForm : System.Windows.Forms.Form
    {
        Command m_dataBuffer = null;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="dataBuffer"> an instance of BarDescriptions class</param>
        public BarDescriptionsForm(Command dataBuffer)
        {
            InitializeComponent();

            m_dataBuffer = dataBuffer;
        }

        private void BarDescriptionsForm_Load(object sender, EventArgs e)
        {
            // set areaReinforcementIdListBox's data source. 
            // Let it displays all the AreaReinforcement Ids in the project
            areaReinforcementIdListBox.DataSource = m_dataBuffer.AreaReinforcementIds;

            // set barDescriptionsDataGridView's data source.
            // let it displays all the BarDescriotions information in the project
            barDescriptionsDataGridView.AutoGenerateColumns = false;

            barDescriptionsDataGridView.DataSource   = m_dataBuffer.SpecificBarDescriptions;
            layerColumn.DataPropertyName             = "layer";
            barTypeColumn.DataPropertyName           = "barType";
            lengthColumn.DataPropertyName            = "barLength";
            hooktype0Column.DataPropertyName         = "oneEndHookType";
            hookType1Column.DataPropertyName         = "otherEndHookType";
            hookSameDirectionColumn.DataPropertyName = "hookSameDirection";
            countColumn.DataPropertyName             = "barCount";
 
            // get data view's row filter condition
            object selectItem = areaReinforcementIdListBox.SelectedItem;

            if (null != selectItem)
            {
                int areaReinforcementIdValue = int.Parse(selectItem.ToString());
                m_dataBuffer.SetViewRowFilterCondition(areaReinforcementIdValue);
                barDescriptionsDataGridView.Refresh();
            }

            areaReinforcementIdListBox.Focus();
        }

        /// <summary>
        ///set data view's row filter condition
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void areaReinforcementIdListBox_SelectedValueChanged(
            object sender, EventArgs e)
        {
            object selectItem            = areaReinforcementIdListBox.SelectedItem;
            int areaReinforcementIdValue = int.Parse(selectItem.ToString());

            m_dataBuffer.SetViewRowFilterCondition(areaReinforcementIdValue);
            barDescriptionsDataGridView.Refresh();
        }

        /// <summary>
        /// export all the BarDescriptions information to a Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exportButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog   = new SaveFileDialog();
            saveDialog.Title            = "Export all Bar Descriptions to Excel.";
            saveDialog.Filter           = "CSV(Comma delimited)(*.csv)|*.csv";
            saveDialog.RestoreDirectory = true;

            if (DialogResult.OK == saveDialog.ShowDialog())
            {
                string saveFullPath = saveDialog.FileName;

                if (!saveFullPath.EndsWith(".csv"))
                {
                    saveFullPath += ".csv";
                }

                exportButton.Text = "Wait...";
                m_dataBuffer.ExportAllData(saveFullPath);
                exportButton.Text = "&Export";
            }
        }

        /// <summary>
        /// close form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
