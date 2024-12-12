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
using System.Windows.Forms;

using TaskDialog = Autodesk.Revit.UI.TaskDialog;

namespace Revit.SDK.Samples.Loads.CS
{
    /// <summary>
    /// mainly deal with the operation on load case page on the form
    /// </summary>
    public partial class LoadsForm
    {
        int m_loadCaseDataGridViewSelectedIndex;
        int m_loadNatureDataGridViewSelectedIndex;
        System.Windows.Forms.DataGridViewTextBoxColumn LoadCasesName;
        System.Windows.Forms.DataGridViewTextBoxColumn LoadCasesNumber;
        System.Windows.Forms.DataGridViewComboBoxColumn LoadCasesNature;
        System.Windows.Forms.DataGridViewComboBoxColumn LoadCasesCategory;
        System.Windows.Forms.DataGridViewTextBoxColumn LoadNatureName;


        // Methods
        /// <summary>
        /// Initialize the data on this page.
        /// </summary>
        void InitializeLoadCasePage()
        {
            InitializeLoadCasesDataGridView();
            InitializeLoadNaturesDataGridView();


            if (0 == m_dataBuffer.LoadCases.Count)
            {
                this.duplicateLoadCasesButton.Enabled = false;
            }
            if (0 == m_dataBuffer.LoadNatures.Count)
            {
                this.addLoadNaturesButton.Enabled = false;
            }
            this.addLoadNaturesButton.Enabled = false;
        }

        /// <summary>
        /// Initialize the loadCasesDataGridView
        /// </summary>
        private void InitializeLoadCasesDataGridView()
        {
            this.LoadCasesName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LoadCasesNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LoadCasesNature = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.LoadCasesCategory = new System.Windows.Forms.DataGridViewComboBoxColumn();
            loadCasesDataGridView.AutoGenerateColumns = false;
            this.loadCasesDataGridView.Columns.AddRange(new DataGridViewColumn[] { this.LoadCasesName, this.LoadCasesNumber, this.LoadCasesNature, this.LoadCasesCategory });
            loadCasesDataGridView.DataSource = m_dataBuffer.LoadCasesMap;

            this.LoadCasesName.DataPropertyName = "LoadCasesName";
            this.LoadCasesName.HeaderText = "Name";
            this.LoadCasesName.Name = "LoadCasesName";
            this.LoadCasesName.ReadOnly = false;
            this.LoadCasesName.Width = loadCasesDataGridView.Width / 6;

            this.LoadCasesNumber.DataPropertyName = "LoadCasesNumber";
            this.LoadCasesNumber.HeaderText = "Case Number";
            this.LoadCasesNumber.Name = "LoadCasesNumber";
            this.LoadCasesNumber.ReadOnly = true;
            this.LoadCasesNumber.Width = loadCasesDataGridView.Width / 4;

            this.LoadCasesNature.DataPropertyName = "LoadCasesNatureId";
            this.LoadCasesNature.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
            this.LoadCasesNature.HeaderText = "Nature";
            this.LoadCasesNature.Name = "LoadCasesNature";
            this.LoadCasesNature.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.LoadCasesNature.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.LoadCasesNature.Width = loadCasesDataGridView.Width / 4;

            LoadCasesNature.DataSource = m_dataBuffer.LoadNatures;
            LoadCasesNature.DisplayMember = "Name";
            LoadCasesNature.ValueMember = "Id";

            this.LoadCasesCategory.DataPropertyName = "LoadCasesCategoryId";
            this.LoadCasesCategory.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
            this.LoadCasesCategory.HeaderText = "Category";
            this.LoadCasesCategory.Name = "LoadCasesCategory";
            this.LoadCasesCategory.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.LoadCasesCategory.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.LoadCasesCategory.Width = loadCasesDataGridView.Width / 4;

            LoadCasesCategory.DataSource = m_dataBuffer.LoadCaseCategories;
            LoadCasesCategory.DisplayMember = "Name";
            LoadCasesCategory.ValueMember = "Id";
            this.loadCasesDataGridView.MultiSelect = false;
            this.loadCasesDataGridView.SelectionMode = DataGridViewSelectionMode.CellSelect;
        }

        /// <summary>
        /// Initialize the loadNaturesDataGridView
        /// </summary>
        private void InitializeLoadNaturesDataGridView()
        {
            this.LoadNatureName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            loadNaturesDataGridView.AutoGenerateColumns = false;
            this.loadNaturesDataGridView.Columns.AddRange(new DataGridViewColumn[] { this.LoadNatureName });
            loadNaturesDataGridView.DataSource = m_dataBuffer.LoadNaturesMap;
            this.LoadNatureName.DataPropertyName = "LoadNaturesName";
            this.LoadNatureName.HeaderText = "Name";
            this.LoadNatureName.Name = "LoadNaturesName";
            this.LoadNatureName.ReadOnly = false;
            this.LoadNatureName.Width = loadCasesDataGridView.Width - 100;
            this.loadNaturesDataGridView.MultiSelect = false;
            this.loadNaturesDataGridView.SelectionMode = DataGridViewSelectionMode.CellSelect;

        }

        /// <summary>
        /// Respond the loadCasesDataGridView_CellClick event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loadCasesDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Initilize();
            m_loadCaseDataGridViewSelectedIndex = e.RowIndex;
        }

        /// <summary>
        /// Respond the loadNaturesDataGridView_CellClick event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loadNaturesDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Initilize();
            m_loadNatureDataGridViewSelectedIndex = e.RowIndex;
        }

        /// <summary>
        /// Respond the loadCasesDataGridView_ColumnHeaderMouseClick event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loadCasesDataGridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            m_loadCaseDataGridViewSelectedIndex = e.RowIndex;
        }

        /// <summary>
        /// Respond the loadNaturesDataGridView_RowHeaderMouseClick event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loadNaturesDataGridView_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            m_loadNatureDataGridViewSelectedIndex = e.RowIndex;
        }

        /// <summary>
        /// Respond the DataGridView cell validating event, 
        /// check the user's input whether it is correct.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loadNaturesDataGridView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            object objectTemp = this.loadNaturesDataGridView.CurrentCell.Value;
            string nameTemp = objectTemp as string;

            object changeValue = e.FormattedValue;
            string changeValueTemp = changeValue as string;

            if (nameTemp == changeValueTemp)
            {
                return;
            }

            if (null == changeValueTemp)
            {
                TaskDialog.Show("Revit", "Name can not be null");
                e.Cancel = true;
                return;
            }

            if ("" == changeValueTemp)
            {
                TaskDialog.Show("Revit", "Name can not be null");
                e.Cancel = true;
                return;
            }

            if (!m_dataBuffer.LoadCasesDeal.IsNatureNameUnique(changeValueTemp))
            {
                TaskDialog.Show("Revit", "Name can not be same");
                e.Cancel = true;
                return;
            }
        }

        /// <summary>
        /// Respond the DataGridView cell validating event, 
        /// check the user's input whether it is correct.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loadCasesDataGridView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex != 0)
            {
                return;
            }

            DataGridViewCell cellTemp = this.loadCasesDataGridView.CurrentCell;
            if (null == cellTemp)
            {
                return;
            }
            string nameTemp = cellTemp.Value as string;
            if (null == nameTemp)
            {
                e.Cancel = false;
                return;
            }

            object changeValue = e.FormattedValue;
            string changeValueTemp = changeValue as string;

            if (nameTemp == changeValueTemp)
            {
                return;
            }

            if (null == changeValueTemp)
            {
                TaskDialog.Show("Revit", "Name can not be null");
                e.Cancel = true;
                return;
            }

            if ("" == changeValueTemp)
            {
                TaskDialog.Show("Revit", "Name can not be null");
                e.Cancel = true;
                return;
            }

            if (!m_dataBuffer.LoadCasesDeal.IsCaseNameUnique(changeValueTemp))
            {
                TaskDialog.Show("Revit", "Name can not be same");
                e.Cancel = true;
                return;
            }
        }

        /// <summary>
        /// When duplicateLoadCasesButton clicked, duplicate a load case. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void duplicateLoadCasesButton_Click(object sender, EventArgs e)
        {
            m_loadCaseDataGridViewSelectedIndex = this.loadCasesDataGridView.CurrentCell.RowIndex;
            if (!m_dataBuffer.LoadCasesDeal.DuplicateLoadCase(m_loadCaseDataGridViewSelectedIndex))
            {
                TaskDialog.Show("Revit", "Duplicate failed");
                return;
            }
            this.ReLoad();
        }

        /// <summary>
        /// When addLoadNaturesButton clicked, add a new load nature. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addLoadNaturesButton_Click(object sender, EventArgs e)
        {
            if (!m_dataBuffer.LoadCasesDeal.AddLoadNature(m_loadNatureDataGridViewSelectedIndex))
            {
                TaskDialog.Show("Revit", "Add Nature Failed");
                return;
            }
            this.ReLoad();
        }

        /// <summary>
        /// Reload the data of the cases and natures
        /// </summary>
        private void ReLoad()
        {
            this.loadNaturesDataGridView.DataSource = null;
            this.loadCasesDataGridView.DataSource = null;
            this.LoadCasesNature.SortMode = DataGridViewColumnSortMode.Automatic;
            this.loadNaturesDataGridView.DataSource = m_dataBuffer.LoadNaturesMap;
            this.loadCasesDataGridView.DataSource = m_dataBuffer.LoadCasesMap;
            this.Refresh();
            return;
        }

        /// <summary>
        /// enable button
        /// </summary>
        private void Initilize()
        {
            if (this.loadCasesDataGridView.Focused)
            {
                this.addLoadNaturesButton.Enabled = false;
                this.duplicateLoadCasesButton.Enabled = true;

            }
            else if (this.loadNaturesDataGridView.Focused)
            {
                this.addLoadNaturesButton.Enabled = true;
                this.duplicateLoadCasesButton.Enabled = false;

            }
            this.Refresh();
            return;
        }
    }
}
