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

namespace Revit.SDK.Samples.VisibilityControl.CS
{
    partial class VisibilityCtrlForm
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
            this.visibilityCtrlGroupBox = new System.Windows.Forms.GroupBox();
            this.allCategoriesListView = new System.Windows.Forms.ListView();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okBotton = new System.Windows.Forms.Button();
            this.pickOneRadioButton = new System.Windows.Forms.RadioButton();
            this.isolategroupBox = new System.Windows.Forms.GroupBox();
            this.windowSelectRadioButton = new System.Windows.Forms.RadioButton();
            this.isolateButton = new System.Windows.Forms.Button();
            this.checkAllButton = new System.Windows.Forms.Button();
            this.checkNoneButton = new System.Windows.Forms.Button();
            this.visibilityCtrlGroupBox.SuspendLayout();
            this.isolategroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // visibilityCtrlGroupBox
            // 
            this.visibilityCtrlGroupBox.Controls.Add(this.checkNoneButton);
            this.visibilityCtrlGroupBox.Controls.Add(this.checkAllButton);
            this.visibilityCtrlGroupBox.Controls.Add(this.allCategoriesListView);
            this.visibilityCtrlGroupBox.Controls.Add(this.cancelButton);
            this.visibilityCtrlGroupBox.Controls.Add(this.okBotton);
            this.visibilityCtrlGroupBox.Location = new System.Drawing.Point(12, 12);
            this.visibilityCtrlGroupBox.Name = "visibilityCtrlGroupBox";
            this.visibilityCtrlGroupBox.Size = new System.Drawing.Size(620, 339);
            this.visibilityCtrlGroupBox.TabIndex = 3;
            this.visibilityCtrlGroupBox.TabStop = false;
            this.visibilityCtrlGroupBox.Text = "Category visibility";
            // 
            // allCategoriesListView
            // 
            this.allCategoriesListView.CheckBoxes = true;
            this.allCategoriesListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.allCategoriesListView.Location = new System.Drawing.Point(6, 19);
            this.allCategoriesListView.Name = "allCategoriesListView";
            this.allCategoriesListView.Size = new System.Drawing.Size(527, 314);
            this.allCategoriesListView.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.allCategoriesListView.TabIndex = 5;
            this.allCategoriesListView.UseCompatibleStateImageBehavior = false;
            this.allCategoriesListView.View = System.Windows.Forms.View.Details;
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(539, 310);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // okBotton
            // 
            this.okBotton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okBotton.Location = new System.Drawing.Point(539, 281);
            this.okBotton.Name = "okBotton";
            this.okBotton.Size = new System.Drawing.Size(75, 23);
            this.okBotton.TabIndex = 1;
            this.okBotton.Text = "&OK";
            this.okBotton.UseVisualStyleBackColor = true;
            // 
            // pickOneRadioButton
            // 
            this.pickOneRadioButton.AutoSize = true;
            this.pickOneRadioButton.Checked = true;
            this.pickOneRadioButton.Location = new System.Drawing.Point(6, 19);
            this.pickOneRadioButton.Name = "pickOneRadioButton";
            this.pickOneRadioButton.Size = new System.Drawing.Size(66, 17);
            this.pickOneRadioButton.TabIndex = 2;
            this.pickOneRadioButton.TabStop = true;
            this.pickOneRadioButton.Text = "PickOne";
            this.pickOneRadioButton.UseVisualStyleBackColor = true;
            // 
            // isolategroupBox
            // 
            this.isolategroupBox.Controls.Add(this.windowSelectRadioButton);
            this.isolategroupBox.Controls.Add(this.pickOneRadioButton);
            this.isolategroupBox.Controls.Add(this.isolateButton);
            this.isolategroupBox.Location = new System.Drawing.Point(364, 357);
            this.isolategroupBox.Name = "isolategroupBox";
            this.isolategroupBox.Size = new System.Drawing.Size(268, 49);
            this.isolategroupBox.TabIndex = 2;
            this.isolategroupBox.TabStop = false;
            this.isolategroupBox.Text = "Isolate element(s)";
            // 
            // windowSelectRadioButton
            // 
            this.windowSelectRadioButton.AutoSize = true;
            this.windowSelectRadioButton.Location = new System.Drawing.Point(78, 19);
            this.windowSelectRadioButton.Name = "windowSelectRadioButton";
            this.windowSelectRadioButton.Size = new System.Drawing.Size(94, 17);
            this.windowSelectRadioButton.TabIndex = 2;
            this.windowSelectRadioButton.TabStop = true;
            this.windowSelectRadioButton.Text = "WindowSelect";
            this.windowSelectRadioButton.UseVisualStyleBackColor = true;
            // 
            // isolateButton
            // 
            this.isolateButton.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.isolateButton.Location = new System.Drawing.Point(187, 16);
            this.isolateButton.Name = "isolateButton";
            this.isolateButton.Size = new System.Drawing.Size(75, 23);
            this.isolateButton.TabIndex = 3;
            this.isolateButton.Text = "&Isolate";
            this.isolateButton.UseVisualStyleBackColor = true;
            this.isolateButton.Click += new System.EventHandler(this.isolateButton_Click);
            // 
            // checkAllButton
            // 
            this.checkAllButton.Location = new System.Drawing.Point(539, 223);
            this.checkAllButton.Name = "checkAllButton";
            this.checkAllButton.Size = new System.Drawing.Size(75, 23);
            this.checkAllButton.TabIndex = 6;
            this.checkAllButton.Text = "Check All";
            this.checkAllButton.UseVisualStyleBackColor = true;
            this.checkAllButton.Click += new System.EventHandler(this.checkAllButton_Click);
            // 
            // checkNoneButton
            // 
            this.checkNoneButton.Location = new System.Drawing.Point(539, 252);
            this.checkNoneButton.Name = "checkNoneButton";
            this.checkNoneButton.Size = new System.Drawing.Size(75, 23);
            this.checkNoneButton.TabIndex = 6;
            this.checkNoneButton.Text = "Check None";
            this.checkNoneButton.UseVisualStyleBackColor = true;
            this.checkNoneButton.Click += new System.EventHandler(this.checkNoneButton_Click);
            // 
            // VisibilityCtrlForm
            // 
            this.AcceptButton = this.okBotton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(644, 418);
            this.Controls.Add(this.visibilityCtrlGroupBox);
            this.Controls.Add(this.isolategroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VisibilityCtrlForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Visibility Controller";
            this.Load += new System.EventHandler(this.VisibilityCtrlForm_Load);
            this.visibilityCtrlGroupBox.ResumeLayout(false);
            this.isolategroupBox.ResumeLayout(false);
            this.isolategroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox visibilityCtrlGroupBox;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okBotton;
        private System.Windows.Forms.RadioButton pickOneRadioButton;
        private System.Windows.Forms.GroupBox isolategroupBox;
        private System.Windows.Forms.RadioButton windowSelectRadioButton;
        private System.Windows.Forms.Button isolateButton;
        private System.Windows.Forms.ListView allCategoriesListView;
        private System.Windows.Forms.Button checkNoneButton;
        private System.Windows.Forms.Button checkAllButton;
    }
}