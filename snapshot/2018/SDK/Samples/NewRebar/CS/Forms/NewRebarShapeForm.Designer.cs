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

namespace Revit.SDK.Samples.NewRebar.CS
{
    partial class NewRebarShapeForm
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
            this.addParameterButton = new System.Windows.Forms.Button();
            this.addConstraintButton = new System.Windows.Forms.Button();
            this.parameterListBox = new System.Windows.Forms.ListBox();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.constraintListBox = new System.Windows.Forms.ListBox();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.parameterGroupBox = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.hookGroupBox = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.endHookOrientationcomboBox = new System.Windows.Forms.ComboBox();
            this.startHookOrientationComboBox = new System.Windows.Forms.ComboBox();
            this.endHookAngleComboBox = new System.Windows.Forms.ComboBox();
            this.startHookAngleComboBox = new System.Windows.Forms.ComboBox();
            this.useHooksCheckBox = new System.Windows.Forms.CheckBox();
            this.parameterGroupBox.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.hookGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // addParameterButton
            // 
            this.addParameterButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.addParameterButton.Location = new System.Drawing.Point(3, 195);
            this.addParameterButton.Name = "addParameterButton";
            this.addParameterButton.Size = new System.Drawing.Size(137, 31);
            this.addParameterButton.TabIndex = 2;
            this.addParameterButton.Text = "Add &Parameter";
            this.addParameterButton.UseVisualStyleBackColor = true;
            this.addParameterButton.Click += new System.EventHandler(this.addParameterButton_Click);
            // 
            // addConstraintButton
            // 
            this.addConstraintButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.addConstraintButton.Location = new System.Drawing.Point(3, 195);
            this.addConstraintButton.Name = "addConstraintButton";
            this.addConstraintButton.Size = new System.Drawing.Size(200, 31);
            this.addConstraintButton.TabIndex = 3;
            this.addConstraintButton.Text = "Add Con&straint";
            this.addConstraintButton.UseVisualStyleBackColor = true;
            this.addConstraintButton.Click += new System.EventHandler(this.addConstraintButton_Click);
            // 
            // parameterListBox
            // 
            this.parameterListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.parameterListBox.FormattingEnabled = true;
            this.parameterListBox.HorizontalScrollbar = true;
            this.parameterListBox.Location = new System.Drawing.Point(3, 16);
            this.parameterListBox.Name = "parameterListBox";
            this.parameterListBox.Size = new System.Drawing.Size(137, 173);
            this.parameterListBox.TabIndex = 4;
            this.parameterListBox.SelectedIndexChanged += new System.EventHandler(this.parameterListBox_SelectedIndexChanged);
            // 
            // propertyGrid
            // 
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid.Location = new System.Drawing.Point(3, 16);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(222, 365);
            this.propertyGrid.TabIndex = 5;
            // 
            // constraintListBox
            // 
            this.constraintListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.constraintListBox.FormattingEnabled = true;
            this.constraintListBox.Location = new System.Drawing.Point(3, 16);
            this.constraintListBox.Name = "constraintListBox";
            this.constraintListBox.Size = new System.Drawing.Size(200, 173);
            this.constraintListBox.TabIndex = 6;
            this.constraintListBox.SelectedIndexChanged += new System.EventHandler(this.constraintListBox_SelectedIndexChanged);
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(237, 425);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 29);
            this.okButton.TabIndex = 7;
            this.okButton.Text = "&OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(342, 425);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 29);
            this.cancelButton.TabIndex = 8;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // parameterGroupBox
            // 
            this.parameterGroupBox.Controls.Add(this.parameterListBox);
            this.parameterGroupBox.Controls.Add(this.addParameterButton);
            this.parameterGroupBox.Location = new System.Drawing.Point(12, 26);
            this.parameterGroupBox.Name = "parameterGroupBox";
            this.parameterGroupBox.Size = new System.Drawing.Size(143, 229);
            this.parameterGroupBox.TabIndex = 9;
            this.parameterGroupBox.TabStop = false;
            this.parameterGroupBox.Text = "Parameters";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.constraintListBox);
            this.groupBox2.Controls.Add(this.addConstraintButton);
            this.groupBox2.Location = new System.Drawing.Point(181, 26);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(206, 229);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Constraints";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.propertyGrid);
            this.groupBox3.Location = new System.Drawing.Point(413, 26);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(228, 384);
            this.groupBox3.TabIndex = 11;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Properties";
            // 
            // hookGroupBox
            // 
            this.hookGroupBox.Controls.Add(this.label4);
            this.hookGroupBox.Controls.Add(this.label3);
            this.hookGroupBox.Controls.Add(this.label2);
            this.hookGroupBox.Controls.Add(this.label1);
            this.hookGroupBox.Controls.Add(this.endHookOrientationcomboBox);
            this.hookGroupBox.Controls.Add(this.startHookOrientationComboBox);
            this.hookGroupBox.Controls.Add(this.endHookAngleComboBox);
            this.hookGroupBox.Controls.Add(this.startHookAngleComboBox);
            this.hookGroupBox.Controls.Add(this.useHooksCheckBox);
            this.hookGroupBox.Location = new System.Drawing.Point(15, 257);
            this.hookGroupBox.Name = "hookGroupBox";
            this.hookGroupBox.Size = new System.Drawing.Size(368, 153);
            this.hookGroupBox.TabIndex = 12;
            this.hookGroupBox.TabStop = false;
            this.hookGroupBox.Text = "Rebar Hook";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(222, 100);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(109, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "End Hook Orientation";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "End Hook Angle";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(222, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Start Hook Orientation";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Start Hook Angle";
            // 
            // endHookOrientationcomboBox
            // 
            this.endHookOrientationcomboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.endHookOrientationcomboBox.Enabled = false;
            this.endHookOrientationcomboBox.FormattingEnabled = true;
            this.endHookOrientationcomboBox.Location = new System.Drawing.Point(222, 120);
            this.endHookOrientationcomboBox.Name = "endHookOrientationcomboBox";
            this.endHookOrientationcomboBox.Size = new System.Drawing.Size(121, 21);
            this.endHookOrientationcomboBox.TabIndex = 4;
            // 
            // startHookOrientationComboBox
            // 
            this.startHookOrientationComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.startHookOrientationComboBox.Enabled = false;
            this.startHookOrientationComboBox.FormattingEnabled = true;
            this.startHookOrientationComboBox.Location = new System.Drawing.Point(222, 62);
            this.startHookOrientationComboBox.Name = "startHookOrientationComboBox";
            this.startHookOrientationComboBox.Size = new System.Drawing.Size(121, 21);
            this.startHookOrientationComboBox.TabIndex = 3;
            // 
            // endHookAngleComboBox
            // 
            this.endHookAngleComboBox.Enabled = false;
            this.endHookAngleComboBox.FormattingEnabled = true;
            this.endHookAngleComboBox.Location = new System.Drawing.Point(23, 120);
            this.endHookAngleComboBox.Name = "endHookAngleComboBox";
            this.endHookAngleComboBox.Size = new System.Drawing.Size(121, 21);
            this.endHookAngleComboBox.TabIndex = 2;
            // 
            // startHookAngleComboBox
            // 
            this.startHookAngleComboBox.Enabled = false;
            this.startHookAngleComboBox.FormattingEnabled = true;
            this.startHookAngleComboBox.Location = new System.Drawing.Point(23, 62);
            this.startHookAngleComboBox.Name = "startHookAngleComboBox";
            this.startHookAngleComboBox.Size = new System.Drawing.Size(121, 21);
            this.startHookAngleComboBox.TabIndex = 1;
            // 
            // useHooksCheckBox
            // 
            this.useHooksCheckBox.AutoSize = true;
            this.useHooksCheckBox.Location = new System.Drawing.Point(23, 20);
            this.useHooksCheckBox.Name = "useHooksCheckBox";
            this.useHooksCheckBox.Size = new System.Drawing.Size(103, 17);
            this.useHooksCheckBox.TabIndex = 0;
            this.useHooksCheckBox.Text = "Set Rebar Hook";
            this.useHooksCheckBox.UseVisualStyleBackColor = true;
            this.useHooksCheckBox.CheckedChanged += new System.EventHandler(this.useHooksCheckBox_CheckedChanged);
            // 
            // NewRebarShapeForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(661, 465);
            this.Controls.Add(this.hookGroupBox);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.parameterGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewRebarShapeForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Create RebarShape";
            this.Load += new System.EventHandler(this.NewRebarShapeForm_Load);
            this.parameterGroupBox.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.hookGroupBox.ResumeLayout(false);
            this.hookGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button addParameterButton;
        private System.Windows.Forms.Button addConstraintButton;
        private System.Windows.Forms.ListBox parameterListBox;
        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.ListBox constraintListBox;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.GroupBox parameterGroupBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox hookGroupBox;
        private System.Windows.Forms.ComboBox endHookOrientationcomboBox;
        private System.Windows.Forms.ComboBox startHookOrientationComboBox;
        private System.Windows.Forms.ComboBox endHookAngleComboBox;
        private System.Windows.Forms.ComboBox startHookAngleComboBox;
        private System.Windows.Forms.CheckBox useHooksCheckBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}