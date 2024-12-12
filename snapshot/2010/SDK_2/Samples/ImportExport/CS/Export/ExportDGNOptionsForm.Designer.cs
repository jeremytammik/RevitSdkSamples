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
            this.checkBoxEnableTemplateFile = new System.Windows.Forms.CheckBox();
            this.buttonBrowserTemplate = new System.Windows.Forms.Button();
            this.textBoxTemplateFile = new System.Windows.Forms.TextBox();
            this.labelLayerMapping = new System.Windows.Forms.Label();
            this.comboBoxLayerSettings = new System.Windows.Forms.ComboBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // checkBoxEnableTemplateFile
            // 
            this.checkBoxEnableTemplateFile.AutoSize = true;
            this.checkBoxEnableTemplateFile.Location = new System.Drawing.Point(14, 59);
            this.checkBoxEnableTemplateFile.Name = "checkBoxEnableTemplateFile";
            this.checkBoxEnableTemplateFile.Size = new System.Drawing.Size(145, 17);
            this.checkBoxEnableTemplateFile.TabIndex = 8;
            this.checkBoxEnableTemplateFile.Text = "Enable DGN template file";
            this.checkBoxEnableTemplateFile.UseVisualStyleBackColor = true;
            this.checkBoxEnableTemplateFile.CheckedChanged += new System.EventHandler(this.checkBoxEnableTemplateFile_CheckedChanged);
            // 
            // buttonBrowserTemplate
            // 
            this.buttonBrowserTemplate.Location = new System.Drawing.Point(319, 81);
            this.buttonBrowserTemplate.Name = "buttonBrowserTemplate";
            this.buttonBrowserTemplate.Size = new System.Drawing.Size(24, 20);
            this.buttonBrowserTemplate.TabIndex = 10;
            this.buttonBrowserTemplate.Text = "...";
            this.buttonBrowserTemplate.UseVisualStyleBackColor = true;
            this.buttonBrowserTemplate.Click += new System.EventHandler(this.buttonBrowserTemplate_Click);
            // 
            // textBoxTemplateFile
            // 
            this.textBoxTemplateFile.Location = new System.Drawing.Point(14, 82);
            this.textBoxTemplateFile.Name = "textBoxTemplateFile";
            this.textBoxTemplateFile.Size = new System.Drawing.Size(308, 20);
            this.textBoxTemplateFile.TabIndex = 9;
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
            this.comboBoxLayerSettings.Size = new System.Drawing.Size(329, 21);
            this.comboBoxLayerSettings.TabIndex = 15;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(268, 123);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 17;
            this.buttonCancel.Text = "&Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(186, 123);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 16;
            this.buttonOK.Text = "&OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // ExportDGNOptionsForm
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(355, 154);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.comboBoxLayerSettings);
            this.Controls.Add(this.labelLayerMapping);
            this.Controls.Add(this.buttonBrowserTemplate);
            this.Controls.Add(this.textBoxTemplateFile);
            this.Controls.Add(this.checkBoxEnableTemplateFile);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExportDGNOptionsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Export DGN Options";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBoxEnableTemplateFile;
        private System.Windows.Forms.Button buttonBrowserTemplate;
        private System.Windows.Forms.TextBox textBoxTemplateFile;
        private System.Windows.Forms.Label labelLayerMapping;
        private System.Windows.Forms.ComboBox comboBoxLayerSettings;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
    }
}