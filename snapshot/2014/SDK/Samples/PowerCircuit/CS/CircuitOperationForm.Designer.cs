//
// (C) Copyright 2003-2013 by Autodesk, Inc.
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

namespace Revit.SDK.Samples.PowerCircuit.CS
{
    partial class CircuitOperationForm
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
            this.components = new System.ComponentModel.Container();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonDisconnectPanel = new System.Windows.Forms.Button();
            this.buttonSelectPanel = new System.Windows.Forms.Button();
            this.buttonEdit = new System.Windows.Forms.Button();
            this.buttonCreate = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Image = global::Revit.SDK.Samples.PowerCircuit.CS.Properties.Resources.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(177, 8);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(84, 29);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonDisconnectPanel
            // 
            this.buttonDisconnectPanel.BackColor = System.Drawing.Color.Transparent;
            this.buttonDisconnectPanel.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonDisconnectPanel.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.buttonDisconnectPanel.FlatAppearance.BorderSize = 0;
            this.buttonDisconnectPanel.Image = global::Revit.SDK.Samples.PowerCircuit.CS.Properties.Resources.DisconnectPanel;
            this.buttonDisconnectPanel.Location = new System.Drawing.Point(133, 9);
            this.buttonDisconnectPanel.Name = "buttonDisconnectPanel";
            this.buttonDisconnectPanel.Size = new System.Drawing.Size(34, 28);
            this.buttonDisconnectPanel.TabIndex = 3;
            this.buttonDisconnectPanel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.buttonDisconnectPanel.UseVisualStyleBackColor = false;
            this.buttonDisconnectPanel.Click += new System.EventHandler(this.buttonDisconnectPanel_Click);
            // 
            // buttonSelectPanel
            // 
            this.buttonSelectPanel.BackColor = System.Drawing.Color.Transparent;
            this.buttonSelectPanel.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonSelectPanel.FlatAppearance.BorderSize = 0;
            this.buttonSelectPanel.Image = global::Revit.SDK.Samples.PowerCircuit.CS.Properties.Resources.SelectPanel;
            this.buttonSelectPanel.Location = new System.Drawing.Point(93, 9);
            this.buttonSelectPanel.Name = "buttonSelectPanel";
            this.buttonSelectPanel.Size = new System.Drawing.Size(34, 28);
            this.buttonSelectPanel.TabIndex = 2;
            this.buttonSelectPanel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.buttonSelectPanel.UseVisualStyleBackColor = false;
            this.buttonSelectPanel.Click += new System.EventHandler(this.buttonSelectPanel_Click);
            // 
            // buttonEdit
            // 
            this.buttonEdit.BackColor = System.Drawing.Color.Transparent;
            this.buttonEdit.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonEdit.FlatAppearance.BorderSize = 0;
            this.buttonEdit.Image = global::Revit.SDK.Samples.PowerCircuit.CS.Properties.Resources.EditCircuit;
            this.buttonEdit.Location = new System.Drawing.Point(55, 9);
            this.buttonEdit.Name = "buttonEdit";
            this.buttonEdit.Size = new System.Drawing.Size(34, 28);
            this.buttonEdit.TabIndex = 1;
            this.buttonEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.buttonEdit.UseVisualStyleBackColor = false;
            this.buttonEdit.Click += new System.EventHandler(this.buttonEdit_Click);
            // 
            // buttonCreate
            // 
            this.buttonCreate.BackColor = System.Drawing.Color.Transparent;
            this.buttonCreate.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonCreate.FlatAppearance.BorderSize = 0;
            this.buttonCreate.Image = global::Revit.SDK.Samples.PowerCircuit.CS.Properties.Resources.CreateCircuit;
            this.buttonCreate.Location = new System.Drawing.Point(7, 9);
            this.buttonCreate.Name = "buttonCreate";
            this.buttonCreate.Size = new System.Drawing.Size(34, 28);
            this.buttonCreate.TabIndex = 0;
            this.buttonCreate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.buttonCreate.UseVisualStyleBackColor = false;
            this.buttonCreate.Click += new System.EventHandler(this.buttonCreate_Click);
            // 
            // CircuitOperationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(267, 43);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonDisconnectPanel);
            this.Controls.Add(this.buttonSelectPanel);
            this.Controls.Add(this.buttonEdit);
            this.Controls.Add(this.buttonCreate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CircuitOperationForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Power Circuit";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonCreate;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonEdit;
        private System.Windows.Forms.Button buttonSelectPanel;
        private System.Windows.Forms.Button buttonDisconnectPanel;
        private System.Windows.Forms.ToolTip toolTip;
    }
}