//
// (C) Copyright 2003-2008 by Autodesk, Inc.
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

namespace Revit.SDK.Samples.ImportExport.CS
{
    /// <summary>
    /// Provide a dialog which lets users choose views to export.
    /// </summary>
    partial class SelectViewsForm
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
            this.groupBoxShow = new System.Windows.Forms.GroupBox();
            this.checkBoxViews = new System.Windows.Forms.CheckBox();
            this.checkBoxSheets = new System.Windows.Forms.CheckBox();
            this.buttonCheckAll = new System.Windows.Forms.Button();
            this.buttonCheckNone = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.checkedListBoxViews = new System.Windows.Forms.CheckedListBox();
            this.groupBoxShow.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxShow
            // 
            this.groupBoxShow.Controls.Add(this.checkBoxViews);
            this.groupBoxShow.Controls.Add(this.checkBoxSheets);
            this.groupBoxShow.Location = new System.Drawing.Point(12, 288);
            this.groupBoxShow.Name = "groupBoxShow";
            this.groupBoxShow.Size = new System.Drawing.Size(311, 41);
            this.groupBoxShow.TabIndex = 1;
            this.groupBoxShow.TabStop = false;
            this.groupBoxShow.Text = "Show";
            // 
            // checkBoxViews
            // 
            this.checkBoxViews.AutoSize = true;
            this.checkBoxViews.Checked = true;
            this.checkBoxViews.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxViews.Location = new System.Drawing.Point(148, 18);
            this.checkBoxViews.Name = "checkBoxViews";
            this.checkBoxViews.Size = new System.Drawing.Size(54, 17);
            this.checkBoxViews.TabIndex = 1;
            this.checkBoxViews.Text = "Views";
            this.checkBoxViews.UseVisualStyleBackColor = true;
            this.checkBoxViews.CheckedChanged += new System.EventHandler(this.checkBoxViews_CheckedChanged);
            // 
            // checkBoxSheets
            // 
            this.checkBoxSheets.AutoSize = true;
            this.checkBoxSheets.Checked = true;
            this.checkBoxSheets.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxSheets.Location = new System.Drawing.Point(7, 19);
            this.checkBoxSheets.Name = "checkBoxSheets";
            this.checkBoxSheets.Size = new System.Drawing.Size(59, 17);
            this.checkBoxSheets.TabIndex = 0;
            this.checkBoxSheets.Text = "Sheets";
            this.checkBoxSheets.UseVisualStyleBackColor = true;
            this.checkBoxSheets.CheckedChanged += new System.EventHandler(this.checkBoxSheets_CheckedChanged);
            // 
            // buttonCheckAll
            // 
            this.buttonCheckAll.Location = new System.Drawing.Point(333, 24);
            this.buttonCheckAll.Name = "buttonCheckAll";
            this.buttonCheckAll.Size = new System.Drawing.Size(85, 23);
            this.buttonCheckAll.TabIndex = 0;
            this.buttonCheckAll.Text = "Check &All";
            this.buttonCheckAll.UseVisualStyleBackColor = true;
            this.buttonCheckAll.Click += new System.EventHandler(this.buttonCheckAll_Click);
            // 
            // buttonCheckNone
            // 
            this.buttonCheckNone.Location = new System.Drawing.Point(333, 65);
            this.buttonCheckNone.Name = "buttonCheckNone";
            this.buttonCheckNone.Size = new System.Drawing.Size(85, 23);
            this.buttonCheckNone.TabIndex = 1;
            this.buttonCheckNone.Text = "Check &None";
            this.buttonCheckNone.UseVisualStyleBackColor = true;
            this.buttonCheckNone.Click += new System.EventHandler(this.buttonCheckNone_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(238, 361);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(85, 23);
            this.buttonOK.TabIndex = 2;
            this.buttonOK.Text = "&OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(333, 361);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(85, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "&Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // checkedListBoxViews
            // 
            this.checkedListBoxViews.CheckOnClick = true;
            this.checkedListBoxViews.FormattingEnabled = true;
            this.checkedListBoxViews.Location = new System.Drawing.Point(12, 12);
            this.checkedListBoxViews.Name = "checkedListBoxViews";
            this.checkedListBoxViews.Size = new System.Drawing.Size(311, 274);
            this.checkedListBoxViews.TabIndex = 3;
            // 
            // SelectViewsForm
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(432, 394);
            this.Controls.Add(this.checkedListBoxViews);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCheckNone);
            this.Controls.Add(this.buttonCheckAll);
            this.Controls.Add(this.groupBoxShow);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectViewsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "View/Sheet Set";
            this.groupBoxShow.ResumeLayout(false);
            this.groupBoxShow.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxShow;
        private System.Windows.Forms.CheckBox checkBoxViews;
        private System.Windows.Forms.CheckBox checkBoxSheets;
        private System.Windows.Forms.Button buttonCheckAll;
        private System.Windows.Forms.Button buttonCheckNone;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.CheckedListBox checkedListBoxViews;
    }
}