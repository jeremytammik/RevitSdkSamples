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
            this.clearButton = new System.Windows.Forms.Button();
            this.reverseSelButton = new System.Windows.Forms.Button();
            this.createCSButton = new System.Windows.Forms.Button();
            this.selectAllButton = new System.Windows.Forms.Button();
            this.facesCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.SuspendLayout();
            // 
            // byFaceArrayCheckBox
            // 
            this.byFaceArrayCheckBox.AutoSize = true;
            this.byFaceArrayCheckBox.Location = new System.Drawing.Point(122, 13);
            this.byFaceArrayCheckBox.Name = "byFaceArrayCheckBox";
            this.byFaceArrayCheckBox.Size = new System.Drawing.Size(121, 17);
            this.byFaceArrayCheckBox.TabIndex = 23;
            this.byFaceArrayCheckBox.Text = "Create by &face array";
            this.byFaceArrayCheckBox.UseVisualStyleBackColor = true;
            this.byFaceArrayCheckBox.CheckedChanged += new System.EventHandler(this.byFaceArrayCheckBox_CheckedChanged);
            // 
            // facesLabel
            // 
            this.facesLabel.AutoSize = true;
            this.facesLabel.Location = new System.Drawing.Point(9, 13);
            this.facesLabel.Name = "facesLabel";
            this.facesLabel.Size = new System.Drawing.Size(96, 13);
            this.facesLabel.TabIndex = 21;
            this.facesLabel.Text = "Faces of the mass:";
            // 
            // clearButton
            // 
            this.clearButton.Location = new System.Drawing.Point(122, 89);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(136, 24);
            this.clearButton.TabIndex = 26;
            this.clearButton.Text = "C&lear";
            this.clearButton.UseVisualStyleBackColor = true;
            this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
            // 
            // reverseSelButton
            // 
            this.reverseSelButton.Location = new System.Drawing.Point(122, 61);
            this.reverseSelButton.Name = "reverseSelButton";
            this.reverseSelButton.Size = new System.Drawing.Size(136, 24);
            this.reverseSelButton.TabIndex = 25;
            this.reverseSelButton.Text = "&Reverse Selection";
            this.reverseSelButton.UseVisualStyleBackColor = true;
            this.reverseSelButton.Click += new System.EventHandler(this.reverseSelButton_Click);
            // 
            // createCSButton
            // 
            this.createCSButton.Location = new System.Drawing.Point(122, 116);
            this.createCSButton.Name = "createCSButton";
            this.createCSButton.Size = new System.Drawing.Size(136, 24);
            this.createCSButton.TabIndex = 27;
            this.createCSButton.Text = "&Create Curtain System";
            this.createCSButton.UseVisualStyleBackColor = true;
            this.createCSButton.Click += new System.EventHandler(this.createCSButton_Click);
            // 
            // selectAllButton
            // 
            this.selectAllButton.Location = new System.Drawing.Point(122, 34);
            this.selectAllButton.Name = "selectAllButton";
            this.selectAllButton.Size = new System.Drawing.Size(136, 24);
            this.selectAllButton.TabIndex = 24;
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
            this.facesCheckedListBox.Location = new System.Drawing.Point(12, 34);
            this.facesCheckedListBox.Name = "facesCheckedListBox";
            this.facesCheckedListBox.Size = new System.Drawing.Size(93, 106);
            this.facesCheckedListBox.TabIndex = 22;
            // 
            // CreateCurtainSystemDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(266, 148);
            this.Controls.Add(this.byFaceArrayCheckBox);
            this.Controls.Add(this.facesLabel);
            this.Controls.Add(this.clearButton);
            this.Controls.Add(this.reverseSelButton);
            this.Controls.Add(this.createCSButton);
            this.Controls.Add(this.selectAllButton);
            this.Controls.Add(this.facesCheckedListBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
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
        private System.Windows.Forms.Button clearButton;
        private System.Windows.Forms.Button reverseSelButton;
        private System.Windows.Forms.Button createCSButton;
        private System.Windows.Forms.Button selectAllButton;
        private System.Windows.Forms.CheckedListBox facesCheckedListBox;
    }
}