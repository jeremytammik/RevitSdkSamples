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


using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Revit.SDK.Samples.CreateBeamsColumnsBraces.CS
{
   /// <summary>
   /// UI
   /// </summary>
   public class CreateBeamsColumnsBracesForm : System.Windows.Forms.Form
   {
      /// <summary>
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.Container? components = null;
      private System.Windows.Forms.Button? OKButton;
      private System.Windows.Forms.TextBox? XTextBox;
      private System.Windows.Forms.TextBox? DistanceTextBox;
      private System.Windows.Forms.TextBox? YTextBox;
      private System.Windows.Forms.ComboBox? columnComboBox;
      private System.Windows.Forms.ComboBox? beamComboBox;
      private System.Windows.Forms.ComboBox? braceComboBox;
      private System.Windows.Forms.Button? cancelButton;
      private System.Windows.Forms.TextBox? floornumberTextBox;
      private System.Windows.Forms.Label? columnLabel;
      private System.Windows.Forms.Label? beamLabel;
      private System.Windows.Forms.Label? braceLabel;
      private System.Windows.Forms.Label? DistanceLabel;
      private System.Windows.Forms.Label? YLabel;
      private System.Windows.Forms.Label? XLabel;
      private System.Windows.Forms.Label? floornumberLabel;
      private System.Windows.Forms.Label? unitLabel;

      // To store the datas
      CreateBeamsColumnsBraces? m_dataBuffer = null;

      /// <summary>
      /// constructor
      /// </summary>
      /// <param name="dataBuffer">the revit datas</param>
      public CreateBeamsColumnsBracesForm(CreateBeamsColumnsBraces? dataBuffer)
      {
         //
         // Required for Windows Form Designer support
         //
         InitializeComponent();

         m_dataBuffer = dataBuffer;
      }

      /// <summary>
      /// Clean up any resources being used.
      /// </summary>
      protected override void Dispose(bool disposing)
      {
         if (disposing)
         {
            if (components != null)
            {
               components.Dispose();
            }
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
         OKButton = new System.Windows.Forms.Button();
         XTextBox = new System.Windows.Forms.TextBox();
         YTextBox = new System.Windows.Forms.TextBox();
         DistanceTextBox = new System.Windows.Forms.TextBox();
         columnComboBox = new System.Windows.Forms.ComboBox();
         beamComboBox = new System.Windows.Forms.ComboBox();
         braceComboBox = new System.Windows.Forms.ComboBox();
         columnLabel = new System.Windows.Forms.Label();
         beamLabel = new System.Windows.Forms.Label();
         braceLabel = new System.Windows.Forms.Label();
         floornumberTextBox = new System.Windows.Forms.TextBox();
         DistanceLabel = new System.Windows.Forms.Label();
         YLabel = new System.Windows.Forms.Label();
         XLabel = new System.Windows.Forms.Label();
         floornumberLabel = new System.Windows.Forms.Label();
         cancelButton = new System.Windows.Forms.Button();
         unitLabel = new System.Windows.Forms.Label();
         SuspendLayout();
         // 
         // OKButton
         // 
         OKButton.Location = new System.Drawing.Point(296, 208);
         OKButton.Name = "OKButton";
         OKButton.Size = new System.Drawing.Size(75, 23);
         OKButton.TabIndex = 8;
         OKButton.Text = "&OK";
         OKButton.Click += new System.EventHandler(OKButton_Click);
         // 
         // XTextBox
         // 
         XTextBox.Location = new System.Drawing.Point(16, 96);
         XTextBox.Name = "XTextBox";
         XTextBox.Size = new System.Drawing.Size(136, 20);
         XTextBox.TabIndex = 2;
         XTextBox.Validating += new System.ComponentModel.CancelEventHandler(XTextBox_Validating);
         // 
         // YTextBox
         // 
         YTextBox.Location = new System.Drawing.Point(16, 152);
         YTextBox.Name = "YTextBox";
         YTextBox.Size = new System.Drawing.Size(136, 20);
         YTextBox.TabIndex = 3;
         YTextBox.Validating += new System.ComponentModel.CancelEventHandler(YTextBox_Validating);
         // 
         // DistanceTextBox
         // 
         DistanceTextBox.Location = new System.Drawing.Point(16, 40);
         DistanceTextBox.Name = "DistanceTextBox";
         DistanceTextBox.Size = new System.Drawing.Size(112, 20);
         DistanceTextBox.TabIndex = 1;
         DistanceTextBox.Validating += new System.ComponentModel.CancelEventHandler(DistanceTextBox_Validating);
         // 
         // columnComboBox
         // 
         columnComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         columnComboBox.Location = new System.Drawing.Point(240, 40);
         columnComboBox.Name = "columnComboBox";
         columnComboBox.Size = new System.Drawing.Size(288, 21);
         columnComboBox.TabIndex = 5;
         // 
         // beamComboBox
         // 
         beamComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         beamComboBox.Location = new System.Drawing.Point(240, 96);
         beamComboBox.Name = "beamComboBox";
         beamComboBox.Size = new System.Drawing.Size(288, 21);
         beamComboBox.TabIndex = 6;
         // 
         // braceComboBox
         // 
         braceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         braceComboBox.Location = new System.Drawing.Point(240, 152);
         braceComboBox.Name = "braceComboBox";
         braceComboBox.Size = new System.Drawing.Size(288, 21);
         braceComboBox.TabIndex = 7;
         // 
         // columnLabel
         // 
         columnLabel.Location = new System.Drawing.Point(240, 16);
         columnLabel.Name = "columnLabel";
         columnLabel.Size = new System.Drawing.Size(120, 23);
         columnLabel.TabIndex = 10;
         columnLabel.Text = "Type of Columns:";
         // 
         // beamLabel
         // 
         beamLabel.Location = new System.Drawing.Point(240, 72);
         beamLabel.Name = "beamLabel";
         beamLabel.Size = new System.Drawing.Size(120, 23);
         beamLabel.TabIndex = 11;
         beamLabel.Text = "Type of Beams:";
         // 
         // braceLabel
         // 
         braceLabel.Location = new System.Drawing.Point(240, 128);
         braceLabel.Name = "braceLabel";
         braceLabel.Size = new System.Drawing.Size(120, 23);
         braceLabel.TabIndex = 12;
         braceLabel.Text = "Type of Braces:";
         // 
         // floornumberTextBox
         // 
         floornumberTextBox.Location = new System.Drawing.Point(16, 208);
         floornumberTextBox.Name = "floornumberTextBox";
         floornumberTextBox.Size = new System.Drawing.Size(112, 20);
         floornumberTextBox.TabIndex = 4;
         floornumberTextBox.Validating += new System.ComponentModel.CancelEventHandler(floornumberTextBox_Validating);
         // 
         // DistanceLabel
         // 
         DistanceLabel.Location = new System.Drawing.Point(16, 16);
         DistanceLabel.Name = "DistanceLabel";
         DistanceLabel.Size = new System.Drawing.Size(152, 23);
         DistanceLabel.TabIndex = 14;
         DistanceLabel.Text = "Distance between Columns:";
         // 
         // YLabel
         // 
         YLabel.Location = new System.Drawing.Point(16, 128);
         YLabel.Name = "YLabel";
         YLabel.Size = new System.Drawing.Size(200, 23);
         YLabel.TabIndex = 15;
         YLabel.Text = "Number of Columns in the Y Direction:";
         // 
         // XLabel
         // 
         XLabel.Location = new System.Drawing.Point(16, 72);
         XLabel.Name = "XLabel";
         XLabel.Size = new System.Drawing.Size(200, 23);
         XLabel.TabIndex = 16;
         XLabel.Text = "Number of Columns in the X Direction:";
         // 
         // floornumberLabel
         // 
         floornumberLabel.Location = new System.Drawing.Point(16, 184);
         floornumberLabel.Name = "floornumberLabel";
         floornumberLabel.Size = new System.Drawing.Size(144, 23);
         floornumberLabel.TabIndex = 17;
         floornumberLabel.Text = "Number of Floors:";
         // 
         // cancelButton
         // 
         cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         cancelButton.Location = new System.Drawing.Point(392, 208);
         cancelButton.Name = "cancelButton";
         cancelButton.Size = new System.Drawing.Size(75, 23);
         cancelButton.TabIndex = 9;
         cancelButton.Text = "&Cancel";
         cancelButton.Click += new System.EventHandler(cancelButton_Click);
         // 
         // unitLabel
         // 
         unitLabel.Location = new System.Drawing.Point(136, 42);
         unitLabel.Name = "unitLabel";
         unitLabel.Size = new System.Drawing.Size(32, 23);
         unitLabel.TabIndex = 18;
         unitLabel.Text = "feet";
         // 
         // CreateBeamsColumnsBracesForm
         // 
         AcceptButton = OKButton;
         AutoScaleBaseSize = new System.Drawing.Size(5, 13);
         CancelButton = cancelButton;
         ClientSize = new System.Drawing.Size(546, 246);
         Controls.Add(unitLabel);
         Controls.Add(cancelButton);
         Controls.Add(floornumberLabel);
         Controls.Add(XLabel);
         Controls.Add(YLabel);
         Controls.Add(DistanceLabel);
         Controls.Add(floornumberTextBox);
         Controls.Add(DistanceTextBox);
         Controls.Add(YTextBox);
         Controls.Add(XTextBox);
         Controls.Add(braceLabel);
         Controls.Add(beamLabel);
         Controls.Add(columnLabel);
         Controls.Add(braceComboBox);
         Controls.Add(beamComboBox);
         Controls.Add(columnComboBox);
         Controls.Add(OKButton);
         FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
         MaximizeBox = false;
         MinimizeBox = false;
         Name = "CreateBeamsColumnsBracesForm";
         ShowInTaskbar = false;
         StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
         Text = "Create Beams Columns and Braces";
         Load += new System.EventHandler(CreateBeamsColumnsBracesForm_Load);
         ResumeLayout(false);
         PerformLayout();

      }
      #endregion

      /// <summary>
      /// Refresh the text box for the default datas
      /// </summary>
      private void TextBoxRefresh()
      {
         if (XTextBox != null && YTextBox != null && DistanceTextBox != null && floornumberTextBox != null)
         {
            XTextBox.Text = "2";
            YTextBox.Text = "2";
            DistanceTextBox.Text = "20.0";
            floornumberTextBox.Text = "1";
         }

      }

      private void CreateBeamsColumnsBracesForm_Load(object? sender, System.EventArgs e)
      {
         TextBoxRefresh();
         if (columnComboBox == null || beamComboBox == null || braceComboBox == null)
            return;
         bool notLoadSymbol = false;
         if (0 == m_dataBuffer?.ColumnMaps.Count)
         {
            MessageBox.Show("No Structural Columns family is loaded in the project, please load one firstly.", "Revit");
            notLoadSymbol = true;
         }
         if (0 == m_dataBuffer?.BeamMaps.Count)
         {
            MessageBox.Show("No Structural Framing family is loaded in the project, please load one firstly.", "Revit");
            notLoadSymbol = true;
         }

         if (notLoadSymbol)
         {
            DialogResult = DialogResult.Cancel;
            Close();
            return;
         }

         columnComboBox.DataSource = m_dataBuffer?.ColumnMaps;
         columnComboBox.DisplayMember = "SymbolName";
         columnComboBox.ValueMember = "ElementType";

         beamComboBox.DataSource = m_dataBuffer?.BeamMaps;
         beamComboBox.DisplayMember = "SymbolName";
         beamComboBox.ValueMember = "ElementType";

         braceComboBox.DataSource = m_dataBuffer?.BraceMaps;
         braceComboBox.DisplayMember = "SymbolName";
         braceComboBox.ValueMember = "ElementType";
      }

      /// <summary>
      /// accept use's inpurt and create columns, beams and braces
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void OKButton_Click(object? sender, System.EventArgs e)
      {
         //check whether the input is correct and create elements
         try
         {
            if (XTextBox == null || YTextBox == null || DistanceTextBox == null || columnComboBox == null || beamComboBox == null || braceComboBox == null || floornumberTextBox == null)
               return;
            int xNumber = int.Parse(XTextBox.Text);
            int yNumber = int.Parse(YTextBox.Text);
            double distance = double.Parse(DistanceTextBox.Text);
            object? columnType = columnComboBox.SelectedValue;
            object? beamType = beamComboBox.SelectedValue;
            object? braceType = braceComboBox.SelectedValue;
            int floorNumber = int.Parse(floornumberTextBox.Text);
            if (columnType != null && beamType != null && braceType != null)
            {
               m_dataBuffer?.CreateMatrix(xNumber, yNumber, distance);
               m_dataBuffer?.AddInstance(columnType, beamType, braceType, floorNumber);
            }
            DialogResult = DialogResult.OK;
            Close();
         }
         catch (Exception)
         {
            MessageBox.Show("Please input datas correctly.", "Revit");
         }
      }

      /// <summary>
      /// cancel the command
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void cancelButton_Click(object? sender, System.EventArgs? e)
      {
         DialogResult = DialogResult.Cancel;
         Close();
      }

      /// <summary>
      /// Verify the distance
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void DistanceTextBox_Validating(object? sender, System.ComponentModel.CancelEventArgs e)
      {
         if (DistanceTextBox == null)
            return;
         double distance = 0.1;
         try
         {
            distance = double.Parse(DistanceTextBox.Text);
         }
         catch (Exception)
         {
            MessageBox.Show("Please enter a value larger than 5 and less than 30000.", "Revit");
            DistanceTextBox.Text = "";
            DistanceTextBox.Focus();
            return;
         }

         if (distance <= 5)
         {
            MessageBox.Show("Please enter a value larger than 5.", "Revit");
            DistanceTextBox.Text = "";
            DistanceTextBox.Focus();
            return;
         }

         if (distance > 30000)
         {
            MessageBox.Show("Please enter a value less than 30000.", "Revit");
            DistanceTextBox.Text = "";
            DistanceTextBox.Focus();
            return;
         }
      }

      /// <summary>
      /// Verify the number of X direction
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void XTextBox_Validating(object? sender, System.ComponentModel.CancelEventArgs e)
      {
         if (XTextBox == null)
            return;
         int xNumber = 1;
         try
         {
            xNumber = int.Parse(XTextBox.Text);
         }
         catch (Exception)
         {
            MessageBox.Show("Please input an integer for X direction between 1 to 20.", "Revit");
            XTextBox.Text = "";
         }
         if (xNumber < 1 || xNumber > 20)
         {
            MessageBox.Show("Please input an integer for X direction between 1 to 20.", "Revit");
            XTextBox.Text = "";
         }
      }

      /// <summary>
      /// Verify the number of Y direction
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void YTextBox_Validating(object? sender, System.ComponentModel.CancelEventArgs e)
      {
         if (YTextBox == null)
            return;
         int yNumber = 1;
         try
         {
            yNumber = int.Parse(YTextBox.Text);
         }
         catch (Exception)
         {
            MessageBox.Show("Please input an integer for Y direction between 1 to 20.", "Revit");
            YTextBox.Text = "";
         }
         if (yNumber < 1 || yNumber > 20)
         {
            MessageBox.Show("Please input an integer for Y direction between 1 to 20.", "Revit");
            YTextBox.Text = "";
         }
      }

      /// <summary>
      /// Verify the number of floors
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void floornumberTextBox_Validating(object? sender, System.ComponentModel.CancelEventArgs e)
      {
         if (floornumberTextBox == null)
            return;
         int floorNumber = 1;
         try
         {
            floorNumber = int.Parse(floornumberTextBox.Text);
         }
         catch (Exception)
         {
            MessageBox.Show("Please input an integer for the number of floors between 1 to 10.", "Revit");
            floornumberTextBox.Text = "";
         }
         if (floorNumber < 1 || floorNumber > 10)
         {
            MessageBox.Show("Please input an integer for the number of floors between 1 to 10.", "Revit");
            floornumberTextBox.Text = "";
         }
      }
   }
}
