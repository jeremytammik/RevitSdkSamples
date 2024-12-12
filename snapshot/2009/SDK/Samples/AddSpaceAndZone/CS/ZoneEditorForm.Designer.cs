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

namespace Revit.SDK.Samples.AddSpaceAndZone.CS
{
    partial class ZoneEditorForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private Autodesk.Revit.Elements.Zone m_zone;
        private ZoneNode m_zoneNode;
        private DataManager m_dataManager;
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
            this.availableSpacesListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.currentSpacesListView = new System.Windows.Forms.ListView();
            this.addSpaceButton = new System.Windows.Forms.Button();
            this.removeSpaceButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // availableSpacesListView
            // 
            this.availableSpacesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.availableSpacesListView.Location = new System.Drawing.Point(12, 40);
            this.availableSpacesListView.Name = "availableSpacesListView";
            this.availableSpacesListView.Size = new System.Drawing.Size(135, 167);
            this.availableSpacesListView.TabIndex = 0;
            this.availableSpacesListView.UseCompatibleStateImageBehavior = false;
            this.availableSpacesListView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Space";
            this.columnHeader1.Width = 63;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Zone";
            this.columnHeader2.Width = 68;
            // 
            // currentSpacesListView
            // 
            this.currentSpacesListView.Location = new System.Drawing.Point(247, 40);
            this.currentSpacesListView.Name = "currentSpacesListView";
            this.currentSpacesListView.Size = new System.Drawing.Size(135, 167);
            this.currentSpacesListView.TabIndex = 1;
            this.currentSpacesListView.UseCompatibleStateImageBehavior = false;
            this.currentSpacesListView.View = System.Windows.Forms.View.List;
            // 
            // addSpaceButton
            // 
            this.addSpaceButton.Location = new System.Drawing.Point(166, 83);
            this.addSpaceButton.Name = "addSpaceButton";
            this.addSpaceButton.Size = new System.Drawing.Size(75, 23);
            this.addSpaceButton.TabIndex = 2;
            this.addSpaceButton.Text = "Add";
            this.addSpaceButton.UseVisualStyleBackColor = true;
            this.addSpaceButton.Click += new System.EventHandler(this.addSpaceButton_Click);
            // 
            // removeSpaceButton
            // 
            this.removeSpaceButton.Location = new System.Drawing.Point(166, 129);
            this.removeSpaceButton.Name = "removeSpaceButton";
            this.removeSpaceButton.Size = new System.Drawing.Size(75, 23);
            this.removeSpaceButton.TabIndex = 3;
            this.removeSpaceButton.Text = "&Remove";
            this.removeSpaceButton.UseVisualStyleBackColor = true;
            this.removeSpaceButton.Click += new System.EventHandler(this.removeSpaceButton_Click);
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(166, 224);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 4;
            this.okButton.Text = "&Ok";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Available Spaces:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(244, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(114, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Currrent Zone Spaces:";
            // 
            // ZoneEditorForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(394, 259);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.removeSpaceButton);
            this.Controls.Add(this.addSpaceButton);
            this.Controls.Add(this.currentSpacesListView);
            this.Controls.Add(this.availableSpacesListView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ZoneEditorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ZoneEditorForm";
            this.Load += new System.EventHandler(this.ZoneEditorForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView availableSpacesListView;
        private System.Windows.Forms.ListView currentSpacesListView;
        private System.Windows.Forms.Button addSpaceButton;
        private System.Windows.Forms.Button removeSpaceButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
    }
}