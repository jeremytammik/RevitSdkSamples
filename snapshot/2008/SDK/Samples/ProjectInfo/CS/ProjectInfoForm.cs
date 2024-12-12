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
using Autodesk.Revit.Parameters;

namespace Revit.SDK.Samples.ProjectInfo.CS
{
    /// <summary>
    /// This form class display all the member properties of ProjectInfo.
    /// user can edit and modify the values of these properties
    /// </summary>
    public partial class ProjectInfoForm : Form
    {
        ProjectInfoData m_projectInfoData = null;

        /// <summary>
        /// default constructor, can not create this form instance without parameter
        /// </summary>
        private ProjectInfoForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialize GUI with ProjectInfoData 
        /// </summary>
        /// <param name="projectInfoData"></param>
        public ProjectInfoForm(ProjectInfoData projectInfoData) : this()
        {
            m_projectInfoData = projectInfoData;

            //Initialize ComboBox with all building types
            buildingTypeComboBox.DataSource = ProjectInfoData.AllBuildingTypes;

            if(m_projectInfoData != null)
            {
                //Get data from projectInfoData
                nameTextBox.Text = m_projectInfoData.Name;
                numberTextBox.Text = m_projectInfoData.Number;
                statusTextBox.Text = m_projectInfoData.Status;
                issueDateTextBox.Text = m_projectInfoData.IssueDate;
                addressTextBox.Text = m_projectInfoData.Address;
                clientNameTextBox.Text = m_projectInfoData.ClientName;
                zipCodeTextBox.Text = m_projectInfoData.ZipCode;
                buildingTypeComboBox.SelectedItem = m_projectInfoData.BuildingType;
            }
        }

        /// <summary>
        /// Update ProjectInfo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void okButton_Click(object sender, EventArgs e)
        {
            if(m_projectInfoData != null)
            {
                //Set data to projectInfoData
                m_projectInfoData.Name = nameTextBox.Text;
                m_projectInfoData.Number = numberTextBox.Text;
                m_projectInfoData.Status = statusTextBox.Text;
                m_projectInfoData.IssueDate = issueDateTextBox.Text;
                m_projectInfoData.Address = addressTextBox.Text;
                m_projectInfoData.ZipCode = zipCodeTextBox.Text;
                m_projectInfoData.ClientName = clientNameTextBox.Text;
                m_projectInfoData.BuildingType = buildingTypeComboBox.Text;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}