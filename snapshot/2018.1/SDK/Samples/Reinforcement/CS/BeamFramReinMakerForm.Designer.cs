//
// (C) Copyright 2003-2017 by Autodesk, Inc.
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


namespace Revit.SDK.Samples.Reinforcement.CS
{
    partial class BeamFramReinMakerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; 
        /// otherwise, false.</param>
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
            this.barTypeGroupBox = new System.Windows.Forms.GroupBox();
            this.transverseRebarTypeComboBox = new System.Windows.Forms.ComboBox();
            this.transverseBarLabel = new System.Windows.Forms.Label();
            this.bottomRebarTypeComboBox = new System.Windows.Forms.ComboBox();
            this.bottomBarLabel3 = new System.Windows.Forms.Label();
            this.topCenterRebarTypeComboBox = new System.Windows.Forms.ComboBox();
            this.topEndRebarTypeComboBox = new System.Windows.Forms.ComboBox();
            this.topCenterBarLabel = new System.Windows.Forms.Label();
            this.topEndBarLabel = new System.Windows.Forms.Label();
            this.barSpacingGroupBox = new System.Windows.Forms.GroupBox();
            this.centerUnitLabel = new System.Windows.Forms.Label();
            this.endUnitLabel = new System.Windows.Forms.Label();
            this.transverseCenterSpacingTextBox = new System.Windows.Forms.TextBox();
            this.transverseEndSpacingTextBox = new System.Windows.Forms.TextBox();
            this.transverseCenterLabel = new System.Windows.Forms.Label();
            this.transverseEndSpacingLabel = new System.Windows.Forms.Label();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.hookTypeGroupBox = new System.Windows.Forms.GroupBox();
            this.transverseBarHookComboBox = new System.Windows.Forms.ComboBox();
            this.topBarHookComboBox = new System.Windows.Forms.ComboBox();
            this.transverseBraHookLabel = new System.Windows.Forms.Label();
            this.topHookLabel = new System.Windows.Forms.Label();
            this.barTypeGroupBox.SuspendLayout();
            this.barSpacingGroupBox.SuspendLayout();
            this.hookTypeGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // barTypeGroupBox
            // 
            this.barTypeGroupBox.Controls.Add(this.transverseRebarTypeComboBox);
            this.barTypeGroupBox.Controls.Add(this.transverseBarLabel);
            this.barTypeGroupBox.Controls.Add(this.bottomRebarTypeComboBox);
            this.barTypeGroupBox.Controls.Add(this.bottomBarLabel3);
            this.barTypeGroupBox.Controls.Add(this.topCenterRebarTypeComboBox);
            this.barTypeGroupBox.Controls.Add(this.topEndRebarTypeComboBox);
            this.barTypeGroupBox.Controls.Add(this.topCenterBarLabel);
            this.barTypeGroupBox.Controls.Add(this.topEndBarLabel);
            this.barTypeGroupBox.Location = new System.Drawing.Point(12, 12);
            this.barTypeGroupBox.Name = "barTypeGroupBox";
            this.barTypeGroupBox.Size = new System.Drawing.Size(305, 160);
            this.barTypeGroupBox.TabIndex = 0;
            this.barTypeGroupBox.TabStop = false;
            this.barTypeGroupBox.Text = " Bar Type";
            // 
            // transverseRebarTypeComboBox
            // 
            this.transverseRebarTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.transverseRebarTypeComboBox.FormattingEnabled = true;
            this.transverseRebarTypeComboBox.Location = new System.Drawing.Point(110, 126);
            this.transverseRebarTypeComboBox.Name = "transverseRebarTypeComboBox";
            this.transverseRebarTypeComboBox.Size = new System.Drawing.Size(189, 21);
            this.transverseRebarTypeComboBox.TabIndex = 8;
            // 
            // transverseBarLabel
            // 
            this.transverseBarLabel.AutoSize = true;
            this.transverseBarLabel.Location = new System.Drawing.Point(7, 129);
            this.transverseBarLabel.Name = "transverseBarLabel";
            this.transverseBarLabel.Size = new System.Drawing.Size(82, 13);
            this.transverseBarLabel.TabIndex = 4;
            this.transverseBarLabel.Text = "Transverse Bar:";
            // 
            // bottomRebarTypeComboBox
            // 
            this.bottomRebarTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.bottomRebarTypeComboBox.FormattingEnabled = true;
            this.bottomRebarTypeComboBox.Location = new System.Drawing.Point(110, 91);
            this.bottomRebarTypeComboBox.Name = "bottomRebarTypeComboBox";
            this.bottomRebarTypeComboBox.Size = new System.Drawing.Size(189, 21);
            this.bottomRebarTypeComboBox.TabIndex = 7;
            // 
            // bottomBarLabel3
            // 
            this.bottomBarLabel3.AutoSize = true;
            this.bottomBarLabel3.Location = new System.Drawing.Point(7, 94);
            this.bottomBarLabel3.Name = "bottomBarLabel3";
            this.bottomBarLabel3.Size = new System.Drawing.Size(62, 13);
            this.bottomBarLabel3.TabIndex = 3;
            this.bottomBarLabel3.Text = "Bottom Bar:";
            // 
            // topCenterRebarTypeComboBox
            // 
            this.topCenterRebarTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.topCenterRebarTypeComboBox.FormattingEnabled = true;
            this.topCenterRebarTypeComboBox.Location = new System.Drawing.Point(110, 56);
            this.topCenterRebarTypeComboBox.Name = "topCenterRebarTypeComboBox";
            this.topCenterRebarTypeComboBox.Size = new System.Drawing.Size(189, 21);
            this.topCenterRebarTypeComboBox.TabIndex = 6;
            // 
            // topEndRebarTypeComboBox
            // 
            this.topEndRebarTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.topEndRebarTypeComboBox.FormattingEnabled = true;
            this.topEndRebarTypeComboBox.Location = new System.Drawing.Point(110, 21);
            this.topEndRebarTypeComboBox.Name = "topEndRebarTypeComboBox";
            this.topEndRebarTypeComboBox.Size = new System.Drawing.Size(189, 21);
            this.topEndRebarTypeComboBox.TabIndex = 5;
            // 
            // topCenterBarLabel
            // 
            this.topCenterBarLabel.AutoSize = true;
            this.topCenterBarLabel.Location = new System.Drawing.Point(7, 59);
            this.topCenterBarLabel.Name = "topCenterBarLabel";
            this.topCenterBarLabel.Size = new System.Drawing.Size(82, 13);
            this.topCenterBarLabel.TabIndex = 2;
            this.topCenterBarLabel.Text = "Top Center Bar:";
            // 
            // topEndBarLabel
            // 
            this.topEndBarLabel.AutoSize = true;
            this.topEndBarLabel.Location = new System.Drawing.Point(7, 24);
            this.topEndBarLabel.Name = "topEndBarLabel";
            this.topEndBarLabel.Size = new System.Drawing.Size(70, 13);
            this.topEndBarLabel.TabIndex = 1;
            this.topEndBarLabel.Text = "Top End Bar:";
            // 
            // barSpacingGroupBox
            // 
            this.barSpacingGroupBox.Controls.Add(this.centerUnitLabel);
            this.barSpacingGroupBox.Controls.Add(this.endUnitLabel);
            this.barSpacingGroupBox.Controls.Add(this.transverseCenterSpacingTextBox);
            this.barSpacingGroupBox.Controls.Add(this.transverseEndSpacingTextBox);
            this.barSpacingGroupBox.Controls.Add(this.transverseCenterLabel);
            this.barSpacingGroupBox.Controls.Add(this.transverseEndSpacingLabel);
            this.barSpacingGroupBox.Location = new System.Drawing.Point(12, 293);
            this.barSpacingGroupBox.Name = "barSpacingGroupBox";
            this.barSpacingGroupBox.Size = new System.Drawing.Size(305, 86);
            this.barSpacingGroupBox.TabIndex = 14;
            this.barSpacingGroupBox.TabStop = false;
            this.barSpacingGroupBox.Text = "Bar Spacing";
            // 
            // centerUnitLabel
            // 
            this.centerUnitLabel.AutoSize = true;
            this.centerUnitLabel.Location = new System.Drawing.Point(271, 56);
            this.centerUnitLabel.Name = "centerUnitLabel";
            this.centerUnitLabel.Size = new System.Drawing.Size(28, 13);
            this.centerUnitLabel.TabIndex = 22;
            this.centerUnitLabel.Text = "Feet";
            // 
            // endUnitLabel
            // 
            this.endUnitLabel.AutoSize = true;
            this.endUnitLabel.Location = new System.Drawing.Point(271, 24);
            this.endUnitLabel.Name = "endUnitLabel";
            this.endUnitLabel.Size = new System.Drawing.Size(28, 13);
            this.endUnitLabel.TabIndex = 21;
            this.endUnitLabel.Text = "Feet";
            // 
            // transverseCenterSpacingTextBox
            // 
            this.transverseCenterSpacingTextBox.Location = new System.Drawing.Point(110, 53);
            this.transverseCenterSpacingTextBox.Name = "transverseCenterSpacingTextBox";
            this.transverseCenterSpacingTextBox.Size = new System.Drawing.Size(155, 20);
            this.transverseCenterSpacingTextBox.TabIndex = 18;
            // 
            // transverseEndSpacingTextBox
            // 
            this.transverseEndSpacingTextBox.Location = new System.Drawing.Point(110, 21);
            this.transverseEndSpacingTextBox.Name = "transverseEndSpacingTextBox";
            this.transverseEndSpacingTextBox.Size = new System.Drawing.Size(155, 20);
            this.transverseEndSpacingTextBox.TabIndex = 17;
            // 
            // transverseCenterLabel
            // 
            this.transverseCenterLabel.AutoSize = true;
            this.transverseCenterLabel.Location = new System.Drawing.Point(7, 56);
            this.transverseCenterLabel.Name = "transverseCenterLabel";
            this.transverseCenterLabel.Size = new System.Drawing.Size(97, 13);
            this.transverseCenterLabel.TabIndex = 16;
            this.transverseCenterLabel.Text = "Transverse Center:";
            // 
            // transverseEndSpacingLabel
            // 
            this.transverseEndSpacingLabel.AutoSize = true;
            this.transverseEndSpacingLabel.Location = new System.Drawing.Point(7, 24);
            this.transverseEndSpacingLabel.Name = "transverseEndSpacingLabel";
            this.transverseEndSpacingLabel.Size = new System.Drawing.Size(85, 13);
            this.transverseEndSpacingLabel.TabIndex = 15;
            this.transverseEndSpacingLabel.Text = "Transverse End:";
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(131, 398);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(90, 25);
            this.okButton.TabIndex = 19;
            this.okButton.Text = "&OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(227, 398);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(90, 25);
            this.cancelButton.TabIndex = 20;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // hookTypeGroupBox
            // 
            this.hookTypeGroupBox.Controls.Add(this.transverseBarHookComboBox);
            this.hookTypeGroupBox.Controls.Add(this.topBarHookComboBox);
            this.hookTypeGroupBox.Controls.Add(this.transverseBraHookLabel);
            this.hookTypeGroupBox.Controls.Add(this.topHookLabel);
            this.hookTypeGroupBox.Location = new System.Drawing.Point(12, 188);
            this.hookTypeGroupBox.Name = "hookTypeGroupBox";
            this.hookTypeGroupBox.Size = new System.Drawing.Size(305, 87);
            this.hookTypeGroupBox.TabIndex = 9;
            this.hookTypeGroupBox.TabStop = false;
            this.hookTypeGroupBox.Text = "Hook Type";
            // 
            // transverseBarHookComboBox
            // 
            this.transverseBarHookComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.transverseBarHookComboBox.FormattingEnabled = true;
            this.transverseBarHookComboBox.Location = new System.Drawing.Point(110, 55);
            this.transverseBarHookComboBox.Name = "transverseBarHookComboBox";
            this.transverseBarHookComboBox.Size = new System.Drawing.Size(189, 21);
            this.transverseBarHookComboBox.TabIndex = 13;
            // 
            // topBarHookComboBox
            // 
            this.topBarHookComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.topBarHookComboBox.FormattingEnabled = true;
            this.topBarHookComboBox.Location = new System.Drawing.Point(110, 19);
            this.topBarHookComboBox.Name = "topBarHookComboBox";
            this.topBarHookComboBox.Size = new System.Drawing.Size(189, 21);
            this.topBarHookComboBox.TabIndex = 12;
            // 
            // transverseBraHookLabel
            // 
            this.transverseBraHookLabel.AutoSize = true;
            this.transverseBraHookLabel.Location = new System.Drawing.Point(7, 58);
            this.transverseBraHookLabel.Name = "transverseBraHookLabel";
            this.transverseBraHookLabel.Size = new System.Drawing.Size(92, 13);
            this.transverseBraHookLabel.TabIndex = 11;
            this.transverseBraHookLabel.Text = "Transverse Hook:";
            // 
            // topHookLabel
            // 
            this.topHookLabel.AutoSize = true;
            this.topHookLabel.Location = new System.Drawing.Point(7, 22);
            this.topHookLabel.Name = "topHookLabel";
            this.topHookLabel.Size = new System.Drawing.Size(58, 13);
            this.topHookLabel.TabIndex = 10;
            this.topHookLabel.Text = "Top Hook:";
            // 
            // BeamFramReinMakerForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(329, 435);
            this.Controls.Add(this.hookTypeGroupBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.barSpacingGroupBox);
            this.Controls.Add(this.barTypeGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BeamFramReinMakerForm";
            this.ShowInTaskbar = false;
            this.Text = "Beam Reinforcment";
            this.barTypeGroupBox.ResumeLayout(false);
            this.barTypeGroupBox.PerformLayout();
            this.barSpacingGroupBox.ResumeLayout(false);
            this.barSpacingGroupBox.PerformLayout();
            this.hookTypeGroupBox.ResumeLayout(false);
            this.hookTypeGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox barTypeGroupBox;
        private System.Windows.Forms.ComboBox topEndRebarTypeComboBox;
        private System.Windows.Forms.Label topCenterBarLabel;
        private System.Windows.Forms.Label topEndBarLabel;
        private System.Windows.Forms.ComboBox topCenterRebarTypeComboBox;
        private System.Windows.Forms.Label transverseBarLabel;
        private System.Windows.Forms.ComboBox bottomRebarTypeComboBox;
        private System.Windows.Forms.Label bottomBarLabel3;
        private System.Windows.Forms.GroupBox barSpacingGroupBox;
        private System.Windows.Forms.ComboBox transverseRebarTypeComboBox;
        private System.Windows.Forms.Label transverseCenterLabel;
        private System.Windows.Forms.Label transverseEndSpacingLabel;
        private System.Windows.Forms.TextBox transverseCenterSpacingTextBox;
        private System.Windows.Forms.TextBox transverseEndSpacingTextBox;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
       private System.Windows.Forms.GroupBox hookTypeGroupBox;
       private System.Windows.Forms.Label transverseBraHookLabel;
       private System.Windows.Forms.Label topHookLabel;
       private System.Windows.Forms.ComboBox transverseBarHookComboBox;
       private System.Windows.Forms.ComboBox topBarHookComboBox;
       private System.Windows.Forms.Label centerUnitLabel;
       private System.Windows.Forms.Label endUnitLabel;
    }
}