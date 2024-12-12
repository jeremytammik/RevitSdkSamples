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

namespace Revit.SDK.Samples.NewHostedSweep.CS
{
    partial class EdgeFetchForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EdgeFetchForm));
            this.comboBoxTypes = new System.Windows.Forms.ComboBox();
            this.groupBoxEdges = new System.Windows.Forms.GroupBox();
            this.treeViewHost = new System.Windows.Forms.TreeView();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pictureBoxPreview = new System.Windows.Forms.PictureBox();
            this.label = new System.Windows.Forms.Label();
            this.imageListForCheckBox = new System.Windows.Forms.ImageList(this.components);
            this.labelHit = new System.Windows.Forms.Label();
            this.groupBoxEdges.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPreview)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBoxTypes
            // 
            this.comboBoxTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTypes.FormattingEnabled = true;
            this.comboBoxTypes.Location = new System.Drawing.Point(12, 38);
            this.comboBoxTypes.Name = "comboBoxTypes";
            this.comboBoxTypes.Size = new System.Drawing.Size(207, 21);
            this.comboBoxTypes.TabIndex = 3;
            // 
            // groupBoxEdges
            // 
            this.groupBoxEdges.Controls.Add(this.treeViewHost);
            this.groupBoxEdges.Location = new System.Drawing.Point(12, 65);
            this.groupBoxEdges.Name = "groupBoxEdges";
            this.groupBoxEdges.Size = new System.Drawing.Size(207, 610);
            this.groupBoxEdges.TabIndex = 4;
            this.groupBoxEdges.TabStop = false;
            this.groupBoxEdges.Text = "Extract Edges for HostedSweep";
            // 
            // treeViewHost
            // 
            this.treeViewHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewHost.Location = new System.Drawing.Point(3, 16);
            this.treeViewHost.Name = "treeViewHost";
            this.treeViewHost.Size = new System.Drawing.Size(201, 591);
            this.treeViewHost.TabIndex = 1;
            this.treeViewHost.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeViewHost_BeforeExpand);
            this.treeViewHost.NodeMouseHover += new System.Windows.Forms.TreeNodeMouseHoverEventHandler(this.treeViewHost_NodeMouseHover);
            this.treeViewHost.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeViewHost_BeforeCollapse);
            this.treeViewHost.KeyDown += new System.Windows.Forms.KeyEventHandler(this.treeViewHost_KeyDown);
            this.treeViewHost.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeViewHost_MouseDown);
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(28, 681);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 5;
            this.buttonOK.Text = "&OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(125, 681);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 6;
            this.buttonCancel.Text = "&Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.pictureBoxPreview);
            this.groupBox1.Location = new System.Drawing.Point(225, 11);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(604, 615);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Preview";
            // 
            // pictureBoxPreview
            // 
            this.pictureBoxPreview.BackColor = System.Drawing.SystemColors.WindowText;
            this.pictureBoxPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxPreview.Location = new System.Drawing.Point(3, 16);
            this.pictureBoxPreview.Name = "pictureBoxPreview";
            this.pictureBoxPreview.Size = new System.Drawing.Size(598, 596);
            this.pictureBoxPreview.TabIndex = 10;
            this.pictureBoxPreview.TabStop = false;
            this.pictureBoxPreview.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBoxPreview_MouseDown);
            this.pictureBoxPreview.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBoxPreview_MouseMove);
            this.pictureBoxPreview.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBoxPreview_Paint);
            this.pictureBoxPreview.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBoxPreview_MouseClick);
            // 
            // label
            // 
            this.label.AutoSize = true;
            this.label.Location = new System.Drawing.Point(15, 11);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(157, 13);
            this.label.TabIndex = 9;
            this.label.Text = "Select a type for HostedSweep:";
            // 
            // imageListForCheckBox
            // 
            this.imageListForCheckBox.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListForCheckBox.ImageStream")));
            this.imageListForCheckBox.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListForCheckBox.Images.SetKeyName(0, "CBUnchecked.bmp");
            this.imageListForCheckBox.Images.SetKeyName(1, "CBchecked.bmp");
            this.imageListForCheckBox.Images.SetKeyName(2, "CBIndeterminate.bmp");
            // 
            // labelHit
            // 
            this.labelHit.AutoSize = true;
            this.labelHit.Location = new System.Drawing.Point(226, 633);
            this.labelHit.Name = "labelHit";
            this.labelHit.Size = new System.Drawing.Size(527, 52);
            this.labelHit.TabIndex = 10;
            this.labelHit.Text = resources.GetString("labelHit.Text");
            // 
            // EdgeFetchForm
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(834, 716);
            this.Controls.Add(this.labelHit);
            this.Controls.Add(this.label);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.groupBoxEdges);
            this.Controls.Add(this.comboBoxTypes);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EdgeFetchForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Extract edges for Hosted Sweep";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EdgeFetch_KeyDown);
            this.Load += new System.EventHandler(this.EdgeFetch_Load);
            this.groupBoxEdges.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPreview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxTypes;
        private System.Windows.Forms.GroupBox groupBoxEdges;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TreeView treeViewHost;
        private System.Windows.Forms.Label label;
        private System.Windows.Forms.ImageList imageListForCheckBox;
        private System.Windows.Forms.PictureBox pictureBoxPreview;
        private System.Windows.Forms.Label labelHit;
    }
}