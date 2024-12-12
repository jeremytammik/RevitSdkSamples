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

namespace Revit.SDK.Samples.CurtainWallGrid.CS
{
   partial class GridForm
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
            this.mainTabControl = new System.Windows.Forms.TabControl();
            this.curtainWallTabPage = new System.Windows.Forms.TabPage();
            this.exitButton = new System.Windows.Forms.Button();
            this.wallTypeMsgLabel = new System.Windows.Forms.Label();
            this.viewMsgLabel = new System.Windows.Forms.Label();
            this.createButton = new System.Windows.Forms.Button();
            this.clearButton = new System.Windows.Forms.Button();
            this.wallTypelabel = new System.Windows.Forms.Label();
            this.wallTypeComboBox = new System.Windows.Forms.ComboBox();
            this.viewLabel = new System.Windows.Forms.Label();
            this.viewComboBox = new System.Windows.Forms.ComboBox();
            this.curtainWallPictureBox = new System.Windows.Forms.PictureBox();
            this.curtainGridTabPage = new System.Windows.Forms.TabPage();
            this.gridPropertyPanel = new System.Windows.Forms.Panel();
            this.gridExitButton = new System.Windows.Forms.Button();
            this.curtainGridPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.curtainGridPicturePanel = new System.Windows.Forms.Panel();
            this.gridOperationLabel = new System.Windows.Forms.Label();
            this.lineOperationsComboBox = new System.Windows.Forms.ComboBox();
            this.curtainGridPictureBox = new System.Windows.Forms.PictureBox();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.operationStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.mainTabControl.SuspendLayout();
            this.curtainWallTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.curtainWallPictureBox)).BeginInit();
            this.curtainGridTabPage.SuspendLayout();
            this.gridPropertyPanel.SuspendLayout();
            this.curtainGridPicturePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.curtainGridPictureBox)).BeginInit();
            this.statusStrip.SuspendLayout();
            this.mainPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainTabControl
            // 
            this.mainTabControl.Controls.Add(this.curtainWallTabPage);
            this.mainTabControl.Controls.Add(this.curtainGridTabPage);
            this.mainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTabControl.Location = new System.Drawing.Point(0, 0);
            this.mainTabControl.Name = "mainTabControl";
            this.mainTabControl.SelectedIndex = 0;
            this.mainTabControl.Size = new System.Drawing.Size(467, 321);
            this.mainTabControl.TabIndex = 0;
            this.mainTabControl.SelectedIndexChanged += new System.EventHandler(this.mainTabControl_SelectedIndexChanged);
            this.mainTabControl.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.mainTabControl_Selecting);
            this.mainTabControl.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.mainTabControl_KeyPress);
            // 
            // curtainWallTabPage
            // 
            this.curtainWallTabPage.Controls.Add(this.exitButton);
            this.curtainWallTabPage.Controls.Add(this.wallTypeMsgLabel);
            this.curtainWallTabPage.Controls.Add(this.viewMsgLabel);
            this.curtainWallTabPage.Controls.Add(this.createButton);
            this.curtainWallTabPage.Controls.Add(this.clearButton);
            this.curtainWallTabPage.Controls.Add(this.wallTypelabel);
            this.curtainWallTabPage.Controls.Add(this.wallTypeComboBox);
            this.curtainWallTabPage.Controls.Add(this.viewLabel);
            this.curtainWallTabPage.Controls.Add(this.viewComboBox);
            this.curtainWallTabPage.Controls.Add(this.curtainWallPictureBox);
            this.curtainWallTabPage.Location = new System.Drawing.Point(4, 22);
            this.curtainWallTabPage.Name = "curtainWallTabPage";
            this.curtainWallTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.curtainWallTabPage.Size = new System.Drawing.Size(459, 295);
            this.curtainWallTabPage.TabIndex = 0;
            this.curtainWallTabPage.Text = "Create Curtain Wall";
            this.curtainWallTabPage.UseVisualStyleBackColor = true;
            // 
            // exitButton
            // 
            this.exitButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.exitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.exitButton.Location = new System.Drawing.Point(276, 262);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(165, 23);
            this.exitButton.TabIndex = 7;
            this.exitButton.Text = "E&xit";
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.Click += new System.EventHandler(this.exitButton_Click);
            // 
            // wallTypeMsgLabel
            // 
            this.wallTypeMsgLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.wallTypeMsgLabel.AutoSize = true;
            this.wallTypeMsgLabel.Location = new System.Drawing.Point(273, 88);
            this.wallTypeMsgLabel.Name = "wallTypeMsgLabel";
            this.wallTypeMsgLabel.Size = new System.Drawing.Size(107, 13);
            this.wallTypeMsgLabel.TabIndex = 6;
            this.wallTypeMsgLabel.Text = "Specify the wall type:";
            // 
            // viewMsgLabel
            // 
            this.viewMsgLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.viewMsgLabel.AutoSize = true;
            this.viewMsgLabel.Location = new System.Drawing.Point(273, 36);
            this.viewMsgLabel.Name = "viewMsgLabel";
            this.viewMsgLabel.Size = new System.Drawing.Size(168, 13);
            this.viewMsgLabel.TabIndex = 5;
            this.viewMsgLabel.Text = "Specify a view for the curtain wall:";
            // 
            // createButton
            // 
            this.createButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.createButton.Enabled = false;
            this.createButton.Location = new System.Drawing.Point(276, 186);
            this.createButton.Name = "createButton";
            this.createButton.Size = new System.Drawing.Size(165, 23);
            this.createButton.TabIndex = 4;
            this.createButton.Text = "Create Curtain &Wall";
            this.createButton.UseVisualStyleBackColor = true;
            this.createButton.Click += new System.EventHandler(this.createButton_Click);
            // 
            // clearButton
            // 
            this.clearButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.clearButton.Enabled = false;
            this.clearButton.Location = new System.Drawing.Point(276, 147);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(165, 23);
            this.clearButton.TabIndex = 3;
            this.clearButton.Text = "&Clear Baseline";
            this.clearButton.UseVisualStyleBackColor = true;
            this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
            // 
            // wallTypelabel
            // 
            this.wallTypelabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.wallTypelabel.AutoSize = true;
            this.wallTypelabel.Location = new System.Drawing.Point(273, 112);
            this.wallTypelabel.Name = "wallTypelabel";
            this.wallTypelabel.Size = new System.Drawing.Size(37, 13);
            this.wallTypelabel.TabIndex = 4;
            this.wallTypelabel.Text = " Type:";
            // 
            // wallTypeComboBox
            // 
            this.wallTypeComboBox.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.wallTypeComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.wallTypeComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.wallTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.wallTypeComboBox.FormattingEnabled = true;
            this.wallTypeComboBox.Location = new System.Drawing.Point(312, 109);
            this.wallTypeComboBox.Name = "wallTypeComboBox";
            this.wallTypeComboBox.Size = new System.Drawing.Size(129, 21);
            this.wallTypeComboBox.TabIndex = 2;
            this.wallTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.wallTypeComboBox_SelectedIndexChanged);
            // 
            // viewLabel
            // 
            this.viewLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.viewLabel.AutoSize = true;
            this.viewLabel.Location = new System.Drawing.Point(273, 62);
            this.viewLabel.Name = "viewLabel";
            this.viewLabel.Size = new System.Drawing.Size(33, 13);
            this.viewLabel.TabIndex = 2;
            this.viewLabel.Text = "View:";
            // 
            // viewComboBox
            // 
            this.viewComboBox.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.viewComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.viewComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.viewComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.viewComboBox.FormattingEnabled = true;
            this.viewComboBox.Location = new System.Drawing.Point(312, 59);
            this.viewComboBox.Name = "viewComboBox";
            this.viewComboBox.Size = new System.Drawing.Size(129, 21);
            this.viewComboBox.TabIndex = 1;
            this.viewComboBox.SelectedIndexChanged += new System.EventHandler(this.viewComboBox_SelectedIndexChanged);
            // 
            // curtainWallPictureBox
            // 
            this.curtainWallPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.curtainWallPictureBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.curtainWallPictureBox.Location = new System.Drawing.Point(3, 3);
            this.curtainWallPictureBox.Name = "curtainWallPictureBox";
            this.curtainWallPictureBox.Size = new System.Drawing.Size(266, 289);
            this.curtainWallPictureBox.TabIndex = 0;
            this.curtainWallPictureBox.TabStop = false;
            this.curtainWallPictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.curtainWallPictureBox_Paint);
            this.curtainWallPictureBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.curtainWallPictureBox_MouseClick);
            this.curtainWallPictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.curtainWallPictureBox_MouseMove);
            // 
            // curtainGridTabPage
            // 
            this.curtainGridTabPage.Controls.Add(this.gridPropertyPanel);
            this.curtainGridTabPage.Controls.Add(this.curtainGridPicturePanel);
            this.curtainGridTabPage.Location = new System.Drawing.Point(4, 22);
            this.curtainGridTabPage.Name = "curtainGridTabPage";
            this.curtainGridTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.curtainGridTabPage.Size = new System.Drawing.Size(459, 295);
            this.curtainGridTabPage.TabIndex = 1;
            this.curtainGridTabPage.Text = "Curtain Grid";
            this.curtainGridTabPage.UseVisualStyleBackColor = true;
            // 
            // gridPropertyPanel
            // 
            this.gridPropertyPanel.Controls.Add(this.gridExitButton);
            this.gridPropertyPanel.Controls.Add(this.curtainGridPropertyGrid);
            this.gridPropertyPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridPropertyPanel.Location = new System.Drawing.Point(295, 3);
            this.gridPropertyPanel.Name = "gridPropertyPanel";
            this.gridPropertyPanel.Size = new System.Drawing.Size(161, 289);
            this.gridPropertyPanel.TabIndex = 2;
            // 
            // gridExitButton
            // 
            this.gridExitButton.Location = new System.Drawing.Point(15, 264);
            this.gridExitButton.Name = "gridExitButton";
            this.gridExitButton.Size = new System.Drawing.Size(132, 24);
            this.gridExitButton.TabIndex = 1;
            this.gridExitButton.Text = "E&xit";
            this.gridExitButton.UseVisualStyleBackColor = true;
            this.gridExitButton.Click += new System.EventHandler(this.gridExitButton_Click);
            // 
            // curtainGridPropertyGrid
            // 
            this.curtainGridPropertyGrid.CategoryForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.curtainGridPropertyGrid.Dock = System.Windows.Forms.DockStyle.Top;
            this.curtainGridPropertyGrid.HelpVisible = false;
            this.curtainGridPropertyGrid.Location = new System.Drawing.Point(0, 0);
            this.curtainGridPropertyGrid.Name = "curtainGridPropertyGrid";
            this.curtainGridPropertyGrid.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.curtainGridPropertyGrid.Size = new System.Drawing.Size(161, 262);
            this.curtainGridPropertyGrid.TabIndex = 0;
            this.curtainGridPropertyGrid.ToolbarVisible = false;
            // 
            // curtainGridPicturePanel
            // 
            this.curtainGridPicturePanel.Controls.Add(this.gridOperationLabel);
            this.curtainGridPicturePanel.Controls.Add(this.lineOperationsComboBox);
            this.curtainGridPicturePanel.Controls.Add(this.curtainGridPictureBox);
            this.curtainGridPicturePanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.curtainGridPicturePanel.Location = new System.Drawing.Point(3, 3);
            this.curtainGridPicturePanel.Name = "curtainGridPicturePanel";
            this.curtainGridPicturePanel.Size = new System.Drawing.Size(292, 289);
            this.curtainGridPicturePanel.TabIndex = 1;
            // 
            // gridOperationLabel
            // 
            this.gridOperationLabel.AutoSize = true;
            this.gridOperationLabel.Location = new System.Drawing.Point(4, 270);
            this.gridOperationLabel.Name = "gridOperationLabel";
            this.gridOperationLabel.Size = new System.Drawing.Size(78, 13);
            this.gridOperationLabel.TabIndex = 3;
            this.gridOperationLabel.Text = "Grid Operation:";
            // 
            // lineOperationsComboBox
            // 
            this.lineOperationsComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.lineOperationsComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.lineOperationsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lineOperationsComboBox.FormattingEnabled = true;
            this.lineOperationsComboBox.Location = new System.Drawing.Point(86, 267);
            this.lineOperationsComboBox.Name = "lineOperationsComboBox";
            this.lineOperationsComboBox.Size = new System.Drawing.Size(191, 21);
            this.lineOperationsComboBox.TabIndex = 2;
            this.lineOperationsComboBox.SelectedIndexChanged += new System.EventHandler(this.lineOperationsComboBox_SelectedIndexChanged);
            // 
            // curtainGridPictureBox
            // 
            this.curtainGridPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.curtainGridPictureBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.curtainGridPictureBox.Location = new System.Drawing.Point(0, 0);
            this.curtainGridPictureBox.Name = "curtainGridPictureBox";
            this.curtainGridPictureBox.Size = new System.Drawing.Size(292, 262);
            this.curtainGridPictureBox.TabIndex = 0;
            this.curtainGridPictureBox.TabStop = false;
            this.curtainGridPictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.curtainGridPictureBox_Paint);
            this.curtainGridPictureBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.curtainGridPictureBox_MouseClick);
            this.curtainGridPictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.curtainGridPictureBox_MouseMove);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.operationStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 321);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(467, 22);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.Stretch = false;
            this.statusStrip.TabIndex = 1;
            // 
            // operationStatusLabel
            // 
            this.operationStatusLabel.Name = "operationStatusLabel";
            this.operationStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.mainTabControl);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(0, 0);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(467, 321);
            this.mainPanel.TabIndex = 2;
            // 
            // GridForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(467, 343);
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.statusStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GridForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Curtain Wall";
            this.Load += new System.EventHandler(this.GridForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GridForm_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.GridForm_KeyUp);
            this.mainTabControl.ResumeLayout(false);
            this.curtainWallTabPage.ResumeLayout(false);
            this.curtainWallTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.curtainWallPictureBox)).EndInit();
            this.curtainGridTabPage.ResumeLayout(false);
            this.gridPropertyPanel.ResumeLayout(false);
            this.curtainGridPicturePanel.ResumeLayout(false);
            this.curtainGridPicturePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.curtainGridPictureBox)).EndInit();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.mainPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.TabControl mainTabControl;
      private System.Windows.Forms.TabPage curtainWallTabPage;
      private System.Windows.Forms.TabPage curtainGridTabPage;
      private System.Windows.Forms.PictureBox curtainWallPictureBox;
      private System.Windows.Forms.Label viewLabel;
      private System.Windows.Forms.ComboBox viewComboBox;
      private System.Windows.Forms.Label wallTypelabel;
      private System.Windows.Forms.ComboBox wallTypeComboBox;
      private System.Windows.Forms.Button createButton;
      private System.Windows.Forms.Button clearButton;
      private System.Windows.Forms.PropertyGrid curtainGridPropertyGrid;
      private System.Windows.Forms.StatusStrip statusStrip;
      private System.Windows.Forms.Panel mainPanel;
      private System.Windows.Forms.ToolStripStatusLabel operationStatusLabel;
      private System.Windows.Forms.Panel curtainGridPicturePanel;
      private System.Windows.Forms.PictureBox curtainGridPictureBox;
      private System.Windows.Forms.ComboBox lineOperationsComboBox;
      private System.Windows.Forms.Label gridOperationLabel;
      private System.Windows.Forms.Button exitButton;
      private System.Windows.Forms.Label wallTypeMsgLabel;
      private System.Windows.Forms.Label viewMsgLabel;
      private System.Windows.Forms.Panel gridPropertyPanel;
      private System.Windows.Forms.Button gridExitButton;
   }
}