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
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE. AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
//

namespace Revit.SDK.Samples.PlaceFamilyInstanceByFace.CS
{
    partial class PlaceFamilyInstanceForm
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
            this.buttonCreate = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.comboBoxFace = new System.Windows.Forms.ComboBox();
            this.labelFace = new System.Windows.Forms.Label();
            this.labelFamilySymbol = new System.Windows.Forms.Label();
            this.comboBoxFamily = new System.Windows.Forms.ComboBox();
            this.labelSecond = new System.Windows.Forms.Label();
            this.labelFirst = new System.Windows.Forms.Label();
            this.PointControlSecond = new Revit.SDK.Samples.PlaceFamilyInstanceByFace.CS.PointUserControl();
            this.PointControlFirst = new Revit.SDK.Samples.PlaceFamilyInstanceByFace.CS.PointUserControl();
            this.SuspendLayout();
            // 
            // buttonCreate
            // 
            this.buttonCreate.Location = new System.Drawing.Point(144, 159);
            this.buttonCreate.Name = "buttonCreate";
            this.buttonCreate.Size = new System.Drawing.Size(73, 23);
            this.buttonCreate.TabIndex = 0;
            this.buttonCreate.Text = "Crea&te";
            this.buttonCreate.UseVisualStyleBackColor = true;
            this.buttonCreate.Click += new System.EventHandler(this.buttonCreate_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(233, 159);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(73, 23);
            this.buttonCancel.TabIndex = 16;
            this.buttonCancel.Text = "&Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // comboBoxFace
            // 
            this.comboBoxFace.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFace.FormattingEnabled = true;
            this.comboBoxFace.Location = new System.Drawing.Point(92, 14);
            this.comboBoxFace.Name = "comboBoxFace";
            this.comboBoxFace.Size = new System.Drawing.Size(214, 21);
            this.comboBoxFace.TabIndex = 7;
            this.comboBoxFace.SelectedIndexChanged += new System.EventHandler(this.comboBoxFace_SelectedIndexChanged);
            // 
            // labelFace
            // 
            this.labelFace.AutoSize = true;
            this.labelFace.Location = new System.Drawing.Point(9, 17);
            this.labelFace.Name = "labelFace";
            this.labelFace.Size = new System.Drawing.Size(37, 13);
            this.labelFace.TabIndex = 24;
            this.labelFace.Text = "Face :";
            // 
            // labelFamilySymbol
            // 
            this.labelFamilySymbol.AutoSize = true;
            this.labelFamilySymbol.Location = new System.Drawing.Point(9, 49);
            this.labelFamilySymbol.Name = "labelFamilySymbol";
            this.labelFamilySymbol.Size = new System.Drawing.Size(82, 13);
            this.labelFamilySymbol.TabIndex = 25;
            this.labelFamilySymbol.Text = "Family Symbol  :";
            // 
            // comboBoxFamily
            // 
            this.comboBoxFamily.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFamily.FormattingEnabled = true;
            this.comboBoxFamily.Location = new System.Drawing.Point(92, 46);
            this.comboBoxFamily.MaxDropDownItems = 9;
            this.comboBoxFamily.Name = "comboBoxFamily";
            this.comboBoxFamily.Size = new System.Drawing.Size(214, 21);
            this.comboBoxFamily.TabIndex = 26;
            // 
            // labelSecond
            // 
            this.labelSecond.AutoSize = true;
            this.labelSecond.Location = new System.Drawing.Point(9, 115);
            this.labelSecond.Name = "labelSecond";
            this.labelSecond.Size = new System.Drawing.Size(62, 13);
            this.labelSecond.TabIndex = 30;
            this.labelSecond.Text = "End Point  :";
            // 
            // labelFirst
            // 
            this.labelFirst.AutoSize = true;
            this.labelFirst.Location = new System.Drawing.Point(9, 83);
            this.labelFirst.Name = "labelFirst";
            this.labelFirst.Size = new System.Drawing.Size(65, 13);
            this.labelFirst.TabIndex = 29;
            this.labelFirst.Text = "Start Point  :";
            // 
            // PointControlSecond
            // 
            this.PointControlSecond.AutoSize = true;
            this.PointControlSecond.Location = new System.Drawing.Point(90, 112);
            this.PointControlSecond.Name = "PointControlSecond";
            this.PointControlSecond.Size = new System.Drawing.Size(216, 24);
            this.PointControlSecond.TabIndex = 28;
            // 
            // PointControlFirst
            // 
            this.PointControlFirst.AutoSize = true;
            this.PointControlFirst.Location = new System.Drawing.Point(90, 79);
            this.PointControlFirst.Name = "PointControlFirst";
            this.PointControlFirst.Size = new System.Drawing.Size(216, 24);
            this.PointControlFirst.TabIndex = 27;
            // 
            // PlaceFamilyInstanceForm
            // 
            this.AcceptButton = this.buttonCreate;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(319, 193);
            this.Controls.Add(this.PointControlSecond);
            this.Controls.Add(this.PointControlFirst);
            this.Controls.Add(this.labelSecond);
            this.Controls.Add(this.labelFirst);
            this.Controls.Add(this.comboBoxFamily);
            this.Controls.Add(this.labelFamilySymbol);
            this.Controls.Add(this.labelFace);
            this.Controls.Add(this.comboBoxFace);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonCreate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PlaceFamilyInstanceForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Place Family Instance";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonCreate;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.ComboBox comboBoxFace;
        private System.Windows.Forms.Label labelFace;
        private System.Windows.Forms.Label labelFamilySymbol;
        private System.Windows.Forms.ComboBox comboBoxFamily;
        private PointUserControl PointControlSecond;
        private PointUserControl PointControlFirst;
        private System.Windows.Forms.Label labelSecond;
        private System.Windows.Forms.Label labelFirst;
    }
}