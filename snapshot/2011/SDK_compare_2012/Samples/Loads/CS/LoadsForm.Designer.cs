//
// (C) Copyright 2003-2010 by Autodesk, Inc. 
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


namespace Revit.SDK.Samples.Loads.CS
{
    partial class LoadsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.loadCasesTabPage = new System.Windows.Forms.TabPage();
            this.addLoadNaturesButton = new System.Windows.Forms.Button();
            this.duplicateLoadCasesButton = new System.Windows.Forms.Button();
            this.loadNaturesroupBox = new System.Windows.Forms.GroupBox();
            this.loadNaturesDataGridView = new System.Windows.Forms.DataGridView();
            this.loadCasesGroupBox = new System.Windows.Forms.GroupBox();
            this.loadCasesDataGridView = new System.Windows.Forms.DataGridView();
            this.LoadCombinationsTabPage = new System.Windows.Forms.TabPage();
            this.combinationCreationGroupBox = new System.Windows.Forms.GroupBox();
            this.deleteCombinationButton = new System.Windows.Forms.Button();
            this.newCombinationButton = new System.Windows.Forms.Button();
            this.combinationStateComboBox = new System.Windows.Forms.ComboBox();
            this.combinationStateLabel = new System.Windows.Forms.Label();
            this.combinationTypeComboBox = new System.Windows.Forms.ComboBox();
            this.combinationTypeLabel = new System.Windows.Forms.Label();
            this.combinationNameTextBox = new System.Windows.Forms.TextBox();
            this.combinnationNameLabel = new System.Windows.Forms.Label();
            this.combinationFormulaGroupBox = new System.Windows.Forms.GroupBox();
            this.formulaDeleteButton = new System.Windows.Forms.Button();
            this.formulaAddButton = new System.Windows.Forms.Button();
            this.formulaDataGridView = new System.Windows.Forms.DataGridView();
            this.usageGroupBox = new System.Windows.Forms.GroupBox();
            this.usageDataGridView = new System.Windows.Forms.DataGridView();
            this.usageDeleteButton = new System.Windows.Forms.Button();
            this.usageAddButton = new System.Windows.Forms.Button();
            this.usageCheckNoneButton = new System.Windows.Forms.Button();
            this.usageCheckAllButton = new System.Windows.Forms.Button();
            this.combinationInfoGroupBox = new System.Windows.Forms.GroupBox();
            this.combinationDataGridView = new System.Windows.Forms.DataGridView();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.loadCasesTabPage.SuspendLayout();
            this.loadNaturesroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.loadNaturesDataGridView)).BeginInit();
            this.loadCasesGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.loadCasesDataGridView)).BeginInit();
            this.LoadCombinationsTabPage.SuspendLayout();
            this.combinationCreationGroupBox.SuspendLayout();
            this.combinationFormulaGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.formulaDataGridView)).BeginInit();
            this.usageGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.usageDataGridView)).BeginInit();
            this.combinationInfoGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.combinationDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.loadCasesTabPage);
            this.tabControl1.Controls.Add(this.LoadCombinationsTabPage);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(699, 475);
            this.tabControl1.TabIndex = 0;
            // 
            // loadCasesTabPage
            // 
            this.loadCasesTabPage.Controls.Add(this.addLoadNaturesButton);
            this.loadCasesTabPage.Controls.Add(this.duplicateLoadCasesButton);
            this.loadCasesTabPage.Controls.Add(this.loadNaturesroupBox);
            this.loadCasesTabPage.Controls.Add(this.loadCasesGroupBox);
            this.loadCasesTabPage.Location = new System.Drawing.Point(4, 22);
            this.loadCasesTabPage.Name = "loadCasesTabPage";
            this.loadCasesTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.loadCasesTabPage.Size = new System.Drawing.Size(691, 449);
            this.loadCasesTabPage.TabIndex = 0;
            this.loadCasesTabPage.Text = "Load Cases";
            this.loadCasesTabPage.UseVisualStyleBackColor = true;
            // 
            // addLoadNaturesButton
            // 
            this.addLoadNaturesButton.Location = new System.Drawing.Point(609, 273);
            this.addLoadNaturesButton.Name = "addLoadNaturesButton";
            this.addLoadNaturesButton.Size = new System.Drawing.Size(75, 23);
            this.addLoadNaturesButton.TabIndex = 4;
            this.addLoadNaturesButton.Text = "&Add";
            this.addLoadNaturesButton.UseVisualStyleBackColor = true;
            this.addLoadNaturesButton.Click += new System.EventHandler(this.addLoadNaturesButton_Click);
            // 
            // duplicateLoadCasesButton
            // 
            this.duplicateLoadCasesButton.Location = new System.Drawing.Point(609, 42);
            this.duplicateLoadCasesButton.Name = "duplicateLoadCasesButton";
            this.duplicateLoadCasesButton.Size = new System.Drawing.Size(75, 23);
            this.duplicateLoadCasesButton.TabIndex = 3;
            this.duplicateLoadCasesButton.Text = "&Duplicate";
            this.duplicateLoadCasesButton.UseVisualStyleBackColor = true;
            this.duplicateLoadCasesButton.Click += new System.EventHandler(this.duplicateLoadCasesButton_Click);
            // 
            // loadNaturesroupBox
            // 
            this.loadNaturesroupBox.Controls.Add(this.loadNaturesDataGridView);
            this.loadNaturesroupBox.Location = new System.Drawing.Point(30, 244);
            this.loadNaturesroupBox.Name = "loadNaturesroupBox";
            this.loadNaturesroupBox.Size = new System.Drawing.Size(573, 190);
            this.loadNaturesroupBox.TabIndex = 1;
            this.loadNaturesroupBox.TabStop = false;
            this.loadNaturesroupBox.Text = "Load Natures";
            // 
            // loadNaturesDataGridView
            // 
            this.loadNaturesDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.loadNaturesDataGridView.Location = new System.Drawing.Point(7, 20);
            this.loadNaturesDataGridView.Name = "loadNaturesDataGridView";
            this.loadNaturesDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.loadNaturesDataGridView.Size = new System.Drawing.Size(560, 164);
            this.loadNaturesDataGridView.TabIndex = 0;
            this.loadNaturesDataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.loadNaturesDataGridView_CellClick);
            this.loadNaturesDataGridView.RowHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.loadNaturesDataGridView_RowHeaderMouseClick);
            this.loadNaturesDataGridView.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.loadNaturesDataGridView_CellValidating);
            // 
            // loadCasesGroupBox
            // 
            this.loadCasesGroupBox.Controls.Add(this.loadCasesDataGridView);
            this.loadCasesGroupBox.Location = new System.Drawing.Point(30, 23);
            this.loadCasesGroupBox.Name = "loadCasesGroupBox";
            this.loadCasesGroupBox.Size = new System.Drawing.Size(573, 215);
            this.loadCasesGroupBox.TabIndex = 0;
            this.loadCasesGroupBox.TabStop = false;
            this.loadCasesGroupBox.Text = "Load Cases";
            // 
            // loadCasesDataGridView
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Gray;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.loadCasesDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.loadCasesDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.loadCasesDataGridView.GridColor = System.Drawing.SystemColors.ActiveBorder;
            this.loadCasesDataGridView.Location = new System.Drawing.Point(7, 19);
            this.loadCasesDataGridView.Name = "loadCasesDataGridView";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Silver;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.loadCasesDataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.loadCasesDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.loadCasesDataGridView.Size = new System.Drawing.Size(560, 190);
            this.loadCasesDataGridView.TabIndex = 0;
            this.loadCasesDataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.loadCasesDataGridView_CellClick);
            this.loadCasesDataGridView.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.loadCasesDataGridView_ColumnHeaderMouseClick);
            this.loadCasesDataGridView.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.loadCasesDataGridView_CellValidating);
            // 
            // LoadCombinationsTabPage
            // 
            this.LoadCombinationsTabPage.Controls.Add(this.combinationCreationGroupBox);
            this.LoadCombinationsTabPage.Controls.Add(this.combinationInfoGroupBox);
            this.LoadCombinationsTabPage.Location = new System.Drawing.Point(4, 22);
            this.LoadCombinationsTabPage.Name = "LoadCombinationsTabPage";
            this.LoadCombinationsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.LoadCombinationsTabPage.Size = new System.Drawing.Size(691, 449);
            this.LoadCombinationsTabPage.TabIndex = 1;
            this.LoadCombinationsTabPage.Text = "Load Combinations";
            this.LoadCombinationsTabPage.UseVisualStyleBackColor = true;
            // 
            // combinationCreationGroupBox
            // 
            this.combinationCreationGroupBox.Controls.Add(this.deleteCombinationButton);
            this.combinationCreationGroupBox.Controls.Add(this.newCombinationButton);
            this.combinationCreationGroupBox.Controls.Add(this.combinationStateComboBox);
            this.combinationCreationGroupBox.Controls.Add(this.combinationStateLabel);
            this.combinationCreationGroupBox.Controls.Add(this.combinationTypeComboBox);
            this.combinationCreationGroupBox.Controls.Add(this.combinationTypeLabel);
            this.combinationCreationGroupBox.Controls.Add(this.combinationNameTextBox);
            this.combinationCreationGroupBox.Controls.Add(this.combinnationNameLabel);
            this.combinationCreationGroupBox.Controls.Add(this.combinationFormulaGroupBox);
            this.combinationCreationGroupBox.Controls.Add(this.usageGroupBox);
            this.combinationCreationGroupBox.Location = new System.Drawing.Point(18, 203);
            this.combinationCreationGroupBox.Name = "combinationCreationGroupBox";
            this.combinationCreationGroupBox.Size = new System.Drawing.Size(651, 229);
            this.combinationCreationGroupBox.TabIndex = 1;
            this.combinationCreationGroupBox.TabStop = false;
            this.combinationCreationGroupBox.Text = "Load Combination Creation";
            // 
            // deleteCombinationButton
            // 
            this.deleteCombinationButton.Location = new System.Drawing.Point(486, 193);
            this.deleteCombinationButton.Name = "deleteCombinationButton";
            this.deleteCombinationButton.Size = new System.Drawing.Size(146, 23);
            this.deleteCombinationButton.TabIndex = 9;
            this.deleteCombinationButton.Text = "D&elete Combination";
            this.deleteCombinationButton.UseVisualStyleBackColor = true;
            this.deleteCombinationButton.Click += new System.EventHandler(this.deleteCombinationButton_Click);
            // 
            // newCombinationButton
            // 
            this.newCombinationButton.Location = new System.Drawing.Point(486, 150);
            this.newCombinationButton.Name = "newCombinationButton";
            this.newCombinationButton.Size = new System.Drawing.Size(146, 23);
            this.newCombinationButton.TabIndex = 8;
            this.newCombinationButton.Text = "&New Combination";
            this.newCombinationButton.UseVisualStyleBackColor = true;
            this.newCombinationButton.Click += new System.EventHandler(this.newCombinationButton_Click);
            // 
            // combinationStateComboBox
            // 
            this.combinationStateComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combinationStateComboBox.FormattingEnabled = true;
            this.combinationStateComboBox.Location = new System.Drawing.Point(528, 109);
            this.combinationStateComboBox.Name = "combinationStateComboBox";
            this.combinationStateComboBox.Size = new System.Drawing.Size(104, 21);
            this.combinationStateComboBox.TabIndex = 7;
            // 
            // combinationStateLabel
            // 
            this.combinationStateLabel.AutoSize = true;
            this.combinationStateLabel.Location = new System.Drawing.Point(483, 112);
            this.combinationStateLabel.Name = "combinationStateLabel";
            this.combinationStateLabel.Size = new System.Drawing.Size(35, 13);
            this.combinationStateLabel.TabIndex = 6;
            this.combinationStateLabel.Text = "State:";
            // 
            // combinationTypeComboBox
            // 
            this.combinationTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combinationTypeComboBox.FormattingEnabled = true;
            this.combinationTypeComboBox.Location = new System.Drawing.Point(528, 66);
            this.combinationTypeComboBox.Name = "combinationTypeComboBox";
            this.combinationTypeComboBox.Size = new System.Drawing.Size(104, 21);
            this.combinationTypeComboBox.TabIndex = 5;
            // 
            // combinationTypeLabel
            // 
            this.combinationTypeLabel.AutoSize = true;
            this.combinationTypeLabel.Location = new System.Drawing.Point(483, 69);
            this.combinationTypeLabel.Name = "combinationTypeLabel";
            this.combinationTypeLabel.Size = new System.Drawing.Size(34, 13);
            this.combinationTypeLabel.TabIndex = 4;
            this.combinationTypeLabel.Text = "Type:";
            // 
            // combinationNameTextBox
            // 
            this.combinationNameTextBox.Location = new System.Drawing.Point(528, 25);
            this.combinationNameTextBox.Name = "combinationNameTextBox";
            this.combinationNameTextBox.Size = new System.Drawing.Size(104, 20);
            this.combinationNameTextBox.TabIndex = 3;
            // 
            // combinnationNameLabel
            // 
            this.combinnationNameLabel.AutoSize = true;
            this.combinnationNameLabel.Location = new System.Drawing.Point(483, 28);
            this.combinnationNameLabel.Name = "combinnationNameLabel";
            this.combinnationNameLabel.Size = new System.Drawing.Size(38, 13);
            this.combinnationNameLabel.TabIndex = 2;
            this.combinnationNameLabel.Text = "Name:";
            // 
            // combinationFormulaGroupBox
            // 
            this.combinationFormulaGroupBox.Controls.Add(this.formulaDeleteButton);
            this.combinationFormulaGroupBox.Controls.Add(this.formulaAddButton);
            this.combinationFormulaGroupBox.Controls.Add(this.formulaDataGridView);
            this.combinationFormulaGroupBox.Location = new System.Drawing.Point(279, 19);
            this.combinationFormulaGroupBox.Name = "combinationFormulaGroupBox";
            this.combinationFormulaGroupBox.Size = new System.Drawing.Size(201, 197);
            this.combinationFormulaGroupBox.TabIndex = 1;
            this.combinationFormulaGroupBox.TabStop = false;
            this.combinationFormulaGroupBox.Text = "Load Combination Formula";
            // 
            // formulaDeleteButton
            // 
            this.formulaDeleteButton.Location = new System.Drawing.Point(112, 168);
            this.formulaDeleteButton.Name = "formulaDeleteButton";
            this.formulaDeleteButton.Size = new System.Drawing.Size(75, 23);
            this.formulaDeleteButton.TabIndex = 6;
            this.formulaDeleteButton.Text = "D&elete";
            this.formulaDeleteButton.UseVisualStyleBackColor = true;
            this.formulaDeleteButton.Click += new System.EventHandler(this.formulaDeleteButton_Click);
            // 
            // formulaAddButton
            // 
            this.formulaAddButton.Location = new System.Drawing.Point(15, 168);
            this.formulaAddButton.Name = "formulaAddButton";
            this.formulaAddButton.Size = new System.Drawing.Size(75, 23);
            this.formulaAddButton.TabIndex = 5;
            this.formulaAddButton.Text = "&Add";
            this.formulaAddButton.UseVisualStyleBackColor = true;
            this.formulaAddButton.Click += new System.EventHandler(this.formulaAddButton_Click);
            // 
            // formulaDataGridView
            // 
            this.formulaDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.formulaDataGridView.Location = new System.Drawing.Point(15, 25);
            this.formulaDataGridView.Name = "formulaDataGridView";
            this.formulaDataGridView.Size = new System.Drawing.Size(172, 129);
            this.formulaDataGridView.TabIndex = 4;
            this.formulaDataGridView.GotFocus += new System.EventHandler(this.formulaDataGridView_GotFocus);
            // 
            // usageGroupBox
            // 
            this.usageGroupBox.Controls.Add(this.usageDataGridView);
            this.usageGroupBox.Controls.Add(this.usageDeleteButton);
            this.usageGroupBox.Controls.Add(this.usageAddButton);
            this.usageGroupBox.Controls.Add(this.usageCheckNoneButton);
            this.usageGroupBox.Controls.Add(this.usageCheckAllButton);
            this.usageGroupBox.Location = new System.Drawing.Point(16, 19);
            this.usageGroupBox.Name = "usageGroupBox";
            this.usageGroupBox.Size = new System.Drawing.Size(257, 197);
            this.usageGroupBox.TabIndex = 0;
            this.usageGroupBox.TabStop = false;
            this.usageGroupBox.Text = "Load Combination Usage";
            // 
            // usageDataGridView
            // 
            this.usageDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.usageDataGridView.Location = new System.Drawing.Point(6, 25);
            this.usageDataGridView.Name = "usageDataGridView";
            this.usageDataGridView.Size = new System.Drawing.Size(164, 159);
            this.usageDataGridView.TabIndex = 5;
            this.usageDataGridView.GotFocus += new System.EventHandler(this.usageDataGridView_GotFocus);
            this.usageDataGridView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.usageDataGridView_CellValueChanged);
            this.usageDataGridView.SelectionChanged += new System.EventHandler(this.usageDataGridView_SelectionChanged);
            // 
            // usageDeleteButton
            // 
            this.usageDeleteButton.Location = new System.Drawing.Point(176, 161);
            this.usageDeleteButton.Name = "usageDeleteButton";
            this.usageDeleteButton.Size = new System.Drawing.Size(75, 23);
            this.usageDeleteButton.TabIndex = 4;
            this.usageDeleteButton.Text = "D&elete";
            this.usageDeleteButton.UseVisualStyleBackColor = true;
            this.usageDeleteButton.Click += new System.EventHandler(this.usageDeleteButton_Click);
            // 
            // usageAddButton
            // 
            this.usageAddButton.Location = new System.Drawing.Point(176, 115);
            this.usageAddButton.Name = "usageAddButton";
            this.usageAddButton.Size = new System.Drawing.Size(75, 23);
            this.usageAddButton.TabIndex = 3;
            this.usageAddButton.Text = "Ad&d";
            this.usageAddButton.UseVisualStyleBackColor = true;
            this.usageAddButton.Click += new System.EventHandler(this.usageAddButton_Click);
            // 
            // usageCheckNoneButton
            // 
            this.usageCheckNoneButton.Location = new System.Drawing.Point(176, 72);
            this.usageCheckNoneButton.Name = "usageCheckNoneButton";
            this.usageCheckNoneButton.Size = new System.Drawing.Size(75, 23);
            this.usageCheckNoneButton.TabIndex = 2;
            this.usageCheckNoneButton.Text = "Check &None";
            this.usageCheckNoneButton.UseVisualStyleBackColor = true;
            this.usageCheckNoneButton.Click += new System.EventHandler(this.usageCheckNoneButton_Click);
            // 
            // usageCheckAllButton
            // 
            this.usageCheckAllButton.Location = new System.Drawing.Point(176, 25);
            this.usageCheckAllButton.Name = "usageCheckAllButton";
            this.usageCheckAllButton.Size = new System.Drawing.Size(75, 23);
            this.usageCheckAllButton.TabIndex = 1;
            this.usageCheckAllButton.Text = "Check &All";
            this.usageCheckAllButton.UseVisualStyleBackColor = true;
            this.usageCheckAllButton.Click += new System.EventHandler(this.usageCheckAllButton_Click);
            // 
            // combinationInfoGroupBox
            // 
            this.combinationInfoGroupBox.Controls.Add(this.combinationDataGridView);
            this.combinationInfoGroupBox.Location = new System.Drawing.Point(18, 17);
            this.combinationInfoGroupBox.Name = "combinationInfoGroupBox";
            this.combinationInfoGroupBox.Size = new System.Drawing.Size(651, 180);
            this.combinationInfoGroupBox.TabIndex = 0;
            this.combinationInfoGroupBox.TabStop = false;
            this.combinationInfoGroupBox.Text = "Load Combination Information";
            // 
            // combinationDataGridView
            // 
            this.combinationDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.combinationDataGridView.Location = new System.Drawing.Point(16, 19);
            this.combinationDataGridView.Name = "combinationDataGridView";
            this.combinationDataGridView.Size = new System.Drawing.Size(616, 144);
            this.combinationDataGridView.TabIndex = 0;
            this.combinationDataGridView.GotFocus += new System.EventHandler(this.combinationDataGridView_GotFocus);
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(528, 493);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 1;
            this.okButton.Text = "&OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(632, 493);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // LoadsForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(723, 527);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoadsForm";
            this.ShowInTaskbar = false;
            this.Text = "Loads";
            this.Load += new System.EventHandler(this.LoadsForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.loadCasesTabPage.ResumeLayout(false);
            this.loadNaturesroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.loadNaturesDataGridView)).EndInit();
            this.loadCasesGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.loadCasesDataGridView)).EndInit();
            this.LoadCombinationsTabPage.ResumeLayout(false);
            this.combinationCreationGroupBox.ResumeLayout(false);
            this.combinationCreationGroupBox.PerformLayout();
            this.combinationFormulaGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.formulaDataGridView)).EndInit();
            this.usageGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.usageDataGridView)).EndInit();
            this.combinationInfoGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.combinationDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage loadCasesTabPage;
        private System.Windows.Forms.TabPage LoadCombinationsTabPage;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.GroupBox combinationCreationGroupBox;
        private System.Windows.Forms.GroupBox combinationInfoGroupBox;
        private System.Windows.Forms.GroupBox usageGroupBox;
        private System.Windows.Forms.DataGridView combinationDataGridView;
        private System.Windows.Forms.Button usageDeleteButton;
        private System.Windows.Forms.Button usageAddButton;
        private System.Windows.Forms.Button usageCheckNoneButton;
        private System.Windows.Forms.Button usageCheckAllButton;
        private System.Windows.Forms.GroupBox combinationFormulaGroupBox;
        private System.Windows.Forms.TextBox combinationNameTextBox;
        private System.Windows.Forms.Label combinnationNameLabel;
        private System.Windows.Forms.Label combinationTypeLabel;
        private System.Windows.Forms.Button deleteCombinationButton;
        private System.Windows.Forms.Button newCombinationButton;
        private System.Windows.Forms.ComboBox combinationStateComboBox;
        private System.Windows.Forms.Label combinationStateLabel;
        private System.Windows.Forms.ComboBox combinationTypeComboBox;
        private System.Windows.Forms.DataGridView formulaDataGridView;
        private System.Windows.Forms.Button formulaDeleteButton;
        private System.Windows.Forms.Button formulaAddButton;
        private System.Windows.Forms.GroupBox loadNaturesroupBox;
        private System.Windows.Forms.DataGridView loadNaturesDataGridView;
        private System.Windows.Forms.GroupBox loadCasesGroupBox;
        private System.Windows.Forms.DataGridView loadCasesDataGridView;
        private System.Windows.Forms.Button duplicateLoadCasesButton;
        private System.Windows.Forms.DataGridView usageDataGridView;
        private System.Windows.Forms.Button addLoadNaturesButton;
    }
}