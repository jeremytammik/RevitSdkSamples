//
// (C) Copyright 2003-2015 by Autodesk, Inc.
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

namespace Revit.SDK.Samples.ChangesMonitor.CS
{
    partial class ChangesInformationForm
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
            this.changesdataGridView = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.changesdataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // changesdataGridView
            // 
            this.changesdataGridView.AllowUserToAddRows = false;
            this.changesdataGridView.AllowUserToDeleteRows = false;
            this.changesdataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.changesdataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.changesdataGridView.Location = new System.Drawing.Point(-1, -2);
            this.changesdataGridView.Name = "changesdataGridView";
            this.changesdataGridView.ReadOnly = true;
            this.changesdataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.changesdataGridView.Size = new System.Drawing.Size(665, 81);
            this.changesdataGridView.TabIndex = 0;
            this.changesdataGridView.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.changesdataGridView_RowsAdded);
            // 
            // ChangesInformationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(664, 77);
            this.Controls.Add(this.changesdataGridView);
            this.MaximizeBox = false;
            this.Name = "ChangesInformationForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Changes Information";
            this.Shown += new System.EventHandler(this.ChangesInfoForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.changesdataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView changesdataGridView;
    }
}