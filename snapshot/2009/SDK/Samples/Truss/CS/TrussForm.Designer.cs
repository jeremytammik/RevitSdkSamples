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
namespace Revit.SDK.Samples.Truss.CS
{
    /// <summary>
    /// window form contains one three picture box to show the 
    /// profile of truss geometry and profile and tabControl.
    /// User can create truss, edit profile of truss and change type of truss members.
    /// </summary>
    partial class TrussForm
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
            this.TrussTypeComboBox = new System.Windows.Forms.ComboBox();
            this.TrussGraphicsTabControl = new System.Windows.Forms.TabControl();
            this.ViewTabPage = new System.Windows.Forms.TabPage();
            this.Notelabel = new System.Windows.Forms.Label();
            this.ViewComboBox = new System.Windows.Forms.ComboBox();
            this.ViewLabel = new System.Windows.Forms.Label();
            this.CreateButton = new System.Windows.Forms.Button();
            this.TrussMembersTabPage = new System.Windows.Forms.TabPage();
            this.BeamTypeLabel = new System.Windows.Forms.Label();
            this.ChangeBeamTypeButton = new System.Windows.Forms.Button();
            this.BeamTypeComboBox = new System.Windows.Forms.ComboBox();
            this.TrussMembersPictureBox = new System.Windows.Forms.PictureBox();
            this.ProfileEditTabPage = new System.Windows.Forms.TabPage();
            this.CleanChordbutton = new System.Windows.Forms.Button();
            this.BottomChordButton = new System.Windows.Forms.Button();
            this.TopChordButton = new System.Windows.Forms.Button();
            this.UpdateButton = new System.Windows.Forms.Button();
            this.RestoreButton = new System.Windows.Forms.Button();
            this.ProfileEditPictureBox = new System.Windows.Forms.PictureBox();
            this.TrussTypeLabel = new System.Windows.Forms.Label();
            this.CloseButton = new System.Windows.Forms.Button();
            this.TrussGraphicsTabControl.SuspendLayout();
            this.ViewTabPage.SuspendLayout();
            this.TrussMembersTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TrussMembersPictureBox)).BeginInit();
            this.ProfileEditTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ProfileEditPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // TrussTypeComboBox
            // 
            this.TrussTypeComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.TrussTypeComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.TrussTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TrussTypeComboBox.FormattingEnabled = true;
            this.TrussTypeComboBox.Location = new System.Drawing.Point(84, 6);
            this.TrussTypeComboBox.Name = "TrussTypeComboBox";
            this.TrussTypeComboBox.Size = new System.Drawing.Size(212, 21);
            this.TrussTypeComboBox.TabIndex = 1;
            this.TrussTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.TrussTypeComboBox_SelectedIndexChanged);
            // 
            // TrussGraphicsTabControl
            // 
            this.TrussGraphicsTabControl.Controls.Add(this.ViewTabPage);
            this.TrussGraphicsTabControl.Controls.Add(this.TrussMembersTabPage);
            this.TrussGraphicsTabControl.Controls.Add(this.ProfileEditTabPage);
            this.TrussGraphicsTabControl.Location = new System.Drawing.Point(12, 41);
            this.TrussGraphicsTabControl.Name = "TrussGraphicsTabControl";
            this.TrussGraphicsTabControl.SelectedIndex = 0;
            this.TrussGraphicsTabControl.Size = new System.Drawing.Size(404, 343);
            this.TrussGraphicsTabControl.TabIndex = 2;
            this.TrussGraphicsTabControl.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.TrussGraphicsTabControl_Selecting);
            this.TrussGraphicsTabControl.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TrussGraphicsTabControl_KeyPress);
            // 
            // ViewTabPage
            // 
            this.ViewTabPage.Controls.Add(this.Notelabel);
            this.ViewTabPage.Controls.Add(this.ViewComboBox);
            this.ViewTabPage.Controls.Add(this.ViewLabel);
            this.ViewTabPage.Controls.Add(this.CreateButton);
            this.ViewTabPage.Location = new System.Drawing.Point(4, 22);
            this.ViewTabPage.Name = "ViewTabPage";
            this.ViewTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.ViewTabPage.Size = new System.Drawing.Size(396, 317);
            this.ViewTabPage.TabIndex = 0;
            this.ViewTabPage.Text = "Create Truss";
            this.ViewTabPage.UseVisualStyleBackColor = true;
            // 
            // Notelabel
            // 
            this.Notelabel.AutoSize = true;
            this.Notelabel.Location = new System.Drawing.Point(6, 17);
            this.Notelabel.Name = "Notelabel";
            this.Notelabel.Size = new System.Drawing.Size(155, 13);
            this.Notelabel.TabIndex = 5;
            this.Notelabel.Text = "Select a View to build truss on :";
            // 
            // ViewComboBox
            // 
            this.ViewComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.ViewComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.ViewComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ViewComboBox.FormattingEnabled = true;
            this.ViewComboBox.Location = new System.Drawing.Point(48, 57);
            this.ViewComboBox.Name = "ViewComboBox";
            this.ViewComboBox.Size = new System.Drawing.Size(236, 21);
            this.ViewComboBox.TabIndex = 4;
            this.ViewComboBox.SelectedIndexChanged += new System.EventHandler(this.ViewComboBox_SelectedIndexChanged);
            // 
            // ViewLabel
            // 
            this.ViewLabel.AutoSize = true;
            this.ViewLabel.Location = new System.Drawing.Point(6, 60);
            this.ViewLabel.Name = "ViewLabel";
            this.ViewLabel.Size = new System.Drawing.Size(36, 13);
            this.ViewLabel.TabIndex = 3;
            this.ViewLabel.Text = "View :";
            // 
            // CreateButton
            // 
            this.CreateButton.Location = new System.Drawing.Point(301, 55);
            this.CreateButton.Name = "CreateButton";
            this.CreateButton.Size = new System.Drawing.Size(80, 23);
            this.CreateButton.TabIndex = 2;
            this.CreateButton.Text = "&Create Truss";
            this.CreateButton.UseVisualStyleBackColor = true;
            this.CreateButton.Click += new System.EventHandler(this.CreateButton_Click);
            // 
            // TrussMembersTabPage
            // 
            this.TrussMembersTabPage.Controls.Add(this.BeamTypeLabel);
            this.TrussMembersTabPage.Controls.Add(this.ChangeBeamTypeButton);
            this.TrussMembersTabPage.Controls.Add(this.BeamTypeComboBox);
            this.TrussMembersTabPage.Controls.Add(this.TrussMembersPictureBox);
            this.TrussMembersTabPage.Location = new System.Drawing.Point(4, 22);
            this.TrussMembersTabPage.Name = "TrussMembersTabPage";
            this.TrussMembersTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.TrussMembersTabPage.Size = new System.Drawing.Size(396, 317);
            this.TrussMembersTabPage.TabIndex = 1;
            this.TrussMembersTabPage.Text = "Truss Members";
            this.TrussMembersTabPage.UseVisualStyleBackColor = true;
            // 
            // BeamTypeLabel
            // 
            this.BeamTypeLabel.AutoSize = true;
            this.BeamTypeLabel.Location = new System.Drawing.Point(6, 291);
            this.BeamTypeLabel.Name = "BeamTypeLabel";
            this.BeamTypeLabel.Size = new System.Drawing.Size(67, 13);
            this.BeamTypeLabel.TabIndex = 3;
            this.BeamTypeLabel.Text = "Beam Type :";
            // 
            // ChangeBeamTypeButton
            // 
            this.ChangeBeamTypeButton.Enabled = false;
            this.ChangeBeamTypeButton.Location = new System.Drawing.Point(309, 288);
            this.ChangeBeamTypeButton.Name = "ChangeBeamTypeButton";
            this.ChangeBeamTypeButton.Size = new System.Drawing.Size(81, 21);
            this.ChangeBeamTypeButton.TabIndex = 2;
            this.ChangeBeamTypeButton.Text = "Change Type";
            this.ChangeBeamTypeButton.UseVisualStyleBackColor = true;
            this.ChangeBeamTypeButton.Click += new System.EventHandler(this.ChangeBeamTypeButton_Click);
            // 
            // BeamTypeComboBox
            // 
            this.BeamTypeComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.BeamTypeComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.BeamTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.BeamTypeComboBox.Enabled = false;
            this.BeamTypeComboBox.FormattingEnabled = true;
            this.BeamTypeComboBox.Location = new System.Drawing.Point(79, 288);
            this.BeamTypeComboBox.Name = "BeamTypeComboBox";
            this.BeamTypeComboBox.Size = new System.Drawing.Size(217, 21);
            this.BeamTypeComboBox.TabIndex = 1;
            this.BeamTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.BeamTypeComboBox_SelectedIndexChanged);
            // 
            // TrussMembersPictureBox
            // 
            this.TrussMembersPictureBox.Location = new System.Drawing.Point(7, 7);
            this.TrussMembersPictureBox.Name = "TrussMembersPictureBox";
            this.TrussMembersPictureBox.Size = new System.Drawing.Size(384, 274);
            this.TrussMembersPictureBox.TabIndex = 0;
            this.TrussMembersPictureBox.TabStop = false;
            this.TrussMembersPictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TrussGeometryPictureBox_MouseMove);
            this.TrussMembersPictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.TrussGeometryPictureBox_Paint);
            this.TrussMembersPictureBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.TrussGeometryPictureBox_MouseClick);
            // 
            // ProfileEditTabPage
            // 
            this.ProfileEditTabPage.Controls.Add(this.CleanChordbutton);
            this.ProfileEditTabPage.Controls.Add(this.BottomChordButton);
            this.ProfileEditTabPage.Controls.Add(this.TopChordButton);
            this.ProfileEditTabPage.Controls.Add(this.UpdateButton);
            this.ProfileEditTabPage.Controls.Add(this.RestoreButton);
            this.ProfileEditTabPage.Controls.Add(this.ProfileEditPictureBox);
            this.ProfileEditTabPage.Location = new System.Drawing.Point(4, 22);
            this.ProfileEditTabPage.Name = "ProfileEditTabPage";
            this.ProfileEditTabPage.Size = new System.Drawing.Size(396, 317);
            this.ProfileEditTabPage.TabIndex = 2;
            this.ProfileEditTabPage.Text = "Profile Edit";
            this.ProfileEditTabPage.UseVisualStyleBackColor = true;
            // 
            // CleanChordbutton
            // 
            this.CleanChordbutton.Location = new System.Drawing.Point(177, 286);
            this.CleanChordbutton.Name = "CleanChordbutton";
            this.CleanChordbutton.Size = new System.Drawing.Size(51, 23);
            this.CleanChordbutton.TabIndex = 8;
            this.CleanChordbutton.Text = "&Clean";
            this.CleanChordbutton.UseVisualStyleBackColor = true;
            this.CleanChordbutton.Click += new System.EventHandler(this.CleanChordbutton_Click);
            // 
            // BottomChordButton
            // 
            this.BottomChordButton.Location = new System.Drawing.Point(88, 286);
            this.BottomChordButton.Name = "BottomChordButton";
            this.BottomChordButton.Size = new System.Drawing.Size(83, 23);
            this.BottomChordButton.TabIndex = 7;
            this.BottomChordButton.Text = "&Bottom Chord";
            this.BottomChordButton.UseVisualStyleBackColor = true;
            this.BottomChordButton.Click += new System.EventHandler(this.BottomChordButton_Click);
            // 
            // TopChordButton
            // 
            this.TopChordButton.Location = new System.Drawing.Point(7, 286);
            this.TopChordButton.Name = "TopChordButton";
            this.TopChordButton.Size = new System.Drawing.Size(75, 23);
            this.TopChordButton.TabIndex = 6;
            this.TopChordButton.Text = "&Top Chord";
            this.TopChordButton.UseVisualStyleBackColor = true;
            this.TopChordButton.Click += new System.EventHandler(this.TopChordButton_Click);
            // 
            // UpdateButton
            // 
            this.UpdateButton.Location = new System.Drawing.Point(315, 287);
            this.UpdateButton.Name = "UpdateButton";
            this.UpdateButton.Size = new System.Drawing.Size(75, 23);
            this.UpdateButton.TabIndex = 5;
            this.UpdateButton.Text = "&Update";
            this.UpdateButton.UseVisualStyleBackColor = true;
            this.UpdateButton.Click += new System.EventHandler(this.UpdateButton_Click);
            // 
            // RestoreButton
            // 
            this.RestoreButton.Location = new System.Drawing.Point(234, 287);
            this.RestoreButton.Name = "RestoreButton";
            this.RestoreButton.Size = new System.Drawing.Size(75, 23);
            this.RestoreButton.TabIndex = 4;
            this.RestoreButton.Text = "&Restore";
            this.RestoreButton.UseVisualStyleBackColor = true;
            this.RestoreButton.Click += new System.EventHandler(this.RestoreButton_Click);
            // 
            // ProfileEditPictureBox
            // 
            this.ProfileEditPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ProfileEditPictureBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.ProfileEditPictureBox.Location = new System.Drawing.Point(7, 6);
            this.ProfileEditPictureBox.Name = "ProfileEditPictureBox";
            this.ProfileEditPictureBox.Size = new System.Drawing.Size(384, 274);
            this.ProfileEditPictureBox.TabIndex = 3;
            this.ProfileEditPictureBox.TabStop = false;
            this.ProfileEditPictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ProfileEditPictureBox_MouseMove);
            this.ProfileEditPictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.ProfileEditPictureBox_Paint);
            this.ProfileEditPictureBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ProfileEditPictureBox_MouseClick);
            // 
            // TrussTypeLabel
            // 
            this.TrussTypeLabel.AutoSize = true;
            this.TrussTypeLabel.Location = new System.Drawing.Point(12, 9);
            this.TrussTypeLabel.Name = "TrussTypeLabel";
            this.TrussTypeLabel.Size = new System.Drawing.Size(66, 13);
            this.TrussTypeLabel.TabIndex = 3;
            this.TrussTypeLabel.Text = "Truss Type :";
            // 
            // CloseButton
            // 
            this.CloseButton.Location = new System.Drawing.Point(337, 390);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(80, 23);
            this.CloseButton.TabIndex = 4;
            this.CloseButton.Text = "&Close";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // TrussForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CloseButton;
            this.ClientSize = new System.Drawing.Size(426, 420);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.TrussTypeLabel);
            this.Controls.Add(this.TrussGraphicsTabControl);
            this.Controls.Add(this.TrussTypeComboBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TrussForm";
            this.ShowInTaskbar = false;
            this.Text = "TrussForm";
            this.Load += new System.EventHandler(this.TrussForm_Load);
            this.TrussGraphicsTabControl.ResumeLayout(false);
            this.ViewTabPage.ResumeLayout(false);
            this.ViewTabPage.PerformLayout();
            this.TrussMembersTabPage.ResumeLayout(false);
            this.TrussMembersTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TrussMembersPictureBox)).EndInit();
            this.ProfileEditTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ProfileEditPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox TrussTypeComboBox;
        private System.Windows.Forms.TabControl TrussGraphicsTabControl;
        private System.Windows.Forms.TabPage ViewTabPage;
        private System.Windows.Forms.TabPage TrussMembersTabPage;
        private System.Windows.Forms.TabPage ProfileEditTabPage;
        private System.Windows.Forms.Button CreateButton;
        private System.Windows.Forms.PictureBox TrussMembersPictureBox;
        private System.Windows.Forms.Button UpdateButton;
        private System.Windows.Forms.Button RestoreButton;
        private System.Windows.Forms.PictureBox ProfileEditPictureBox;
        private System.Windows.Forms.Label TrussTypeLabel;
        private System.Windows.Forms.Button TopChordButton;
        private System.Windows.Forms.Button BottomChordButton;
        private System.Windows.Forms.Button CleanChordbutton;
        private System.Windows.Forms.Button ChangeBeamTypeButton;
        private System.Windows.Forms.ComboBox BeamTypeComboBox;
        private System.Windows.Forms.Label BeamTypeLabel;
        private System.Windows.Forms.ComboBox ViewComboBox;
        private System.Windows.Forms.Label ViewLabel;
        private System.Windows.Forms.Label Notelabel;
        private System.Windows.Forms.Button CloseButton;
    }
}