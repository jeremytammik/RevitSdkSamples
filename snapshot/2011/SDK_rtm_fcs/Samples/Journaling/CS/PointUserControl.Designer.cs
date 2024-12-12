//
// (C) Copyright 2003-2010 by Autodesk, Inc.
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


namespace Revit.SDK.Samples.ModelLines.CS
{
    partial class PointUserControl
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
            this.secondBracketLabel = new System.Windows.Forms.Label();
            this.secondCommaLabel = new System.Windows.Forms.Label();
            this.firstCommaLabel = new System.Windows.Forms.Label();
            this.zCoordinateTextBox = new System.Windows.Forms.TextBox();
            this.yCoordinateTextBox = new System.Windows.Forms.TextBox();
            this.xCoordinateTextBox = new System.Windows.Forms.TextBox();
            this.firstBracketLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // secondBracketLabel
            // 
            this.secondBracketLabel.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.secondBracketLabel.AutoSize = true;
            this.secondBracketLabel.Location = new System.Drawing.Point(203, 8);
            this.secondBracketLabel.Name = "secondBracketLabel";
            this.secondBracketLabel.Size = new System.Drawing.Size(10, 13);
            this.secondBracketLabel.TabIndex = 37;
            this.secondBracketLabel.Text = ")";
            // 
            // secondCommaLabel
            // 
            this.secondCommaLabel.AutoSize = true;
            this.secondCommaLabel.Location = new System.Drawing.Point(134, 7);
            this.secondCommaLabel.Name = "secondCommaLabel";
            this.secondCommaLabel.Size = new System.Drawing.Size(10, 13);
            this.secondCommaLabel.TabIndex = 36;
            this.secondCommaLabel.Text = ",";
            // 
            // firstCommaLabel
            // 
            this.firstCommaLabel.AutoSize = true;
            this.firstCommaLabel.Location = new System.Drawing.Point(66, 8);
            this.firstCommaLabel.Name = "firstCommaLabel";
            this.firstCommaLabel.Size = new System.Drawing.Size(10, 13);
            this.firstCommaLabel.TabIndex = 35;
            this.firstCommaLabel.Text = ",";
            // 
            // zCoordinateTextBox
            // 
            this.zCoordinateTextBox.Location = new System.Drawing.Point(152, 3);
            this.zCoordinateTextBox.Name = "zCoordinateTextBox";
            this.zCoordinateTextBox.Size = new System.Drawing.Size(45, 20);
            this.zCoordinateTextBox.TabIndex = 3;
            this.zCoordinateTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.CoordinateTextBox_Validating);
            // 
            // yCoordinateTextBox
            // 
            this.yCoordinateTextBox.Location = new System.Drawing.Point(83, 3);
            this.yCoordinateTextBox.Name = "yCoordinateTextBox";
            this.yCoordinateTextBox.Size = new System.Drawing.Size(45, 20);
            this.yCoordinateTextBox.TabIndex = 2;
            this.yCoordinateTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.CoordinateTextBox_Validating);
            // 
            // xCoordinateTextBox
            // 
            this.xCoordinateTextBox.Location = new System.Drawing.Point(15, 4);
            this.xCoordinateTextBox.Name = "xCoordinateTextBox";
            this.xCoordinateTextBox.Size = new System.Drawing.Size(45, 20);
            this.xCoordinateTextBox.TabIndex = 1;
            this.xCoordinateTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.CoordinateTextBox_Validating);
            // 
            // firstBracketLabel
            // 
            this.firstBracketLabel.AutoSize = true;
            this.firstBracketLabel.Location = new System.Drawing.Point(-1, 6);
            this.firstBracketLabel.Name = "firstBracketLabel";
            this.firstBracketLabel.Size = new System.Drawing.Size(10, 13);
            this.firstBracketLabel.TabIndex = 34;
            this.firstBracketLabel.Text = "(";
            // 
            // PointUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.secondBracketLabel);
            this.Controls.Add(this.secondCommaLabel);
            this.Controls.Add(this.firstCommaLabel);
            this.Controls.Add(this.zCoordinateTextBox);
            this.Controls.Add(this.yCoordinateTextBox);
            this.Controls.Add(this.xCoordinateTextBox);
            this.Controls.Add(this.firstBracketLabel);
            this.Name = "PointUserControl";
            this.Size = new System.Drawing.Size(213, 28);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label secondBracketLabel;
        private System.Windows.Forms.Label secondCommaLabel;
        private System.Windows.Forms.Label firstCommaLabel;
        private System.Windows.Forms.TextBox zCoordinateTextBox;
        private System.Windows.Forms.TextBox yCoordinateTextBox;
        private System.Windows.Forms.TextBox xCoordinateTextBox;
        private System.Windows.Forms.Label firstBracketLabel;
    }
}
