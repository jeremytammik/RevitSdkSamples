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

namespace Revit.SDK.Samples.Materials.CS
{
    partial class MaterialsForm
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
            this.materialsListBox = new System.Windows.Forms.ListBox();
            this.categoryComboBox = new System.Windows.Forms.ComboBox();
            this.materialPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.nameLabel = new System.Windows.Forms.Label();
            this.duplicateButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.typeLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // materialsListBox
            // 
            this.materialsListBox.FormattingEnabled = true;
            this.materialsListBox.Location = new System.Drawing.Point(12, 63);
            this.materialsListBox.Name = "materialsListBox";
            this.materialsListBox.Size = new System.Drawing.Size(200, 264);
            this.materialsListBox.TabIndex = 2;
            this.materialsListBox.SelectedIndexChanged += new System.EventHandler(this.materialsListBox_SelectedIndexChanged);
            // 
            // categoryComboBox
            // 
            this.categoryComboBox.FormattingEnabled = true;
            this.categoryComboBox.Items.AddRange(new object[] {
            "All",
            "MaterialConcrete",
            "MaterialGeneric",
            "MaterialOther",
            "MaterialSteel",
            "MaterialWood"});
            this.categoryComboBox.Location = new System.Drawing.Point(55, 12);
            this.categoryComboBox.Name = "categoryComboBox";
            this.categoryComboBox.Size = new System.Drawing.Size(157, 21);
            this.categoryComboBox.TabIndex = 1;
            this.categoryComboBox.Text = "All";
            this.categoryComboBox.SelectionChangeCommitted += new System.EventHandler(this.categoryComboBox_SelectionChangeCommitted);
            // 
            // materialPropertyGrid
            // 
            this.materialPropertyGrid.Location = new System.Drawing.Point(240, 12);
            this.materialPropertyGrid.Name = "materialPropertyGrid";
            this.materialPropertyGrid.Size = new System.Drawing.Size(315, 315);
            this.materialPropertyGrid.TabIndex = 4;
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(12, 46);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(35, 13);
            this.nameLabel.TabIndex = 4;
            this.nameLabel.Text = "Name";
            // 
            // duplicateButton
            // 
            this.duplicateButton.Enabled = false;
            this.duplicateButton.Location = new System.Drawing.Point(12, 345);
            this.duplicateButton.Name = "duplicateButton";
            this.duplicateButton.Size = new System.Drawing.Size(75, 23);
            this.duplicateButton.TabIndex = 3;
            this.duplicateButton.Text = "&Duplicate";
            this.duplicateButton.UseVisualStyleBackColor = true;
            this.duplicateButton.Click += new System.EventHandler(this.duplicateButton_Click);
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(399, 345);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 5;
            this.okButton.Text = "&OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(480, 345);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 6;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // typeLabel
            // 
            this.typeLabel.AutoSize = true;
            this.typeLabel.Location = new System.Drawing.Point(12, 15);
            this.typeLabel.Name = "typeLabel";
            this.typeLabel.Size = new System.Drawing.Size(31, 13);
            this.typeLabel.TabIndex = 4;
            this.typeLabel.Text = "Type";
            // 
            // MaterialsForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(567, 388);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.duplicateButton);
            this.Controls.Add(this.typeLabel);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.materialPropertyGrid);
            this.Controls.Add(this.categoryComboBox);
            this.Controls.Add(this.materialsListBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MaterialsForm";
            this.ShowInTaskbar = false;
            this.Text = "Materials Property";
            this.Load += new System.EventHandler(this.MaterialsForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox materialsListBox;
        private System.Windows.Forms.ComboBox categoryComboBox;
        private System.Windows.Forms.PropertyGrid materialPropertyGrid;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.Button duplicateButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label typeLabel;
    }
}