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

namespace Revit.SDK.Samples.GridCreation.CS
{
    /// <summary>
    /// The dialog which provides the options of creating grids with selected lines/arcs
    /// </summary>
    public partial class CreateWithSelectedCurvesForm : System.Windows.Forms.Form
    {
        // data class object
        private CreateWithSelectedCurvesData m_data;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="data">Data class object</param>
        public CreateWithSelectedCurvesForm(CreateWithSelectedCurvesData data)
        {
            m_data = data;

            InitializeComponent();
            // Set state of controls
            InitializeControls(); 
        }

        /// <summary>
        /// Set state of controls
        /// </summary>
        private void InitializeControls()
        {
            comboBoxBubbleLocation.SelectedIndex = 1;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            // Check if input are validated
            if (ValidateValues())
            {
                // Transfer data back into data class
                SetData();
            }
            else
            {
                this.DialogResult = DialogResult.None;
            }
        }

        /// <summary>
        /// Check if input are validated
        /// </summary>
        /// <returns>Whether input is validated</returns>        
        private bool ValidateValues()
        {
            return Validation.ValidateLabel(textBoxFirstLabel, m_data.LabelsList);
        }

        /// <summary>
        /// Transfer data back into data class
        /// </summary>
        private void SetData()
        {
            m_data.BubbleLocation = (BubbleLocation)comboBoxBubbleLocation.SelectedIndex;
            m_data.FirstLabel = textBoxFirstLabel.Text;
            m_data.DeleteSelectedElements = checkBoxDeleteElements.Checked;
        }
    }
}