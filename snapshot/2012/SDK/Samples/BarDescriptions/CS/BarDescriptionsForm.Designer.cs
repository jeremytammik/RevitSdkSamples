//
// (C) Copyright 2003-2011 by Autodesk, Inc.
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

namespace Revit.SDK.Samples.BarDescriptions.CS
{
    partial class BarDescriptionsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; 
        /// otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.barDescriptionsGroupBox = new System.Windows.Forms.GroupBox();
            this.barDescriptionsDataGridView = new System.Windows.Forms.DataGridView();
            this.layerColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.barTypeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lengthColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hooktype0Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hookType1Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hookSameDirectionColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.countColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.closeButton = new System.Windows.Forms.Button();
            this.areaReinforcementGroupBox = new System.Windows.Forms.GroupBox();
            this.areaReinforcementIdListBox = new System.Windows.Forms.ListBox();
            this.exportButton = new System.Windows.Forms.Button();
            this.barDescriptionsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.barDescriptionsDataGridView)).BeginInit();
            this.areaReinforcementGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // barDescriptionsGroupBox
            // 
            this.barDescriptionsGroupBox.Controls.Add(this.barDescriptionsDataGridView);
            this.barDescriptionsGroupBox.Location = new System.Drawing.Point(165, 10);
            this.barDescriptionsGroupBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.barDescriptionsGroupBox.Name = "barDescriptionsGroupBox";
            this.barDescriptionsGroupBox.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.barDescriptionsGroupBox.Size = new System.Drawing.Size(544, 338);
            this.barDescriptionsGroupBox.TabIndex = 2;
            this.barDescriptionsGroupBox.TabStop = false;
            this.barDescriptionsGroupBox.Text = "Bar Descriptions";
            // 
            // barDescriptionsDataGridView
            // 
            this.barDescriptionsDataGridView.AllowUserToAddRows = false;
            this.barDescriptionsDataGridView.AllowUserToDeleteRows = false;
            this.barDescriptionsDataGridView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.barDescriptionsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.barDescriptionsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.layerColumn,
            this.barTypeColumn,
            this.lengthColumn,
            this.hooktype0Column,
            this.hookType1Column,
            this.hookSameDirectionColumn,
            this.countColumn});
            this.barDescriptionsDataGridView.Location = new System.Drawing.Point(8, 20);
            this.barDescriptionsDataGridView.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.barDescriptionsDataGridView.Name = "barDescriptionsDataGridView";
            this.barDescriptionsDataGridView.ReadOnly = true;
            this.barDescriptionsDataGridView.RowHeadersVisible = false;
            this.barDescriptionsDataGridView.RowTemplate.Height = 24;
            this.barDescriptionsDataGridView.Size = new System.Drawing.Size(527, 314);
            this.barDescriptionsDataGridView.TabIndex = 2;
            this.barDescriptionsDataGridView.TabStop = false;
            // 
            // layerColumn
            // 
            this.layerColumn.HeaderText = "Layer";
            this.layerColumn.Name = "layerColumn";
            this.layerColumn.ReadOnly = true;
            this.layerColumn.Width = 50;
            // 
            // barTypeColumn
            // 
            this.barTypeColumn.HeaderText = "BarType";
            this.barTypeColumn.Name = "barTypeColumn";
            this.barTypeColumn.ReadOnly = true;
            this.barTypeColumn.Width = 70;
            // 
            // lengthColumn
            // 
            this.lengthColumn.HeaderText = "Length (feet)";
            this.lengthColumn.Name = "lengthColumn";
            this.lengthColumn.ReadOnly = true;
            this.lengthColumn.Width = 60;
            // 
            // hooktype0Column
            // 
            this.hooktype0Column.HeaderText = "HookType0";
            this.hooktype0Column.Name = "hooktype0Column";
            this.hooktype0Column.ReadOnly = true;
            this.hooktype0Column.Width = 200;
            // 
            // hookType1Column
            // 
            this.hookType1Column.HeaderText = "HookType1";
            this.hookType1Column.Name = "hookType1Column";
            this.hookType1Column.ReadOnly = true;
            this.hookType1Column.Width = 200;
            // 
            // hookSameDirectionColumn
            // 
            this.hookSameDirectionColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.hookSameDirectionColumn.HeaderText = "Hook in Same Direction";
            this.hookSameDirectionColumn.Name = "hookSameDirectionColumn";
            this.hookSameDirectionColumn.ReadOnly = true;
            this.hookSameDirectionColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.hookSameDirectionColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.hookSameDirectionColumn.Width = 70;
            // 
            // countColumn
            // 
            this.countColumn.HeaderText = "Count";
            this.countColumn.Name = "countColumn";
            this.countColumn.ReadOnly = true;
            this.countColumn.Width = 50;
            // 
            // closeButton
            // 
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeButton.Location = new System.Drawing.Point(643, 359);
            this.closeButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(58, 27);
            this.closeButton.TabIndex = 4;
            this.closeButton.Text = "&Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // areaReinforcementGroupBox
            // 
            this.areaReinforcementGroupBox.Controls.Add(this.areaReinforcementIdListBox);
            this.areaReinforcementGroupBox.Location = new System.Drawing.Point(9, 10);
            this.areaReinforcementGroupBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.areaReinforcementGroupBox.Name = "areaReinforcementGroupBox";
            this.areaReinforcementGroupBox.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.areaReinforcementGroupBox.Size = new System.Drawing.Size(139, 339);
            this.areaReinforcementGroupBox.TabIndex = 0;
            this.areaReinforcementGroupBox.TabStop = false;
            this.areaReinforcementGroupBox.Text = "Element Ids of AreaReinforcement";
            // 
            // areaReinforcementIdListBox
            // 
            this.areaReinforcementIdListBox.FormattingEnabled = true;
            this.areaReinforcementIdListBox.Location = new System.Drawing.Point(4, 32);
            this.areaReinforcementIdListBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.areaReinforcementIdListBox.Name = "areaReinforcementIdListBox";
            this.areaReinforcementIdListBox.Size = new System.Drawing.Size(131, 303);
            this.areaReinforcementIdListBox.TabIndex = 1;
            this.areaReinforcementIdListBox.SelectedValueChanged += new System.EventHandler(this.areaReinforcementIdListBox_SelectedValueChanged);
            // 
            // exportButton
            // 
            this.exportButton.Location = new System.Drawing.Point(520, 359);
            this.exportButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.exportButton.Name = "exportButton";
            this.exportButton.Size = new System.Drawing.Size(58, 27);
            this.exportButton.TabIndex = 3;
            this.exportButton.Text = "&Export";
            this.exportButton.UseVisualStyleBackColor = true;
            this.exportButton.Click += new System.EventHandler(this.exportButton_Click);
            // 
            // BarDescriptionsForm
            // 
            this.AcceptButton = this.exportButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.closeButton;
            this.ClientSize = new System.Drawing.Size(713, 404);
            this.Controls.Add(this.exportButton);
            this.Controls.Add(this.areaReinforcementGroupBox);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.barDescriptionsGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BarDescriptionsForm";
            this.ShowInTaskbar = false;
            this.Text = "Bar Descriptions";
            this.Load += new System.EventHandler(this.BarDescriptionsForm_Load);
            this.barDescriptionsGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.barDescriptionsDataGridView)).EndInit();
            this.areaReinforcementGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox barDescriptionsGroupBox;
        private System.Windows.Forms.DataGridView barDescriptionsDataGridView;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.GroupBox areaReinforcementGroupBox;
        private System.Windows.Forms.ListBox areaReinforcementIdListBox;
        private System.Windows.Forms.Button exportButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn layerColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn barTypeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lengthColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn hooktype0Column;
        private System.Windows.Forms.DataGridViewTextBoxColumn hookType1Column;
        private System.Windows.Forms.DataGridViewCheckBoxColumn hookSameDirectionColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn countColumn;
    }
}