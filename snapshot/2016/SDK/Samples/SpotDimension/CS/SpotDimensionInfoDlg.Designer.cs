//
// (C) Copyright 2003-2015 by Autodesk, Inc. 
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


namespace Revit.SDK.Samples.SpotDimension.CS
{
    /// <summary>
    /// spot dimension designer class
    /// </summary>
    partial class SpotDimensionInfoDlg
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private SpotDimensionsData m_data;
        private SpotDimensionParams m_typeParamsData;
        private Autodesk.Revit.DB.SpotDimension m_lastSelectDimention;

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
            this.spotDimensionsListView = new System.Windows.Forms.ListView();
            this.typeParamsDataGridView = new System.Windows.Forms.DataGridView();
            this.viewsComboBox = new System.Windows.Forms.ComboBox();
            this.viewLabel = new System.Windows.Forms.Label();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.dimensionsGroupBox = new System.Windows.Forms.GroupBox();
            this.infoGroupBox = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.typeParamsDataGridView)).BeginInit();
            this.dimensionsGroupBox.SuspendLayout();
            this.infoGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // spotDimensionsListView
            // 
            this.spotDimensionsListView.Location = new System.Drawing.Point(6, 19);
            this.spotDimensionsListView.MultiSelect = false;
            this.spotDimensionsListView.Name = "spotDimensionsListView";
            this.spotDimensionsListView.Size = new System.Drawing.Size(356, 165);
            this.spotDimensionsListView.TabIndex = 0;
            this.spotDimensionsListView.UseCompatibleStateImageBehavior = false;
            this.spotDimensionsListView.View = System.Windows.Forms.View.List;
            this.spotDimensionsListView.SelectedIndexChanged += new System.EventHandler(this.spotDimensionsListView_SelectedIndexChanged);
            // 
            // typeParamsDataGridView
            // 
            this.typeParamsDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.typeParamsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.typeParamsDataGridView.Location = new System.Drawing.Point(6, 19);
            this.typeParamsDataGridView.Name = "typeParamsDataGridView";
            this.typeParamsDataGridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.typeParamsDataGridView.Size = new System.Drawing.Size(356, 183);
            this.typeParamsDataGridView.TabIndex = 2;
            // 
            // viewsComboBox
            // 
            this.viewsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.viewsComboBox.FormattingEnabled = true;
            this.viewsComboBox.Location = new System.Drawing.Point(59, 16);
            this.viewsComboBox.Name = "viewsComboBox";
            this.viewsComboBox.Size = new System.Drawing.Size(324, 21);
            this.viewsComboBox.TabIndex = 3;
            this.viewsComboBox.SelectedIndexChanged += new System.EventHandler(this.viewsComboBox_SelectedIndexChanged);
            // 
            // viewLabel
            // 
            this.viewLabel.AutoSize = true;
            this.viewLabel.Location = new System.Drawing.Point(12, 19);
            this.viewLabel.Name = "viewLabel";
            this.viewLabel.Size = new System.Drawing.Size(30, 13);
            this.viewLabel.TabIndex = 4;
            this.viewLabel.Text = "View";
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(227, 457);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 5;
            this.okButton.Text = "&Ok";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(308, 457);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 6;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // dimensionsGroupBox
            // 
            this.dimensionsGroupBox.Controls.Add(this.spotDimensionsListView);
            this.dimensionsGroupBox.Location = new System.Drawing.Point(15, 47);
            this.dimensionsGroupBox.Name = "dimensionsGroupBox";
            this.dimensionsGroupBox.Size = new System.Drawing.Size(368, 190);
            this.dimensionsGroupBox.TabIndex = 7;
            this.dimensionsGroupBox.TabStop = false;
            this.dimensionsGroupBox.Text = "SpotDimensions";
            // 
            // infoGroupBox
            // 
            this.infoGroupBox.Controls.Add(this.typeParamsDataGridView);
            this.infoGroupBox.Location = new System.Drawing.Point(15, 243);
            this.infoGroupBox.Name = "infoGroupBox";
            this.infoGroupBox.Size = new System.Drawing.Size(368, 208);
            this.infoGroupBox.TabIndex = 8;
            this.infoGroupBox.TabStop = false;
            this.infoGroupBox.Text = "Parameters Information";
            // 
            // SpotDimensionInfoDlg
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(395, 492);
            this.Controls.Add(this.infoGroupBox);
            this.Controls.Add(this.dimensionsGroupBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.viewLabel);
            this.Controls.Add(this.viewsComboBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SpotDimensionInfoDlg";
            this.ShowInTaskbar = false;
            this.Text = "Informations of SpotDimension";
            ((System.ComponentModel.ISupportInitialize)(this.typeParamsDataGridView)).EndInit();
            this.dimensionsGroupBox.ResumeLayout(false);
            this.infoGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView spotDimensionsListView;
        private System.Windows.Forms.DataGridView typeParamsDataGridView;
        private System.Windows.Forms.ComboBox viewsComboBox;
        private System.Windows.Forms.Label viewLabel;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.GroupBox dimensionsGroupBox;
        private System.Windows.Forms.GroupBox infoGroupBox;
    }
}