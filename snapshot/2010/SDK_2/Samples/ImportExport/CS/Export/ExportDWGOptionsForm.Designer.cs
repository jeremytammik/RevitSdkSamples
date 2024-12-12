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
    /// <summary>
    /// Provide a dialog which provides the options of lower priority information for export
    /// </summary>
    partial class ExportDWGOptionsForm
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
            this.labelLayersAndProperties = new System.Windows.Forms.Label();
            this.comboBoxLayersAndProperties = new System.Windows.Forms.ComboBox();
            this.labelLinetypeScaling = new System.Windows.Forms.Label();
            this.comboBoxLinetypeScaling = new System.Windows.Forms.ComboBox();
            this.labelCoorSystem = new System.Windows.Forms.Label();
            this.comboBoxCoorSystem = new System.Windows.Forms.ComboBox();
            this.labelDWGUnit = new System.Windows.Forms.Label();
            this.comboBoxDWGUnit = new System.Windows.Forms.ComboBox();
            this.labelSolids = new System.Windows.Forms.Label();
            this.comboBoxSolids = new System.Windows.Forms.ComboBox();
            this.checkBoxExportingAreas = new System.Windows.Forms.CheckBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.labelLayerSettings = new System.Windows.Forms.Label();
            this.comboBoxLayerSettings = new System.Windows.Forms.ComboBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.checkBoxMergeViews = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // labelLayersAndProperties
            // 
            this.labelLayersAndProperties.AutoSize = true;
            this.labelLayersAndProperties.Location = new System.Drawing.Point(13, 13);
            this.labelLayersAndProperties.Name = "labelLayersAndProperties";
            this.labelLayersAndProperties.Size = new System.Drawing.Size(111, 13);
            this.labelLayersAndProperties.TabIndex = 0;
            this.labelLayersAndProperties.Text = "Layers and properties:";
            // 
            // comboBoxLayersAndProperties
            // 
            this.comboBoxLayersAndProperties.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLayersAndProperties.FormattingEnabled = true;
            this.comboBoxLayersAndProperties.Location = new System.Drawing.Point(16, 30);
            this.comboBoxLayersAndProperties.Name = "comboBoxLayersAndProperties";
            this.comboBoxLayersAndProperties.Size = new System.Drawing.Size(301, 21);
            this.comboBoxLayersAndProperties.TabIndex = 1;
            // 
            // labelLinetypeScaling
            // 
            this.labelLinetypeScaling.AutoSize = true;
            this.labelLinetypeScaling.Location = new System.Drawing.Point(13, 96);
            this.labelLinetypeScaling.Name = "labelLinetypeScaling";
            this.labelLinetypeScaling.Size = new System.Drawing.Size(86, 13);
            this.labelLinetypeScaling.TabIndex = 0;
            this.labelLinetypeScaling.Text = "Linetype scaling:";
            // 
            // comboBoxLinetypeScaling
            // 
            this.comboBoxLinetypeScaling.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLinetypeScaling.FormattingEnabled = true;
            this.comboBoxLinetypeScaling.Location = new System.Drawing.Point(16, 113);
            this.comboBoxLinetypeScaling.Name = "comboBoxLinetypeScaling";
            this.comboBoxLinetypeScaling.Size = new System.Drawing.Size(301, 21);
            this.comboBoxLinetypeScaling.TabIndex = 3;
            // 
            // labelCoorSystem
            // 
            this.labelCoorSystem.AutoSize = true;
            this.labelCoorSystem.Location = new System.Drawing.Point(13, 142);
            this.labelCoorSystem.Name = "labelCoorSystem";
            this.labelCoorSystem.Size = new System.Drawing.Size(123, 13);
            this.labelCoorSystem.TabIndex = 0;
            this.labelCoorSystem.Text = "Coordinate system basis:";
            // 
            // comboBoxCoorSystem
            // 
            this.comboBoxCoorSystem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCoorSystem.FormattingEnabled = true;
            this.comboBoxCoorSystem.Location = new System.Drawing.Point(16, 157);
            this.comboBoxCoorSystem.Name = "comboBoxCoorSystem";
            this.comboBoxCoorSystem.Size = new System.Drawing.Size(301, 21);
            this.comboBoxCoorSystem.TabIndex = 4;
            // 
            // labelDWGUnit
            // 
            this.labelDWGUnit.AutoSize = true;
            this.labelDWGUnit.Location = new System.Drawing.Point(13, 184);
            this.labelDWGUnit.Name = "labelDWGUnit";
            this.labelDWGUnit.Size = new System.Drawing.Size(90, 13);
            this.labelDWGUnit.TabIndex = 0;
            this.labelDWGUnit.Text = "One DWG unit is:";
            // 
            // comboBoxDWGUnit
            // 
            this.comboBoxDWGUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDWGUnit.FormattingEnabled = true;
            this.comboBoxDWGUnit.Location = new System.Drawing.Point(16, 198);
            this.comboBoxDWGUnit.Name = "comboBoxDWGUnit";
            this.comboBoxDWGUnit.Size = new System.Drawing.Size(301, 21);
            this.comboBoxDWGUnit.TabIndex = 5;
            // 
            // labelSolids
            // 
            this.labelSolids.AutoSize = true;
            this.labelSolids.Location = new System.Drawing.Point(13, 224);
            this.labelSolids.Name = "labelSolids";
            this.labelSolids.Size = new System.Drawing.Size(113, 13);
            this.labelSolids.TabIndex = 0;
            this.labelSolids.Text = "Solids (3D views only):";
            // 
            // comboBoxSolids
            // 
            this.comboBoxSolids.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSolids.FormattingEnabled = true;
            this.comboBoxSolids.Location = new System.Drawing.Point(16, 241);
            this.comboBoxSolids.Name = "comboBoxSolids";
            this.comboBoxSolids.Size = new System.Drawing.Size(301, 21);
            this.comboBoxSolids.TabIndex = 6;
            // 
            // checkBoxExportingAreas
            // 
            this.checkBoxExportingAreas.AutoSize = true;
            this.checkBoxExportingAreas.Location = new System.Drawing.Point(16, 271);
            this.checkBoxExportingAreas.Name = "checkBoxExportingAreas";
            this.checkBoxExportingAreas.Size = new System.Drawing.Size(194, 17);
            this.checkBoxExportingAreas.TabIndex = 7;
            this.checkBoxExportingAreas.Text = "Export rooms and areas as polylines";
            this.checkBoxExportingAreas.UseVisualStyleBackColor = true;
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(161, 335);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "&OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // labelLayerSettings
            // 
            this.labelLayerSettings.AutoSize = true;
            this.labelLayerSettings.Location = new System.Drawing.Point(13, 55);
            this.labelLayerSettings.Name = "labelLayerSettings";
            this.labelLayerSettings.Size = new System.Drawing.Size(75, 13);
            this.labelLayerSettings.TabIndex = 0;
            this.labelLayerSettings.Text = "Layer settings:";
            // 
            // comboBoxLayerSettings
            // 
            this.comboBoxLayerSettings.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLayerSettings.FormattingEnabled = true;
            this.comboBoxLayerSettings.Location = new System.Drawing.Point(16, 72);
            this.comboBoxLayerSettings.Name = "comboBoxLayerSettings";
            this.comboBoxLayerSettings.Size = new System.Drawing.Size(301, 21);
            this.comboBoxLayerSettings.TabIndex = 2;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(242, 335);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 8;
            this.buttonCancel.Text = "&Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // checkBoxMergeViews
            // 
            this.checkBoxMergeViews.AutoSize = true;
            this.checkBoxMergeViews.Location = new System.Drawing.Point(16, 294);
            this.checkBoxMergeViews.Name = "checkBoxMergeViews";
            this.checkBoxMergeViews.Size = new System.Drawing.Size(220, 17);
            this.checkBoxMergeViews.TabIndex = 9;
            this.checkBoxMergeViews.Text = "Create separate files for each view/sheet";
            this.checkBoxMergeViews.UseVisualStyleBackColor = true;
            // 
            // ExportDWGOptionsForm
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(336, 368);
            this.Controls.Add(this.checkBoxMergeViews);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.checkBoxExportingAreas);
            this.Controls.Add(this.comboBoxSolids);
            this.Controls.Add(this.labelSolids);
            this.Controls.Add(this.comboBoxDWGUnit);
            this.Controls.Add(this.labelDWGUnit);
            this.Controls.Add(this.comboBoxCoorSystem);
            this.Controls.Add(this.labelCoorSystem);
            this.Controls.Add(this.comboBoxLinetypeScaling);
            this.Controls.Add(this.labelLinetypeScaling);
            this.Controls.Add(this.comboBoxLayerSettings);
            this.Controls.Add(this.labelLayerSettings);
            this.Controls.Add(this.comboBoxLayersAndProperties);
            this.Controls.Add(this.labelLayersAndProperties);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExportDWGOptionsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Export DWG Options";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelLayersAndProperties;
        private System.Windows.Forms.ComboBox comboBoxLayersAndProperties;
        private System.Windows.Forms.Label labelLinetypeScaling;
        private System.Windows.Forms.ComboBox comboBoxLinetypeScaling;
        private System.Windows.Forms.Label labelCoorSystem;
        private System.Windows.Forms.ComboBox comboBoxCoorSystem;
        private System.Windows.Forms.Label labelDWGUnit;
        private System.Windows.Forms.ComboBox comboBoxDWGUnit;
        private System.Windows.Forms.Label labelSolids;
        private System.Windows.Forms.ComboBox comboBoxSolids;
        private System.Windows.Forms.CheckBox checkBoxExportingAreas;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Label labelLayerSettings;
        private System.Windows.Forms.ComboBox comboBoxLayerSettings;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.CheckBox checkBoxMergeViews;
    }
}