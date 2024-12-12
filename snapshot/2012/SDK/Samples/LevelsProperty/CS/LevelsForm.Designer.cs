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


namespace Revit.SDK.Samples.LevelsProperty.CS
{
    /// <summary>
    /// new level form
    /// </summary>
    partial class LevelsForm
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
            this.components = new System.ComponentModel.Container();
            this.levelsDataGridView = new System.Windows.Forms.DataGridView();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.addButton = new System.Windows.Forms.Button();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.levelsDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // levelsDataGridView
            // 
            this.levelsDataGridView.AllowUserToAddRows = false;
            this.levelsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.levelsDataGridView.Location = new System.Drawing.Point(16, 15);
            this.levelsDataGridView.Margin = new System.Windows.Forms.Padding(4, 4, 4, 7);
            this.levelsDataGridView.Name = "levelsDataGridView";
            this.levelsDataGridView.RowTemplate.Height = 24;
            this.levelsDataGridView.Size = new System.Drawing.Size(550, 445);
            this.levelsDataGridView.TabIndex = 0;
            this.levelsDataGridView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.levelsDataGridView_CellValueChanged);
            this.levelsDataGridView.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.levelsDataGridView_CellValidating);
            this.levelsDataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.levelsDataGridView_DataError);
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(382, 512);
            this.okButton.Margin = new System.Windows.Forms.Padding(4, 7, 4, 4);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(88, 28);
            this.okButton.TabIndex = 1;
            this.okButton.Text = "&OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(478, 512);
            this.cancelButton.Margin = new System.Windows.Forms.Padding(4, 4, 7, 4);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(88, 28);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // deleteButton
            // 
            this.deleteButton.Image = global::Revit.SDK.Samples.LevelsProperty.CS.Properties.Resources.delete;
            this.deleteButton.Location = new System.Drawing.Point(51, 473);
            this.deleteButton.Margin = new System.Windows.Forms.Padding(4, 6, 4, 4);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(30, 28);
            this.deleteButton.TabIndex = 4;
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            // 
            // addButton
            // 
            this.addButton.Image = global::Revit.SDK.Samples.LevelsProperty.CS.Properties.Resources._new;
            this.addButton.Location = new System.Drawing.Point(13, 473);
            this.addButton.Margin = new System.Windows.Forms.Padding(4, 6, 4, 4);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(30, 28);
            this.addButton.TabIndex = 3;
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // LevelsForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(579, 553);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.levelsDataGridView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LevelsForm";
            this.ShowInTaskbar = false;
            this.Text = "Levels Property";
            ((System.ComponentModel.ISupportInitialize)(this.levelsDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView levelsDataGridView;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Button deleteButton;

        //The following code is added by programmer.
        private System.Windows.Forms.DataGridViewTextBoxColumn LevelName;
        private System.Windows.Forms.DataGridViewTextBoxColumn LevelElevation;
    }
}