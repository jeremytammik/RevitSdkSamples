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
    partial class ThirdPanel
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
            this.confirmPanel = new System.Windows.Forms.Panel();
            this.confirmGroupBox = new System.Windows.Forms.GroupBox();
            this.printingProgressBar = new System.Windows.Forms.ProgressBar();
            this.printButton = new System.Windows.Forms.Button();
            this.settingButton = new System.Windows.Forms.Button();
            this.confirmTreeView = new System.Windows.Forms.TreeView();
            this.confirmPanel.SuspendLayout();
            this.confirmGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // confirmPanel
            // 
            this.confirmPanel.Controls.Add(this.confirmGroupBox);
            this.confirmPanel.Location = new System.Drawing.Point(0, 0);
            this.confirmPanel.Name = "confirmPanel";
            this.confirmPanel.Size = new System.Drawing.Size(280, 190);
            this.confirmPanel.TabIndex = 0;
            // 
            // confirmGroupBox
            // 
            this.confirmGroupBox.Controls.Add(this.printingProgressBar);
            this.confirmGroupBox.Controls.Add(this.printButton);
            this.confirmGroupBox.Controls.Add(this.settingButton);
            this.confirmGroupBox.Controls.Add(this.confirmTreeView);
            this.confirmGroupBox.Location = new System.Drawing.Point(0, 0);
            this.confirmGroupBox.Name = "confirmGroupBox";
            this.confirmGroupBox.Size = new System.Drawing.Size(280, 190);
            this.confirmGroupBox.TabIndex = 0;
            this.confirmGroupBox.TabStop = false;
            this.confirmGroupBox.Text = "Confirmation";
            // 
            // printingProgressBar
            // 
            this.printingProgressBar.Location = new System.Drawing.Point(6, 70);
            this.printingProgressBar.Name = "printingProgressBar";
            this.printingProgressBar.Size = new System.Drawing.Size(268, 23);
            this.printingProgressBar.Step = 1;
            this.printingProgressBar.TabIndex = 2;
            this.printingProgressBar.Visible = false;
            // 
            // printButton
            // 
            this.printButton.Location = new System.Drawing.Point(199, 161);
            this.printButton.Name = "printButton";
            this.printButton.Size = new System.Drawing.Size(75, 23);
            this.printButton.TabIndex = 1;
            this.printButton.Text = "Print";
            this.printButton.UseVisualStyleBackColor = true;
            this.printButton.Click += new System.EventHandler(this.printButton_Click);
            // 
            // settingButton
            // 
            this.settingButton.Location = new System.Drawing.Point(118, 161);
            this.settingButton.Name = "settingButton";
            this.settingButton.Size = new System.Drawing.Size(75, 23);
            this.settingButton.TabIndex = 1;
            this.settingButton.Text = "Settings...";
            this.settingButton.UseVisualStyleBackColor = true;
            this.settingButton.Click += new System.EventHandler(this.settingButton_Click);
            // 
            // confirmTreeView
            // 
            this.confirmTreeView.Location = new System.Drawing.Point(6, 19);
            this.confirmTreeView.Name = "confirmTreeView";
            this.confirmTreeView.Size = new System.Drawing.Size(268, 136);
            this.confirmTreeView.TabIndex = 0;
            // 
            // ThirdPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.confirmPanel);
            this.Name = "ThirdPanel";
            this.Size = new System.Drawing.Size(280, 190);
            this.confirmPanel.ResumeLayout(false);
            this.confirmGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel confirmPanel;
        private System.Windows.Forms.GroupBox confirmGroupBox;
        private System.Windows.Forms.Button printButton;
        private System.Windows.Forms.Button settingButton;
        private System.Windows.Forms.TreeView confirmTreeView;
        private System.Windows.Forms.ProgressBar printingProgressBar;
    }
}
