//
// (C) Copyright 2003-2014 by Autodesk, Inc.
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
    partial class ExportIMGOptionsForm
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
            this.outputGroupBox = new System.Windows.Forms.GroupBox();
            this.saveAs = new System.Windows.Forms.TextBox();
            this.name = new System.Windows.Forms.Label();
            this.changePath = new System.Windows.Forms.Button();
            this.exportRangeGroupBox = new System.Windows.Forms.GroupBox();
            this.selectRange = new System.Windows.Forms.Button();
            this.viewsSheetsOption = new System.Windows.Forms.RadioButton();
            this.visiblePortionOption = new System.Windows.Forms.RadioButton();
            this.currentWindowOption = new System.Windows.Forms.RadioButton();
            this.imageSizeGroupBox = new System.Windows.Forms.GroupBox();
            this.pixels = new System.Windows.Forms.Label();
            this.pixelValue = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.verticalOption = new System.Windows.Forms.RadioButton();
            this.horizontalOption = new System.Windows.Forms.RadioButton();
            this.actual = new System.Windows.Forms.Label();
            this.zoomSize = new System.Windows.Forms.NumericUpDown();
            this.Direction = new System.Windows.Forms.Label();
            this.zoomOption = new System.Windows.Forms.RadioButton();
            this.fitToOption = new System.Windows.Forms.RadioButton();
            this.formatGroupBox = new System.Windows.Forms.GroupBox();
            this.RIQCom = new System.Windows.Forms.ComboBox();
            this.noShadedCom = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Non = new System.Windows.Forms.Label();
            this.shadedCom = new System.Windows.Forms.ComboBox();
            this.shaded = new System.Windows.Forms.Label();
            this.okButton = new System.Windows.Forms.Button();
            this.cannelButton = new System.Windows.Forms.Button();
            this.outputGroupBox.SuspendLayout();
            this.exportRangeGroupBox.SuspendLayout();
            this.imageSizeGroupBox.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.zoomSize)).BeginInit();
            this.formatGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // outputGroupBox
            // 
            this.outputGroupBox.Controls.Add(this.saveAs);
            this.outputGroupBox.Controls.Add(this.name);
            this.outputGroupBox.Controls.Add(this.changePath);
            this.outputGroupBox.Location = new System.Drawing.Point(12, 12);
            this.outputGroupBox.Name = "outputGroupBox";
            this.outputGroupBox.Size = new System.Drawing.Size(489, 58);
            this.outputGroupBox.TabIndex = 0;
            this.outputGroupBox.TabStop = false;
            this.outputGroupBox.Text = "Output";
            // 
            // saveAs
            // 
            this.saveAs.Location = new System.Drawing.Point(55, 20);
            this.saveAs.Name = "saveAs";
            this.saveAs.Size = new System.Drawing.Size(385, 21);
            this.saveAs.TabIndex = 1;
            // 
            // name
            // 
            this.name.AutoSize = true;
            this.name.Location = new System.Drawing.Point(13, 24);
            this.name.Name = "name";
            this.name.Size = new System.Drawing.Size(38, 13);
            this.name.TabIndex = 1;
            this.name.Text = "Name:";
            // 
            // changePath
            // 
            this.changePath.Location = new System.Drawing.Point(443, 19);
            this.changePath.Name = "changePath";
            this.changePath.Size = new System.Drawing.Size(27, 23);
            this.changePath.TabIndex = 1;
            this.changePath.Text = "...";
            this.changePath.UseVisualStyleBackColor = true;
            this.changePath.Click += new System.EventHandler(this.Change_Click);
            // 
            // exportRangeGroupBox
            // 
            this.exportRangeGroupBox.Controls.Add(this.selectRange);
            this.exportRangeGroupBox.Controls.Add(this.viewsSheetsOption);
            this.exportRangeGroupBox.Controls.Add(this.visiblePortionOption);
            this.exportRangeGroupBox.Controls.Add(this.currentWindowOption);
            this.exportRangeGroupBox.Location = new System.Drawing.Point(12, 76);
            this.exportRangeGroupBox.Name = "exportRangeGroupBox";
            this.exportRangeGroupBox.Size = new System.Drawing.Size(489, 46);
            this.exportRangeGroupBox.TabIndex = 1;
            this.exportRangeGroupBox.TabStop = false;
            this.exportRangeGroupBox.Text = "Export Range";
            // 
            // selectRange
            // 
            this.selectRange.Location = new System.Drawing.Point(443, 16);
            this.selectRange.Name = "selectRange";
            this.selectRange.Size = new System.Drawing.Size(27, 23);
            this.selectRange.TabIndex = 9;
            this.selectRange.Text = "...";
            this.selectRange.UseVisualStyleBackColor = true;
            this.selectRange.Click += new System.EventHandler(this.Select_Click);
            // 
            // viewsSheetsOption
            // 
            this.viewsSheetsOption.AutoSize = true;
            this.viewsSheetsOption.Location = new System.Drawing.Point(307, 19);
            this.viewsSheetsOption.Name = "viewsSheetsOption";
            this.viewsSheetsOption.Size = new System.Drawing.Size(132, 17);
            this.viewsSheetsOption.TabIndex = 8;
            this.viewsSheetsOption.TabStop = true;
            this.viewsSheetsOption.Text = "Selected views/sheets";
            this.viewsSheetsOption.UseVisualStyleBackColor = true;
            this.viewsSheetsOption.CheckedChanged += new System.EventHandler(this.ViewsSheets_CheckedChanged);
            // 
            // visiblePortionOption
            // 
            this.visiblePortionOption.AutoSize = true;
            this.visiblePortionOption.Location = new System.Drawing.Point(124, 19);
            this.visiblePortionOption.Name = "visiblePortionOption";
            this.visiblePortionOption.Size = new System.Drawing.Size(181, 17);
            this.visiblePortionOption.TabIndex = 7;
            this.visiblePortionOption.TabStop = true;
            this.visiblePortionOption.Text = "Visible portion of current window";
            this.visiblePortionOption.UseVisualStyleBackColor = true;
            this.visiblePortionOption.CheckedChanged += new System.EventHandler(this.VisiblePortion_CheckedChanged);
            // 
            // currentWindowOption
            // 
            this.currentWindowOption.AutoSize = true;
            this.currentWindowOption.Location = new System.Drawing.Point(20, 19);
            this.currentWindowOption.Name = "currentWindowOption";
            this.currentWindowOption.Size = new System.Drawing.Size(101, 17);
            this.currentWindowOption.TabIndex = 6;
            this.currentWindowOption.TabStop = true;
            this.currentWindowOption.Text = "Current window";
            this.currentWindowOption.UseVisualStyleBackColor = true;
            this.currentWindowOption.CheckedChanged += new System.EventHandler(this.CurrentWindow_CheckedChanged);
            // 
            // imageSizeGroupBox
            // 
            this.imageSizeGroupBox.Controls.Add(this.pixels);
            this.imageSizeGroupBox.Controls.Add(this.pixelValue);
            this.imageSizeGroupBox.Controls.Add(this.panel1);
            this.imageSizeGroupBox.Controls.Add(this.actual);
            this.imageSizeGroupBox.Controls.Add(this.zoomSize);
            this.imageSizeGroupBox.Controls.Add(this.Direction);
            this.imageSizeGroupBox.Controls.Add(this.zoomOption);
            this.imageSizeGroupBox.Controls.Add(this.fitToOption);
            this.imageSizeGroupBox.Location = new System.Drawing.Point(12, 128);
            this.imageSizeGroupBox.Name = "imageSizeGroupBox";
            this.imageSizeGroupBox.Size = new System.Drawing.Size(489, 77);
            this.imageSizeGroupBox.TabIndex = 2;
            this.imageSizeGroupBox.TabStop = false;
            this.imageSizeGroupBox.Text = "Image size";
            // 
            // pixels
            // 
            this.pixels.AutoSize = true;
            this.pixels.Location = new System.Drawing.Point(169, 20);
            this.pixels.Name = "pixels";
            this.pixels.Size = new System.Drawing.Size(34, 13);
            this.pixels.TabIndex = 2;
            this.pixels.Text = "pixels";
            // 
            // pixelValue
            // 
            this.pixelValue.Location = new System.Drawing.Point(84, 16);
            this.pixelValue.Name = "pixelValue";
            this.pixelValue.Size = new System.Drawing.Size(78, 21);
            this.pixelValue.TabIndex = 8;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.verticalOption);
            this.panel1.Controls.Add(this.horizontalOption);
            this.panel1.Location = new System.Drawing.Point(306, 16);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(162, 21);
            this.panel1.TabIndex = 7;
            // 
            // verticalOption
            // 
            this.verticalOption.AutoSize = true;
            this.verticalOption.Location = new System.Drawing.Point(3, 3);
            this.verticalOption.Name = "verticalOption";
            this.verticalOption.Size = new System.Drawing.Size(60, 17);
            this.verticalOption.TabIndex = 0;
            this.verticalOption.TabStop = true;
            this.verticalOption.Text = "Vertical";
            this.verticalOption.UseVisualStyleBackColor = true;
            this.verticalOption.CheckedChanged += new System.EventHandler(this.Vertical_CheckedChanged);
            // 
            // horizontalOption
            // 
            this.horizontalOption.AutoSize = true;
            this.horizontalOption.Location = new System.Drawing.Point(87, 3);
            this.horizontalOption.Name = "horizontalOption";
            this.horizontalOption.Size = new System.Drawing.Size(73, 17);
            this.horizontalOption.TabIndex = 1;
            this.horizontalOption.TabStop = true;
            this.horizontalOption.Text = "Horizontal";
            this.horizontalOption.UseVisualStyleBackColor = true;
            this.horizontalOption.CheckedChanged += new System.EventHandler(this.Horizontal_CheckedChanged);
            // 
            // actual
            // 
            this.actual.AutoSize = true;
            this.actual.Location = new System.Drawing.Point(169, 48);
            this.actual.Name = "actual";
            this.actual.Size = new System.Drawing.Size(84, 13);
            this.actual.TabIndex = 6;
            this.actual.Text = "% of actual size";
            // 
            // zoomSize
            // 
            this.zoomSize.Location = new System.Drawing.Point(84, 44);
            this.zoomSize.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.zoomSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.zoomSize.Name = "zoomSize";
            this.zoomSize.Size = new System.Drawing.Size(78, 21);
            this.zoomSize.TabIndex = 5;
            this.zoomSize.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // Direction
            // 
            this.Direction.AutoSize = true;
            this.Direction.Location = new System.Drawing.Point(240, 20);
            this.Direction.Name = "Direction";
            this.Direction.Size = new System.Drawing.Size(53, 13);
            this.Direction.TabIndex = 2;
            this.Direction.Text = "Direction:";
            // 
            // zoomOption
            // 
            this.zoomOption.AutoSize = true;
            this.zoomOption.Location = new System.Drawing.Point(20, 46);
            this.zoomOption.Name = "zoomOption";
            this.zoomOption.Size = new System.Drawing.Size(64, 17);
            this.zoomOption.TabIndex = 1;
            this.zoomOption.TabStop = true;
            this.zoomOption.Text = "Zoom to";
            this.zoomOption.UseVisualStyleBackColor = true;
            this.zoomOption.CheckedChanged += new System.EventHandler(this.Zoom_CheckedChanged);
            // 
            // fitToOption
            // 
            this.fitToOption.AutoSize = true;
            this.fitToOption.Location = new System.Drawing.Point(20, 18);
            this.fitToOption.Name = "fitToOption";
            this.fitToOption.Size = new System.Drawing.Size(50, 17);
            this.fitToOption.TabIndex = 0;
            this.fitToOption.TabStop = true;
            this.fitToOption.Text = "Fit to";
            this.fitToOption.UseVisualStyleBackColor = true;
            this.fitToOption.CheckedChanged += new System.EventHandler(this.FitTo_CheckedChanged);
            // 
            // formatGroupBox
            // 
            this.formatGroupBox.Controls.Add(this.RIQCom);
            this.formatGroupBox.Controls.Add(this.noShadedCom);
            this.formatGroupBox.Controls.Add(this.label1);
            this.formatGroupBox.Controls.Add(this.Non);
            this.formatGroupBox.Controls.Add(this.shadedCom);
            this.formatGroupBox.Controls.Add(this.shaded);
            this.formatGroupBox.Location = new System.Drawing.Point(12, 211);
            this.formatGroupBox.Name = "formatGroupBox";
            this.formatGroupBox.Size = new System.Drawing.Size(489, 70);
            this.formatGroupBox.TabIndex = 4;
            this.formatGroupBox.TabStop = false;
            this.formatGroupBox.Text = "Format";
            // 
            // RIQCom
            // 
            this.RIQCom.FormattingEnabled = true;
            this.RIQCom.Location = new System.Drawing.Point(347, 32);
            this.RIQCom.Name = "RIQCom";
            this.RIQCom.Size = new System.Drawing.Size(121, 21);
            this.RIQCom.TabIndex = 10;
            // 
            // noShadedCom
            // 
            this.noShadedCom.FormattingEnabled = true;
            this.noShadedCom.Location = new System.Drawing.Point(180, 32);
            this.noShadedCom.Name = "noShadedCom";
            this.noShadedCom.Size = new System.Drawing.Size(121, 21);
            this.noShadedCom.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(347, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Raster Image Quality";
            // 
            // Non
            // 
            this.Non.AutoSize = true;
            this.Non.Location = new System.Drawing.Point(180, 16);
            this.Non.Name = "Non";
            this.Non.Size = new System.Drawing.Size(98, 13);
            this.Non.TabIndex = 7;
            this.Non.Text = "Non shaded views:";
            // 
            // shadedCom
            // 
            this.shadedCom.FormattingEnabled = true;
            this.shadedCom.Location = new System.Drawing.Point(13, 32);
            this.shadedCom.Name = "shadedCom";
            this.shadedCom.Size = new System.Drawing.Size(121, 21);
            this.shadedCom.TabIndex = 6;
            // 
            // shaded
            // 
            this.shaded.AutoSize = true;
            this.shaded.Location = new System.Drawing.Point(13, 16);
            this.shaded.Name = "shaded";
            this.shaded.Size = new System.Drawing.Size(77, 13);
            this.shaded.TabIndex = 5;
            this.shaded.Text = "Shaded views:";
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(336, 307);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 0;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.OK_Click);
            // 
            // cannelButton
            // 
            this.cannelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cannelButton.Location = new System.Drawing.Point(426, 307);
            this.cannelButton.Name = "cannelButton";
            this.cannelButton.Size = new System.Drawing.Size(75, 23);
            this.cannelButton.TabIndex = 5;
            this.cannelButton.Text = "Cannel";
            this.cannelButton.UseVisualStyleBackColor = true;
            this.cannelButton.Click += new System.EventHandler(this.Cannel_Click);
            // 
            // ExportIMGOptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(516, 341);
            this.Controls.Add(this.outputGroupBox);
            this.Controls.Add(this.cannelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.formatGroupBox);
            this.Controls.Add(this.imageSizeGroupBox);
            this.Controls.Add(this.exportRangeGroupBox);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExportIMGOptionsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Export Image";
            this.outputGroupBox.ResumeLayout(false);
            this.outputGroupBox.PerformLayout();
            this.exportRangeGroupBox.ResumeLayout(false);
            this.exportRangeGroupBox.PerformLayout();
            this.imageSizeGroupBox.ResumeLayout(false);
            this.imageSizeGroupBox.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.zoomSize)).EndInit();
            this.formatGroupBox.ResumeLayout(false);
            this.formatGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox outputGroupBox;
        private System.Windows.Forms.Button changePath;
        private System.Windows.Forms.TextBox saveAs;
        private System.Windows.Forms.Label name;
        private System.Windows.Forms.GroupBox exportRangeGroupBox;
        private System.Windows.Forms.GroupBox imageSizeGroupBox;
        private System.Windows.Forms.GroupBox formatGroupBox;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cannelButton;
        private System.Windows.Forms.RadioButton viewsSheetsOption;
        private System.Windows.Forms.RadioButton visiblePortionOption;
        private System.Windows.Forms.RadioButton currentWindowOption;
        private System.Windows.Forms.Button selectRange;
        private System.Windows.Forms.RadioButton fitToOption;
        private System.Windows.Forms.RadioButton horizontalOption;
        private System.Windows.Forms.RadioButton verticalOption;
        private System.Windows.Forms.Label Direction;
        private System.Windows.Forms.RadioButton zoomOption;
        private System.Windows.Forms.NumericUpDown zoomSize;
        private System.Windows.Forms.Label actual;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label Non;
        private System.Windows.Forms.ComboBox shadedCom;
        private System.Windows.Forms.Label shaded;
        private System.Windows.Forms.ComboBox noShadedCom;
        private System.Windows.Forms.ComboBox RIQCom;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox pixelValue;
        private System.Windows.Forms.Label pixels;

    }
}