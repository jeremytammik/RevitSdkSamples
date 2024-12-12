//
// (C) Copyright 2003-2013 by Autodesk, Inc.
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


namespace Revit.SDK.Samples.VersionChecking.CS
{
    partial class versionCheckingForm
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
            this.versionInformationTextBox = new System.Windows.Forms.TextBox();
            this.versionInformation = new System.Windows.Forms.GroupBox();
            this.closeButton = new System.Windows.Forms.Button();
            this.versionInformation.SuspendLayout();
            this.SuspendLayout();
            // 
            // versionInformationTextBox
            // 
            this.versionInformationTextBox.BackColor = System.Drawing.SystemColors.MenuBar;
            this.versionInformationTextBox.Location = new System.Drawing.Point(6, 21);
            this.versionInformationTextBox.Multiline = true;
            this.versionInformationTextBox.Name = "versionInformationTextBox";
            this.versionInformationTextBox.Size = new System.Drawing.Size(304, 157);
            this.versionInformationTextBox.TabIndex = 0;
            // 
            // versionInformation
            // 
            this.versionInformation.Controls.Add(this.versionInformationTextBox);
            this.versionInformation.Location = new System.Drawing.Point(12, 21);
            this.versionInformation.Name = "versionInformation";
            this.versionInformation.Size = new System.Drawing.Size(318, 184);
            this.versionInformation.TabIndex = 1;
            this.versionInformation.TabStop = false;
            this.versionInformation.Text = "Version Information";
            // 
            // closeButton
            // 
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeButton.Location = new System.Drawing.Point(247, 215);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 2;
            this.closeButton.Text = "&Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // versionCheckingForm
            // 
            this.AcceptButton = this.closeButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.closeButton;
            this.ClientSize = new System.Drawing.Size(339, 246);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.versionInformation);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "versionCheckingForm";
            this.ShowInTaskbar = false;
            this.Text = "About Revit";
            this.Load += new System.EventHandler(this.VersionCheckingForm_Load);
            this.versionInformation.ResumeLayout(false);
            this.versionInformation.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox versionInformationTextBox;
        private System.Windows.Forms.GroupBox versionInformation;
        private System.Windows.Forms.Button closeButton;
    }
}