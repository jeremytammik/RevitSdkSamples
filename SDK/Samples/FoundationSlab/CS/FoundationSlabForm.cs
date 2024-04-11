//
// (C) Copyright 2003-2019 by Autodesk, Inc.
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM 'AS IS' AND WITH ALL ITS FAULTS.
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

namespace Revit.SDK.Samples.FoundationSlab.CS
{
    /// <summary>
    /// A class to show properties and profiles of the foundation slabs.
    /// </summary>
    public partial class FoundationSlabForm : System.Windows.Forms.Form
    {
        // Revit datas for UI to display and operate.
        SlabData m_datas;

        // The columns of DataGridView.
        DataGridViewCheckBoxColumn selectedColumn = new DataGridViewCheckBoxColumn();
        DataGridViewTextBoxColumn markColumn = new DataGridViewTextBoxColumn();
        DataGridViewTextBoxColumn levelNameColumn = new DataGridViewTextBoxColumn();
        DataGridViewTextBoxColumn slabTypeNameColumn = new DataGridViewTextBoxColumn();

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="datas">The object contains floors' datas.</param>
        public FoundationSlabForm(SlabData datas)
        {
            m_datas = datas;

            InitializeComponent();
            InitializeDataGridView(); // DataGridView initialization.
        }

        private void InitializeDataGridView()
        {
            // Edit the columns of the dataGridView.
            dataGridView.AutoGenerateColumns = false;
            dataGridView.Columns.AddRange(new DataGridViewColumn[] { selectedColumn, markColumn, levelNameColumn, slabTypeNameColumn });
            dataGridView.DataSource = m_datas.BaseSlabList;

            // Select
            selectedColumn.DataPropertyName = "Selected";
            selectedColumn.HeaderText = "Select";
            selectedColumn.Name = "SelectedColumn";
            selectedColumn.ReadOnly = false;
            selectedColumn.Width = dataGridView.Width / 8;

            // Mark
            markColumn.DataPropertyName = "Mark";
            markColumn.HeaderText = "Mark";
            markColumn.Name = "MarkColumn";
            markColumn.ReadOnly = true;
            markColumn.Width = dataGridView.Width / 9;

            int remainWidth = dataGridView.Width - dataGridView.RowHeadersWidth - selectedColumn.Width - markColumn.Width;

            // Level
            levelNameColumn.DataPropertyName = "LevelName";
            levelNameColumn.HeaderText = "Level";
            levelNameColumn.Name = "levelNameColumn";
            levelNameColumn.ReadOnly = true;
            levelNameColumn.Width = remainWidth / 2 - 2;

            // Slab Type
            slabTypeNameColumn.DataPropertyName = "SlabTypeName";
            slabTypeNameColumn.HeaderText = "Slab Type";
            slabTypeNameColumn.Name = "SlabTypeNameColumn";
            slabTypeNameColumn.ReadOnly = true;
            slabTypeNameColumn.Width = remainWidth / 2;
        }

        /// <summary>
        /// Form load.
        /// </summary>
        /// <param name="sender">This object.</param>
        /// <param name="e">A object contains the event data.</param>
        private void FoundationSlabForm_Load(object sender, EventArgs e)
        {
            okButton.Enabled = m_datas.CheckHaveSelected();
            typeComboBox.DataSource = m_datas.FoundationSlabTypeList;
            typeComboBox.DisplayMember = "Name";

            if (0 == m_datas.BaseSlabList.Count)
            {
                selectAllButton.Enabled = false;
                clearAllButton.Enabled = false;
            }
        }

        /// <summary>
        /// Cell value changed.
        /// </summary>
        /// <param name="sender">This object.</param>
        /// <param name="e">A object contains the event data.</param>
        private void dataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            okButton.Enabled = m_datas.CheckHaveSelected();
            pictureBox.Refresh();
        }

        /// <summary>
        /// Cell click.
        /// </summary>
        /// <param name="sender">This object.</param>
        /// <param name="e">A object contains the event data.</param>
        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Click on CheckBoxes.
            if ((e.ColumnIndex >= 0) && ("CheckBoxes" == dataGridView.Columns[e.ColumnIndex].Name))
            {
                EventArgs newE = new EventArgs();
                this.dataGridView_CurrentCellDirtyStateChanged(this, newE);
            }
        }

        /// <summary>
        /// Current Cell Dirty State Changed.
        /// </summary>
        /// <param name="sender">This object.</param>
        /// <param name="e">A object contains the event data.</param>
        private void dataGridView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridView.IsCurrentCellDirty)
            {
                dataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit); // Commit dataGridView.
            }
        }

        /// <summary>
        /// Paint.
        /// </summary>
        /// <param name="sender">This object.</param>
        /// <param name="e">A object contains the event data.</param>
        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            Sketch.DrawProfile(e.Graphics, pictureBox.DisplayRectangle, m_datas.BaseSlabList);   // Draw profiles.
        }

        /// <summary>
        /// Click ok button.
        /// </summary>
        /// <param name="sender">This object.</param>
        /// <param name="e">A object contains the event data.</param>
        private void okButton_Click(object sender, EventArgs e)
        {
            bool IsSuccess = m_datas.CreateFoundationSlabs();
            if (IsSuccess)
                this.DialogResult = DialogResult.OK;
            else
                this.DialogResult = DialogResult.Cancel;
            Close();
        }

        /// <summary>
        /// Click cancel button.
        /// </summary>
        /// <param name="sender">This object.</param>
        /// <param name="e">A object contains the event data.</param>
        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Select type.
        /// </summary>
        /// <param name="sender">This object.</param>
        /// <param name="e">A object contains the event data.</param>
        private void typeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_datas.FoundationSlabType = typeComboBox.SelectedItem;
        }

        /// <summary>
        /// Click selectAllButton.
        /// </summary>
        /// <param name="sender">This object.</param>
        /// <param name="e">A object contains the event data.</param>
        private void selectAllButton_Click(object sender, EventArgs e)
        {
            m_datas.ChangeAllSelected(true);
            dataGridView.Refresh();
            okButton.Enabled = true;
            pictureBox.Refresh();
        }

        /// <summary>
        /// Click clearAllButton.
        /// </summary>
        /// <param name="sender">This object.</param>
        /// <param name="e">A object contains the event data.</param>
        private void clearAllButton_Click(object sender, EventArgs e)
        {
            m_datas.ChangeAllSelected(false);
            dataGridView.Refresh();
            okButton.Enabled = false;
            pictureBox.Refresh();
        }

    }
}