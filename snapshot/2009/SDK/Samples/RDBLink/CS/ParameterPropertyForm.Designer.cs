//
// (C) Copyright 2003-2008 by Autodesk, Inc.
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
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.?AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
//


namespace Revit.SDK.Samples.RDBLink.CS
{
    partial class ParameterPropertyForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ParameterPropertyForm));
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.sharedParameterRadioButton = new System.Windows.Forms.RadioButton();
            this.projectParameterRadioButton = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.typeRadioButton = new System.Windows.Forms.RadioButton();
            this.instanceRadioButton = new System.Windows.Forms.RadioButton();
            this.typeComboBox = new System.Windows.Forms.ComboBox();
            this.groupComboBox = new System.Windows.Forms.ComboBox();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.categoryCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.checkNoneButton = new System.Windows.Forms.Button();
            this.checkAllButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // okButton
            // 
            resources.ApplyResources(this.okButton, "okButton");
            this.okButton.Name = "okButton";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            resources.ApplyResources(this.cancelButton, "cancelButton");
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // sharedParameterRadioButton
            // 
            resources.ApplyResources(this.sharedParameterRadioButton, "sharedParameterRadioButton");
            this.sharedParameterRadioButton.Checked = true;
            this.sharedParameterRadioButton.Name = "sharedParameterRadioButton";
            this.sharedParameterRadioButton.TabStop = true;
            this.sharedParameterRadioButton.UseVisualStyleBackColor = true;
            // 
            // projectParameterRadioButton
            // 
            resources.ApplyResources(this.projectParameterRadioButton, "projectParameterRadioButton");
            this.projectParameterRadioButton.Name = "projectParameterRadioButton";
            this.projectParameterRadioButton.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.sharedParameterRadioButton);
            this.groupBox1.Controls.Add(this.projectParameterRadioButton);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.typeRadioButton);
            this.groupBox2.Controls.Add(this.instanceRadioButton);
            this.groupBox2.Controls.Add(this.typeComboBox);
            this.groupBox2.Controls.Add(this.groupComboBox);
            this.groupBox2.Controls.Add(this.nameTextBox);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // typeRadioButton
            // 
            resources.ApplyResources(this.typeRadioButton, "typeRadioButton");
            this.typeRadioButton.Name = "typeRadioButton";
            this.typeRadioButton.UseVisualStyleBackColor = true;
            // 
            // instanceRadioButton
            // 
            resources.ApplyResources(this.instanceRadioButton, "instanceRadioButton");
            this.instanceRadioButton.Checked = true;
            this.instanceRadioButton.Name = "instanceRadioButton";
            this.instanceRadioButton.TabStop = true;
            this.instanceRadioButton.UseVisualStyleBackColor = true;
            // 
            // typeComboBox
            // 
            this.typeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.typeComboBox.FormattingEnabled = true;
            resources.ApplyResources(this.typeComboBox, "typeComboBox");
            this.typeComboBox.Name = "typeComboBox";
            // 
            // groupComboBox
            // 
            this.groupComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.groupComboBox.FormattingEnabled = true;
            resources.ApplyResources(this.groupComboBox, "groupComboBox");
            this.groupComboBox.Name = "groupComboBox";
            // 
            // nameTextBox
            // 
            resources.ApplyResources(this.nameTextBox, "nameTextBox");
            this.nameTextBox.Name = "nameTextBox";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.categoryCheckedListBox);
            this.groupBox3.Controls.Add(this.checkNoneButton);
            this.groupBox3.Controls.Add(this.checkAllButton);
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // categoryCheckedListBox
            // 
            this.categoryCheckedListBox.CheckOnClick = true;
            this.categoryCheckedListBox.FormattingEnabled = true;
            resources.ApplyResources(this.categoryCheckedListBox, "categoryCheckedListBox");
            this.categoryCheckedListBox.Name = "categoryCheckedListBox";
            this.categoryCheckedListBox.Sorted = true;
            // 
            // checkNoneButton
            // 
            resources.ApplyResources(this.checkNoneButton, "checkNoneButton");
            this.checkNoneButton.Name = "checkNoneButton";
            this.checkNoneButton.UseVisualStyleBackColor = true;
            this.checkNoneButton.Click += new System.EventHandler(this.checkNoneButton_Click);
            // 
            // checkAllButton
            // 
            resources.ApplyResources(this.checkAllButton, "checkAllButton");
            this.checkAllButton.Name = "checkAllButton";
            this.checkAllButton.UseVisualStyleBackColor = true;
            this.checkAllButton.Click += new System.EventHandler(this.checkAllButton_Click);
            // 
            // ParameterPropertyForm
            // 
            this.AcceptButton = this.okButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ParameterPropertyForm";
            this.ShowInTaskbar = false;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.RadioButton sharedParameterRadioButton;
        private System.Windows.Forms.RadioButton projectParameterRadioButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox typeComboBox;
        private System.Windows.Forms.ComboBox groupComboBox;
        private System.Windows.Forms.RadioButton typeRadioButton;
        private System.Windows.Forms.RadioButton instanceRadioButton;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button checkNoneButton;
        private System.Windows.Forms.Button checkAllButton;
        private System.Windows.Forms.CheckedListBox categoryCheckedListBox;
    }
}