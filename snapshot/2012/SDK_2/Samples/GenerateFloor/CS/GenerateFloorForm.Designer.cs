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

namespace Revit.SDK.Samples.GenerateFloor.CS
{
    partial class GenerateFloorForm
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
            this.previewGroupBox = new System.Windows.Forms.GroupBox();
            this.previewPictureBox = new System.Windows.Forms.PictureBox();
            this.floorTypesComboBox = new System.Windows.Forms.ComboBox();
            this.structuralCheckBox = new System.Windows.Forms.CheckBox();
            this.OK = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.floorTypeLabel = new System.Windows.Forms.Label();
            this.previewGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.previewPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // previewGroupBox
            // 
            this.previewGroupBox.Controls.Add(this.previewPictureBox);
            this.previewGroupBox.Location = new System.Drawing.Point(12, 12);
            this.previewGroupBox.Name = "previewGroupBox";
            this.previewGroupBox.Size = new System.Drawing.Size(268, 242);
            this.previewGroupBox.TabIndex = 0;
            this.previewGroupBox.TabStop = false;
            this.previewGroupBox.Text = "Preview";
            // 
            // previewPictureBox
            // 
            this.previewPictureBox.BackColor = System.Drawing.SystemColors.ControlText;
            this.previewPictureBox.Location = new System.Drawing.Point(6, 19);
            this.previewPictureBox.Name = "previewPictureBox";
            this.previewPictureBox.Size = new System.Drawing.Size(256, 208);
            this.previewPictureBox.TabIndex = 0;
            this.previewPictureBox.TabStop = false;
            this.previewPictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.previewPictureBox_Paint);
            // 
            // floorTypesComboBox
            // 
            this.floorTypesComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.floorTypesComboBox.FormattingEnabled = true;
            this.floorTypesComboBox.Location = new System.Drawing.Point(295, 52);
            this.floorTypesComboBox.Name = "floorTypesComboBox";
            this.floorTypesComboBox.Size = new System.Drawing.Size(237, 21);
            this.floorTypesComboBox.TabIndex = 1;
            this.floorTypesComboBox.SelectionChangeCommitted += new System.EventHandler(this.floorTypesComboBox_SelectionChangeCommitted);
            // 
            // structuralCheckBox
            // 
            this.structuralCheckBox.AutoSize = true;
            this.structuralCheckBox.Checked = true;
            this.structuralCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.structuralCheckBox.Location = new System.Drawing.Point(295, 92);
            this.structuralCheckBox.Name = "structuralCheckBox";
            this.structuralCheckBox.Size = new System.Drawing.Size(71, 17);
            this.structuralCheckBox.TabIndex = 2;
            this.structuralCheckBox.Text = "Structural";
            this.structuralCheckBox.UseVisualStyleBackColor = true;
            this.structuralCheckBox.CheckedChanged += new System.EventHandler(this.structralCheckBox_CheckedChanged);
            // 
            // OK
            // 
            this.OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OK.Location = new System.Drawing.Point(376, 244);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(75, 23);
            this.OK.TabIndex = 3;
            this.OK.Text = "&OK";
            this.OK.UseVisualStyleBackColor = true;
            // 
            // Cancel
            // 
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(457, 244);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 3;
            this.Cancel.Text = "&Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            // 
            // floorTypeLabel
            // 
            this.floorTypeLabel.AutoSize = true;
            this.floorTypeLabel.Location = new System.Drawing.Point(292, 22);
            this.floorTypeLabel.Name = "floorTypeLabel";
            this.floorTypeLabel.Size = new System.Drawing.Size(62, 13);
            this.floorTypeLabel.TabIndex = 4;
            this.floorTypeLabel.Text = "Floor Types";
            // 
            // GenerateFloorForm
            // 
            this.AcceptButton = this.OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancel;
            this.ClientSize = new System.Drawing.Size(544, 279);
            this.Controls.Add(this.floorTypeLabel);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.structuralCheckBox);
            this.Controls.Add(this.floorTypesComboBox);
            this.Controls.Add(this.previewGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GenerateFloorForm";
            this.ShowInTaskbar = false;
            this.Text = "Generate Floor";
            this.Load += new System.EventHandler(this.GenerateFloorForm_Load);
            this.previewGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.previewPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox previewGroupBox;
        private System.Windows.Forms.PictureBox previewPictureBox;
        private System.Windows.Forms.ComboBox floorTypesComboBox;
        private System.Windows.Forms.CheckBox structuralCheckBox;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Label floorTypeLabel;
    }
}