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
    public partial class ProjectUnitForm : Form
    {
        //Required designer variable.
        private ProjectUnitData m_dataBuffer;

        /// <summary>
        /// Initialize GUI with ProjectUnitData 
        /// </summary>
        /// <param name="dataBuffer">relevant data from revit</param>
        public ProjectUnitForm(ProjectUnitData dataBuffer)
        {
            InitializeComponent();
            m_dataBuffer = dataBuffer;
        }

        private void ProjectUnitForm_Load(object sender, EventArgs e)
        {
            try
            {
                // Initialize the combo box and list view. 
                this.disciplineCombox.BeginUpdate();
                foreach (Autodesk.Revit.DB.UnitGroup ug in Enum.GetValues(typeof(
                         Autodesk.Revit.DB.UnitGroup)))
                {
                    this.disciplineCombox.Items.AddRange(new object[] { ug });
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
                this.DecimalSymbolComboBox.SelectedItem = m_dataBuffer.DecimalSymbol;

                this.DigitGroupingAmountComboBox.BeginUpdate();
                foreach (Autodesk.Revit.DB.DigitGroupingAmount dga in Enum.GetValues(typeof(
                         Autodesk.Revit.DB.DigitGroupingAmount)))
                {
                    this.DigitGroupingAmountComboBox.Items.AddRange(new object[] { dga });
                }
                this.DigitGroupingAmountComboBox.EndUpdate();
                this.DigitGroupingAmountComboBox.SelectedItem = m_dataBuffer.DigitGroupingAmount;

                this.DigitGroupingSymbolComboBox.BeginUpdate();
                foreach (string enumName in Enum.GetNames(typeof(
                         Autodesk.Revit.DB.DigitGroupingSymbol)))
                {
                    this.DigitGroupingSymbolComboBox.Items.AddRange(new object[] { enumName });
                }
                this.DigitGroupingSymbolComboBox.EndUpdate();
                this.DigitGroupingSymbolComboBox.SelectedItem = Enum.GetName(typeof(Autodesk.Revit.DB.DigitGroupingSymbol), m_dataBuffer.DigitGroupingSymbol);

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
        /// Fill format which's discipline is the selected item of discipline combo box to the grid 
        /// </summary>
        private void FillGrid()
        {
            try
            {
                // using the static example value 1234.56789
                int count = 0;
                Dictionary<Autodesk.Revit.DB.UnitType, Autodesk.Revit.DB.FormatOptions>.Enumerator iterator = m_dataBuffer.UnitType_FormatOptions.GetEnumerator();
                while (iterator.MoveNext())
                {
                    if (Autodesk.Revit.DB.UnitUtils.GetUnitGroup(iterator.Current.Key).ToString() == this.disciplineCombox.SelectedItem.ToString())
                    {
                        this.dataGridView.Rows.Add();
                        this.dataGridView["UnitType", count].Value = iterator.Current.Key;
                        this.dataGridView["Label_UnitType", count].Value = m_dataBuffer.UnitType_Label[iterator.Current.Key];
                        this.dataGridView["FormatOptions", count].Value =
                           Autodesk.Revit.DB.UnitFormatUtils.FormatValueToString(m_dataBuffer.Units, iterator.Current.Key, 1234.56789, false, false);
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
                using (FormatForm displayForm = new FormatForm(m_dataBuffer, m_dataBuffer.UnitType_FormatOptions[(Autodesk.Revit.DB.UnitType)this.dataGridView["UnitType", e.RowIndex].Value]))
                {
                    DialogResult result;
                    while (DialogResult.Cancel != (result = displayForm.ShowDialog()))
                    {
                        if (DialogResult.OK == result)
                        {
                            try
                            {
                                this.m_dataBuffer.SetFormatOptions((Autodesk.Revit.DB.UnitType)this.dataGridView["UnitType", e.RowIndex].Value, displayForm.FormatOptions);
                                this.dataGridView["FormatOptions", e.RowIndex].Value =
                                   Autodesk.Revit.DB.UnitFormatUtils.FormatValueToString(m_dataBuffer.Units, (Autodesk.Revit.DB.UnitType)this.dataGridView["UnitType", e.RowIndex].Value, 1234.56789, false, false);
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

        private string setDecimalSymbolAndGrouping()
        {
            string format = DecimalSymbolAndGroupingtextBox.Text;

            try
            {
                m_dataBuffer.SetDecimalSymbolAndGrouping(
                   (Autodesk.Revit.DB.DecimalSymbol)DecimalSymbolComboBox.SelectedItem,
                   (Autodesk.Revit.DB.DigitGroupingSymbol)Enum.Parse(typeof(Autodesk.Revit.DB.DigitGroupingSymbol), (string)DigitGroupingSymbolComboBox.SelectedItem),
                   (Autodesk.Revit.DB.DigitGroupingAmount)DigitGroupingAmountComboBox.SelectedItem);
                format = getDecimalSymbolAndGroupingstring();
            }
            catch (System.Exception ex)
            {
                TaskDialog.Show(ex.GetType().ToString(), "Set Decimal Symbol and Grouping error : \n" + ex.Message, TaskDialogCommonButtons.Ok);
                throw;
            }

            return format;
        }

        private string getDecimalSymbolAndGroupingstring()
        {
            Autodesk.Revit.DB.FormatValueOptions formatvalueoptions = new Autodesk.Revit.DB.FormatValueOptions();
            formatvalueoptions.AppendUnitSymbol = false;

            Autodesk.Revit.DB.FormatOptions formatoptions = new Autodesk.Revit.DB.FormatOptions(Autodesk.Revit.DB.UnitSymbolType.UST_NONE, Autodesk.Revit.DB.DisplayUnitType.DUT_CURRENCY);
            formatoptions.UseDefault = false;
            formatoptions.DisplayUnits = Autodesk.Revit.DB.DisplayUnitType.DUT_CURRENCY;
            formatoptions.UnitSymbol = Autodesk.Revit.DB.UnitSymbolType.UST_NONE;
            formatoptions.Accuracy = 0.01;
            //formatoptions.SuppressLeadingZeros = true;
            formatoptions.SuppressSpaces = false;
            formatoptions.SuppressTrailingZeros = false;
            formatoptions.UseGrouping = true;
            formatoptions.UsePlusPrefix = false;

            formatvalueoptions.SetFormatOptions(formatoptions);

            return Autodesk.Revit.DB.UnitFormatUtils.FormatValueToString(this.m_dataBuffer.Units, Autodesk.Revit.DB.UnitType.UT_Number, 123456789.0, false, false, formatvalueoptions);
        }

        private void DecimalSymbolComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((Autodesk.Revit.DB.DecimalSymbol)this.DecimalSymbolComboBox.SelectedItem == Autodesk.Revit.DB.DecimalSymbol.Comma
                  && (string)this.DigitGroupingSymbolComboBox.SelectedItem == Enum.GetName(typeof(Autodesk.Revit.DB.DigitGroupingSymbol), Autodesk.Revit.DB.DigitGroupingSymbol.Comma))
            {
                this.DigitGroupingSymbolComboBox.SelectedItem = Enum.GetName(typeof(Autodesk.Revit.DB.DigitGroupingSymbol), Autodesk.Revit.DB.DigitGroupingSymbol.Dot);
            }

            if ((Autodesk.Revit.DB.DecimalSymbol)this.DecimalSymbolComboBox.SelectedItem == Autodesk.Revit.DB.DecimalSymbol.Dot
                  && (string)this.DigitGroupingSymbolComboBox.SelectedItem == Enum.GetName(typeof(Autodesk.Revit.DB.DigitGroupingSymbol), Autodesk.Revit.DB.DigitGroupingSymbol.Dot))
            {
                this.DigitGroupingSymbolComboBox.SelectedItem = Enum.GetName(typeof(Autodesk.Revit.DB.DigitGroupingSymbol), Autodesk.Revit.DB.DigitGroupingSymbol.Comma);
            }

            try
            {
                this.DecimalSymbolAndGroupingtextBox.Text = setDecimalSymbolAndGrouping();
            }
            catch
            {
                this.DecimalSymbolComboBox.SelectedItem = m_dataBuffer.DecimalSymbol;
            }
        }

        private void DigitGroupingAmountComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.DecimalSymbolAndGroupingtextBox.Text = setDecimalSymbolAndGrouping();
            }
            catch
            {
                this.DigitGroupingAmountComboBox.SelectedItem = m_dataBuffer.DigitGroupingAmount;
            }
        }

        private void DigitGroupingSymbolComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((string)this.DigitGroupingSymbolComboBox.SelectedItem == Enum.GetName(typeof(Autodesk.Revit.DB.DigitGroupingSymbol), Autodesk.Revit.DB.DigitGroupingSymbol.Comma)
                  && (Autodesk.Revit.DB.DecimalSymbol)this.DecimalSymbolComboBox.SelectedItem == Autodesk.Revit.DB.DecimalSymbol.Comma)
            {
                this.DecimalSymbolComboBox.SelectedItem = Autodesk.Revit.DB.DecimalSymbol.Dot;
            }

            if ((string)this.DigitGroupingSymbolComboBox.SelectedItem == Enum.GetName(typeof(Autodesk.Revit.DB.DigitGroupingSymbol), Autodesk.Revit.DB.DigitGroupingSymbol.Dot)
                  && (Autodesk.Revit.DB.DecimalSymbol)this.DecimalSymbolComboBox.SelectedItem == Autodesk.Revit.DB.DecimalSymbol.Dot)
            {
                this.DecimalSymbolComboBox.SelectedItem = Autodesk.Revit.DB.DecimalSymbol.Comma;
            }

            try
            {
                this.DecimalSymbolAndGroupingtextBox.Text = setDecimalSymbolAndGrouping();
            }
            catch
            {
                this.DigitGroupingSymbolComboBox.SelectedItem = Enum.GetName(typeof(Autodesk.Revit.DB.DigitGroupingSymbol), m_dataBuffer.DigitGroupingSymbol);
            }
        }
    }
}
