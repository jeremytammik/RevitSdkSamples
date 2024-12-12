//
// (C) Copyright 2003-2007 by Autodesk, Inc.
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

namespace Revit.SDK.Samples.ViewPrinter.CS
{
    partial class SecondPanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.viewToPrintPanel = new System.Windows.Forms.Panel();
            this.viewToPrintGroupBox = new System.Windows.Forms.GroupBox();
            this.viewToPrintTreeView = new System.Windows.Forms.TreeView();
            this.viewToPrintPanel.SuspendLayout();
            this.viewToPrintGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // viewToPrintPanel
            // 
            this.viewToPrintPanel.Controls.Add(this.viewToPrintGroupBox);
            this.viewToPrintPanel.Location = new System.Drawing.Point(0, 0);
            this.viewToPrintPanel.Name = "viewToPrintPanel";
            this.viewToPrintPanel.Size = new System.Drawing.Size(280, 190);
            this.viewToPrintPanel.TabIndex = 1;
            // 
            // viewToPrintGroupBox
            // 
            this.viewToPrintGroupBox.Controls.Add(this.viewToPrintTreeView);
            this.viewToPrintGroupBox.Location = new System.Drawing.Point(0, 0);
            this.viewToPrintGroupBox.Name = "viewToPrintGroupBox";
            this.viewToPrintGroupBox.Size = new System.Drawing.Size(280, 190);
            this.viewToPrintGroupBox.TabIndex = 0;
            this.viewToPrintGroupBox.TabStop = false;
            this.viewToPrintGroupBox.Text = "Views to print";
            // 
            // viewToPrintTreeView
            // 
            this.viewToPrintTreeView.CheckBoxes = true;
            this.viewToPrintTreeView.Location = new System.Drawing.Point(6, 19);
            this.viewToPrintTreeView.Name = "viewToPrintTreeView";
            this.viewToPrintTreeView.Size = new System.Drawing.Size(268, 165);
            this.viewToPrintTreeView.TabIndex = 0;
            this.viewToPrintTreeView.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.viewToPrintTreeView_AfterCheck);
            // 
            // SecondPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.viewToPrintPanel);
            this.Name = "SecondPanel";
            this.Size = new System.Drawing.Size(280, 190);
            this.viewToPrintPanel.ResumeLayout(false);
            this.viewToPrintGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel viewToPrintPanel;
        private System.Windows.Forms.GroupBox viewToPrintGroupBox;
        private System.Windows.Forms.TreeView viewToPrintTreeView;
    }
}
