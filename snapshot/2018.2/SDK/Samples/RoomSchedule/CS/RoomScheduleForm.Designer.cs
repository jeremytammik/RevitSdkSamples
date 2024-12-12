//
// (C) Copyright 2003-2017 by Autodesk, Inc.
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

namespace Revit.SDK.Samples.RoomSchedule
{
    /// <summary>
    /// Room Schedule form, used to retrieve data from .xls data source and create new rooms.
    /// </summary>
    partial class RoomScheduleForm
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
            this.importRoomButton = new System.Windows.Forms.Button();
            this.sheetDataGridView = new System.Windows.Forms.DataGridView();
            this.tableLabel = new System.Windows.Forms.Label();
            this.tablesComboBox = new System.Windows.Forms.ComboBox();
            this.levelLabel = new System.Windows.Forms.Label();
            this.revitRoomDataGridView = new System.Windows.Forms.DataGridView();
            this.levelComboBox = new System.Windows.Forms.ComboBox();
            this.roomsGroupBox = new System.Windows.Forms.GroupBox();
            this.clearIDButton = new System.Windows.Forms.Button();
            this.showAllRoomsCheckBox = new System.Windows.Forms.CheckBox();
            this.newRoomButton = new System.Windows.Forms.Button();
            this.phaseComboBox = new System.Windows.Forms.ComboBox();
            this.Phase = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.roomExcelTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.closeButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.sheetDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.revitRoomDataGridView)).BeginInit();
            this.roomsGroupBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // importRoomButton
            // 
            this.importRoomButton.Location = new System.Drawing.Point(595, 17);
            this.importRoomButton.Name = "importRoomButton";
            this.importRoomButton.Size = new System.Drawing.Size(109, 23);
            this.importRoomButton.TabIndex = 1;
            this.importRoomButton.Text = "&Import Excel...";
            this.importRoomButton.UseVisualStyleBackColor = true;
            this.importRoomButton.Click += new System.EventHandler(this.importRoomButton_Click);
            // 
            // sheetDataGridView
            // 
            this.sheetDataGridView.AllowUserToAddRows = false;
            this.sheetDataGridView.AllowUserToDeleteRows = false;
            this.sheetDataGridView.AllowUserToResizeRows = false;
            this.sheetDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.sheetDataGridView.Location = new System.Drawing.Point(5, 48);
            this.sheetDataGridView.Name = "sheetDataGridView";
            this.sheetDataGridView.ReadOnly = true;
            this.sheetDataGridView.RowHeadersVisible = false;
            this.sheetDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.sheetDataGridView.Size = new System.Drawing.Size(699, 155);
            this.sheetDataGridView.TabIndex = 1;
            // 
            // tableLabel
            // 
            this.tableLabel.AutoSize = true;
            this.tableLabel.Location = new System.Drawing.Point(6, 22);
            this.tableLabel.Name = "tableLabel";
            this.tableLabel.Size = new System.Drawing.Size(66, 13);
            this.tableLabel.TabIndex = 2;
            this.tableLabel.Text = "Room Sheet";
            // 
            // tablesComboBox
            // 
            this.tablesComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tablesComboBox.FormattingEnabled = true;
            this.tablesComboBox.Location = new System.Drawing.Point(78, 19);
            this.tablesComboBox.Name = "tablesComboBox";
            this.tablesComboBox.Size = new System.Drawing.Size(187, 21);
            this.tablesComboBox.TabIndex = 0;
            this.tablesComboBox.SelectedIndexChanged += new System.EventHandler(this.tablesComboBox_SelectedIndexChanged);
            // 
            // levelLabel
            // 
            this.levelLabel.AutoSize = true;
            this.levelLabel.Location = new System.Drawing.Point(39, 19);
            this.levelLabel.Name = "levelLabel";
            this.levelLabel.Size = new System.Drawing.Size(33, 13);
            this.levelLabel.TabIndex = 2;
            this.levelLabel.Text = "Level";
            // 
            // revitRoomDataGridView
            // 
            this.revitRoomDataGridView.AllowUserToAddRows = false;
            this.revitRoomDataGridView.AllowUserToDeleteRows = false;
            this.revitRoomDataGridView.AllowUserToResizeRows = false;
            this.revitRoomDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.revitRoomDataGridView.Location = new System.Drawing.Point(6, 42);
            this.revitRoomDataGridView.Name = "revitRoomDataGridView";
            this.revitRoomDataGridView.ReadOnly = true;
            this.revitRoomDataGridView.RowHeadersVisible = false;
            this.revitRoomDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.revitRoomDataGridView.Size = new System.Drawing.Size(699, 169);
            this.revitRoomDataGridView.TabIndex = 1;
            // 
            // levelComboBox
            // 
            this.levelComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.levelComboBox.FormattingEnabled = true;
            this.levelComboBox.Location = new System.Drawing.Point(78, 16);
            this.levelComboBox.Name = "levelComboBox";
            this.levelComboBox.Size = new System.Drawing.Size(187, 21);
            this.levelComboBox.Sorted = true;
            this.levelComboBox.TabIndex = 0;
            this.levelComboBox.SelectedIndexChanged += new System.EventHandler(this.levelComboBox_SelectedIndexChanged);
            // 
            // roomsGroupBox
            // 
            this.roomsGroupBox.Controls.Add(this.clearIDButton);
            this.roomsGroupBox.Controls.Add(this.showAllRoomsCheckBox);
            this.roomsGroupBox.Controls.Add(this.revitRoomDataGridView);
            this.roomsGroupBox.Controls.Add(this.levelComboBox);
            this.roomsGroupBox.Controls.Add(this.levelLabel);
            this.roomsGroupBox.Location = new System.Drawing.Point(12, 256);
            this.roomsGroupBox.Name = "roomsGroupBox";
            this.roomsGroupBox.Size = new System.Drawing.Size(711, 220);
            this.roomsGroupBox.TabIndex = 1;
            this.roomsGroupBox.TabStop = false;
            this.roomsGroupBox.Text = "Revit Rooms";
            // 
            // clearIDButton
            // 
            this.clearIDButton.Location = new System.Drawing.Point(560, 13);
            this.clearIDButton.Name = "clearIDButton";
            this.clearIDButton.Size = new System.Drawing.Size(144, 23);
            this.clearIDButton.TabIndex = 3;
            this.clearIDButton.Text = "Clear &External Room ID";
            this.clearIDButton.UseVisualStyleBackColor = true;
            this.clearIDButton.Click += new System.EventHandler(this.clearIDButton_Click);
            // 
            // showAllRoomsCheckBox
            // 
            this.showAllRoomsCheckBox.AutoSize = true;
            this.showAllRoomsCheckBox.Location = new System.Drawing.Point(277, 18);
            this.showAllRoomsCheckBox.Name = "showAllRoomsCheckBox";
            this.showAllRoomsCheckBox.Size = new System.Drawing.Size(103, 17);
            this.showAllRoomsCheckBox.TabIndex = 2;
            this.showAllRoomsCheckBox.Text = "&Show All Rooms";
            this.showAllRoomsCheckBox.UseVisualStyleBackColor = true;
            this.showAllRoomsCheckBox.CheckedChanged += new System.EventHandler(this.showAllRoomsCheckBox_CheckedChanged);
            // 
            // newRoomButton
            // 
            this.newRoomButton.Location = new System.Drawing.Point(271, 209);
            this.newRoomButton.Name = "newRoomButton";
            this.newRoomButton.Size = new System.Drawing.Size(136, 23);
            this.newRoomButton.TabIndex = 3;
            this.newRoomButton.Text = "Create Unplaced &Rooms";
            this.newRoomButton.UseVisualStyleBackColor = true;
            this.newRoomButton.Click += new System.EventHandler(this.newRoomButton_Click);
            // 
            // phaseComboBox
            // 
            this.phaseComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.phaseComboBox.FormattingEnabled = true;
            this.phaseComboBox.Location = new System.Drawing.Point(78, 209);
            this.phaseComboBox.Name = "phaseComboBox";
            this.phaseComboBox.Size = new System.Drawing.Size(187, 21);
            this.phaseComboBox.Sorted = true;
            this.phaseComboBox.TabIndex = 2;
            // 
            // Phase
            // 
            this.Phase.AutoSize = true;
            this.Phase.Location = new System.Drawing.Point(32, 214);
            this.Phase.Name = "Phase";
            this.Phase.Size = new System.Drawing.Size(40, 13);
            this.Phase.TabIndex = 2;
            this.Phase.Text = "Phase ";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.roomExcelTextBox);
            this.groupBox1.Controls.Add(this.tablesComboBox);
            this.groupBox1.Controls.Add(this.newRoomButton);
            this.groupBox1.Controls.Add(this.phaseComboBox);
            this.groupBox1.Controls.Add(this.tableLabel);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.Phase);
            this.groupBox1.Controls.Add(this.sheetDataGridView);
            this.groupBox1.Controls.Add(this.importRoomButton);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(711, 238);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Spreadsheet Rooms Information";
            // 
            // roomExcelTextBox
            // 
            this.roomExcelTextBox.Location = new System.Drawing.Point(271, 19);
            this.roomExcelTextBox.Name = "roomExcelTextBox";
            this.roomExcelTextBox.ReadOnly = true;
            this.roomExcelTextBox.Size = new System.Drawing.Size(318, 20);
            this.roomExcelTextBox.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 2;
            // 
            // closeButton
            // 
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeButton.Location = new System.Drawing.Point(648, 482);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 2;
            this.closeButton.Text = "&Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // RoomScheduleForm
            // 
            this.AcceptButton = this.closeButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.closeButton;
            this.ClientSize = new System.Drawing.Size(735, 512);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.roomsGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RoomScheduleForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Room Schedule";
            ((System.ComponentModel.ISupportInitialize)(this.sheetDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.revitRoomDataGridView)).EndInit();
            this.roomsGroupBox.ResumeLayout(false);
            this.roomsGroupBox.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button importRoomButton;
        private System.Windows.Forms.DataGridView sheetDataGridView;
        private System.Windows.Forms.Label tableLabel;
        private System.Windows.Forms.ComboBox tablesComboBox;
        private System.Windows.Forms.Label levelLabel;
        private System.Windows.Forms.DataGridView revitRoomDataGridView;
        private System.Windows.Forms.ComboBox levelComboBox;
        private System.Windows.Forms.GroupBox roomsGroupBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Button newRoomButton;
        private System.Windows.Forms.ComboBox phaseComboBox;
        private System.Windows.Forms.Label Phase;
        private System.Windows.Forms.CheckBox showAllRoomsCheckBox;
        private System.Windows.Forms.Button clearIDButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox roomExcelTextBox;
    }
}

