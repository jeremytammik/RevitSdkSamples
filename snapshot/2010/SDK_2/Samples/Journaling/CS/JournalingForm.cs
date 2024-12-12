//
// (C) Copyright 2003-2009 by Autodesk, Inc.
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
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Autodesk.Revit.Geometry;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Symbols;

namespace Revit.SDK.Samples.Journaling.CS
{

    /// <summary>
    /// The form used to collect the support data for wall creation and store in journal 
    /// </summary>
    public partial class JournalingForm : System.Windows.Forms.Form
    {
        // Private members
        const double Precision = 0.00001;   //precision when judge whether two doubles are equal
        Journaling m_dataBuffer;    // A reference of Journaling.


        // Methods
        /// <summary>
        /// Constructor of JournalingForm
        /// </summary>
        /// <param name="dataBuffer">A reference of Journaling class</param>
        public JournalingForm(Journaling dataBuffer)
        {
            // Required for Windows Form Designer support
            InitializeComponent();

            //Get a reference of ModelLines
            m_dataBuffer = dataBuffer;

            // Bind the data source of the typeComboBox and levelComboBox
            typeComboBox.DataSource = m_dataBuffer.WallTypes;
            typeComboBox.DisplayMember = "Name";
            levelComboBox.DataSource = m_dataBuffer.Levels;
            levelComboBox.DisplayMember = "Name";
        }


        /// <summary>
        /// The okButton click event method,
        /// this method collect the data, and pass them to the journaling class
        /// </summary>
        private void okButton_Click(object sender, EventArgs e)
        {
            // Get the support data from the UI controls
            XYZ startPoint = startPointUserControl.GetPointData();  // start point 
            XYZ endPoint = endPointUserControl.GetPointData();      // end point
            if (startPoint.Equals(endPoint))    // Don't allow start point equals end point
            {
                MessageBox.Show("Start point should not equal end point.", "Revit");
                return;
            }

            double diff = Math.Abs(startPoint.Z - endPoint.Z);
            if (diff > Precision)
            {
                MessageBox.Show("Z coordinate of start and end points should be equal.", "Revit");
                return;
            }

            Level level = levelComboBox.SelectedItem as Level;  // level information
            if (null == level)  // assert it in't null
            {
                MessageBox.Show("The selected level is null or incorrect.", "Revit");
                return;
            }

            WallType type = typeComboBox.SelectedItem as WallType;  // wall type
            if(null == type)    // assert it isn't null
            {
                MessageBox.Show("The selected wall type is null or incorrect.", "Revit");
                return;
            }

            // Invoke SetNecessaryData method to set the collected support data 
            m_dataBuffer.SetNecessaryData(startPoint, endPoint, level, type);

            // Set result information and close the form
            this.DialogResult = DialogResult.OK;
            this.Close();
        }


        /// <summary>
        /// The cancelButton click event method
        /// </summary>
        private void cancelButton_Click(object sender, EventArgs e)
        {
            // Only set result to be cancel and close the form
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}