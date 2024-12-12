//
// (C) Copyright 2003-2015 by Autodesk, Inc.
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


namespace Revit.SDK.Samples.AnalyticalSupportData_Info.CS
{
    partial class AnalyticalSupportData_InfoForm
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
           this.closeButton = new System.Windows.Forms.Button();
           this.elementInfoDataGridView = new System.Windows.Forms.DataGridView();
           this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
           this.typeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
           this.support = new System.Windows.Forms.DataGridViewTextBoxColumn();
           this.remark = new System.Windows.Forms.DataGridViewTextBoxColumn();
           ((System.ComponentModel.ISupportInitialize)(this.elementInfoDataGridView)).BeginInit();
           this.SuspendLayout();
           // 
           // closeButton
           // 
           this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
           this.closeButton.Location = new System.Drawing.Point(690, 342);
           this.closeButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
           this.closeButton.Name = "closeButton";
           this.closeButton.Size = new System.Drawing.Size(68, 24);
           this.closeButton.TabIndex = 0;
           this.closeButton.Text = "&Close";
           this.closeButton.UseVisualStyleBackColor = true;
           this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
           // 
           // elementInfoDataGridView
           // 
           this.elementInfoDataGridView.AllowUserToAddRows = false;
           this.elementInfoDataGridView.AllowUserToDeleteRows = false;
           this.elementInfoDataGridView.BackgroundColor = System.Drawing.SystemColors.ActiveCaptionText;
           this.elementInfoDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
           this.elementInfoDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.typeName,
            this.support,
            this.remark});
           this.elementInfoDataGridView.Location = new System.Drawing.Point(15, 10);
           this.elementInfoDataGridView.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
           this.elementInfoDataGridView.Name = "elementInfoDataGridView";
           this.elementInfoDataGridView.ReadOnly = true;
           this.elementInfoDataGridView.RowHeadersVisible = false;
           this.elementInfoDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
           this.elementInfoDataGridView.RowTemplate.Height = 24;
           this.elementInfoDataGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
           this.elementInfoDataGridView.Size = new System.Drawing.Size(743, 324);
           this.elementInfoDataGridView.TabIndex = 1;
           // 
           // id
           // 
           this.id.HeaderText = "Element ID";
           this.id.Name = "id";
           this.id.ReadOnly = true;
           this.id.Width = 90;
           // 
           // typeName
           // 
           this.typeName.HeaderText = "Element Type";
           this.typeName.Name = "typeName";
           this.typeName.ReadOnly = true;
           this.typeName.Width = 250;
           // 
           // support
           // 
           this.support.HeaderText = "Support Type";
           this.support.Name = "support";
           this.support.ReadOnly = true;
           this.support.Width = 200;
           // 
           // remark
           // 
           this.remark.HeaderText = "Remark";
           this.remark.Name = "remark";
           this.remark.ReadOnly = true;
           this.remark.Width = 200;
           // 
           // AnalyticalSupportData_InfoForm
           // 
           this.AcceptButton = this.closeButton;
           this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
           this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
           this.CancelButton = this.closeButton;
           this.ClientSize = new System.Drawing.Size(772, 370);
           this.Controls.Add(this.elementInfoDataGridView);
           this.Controls.Add(this.closeButton);
           this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
           this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
           this.MaximizeBox = false;
           this.MinimizeBox = false;
           this.Name = "AnalyticalSupportData_InfoForm";
           this.ShowInTaskbar = false;
           this.Text = "Analytical Support Data";
           ((System.ComponentModel.ISupportInitialize)(this.elementInfoDataGridView)).EndInit();
           this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button closeButton;
       private System.Windows.Forms.DataGridView elementInfoDataGridView;
       private System.Windows.Forms.DataGridViewTextBoxColumn id;
       private System.Windows.Forms.DataGridViewTextBoxColumn typeName;
       private System.Windows.Forms.DataGridViewTextBoxColumn support;
       private System.Windows.Forms.DataGridViewTextBoxColumn remark;
    }
}