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


namespace Revit.SDK.Samples.BoundaryConditions.CS
{
    partial class SpringModulusForm
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
            this.springModulusTextBox = new System.Windows.Forms.TextBox();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.springModulusLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // springModulusTextBox
            // 
            this.springModulusTextBox.Location = new System.Drawing.Point(14, 47);
            this.springModulusTextBox.Name = "springModulusTextBox";
            this.springModulusTextBox.Size = new System.Drawing.Size(263, 22);
            this.springModulusTextBox.TabIndex = 0;
            this.springModulusTextBox.Leave += new System.EventHandler(this.springModulusTextBox_Leave);
            this.springModulusTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.springModulusTextBox_KeyDown);
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(108, 95);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(77, 28);
            this.okButton.TabIndex = 1;
            this.okButton.Text = "&OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(200, 95);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(77, 28);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // springModulusLabel
            // 
            this.springModulusLabel.AutoSize = true;
            this.springModulusLabel.Location = new System.Drawing.Point(12, 18);
            this.springModulusLabel.Name = "springModulusLabel";
            this.springModulusLabel.Size = new System.Drawing.Size(204, 17);
            this.springModulusLabel.TabIndex = 3;
            this.springModulusLabel.Text = "Please enter a positive number";
            // 
            // SpringModulusForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(298, 138);
            this.Controls.Add(this.springModulusLabel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.springModulusTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SpringModulusForm";
            this.ShowInTaskbar = false;
            this.Text = "Spring Modulus";
            this.Load += new System.EventHandler(this.SpringModulusForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox springModulusTextBox;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label springModulusLabel;
    }
}