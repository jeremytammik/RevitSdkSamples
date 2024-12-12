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

namespace Revit.SDK.Samples.TagBeam.CS
{
    partial class TagBeamForm
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
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.tagGroupBox = new System.Windows.Forms.GroupBox();
            this.tagSymbolLabel = new System.Windows.Forms.Label();
            this.tagLabel = new System.Windows.Forms.Label();
            this.tagSymbolComboBox = new System.Windows.Forms.ComboBox();
            this.tagComboBox = new System.Windows.Forms.ComboBox();
            this.tagOrientationComboBox = new System.Windows.Forms.ComboBox();
            this.tagOrientation = new System.Windows.Forms.Label();
            this.leadercheckBox = new System.Windows.Forms.CheckBox();
            this.tagGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(231, 160);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 4;
            this.okButton.Text = "&OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(312, 160);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // tagGroupBox
            // 
            this.tagGroupBox.Controls.Add(this.tagSymbolLabel);
            this.tagGroupBox.Controls.Add(this.tagLabel);
            this.tagGroupBox.Controls.Add(this.tagSymbolComboBox);
            this.tagGroupBox.Controls.Add(this.tagComboBox);
            this.tagGroupBox.ForeColor = System.Drawing.Color.Black;
            this.tagGroupBox.Location = new System.Drawing.Point(12, 12);
            this.tagGroupBox.Name = "tagGroupBox";
            this.tagGroupBox.Size = new System.Drawing.Size(375, 78);
            this.tagGroupBox.TabIndex = 7;
            this.tagGroupBox.TabStop = false;
            // 
            // tagSymbolLabel
            // 
            this.tagSymbolLabel.AutoSize = true;
            this.tagSymbolLabel.Location = new System.Drawing.Point(12, 48);
            this.tagSymbolLabel.Name = "tagSymbolLabel";
            this.tagSymbolLabel.Size = new System.Drawing.Size(56, 13);
            this.tagSymbolLabel.TabIndex = 5;
            this.tagSymbolLabel.Text = "Tag Type:";
            this.tagSymbolLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tagLabel
            // 
            this.tagLabel.AutoSize = true;
            this.tagLabel.Location = new System.Drawing.Point(12, 21);
            this.tagLabel.Name = "tagLabel";
            this.tagLabel.Size = new System.Drawing.Size(65, 13);
            this.tagLabel.TabIndex = 4;
            this.tagLabel.Text = "Tag Mode : ";
            this.tagLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tagSymbolComboBox
            // 
            this.tagSymbolComboBox.FormattingEnabled = true;
            this.tagSymbolComboBox.Location = new System.Drawing.Point(83, 45);
            this.tagSymbolComboBox.Name = "tagSymbolComboBox";
            this.tagSymbolComboBox.Size = new System.Drawing.Size(270, 21);
            this.tagSymbolComboBox.TabIndex = 3;
            // 
            // tagComboBox
            // 
            this.tagComboBox.FormattingEnabled = true;
            this.tagComboBox.Location = new System.Drawing.Point(83, 18);
            this.tagComboBox.Name = "tagComboBox";
            this.tagComboBox.Size = new System.Drawing.Size(270, 21);
            this.tagComboBox.TabIndex = 1;
            this.tagComboBox.SelectedIndexChanged += new System.EventHandler(this.tagComboBox_SelectedIndexChanged);
            // 
            // tagOrientationComboBox
            // 
            this.tagOrientationComboBox.FormattingEnabled = true;
            this.tagOrientationComboBox.Location = new System.Drawing.Point(112, 99);
            this.tagOrientationComboBox.Name = "tagOrientationComboBox";
            this.tagOrientationComboBox.Size = new System.Drawing.Size(253, 21);
            this.tagOrientationComboBox.TabIndex = 10;
            // 
            // tagOrientation
            // 
            this.tagOrientation.AutoSize = true;
            this.tagOrientation.Location = new System.Drawing.Point(23, 102);
            this.tagOrientation.Name = "tagOrientation";
            this.tagOrientation.Size = new System.Drawing.Size(83, 13);
            this.tagOrientation.TabIndex = 11;
            this.tagOrientation.Text = "Tag Orientation:";
            // 
            // leadercheckBox
            // 
            this.leadercheckBox.AutoSize = true;
            this.leadercheckBox.Checked = true;
            this.leadercheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.leadercheckBox.Location = new System.Drawing.Point(27, 135);
            this.leadercheckBox.Name = "leadercheckBox";
            this.leadercheckBox.Size = new System.Drawing.Size(84, 17);
            this.leadercheckBox.TabIndex = 12;
            this.leadercheckBox.Text = "Have leader";
            this.leadercheckBox.UseVisualStyleBackColor = true;
            // 
            // TagBeamForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(402, 195);
            this.Controls.Add(this.leadercheckBox);
            this.Controls.Add(this.tagOrientation);
            this.Controls.Add(this.tagOrientationComboBox);
            this.Controls.Add(this.tagGroupBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TagBeamForm";
            this.ShowInTaskbar = false;
            this.Text = "Tag Beam";
            this.tagGroupBox.ResumeLayout(false);
            this.tagGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.GroupBox tagGroupBox;
        private System.Windows.Forms.ComboBox tagComboBox;
        private System.Windows.Forms.Label tagSymbolLabel;
        private System.Windows.Forms.Label tagLabel;
        private System.Windows.Forms.ComboBox tagSymbolComboBox;
        private System.Windows.Forms.ComboBox tagOrientationComboBox;
        private System.Windows.Forms.Label tagOrientation;
        private System.Windows.Forms.CheckBox leadercheckBox;
       
    }
}