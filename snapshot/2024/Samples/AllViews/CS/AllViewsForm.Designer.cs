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

namespace Revit.SDK.Samples.AllViews.CS
{
    partial class AllViewsForm
    {
        private ViewsMgr m_data;
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
			this.allViewsTreeView = new System.Windows.Forms.TreeView();
			this.GenerateSheetGroupBox = new System.Windows.Forms.GroupBox();
			this.sheetNameTextBox = new System.Windows.Forms.TextBox();
			this.titleBlocksLabel = new System.Windows.Forms.Label();
			this.cancelButton = new System.Windows.Forms.Button();
			this.oKButton = new System.Windows.Forms.Button();
			this.titleBlocksListBox = new System.Windows.Forms.ListBox();
			this.sheetNameLabel = new System.Windows.Forms.Label();
			this.setLabelOffsetXTextBox = new System.Windows.Forms.TextBox();
			this.setLabelOffsetButton = new System.Windows.Forms.Button();
			this.setLabelOffsetYTextBox = new System.Windows.Forms.TextBox();
			this.setLabelLineLengthTextBox = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.getLabelLineLengthTextBox = new System.Windows.Forms.TextBox();
			this.selectSheetNameTextBox = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.groupBoxRotation = new System.Windows.Forms.GroupBox();
			this.counterClockWiseRadioButton = new System.Windows.Forms.RadioButton();
			this.clockWiseRadioButton = new System.Windows.Forms.RadioButton();
			this.noneRadioButton = new System.Windows.Forms.RadioButton();
			this.setRotationButton = new System.Windows.Forms.Button();
			this.labelUpdateOffset = new System.Windows.Forms.Label();
			this.allViewsGroupBox = new System.Windows.Forms.GroupBox();
			this.CreateNewSheetAndViewportsGroupBox1 = new System.Windows.Forms.GroupBox();
			this.ViewportPropertiesGroupBox2 = new System.Windows.Forms.GroupBox();
			this.groupBox8 = new System.Windows.Forms.GroupBox();
			this.getOrientationTtextBox = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.getBoxCenterTextBox = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox7 = new System.Windows.Forms.GroupBox();
			this.getLabelLineOffsetTextBox = new System.Windows.Forms.TextBox();
			this.groupBox6 = new System.Windows.Forms.GroupBox();
			this.label12 = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this.getMaxLabelOutlineTextBox = new System.Windows.Forms.TextBox();
			this.getMinLabelOutlineTextBox = new System.Windows.Forms.TextBox();
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this.label11 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.getMaxBoxOutlineTextBox = new System.Windows.Forms.TextBox();
			this.getMinBoxOutlineTextBox = new System.Windows.Forms.TextBox();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.setLabelLengthButton = new System.Windows.Forms.Button();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.label7 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.selectViewportButton = new System.Windows.Forms.Button();
			this.selectAssociatedViewNameTextBox = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.GenerateSheetGroupBox.SuspendLayout();
			this.groupBoxRotation.SuspendLayout();
			this.allViewsGroupBox.SuspendLayout();
			this.CreateNewSheetAndViewportsGroupBox1.SuspendLayout();
			this.ViewportPropertiesGroupBox2.SuspendLayout();
			this.groupBox8.SuspendLayout();
			this.groupBox7.SuspendLayout();
			this.groupBox6.SuspendLayout();
			this.groupBox5.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// allViewsTreeView
			// 
			this.allViewsTreeView.CheckBoxes = true;
			this.allViewsTreeView.Location = new System.Drawing.Point(12, 37);
			this.allViewsTreeView.Margin = new System.Windows.Forms.Padding(6);
			this.allViewsTreeView.Name = "allViewsTreeView";
			this.allViewsTreeView.Size = new System.Drawing.Size(474, 414);
			this.allViewsTreeView.TabIndex = 0;
			this.allViewsTreeView.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.allViewsTreeView_AfterCheck);
			// 
			// GenerateSheetGroupBox
			// 
			this.GenerateSheetGroupBox.Controls.Add(this.sheetNameTextBox);
			this.GenerateSheetGroupBox.Controls.Add(this.titleBlocksLabel);
			this.GenerateSheetGroupBox.Controls.Add(this.cancelButton);
			this.GenerateSheetGroupBox.Controls.Add(this.oKButton);
			this.GenerateSheetGroupBox.Controls.Add(this.titleBlocksListBox);
			this.GenerateSheetGroupBox.Controls.Add(this.sheetNameLabel);
			this.GenerateSheetGroupBox.Location = new System.Drawing.Point(525, 28);
			this.GenerateSheetGroupBox.Margin = new System.Windows.Forms.Padding(6);
			this.GenerateSheetGroupBox.Name = "GenerateSheetGroupBox";
			this.GenerateSheetGroupBox.Padding = new System.Windows.Forms.Padding(6);
			this.GenerateSheetGroupBox.Size = new System.Drawing.Size(521, 469);
			this.GenerateSheetGroupBox.TabIndex = 1;
			this.GenerateSheetGroupBox.TabStop = false;
			this.GenerateSheetGroupBox.Text = "Generate Sheet";
			// 
			// sheetNameTextBox
			// 
			this.sheetNameTextBox.Location = new System.Drawing.Point(156, 321);
			this.sheetNameTextBox.Margin = new System.Windows.Forms.Padding(6);
			this.sheetNameTextBox.Name = "sheetNameTextBox";
			this.sheetNameTextBox.Size = new System.Drawing.Size(172, 31);
			this.sheetNameTextBox.TabIndex = 2;
			this.sheetNameTextBox.Text = "Unnamed";
			// 
			// titleBlocksLabel
			// 
			this.titleBlocksLabel.AutoSize = true;
			this.titleBlocksLabel.Location = new System.Drawing.Point(12, 37);
			this.titleBlocksLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.titleBlocksLabel.Name = "titleBlocksLabel";
			this.titleBlocksLabel.Size = new System.Drawing.Size(117, 25);
			this.titleBlocksLabel.TabIndex = 3;
			this.titleBlocksLabel.Text = "TitleBlocks";
			// 
			// cancelButton
			// 
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(235, 398);
			this.cancelButton.Margin = new System.Windows.Forms.Padding(6);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(150, 44);
			this.cancelButton.TabIndex = 4;
			this.cancelButton.Text = "&Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			// 
			// oKButton
			// 
			this.oKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.oKButton.Location = new System.Drawing.Point(73, 398);
			this.oKButton.Margin = new System.Windows.Forms.Padding(6);
			this.oKButton.Name = "oKButton";
			this.oKButton.Size = new System.Drawing.Size(150, 44);
			this.oKButton.TabIndex = 3;
			this.oKButton.Text = "&OK";
			this.oKButton.UseVisualStyleBackColor = true;
			this.oKButton.Click += new System.EventHandler(this.oKButton_Click);
			// 
			// titleBlocksListBox
			// 
			this.titleBlocksListBox.FormattingEnabled = true;
			this.titleBlocksListBox.ItemHeight = 25;
			this.titleBlocksListBox.Location = new System.Drawing.Point(18, 85);
			this.titleBlocksListBox.Margin = new System.Windows.Forms.Padding(6);
			this.titleBlocksListBox.Name = "titleBlocksListBox";
			this.titleBlocksListBox.Size = new System.Drawing.Size(367, 204);
			this.titleBlocksListBox.Sorted = true;
			this.titleBlocksListBox.TabIndex = 6;
			this.titleBlocksListBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.titleBlocksListBox_MouseClick);
			// 
			// sheetNameLabel
			// 
			this.sheetNameLabel.AutoSize = true;
			this.sheetNameLabel.Location = new System.Drawing.Point(12, 327);
			this.sheetNameLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.sheetNameLabel.Name = "sheetNameLabel";
			this.sheetNameLabel.Size = new System.Drawing.Size(130, 25);
			this.sheetNameLabel.TabIndex = 5;
			this.sheetNameLabel.Text = "Sheet Name";
			// 
			// setLabelOffsetXTextBox
			// 
			this.setLabelOffsetXTextBox.Enabled = false;
			this.setLabelOffsetXTextBox.Location = new System.Drawing.Point(57, 30);
			this.setLabelOffsetXTextBox.Name = "setLabelOffsetXTextBox";
			this.setLabelOffsetXTextBox.Size = new System.Drawing.Size(240, 31);
			this.setLabelOffsetXTextBox.TabIndex = 3;
			// 
			// setLabelOffsetButton
			// 
			this.setLabelOffsetButton.Enabled = false;
			this.setLabelOffsetButton.Location = new System.Drawing.Point(313, 41);
			this.setLabelOffsetButton.Name = "setLabelOffsetButton";
			this.setLabelOffsetButton.Size = new System.Drawing.Size(150, 44);
			this.setLabelOffsetButton.TabIndex = 4;
			this.setLabelOffsetButton.Text = "Apply";
			this.setLabelOffsetButton.UseVisualStyleBackColor = true;
			this.setLabelOffsetButton.Click += new System.EventHandler(this.setLabelOffsetButton_Click);
			// 
			// setLabelOffsetYTextBox
			// 
			this.setLabelOffsetYTextBox.Enabled = false;
			this.setLabelOffsetYTextBox.Location = new System.Drawing.Point(57, 67);
			this.setLabelOffsetYTextBox.Name = "setLabelOffsetYTextBox";
			this.setLabelOffsetYTextBox.Size = new System.Drawing.Size(240, 31);
			this.setLabelOffsetYTextBox.TabIndex = 5;
			// 
			// setLabelLineLengthTextBox
			// 
			this.setLabelLineLengthTextBox.Enabled = false;
			this.setLabelLineLengthTextBox.Location = new System.Drawing.Point(18, 36);
			this.setLabelLineLengthTextBox.Name = "setLabelLineLengthTextBox";
			this.setLabelLineLengthTextBox.Size = new System.Drawing.Size(240, 31);
			this.setLabelLineLengthTextBox.TabIndex = 7;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(524, 49);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(78, 25);
			this.label3.TabIndex = 14;
			this.label3.Text = "Length";
			// 
			// getLabelLineLengthTextBox
			// 
			this.getLabelLineLengthTextBox.BackColor = System.Drawing.SystemColors.Control;
			this.getLabelLineLengthTextBox.Enabled = false;
			this.getLabelLineLengthTextBox.Location = new System.Drawing.Point(608, 46);
			this.getLabelLineLengthTextBox.Name = "getLabelLineLengthTextBox";
			this.getLabelLineLengthTextBox.Size = new System.Drawing.Size(240, 31);
			this.getLabelLineLengthTextBox.TabIndex = 15;
			// 
			// selectSheetNameTextBox
			// 
			this.selectSheetNameTextBox.ForeColor = System.Drawing.SystemColors.WindowText;
			this.selectSheetNameTextBox.Location = new System.Drawing.Point(145, 32);
			this.selectSheetNameTextBox.Name = "selectSheetNameTextBox";
			this.selectSheetNameTextBox.Size = new System.Drawing.Size(205, 31);
			this.selectSheetNameTextBox.TabIndex = 22;
			this.selectSheetNameTextBox.Text = "Unnamed";
			this.selectSheetNameTextBox.TextChanged += new System.EventHandler(this.selectSheetNameTextBox_TextChanged);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(9, 35);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(130, 25);
			this.label6.TabIndex = 23;
			this.label6.Text = "Sheet Name";
			// 
			// groupBoxRotation
			// 
			this.groupBoxRotation.Controls.Add(this.counterClockWiseRadioButton);
			this.groupBoxRotation.Controls.Add(this.clockWiseRadioButton);
			this.groupBoxRotation.Controls.Add(this.noneRadioButton);
			this.groupBoxRotation.Location = new System.Drawing.Point(102, 28);
			this.groupBoxRotation.Name = "groupBoxRotation";
			this.groupBoxRotation.Size = new System.Drawing.Size(598, 63);
			this.groupBoxRotation.TabIndex = 44;
			this.groupBoxRotation.TabStop = false;
			this.groupBoxRotation.Text = "Orientation";
			// 
			// counterClockWiseRadioButton
			// 
			this.counterClockWiseRadioButton.AutoSize = true;
			this.counterClockWiseRadioButton.Enabled = false;
			this.counterClockWiseRadioButton.Location = new System.Drawing.Point(336, 27);
			this.counterClockWiseRadioButton.Name = "counterClockWiseRadioButton";
			this.counterClockWiseRadioButton.Size = new System.Drawing.Size(221, 29);
			this.counterClockWiseRadioButton.TabIndex = 2;
			this.counterClockWiseRadioButton.TabStop = true;
			this.counterClockWiseRadioButton.Text = "CounterClockWise";
			this.counterClockWiseRadioButton.UseVisualStyleBackColor = true;
			this.counterClockWiseRadioButton.CheckedChanged += new System.EventHandler(this.counterClockWiseRadioButton_CheckedChanged);
			// 
			// clockWiseRadioButton
			// 
			this.clockWiseRadioButton.AutoSize = true;
			this.clockWiseRadioButton.Enabled = false;
			this.clockWiseRadioButton.Location = new System.Drawing.Point(185, 27);
			this.clockWiseRadioButton.Name = "clockWiseRadioButton";
			this.clockWiseRadioButton.Size = new System.Drawing.Size(145, 29);
			this.clockWiseRadioButton.TabIndex = 1;
			this.clockWiseRadioButton.TabStop = true;
			this.clockWiseRadioButton.Text = "ClockWise";
			this.clockWiseRadioButton.UseVisualStyleBackColor = true;
			this.clockWiseRadioButton.CheckedChanged += new System.EventHandler(this.clockWiseRadioButton_CheckedChanged);
			// 
			// noneRadioButton
			// 
			this.noneRadioButton.AutoSize = true;
			this.noneRadioButton.Checked = true;
			this.noneRadioButton.Enabled = false;
			this.noneRadioButton.Location = new System.Drawing.Point(85, 27);
			this.noneRadioButton.Name = "noneRadioButton";
			this.noneRadioButton.Size = new System.Drawing.Size(94, 29);
			this.noneRadioButton.TabIndex = 0;
			this.noneRadioButton.TabStop = true;
			this.noneRadioButton.Text = "None";
			this.noneRadioButton.UseVisualStyleBackColor = true;
			this.noneRadioButton.CheckedChanged += new System.EventHandler(this.noneRadioButton_CheckedChanged);
			// 
			// setRotationButton
			// 
			this.setRotationButton.Enabled = false;
			this.setRotationButton.Location = new System.Drawing.Point(829, 37);
			this.setRotationButton.Name = "setRotationButton";
			this.setRotationButton.Size = new System.Drawing.Size(150, 44);
			this.setRotationButton.TabIndex = 45;
			this.setRotationButton.Text = "Apply";
			this.setRotationButton.UseVisualStyleBackColor = true;
			this.setRotationButton.Click += new System.EventHandler(this.setRotationButton_Click);
			// 
			// labelUpdateOffset
			// 
			this.labelUpdateOffset.AutoSize = true;
			this.labelUpdateOffset.Location = new System.Drawing.Point(25, 49);
			this.labelUpdateOffset.Name = "labelUpdateOffset";
			this.labelUpdateOffset.Size = new System.Drawing.Size(69, 25);
			this.labelUpdateOffset.TabIndex = 47;
			this.labelUpdateOffset.Text = "Offset";
			// 
			// allViewsGroupBox
			// 
			this.allViewsGroupBox.Controls.Add(this.allViewsTreeView);
			this.allViewsGroupBox.Location = new System.Drawing.Point(15, 33);
			this.allViewsGroupBox.Margin = new System.Windows.Forms.Padding(6);
			this.allViewsGroupBox.Name = "allViewsGroupBox";
			this.allViewsGroupBox.Padding = new System.Windows.Forms.Padding(6);
			this.allViewsGroupBox.Size = new System.Drawing.Size(498, 464);
			this.allViewsGroupBox.TabIndex = 0;
			this.allViewsGroupBox.TabStop = false;
			this.allViewsGroupBox.Text = "All Views";
			// 
			// CreateNewSheetAndViewportsGroupBox1
			// 
			this.CreateNewSheetAndViewportsGroupBox1.Controls.Add(this.GenerateSheetGroupBox);
			this.CreateNewSheetAndViewportsGroupBox1.Controls.Add(this.allViewsGroupBox);
			this.CreateNewSheetAndViewportsGroupBox1.Location = new System.Drawing.Point(12, 12);
			this.CreateNewSheetAndViewportsGroupBox1.Name = "CreateNewSheetAndViewportsGroupBox1";
			this.CreateNewSheetAndViewportsGroupBox1.Size = new System.Drawing.Size(1081, 509);
			this.CreateNewSheetAndViewportsGroupBox1.TabIndex = 50;
			this.CreateNewSheetAndViewportsGroupBox1.TabStop = false;
			this.CreateNewSheetAndViewportsGroupBox1.Text = "Create new Sheet and Viewports";
			// 
			// ViewportPropertiesGroupBox2
			// 
			this.ViewportPropertiesGroupBox2.Controls.Add(this.groupBox8);
			this.ViewportPropertiesGroupBox2.Controls.Add(this.groupBox7);
			this.ViewportPropertiesGroupBox2.Controls.Add(this.groupBox6);
			this.ViewportPropertiesGroupBox2.Controls.Add(this.groupBox5);
			this.ViewportPropertiesGroupBox2.Controls.Add(this.groupBox4);
			this.ViewportPropertiesGroupBox2.Controls.Add(this.groupBox3);
			this.ViewportPropertiesGroupBox2.Controls.Add(this.groupBox2);
			this.ViewportPropertiesGroupBox2.Controls.Add(this.groupBox1);
			this.ViewportPropertiesGroupBox2.Location = new System.Drawing.Point(12, 533);
			this.ViewportPropertiesGroupBox2.Name = "ViewportPropertiesGroupBox2";
			this.ViewportPropertiesGroupBox2.Size = new System.Drawing.Size(1092, 735);
			this.ViewportPropertiesGroupBox2.TabIndex = 51;
			this.ViewportPropertiesGroupBox2.TabStop = false;
			this.ViewportPropertiesGroupBox2.Text = "Viewport Properties";
			// 
			// groupBox8
			// 
			this.groupBox8.Controls.Add(this.getOrientationTtextBox);
			this.groupBox8.Controls.Add(this.label2);
			this.groupBox8.Controls.Add(this.getBoxCenterTextBox);
			this.groupBox8.Controls.Add(this.label1);
			this.groupBox8.Location = new System.Drawing.Point(30, 639);
			this.groupBox8.Name = "groupBox8";
			this.groupBox8.Size = new System.Drawing.Size(1051, 85);
			this.groupBox8.TabIndex = 70;
			this.groupBox8.TabStop = false;
			this.groupBox8.Text = "Others";
			// 
			// getOrientationTtextBox
			// 
			this.getOrientationTtextBox.BackColor = System.Drawing.SystemColors.Control;
			this.getOrientationTtextBox.Enabled = false;
			this.getOrientationTtextBox.Location = new System.Drawing.Point(748, 33);
			this.getOrientationTtextBox.Name = "getOrientationTtextBox";
			this.getOrientationTtextBox.Size = new System.Drawing.Size(268, 31);
			this.getOrientationTtextBox.TabIndex = 3;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(583, 36);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(117, 25);
			this.label2.TabIndex = 2;
			this.label2.Text = "Orientation";
			// 
			// getBoxCenterTextBox
			// 
			this.getBoxCenterTextBox.BackColor = System.Drawing.SystemColors.Control;
			this.getBoxCenterTextBox.Enabled = false;
			this.getBoxCenterTextBox.Location = new System.Drawing.Point(145, 33);
			this.getBoxCenterTextBox.Name = "getBoxCenterTextBox";
			this.getBoxCenterTextBox.Size = new System.Drawing.Size(418, 31);
			this.getBoxCenterTextBox.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(20, 36);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(119, 25);
			this.label1.TabIndex = 0;
			this.label1.Text = "Box Center";
			// 
			// groupBox7
			// 
			this.groupBox7.Controls.Add(this.getLabelLineOffsetTextBox);
			this.groupBox7.Controls.Add(this.labelUpdateOffset);
			this.groupBox7.Controls.Add(this.getLabelLineLengthTextBox);
			this.groupBox7.Controls.Add(this.label3);
			this.groupBox7.Location = new System.Drawing.Point(30, 530);
			this.groupBox7.Name = "groupBox7";
			this.groupBox7.Size = new System.Drawing.Size(1051, 100);
			this.groupBox7.TabIndex = 69;
			this.groupBox7.TabStop = false;
			this.groupBox7.Text = "Label Line";
			// 
			// getLabelLineOffsetTextBox
			// 
			this.getLabelLineOffsetTextBox.BackColor = System.Drawing.SystemColors.Control;
			this.getLabelLineOffsetTextBox.Enabled = false;
			this.getLabelLineOffsetTextBox.Location = new System.Drawing.Point(100, 46);
			this.getLabelLineOffsetTextBox.Name = "getLabelLineOffsetTextBox";
			this.getLabelLineOffsetTextBox.Size = new System.Drawing.Size(418, 31);
			this.getLabelLineOffsetTextBox.TabIndex = 68;
			// 
			// groupBox6
			// 
			this.groupBox6.Controls.Add(this.label12);
			this.groupBox6.Controls.Add(this.label13);
			this.groupBox6.Controls.Add(this.getMaxLabelOutlineTextBox);
			this.groupBox6.Controls.Add(this.getMinLabelOutlineTextBox);
			this.groupBox6.Location = new System.Drawing.Point(30, 437);
			this.groupBox6.Name = "groupBox6";
			this.groupBox6.Size = new System.Drawing.Size(1051, 84);
			this.groupBox6.TabIndex = 67;
			this.groupBox6.TabStop = false;
			this.groupBox6.Text = "Label Outline";
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Location = new System.Drawing.Point(528, 30);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(53, 25);
			this.label12.TabIndex = 65;
			this.label12.Text = "Max";
			// 
			// label13
			// 
			this.label13.AutoSize = true;
			this.label13.Location = new System.Drawing.Point(18, 30);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(47, 25);
			this.label13.TabIndex = 64;
			this.label13.Text = "Min";
			// 
			// getMaxLabelOutlineTextBox
			// 
			this.getMaxLabelOutlineTextBox.BackColor = System.Drawing.SystemColors.Control;
			this.getMaxLabelOutlineTextBox.Enabled = false;
			this.getMaxLabelOutlineTextBox.Location = new System.Drawing.Point(598, 27);
			this.getMaxLabelOutlineTextBox.Name = "getMaxLabelOutlineTextBox";
			this.getMaxLabelOutlineTextBox.Size = new System.Drawing.Size(418, 31);
			this.getMaxLabelOutlineTextBox.TabIndex = 63;
			// 
			// getMinLabelOutlineTextBox
			// 
			this.getMinLabelOutlineTextBox.BackColor = System.Drawing.SystemColors.Control;
			this.getMinLabelOutlineTextBox.Enabled = false;
			this.getMinLabelOutlineTextBox.Location = new System.Drawing.Point(68, 27);
			this.getMinLabelOutlineTextBox.Name = "getMinLabelOutlineTextBox";
			this.getMinLabelOutlineTextBox.Size = new System.Drawing.Size(418, 31);
			this.getMinLabelOutlineTextBox.TabIndex = 62;
			// 
			// groupBox5
			// 
			this.groupBox5.Controls.Add(this.label11);
			this.groupBox5.Controls.Add(this.label10);
			this.groupBox5.Controls.Add(this.getMaxBoxOutlineTextBox);
			this.groupBox5.Controls.Add(this.getMinBoxOutlineTextBox);
			this.groupBox5.Location = new System.Drawing.Point(30, 344);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Size = new System.Drawing.Size(1051, 84);
			this.groupBox5.TabIndex = 66;
			this.groupBox5.TabStop = false;
			this.groupBox5.Text = "Box Outline";
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(528, 30);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(53, 25);
			this.label11.TabIndex = 65;
			this.label11.Text = "Max";
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(18, 30);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(47, 25);
			this.label10.TabIndex = 64;
			this.label10.Text = "Min";
			// 
			// getMaxBoxOutlineTextBox
			// 
			this.getMaxBoxOutlineTextBox.BackColor = System.Drawing.SystemColors.Control;
			this.getMaxBoxOutlineTextBox.Enabled = false;
			this.getMaxBoxOutlineTextBox.Location = new System.Drawing.Point(598, 27);
			this.getMaxBoxOutlineTextBox.Name = "getMaxBoxOutlineTextBox";
			this.getMaxBoxOutlineTextBox.Size = new System.Drawing.Size(418, 31);
			this.getMaxBoxOutlineTextBox.TabIndex = 63;
			// 
			// getMinBoxOutlineTextBox
			// 
			this.getMinBoxOutlineTextBox.BackColor = System.Drawing.SystemColors.Control;
			this.getMinBoxOutlineTextBox.Enabled = false;
			this.getMinBoxOutlineTextBox.ForeColor = System.Drawing.SystemColors.WindowText;
			this.getMinBoxOutlineTextBox.Location = new System.Drawing.Point(68, 27);
			this.getMinBoxOutlineTextBox.Name = "getMinBoxOutlineTextBox";
			this.getMinBoxOutlineTextBox.Size = new System.Drawing.Size(418, 31);
			this.getMinBoxOutlineTextBox.TabIndex = 62;
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.setLabelLengthButton);
			this.groupBox4.Controls.Add(this.setLabelLineLengthTextBox);
			this.groupBox4.Location = new System.Drawing.Point(525, 233);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(556, 104);
			this.groupBox4.TabIndex = 59;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Label Line Length";
			// 
			// setLabelLengthButton
			// 
			this.setLabelLengthButton.Enabled = false;
			this.setLabelLengthButton.Location = new System.Drawing.Point(334, 30);
			this.setLabelLengthButton.Name = "setLabelLengthButton";
			this.setLabelLengthButton.Size = new System.Drawing.Size(150, 44);
			this.setLabelLengthButton.TabIndex = 55;
			this.setLabelLengthButton.Text = "Apply";
			this.setLabelLengthButton.UseVisualStyleBackColor = true;
			this.setLabelLengthButton.Click += new System.EventHandler(this.setLabelLengthButton_Click);
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.label7);
			this.groupBox3.Controls.Add(this.label5);
			this.groupBox3.Controls.Add(this.setLabelOffsetYTextBox);
			this.groupBox3.Controls.Add(this.setLabelOffsetButton);
			this.groupBox3.Controls.Add(this.setLabelOffsetXTextBox);
			this.groupBox3.Location = new System.Drawing.Point(30, 231);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(489, 104);
			this.groupBox3.TabIndex = 58;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Label Offset";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(25, 67);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(27, 25);
			this.label7.TabIndex = 57;
			this.label7.Text = "Y";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(25, 30);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(26, 25);
			this.label5.TabIndex = 56;
			this.label5.Text = "X";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.setRotationButton);
			this.groupBox2.Controls.Add(this.groupBoxRotation);
			this.groupBox2.Location = new System.Drawing.Point(30, 125);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(1051, 97);
			this.groupBox2.TabIndex = 54;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Rotation";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.selectViewportButton);
			this.groupBox1.Controls.Add(this.selectAssociatedViewNameTextBox);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.selectSheetNameTextBox);
			this.groupBox1.Location = new System.Drawing.Point(30, 27);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(1051, 89);
			this.groupBox1.TabIndex = 53;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Select Viewport";
			// 
			// selectViewportButton
			// 
			this.selectViewportButton.Location = new System.Drawing.Point(829, 25);
			this.selectViewportButton.Name = "selectViewportButton";
			this.selectViewportButton.Size = new System.Drawing.Size(150, 44);
			this.selectViewportButton.TabIndex = 52;
			this.selectViewportButton.Text = "Select";
			this.selectViewportButton.UseVisualStyleBackColor = true;
			this.selectViewportButton.Click += new System.EventHandler(this.selectViewportButton_Click);
			// 
			// selectAssociatedViewNameTextBox
			// 
			this.selectAssociatedViewNameTextBox.Location = new System.Drawing.Point(602, 32);
			this.selectAssociatedViewNameTextBox.Name = "selectAssociatedViewNameTextBox";
			this.selectAssociatedViewNameTextBox.Size = new System.Drawing.Size(205, 31);
			this.selectAssociatedViewNameTextBox.TabIndex = 51;
			this.selectAssociatedViewNameTextBox.Text = "Level 1";
			this.selectAssociatedViewNameTextBox.TextChanged += new System.EventHandler(this.selectViewportNameTextBox_TextChanged);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(364, 35);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(232, 25);
			this.label4.TabIndex = 50;
			this.label4.Text = "Associated View Name";
			// 
			// AllViewsForm
			// 
			this.AcceptButton = this.oKButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(1109, 1292);
			this.Controls.Add(this.ViewportPropertiesGroupBox2);
			this.Controls.Add(this.CreateNewSheetAndViewportsGroupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Margin = new System.Windows.Forms.Padding(6);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AllViewsForm";
			this.ShowInTaskbar = false;
			this.Text = "All Views";
			this.Load += new System.EventHandler(this.AllViewsForm_Load);
			this.GenerateSheetGroupBox.ResumeLayout(false);
			this.GenerateSheetGroupBox.PerformLayout();
			this.groupBoxRotation.ResumeLayout(false);
			this.groupBoxRotation.PerformLayout();
			this.allViewsGroupBox.ResumeLayout(false);
			this.CreateNewSheetAndViewportsGroupBox1.ResumeLayout(false);
			this.ViewportPropertiesGroupBox2.ResumeLayout(false);
			this.groupBox8.ResumeLayout(false);
			this.groupBox8.PerformLayout();
			this.groupBox7.ResumeLayout(false);
			this.groupBox7.PerformLayout();
			this.groupBox6.ResumeLayout(false);
			this.groupBox6.PerformLayout();
			this.groupBox5.ResumeLayout(false);
			this.groupBox5.PerformLayout();
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox GenerateSheetGroupBox;
        private System.Windows.Forms.Button oKButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.TreeView allViewsTreeView;
        private System.Windows.Forms.Label titleBlocksLabel;
        private System.Windows.Forms.TextBox sheetNameTextBox;
        private System.Windows.Forms.Label sheetNameLabel;
        private System.Windows.Forms.ListBox titleBlocksListBox;
      private System.Windows.Forms.TextBox setLabelOffsetXTextBox;
      private System.Windows.Forms.TextBox setLabelOffsetYTextBox;
      private System.Windows.Forms.TextBox setLabelLineLengthTextBox;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.TextBox getLabelLineLengthTextBox;
      private System.Windows.Forms.TextBox selectSheetNameTextBox;
      private System.Windows.Forms.Label label6;
      private System.Windows.Forms.GroupBox groupBoxRotation;
      private System.Windows.Forms.RadioButton counterClockWiseRadioButton;
      private System.Windows.Forms.RadioButton clockWiseRadioButton;
      private System.Windows.Forms.RadioButton noneRadioButton;
      private System.Windows.Forms.Label labelUpdateOffset;
      private System.Windows.Forms.GroupBox allViewsGroupBox;
      private System.Windows.Forms.GroupBox CreateNewSheetAndViewportsGroupBox1;
      private System.Windows.Forms.GroupBox ViewportPropertiesGroupBox2;
      private System.Windows.Forms.TextBox selectAssociatedViewNameTextBox;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.GroupBox groupBox1;
      private System.Windows.Forms.GroupBox groupBox4;
      private System.Windows.Forms.GroupBox groupBox3;
      private System.Windows.Forms.Label label7;
      private System.Windows.Forms.Label label5;
      private System.Windows.Forms.GroupBox groupBox2;
      private System.Windows.Forms.GroupBox groupBox6;
      private System.Windows.Forms.Label label12;
      private System.Windows.Forms.Label label13;
      private System.Windows.Forms.TextBox getMaxLabelOutlineTextBox;
      private System.Windows.Forms.TextBox getMinLabelOutlineTextBox;
      private System.Windows.Forms.GroupBox groupBox5;
      private System.Windows.Forms.Label label11;
      private System.Windows.Forms.Label label10;
      private System.Windows.Forms.TextBox getMaxBoxOutlineTextBox;
      private System.Windows.Forms.TextBox getMinBoxOutlineTextBox;
      private System.Windows.Forms.GroupBox groupBox7;
      private System.Windows.Forms.TextBox getLabelLineOffsetTextBox;
      private System.Windows.Forms.Button setLabelOffsetButton;
      private System.Windows.Forms.Button setRotationButton;
      private System.Windows.Forms.Button selectViewportButton;
      private System.Windows.Forms.Button setLabelLengthButton;
      private System.Windows.Forms.GroupBox groupBox8;
      private System.Windows.Forms.TextBox getOrientationTtextBox;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.TextBox getBoxCenterTextBox;
      private System.Windows.Forms.Label label1;
   }
}