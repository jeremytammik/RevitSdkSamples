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

namespace Revit.SDK.Samples.NewRebar.CS
{
    partial class AddParameter
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
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.paramValueTextBox = new System.Windows.Forms.TextBox();
            this.paramNameTextBox = new System.Windows.Forms.TextBox();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.paramFormulaRadioButton = new System.Windows.Forms.RadioButton();
            this.paramDoubleRadioButton = new System.Windows.Forms.RadioButton();
            this.groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(43, 112);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Value";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(43, 79);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Name";
            // 
            // paramValueTextBox
            // 
            this.paramValueTextBox.Location = new System.Drawing.Point(102, 105);
            this.paramValueTextBox.Name = "paramValueTextBox";
            this.paramValueTextBox.Size = new System.Drawing.Size(142, 20);
            this.paramValueTextBox.TabIndex = 6;
            // 
            // paramNameTextBox
            // 
            this.paramNameTextBox.Location = new System.Drawing.Point(102, 79);
            this.paramNameTextBox.Name = "paramNameTextBox";
            this.paramNameTextBox.Size = new System.Drawing.Size(142, 20);
            this.paramNameTextBox.TabIndex = 5;
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(64, 146);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(72, 23);
            this.okButton.TabIndex = 2;
            this.okButton.Text = "&OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(159, 146);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(72, 23);
            this.cancelButton.TabIndex = 10;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // groupBox
            // 
            this.groupBox.Controls.Add(this.paramFormulaRadioButton);
            this.groupBox.Controls.Add(this.paramDoubleRadioButton);
            this.groupBox.Location = new System.Drawing.Point(43, 12);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(201, 47);
            this.groupBox.TabIndex = 11;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "Parameter Type";
            // 
            // paramFormulaRadioButton
            // 
            this.paramFormulaRadioButton.AutoSize = true;
            this.paramFormulaRadioButton.Location = new System.Drawing.Point(116, 19);
            this.paramFormulaRadioButton.Name = "paramFormulaRadioButton";
            this.paramFormulaRadioButton.Size = new System.Drawing.Size(62, 17);
            this.paramFormulaRadioButton.TabIndex = 1;
            this.paramFormulaRadioButton.Text = "Formula";
            this.paramFormulaRadioButton.UseVisualStyleBackColor = true;
            // 
            // paramDoubleRadioButton
            // 
            this.paramDoubleRadioButton.AutoSize = true;
            this.paramDoubleRadioButton.Checked = true;
            this.paramDoubleRadioButton.Location = new System.Drawing.Point(21, 19);
            this.paramDoubleRadioButton.Name = "paramDoubleRadioButton";
            this.paramDoubleRadioButton.Size = new System.Drawing.Size(59, 17);
            this.paramDoubleRadioButton.TabIndex = 0;
            this.paramDoubleRadioButton.TabStop = true;
            this.paramDoubleRadioButton.Text = "Double";
            this.paramDoubleRadioButton.UseVisualStyleBackColor = true;
            // 
            // AddParameter
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(285, 181);
            this.Controls.Add(this.groupBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.paramValueTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.paramNameTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddParameter";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add Parameter";
            this.groupBox.ResumeLayout(false);
            this.groupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox paramValueTextBox;
        private System.Windows.Forms.TextBox paramNameTextBox;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.GroupBox groupBox;
        private System.Windows.Forms.RadioButton paramFormulaRadioButton;
        private System.Windows.Forms.RadioButton paramDoubleRadioButton;
    }
}