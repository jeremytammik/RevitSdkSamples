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

namespace Revit.SDK.Samples.WindowWizard.CS
{
    partial class WizardForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WizardForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.Step3_MemberSizes = new System.Windows.Forms.GroupBox();
            this.Step3_DiagonalsLable = new System.Windows.Forms.Label();
            this.Step3_TopChordLable = new System.Windows.Forms.Label();
            this.m_WinType = new System.Windows.Forms.ComboBox();
            this.m_unitSys = new System.Windows.Forms.ComboBox();
            this.Step1_Steps = new System.Windows.Forms.GroupBox();
            this.InputPathLabel = new System.Windows.Forms.Label();
            this.WindowPropertyLabel = new System.Windows.Forms.Label();
            this.InputDimensionLabel = new System.Windows.Forms.Label();
            this.Step1_HelpLable = new System.Windows.Forms.Label();
            this.Step1_HelpButton = new System.Windows.Forms.Button();
            this.SelectTypeLabel = new System.Windows.Forms.Label();
            this.Step1_NextButton = new System.Windows.Forms.Button();
            this.Step1_BackButton = new System.Windows.Forms.Button();
            this.Step1_CancelButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.m_comboType = new System.Windows.Forms.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_duplicateType = new System.Windows.Forms.Button();
            this.button_newType = new System.Windows.Forms.Button();
            this.m_sillHeight = new System.Windows.Forms.TextBox();
            this.m_inset = new System.Windows.Forms.TextBox();
            this.m_width = new System.Windows.Forms.TextBox();
            this.m_height = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.m_sashMat = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.m_glassMat = new System.Windows.Forms.ComboBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.m_buttonBrowser = new System.Windows.Forms.Button();
            this.m_pathName = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.panel1.SuspendLayout();
            this.Step3_MemberSizes.SuspendLayout();
            this.Step1_Steps.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.Step3_MemberSizes);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // Step3_MemberSizes
            // 
            this.Step3_MemberSizes.Controls.Add(this.Step3_DiagonalsLable);
            this.Step3_MemberSizes.Controls.Add(this.Step3_TopChordLable);
            this.Step3_MemberSizes.Controls.Add(this.m_WinType);
            this.Step3_MemberSizes.Controls.Add(this.m_unitSys);
            this.Step3_MemberSizes.ForeColor = System.Drawing.Color.Blue;
            resources.ApplyResources(this.Step3_MemberSizes, "Step3_MemberSizes");
            this.Step3_MemberSizes.Name = "Step3_MemberSizes";
            this.Step3_MemberSizes.TabStop = false;
            // 
            // Step3_DiagonalsLable
            // 
            resources.ApplyResources(this.Step3_DiagonalsLable, "Step3_DiagonalsLable");
            this.Step3_DiagonalsLable.ForeColor = System.Drawing.Color.Blue;
            this.Step3_DiagonalsLable.Name = "Step3_DiagonalsLable";
            // 
            // Step3_TopChordLable
            // 
            resources.ApplyResources(this.Step3_TopChordLable, "Step3_TopChordLable");
            this.Step3_TopChordLable.ForeColor = System.Drawing.Color.Blue;
            this.Step3_TopChordLable.Name = "Step3_TopChordLable";
            // 
            // m_WinType
            // 
            this.m_WinType.FormattingEnabled = true;
            this.m_WinType.Items.AddRange(new object[] {
            resources.GetString("m_WinType.Items"),
            resources.GetString("m_WinType.Items1")});
            resources.ApplyResources(this.m_WinType, "m_WinType");
            this.m_WinType.Name = "m_WinType";
            // 
            // m_unitSys
            // 
            this.m_unitSys.FormattingEnabled = true;
            this.m_unitSys.Items.AddRange(new object[] {
            resources.GetString("m_unitSys.Items"),
            resources.GetString("m_unitSys.Items1")});
            resources.ApplyResources(this.m_unitSys, "m_unitSys");
            this.m_unitSys.Name = "m_unitSys";
            this.m_unitSys.Tag = "";
            // 
            // Step1_Steps
            // 
            this.Step1_Steps.Controls.Add(this.InputPathLabel);
            this.Step1_Steps.Controls.Add(this.WindowPropertyLabel);
            this.Step1_Steps.Controls.Add(this.InputDimensionLabel);
            this.Step1_Steps.Controls.Add(this.Step1_HelpLable);
            this.Step1_Steps.Controls.Add(this.Step1_HelpButton);
            this.Step1_Steps.Controls.Add(this.SelectTypeLabel);
            this.Step1_Steps.ForeColor = System.Drawing.Color.Blue;
            resources.ApplyResources(this.Step1_Steps, "Step1_Steps");
            this.Step1_Steps.Name = "Step1_Steps";
            this.Step1_Steps.TabStop = false;
            // 
            // InputPathLabel
            // 
            resources.ApplyResources(this.InputPathLabel, "InputPathLabel");
            this.InputPathLabel.ForeColor = System.Drawing.Color.Gray;
            this.InputPathLabel.Name = "InputPathLabel";
            // 
            // WindowPropertyLabel
            // 
            resources.ApplyResources(this.WindowPropertyLabel, "WindowPropertyLabel");
            this.WindowPropertyLabel.ForeColor = System.Drawing.Color.Gray;
            this.WindowPropertyLabel.Name = "WindowPropertyLabel";
            // 
            // InputDimensionLabel
            // 
            resources.ApplyResources(this.InputDimensionLabel, "InputDimensionLabel");
            this.InputDimensionLabel.ForeColor = System.Drawing.Color.Black;
            this.InputDimensionLabel.Name = "InputDimensionLabel";
            // 
            // Step1_HelpLable
            // 
            resources.ApplyResources(this.Step1_HelpLable, "Step1_HelpLable");
            this.Step1_HelpLable.ForeColor = System.Drawing.Color.Black;
            this.Step1_HelpLable.Name = "Step1_HelpLable";
            // 
            // Step1_HelpButton
            // 
            resources.ApplyResources(this.Step1_HelpButton, "Step1_HelpButton");
            this.Step1_HelpButton.Name = "Step1_HelpButton";
            this.Step1_HelpButton.UseVisualStyleBackColor = true;
            this.Step1_HelpButton.Click += new System.EventHandler(this.Step1_HelpButton_Click);
            // 
            // SelectTypeLabel
            // 
            resources.ApplyResources(this.SelectTypeLabel, "SelectTypeLabel");
            this.SelectTypeLabel.ForeColor = System.Drawing.Color.Black;
            this.SelectTypeLabel.Name = "SelectTypeLabel";
            // 
            // Step1_NextButton
            // 
            resources.ApplyResources(this.Step1_NextButton, "Step1_NextButton");
            this.Step1_NextButton.ForeColor = System.Drawing.Color.Black;
            this.Step1_NextButton.Name = "Step1_NextButton";
            this.Step1_NextButton.UseVisualStyleBackColor = true;
            this.Step1_NextButton.Click += new System.EventHandler(this.Step1_NextButton_Click);
            // 
            // Step1_BackButton
            // 
            resources.ApplyResources(this.Step1_BackButton, "Step1_BackButton");
            this.Step1_BackButton.ForeColor = System.Drawing.Color.Black;
            this.Step1_BackButton.Name = "Step1_BackButton";
            this.Step1_BackButton.UseVisualStyleBackColor = true;
            this.Step1_BackButton.Click += new System.EventHandler(this.Step1_BackButton_Click);
            // 
            // Step1_CancelButton
            // 
            resources.ApplyResources(this.Step1_CancelButton, "Step1_CancelButton");
            this.Step1_CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Step1_CancelButton.ForeColor = System.Drawing.Color.Black;
            this.Step1_CancelButton.Name = "Step1_CancelButton";
            this.Step1_CancelButton.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.ForeColor = System.Drawing.Color.Blue;
            this.label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.ForeColor = System.Drawing.Color.Blue;
            this.label3.Name = "label3";
            // 
            // m_comboType
            // 
            this.m_comboType.FormattingEnabled = true;
            resources.ApplyResources(this.m_comboType, "m_comboType");
            this.m_comboType.Name = "m_comboType";
            this.m_comboType.SelectedIndexChanged += new System.EventHandler(this.m_comboType_SelectedIndexChanged);
            this.m_comboType.Leave += new System.EventHandler(this.m_comboType_Leave);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.groupBox1);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button_duplicateType);
            this.groupBox1.Controls.Add(this.button_newType);
            this.groupBox1.Controls.Add(this.m_sillHeight);
            this.groupBox1.Controls.Add(this.m_inset);
            this.groupBox1.Controls.Add(this.m_width);
            this.groupBox1.Controls.Add(this.m_height);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.m_comboType);
            this.groupBox1.ForeColor = System.Drawing.Color.Blue;
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // button_duplicateType
            // 
            this.button_duplicateType.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.button_duplicateType, "button_duplicateType");
            this.button_duplicateType.ForeColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.button_duplicateType.Name = "button_duplicateType";
            this.button_duplicateType.Tag = "";
            this.button_duplicateType.UseVisualStyleBackColor = false;
            this.button_duplicateType.Click += new System.EventHandler(this.button_duplicateType_Click);
            // 
            // button_newType
            // 
            resources.ApplyResources(this.button_newType, "button_newType");
            this.button_newType.ForeColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.button_newType.Name = "button_newType";
            this.button_newType.UseVisualStyleBackColor = false;
            this.button_newType.Click += new System.EventHandler(this.button_newType_Click);
            // 
            // m_sillHeight
            // 
            resources.ApplyResources(this.m_sillHeight, "m_sillHeight");
            this.m_sillHeight.Name = "m_sillHeight";
            this.m_sillHeight.TextChanged += new System.EventHandler(this.m_sillHeight_TextChanged);
            this.m_sillHeight.Leave += new System.EventHandler(this.m_sillHeight_Leave);
            // 
            // m_inset
            // 
            resources.ApplyResources(this.m_inset, "m_inset");
            this.m_inset.Name = "m_inset";
            this.m_inset.TextChanged += new System.EventHandler(this.m_inset_TextChanged);
            this.m_inset.Leave += new System.EventHandler(this.m_inset_Leave);
            // 
            // m_width
            // 
            resources.ApplyResources(this.m_width, "m_width");
            this.m_width.Name = "m_width";
            this.m_width.TextChanged += new System.EventHandler(this.m_width_TextChanged);
            this.m_width.Leave += new System.EventHandler(this.m_width_Leave);
            // 
            // m_height
            // 
            resources.ApplyResources(this.m_height, "m_height");
            this.m_height.Name = "m_height";
            this.m_height.TextChanged += new System.EventHandler(this.m_height_TextChanged);
            this.m_height.Leave += new System.EventHandler(this.m_height_Leave);
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.ForeColor = System.Drawing.Color.Blue;
            this.label6.Name = "label6";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.ForeColor = System.Drawing.Color.Blue;
            this.label5.Name = "label5";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.ForeColor = System.Drawing.Color.Blue;
            this.label4.Name = "label4";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.groupBox2);
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.Name = "panel3";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.m_sashMat);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.m_glassMat);
            this.groupBox2.ForeColor = System.Drawing.Color.Blue;
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.ForeColor = System.Drawing.Color.Blue;
            this.label7.Name = "label7";
            // 
            // m_sashMat
            // 
            this.m_sashMat.FormattingEnabled = true;
            resources.ApplyResources(this.m_sashMat, "m_sashMat");
            this.m_sashMat.Name = "m_sashMat";
            this.m_sashMat.SelectedIndexChanged += new System.EventHandler(this.m_sashMat_SelectedIndexChanged);
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.ForeColor = System.Drawing.Color.Blue;
            this.label11.Name = "label11";
            // 
            // m_glassMat
            // 
            this.m_glassMat.FormattingEnabled = true;
            resources.ApplyResources(this.m_glassMat, "m_glassMat");
            this.m_glassMat.Name = "m_glassMat";
            this.m_glassMat.SelectedIndexChanged += new System.EventHandler(this.m_glassMat_SelectedIndexChanged);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.groupBox3);
            resources.ApplyResources(this.panel4, "panel4");
            this.panel4.Name = "panel4";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.m_buttonBrowser);
            this.groupBox3.Controls.Add(this.m_pathName);
            this.groupBox3.Controls.Add(this.dataGridView1);
            this.groupBox3.ForeColor = System.Drawing.Color.Blue;
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // m_buttonBrowser
            // 
            this.m_buttonBrowser.ForeColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.m_buttonBrowser, "m_buttonBrowser");
            this.m_buttonBrowser.Name = "m_buttonBrowser";
            this.m_buttonBrowser.UseVisualStyleBackColor = true;
            this.m_buttonBrowser.Click += new System.EventHandler(this.m_buttonBrowser_Click);
            // 
            // m_pathName
            // 
            resources.ApplyResources(this.m_pathName, "m_pathName");
            this.m_pathName.Name = "m_pathName";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            resources.ApplyResources(this.dataGridView1, "dataGridView1");
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 24;
            // 
            // WizardForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.Step1_CancelButton;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.Step1_NextButton);
            this.Controls.Add(this.Step1_BackButton);
            this.Controls.Add(this.Step1_Steps);
            this.Controls.Add(this.Step1_CancelButton);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel4);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WizardForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.panel1.ResumeLayout(false);
            this.Step3_MemberSizes.ResumeLayout(false);
            this.Step3_MemberSizes.PerformLayout();
            this.Step1_Steps.ResumeLayout(false);
            this.Step1_Steps.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox Step1_Steps;
        private System.Windows.Forms.Label Step1_HelpLable;
        private System.Windows.Forms.Button Step1_HelpButton;
        private System.Windows.Forms.Label WindowPropertyLabel;
        private System.Windows.Forms.Label InputDimensionLabel;
        private System.Windows.Forms.Label SelectTypeLabel;
        private System.Windows.Forms.Button Step1_NextButton;
        private System.Windows.Forms.Label InputPathLabel;
        private System.Windows.Forms.Button Step1_BackButton;
        private System.Windows.Forms.Button Step1_CancelButton;
        private System.Windows.Forms.GroupBox Step3_MemberSizes;
        private System.Windows.Forms.Label Step3_DiagonalsLable;
        private System.Windows.Forms.Label Step3_TopChordLable;
        private System.Windows.Forms.ComboBox m_WinType;
        private System.Windows.Forms.ComboBox m_unitSys;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox m_comboType;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox m_sillHeight;
        private System.Windows.Forms.TextBox m_inset;
        private System.Windows.Forms.TextBox m_width;
        private System.Windows.Forms.TextBox m_height;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox m_sashMat;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox m_glassMat;
        private System.Windows.Forms.Button button_newType;
        private System.Windows.Forms.Button button_duplicateType;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button m_buttonBrowser;
        private System.Windows.Forms.TextBox m_pathName;
    }
}