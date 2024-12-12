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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Revit.SDK.Samples.GridCreation.CS
{
    public partial class CreateRadialAndArcGridsForm : Form
    {
        // data class object
        private CreateRadialAndArcGridsData m_data;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="data">Data class object</param>
        public CreateRadialAndArcGridsForm(CreateRadialAndArcGridsData data)
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
            labelUnitFirstRadius.Text = unit;
            labelXCoordUnit.Text = unit;
            labelYCoordUnit.Text = unit;

            // Set length values
            textBoxArcSpacing.Text = Unit.CovertFromAPI(m_data.Dut, 10).ToString();
            textBoxArcFirstRadius.Text = textBoxArcSpacing.Text;
            textBoxLineFirstDistance.Text = Unit.CovertFromAPI(m_data.Dut, 8).ToString();

            radioButton360.Checked = true;
            radioButtonCustomize.Checked = false;
            labelStartDegree.Enabled = false;
            textBoxStartDegree.Enabled = false;
            labelEndDegree.Enabled = false;
            textBoxEndDegree.Enabled = false;
            comboBoxArcBubbleLocation.SelectedIndex = 1;
            comboBoxLineBubbleLocation.SelectedIndex = 1;
        }

        private void radioButtonCustomize_CheckedChanged(object sender, EventArgs e)
        {
            bool IsCustomize = radioButtonCustomize.Checked;
            labelStartDegree.Enabled = IsCustomize;
            textBoxStartDegree.Enabled = IsCustomize;
            labelEndDegree.Enabled = IsCustomize;
            textBoxEndDegree.Enabled = IsCustomize;
        }

        private void radioButton360_MouseClick(object sender, MouseEventArgs e)
        {
            radioButtonCustomize.Checked = !radioButton360.Checked;
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

            if (radioButton360.Checked)
            {
                m_data.StartDegree = 0;
                m_data.EndDegree = 2 * Values.PI;
            }
            else
            {
                m_data.StartDegree = Convert.ToDouble(textBoxStartDegree.Text) * Values.DEGTORAD;
                m_data.EndDegree = Convert.ToDouble(textBoxEndDegree.Text) * Values.DEGTORAD;
            }

            m_data.ArcNumber = Convert.ToUInt32(textBoxArcNumber.Text);
            m_data.LineNumber = Convert.ToUInt32(textBoxLineNumber.Text);


            if (Convert.ToUInt32(textBoxArcNumber.Text) != 0)
            {
                m_data.ArcSpacing = Unit.CovertToAPI(Convert.ToDouble(textBoxArcSpacing.Text), m_data.Dut);
                m_data.ArcFirstRadius = Unit.CovertToAPI(Convert.ToDouble(textBoxArcFirstRadius.Text), m_data.Dut);
                m_data.ArcFirstBubbleLoc = (BubbleLocation)comboBoxArcBubbleLocation.SelectedIndex;
                m_data.ArcFirstLabel = textBoxArcFirstLabel.Text;
            }

            if (Convert.ToUInt32(textBoxLineNumber.Text) != 0)
            {
                m_data.LineFirstDistance = Unit.CovertToAPI(Convert.ToDouble(textBoxLineFirstDistance.Text), m_data.Dut);
                m_data.LineFirstBubbleLoc = (BubbleLocation)comboBoxLineBubbleLocation.SelectedIndex;
                m_data.LineFirstLabel = textBoxLineFirstLabel.Text;
            }
        }

        /// <summary>
        /// Check if input are validated
        /// </summary>
        /// <returns>Whether input is validated</returns>
        private bool ValidateValues()
        {
            if (!Validation.ValidateNumbers(textBoxArcNumber, textBoxLineNumber))
            {
                return false;
            }

            if (!Validation.ValidateCoord(textBoxXCoord) || !Validation.ValidateCoord(textBoxYCoord))
            {
                return false;
            }

            if (Convert.ToUInt32(textBoxArcNumber.Text) != 0)
            {
                if (!Validation.ValidateLength(textBoxArcSpacing, "Spacing", false) ||
                    !Validation.ValidateLength(textBoxArcFirstRadius, "Radius", false) ||
                    !Validation.ValidateLabel(textBoxArcFirstLabel, m_data.LabelsList))
                {
                    return false;
                }
            }

            if (Convert.ToUInt32(textBoxLineNumber.Text) != 0)
            {
                if (!Validation.ValidateLength(textBoxLineFirstDistance, "Distance", true) ||
                !Validation.ValidateLabel(textBoxLineFirstLabel, m_data.LabelsList))
                {
                    return false;
                }
            }

            if (Convert.ToUInt32(textBoxArcNumber.Text) != 0 && Convert.ToUInt32(textBoxLineNumber.Text) != 0)
            {
                if (!Validation.ValidateLabels(textBoxArcFirstLabel, textBoxLineFirstLabel))
                {
                    return false;
                }
            }

            if (radioButtonCustomize.Checked)
            {
                if (!Validation.ValidateDegrees(textBoxStartDegree, textBoxEndDegree))
                {
                    return false;
                }
            }

            return true;
        }
    }
}