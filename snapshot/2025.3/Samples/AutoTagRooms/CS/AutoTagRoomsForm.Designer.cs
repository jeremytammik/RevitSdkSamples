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

namespace Revit.SDK.Samples.AutoTagRooms.CS
{
    /// <summary>
    /// The graphic user interface of auto tag rooms
    /// </summary>
    partial class AutoTagRoomsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private RoomsData m_roomsData;

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
            this.levelsComboBox = new System.Windows.Forms.ComboBox();
            this.levelLabel = new System.Windows.Forms.Label();
            this.tagTypeLabel = new System.Windows.Forms.Label();
            this.tagTypesComboBox = new System.Windows.Forms.ComboBox();
            this.roomsListView = new System.Windows.Forms.ListView();
            this.autoTagButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // levelsComboBox
            // 
            this.levelsComboBox.FormattingEnabled = true;
            this.levelsComboBox.Location = new System.Drawing.Point(96, 6);
            this.levelsComboBox.Name = "levelsComboBox";
            this.levelsComboBox.Size = new System.Drawing.Size(175, 21);
            this.levelsComboBox.Sorted = true;
            this.levelsComboBox.TabIndex = 0;
            this.levelsComboBox.SelectedIndexChanged += new System.EventHandler(this.levelsComboBox_SelectedIndexChanged);
            // 
            // levelLabel
            // 
            this.levelLabel.AutoSize = true;
            this.levelLabel.Location = new System.Drawing.Point(12, 9);
            this.levelLabel.Name = "levelLabel";
            this.levelLabel.Size = new System.Drawing.Size(36, 13);
            this.levelLabel.TabIndex = 1;
            this.levelLabel.Text = "Level:";
            // 
            // tagTypeLabel
            // 
            this.tagTypeLabel.AutoSize = true;
            this.tagTypeLabel.Location = new System.Drawing.Point(9, 36);
            this.tagTypeLabel.Name = "tagTypeLabel";
            this.tagTypeLabel.Size = new System.Drawing.Size(56, 13);
            this.tagTypeLabel.TabIndex = 2;
            this.tagTypeLabel.Text = "Tag Type:";
            // 
            // tagTypesComboBox
            // 
            this.tagTypesComboBox.FormattingEnabled = true;
            this.tagTypesComboBox.Location = new System.Drawing.Point(96, 33);
            this.tagTypesComboBox.Name = "tagTypesComboBox";
            this.tagTypesComboBox.Size = new System.Drawing.Size(175, 21);
            this.tagTypesComboBox.TabIndex = 3;
            // 
            // roomsListView
            // 
            this.roomsListView.Location = new System.Drawing.Point(11, 65);
            this.roomsListView.Name = "roomsListView";
            this.roomsListView.Size = new System.Drawing.Size(499, 175);
            this.roomsListView.TabIndex = 4;
            this.roomsListView.UseCompatibleStateImageBehavior = false;
            this.roomsListView.View = System.Windows.Forms.View.Details;
            // 
            // autoTagButton
            // 
            this.autoTagButton.Location = new System.Drawing.Point(354, 36);
            this.autoTagButton.Name = "autoTagButton";
            this.autoTagButton.Size = new System.Drawing.Size(156, 23);
            this.autoTagButton.TabIndex = 5;
            this.autoTagButton.Text = "&Auto Tag All";
            this.autoTagButton.UseVisualStyleBackColor = true;
            this.autoTagButton.Click += new System.EventHandler(this.autoTagButton_Click);
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(355, 246);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 6;
            this.okButton.Text = "&OK";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(436, 246);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 7;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // AutoTagRoomsForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(523, 274);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.autoTagButton);
            this.Controls.Add(this.roomsListView);
            this.Controls.Add(this.tagTypesComboBox);
            this.Controls.Add(this.tagTypeLabel);
            this.Controls.Add(this.levelLabel);
            this.Controls.Add(this.levelsComboBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AutoTagRoomsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Room Tags";
            this.Load += new System.EventHandler(this.AutoTagRoomsForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox levelsComboBox;
        private System.Windows.Forms.Label levelLabel;
        private System.Windows.Forms.Label tagTypeLabel;
        private System.Windows.Forms.ComboBox tagTypesComboBox;
        private System.Windows.Forms.ListView roomsListView;
        private System.Windows.Forms.Button autoTagButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
    }
}