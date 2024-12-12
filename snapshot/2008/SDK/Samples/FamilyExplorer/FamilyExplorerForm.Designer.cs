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

namespace Revit.SDK.Samples.FamilyExplorer.CS
{
    partial class FamilyExplorerForm
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
            this.previewPictureBox = new System.Windows.Forms.PictureBox();
            this.familiesListBox = new System.Windows.Forms.ListBox();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.previewPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // previewPictureBox
            // 
            this.previewPictureBox.BackColor = System.Drawing.Color.White;
            this.previewPictureBox.Location = new System.Drawing.Point(12, 12);
            this.previewPictureBox.Name = "previewPictureBox";
            this.previewPictureBox.Size = new System.Drawing.Size(327, 317);
            this.previewPictureBox.TabIndex = 0;
            this.previewPictureBox.TabStop = false;
            this.previewPictureBox.DoubleClick += new System.EventHandler(this.previewPictureBox_DoubleClick);
            this.previewPictureBox.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.previewPictureBox_MouseWheel);
            this.previewPictureBox.MouseLeave += new System.EventHandler(this.previewPictureBox_MouseLeave);
            this.previewPictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.previewPictureBox_MouseDown);
            this.previewPictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.previewPictureBox_MouseMove);
            this.previewPictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.previewPictureBox_Paint);
            this.previewPictureBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.previewPictureBox_KeyUp);
            this.previewPictureBox.MouseHover += new System.EventHandler(this.previewPictureBox_MouseHover);
            this.previewPictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.previewPictureBox_MouseUp);
            this.previewPictureBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.previewPictureBox_KeyDown);
            // 
            // familiesListBox
            // 
            this.familiesListBox.FormattingEnabled = true;
            this.familiesListBox.Location = new System.Drawing.Point(345, 12);
            this.familiesListBox.Name = "familiesListBox";
            this.familiesListBox.Size = new System.Drawing.Size(192, 316);
            this.familiesListBox.Sorted = true;
            this.familiesListBox.TabIndex = 1;
            this.familiesListBox.DoubleClick += new System.EventHandler(this.familiesListBox_DoubleClick);
            this.familiesListBox.SelectedIndexChanged += new System.EventHandler(this.familiesListBox_SelectedIndexChanged);
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(381, 334);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 2;
            this.okButton.Text = "&OK";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(462, 334);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // FamilyExplorerForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(549, 363);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.familiesListBox);
            this.Controls.Add(this.previewPictureBox);
            this.DoubleBuffered = true;
            this.MinimizeBox = false;
            this.Name = "FamilyExplorerForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Family Explorer";
            this.SizeChanged += new System.EventHandler(this.FamilyExplorerForm_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.previewPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox previewPictureBox;
        private System.Windows.Forms.ListBox familiesListBox;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
    }
}