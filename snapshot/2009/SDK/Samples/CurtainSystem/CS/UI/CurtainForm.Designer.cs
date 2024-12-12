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

namespace Revit.SDK.Samples.CurtainSystem.CS.UI
{
    partial class CurtainForm
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
            this.mainPanel = new System.Windows.Forms.Panel();
            this.csListBox = new System.Windows.Forms.CheckedListBox();
            this.deleteCSButton = new System.Windows.Forms.Button();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.operationStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.exitButton = new System.Windows.Forms.Button();
            this.facesCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.facesLabel = new System.Windows.Forms.Label();
            this.removeCGButton = new System.Windows.Forms.Button();
            this.cgLabel = new System.Windows.Forms.Label();
            this.addCGButton = new System.Windows.Forms.Button();
            this.csLabel = new System.Windows.Forms.Label();
            this.cgCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.createCSButton = new System.Windows.Forms.Button();
            this.mainPanel.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.csListBox);
            this.mainPanel.Controls.Add(this.deleteCSButton);
            this.mainPanel.Controls.Add(this.statusStrip);
            this.mainPanel.Controls.Add(this.exitButton);
            this.mainPanel.Controls.Add(this.facesCheckedListBox);
            this.mainPanel.Controls.Add(this.facesLabel);
            this.mainPanel.Controls.Add(this.removeCGButton);
            this.mainPanel.Controls.Add(this.cgLabel);
            this.mainPanel.Controls.Add(this.addCGButton);
            this.mainPanel.Controls.Add(this.csLabel);
            this.mainPanel.Controls.Add(this.cgCheckedListBox);
            this.mainPanel.Controls.Add(this.createCSButton);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(0, 0);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(390, 257);
            this.mainPanel.TabIndex = 0;
            // 
            // csListBox
            // 
            this.csListBox.FormattingEnabled = true;
            this.csListBox.Location = new System.Drawing.Point(12, 29);
            this.csListBox.Name = "csListBox";
            this.csListBox.Size = new System.Drawing.Size(128, 139);
            this.csListBox.TabIndex = 2;
            this.csListBox.SelectedIndexChanged += new System.EventHandler(this.csListBox_SelectedIndexChanged);
            // 
            // deleteCSButton
            // 
            this.deleteCSButton.Location = new System.Drawing.Point(12, 204);
            this.deleteCSButton.Name = "deleteCSButton";
            this.deleteCSButton.Size = new System.Drawing.Size(128, 26);
            this.deleteCSButton.TabIndex = 4;
            this.deleteCSButton.Text = "&Delete Curtain System";
            this.deleteCSButton.UseVisualStyleBackColor = true;
            this.deleteCSButton.Click += new System.EventHandler(this.deleteCSButton_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.operationStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 235);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(390, 22);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.Stretch = false;
            this.statusStrip.TabIndex = 12;
            // 
            // operationStatusLabel
            // 
            this.operationStatusLabel.Name = "operationStatusLabel";
            this.operationStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // exitButton
            // 
            this.exitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.exitButton.Location = new System.Drawing.Point(191, 204);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(155, 26);
            this.exitButton.TabIndex = 11;
            this.exitButton.Text = "E&xit";
            this.exitButton.UseVisualStyleBackColor = true;
            // 
            // facesCheckedListBox
            // 
            this.facesCheckedListBox.CheckOnClick = true;
            this.facesCheckedListBox.FormattingEnabled = true;
            this.facesCheckedListBox.Location = new System.Drawing.Point(158, 29);
            this.facesCheckedListBox.Name = "facesCheckedListBox";
            this.facesCheckedListBox.Size = new System.Drawing.Size(92, 139);
            this.facesCheckedListBox.TabIndex = 6;
            // 
            // facesLabel
            // 
            this.facesLabel.AutoSize = true;
            this.facesLabel.Location = new System.Drawing.Point(155, 9);
            this.facesLabel.Name = "facesLabel";
            this.facesLabel.Size = new System.Drawing.Size(95, 13);
            this.facesLabel.TabIndex = 5;
            this.facesLabel.Text = "Uncovered Faces:";
            // 
            // removeCGButton
            // 
            this.removeCGButton.Location = new System.Drawing.Point(270, 174);
            this.removeCGButton.Name = "removeCGButton";
            this.removeCGButton.Size = new System.Drawing.Size(109, 26);
            this.removeCGButton.TabIndex = 10;
            this.removeCGButton.Text = "<< &Remove";
            this.removeCGButton.UseVisualStyleBackColor = true;
            this.removeCGButton.Click += new System.EventHandler(this.removeCGButton_Click);
            // 
            // cgLabel
            // 
            this.cgLabel.AutoSize = true;
            this.cgLabel.Location = new System.Drawing.Point(267, 9);
            this.cgLabel.Name = "cgLabel";
            this.cgLabel.Size = new System.Drawing.Size(70, 13);
            this.cgLabel.TabIndex = 8;
            this.cgLabel.Text = "Curtain Grids:";
            // 
            // addCGButton
            // 
            this.addCGButton.Location = new System.Drawing.Point(158, 174);
            this.addCGButton.Name = "addCGButton";
            this.addCGButton.Size = new System.Drawing.Size(92, 26);
            this.addCGButton.TabIndex = 7;
            this.addCGButton.Text = "&Add >>";
            this.addCGButton.UseVisualStyleBackColor = true;
            this.addCGButton.Click += new System.EventHandler(this.addCGButton_Click);
            // 
            // csLabel
            // 
            this.csLabel.AutoSize = true;
            this.csLabel.Location = new System.Drawing.Point(9, 9);
            this.csLabel.Name = "csLabel";
            this.csLabel.Size = new System.Drawing.Size(85, 13);
            this.csLabel.TabIndex = 1;
            this.csLabel.Text = "Curtain Systems:";
            // 
            // cgCheckedListBox
            // 
            this.cgCheckedListBox.CheckOnClick = true;
            this.cgCheckedListBox.FormattingEnabled = true;
            this.cgCheckedListBox.Location = new System.Drawing.Point(270, 29);
            this.cgCheckedListBox.Name = "cgCheckedListBox";
            this.cgCheckedListBox.Size = new System.Drawing.Size(109, 139);
            this.cgCheckedListBox.TabIndex = 9;
            this.cgCheckedListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.cgCheckedListBox_ItemCheck);
            // 
            // createCSButton
            // 
            this.createCSButton.Location = new System.Drawing.Point(12, 174);
            this.createCSButton.Name = "createCSButton";
            this.createCSButton.Size = new System.Drawing.Size(128, 26);
            this.createCSButton.TabIndex = 3;
            this.createCSButton.Text = "&Create Curtain System";
            this.createCSButton.UseVisualStyleBackColor = true;
            this.createCSButton.Click += new System.EventHandler(this.createCSButton_Click);
            // 
            // CurtainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.exitButton;
            this.ClientSize = new System.Drawing.Size(390, 257);
            this.Controls.Add(this.mainPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CurtainForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Curtain System";
            this.mainPanel.ResumeLayout(false);
            this.mainPanel.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel operationStatusLabel;
        private System.Windows.Forms.Label facesLabel;
        private System.Windows.Forms.Label cgLabel;
        private System.Windows.Forms.CheckedListBox facesCheckedListBox;
        private System.Windows.Forms.CheckedListBox cgCheckedListBox;
        private System.Windows.Forms.Button createCSButton;
        private System.Windows.Forms.Label csLabel;
        private System.Windows.Forms.Button addCGButton;
        private System.Windows.Forms.Button removeCGButton;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.Button deleteCSButton;
        private System.Windows.Forms.CheckedListBox csListBox;
    }
}