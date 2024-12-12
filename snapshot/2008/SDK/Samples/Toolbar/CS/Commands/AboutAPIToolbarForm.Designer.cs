//
// (C) Copyright 2003-2007 by Autodesk, Inc.
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

namespace Revit.SDK.Samples.Toolbar.CS
{
    partial class AboutAPIToolbarForm
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
            this.aboutToolbarRichTextBox = new System.Windows.Forms.RichTextBox();
            this.contactLinkLabel = new System.Windows.Forms.LinkLabel();
            this.okButton = new System.Windows.Forms.Button();
            this.aboutToolbarLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // aboutToolbarRichTextBox
            // 
            this.aboutToolbarRichTextBox.BackColor = System.Drawing.Color.White;
            this.aboutToolbarRichTextBox.Location = new System.Drawing.Point(9, 28);
            this.aboutToolbarRichTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.aboutToolbarRichTextBox.Name = "aboutToolbarRichTextBox";
            this.aboutToolbarRichTextBox.ReadOnly = true;
            this.aboutToolbarRichTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
            this.aboutToolbarRichTextBox.Size = new System.Drawing.Size(409, 226);
            this.aboutToolbarRichTextBox.TabIndex = 6;
            this.aboutToolbarRichTextBox.TabStop = false;
            this.aboutToolbarRichTextBox.Text = "";
            // 
            // contactLinkLabel
            // 
            this.contactLinkLabel.AutoSize = true;
            this.contactLinkLabel.LinkArea = new System.Windows.Forms.LinkArea(0, 32);
            this.contactLinkLabel.Location = new System.Drawing.Point(9, 270);
            this.contactLinkLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.contactLinkLabel.Name = "contactLinkLabel";
            this.contactLinkLabel.Size = new System.Drawing.Size(58, 17);
            this.contactLinkLabel.TabIndex = 2;
            this.contactLinkLabel.TabStop = true;
            this.contactLinkLabel.Text = "Contact us";
            this.contactLinkLabel.UseCompatibleTextRendering = true;
            this.contactLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.contactLinkLabel1_LinkClicked);
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(359, 270);
            this.okButton.Margin = new System.Windows.Forms.Padding(2);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(58, 24);
            this.okButton.TabIndex = 1;
            this.okButton.Text = "&OK";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // aboutToolbarLabel
            // 
            this.aboutToolbarLabel.AutoSize = true;
            this.aboutToolbarLabel.Location = new System.Drawing.Point(10, 11);
            this.aboutToolbarLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.aboutToolbarLabel.Name = "aboutToolbarLabel";
            this.aboutToolbarLabel.Size = new System.Drawing.Size(243, 13);
            this.aboutToolbarLabel.TabIndex = 8;
            this.aboutToolbarLabel.Text = "Introduce custom Toolbar functionality in RevitAPI";
            // 
            // AboutAPIToolbarForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(427, 306);
            this.Controls.Add(this.aboutToolbarLabel);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.contactLinkLabel);
            this.Controls.Add(this.aboutToolbarRichTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutAPIToolbarForm";
            this.ShowInTaskbar = false;
            this.Text = "About Toolbar";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox aboutToolbarRichTextBox;
        private System.Windows.Forms.LinkLabel contactLinkLabel;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Label aboutToolbarLabel;


    }
}