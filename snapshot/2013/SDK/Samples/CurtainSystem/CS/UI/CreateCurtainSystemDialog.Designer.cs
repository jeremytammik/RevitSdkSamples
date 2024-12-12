//
// (C) Copyright 2003-2012 by Autodesk, Inc.
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

namespace Revit.SDK.Samples.CurtainSystem.CS.UI
{
    partial class CreateCurtainSystemDialog
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
            this.byFaceArrayCheckBox = new System.Windows.Forms.CheckBox();
            this.facesLabel = new System.Windows.Forms.Label();
            this.selectNoneButton = new System.Windows.Forms.Button();
            this.createCSButton = new System.Windows.Forms.Button();
            this.selectAllButton = new System.Windows.Forms.Button();
            this.facesCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.reverseSelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // byFaceArrayCheckBox
            // 
            this.byFaceArrayCheckBox.AutoSize = true;
            this.byFaceArrayCheckBox.Location = new System.Drawing.Point(147, 145);
            this.byFaceArrayCheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.byFaceArrayCheckBox.Name = "byFaceArrayCheckBox";
            this.byFaceArrayCheckBox.Size = new System.Drawing.Size(159, 21);
            this.byFaceArrayCheckBox.TabIndex = 2;
            this.byFaceArrayCheckBox.Text = "Create by &face array";
            this.byFaceArrayCheckBox.UseVisualStyleBackColor = true;
            this.byFaceArrayCheckBox.CheckedChanged += new System.EventHandler(this.byFaceArrayCheckBox_CheckedChanged);
            // 
            // facesLabel
            // 
            this.facesLabel.AutoSize = true;
            this.facesLabel.Location = new System.Drawing.Point(12, 16);
            this.facesLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.facesLabel.Name = "facesLabel";
            this.facesLabel.Size = new System.Drawing.Size(127, 17);
            this.facesLabel.TabIndex = 21;
            this.facesLabel.Text = "Faces of the mass:";
            // 
            // selectNoneButton
            // 
            this.selectNoneButton.Location = new System.Drawing.Point(147, 75);
            this.selectNoneButton.Margin = new System.Windows.Forms.Padding(4);
            this.selectNoneButton.Name = "selectNoneButton";
            this.selectNoneButton.Size = new System.Drawing.Size(159, 28);
            this.selectNoneButton.TabIndex = 4;
            this.selectNoneButton.Text = "Select &None";
            this.selectNoneButton.UseVisualStyleBackColor = true;
            this.selectNoneButton.Click += new System.EventHandler(this.clearButton_Click);
            // 
            // createCSButton
            // 
            this.createCSButton.Location = new System.Drawing.Point(122, 197);
            this.createCSButton.Margin = new System.Windows.Forms.Padding(7, 7, 4, 7);
            this.createCSButton.Name = "createCSButton";
            this.createCSButton.Size = new System.Drawing.Size(88, 28);
            this.createCSButton.TabIndex = 6;
            this.createCSButton.Text = "OK";
            this.createCSButton.UseVisualStyleBackColor = true;
            this.createCSButton.Click += new System.EventHandler(this.createCSButton_Click);
            // 
            // selectAllButton
            // 
            this.selectAllButton.Location = new System.Drawing.Point(147, 42);
            this.selectAllButton.Margin = new System.Windows.Forms.Padding(4);
            this.selectAllButton.Name = "selectAllButton";
            this.selectAllButton.Size = new System.Drawing.Size(158, 28);
            this.selectAllButton.TabIndex = 3;
            this.selectAllButton.Text = "&Select All";
            this.selectAllButton.UseVisualStyleBackColor = true;
            this.selectAllButton.Click += new System.EventHandler(this.selectAllButton_Click);
            // 
            // facesCheckedListBox
            // 
            this.facesCheckedListBox.CheckOnClick = true;
            this.facesCheckedListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.facesCheckedListBox.FormattingEnabled = true;
            this.facesCheckedListBox.Items.AddRange(new object[] {
            "Face 0",
            "Face 1",
            "Face 2",
            "Face 3",
            "Face 4",
            "Face 5"});
            this.facesCheckedListBox.Location = new System.Drawing.Point(16, 42);
            this.facesCheckedListBox.Margin = new System.Windows.Forms.Padding(4);
            this.facesCheckedListBox.Name = "facesCheckedListBox";
            this.facesCheckedListBox.Size = new System.Drawing.Size(123, 124);
            this.facesCheckedListBox.TabIndex = 1;
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(218, 197);
            this.cancelButton.Margin = new System.Windows.Forms.Padding(4, 7, 7, 7);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(88, 28);
            this.cancelButton.TabIndex = 7;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // reverseSelButton
            // 
            this.reverseSelButton.Location = new System.Drawing.Point(147, 111);
            this.reverseSelButton.Margin = new System.Windows.Forms.Padding(4);
            this.reverseSelButton.Name = "reverseSelButton";
            this.reverseSelButton.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.reverseSelButton.Size = new System.Drawing.Size(158, 28);
            this.reverseSelButton.TabIndex = 22;
            this.reverseSelButton.Text = "&Reverse Selection";
            this.reverseSelButton.UseVisualStyleBackColor = true;
            this.reverseSelButton.Click += new System.EventHandler(this.reverseSelButton_Click);
            // 
            // CreateCurtainSystemDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(318, 241);
            this.Controls.Add(this.reverseSelButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.byFaceArrayCheckBox);
            this.Controls.Add(this.facesLabel);
            this.Controls.Add(this.selectNoneButton);
            this.Controls.Add(this.createCSButton);
            this.Controls.Add(this.selectAllButton);
            this.Controls.Add(this.facesCheckedListBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CreateCurtainSystemDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Create Curtain System";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CreateCurtainSystemDialog_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox byFaceArrayCheckBox;
        private System.Windows.Forms.Label facesLabel;
        private System.Windows.Forms.Button selectNoneButton;
        private System.Windows.Forms.Button createCSButton;
        private System.Windows.Forms.Button selectAllButton;
        private System.Windows.Forms.CheckedListBox facesCheckedListBox;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button reverseSelButton;
    }
}