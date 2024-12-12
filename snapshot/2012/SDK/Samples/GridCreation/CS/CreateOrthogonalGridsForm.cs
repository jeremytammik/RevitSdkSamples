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
    ///  The dialog which provides the options of creating orthogonal grids
    /// </summary>
    public partial class CreateOrthogonalGridsForm : System.Windows.Forms.Form
    {
        // data class object
        private CreateOrthogonalGridsData m_data;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="data">Data class object</param>
        public CreateOrthogonalGridsForm(CreateOrthogonalGridsData data)
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
            // Set length unit related labels
            String unit = Properties.Resources.ResourceManager.GetString(m_data.Dut.ToString());
            labelUnitX.Text = unit;
            labelUnitY.Text = unit;
            labelXCoordUnit.Text = unit;
            labelYCoordUnit.Text = unit;


            // Set spacing values
            textBoxXSpacing.Text = Unit.CovertFromAPI(m_data.Dut, 10).ToString();
            textBoxYSpacing.Text = textBoxXSpacing.Text;

            // Set bubble locations to end point
            comboBoxXBubbleLocation.SelectedIndex = 1;
            comboBoxYBubbleLocation.SelectedIndex = 1;
        }

        private void buttonCreate_Click(object sender, EventArgs e)
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
        /// Transfer data back into data class
        /// </summary>
        private void SetData()
        {
            m_data.XOrigin = Unit.CovertToAPI(Convert.ToDouble(textBoxXCoord.Text), m_data.Dut);
            m_data.YOrigin = Unit.CovertToAPI(Convert.ToDouble(textBoxYCoord.Text), m_data.Dut);            
            m_data.XNumber = Convert.ToUInt32(textBoxXNumber.Text);
            m_data.YNumber = Convert.ToUInt32(textBoxYNumber.Text);

            if (Convert.ToUInt32(textBoxXNumber.Text) != 0)
            {
                m_data.XSpacing = Unit.CovertToAPI(Convert.ToDouble(textBoxXSpacing.Text), m_data.Dut);
                m_data.XBubbleLoc = (BubbleLocation)comboBoxXBubbleLocation.SelectedIndex;
                m_data.XFirstLabel = textBoxXFirstLabel.Text;
            }

            if (Convert.ToUInt32(textBoxYNumber.Text) != 0)
            {
                m_data.YSpacing = Unit.CovertToAPI(Convert.ToDouble(textBoxYSpacing.Text), m_data.Dut);
                m_data.YBubbleLoc = (BubbleLocation)comboBoxYBubbleLocation.SelectedIndex;
                m_data.YFirstLabel = textBoxYFirstLabel.Text;
            }  
        }

        /// <summary>
        /// Check if input are validated
        /// </summary>
        /// <returns>Whether input is validated</returns>        
        private bool ValidateValues()
        {
            if (!Validation.ValidateNumbers(textBoxXNumber, textBoxYNumber))
            {
                return false;
            }

            if (!Validation.ValidateCoord(textBoxXCoord) || !Validation.ValidateCoord(textBoxYCoord))
            {
                return false;
            }

            if (Convert.ToUInt32(textBoxXNumber.Text) != 0)
            {
                if (!Validation.ValidateLength(textBoxXSpacing, "Spacing", false) ||
                    !Validation.ValidateLabel(textBoxXFirstLabel, m_data.LabelsList))
                {
                    return false;
                }
            }

            if (Convert.ToUInt32(textBoxYNumber.Text) != 0)
            {
                if (!Validation.ValidateLength(textBoxYSpacing, "Spacing", false) ||
                    !Validation.ValidateLabel(textBoxYFirstLabel, m_data.LabelsList))
                {
                    return false;
                }
            }
           
            if (Convert.ToUInt32(textBoxXNumber.Text) != 0 && Convert.ToUInt32(textBoxYNumber.Text) != 0)
            {
                if (!Validation.ValidateLabels(textBoxXFirstLabel, textBoxYFirstLabel))
                {
                    return false;
                }
            }

            return true;
        }
    }
}