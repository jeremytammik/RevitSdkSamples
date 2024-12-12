//
// (C) Copyright 2003-2019 by Autodesk, Inc.
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
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.Units.CS
{
    /// <summary>
    /// 
    /// </summary>
    public partial class FormatForm : Form
    {
        //Required designer variable.
        private Autodesk.Revit.DB.UnitType m_unittype;
        private Autodesk.Revit.DB.FormatOptions m_formatoptions;

        /// <summary>
        /// format options. 
        /// </summary>
        public Autodesk.Revit.DB.FormatOptions FormatOptions
        {
            get
            {
                return this.m_formatoptions;
            }
        }

        /// <summary>
        /// Initialize GUI with FormatData 
        /// </summary>
        ///// <param name="dataBuffer">relevant data from revit</param>
        public FormatForm(Autodesk.Revit.DB.UnitType unittype, Autodesk.Revit.DB.FormatOptions formatoptions)
        {
            InitializeComponent();
            m_unittype = unittype;
            m_formatoptions = new Autodesk.Revit.DB.FormatOptions(formatoptions.DisplayUnits, formatoptions.UnitSymbol);
            m_formatoptions.UseDefault = formatoptions.UseDefault;
            m_formatoptions.Accuracy = formatoptions.Accuracy;
            m_formatoptions.SuppressTrailingZeros = formatoptions.SuppressTrailingZeros;
            m_formatoptions.SuppressLeadingZeros = formatoptions.SuppressLeadingZeros;
            m_formatoptions.UsePlusPrefix = formatoptions.UsePlusPrefix;
            m_formatoptions.UseDigitGrouping = formatoptions.UseDigitGrouping;
            m_formatoptions.SuppressSpaces = formatoptions.SuppressSpaces;
        }

        private void FormatForm_Load(object sender, EventArgs e)
        {
            try
            {
                bool isUseDefault = m_formatoptions.UseDefault;
                if (isUseDefault)
                {
                   m_formatoptions.UseDefault = false;
                }
                
                FormatForm_Clear();

                // Initialize the combo box and check box. 
                this.DisplayUnitTypecomboBox.BeginUpdate();
                this.DisplayUnitcomboBox.BeginUpdate();
                foreach (Autodesk.Revit.DB.DisplayUnitType displayUnitType in Autodesk.Revit.DB.UnitUtils.GetValidDisplayUnits(m_unittype))
                {
                   this.DisplayUnitTypecomboBox.Items.AddRange(new object[] { displayUnitType });
                   this.DisplayUnitcomboBox.Items.Add(Autodesk.Revit.DB.LabelUtils.GetLabelFor(displayUnitType));
                }
                this.DisplayUnitTypecomboBox.SelectedItem = m_formatoptions.DisplayUnits;
                this.DisplayUnitcomboBox.SelectedIndex = this.DisplayUnitTypecomboBox.SelectedIndex;
                this.DisplayUnitTypecomboBox.EndUpdate();
                this.DisplayUnitcomboBox.EndUpdate();

                this.AccuracytextBox.Text = m_formatoptions.Accuracy.ToString("###########0.############");

                this.SuppressTrailingZeroscheckBox.Checked = m_formatoptions.SuppressTrailingZeros;
                this.SuppressLeadingZeroscheckBox.Checked = m_formatoptions.SuppressLeadingZeros;
                this.UsePlusPrefixcheckBox.Checked = m_formatoptions.UsePlusPrefix;
                this.UseDigitGroupingcheckBox.Checked = m_formatoptions.UseDigitGrouping;
                this.SuppressSpacescheckBox.Checked = m_formatoptions.SuppressSpaces;

                m_formatoptions.UseDefault = isUseDefault;
                this.UseDefaultcheckBox.Checked = m_formatoptions.UseDefault;

                if (!Autodesk.Revit.DB.Units.IsModifiableUnitType(m_unittype))
                {
                   this.Text = "This unit type is unmodifiable";
                   this.UseDefaultcheckBox.Checked = true;
                   this.UseDefaultcheckBox.Enabled = false;
                   this.buttonOK.Enabled = false;
                }
            }
            catch
            {
                throw;
            }
        }

        private void FormatForm_Clear()
        {
            // Clear the combo box and check box. 
            this.UseDefaultcheckBox.Checked = false;
            
            this.DisplayUnitTypecomboBox.Items.Clear();
            this.DisplayUnitcomboBox.Items.Clear();
           
            this.AccuracytextBox.Text = "";
            
            this.UnitSymbolTypecomboBox.Items.Clear();
            this.UnitSymbolcomboBox.Items.Clear();
        
            this.SuppressTrailingZeroscheckBox.Checked = false;
            this.SuppressLeadingZeroscheckBox.Checked = false;
            this.UsePlusPrefixcheckBox.Checked = false;
            this.UseDigitGroupingcheckBox.Checked = false;
            this.SuppressSpacescheckBox.Checked = false;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            try
            {
                // Save the properties of FormatOptions. 
                m_formatoptions.UseDefault = this.UseDefaultcheckBox.Checked;

                if (!m_formatoptions.UseDefault)
                {
                   this.DisplayUnitTypecomboBox.SelectedIndex = this.DisplayUnitcomboBox.SelectedIndex;
                   m_formatoptions.DisplayUnits = (Autodesk.Revit.DB.DisplayUnitType)this.DisplayUnitTypecomboBox.SelectedItem;

                   m_formatoptions.Accuracy = double.Parse(this.AccuracytextBox.Text);

                   this.UnitSymbolTypecomboBox.SelectedIndex = this.UnitSymbolcomboBox.SelectedIndex;
                   m_formatoptions.UnitSymbol = (Autodesk.Revit.DB.UnitSymbolType)this.UnitSymbolTypecomboBox.SelectedItem;

                   m_formatoptions.SuppressTrailingZeros = this.SuppressTrailingZeroscheckBox.Checked;
                   m_formatoptions.SuppressLeadingZeros = this.SuppressLeadingZeroscheckBox.Checked;
                   m_formatoptions.UsePlusPrefix = this.UsePlusPrefixcheckBox.Checked;
                   m_formatoptions.UseDigitGrouping = this.UseDigitGroupingcheckBox.Checked;
                   m_formatoptions.SuppressSpaces = this.SuppressSpacescheckBox.Checked;
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (System.Exception ex)
            {
                TaskDialog.Show("Invalid Input", ex.Message, TaskDialogCommonButtons.Ok);
            }
        }

        private void DisplayUnitcomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.DisplayUnitTypecomboBox.SelectedIndex = this.DisplayUnitcomboBox.SelectedIndex;

            this.UnitSymbolTypecomboBox.Items.Clear();
            this.UnitSymbolcomboBox.Items.Clear();
            
            this.UnitSymbolTypecomboBox.BeginUpdate();
            this.UnitSymbolcomboBox.BeginUpdate();
            foreach (Autodesk.Revit.DB.UnitSymbolType unitSymbolType in Autodesk.Revit.DB.FormatOptions.GetValidUnitSymbols((Autodesk.Revit.DB.DisplayUnitType)this.DisplayUnitTypecomboBox.SelectedItem))
            {
                this.UnitSymbolTypecomboBox.Items.AddRange(new object[] { unitSymbolType });
                if (unitSymbolType == Autodesk.Revit.DB.UnitSymbolType.UST_NONE)
                {
                   this.UnitSymbolcomboBox.Items.Add("");
                }
                else
                {
                   this.UnitSymbolcomboBox.Items.Add(Autodesk.Revit.DB.LabelUtils.GetLabelFor(unitSymbolType));
                }
            }
            this.UnitSymbolTypecomboBox.SelectedItem = m_formatoptions.UnitSymbol;
            if (this.UnitSymbolTypecomboBox.SelectedIndex < 0)
            {
               this.UnitSymbolTypecomboBox.SelectedIndex = 0;
               m_formatoptions.UnitSymbol = (Autodesk.Revit.DB.UnitSymbolType)this.UnitSymbolTypecomboBox.SelectedItem;
            }
            this.UnitSymbolcomboBox.SelectedIndex = this.UnitSymbolTypecomboBox.SelectedIndex;
            this.UnitSymbolTypecomboBox.EndUpdate();
            this.UnitSymbolcomboBox.EndUpdate();
        }

        private void UseDefaultcheckBox_CheckedChanged(object sender, EventArgs e)
        {
           m_formatoptions.UseDefault = this.UseDefaultcheckBox.Checked; 
           if (this.UseDefaultcheckBox.Checked)
           {
              this.DisplayUnitcomboBox.Enabled = false;
              this.AccuracytextBox.Enabled = false;
              this.UnitSymbolcomboBox.Enabled = false;
              this.SuppressTrailingZeroscheckBox.Enabled = false;
              this.SuppressLeadingZeroscheckBox.Enabled = false;
              this.UsePlusPrefixcheckBox.Enabled = false;
              this.UseDigitGroupingcheckBox.Enabled = false;
              this.SuppressSpacescheckBox.Enabled = false;
           }
           else
           {
              this.DisplayUnitcomboBox.Enabled = true;
              this.AccuracytextBox.Enabled = true;
              this.UnitSymbolcomboBox.Enabled = true;
              this.SuppressTrailingZeroscheckBox.Enabled = true;
              this.SuppressLeadingZeroscheckBox.Enabled = true;
              this.UsePlusPrefixcheckBox.Enabled = true;
              this.UseDigitGroupingcheckBox.Enabled = true;
              this.SuppressSpacescheckBox.Enabled = true;
           }
        }
    }
}
