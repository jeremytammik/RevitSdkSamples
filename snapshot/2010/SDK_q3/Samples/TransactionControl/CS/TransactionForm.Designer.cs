//
// (C) Copyright 2003-2009 by Autodesk, Inc.
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted,
// provided that the above copyright notify appears in all copies and
// that both that copyright notify and the limited warranty and
// restricted rights notify below appear in all supporting
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

namespace Revit.SDK.Samples.TransactionControl.CS
{
    partial class TransactionForm
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
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.abortTransButton = new System.Windows.Forms.Button();
            this.endTransButton = new System.Windows.Forms.Button();
            this.beginTransButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.deleteWallButton = new System.Windows.Forms.Button();
            this.moveWallButton = new System.Windows.Forms.Button();
            this.createWallbutton = new System.Windows.Forms.Button();
            this.transactionsTreeView = new System.Windows.Forms.TreeView();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(212, 271);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 7;
            this.okButton.Text = "&OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(293, 271);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 8;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.abortTransButton);
            this.groupBox1.Controls.Add(this.endTransButton);
            this.groupBox1.Controls.Add(this.beginTransButton);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(115, 115);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Transaction";
            // 
            // abortTransButton
            // 
            this.abortTransButton.Location = new System.Drawing.Point(20, 77);
            this.abortTransButton.Name = "abortTransButton";
            this.abortTransButton.Size = new System.Drawing.Size(75, 23);
            this.abortTransButton.TabIndex = 2;
            this.abortTransButton.Text = "&Abort";
            this.abortTransButton.UseVisualStyleBackColor = true;
            this.abortTransButton.Click += new System.EventHandler(this.abortTransButton_Click);
            // 
            // endTransButton
            // 
            this.endTransButton.Location = new System.Drawing.Point(20, 48);
            this.endTransButton.Name = "endTransButton";
            this.endTransButton.Size = new System.Drawing.Size(75, 23);
            this.endTransButton.TabIndex = 1;
            this.endTransButton.Text = "&End";
            this.endTransButton.UseVisualStyleBackColor = true;
            this.endTransButton.Click += new System.EventHandler(this.endTransButton_Click);
            // 
            // beginTransButton
            // 
            this.beginTransButton.Location = new System.Drawing.Point(20, 19);
            this.beginTransButton.Name = "beginTransButton";
            this.beginTransButton.Size = new System.Drawing.Size(75, 23);
            this.beginTransButton.TabIndex = 0;
            this.beginTransButton.Text = "&Begin";
            this.beginTransButton.UseVisualStyleBackColor = true;
            this.beginTransButton.Click += new System.EventHandler(this.beginTransButton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.deleteWallButton);
            this.groupBox2.Controls.Add(this.moveWallButton);
            this.groupBox2.Controls.Add(this.createWallbutton);
            this.groupBox2.Location = new System.Drawing.Point(13, 134);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(115, 117);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Operations";
            // 
            // deleteWallButton
            // 
            this.deleteWallButton.Location = new System.Drawing.Point(20, 77);
            this.deleteWallButton.Name = "deleteWallButton";
            this.deleteWallButton.Size = new System.Drawing.Size(75, 23);
            this.deleteWallButton.TabIndex = 2;
            this.deleteWallButton.Text = "&Delete Wall";
            this.deleteWallButton.UseVisualStyleBackColor = true;
            this.deleteWallButton.Click += new System.EventHandler(this.deleteWallButton_Click);
            // 
            // moveWallButton
            // 
            this.moveWallButton.Location = new System.Drawing.Point(20, 48);
            this.moveWallButton.Name = "moveWallButton";
            this.moveWallButton.Size = new System.Drawing.Size(75, 23);
            this.moveWallButton.TabIndex = 1;
            this.moveWallButton.Text = "&Move Wall";
            this.moveWallButton.UseVisualStyleBackColor = true;
            this.moveWallButton.Click += new System.EventHandler(this.moveWallButton_Click);
            // 
            // createWallbutton
            // 
            this.createWallbutton.Location = new System.Drawing.Point(20, 19);
            this.createWallbutton.Name = "createWallbutton";
            this.createWallbutton.Size = new System.Drawing.Size(75, 23);
            this.createWallbutton.TabIndex = 0;
            this.createWallbutton.Text = "C&reate Wall";
            this.createWallbutton.UseVisualStyleBackColor = true;
            this.createWallbutton.Click += new System.EventHandler(this.createWallbutton_Click);
            // 
            // transactionsTreeView
            // 
            this.transactionsTreeView.Location = new System.Drawing.Point(134, 13);
            this.transactionsTreeView.Name = "transactionsTreeView";
            this.transactionsTreeView.Size = new System.Drawing.Size(234, 252);
            this.transactionsTreeView.TabIndex = 11;
            // 
            // TransactionForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(380, 302);
            this.Controls.Add(this.transactionsTreeView);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TransactionForm";
            this.ShowInTaskbar = false;
            this.Text = "Transaction";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button abortTransButton;
        private System.Windows.Forms.Button endTransButton;
        private System.Windows.Forms.Button beginTransButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button deleteWallButton;
        private System.Windows.Forms.Button moveWallButton;
        private System.Windows.Forms.Button createWallbutton;
        private System.Windows.Forms.TreeView transactionsTreeView;
    }
}