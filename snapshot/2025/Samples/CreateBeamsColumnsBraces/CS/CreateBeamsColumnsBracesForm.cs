//
// (C) Copyright 2003-2023 by Autodesk, Inc.
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
using System.Windows.Forms;

using TaskDialog = Autodesk.Revit.UI.TaskDialog;

namespace Revit.SDK.Samples.CreateBeamsColumnsBraces.CS
{
    /// <summary>
    /// UI
    /// </summary>
    public partial class CreateBeamsColumnsBracesForm : System.Windows.Forms.Form
    {
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
