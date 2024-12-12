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
namespace Revit.SDK.Samples.ModelessForm_IdlingEvent.CS
{
    partial class ModelessForm
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
            this.btnExit = new System.Windows.Forms.Button();
            this.btnDeleted = new System.Windows.Forms.Button();
            this.btnFlipUpDown = new System.Windows.Forms.Button();
            this.btnFlipLeftRight = new System.Windows.Forms.Button();
            this.btnFlipLeft = new System.Windows.Forms.Button();
            this.btnFlipRight = new System.Windows.Forms.Button();
            this.btnFlipUp = new System.Windows.Forms.Button();
            this.btnFlipDown = new System.Windows.Forms.Button();
            this.btnRotate = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(66, 146);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(89, 23);
            this.btnExit.TabIndex = 0;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnDeleted
            // 
            this.btnDeleted.Location = new System.Drawing.Point(115, 102);
            this.btnDeleted.Name = "btnDeleted";
            this.btnDeleted.Size = new System.Drawing.Size(89, 24);
            this.btnDeleted.TabIndex = 8;
            this.btnDeleted.Text = "Delete";
            this.btnDeleted.UseVisualStyleBackColor = true;
            this.btnDeleted.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnFlipUpDown
            // 
            this.btnFlipUpDown.Location = new System.Drawing.Point(115, 12);
            this.btnFlipUpDown.Name = "btnFlipUpDown";
            this.btnFlipUpDown.Size = new System.Drawing.Size(89, 24);
            this.btnFlipUpDown.TabIndex = 2;
            this.btnFlipUpDown.Text = "In / Out";
            this.btnFlipUpDown.UseVisualStyleBackColor = true;
            this.btnFlipUpDown.Click += new System.EventHandler(this.btnFlipInOut_Click);
            // 
            // btnFlipLeftRight
            // 
            this.btnFlipLeftRight.Location = new System.Drawing.Point(12, 12);
            this.btnFlipLeftRight.Name = "btnFlipLeftRight";
            this.btnFlipLeftRight.Size = new System.Drawing.Size(89, 24);
            this.btnFlipLeftRight.TabIndex = 1;
            this.btnFlipLeftRight.Text = "Left / Right";
            this.btnFlipLeftRight.UseVisualStyleBackColor = true;
            this.btnFlipLeftRight.Click += new System.EventHandler(this.btnFlipLeftRight_Click);
            // 
            // btnFlipLeft
            // 
            this.btnFlipLeft.Location = new System.Drawing.Point(12, 42);
            this.btnFlipLeft.Name = "btnFlipLeft";
            this.btnFlipLeft.Size = new System.Drawing.Size(89, 24);
            this.btnFlipLeft.TabIndex = 3;
            this.btnFlipLeft.Text = "Left";
            this.btnFlipLeft.UseVisualStyleBackColor = true;
            this.btnFlipLeft.Click += new System.EventHandler(this.btnFlipLeft_Click);
            // 
            // btnFlipRight
            // 
            this.btnFlipRight.Location = new System.Drawing.Point(12, 72);
            this.btnFlipRight.Name = "btnFlipRight";
            this.btnFlipRight.Size = new System.Drawing.Size(89, 24);
            this.btnFlipRight.TabIndex = 5;
            this.btnFlipRight.Text = "Right";
            this.btnFlipRight.UseVisualStyleBackColor = true;
            this.btnFlipRight.Click += new System.EventHandler(this.btnFlipRight_Click);
            // 
            // btnFlipUp
            // 
            this.btnFlipUp.Location = new System.Drawing.Point(115, 42);
            this.btnFlipUp.Name = "btnFlipUp";
            this.btnFlipUp.Size = new System.Drawing.Size(89, 24);
            this.btnFlipUp.TabIndex = 4;
            this.btnFlipUp.Text = "Out";
            this.btnFlipUp.UseVisualStyleBackColor = true;
            this.btnFlipUp.Click += new System.EventHandler(this.btnFlipOut_Click);
            // 
            // btnFlipDown
            // 
            this.btnFlipDown.Location = new System.Drawing.Point(115, 72);
            this.btnFlipDown.Name = "btnFlipDown";
            this.btnFlipDown.Size = new System.Drawing.Size(89, 24);
            this.btnFlipDown.TabIndex = 6;
            this.btnFlipDown.Text = "In";
            this.btnFlipDown.UseVisualStyleBackColor = true;
            this.btnFlipDown.Click += new System.EventHandler(this.btnFlipIn_Click);
            // 
            // btnRotate
            // 
            this.btnRotate.Location = new System.Drawing.Point(12, 102);
            this.btnRotate.Name = "btnRotate";
            this.btnRotate.Size = new System.Drawing.Size(89, 24);
            this.btnRotate.TabIndex = 7;
            this.btnRotate.Text = "Rotate";
            this.btnRotate.UseVisualStyleBackColor = true;
            this.btnRotate.Click += new System.EventHandler(this.btnRotate_Click);
            // 
            // ModelessForm
            // 
            this.AcceptButton = this.btnExit;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(216, 184);
            this.Controls.Add(this.btnRotate);
            this.Controls.Add(this.btnFlipRight);
            this.Controls.Add(this.btnFlipLeft);
            this.Controls.Add(this.btnFlipLeftRight);
            this.Controls.Add(this.btnFlipDown);
            this.Controls.Add(this.btnFlipUp);
            this.Controls.Add(this.btnFlipUpDown);
            this.Controls.Add(this.btnDeleted);
            this.Controls.Add(this.btnExit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ModelessForm";
            this.Opacity = 0.75D;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Door Operator (Idling Event)";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnDeleted;
        private System.Windows.Forms.Button btnFlipUpDown;
        private System.Windows.Forms.Button btnFlipLeftRight;
        private System.Windows.Forms.Button btnFlipLeft;
        private System.Windows.Forms.Button btnFlipRight;
        private System.Windows.Forms.Button btnFlipUp;
        private System.Windows.Forms.Button btnFlipDown;
        private System.Windows.Forms.Button btnRotate;
    }
}