//
// (C) Copyright 2003-2012 by Autodesk, Inc.
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
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBoxTrans = new System.Windows.Forms.GroupBox();
            this.btnRollbackTrans = new System.Windows.Forms.Button();
            this.btnCommitTrans = new System.Windows.Forms.Button();
            this.btnStartTrans = new System.Windows.Forms.Button();
            this.groupBoxOperation = new System.Windows.Forms.GroupBox();
            this.btnDeleteWall = new System.Windows.Forms.Button();
            this.btnMoveWall = new System.Windows.Forms.Button();
            this.btnCreateWall = new System.Windows.Forms.Button();
            this.transactionsTreeView = new System.Windows.Forms.TreeView();
            this.btnRollbackTransGroup = new System.Windows.Forms.Button();
            this.groupBoxTransGroup = new System.Windows.Forms.GroupBox();
            this.btnCommitTransGroup = new System.Windows.Forms.Button();
            this.btnStartTransGroup = new System.Windows.Forms.Button();
            this.groupBoxTrans.SuspendLayout();
            this.groupBoxOperation.SuspendLayout();
            this.groupBoxTransGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(262, 381);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 7;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.okButton_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(343, 381);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // groupBoxTrans
            // 
            this.groupBoxTrans.Controls.Add(this.btnRollbackTrans);
            this.groupBoxTrans.Controls.Add(this.btnCommitTrans);
            this.groupBoxTrans.Controls.Add(this.btnStartTrans);
            this.groupBoxTrans.Location = new System.Drawing.Point(13, 131);
            this.groupBoxTrans.Name = "groupBoxTrans";
            this.groupBoxTrans.Size = new System.Drawing.Size(115, 111);
            this.groupBoxTrans.TabIndex = 9;
            this.groupBoxTrans.TabStop = false;
            this.groupBoxTrans.Text = "Transaction";
            // 
            // btnRollbackTrans
            // 
            this.btnRollbackTrans.Location = new System.Drawing.Point(20, 48);
            this.btnRollbackTrans.Name = "btnRollbackTrans";
            this.btnRollbackTrans.Size = new System.Drawing.Size(75, 23);
            this.btnRollbackTrans.TabIndex = 2;
            this.btnRollbackTrans.Text = "&Rollback";
            this.btnRollbackTrans.UseVisualStyleBackColor = true;
            this.btnRollbackTrans.Click += new System.EventHandler(this.rollbackTransButton_Click);
            // 
            // btnCommitTrans
            // 
            this.btnCommitTrans.Location = new System.Drawing.Point(20, 77);
            this.btnCommitTrans.Name = "btnCommitTrans";
            this.btnCommitTrans.Size = new System.Drawing.Size(75, 23);
            this.btnCommitTrans.TabIndex = 1;
            this.btnCommitTrans.Text = "&Commit";
            this.btnCommitTrans.UseVisualStyleBackColor = true;
            this.btnCommitTrans.Click += new System.EventHandler(this.commitTransButton_Click);
            // 
            // btnStartTrans
            // 
            this.btnStartTrans.Location = new System.Drawing.Point(20, 19);
            this.btnStartTrans.Name = "btnStartTrans";
            this.btnStartTrans.Size = new System.Drawing.Size(75, 23);
            this.btnStartTrans.TabIndex = 0;
            this.btnStartTrans.Text = "&Start";
            this.btnStartTrans.UseVisualStyleBackColor = true;
            this.btnStartTrans.Click += new System.EventHandler(this.startTransButton_Click);
            // 
            // groupBoxOperation
            // 
            this.groupBoxOperation.Controls.Add(this.btnDeleteWall);
            this.groupBoxOperation.Controls.Add(this.btnMoveWall);
            this.groupBoxOperation.Controls.Add(this.btnCreateWall);
            this.groupBoxOperation.Location = new System.Drawing.Point(13, 248);
            this.groupBoxOperation.Name = "groupBoxOperation";
            this.groupBoxOperation.Size = new System.Drawing.Size(115, 110);
            this.groupBoxOperation.TabIndex = 10;
            this.groupBoxOperation.TabStop = false;
            this.groupBoxOperation.Text = "Operations";
            // 
            // btnDeleteWall
            // 
            this.btnDeleteWall.Location = new System.Drawing.Point(20, 77);
            this.btnDeleteWall.Name = "btnDeleteWall";
            this.btnDeleteWall.Size = new System.Drawing.Size(75, 23);
            this.btnDeleteWall.TabIndex = 2;
            this.btnDeleteWall.Text = "&Delete Wall";
            this.btnDeleteWall.UseVisualStyleBackColor = true;
            this.btnDeleteWall.Click += new System.EventHandler(this.deleteWallButton_Click);
            // 
            // btnMoveWall
            // 
            this.btnMoveWall.Location = new System.Drawing.Point(20, 48);
            this.btnMoveWall.Name = "btnMoveWall";
            this.btnMoveWall.Size = new System.Drawing.Size(75, 23);
            this.btnMoveWall.TabIndex = 1;
            this.btnMoveWall.Text = "&Move Wall";
            this.btnMoveWall.UseVisualStyleBackColor = true;
            this.btnMoveWall.Click += new System.EventHandler(this.moveWallButton_Click);
            // 
            // btnCreateWall
            // 
            this.btnCreateWall.Location = new System.Drawing.Point(20, 19);
            this.btnCreateWall.Name = "btnCreateWall";
            this.btnCreateWall.Size = new System.Drawing.Size(75, 23);
            this.btnCreateWall.TabIndex = 0;
            this.btnCreateWall.Text = "C&reate Wall";
            this.btnCreateWall.UseVisualStyleBackColor = true;
            this.btnCreateWall.Click += new System.EventHandler(this.createWallbutton_Click);
            // 
            // transactionsTreeView
            // 
            this.transactionsTreeView.Location = new System.Drawing.Point(136, 11);
            this.transactionsTreeView.Name = "transactionsTreeView";
            this.transactionsTreeView.Size = new System.Drawing.Size(282, 347);
            this.transactionsTreeView.TabIndex = 11;
            // 
            // btnRollbackTransGroup
            // 
            this.btnRollbackTransGroup.Location = new System.Drawing.Point(20, 49);
            this.btnRollbackTransGroup.Name = "btnRollbackTransGroup";
            this.btnRollbackTransGroup.Size = new System.Drawing.Size(75, 23);
            this.btnRollbackTransGroup.TabIndex = 0;
            this.btnRollbackTransGroup.Text = "&Rollback";
            this.btnRollbackTransGroup.UseVisualStyleBackColor = true;
            this.btnRollbackTransGroup.Click += new System.EventHandler(this.btnRollbackTransGroup_Click);
            // 
            // groupBoxTransGroup
            // 
            this.groupBoxTransGroup.Controls.Add(this.btnCommitTransGroup);
            this.groupBoxTransGroup.Controls.Add(this.btnStartTransGroup);
            this.groupBoxTransGroup.Controls.Add(this.btnRollbackTransGroup);
            this.groupBoxTransGroup.Location = new System.Drawing.Point(13, 11);
            this.groupBoxTransGroup.Name = "groupBoxTransGroup";
            this.groupBoxTransGroup.Size = new System.Drawing.Size(115, 112);
            this.groupBoxTransGroup.TabIndex = 12;
            this.groupBoxTransGroup.TabStop = false;
            this.groupBoxTransGroup.Text = "Transaction Group";
            // 
            // btnCommitTransGroup
            // 
            this.btnCommitTransGroup.Location = new System.Drawing.Point(20, 78);
            this.btnCommitTransGroup.Name = "btnCommitTransGroup";
            this.btnCommitTransGroup.Size = new System.Drawing.Size(75, 23);
            this.btnCommitTransGroup.TabIndex = 2;
            this.btnCommitTransGroup.Text = "&Commit";
            this.btnCommitTransGroup.UseVisualStyleBackColor = true;
            this.btnCommitTransGroup.Click += new System.EventHandler(this.btnCommitTransGroup_Click);
            // 
            // btnStartTransGroup
            // 
            this.btnStartTransGroup.Location = new System.Drawing.Point(20, 20);
            this.btnStartTransGroup.Name = "btnStartTransGroup";
            this.btnStartTransGroup.Size = new System.Drawing.Size(75, 23);
            this.btnStartTransGroup.TabIndex = 1;
            this.btnStartTransGroup.Text = "&Start";
            this.btnStartTransGroup.UseVisualStyleBackColor = true;
            this.btnStartTransGroup.Click += new System.EventHandler(this.btnStartTransGroup_Click);
            // 
            // TransactionForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(431, 413);
            this.Controls.Add(this.groupBoxTransGroup);
            this.Controls.Add(this.transactionsTreeView);
            this.Controls.Add(this.groupBoxOperation);
            this.Controls.Add(this.groupBoxTrans);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TransactionForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Transaction";
            this.groupBoxTrans.ResumeLayout(false);
            this.groupBoxOperation.ResumeLayout(false);
            this.groupBoxTransGroup.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBoxTrans;
        private System.Windows.Forms.Button btnRollbackTrans;
        private System.Windows.Forms.Button btnCommitTrans;
        private System.Windows.Forms.Button btnStartTrans;
        private System.Windows.Forms.GroupBox groupBoxOperation;
        private System.Windows.Forms.Button btnDeleteWall;
        private System.Windows.Forms.Button btnMoveWall;
        private System.Windows.Forms.Button btnCreateWall;
        private System.Windows.Forms.TreeView transactionsTreeView;
        private System.Windows.Forms.Button btnRollbackTransGroup;
        private System.Windows.Forms.GroupBox groupBoxTransGroup;
        private System.Windows.Forms.Button btnCommitTransGroup;
        private System.Windows.Forms.Button btnStartTransGroup;
    }
}