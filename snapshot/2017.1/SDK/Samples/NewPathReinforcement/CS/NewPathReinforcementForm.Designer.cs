//
// (C) Copyright 2003-2016 by Autodesk, Inc.
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
namespace Revit.SDK.Samples.NewPathReinforcement.CS
{
    partial class NewPathReinforcementForm
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
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.createButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.flipCheckBox = new System.Windows.Forms.CheckBox();
            this.labelNote = new System.Windows.Forms.Label();
            this.previewButton = new System.Windows.Forms.Button();
            this.cleanButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.BackColor = System.Drawing.SystemColors.Window;
            this.pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox.Location = new System.Drawing.Point(14, 12);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(446, 314);
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            this.pictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PictureBox_MouseDown);
            this.pictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PictureBox_MouseMove);
            this.pictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.PictureBox_Paint);
            // 
            // createButton
            // 
            this.createButton.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.createButton.Location = new System.Drawing.Point(279, 403);
            this.createButton.Name = "createButton";
            this.createButton.Size = new System.Drawing.Size(87, 23);
            this.createButton.TabIndex = 1;
            this.createButton.Text = "C&reate";
            this.createButton.UseVisualStyleBackColor = true;
            this.createButton.Click += new System.EventHandler(this.CreateButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelButton.Location = new System.Drawing.Point(373, 403);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(87, 23);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // flipCheckBox
            // 
            this.flipCheckBox.AutoSize = true;
            this.flipCheckBox.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.flipCheckBox.Location = new System.Drawing.Point(415, 332);
            this.flipCheckBox.Name = "flipCheckBox";
            this.flipCheckBox.Size = new System.Drawing.Size(45, 17);
            this.flipCheckBox.TabIndex = 3;
            this.flipCheckBox.Text = "Flip";
            this.flipCheckBox.UseVisualStyleBackColor = true;
            this.flipCheckBox.CheckedChanged += new System.EventHandler(this.FlipCheckBox_CheckedChanged);
            // 
            // labelNote
            // 
            this.labelNote.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNote.Location = new System.Drawing.Point(11, 336);
            this.labelNote.Name = "labelNote";
            this.labelNote.Size = new System.Drawing.Size(245, 33);
            this.labelNote.TabIndex = 4;
            this.labelNote.Text = "Click mouse left button and drag to draw a curve. Right click to finish drawing.";
            // 
            // previewButton
            // 
            this.previewButton.Enabled = false;
            this.previewButton.Location = new System.Drawing.Point(373, 364);
            this.previewButton.Name = "previewButton";
            this.previewButton.Size = new System.Drawing.Size(87, 23);
            this.previewButton.TabIndex = 5;
            this.previewButton.Text = "&Preview";
            this.previewButton.UseVisualStyleBackColor = true;
            this.previewButton.Click += new System.EventHandler(this.ButtonPreview_Click);
            // 
            // cleanButton
            // 
            this.cleanButton.Location = new System.Drawing.Point(279, 364);
            this.cleanButton.Name = "cleanButton";
            this.cleanButton.Size = new System.Drawing.Size(87, 23);
            this.cleanButton.TabIndex = 6;
            this.cleanButton.Text = "C&lean";
            this.cleanButton.UseVisualStyleBackColor = true;
            this.cleanButton.Click += new System.EventHandler(this.ButtonClean_Click);
            // 
            // NewPathReinforcementForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(472, 438);
            this.Controls.Add(this.cleanButton);
            this.Controls.Add(this.previewButton);
            this.Controls.Add(this.labelNote);
            this.Controls.Add(this.flipCheckBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.createButton);
            this.Controls.Add(this.pictureBox);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewPathReinforcementForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "New Path Reinforcement";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.NewPathReinforcementForm_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Button createButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.CheckBox flipCheckBox;
        private System.Windows.Forms.Label labelNote;
        private System.Windows.Forms.Button previewButton;
        private System.Windows.Forms.Button cleanButton;
    }
}