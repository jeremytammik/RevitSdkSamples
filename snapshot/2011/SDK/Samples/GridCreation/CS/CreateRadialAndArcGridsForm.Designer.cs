//
// (C) Copyright 2003-2010 by Autodesk, Inc.
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

namespace Revit.SDK.Samples.GridCreation.CS
{
    /// <summary>
    /// The dialog which provides the options of creating radial and arc grids
    /// </summary>
    partial class CreateRadialAndArcGridsForm
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
            this.textBoxArcSpacing = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.labelUnitFirstRadius = new System.Windows.Forms.Label();
            this.labelUnitX = new System.Windows.Forms.Label();
            this.textBoxArcFirstRadius = new System.Windows.Forms.TextBox();
            this.comboBoxArcBubbleLocation = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.textBoxArcFirstLabel = new System.Windows.Forms.TextBox();
            this.textBoxArcNumber = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxYCoord = new System.Windows.Forms.TextBox();
            this.textBoxXCoord = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.labelUnitY = new System.Windows.Forms.Label();
            this.textBoxLineFirstDistance = new System.Windows.Forms.TextBox();
            this.comboBoxLineBubbleLocation = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.textBoxLineNumber = new System.Windows.Forms.TextBox();
            this.textBoxLineFirstLabel = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelYCoordUnit = new System.Windows.Forms.Label();
            this.labelXCoordUnit = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonCreate = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.textBoxEndDegree = new System.Windows.Forms.TextBox();
            this.textBoxStartDegree = new System.Windows.Forms.TextBox();
            this.labelEndDegree = new System.Windows.Forms.Label();
            this.labelStartDegree = new System.Windows.Forms.Label();
            this.radioButtonCustomize = new System.Windows.Forms.RadioButton();
            this.radioButton360 = new System.Windows.Forms.RadioButton();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxArcSpacing
            // 
            this.textBoxArcSpacing.Location = new System.Drawing.Point(132, 17);
            this.textBoxArcSpacing.Name = "textBoxArcSpacing";
            this.textBoxArcSpacing.Size = new System.Drawing.Size(108, 20);
            this.textBoxArcSpacing.TabIndex = 0;
            this.textBoxArcSpacing.Tag = "10.0";
            this.textBoxArcSpacing.Text = "10.0";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(13, 20);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(113, 18);
            this.label6.TabIndex = 6;
            this.label6.Text = "Spacing:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.labelUnitFirstRadius);
            this.groupBox3.Controls.Add(this.labelUnitX);
            this.groupBox3.Controls.Add(this.textBoxArcFirstRadius);
            this.groupBox3.Controls.Add(this.comboBoxArcBubbleLocation);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.textBoxArcFirstLabel);
            this.groupBox3.Controls.Add(this.textBoxArcNumber);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.textBoxArcSpacing);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Location = new System.Drawing.Point(12, 175);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(544, 116);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Arc Grids";
            // 
            // labelUnitFirstRadius
            // 
            this.labelUnitFirstRadius.Location = new System.Drawing.Point(240, 52);
            this.labelUnitFirstRadius.Name = "labelUnitFirstRadius";
            this.labelUnitFirstRadius.Size = new System.Drawing.Size(23, 23);
            this.labelUnitFirstRadius.TabIndex = 31;
            // 
            // labelUnitX
            // 
            this.labelUnitX.Location = new System.Drawing.Point(240, 20);
            this.labelUnitX.Name = "labelUnitX";
            this.labelUnitX.Size = new System.Drawing.Size(23, 23);
            this.labelUnitX.TabIndex = 13;
            // 
            // textBoxArcFirstRadius
            // 
            this.textBoxArcFirstRadius.Location = new System.Drawing.Point(132, 50);
            this.textBoxArcFirstRadius.Name = "textBoxArcFirstRadius";
            this.textBoxArcFirstRadius.Size = new System.Drawing.Size(108, 20);
            this.textBoxArcFirstRadius.TabIndex = 2;
            this.textBoxArcFirstRadius.Text = "10.0";
            // 
            // comboBoxArcBubbleLocation
            // 
            this.comboBoxArcBubbleLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxArcBubbleLocation.FormattingEnabled = true;
            this.comboBoxArcBubbleLocation.Items.AddRange(new object[] {
            "At start point of arcs",
            "At end point of arcs"});
            this.comboBoxArcBubbleLocation.Location = new System.Drawing.Point(133, 83);
            this.comboBoxArcBubbleLocation.Name = "comboBoxArcBubbleLocation";
            this.comboBoxArcBubbleLocation.Size = new System.Drawing.Size(373, 21);
            this.comboBoxArcBubbleLocation.TabIndex = 4;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(13, 52);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(113, 18);
            this.label10.TabIndex = 7;
            this.label10.Text = "Radius of first grid:";
            // 
            // textBoxArcFirstLabel
            // 
            this.textBoxArcFirstLabel.Location = new System.Drawing.Point(398, 50);
            this.textBoxArcFirstLabel.Name = "textBoxArcFirstLabel";
            this.textBoxArcFirstLabel.Size = new System.Drawing.Size(108, 20);
            this.textBoxArcFirstLabel.TabIndex = 3;
            this.textBoxArcFirstLabel.Tag = "";
            this.textBoxArcFirstLabel.Text = "1";
            // 
            // textBoxArcNumber
            // 
            this.textBoxArcNumber.Location = new System.Drawing.Point(398, 17);
            this.textBoxArcNumber.Name = "textBoxArcNumber";
            this.textBoxArcNumber.Size = new System.Drawing.Size(108, 20);
            this.textBoxArcNumber.TabIndex = 1;
            this.textBoxArcNumber.Text = "3";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(279, 52);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(113, 18);
            this.label9.TabIndex = 30;
            this.label9.Text = "Label of first grid:";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(13, 85);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(113, 18);
            this.label5.TabIndex = 29;
            this.label5.Text = "Bubble location:";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(279, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 18);
            this.label3.TabIndex = 6;
            this.label3.Text = "Number:";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(279, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(113, 18);
            this.label4.TabIndex = 6;
            this.label4.Text = "Number:";
            // 
            // textBoxYCoord
            // 
            this.textBoxYCoord.Location = new System.Drawing.Point(398, 22);
            this.textBoxYCoord.Name = "textBoxYCoord";
            this.textBoxYCoord.Size = new System.Drawing.Size(108, 20);
            this.textBoxYCoord.TabIndex = 1;
            this.textBoxYCoord.Text = "0";
            // 
            // textBoxXCoord
            // 
            this.textBoxXCoord.Location = new System.Drawing.Point(132, 21);
            this.textBoxXCoord.Name = "textBoxXCoord";
            this.textBoxXCoord.Size = new System.Drawing.Size(108, 20);
            this.textBoxXCoord.TabIndex = 0;
            this.textBoxXCoord.Text = "0";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(279, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 18);
            this.label2.TabIndex = 0;
            this.label2.Text = "Y coordinate:";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(13, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "X coordinate:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.labelUnitY);
            this.groupBox2.Controls.Add(this.textBoxLineFirstDistance);
            this.groupBox2.Controls.Add(this.comboBoxLineBubbleLocation);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.textBoxLineNumber);
            this.groupBox2.Controls.Add(this.textBoxLineFirstLabel);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Location = new System.Drawing.Point(13, 297);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(543, 119);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Radial Grids";
            // 
            // labelUnitY
            // 
            this.labelUnitY.Location = new System.Drawing.Point(508, 88);
            this.labelUnitY.Name = "labelUnitY";
            this.labelUnitY.Size = new System.Drawing.Size(23, 23);
            this.labelUnitY.TabIndex = 13;
            // 
            // textBoxLineFirstDistance
            // 
            this.textBoxLineFirstDistance.Location = new System.Drawing.Point(236, 86);
            this.textBoxLineFirstDistance.Name = "textBoxLineFirstDistance";
            this.textBoxLineFirstDistance.Size = new System.Drawing.Size(270, 20);
            this.textBoxLineFirstDistance.TabIndex = 3;
            this.textBoxLineFirstDistance.Text = "8.0";
            // 
            // comboBoxLineBubbleLocation
            // 
            this.comboBoxLineBubbleLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLineBubbleLocation.FormattingEnabled = true;
            this.comboBoxLineBubbleLocation.Items.AddRange(new object[] {
            "At start point of lines",
            "At end point of lines"});
            this.comboBoxLineBubbleLocation.Location = new System.Drawing.Point(131, 54);
            this.comboBoxLineBubbleLocation.Name = "comboBoxLineBubbleLocation";
            this.comboBoxLineBubbleLocation.Size = new System.Drawing.Size(374, 21);
            this.comboBoxLineBubbleLocation.TabIndex = 2;
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(6, 88);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(224, 18);
            this.label11.TabIndex = 7;
            this.label11.Text = "Distance from origin to start point:";
            // 
            // textBoxLineNumber
            // 
            this.textBoxLineNumber.Location = new System.Drawing.Point(398, 23);
            this.textBoxLineNumber.Name = "textBoxLineNumber";
            this.textBoxLineNumber.Size = new System.Drawing.Size(108, 20);
            this.textBoxLineNumber.TabIndex = 1;
            this.textBoxLineNumber.Tag = "3";
            this.textBoxLineNumber.Text = "3";
            // 
            // textBoxLineFirstLabel
            // 
            this.textBoxLineFirstLabel.Location = new System.Drawing.Point(131, 20);
            this.textBoxLineFirstLabel.Name = "textBoxLineFirstLabel";
            this.textBoxLineFirstLabel.Size = new System.Drawing.Size(108, 20);
            this.textBoxLineFirstLabel.TabIndex = 0;
            this.textBoxLineFirstLabel.Tag = "A";
            this.textBoxLineFirstLabel.Text = "A";
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(6, 23);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(119, 18);
            this.label13.TabIndex = 30;
            this.label13.Text = "Label of first grid:";
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(6, 57);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(119, 18);
            this.label12.TabIndex = 29;
            this.label12.Text = "Bubble location:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBoxYCoord);
            this.groupBox1.Controls.Add(this.labelYCoordUnit);
            this.groupBox1.Controls.Add(this.labelXCoordUnit);
            this.groupBox1.Controls.Add(this.textBoxXCoord);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(13, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(543, 55);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Center of Arc Grids";
            // 
            // labelYCoordUnit
            // 
            this.labelYCoordUnit.Location = new System.Drawing.Point(508, 22);
            this.labelYCoordUnit.Name = "labelYCoordUnit";
            this.labelYCoordUnit.Size = new System.Drawing.Size(23, 23);
            this.labelYCoordUnit.TabIndex = 13;
            // 
            // labelXCoordUnit
            // 
            this.labelXCoordUnit.Location = new System.Drawing.Point(240, 24);
            this.labelXCoordUnit.Name = "labelXCoordUnit";
            this.labelXCoordUnit.Size = new System.Drawing.Size(23, 23);
            this.labelXCoordUnit.TabIndex = 13;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(463, 436);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(94, 23);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "&Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonCreate
            // 
            this.buttonCreate.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonCreate.Location = new System.Drawing.Point(356, 436);
            this.buttonCreate.Name = "buttonCreate";
            this.buttonCreate.Size = new System.Drawing.Size(94, 23);
            this.buttonCreate.TabIndex = 4;
            this.buttonCreate.Text = "Create &Grids";
            this.buttonCreate.UseVisualStyleBackColor = true;
            this.buttonCreate.Click += new System.EventHandler(this.buttonCreate_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.textBoxEndDegree);
            this.groupBox4.Controls.Add(this.textBoxStartDegree);
            this.groupBox4.Controls.Add(this.labelEndDegree);
            this.groupBox4.Controls.Add(this.labelStartDegree);
            this.groupBox4.Controls.Add(this.radioButtonCustomize);
            this.groupBox4.Controls.Add(this.radioButton360);
            this.groupBox4.Location = new System.Drawing.Point(13, 74);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(543, 95);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Span of Grids";
            // 
            // textBoxEndDegree
            // 
            this.textBoxEndDegree.Location = new System.Drawing.Point(397, 62);
            this.textBoxEndDegree.Name = "textBoxEndDegree";
            this.textBoxEndDegree.Size = new System.Drawing.Size(108, 20);
            this.textBoxEndDegree.TabIndex = 3;
            this.textBoxEndDegree.Text = "360";
            // 
            // textBoxStartDegree
            // 
            this.textBoxStartDegree.Location = new System.Drawing.Point(131, 62);
            this.textBoxStartDegree.Name = "textBoxStartDegree";
            this.textBoxStartDegree.Size = new System.Drawing.Size(108, 20);
            this.textBoxStartDegree.TabIndex = 2;
            this.textBoxStartDegree.Text = "0";
            // 
            // labelEndDegree
            // 
            this.labelEndDegree.Location = new System.Drawing.Point(279, 64);
            this.labelEndDegree.Name = "labelEndDegree";
            this.labelEndDegree.Size = new System.Drawing.Size(113, 18);
            this.labelEndDegree.TabIndex = 2;
            this.labelEndDegree.Text = "End degree:";
            // 
            // labelStartDegree
            // 
            this.labelStartDegree.Location = new System.Drawing.Point(24, 64);
            this.labelStartDegree.Name = "labelStartDegree";
            this.labelStartDegree.Size = new System.Drawing.Size(101, 18);
            this.labelStartDegree.TabIndex = 2;
            this.labelStartDegree.Text = "Start degree:";
            // 
            // radioButtonCustomize
            // 
            this.radioButtonCustomize.Location = new System.Drawing.Point(9, 41);
            this.radioButtonCustomize.Name = "radioButtonCustomize";
            this.radioButtonCustomize.Size = new System.Drawing.Size(104, 24);
            this.radioButtonCustomize.TabIndex = 1;
            this.radioButtonCustomize.Text = "Customize";
            this.radioButtonCustomize.UseVisualStyleBackColor = true;
            this.radioButtonCustomize.CheckedChanged += new System.EventHandler(this.radioButtonCustomize_CheckedChanged);
            // 
            // radioButton360
            // 
            this.radioButton360.AutoSize = true;
            this.radioButton360.Checked = true;
            this.radioButton360.Location = new System.Drawing.Point(9, 20);
            this.radioButton360.Name = "radioButton360";
            this.radioButton360.Size = new System.Drawing.Size(79, 17);
            this.radioButton360.TabIndex = 0;
            this.radioButton360.TabStop = true;
            this.radioButton360.Text = "360 degree";
            this.radioButton360.UseVisualStyleBackColor = true;
            this.radioButton360.MouseClick += new System.Windows.Forms.MouseEventHandler(this.radioButton360_MouseClick);
            // 
            // CreateRadialAndArcGridsForm
            // 
            this.AcceptButton = this.buttonCreate;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(568, 466);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonCreate);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CreateRadialAndArcGridsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Create Radial and Arc Grids";
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxArcSpacing;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxYCoord;
        private System.Windows.Forms.TextBox textBoxXCoord;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBoxLineNumber;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBoxArcNumber;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonCreate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox textBoxEndDegree;
        private System.Windows.Forms.TextBox textBoxStartDegree;
        private System.Windows.Forms.Label labelEndDegree;
        private System.Windows.Forms.Label labelStartDegree;
        private System.Windows.Forms.RadioButton radioButtonCustomize;
        private System.Windows.Forms.RadioButton radioButton360;
        private System.Windows.Forms.TextBox textBoxArcFirstRadius;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBoxLineFirstDistance;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox comboBoxArcBubbleLocation;
        private System.Windows.Forms.TextBox textBoxArcFirstLabel;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBoxLineBubbleLocation;
        private System.Windows.Forms.TextBox textBoxLineFirstLabel;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label labelUnitX;
        private System.Windows.Forms.Label labelUnitY;
        private System.Windows.Forms.Label labelUnitFirstRadius;
        private System.Windows.Forms.Label labelYCoordUnit;
        private System.Windows.Forms.Label labelXCoordUnit;


    }
}