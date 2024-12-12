//
// (C) Copyright 2003-2008 by Autodesk, Inc.
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

namespace Revit.SDK.Samples.ProjectUnit.CS
{
    partial class ProjectUnitForm
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
           this.discipline = new System.Windows.Forms.Label();
           this.disciplineCombox = new System.Windows.Forms.ComboBox();
           this.groupBox = new System.Windows.Forms.GroupBox();
           this.DigitGroupingSymbolTypeComboBox = new System.Windows.Forms.ComboBox();
           this.DigitGroupingAmountComboBox = new System.Windows.Forms.ComboBox();
           this.DigitGroupingSymbolType = new System.Windows.Forms.Label();
           this.DigitGroupingAmount = new System.Windows.Forms.Label();
           this.decimalComboBox = new System.Windows.Forms.ComboBox();
           this.Decimalsymbol = new System.Windows.Forms.Label();
           this.okButton = new System.Windows.Forms.Button();
           this.cancelButton = new System.Windows.Forms.Button();
           this.groupBox2 = new System.Windows.Forms.GroupBox();
           this.listView = new System.Windows.Forms.ListView();
           this.no = new System.Windows.Forms.ColumnHeader();
           this.Unit = new System.Windows.Forms.ColumnHeader();
           this.FormatUnit = new System.Windows.Forms.ColumnHeader();
           this.UnitSuffix = new System.Windows.Forms.ColumnHeader();
           this.Rounding = new System.Windows.Forms.ColumnHeader();
           this.groupBox.SuspendLayout();
           this.groupBox2.SuspendLayout();
           this.SuspendLayout();
           // 
           // discipline
           // 
           this.discipline.AutoSize = true;
           this.discipline.Location = new System.Drawing.Point(12, 24);
           this.discipline.Name = "discipline";
           this.discipline.Size = new System.Drawing.Size(58, 13);
           this.discipline.TabIndex = 0;
           this.discipline.Text = "Discipline :";
           // 
           // disciplineCombox
           // 
           this.disciplineCombox.FormattingEnabled = true;
           this.disciplineCombox.Location = new System.Drawing.Point(76, 21);
           this.disciplineCombox.Name = "disciplineCombox";
           this.disciplineCombox.Size = new System.Drawing.Size(379, 21);
           this.disciplineCombox.TabIndex = 1;
           this.disciplineCombox.SelectedIndexChanged += new System.EventHandler(this.disciplineCombox_SelectedIndexChanged);
           // 
           // groupBox
           // 
           this.groupBox.Controls.Add(this.DigitGroupingSymbolTypeComboBox);
           this.groupBox.Controls.Add(this.DigitGroupingAmountComboBox);
           this.groupBox.Controls.Add(this.DigitGroupingSymbolType);
           this.groupBox.Controls.Add(this.DigitGroupingAmount);
           this.groupBox.Controls.Add(this.decimalComboBox);
           this.groupBox.Controls.Add(this.Decimalsymbol);
           this.groupBox.Location = new System.Drawing.Point(12, 305);
           this.groupBox.Name = "groupBox";
           this.groupBox.Size = new System.Drawing.Size(471, 74);
           this.groupBox.TabIndex = 3;
           this.groupBox.TabStop = false;
           this.groupBox.Text = "Options";
           // 
           // DigitGroupingSymbolTypeComboBox
           // 
           this.DigitGroupingSymbolTypeComboBox.FormattingEnabled = true;
           this.DigitGroupingSymbolTypeComboBox.Location = new System.Drawing.Point(322, 32);
           this.DigitGroupingSymbolTypeComboBox.Name = "DigitGroupingSymbolTypeComboBox";
           this.DigitGroupingSymbolTypeComboBox.Size = new System.Drawing.Size(121, 21);
           this.DigitGroupingSymbolTypeComboBox.TabIndex = 7;
           // 
           // DigitGroupingAmountComboBox
           // 
           this.DigitGroupingAmountComboBox.FormattingEnabled = true;
           this.DigitGroupingAmountComboBox.Location = new System.Drawing.Point(162, 32);
           this.DigitGroupingAmountComboBox.Name = "DigitGroupingAmountComboBox";
           this.DigitGroupingAmountComboBox.Size = new System.Drawing.Size(121, 21);
           this.DigitGroupingAmountComboBox.TabIndex = 6;
           // 
           // DigitGroupingSymbolType
           // 
           this.DigitGroupingSymbolType.AutoSize = true;
           this.DigitGroupingSymbolType.Location = new System.Drawing.Point(319, 16);
           this.DigitGroupingSymbolType.Name = "DigitGroupingSymbolType";
           this.DigitGroupingSymbolType.Size = new System.Drawing.Size(132, 13);
           this.DigitGroupingSymbolType.TabIndex = 5;
           this.DigitGroupingSymbolType.Text = "DigitGroupingSymbolType:";
           // 
           // DigitGroupingAmount
           // 
           this.DigitGroupingAmount.AutoSize = true;
           this.DigitGroupingAmount.Location = new System.Drawing.Point(159, 16);
           this.DigitGroupingAmount.Name = "DigitGroupingAmount";
           this.DigitGroupingAmount.Size = new System.Drawing.Size(110, 13);
           this.DigitGroupingAmount.TabIndex = 4;
           this.DigitGroupingAmount.Text = "DigitGroupingAmount:";
           // 
           // decimalComboBox
           // 
           this.decimalComboBox.FormattingEnabled = true;
           this.decimalComboBox.Location = new System.Drawing.Point(9, 32);
           this.decimalComboBox.Name = "decimalComboBox";
           this.decimalComboBox.Size = new System.Drawing.Size(121, 21);
           this.decimalComboBox.TabIndex = 3;
           // 
           // Decimalsymbol
           // 
           this.Decimalsymbol.AutoSize = true;
           this.Decimalsymbol.Location = new System.Drawing.Point(6, 16);
           this.Decimalsymbol.Name = "Decimalsymbol";
           this.Decimalsymbol.Size = new System.Drawing.Size(82, 13);
           this.Decimalsymbol.TabIndex = 1;
           this.Decimalsymbol.Text = "DecimalSymbol:";
           // 
           // okButton
           // 
           this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
           this.okButton.Location = new System.Drawing.Point(300, 410);
           this.okButton.Name = "okButton";
           this.okButton.Size = new System.Drawing.Size(75, 23);
           this.okButton.TabIndex = 4;
           this.okButton.Text = "&OK";
           this.okButton.UseVisualStyleBackColor = true;
           this.okButton.Click += new System.EventHandler(this.okButton_Click);
           // 
           // cancelButton
           // 
           this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
           this.cancelButton.Location = new System.Drawing.Point(408, 410);
           this.cancelButton.Name = "cancelButton";
           this.cancelButton.Size = new System.Drawing.Size(75, 23);
           this.cancelButton.TabIndex = 5;
           this.cancelButton.Text = "&Cancel";
           this.cancelButton.UseVisualStyleBackColor = true;
           this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
           // 
           // groupBox2
           // 
           this.groupBox2.Controls.Add(this.listView);
           this.groupBox2.Location = new System.Drawing.Point(12, 48);
           this.groupBox2.Name = "groupBox2";
           this.groupBox2.Size = new System.Drawing.Size(471, 251);
           this.groupBox2.TabIndex = 8;
           this.groupBox2.TabStop = false;
           this.groupBox2.Text = "GroupBox";
           // 
           // listView
           // 
           this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.no,
            this.Unit,
            this.FormatUnit,
            this.UnitSuffix,
            this.Rounding});
           this.listView.Dock = System.Windows.Forms.DockStyle.Fill;
           this.listView.GridLines = true;
           this.listView.ImeMode = System.Windows.Forms.ImeMode.NoControl;
           this.listView.Location = new System.Drawing.Point(3, 16);
           this.listView.Name = "listView";
           this.listView.Size = new System.Drawing.Size(465, 232);
           this.listView.TabIndex = 8;
           this.listView.UseCompatibleStateImageBehavior = false;
           this.listView.View = System.Windows.Forms.View.Details;
           // 
           // no
           // 
           this.no.Text = "";
           this.no.Width = 0;
           // 
           // Unit
           // 
           this.Unit.Text = "Units";
           this.Unit.Width = 79;
           // 
           // FormatUnit
           // 
           this.FormatUnit.Text = "Format_Unit";
           this.FormatUnit.Width = 98;
           // 
           // UnitSuffix
           // 
           this.UnitSuffix.Text = "Unit_Suffix";
           this.UnitSuffix.Width = 85;
           // 
           // Rounding
           // 
           this.Rounding.Text = "Rounding";
           this.Rounding.Width = 165;
           // 
           // ProjectUnitForm
           // 
           this.AcceptButton = this.okButton;
           this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
           this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
           this.CancelButton = this.cancelButton;
           this.ClientSize = new System.Drawing.Size(497, 455);
           this.Controls.Add(this.groupBox2);
           this.Controls.Add(this.cancelButton);
           this.Controls.Add(this.okButton);
           this.Controls.Add(this.groupBox);
           this.Controls.Add(this.disciplineCombox);
           this.Controls.Add(this.discipline);
           this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
           this.Margin = new System.Windows.Forms.Padding(2);
           this.MaximizeBox = false;
           this.MinimizeBox = false;
           this.Name = "ProjectUnitForm";
           this.ShowInTaskbar = false;
           this.Text = "Project Unit";
           this.groupBox.ResumeLayout(false);
           this.groupBox.PerformLayout();
           this.groupBox2.ResumeLayout(false);
           this.ResumeLayout(false);
           this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label discipline;
        private System.Windows.Forms.ComboBox disciplineCombox;
        private System.Windows.Forms.GroupBox groupBox;
        private System.Windows.Forms.Label Decimalsymbol;
        private System.Windows.Forms.ComboBox decimalComboBox;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.ComboBox DigitGroupingSymbolTypeComboBox;
        private System.Windows.Forms.ComboBox DigitGroupingAmountComboBox;
        private System.Windows.Forms.Label DigitGroupingSymbolType;
        private System.Windows.Forms.Label DigitGroupingAmount;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListView listView;
        private System.Windows.Forms.ColumnHeader no;
        private System.Windows.Forms.ColumnHeader Unit;
        private System.Windows.Forms.ColumnHeader FormatUnit;
        private System.Windows.Forms.ColumnHeader UnitSuffix;
        private System.Windows.Forms.ColumnHeader Rounding;
       
    }
}