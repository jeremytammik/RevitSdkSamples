//
// (C) Copyright 2003-2013 by Autodesk, Inc.
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
        private ProjectUnitData m_dataBuffer;
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
        public FormatForm(ProjectUnitData dataBuffer, Autodesk.Revit.DB.FormatOptions formatoptions)
        {
            InitializeComponent();
            m_dataBuffer = dataBuffer;
            m_formatoptions = new Autodesk.Revit.DB.FormatOptions(formatoptions.UnitSymbol, formatoptions.DisplayUnits);
            m_formatoptions.UseDefault = formatoptions.UseDefault;
            m_formatoptions.Accuracy = formatoptions.Accuracy;
            m_formatoptions.SuppressTrailingZeros = formatoptions.SuppressTrailingZeros;
            m_formatoptions.SuppressLeadingZeros = formatoptions.SuppressLeadingZeros;
            m_formatoptions.UsePlusPrefix = formatoptions.UsePlusPrefix;
            m_formatoptions.UseGrouping = formatoptions.UseGrouping;
            m_formatoptions.SuppressSpaces = formatoptions.SuppressSpaces;
        }

        private void FormatForm_Load(object sender, EventArgs e)
        {
            try
            {
                // Initialize the combo box and check box. 
                this.UseDefaultcheckBox.Checked = m_formatoptions.UseDefault;

                if (this.DisplayUnitTypecomboBox.Items.Count == 0)
                {
                    this.DisplayUnitTypecomboBox.BeginUpdate();
                    this.DisplayUnitscomboBox.BeginUpdate();
                    Dictionary<Autodesk.Revit.DB.DisplayUnitType, string>.Enumerator iterator1 = m_dataBuffer.DisplayUnitType_Label.GetEnumerator();
                    while (iterator1.MoveNext())
                    {
                        this.DisplayUnitTypecomboBox.Items.AddRange(new object[] { iterator1.Current.Key });
                        this.DisplayUnitscomboBox.Items.Add(iterator1.Current.Value);
                    }
                    this.DisplayUnitTypecomboBox.SelectedItem = m_formatoptions.DisplayUnits;
                    this.DisplayUnitscomboBox.SelectedIndex = this.DisplayUnitTypecomboBox.SelectedIndex;
                    this.DisplayUnitTypecomboBox.EndUpdate();
                    this.DisplayUnitscomboBox.EndUpdate();
                }

                this.AccuracytextBox.Text = m_formatoptions.Accuracy.ToString("###########0.############");

                if (this.UnitSymbolTypecomboBox.Items.Count == 0)
                {
                    this.UnitSymbolTypecomboBox.BeginUpdate();
                    this.UnitSymbolcomboBox.BeginUpdate();
                    Dictionary<Autodesk.Revit.DB.UnitSymbolType, string>.Enumerator iterator2 = m_dataBuffer.UnitSymbolType_Label.GetEnumerator();
                    while (iterator2.MoveNext())
                    {
                        this.UnitSymbolTypecomboBox.Items.AddRange(new object[] { iterator2.Current.Key });
                        this.UnitSymbolcomboBox.Items.Add(iterator2.Current.Value);
                    }
                    this.UnitSymbolTypecomboBox.SelectedItem = m_formatoptions.UnitSymbol;
                    this.UnitSymbolcomboBox.SelectedIndex = this.UnitSymbolTypecomboBox.SelectedIndex;
                    this.UnitSymbolTypecomboBox.EndUpdate();
                    this.UnitSymbolcomboBox.EndUpdate();
                }

                this.SuppressTrailingZeroscheckBox.Checked = m_formatoptions.SuppressTrailingZeros;
                this.SuppressLeadingZeroscheckBox.Checked = m_formatoptions.SuppressLeadingZeros;
                this.UsePlusPrefixcheckBox.Checked = m_formatoptions.UsePlusPrefix;
                this.UseGroupingcheckBox.Checked = m_formatoptions.UseGrouping;
                this.SuppressSpacescheckBox.Checked = m_formatoptions.SuppressSpaces;
            }
            catch
            {
                throw;
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            try
            {
                // Save the properties of FormatOptions. 
                m_formatoptions.UseDefault = this.UseDefaultcheckBox.Checked;

                this.DisplayUnitTypecomboBox.SelectedIndex = this.DisplayUnitscomboBox.SelectedIndex;
                m_formatoptions.DisplayUnits = (Autodesk.Revit.DB.DisplayUnitType)this.DisplayUnitTypecomboBox.SelectedItem;

                m_formatoptions.Accuracy = double.Parse(this.AccuracytextBox.Text);

                this.UnitSymbolTypecomboBox.SelectedIndex = this.UnitSymbolcomboBox.SelectedIndex;
                m_formatoptions.UnitSymbol = (Autodesk.Revit.DB.UnitSymbolType)this.UnitSymbolTypecomboBox.SelectedItem;

                m_formatoptions.SuppressTrailingZeros = this.SuppressTrailingZeroscheckBox.Checked;
                m_formatoptions.SuppressLeadingZeros = this.SuppressLeadingZeroscheckBox.Checked;
                m_formatoptions.UsePlusPrefix = this.UsePlusPrefixcheckBox.Checked;
                m_formatoptions.UseGrouping = this.UseGroupingcheckBox.Checked;
                m_formatoptions.SuppressSpaces = this.SuppressSpacescheckBox.Checked;

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (System.Exception ex)
            {
                TaskDialog.Show("Invalid Input", ex.Message, TaskDialogCommonButtons.Ok);
            }
        }
    }
}
