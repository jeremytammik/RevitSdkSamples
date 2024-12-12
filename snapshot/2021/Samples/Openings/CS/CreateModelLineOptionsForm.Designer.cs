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


namespace Revit.SDK.Samples.Openings.CS
{
    /// <summary>
    /// creat model line options form
    /// </summary>
    partial class CreateModelLineOptionsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed;
        /// otherwise, false.</param>
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
            this.CreateAllRadioButton = new System.Windows.Forms.RadioButton();
            this.CreateShaftRadioButton = new System.Windows.Forms.RadioButton();
            this.CreateDisplayRadioButton = new System.Windows.Forms.RadioButton();
            this.CreateButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.OptionsGroupBox = new System.Windows.Forms.GroupBox();
            this.OptionsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // CreateAllRadioButton
            // 
            this.CreateAllRadioButton.AutoSize = true;
            this.CreateAllRadioButton.Location = new System.Drawing.Point(17, 32);
            this.CreateAllRadioButton.Name = "CreateAllRadioButton";
            this.CreateAllRadioButton.Size = new System.Drawing.Size(197, 17);
            this.CreateAllRadioButton.TabIndex = 0;
            this.CreateAllRadioButton.TabStop = true;
            this.CreateAllRadioButton.Text = "Create X Model Line on all Openings";
            this.CreateAllRadioButton.UseVisualStyleBackColor = true;
            // 
            // CreateShaftRadioButton
            // 
            this.CreateShaftRadioButton.AutoSize = true;
            this.CreateShaftRadioButton.Location = new System.Drawing.Point(17, 66);
            this.CreateShaftRadioButton.Name = "CreateShaftRadioButton";
            this.CreateShaftRadioButton.Size = new System.Drawing.Size(225, 17);
            this.CreateShaftRadioButton.TabIndex = 1;
            this.CreateShaftRadioButton.TabStop = true;
            this.CreateShaftRadioButton.Text = "Create X Model Line on all Shaft Openings";
            this.CreateShaftRadioButton.UseVisualStyleBackColor = true;
            // 
            // CreateDisplayRadioButton
            // 
            this.CreateDisplayRadioButton.AutoSize = true;
            this.CreateDisplayRadioButton.Checked = true;
            this.CreateDisplayRadioButton.Location = new System.Drawing.Point(17, 105);
            this.CreateDisplayRadioButton.Name = "CreateDisplayRadioButton";
            this.CreateDisplayRadioButton.Size = new System.Drawing.Size(244, 17);
            this.CreateDisplayRadioButton.TabIndex = 2;
            this.CreateDisplayRadioButton.TabStop = true;
            this.CreateDisplayRadioButton.Text = "Create X Model Line on the displayed Opening";
            this.CreateDisplayRadioButton.UseVisualStyleBackColor = true;
            // 
            // CreateButton
            // 
            this.CreateButton.Location = new System.Drawing.Point(141, 164);
            this.CreateButton.Name = "CreateButton";
            this.CreateButton.Size = new System.Drawing.Size(75, 23);
            this.CreateButton.TabIndex = 3;
            this.CreateButton.Text = "C&reate";
            this.CreateButton.UseVisualStyleBackColor = true;
            this.CreateButton.Click += new System.EventHandler(this.CreateButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(222, 164);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // OptionsGroupBox
            // 
            this.OptionsGroupBox.Controls.Add(this.CreateAllRadioButton);
            this.OptionsGroupBox.Controls.Add(this.CreateShaftRadioButton);
            this.OptionsGroupBox.Controls.Add(this.CreateDisplayRadioButton);
            this.OptionsGroupBox.Location = new System.Drawing.Point(12, 12);
            this.OptionsGroupBox.Name = "OptionsGroupBox";
            this.OptionsGroupBox.Size = new System.Drawing.Size(285, 146);
            this.OptionsGroupBox.TabIndex = 5;
            this.OptionsGroupBox.TabStop = false;
            this.OptionsGroupBox.Text = "Options :";
            // 
            // CreateModelLineOptionsForm
            // 
            this.AcceptButton = this.CreateButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(309, 197);
            this.Controls.Add(this.OptionsGroupBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.CreateButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CreateModelLineOptionsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Create X Model Line Options";
            this.OptionsGroupBox.ResumeLayout(false);
            this.OptionsGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton CreateAllRadioButton;
        private System.Windows.Forms.RadioButton CreateShaftRadioButton;
        private System.Windows.Forms.RadioButton CreateDisplayRadioButton;
        private System.Windows.Forms.Button CreateButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.GroupBox OptionsGroupBox;
    }
}