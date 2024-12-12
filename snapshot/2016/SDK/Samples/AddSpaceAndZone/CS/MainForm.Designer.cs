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

namespace Revit.SDK.Samples.AddSpaceAndZone.CS
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.levelComboBox = new System.Windows.Forms.ComboBox();
            this.createSpacesButton = new System.Windows.Forms.Button();
            this.spacesListView = new System.Windows.Forms.ListView();
            this.spaceColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.zoneColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.spaceTabPage = new System.Windows.Forms.TabPage();
            this.zoneTabPage = new System.Windows.Forms.TabPage();
            this.createZoneButton = new System.Windows.Forms.Button();
            this.editZoneButton = new System.Windows.Forms.Button();
            this.zonesTreeView = new System.Windows.Forms.TreeView();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl.SuspendLayout();
            this.spaceTabPage.SuspendLayout();
            this.zoneTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(172, 433);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 0;
            this.okButton.Text = "&Ok";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(287, 433);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // levelComboBox
            // 
            this.levelComboBox.FormattingEnabled = true;
            this.levelComboBox.Location = new System.Drawing.Point(82, 29);
            this.levelComboBox.Name = "levelComboBox";
            this.levelComboBox.Size = new System.Drawing.Size(286, 21);
            this.levelComboBox.TabIndex = 2;
            this.levelComboBox.SelectedIndexChanged += new System.EventHandler(this.levelComboBox_SelectedIndexChanged);
            // 
            // createSpacesButton
            // 
            this.createSpacesButton.Location = new System.Drawing.Point(34, 316);
            this.createSpacesButton.Name = "createSpacesButton";
            this.createSpacesButton.Size = new System.Drawing.Size(281, 23);
            this.createSpacesButton.TabIndex = 3;
            this.createSpacesButton.Text = "Create Spaces";
            this.createSpacesButton.UseVisualStyleBackColor = true;
            this.createSpacesButton.Click += new System.EventHandler(this.createSpacesButton_Click);
            // 
            // spacesListView
            // 
            this.spacesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.spaceColumnHeader,
            this.zoneColumnHeader});
            this.spacesListView.Location = new System.Drawing.Point(3, 6);
            this.spacesListView.Name = "spacesListView";
            this.spacesListView.Size = new System.Drawing.Size(343, 304);
            this.spacesListView.TabIndex = 4;
            this.spacesListView.UseCompatibleStateImageBehavior = false;
            this.spacesListView.View = System.Windows.Forms.View.Details;
            // 
            // spaceColumnHeader
            // 
            this.spaceColumnHeader.Text = "Space";
            // 
            // zoneColumnHeader
            // 
            this.zoneColumnHeader.Text = "Zone";
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.spaceTabPage);
            this.tabControl.Controls.Add(this.zoneTabPage);
            this.tabControl.Location = new System.Drawing.Point(12, 54);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(360, 373);
            this.tabControl.TabIndex = 5;
            // 
            // spaceTabPage
            // 
            this.spaceTabPage.Controls.Add(this.spacesListView);
            this.spaceTabPage.Controls.Add(this.createSpacesButton);
            this.spaceTabPage.Location = new System.Drawing.Point(4, 23);
            this.spaceTabPage.Name = "spaceTabPage";
            this.spaceTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.spaceTabPage.Size = new System.Drawing.Size(352, 346);
            this.spaceTabPage.TabIndex = 0;
            this.spaceTabPage.Text = "Spaces";
            this.spaceTabPage.UseVisualStyleBackColor = true;
            // 
            // zoneTabPage
            // 
            this.zoneTabPage.Controls.Add(this.createZoneButton);
            this.zoneTabPage.Controls.Add(this.editZoneButton);
            this.zoneTabPage.Controls.Add(this.zonesTreeView);
            this.zoneTabPage.Location = new System.Drawing.Point(4, 23);
            this.zoneTabPage.Name = "zoneTabPage";
            this.zoneTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.zoneTabPage.Size = new System.Drawing.Size(352, 346);
            this.zoneTabPage.TabIndex = 1;
            this.zoneTabPage.Text = "Zones";
            this.zoneTabPage.UseVisualStyleBackColor = true;
            // 
            // createZoneButton
            // 
            this.createZoneButton.Location = new System.Drawing.Point(40, 317);
            this.createZoneButton.Name = "createZoneButton";
            this.createZoneButton.Size = new System.Drawing.Size(115, 23);
            this.createZoneButton.TabIndex = 2;
            this.createZoneButton.Text = "Create Zone";
            this.createZoneButton.UseVisualStyleBackColor = true;
            this.createZoneButton.Click += new System.EventHandler(this.createZoneButton_Click);
            // 
            // editZoneButton
            // 
            this.editZoneButton.Location = new System.Drawing.Point(195, 317);
            this.editZoneButton.Name = "editZoneButton";
            this.editZoneButton.Size = new System.Drawing.Size(141, 23);
            this.editZoneButton.TabIndex = 1;
            this.editZoneButton.Text = "Edit Zone";
            this.editZoneButton.UseVisualStyleBackColor = true;
            this.editZoneButton.Click += new System.EventHandler(this.editZoneButton_Click);
            // 
            // zonesTreeView
            // 
            this.zonesTreeView.Location = new System.Drawing.Point(3, 6);
            this.zonesTreeView.Name = "zonesTreeView";
            this.zonesTreeView.ShowNodeToolTips = true;
            this.zonesTreeView.Size = new System.Drawing.Size(343, 296);
            this.zonesTreeView.TabIndex = 0;
            this.zonesTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.zonesTreeView_AfterSelect);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(-2, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 23);
            this.label1.TabIndex = 6;
            this.label1.Text = "Level :";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MainForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(382, 462);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.levelComboBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AddSpaceAndZone";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tabControl.ResumeLayout(false);
            this.spaceTabPage.ResumeLayout(false);
            this.zoneTabPage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.ComboBox levelComboBox;
        private System.Windows.Forms.Button createSpacesButton;
        private System.Windows.Forms.ListView spacesListView;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage spaceTabPage;
        private System.Windows.Forms.TabPage zoneTabPage;
        private System.Windows.Forms.TreeView zonesTreeView;
        private System.Windows.Forms.Button editZoneButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button createZoneButton;
        private System.Windows.Forms.ColumnHeader spaceColumnHeader;
        private System.Windows.Forms.ColumnHeader zoneColumnHeader;
    }
}