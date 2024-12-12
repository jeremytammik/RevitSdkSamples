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

namespace Revit.SDK.Samples.GridCreation.CS
{
    partial class GridCreationOptionForm
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
            this.radioButtonSelect = new System.Windows.Forms.RadioButton();
            this.radioButtonRadialAndCircularGrids = new System.Windows.Forms.RadioButton();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.groupBoxCreateOptions = new System.Windows.Forms.GroupBox();
            this.radioButtonOrthogonalGrids = new System.Windows.Forms.RadioButton();
            this.groupBoxCreateOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // radioButtonSelect
            // 
            this.radioButtonSelect.AutoSize = true;
            this.radioButtonSelect.Checked = true;
            this.radioButtonSelect.Location = new System.Drawing.Point(6, 19);
            this.radioButtonSelect.Name = "radioButtonSelect";
            this.radioButtonSelect.Size = new System.Drawing.Size(205, 17);
            this.radioButtonSelect.TabIndex = 0;
            this.radioButtonSelect.TabStop = true;
            this.radioButtonSelect.Text = "Create grids with selected lines or arcs";
            this.radioButtonSelect.UseVisualStyleBackColor = true;
            // 
            // radioButtonRadialAndCircularGrids
            // 
            this.radioButtonRadialAndCircularGrids.AutoSize = true;
            this.radioButtonRadialAndCircularGrids.Location = new System.Drawing.Point(6, 69);
            this.radioButtonRadialAndCircularGrids.Name = "radioButtonRadialAndCircularGrids";
            this.radioButtonRadialAndCircularGrids.Size = new System.Drawing.Size(199, 17);
            this.radioButtonRadialAndCircularGrids.TabIndex = 2;
            this.radioButtonRadialAndCircularGrids.Text = "Create a batch of radial and arc grids";
            this.radioButtonRadialAndCircularGrids.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(159, 129);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(90, 23);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "&Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(58, 129);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(90, 23);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "&OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // groupBoxCreateOptions
            // 
            this.groupBoxCreateOptions.Controls.Add(this.radioButtonSelect);
            this.groupBoxCreateOptions.Controls.Add(this.radioButtonOrthogonalGrids);
            this.groupBoxCreateOptions.Controls.Add(this.radioButtonRadialAndCircularGrids);
            this.groupBoxCreateOptions.Location = new System.Drawing.Point(12, 12);
            this.groupBoxCreateOptions.Name = "groupBoxCreateOptions";
            this.groupBoxCreateOptions.Size = new System.Drawing.Size(237, 96);
            this.groupBoxCreateOptions.TabIndex = 13;
            this.groupBoxCreateOptions.TabStop = false;
            this.groupBoxCreateOptions.Text = "Choose the way to create grids";
            // 
            // radioButtonOrthogonalGrids
            // 
            this.radioButtonOrthogonalGrids.AutoSize = true;
            this.radioButtonOrthogonalGrids.Location = new System.Drawing.Point(6, 44);
            this.radioButtonOrthogonalGrids.Name = "radioButtonOrthogonalGrids";
            this.radioButtonOrthogonalGrids.Size = new System.Drawing.Size(185, 17);
            this.radioButtonOrthogonalGrids.TabIndex = 1;
            this.radioButtonOrthogonalGrids.Text = "Create a batch of orthogonal grids";
            this.radioButtonOrthogonalGrids.UseVisualStyleBackColor = true;
            // 
            // GridCreationOptionForm
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(265, 164);
            this.Controls.Add(this.groupBoxCreateOptions);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GridCreationOptionForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Grid Creation";
            this.groupBoxCreateOptions.ResumeLayout(false);
            this.groupBoxCreateOptions.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton radioButtonSelect;
        private System.Windows.Forms.RadioButton radioButtonRadialAndCircularGrids;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.GroupBox groupBoxCreateOptions;
        private System.Windows.Forms.RadioButton radioButtonOrthogonalGrids;
    }
}