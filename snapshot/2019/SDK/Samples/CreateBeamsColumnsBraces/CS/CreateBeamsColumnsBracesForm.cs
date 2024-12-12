//
// (C) Copyright 2003-2017 by Autodesk, Inc.
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

using Autodesk.Revit.UI;

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
        private System.ComponentModel.Container components = null;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.TextBox XTextBox;
        private System.Windows.Forms.TextBox DistanceTextBox;
        private System.Windows.Forms.TextBox YTextBox;
        private System.Windows.Forms.ComboBox columnComboBox;
        private System.Windows.Forms.ComboBox beamComboBox;
        private System.Windows.Forms.ComboBox braceComboBox;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.TextBox floornumberTextBox;
        private System.Windows.Forms.Label columnLabel;
        private System.Windows.Forms.Label beamLabel;
        private System.Windows.Forms.Label braceLabel;
        private System.Windows.Forms.Label DistanceLabel;
        private System.Windows.Forms.Label YLabel;
        private System.Windows.Forms.Label XLabel;
        private System.Windows.Forms.Label floornumberLabel;
        private System.Windows.Forms.Label unitLabel;

        // To store the datas
        Command m_dataBuffer = null;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="dataBuffer">the revit datas</param>
        public CreateBeamsColumnsBracesForm(Command dataBuffer)
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
            this.OKButton = new System.Windows.Forms.Button();
            this.XTextBox = new System.Windows.Forms.TextBox();
            this.YTextBox = new System.Windows.Forms.TextBox();
            this.DistanceTextBox = new System.Windows.Forms.TextBox();
            this.columnComboBox = new System.Windows.Forms.ComboBox();
            this.beamComboBox = new System.Windows.Forms.ComboBox();
            this.braceComboBox = new System.Windows.Forms.ComboBox();
            this.columnLabel = new System.Windows.Forms.Label();
            this.beamLabel = new System.Windows.Forms.Label();
            this.braceLabel = new System.Windows.Forms.Label();
            this.floornumberTextBox = new System.Windows.Forms.TextBox();
            this.DistanceLabel = new System.Windows.Forms.Label();
            this.YLabel = new System.Windows.Forms.Label();
            this.XLabel = new System.Windows.Forms.Label();
            this.floornumberLabel = new System.Windows.Forms.Label();
            this.cancelButton = new System.Windows.Forms.Button();
            this.unitLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(296, 208);
            this.OKButton.Name = "OKButton";
            this.OKButton.TabIndex = 8;
            this.OKButton.Text = "&OK";
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // XTextBox
            // 
            this.XTextBox.Location = new System.Drawing.Point(16, 96);
            this.XTextBox.Name = "XTextBox";
            this.XTextBox.Size = new System.Drawing.Size(136, 20);
            this.XTextBox.TabIndex = 2;
            this.XTextBox.Text = "";
            this.XTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.XTextBox_Validating);
            // 
            // YTextBox
            // 
            this.YTextBox.Location = new System.Drawing.Point(16, 152);
            this.YTextBox.Name = "YTextBox";
            this.YTextBox.Size = new System.Drawing.Size(136, 20);
            this.YTextBox.TabIndex = 3;
            this.YTextBox.Text = "";
            this.YTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.YTextBox_Validating);
            // 
            // DistanceTextBox
            // 
            this.DistanceTextBox.Location = new System.Drawing.Point(16, 40);
            this.DistanceTextBox.Name = "DistanceTextBox";
            this.DistanceTextBox.Size = new System.Drawing.Size(112, 20);
            this.DistanceTextBox.TabIndex = 1;
            this.DistanceTextBox.Text = "";
            this.DistanceTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.DistanceTextBox_Validating);
            // 
            // columnComboBox
            // 
            this.columnComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.columnComboBox.Location = new System.Drawing.Point(240, 40);
            this.columnComboBox.Name = "columnComboBox";
            this.columnComboBox.Size = new System.Drawing.Size(288, 21);
            this.columnComboBox.TabIndex = 5;
            // 
            // beamComboBox
            // 
            this.beamComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.beamComboBox.Location = new System.Drawing.Point(240, 96);
            this.beamComboBox.Name = "beamComboBox";
            this.beamComboBox.Size = new System.Drawing.Size(288, 21);
            this.beamComboBox.TabIndex = 6;
            // 
            // braceComboBox
            // 
            this.braceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.braceComboBox.Location = new System.Drawing.Point(240, 152);
            this.braceComboBox.Name = "braceComboBox";
            this.braceComboBox.Size = new System.Drawing.Size(288, 21);
            this.braceComboBox.TabIndex = 7;
            // 
            // columnLabel
            // 
            this.columnLabel.Location = new System.Drawing.Point(240, 16);
            this.columnLabel.Name = "columnLabel";
            this.columnLabel.Size = new System.Drawing.Size(120, 23);
            this.columnLabel.TabIndex = 10;
            this.columnLabel.Text = "Type of Columns:";
            // 
            // beamLabel
            // 
            this.beamLabel.Location = new System.Drawing.Point(240, 72);
            this.beamLabel.Name = "beamLabel";
            this.beamLabel.Size = new System.Drawing.Size(120, 23);
            this.beamLabel.TabIndex = 11;
            this.beamLabel.Text = "Type of Beams:";
            // 
            // braceLabel
            // 
            this.braceLabel.Location = new System.Drawing.Point(240, 128);
            this.braceLabel.Name = "braceLabel";
            this.braceLabel.Size = new System.Drawing.Size(120, 23);
            this.braceLabel.TabIndex = 12;
            this.braceLabel.Text = "Type of Braces:";
            // 
            // floornumberTextBox
            // 
            this.floornumberTextBox.Location = new System.Drawing.Point(16, 208);
            this.floornumberTextBox.Name = "floornumberTextBox";
            this.floornumberTextBox.Size = new System.Drawing.Size(112, 20);
            this.floornumberTextBox.TabIndex = 4;
            this.floornumberTextBox.Text = "";
            this.floornumberTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.floornumberTextBox_Validating);
            // 
            // DistanceLabel
            // 
            this.DistanceLabel.Location = new System.Drawing.Point(16, 16);
            this.DistanceLabel.Name = "DistanceLabel";
            this.DistanceLabel.Size = new System.Drawing.Size(152, 23);
            this.DistanceLabel.TabIndex = 14;
            this.DistanceLabel.Text = "Distance between Columns:";
            // 
            // YLabel
            // 
            this.YLabel.Location = new System.Drawing.Point(16, 128);
            this.YLabel.Name = "YLabel";
            this.YLabel.Size = new System.Drawing.Size(200, 23);
            this.YLabel.TabIndex = 15;
            this.YLabel.Text = "Number of Columns in the Y Direction:";
            // 
            // XLabel
            // 
            this.XLabel.Location = new System.Drawing.Point(16, 72);
            this.XLabel.Name = "XLabel";
            this.XLabel.Size = new System.Drawing.Size(200, 23);
            this.XLabel.TabIndex = 16;
            this.XLabel.Text = "Number of Columns in the X Direction:";
            // 
            // floornumberLabel
            // 
            this.floornumberLabel.Location = new System.Drawing.Point(16, 184);
            this.floornumberLabel.Name = "floornumberLabel";
            this.floornumberLabel.Size = new System.Drawing.Size(144, 23);
            this.floornumberLabel.TabIndex = 17;
            this.floornumberLabel.Text = "Number of Floors:";
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(392, 208);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.TabIndex = 9;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // unitLabel
            // 
            this.unitLabel.Location = new System.Drawing.Point(136, 42);
            this.unitLabel.Name = "unitLabel";
            this.unitLabel.Size = new System.Drawing.Size(32, 23);
            this.unitLabel.TabIndex = 18;
            this.unitLabel.Text = "feet";
            // 
            // CreateBeamsColumnsBracesForm
            // 
            this.AcceptButton = this.OKButton;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(546, 246);
            this.Controls.Add(this.unitLabel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.floornumberLabel);
            this.Controls.Add(this.XLabel);
            this.Controls.Add(this.YLabel);
            this.Controls.Add(this.DistanceLabel);
            this.Controls.Add(this.floornumberTextBox);
            this.Controls.Add(this.DistanceTextBox);
            this.Controls.Add(this.YTextBox);
            this.Controls.Add(this.XTextBox);
            this.Controls.Add(this.braceLabel);
            this.Controls.Add(this.beamLabel);
            this.Controls.Add(this.columnLabel);
            this.Controls.Add(this.braceComboBox);
            this.Controls.Add(this.beamComboBox);
            this.Controls.Add(this.columnComboBox);
            this.Controls.Add(this.OKButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CreateBeamsColumnsBracesForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Create Beams Columns and Braces";
            this.Load += new System.EventHandler(this.CreateBeamsColumnsBracesForm_Load);
            this.ResumeLayout(false);

        }
        #endregion

        /// <summary>
        /// Refresh the text box for the default data
        /// </summary>
        private void TextBoxRefresh()
        {
            this.XTextBox.Text = "2";
            this.YTextBox.Text = "2";
            this.DistanceTextBox.Text = 20.0.ToString("0.0");
            this.floornumberTextBox.Text = "1";
        }

        private void CreateBeamsColumnsBracesForm_Load(object sender, System.EventArgs e)
        {
            this.TextBoxRefresh();

            bool notLoadSymbol = false;
            if (0 == m_dataBuffer.ColumnMaps.Count)
            {
                TaskDialog.Show("Revit", "No Structural Columns family is loaded in the project, please load one firstly.");
                notLoadSymbol = true;
            }
            if (0 == m_dataBuffer.BeamMaps.Count)
            {
                TaskDialog.Show("Revit", "No Structural Framing family is loaded in the project, please load one firstly.");
                notLoadSymbol = true;
            }

            if (notLoadSymbol)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
                return;
            }

            this.columnComboBox.DataSource = m_dataBuffer.ColumnMaps;
            this.columnComboBox.DisplayMember = "SymbolName";
            this.columnComboBox.ValueMember = "ElementType";

            this.beamComboBox.DataSource = m_dataBuffer.BeamMaps;
            this.beamComboBox.DisplayMember = "SymbolName";
            this.beamComboBox.ValueMember = "ElementType";

            this.braceComboBox.DataSource = m_dataBuffer.BraceMaps;
            this.braceComboBox.DisplayMember = "SymbolName";
            this.braceComboBox.ValueMember = "ElementType";
        }

        /// <summary>
        /// accept use's input and create columns, beams and braces
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OKButton_Click(object sender, System.EventArgs e)
        {
            //check whether the input is correct and create elements
            try
            {
                int xNumber = int.Parse(this.XTextBox.Text);
                int yNumber = int.Parse(this.YTextBox.Text);
                double distance = double.Parse(this.DistanceTextBox.Text);
                object columnType = columnComboBox.SelectedValue;
                object beamType = beamComboBox.SelectedValue;
                object braceType = braceComboBox.SelectedValue;
                int floorNumber = int.Parse(floornumberTextBox.Text);

                m_dataBuffer.CreateMatrix(xNumber, yNumber, distance);
                m_dataBuffer.AddInstance(columnType, beamType, braceType, floorNumber);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception)
            {
                TaskDialog.Show("Revit", "Please input datas correctly.");
            }
        }

        /// <summary>
        /// cancel the command
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancelButton_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// Verify the distance
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DistanceTextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            double distance = 0.1;
            try
            {
                distance = double.Parse(this.DistanceTextBox.Text);
            }
            catch (Exception)
            {
                TaskDialog.Show("Revit", "Please enter a value larger than 5 and less than 30000.");
                this.DistanceTextBox.Text = "";
                this.DistanceTextBox.Focus();
                return;
            }

            if (distance <= 5)
            {
                TaskDialog.Show("Revit", "Please enter a value larger than 5.");
                this.DistanceTextBox.Text = "";
                this.DistanceTextBox.Focus();
                return;
            }

            if (distance > 30000)
            {
                TaskDialog.Show("Revit", "Please enter a value less than 30000.");
                this.DistanceTextBox.Text = "";
                this.DistanceTextBox.Focus();
                return;
            }
        }

        /// <summary>
        /// Verify the number of X direction
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void XTextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            int xNumber = 1;
            try
            {
                xNumber = int.Parse(this.XTextBox.Text);
            }
            catch (Exception)
            {
                TaskDialog.Show("Revit", "Please input an integer for X direction between 1 to 20.");
                this.XTextBox.Text = "";
            }
            if (xNumber < 1 || xNumber > 20)
            {
                TaskDialog.Show("Revit", "Please input an integer for X direction between 1 to 20.");
                this.XTextBox.Text = "";
            }
        }

        /// <summary>
        /// Verify the number of Y direction
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void YTextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            int yNumber = 1;
            try
            {
                yNumber = int.Parse(this.YTextBox.Text);
            }
            catch (Exception)
            {
                TaskDialog.Show("Revit", "Please input an integer for Y direction between 1 to 20.");
                this.YTextBox.Text = "";
            }
            if (yNumber < 1 || yNumber > 20)
            {
                TaskDialog.Show("Revit", "Please input an integer for Y direction between 1 to 20.");
                this.YTextBox.Text = "";
            }
        }

        /// <summary>
        /// Verify the number of floors
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void floornumberTextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            int floorNumber = 1;
            try
            {
                floorNumber = int.Parse(floornumberTextBox.Text);
            }
            catch (Exception)
            {
                TaskDialog.Show("Revit", "Please input an integer for the number of floors between 1 to 10.");
                this.floornumberTextBox.Text = "";
            }
            if (floorNumber < 1 || floorNumber > 10)
            {
                TaskDialog.Show("Revit", "Please input an integer for the number of floors between 1 to 10.");
                this.floornumberTextBox.Text = "";
            }
        }
    }
}
