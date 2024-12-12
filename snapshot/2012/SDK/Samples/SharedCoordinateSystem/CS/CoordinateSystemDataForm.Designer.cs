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

namespace Revit.SDK.Samples.SharedCoordinateSystem.CS
{
    /// <summary>
    /// coordinate system data form
    /// </summary>
   partial class CoordinateSystemDataForm
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
         this.coordinateSystemTabControl = new System.Windows.Forms.TabControl();
         this.locationTabPage = new System.Windows.Forms.TabPage();
         this.introduceLabel = new System.Windows.Forms.Label();
         this.label4 = new System.Windows.Forms.Label();
         this.label3 = new System.Windows.Forms.Label();
         this.label2 = new System.Windows.Forms.Label();
         this.label1 = new System.Windows.Forms.Label();
         this.eatWestTextBox = new System.Windows.Forms.TextBox();
         this.northSouthTextBox = new System.Windows.Forms.TextBox();
         this.elevationTextBox = new System.Windows.Forms.TextBox();
         this.angleTextBox = new System.Windows.Forms.TextBox();
         this.makeCurrentButton = new System.Windows.Forms.Button();
         this.duplicateButton = new System.Windows.Forms.Button();
         this.listlabel = new System.Windows.Forms.Label();
         this.locationListBox = new System.Windows.Forms.ListBox();
         this.placeTabPage = new System.Windows.Forms.TabPage();
         this.timeZoneComboBox = new System.Windows.Forms.ComboBox();
         this.longitudeTextBox = new System.Windows.Forms.TextBox();
         this.latitudeTextBox = new System.Windows.Forms.TextBox();
         this.cityNameComboBox = new System.Windows.Forms.ComboBox();
         this.timeZoneLabel = new System.Windows.Forms.Label();
         this.longitudeLabel = new System.Windows.Forms.Label();
         this.latitudeLabel = new System.Windows.Forms.Label();
         this.cityNameLabel = new System.Windows.Forms.Label();
         this.siteIntroduceLabel = new System.Windows.Forms.Label();
         this.okButton = new System.Windows.Forms.Button();
         this.cancelButton = new System.Windows.Forms.Button();
         this.coordinateSystemTabControl.SuspendLayout();
         this.locationTabPage.SuspendLayout();
         this.placeTabPage.SuspendLayout();
         this.SuspendLayout();
         // 
         // coordinateSystemTabControl
         // 
         this.coordinateSystemTabControl.Controls.Add(this.locationTabPage);
         this.coordinateSystemTabControl.Controls.Add(this.placeTabPage);
         this.coordinateSystemTabControl.Location = new System.Drawing.Point(5, 12);
         this.coordinateSystemTabControl.Name = "coordinateSystemTabControl";
         this.coordinateSystemTabControl.SelectedIndex = 0;
         this.coordinateSystemTabControl.Size = new System.Drawing.Size(458, 307);
         this.coordinateSystemTabControl.TabIndex = 0;
         // 
         // locationTabPage
         // 
         this.locationTabPage.Controls.Add(this.introduceLabel);
         this.locationTabPage.Controls.Add(this.label4);
         this.locationTabPage.Controls.Add(this.label3);
         this.locationTabPage.Controls.Add(this.label2);
         this.locationTabPage.Controls.Add(this.label1);
         this.locationTabPage.Controls.Add(this.eatWestTextBox);
         this.locationTabPage.Controls.Add(this.northSouthTextBox);
         this.locationTabPage.Controls.Add(this.elevationTextBox);
         this.locationTabPage.Controls.Add(this.angleTextBox);
         this.locationTabPage.Controls.Add(this.makeCurrentButton);
         this.locationTabPage.Controls.Add(this.duplicateButton);
         this.locationTabPage.Controls.Add(this.listlabel);
         this.locationTabPage.Controls.Add(this.locationListBox);
         this.locationTabPage.Location = new System.Drawing.Point(4, 22);
         this.locationTabPage.Name = "locationTabPage";
         this.locationTabPage.Padding = new System.Windows.Forms.Padding(3);
         this.locationTabPage.Size = new System.Drawing.Size(450, 281);
         this.locationTabPage.TabIndex = 0;
         this.locationTabPage.Text = "Locations";
         this.locationTabPage.UseVisualStyleBackColor = true;
         // 
         // introduceLabel
         // 
         this.introduceLabel.AutoSize = true;
         this.introduceLabel.Location = new System.Drawing.Point(7, 14);
         this.introduceLabel.Name = "introduceLabel";
         this.introduceLabel.Size = new System.Drawing.Size(387, 26);
         this.introduceLabel.TabIndex = 1;
         this.introduceLabel.Text = "Used for orientation and position of the project on the site and in relation to o" +
             "ther \r\nbuildings. There may be many Shared Locations defined in one project.\r\n";
         // 
         // label4
         // 
         this.label4.AutoSize = true;
         this.label4.Location = new System.Drawing.Point(205, 256);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(158, 13);
         this.label4.TabIndex = 11;
         this.label4.Text = "Elevation Above Ground Level :";
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Location = new System.Drawing.Point(205, 230);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(120, 13);
         this.label3.TabIndex = 10;
         this.label3.Text = "Angle From True North :";
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(7, 256);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(113, 13);
         this.label2.TabIndex = 7;
         this.label2.Text = "North to South Offset :";
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(7, 230);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(105, 13);
         this.label1.TabIndex = 6;
         this.label1.Text = "East to West Offset :";
         // 
         // eatWestTextBox
         // 
         this.eatWestTextBox.Location = new System.Drawing.Point(126, 227);
         this.eatWestTextBox.Name = "eatWestTextBox";
         this.eatWestTextBox.Size = new System.Drawing.Size(72, 20);
         this.eatWestTextBox.TabIndex = 8;
         // 
         // northSouthTextBox
         // 
         this.northSouthTextBox.Location = new System.Drawing.Point(126, 253);
         this.northSouthTextBox.Name = "northSouthTextBox";
         this.northSouthTextBox.Size = new System.Drawing.Size(72, 20);
         this.northSouthTextBox.TabIndex = 9;
         // 
         // elevationTextBox
         // 
         this.elevationTextBox.Location = new System.Drawing.Point(369, 253);
         this.elevationTextBox.Name = "elevationTextBox";
         this.elevationTextBox.Size = new System.Drawing.Size(72, 20);
         this.elevationTextBox.TabIndex = 13;
         // 
         // angleTextBox
         // 
         this.angleTextBox.Location = new System.Drawing.Point(369, 227);
         this.angleTextBox.Name = "angleTextBox";
         this.angleTextBox.Size = new System.Drawing.Size(72, 20);
         this.angleTextBox.TabIndex = 12;
         this.angleTextBox.Leave += new System.EventHandler(this.angleTextBox_Leave);
         // 
         // makeCurrentButton
         // 
         this.makeCurrentButton.Location = new System.Drawing.Point(313, 118);
         this.makeCurrentButton.Name = "makeCurrentButton";
         this.makeCurrentButton.Size = new System.Drawing.Size(128, 23);
         this.makeCurrentButton.TabIndex = 5;
         this.makeCurrentButton.Text = "&Make Current";
         this.makeCurrentButton.UseVisualStyleBackColor = true;
         this.makeCurrentButton.Click += new System.EventHandler(this.makeCurrentButton_Click);
         // 
         // duplicateButton
         // 
         this.duplicateButton.Location = new System.Drawing.Point(313, 89);
         this.duplicateButton.Name = "duplicateButton";
         this.duplicateButton.Size = new System.Drawing.Size(128, 23);
         this.duplicateButton.TabIndex = 4;
         this.duplicateButton.Text = "&Duplicate...";
         this.duplicateButton.UseVisualStyleBackColor = true;
         this.duplicateButton.Click += new System.EventHandler(this.duplicateButton_Click);
         // 
         // listlabel
         // 
         this.listlabel.AutoSize = true;
         this.listlabel.Location = new System.Drawing.Point(6, 69);
         this.listlabel.Name = "listlabel";
         this.listlabel.Size = new System.Drawing.Size(168, 13);
         this.listlabel.TabIndex = 2;
         this.listlabel.Text = "Locations definded in this project :";
         // 
         // locationListBox
         // 
         this.locationListBox.FormattingEnabled = true;
         this.locationListBox.Location = new System.Drawing.Point(9, 89);
         this.locationListBox.Name = "locationListBox";
         this.locationListBox.Size = new System.Drawing.Size(298, 121);
         this.locationListBox.Sorted = true;
         this.locationListBox.TabIndex = 3;
         this.locationListBox.SelectedIndexChanged += new System.EventHandler(this.locationListBox_SelectedIndexChanged);
         // 
         // placeTabPage
         // 
         this.placeTabPage.Controls.Add(this.timeZoneComboBox);
         this.placeTabPage.Controls.Add(this.longitudeTextBox);
         this.placeTabPage.Controls.Add(this.latitudeTextBox);
         this.placeTabPage.Controls.Add(this.cityNameComboBox);
         this.placeTabPage.Controls.Add(this.timeZoneLabel);
         this.placeTabPage.Controls.Add(this.longitudeLabel);
         this.placeTabPage.Controls.Add(this.latitudeLabel);
         this.placeTabPage.Controls.Add(this.cityNameLabel);
         this.placeTabPage.Controls.Add(this.siteIntroduceLabel);
         this.placeTabPage.Location = new System.Drawing.Point(4, 22);
         this.placeTabPage.Name = "placeTabPage";
         this.placeTabPage.Padding = new System.Windows.Forms.Padding(3);
         this.placeTabPage.Size = new System.Drawing.Size(450, 281);
         this.placeTabPage.TabIndex = 1;
         this.placeTabPage.Text = "Place";
         this.placeTabPage.UseVisualStyleBackColor = true;
         // 
         // timeZoneComboBox
         // 
         this.timeZoneComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.timeZoneComboBox.FormattingEnabled = true;
         this.timeZoneComboBox.Location = new System.Drawing.Point(136, 166);
         this.timeZoneComboBox.Name = "timeZoneComboBox";
         this.timeZoneComboBox.Size = new System.Drawing.Size(200, 21);
         this.timeZoneComboBox.TabIndex = 8;
         this.timeZoneComboBox.SelectedValueChanged += new System.EventHandler(this.timeZoneComboBox_SelectedValueChanged);
         // 
         // longitudeTextBox
         // 
         this.longitudeTextBox.Location = new System.Drawing.Point(136, 138);
         this.longitudeTextBox.Name = "longitudeTextBox";
         this.longitudeTextBox.Size = new System.Drawing.Size(200, 20);
         this.longitudeTextBox.TabIndex = 7;
         this.longitudeTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
         this.longitudeTextBox.Leave += new System.EventHandler(this.longitudeTextBox_Leave);
         this.longitudeTextBox.TextChanged += new System.EventHandler(this.longitudeTextBox_TextChanged);
         // 
         // latitudeTextBox
         // 
         this.latitudeTextBox.Location = new System.Drawing.Point(136, 111);
         this.latitudeTextBox.Name = "latitudeTextBox";
         this.latitudeTextBox.Size = new System.Drawing.Size(200, 20);
         this.latitudeTextBox.TabIndex = 6;
         this.latitudeTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
         this.latitudeTextBox.Leave += new System.EventHandler(this.latitudeTextBox_Leave);
         this.latitudeTextBox.TextChanged += new System.EventHandler(this.latitudeTextBox_TextChanged);        
         // 
         // cityNameComboBox
         // 
         this.cityNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.cityNameComboBox.FormattingEnabled = true;
         this.cityNameComboBox.Location = new System.Drawing.Point(136, 83);
         this.cityNameComboBox.Name = "cityNameComboBox";
         this.cityNameComboBox.Size = new System.Drawing.Size(200, 21);
         this.cityNameComboBox.TabIndex = 5;
         this.cityNameComboBox.SelectedValueChanged += new System.EventHandler(this.cityNameComboBox_SelectedValueChanged);
         // 
         // timeZoneLabel
         // 
         this.timeZoneLabel.AutoSize = true;
         this.timeZoneLabel.Location = new System.Drawing.Point(15, 169);
         this.timeZoneLabel.Name = "timeZoneLabel";
         this.timeZoneLabel.Size = new System.Drawing.Size(64, 13);
         this.timeZoneLabel.TabIndex = 4;
         this.timeZoneLabel.Text = "Time Zone :";
         // 
         // longitudeLabel
         // 
         this.longitudeLabel.AutoSize = true;
         this.longitudeLabel.Location = new System.Drawing.Point(15, 141);
         this.longitudeLabel.Name = "longitudeLabel";
         this.longitudeLabel.Size = new System.Drawing.Size(60, 13);
         this.longitudeLabel.TabIndex = 3;
         this.longitudeLabel.Text = "Longitude :";
         // 
         // latitudeLabel
         // 
         this.latitudeLabel.AutoSize = true;
         this.latitudeLabel.Location = new System.Drawing.Point(15, 114);
         this.latitudeLabel.Name = "latitudeLabel";
         this.latitudeLabel.Size = new System.Drawing.Size(51, 13);
         this.latitudeLabel.TabIndex = 2;
         this.latitudeLabel.Text = "Latitude :";
         // 
         // cityNameLabel
         // 
         this.cityNameLabel.AutoSize = true;
         this.cityNameLabel.Location = new System.Drawing.Point(15, 86);
         this.cityNameLabel.Name = "cityNameLabel";
         this.cityNameLabel.Size = new System.Drawing.Size(30, 13);
         this.cityNameLabel.TabIndex = 1;
         this.cityNameLabel.Text = "City :";
         // 
         // siteIntroduceLabel
         // 
         this.siteIntroduceLabel.AutoSize = true;
         this.siteIntroduceLabel.Location = new System.Drawing.Point(15, 22);
         this.siteIntroduceLabel.Name = "siteIntroduceLabel";
         this.siteIntroduceLabel.Size = new System.Drawing.Size(410, 26);
         this.siteIntroduceLabel.TabIndex = 0;
         this.siteIntroduceLabel.Text = "There is a single Place for each Revit project that defines where the project is " +
             "placed \r\nin the world.";
         // 
         // okButton
         // 
         this.okButton.Location = new System.Drawing.Point(277, 325);
         this.okButton.Name = "okButton";
         this.okButton.Size = new System.Drawing.Size(90, 25);
         this.okButton.TabIndex = 14;
         this.okButton.Text = "&OK";
         this.okButton.UseVisualStyleBackColor = true;
         this.okButton.Click += new System.EventHandler(this.okButton_Click);
         // 
         // cancelButton
         // 
         this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         this.cancelButton.Location = new System.Drawing.Point(373, 325);
         this.cancelButton.Name = "cancelButton";
         this.cancelButton.Size = new System.Drawing.Size(90, 25);
         this.cancelButton.TabIndex = 15;
         this.cancelButton.Text = "&Cancel";
         this.cancelButton.UseVisualStyleBackColor = true;
         // 
         // CoordinateSystemDataForm
         // 
         this.AcceptButton = this.okButton;
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.CancelButton = this.cancelButton;
         this.ClientSize = new System.Drawing.Size(470, 361);
         this.Controls.Add(this.cancelButton);
         this.Controls.Add(this.okButton);
         this.Controls.Add(this.coordinateSystemTabControl);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "CoordinateSystemDataForm";
         this.ShowInTaskbar = false;
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
         this.Text = "Manage Locations and Place";
         this.Load += new System.EventHandler(this.CoordinateSystemDataForm_Load);
         this.coordinateSystemTabControl.ResumeLayout(false);
         this.locationTabPage.ResumeLayout(false);
         this.locationTabPage.PerformLayout();
         this.placeTabPage.ResumeLayout(false);
         this.placeTabPage.PerformLayout();
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.TabControl coordinateSystemTabControl;
      private System.Windows.Forms.TabPage locationTabPage;
      private System.Windows.Forms.TabPage placeTabPage;
      private System.Windows.Forms.Button okButton;
      private System.Windows.Forms.Button cancelButton;
      private System.Windows.Forms.ListBox locationListBox;
      private System.Windows.Forms.Label listlabel;
      private System.Windows.Forms.Button makeCurrentButton;
      private System.Windows.Forms.Button duplicateButton;
      private System.Windows.Forms.TextBox eatWestTextBox;
      private System.Windows.Forms.TextBox northSouthTextBox;
      private System.Windows.Forms.TextBox elevationTextBox;
      private System.Windows.Forms.TextBox angleTextBox;
      private System.Windows.Forms.Label introduceLabel;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.Label timeZoneLabel;
      private System.Windows.Forms.Label longitudeLabel;
      private System.Windows.Forms.Label latitudeLabel;
      private System.Windows.Forms.Label cityNameLabel;
      private System.Windows.Forms.Label siteIntroduceLabel;
      private System.Windows.Forms.ComboBox timeZoneComboBox;
      private System.Windows.Forms.TextBox longitudeTextBox;
      private System.Windows.Forms.TextBox latitudeTextBox;
      private System.Windows.Forms.ComboBox cityNameComboBox;
   }
}
