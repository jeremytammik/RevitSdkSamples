//
// (C) Copyright 2003-2011 by Autodesk, Inc.
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

namespace Revit.SDK.Samples.ImportExport.CS
{
    partial class ExportCivil3DForm
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
            this.buttonBrowser = new System.Windows.Forms.Button();
            this.labelSaveAs = new System.Windows.Forms.Label();
            this.textBoxSaveAs = new System.Windows.Forms.TextBox();
            this.label3DView = new System.Windows.Forms.Label();
            this.comboBox3DView = new System.Windows.Forms.ComboBox();
            this.labelGrossBuildingPlan = new System.Windows.Forms.Label();
            this.comboBoxGrossBuildPlan = new System.Windows.Forms.ComboBox();
            this.labelPropertyLine = new System.Windows.Forms.Label();
            this.comboBoxPropertyLine = new System.Windows.Forms.ComboBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxPropertyLineOffset = new System.Windows.Forms.TextBox();
            this.labelUnit = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonBrowser
            // 
            this.buttonBrowser.Location = new System.Drawing.Point(322, 25);
            this.buttonBrowser.Name = "buttonBrowser";
            this.buttonBrowser.Size = new System.Drawing.Size(24, 20);
            this.buttonBrowser.TabIndex = 5;
            this.buttonBrowser.Text = "...";
            this.buttonBrowser.UseVisualStyleBackColor = true;
            this.buttonBrowser.Click += new System.EventHandler(this.buttonBrowser_Click);
            // 
            // labelSaveAs
            // 
            this.labelSaveAs.AutoSize = true;
            this.labelSaveAs.Location = new System.Drawing.Point(11, 9);
            this.labelSaveAs.Name = "labelSaveAs";
            this.labelSaveAs.Size = new System.Drawing.Size(50, 13);
            this.labelSaveAs.TabIndex = 4;
            this.labelSaveAs.Text = "Save As:";
            // 
            // textBoxSaveAs
            // 
            this.textBoxSaveAs.Location = new System.Drawing.Point(14, 25);
            this.textBoxSaveAs.Name = "textBoxSaveAs";
            this.textBoxSaveAs.Size = new System.Drawing.Size(308, 20);
            this.textBoxSaveAs.TabIndex = 3;
            // 
            // label3DView
            // 
            this.label3DView.AutoSize = true;
            this.label3DView.Location = new System.Drawing.Point(11, 51);
            this.label3DView.Name = "label3DView";
            this.label3DView.Size = new System.Drawing.Size(101, 13);
            this.label3DView.TabIndex = 6;
            this.label3DView.Text = "3D View For Export:";
            // 
            // comboBox3DView
            // 
            this.comboBox3DView.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox3DView.FormattingEnabled = true;
            this.comboBox3DView.Location = new System.Drawing.Point(14, 67);
            this.comboBox3DView.Name = "comboBox3DView";
            this.comboBox3DView.Size = new System.Drawing.Size(332, 21);
            this.comboBox3DView.TabIndex = 7;
            // 
            // labelGrossBuildingPlan
            // 
            this.labelGrossBuildingPlan.AutoSize = true;
            this.labelGrossBuildingPlan.Location = new System.Drawing.Point(11, 92);
            this.labelGrossBuildingPlan.Name = "labelGrossBuildingPlan";
            this.labelGrossBuildingPlan.Size = new System.Drawing.Size(101, 13);
            this.labelGrossBuildingPlan.TabIndex = 6;
            this.labelGrossBuildingPlan.Text = "Gross Building Plan:";
            // 
            // comboBoxGrossBuildPlan
            // 
            this.comboBoxGrossBuildPlan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxGrossBuildPlan.FormattingEnabled = true;
            this.comboBoxGrossBuildPlan.Location = new System.Drawing.Point(14, 109);
            this.comboBoxGrossBuildPlan.Name = "comboBoxGrossBuildPlan";
            this.comboBoxGrossBuildPlan.Size = new System.Drawing.Size(332, 21);
            this.comboBoxGrossBuildPlan.TabIndex = 7;
            // 
            // labelPropertyLine
            // 
            this.labelPropertyLine.AutoSize = true;
            this.labelPropertyLine.Location = new System.Drawing.Point(11, 136);
            this.labelPropertyLine.Name = "labelPropertyLine";
            this.labelPropertyLine.Size = new System.Drawing.Size(72, 13);
            this.labelPropertyLine.TabIndex = 6;
            this.labelPropertyLine.Text = "Property Line:";
            // 
            // comboBoxPropertyLine
            // 
            this.comboBoxPropertyLine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPropertyLine.FormattingEnabled = true;
            this.comboBoxPropertyLine.Location = new System.Drawing.Point(14, 153);
            this.comboBoxPropertyLine.Name = "comboBoxPropertyLine";
            this.comboBoxPropertyLine.Size = new System.Drawing.Size(332, 21);
            this.comboBoxPropertyLine.TabIndex = 7;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(271, 220);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 19;
            this.buttonCancel.Text = "&Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(190, 220);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 18;
            this.buttonOK.Text = "&Save";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 186);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "Property Line Offset:";
            // 
            // textBoxPropertyLineOffset
            // 
            this.textBoxPropertyLineOffset.Location = new System.Drawing.Point(120, 183);
            this.textBoxPropertyLineOffset.Name = "textBoxPropertyLineOffset";
            this.textBoxPropertyLineOffset.Size = new System.Drawing.Size(202, 20);
            this.textBoxPropertyLineOffset.TabIndex = 21;
            // 
            // labelUnit
            // 
            this.labelUnit.Location = new System.Drawing.Point(319, 186);
            this.labelUnit.Name = "labelUnit";
            this.labelUnit.Size = new System.Drawing.Size(24, 13);
            this.labelUnit.TabIndex = 22;
            this.labelUnit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ExportCivil3DForm
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(360, 250);
            this.Controls.Add(this.labelUnit);
            this.Controls.Add(this.textBoxPropertyLineOffset);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.comboBoxPropertyLine);
            this.Controls.Add(this.labelPropertyLine);
            this.Controls.Add(this.comboBoxGrossBuildPlan);
            this.Controls.Add(this.labelGrossBuildingPlan);
            this.Controls.Add(this.comboBox3DView);
            this.Controls.Add(this.label3DView);
            this.Controls.Add(this.buttonBrowser);
            this.Controls.Add(this.labelSaveAs);
            this.Controls.Add(this.textBoxSaveAs);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExportCivil3DForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Export Civil3D";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonBrowser;
        private System.Windows.Forms.Label labelSaveAs;
        private System.Windows.Forms.TextBox textBoxSaveAs;
        private System.Windows.Forms.Label label3DView;
        private System.Windows.Forms.ComboBox comboBox3DView;
        private System.Windows.Forms.Label labelGrossBuildingPlan;
        private System.Windows.Forms.ComboBox comboBoxGrossBuildPlan;
        private System.Windows.Forms.Label labelPropertyLine;
        private System.Windows.Forms.ComboBox comboBoxPropertyLine;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxPropertyLineOffset;
        private System.Windows.Forms.Label labelUnit;
    }
}
