//
// (C) Copyright 2003-2007 by Autodesk, Inc.
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted,
// provided that the above copyright notify appears in all copies and
// that both that copyright notify and the limited warranty and
// restricted rights notify below appear in all supporting
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

namespace Revit.SDK.Samples.Transaction.CS
{
    partial class CreateWallForm
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
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.spXTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.spYTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.spZTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.epXTextBox = new System.Windows.Forms.TextBox();
            this.epYTextBox = new System.Windows.Forms.TextBox();
            this.epZTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.levelsComboBox = new System.Windows.Forms.ComboBox();
            this.wallTypesComboBox = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(96, 253);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 8;
            this.okButton.Text = "&OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(177, 253);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 9;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // spXTextBox
            // 
            this.spXTextBox.Location = new System.Drawing.Point(26, 40);
            this.spXTextBox.Name = "spXTextBox";
            this.spXTextBox.Size = new System.Drawing.Size(61, 20);
            this.spXTextBox.TabIndex = 0;
            this.spXTextBox.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "X:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Start point:";
            // 
            // spYTextBox
            // 
            this.spYTextBox.Location = new System.Drawing.Point(109, 40);
            this.spYTextBox.Name = "spYTextBox";
            this.spYTextBox.Size = new System.Drawing.Size(61, 20);
            this.spYTextBox.TabIndex = 1;
            this.spYTextBox.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(90, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Y:";
            // 
            // spZTextBox
            // 
            this.spZTextBox.Location = new System.Drawing.Point(191, 40);
            this.spZTextBox.Name = "spZTextBox";
            this.spZTextBox.Size = new System.Drawing.Size(61, 20);
            this.spZTextBox.TabIndex = 2;
            this.spZTextBox.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(172, 43);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Z:";
            // 
            // epXTextBox
            // 
            this.epXTextBox.Location = new System.Drawing.Point(26, 97);
            this.epXTextBox.Name = "epXTextBox";
            this.epXTextBox.Size = new System.Drawing.Size(61, 20);
            this.epXTextBox.TabIndex = 3;
            this.epXTextBox.Text = "0";
            // 
            // epYTextBox
            // 
            this.epYTextBox.Location = new System.Drawing.Point(109, 97);
            this.epYTextBox.Name = "epYTextBox";
            this.epYTextBox.Size = new System.Drawing.Size(61, 20);
            this.epYTextBox.TabIndex = 4;
            this.epYTextBox.Text = "0";
            // 
            // epZTextBox
            // 
            this.epZTextBox.Location = new System.Drawing.Point(191, 97);
            this.epZTextBox.Name = "epZTextBox";
            this.epZTextBox.Size = new System.Drawing.Size(61, 20);
            this.epZTextBox.TabIndex = 5;
            this.epZTextBox.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 100);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "X:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(90, 100);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(17, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Y:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(172, 100);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(17, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "Z:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(7, 73);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(55, 13);
            this.label8.TabIndex = 14;
            this.label8.Text = "End point:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(7, 187);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(36, 13);
            this.label9.TabIndex = 19;
            this.label9.Text = "Level:";
            // 
            // levelsComboBox
            // 
            this.levelsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.levelsComboBox.FormattingEnabled = true;
            this.levelsComboBox.Location = new System.Drawing.Point(26, 211);
            this.levelsComboBox.Name = "levelsComboBox";
            this.levelsComboBox.Size = new System.Drawing.Size(103, 21);
            this.levelsComboBox.TabIndex = 7;
            // 
            // wallTypesComboBox
            // 
            this.wallTypesComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.wallTypesComboBox.FormattingEnabled = true;
            this.wallTypesComboBox.Location = new System.Drawing.Point(26, 155);
            this.wallTypesComboBox.Name = "wallTypesComboBox";
            this.wallTypesComboBox.Size = new System.Drawing.Size(226, 21);
            this.wallTypesComboBox.TabIndex = 6;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(7, 131);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(54, 13);
            this.label10.TabIndex = 18;
            this.label10.Text = "Wall type:";
            // 
            // CreateWallForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(264, 288);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.wallTypesComboBox);
            this.Controls.Add(this.levelsComboBox);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.epZTextBox);
            this.Controls.Add(this.spZTextBox);
            this.Controls.Add(this.epYTextBox);
            this.Controls.Add(this.spYTextBox);
            this.Controls.Add(this.epXTextBox);
            this.Controls.Add(this.spXTextBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CreateWallForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Create Wall";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.TextBox spXTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox spYTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox spZTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox epXTextBox;
        private System.Windows.Forms.TextBox epYTextBox;
        private System.Windows.Forms.TextBox epZTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox levelsComboBox;
        private System.Windows.Forms.ComboBox wallTypesComboBox;
        private System.Windows.Forms.Label label10;
    }
}