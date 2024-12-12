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
namespace Revit.SDK.Samples.DoorSwing.CS
{
   partial class InitializeForm
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
         this.previewPictureBox = new System.Windows.Forms.PictureBox();
         this.customizeDoorOpeningDataGridView = new System.Windows.Forms.DataGridView();
         this.familyNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.OpeningColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
         this.okButton = new System.Windows.Forms.Button();
         this.cancelButton = new System.Windows.Forms.Button();
         this.FamilyWithOpeningParameter = new System.Windows.Forms.Label();
         this.FamilyGeometry = new System.Windows.Forms.Label();
         ((System.ComponentModel.ISupportInitialize)(this.previewPictureBox)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.customizeDoorOpeningDataGridView)).BeginInit();
         this.SuspendLayout();
         // 
         // previewPictureBox
         // 
         this.previewPictureBox.Location = new System.Drawing.Point(11, 30);
         this.previewPictureBox.Margin = new System.Windows.Forms.Padding(2);
         this.previewPictureBox.Name = "previewPictureBox";
         this.previewPictureBox.Size = new System.Drawing.Size(325, 341);
         this.previewPictureBox.TabIndex = 0;
         this.previewPictureBox.TabStop = false;
         this.previewPictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.previewPictureBox_Paint);
         // 
         // customizeDoorOpeningDataGridView
         // 
         this.customizeDoorOpeningDataGridView.AllowUserToAddRows = false;
         this.customizeDoorOpeningDataGridView.AllowUserToDeleteRows = false;
         this.customizeDoorOpeningDataGridView.AllowUserToOrderColumns = true;
         this.customizeDoorOpeningDataGridView.BackgroundColor = System.Drawing.Color.White;
         this.customizeDoorOpeningDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
         this.customizeDoorOpeningDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.familyNameColumn,
            this.OpeningColumn});
         this.customizeDoorOpeningDataGridView.Location = new System.Drawing.Point(353, 30);
         this.customizeDoorOpeningDataGridView.Margin = new System.Windows.Forms.Padding(2);
         this.customizeDoorOpeningDataGridView.Name = "customizeDoorOpeningDataGridView";
         this.customizeDoorOpeningDataGridView.RowHeadersVisible = false;
         this.customizeDoorOpeningDataGridView.RowTemplate.Height = 24;
         this.customizeDoorOpeningDataGridView.Size = new System.Drawing.Size(365, 341);
         this.customizeDoorOpeningDataGridView.TabIndex = 1;
         this.customizeDoorOpeningDataGridView.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.customizeDoorOpeningDataGridView_RowEnter);
         // 
         // familyNameColumn
         // 
         this.familyNameColumn.HeaderText = "Family Name";
         this.familyNameColumn.Name = "familyNameColumn";
         this.familyNameColumn.ReadOnly = true;
         this.familyNameColumn.Width = 180;
         // 
         // OpeningColumn
         // 
         this.OpeningColumn.HeaderText = "Door Opening";
         this.OpeningColumn.Name = "OpeningColumn";
         this.OpeningColumn.Width = 180;
         // 
         // okButton
         // 
         this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
         this.okButton.Location = new System.Drawing.Point(512, 385);
         this.okButton.Margin = new System.Windows.Forms.Padding(2);
         this.okButton.Name = "okButton";
         this.okButton.Size = new System.Drawing.Size(73, 29);
         this.okButton.TabIndex = 2;
         this.okButton.Text = "&OK";
         this.okButton.UseVisualStyleBackColor = true;
         // 
         // cancelButton
         // 
         this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         this.cancelButton.Location = new System.Drawing.Point(645, 385);
         this.cancelButton.Margin = new System.Windows.Forms.Padding(2);
         this.cancelButton.Name = "cancelButton";
         this.cancelButton.Size = new System.Drawing.Size(73, 29);
         this.cancelButton.TabIndex = 3;
         this.cancelButton.Text = "&Cancel";
         this.cancelButton.UseVisualStyleBackColor = true;
         // 
         // FamilyWithOpeningParameter
         // 
         this.FamilyWithOpeningParameter.AutoSize = true;
         this.FamilyWithOpeningParameter.Location = new System.Drawing.Point(350, 8);
         this.FamilyWithOpeningParameter.Name = "FamilyWithOpeningParameter";
         this.FamilyWithOpeningParameter.Size = new System.Drawing.Size(247, 13);
         this.FamilyWithOpeningParameter.TabIndex = 7;
         this.FamilyWithOpeningParameter.Text = "Select a family to customize its opening expression:";
         // 
         // FamilyGeometry
         // 
         this.FamilyGeometry.AutoSize = true;
         this.FamilyGeometry.Location = new System.Drawing.Point(9, 9);
         this.FamilyGeometry.Name = "FamilyGeometry";
         this.FamilyGeometry.Size = new System.Drawing.Size(85, 13);
         this.FamilyGeometry.TabIndex = 9;
         this.FamilyGeometry.Text = "Family geometry:";
         // 
         // InitializeForm
         // 
         this.AcceptButton = this.okButton;
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.CancelButton = this.cancelButton;
         this.ClientSize = new System.Drawing.Size(731, 425);
         this.Controls.Add(this.FamilyGeometry);
         this.Controls.Add(this.FamilyWithOpeningParameter);
         this.Controls.Add(this.cancelButton);
         this.Controls.Add(this.okButton);
         this.Controls.Add(this.customizeDoorOpeningDataGridView);
         this.Controls.Add(this.previewPictureBox);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
         this.Margin = new System.Windows.Forms.Padding(2);
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "InitializeForm";
         this.ShowInTaskbar = false;
         this.Text = "Customize Door Opening Expression";
         ((System.ComponentModel.ISupportInitialize)(this.previewPictureBox)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.customizeDoorOpeningDataGridView)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.PictureBox previewPictureBox;
      private System.Windows.Forms.DataGridView customizeDoorOpeningDataGridView;
      private System.Windows.Forms.Button okButton;
      private System.Windows.Forms.Button cancelButton;
      private System.Windows.Forms.DataGridViewTextBoxColumn familyNameColumn;
      private System.Windows.Forms.DataGridViewComboBoxColumn OpeningColumn;
      private System.Windows.Forms.Label FamilyWithOpeningParameter;
      private System.Windows.Forms.Label FamilyGeometry;
   }
}