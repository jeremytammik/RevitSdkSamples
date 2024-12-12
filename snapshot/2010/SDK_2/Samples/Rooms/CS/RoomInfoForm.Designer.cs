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


namespace Revit.SDK.Samples.Rooms.CS
{
   partial class roomsInformationForm
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
          this.addTagsButton = new System.Windows.Forms.Button();
          this.closeButton = new System.Windows.Forms.Button();
          this.roomsGroupBox = new System.Windows.Forms.GroupBox();
          this.roomsListView = new System.Windows.Forms.ListView();
          this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
          this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
          this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
          this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
          this.columnHeader8 = new System.Windows.Forms.ColumnHeader();
          this.columnHeader9 = new System.Windows.Forms.ColumnHeader();
          this.columnHeader10 = new System.Windows.Forms.ColumnHeader();
          this.reorderButton = new System.Windows.Forms.Button();
          this.exportButton = new System.Windows.Forms.Button();
          this.departmentGroupBox = new System.Windows.Forms.GroupBox();
          this.departmentsListView = new System.Windows.Forms.ListView();
          this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
          this.columnHeader7 = new System.Windows.Forms.ColumnHeader();
          this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
          this.roomsGroupBox.SuspendLayout();
          this.departmentGroupBox.SuspendLayout();
          this.SuspendLayout();
          // 
          // addTagsButton
          // 
          this.addTagsButton.Location = new System.Drawing.Point(428, 321);
          this.addTagsButton.Name = "addTagsButton";
          this.addTagsButton.Size = new System.Drawing.Size(90, 25);
          this.addTagsButton.TabIndex = 1;
          this.addTagsButton.Text = "&Add Tags";
          this.addTagsButton.UseVisualStyleBackColor = true;
          this.addTagsButton.Click += new System.EventHandler(this.addTagButton_Click);
          // 
          // closeButton
          // 
          this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
          this.closeButton.Location = new System.Drawing.Point(428, 414);
          this.closeButton.Name = "closeButton";
          this.closeButton.Size = new System.Drawing.Size(90, 25);
          this.closeButton.TabIndex = 4;
          this.closeButton.Text = "&Close";
          this.closeButton.UseVisualStyleBackColor = true;
          // 
          // roomsGroupBox
          // 
          this.roomsGroupBox.Controls.Add(this.roomsListView);
          this.roomsGroupBox.Location = new System.Drawing.Point(4, 12);
          this.roomsGroupBox.Name = "roomsGroupBox";
          this.roomsGroupBox.Size = new System.Drawing.Size(519, 246);
          this.roomsGroupBox.TabIndex = 7;
          this.roomsGroupBox.TabStop = false;
          this.roomsGroupBox.Text = "Rooms Information";
          // 
          // roomsListView
          // 
          this.roomsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader4,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader8,
            this.columnHeader9,
            this.columnHeader10});
          this.roomsListView.FullRowSelect = true;
          this.roomsListView.GridLines = true;
          this.roomsListView.Location = new System.Drawing.Point(6, 19);
          this.roomsListView.MultiSelect = false;
          this.roomsListView.Name = "roomsListView";
          this.roomsListView.Size = new System.Drawing.Size(508, 215);
          this.roomsListView.Sorting = System.Windows.Forms.SortOrder.Ascending;
          this.roomsListView.TabIndex = 5;
          this.roomsListView.UseCompatibleStateImageBehavior = false;
          this.roomsListView.View = System.Windows.Forms.View.Details;
          // 
          // columnHeader1
          // 
          this.columnHeader1.Text = "ID";
          this.columnHeader1.Width = 80;
          // 
          // columnHeader4
          // 
          this.columnHeader4.Text = "Name";
          // 
          // columnHeader2
          // 
          this.columnHeader2.Text = "Number";
          this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
          // 
          // columnHeader3
          // 
          this.columnHeader3.Text = "level";
          this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
          // 
          // columnHeader8
          // 
          this.columnHeader8.Text = "Department";
          this.columnHeader8.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
          this.columnHeader8.Width = 80;
          // 
          // columnHeader9
          // 
          this.columnHeader9.Text = "Area";
          this.columnHeader9.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
          this.columnHeader9.Width = 80;
          // 
          // columnHeader10
          // 
          this.columnHeader10.Text = "Have tag";
          this.columnHeader10.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
          // 
          // reorderButton
          // 
          this.reorderButton.Location = new System.Drawing.Point(428, 352);
          this.reorderButton.Name = "reorderButton";
          this.reorderButton.Size = new System.Drawing.Size(90, 25);
          this.reorderButton.TabIndex = 2;
          this.reorderButton.Text = "&Reorder";
          this.reorderButton.UseVisualStyleBackColor = true;
          this.reorderButton.Click += new System.EventHandler(this.reorderButton_Click);
          // 
          // exportButton
          // 
          this.exportButton.Location = new System.Drawing.Point(428, 383);
          this.exportButton.Name = "exportButton";
          this.exportButton.Size = new System.Drawing.Size(90, 25);
          this.exportButton.TabIndex = 3;
          this.exportButton.Text = "&Export";
          this.exportButton.UseVisualStyleBackColor = true;
          this.exportButton.Click += new System.EventHandler(this.exportButton_Click);
          // 
          // departmentGroupBox
          // 
          this.departmentGroupBox.Controls.Add(this.departmentsListView);
          this.departmentGroupBox.Location = new System.Drawing.Point(4, 276);
          this.departmentGroupBox.Name = "departmentGroupBox";
          this.departmentGroupBox.Size = new System.Drawing.Size(399, 171);
          this.departmentGroupBox.TabIndex = 8;
          this.departmentGroupBox.TabStop = false;
          this.departmentGroupBox.Text = "Area of Departments";
          // 
          // departmentsListView
          // 
          this.departmentsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader5});
          this.departmentsListView.FullRowSelect = true;
          this.departmentsListView.GridLines = true;
          this.departmentsListView.Location = new System.Drawing.Point(6, 19);
          this.departmentsListView.Name = "departmentsListView";
          this.departmentsListView.Size = new System.Drawing.Size(387, 144);
          this.departmentsListView.Sorting = System.Windows.Forms.SortOrder.Ascending;
          this.departmentsListView.TabIndex = 6;
          this.departmentsListView.UseCompatibleStateImageBehavior = false;
          this.departmentsListView.View = System.Windows.Forms.View.Details;
          // 
          // columnHeader6
          // 
          this.columnHeader6.Text = "Department";
          this.columnHeader6.Width = 120;
          // 
          // columnHeader7
          // 
          this.columnHeader7.Text = "Rooms Amount";
          this.columnHeader7.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
          this.columnHeader7.Width = 100;
          // 
          // columnHeader5
          // 
          this.columnHeader5.Text = "Total Area";
          this.columnHeader5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
          this.columnHeader5.Width = 100;
          // 
          // roomsInformationForm
          // 
          this.AcceptButton = this.exportButton;
          this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
          this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
          this.CancelButton = this.closeButton;
          this.ClientSize = new System.Drawing.Size(528, 452);
          this.Controls.Add(this.departmentGroupBox);
          this.Controls.Add(this.exportButton);
          this.Controls.Add(this.reorderButton);
          this.Controls.Add(this.roomsGroupBox);
          this.Controls.Add(this.closeButton);
          this.Controls.Add(this.addTagsButton);
          this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
          this.MaximizeBox = false;
          this.MinimizeBox = false;
          this.Name = "roomsInformationForm";
          this.ShowInTaskbar = false;
          this.Text = "Rooms information";
          this.Load += new System.EventHandler(this.RoomInfoForm_Load);
          this.roomsGroupBox.ResumeLayout(false);
          this.departmentGroupBox.ResumeLayout(false);
          this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.Button addTagsButton;
      private System.Windows.Forms.Button closeButton;
      private System.Windows.Forms.GroupBox roomsGroupBox;
      private System.Windows.Forms.ListView roomsListView;
      private System.Windows.Forms.ColumnHeader columnHeader1;
      private System.Windows.Forms.ColumnHeader columnHeader4;
      private System.Windows.Forms.ColumnHeader columnHeader2;
      private System.Windows.Forms.ColumnHeader columnHeader3;
      private System.Windows.Forms.Button reorderButton;
      private System.Windows.Forms.ColumnHeader columnHeader8;
      private System.Windows.Forms.ColumnHeader columnHeader9;
      private System.Windows.Forms.Button exportButton;
      private System.Windows.Forms.ColumnHeader columnHeader10;
      private System.Windows.Forms.GroupBox departmentGroupBox;
      private System.Windows.Forms.ListView departmentsListView;
      private System.Windows.Forms.ColumnHeader columnHeader6;
      private System.Windows.Forms.ColumnHeader columnHeader7;
      private System.Windows.Forms.ColumnHeader columnHeader5;
   }
}