//
// (C) Copyright 2003-2016 by Autodesk, Inc.
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


namespace Revit.SDK.Samples.Reinforcement.CS
{
    partial class ColumnFramReinMakerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; 
        /// otherwise, false.</param>
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
            this.rebarTypesGroupBox = new System.Windows.Forms.GroupBox();
            this.centerTransverseRebarTypeComboBox = new System.Windows.Forms.ComboBox();
            this.endTransverseRebarTypeComboBox = new System.Windows.Forms.ComboBox();
            this.verticalRebarTypeComboBox = new System.Windows.Forms.ComboBox();
            this.centerTransverseRebarLabel = new System.Windows.Forms.Label();
            this.endTranseverseRebarLabel = new System.Windows.Forms.Label();
            this.verticalRebarTypeLabel = new System.Windows.Forms.Label();
            this.rebarSpacingGroupBox = new System.Windows.Forms.GroupBox();
            this.centerRebarUnitLabel = new System.Windows.Forms.Label();
            this.endRebarUnitLabel = new System.Windows.Forms.Label();
            this.centerSpacingTextBox = new System.Windows.Forms.TextBox();
            this.endSpacingTextBox = new System.Windows.Forms.TextBox();
            this.centerSpacingLabel = new System.Windows.Forms.Label();
            this.endSpacingLabel = new System.Windows.Forms.Label();
            this.rebarQuantityLabel = new System.Windows.Forms.Label();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.hookTypeGroupBox = new System.Windows.Forms.GroupBox();
            this.transverseRebarHookComboBox = new System.Windows.Forms.ComboBox();
            this.transverseRebarHookLabel = new System.Windows.Forms.Label();
            this.rebarQuantityNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.rebarTypesGroupBox.SuspendLayout();
            this.rebarSpacingGroupBox.SuspendLayout();
            this.hookTypeGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rebarQuantityNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // rebarTypesGroupBox
            // 
            this.rebarTypesGroupBox.Controls.Add(this.centerTransverseRebarTypeComboBox);
            this.rebarTypesGroupBox.Controls.Add(this.endTransverseRebarTypeComboBox);
            this.rebarTypesGroupBox.Controls.Add(this.verticalRebarTypeComboBox);
            this.rebarTypesGroupBox.Controls.Add(this.centerTransverseRebarLabel);
            this.rebarTypesGroupBox.Controls.Add(this.endTranseverseRebarLabel);
            this.rebarTypesGroupBox.Controls.Add(this.verticalRebarTypeLabel);
            this.rebarTypesGroupBox.Location = new System.Drawing.Point(10, 12);
            this.rebarTypesGroupBox.Name = "rebarTypesGroupBox";
            this.rebarTypesGroupBox.Size = new System.Drawing.Size(307, 122);
            this.rebarTypesGroupBox.TabIndex = 0;
            this.rebarTypesGroupBox.TabStop = false;
            this.rebarTypesGroupBox.Text = "Bar Type";
            // 
            // centerTransverseRebarTypeComboBox
            // 
            this.centerTransverseRebarTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.centerTransverseRebarTypeComboBox.FormattingEnabled = true;
            this.centerTransverseRebarTypeComboBox.Location = new System.Drawing.Point(130, 89);
            this.centerTransverseRebarTypeComboBox.Name = "centerTransverseRebarTypeComboBox";
            this.centerTransverseRebarTypeComboBox.Size = new System.Drawing.Size(171, 21);
            this.centerTransverseRebarTypeComboBox.TabIndex = 6;
            // 
            // endTransverseRebarTypeComboBox
            // 
            this.endTransverseRebarTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.endTransverseRebarTypeComboBox.FormattingEnabled = true;
            this.endTransverseRebarTypeComboBox.Location = new System.Drawing.Point(130, 55);
            this.endTransverseRebarTypeComboBox.Name = "endTransverseRebarTypeComboBox";
            this.endTransverseRebarTypeComboBox.Size = new System.Drawing.Size(171, 21);
            this.endTransverseRebarTypeComboBox.TabIndex = 5;
            // 
            // verticalRebarTypeComboBox
            // 
            this.verticalRebarTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.verticalRebarTypeComboBox.FormattingEnabled = true;
            this.verticalRebarTypeComboBox.Location = new System.Drawing.Point(130, 21);
            this.verticalRebarTypeComboBox.Name = "verticalRebarTypeComboBox";
            this.verticalRebarTypeComboBox.Size = new System.Drawing.Size(171, 21);
            this.verticalRebarTypeComboBox.TabIndex = 4;
            // 
            // centerTransverseRebarLabel
            // 
            this.centerTransverseRebarLabel.AutoSize = true;
            this.centerTransverseRebarLabel.Location = new System.Drawing.Point(8, 92);
            this.centerTransverseRebarLabel.Name = "centerTransverseRebarLabel";
            this.centerTransverseRebarLabel.Size = new System.Drawing.Size(116, 13);
            this.centerTransverseRebarLabel.TabIndex = 3;
            this.centerTransverseRebarLabel.Text = "Center Transverse Bar:";
            // 
            // endTranseverseRebarLabel
            // 
            this.endTranseverseRebarLabel.AutoSize = true;
            this.endTranseverseRebarLabel.Location = new System.Drawing.Point(8, 58);
            this.endTranseverseRebarLabel.Name = "endTranseverseRebarLabel";
            this.endTranseverseRebarLabel.Size = new System.Drawing.Size(107, 13);
            this.endTranseverseRebarLabel.TabIndex = 2;
            this.endTranseverseRebarLabel.Text = "End  Transverse Bar:";
            // 
            // verticalRebarTypeLabel
            // 
            this.verticalRebarTypeLabel.AutoSize = true;
            this.verticalRebarTypeLabel.Location = new System.Drawing.Point(8, 24);
            this.verticalRebarTypeLabel.Name = "verticalRebarTypeLabel";
            this.verticalRebarTypeLabel.Size = new System.Drawing.Size(64, 13);
            this.verticalRebarTypeLabel.TabIndex = 1;
            this.verticalRebarTypeLabel.Text = "Vertical Bar:";
            // 
            // rebarSpacingGroupBox
            // 
            this.rebarSpacingGroupBox.Controls.Add(this.centerRebarUnitLabel);
            this.rebarSpacingGroupBox.Controls.Add(this.endRebarUnitLabel);
            this.rebarSpacingGroupBox.Controls.Add(this.centerSpacingTextBox);
            this.rebarSpacingGroupBox.Controls.Add(this.endSpacingTextBox);
            this.rebarSpacingGroupBox.Controls.Add(this.centerSpacingLabel);
            this.rebarSpacingGroupBox.Controls.Add(this.endSpacingLabel);
            this.rebarSpacingGroupBox.Location = new System.Drawing.Point(10, 222);
            this.rebarSpacingGroupBox.Name = "rebarSpacingGroupBox";
            this.rebarSpacingGroupBox.Size = new System.Drawing.Size(307, 81);
            this.rebarSpacingGroupBox.TabIndex = 12;
            this.rebarSpacingGroupBox.TabStop = false;
            this.rebarSpacingGroupBox.Text = "Bar Spacing";
            // 
            // centerRebarUnitLabel
            // 
            this.centerRebarUnitLabel.AutoSize = true;
            this.centerRebarUnitLabel.Location = new System.Drawing.Point(273, 52);
            this.centerRebarUnitLabel.Name = "centerRebarUnitLabel";
            this.centerRebarUnitLabel.Size = new System.Drawing.Size(28, 13);
            this.centerRebarUnitLabel.TabIndex = 22;
            this.centerRebarUnitLabel.Text = "Feet";
            // 
            // endRebarUnitLabel
            // 
            this.endRebarUnitLabel.AutoSize = true;
            this.endRebarUnitLabel.Location = new System.Drawing.Point(273, 22);
            this.endRebarUnitLabel.Name = "endRebarUnitLabel";
            this.endRebarUnitLabel.Size = new System.Drawing.Size(28, 13);
            this.endRebarUnitLabel.TabIndex = 21;
            this.endRebarUnitLabel.Text = "Feet";
            // 
            // centerSpacingTextBox
            // 
            this.centerSpacingTextBox.Location = new System.Drawing.Point(130, 49);
            this.centerSpacingTextBox.Name = "centerSpacingTextBox";
            this.centerSpacingTextBox.Size = new System.Drawing.Size(140, 20);
            this.centerSpacingTextBox.TabIndex = 16;
            // 
            // endSpacingTextBox
            // 
            this.endSpacingTextBox.Location = new System.Drawing.Point(130, 19);
            this.endSpacingTextBox.Name = "endSpacingTextBox";
            this.endSpacingTextBox.Size = new System.Drawing.Size(140, 20);
            this.endSpacingTextBox.TabIndex = 15;
            // 
            // centerSpacingLabel
            // 
            this.centerSpacingLabel.AutoSize = true;
            this.centerSpacingLabel.Location = new System.Drawing.Point(8, 52);
            this.centerSpacingLabel.Name = "centerSpacingLabel";
            this.centerSpacingLabel.Size = new System.Drawing.Size(97, 13);
            this.centerSpacingLabel.TabIndex = 14;
            this.centerSpacingLabel.Text = "Transverse Center:";
            // 
            // endSpacingLabel
            // 
            this.endSpacingLabel.AutoSize = true;
            this.endSpacingLabel.Location = new System.Drawing.Point(8, 22);
            this.endSpacingLabel.Name = "endSpacingLabel";
            this.endSpacingLabel.Size = new System.Drawing.Size(85, 13);
            this.endSpacingLabel.TabIndex = 13;
            this.endSpacingLabel.Text = "Transverse End:";
            // 
            // rebarQuantityLabel
            // 
            this.rebarQuantityLabel.AutoSize = true;
            this.rebarQuantityLabel.Location = new System.Drawing.Point(18, 322);
            this.rebarQuantityLabel.Name = "rebarQuantityLabel";
            this.rebarQuantityLabel.Size = new System.Drawing.Size(118, 13);
            this.rebarQuantityLabel.TabIndex = 17;
            this.rebarQuantityLabel.Text = "Quantity of Vertical Bar:";
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(131, 359);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(90, 25);
            this.okButton.TabIndex = 19;
            this.okButton.Text = "&OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(227, 359);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(90, 25);
            this.cancelButton.TabIndex = 20;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // hookTypeGroupBox
            // 
            this.hookTypeGroupBox.Controls.Add(this.transverseRebarHookComboBox);
            this.hookTypeGroupBox.Controls.Add(this.transverseRebarHookLabel);
            this.hookTypeGroupBox.Location = new System.Drawing.Point(10, 150);
            this.hookTypeGroupBox.Name = "hookTypeGroupBox";
            this.hookTypeGroupBox.Size = new System.Drawing.Size(307, 55);
            this.hookTypeGroupBox.TabIndex = 7;
            this.hookTypeGroupBox.TabStop = false;
            this.hookTypeGroupBox.Text = "Hook Type";
            // 
            // transverseRebarHookComboBox
            // 
            this.transverseRebarHookComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.transverseRebarHookComboBox.FormattingEnabled = true;
            this.transverseRebarHookComboBox.Location = new System.Drawing.Point(130, 20);
            this.transverseRebarHookComboBox.Name = "transverseRebarHookComboBox";
            this.transverseRebarHookComboBox.Size = new System.Drawing.Size(171, 21);
            this.transverseRebarHookComboBox.TabIndex = 11;
            // 
            // transverseRebarHookLabel
            // 
            this.transverseRebarHookLabel.AutoSize = true;
            this.transverseRebarHookLabel.Location = new System.Drawing.Point(8, 23);
            this.transverseRebarHookLabel.Name = "transverseRebarHookLabel";
            this.transverseRebarHookLabel.Size = new System.Drawing.Size(82, 13);
            this.transverseRebarHookLabel.TabIndex = 9;
            this.transverseRebarHookLabel.Text = "Transverse Bar:";
            // 
            // rebarQuantityNumericUpDown
            // 
            this.rebarQuantityNumericUpDown.Location = new System.Drawing.Point(140, 320);
            this.rebarQuantityNumericUpDown.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.rebarQuantityNumericUpDown.Name = "rebarQuantityNumericUpDown";
            this.rebarQuantityNumericUpDown.Size = new System.Drawing.Size(171, 20);
            this.rebarQuantityNumericUpDown.TabIndex = 18;
            this.rebarQuantityNumericUpDown.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // ColumnFramReinMakerForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(329, 393);
            this.Controls.Add(this.rebarQuantityNumericUpDown);
            this.Controls.Add(this.hookTypeGroupBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.rebarSpacingGroupBox);
            this.Controls.Add(this.rebarTypesGroupBox);
            this.Controls.Add(this.rebarQuantityLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ColumnFramReinMakerForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Column Reinforcement";
            this.rebarTypesGroupBox.ResumeLayout(false);
            this.rebarTypesGroupBox.PerformLayout();
            this.rebarSpacingGroupBox.ResumeLayout(false);
            this.rebarSpacingGroupBox.PerformLayout();
            this.hookTypeGroupBox.ResumeLayout(false);
            this.hookTypeGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rebarQuantityNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

       private System.Windows.Forms.GroupBox rebarTypesGroupBox;
       private System.Windows.Forms.GroupBox rebarSpacingGroupBox;
       private System.Windows.Forms.Label centerTransverseRebarLabel;
       private System.Windows.Forms.Label endTranseverseRebarLabel;
       private System.Windows.Forms.Label verticalRebarTypeLabel;
       private System.Windows.Forms.ComboBox verticalRebarTypeComboBox;
       private System.Windows.Forms.ComboBox centerTransverseRebarTypeComboBox;
       private System.Windows.Forms.ComboBox endTransverseRebarTypeComboBox;
       private System.Windows.Forms.Label centerSpacingLabel;
       private System.Windows.Forms.Label endSpacingLabel;
       private System.Windows.Forms.TextBox centerSpacingTextBox;
       private System.Windows.Forms.TextBox endSpacingTextBox;
       private System.Windows.Forms.Label rebarQuantityLabel;
       private System.Windows.Forms.Button okButton;
       private System.Windows.Forms.Button cancelButton;
       private System.Windows.Forms.GroupBox hookTypeGroupBox;
        private System.Windows.Forms.ComboBox transverseRebarHookComboBox;
        private System.Windows.Forms.Label transverseRebarHookLabel;
       private System.Windows.Forms.Label centerRebarUnitLabel;
       private System.Windows.Forms.Label endRebarUnitLabel;
       private System.Windows.Forms.NumericUpDown rebarQuantityNumericUpDown;
    }
}