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
            this.parametersPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.nameLabel = new System.Windows.Forms.Label();
            this.duplicateButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.typeLabel = new System.Windows.Forms.Label();
            this.materialInfoTabControl = new System.Windows.Forms.TabControl();
            this.parametersTabPage = new System.Windows.Forms.TabPage();
            this.renderAppearanceTabPage = new System.Windows.Forms.TabPage();
            this.renderAppearancePropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.renderAppearanceNameTextBox = new System.Windows.Forms.TextBox();
            this.replaceButton = new System.Windows.Forms.Button();
            this.materialInfoTabControl.SuspendLayout();
            this.parametersTabPage.SuspendLayout();
            this.renderAppearanceTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // materialsListBox
            // 
            this.materialsListBox.FormattingEnabled = true;
            this.materialsListBox.HorizontalScrollbar = true;
            this.materialsListBox.Location = new System.Drawing.Point(12, 63);
            this.materialsListBox.Name = "materialsListBox";
            this.materialsListBox.Size = new System.Drawing.Size(200, 264);
            this.materialsListBox.Sorted = true;
            this.materialsListBox.TabIndex = 2;
            this.materialsListBox.SelectedIndexChanged += new System.EventHandler(this.materialsListBox_SelectedIndexChanged);
            // 
            // categoryComboBox
            // 
            this.categoryComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
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
            this.categoryComboBox.SelectionChangeCommitted += new System.EventHandler(this.categoryComboBox_SelectionChangeCommitted);
            // 
            // parametersPropertyGrid
            // 
            this.parametersPropertyGrid.Location = new System.Drawing.Point(0, 0);
            this.parametersPropertyGrid.Name = "parametersPropertyGrid";
            this.parametersPropertyGrid.Size = new System.Drawing.Size(307, 289);
            this.parametersPropertyGrid.TabIndex = 4;
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
            // materialInfoTabControl
            // 
            this.materialInfoTabControl.Controls.Add(this.parametersTabPage);
            this.materialInfoTabControl.Controls.Add(this.renderAppearanceTabPage);
            this.materialInfoTabControl.Location = new System.Drawing.Point(240, 12);
            this.materialInfoTabControl.Name = "materialInfoTabControl";
            this.materialInfoTabControl.SelectedIndex = 0;
            this.materialInfoTabControl.Size = new System.Drawing.Size(315, 315);
            this.materialInfoTabControl.TabIndex = 7;
            // 
            // parametersTabPage
            // 
            this.parametersTabPage.BackColor = System.Drawing.Color.Transparent;
            this.parametersTabPage.Controls.Add(this.parametersPropertyGrid);
            this.parametersTabPage.Location = new System.Drawing.Point(4, 22);
            this.parametersTabPage.Name = "parametersTabPage";
            this.parametersTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.parametersTabPage.Size = new System.Drawing.Size(307, 289);
            this.parametersTabPage.TabIndex = 0;
            this.parametersTabPage.Text = "Physical";
            // 
            // renderAppearanceTabPage
            // 
            this.renderAppearanceTabPage.BackColor = System.Drawing.SystemColors.Control;
            this.renderAppearanceTabPage.Controls.Add(this.replaceButton);
            this.renderAppearanceTabPage.Controls.Add(this.renderAppearancePropertyGrid);
            this.renderAppearanceTabPage.Controls.Add(this.renderAppearanceNameTextBox);
            this.renderAppearanceTabPage.Location = new System.Drawing.Point(4, 22);
            this.renderAppearanceTabPage.Name = "renderAppearanceTabPage";
            this.renderAppearanceTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.renderAppearanceTabPage.Size = new System.Drawing.Size(307, 289);
            this.renderAppearanceTabPage.TabIndex = 1;
            this.renderAppearanceTabPage.Text = "Render Appearance";
            // 
            // renderAppearancePropertyGrid
            // 
            this.renderAppearancePropertyGrid.Location = new System.Drawing.Point(0, 35);
            this.renderAppearancePropertyGrid.Name = "renderAppearancePropertyGrid";
            this.renderAppearancePropertyGrid.Size = new System.Drawing.Size(307, 254);
            this.renderAppearancePropertyGrid.TabIndex = 0;
            // 
            // renderAppearanceNameTextBox
            // 
            this.renderAppearanceNameTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.renderAppearanceNameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.renderAppearanceNameTextBox.Location = new System.Drawing.Point(6, 11);
            this.renderAppearanceNameTextBox.Name = "renderAppearanceNameTextBox";
            this.renderAppearanceNameTextBox.ReadOnly = true;
            this.renderAppearanceNameTextBox.Size = new System.Drawing.Size(214, 13);
            this.renderAppearanceNameTextBox.TabIndex = 2;
            // 
            // replaceButton
            // 
            this.replaceButton.Location = new System.Drawing.Point(229, 6);
            this.replaceButton.Name = "replaceButton";
            this.replaceButton.Size = new System.Drawing.Size(75, 23);
            this.replaceButton.TabIndex = 1;
            this.replaceButton.Text = "Replace...";
            this.replaceButton.UseVisualStyleBackColor = true;
            this.replaceButton.Click += new System.EventHandler(this.replaceButton_Click);
            // 
            // MaterialsForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(567, 388);
            this.Controls.Add(this.materialInfoTabControl);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.duplicateButton);
            this.Controls.Add(this.typeLabel);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.categoryComboBox);
            this.Controls.Add(this.materialsListBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MaterialsForm";
            this.ShowInTaskbar = false;
            this.Text = "Materials";
            this.Load += new System.EventHandler(this.MaterialsForm_Load);
            this.materialInfoTabControl.ResumeLayout(false);
            this.parametersTabPage.ResumeLayout(false);
            this.renderAppearanceTabPage.ResumeLayout(false);
            this.renderAppearanceTabPage.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox materialsListBox;
        private System.Windows.Forms.ComboBox categoryComboBox;
        private System.Windows.Forms.PropertyGrid parametersPropertyGrid;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.Button duplicateButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label typeLabel;
        private System.Windows.Forms.TabControl materialInfoTabControl;
        private System.Windows.Forms.TabPage renderAppearanceTabPage;
        private System.Windows.Forms.TabPage parametersTabPage;
        private System.Windows.Forms.Button replaceButton;
        private System.Windows.Forms.PropertyGrid renderAppearancePropertyGrid;
        private System.Windows.Forms.TextBox renderAppearanceNameTextBox;
    }
}