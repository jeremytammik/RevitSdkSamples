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

namespace Revit.SDK.Samples.ViewPrinter.CS
{
    partial class viewSheetSetForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.viewSheetSetNameComboBox = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.showViewsCheckBox = new System.Windows.Forms.CheckBox();
            this.showSheetsCheckBox = new System.Windows.Forms.CheckBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.saveAsButton = new System.Windows.Forms.Button();
            this.revertButton = new System.Windows.Forms.Button();
            this.reNameButton = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.checkAllButton = new System.Windows.Forms.Button();
            this.checkNoneButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.viewSheetSetListView = new System.Windows.Forms.ListView();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name:";
            // 
            // viewSheetSetNameComboBox
            // 
            this.viewSheetSetNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.viewSheetSetNameComboBox.FormattingEnabled = true;
            this.viewSheetSetNameComboBox.Location = new System.Drawing.Point(56, 18);
            this.viewSheetSetNameComboBox.Name = "viewSheetSetNameComboBox";
            this.viewSheetSetNameComboBox.Size = new System.Drawing.Size(243, 21);
            this.viewSheetSetNameComboBox.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.showViewsCheckBox);
            this.groupBox1.Controls.Add(this.showSheetsCheckBox);
            this.groupBox1.Location = new System.Drawing.Point(12, 293);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(287, 61);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Show";
            // 
            // showViewsCheckBox
            // 
            this.showViewsCheckBox.AutoSize = true;
            this.showViewsCheckBox.Checked = true;
            this.showViewsCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showViewsCheckBox.Location = new System.Drawing.Point(118, 28);
            this.showViewsCheckBox.Name = "showViewsCheckBox";
            this.showViewsCheckBox.Size = new System.Drawing.Size(54, 17);
            this.showViewsCheckBox.TabIndex = 7;
            this.showViewsCheckBox.Text = "&Views";
            this.showViewsCheckBox.UseVisualStyleBackColor = true;
            this.showViewsCheckBox.CheckedChanged += new System.EventHandler(this.showViewsCheckBox_CheckedChanged);
            // 
            // showSheetsCheckBox
            // 
            this.showSheetsCheckBox.AutoSize = true;
            this.showSheetsCheckBox.Checked = true;
            this.showSheetsCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showSheetsCheckBox.Location = new System.Drawing.Point(6, 28);
            this.showSheetsCheckBox.Name = "showSheetsCheckBox";
            this.showSheetsCheckBox.Size = new System.Drawing.Size(57, 17);
            this.showSheetsCheckBox.TabIndex = 7;
            this.showSheetsCheckBox.Text = "s&heets";
            this.showSheetsCheckBox.UseVisualStyleBackColor = true;
            this.showSheetsCheckBox.CheckedChanged += new System.EventHandler(this.showSheetsCheckBox_CheckedChanged);
            // 
            // saveButton
            // 
            this.saveButton.Enabled = false;
            this.saveButton.Location = new System.Drawing.Point(328, 21);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(172, 23);
            this.saveButton.TabIndex = 5;
            this.saveButton.Text = "&Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // saveAsButton
            // 
            this.saveAsButton.Location = new System.Drawing.Point(328, 50);
            this.saveAsButton.Name = "saveAsButton";
            this.saveAsButton.Size = new System.Drawing.Size(172, 23);
            this.saveAsButton.TabIndex = 5;
            this.saveAsButton.Text = "Sa&veAs...";
            this.saveAsButton.UseVisualStyleBackColor = true;
            this.saveAsButton.Click += new System.EventHandler(this.saveAsButton_Click);
            // 
            // revertButton
            // 
            this.revertButton.Enabled = false;
            this.revertButton.Location = new System.Drawing.Point(328, 79);
            this.revertButton.Name = "revertButton";
            this.revertButton.Size = new System.Drawing.Size(172, 23);
            this.revertButton.TabIndex = 5;
            this.revertButton.Text = "&Revert";
            this.revertButton.UseVisualStyleBackColor = true;
            this.revertButton.Click += new System.EventHandler(this.revertButton_Click);
            // 
            // reNameButton
            // 
            this.reNameButton.Location = new System.Drawing.Point(328, 108);
            this.reNameButton.Name = "reNameButton";
            this.reNameButton.Size = new System.Drawing.Size(172, 23);
            this.reNameButton.TabIndex = 5;
            this.reNameButton.Text = "Ren&ame";
            this.reNameButton.UseVisualStyleBackColor = true;
            this.reNameButton.Click += new System.EventHandler(this.reNameButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Location = new System.Drawing.Point(328, 137);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(172, 23);
            this.deleteButton.TabIndex = 5;
            this.deleteButton.Text = "&Delete";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            // 
            // checkAllButton
            // 
            this.checkAllButton.Location = new System.Drawing.Point(328, 186);
            this.checkAllButton.Name = "checkAllButton";
            this.checkAllButton.Size = new System.Drawing.Size(172, 23);
            this.checkAllButton.TabIndex = 5;
            this.checkAllButton.Text = "&Check All";
            this.checkAllButton.UseVisualStyleBackColor = true;
            this.checkAllButton.Click += new System.EventHandler(this.checkAllButton_Click);
            // 
            // checkNoneButton
            // 
            this.checkNoneButton.Location = new System.Drawing.Point(328, 215);
            this.checkNoneButton.Name = "checkNoneButton";
            this.checkNoneButton.Size = new System.Drawing.Size(172, 23);
            this.checkNoneButton.TabIndex = 5;
            this.checkNoneButton.Text = "Check &None";
            this.checkNoneButton.UseVisualStyleBackColor = true;
            this.checkNoneButton.Click += new System.EventHandler(this.checkNoneButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(425, 370);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 6;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(344, 370);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 6;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // viewSheetSetListView
            // 
            this.viewSheetSetListView.CheckBoxes = true;
            this.viewSheetSetListView.Location = new System.Drawing.Point(12, 45);
            this.viewSheetSetListView.Name = "viewSheetSetListView";
            this.viewSheetSetListView.Size = new System.Drawing.Size(287, 242);
            this.viewSheetSetListView.Sorting = System.Windows.Forms.SortOrder.Descending;
            this.viewSheetSetListView.TabIndex = 7;
            this.viewSheetSetListView.UseCompatibleStateImageBehavior = false;
            this.viewSheetSetListView.View = System.Windows.Forms.View.List;
            // 
            // viewSheetSetForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(512, 405);
            this.Controls.Add(this.viewSheetSetListView);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.checkNoneButton);
            this.Controls.Add(this.checkAllButton);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.reNameButton);
            this.Controls.Add(this.revertButton);
            this.Controls.Add(this.saveAsButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.viewSheetSetNameComboBox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "viewSheetSetForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "View/Sheet Set";
            this.Load += new System.EventHandler(this.ViewSheetSetForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox viewSheetSetNameComboBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button saveAsButton;
        private System.Windows.Forms.Button revertButton;
        private System.Windows.Forms.Button reNameButton;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Button checkAllButton;
        private System.Windows.Forms.Button checkNoneButton;
        private System.Windows.Forms.CheckBox showViewsCheckBox;
        private System.Windows.Forms.CheckBox showSheetsCheckBox;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.ListView viewSheetSetListView;
    }
}