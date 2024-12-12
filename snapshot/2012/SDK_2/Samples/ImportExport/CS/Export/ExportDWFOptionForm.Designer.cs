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

namespace Revit.SDK.Samples.ImportExport.CS
{
    /// <summary>
    /// Provide a dialog which provides the options of lower priority information 
    /// for export DWF format
    /// </summary>
    partial class ExportDWFOptionForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBoxRoomsAndAreas = new System.Windows.Forms.CheckBox();
            this.checkBoxModelElements = new System.Windows.Forms.CheckBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.checkBoxMergeViews = new System.Windows.Forms.CheckBox();
            this.groupBoxGraphicsSetting = new System.Windows.Forms.GroupBox();
            this.comboBoxImageQuality = new System.Windows.Forms.ComboBox();
            this.labelImageQuality = new System.Windows.Forms.Label();
            this.radioButtonCompressedFormat = new System.Windows.Forms.RadioButton();
            this.radioButtonStandardFormat = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.groupBoxGraphicsSetting.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBoxRoomsAndAreas);
            this.groupBox1.Controls.Add(this.checkBoxModelElements);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(308, 68);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Export Object Data";
            // 
            // checkBoxRoomsAndAreas
            // 
            this.checkBoxRoomsAndAreas.AutoSize = true;
            this.checkBoxRoomsAndAreas.Location = new System.Drawing.Point(10, 43);
            this.checkBoxRoomsAndAreas.Name = "checkBoxRoomsAndAreas";
            this.checkBoxRoomsAndAreas.Size = new System.Drawing.Size(110, 17);
            this.checkBoxRoomsAndAreas.TabIndex = 0;
            this.checkBoxRoomsAndAreas.Text = "Rooms and Areas";
            this.checkBoxRoomsAndAreas.UseVisualStyleBackColor = true;
            // 
            // checkBoxModelElements
            // 
            this.checkBoxModelElements.AutoSize = true;
            this.checkBoxModelElements.Location = new System.Drawing.Point(10, 20);
            this.checkBoxModelElements.Name = "checkBoxModelElements";
            this.checkBoxModelElements.Size = new System.Drawing.Size(100, 17);
            this.checkBoxModelElements.TabIndex = 0;
            this.checkBoxModelElements.Text = "Model Elements";
            this.checkBoxModelElements.UseVisualStyleBackColor = true;
            this.checkBoxModelElements.CheckedChanged += new System.EventHandler(this.checkBoxModelElements_CheckedChanged);
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(114, 230);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(91, 23);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "&OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(220, 230);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(101, 23);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "&Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // checkBoxMergeViews
            // 
            this.checkBoxMergeViews.AutoSize = true;
            this.checkBoxMergeViews.Location = new System.Drawing.Point(23, 195);
            this.checkBoxMergeViews.Name = "checkBoxMergeViews";
            this.checkBoxMergeViews.Size = new System.Drawing.Size(226, 17);
            this.checkBoxMergeViews.TabIndex = 4;
            this.checkBoxMergeViews.Text = "Create separate files for each view/sheet";
            this.checkBoxMergeViews.UseVisualStyleBackColor = true;
            // 
            // groupBoxGraphicsSetting
            // 
            this.groupBoxGraphicsSetting.Controls.Add(this.comboBoxImageQuality);
            this.groupBoxGraphicsSetting.Controls.Add(this.labelImageQuality);
            this.groupBoxGraphicsSetting.Controls.Add(this.radioButtonCompressedFormat);
            this.groupBoxGraphicsSetting.Controls.Add(this.radioButtonStandardFormat);
            this.groupBoxGraphicsSetting.Location = new System.Drawing.Point(13, 88);
            this.groupBoxGraphicsSetting.Name = "groupBoxGraphicsSetting";
            this.groupBoxGraphicsSetting.Size = new System.Drawing.Size(308, 100);
            this.groupBoxGraphicsSetting.TabIndex = 5;
            this.groupBoxGraphicsSetting.TabStop = false;
            this.groupBoxGraphicsSetting.Text = "Graphics Settings";
            // 
            // comboBoxImageQuality
            // 
            this.comboBoxImageQuality.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxImageQuality.FormattingEnabled = true;
            this.comboBoxImageQuality.Location = new System.Drawing.Point(129, 66);
            this.comboBoxImageQuality.Name = "comboBoxImageQuality";
            this.comboBoxImageQuality.Size = new System.Drawing.Size(163, 21);
            this.comboBoxImageQuality.TabIndex = 3;
            // 
            // labelImageQuality
            // 
            this.labelImageQuality.AutoSize = true;
            this.labelImageQuality.Location = new System.Drawing.Point(54, 69);
            this.labelImageQuality.Name = "labelImageQuality";
            this.labelImageQuality.Size = new System.Drawing.Size(78, 13);
            this.labelImageQuality.TabIndex = 2;
            this.labelImageQuality.Text = "Image Quality:";
            // 
            // radioButtonCompressedFormat
            // 
            this.radioButtonCompressedFormat.AutoSize = true;
            this.radioButtonCompressedFormat.Location = new System.Drawing.Point(10, 41);
            this.radioButtonCompressedFormat.Name = "radioButtonCompressedFormat";
            this.radioButtonCompressedFormat.Size = new System.Drawing.Size(170, 17);
            this.radioButtonCompressedFormat.TabIndex = 0;
            this.radioButtonCompressedFormat.TabStop = true;
            this.radioButtonCompressedFormat.Text = "Use compressed raster format";
            this.radioButtonCompressedFormat.UseVisualStyleBackColor = true;
            this.radioButtonCompressedFormat.CheckedChanged += new System.EventHandler(this.radioButtonCompressedFormat_CheckedChanged);
            // 
            // radioButtonStandardFormat
            // 
            this.radioButtonStandardFormat.AutoSize = true;
            this.radioButtonStandardFormat.Location = new System.Drawing.Point(10, 20);
            this.radioButtonStandardFormat.Name = "radioButtonStandardFormat";
            this.radioButtonStandardFormat.Size = new System.Drawing.Size(124, 17);
            this.radioButtonStandardFormat.TabIndex = 0;
            this.radioButtonStandardFormat.TabStop = true;
            this.radioButtonStandardFormat.Text = "Use standard format";
            this.radioButtonStandardFormat.UseVisualStyleBackColor = true;
            // 
            // ExportDWFOptionForm
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(335, 262);
            this.Controls.Add(this.groupBoxGraphicsSetting);
            this.Controls.Add(this.checkBoxMergeViews);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExportDWFOptionForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Export DWF Options";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBoxGraphicsSetting.ResumeLayout(false);
            this.groupBoxGraphicsSetting.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBoxRoomsAndAreas;
        private System.Windows.Forms.CheckBox checkBoxModelElements;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.CheckBox checkBoxMergeViews;
        private System.Windows.Forms.GroupBox groupBoxGraphicsSetting;
        private System.Windows.Forms.ComboBox comboBoxImageQuality;
        private System.Windows.Forms.Label labelImageQuality;
        private System.Windows.Forms.RadioButton radioButtonCompressedFormat;
        private System.Windows.Forms.RadioButton radioButtonStandardFormat;
    }
}
