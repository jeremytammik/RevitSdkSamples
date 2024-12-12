//
// (C) Copyright 2003-2013 by Autodesk, Inc.
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
namespace Revit.SDK.Samples.SlabShapeEditing.CS
{
    partial class SlabShapeEditingForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SlabShapeEditingForm));
            this.SlabShapePictureBox = new System.Windows.Forms.PictureBox();
            this.PointButton = new System.Windows.Forms.Button();
            this.LineButton = new System.Windows.Forms.Button();
            this.DistanceLabel = new System.Windows.Forms.Label();
            this.DistanceTextBox = new System.Windows.Forms.TextBox();
            this.MoveButton = new System.Windows.Forms.Button();
            this.ResetButton = new System.Windows.Forms.Button();
            this.OKButton = new System.Windows.Forms.Button();
            this.UpdateButton = new System.Windows.Forms.Button();
            this.NoteLabel = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.SlabShapePictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // SlabShapePictureBox
            // 
            this.SlabShapePictureBox.BackColor = System.Drawing.Color.White;
            this.SlabShapePictureBox.Location = new System.Drawing.Point(12, 13);
            this.SlabShapePictureBox.Name = "SlabShapePictureBox";
            this.SlabShapePictureBox.Size = new System.Drawing.Size(354, 301);
            this.SlabShapePictureBox.TabIndex = 0;
            this.SlabShapePictureBox.TabStop = false;
            this.SlabShapePictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SlabShapePictureBox_MouseDown);
            this.SlabShapePictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SlabShapePictureBox_MouseMove);
            this.SlabShapePictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.SlabShapePictureBox_Paint);
            this.SlabShapePictureBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.SlabShapePictureBox_MouseClick);
            this.SlabShapePictureBox.MouseHover += new System.EventHandler(this.SlabShapePictureBox_MouseHover);
            // 
            // PointButton
            // 
            this.PointButton.Image = ((System.Drawing.Image)(resources.GetObject("PointButton.Image")));
            this.PointButton.Location = new System.Drawing.Point(46, 342);
            this.PointButton.Name = "PointButton";
            this.PointButton.Size = new System.Drawing.Size(28, 28);
            this.PointButton.TabIndex = 1;
            this.PointButton.UseVisualStyleBackColor = true;
            this.PointButton.Click += new System.EventHandler(this.PointButton_Click);
            this.PointButton.MouseHover += new System.EventHandler(this.PointButton_MouseHover);
            // 
            // LineButton
            // 
            this.LineButton.Image = ((System.Drawing.Image)(resources.GetObject("LineButton.Image")));
            this.LineButton.Location = new System.Drawing.Point(80, 342);
            this.LineButton.Name = "LineButton";
            this.LineButton.Size = new System.Drawing.Size(28, 28);
            this.LineButton.TabIndex = 2;
            this.LineButton.UseVisualStyleBackColor = true;
            this.LineButton.Click += new System.EventHandler(this.LineButton_Click);
            this.LineButton.MouseHover += new System.EventHandler(this.LineButton_MouseHover);
            // 
            // DistanceLabel
            // 
            this.DistanceLabel.AutoSize = true;
            this.DistanceLabel.Location = new System.Drawing.Point(113, 351);
            this.DistanceLabel.Name = "DistanceLabel";
            this.DistanceLabel.Size = new System.Drawing.Size(93, 14);
            this.DistanceLabel.TabIndex = 4;
            this.DistanceLabel.Text = "Distance (Feet):";
            // 
            // DistanceTextBox
            // 
            this.DistanceTextBox.Location = new System.Drawing.Point(210, 348);
            this.DistanceTextBox.Name = "DistanceTextBox";
            this.DistanceTextBox.Size = new System.Drawing.Size(75, 22);
            this.DistanceTextBox.TabIndex = 5;
            // 
            // MoveButton
            // 
            this.MoveButton.Image = ((System.Drawing.Image)(resources.GetObject("MoveButton.Image")));
            this.MoveButton.Location = new System.Drawing.Point(12, 342);
            this.MoveButton.Name = "MoveButton";
            this.MoveButton.Size = new System.Drawing.Size(28, 28);
            this.MoveButton.TabIndex = 6;
            this.MoveButton.UseVisualStyleBackColor = true;
            this.MoveButton.Click += new System.EventHandler(this.MoveButton_Click);
            this.MoveButton.MouseHover += new System.EventHandler(this.MoveButton_MouseHover);
            // 
            // ResetButton
            // 
            this.ResetButton.Location = new System.Drawing.Point(210, 387);
            this.ResetButton.Name = "ResetButton";
            this.ResetButton.Size = new System.Drawing.Size(75, 25);
            this.ResetButton.TabIndex = 7;
            this.ResetButton.Text = "&Reset";
            this.ResetButton.UseVisualStyleBackColor = true;
            this.ResetButton.Click += new System.EventHandler(this.ResetButton_Click);
            // 
            // OKButton
            // 
            this.OKButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.OKButton.Location = new System.Drawing.Point(291, 387);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 25);
            this.OKButton.TabIndex = 9;
            this.OKButton.Text = "&OK";
            this.OKButton.UseVisualStyleBackColor = true;
            // 
            // UpdateButton
            // 
            this.UpdateButton.Location = new System.Drawing.Point(291, 348);
            this.UpdateButton.Name = "UpdateButton";
            this.UpdateButton.Size = new System.Drawing.Size(75, 25);
            this.UpdateButton.TabIndex = 10;
            this.UpdateButton.Text = "&Update";
            this.UpdateButton.UseVisualStyleBackColor = true;
            this.UpdateButton.Click += new System.EventHandler(this.UpdateButton_Click);
            // 
            // NoteLabel
            // 
            this.NoteLabel.AutoSize = true;
            this.NoteLabel.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NoteLabel.Location = new System.Drawing.Point(12, 317);
            this.NoteLabel.Name = "NoteLabel";
            this.NoteLabel.Size = new System.Drawing.Size(316, 15);
            this.NoteLabel.TabIndex = 11;
            this.NoteLabel.Text = "Click right mouse button and move mouse to rotate Slab.";
            // 
            // SlabShapeEditingForm
            // 
            this.AcceptButton = this.OKButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.OKButton;
            this.ClientSize = new System.Drawing.Size(377, 425);
            this.Controls.Add(this.NoteLabel);
            this.Controls.Add(this.UpdateButton);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.ResetButton);
            this.Controls.Add(this.MoveButton);
            this.Controls.Add(this.DistanceTextBox);
            this.Controls.Add(this.DistanceLabel);
            this.Controls.Add(this.LineButton);
            this.Controls.Add(this.PointButton);
            this.Controls.Add(this.SlabShapePictureBox);
            this.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SlabShapeEditingForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Slab Shape Editing";
            ((System.ComponentModel.ISupportInitialize)(this.SlabShapePictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox SlabShapePictureBox;
        private System.Windows.Forms.Button PointButton;
        private System.Windows.Forms.Button LineButton;
        private System.Windows.Forms.Label DistanceLabel;
        private System.Windows.Forms.TextBox DistanceTextBox;
        private System.Windows.Forms.Button MoveButton;
        private System.Windows.Forms.Button ResetButton;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Button UpdateButton;
        private System.Windows.Forms.Label NoteLabel;
        private System.Windows.Forms.ToolTip toolTip;
    }
}