//
// (C) Copyright 2003-2007 by Autodesk, Inc.
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

namespace Revit.SDK.Samples.ProjectUnit.CS
{
    /// <summary>
    /// retrieve or change the project unit setting by API. 
    /// include the decimal symbol type, the slope type setting and some other format information.
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
            InitializeCom();           
        }

        /// <summary>
        /// Initialize the combo box and list view. 
        /// </summary>
        private void InitializeCom()
        {
            try
            {
                this.disciplineCombox.DropDownStyle = ComboBoxStyle.DropDownList;
                this.decimalComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                this.slopeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                
                this.disciplineCombox.BeginUpdate();       
                foreach (Autodesk.Revit.FormatOptions format in m_dataBuffer.FormatOptionList)
                {
                    if (!this.disciplineCombox.Items.Contains(format.Discipline))
                    {
                        this.disciplineCombox.Items.AddRange(new object[] { format.Discipline });
                    }
                }

                this.disciplineCombox.EndUpdate();

                if (this.disciplineCombox.Items.Count > 0)
                {
                    this.disciplineCombox.SelectedIndex = 0;
                }

                foreach (Autodesk.Revit.Enums.DecimalSymbolType de in Enum.GetValues(typeof(
                         Autodesk.Revit.Enums.DecimalSymbolType)))
                {
                    this.decimalComboBox.Items.AddRange(new object[] { de });
                }

                this.decimalComboBox.SelectedItem = m_dataBuffer.DecimalSyType;

                foreach (Autodesk.Revit.Enums.RiseRunOrAngleType slo in Enum.GetValues(typeof(
                         Autodesk.Revit.Enums.RiseRunOrAngleType)))
                {
                    this.slopeComboBox.Items.AddRange(new object[] { slo });
                }

                this.slopeComboBox.SelectedItem = m_dataBuffer.SlopeType;    

            }
            catch
            {
                throw;
            }            
        }

        /// <summary>
        /// Function handle the event that the selected index of combo box changed. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void disciplineCombox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.listView.Items.Clear();
            foreach (Autodesk.Revit.FormatOptions format in m_dataBuffer.FormatOptionList)              
            {
                if (format.Discipline.ToString() == this.disciplineCombox.SelectedItem.ToString())
                {
                    AddListItem(format);
                }
            }
        }

        /// <summary>
        /// Add format which's discipline is the selected item of discipline combo box to the list 
        /// </summary>
        /// <param name="format">the format to add</param>
        private void AddListItem(Autodesk.Revit.FormatOptions format)
        {
            try
            {
                ListViewItem listitem = new ListViewItem();
                listitem.SubItems.AddRange(new String[] { format.Name, 
                                                          format.Units.ToString(), 
                                                          format.Unitsuffix.ToString(), 
                                                          format.Rounding.ToString() });
                this.listView.Items.Add(listitem);
            }
            catch (Exception ex)
            {
                ListViewItem listitem = new ListViewItem();
                if (ex.TargetSite.Name == "get_Unitsuffix")
                {
                    try
                    {
                        listitem.SubItems.AddRange(new String[] { format.Name, 
                                                                  format.Units.ToString(), 
                                                                  "", 
                                                                  format.Rounding.ToString() });

                    }
                    catch (System.Exception /*exx*/)
                    {
                        listitem.SubItems.AddRange(new String[] { format.Name, 
                                                                  format.Units.ToString(), 
                                                                  "", 
                                                                  "" });
                    }
                }
                else
                {
                    listitem.SubItems.AddRange(new String[] { format.Name, 
                                                              format.Units.ToString(), 
                                                              format.Unitsuffix.ToString(), 
                                                              "" });
                }

                this.listView.Items.Add(listitem);
            }               

        }

        /// <summary>
        /// Reset the project unit decimal symbol type when click ok.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void okButton_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (Autodesk.Revit.Enums.DecimalSymbolType de in Enum.GetValues(typeof(
                    Autodesk.Revit.Enums.DecimalSymbolType)))
                {
                    if (de.ToString() == this.decimalComboBox.SelectedItem.ToString())
                    {
                        m_dataBuffer.DecimalSyType = de;
                    }
                }

                foreach (Autodesk.Revit.Enums.RiseRunOrAngleType slo in Enum.GetValues(typeof(
                    Autodesk.Revit.Enums.RiseRunOrAngleType)))
                {
                    if (slo.ToString() == this.slopeComboBox.SelectedItem.ToString())
                    {
                        m_dataBuffer.SlopeType = slo;
                    }
                }

                this.Close();
            }
            catch
            {
                throw;
            }           
        }

        /// <summary>
        /// Close the form when click cancel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}