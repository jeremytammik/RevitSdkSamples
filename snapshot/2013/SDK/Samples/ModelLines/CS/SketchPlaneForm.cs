//
// (C) Copyright 2003-2012 by Autodesk, Inc. 
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

using Autodesk.Revit.DB;

namespace Revit.SDK.Samples.ModelLines.CS
{
    /// <summary>
    /// This UserControl is used to collect the information for sketch plane creation
    /// </summary>
    public partial class SketchPlaneForm : System.Windows.Forms.Form
    {
        // Private members
        ModelLines m_dataBuffer;   // A reference of ModelLines.

        /// <summary>
        /// Constructor of SketchPlaneForm
        /// </summary>
        /// <param name="dataBuffer">a reference of ModelLines class</param>
        public SketchPlaneForm(ModelLines dataBuffer)
        {
            // Required for Windows Form Designer support
            InitializeComponent();

            //Get a reference of ModelLines
            m_dataBuffer = dataBuffer;
        }

        /// <summary>
        /// Check the data which the user input are integrated or not 
        /// </summary>
        /// <returns>If the data are integrated return true, otherwise false</returns>
        bool AssertDataIntegrity()
        {
            return (normalUserControl.AssertPointIntegrity()
            && originUserControl.AssertPointIntegrity());
        }

        /// <summary>
        /// The event method for okButton click
        /// </summary>
        private void okButton_Click(object sender, EventArgs e)
        {
            // First, check data integrity 
            if (!AssertDataIntegrity())
            {
                MessageBox.Show("Please make the data integrated first.", "Revit");
                return;
            }

            try
            {
                // Get the necessary information and invoke the method to create sketch plane
                Autodesk.Revit.DB.XYZ normal = normalUserControl.GetPointData();
                Autodesk.Revit.DB.XYZ origin = originUserControl.GetPointData();
                m_dataBuffer.CreateSketchPlane(normal, origin);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Revit");
                return;
            }

            // If the creation is successful, close this form
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// The event method for cancelButton click
        /// </summary>
        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
