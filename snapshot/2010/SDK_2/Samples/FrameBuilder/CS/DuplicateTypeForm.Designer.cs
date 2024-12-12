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

namespace Revit.SDK.Samples.FrameBuilder.CS
{
    partial class DuplicateTypeForm
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
         this.familyTextBox = new System.Windows.Forms.TextBox();
         this.label1 = new System.Windows.Forms.Label();
         this.label2 = new System.Windows.Forms.Label();
         this.typeNameTextBox = new System.Windows.Forms.TextBox();
         this.typeParameterPropertyGrid = new System.Windows.Forms.PropertyGrid();
         this.typeParametersGroupBox = new System.Windows.Forms.GroupBox();
         this.OKButton = new System.Windows.Forms.Button();
         this.cancelButton = new System.Windows.Forms.Button();
         this.typeParametersGroupBox.SuspendLayout();
         this.SuspendLayout();
         // 
         // familyTextBox
         // 
         this.familyTextBox.Location = new System.Drawing.Point(86, 12);
         this.familyTextBox.Name = "familyTextBox";
         this.familyTextBox.ReadOnly = true;
         this.familyTextBox.Size = new System.Drawing.Size(280, 20);
         this.familyTextBox.TabIndex = 0;
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(12, 15);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(39, 13);
         this.label1.TabIndex = 1;
         this.label1.Text = "Family:";
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(12, 47);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(34, 13);
         this.label2.TabIndex = 2;
         this.label2.Text = "Type:";
         // 
         // typeNameTextBox
         // 
         this.typeNameTextBox.Location = new System.Drawing.Point(86, 44);
         this.typeNameTextBox.Name = "typeNameTextBox";
         this.typeNameTextBox.ReadOnly = true;
         this.typeNameTextBox.Size = new System.Drawing.Size(280, 20);
         this.typeNameTextBox.TabIndex = 3;
         // 
         // typeParameterPropertyGrid
         // 
         this.typeParameterPropertyGrid.Location = new System.Drawing.Point(6, 19);
         this.typeParameterPropertyGrid.Name = "typeParameterPropertyGrid";
         this.typeParameterPropertyGrid.Size = new System.Drawing.Size(348, 271);
         this.typeParameterPropertyGrid.TabIndex = 4;
         // 
         // typeParametersGroupBox
         // 
         this.typeParametersGroupBox.Controls.Add(this.typeParameterPropertyGrid);
         this.typeParametersGroupBox.Location = new System.Drawing.Point(12, 75);
         this.typeParametersGroupBox.Name = "typeParametersGroupBox";
         this.typeParametersGroupBox.Size = new System.Drawing.Size(360, 296);
         this.typeParametersGroupBox.TabIndex = 5;
         this.typeParametersGroupBox.TabStop = false;
         this.typeParametersGroupBox.Text = "Type Parameters:";
         // 
         // OKButton
         // 
         this.OKButton.Location = new System.Drawing.Point(207, 379);
         this.OKButton.Name = "OKButton";
         this.OKButton.Size = new System.Drawing.Size(75, 23);
         this.OKButton.TabIndex = 6;
         this.OKButton.Text = "&OK";
         this.OKButton.UseVisualStyleBackColor = true;
         this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
         // 
         // cancelButton
         // 
         this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         this.cancelButton.Location = new System.Drawing.Point(291, 379);
         this.cancelButton.Name = "cancelButton";
         this.cancelButton.Size = new System.Drawing.Size(75, 23);
         this.cancelButton.TabIndex = 7;
         this.cancelButton.Text = "&Cancel";
         this.cancelButton.UseVisualStyleBackColor = true;
         this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
         // 
         // DuplicateTypeForm
         // 
         this.AcceptButton = this.OKButton;
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.CancelButton = this.cancelButton;
         this.ClientSize = new System.Drawing.Size(383, 414);
         this.Controls.Add(this.cancelButton);
         this.Controls.Add(this.OKButton);
         this.Controls.Add(this.typeParametersGroupBox);
         this.Controls.Add(this.typeNameTextBox);
         this.Controls.Add(this.label2);
         this.Controls.Add(this.label1);
         this.Controls.Add(this.familyTextBox);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "DuplicateTypeForm";
         this.ShowInTaskbar = false;
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
         this.Text = "Type Properties";
         this.Load += new System.EventHandler(this.DuplicateTypeForm_Load);
         this.typeParametersGroupBox.ResumeLayout(false);
         this.ResumeLayout(false);
         this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox familyTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox typeNameTextBox;
        private System.Windows.Forms.PropertyGrid typeParameterPropertyGrid;
        private System.Windows.Forms.GroupBox typeParametersGroupBox;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Button cancelButton;
    }
}