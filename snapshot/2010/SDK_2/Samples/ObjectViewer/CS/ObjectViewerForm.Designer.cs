//
// (C) Copyright 2003-2009 by Autodesk, Inc.
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

namespace Revit.SDK.Samples.ObjectViewer.CS
{
    partial class ObjectViewerForm
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
            this.parametersDataGridView = new System.Windows.Forms.DataGridView();
            this.physicalModelRadioButton = new System.Windows.Forms.RadioButton();
            this.analyticalModelRadioButton = new System.Windows.Forms.RadioButton();
            this.previewBox = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.viewDirectionComboBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.viewListBox = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.detailLevelComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.okButton = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.parametersDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.previewBox)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            //
            // parametersDataGridView
            //
            this.parametersDataGridView.AllowUserToAddRows = false;
            this.parametersDataGridView.AllowUserToDeleteRows = false;
            this.parametersDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.parametersDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.parametersDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.parametersDataGridView.Location = new System.Drawing.Point(490, 25);
            this.parametersDataGridView.Name = "parametersDataGridView";
            this.parametersDataGridView.RowHeadersVisible = false;
            this.parametersDataGridView.RowTemplate.Height = 24;
            this.parametersDataGridView.Size = new System.Drawing.Size(404, 567);
            this.parametersDataGridView.TabIndex = 10;
            this.parametersDataGridView.TabStop = false;
            //
            // physicalModelRadioButton
            //
            this.physicalModelRadioButton.AutoSize = true;
            this.physicalModelRadioButton.Checked = true;
            this.physicalModelRadioButton.Location = new System.Drawing.Point(6, 110);
            this.physicalModelRadioButton.Name = "physicalModelRadioButton";
            this.physicalModelRadioButton.Size = new System.Drawing.Size(96, 17);
            this.physicalModelRadioButton.TabIndex = 8;
            this.physicalModelRadioButton.TabStop = true;
            this.physicalModelRadioButton.Text = "Physical Model";
            this.physicalModelRadioButton.UseVisualStyleBackColor = true;
            this.physicalModelRadioButton.CheckedChanged += new System.EventHandler(this.physicalModelRadioButton_CheckedChanged);
            //
            // analyticalModelRadioButton
            //
            this.analyticalModelRadioButton.AutoSize = true;
            this.analyticalModelRadioButton.Location = new System.Drawing.Point(6, 133);
            this.analyticalModelRadioButton.Name = "analyticalModelRadioButton";
            this.analyticalModelRadioButton.Size = new System.Drawing.Size(102, 17);
            this.analyticalModelRadioButton.TabIndex = 9;
            this.analyticalModelRadioButton.TabStop = true;
            this.analyticalModelRadioButton.Text = "Analytical Model";
            this.analyticalModelRadioButton.UseVisualStyleBackColor = true;
            this.analyticalModelRadioButton.CheckedChanged += new System.EventHandler(this.analyticalModelRadioButton_CheckedChanged);
            //
            // previewBox
            //
            this.previewBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.previewBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.previewBox.Cursor = System.Windows.Forms.Cursors.Cross;
            this.previewBox.Location = new System.Drawing.Point(12, 25);
            this.previewBox.Name = "previewBox";
            this.previewBox.Size = new System.Drawing.Size(472, 425);
            this.previewBox.TabIndex = 4;
            this.previewBox.TabStop = false;
            this.previewBox.Paint += new System.Windows.Forms.PaintEventHandler(this.previewBox_Paint);
            //
            // groupBox1
            //
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.viewDirectionComboBox);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.viewListBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.detailLevelComboBox);
            this.groupBox1.Controls.Add(this.physicalModelRadioButton);
            this.groupBox1.Controls.Add(this.analyticalModelRadioButton);
            this.groupBox1.Location = new System.Drawing.Point(12, 456);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(472, 160);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Preview";
            //
            // label5
            //
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 65);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(78, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "View Direction:";
            //
            // viewDirectionComboBox
            //
            this.viewDirectionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.viewDirectionComboBox.FormattingEnabled = true;
            this.viewDirectionComboBox.Items.AddRange(new object[] {
            "Top",
            "Front",
            "Left",
            "Right",
            "Bottom",
            "Back",
            "IsoMetric"});
            this.viewDirectionComboBox.Location = new System.Drawing.Point(6, 82);
            this.viewDirectionComboBox.Name = "viewDirectionComboBox";
            this.viewDirectionComboBox.Size = new System.Drawing.Size(158, 21);
            this.viewDirectionComboBox.TabIndex = 6;
            this.viewDirectionComboBox.SelectedIndexChanged += new System.EventHandler(this.viewDirectionComboBox_SelectedIndexChanged);
            //
            // label4
            //
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(167, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(82, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Displayed View:";
            //
            // viewListBox
            //
            this.viewListBox.FormattingEnabled = true;
            this.viewListBox.Location = new System.Drawing.Point(170, 33);
            this.viewListBox.Name = "viewListBox";
            this.viewListBox.Size = new System.Drawing.Size(296, 121);
            this.viewListBox.TabIndex = 7;
            this.viewListBox.SelectedIndexChanged += new System.EventHandler(this.viewListBox_SelectedIndexChanged);
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Detail Level:";
            //
            // detailLevelComboBox
            //
            this.detailLevelComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.detailLevelComboBox.FormattingEnabled = true;
            this.detailLevelComboBox.Items.AddRange(new object[] {
            "Undefined",
            "Coarse",
            "Medium",
            "Fine"});
            this.detailLevelComboBox.Location = new System.Drawing.Point(6, 33);
            this.detailLevelComboBox.Name = "detailLevelComboBox";
            this.detailLevelComboBox.Size = new System.Drawing.Size(158, 21);
            this.detailLevelComboBox.TabIndex = 5;
            this.detailLevelComboBox.SelectedIndexChanged += new System.EventHandler(this.detailLevelComboBox_SelectedIndexChanged);
            //
            // label2
            //
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Preview:";
            //
            // label3
            //
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(491, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Parameters:";
            //
            // okButton
            //
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.Location = new System.Drawing.Point(738, 598);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 11;
            this.okButton.Text = "&OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.OKButton_Click);
            //
            // closeButton
            //
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeButton.Location = new System.Drawing.Point(819, 598);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 12;
            this.closeButton.Text = "&Cancel";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            //
            // ObjectViewerForm
            //
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.closeButton;
            this.ClientSize = new System.Drawing.Size(906, 628);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.previewBox);
            this.Controls.Add(this.parametersDataGridView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ObjectViewerForm";
            this.ShowInTaskbar = false;
            this.Text = "Object Viewer";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ObjectViewerForm_FormClosed);
            this.Load += new System.EventHandler(this.ObjectViewerForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.parametersDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.previewBox)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView parametersDataGridView;
        private System.Windows.Forms.RadioButton physicalModelRadioButton;
        private System.Windows.Forms.RadioButton analyticalModelRadioButton;
        private System.Windows.Forms.PictureBox previewBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox detailLevelComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox viewListBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox viewDirectionComboBox;
    }
}
