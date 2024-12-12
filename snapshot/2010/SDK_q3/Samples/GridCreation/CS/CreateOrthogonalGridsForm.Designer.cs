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

namespace Revit.SDK.Samples.GridCreation.CS
{
    partial class CreateOrthogonalGridsForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelYCoordUnit = new System.Windows.Forms.Label();
            this.labelXCoordUnit = new System.Windows.Forms.Label();
            this.textBoxYCoord = new System.Windows.Forms.TextBox();
            this.textBoxXCoord = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxYNumber = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxXNumber = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.labelUnitY = new System.Windows.Forms.Label();
            this.textBoxYSpacing = new System.Windows.Forms.TextBox();
            this.textBoxYFirstLabel = new System.Windows.Forms.TextBox();
            this.comboBoxYBubbleLocation = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.labelUnitX = new System.Windows.Forms.Label();
            this.textBoxXSpacing = new System.Windows.Forms.TextBox();
            this.textBoxXFirstLabel = new System.Windows.Forms.TextBox();
            this.comboBoxXBubbleLocation = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.buttonCreate = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.labelYCoordUnit);
            this.groupBox1.Controls.Add(this.labelXCoordUnit);
            this.groupBox1.Controls.Add(this.textBoxYCoord);
            this.groupBox1.Controls.Add(this.textBoxXCoord);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(13, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(520, 55);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Origin of the Grids";
            // 
            // labelYCoordUnit
            // 
            this.labelYCoordUnit.Location = new System.Drawing.Point(484, 23);
            this.labelYCoordUnit.Name = "labelYCoordUnit";
            this.labelYCoordUnit.Size = new System.Drawing.Size(23, 13);
            this.labelYCoordUnit.TabIndex = 7;
            // 
            // labelXCoordUnit
            // 
            this.labelXCoordUnit.Location = new System.Drawing.Point(227, 23);
            this.labelXCoordUnit.Name = "labelXCoordUnit";
            this.labelXCoordUnit.Size = new System.Drawing.Size(23, 13);
            this.labelXCoordUnit.TabIndex = 7;
            // 
            // textBoxYCoord
            // 
            this.textBoxYCoord.Location = new System.Drawing.Point(385, 20);
            this.textBoxYCoord.Name = "textBoxYCoord";
            this.textBoxYCoord.Size = new System.Drawing.Size(98, 20);
            this.textBoxYCoord.TabIndex = 1;
            this.textBoxYCoord.Tag = "0";
            this.textBoxYCoord.Text = "0";
            // 
            // textBoxXCoord
            // 
            this.textBoxXCoord.Location = new System.Drawing.Point(109, 20);
            this.textBoxXCoord.Name = "textBoxXCoord";
            this.textBoxXCoord.Size = new System.Drawing.Size(112, 20);
            this.textBoxXCoord.TabIndex = 0;
            this.textBoxXCoord.Tag = "0";
            this.textBoxXCoord.Text = "0";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(261, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(122, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Y coordinate:";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(7, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "X coordinate:";
            // 
            // textBoxYNumber
            // 
            this.textBoxYNumber.Location = new System.Drawing.Point(385, 24);
            this.textBoxYNumber.Name = "textBoxYNumber";
            this.textBoxYNumber.Size = new System.Drawing.Size(120, 20);
            this.textBoxYNumber.TabIndex = 1;
            this.textBoxYNumber.Tag = "3";
            this.textBoxYNumber.Text = "3";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(261, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(122, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Number:";
            // 
            // textBoxXNumber
            // 
            this.textBoxXNumber.Location = new System.Drawing.Point(385, 23);
            this.textBoxXNumber.Name = "textBoxXNumber";
            this.textBoxXNumber.Size = new System.Drawing.Size(120, 20);
            this.textBoxXNumber.TabIndex = 1;
            this.textBoxXNumber.Tag = "3";
            this.textBoxXNumber.Text = "3";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.labelUnitY);
            this.groupBox2.Controls.Add(this.textBoxYNumber);
            this.groupBox2.Controls.Add(this.textBoxYSpacing);
            this.groupBox2.Controls.Add(this.textBoxYFirstLabel);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.comboBoxYBubbleLocation);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Location = new System.Drawing.Point(14, 165);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(519, 85);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Y direction Grids";
            // 
            // labelUnitY
            // 
            this.labelUnitY.Location = new System.Drawing.Point(227, 26);
            this.labelUnitY.Name = "labelUnitY";
            this.labelUnitY.Size = new System.Drawing.Size(23, 13);
            this.labelUnitY.TabIndex = 7;
            // 
            // textBoxYSpacing
            // 
            this.textBoxYSpacing.Location = new System.Drawing.Point(110, 24);
            this.textBoxYSpacing.Name = "textBoxYSpacing";
            this.textBoxYSpacing.Size = new System.Drawing.Size(112, 20);
            this.textBoxYSpacing.TabIndex = 0;
            this.textBoxYSpacing.Tag = "10.0";
            this.textBoxYSpacing.Text = "10.0";
            // 
            // textBoxYFirstLabel
            // 
            this.textBoxYFirstLabel.Location = new System.Drawing.Point(385, 53);
            this.textBoxYFirstLabel.Name = "textBoxYFirstLabel";
            this.textBoxYFirstLabel.Size = new System.Drawing.Size(120, 20);
            this.textBoxYFirstLabel.TabIndex = 3;
            this.textBoxYFirstLabel.Tag = "A";
            this.textBoxYFirstLabel.Text = "A";
            // 
            // comboBoxYBubbleLocation
            // 
            this.comboBoxYBubbleLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxYBubbleLocation.FormattingEnabled = true;
            this.comboBoxYBubbleLocation.Items.AddRange(new object[] {
            "At start point of lines",
            "At end point of lines"});
            this.comboBoxYBubbleLocation.Location = new System.Drawing.Point(108, 53);
            this.comboBoxYBubbleLocation.Name = "comboBoxYBubbleLocation";
            this.comboBoxYBubbleLocation.Size = new System.Drawing.Size(146, 21);
            this.comboBoxYBubbleLocation.TabIndex = 2;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(7, 55);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(95, 13);
            this.label10.TabIndex = 6;
            this.label10.Text = "Bubble location:";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(6, 26);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(95, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Spacing:";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(261, 55);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(122, 13);
            this.label9.TabIndex = 6;
            this.label9.Text = "Label of first grid:";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(261, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(122, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Number:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.labelUnitX);
            this.groupBox3.Controls.Add(this.textBoxXNumber);
            this.groupBox3.Controls.Add(this.textBoxXSpacing);
            this.groupBox3.Controls.Add(this.textBoxXFirstLabel);
            this.groupBox3.Controls.Add(this.comboBoxXBubbleLocation);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Location = new System.Drawing.Point(13, 73);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(520, 86);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "X direction Grids";
            // 
            // labelUnitX
            // 
            this.labelUnitX.Location = new System.Drawing.Point(227, 25);
            this.labelUnitX.Name = "labelUnitX";
            this.labelUnitX.Size = new System.Drawing.Size(23, 13);
            this.labelUnitX.TabIndex = 7;
            // 
            // textBoxXSpacing
            // 
            this.textBoxXSpacing.Location = new System.Drawing.Point(109, 23);
            this.textBoxXSpacing.Name = "textBoxXSpacing";
            this.textBoxXSpacing.Size = new System.Drawing.Size(112, 20);
            this.textBoxXSpacing.TabIndex = 0;
            this.textBoxXSpacing.Tag = "10.0";
            this.textBoxXSpacing.Text = "10.0";
            // 
            // textBoxXFirstLabel
            // 
            this.textBoxXFirstLabel.Location = new System.Drawing.Point(385, 53);
            this.textBoxXFirstLabel.Name = "textBoxXFirstLabel";
            this.textBoxXFirstLabel.Size = new System.Drawing.Size(120, 20);
            this.textBoxXFirstLabel.TabIndex = 3;
            this.textBoxXFirstLabel.Tag = "";
            this.textBoxXFirstLabel.Text = "1";
            // 
            // comboBoxXBubbleLocation
            // 
            this.comboBoxXBubbleLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxXBubbleLocation.FormattingEnabled = true;
            this.comboBoxXBubbleLocation.Items.AddRange(new object[] {
            "At start point of lines",
            "At end point of lines"});
            this.comboBoxXBubbleLocation.Location = new System.Drawing.Point(108, 53);
            this.comboBoxXBubbleLocation.Name = "comboBoxXBubbleLocation";
            this.comboBoxXBubbleLocation.Size = new System.Drawing.Size(147, 21);
            this.comboBoxXBubbleLocation.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(7, 25);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(95, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Spacing:";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(7, 55);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(95, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Bubble location:";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(261, 55);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(122, 13);
            this.label8.TabIndex = 6;
            this.label8.Text = "Label of first grid:";
            // 
            // buttonCreate
            // 
            this.buttonCreate.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonCreate.Location = new System.Drawing.Point(327, 269);
            this.buttonCreate.Name = "buttonCreate";
            this.buttonCreate.Size = new System.Drawing.Size(94, 23);
            this.buttonCreate.TabIndex = 3;
            this.buttonCreate.Text = "Create &Grids";
            this.buttonCreate.UseVisualStyleBackColor = true;
            this.buttonCreate.Click += new System.EventHandler(this.buttonCreate_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(439, 269);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(94, 23);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "&Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // CreateOrthogonalGridsForm
            // 
            this.AcceptButton = this.buttonCreate;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(545, 304);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonCreate);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CreateOrthogonalGridsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Create Orthogonal Grids";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBoxYCoord;
        private System.Windows.Forms.TextBox textBoxXCoord;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxYNumber;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxXNumber;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox textBoxXSpacing;
        private System.Windows.Forms.TextBox textBoxYSpacing;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBoxXFirstLabel;
        private System.Windows.Forms.ComboBox comboBoxXBubbleLocation;
        private System.Windows.Forms.Button buttonCreate;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.TextBox textBoxYFirstLabel;
        private System.Windows.Forms.ComboBox comboBoxYBubbleLocation;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label labelUnitY;
        private System.Windows.Forms.Label labelUnitX;
        private System.Windows.Forms.Label labelYCoordUnit;
        private System.Windows.Forms.Label labelXCoordUnit;
    }
}