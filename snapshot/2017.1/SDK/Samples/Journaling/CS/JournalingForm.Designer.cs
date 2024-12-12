//
// (C) Copyright 2003-2016 by Autodesk, Inc.
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


namespace Revit.SDK.Samples.Journaling.CS
{
    partial class JournalingForm
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
            this.typeLabel = new System.Windows.Forms.Label();
            this.typeComboBox = new System.Windows.Forms.ComboBox();
            this.levelLabel = new System.Windows.Forms.Label();
            this.levelComboBox = new System.Windows.Forms.ComboBox();
            this.locationGroupBox = new System.Windows.Forms.GroupBox();
            this.endPointUserControl = new Revit.SDK.Samples.ModelLines.CS.PointUserControl();
            this.startPointUserControl = new Revit.SDK.Samples.ModelLines.CS.PointUserControl();
            this.endPointLabel = new System.Windows.Forms.Label();
            this.startPointLabel = new System.Windows.Forms.Label();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.locationGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // typeLabel
            // 
            this.typeLabel.AutoSize = true;
            this.typeLabel.Location = new System.Drawing.Point(34, 19);
            this.typeLabel.Name = "typeLabel";
            this.typeLabel.Size = new System.Drawing.Size(58, 13);
            this.typeLabel.TabIndex = 0;
            this.typeLabel.Text = "Wall Type:";
            // 
            // typeComboBox
            // 
            this.typeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.typeComboBox.FormattingEnabled = true;
            this.typeComboBox.Location = new System.Drawing.Point(99, 16);
            this.typeComboBox.Name = "typeComboBox";
            this.typeComboBox.Size = new System.Drawing.Size(205, 21);
            this.typeComboBox.TabIndex = 0;
            // 
            // levelLabel
            // 
            this.levelLabel.AutoSize = true;
            this.levelLabel.Location = new System.Drawing.Point(56, 59);
            this.levelLabel.Name = "levelLabel";
            this.levelLabel.Size = new System.Drawing.Size(36, 13);
            this.levelLabel.TabIndex = 2;
            this.levelLabel.Text = "Level:";
            // 
            // levelComboBox
            // 
            this.levelComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.levelComboBox.FormattingEnabled = true;
            this.levelComboBox.Location = new System.Drawing.Point(99, 56);
            this.levelComboBox.Name = "levelComboBox";
            this.levelComboBox.Size = new System.Drawing.Size(205, 21);
            this.levelComboBox.TabIndex = 1;
            // 
            // locationGroupBox
            // 
            this.locationGroupBox.Controls.Add(this.endPointUserControl);
            this.locationGroupBox.Controls.Add(this.startPointUserControl);
            this.locationGroupBox.Controls.Add(this.endPointLabel);
            this.locationGroupBox.Controls.Add(this.startPointLabel);
            this.locationGroupBox.Location = new System.Drawing.Point(15, 83);
            this.locationGroupBox.Name = "locationGroupBox";
            this.locationGroupBox.Size = new System.Drawing.Size(289, 86);
            this.locationGroupBox.TabIndex = 4;
            this.locationGroupBox.TabStop = false;
            this.locationGroupBox.Text = "Wall Location Line";
            // 
            // endPointUserControl
            // 
            this.endPointUserControl.Location = new System.Drawing.Point(70, 51);
            this.endPointUserControl.Name = "endPointUserControl";
            this.endPointUserControl.Size = new System.Drawing.Size(213, 28);
            this.endPointUserControl.TabIndex = 1;
            // 
            // startPointUserControl
            // 
            this.startPointUserControl.Location = new System.Drawing.Point(70, 19);
            this.startPointUserControl.Name = "startPointUserControl";
            this.startPointUserControl.Size = new System.Drawing.Size(213, 28);
            this.startPointUserControl.TabIndex = 0;
            // 
            // endPointLabel
            // 
            this.endPointLabel.AutoSize = true;
            this.endPointLabel.Location = new System.Drawing.Point(19, 57);
            this.endPointLabel.Name = "endPointLabel";
            this.endPointLabel.Size = new System.Drawing.Size(29, 13);
            this.endPointLabel.TabIndex = 1;
            this.endPointLabel.Text = "End:";
            // 
            // startPointLabel
            // 
            this.startPointLabel.AutoSize = true;
            this.startPointLabel.Location = new System.Drawing.Point(19, 29);
            this.startPointLabel.Name = "startPointLabel";
            this.startPointLabel.Size = new System.Drawing.Size(32, 13);
            this.startPointLabel.TabIndex = 0;
            this.startPointLabel.Text = "Start:";
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(142, 175);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(79, 23);
            this.okButton.TabIndex = 2;
            this.okButton.Text = "&OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(227, 175);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(77, 23);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // JournalingForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(319, 204);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.locationGroupBox);
            this.Controls.Add(this.levelComboBox);
            this.Controls.Add(this.levelLabel);
            this.Controls.Add(this.typeComboBox);
            this.Controls.Add(this.typeLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "JournalingForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Journaling";
            this.locationGroupBox.ResumeLayout(false);
            this.locationGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label typeLabel;
        private System.Windows.Forms.ComboBox typeComboBox;
        private System.Windows.Forms.Label levelLabel;
        private System.Windows.Forms.ComboBox levelComboBox;
        private System.Windows.Forms.GroupBox locationGroupBox;
        private System.Windows.Forms.Label endPointLabel;
        private System.Windows.Forms.Label startPointLabel;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private Revit.SDK.Samples.ModelLines.CS.PointUserControl endPointUserControl;
        private Revit.SDK.Samples.ModelLines.CS.PointUserControl startPointUserControl;
    }
}