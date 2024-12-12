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
    partial class ViewPrinterForm
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
            this.cancelButton = new System.Windows.Forms.Button();
            this.nextButton = new System.Windows.Forms.Button();
            this.lastButton = new System.Windows.Forms.Button();
            this.firstPanel = new Revit.SDK.Samples.ViewPrinter.CS.FirstPanel();
            this.secondPanel = new Revit.SDK.Samples.ViewPrinter.CS.SecondPanel();
            this.thirdPanel = new Revit.SDK.Samples.ViewPrinter.CS.ThirdPanel();
            this.splitLinePictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitLinePictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(220, 226);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 0;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // nextButton
            // 
            this.nextButton.Location = new System.Drawing.Point(139, 226);
            this.nextButton.Name = "nextButton";
            this.nextButton.Size = new System.Drawing.Size(75, 23);
            this.nextButton.TabIndex = 0;
            this.nextButton.Text = "Next >";
            this.nextButton.UseVisualStyleBackColor = true;
            this.nextButton.Click += new System.EventHandler(this.nextButton_Click);
            // 
            // lastButton
            // 
            this.lastButton.Enabled = false;
            this.lastButton.Location = new System.Drawing.Point(58, 226);
            this.lastButton.Name = "lastButton";
            this.lastButton.Size = new System.Drawing.Size(75, 23);
            this.lastButton.TabIndex = 0;
            this.lastButton.Text = "< Previous";
            this.lastButton.UseVisualStyleBackColor = true;
            this.lastButton.Click += new System.EventHandler(this.lastButton_Click);
            // 
            // firstPanel
            // 
            this.firstPanel.Location = new System.Drawing.Point(12, 12);
            this.firstPanel.Name = "firstPanel";
            this.firstPanel.Size = new System.Drawing.Size(280, 190);
            this.firstPanel.TabIndex = 3;
            // 
            // secondPanel
            // 
            this.secondPanel.Location = new System.Drawing.Point(12, 12);
            this.secondPanel.Name = "secondPanel";
            this.secondPanel.Size = new System.Drawing.Size(280, 190);
            this.secondPanel.TabIndex = 2;
            this.secondPanel.Visible = false;
            // 
            // thirdPanel
            // 
            this.thirdPanel.Location = new System.Drawing.Point(12, 12);
            this.thirdPanel.Name = "thirdPanel";
            this.thirdPanel.Size = new System.Drawing.Size(280, 190);
            this.thirdPanel.TabIndex = 1;
            this.thirdPanel.Visible = false;
            // 
            // splitLinePictureBox
            // 
            this.splitLinePictureBox.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.splitLinePictureBox.Location = new System.Drawing.Point(12, 214);
            this.splitLinePictureBox.Name = "splitLinePictureBox";
            this.splitLinePictureBox.Size = new System.Drawing.Size(280, 1);
            this.splitLinePictureBox.TabIndex = 4;
            this.splitLinePictureBox.TabStop = false;
            // 
            // ViewPrinterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(304, 260);
            this.Controls.Add(this.splitLinePictureBox);
            this.Controls.Add(this.firstPanel);
            this.Controls.Add(this.secondPanel);
            this.Controls.Add(this.thirdPanel);
            this.Controls.Add(this.lastButton);
            this.Controls.Add(this.nextButton);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ViewPrinterForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "View Printer";
            ((System.ComponentModel.ISupportInitialize)(this.splitLinePictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button nextButton;
        private System.Windows.Forms.Button lastButton;
        private ThirdPanel thirdPanel;
        private SecondPanel secondPanel;
        private FirstPanel firstPanel;
        private System.Windows.Forms.PictureBox splitLinePictureBox;
    }
}