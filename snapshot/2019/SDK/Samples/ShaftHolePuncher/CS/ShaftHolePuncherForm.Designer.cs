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
namespace Revit.SDK.Samples.ShaftHolePuncher.CS
{
    /// <summary>
    /// window form contains one picture box to show the 
    /// profile of wall (or floor), and three command buttons.
    /// User can draw curves of opening in picture box.
    /// </summary>
    partial class ShaftHolePuncherForm
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
            this.labelNote = new System.Windows.Forms.Label();
            this.cleanButton = new System.Windows.Forms.Button();
            this.ScaleComboBox = new System.Windows.Forms.ComboBox();
            this.scaleLabel = new System.Windows.Forms.Label();
            this.DirectionLabel = new System.Windows.Forms.Label();
            this.DirectionComboBox = new System.Windows.Forms.ComboBox();
            this.DirectionPanel = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.DirectionPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.BackColor = System.Drawing.SystemColors.Window;
            this.pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox.Location = new System.Drawing.Point(10, 12);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(450, 300);
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            this.pictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PictureBox_MouseDown);
            this.pictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PictureBox_MouseMove);
            this.pictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.PictureBox_Paint);
            // 
            // createButton
            // 
            this.createButton.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.createButton.Location = new System.Drawing.Point(275, 395);
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
            this.cancelButton.Location = new System.Drawing.Point(369, 395);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(87, 23);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // labelNote
            // 
            this.labelNote.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNote.Location = new System.Drawing.Point(7, 328);
            this.labelNote.Name = "labelNote";
            this.labelNote.Size = new System.Drawing.Size(233, 33);
            this.labelNote.TabIndex = 4;
            this.labelNote.Text = "Click and drag to create a curve. Right click to close the curve.";
            // 
            // cleanButton
            // 
            this.cleanButton.Location = new System.Drawing.Point(369, 357);
            this.cleanButton.Name = "cleanButton";
            this.cleanButton.Size = new System.Drawing.Size(87, 23);
            this.cleanButton.TabIndex = 3;
            this.cleanButton.Text = "C&lean";
            this.cleanButton.UseVisualStyleBackColor = true;
            this.cleanButton.Click += new System.EventHandler(this.ButtonClean_Click);
            // 
            // ScaleComboBox
            // 
            this.ScaleComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ScaleComboBox.FormattingEnabled = true;
            this.ScaleComboBox.Items.AddRange(new object[] {
            "1",
            "0.1",
            "0.3",
            "0.5",
            "0.8",
            "2",
            "3"});
            this.ScaleComboBox.Location = new System.Drawing.Point(369, 325);
            this.ScaleComboBox.Name = "ScaleComboBox";
            this.ScaleComboBox.Size = new System.Drawing.Size(85, 21);
            this.ScaleComboBox.TabIndex = 4;
            this.ScaleComboBox.Visible = false;
            this.ScaleComboBox.SelectedIndexChanged += new System.EventHandler(this.ScaleComboBox_SelectedIndexChanged);
            // 
            // scaleLabel
            // 
            this.scaleLabel.AutoSize = true;
            this.scaleLabel.Location = new System.Drawing.Point(316, 328);
            this.scaleLabel.Name = "scaleLabel";
            this.scaleLabel.Size = new System.Drawing.Size(47, 13);
            this.scaleLabel.TabIndex = 8;
            this.scaleLabel.Text = "Scale :";
            this.scaleLabel.Visible = false;
            // 
            // DirectionLabel
            // 
            this.DirectionLabel.AutoSize = true;
            this.DirectionLabel.Location = new System.Drawing.Point(3, 6);
            this.DirectionLabel.Name = "DirectionLabel";
            this.DirectionLabel.Size = new System.Drawing.Size(67, 13);
            this.DirectionLabel.TabIndex = 9;
            this.DirectionLabel.Text = "Direction :";
            // 
            // DirectionComboBox
            // 
            this.DirectionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DirectionComboBox.FormattingEnabled = true;
            this.DirectionComboBox.Items.AddRange(new object[] {
            "Z-axis",
            "Y-axis"});
            this.DirectionComboBox.Location = new System.Drawing.Point(76, 3);
            this.DirectionComboBox.Name = "DirectionComboBox";
            this.DirectionComboBox.Size = new System.Drawing.Size(85, 21);
            this.DirectionComboBox.TabIndex = 10;
            this.DirectionComboBox.SelectedIndexChanged += new System.EventHandler(this.DirectionComboBox_SelectedIndexChanged);
            // 
            // DirectionPanel
            // 
            this.DirectionPanel.Controls.Add(this.DirectionLabel);
            this.DirectionPanel.Controls.Add(this.DirectionComboBox);
            this.DirectionPanel.Location = new System.Drawing.Point(291, 323);
            this.DirectionPanel.Name = "DirectionPanel";
            this.DirectionPanel.Size = new System.Drawing.Size(169, 28);
            this.DirectionPanel.TabIndex = 11;
            this.DirectionPanel.Visible = false;
            // 
            // ShaftHolePuncherForm
            // 
            this.AcceptButton = this.createButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(470, 429);
            this.Controls.Add(this.DirectionPanel);
            this.Controls.Add(this.scaleLabel);
            this.Controls.Add(this.ScaleComboBox);
            this.Controls.Add(this.cleanButton);
            this.Controls.Add(this.labelNote);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.createButton);
            this.Controls.Add(this.pictureBox);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ShaftHolePuncherForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Shaft Hole Puncher";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.DirectionPanel.ResumeLayout(false);
            this.DirectionPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Button createButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label labelNote;
        private System.Windows.Forms.Button cleanButton;
        private System.Windows.Forms.ComboBox ScaleComboBox;
        private System.Windows.Forms.Label scaleLabel;
        private System.Windows.Forms.Label DirectionLabel;
        private System.Windows.Forms.ComboBox DirectionComboBox;
        private System.Windows.Forms.Panel DirectionPanel;
    }
}