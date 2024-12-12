//
// (C) Copyright 2003-2008 by Autodesk, Inc.
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

namespace Revit.SDK.Samples.AllViews.CS
{
    partial class AllViewsForm
    {
        private ViewsMgr m_data;
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
            this.allViewsGroupBox = new System.Windows.Forms.GroupBox();
            this.allViewsTreeView = new System.Windows.Forms.TreeView();
            this.GenerateSheetGroupBox = new System.Windows.Forms.GroupBox();
            this.titleBlocksListBox = new System.Windows.Forms.ListBox();
            this.sheetNameLabel = new System.Windows.Forms.Label();
            this.sheetNameTextBox = new System.Windows.Forms.TextBox();
            this.titleBlocksLabel = new System.Windows.Forms.Label();
            this.cancelButton = new System.Windows.Forms.Button();
            this.oKButton = new System.Windows.Forms.Button();
            this.allViewsGroupBox.SuspendLayout();
            this.GenerateSheetGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // allViewsGroupBox
            // 
            this.allViewsGroupBox.Controls.Add(this.allViewsTreeView);
            this.allViewsGroupBox.Location = new System.Drawing.Point(12, 12);
            this.allViewsGroupBox.Name = "allViewsGroupBox";
            this.allViewsGroupBox.Size = new System.Drawing.Size(212, 242);
            this.allViewsGroupBox.TabIndex = 0;
            this.allViewsGroupBox.TabStop = false;
            this.allViewsGroupBox.Text = "All Views";
            // 
            // allViewsTreeView
            // 
            this.allViewsTreeView.CheckBoxes = true;
            this.allViewsTreeView.Location = new System.Drawing.Point(6, 19);
            this.allViewsTreeView.Name = "allViewsTreeView";
            this.allViewsTreeView.Size = new System.Drawing.Size(200, 217);
            this.allViewsTreeView.TabIndex = 0;
            this.allViewsTreeView.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.allViewsTreeView_AfterCheck);
            // 
            // GenerateSheetGroupBox
            // 
            this.GenerateSheetGroupBox.Controls.Add(this.titleBlocksListBox);
            this.GenerateSheetGroupBox.Controls.Add(this.sheetNameLabel);
            this.GenerateSheetGroupBox.Controls.Add(this.sheetNameTextBox);
            this.GenerateSheetGroupBox.Controls.Add(this.titleBlocksLabel);
            this.GenerateSheetGroupBox.Controls.Add(this.cancelButton);
            this.GenerateSheetGroupBox.Controls.Add(this.oKButton);
            this.GenerateSheetGroupBox.Location = new System.Drawing.Point(267, 12);
            this.GenerateSheetGroupBox.Name = "GenerateSheetGroupBox";
            this.GenerateSheetGroupBox.Size = new System.Drawing.Size(172, 236);
            this.GenerateSheetGroupBox.TabIndex = 1;
            this.GenerateSheetGroupBox.TabStop = false;
            this.GenerateSheetGroupBox.Text = "Generate Sheet";
            // 
            // titleBlocksListBox
            // 
            this.titleBlocksListBox.FormattingEnabled = true;
            this.titleBlocksListBox.Location = new System.Drawing.Point(9, 44);
            this.titleBlocksListBox.Name = "titleBlocksListBox";
            this.titleBlocksListBox.Size = new System.Drawing.Size(157, 108);
            this.titleBlocksListBox.Sorted = true;
            this.titleBlocksListBox.TabIndex = 6;
            this.titleBlocksListBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.titleBlocksListBox_MouseClick);
            // 
            // sheetNameLabel
            // 
            this.sheetNameLabel.AutoSize = true;
            this.sheetNameLabel.Location = new System.Drawing.Point(6, 170);
            this.sheetNameLabel.Name = "sheetNameLabel";
            this.sheetNameLabel.Size = new System.Drawing.Size(66, 13);
            this.sheetNameLabel.TabIndex = 5;
            this.sheetNameLabel.Text = "Sheet Name";
            // 
            // sheetNameTextBox
            // 
            this.sheetNameTextBox.Location = new System.Drawing.Point(78, 167);
            this.sheetNameTextBox.Name = "sheetNameTextBox";
            this.sheetNameTextBox.Size = new System.Drawing.Size(88, 20);
            this.sheetNameTextBox.TabIndex = 2;
            this.sheetNameTextBox.Text = "Unname";
            // 
            // titleBlocksLabel
            // 
            this.titleBlocksLabel.AutoSize = true;
            this.titleBlocksLabel.Location = new System.Drawing.Point(6, 19);
            this.titleBlocksLabel.Name = "titleBlocksLabel";
            this.titleBlocksLabel.Size = new System.Drawing.Size(59, 13);
            this.titleBlocksLabel.TabIndex = 3;
            this.titleBlocksLabel.Text = "TitleBlocks";
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(91, 207);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // oKButton
            // 
            this.oKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.oKButton.Location = new System.Drawing.Point(6, 207);
            this.oKButton.Name = "oKButton";
            this.oKButton.Size = new System.Drawing.Size(75, 23);
            this.oKButton.TabIndex = 3;
            this.oKButton.Text = "&OK";
            this.oKButton.UseVisualStyleBackColor = true;
            this.oKButton.Click += new System.EventHandler(this.oKButton_Click);
            // 
            // AllViewsForm
            // 
            this.AcceptButton = this.oKButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(459, 266);
            this.Controls.Add(this.GenerateSheetGroupBox);
            this.Controls.Add(this.allViewsGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AllViewsForm";
            this.ShowInTaskbar = false;
            this.Text = "All Views";
            this.Load += new System.EventHandler(this.AllViewsForm_Load);
            this.allViewsGroupBox.ResumeLayout(false);
            this.GenerateSheetGroupBox.ResumeLayout(false);
            this.GenerateSheetGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox allViewsGroupBox;
        private System.Windows.Forms.GroupBox GenerateSheetGroupBox;
        private System.Windows.Forms.Button oKButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.TreeView allViewsTreeView;
        private System.Windows.Forms.Label titleBlocksLabel;
        private System.Windows.Forms.TextBox sheetNameTextBox;
        private System.Windows.Forms.Label sheetNameLabel;
        private System.Windows.Forms.ListBox titleBlocksListBox;
    }
}