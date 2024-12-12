//
// (C) Copyright 2003-2012 by Autodesk, Inc.
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


namespace Revit.SDK.Samples.InPlaceMembers.CS
{
    partial class InPlaceMembersForm
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
            this.instancePropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.OKbutton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.modelPictureBox = new Revit.SDK.Samples.InPlaceMembers.CS.PictureBox3D();
            this.SuspendLayout();
            // 
            // instancePropertyGrid
            // 
            this.instancePropertyGrid.Location = new System.Drawing.Point(12, 12);
            this.instancePropertyGrid.Name = "instancePropertyGrid";
            this.instancePropertyGrid.Size = new System.Drawing.Size(547, 238);
            this.instancePropertyGrid.TabIndex = 2;
            // 
            // OKbutton
            // 
            this.OKbutton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.OKbutton.Location = new System.Drawing.Point(346, 573);
            this.OKbutton.Name = "OKbutton";
            this.OKbutton.Size = new System.Drawing.Size(88, 23);
            this.OKbutton.TabIndex = 0;
            this.OKbutton.Text = "&OK";
            this.OKbutton.UseVisualStyleBackColor = true;
            this.OKbutton.Click += new System.EventHandler(this.OKbutton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(471, 573);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(88, 23);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // modelPictureBox
            // 
            this.modelPictureBox.Cursor = System.Windows.Forms.Cursors.SizeAll;
            this.modelPictureBox.DataSource = null;
            this.modelPictureBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.modelPictureBox.Location = new System.Drawing.Point(12, 256);
            this.modelPictureBox.Name = "modelPictureBox";
            this.modelPictureBox.Size = new System.Drawing.Size(547, 311);
            this.modelPictureBox.TabIndex = 1;
            this.modelPictureBox.UseVisualStyleBackColor = true;
            // 
            // InPlaceMembersForm
            // 
            this.AcceptButton = this.OKbutton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(571, 606);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.OKbutton);
            this.Controls.Add(this.modelPictureBox);
            this.Controls.Add(this.instancePropertyGrid);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InPlaceMembersForm";
            this.ShowInTaskbar = false;
            this.Text = "In-Place Members";
            this.Load += new System.EventHandler(this.InPlaceMembersForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid instancePropertyGrid;
        private PictureBox3D modelPictureBox;
        private System.Windows.Forms.Button OKbutton;
        private System.Windows.Forms.Button cancelButton;
    }
}