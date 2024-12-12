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

namespace Revit.SDK.Samples.FrameBuilder.CS
{
    partial class CreateFrameForm
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
            this.unitLabel = new System.Windows.Forms.Label();
            this.cancelButton = new System.Windows.Forms.Button();
            this.floornumberLabel = new System.Windows.Forms.Label();
            this.XLabel = new System.Windows.Forms.Label();
            this.YLabel = new System.Windows.Forms.Label();
            this.DistanceLabel = new System.Windows.Forms.Label();
            this.floorNumberTextBox = new System.Windows.Forms.TextBox();
            this.distanceTextBox = new System.Windows.Forms.TextBox();
            this.yNumberTextBox = new System.Windows.Forms.TextBox();
            this.xNumberTextBox = new System.Windows.Forms.TextBox();
            this.braceLabel = new System.Windows.Forms.Label();
            this.beamLabel = new System.Windows.Forms.Label();
            this.columnLabel = new System.Windows.Forms.Label();
            this.braceTypeComboBox = new System.Windows.Forms.ComboBox();
            this.beamTypeComboBox = new System.Windows.Forms.ComboBox();
            this.columnTypeComboBox = new System.Windows.Forms.ComboBox();
            this.OKButton = new System.Windows.Forms.Button();
            this.columnDuplicateButton = new System.Windows.Forms.Button();
            this.beamDuplicateButton = new System.Windows.Forms.Button();
            this.braceDuplicateButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.levelHeightTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.originXtextBox = new System.Windows.Forms.TextBox();
            this.originYtextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.originAngletextBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // unitLabel
            // 
            this.unitLabel.Location = new System.Drawing.Point(133, 36);
            this.unitLabel.Name = "unitLabel";
            this.unitLabel.Size = new System.Drawing.Size(32, 18);
            this.unitLabel.TabIndex = 35;
            this.unitLabel.Text = "feet";
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(516, 324);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 27;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // floornumberLabel
            // 
            this.floornumberLabel.Location = new System.Drawing.Point(12, 177);
            this.floornumberLabel.Name = "floornumberLabel";
            this.floornumberLabel.Size = new System.Drawing.Size(144, 21);
            this.floornumberLabel.TabIndex = 34;
            this.floornumberLabel.Text = "Number of Floors:";
            // 
            // XLabel
            // 
            this.XLabel.Location = new System.Drawing.Point(12, 65);
            this.XLabel.Name = "XLabel";
            this.XLabel.Size = new System.Drawing.Size(200, 21);
            this.XLabel.TabIndex = 33;
            this.XLabel.Text = "Number of Columns in the X Direction:";
            // 
            // YLabel
            // 
            this.YLabel.Location = new System.Drawing.Point(12, 121);
            this.YLabel.Name = "YLabel";
            this.YLabel.Size = new System.Drawing.Size(200, 21);
            this.YLabel.TabIndex = 32;
            this.YLabel.Text = "Number of Columns in the Y Direction:";
            // 
            // DistanceLabel
            // 
            this.DistanceLabel.Location = new System.Drawing.Point(12, 9);
            this.DistanceLabel.Name = "DistanceLabel";
            this.DistanceLabel.Size = new System.Drawing.Size(152, 21);
            this.DistanceLabel.TabIndex = 31;
            this.DistanceLabel.Text = "Distance between Columns:";
            // 
            // floorNumberTextBox
            // 
            this.floorNumberTextBox.Location = new System.Drawing.Point(15, 200);
            this.floorNumberTextBox.Name = "floorNumberTextBox";
            this.floorNumberTextBox.Size = new System.Drawing.Size(112, 20);
            this.floorNumberTextBox.TabIndex = 22;
            this.floorNumberTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.floorNumberTextBox_Validating);
            // 
            // distanceTextBox
            // 
            this.distanceTextBox.Location = new System.Drawing.Point(15, 33);
            this.distanceTextBox.Name = "distanceTextBox";
            this.distanceTextBox.Size = new System.Drawing.Size(112, 20);
            this.distanceTextBox.TabIndex = 19;
            this.distanceTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.distanceTextBox_Validating);
            // 
            // yNumberTextBox
            // 
            this.yNumberTextBox.Location = new System.Drawing.Point(15, 145);
            this.yNumberTextBox.Name = "yNumberTextBox";
            this.yNumberTextBox.Size = new System.Drawing.Size(112, 20);
            this.yNumberTextBox.TabIndex = 21;
            this.yNumberTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.yNumberTextBox_Validating);
            // 
            // xNumberTextBox
            // 
            this.xNumberTextBox.Location = new System.Drawing.Point(15, 89);
            this.xNumberTextBox.Name = "xNumberTextBox";
            this.xNumberTextBox.Size = new System.Drawing.Size(112, 20);
            this.xNumberTextBox.TabIndex = 20;
            this.xNumberTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.xNumberTextBox_Validating);
            // 
            // braceLabel
            // 
            this.braceLabel.Location = new System.Drawing.Point(214, 121);
            this.braceLabel.Name = "braceLabel";
            this.braceLabel.Size = new System.Drawing.Size(120, 20);
            this.braceLabel.TabIndex = 30;
            this.braceLabel.Text = "Type of Braces:";
            // 
            // beamLabel
            // 
            this.beamLabel.Location = new System.Drawing.Point(214, 64);
            this.beamLabel.Name = "beamLabel";
            this.beamLabel.Size = new System.Drawing.Size(120, 21);
            this.beamLabel.TabIndex = 29;
            this.beamLabel.Text = "Type of Beams:";
            // 
            // columnLabel
            // 
            this.columnLabel.Location = new System.Drawing.Point(214, 9);
            this.columnLabel.Name = "columnLabel";
            this.columnLabel.Size = new System.Drawing.Size(120, 20);
            this.columnLabel.TabIndex = 28;
            this.columnLabel.Text = "Type of Columns:";
            // 
            // braceTypeComboBox
            // 
            this.braceTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.braceTypeComboBox.Location = new System.Drawing.Point(214, 144);
            this.braceTypeComboBox.MaxDropDownItems = 10;
            this.braceTypeComboBox.Name = "braceTypeComboBox";
            this.braceTypeComboBox.Size = new System.Drawing.Size(281, 21);
            this.braceTypeComboBox.TabIndex = 25;
            this.braceTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.braceTypeComboBox_SelectedIndexChanged);
            // 
            // beamTypeComboBox
            // 
            this.beamTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.beamTypeComboBox.Location = new System.Drawing.Point(214, 88);
            this.beamTypeComboBox.MaxDropDownItems = 10;
            this.beamTypeComboBox.Name = "beamTypeComboBox";
            this.beamTypeComboBox.Size = new System.Drawing.Size(281, 21);
            this.beamTypeComboBox.TabIndex = 24;
            this.beamTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.beamTypeComboBox_SelectedIndexChanged);
            // 
            // columnTypeComboBox
            // 
            this.columnTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.columnTypeComboBox.Location = new System.Drawing.Point(214, 32);
            this.columnTypeComboBox.MaxDropDownItems = 10;
            this.columnTypeComboBox.Name = "columnTypeComboBox";
            this.columnTypeComboBox.Size = new System.Drawing.Size(281, 21);
            this.columnTypeComboBox.TabIndex = 23;
            this.columnTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.columnTypeComboBox_SelectedIndexChanged);
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(435, 324);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 26;
            this.OKButton.Text = "&OK";
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // columnDuplicateButton
            // 
            this.columnDuplicateButton.Location = new System.Drawing.Point(501, 30);
            this.columnDuplicateButton.Name = "columnDuplicateButton";
            this.columnDuplicateButton.Size = new System.Drawing.Size(102, 24);
            this.columnDuplicateButton.TabIndex = 36;
            this.columnDuplicateButton.Text = "&ColumnDuplicate";
            this.columnDuplicateButton.UseVisualStyleBackColor = true;
            this.columnDuplicateButton.Click += new System.EventHandler(this.columnDuplicateButton_Click);
            // 
            // beamDuplicateButton
            // 
            this.beamDuplicateButton.Location = new System.Drawing.Point(501, 85);
            this.beamDuplicateButton.Name = "beamDuplicateButton";
            this.beamDuplicateButton.Size = new System.Drawing.Size(102, 24);
            this.beamDuplicateButton.TabIndex = 37;
            this.beamDuplicateButton.Text = "&BeamDuplicate";
            this.beamDuplicateButton.UseVisualStyleBackColor = true;
            this.beamDuplicateButton.Click += new System.EventHandler(this.beamDuplicateButton_Click);
            // 
            // braceDuplicateButton
            // 
            this.braceDuplicateButton.Location = new System.Drawing.Point(501, 142);
            this.braceDuplicateButton.Name = "braceDuplicateButton";
            this.braceDuplicateButton.Size = new System.Drawing.Size(102, 23);
            this.braceDuplicateButton.TabIndex = 38;
            this.braceDuplicateButton.Text = "B&raceDuplicate";
            this.braceDuplicateButton.UseVisualStyleBackColor = true;
            this.braceDuplicateButton.Click += new System.EventHandler(this.braceDuplicateButton_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 233);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(175, 21);
            this.label1.TabIndex = 40;
            this.label1.Text = "Height of Auto Generated Levels:";
            // 
            // levelHeightTextBox
            // 
            this.levelHeightTextBox.Enabled = false;
            this.levelHeightTextBox.Location = new System.Drawing.Point(15, 255);
            this.levelHeightTextBox.Name = "levelHeightTextBox";
            this.levelHeightTextBox.Size = new System.Drawing.Size(112, 20);
            this.levelHeightTextBox.TabIndex = 39;
            this.levelHeightTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.levelHeightTextBox_Validating);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(132, 258);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 18);
            this.label2.TabIndex = 41;
            this.label2.Text = "feet";
            // 
            // originXtextBox
            // 
            this.originXtextBox.Location = new System.Drawing.Point(12, 45);
            this.originXtextBox.Name = "originXtextBox";
            this.originXtextBox.Size = new System.Drawing.Size(100, 20);
            this.originXtextBox.TabIndex = 42;
            this.originXtextBox.Validating += new System.ComponentModel.CancelEventHandler(this.originXtextBox_Validating);
            // 
            // originYtextBox
            // 
            this.originYtextBox.Location = new System.Drawing.Point(156, 45);
            this.originYtextBox.Name = "originYtextBox";
            this.originYtextBox.Size = new System.Drawing.Size(100, 20);
            this.originYtextBox.TabIndex = 43;
            this.originYtextBox.Validating += new System.ComponentModel.CancelEventHandler(this.originYtextBox_Validating);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 13);
            this.label4.TabIndex = 45;
            this.label4.Text = "Origin (X)";
            // 
            // originAngletextBox
            // 
            this.originAngletextBox.Location = new System.Drawing.Point(12, 100);
            this.originAngletextBox.Name = "originAngletextBox";
            this.originAngletextBox.Size = new System.Drawing.Size(100, 20);
            this.originAngletextBox.TabIndex = 46;
            this.originAngletextBox.Validating += new System.ComponentModel.CancelEventHandler(this.originAngletextBox_Validating);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.originYtextBox);
            this.groupBox1.Controls.Add(this.originAngletextBox);
            this.groupBox1.Controls.Add(this.originXtextBox);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(214, 179);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(377, 139);
            this.groupBox1.TabIndex = 47;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Location";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(262, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 18);
            this.label3.TabIndex = 50;
            this.label3.Text = "feet";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(118, 48);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(32, 18);
            this.label7.TabIndex = 49;
            this.label7.Text = "feet,";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(161, 21);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(20, 13);
            this.label6.TabIndex = 48;
            this.label6.Text = "(Y)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 76);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 13);
            this.label5.TabIndex = 47;
            this.label5.Text = "Rotate Angle";
            // 
            // CreateFrameForm
            // 
            this.AcceptButton = this.OKButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(607, 353);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.levelHeightTextBox);
            this.Controls.Add(this.braceDuplicateButton);
            this.Controls.Add(this.beamDuplicateButton);
            this.Controls.Add(this.columnDuplicateButton);
            this.Controls.Add(this.unitLabel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.floornumberLabel);
            this.Controls.Add(this.XLabel);
            this.Controls.Add(this.YLabel);
            this.Controls.Add(this.DistanceLabel);
            this.Controls.Add(this.floorNumberTextBox);
            this.Controls.Add(this.distanceTextBox);
            this.Controls.Add(this.yNumberTextBox);
            this.Controls.Add(this.xNumberTextBox);
            this.Controls.Add(this.braceLabel);
            this.Controls.Add(this.beamLabel);
            this.Controls.Add(this.columnLabel);
            this.Controls.Add(this.braceTypeComboBox);
            this.Controls.Add(this.beamTypeComboBox);
            this.Controls.Add(this.columnTypeComboBox);
            this.Controls.Add(this.OKButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CreateFrameForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Frame Builder";
            this.Load += new System.EventHandler(this.CreateFramingForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label unitLabel;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label floornumberLabel;
        private System.Windows.Forms.Label XLabel;
        private System.Windows.Forms.Label YLabel;
        private System.Windows.Forms.Label DistanceLabel;
        private System.Windows.Forms.TextBox floorNumberTextBox;
        private System.Windows.Forms.TextBox distanceTextBox;
        private System.Windows.Forms.TextBox yNumberTextBox;
        private System.Windows.Forms.TextBox xNumberTextBox;
        private System.Windows.Forms.Label braceLabel;
        private System.Windows.Forms.Label beamLabel;
        private System.Windows.Forms.Label columnLabel;
        private System.Windows.Forms.ComboBox braceTypeComboBox;
        private System.Windows.Forms.ComboBox beamTypeComboBox;
        private System.Windows.Forms.ComboBox columnTypeComboBox;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Button columnDuplicateButton;
        private System.Windows.Forms.Button beamDuplicateButton;
        private System.Windows.Forms.Button braceDuplicateButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox levelHeightTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox originXtextBox;
        private System.Windows.Forms.TextBox originYtextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox originAngletextBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
    }
}