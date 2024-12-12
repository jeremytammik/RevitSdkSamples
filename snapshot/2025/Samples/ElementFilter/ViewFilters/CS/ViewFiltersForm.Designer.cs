//
// (C) Copyright 2003-2023 by Autodesk, Inc.
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


namespace Revit.SDK.Samples.ViewFilters.CS
{
    partial class ViewFiltersForm
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
          this.filtersGroupBox = new System.Windows.Forms.GroupBox();
          this.filtersListBox = new System.Windows.Forms.ListBox();
          this.deleteButton = new System.Windows.Forms.Button();
          this.newButton = new System.Windows.Forms.Button();
          this.categoriesGroupBox = new System.Windows.Forms.GroupBox();
          this.label2 = new System.Windows.Forms.Label();
          this.categoryCheckedListBox = new System.Windows.Forms.CheckedListBox();
          this.checkNoneButton = new System.Windows.Forms.Button();
          this.checkAllButton = new System.Windows.Forms.Button();
          this.hideUnCheckCheckBox = new System.Windows.Forms.CheckBox();
          this.groupBox1 = new System.Windows.Forms.GroupBox();
          this.rulesListBox = new System.Windows.Forms.ListBox();
          this.epsilonLabel = new System.Windows.Forms.Label();
          this.label3 = new System.Windows.Forms.Label();
          this.ruleValueComboBox = new System.Windows.Forms.ComboBox();
          this.criteriaComboBox = new System.Windows.Forms.ComboBox();
          this.paramerComboBox = new System.Windows.Forms.ComboBox();
          this.label1 = new System.Windows.Forms.Label();
          this.deleRuleButton = new System.Windows.Forms.Button();
          this.updateButton = new System.Windows.Forms.Button();
          this.newRuleButton = new System.Windows.Forms.Button();
          this.caseSensitiveCheckBox = new System.Windows.Forms.CheckBox();
          this.epsilonTextBox = new System.Windows.Forms.TextBox();
          this.cancelButton = new System.Windows.Forms.Button();
          this.okButton = new System.Windows.Forms.Button();
          this.filtersGroupBox.SuspendLayout();
          this.categoriesGroupBox.SuspendLayout();
          this.groupBox1.SuspendLayout();
          this.SuspendLayout();
          // 
          // filtersGroupBox
          // 
          this.filtersGroupBox.Controls.Add(this.filtersListBox);
          this.filtersGroupBox.Controls.Add(this.deleteButton);
          this.filtersGroupBox.Controls.Add(this.newButton);
          this.filtersGroupBox.Location = new System.Drawing.Point(4, 12);
          this.filtersGroupBox.Name = "filtersGroupBox";
          this.filtersGroupBox.Size = new System.Drawing.Size(187, 347);
          this.filtersGroupBox.TabIndex = 7;
          this.filtersGroupBox.TabStop = false;
          this.filtersGroupBox.Text = "Filters";
          // 
          // filtersListBox
          // 
          this.filtersListBox.FormattingEnabled = true;
          this.filtersListBox.Location = new System.Drawing.Point(8, 19);
          this.filtersListBox.Name = "filtersListBox";
          this.filtersListBox.Size = new System.Drawing.Size(173, 290);
          this.filtersListBox.TabIndex = 0;
          this.filtersListBox.SelectedIndexChanged += new System.EventHandler(this.filtersListBox_SelectedIndexChanged);
          // 
          // deleteButton
          // 
          this.deleteButton.Location = new System.Drawing.Point(89, 315);
          this.deleteButton.Name = "deleteButton";
          this.deleteButton.Size = new System.Drawing.Size(75, 23);
          this.deleteButton.TabIndex = 2;
          this.deleteButton.Text = "&Delete";
          this.deleteButton.UseVisualStyleBackColor = true;
          this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
          // 
          // newButton
          // 
          this.newButton.Location = new System.Drawing.Point(8, 315);
          this.newButton.Name = "newButton";
          this.newButton.Size = new System.Drawing.Size(75, 23);
          this.newButton.TabIndex = 1;
          this.newButton.Text = "&New";
          this.newButton.UseVisualStyleBackColor = true;
          this.newButton.Click += new System.EventHandler(this.newButton_Click);
          // 
          // categoriesGroupBox
          // 
          this.categoriesGroupBox.Controls.Add(this.label2);
          this.categoriesGroupBox.Controls.Add(this.categoryCheckedListBox);
          this.categoriesGroupBox.Controls.Add(this.checkNoneButton);
          this.categoriesGroupBox.Controls.Add(this.checkAllButton);
          this.categoriesGroupBox.Controls.Add(this.hideUnCheckCheckBox);
          this.categoriesGroupBox.Location = new System.Drawing.Point(197, 12);
          this.categoriesGroupBox.Name = "categoriesGroupBox";
          this.categoriesGroupBox.Size = new System.Drawing.Size(199, 347);
          this.categoriesGroupBox.TabIndex = 8;
          this.categoriesGroupBox.TabStop = false;
          this.categoriesGroupBox.Text = "Categories";
          // 
          // label2
          // 
          this.label2.Location = new System.Drawing.Point(4, 19);
          this.label2.Name = "label2";
          this.label2.Size = new System.Drawing.Size(184, 63);
          this.label2.TabIndex = 3;
          this.label2.Text = "Select one or more categories to be included in the filter. Parameters common to " +
              "these categories will be available for defining filter rules.";
          // 
          // categoryCheckedListBox
          // 
          this.categoryCheckedListBox.CheckOnClick = true;
          this.categoryCheckedListBox.FormattingEnabled = true;
          this.categoryCheckedListBox.Location = new System.Drawing.Point(7, 87);
          this.categoryCheckedListBox.Name = "categoryCheckedListBox";
          this.categoryCheckedListBox.Size = new System.Drawing.Size(184, 199);
          this.categoryCheckedListBox.Sorted = true;
          this.categoryCheckedListBox.TabIndex = 3;
          this.categoryCheckedListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.categoryCheckedListBox_ItemCheck);
          // 
          // checkNoneButton
          // 
          this.checkNoneButton.Location = new System.Drawing.Point(92, 292);
          this.checkNoneButton.Name = "checkNoneButton";
          this.checkNoneButton.Size = new System.Drawing.Size(75, 23);
          this.checkNoneButton.TabIndex = 5;
          this.checkNoneButton.Text = "Check N&one";
          this.checkNoneButton.UseVisualStyleBackColor = true;
          this.checkNoneButton.Click += new System.EventHandler(this.checkNoneButton_Click);
          // 
          // checkAllButton
          // 
          this.checkAllButton.Location = new System.Drawing.Point(7, 292);
          this.checkAllButton.Name = "checkAllButton";
          this.checkAllButton.Size = new System.Drawing.Size(75, 23);
          this.checkAllButton.TabIndex = 4;
          this.checkAllButton.Text = "Check &All";
          this.checkAllButton.UseVisualStyleBackColor = true;
          this.checkAllButton.Click += new System.EventHandler(this.checkAllButton_Click);
          // 
          // hideUnCheckCheckBox
          // 
          this.hideUnCheckCheckBox.AutoSize = true;
          this.hideUnCheckCheckBox.Location = new System.Drawing.Point(10, 321);
          this.hideUnCheckCheckBox.Name = "hideUnCheckCheckBox";
          this.hideUnCheckCheckBox.Size = new System.Drawing.Size(160, 17);
          this.hideUnCheckCheckBox.TabIndex = 10;
          this.hideUnCheckCheckBox.Text = "Hide un-checked categories";
          this.hideUnCheckCheckBox.UseVisualStyleBackColor = true;
          this.hideUnCheckCheckBox.CheckedChanged += new System.EventHandler(this.hideUnCheckCheckBox_CheckedChanged);
          this.hideUnCheckCheckBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ViewFiltersForm_MouseMove);
          // 
          // groupBox1
          // 
          this.groupBox1.Controls.Add(this.rulesListBox);
          this.groupBox1.Controls.Add(this.epsilonLabel);
          this.groupBox1.Controls.Add(this.label3);
          this.groupBox1.Controls.Add(this.ruleValueComboBox);
          this.groupBox1.Controls.Add(this.criteriaComboBox);
          this.groupBox1.Controls.Add(this.paramerComboBox);
          this.groupBox1.Controls.Add(this.label1);
          this.groupBox1.Controls.Add(this.deleRuleButton);
          this.groupBox1.Controls.Add(this.updateButton);
          this.groupBox1.Controls.Add(this.newRuleButton);
          this.groupBox1.Controls.Add(this.caseSensitiveCheckBox);
          this.groupBox1.Controls.Add(this.epsilonTextBox);
          this.groupBox1.Location = new System.Drawing.Point(402, 12);
          this.groupBox1.Name = "groupBox1";
          this.groupBox1.Size = new System.Drawing.Size(186, 347);
          this.groupBox1.TabIndex = 8;
          this.groupBox1.TabStop = false;
          this.groupBox1.Text = "Filter Rules";
          // 
          // rulesListBox
          // 
          this.rulesListBox.FormattingEnabled = true;
          this.rulesListBox.Location = new System.Drawing.Point(9, 52);
          this.rulesListBox.Name = "rulesListBox";
          this.rulesListBox.Size = new System.Drawing.Size(170, 108);
          this.rulesListBox.TabIndex = 6;
          this.rulesListBox.SelectedIndexChanged += new System.EventHandler(this.rulesListBox_SelectedIndexChanged);
          this.rulesListBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ViewFiltersForm_MouseMove);
          // 
          // epsilonLabel
          // 
          this.epsilonLabel.AutoSize = true;
          this.epsilonLabel.Location = new System.Drawing.Point(9, 265);
          this.epsilonLabel.Name = "epsilonLabel";
          this.epsilonLabel.Size = new System.Drawing.Size(44, 13);
          this.epsilonLabel.TabIndex = 6;
          this.epsilonLabel.Text = "Epsilon:";
          this.epsilonLabel.Visible = false;
          // 
          // label3
          // 
          this.label3.Location = new System.Drawing.Point(6, 19);
          this.label3.Name = "label3";
          this.label3.Size = new System.Drawing.Size(174, 38);
          this.label3.TabIndex = 5;
          this.label3.Text = "Filter rules of this filter, they\'re combined by Logical-And";
          // 
          // ruleValueComboBox
          // 
          this.ruleValueComboBox.FormattingEnabled = true;
          this.ruleValueComboBox.Location = new System.Drawing.Point(9, 235);
          this.ruleValueComboBox.Name = "ruleValueComboBox";
          this.ruleValueComboBox.Size = new System.Drawing.Size(170, 21);
          this.ruleValueComboBox.Sorted = true;
          this.ruleValueComboBox.TabIndex = 9;
          this.ruleValueComboBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ViewFiltersForm_MouseMove);
          // 
          // criteriaComboBox
          // 
          this.criteriaComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
          this.criteriaComboBox.FormattingEnabled = true;
          this.criteriaComboBox.Location = new System.Drawing.Point(9, 208);
          this.criteriaComboBox.MaxDropDownItems = 12;
          this.criteriaComboBox.Name = "criteriaComboBox";
          this.criteriaComboBox.Size = new System.Drawing.Size(170, 21);
          this.criteriaComboBox.Sorted = true;
          this.criteriaComboBox.TabIndex = 8;
          this.criteriaComboBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ViewFiltersForm_MouseMove);
          // 
          // paramerComboBox
          // 
          this.paramerComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
          this.paramerComboBox.DropDownWidth = 170;
          this.paramerComboBox.FormattingEnabled = true;
          this.paramerComboBox.Location = new System.Drawing.Point(9, 181);
          this.paramerComboBox.Name = "paramerComboBox";
          this.paramerComboBox.Size = new System.Drawing.Size(170, 21);
          this.paramerComboBox.Sorted = true;
          this.paramerComboBox.TabIndex = 7;
          this.paramerComboBox.SelectedIndexChanged += new System.EventHandler(this.paramerComboBox_SelectedIndexChanged);
          this.paramerComboBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ViewFiltersForm_MouseMove);
          this.paramerComboBox.DropDown += new System.EventHandler(this.paramerComboBox_DropDown);
          // 
          // label1
          // 
          this.label1.AutoSize = true;
          this.label1.Location = new System.Drawing.Point(6, 165);
          this.label1.Name = "label1";
          this.label1.Size = new System.Drawing.Size(46, 13);
          this.label1.TabIndex = 2;
          this.label1.Text = "Filter by:";
          // 
          // deleRuleButton
          // 
          this.deleRuleButton.Location = new System.Drawing.Point(9, 315);
          this.deleRuleButton.Name = "deleRuleButton";
          this.deleRuleButton.Size = new System.Drawing.Size(75, 23);
          this.deleRuleButton.TabIndex = 14;
          this.deleRuleButton.Text = "D&elete";
          this.deleRuleButton.UseVisualStyleBackColor = true;
          this.deleRuleButton.Click += new System.EventHandler(this.deleRuleButton_Click);
          // 
          // updateButton
          // 
          this.updateButton.Location = new System.Drawing.Point(90, 289);
          this.updateButton.Name = "updateButton";
          this.updateButton.Size = new System.Drawing.Size(75, 23);
          this.updateButton.TabIndex = 13;
          this.updateButton.Text = "&Update";
          this.updateButton.UseVisualStyleBackColor = true;
          this.updateButton.Click += new System.EventHandler(this.updateButton_Click);
          // 
          // newRuleButton
          // 
          this.newRuleButton.Location = new System.Drawing.Point(9, 289);
          this.newRuleButton.Name = "newRuleButton";
          this.newRuleButton.Size = new System.Drawing.Size(75, 23);
          this.newRuleButton.TabIndex = 12;
          this.newRuleButton.Text = "&New";
          this.newRuleButton.UseVisualStyleBackColor = true;
          this.newRuleButton.Click += new System.EventHandler(this.newRuleButton_Click);
          // 
          // caseSensitiveCheckBox
          // 
          this.caseSensitiveCheckBox.AutoSize = true;
          this.caseSensitiveCheckBox.Location = new System.Drawing.Point(10, 264);
          this.caseSensitiveCheckBox.Name = "caseSensitiveCheckBox";
          this.caseSensitiveCheckBox.Size = new System.Drawing.Size(96, 17);
          this.caseSensitiveCheckBox.TabIndex = 10;
          this.caseSensitiveCheckBox.Text = "Case Sensitive";
          this.caseSensitiveCheckBox.UseVisualStyleBackColor = true;
          this.caseSensitiveCheckBox.Visible = false;
          this.caseSensitiveCheckBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ViewFiltersForm_MouseMove);
          // 
          // epsilonTextBox
          // 
          this.epsilonTextBox.Location = new System.Drawing.Point(67, 262);
          this.epsilonTextBox.Name = "epsilonTextBox";
          this.epsilonTextBox.Size = new System.Drawing.Size(112, 20);
          this.epsilonTextBox.TabIndex = 11;
          this.epsilonTextBox.Visible = false;
          this.epsilonTextBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ViewFiltersForm_MouseMove);
          // 
          // cancelButton
          // 
          this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
          this.cancelButton.Location = new System.Drawing.Point(512, 365);
          this.cancelButton.Name = "cancelButton";
          this.cancelButton.Size = new System.Drawing.Size(75, 23);
          this.cancelButton.TabIndex = 16;
          this.cancelButton.Text = "&Cancel";
          this.cancelButton.UseVisualStyleBackColor = true;
          // 
          // okButton
          // 
          this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
          this.okButton.Location = new System.Drawing.Point(431, 365);
          this.okButton.Name = "okButton";
          this.okButton.Size = new System.Drawing.Size(75, 23);
          this.okButton.TabIndex = 15;
          this.okButton.Text = "&OK";
          this.okButton.UseVisualStyleBackColor = true;
          this.okButton.Click += new System.EventHandler(this.okButton_Click);
          // 
          // ViewFiltersForm
          // 
          this.AcceptButton = this.okButton;
          this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
          this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
          this.CancelButton = this.cancelButton;
          this.ClientSize = new System.Drawing.Size(599, 394);
          this.Controls.Add(this.groupBox1);
          this.Controls.Add(this.categoriesGroupBox);
          this.Controls.Add(this.filtersGroupBox);
          this.Controls.Add(this.okButton);
          this.Controls.Add(this.cancelButton);
          this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
          this.MaximizeBox = false;
          this.MinimizeBox = false;
          this.Name = "ViewFiltersForm";
          this.ShowInTaskbar = false;
          this.Text = "Filters";
          this.Load += new System.EventHandler(this.ViewFiltersForm_Load);
          this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ViewFiltersForm_MouseMove);
          this.filtersGroupBox.ResumeLayout(false);
          this.categoriesGroupBox.ResumeLayout(false);
          this.categoriesGroupBox.PerformLayout();
          this.groupBox1.ResumeLayout(false);
          this.groupBox1.PerformLayout();
          this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.GroupBox filtersGroupBox;
      private System.Windows.Forms.GroupBox categoriesGroupBox;
      private System.Windows.Forms.Button deleteButton;
      private System.Windows.Forms.Button newButton;
      private System.Windows.Forms.CheckedListBox categoryCheckedListBox;
      private System.Windows.Forms.Button checkNoneButton;
      private System.Windows.Forms.Button checkAllButton;
      private System.Windows.Forms.GroupBox groupBox1;
      private System.Windows.Forms.Button deleRuleButton;
      private System.Windows.Forms.Button newRuleButton;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.ComboBox ruleValueComboBox;
      private System.Windows.Forms.ComboBox criteriaComboBox;
      private System.Windows.Forms.ComboBox paramerComboBox;
      private System.Windows.Forms.CheckBox caseSensitiveCheckBox;
      private System.Windows.Forms.Button cancelButton;
      private System.Windows.Forms.Button okButton;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.TextBox epsilonTextBox;
      private System.Windows.Forms.Label epsilonLabel;
      private System.Windows.Forms.ListBox rulesListBox;
      private System.Windows.Forms.ListBox filtersListBox;
      private System.Windows.Forms.Button updateButton;
      private System.Windows.Forms.CheckBox hideUnCheckCheckBox;
   }
}