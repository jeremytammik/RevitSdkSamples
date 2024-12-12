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

namespace Revit.SDK.Samples.NewHostedSweep.CS
{
    partial class MainForm
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
            this.buttonCreate = new System.Windows.Forms.Button();
            this.buttonModify = new System.Windows.Forms.Button();
            this.listBoxCreatedHostedSweeps = new System.Windows.Forms.ListBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.comboBoxHostedSweepType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonCreate
            // 
            this.buttonCreate.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonCreate.Location = new System.Drawing.Point(12, 295);
            this.buttonCreate.Name = "buttonCreate";
            this.buttonCreate.Size = new System.Drawing.Size(68, 23);
            this.buttonCreate.TabIndex = 1;
            this.buttonCreate.Text = "&Create...";
            this.buttonCreate.UseVisualStyleBackColor = true;
            this.buttonCreate.Click += new System.EventHandler(this.buttonCreate_Click);
            // 
            // buttonModify
            // 
            this.buttonModify.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonModify.Enabled = false;
            this.buttonModify.Location = new System.Drawing.Point(86, 295);
            this.buttonModify.Name = "buttonModify";
            this.buttonModify.Size = new System.Drawing.Size(68, 23);
            this.buttonModify.TabIndex = 3;
            this.buttonModify.Text = "&Modify...";
            this.buttonModify.UseVisualStyleBackColor = true;
            this.buttonModify.Click += new System.EventHandler(this.buttonModify_Click);
            // 
            // listBoxCreatedHostedSweeps
            // 
            this.listBoxCreatedHostedSweeps.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxCreatedHostedSweeps.FormattingEnabled = true;
            this.listBoxCreatedHostedSweeps.Location = new System.Drawing.Point(12, 87);
            this.listBoxCreatedHostedSweeps.Name = "listBoxCreatedHostedSweeps";
            this.listBoxCreatedHostedSweeps.Size = new System.Drawing.Size(216, 199);
            this.listBoxCreatedHostedSweeps.TabIndex = 4;
            this.listBoxCreatedHostedSweeps.SelectedValueChanged += new System.EventHandler(this.listBoxHostedSweeps_SelectedValueChanged);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonOK.Location = new System.Drawing.Point(160, 295);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(68, 23);
            this.buttonOK.TabIndex = 5;
            this.buttonOK.Text = "&OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // comboBoxHostedSweepType
            // 
            this.comboBoxHostedSweepType.FormattingEnabled = true;
            this.comboBoxHostedSweepType.Location = new System.Drawing.Point(12, 31);
            this.comboBoxHostedSweepType.Name = "comboBoxHostedSweepType";
            this.comboBoxHostedSweepType.Size = new System.Drawing.Size(216, 21);
            this.comboBoxHostedSweepType.TabIndex = 6;
            this.comboBoxHostedSweepType.SelectedIndexChanged += new System.EventHandler(this.comboBoxHostedSweepType_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(141, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Select a hosted sweep type:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(202, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Select a created hosted sweep to modify:";
            // 
            // MainForm
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonOK;
            this.ClientSize = new System.Drawing.Size(240, 330);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBoxCreatedHostedSweeps);
            this.Controls.Add(this.comboBoxHostedSweepType);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonModify);
            this.Controls.Add(this.buttonCreate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Hosted Sweep";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonCreate;
        private System.Windows.Forms.Button buttonModify;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.ListBox listBoxCreatedHostedSweeps;
        private System.Windows.Forms.ComboBox comboBoxHostedSweepType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;

    }
}