//
// (C) Copyright 2003-2013 by Autodesk, Inc.
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
    /// It contains a dialog which provides the options of common information for export
    /// </summary>
    partial class ExportWithViewsForm
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
            this.textBoxSaveAs = new System.Windows.Forms.TextBox();
            this.labelSaveAs = new System.Windows.Forms.Label();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOptions = new System.Windows.Forms.Button();
            this.buttonBrowser = new System.Windows.Forms.Button();
            this.groupBoxRange = new System.Windows.Forms.GroupBox();
            this.buttonSelectViews = new System.Windows.Forms.Button();
            this.radioButtonSelectView = new System.Windows.Forms.RadioButton();
            this.radioButtonCurrentView = new System.Windows.Forms.RadioButton();
            this.groupBoxRange.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxSaveAs
            // 
            this.textBoxSaveAs.Location = new System.Drawing.Point(67, 19);
            this.textBoxSaveAs.Name = "textBoxSaveAs";
            this.textBoxSaveAs.Size = new System.Drawing.Size(308, 21);
            this.textBoxSaveAs.TabIndex = 1;
            // 
            // labelSaveAs
            // 
            this.labelSaveAs.AutoSize = true;
            this.labelSaveAs.Location = new System.Drawing.Point(14, 23);
            this.labelSaveAs.Name = "labelSaveAs";
            this.labelSaveAs.Size = new System.Drawing.Size(50, 13);
            this.labelSaveAs.TabIndex = 1;
            this.labelSaveAs.Text = "Save As:";
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(235, 141);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 21);
            this.buttonSave.TabIndex = 0;
            this.buttonSave.Text = "&Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(320, 141);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 21);
            this.buttonCancel.TabIndex = 6;
            this.buttonCancel.Text = "&Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonOptions
            // 
            this.buttonOptions.Location = new System.Drawing.Point(150, 141);
            this.buttonOptions.Name = "buttonOptions";
            this.buttonOptions.Size = new System.Drawing.Size(75, 21);
            this.buttonOptions.TabIndex = 5;
            this.buttonOptions.Text = "&Options...";
            this.buttonOptions.UseVisualStyleBackColor = true;
            this.buttonOptions.Click += new System.EventHandler(this.buttonOptions_Click);
            // 
            // buttonBrowser
            // 
            this.buttonBrowser.Location = new System.Drawing.Point(375, 19);
            this.buttonBrowser.Name = "buttonBrowser";
            this.buttonBrowser.Size = new System.Drawing.Size(24, 20);
            this.buttonBrowser.TabIndex = 2;
            this.buttonBrowser.Text = "...";
            this.buttonBrowser.UseVisualStyleBackColor = true;
            this.buttonBrowser.Click += new System.EventHandler(this.buttonBrowser_Click);
            // 
            // groupBoxRange
            // 
            this.groupBoxRange.Controls.Add(this.buttonSelectViews);
            this.groupBoxRange.Controls.Add(this.radioButtonSelectView);
            this.groupBoxRange.Controls.Add(this.radioButtonCurrentView);
            this.groupBoxRange.Location = new System.Drawing.Point(17, 46);
            this.groupBoxRange.Name = "groupBoxRange";
            this.groupBoxRange.Size = new System.Drawing.Size(378, 73);
            this.groupBoxRange.TabIndex = 4;
            this.groupBoxRange.TabStop = false;
            this.groupBoxRange.Text = "Range";
            // 
            // buttonSelectViews
            // 
            this.buttonSelectViews.Enabled = false;
            this.buttonSelectViews.Location = new System.Drawing.Point(145, 42);
            this.buttonSelectViews.Name = "buttonSelectViews";
            this.buttonSelectViews.Size = new System.Drawing.Size(27, 21);
            this.buttonSelectViews.TabIndex = 2;
            this.buttonSelectViews.Text = "...";
            this.buttonSelectViews.UseVisualStyleBackColor = true;
            this.buttonSelectViews.Click += new System.EventHandler(this.buttonSelectViews_Click);
            // 
            // radioButtonSelectView
            // 
            this.radioButtonSelectView.AutoSize = true;
            this.radioButtonSelectView.Location = new System.Drawing.Point(9, 44);
            this.radioButtonSelectView.Name = "radioButtonSelectView";
            this.radioButtonSelectView.Size = new System.Drawing.Size(132, 17);
            this.radioButtonSelectView.TabIndex = 1;
            this.radioButtonSelectView.Text = "Selected views/sheets";
            this.radioButtonSelectView.UseVisualStyleBackColor = true;
            this.radioButtonSelectView.CheckedChanged += new System.EventHandler(this.radioButtonSelectView_CheckedChanged);
            // 
            // radioButtonCurrentView
            // 
            this.radioButtonCurrentView.AutoSize = true;
            this.radioButtonCurrentView.Checked = true;
            this.radioButtonCurrentView.Location = new System.Drawing.Point(9, 20);
            this.radioButtonCurrentView.Name = "radioButtonCurrentView";
            this.radioButtonCurrentView.Size = new System.Drawing.Size(87, 17);
            this.radioButtonCurrentView.TabIndex = 0;
            this.radioButtonCurrentView.TabStop = true;
            this.radioButtonCurrentView.Text = "Current view";
            this.radioButtonCurrentView.UseVisualStyleBackColor = true;
            // 
            // ExportWithViewsForm
            // 
            this.AcceptButton = this.buttonSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(411, 169);
            this.Controls.Add(this.groupBoxRange);
            this.Controls.Add(this.buttonOptions);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonBrowser);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.labelSaveAs);
            this.Controls.Add(this.textBoxSaveAs);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExportWithViewsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Export";
            this.groupBoxRange.ResumeLayout(false);
            this.groupBoxRange.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxSaveAs;
        private System.Windows.Forms.Label labelSaveAs;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOptions;
        private System.Windows.Forms.Button buttonBrowser;
        private System.Windows.Forms.GroupBox groupBoxRange;
        private System.Windows.Forms.Button buttonSelectViews;
        private System.Windows.Forms.RadioButton radioButtonSelectView;
        private System.Windows.Forms.RadioButton radioButtonCurrentView;
    }
}
