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

namespace Revit.SDK.Samples.BlendVertexConnectTable.CS
{
    /// <summary>
    /// vertex connect table form
    /// </summary>
    partial class VertexConnectTableForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.okButton = new System.Windows.Forms.Button();
            this.vertexConnectGridView = new System.Windows.Forms.DataGridView();
            this.geomPictureBox = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.vertexConnectGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.geomPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(423, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(154, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Blend Vertex Connection Table";
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.okButton.Location = new System.Drawing.Point(631, 367);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 2;
            this.okButton.Text = "&Close";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // vertexConnectGridView
            // 
            this.vertexConnectGridView.AllowUserToAddRows = false;
            this.vertexConnectGridView.AllowUserToDeleteRows = false;
            this.vertexConnectGridView.AllowUserToResizeColumns = false;
            this.vertexConnectGridView.AllowUserToResizeRows = false;
            this.vertexConnectGridView.BackgroundColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.vertexConnectGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.vertexConnectGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.vertexConnectGridView.Location = new System.Drawing.Point(426, 25);
            this.vertexConnectGridView.Name = "vertexConnectGridView";
            this.vertexConnectGridView.ReadOnly = true;
            this.vertexConnectGridView.RowHeadersVisible = false;
            this.vertexConnectGridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.vertexConnectGridView.Size = new System.Drawing.Size(280, 329);
            this.vertexConnectGridView.TabIndex = 3;
            // 
            // geomPictureBox
            // 
            this.geomPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.geomPictureBox.Location = new System.Drawing.Point(12, 25);
            this.geomPictureBox.Name = "geomPictureBox";
            this.geomPictureBox.Size = new System.Drawing.Size(408, 365);
            this.geomPictureBox.TabIndex = 4;
            this.geomPictureBox.TabStop = false;
            this.geomPictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.geomPictureBox_Paint);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(108, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Blend Edges Preview";
            // 
            // VertexConnectTableForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.okButton;
            this.ClientSize = new System.Drawing.Size(718, 402);
            this.Controls.Add(this.geomPictureBox);
            this.Controls.Add(this.vertexConnectGridView);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VertexConnectTableForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Blend Vertex Connection Table";
            this.Move += new System.EventHandler(this.VertexConnectTableForm_Move);
            ((System.ComponentModel.ISupportInitialize)(this.vertexConnectGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.geomPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.DataGridView vertexConnectGridView;
        private System.Windows.Forms.PictureBox geomPictureBox;
        private System.Windows.Forms.Label label2;
    }
}

