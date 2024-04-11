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
using TaskDialog = Autodesk.Revit.UI.TaskDialog;

namespace Revit.SDK.Samples.Units.CS
{
    /// <summary>
    /// 
    /// </summary>
    public partial class UnitsForm : Form
    {
        //Required designer variable.
       private Autodesk.Revit.DB.Units m_units;

        /// <summary>
        /// Initialize GUI with ProjectUnitData 
        /// </summary>
        /// <param name="units">units in current document</param>
        public UnitsForm(Autodesk.Revit.DB.Units units)
        {
            InitializeComponent();
            m_units = units;
        }

        private void UnitsForm_Load(object sender, EventArgs e)
        {
            try
            {
                // Initialize the combo box and list view. 
                this.disciplineCombox.BeginUpdate();
                foreach (Autodesk.Revit.DB.ForgeTypeId disciplineTypeId in Autodesk.Revit.DB.UnitUtils.GetAllDisciplines())
                {
                    this.disciplineCombox.Items.AddRange(new object[] { Autodesk.Revit.DB.LabelUtils.GetLabelForDiscipline(disciplineTypeId) });
                }
                this.disciplineCombox.SelectedItem = disciplineCombox.Items[0];
                this.disciplineCombox.EndUpdate();

                this.DecimalSymbolComboBox.BeginUpdate();
                foreach (Autodesk.Revit.DB.DecimalSymbol ds in Enum.GetValues(typeof(
                         Autodesk.Revit.DB.DecimalSymbol)))
                {
                    this.DecimalSymbolComboBox.Items.AddRange(new object[] { ds });
                }
                this.DecimalSymbolComboBox.EndUpdate();
                this.DecimalSymbolComboBox.SelectedItem = m_units.DecimalSymbol;

                this.DigitGroupingAmountComboBox.BeginUpdate();
                foreach (Autodesk.Revit.DB.DigitGroupingAmount dga in Enum.GetValues(typeof(
                         Autodesk.Revit.DB.DigitGroupingAmount)))
                {
                    this.DigitGroupingAmountComboBox.Items.AddRange(new object[] { dga });
                }
                this.DigitGroupingAmountComboBox.EndUpdate();
                this.DigitGroupingAmountComboBox.SelectedItem = m_units.DigitGroupingAmount;

                this.DigitGroupingSymbolComboBox.BeginUpdate();
                foreach (string enumName in Enum.GetNames(typeof(
                         Autodesk.Revit.DB.DigitGroupingSymbol)))
                {
                    this.DigitGroupingSymbolComboBox.Items.AddRange(new object[] { enumName });
                }
                this.DigitGroupingSymbolComboBox.EndUpdate();
                this.DigitGroupingSymbolComboBox.SelectedItem = Enum.GetName(typeof(Autodesk.Revit.DB.DigitGroupingSymbol), m_units.DigitGroupingSymbol);

                this.DecimalSymbolAndGroupingtextBox.Text = getDecimalSymbolAndGroupingstring();

                this.DigitGroupingSymbolComboBox.SelectedIndexChanged += new System.EventHandler(this.DigitGroupingSymbolComboBox_SelectedIndexChanged);
                this.DigitGroupingAmountComboBox.SelectedIndexChanged += new System.EventHandler(this.DigitGroupingAmountComboBox_SelectedIndexChanged);
                this.DecimalSymbolComboBox.SelectedIndexChanged += new System.EventHandler(this.DecimalSymbolComboBox_SelectedIndexChanged);
            }
            catch
            {
                throw;
            }
        }

        private void disciplineCombox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.dataGridView.Rows.Clear();
            FillGrid();
        }

        /// <summary>
        /// Fill the grid with selected discipline 
        /// </summary>
        private void FillGrid()
        {
            try
            {
                // using the static example value 1234.56789
                int count = 0;
                foreach (Autodesk.Revit.DB.ForgeTypeId specTypeId in Autodesk.Revit.DB.UnitUtils.GetAllMeasurableSpecs())
                {
                    if (Autodesk.Revit.DB.LabelUtils.GetLabelForDiscipline(Autodesk.Revit.DB.UnitUtils.GetDiscipline(specTypeId)) == this.disciplineCombox.SelectedItem.ToString())
                    {
                        this.dataGridView.Rows.Add();
                        this.dataGridView["UnitType", count].Value = specTypeId;
                        this.dataGridView["Label_UnitType", count].Value = Autodesk.Revit.DB.LabelUtils.GetLabelForSpec(specTypeId);
                        this.dataGridView["FormatOptions", count].Value =
                           Autodesk.Revit.DB.UnitFormatUtils.Format(m_units, specTypeId, 1234.56789, false);
                        count++;
                    }
                }
            }
            catch
            {
                throw;
            }

        }

        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                // show UI
                Autodesk.Revit.DB.ForgeTypeId specTypeId = (Autodesk.Revit.DB.ForgeTypeId)this.dataGridView["UnitType", e.RowIndex].Value;
                using (FormatForm displayForm = new FormatForm(specTypeId, m_units.GetFormatOptions(specTypeId)))
                {
                    DialogResult result;
                    while (DialogResult.Cancel != (result = displayForm.ShowDialog()))
                    {
                        if (DialogResult.OK == result)
                        {
                            try
                            {
                                this.m_units.SetFormatOptions((Autodesk.Revit.DB.ForgeTypeId)this.dataGridView["UnitType", e.RowIndex].Value, displayForm.FormatOptions);
                                this.dataGridView["FormatOptions", e.RowIndex].Value =
                                   Autodesk.Revit.DB.UnitFormatUtils.Format(m_units, (Autodesk.Revit.DB.ForgeTypeId)this.dataGridView["UnitType", e.RowIndex].Value, 1234.56789, false);
                                break;
                            }
                            catch (System.Exception ex)
                            {
                                TaskDialog.Show(ex.GetType().ToString(), "Set FormatOptions error : \n" + ex.Message, TaskDialogCommonButtons.Ok);
                            }
                        }
                    }
                }
            }
        }

        private string getDecimalSymbolAndGroupingstring()
        {
            Autodesk.Revit.DB.FormatValueOptions formatvalueoptions = new Autodesk.Revit.DB.FormatValueOptions();
            formatvalueoptions.AppendUnitSymbol = false;

            Autodesk.Revit.DB.FormatOptions formatoptions = new Autodesk.Revit.DB.FormatOptions(Autodesk.Revit.DB.UnitTypeId.Currency, new Autodesk.Revit.DB.ForgeTypeId());
            formatoptions.UseDefault = false;
            formatoptions.SetUnitTypeId(Autodesk.Revit.DB.UnitTypeId.Currency);
            formatoptions.SetSymbolTypeId(new Autodesk.Revit.DB.ForgeTypeId());
            formatoptions.Accuracy = 0.01;
            //formatoptions.SuppressLeadingZeros = true;
            formatoptions.SuppressSpaces = false;
            formatoptions.SuppressTrailingZeros = false;
            formatoptions.UseDigitGrouping = true;
            formatoptions.UsePlusPrefix = false;

            formatvalueoptions.SetFormatOptions(formatoptions);

            return Autodesk.Revit.DB.UnitFormatUtils.Format(m_units, Autodesk.Revit.DB.SpecTypeId.Number, 123456789.0, false, formatvalueoptions);
            
        }

        private void DecimalSymbolComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                m_units.DecimalSymbol = (Autodesk.Revit.DB.DecimalSymbol)DecimalSymbolComboBox.SelectedItem;
                this.DecimalSymbolAndGroupingtextBox.Text = getDecimalSymbolAndGroupingstring();
            }
            catch
            {
                this.DecimalSymbolComboBox.SelectedItem = m_units.DecimalSymbol;
            }
        }

        private void DigitGroupingAmountComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                m_units.DigitGroupingAmount = (Autodesk.Revit.DB.DigitGroupingAmount)DigitGroupingAmountComboBox.SelectedItem;
                this.DecimalSymbolAndGroupingtextBox.Text = getDecimalSymbolAndGroupingstring();
            }
            catch
            {
                this.DigitGroupingAmountComboBox.SelectedItem = m_units.DigitGroupingAmount;
            }
        }

        private void DigitGroupingSymbolComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                m_units.DigitGroupingSymbol = (Autodesk.Revit.DB.DigitGroupingSymbol)Enum.Parse(typeof(Autodesk.Revit.DB.DigitGroupingSymbol), (string)DigitGroupingSymbolComboBox.SelectedItem);
                this.DecimalSymbolAndGroupingtextBox.Text = getDecimalSymbolAndGroupingstring();
            }
            catch
            {
                this.DigitGroupingSymbolComboBox.SelectedItem = Enum.GetName(typeof(Autodesk.Revit.DB.DigitGroupingSymbol), m_units.DigitGroupingSymbol);
            }
        }
    }
}
