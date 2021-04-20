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

namespace Revit.SDK.Samples.NewRoof.RoofForms.CS
{
    partial class RoofForm
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
            this.footPrintRoofsListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.roofsTabControl = new System.Windows.Forms.TabControl();
            this.footprintRoofTabPage = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.roofTypesComboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.levelsComboBox = new System.Windows.Forms.ComboBox();
            this.levelLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.selectFootPrintButton = new System.Windows.Forms.Button();
            this.extrusionRoofTabPage = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.extrusionEndTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.extrusionStartTextBox = new System.Windows.Forms.TextBox();
            this.refPanesComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.extrusionRoofTypesComboBox = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.refLevelComboBox = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.selectProfileButton = new System.Windows.Forms.Button();
            this.extrusionRoofsListView = new System.Windows.Forms.ListView();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader7 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader8 = new System.Windows.Forms.ColumnHeader();
            this.editRoofButton = new System.Windows.Forms.Button();
            this.createRoofButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.roofsTabControl.SuspendLayout();
            this.footprintRoofTabPage.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.extrusionRoofTabPage.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // footPrintRoofsListView
            // 
            this.footPrintRoofsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.footPrintRoofsListView.Location = new System.Drawing.Point(3, 6);
            this.footPrintRoofsListView.Name = "footPrintRoofsListView";
            this.footPrintRoofsListView.Size = new System.Drawing.Size(368, 231);
            this.footPrintRoofsListView.TabIndex = 2;
            this.footPrintRoofsListView.UseCompatibleStateImageBehavior = false;
            this.footPrintRoofsListView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Roof Id";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Name";
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Base Level";
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Roof Type";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.roofsTabControl);
            this.groupBox1.Controls.Add(this.editRoofButton);
            this.groupBox1.Controls.Add(this.createRoofButton);
            this.groupBox1.Location = new System.Drawing.Point(2, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(395, 447);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Roofs";
            // 
            // roofsTabControl
            // 
            this.roofsTabControl.Controls.Add(this.footprintRoofTabPage);
            this.roofsTabControl.Controls.Add(this.extrusionRoofTabPage);
            this.roofsTabControl.Location = new System.Drawing.Point(6, 19);
            this.roofsTabControl.Name = "roofsTabControl";
            this.roofsTabControl.SelectedIndex = 0;
            this.roofsTabControl.Size = new System.Drawing.Size(382, 383);
            this.roofsTabControl.TabIndex = 5;
            // 
            // footprintRoofTabPage
            // 
            this.footprintRoofTabPage.Controls.Add(this.groupBox2);
            this.footprintRoofTabPage.Controls.Add(this.footPrintRoofsListView);
            this.footprintRoofTabPage.Location = new System.Drawing.Point(4, 22);
            this.footprintRoofTabPage.Name = "footprintRoofTabPage";
            this.footprintRoofTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.footprintRoofTabPage.Size = new System.Drawing.Size(374, 357);
            this.footprintRoofTabPage.TabIndex = 0;
            this.footprintRoofTabPage.Text = "Footprint Roofs";
            this.footprintRoofTabPage.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.roofTypesComboBox);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.levelsComboBox);
            this.groupBox2.Controls.Add(this.levelLabel);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.selectFootPrintButton);
            this.groupBox2.Location = new System.Drawing.Point(6, 243);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(365, 108);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Create FootPrintRoof";
            // 
            // roofTypesComboBox
            // 
            this.roofTypesComboBox.FormattingEnabled = true;
            this.roofTypesComboBox.Location = new System.Drawing.Point(64, 73);
            this.roofTypesComboBox.Name = "roofTypesComboBox";
            this.roofTypesComboBox.Size = new System.Drawing.Size(295, 21);
            this.roofTypesComboBox.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Roof Type:";
            // 
            // levelsComboBox
            // 
            this.levelsComboBox.FormattingEnabled = true;
            this.levelsComboBox.Location = new System.Drawing.Point(64, 46);
            this.levelsComboBox.Name = "levelsComboBox";
            this.levelsComboBox.Size = new System.Drawing.Size(295, 21);
            this.levelsComboBox.TabIndex = 4;
            // 
            // levelLabel
            // 
            this.levelLabel.AutoSize = true;
            this.levelLabel.Location = new System.Drawing.Point(6, 49);
            this.levelLabel.Name = "levelLabel";
            this.levelLabel.Size = new System.Drawing.Size(36, 13);
            this.levelLabel.TabIndex = 3;
            this.levelLabel.Text = "Level:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Footprint:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // selectFootPrintButton
            // 
            this.selectFootPrintButton.DialogResult = System.Windows.Forms.DialogResult.Retry;
            this.selectFootPrintButton.Location = new System.Drawing.Point(64, 17);
            this.selectFootPrintButton.Name = "selectFootPrintButton";
            this.selectFootPrintButton.Size = new System.Drawing.Size(295, 23);
            this.selectFootPrintButton.TabIndex = 1;
            this.selectFootPrintButton.Text = "&Select Footprint in Revit";
            this.selectFootPrintButton.UseVisualStyleBackColor = true;
            // 
            // extrusionRoofTabPage
            // 
            this.extrusionRoofTabPage.Controls.Add(this.groupBox3);
            this.extrusionRoofTabPage.Controls.Add(this.extrusionRoofsListView);
            this.extrusionRoofTabPage.Location = new System.Drawing.Point(4, 22);
            this.extrusionRoofTabPage.Name = "extrusionRoofTabPage";
            this.extrusionRoofTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.extrusionRoofTabPage.Size = new System.Drawing.Size(374, 357);
            this.extrusionRoofTabPage.TabIndex = 1;
            this.extrusionRoofTabPage.Text = "Extrusion Roofs";
            this.extrusionRoofTabPage.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.extrusionEndTextBox);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.extrusionStartTextBox);
            this.groupBox3.Controls.Add(this.refPanesComboBox);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.extrusionRoofTypesComboBox);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.refLevelComboBox);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.selectProfileButton);
            this.groupBox3.Location = new System.Drawing.Point(3, 207);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(368, 154);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Create ExtrusionRoof";
            // 
            // extrusionEndTextBox
            // 
            this.extrusionEndTextBox.Location = new System.Drawing.Point(285, 123);
            this.extrusionEndTextBox.Name = "extrusionEndTextBox";
            this.extrusionEndTextBox.Size = new System.Drawing.Size(77, 20);
            this.extrusionEndTextBox.TabIndex = 12;
            this.extrusionEndTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.extrusionEndTextBox_Validating);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(207, 126);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Extrusion End:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 126);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Extrusion Start:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // extrusionStartTextBox
            // 
            this.extrusionStartTextBox.Location = new System.Drawing.Point(84, 124);
            this.extrusionStartTextBox.Name = "extrusionStartTextBox";
            this.extrusionStartTextBox.Size = new System.Drawing.Size(74, 20);
            this.extrusionStartTextBox.TabIndex = 9;
            this.extrusionStartTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.extrusionStartTextBox_Validating);
            // 
            // refPanesComboBox
            // 
            this.refPanesComboBox.FormattingEnabled = true;
            this.refPanesComboBox.Location = new System.Drawing.Point(84, 45);
            this.refPanesComboBox.Name = "refPanesComboBox";
            this.refPanesComboBox.Size = new System.Drawing.Size(278, 21);
            this.refPanesComboBox.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Ref Plane:";
            // 
            // extrusionRoofTypesComboBox
            // 
            this.extrusionRoofTypesComboBox.FormattingEnabled = true;
            this.extrusionRoofTypesComboBox.Location = new System.Drawing.Point(84, 97);
            this.extrusionRoofTypesComboBox.Name = "extrusionRoofTypesComboBox";
            this.extrusionRoofTypesComboBox.Size = new System.Drawing.Size(278, 21);
            this.extrusionRoofTypesComboBox.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 100);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Roof Type:";
            // 
            // refLevelComboBox
            // 
            this.refLevelComboBox.FormattingEnabled = true;
            this.refLevelComboBox.Location = new System.Drawing.Point(84, 71);
            this.refLevelComboBox.Name = "refLevelComboBox";
            this.refLevelComboBox.Size = new System.Drawing.Size(278, 21);
            this.refLevelComboBox.TabIndex = 4;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 74);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(56, 13);
            this.label7.TabIndex = 3;
            this.label7.Text = "Ref Level:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 22);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(39, 13);
            this.label8.TabIndex = 2;
            this.label8.Text = "Profile:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // selectProfileButton
            // 
            this.selectProfileButton.DialogResult = System.Windows.Forms.DialogResult.Retry;
            this.selectProfileButton.Location = new System.Drawing.Point(84, 17);
            this.selectProfileButton.Name = "selectProfileButton";
            this.selectProfileButton.Size = new System.Drawing.Size(263, 23);
            this.selectProfileButton.TabIndex = 1;
            this.selectProfileButton.Text = "&Select Profile in Revit";
            this.selectProfileButton.UseVisualStyleBackColor = true;
            // 
            // extrusionRoofsListView
            // 
            this.extrusionRoofsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8});
            this.extrusionRoofsListView.Location = new System.Drawing.Point(3, 6);
            this.extrusionRoofsListView.Name = "extrusionRoofsListView";
            this.extrusionRoofsListView.Size = new System.Drawing.Size(368, 197);
            this.extrusionRoofsListView.TabIndex = 3;
            this.extrusionRoofsListView.UseCompatibleStateImageBehavior = false;
            this.extrusionRoofsListView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Roof Id";
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Name";
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Reference Level";
            this.columnHeader7.Width = 102;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Roof Type";
            // 
            // editRoofButton
            // 
            this.editRoofButton.Location = new System.Drawing.Point(207, 408);
            this.editRoofButton.Name = "editRoofButton";
            this.editRoofButton.Size = new System.Drawing.Size(181, 23);
            this.editRoofButton.TabIndex = 4;
            this.editRoofButton.Text = "Edit";
            this.editRoofButton.UseVisualStyleBackColor = true;
            this.editRoofButton.Click += new System.EventHandler(this.editRoofButton_Click);
            // 
            // createRoofButton
            // 
            this.createRoofButton.Location = new System.Drawing.Point(6, 408);
            this.createRoofButton.Name = "createRoofButton";
            this.createRoofButton.Size = new System.Drawing.Size(181, 23);
            this.createRoofButton.TabIndex = 3;
            this.createRoofButton.Text = "Create";
            this.createRoofButton.UseVisualStyleBackColor = true;
            this.createRoofButton.Click += new System.EventHandler(this.createRoofButton_Click);
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(240, 455);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 4;
            this.okButton.Text = "&OK";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(322, 455);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // RoofForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(401, 487);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RoofForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "New Roof";
            this.Load += new System.EventHandler(this.RoofForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.roofsTabControl.ResumeLayout(false);
            this.footprintRoofTabPage.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.extrusionRoofTabPage.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView footPrintRoofsListView;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button editRoofButton;
        private System.Windows.Forms.Button createRoofButton;
        private System.Windows.Forms.TabControl roofsTabControl;
        private System.Windows.Forms.TabPage footprintRoofTabPage;
        private System.Windows.Forms.TabPage extrusionRoofTabPage;
        private System.Windows.Forms.ListView extrusionRoofsListView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox roofTypesComboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox levelsComboBox;
        private System.Windows.Forms.Label levelLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button selectFootPrintButton;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox extrusionEndTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox extrusionStartTextBox;
        private System.Windows.Forms.ComboBox refPanesComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox extrusionRoofTypesComboBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox refLevelComboBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button selectProfileButton;
    }
}