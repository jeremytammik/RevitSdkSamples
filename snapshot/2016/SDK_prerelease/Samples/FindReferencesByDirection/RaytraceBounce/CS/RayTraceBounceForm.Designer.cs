//
// (C) Copyright 2003-2014 by Autodesk, Inc.
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

namespace Revit.SDK.Samples.RayTraceBounce.CS
{
   partial class RayTraceBounceForm
   {
      /// <summary>
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;


      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.buttonOK = new System.Windows.Forms.Button();
         this.buttonCancel = new System.Windows.Forms.Button();
         this.label1 = new System.Windows.Forms.Label();
         this.label2 = new System.Windows.Forms.Label();
         this.label3 = new System.Windows.Forms.Label();
         this.label4 = new System.Windows.Forms.Label();
         this.label5 = new System.Windows.Forms.Label();
         this.label6 = new System.Windows.Forms.Label();
         this.textBoxLocationX = new System.Windows.Forms.TextBox();
         this.textBoxLocationY = new System.Windows.Forms.TextBox();
         this.textBoxLocationZ = new System.Windows.Forms.TextBox();
         this.textBoxDirectionI = new System.Windows.Forms.TextBox();
         this.textBoxDirectionK = new System.Windows.Forms.TextBox();
         this.textBoxDirectionJ = new System.Windows.Forms.TextBox();
         this.groupBox_StartPoint = new System.Windows.Forms.GroupBox();
         this.groupBox_Direction = new System.Windows.Forms.GroupBox();
         this.label7 = new System.Windows.Forms.Label();
         this.groupBox_StartPoint.SuspendLayout();
         this.groupBox_Direction.SuspendLayout();
         this.SuspendLayout();
         // 
         // buttonOK
         // 
         this.buttonOK.Location = new System.Drawing.Point(142, 183);
         this.buttonOK.Name = "buttonOK";
         this.buttonOK.Size = new System.Drawing.Size(75, 23);
         this.buttonOK.TabIndex = 0;
         this.buttonOK.Text = "Run";
         this.buttonOK.UseVisualStyleBackColor = true;
         this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
         // 
         // buttonCancel
         // 
         this.buttonCancel.Location = new System.Drawing.Point(232, 183);
         this.buttonCancel.Name = "buttonCancel";
         this.buttonCancel.Size = new System.Drawing.Size(75, 23);
         this.buttonCancel.TabIndex = 1;
         this.buttonCancel.Text = "Close";
         this.buttonCancel.UseVisualStyleBackColor = true;
         this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(6, 22);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(20, 13);
         this.label1.TabIndex = 2;
         this.label1.Text = "X: ";
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(6, 52);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(20, 13);
         this.label2.TabIndex = 3;
         this.label2.Text = "Y: ";
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Location = new System.Drawing.Point(6, 85);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(20, 13);
         this.label3.TabIndex = 4;
         this.label3.Text = "Z: ";
         // 
         // label4
         // 
         this.label4.AutoSize = true;
         this.label4.Location = new System.Drawing.Point(6, 22);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(12, 13);
         this.label4.TabIndex = 5;
         this.label4.Text = "i:";
         // 
         // label5
         // 
         this.label5.AutoSize = true;
         this.label5.Location = new System.Drawing.Point(6, 55);
         this.label5.Name = "label5";
         this.label5.Size = new System.Drawing.Size(12, 13);
         this.label5.TabIndex = 6;
         this.label5.Text = "j:";
         // 
         // label6
         // 
         this.label6.AutoSize = true;
         this.label6.Location = new System.Drawing.Point(6, 85);
         this.label6.Name = "label6";
         this.label6.Size = new System.Drawing.Size(16, 13);
         this.label6.TabIndex = 7;
         this.label6.Text = "k:";
         // 
         // textBoxLocationX
         // 
         this.textBoxLocationX.Location = new System.Drawing.Point(32, 22);
         this.textBoxLocationX.Name = "textBoxLocationX";
         this.textBoxLocationX.Size = new System.Drawing.Size(100, 20);
         this.textBoxLocationX.TabIndex = 8;
         // 
         // textBoxLocationY
         // 
         this.textBoxLocationY.Location = new System.Drawing.Point(32, 52);
         this.textBoxLocationY.Name = "textBoxLocationY";
         this.textBoxLocationY.Size = new System.Drawing.Size(100, 20);
         this.textBoxLocationY.TabIndex = 9;
         // 
         // textBoxLocationZ
         // 
         this.textBoxLocationZ.Location = new System.Drawing.Point(32, 85);
         this.textBoxLocationZ.Name = "textBoxLocationZ";
         this.textBoxLocationZ.Size = new System.Drawing.Size(100, 20);
         this.textBoxLocationZ.TabIndex = 10;
         // 
         // textBoxDirectionI
         // 
         this.textBoxDirectionI.Location = new System.Drawing.Point(24, 19);
         this.textBoxDirectionI.Name = "textBoxDirectionI";
         this.textBoxDirectionI.Size = new System.Drawing.Size(100, 20);
         this.textBoxDirectionI.TabIndex = 11;
         // 
         // textBoxDirectionK
         // 
         this.textBoxDirectionK.Location = new System.Drawing.Point(24, 85);
         this.textBoxDirectionK.Name = "textBoxDirectionK";
         this.textBoxDirectionK.Size = new System.Drawing.Size(100, 20);
         this.textBoxDirectionK.TabIndex = 12;
         // 
         // textBoxDirectionJ
         // 
         this.textBoxDirectionJ.Location = new System.Drawing.Point(24, 52);
         this.textBoxDirectionJ.Name = "textBoxDirectionJ";
         this.textBoxDirectionJ.Size = new System.Drawing.Size(100, 20);
         this.textBoxDirectionJ.TabIndex = 13;
         // 
         // groupBox_StartPoint
         // 
         this.groupBox_StartPoint.Controls.Add(this.textBoxLocationY);
         this.groupBox_StartPoint.Controls.Add(this.label1);
         this.groupBox_StartPoint.Controls.Add(this.label2);
         this.groupBox_StartPoint.Controls.Add(this.label3);
         this.groupBox_StartPoint.Controls.Add(this.textBoxLocationX);
         this.groupBox_StartPoint.Controls.Add(this.textBoxLocationZ);
         this.groupBox_StartPoint.Location = new System.Drawing.Point(12, 56);
         this.groupBox_StartPoint.Name = "groupBox_StartPoint";
         this.groupBox_StartPoint.Size = new System.Drawing.Size(145, 118);
         this.groupBox_StartPoint.TabIndex = 16;
         this.groupBox_StartPoint.TabStop = false;
         this.groupBox_StartPoint.Text = "Start Point";
         // 
         // groupBox_Direction
         // 
         this.groupBox_Direction.Controls.Add(this.label4);
         this.groupBox_Direction.Controls.Add(this.label5);
         this.groupBox_Direction.Controls.Add(this.textBoxDirectionJ);
         this.groupBox_Direction.Controls.Add(this.label6);
         this.groupBox_Direction.Controls.Add(this.textBoxDirectionK);
         this.groupBox_Direction.Controls.Add(this.textBoxDirectionI);
         this.groupBox_Direction.Location = new System.Drawing.Point(173, 56);
         this.groupBox_Direction.Name = "groupBox_Direction";
         this.groupBox_Direction.Size = new System.Drawing.Size(134, 118);
         this.groupBox_Direction.TabIndex = 17;
         this.groupBox_Direction.TabStop = false;
         this.groupBox_Direction.Text = "Direction";
         // 
         // label7
         // 
         this.label7.Location = new System.Drawing.Point(9, 9);
         this.label7.Name = "label7";
         this.label7.Size = new System.Drawing.Size(296, 44);
         this.label7.TabIndex = 18;
         this.label7.Text = "This sample shows how to find intersection between ray and face and create connec" +
             "ting lines by Revit API method FindReferencesByDirection.";
         // 
         // RayTraceBounceForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(315, 218);
         this.Controls.Add(this.label7);
         this.Controls.Add(this.groupBox_Direction);
         this.Controls.Add(this.groupBox_StartPoint);
         this.Controls.Add(this.buttonCancel);
         this.Controls.Add(this.buttonOK);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
         this.MaximizeBox = false;
         this.Name = "RayTraceBounceForm";
         this.Text = "RayTraceBounce";
         this.groupBox_StartPoint.ResumeLayout(false);
         this.groupBox_StartPoint.PerformLayout();
         this.groupBox_Direction.ResumeLayout(false);
         this.groupBox_Direction.PerformLayout();
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.Button buttonOK;
      private System.Windows.Forms.Button buttonCancel;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.Label label5;
      private System.Windows.Forms.Label label6;
      private System.Windows.Forms.TextBox textBoxLocationX;
      private System.Windows.Forms.TextBox textBoxLocationY;
      private System.Windows.Forms.TextBox textBoxLocationZ;
      private System.Windows.Forms.TextBox textBoxDirectionI;
      private System.Windows.Forms.TextBox textBoxDirectionK;
      private System.Windows.Forms.TextBox textBoxDirectionJ;
      private System.Windows.Forms.GroupBox groupBox_StartPoint;
      private System.Windows.Forms.GroupBox groupBox_Direction;
      private System.Windows.Forms.Label label7;
   }
}