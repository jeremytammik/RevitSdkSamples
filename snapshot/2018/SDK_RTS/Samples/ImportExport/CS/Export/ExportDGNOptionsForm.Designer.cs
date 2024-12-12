//
// (C) Copyright 2003-2017 by Autodesk, Inc.
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

namespace Revit.SDK.Samples.ImportExport.CS
{
    partial class ExportDGNOptionsForm
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
            this.labelLayerMapping = new System.Windows.Forms.Label();
            this.comboBoxLayerSettings = new System.Windows.Forms.ComboBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.comboBoxExportFileFormat = new System.Windows.Forms.ComboBox();
            this.labelExportFormat = new System.Windows.Forms.Label();
            this.checkBoxHideScopeBoxes = new System.Windows.Forms.CheckBox();
            this.checkBoxHideReferencePlanes = new System.Windows.Forms.CheckBox();
            this.checkBoxHideUnrefereceViewTag = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox2DSeedFile = new System.Windows.Forms.TextBox();
            this.button2DSeedFile = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox3DSeedFile = new System.Windows.Forms.TextBox();
            this.button3DSeedFile = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelLayerMapping
            // 
            this.labelLayerMapping.AutoSize = true;
            this.labelLayerMapping.Location = new System.Drawing.Point(11, 15);
            this.labelLayerMapping.Name = "labelLayerMapping";
            this.labelLayerMapping.Size = new System.Drawing.Size(77, 13);
            this.labelLayerMapping.TabIndex = 14;
            this.labelLayerMapping.Text = "Layer Settings:";
            // 
            // comboBoxLayerSettings
            // 
            this.comboBoxLayerSettings.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLayerSettings.FormattingEnabled = true;
            this.comboBoxLayerSettings.Location = new System.Drawing.Point(14, 31);
            this.comboBoxLayerSettings.Name = "comboBoxLayerSettings";
            this.comboBoxLayerSettings.Size = new System.Drawing.Size(419, 21);
            this.comboBoxLayerSettings.TabIndex = 15;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(358, 332);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 17;
            this.buttonCancel.Text = "&Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(275, 332);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 16;
            this.buttonOK.Text = "&OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // comboBoxExportFileFormat
            // 
            this.comboBoxExportFileFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxExportFileFormat.FormattingEnabled = true;
            this.comboBoxExportFileFormat.Location = new System.Drawing.Point(17, 82);
            this.comboBoxExportFileFormat.Name = "comboBoxExportFileFormat";
            this.comboBoxExportFileFormat.Size = new System.Drawing.Size(416, 21);
            this.comboBoxExportFileFormat.TabIndex = 18;
            // 
            // labelExportFormat
            // 
            this.labelExportFormat.AutoSize = true;
            this.labelExportFormat.Location = new System.Drawing.Point(14, 64);
            this.labelExportFormat.Name = "labelExportFormat";
            this.labelExportFormat.Size = new System.Drawing.Size(100, 13);
            this.labelExportFormat.TabIndex = 19;
            this.labelExportFormat.Text = "Export to file format:";
            // 
            // checkBoxHideScopeBoxes
            // 
            this.checkBoxHideScopeBoxes.AutoSize = true;
            this.checkBoxHideScopeBoxes.Location = new System.Drawing.Point(17, 244);
            this.checkBoxHideScopeBoxes.Name = "checkBoxHideScopeBoxes";
            this.checkBoxHideScopeBoxes.Size = new System.Drawing.Size(111, 17);
            this.checkBoxHideScopeBoxes.TabIndex = 20;
            this.checkBoxHideScopeBoxes.Text = "Hide scope boxes";
            this.checkBoxHideScopeBoxes.UseVisualStyleBackColor = true;
            // 
            // checkBoxHideReferencePlanes
            // 
            this.checkBoxHideReferencePlanes.AutoSize = true;
            this.checkBoxHideReferencePlanes.Location = new System.Drawing.Point(17, 267);
            this.checkBoxHideReferencePlanes.Name = "checkBoxHideReferencePlanes";
            this.checkBoxHideReferencePlanes.Size = new System.Drawing.Size(130, 17);
            this.checkBoxHideReferencePlanes.TabIndex = 20;
            this.checkBoxHideReferencePlanes.Text = "Hide reference planes";
            this.checkBoxHideReferencePlanes.UseVisualStyleBackColor = true;
            // 
            // checkBoxHideUnrefereceViewTag
            // 
            this.checkBoxHideUnrefereceViewTag.AutoSize = true;
            this.checkBoxHideUnrefereceViewTag.Location = new System.Drawing.Point(17, 290);
            this.checkBoxHideUnrefereceViewTag.Name = "checkBoxHideUnrefereceViewTag";
            this.checkBoxHideUnrefereceViewTag.Size = new System.Drawing.Size(162, 17);
            this.checkBoxHideUnrefereceViewTag.TabIndex = 20;
            this.checkBoxHideUnrefereceViewTag.Text = "Hide unreferenced view tags";
            this.checkBoxHideUnrefereceViewTag.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button3DSeedFile);
            this.groupBox1.Controls.Add(this.button2DSeedFile);
            this.groupBox1.Controls.Add(this.textBox3DSeedFile);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBox2DSeedFile);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(14, 119);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(419, 116);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Specify seed file for";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "2D views:";
            // 
            // textBox2DSeedFile
            // 
            this.textBox2DSeedFile.Location = new System.Drawing.Point(10, 37);
            this.textBox2DSeedFile.Name = "textBox2DSeedFile";
            this.textBox2DSeedFile.Size = new System.Drawing.Size(371, 20);
            this.textBox2DSeedFile.TabIndex = 1;
            // 
            // button2DSeedFile
            // 
            this.button2DSeedFile.Location = new System.Drawing.Point(387, 36);
            this.button2DSeedFile.Name = "button2DSeedFile";
            this.button2DSeedFile.Size = new System.Drawing.Size(26, 20);
            this.button2DSeedFile.TabIndex = 2;
            this.button2DSeedFile.Text = "...";
            this.button2DSeedFile.UseVisualStyleBackColor = true;
            this.button2DSeedFile.Click += new System.EventHandler(this.button2DSeedFile_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "3D views:";
            // 
            // textBox3DSeedFile
            // 
            this.textBox3DSeedFile.Location = new System.Drawing.Point(10, 84);
            this.textBox3DSeedFile.Name = "textBox3DSeedFile";
            this.textBox3DSeedFile.Size = new System.Drawing.Size(371, 20);
            this.textBox3DSeedFile.TabIndex = 1;
            // 
            // button3DSeedFile
            // 
            this.button3DSeedFile.Location = new System.Drawing.Point(387, 83);
            this.button3DSeedFile.Name = "button3DSeedFile";
            this.button3DSeedFile.Size = new System.Drawing.Size(26, 20);
            this.button3DSeedFile.TabIndex = 2;
            this.button3DSeedFile.Text = "...";
            this.button3DSeedFile.UseVisualStyleBackColor = true;
            this.button3DSeedFile.Click += new System.EventHandler(this.button3DSeedFile_Click);
            // 
            // ExportDGNOptionsForm
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(448, 372);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.checkBoxHideUnrefereceViewTag);
            this.Controls.Add(this.checkBoxHideReferencePlanes);
            this.Controls.Add(this.checkBoxHideScopeBoxes);
            this.Controls.Add(this.labelExportFormat);
            this.Controls.Add(this.comboBoxExportFileFormat);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.comboBoxLayerSettings);
            this.Controls.Add(this.labelLayerMapping);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExportDGNOptionsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Export DGN Options";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelLayerMapping;
        private System.Windows.Forms.ComboBox comboBoxLayerSettings;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.ComboBox comboBoxExportFileFormat;
        private System.Windows.Forms.Label labelExportFormat;
        private System.Windows.Forms.CheckBox checkBoxHideScopeBoxes;
        private System.Windows.Forms.CheckBox checkBoxHideReferencePlanes;
        private System.Windows.Forms.CheckBox checkBoxHideUnrefereceViewTag;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button3DSeedFile;
        private System.Windows.Forms.Button button2DSeedFile;
        private System.Windows.Forms.TextBox textBox3DSeedFile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox2DSeedFile;
        private System.Windows.Forms.Label label1;
    }
}
