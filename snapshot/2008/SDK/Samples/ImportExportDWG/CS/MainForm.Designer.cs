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

namespace Revit.SDK.Samples.ImportAndExportForDWG.CS
{
    /// <summary>
    /// Provide a dialog which lets user choose the operation(export or import)
    /// </summary>
    partial class MainForm
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
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.groupBoxOperation = new System.Windows.Forms.GroupBox();
            this.radioButtonImport = new System.Windows.Forms.RadioButton();
            this.radioButtonExport = new System.Windows.Forms.RadioButton();
            this.groupBoxOperation.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(38, 93);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "&OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(119, 93);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "&Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // groupBoxOperation
            // 
            this.groupBoxOperation.Controls.Add(this.radioButtonImport);
            this.groupBoxOperation.Controls.Add(this.radioButtonExport);
            this.groupBoxOperation.Location = new System.Drawing.Point(12, 12);
            this.groupBoxOperation.Name = "groupBoxOperation";
            this.groupBoxOperation.Size = new System.Drawing.Size(182, 68);
            this.groupBoxOperation.TabIndex = 2;
            this.groupBoxOperation.TabStop = false;
            this.groupBoxOperation.Text = "Operation";
            // 
            // radioButtonImport
            // 
            this.radioButtonImport.AutoSize = true;
            this.radioButtonImport.Location = new System.Drawing.Point(7, 43);
            this.radioButtonImport.Name = "radioButtonImport";
            this.radioButtonImport.Size = new System.Drawing.Size(142, 17);
            this.radioButtonImport.TabIndex = 1;
            this.radioButtonImport.TabStop = true;
            this.radioButtonImport.Text = "Import from CAD Formats";
            this.radioButtonImport.UseVisualStyleBackColor = true;
            // 
            // radioButtonExport
            // 
            this.radioButtonExport.AutoSize = true;
            this.radioButtonExport.Checked = true;
            this.radioButtonExport.Location = new System.Drawing.Point(7, 20);
            this.radioButtonExport.Name = "radioButtonExport";
            this.radioButtonExport.Size = new System.Drawing.Size(132, 17);
            this.radioButtonExport.TabIndex = 0;
            this.radioButtonExport.TabStop = true;
            this.radioButtonExport.Text = "Export to CAD Formats";
            this.radioButtonExport.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(208, 123);
            this.Controls.Add(this.groupBoxOperation);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Export/Import";
            this.groupBoxOperation.ResumeLayout(false);
            this.groupBoxOperation.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.GroupBox groupBoxOperation;
        private System.Windows.Forms.RadioButton radioButtonImport;
        private System.Windows.Forms.RadioButton radioButtonExport;
    }
}