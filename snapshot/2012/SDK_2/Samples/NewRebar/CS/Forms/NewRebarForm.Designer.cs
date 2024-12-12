//
// (C) Copyright 2003-2011 by Autodesk, Inc.
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

namespace Revit.SDK.Samples.NewRebar.CS
{
    partial class NewRebarForm
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
            this.rebarShapesGroupBox = new System.Windows.Forms.GroupBox();
            this.shapesComboBox = new System.Windows.Forms.ComboBox();
            this.createShapeButton = new System.Windows.Forms.Button();
            this.rebarBarTypeGroupBox = new System.Windows.Forms.GroupBox();
            this.barTypesComboBox = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.nameLabel = new System.Windows.Forms.Label();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.arcTypecomboBox = new System.Windows.Forms.ComboBox();
            this.segmentCountTextBox = new System.Windows.Forms.TextBox();
            this.bySegmentsradioButton = new System.Windows.Forms.RadioButton();
            this.byArcradioButton = new System.Windows.Forms.RadioButton();
            this.rebarShapesGroupBox.SuspendLayout();
            this.rebarBarTypeGroupBox.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(141, 233);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(89, 26);
            this.okButton.TabIndex = 4;
            this.okButton.Text = "&OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(252, 233);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(89, 26);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // rebarShapesGroupBox
            // 
            this.rebarShapesGroupBox.Controls.Add(this.shapesComboBox);
            this.rebarShapesGroupBox.Location = new System.Drawing.Point(26, 12);
            this.rebarShapesGroupBox.Name = "rebarShapesGroupBox";
            this.rebarShapesGroupBox.Size = new System.Drawing.Size(204, 77);
            this.rebarShapesGroupBox.TabIndex = 7;
            this.rebarShapesGroupBox.TabStop = false;
            this.rebarShapesGroupBox.Text = "RebarShape";
            // 
            // shapesComboBox
            // 
            this.shapesComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.shapesComboBox.FormattingEnabled = true;
            this.shapesComboBox.Location = new System.Drawing.Point(31, 31);
            this.shapesComboBox.Name = "shapesComboBox";
            this.shapesComboBox.Size = new System.Drawing.Size(149, 21);
            this.shapesComboBox.TabIndex = 8;
            // 
            // createShapeButton
            // 
            this.createShapeButton.Location = new System.Drawing.Point(20, 151);
            this.createShapeButton.Name = "createShapeButton";
            this.createShapeButton.Size = new System.Drawing.Size(162, 29);
            this.createShapeButton.TabIndex = 9;
            this.createShapeButton.Text = "Create Rebar &Shape ...";
            this.createShapeButton.UseVisualStyleBackColor = true;
            this.createShapeButton.Click += new System.EventHandler(this.createShapeButton_Click);
            // 
            // rebarBarTypeGroupBox
            // 
            this.rebarBarTypeGroupBox.Controls.Add(this.barTypesComboBox);
            this.rebarBarTypeGroupBox.Location = new System.Drawing.Point(26, 116);
            this.rebarBarTypeGroupBox.Name = "rebarBarTypeGroupBox";
            this.rebarBarTypeGroupBox.Size = new System.Drawing.Size(204, 82);
            this.rebarBarTypeGroupBox.TabIndex = 8;
            this.rebarBarTypeGroupBox.TabStop = false;
            this.rebarBarTypeGroupBox.Text = "RebarBarType";
            // 
            // barTypesComboBox
            // 
            this.barTypesComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.barTypesComboBox.FormattingEnabled = true;
            this.barTypesComboBox.Location = new System.Drawing.Point(30, 35);
            this.barTypesComboBox.Name = "barTypesComboBox";
            this.barTypesComboBox.Size = new System.Drawing.Size(150, 21);
            this.barTypesComboBox.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.nameLabel);
            this.groupBox2.Controls.Add(this.nameTextBox);
            this.groupBox2.Controls.Add(this.arcTypecomboBox);
            this.groupBox2.Controls.Add(this.segmentCountTextBox);
            this.groupBox2.Controls.Add(this.bySegmentsradioButton);
            this.groupBox2.Controls.Add(this.byArcradioButton);
            this.groupBox2.Controls.Add(this.createShapeButton);
            this.groupBox2.Location = new System.Drawing.Point(249, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(213, 186);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Create Rebar Shape";
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(20, 36);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(35, 13);
            this.nameLabel.TabIndex = 21;
            this.nameLabel.Text = "Name";
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(79, 31);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(103, 20);
            this.nameTextBox.TabIndex = 20;
            // 
            // arcTypecomboBox
            // 
            this.arcTypecomboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.arcTypecomboBox.FormattingEnabled = true;
            this.arcTypecomboBox.Location = new System.Drawing.Point(79, 68);
            this.arcTypecomboBox.Name = "arcTypecomboBox";
            this.arcTypecomboBox.Size = new System.Drawing.Size(103, 21);
            this.arcTypecomboBox.TabIndex = 19;
            // 
            // segmentCountTextBox
            // 
            this.segmentCountTextBox.Enabled = false;
            this.segmentCountTextBox.Location = new System.Drawing.Point(108, 103);
            this.segmentCountTextBox.Name = "segmentCountTextBox";
            this.segmentCountTextBox.Size = new System.Drawing.Size(74, 20);
            this.segmentCountTextBox.TabIndex = 18;
            // 
            // bySegmentsradioButton
            // 
            this.bySegmentsradioButton.AutoSize = true;
            this.bySegmentsradioButton.Location = new System.Drawing.Point(20, 104);
            this.bySegmentsradioButton.Name = "bySegmentsradioButton";
            this.bySegmentsradioButton.Size = new System.Drawing.Size(87, 17);
            this.bySegmentsradioButton.TabIndex = 17;
            this.bySegmentsradioButton.Text = "By Segments";
            this.bySegmentsradioButton.UseVisualStyleBackColor = true;
            this.bySegmentsradioButton.CheckedChanged += new System.EventHandler(this.bySegmentsradioButton_CheckedChanged);
            // 
            // byArcradioButton
            // 
            this.byArcradioButton.AutoSize = true;
            this.byArcradioButton.Checked = true;
            this.byArcradioButton.Location = new System.Drawing.Point(20, 72);
            this.byArcradioButton.Name = "byArcradioButton";
            this.byArcradioButton.Size = new System.Drawing.Size(56, 17);
            this.byArcradioButton.TabIndex = 16;
            this.byArcradioButton.TabStop = true;
            this.byArcradioButton.Text = "By Arc";
            this.byArcradioButton.UseVisualStyleBackColor = true;
            this.byArcradioButton.CheckedChanged += new System.EventHandler(this.byArcradioButton_CheckedChanged);
            // 
            // NewRebarForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(484, 281);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.rebarBarTypeGroupBox);
            this.Controls.Add(this.rebarShapesGroupBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewRebarForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Create Rebar";
            this.Load += new System.EventHandler(this.NewRebarForm_Load);
            this.rebarShapesGroupBox.ResumeLayout(false);
            this.rebarBarTypeGroupBox.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.GroupBox rebarShapesGroupBox;
        private System.Windows.Forms.Button createShapeButton;
        private System.Windows.Forms.ComboBox shapesComboBox;
        private System.Windows.Forms.GroupBox rebarBarTypeGroupBox;
        private System.Windows.Forms.ComboBox barTypesComboBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.ComboBox arcTypecomboBox;
        private System.Windows.Forms.TextBox segmentCountTextBox;
        private System.Windows.Forms.RadioButton bySegmentsradioButton;
        private System.Windows.Forms.RadioButton byArcradioButton;
    }
}