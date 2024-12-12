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
    /// <summary>
    /// It contains a dialog which provides the options of import
    /// </summary>
    partial class ImportDWGForm
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
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOpen = new System.Windows.Forms.Button();
            this.labelFileName = new System.Windows.Forms.Label();
            this.textBoxFileSource = new System.Windows.Forms.TextBox();
            this.buttonBrowser = new System.Windows.Forms.Button();
            this.groupBoxPositioning = new System.Windows.Forms.GroupBox();
            this.labelAutomaticallyPlace = new System.Windows.Forms.Label();
            this.labelPlaceLevel = new System.Windows.Forms.Label();
            this.radioButtonOrigin2Origin = new System.Windows.Forms.RadioButton();
            this.radioButtonCenter2Center = new System.Windows.Forms.RadioButton();
            this.comboBoxLevel = new System.Windows.Forms.ComboBox();
            this.checkBoxOrient2View = new System.Windows.Forms.CheckBox();
            this.groupBoxScaling = new System.Windows.Forms.GroupBox();
            this.comboBoxUnits = new System.Windows.Forms.ComboBox();
            this.textBoxScale = new System.Windows.Forms.TextBox();
            this.labelScaleFactor = new System.Windows.Forms.Label();
            this.labelUnits = new System.Windows.Forms.Label();
            this.groupBoxColors = new System.Windows.Forms.GroupBox();
            this.radioButtonInvertColor = new System.Windows.Forms.RadioButton();
            this.radioButtonPreserve = new System.Windows.Forms.RadioButton();
            this.radioButtonBlackWhite = new System.Windows.Forms.RadioButton();
            this.checkBoxCurrentViewOnly = new System.Windows.Forms.CheckBox();
            this.comboBoxLayers = new System.Windows.Forms.ComboBox();
            this.labelLayers = new System.Windows.Forms.Label();
            this.groupBoxPositioning.SuspendLayout();
            this.groupBoxScaling.SuspendLayout();
            this.groupBoxColors.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(390, 285);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 21);
            this.buttonCancel.TabIndex = 8;
            this.buttonCancel.Text = "&Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonOpen
            // 
            this.buttonOpen.Location = new System.Drawing.Point(303, 285);
            this.buttonOpen.Name = "buttonOpen";
            this.buttonOpen.Size = new System.Drawing.Size(75, 21);
            this.buttonOpen.TabIndex = 7;
            this.buttonOpen.Text = "&Open";
            this.buttonOpen.UseVisualStyleBackColor = true;
            this.buttonOpen.Click += new System.EventHandler(this.buttonOpen_Click);
            // 
            // labelFileName
            // 
            this.labelFileName.AutoSize = true;
            this.labelFileName.Location = new System.Drawing.Point(10, 15);
            this.labelFileName.Name = "labelFileName";
            this.labelFileName.Size = new System.Drawing.Size(61, 13);
            this.labelFileName.TabIndex = 10;
            this.labelFileName.Text = "File source:";
            // 
            // textBoxFileSource
            // 
            this.textBoxFileSource.Location = new System.Drawing.Point(73, 11);
            this.textBoxFileSource.Name = "textBoxFileSource";
            this.textBoxFileSource.Size = new System.Drawing.Size(369, 20);
            this.textBoxFileSource.TabIndex = 1;
            // 
            // buttonBrowser
            // 
            this.buttonBrowser.Location = new System.Drawing.Point(442, 11);
            this.buttonBrowser.Name = "buttonBrowser";
            this.buttonBrowser.Size = new System.Drawing.Size(24, 20);
            this.buttonBrowser.TabIndex = 0;
            this.buttonBrowser.Text = "...";
            this.buttonBrowser.UseVisualStyleBackColor = true;
            this.buttonBrowser.Click += new System.EventHandler(this.buttonBrowser_Click);
            // 
            // groupBoxPositioning
            // 
            this.groupBoxPositioning.Controls.Add(this.labelAutomaticallyPlace);
            this.groupBoxPositioning.Controls.Add(this.labelPlaceLevel);
            this.groupBoxPositioning.Controls.Add(this.radioButtonOrigin2Origin);
            this.groupBoxPositioning.Controls.Add(this.radioButtonCenter2Center);
            this.groupBoxPositioning.Controls.Add(this.comboBoxLevel);
            this.groupBoxPositioning.Controls.Add(this.checkBoxOrient2View);
            this.groupBoxPositioning.Location = new System.Drawing.Point(229, 43);
            this.groupBoxPositioning.Name = "groupBoxPositioning";
            this.groupBoxPositioning.Size = new System.Drawing.Size(237, 157);
            this.groupBoxPositioning.TabIndex = 5;
            this.groupBoxPositioning.TabStop = false;
            this.groupBoxPositioning.Text = "Positioning";
            // 
            // labelAutomaticallyPlace
            // 
            this.labelAutomaticallyPlace.AutoSize = true;
            this.labelAutomaticallyPlace.Location = new System.Drawing.Point(15, 18);
            this.labelAutomaticallyPlace.Name = "labelAutomaticallyPlace";
            this.labelAutomaticallyPlace.Size = new System.Drawing.Size(98, 13);
            this.labelAutomaticallyPlace.TabIndex = 0;
            this.labelAutomaticallyPlace.Text = "Automatically place";
            // 
            // labelPlaceLevel
            // 
            this.labelPlaceLevel.AutoSize = true;
            this.labelPlaceLevel.Location = new System.Drawing.Point(14, 129);
            this.labelPlaceLevel.Name = "labelPlaceLevel";
            this.labelPlaceLevel.Size = new System.Drawing.Size(74, 13);
            this.labelPlaceLevel.TabIndex = 5;
            this.labelPlaceLevel.Text = "Place at level:";
            // 
            // radioButtonOrigin2Origin
            // 
            this.radioButtonOrigin2Origin.AutoSize = true;
            this.radioButtonOrigin2Origin.Location = new System.Drawing.Point(19, 60);
            this.radioButtonOrigin2Origin.Name = "radioButtonOrigin2Origin";
            this.radioButtonOrigin2Origin.Size = new System.Drawing.Size(92, 17);
            this.radioButtonOrigin2Origin.TabIndex = 2;
            this.radioButtonOrigin2Origin.TabStop = true;
            this.radioButtonOrigin2Origin.Text = "Origin-to-origin";
            this.radioButtonOrigin2Origin.UseVisualStyleBackColor = true;
            // 
            // radioButtonCenter2Center
            // 
            this.radioButtonCenter2Center.AutoSize = true;
            this.radioButtonCenter2Center.Checked = true;
            this.radioButtonCenter2Center.Location = new System.Drawing.Point(19, 37);
            this.radioButtonCenter2Center.Name = "radioButtonCenter2Center";
            this.radioButtonCenter2Center.Size = new System.Drawing.Size(101, 17);
            this.radioButtonCenter2Center.TabIndex = 1;
            this.radioButtonCenter2Center.TabStop = true;
            this.radioButtonCenter2Center.Text = "Center-to-center";
            this.radioButtonCenter2Center.UseVisualStyleBackColor = true;
            // 
            // comboBoxLevel
            // 
            this.comboBoxLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLevel.FormattingEnabled = true;
            this.comboBoxLevel.Location = new System.Drawing.Point(91, 125);
            this.comboBoxLevel.Name = "comboBoxLevel";
            this.comboBoxLevel.Size = new System.Drawing.Size(122, 21);
            this.comboBoxLevel.TabIndex = 4;
            // 
            // checkBoxOrient2View
            // 
            this.checkBoxOrient2View.AutoSize = true;
            this.checkBoxOrient2View.Checked = true;
            this.checkBoxOrient2View.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxOrient2View.Location = new System.Drawing.Point(17, 104);
            this.checkBoxOrient2View.Name = "checkBoxOrient2View";
            this.checkBoxOrient2View.Size = new System.Drawing.Size(92, 17);
            this.checkBoxOrient2View.TabIndex = 3;
            this.checkBoxOrient2View.Text = "Orient to View";
            this.checkBoxOrient2View.UseVisualStyleBackColor = true;
            // 
            // groupBoxScaling
            // 
            this.groupBoxScaling.Controls.Add(this.comboBoxUnits);
            this.groupBoxScaling.Controls.Add(this.textBoxScale);
            this.groupBoxScaling.Controls.Add(this.labelScaleFactor);
            this.groupBoxScaling.Controls.Add(this.labelUnits);
            this.groupBoxScaling.Location = new System.Drawing.Point(10, 202);
            this.groupBoxScaling.Name = "groupBoxScaling";
            this.groupBoxScaling.Size = new System.Drawing.Size(456, 50);
            this.groupBoxScaling.TabIndex = 6;
            this.groupBoxScaling.TabStop = false;
            this.groupBoxScaling.Text = "Scaling";
            // 
            // comboBoxUnits
            // 
            this.comboBoxUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxUnits.FormattingEnabled = true;
            this.comboBoxUnits.Location = new System.Drawing.Point(79, 18);
            this.comboBoxUnits.Name = "comboBoxUnits";
            this.comboBoxUnits.Size = new System.Drawing.Size(121, 21);
            this.comboBoxUnits.TabIndex = 0;
            this.comboBoxUnits.SelectedIndexChanged += new System.EventHandler(this.comboBoxUnits_SelectedIndexChanged);
            // 
            // textBoxScale
            // 
            this.textBoxScale.Enabled = false;
            this.textBoxScale.Location = new System.Drawing.Point(310, 19);
            this.textBoxScale.Name = "textBoxScale";
            this.textBoxScale.Size = new System.Drawing.Size(122, 20);
            this.textBoxScale.TabIndex = 1;
            this.textBoxScale.Text = 1.0.ToString("0.000000");
            this.textBoxScale.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxScale_KeyPress);
            this.textBoxScale.TextChanged += new System.EventHandler(this.textBoxScale_TextChanged);
            // 
            // labelScaleFactor
            // 
            this.labelScaleFactor.AutoSize = true;
            this.labelScaleFactor.Location = new System.Drawing.Point(234, 22);
            this.labelScaleFactor.Name = "labelScaleFactor";
            this.labelScaleFactor.Size = new System.Drawing.Size(67, 13);
            this.labelScaleFactor.TabIndex = 2;
            this.labelScaleFactor.Text = "Scale factor:";
            // 
            // labelUnits
            // 
            this.labelUnits.AutoSize = true;
            this.labelUnits.Location = new System.Drawing.Point(7, 22);
            this.labelUnits.Name = "labelUnits";
            this.labelUnits.Size = new System.Drawing.Size(64, 13);
            this.labelUnits.TabIndex = 0;
            this.labelUnits.Text = "Import units:";
            // 
            // groupBoxColors
            // 
            this.groupBoxColors.Controls.Add(this.radioButtonInvertColor);
            this.groupBoxColors.Controls.Add(this.radioButtonPreserve);
            this.groupBoxColors.Controls.Add(this.radioButtonBlackWhite);
            this.groupBoxColors.Location = new System.Drawing.Point(10, 43);
            this.groupBoxColors.Name = "groupBoxColors";
            this.groupBoxColors.Size = new System.Drawing.Size(200, 95);
            this.groupBoxColors.TabIndex = 2;
            this.groupBoxColors.TabStop = false;
            this.groupBoxColors.Text = "Layer/Level Colors";
            // 
            // radioButtonInvertColor
            // 
            this.radioButtonInvertColor.AutoSize = true;
            this.radioButtonInvertColor.Checked = true;
            this.radioButtonInvertColor.Location = new System.Drawing.Point(14, 66);
            this.radioButtonInvertColor.Name = "radioButtonInvertColor";
            this.radioButtonInvertColor.Size = new System.Drawing.Size(83, 17);
            this.radioButtonInvertColor.TabIndex = 2;
            this.radioButtonInvertColor.TabStop = true;
            this.radioButtonInvertColor.Text = "Invert colors";
            this.radioButtonInvertColor.UseVisualStyleBackColor = true;
            // 
            // radioButtonPreserve
            // 
            this.radioButtonPreserve.AutoSize = true;
            this.radioButtonPreserve.Location = new System.Drawing.Point(14, 43);
            this.radioButtonPreserve.Name = "radioButtonPreserve";
            this.radioButtonPreserve.Size = new System.Drawing.Size(98, 17);
            this.radioButtonPreserve.TabIndex = 1;
            this.radioButtonPreserve.Text = "Preserve colors";
            this.radioButtonPreserve.UseVisualStyleBackColor = true;
            // 
            // radioButtonBlackWhite
            // 
            this.radioButtonBlackWhite.AutoSize = true;
            this.radioButtonBlackWhite.Location = new System.Drawing.Point(15, 20);
            this.radioButtonBlackWhite.Name = "radioButtonBlackWhite";
            this.radioButtonBlackWhite.Size = new System.Drawing.Size(101, 17);
            this.radioButtonBlackWhite.TabIndex = 0;
            this.radioButtonBlackWhite.Text = "Black and white";
            this.radioButtonBlackWhite.UseVisualStyleBackColor = true;
            // 
            // checkBoxCurrentViewOnly
            // 
            this.checkBoxCurrentViewOnly.AutoSize = true;
            this.checkBoxCurrentViewOnly.Location = new System.Drawing.Point(17, 147);
            this.checkBoxCurrentViewOnly.Name = "checkBoxCurrentViewOnly";
            this.checkBoxCurrentViewOnly.Size = new System.Drawing.Size(107, 17);
            this.checkBoxCurrentViewOnly.TabIndex = 3;
            this.checkBoxCurrentViewOnly.Text = "Current view only";
            this.checkBoxCurrentViewOnly.UseVisualStyleBackColor = true;
            this.checkBoxCurrentViewOnly.CheckedChanged += new System.EventHandler(this.checkBoxCurrentViewOnly_CheckedChanged);
            // 
            // comboBoxLayers
            // 
            this.comboBoxLayers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLayers.FormattingEnabled = true;
            this.comboBoxLayers.Location = new System.Drawing.Point(88, 168);
            this.comboBoxLayers.Name = "comboBoxLayers";
            this.comboBoxLayers.Size = new System.Drawing.Size(122, 21);
            this.comboBoxLayers.TabIndex = 4;
            // 
            // labelLayers
            // 
            this.labelLayers.AutoSize = true;
            this.labelLayers.Location = new System.Drawing.Point(14, 172);
            this.labelLayers.Name = "labelLayers";
            this.labelLayers.Size = new System.Drawing.Size(41, 13);
            this.labelLayers.TabIndex = 5;
            this.labelLayers.Text = "Layers:";
            // 
            // ImportDWGForm
            // 
            this.AcceptButton = this.buttonOpen;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(480, 316);
            this.Controls.Add(this.checkBoxCurrentViewOnly);
            this.Controls.Add(this.groupBoxPositioning);
            this.Controls.Add(this.labelLayers);
            this.Controls.Add(this.groupBoxScaling);
            this.Controls.Add(this.groupBoxColors);
            this.Controls.Add(this.buttonBrowser);
            this.Controls.Add(this.comboBoxLayers);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOpen);
            this.Controls.Add(this.labelFileName);
            this.Controls.Add(this.textBoxFileSource);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ImportDWGForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Import";
            this.groupBoxPositioning.ResumeLayout(false);
            this.groupBoxPositioning.PerformLayout();
            this.groupBoxScaling.ResumeLayout(false);
            this.groupBoxScaling.PerformLayout();
            this.groupBoxColors.ResumeLayout(false);
            this.groupBoxColors.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOpen;
        private System.Windows.Forms.Label labelFileName;
        private System.Windows.Forms.TextBox textBoxFileSource;
        private System.Windows.Forms.Button buttonBrowser;
        private System.Windows.Forms.GroupBox groupBoxPositioning;
        private System.Windows.Forms.RadioButton radioButtonOrigin2Origin;
        private System.Windows.Forms.RadioButton radioButtonCenter2Center;
        private System.Windows.Forms.CheckBox checkBoxCurrentViewOnly;
        private System.Windows.Forms.ComboBox comboBoxLevel;
        private System.Windows.Forms.CheckBox checkBoxOrient2View;
        private System.Windows.Forms.GroupBox groupBoxScaling;
        private System.Windows.Forms.TextBox textBoxScale;
        private System.Windows.Forms.Label labelScaleFactor;
        private System.Windows.Forms.Label labelUnits;
        private System.Windows.Forms.GroupBox groupBoxColors;
        private System.Windows.Forms.RadioButton radioButtonInvertColor;
        private System.Windows.Forms.RadioButton radioButtonPreserve;
        private System.Windows.Forms.RadioButton radioButtonBlackWhite;
        private System.Windows.Forms.Label labelAutomaticallyPlace;
        private System.Windows.Forms.Label labelPlaceLevel;
        private System.Windows.Forms.ComboBox comboBoxUnits;
        private System.Windows.Forms.ComboBox comboBoxLayers;
        private System.Windows.Forms.Label labelLayers;
    }
}
