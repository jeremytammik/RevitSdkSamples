//
// (C) Copyright 2003-2016 by Autodesk, Inc. 
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using Autodesk.Revit.DB;

namespace Revit.SDK.Samples.ModelLines.CS
{
    /// <summary>
    /// Stand for the point data of this user control
    /// </summary>
    public partial class PointUserControl : UserControl
    {

        #region Constructors

        /// <summary>
        /// Default constructor of ModelLinesForm
        /// </summary>
        public PointUserControl()
        {
            // Required for Windows Form Designer support
            InitializeComponent();

            // initialize the TextBox data
            xCoordinateTextBox.Text = "0";
            yCoordinateTextBox.Text = "0";
            zCoordinateTextBox.Text = "0";
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get the point data of this user control
        /// </summary>
        /// <returns>the point data stored in this control</returns>
        public Autodesk.Revit.DB.XYZ GetPointData()
        {
            double x = 0;   // Store the temporary x coordinate
            double y = 0;   // Store the temporary y coordinate
            double z = 0;   // Store the temporary z coordinate 
            x = Convert.ToDouble(xCoordinateTextBox.Text);  // Get x coordinate
            y = Convert.ToDouble(yCoordinateTextBox.Text);  // Get x coordinate
            z = Convert.ToDouble(zCoordinateTextBox.Text);  // Get x coordinate
            return new Autodesk.Revit.DB.XYZ(x, y, z);
        }


        /// <summary>
        /// Check the point data which the user input are integrated or not
        /// </summary>
        /// <returns>If the data are integrated return true, otherwise false</returns>
        public bool AssertPointIntegrity()
        {
            if (String.IsNullOrEmpty(xCoordinateTextBox.Text)       // x coordinate empty
                || String.IsNullOrEmpty(yCoordinateTextBox.Text)    // y coordinate empty
                || String.IsNullOrEmpty(zCoordinateTextBox.Text))   // z coordinate empty
            {
                return false;
            }

            // If all coordinates are not empty, return true
            return true;
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// This method is called to Validate whether the TextBox data is a number.
        /// </summary>
        /// <param name="sender">the event sender(can be all TextBox)</param>
        /// <param name="e">contain event data(not used)</param>
        void CoordinateTextBox_Validating(object sender, CancelEventArgs e)
        {
            // Check whether the sender is a TextBox reference
            TextBox numberTextBox = sender as TextBox;
            if (null == numberTextBox)
            {
                // If it is not a TextBox, just return
                return;
            }

            // Invoke IsNumber() method to judge whether the input data are right
            if (!IsNumber(numberTextBox.Text))
            {
                // If not, give error information, and set the text to be empty
                Autodesk.Revit.UI.TaskDialog.Show("Revit", "Please input a double data.");
                //numberTextBox.Text = "";
            }
        }

        /// <summary>
        /// Check whether the string data can represent a double number
        /// </summary>
        /// <param name="number">The test string</param>
        /// <returns>If the string can represent a number return true, otherwise false</returns>
        bool IsNumber(String number)
        {
            // First check whether the string is empty
            if (String.IsNullOrEmpty(number))
            {
                // If the string is empty, return true
                return true;
            }

            // Use Convert.ToDouble() method to changed string to double,
            // If an exception is thrown out, that means the string can't change 
            try
            {
                // Invoke Convert.ToDouble() method
                Convert.ToDouble(number);
            }
            catch (Exception)
            {
                return false;
            }

            // If everything goes well, return true
            return true;
        }

        #endregion
    }
}
