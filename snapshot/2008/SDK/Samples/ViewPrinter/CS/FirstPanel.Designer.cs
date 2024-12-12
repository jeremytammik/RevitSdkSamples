//
// (C) Copyright 2003-2007 by Autodesk, Inc.
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

namespace Revit.SDK.Samples.ViewPrinter.CS
{
    partial class FirstPanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.proToPrintPanel = new System.Windows.Forms.Panel();
            this.proToPrintGroupBox = new System.Windows.Forms.GroupBox();
            this.proToPrintTextBox = new System.Windows.Forms.TextBox();
            this.proOnDiskTextBox = new System.Windows.Forms.TextBox();
            this.proOnDiskButton = new System.Windows.Forms.Button();
            this.proOnDiskRadioButton = new System.Windows.Forms.RadioButton();
            this.currentProRadioButton = new System.Windows.Forms.RadioButton();
            this.prjInfoLabel = new System.Windows.Forms.Label();
            this.proToPrintPanel.SuspendLayout();
            this.proToPrintGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // proToPrintPanel
            // 
            this.proToPrintPanel.Controls.Add(this.proToPrintGroupBox);
            this.proToPrintPanel.Location = new System.Drawing.Point(0, 0);
            this.proToPrintPanel.Name = "proToPrintPanel";
            this.proToPrintPanel.Size = new System.Drawing.Size(280, 190);
            this.proToPrintPanel.TabIndex = 0;
            // 
            // proToPrintGroupBox
            // 
            this.proToPrintGroupBox.Controls.Add(this.prjInfoLabel);
            this.proToPrintGroupBox.Controls.Add(this.proToPrintTextBox);
            this.proToPrintGroupBox.Controls.Add(this.proOnDiskTextBox);
            this.proToPrintGroupBox.Controls.Add(this.proOnDiskButton);
            this.proToPrintGroupBox.Controls.Add(this.proOnDiskRadioButton);
            this.proToPrintGroupBox.Controls.Add(this.currentProRadioButton);
            this.proToPrintGroupBox.Location = new System.Drawing.Point(0, 0);
            this.proToPrintGroupBox.Name = "proToPrintGroupBox";
            this.proToPrintGroupBox.Size = new System.Drawing.Size(280, 190);
            this.proToPrintGroupBox.TabIndex = 0;
            this.proToPrintGroupBox.TabStop = false;
            this.proToPrintGroupBox.Text = "Project to print";
            // 
            // proToPrintTextBox
            // 
            this.proToPrintTextBox.Enabled = false;
            this.proToPrintTextBox.Location = new System.Drawing.Point(6, 91);
            this.proToPrintTextBox.Multiline = true;
            this.proToPrintTextBox.Name = "proToPrintTextBox";
            this.proToPrintTextBox.Size = new System.Drawing.Size(268, 93);
            this.proToPrintTextBox.TabIndex = 3;
            this.proToPrintTextBox.Text = "IssueDate - TBD\r\nStatus - TBD\r\nClientName - TBD\r\nAddress - TBD\r\nName - TBD\r\nNumbe" +
                "r - TBD";
            // 
            // proOnDiskTextBox
            // 
            this.proOnDiskTextBox.Enabled = false;
            this.proOnDiskTextBox.Location = new System.Drawing.Point(6, 42);
            this.proOnDiskTextBox.Name = "proOnDiskTextBox";
            this.proOnDiskTextBox.Size = new System.Drawing.Size(187, 20);
            this.proOnDiskTextBox.TabIndex = 2;
            // 
            // proOnDiskButton
            // 
            this.proOnDiskButton.Enabled = false;
            this.proOnDiskButton.Location = new System.Drawing.Point(199, 42);
            this.proOnDiskButton.Name = "proOnDiskButton";
            this.proOnDiskButton.Size = new System.Drawing.Size(75, 23);
            this.proOnDiskButton.TabIndex = 1;
            this.proOnDiskButton.Text = "Browse...";
            this.proOnDiskButton.UseVisualStyleBackColor = true;
            this.proOnDiskButton.Click += new System.EventHandler(this.proOnDiskButton_Click);
            // 
            // proOnDiskRadioButton
            // 
            this.proOnDiskRadioButton.AutoSize = true;
            this.proOnDiskRadioButton.Location = new System.Drawing.Point(107, 19);
            this.proOnDiskRadioButton.Name = "proOnDiskRadioButton";
            this.proOnDiskRadioButton.Size = new System.Drawing.Size(95, 17);
            this.proOnDiskRadioButton.TabIndex = 0;
            this.proOnDiskRadioButton.Text = "Project on disk";
            this.proOnDiskRadioButton.UseVisualStyleBackColor = true;
            // 
            // currentProRadioButton
            // 
            this.currentProRadioButton.AutoSize = true;
            this.currentProRadioButton.Checked = true;
            this.currentProRadioButton.Location = new System.Drawing.Point(6, 19);
            this.currentProRadioButton.Name = "currentProRadioButton";
            this.currentProRadioButton.Size = new System.Drawing.Size(95, 17);
            this.currentProRadioButton.TabIndex = 0;
            this.currentProRadioButton.TabStop = true;
            this.currentProRadioButton.Text = "Current Project";
            this.currentProRadioButton.UseVisualStyleBackColor = true;
            this.currentProRadioButton.CheckedChanged += new System.EventHandler(this.currentProRadioButton_CheckedChanged);
            // 
            // prjInfoLabel
            // 
            this.prjInfoLabel.AutoSize = true;
            this.prjInfoLabel.Location = new System.Drawing.Point(3, 71);
            this.prjInfoLabel.Name = "prjInfoLabel";
            this.prjInfoLabel.Size = new System.Drawing.Size(95, 13);
            this.prjInfoLabel.TabIndex = 4;
            this.prjInfoLabel.Text = "Project Information";
            // 
            // FirstPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.proToPrintPanel);
            this.Name = "FirstPanel";
            this.Size = new System.Drawing.Size(280, 190);
            this.proToPrintPanel.ResumeLayout(false);
            this.proToPrintGroupBox.ResumeLayout(false);
            this.proToPrintGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel proToPrintPanel;
        private System.Windows.Forms.GroupBox proToPrintGroupBox;
        private System.Windows.Forms.TextBox proOnDiskTextBox;
        private System.Windows.Forms.Button proOnDiskButton;
        private System.Windows.Forms.RadioButton proOnDiskRadioButton;
        private System.Windows.Forms.RadioButton currentProRadioButton;
        private System.Windows.Forms.TextBox proToPrintTextBox;
        private System.Windows.Forms.Label prjInfoLabel;
    }
}
