//
// (C) Copyright 2003-2012 by Autodesk, Inc. 
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


namespace Revit.SDK.Samples.ModelLines.CS
{
    /// <summary>
    /// The Mail Form
    /// </summary>
    partial class ModelLinesForm
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
            this.informationGroupBox = new System.Windows.Forms.GroupBox();
            this.informationDataGridView = new System.Windows.Forms.DataGridView();
            this.typeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.numberColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.otherPanel = new System.Windows.Forms.Panel();
            this.offsetPointUserControl = new Revit.SDK.Samples.ModelLines.CS.PointUserControl();
            this.elementIdComboBox = new System.Windows.Forms.ComboBox();
            this.offsetLabel = new System.Windows.Forms.Label();
            this.copyFromLabel = new System.Windows.Forms.Label();
            this.otherInfoLabel = new System.Windows.Forms.Label();
            this.creationGroupBox = new System.Windows.Forms.GroupBox();
            this.createSketchPlaneButton = new System.Windows.Forms.Button();
            this.sketchPlaneComboBox = new System.Windows.Forms.ComboBox();
            this.sketchPlaneLabel = new System.Windows.Forms.Label();
            this.NurbSplineRadioButton = new System.Windows.Forms.RadioButton();
            this.hermiteSplineRadioButton = new System.Windows.Forms.RadioButton();
            this.ellipseRadioButton = new System.Windows.Forms.RadioButton();
            this.arcRadioButton = new System.Windows.Forms.RadioButton();
            this.lineRadioButton = new System.Windows.Forms.RadioButton();
            this.lineArcPanel = new System.Windows.Forms.Panel();
            this.lineArcInfoLabel = new System.Windows.Forms.Label();
            this.thirdPointUserControl = new Revit.SDK.Samples.ModelLines.CS.PointUserControl();
            this.secondPointUserControl = new Revit.SDK.Samples.ModelLines.CS.PointUserControl();
            this.thirdPointLabel = new System.Windows.Forms.Label();
            this.secondPointLabel = new System.Windows.Forms.Label();
            this.firstPointLabel = new System.Windows.Forms.Label();
            this.firstPointUserControl = new Revit.SDK.Samples.ModelLines.CS.PointUserControl();
            this.createButton = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            this.informationGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.informationDataGridView)).BeginInit();
            this.otherPanel.SuspendLayout();
            this.creationGroupBox.SuspendLayout();
            this.lineArcPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // informationGroupBox
            // 
            this.informationGroupBox.Controls.Add(this.informationDataGridView);
            this.informationGroupBox.Location = new System.Drawing.Point(12, 12);
            this.informationGroupBox.Name = "informationGroupBox";
            this.informationGroupBox.Size = new System.Drawing.Size(481, 161);
            this.informationGroupBox.TabIndex = 1;
            this.informationGroupBox.TabStop = false;
            this.informationGroupBox.Text = "Model Lines Information";
            // 
            // informationDataGridView
            // 
            this.informationDataGridView.AllowUserToAddRows = false;
            this.informationDataGridView.AllowUserToDeleteRows = false;
            this.informationDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.informationDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.typeColumn,
            this.numberColumn});
            this.informationDataGridView.Location = new System.Drawing.Point(6, 19);
            this.informationDataGridView.Name = "informationDataGridView";
            this.informationDataGridView.ReadOnly = true;
            this.informationDataGridView.RowHeadersVisible = false;
            this.informationDataGridView.Size = new System.Drawing.Size(469, 133);
            this.informationDataGridView.TabIndex = 1;
            // 
            // typeColumn
            // 
            this.typeColumn.HeaderText = "Type";
            this.typeColumn.Name = "typeColumn";
            this.typeColumn.ReadOnly = true;
            this.typeColumn.Width = 150;
            // 
            // numberColumn
            // 
            this.numberColumn.HeaderText = "Number";
            this.numberColumn.Name = "numberColumn";
            this.numberColumn.ReadOnly = true;
            this.numberColumn.Width = 150;
            // 
            // otherPanel
            // 
            this.otherPanel.Controls.Add(this.offsetPointUserControl);
            this.otherPanel.Controls.Add(this.elementIdComboBox);
            this.otherPanel.Controls.Add(this.offsetLabel);
            this.otherPanel.Controls.Add(this.copyFromLabel);
            this.otherPanel.Controls.Add(this.otherInfoLabel);
            this.otherPanel.Location = new System.Drawing.Point(7, 19);
            this.otherPanel.Name = "otherPanel";
            this.otherPanel.Size = new System.Drawing.Size(346, 117);
            this.otherPanel.TabIndex = 4;
            this.otherPanel.Visible = false;
            // 
            // offsetPointUserControl
            // 
            this.offsetPointUserControl.Location = new System.Drawing.Point(118, 81);
            this.offsetPointUserControl.Name = "offsetPointUserControl";
            this.offsetPointUserControl.Size = new System.Drawing.Size(213, 28);
            this.offsetPointUserControl.TabIndex = 3;
            // 
            // elementIdComboBox
            // 
            this.elementIdComboBox.FormattingEnabled = true;
            this.elementIdComboBox.Location = new System.Drawing.Point(117, 39);
            this.elementIdComboBox.Name = "elementIdComboBox";
            this.elementIdComboBox.Size = new System.Drawing.Size(213, 21);
            this.elementIdComboBox.TabIndex = 4;
            // 
            // offsetLabel
            // 
            this.offsetLabel.AutoSize = true;
            this.offsetLabel.Location = new System.Drawing.Point(24, 87);
            this.offsetLabel.Name = "offsetLabel";
            this.offsetLabel.Size = new System.Drawing.Size(38, 13);
            this.offsetLabel.TabIndex = 2;
            this.offsetLabel.Text = "Offset:";
            // 
            // copyFromLabel
            // 
            this.copyFromLabel.AutoSize = true;
            this.copyFromLabel.Location = new System.Drawing.Point(24, 42);
            this.copyFromLabel.Name = "copyFromLabel";
            this.copyFromLabel.Size = new System.Drawing.Size(82, 13);
            this.copyFromLabel.TabIndex = 1;
            this.copyFromLabel.Text = "Use curve from:";
            // 
            // otherInfoLabel
            // 
            this.otherInfoLabel.AutoSize = true;
            this.otherInfoLabel.Location = new System.Drawing.Point(3, 12);
            this.otherInfoLabel.Name = "otherInfoLabel";
            this.otherInfoLabel.Size = new System.Drawing.Size(145, 13);
            this.otherInfoLabel.TabIndex = 0;
            this.otherInfoLabel.Text = "New ellipse need information:";
            // 
            // creationGroupBox
            // 
            this.creationGroupBox.Controls.Add(this.createSketchPlaneButton);
            this.creationGroupBox.Controls.Add(this.sketchPlaneComboBox);
            this.creationGroupBox.Controls.Add(this.sketchPlaneLabel);
            this.creationGroupBox.Controls.Add(this.NurbSplineRadioButton);
            this.creationGroupBox.Controls.Add(this.hermiteSplineRadioButton);
            this.creationGroupBox.Controls.Add(this.ellipseRadioButton);
            this.creationGroupBox.Controls.Add(this.arcRadioButton);
            this.creationGroupBox.Controls.Add(this.lineRadioButton);
            this.creationGroupBox.Controls.Add(this.otherPanel);
            this.creationGroupBox.Controls.Add(this.lineArcPanel);
            this.creationGroupBox.Location = new System.Drawing.Point(12, 179);
            this.creationGroupBox.Name = "creationGroupBox";
            this.creationGroupBox.Size = new System.Drawing.Size(481, 167);
            this.creationGroupBox.TabIndex = 3;
            this.creationGroupBox.TabStop = false;
            this.creationGroupBox.Text = "Model Lines Creation";
            // 
            // createSketchPlaneButton
            // 
            this.createSketchPlaneButton.Location = new System.Drawing.Point(294, 139);
            this.createSketchPlaneButton.Name = "createSketchPlaneButton";
            this.createSketchPlaneButton.Size = new System.Drawing.Size(44, 23);
            this.createSketchPlaneButton.TabIndex = 5;
            this.createSketchPlaneButton.Text = "&New";
            this.createSketchPlaneButton.UseVisualStyleBackColor = true;
            this.createSketchPlaneButton.Click += new System.EventHandler(this.createSketchPlaneButton_Click);
            // 
            // sketchPlaneComboBox
            // 
            this.sketchPlaneComboBox.FormattingEnabled = true;
            this.sketchPlaneComboBox.Location = new System.Drawing.Point(119, 139);
            this.sketchPlaneComboBox.Name = "sketchPlaneComboBox";
            this.sketchPlaneComboBox.Size = new System.Drawing.Size(162, 21);
            this.sketchPlaneComboBox.TabIndex = 4;
            // 
            // sketchPlaneLabel
            // 
            this.sketchPlaneLabel.AutoSize = true;
            this.sketchPlaneLabel.Location = new System.Drawing.Point(10, 144);
            this.sketchPlaneLabel.Name = "sketchPlaneLabel";
            this.sketchPlaneLabel.Size = new System.Drawing.Size(74, 13);
            this.sketchPlaneLabel.TabIndex = 18;
            this.sketchPlaneLabel.Text = "Sketch Plane:";
            // 
            // NurbSplineRadioButton
            // 
            this.NurbSplineRadioButton.AutoSize = true;
            this.NurbSplineRadioButton.Location = new System.Drawing.Point(359, 140);
            this.NurbSplineRadioButton.Name = "NurbSplineRadioButton";
            this.NurbSplineRadioButton.Size = new System.Drawing.Size(106, 17);
            this.NurbSplineRadioButton.TabIndex = 10;
            this.NurbSplineRadioButton.TabStop = true;
            this.NurbSplineRadioButton.Text = "ModelNurbSpline";
            this.NurbSplineRadioButton.UseVisualStyleBackColor = true;
            this.NurbSplineRadioButton.CheckedChanged += new System.EventHandler(this.NurbSplineRadioButton_CheckedChanged);
            // 
            // hermiteSplineRadioButton
            // 
            this.hermiteSplineRadioButton.AutoSize = true;
            this.hermiteSplineRadioButton.Location = new System.Drawing.Point(359, 111);
            this.hermiteSplineRadioButton.Name = "hermiteSplineRadioButton";
            this.hermiteSplineRadioButton.Size = new System.Drawing.Size(119, 17);
            this.hermiteSplineRadioButton.TabIndex = 9;
            this.hermiteSplineRadioButton.TabStop = true;
            this.hermiteSplineRadioButton.Text = "ModelHermiteSpline";
            this.hermiteSplineRadioButton.UseVisualStyleBackColor = true;
            this.hermiteSplineRadioButton.CheckedChanged += new System.EventHandler(this.hermiteSplineRadioButton_CheckedChanged);
            // 
            // ellipseRadioButton
            // 
            this.ellipseRadioButton.AutoSize = true;
            this.ellipseRadioButton.Location = new System.Drawing.Point(359, 82);
            this.ellipseRadioButton.Name = "ellipseRadioButton";
            this.ellipseRadioButton.Size = new System.Drawing.Size(84, 17);
            this.ellipseRadioButton.TabIndex = 8;
            this.ellipseRadioButton.TabStop = true;
            this.ellipseRadioButton.Text = "ModelEllipse";
            this.ellipseRadioButton.UseVisualStyleBackColor = true;
            this.ellipseRadioButton.CheckedChanged += new System.EventHandler(this.ellipseRadioButton_CheckedChanged);
            // 
            // arcRadioButton
            // 
            this.arcRadioButton.AutoSize = true;
            this.arcRadioButton.Location = new System.Drawing.Point(359, 24);
            this.arcRadioButton.Name = "arcRadioButton";
            this.arcRadioButton.Size = new System.Drawing.Size(70, 17);
            this.arcRadioButton.TabIndex = 6;
            this.arcRadioButton.TabStop = true;
            this.arcRadioButton.Text = "ModelArc";
            this.arcRadioButton.UseVisualStyleBackColor = true;
            this.arcRadioButton.CheckedChanged += new System.EventHandler(this.arcRadioButton_CheckedChanged);
            // 
            // lineRadioButton
            // 
            this.lineRadioButton.AutoSize = true;
            this.lineRadioButton.Location = new System.Drawing.Point(359, 53);
            this.lineRadioButton.Name = "lineRadioButton";
            this.lineRadioButton.Size = new System.Drawing.Size(74, 17);
            this.lineRadioButton.TabIndex = 7;
            this.lineRadioButton.TabStop = true;
            this.lineRadioButton.Text = "ModelLine";
            this.lineRadioButton.UseVisualStyleBackColor = true;
            this.lineRadioButton.CheckedChanged += new System.EventHandler(this.lineRadioButton_CheckedChanged);
            // 
            // lineArcPanel
            // 
            this.lineArcPanel.Controls.Add(this.lineArcInfoLabel);
            this.lineArcPanel.Controls.Add(this.thirdPointUserControl);
            this.lineArcPanel.Controls.Add(this.secondPointUserControl);
            this.lineArcPanel.Controls.Add(this.thirdPointLabel);
            this.lineArcPanel.Controls.Add(this.secondPointLabel);
            this.lineArcPanel.Controls.Add(this.firstPointLabel);
            this.lineArcPanel.Controls.Add(this.firstPointUserControl);
            this.lineArcPanel.Location = new System.Drawing.Point(7, 19);
            this.lineArcPanel.Name = "lineArcPanel";
            this.lineArcPanel.Size = new System.Drawing.Size(346, 117);
            this.lineArcPanel.TabIndex = 5;
            this.lineArcPanel.Visible = false;
            // 
            // lineArcInfoLabel
            // 
            this.lineArcInfoLabel.AutoSize = true;
            this.lineArcInfoLabel.Location = new System.Drawing.Point(3, 12);
            this.lineArcInfoLabel.Name = "lineArcInfoLabel";
            this.lineArcInfoLabel.Size = new System.Drawing.Size(131, 13);
            this.lineArcInfoLabel.TabIndex = 6;
            this.lineArcInfoLabel.Text = "New arc need information:";
            // 
            // thirdPointUserControl
            // 
            this.thirdPointUserControl.Location = new System.Drawing.Point(118, 92);
            this.thirdPointUserControl.Name = "thirdPointUserControl";
            this.thirdPointUserControl.Size = new System.Drawing.Size(213, 22);
            this.thirdPointUserControl.TabIndex = 16;
            // 
            // secondPointUserControl
            // 
            this.secondPointUserControl.Location = new System.Drawing.Point(118, 63);
            this.secondPointUserControl.Name = "secondPointUserControl";
            this.secondPointUserControl.Size = new System.Drawing.Size(213, 25);
            this.secondPointUserControl.TabIndex = 15;
            // 
            // thirdPointLabel
            // 
            this.thirdPointLabel.AutoSize = true;
            this.thirdPointLabel.Location = new System.Drawing.Point(24, 96);
            this.thirdPointLabel.Name = "thirdPointLabel";
            this.thirdPointLabel.Size = new System.Drawing.Size(61, 13);
            this.thirdPointLabel.TabIndex = 13;
            this.thirdPointLabel.Text = "Third Point:";
            // 
            // secondPointLabel
            // 
            this.secondPointLabel.AutoSize = true;
            this.secondPointLabel.Location = new System.Drawing.Point(24, 67);
            this.secondPointLabel.Name = "secondPointLabel";
            this.secondPointLabel.Size = new System.Drawing.Size(74, 13);
            this.secondPointLabel.TabIndex = 12;
            this.secondPointLabel.Text = "Second Point:";
            // 
            // firstPointLabel
            // 
            this.firstPointLabel.AutoSize = true;
            this.firstPointLabel.Location = new System.Drawing.Point(24, 42);
            this.firstPointLabel.Name = "firstPointLabel";
            this.firstPointLabel.Size = new System.Drawing.Size(56, 13);
            this.firstPointLabel.TabIndex = 11;
            this.firstPointLabel.Text = "First Point:";
            // 
            // firstPointUserControl
            // 
            this.firstPointUserControl.Location = new System.Drawing.Point(118, 35);
            this.firstPointUserControl.Name = "firstPointUserControl";
            this.firstPointUserControl.Size = new System.Drawing.Size(213, 25);
            this.firstPointUserControl.TabIndex = 2;
            // 
            // createButton
            // 
            this.createButton.Location = new System.Drawing.Point(337, 352);
            this.createButton.Name = "createButton";
            this.createButton.Size = new System.Drawing.Size(75, 23);
            this.createButton.TabIndex = 11;
            this.createButton.Text = "C&reate";
            this.createButton.UseVisualStyleBackColor = true;
            this.createButton.Click += new System.EventHandler(this.createButton_Click);
            // 
            // closeButton
            // 
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeButton.Location = new System.Drawing.Point(418, 352);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 12;
            this.closeButton.Text = "&Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // ModelLinesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.closeButton;
            this.ClientSize = new System.Drawing.Size(506, 382);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.createButton);
            this.Controls.Add(this.creationGroupBox);
            this.Controls.Add(this.informationGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ModelLinesForm";
            this.ShowInTaskbar = false;
            this.Text = "Model Lines";
            this.informationGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.informationDataGridView)).EndInit();
            this.otherPanel.ResumeLayout(false);
            this.otherPanel.PerformLayout();
            this.creationGroupBox.ResumeLayout(false);
            this.creationGroupBox.PerformLayout();
            this.lineArcPanel.ResumeLayout(false);
            this.lineArcPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox informationGroupBox;
        private System.Windows.Forms.DataGridView informationDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn typeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn numberColumn;
        private System.Windows.Forms.GroupBox creationGroupBox;
        private System.Windows.Forms.Button createButton;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Panel lineArcPanel;
        private System.Windows.Forms.Label thirdPointLabel;
        private System.Windows.Forms.Label secondPointLabel;
        private System.Windows.Forms.Label firstPointLabel;
        private System.Windows.Forms.Panel otherPanel;
        private System.Windows.Forms.Label otherInfoLabel;
        private System.Windows.Forms.Label offsetLabel;
        private System.Windows.Forms.Label copyFromLabel;
        private System.Windows.Forms.ComboBox elementIdComboBox;
        private System.Windows.Forms.RadioButton NurbSplineRadioButton;
        private System.Windows.Forms.RadioButton hermiteSplineRadioButton;
        private System.Windows.Forms.RadioButton ellipseRadioButton;
        private System.Windows.Forms.RadioButton arcRadioButton;
        private System.Windows.Forms.RadioButton lineRadioButton;
        private PointUserControl offsetPointUserControl;
        private PointUserControl thirdPointUserControl;
        private PointUserControl secondPointUserControl;
        private PointUserControl firstPointUserControl;
        private System.Windows.Forms.Button createSketchPlaneButton;
        private System.Windows.Forms.ComboBox sketchPlaneComboBox;
        private System.Windows.Forms.Label sketchPlaneLabel;
        private System.Windows.Forms.Label lineArcInfoLabel;
    }
}