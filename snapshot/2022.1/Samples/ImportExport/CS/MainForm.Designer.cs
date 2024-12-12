//
// (C) Copyright 2003-2019 by Autodesk, Inc.
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
            this.comboBoxImport = new System.Windows.Forms.ComboBox();
            this.comboBoxExport = new System.Windows.Forms.ComboBox();
            this.radioButtonImport = new System.Windows.Forms.RadioButton();
            this.radioButtonExport = new System.Windows.Forms.RadioButton();
            this.groupBoxOperation.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(103, 112);
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
            this.buttonCancel.Location = new System.Drawing.Point(188, 112);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "&Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // groupBoxOperation
            // 
            this.groupBoxOperation.Controls.Add(this.comboBoxImport);
            this.groupBoxOperation.Controls.Add(this.comboBoxExport);
            this.groupBoxOperation.Controls.Add(this.radioButtonImport);
            this.groupBoxOperation.Controls.Add(this.radioButtonExport);
            this.groupBoxOperation.Location = new System.Drawing.Point(12, 12);
            this.groupBoxOperation.Name = "groupBoxOperation";
            this.groupBoxOperation.Size = new System.Drawing.Size(251, 81);
            this.groupBoxOperation.TabIndex = 2;
            this.groupBoxOperation.TabStop = false;
            this.groupBoxOperation.Text = "Operation";
            // 
            // comboBoxImport
            // 
            this.comboBoxImport.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxImport.FormattingEnabled = true;
            this.comboBoxImport.Location = new System.Drawing.Point(72, 51);
            this.comboBoxImport.Name = "comboBoxImport";
            this.comboBoxImport.Size = new System.Drawing.Size(161, 21);
            this.comboBoxImport.TabIndex = 2;
            // 
            // comboBoxExport
            // 
            this.comboBoxExport.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxExport.FormattingEnabled = true;
            this.comboBoxExport.Location = new System.Drawing.Point(72, 19);
            this.comboBoxExport.Name = "comboBoxExport";
            this.comboBoxExport.Size = new System.Drawing.Size(161, 21);
            this.comboBoxExport.TabIndex = 2;
            // 
            // radioButtonImport
            // 
            this.radioButtonImport.AutoSize = true;
            this.radioButtonImport.Location = new System.Drawing.Point(7, 52);
            this.radioButtonImport.Name = "radioButtonImport";
            this.radioButtonImport.Size = new System.Drawing.Size(54, 17);
            this.radioButtonImport.TabIndex = 1;
            this.radioButtonImport.TabStop = true;
            this.radioButtonImport.Text = "Import";
            this.radioButtonImport.UseVisualStyleBackColor = true;
            // 
            // radioButtonExport
            // 
            this.radioButtonExport.AutoSize = true;
            this.radioButtonExport.Checked = true;
            this.radioButtonExport.Location = new System.Drawing.Point(7, 21);
            this.radioButtonExport.Name = "radioButtonExport";
            this.radioButtonExport.Size = new System.Drawing.Size(58, 17);
            this.radioButtonExport.TabIndex = 0;
            this.radioButtonExport.TabStop = true;
            this.radioButtonExport.Text = "Export ";
            this.radioButtonExport.UseVisualStyleBackColor = true;
            this.radioButtonExport.CheckedChanged += new System.EventHandler(this.radioButtonExport_CheckedChanged);
            // 
            // MainForm
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(278, 145);
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
        private System.Windows.Forms.ComboBox comboBoxImport;
        private System.Windows.Forms.ComboBox comboBoxExport;
    }
}
