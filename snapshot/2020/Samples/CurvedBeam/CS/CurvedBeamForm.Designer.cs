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

namespace Revit.SDK.Samples.CurvedBeam.CS
{
    /// <summary>
    /// new beam form
    /// </summary>
    partial class CurvedBeamForm
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

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.BeamTypeCB = new System.Windows.Forms.ComboBox();
            this.LevelCB = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.newArcButton = new System.Windows.Forms.Button();
            this.newEllipseButton = new System.Windows.Forms.Button();
            this.newNurbSplineButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // BeamTypeCB
            // 
            this.BeamTypeCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.BeamTypeCB.FormattingEnabled = true;
            this.BeamTypeCB.Location = new System.Drawing.Point(95, 6);
            this.BeamTypeCB.Name = "BeamTypeCB";
            this.BeamTypeCB.Size = new System.Drawing.Size(274, 21);
            this.BeamTypeCB.TabIndex = 0;
            // 
            // LevelCB
            // 
            this.LevelCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.LevelCB.FormattingEnabled = true;
            this.LevelCB.Location = new System.Drawing.Point(95, 43);
            this.LevelCB.Name = "LevelCB";
            this.LevelCB.Size = new System.Drawing.Size(274, 21);
            this.LevelCB.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Type of Beam";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(52, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Level";
            // 
            // newArcButton
            // 
            this.newArcButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.newArcButton.Image = global::Revit.SDK.Samples.CurvedBeam.CS.Properties.Resources.arc3;
            this.newArcButton.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.newArcButton.Location = new System.Drawing.Point(33, 83);
            this.newArcButton.Name = "newArcButton";
            this.newArcButton.Size = new System.Drawing.Size(54, 41);
            this.newArcButton.TabIndex = 4;
            this.newArcButton.Text = "&Arc";
            this.newArcButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.newArcButton.UseVisualStyleBackColor = true;
            this.newArcButton.Click += new System.EventHandler(this.newArcButton_Click);
            // 
            // newEllipseButton
            // 
            this.newEllipseButton.Image = global::Revit.SDK.Samples.CurvedBeam.CS.Properties.Resources.partialellipse;
            this.newEllipseButton.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.newEllipseButton.Location = new System.Drawing.Point(162, 83);
            this.newEllipseButton.Name = "newEllipseButton";
            this.newEllipseButton.Size = new System.Drawing.Size(54, 41);
            this.newEllipseButton.TabIndex = 5;
            this.newEllipseButton.Text = "Partial &Ellipse";
            this.newEllipseButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.newEllipseButton.UseVisualStyleBackColor = true;
            this.newEllipseButton.Click += new System.EventHandler(this.newEllipseButton_Click);
            // 
            // newNurbSplineButton
            // 
            this.newNurbSplineButton.Image = global::Revit.SDK.Samples.CurvedBeam.CS.Properties.Resources.spline;
            this.newNurbSplineButton.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.newNurbSplineButton.Location = new System.Drawing.Point(291, 83);
            this.newNurbSplineButton.Name = "newNurbSplineButton";
            this.newNurbSplineButton.Size = new System.Drawing.Size(54, 41);
            this.newNurbSplineButton.TabIndex = 6;
            this.newNurbSplineButton.Text = "&Spline";
            this.newNurbSplineButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.newNurbSplineButton.UseVisualStyleBackColor = true;
            this.newNurbSplineButton.Click += new System.EventHandler(this.newNurbSplineButton_Click);
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label4.Location = new System.Drawing.Point(14, 73);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(358, 2);
            this.label4.TabIndex = 12;
            // 
            // CurvedBeamForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(380, 132);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.newEllipseButton);
            this.Controls.Add(this.newNurbSplineButton);
            this.Controls.Add(this.newArcButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LevelCB);
            this.Controls.Add(this.BeamTypeCB);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CurvedBeamForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Curved Beam";
            this.Load += new System.EventHandler(this.CreateCurvedBeamForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        /// <summary>
        /// method called when form is loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateCurvedBeamForm_Load(object sender, System.EventArgs e)
        {
            //this.TextBoxRefresh();

            this.BeamTypeCB.DataSource = m_dataBuffer.BeamMaps;
            this.BeamTypeCB.DisplayMember = "SymbolName";
            this.BeamTypeCB.ValueMember = "ElementType";

            this.LevelCB.DataSource = m_dataBuffer.LevelMaps;
            this.LevelCB.DisplayMember = "LevelName";
            this.LevelCB.ValueMember = "Level";
        }

        private Command m_dataBuffer;
        private System.Windows.Forms.ComboBox BeamTypeCB;
        private System.Windows.Forms.ComboBox LevelCB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button newArcButton;
        private System.Windows.Forms.Button newEllipseButton;
        private System.Windows.Forms.Button newNurbSplineButton;
        private System.Windows.Forms.Label label4;
    }
}