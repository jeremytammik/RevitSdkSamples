//
// (C) Copyright 2003-2013 by Autodesk, Inc.
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

namespace ConcreteCalculationsExample
{
    partial class Form1
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
            this.ResultTextBox = new System.Windows.Forms.RichTextBox();
            this.ExampleText = new System.Windows.Forms.Label();
            this.ExampleSelection = new System.Windows.Forms.ComboBox();
            this.RunExample = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ResultTextBox
            // 
            this.ResultTextBox.Location = new System.Drawing.Point(-2, 37);
            this.ResultTextBox.Name = "ResultTextBox";
            this.ResultTextBox.Size = new System.Drawing.Size(808, 317);
            this.ResultTextBox.TabIndex = 0;
            this.ResultTextBox.Text = "";
            // 
            // ExampleText
            // 
            this.ExampleText.AutoSize = true;
            this.ExampleText.Location = new System.Drawing.Point(12, 9);
            this.ExampleText.Name = "ExampleText";
            this.ExampleText.Size = new System.Drawing.Size(73, 13);
            this.ExampleText.TabIndex = 6;
            this.ExampleText.Text = "List of Cases: ";
            // 
            // ExampleSelection
            // 
            this.ExampleSelection.FormattingEnabled = true;
            this.ExampleSelection.Location = new System.Drawing.Point(128, 6);
            this.ExampleSelection.Name = "ExampleSelection";
            this.ExampleSelection.Size = new System.Drawing.Size(129, 21);
            this.ExampleSelection.TabIndex = 5;
            // 
            // RunExample
            // 
            this.RunExample.Location = new System.Drawing.Point(691, 361);
            this.RunExample.Name = "RunExample";
            this.RunExample.Size = new System.Drawing.Size(103, 35);
            this.RunExample.TabIndex = 4;
            this.RunExample.Text = "Run";
            this.RunExample.UseVisualStyleBackColor = true;
            this.RunExample.Click += new System.EventHandler(this.RunExample_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(806, 408);
            this.Controls.Add(this.ExampleText);
            this.Controls.Add(this.ExampleSelection);
            this.Controls.Add(this.RunExample);
            this.Controls.Add(this.ResultTextBox);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Concrete Example - Verification Manual";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox ResultTextBox;
        private System.Windows.Forms.Label ExampleText;
        private System.Windows.Forms.ComboBox ExampleSelection;
        private System.Windows.Forms.Button RunExample;
    }
}

