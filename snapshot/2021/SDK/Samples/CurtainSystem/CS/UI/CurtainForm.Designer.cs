//
// (C) Copyright 2003-2019 by Autodesk, Inc.
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
            this.createCSButton = new System.Windows.Forms.Button();
            this.cgCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.addCGButton = new System.Windows.Forms.Button();
            this.cgLabel = new System.Windows.Forms.Label();
            this.removeCGButton = new System.Windows.Forms.Button();
            this.facesLabel = new System.Windows.Forms.Label();
            this.facesCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.exitButton = new System.Windows.Forms.Button();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.operationStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.deleteCSButton = new System.Windows.Forms.Button();
            this.csListBox = new System.Windows.Forms.CheckedListBox();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.csLabel = new System.Windows.Forms.Label();
            this.statusStrip.SuspendLayout();
            this.mainPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // createCSButton
            // 
            this.createCSButton.Image = global::Revit.SDK.Samples.CurtainSystem.CS.Properties.Resources._new;
            this.createCSButton.Location = new System.Drawing.Point(16, 192);
            this.createCSButton.Margin = new System.Windows.Forms.Padding(4);
            this.createCSButton.Name = "createCSButton";
            this.createCSButton.Size = new System.Drawing.Size(32, 30);
            this.createCSButton.TabIndex = 2;
            this.createCSButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.createCSButton.UseVisualStyleBackColor = true;
            this.createCSButton.Click += new System.EventHandler(this.createCSButton_Click);
            // 
            // cgCheckedListBox
            // 
            this.cgCheckedListBox.CheckOnClick = true;
            this.cgCheckedListBox.FormattingEnabled = true;
            this.cgCheckedListBox.Location = new System.Drawing.Point(454, 27);
            this.cgCheckedListBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 4);
            this.cgCheckedListBox.Name = "cgCheckedListBox";
            this.cgCheckedListBox.Size = new System.Drawing.Size(144, 157);
            this.cgCheckedListBox.TabIndex = 7;
            this.cgCheckedListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.cgCheckedListBox_ItemCheck);
            // 
            // addCGButton
            // 
            this.addCGButton.Location = new System.Drawing.Point(348, 67);
            this.addCGButton.Margin = new System.Windows.Forms.Padding(4);
            this.addCGButton.Name = "addCGButton";
            this.addCGButton.Size = new System.Drawing.Size(98, 32);
            this.addCGButton.TabIndex = 5;
            this.addCGButton.Text = "&Add >>";
            this.addCGButton.UseVisualStyleBackColor = true;
            this.addCGButton.Click += new System.EventHandler(this.addCGButton_Click);
            // 
            // cgLabel
            // 
            this.cgLabel.AutoSize = true;
            this.cgLabel.Location = new System.Drawing.Point(451, 7);
            this.cgLabel.Margin = new System.Windows.Forms.Padding(4, 7, 4, 0);
            this.cgLabel.Name = "cgLabel";
            this.cgLabel.Size = new System.Drawing.Size(95, 17);
            this.cgLabel.TabIndex = 8;
            this.cgLabel.Text = "Curtain Grids:";
            // 
            // removeCGButton
            // 
            this.removeCGButton.Location = new System.Drawing.Point(348, 107);
            this.removeCGButton.Margin = new System.Windows.Forms.Padding(4);
            this.removeCGButton.Name = "removeCGButton";
            this.removeCGButton.Size = new System.Drawing.Size(98, 32);
            this.removeCGButton.TabIndex = 6;
            this.removeCGButton.Text = "<< &Remove";
            this.removeCGButton.UseVisualStyleBackColor = true;
            this.removeCGButton.Click += new System.EventHandler(this.removeCGButton_Click);
            // 
            // facesLabel
            // 
            this.facesLabel.AutoSize = true;
            this.facesLabel.Location = new System.Drawing.Point(193, 7);
            this.facesLabel.Margin = new System.Windows.Forms.Padding(4, 7, 4, 0);
            this.facesLabel.Name = "facesLabel";
            this.facesLabel.Size = new System.Drawing.Size(123, 17);
            this.facesLabel.TabIndex = 0;
            this.facesLabel.Text = "Uncovered Faces:";
            // 
            // facesCheckedListBox
            // 
            this.facesCheckedListBox.CheckOnClick = true;
            this.facesCheckedListBox.FormattingEnabled = true;
            this.facesCheckedListBox.Location = new System.Drawing.Point(196, 27);
            this.facesCheckedListBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 4);
            this.facesCheckedListBox.Name = "facesCheckedListBox";
            this.facesCheckedListBox.Size = new System.Drawing.Size(144, 157);
            this.facesCheckedListBox.TabIndex = 4;
            this.facesCheckedListBox.SelectedIndexChanged += new System.EventHandler(this.facesCheckedListBox_SelectedIndexChanged);
            // 
            // exitButton
            // 
            this.exitButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.exitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.exitButton.Location = new System.Drawing.Point(510, 241);
            this.exitButton.Margin = new System.Windows.Forms.Padding(7);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(88, 28);
            this.exitButton.TabIndex = 8;
            this.exitButton.Text = "&Close";
            this.exitButton.UseVisualStyleBackColor = true;
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.operationStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 276);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusStrip.Size = new System.Drawing.Size(614, 22);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.Stretch = false;
            this.statusStrip.TabIndex = 12;
            // 
            // operationStatusLabel
            // 
            this.operationStatusLabel.Name = "operationStatusLabel";
            this.operationStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // deleteCSButton
            // 
            this.deleteCSButton.Image = global::Revit.SDK.Samples.CurtainSystem.CS.Properties.Resources.delete;
            this.deleteCSButton.Location = new System.Drawing.Point(56, 192);
            this.deleteCSButton.Margin = new System.Windows.Forms.Padding(4);
            this.deleteCSButton.Name = "deleteCSButton";
            this.deleteCSButton.Size = new System.Drawing.Size(32, 30);
            this.deleteCSButton.TabIndex = 3;
            this.deleteCSButton.UseVisualStyleBackColor = true;
            this.deleteCSButton.Click += new System.EventHandler(this.deleteCSButton_Click);
            // 
            // csListBox
            // 
            this.csListBox.FormattingEnabled = true;
            this.csListBox.Location = new System.Drawing.Point(16, 27);
            this.csListBox.Margin = new System.Windows.Forms.Padding(7, 3, 7, 4);
            this.csListBox.Name = "csListBox";
            this.csListBox.Size = new System.Drawing.Size(169, 157);
            this.csListBox.TabIndex = 1;
            this.csListBox.SelectedIndexChanged += new System.EventHandler(this.csListBox_SelectedIndexChanged);
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
            this.mainPanel.Margin = new System.Windows.Forms.Padding(4);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(614, 298);
            this.mainPanel.TabIndex = 0;
            this.mainPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.mainPanel_Paint);
            // 
            // csLabel
            // 
            this.csLabel.AutoSize = true;
            this.csLabel.Location = new System.Drawing.Point(13, 7);
            this.csLabel.Margin = new System.Windows.Forms.Padding(4, 7, 4, 0);
            this.csLabel.Name = "csLabel";
            this.csLabel.Size = new System.Drawing.Size(114, 17);
            this.csLabel.TabIndex = 0;
            this.csLabel.Text = "Curtain Systems:";
            this.csLabel.Click += new System.EventHandler(this.csLabel_Click);
            // 
            // CurtainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.exitButton;
            this.ClientSize = new System.Drawing.Size(614, 298);
            this.Controls.Add(this.mainPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CurtainForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Curtain System";
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.mainPanel.ResumeLayout(false);
            this.mainPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button createCSButton;
        private System.Windows.Forms.CheckedListBox cgCheckedListBox;
        private System.Windows.Forms.Button addCGButton;
        private System.Windows.Forms.Label cgLabel;
        private System.Windows.Forms.Button removeCGButton;
        private System.Windows.Forms.Label facesLabel;
        private System.Windows.Forms.CheckedListBox facesCheckedListBox;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel operationStatusLabel;
        private System.Windows.Forms.Button deleteCSButton;
        private System.Windows.Forms.CheckedListBox csListBox;
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.Label csLabel;

    }
}