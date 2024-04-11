//
// (C) Copyright 2003-2007 by Autodesk, Inc.
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
         this.labelspace = new System.Windows.Forms.Label();
         this.groupBox3 = new System.Windows.Forms.GroupBox();
         this.labelUnitFirstRadius = new System.Windows.Forms.Label();
         this.labelUnitX = new System.Windows.Forms.Label();
         this.textBoxArcFirstRadius = new System.Windows.Forms.TextBox();
         this.comboBoxArcBubbleLocation = new System.Windows.Forms.ComboBox();
         this.labelradius = new System.Windows.Forms.Label();
         this.textBoxArcFirstLabel = new System.Windows.Forms.TextBox();
         this.textBoxArcNumber = new System.Windows.Forms.TextBox();
         this.labelfirstgrid = new System.Windows.Forms.Label();
         this.labelbubble = new System.Windows.Forms.Label();
         this.labelnumber = new System.Windows.Forms.Label();
         this.labelr_number = new System.Windows.Forms.Label();
         this.textBoxYCoord = new System.Windows.Forms.TextBox();
         this.textBoxXCoord = new System.Windows.Forms.TextBox();
         this.labely = new System.Windows.Forms.Label();
         this.labelx = new System.Windows.Forms.Label();
         this.groupBox2 = new System.Windows.Forms.GroupBox();
         this.labelUnitY = new System.Windows.Forms.Label();
         this.textBoxLineFirstDistance = new System.Windows.Forms.TextBox();
         this.comboBoxLineBubbleLocation = new System.Windows.Forms.ComboBox();
         this.label1r_distance = new System.Windows.Forms.Label();
         this.textBoxLineNumber = new System.Windows.Forms.TextBox();
         this.textBoxLineFirstLabel = new System.Windows.Forms.TextBox();
         this.label1r_firstgrid = new System.Windows.Forms.Label();
         this.labelr_bubble = new System.Windows.Forms.Label();
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
         // labelspace
         // 
         this.labelspace.Location = new System.Drawing.Point(13, 20);
         this.labelspace.Name = "labelspace";
         this.labelspace.Size = new System.Drawing.Size(113, 18);
         this.labelspace.TabIndex = 6;
         this.labelspace.Text = "Spacing:";
         // 
         // groupBox3
         // 
         this.groupBox3.Controls.Add(this.labelUnitFirstRadius);
         this.groupBox3.Controls.Add(this.labelUnitX);
         this.groupBox3.Controls.Add(this.textBoxArcFirstRadius);
         this.groupBox3.Controls.Add(this.comboBoxArcBubbleLocation);
         this.groupBox3.Controls.Add(this.labelradius);
         this.groupBox3.Controls.Add(this.textBoxArcFirstLabel);
         this.groupBox3.Controls.Add(this.textBoxArcNumber);
         this.groupBox3.Controls.Add(this.labelfirstgrid);
         this.groupBox3.Controls.Add(this.textBoxArcSpacing);
         this.groupBox3.Controls.Add(this.labelbubble);
         this.groupBox3.Controls.Add(this.labelnumber);
         this.groupBox3.Controls.Add(this.labelspace);
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
         // labelradius
         // 
         this.labelradius.Location = new System.Drawing.Point(13, 52);
         this.labelradius.Name = "labelradius";
         this.labelradius.Size = new System.Drawing.Size(113, 18);
         this.labelradius.TabIndex = 7;
         this.labelradius.Text = "Radius of first grid:";
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
         // labelfirstgrid
         // 
         this.labelfirstgrid.Location = new System.Drawing.Point(279, 52);
         this.labelfirstgrid.Name = "labelfirstgrid";
         this.labelfirstgrid.Size = new System.Drawing.Size(113, 18);
         this.labelfirstgrid.TabIndex = 30;
         this.labelfirstgrid.Text = "Label of first grid:";
         // 
         // labelbubble
         // 
         this.labelbubble.Location = new System.Drawing.Point(13, 85);
         this.labelbubble.Name = "labelbubble";
         this.labelbubble.Size = new System.Drawing.Size(113, 18);
         this.labelbubble.TabIndex = 29;
         this.labelbubble.Text = "Bubble location:";
         // 
         // labelnumber
         // 
         this.labelnumber.Location = new System.Drawing.Point(279, 20);
         this.labelnumber.Name = "labelnumber";
         this.labelnumber.Size = new System.Drawing.Size(113, 18);
         this.labelnumber.TabIndex = 6;
         this.labelnumber.Text = "Number:";
         // 
         // labelr_number
         // 
         this.labelr_number.Location = new System.Drawing.Point(279, 23);
         this.labelr_number.Name = "labelr_number";
         this.labelr_number.Size = new System.Drawing.Size(113, 18);
         this.labelr_number.TabIndex = 6;
         this.labelr_number.Text = "Number:";
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
         // labely
         // 
         this.labely.Location = new System.Drawing.Point(279, 25);
         this.labely.Name = "labely";
         this.labely.Size = new System.Drawing.Size(113, 18);
         this.labely.TabIndex = 0;
         this.labely.Text = "Y coordinate:";
         // 
         // labelx
         // 
         this.labelx.Location = new System.Drawing.Point(13, 25);
         this.labelx.Name = "labelx";
         this.labelx.Size = new System.Drawing.Size(112, 18);
         this.labelx.TabIndex = 0;
         this.labelx.Text = "X coordinate:";
         // 
         // groupBox2
         // 
         this.groupBox2.Controls.Add(this.labelUnitY);
         this.groupBox2.Controls.Add(this.textBoxLineFirstDistance);
         this.groupBox2.Controls.Add(this.comboBoxLineBubbleLocation);
         this.groupBox2.Controls.Add(this.label1r_distance);
         this.groupBox2.Controls.Add(this.textBoxLineNumber);
         this.groupBox2.Controls.Add(this.textBoxLineFirstLabel);
         this.groupBox2.Controls.Add(this.label1r_firstgrid);
         this.groupBox2.Controls.Add(this.labelr_number);
         this.groupBox2.Controls.Add(this.labelr_bubble);
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
         // label1r_distance
         // 
         this.label1r_distance.Location = new System.Drawing.Point(6, 88);
         this.label1r_distance.Name = "label1r_distance";
         this.label1r_distance.Size = new System.Drawing.Size(224, 18);
         this.label1r_distance.TabIndex = 7;
         this.label1r_distance.Text = "Distance from origin to start point:";
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
         // label1r_firstgrid
         // 
         this.label1r_firstgrid.Location = new System.Drawing.Point(6, 23);
         this.label1r_firstgrid.Name = "label1r_firstgrid";
         this.label1r_firstgrid.Size = new System.Drawing.Size(119, 18);
         this.label1r_firstgrid.TabIndex = 30;
         this.label1r_firstgrid.Text = "Label of first grid:";
         // 
         // labelr_bubble
         // 
         this.labelr_bubble.Location = new System.Drawing.Point(6, 57);
         this.labelr_bubble.Name = "labelr_bubble";
         this.labelr_bubble.Size = new System.Drawing.Size(119, 18);
         this.labelr_bubble.TabIndex = 29;
         this.labelr_bubble.Text = "Bubble location:";
         // 
         // groupBox1
         // 
         this.groupBox1.Controls.Add(this.textBoxYCoord);
         this.groupBox1.Controls.Add(this.labelYCoordUnit);
         this.groupBox1.Controls.Add(this.labelXCoordUnit);
         this.groupBox1.Controls.Add(this.textBoxXCoord);
         this.groupBox1.Controls.Add(this.labely);
         this.groupBox1.Controls.Add(this.labelx);
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
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
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
      private System.Windows.Forms.Label labelspace;
      private System.Windows.Forms.GroupBox groupBox3;
      private System.Windows.Forms.Label labelr_number;
      private System.Windows.Forms.TextBox textBoxYCoord;
      private System.Windows.Forms.TextBox textBoxXCoord;
      private System.Windows.Forms.Label labely;
      private System.Windows.Forms.Label labelx;
      private System.Windows.Forms.GroupBox groupBox2;
      private System.Windows.Forms.TextBox textBoxLineNumber;
      private System.Windows.Forms.GroupBox groupBox1;
      private System.Windows.Forms.TextBox textBoxArcNumber;
      private System.Windows.Forms.Button buttonCancel;
      private System.Windows.Forms.Button buttonCreate;
      private System.Windows.Forms.Label labelnumber;
      private System.Windows.Forms.GroupBox groupBox4;
      private System.Windows.Forms.TextBox textBoxEndDegree;
      private System.Windows.Forms.TextBox textBoxStartDegree;
      private System.Windows.Forms.Label labelEndDegree;
      private System.Windows.Forms.Label labelStartDegree;
      private System.Windows.Forms.RadioButton radioButtonCustomize;
      private System.Windows.Forms.RadioButton radioButton360;
      private System.Windows.Forms.TextBox textBoxArcFirstRadius;
      private System.Windows.Forms.Label labelradius;
      private System.Windows.Forms.TextBox textBoxLineFirstDistance;
      private System.Windows.Forms.Label label1r_distance;
      private System.Windows.Forms.ComboBox comboBoxArcBubbleLocation;
      private System.Windows.Forms.TextBox textBoxArcFirstLabel;
      private System.Windows.Forms.Label labelfirstgrid;
      private System.Windows.Forms.Label labelbubble;
      private System.Windows.Forms.ComboBox comboBoxLineBubbleLocation;
      private System.Windows.Forms.TextBox textBoxLineFirstLabel;
      private System.Windows.Forms.Label label1r_firstgrid;
      private System.Windows.Forms.Label labelr_bubble;
      private System.Windows.Forms.Label labelUnitX;
      private System.Windows.Forms.Label labelUnitY;
      private System.Windows.Forms.Label labelUnitFirstRadius;
      private System.Windows.Forms.Label labelYCoordUnit;
      private System.Windows.Forms.Label labelXCoordUnit;


   }
}