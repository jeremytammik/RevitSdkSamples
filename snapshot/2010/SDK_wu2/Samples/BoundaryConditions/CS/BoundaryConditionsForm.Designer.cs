//
// (C) Copyright 2003-2009 by Autodesk, Inc.
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
    partial class BoundaryConditionsForm
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
            this.bCLabel = new System.Windows.Forms.Label();
            this.hostLabel = new System.Windows.Forms.Label();
            this.bCPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.bCComboBox = new System.Windows.Forms.ComboBox();
            this.hostTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // bCLabel
            // 
            this.bCLabel.AutoSize = true;
            this.bCLabel.Location = new System.Drawing.Point(200, 20);
            this.bCLabel.Name = "bCLabel";
            this.bCLabel.Size = new System.Drawing.Size(139, 17);
            this.bCLabel.TabIndex = 0;
            this.bCLabel.Text = "Boundary Conditions";
            // 
            // hostLabel
            // 
            this.hostLabel.AutoSize = true;
            this.hostLabel.Location = new System.Drawing.Point(21, 20);
            this.hostLabel.Name = "hostLabel";
            this.hostLabel.Size = new System.Drawing.Size(37, 17);
            this.hostLabel.TabIndex = 1;
            this.hostLabel.Text = "Host";
            // 
            // bCPropertyGrid
            // 
            this.bCPropertyGrid.Location = new System.Drawing.Point(23, 60);
            this.bCPropertyGrid.Name = "bCPropertyGrid";
            this.bCPropertyGrid.Size = new System.Drawing.Size(434, 290);
            this.bCPropertyGrid.TabIndex = 2;
            this.bCPropertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.bCPropertyGrid_PropertyValueChanged);
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(247, 379);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(102, 30);
            this.okButton.TabIndex = 3;
            this.okButton.Text = "&OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(355, 379);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(102, 30);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // bCComboBox
            // 
            this.bCComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.bCComboBox.FormattingEnabled = true;
            this.bCComboBox.Location = new System.Drawing.Point(345, 13);
            this.bCComboBox.Name = "bCComboBox";
            this.bCComboBox.Size = new System.Drawing.Size(112, 24);
            this.bCComboBox.TabIndex = 1;
            this.bCComboBox.SelectedIndexChanged += new System.EventHandler(this.bCComboBox_SelectedIndexChanged);
            // 
            // hostTextBox
            // 
            this.hostTextBox.Location = new System.Drawing.Point(64, 15);
            this.hostTextBox.Name = "hostTextBox";
            this.hostTextBox.ReadOnly = true;
            this.hostTextBox.Size = new System.Drawing.Size(97, 22);
            this.hostTextBox.TabIndex = 6;
            this.hostTextBox.TabStop = false;
            // 
            // BoundaryConditionsForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(480, 438);
            this.Controls.Add(this.hostTextBox);
            this.Controls.Add(this.bCComboBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.bCPropertyGrid);
            this.Controls.Add(this.hostLabel);
            this.Controls.Add(this.bCLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BoundaryConditionsForm";
            this.ShowInTaskbar = false;
            this.Text = "Boundary Conditions";
            this.Load += new System.EventHandler(this.BoundaryConditionsForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label bCLabel;
        private System.Windows.Forms.Label hostLabel;
        private System.Windows.Forms.PropertyGrid bCPropertyGrid;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.ComboBox bCComboBox;
        private System.Windows.Forms.TextBox hostTextBox;
    }
}