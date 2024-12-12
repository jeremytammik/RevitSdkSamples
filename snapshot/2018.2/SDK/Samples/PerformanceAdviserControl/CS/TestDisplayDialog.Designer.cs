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
namespace Revit.SDK.Samples.PerformanceAdviserControl.CS
{
   partial class TestDisplayDialog
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
         this.testData = new System.Windows.Forms.DataGridView();
         this.runTest = new System.Windows.Forms.DataGridViewCheckBoxColumn();
         this.testName = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.IsOurRule = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.btn_RunTests = new System.Windows.Forms.Button();
         this.btn_SelectAll = new System.Windows.Forms.Button();
         this.btn_DeselectAll = new System.Windows.Forms.Button();
         this.btn_Cancel = new System.Windows.Forms.Button();
         ((System.ComponentModel.ISupportInitialize)(this.testData)).BeginInit();
         this.SuspendLayout();
         // 
         // testData
         // 
         this.testData.AllowUserToAddRows = false;
         this.testData.AllowUserToDeleteRows = false;
         this.testData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
         this.testData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.runTest,
            this.testName,
            this.IsOurRule});
         this.testData.Dock = System.Windows.Forms.DockStyle.Top;
         this.testData.Location = new System.Drawing.Point(0, 0);
         this.testData.MultiSelect = false;
         this.testData.Name = "testData";
         this.testData.RowHeadersWidth = 30;
         this.testData.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
         this.testData.Size = new System.Drawing.Size(469, 418);
         this.testData.TabIndex = 2;
         // 
         // runTest
         // 
         this.runTest.HeaderText = " ";
         this.runTest.MinimumWidth = 50;
         this.runTest.Name = "runTest";
         this.runTest.Width = 50;
         // 
         // testName
         // 
         this.testName.FillWeight = 300F;
         this.testName.HeaderText = "Test Name";
         this.testName.MinimumWidth = 300;
         this.testName.Name = "testName";
         this.testName.ReadOnly = true;
         this.testName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
         this.testName.Width = 300;
         // 
         // IsOurRule
         // 
         this.IsOurRule.HeaderText = "Our Rule";
         this.IsOurRule.MinimumWidth = 90;
         this.IsOurRule.Name = "IsOurRule";
         this.IsOurRule.ReadOnly = true;
         this.IsOurRule.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
         this.IsOurRule.Width = 90;
         // 
         // btn_RunTests
         // 
         this.btn_RunTests.Location = new System.Drawing.Point(237, 424);
         this.btn_RunTests.Name = "btn_RunTests";
         this.btn_RunTests.Size = new System.Drawing.Size(147, 23);
         this.btn_RunTests.TabIndex = 3;
         this.btn_RunTests.Text = "Run Selected Tests";
         this.btn_RunTests.UseVisualStyleBackColor = true;
         this.btn_RunTests.Click += new System.EventHandler(this.btn_RunTests_Click);
         // 
         // btn_SelectAll
         // 
         this.btn_SelectAll.Location = new System.Drawing.Point(59, 424);
         this.btn_SelectAll.Name = "btn_SelectAll";
         this.btn_SelectAll.Size = new System.Drawing.Size(147, 23);
         this.btn_SelectAll.TabIndex = 4;
         this.btn_SelectAll.Text = "Select All";
         this.btn_SelectAll.UseVisualStyleBackColor = true;
         this.btn_SelectAll.Click += new System.EventHandler(this.btn_SelectAll_Click);
         // 
         // btn_DeselectAll
         // 
         this.btn_DeselectAll.Location = new System.Drawing.Point(59, 453);
         this.btn_DeselectAll.Name = "btn_DeselectAll";
         this.btn_DeselectAll.Size = new System.Drawing.Size(147, 23);
         this.btn_DeselectAll.TabIndex = 5;
         this.btn_DeselectAll.Text = "Deselect all";
         this.btn_DeselectAll.UseVisualStyleBackColor = true;
         this.btn_DeselectAll.Click += new System.EventHandler(this.btn_DeselectAll_Click);
         // 
         // btn_Cancel
         // 
         this.btn_Cancel.Location = new System.Drawing.Point(237, 453);
         this.btn_Cancel.Name = "btn_Cancel";
         this.btn_Cancel.Size = new System.Drawing.Size(147, 23);
         this.btn_Cancel.TabIndex = 6;
         this.btn_Cancel.Text = "Cancel";
         this.btn_Cancel.UseVisualStyleBackColor = true;
         this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
         // 
         // TestDisplayDialog
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(469, 483);
         this.Controls.Add(this.btn_Cancel);
         this.Controls.Add(this.btn_DeselectAll);
         this.Controls.Add(this.btn_SelectAll);
         this.Controls.Add(this.btn_RunTests);
         this.Controls.Add(this.testData);
         this.MaximizeBox = false;
         this.Name = "TestDisplayDialog";
         this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
         this.Text = "PerformanceAdviser Control";
         ((System.ComponentModel.ISupportInitialize)(this.testData)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.DataGridView testData;
      private System.Windows.Forms.Button btn_RunTests;
      private System.Windows.Forms.Button btn_SelectAll;
      private System.Windows.Forms.Button btn_DeselectAll;
      private System.Windows.Forms.Button btn_Cancel;
      private System.Windows.Forms.DataGridViewCheckBoxColumn runTest;
      private System.Windows.Forms.DataGridViewTextBoxColumn testName;
      private System.Windows.Forms.DataGridViewTextBoxColumn IsOurRule;
   }
}