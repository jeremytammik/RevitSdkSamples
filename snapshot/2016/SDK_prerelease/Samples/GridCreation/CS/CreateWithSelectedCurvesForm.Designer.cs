//
// (C) Copyright 2003-2014 by Autodesk, Inc.
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
    partial class CreateWithSelectedCurvesForm
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
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.textBoxFirstLabel = new System.Windows.Forms.TextBox();
            this.comboBoxBubbleLocation = new System.Windows.Forms.ComboBox();
            this.labelBubbleLocation = new System.Windows.Forms.Label();
            this.labelFirstLabel = new System.Windows.Forms.Label();
            this.groupBoxGridSettings = new System.Windows.Forms.GroupBox();
            this.checkBoxDeleteElements = new System.Windows.Forms.CheckBox();
            this.groupBoxGridSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(232, 144);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(90, 23);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "&Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(126, 144);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(90, 23);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "Create &Grids";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // textBoxFirstLabel
            // 
            this.textBoxFirstLabel.Location = new System.Drawing.Point(124, 53);
            this.textBoxFirstLabel.Name = "textBoxFirstLabel";
            this.textBoxFirstLabel.Size = new System.Drawing.Size(171, 20);
            this.textBoxFirstLabel.TabIndex = 1;
            this.textBoxFirstLabel.Tag = "";
            this.textBoxFirstLabel.Text = "1";
            // 
            // comboBoxBubbleLocation
            // 
            this.comboBoxBubbleLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBubbleLocation.FormattingEnabled = true;
            this.comboBoxBubbleLocation.Items.AddRange(new object[] {
            "At start point of lines/arcs",
            "At end point of  lines/arcs"});
            this.comboBoxBubbleLocation.Location = new System.Drawing.Point(124, 19);
            this.comboBoxBubbleLocation.Name = "comboBoxBubbleLocation";
            this.comboBoxBubbleLocation.Size = new System.Drawing.Size(171, 21);
            this.comboBoxBubbleLocation.TabIndex = 0;
            // 
            // labelBubbleLocation
            // 
            this.labelBubbleLocation.Location = new System.Drawing.Point(6, 21);
            this.labelBubbleLocation.Name = "labelBubbleLocation";
            this.labelBubbleLocation.Size = new System.Drawing.Size(112, 19);
            this.labelBubbleLocation.TabIndex = 16;
            this.labelBubbleLocation.Text = "Bubble location:";
            // 
            // labelFirstLabel
            // 
            this.labelFirstLabel.Location = new System.Drawing.Point(6, 56);
            this.labelFirstLabel.Name = "labelFirstLabel";
            this.labelFirstLabel.Size = new System.Drawing.Size(112, 19);
            this.labelFirstLabel.TabIndex = 15;
            this.labelFirstLabel.Text = "Label of first grid:";
            // 
            // groupBoxGridSettings
            // 
            this.groupBoxGridSettings.Controls.Add(this.checkBoxDeleteElements);
            this.groupBoxGridSettings.Controls.Add(this.labelBubbleLocation);
            this.groupBoxGridSettings.Controls.Add(this.textBoxFirstLabel);
            this.groupBoxGridSettings.Controls.Add(this.labelFirstLabel);
            this.groupBoxGridSettings.Controls.Add(this.comboBoxBubbleLocation);
            this.groupBoxGridSettings.Location = new System.Drawing.Point(12, 12);
            this.groupBoxGridSettings.Name = "groupBoxGridSettings";
            this.groupBoxGridSettings.Size = new System.Drawing.Size(310, 113);
            this.groupBoxGridSettings.TabIndex = 18;
            this.groupBoxGridSettings.TabStop = false;
            this.groupBoxGridSettings.Text = "Settings";
            // 
            // checkBoxDeleteElements
            // 
            this.checkBoxDeleteElements.AutoSize = true;
            this.checkBoxDeleteElements.Checked = true;
            this.checkBoxDeleteElements.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxDeleteElements.Location = new System.Drawing.Point(9, 87);
            this.checkBoxDeleteElements.Name = "checkBoxDeleteElements";
            this.checkBoxDeleteElements.Size = new System.Drawing.Size(232, 17);
            this.checkBoxDeleteElements.TabIndex = 2;
            this.checkBoxDeleteElements.Text = "Delete the selected lines/arcs after creation";
            this.checkBoxDeleteElements.UseVisualStyleBackColor = true;
            // 
            // CreateWithSelectedCurvesForm
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(334, 177);
            this.Controls.Add(this.groupBoxGridSettings);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CreateWithSelectedCurvesForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Create Grids with Lines/Arcs";
            this.groupBoxGridSettings.ResumeLayout(false);
            this.groupBoxGridSettings.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.TextBox textBoxFirstLabel;
        private System.Windows.Forms.ComboBox comboBoxBubbleLocation;
        private System.Windows.Forms.Label labelBubbleLocation;
        private System.Windows.Forms.Label labelFirstLabel;
        private System.Windows.Forms.GroupBox groupBoxGridSettings;
        private System.Windows.Forms.CheckBox checkBoxDeleteElements;
    }
}