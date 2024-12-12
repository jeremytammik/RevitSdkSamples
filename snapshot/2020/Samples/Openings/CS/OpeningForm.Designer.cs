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


namespace Revit.SDK.Samples.Openings.CS
{
    partial class OpeningForm
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
            this.PreviewPictureBox = new System.Windows.Forms.PictureBox();
            this.Createbutton = new System.Windows.Forms.Button();
            this.OpeningPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.OKButton = new System.Windows.Forms.Button();
            this.OpeningListComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.PreviewPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // PreviewPictureBox
            // 
            this.PreviewPictureBox.Location = new System.Drawing.Point(12, 12);
            this.PreviewPictureBox.Name = "PreviewPictureBox";
            this.PreviewPictureBox.Size = new System.Drawing.Size(286, 290);
            this.PreviewPictureBox.TabIndex = 0;
            this.PreviewPictureBox.TabStop = false;
            this.PreviewPictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.PreviewPictureBox_Paint);
            // 
            // Createbutton
            // 
            this.Createbutton.Location = new System.Drawing.Point(370, 308);
            this.Createbutton.Name = "Createbutton";
            this.Createbutton.Size = new System.Drawing.Size(108, 23);
            this.Createbutton.TabIndex = 2;
            this.Createbutton.Text = "&Add X Model Line";
            this.Createbutton.UseVisualStyleBackColor = true;
            this.Createbutton.Click += new System.EventHandler(this.Createbutton_Click);
            // 
            // OpeningPropertyGrid
            // 
            this.OpeningPropertyGrid.Location = new System.Drawing.Point(304, 70);
            this.OpeningPropertyGrid.Name = "OpeningPropertyGrid";
            this.OpeningPropertyGrid.Size = new System.Drawing.Size(254, 232);
            this.OpeningPropertyGrid.TabIndex = 4;
            // 
            // OKButton
            // 
            this.OKButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.OKButton.Location = new System.Drawing.Point(484, 308);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 1;
            this.OKButton.Text = "&OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // OpeningListComboBox
            // 
            this.OpeningListComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.OpeningListComboBox.FormattingEnabled = true;
            this.OpeningListComboBox.Location = new System.Drawing.Point(304, 38);
            this.OpeningListComboBox.Name = "OpeningListComboBox";
            this.OpeningListComboBox.Size = new System.Drawing.Size(254, 21);
            this.OpeningListComboBox.TabIndex = 3;
            this.OpeningListComboBox.SelectedIndexChanged += new System.EventHandler(this.OpeningListComboBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(305, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Openings:";
            // 
            // OpeningForm
            // 
            this.AcceptButton = this.Createbutton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.OKButton;
            this.ClientSize = new System.Drawing.Size(570, 340);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.OpeningListComboBox);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.OpeningPropertyGrid);
            this.Controls.Add(this.Createbutton);
            this.Controls.Add(this.PreviewPictureBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OpeningForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "All openings";
            this.Load += new System.EventHandler(this.OpeningForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PreviewPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox PreviewPictureBox;
        private System.Windows.Forms.Button Createbutton;
        private System.Windows.Forms.PropertyGrid OpeningPropertyGrid;
        private  System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.ComboBox OpeningListComboBox;
        private System.Windows.Forms.Label label1;
    }
}