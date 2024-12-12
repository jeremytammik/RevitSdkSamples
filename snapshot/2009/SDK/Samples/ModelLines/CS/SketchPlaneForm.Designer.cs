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


namespace Revit.SDK.Samples.ModelLines.CS
{
    /// <summary>
    /// This UserControl is used to collect the information for sketch plane creation
    /// </summary>
    partial class SketchPlaneForm
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
            this.normalUserControl = new Revit.SDK.Samples.ModelLines.CS.PointUserControl();
            this.normalLabel = new System.Windows.Forms.Label();
            this.originLabel = new System.Windows.Forms.Label();
            this.originUserControl = new Revit.SDK.Samples.ModelLines.CS.PointUserControl();
            this.creationGroupBox = new System.Windows.Forms.GroupBox();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.creationGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // normalUserControl
            // 
            this.normalUserControl.Location = new System.Drawing.Point(86, 19);
            this.normalUserControl.Name = "normalUserControl";
            this.normalUserControl.Size = new System.Drawing.Size(213, 25);
            this.normalUserControl.TabIndex = 1;
            // 
            // normalLabel
            // 
            this.normalLabel.AutoSize = true;
            this.normalLabel.Location = new System.Drawing.Point(10, 23);
            this.normalLabel.Name = "normalLabel";
            this.normalLabel.Size = new System.Drawing.Size(73, 13);
            this.normalLabel.TabIndex = 5;
            this.normalLabel.Text = "Plane Normal:";
            // 
            // originLabel
            // 
            this.originLabel.AutoSize = true;
            this.originLabel.Location = new System.Drawing.Point(11, 55);
            this.originLabel.Name = "originLabel";
            this.originLabel.Size = new System.Drawing.Size(67, 13);
            this.originLabel.TabIndex = 6;
            this.originLabel.Text = "Plane Origin:";
            // 
            // originUserControl
            // 
            this.originUserControl.Location = new System.Drawing.Point(86, 53);
            this.originUserControl.Name = "originUserControl";
            this.originUserControl.Size = new System.Drawing.Size(213, 25);
            this.originUserControl.TabIndex = 2;
            // 
            // creationGroupBox
            // 
            this.creationGroupBox.Controls.Add(this.normalUserControl);
            this.creationGroupBox.Controls.Add(this.originUserControl);
            this.creationGroupBox.Controls.Add(this.normalLabel);
            this.creationGroupBox.Controls.Add(this.originLabel);
            this.creationGroupBox.Location = new System.Drawing.Point(12, 12);
            this.creationGroupBox.Name = "creationGroupBox";
            this.creationGroupBox.Size = new System.Drawing.Size(305, 84);
            this.creationGroupBox.TabIndex = 8;
            this.creationGroupBox.TabStop = false;
            this.creationGroupBox.Text = "Sketch Plane Creation";
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(161, 102);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 3;
            this.okButton.Text = "&Ok";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(242, 102);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // SketchPlaneForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(329, 131);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.creationGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SketchPlaneForm";
            this.ShowInTaskbar = false;
            this.Text = "Sketch Plane";
            this.creationGroupBox.ResumeLayout(false);
            this.creationGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Revit.SDK.Samples.ModelLines.CS.PointUserControl normalUserControl;
        private System.Windows.Forms.Label normalLabel;
        private System.Windows.Forms.Label originLabel;
        private Revit.SDK.Samples.ModelLines.CS.PointUserControl originUserControl;
        private System.Windows.Forms.GroupBox creationGroupBox;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
    }
}