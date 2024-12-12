//
// (C) Copyright 2003-2017 by Autodesk, Inc.
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


namespace Revit.SDK.Samples.NewOpenings.CS
{
    partial class NewOpeningsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; 
        /// otherwise, false.</param>
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
            this.OkButton = new System.Windows.Forms.Button();
            this.openingPictureBox = new System.Windows.Forms.PictureBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.Notelabel = new System.Windows.Forms.Label();
            this.Notelabel2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.openingPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // OkButton
            // 
            this.OkButton.Location = new System.Drawing.Point(221, 439);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(80, 26);
            this.OkButton.TabIndex = 0;
            this.OkButton.Text = "&OK";
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // openingPictureBox
            // 
            this.openingPictureBox.BackColor = System.Drawing.SystemColors.Window;
            this.openingPictureBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.openingPictureBox.Location = new System.Drawing.Point(0, 0);
            this.openingPictureBox.Name = "openingPictureBox";
            this.openingPictureBox.Size = new System.Drawing.Size(415, 381);
            this.openingPictureBox.TabIndex = 1;
            this.openingPictureBox.TabStop = false;
            this.openingPictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.openingPictureBox_MouseDown);
            this.openingPictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.openingPictureBox_MouseMove);
            this.openingPictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.openingPictureBox_Paint);
            this.openingPictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.openingPictureBox_MouseUp);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(316, 439);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 26);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // Notelabel
            // 
            this.Notelabel.AutoSize = true;
            this.Notelabel.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Notelabel.Location = new System.Drawing.Point(-3, 393);
            this.Notelabel.Name = "Notelabel";
            this.Notelabel.Size = new System.Drawing.Size(415, 13);
            this.Notelabel.TabIndex = 2;
            this.Notelabel.Text = "  Use middle button of mouse to switch tool to draw Opening in Preview";
            // 
            // Notelabel2
            // 
            this.Notelabel2.AutoSize = true;
            this.Notelabel2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Notelabel2.Location = new System.Drawing.Point(-3, 416);
            this.Notelabel2.Name = "Notelabel2";
            this.Notelabel2.Size = new System.Drawing.Size(270, 13);
            this.Notelabel2.TabIndex = 3;
            this.Notelabel2.Text = "  Click right button of mouse to close the lines";
            // 
            // NewOpeningsForm
            // 
            this.AcceptButton = this.OkButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(415, 473);
            this.Controls.Add(this.Notelabel2);
            this.Controls.Add(this.Notelabel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.openingPictureBox);
            this.Controls.Add(this.OkButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewOpeningsForm";
            this.ShowInTaskbar = false;
            this.Text = "New Openings";
            ((System.ComponentModel.ISupportInitialize)(this.openingPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.PictureBox openingPictureBox;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label Notelabel;
        private System.Windows.Forms.Label Notelabel2;

    }
}