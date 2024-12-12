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
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.?AUTODESK, INC.
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
using Autodesk.Revit.Enums;
using Autodesk.Revit;
using RevitApplication = Autodesk.Revit.Application;

namespace Revit.SDK.Samples.RDBLink.CS
{
    /// <summary>
    /// Provides UI for user to modify or create a parameter
    /// </summary>
    public partial class ParameterPropertyForm : Form
    {
        #region Fields
        /// <summary>
        /// category list binded with the parameter
        /// </summary>
        Categories m_categories = null;

        /// <summary>
        /// RevitApplication
        /// </summary>
        RevitApplication m_revitApp;

        /// <summary>
        /// parameter infomation
        /// </summary>
        ParameterInfo m_parameterInfo = null;
        #endregion

        #region Properties
        /// <summary>
        /// Gets IParameterInfo from the form
        /// </summary>
        public ParameterInfo ParameterInfo
        {
            get { return m_parameterInfo; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        private ParameterPropertyForm()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Initializes controls and RevitApplication
        /// </summary>
        /// <param name="revitApp">RevitApplication</param>
        /// <param name="dataSet">DataSet</param>
        public ParameterPropertyForm(RevitApplication revitApp)
            : this()
        {
            m_revitApp = revitApp;
            m_categories = m_revitApp.ActiveDocument.Settings.Categories;
            m_parameterInfo = new ParameterInfo();
            InitializeDataSourcesAndControls();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Set controls data source and initialize their display values
        /// </summary>
        private void InitializeDataSourcesAndControls()
        {
            // initialize ComboBoxes
            groupComboBox.Sorted = true;
            groupComboBox.DataSource = Enum.GetValues(typeof(BuiltInParameterGroup));
            groupComboBox.Format += new ListControlConvertEventHandler(groupComboBox_Format);

            typeComboBox.Sorted = true;
            typeComboBox.DataSource = Enum.GetValues(typeof(ParameterType));

            foreach (Category cat in m_categories)
            {
                categoryCheckedListBox.Items.Add(cat);
            }
            categoryCheckedListBox.DisplayMember = "Name";

            // initialize RadioButtons
            projectParameterRadioButton.Checked = m_parameterInfo.ParameterIsProject;
            sharedParameterRadioButton.Checked = !m_parameterInfo.ParameterIsProject;
            typeRadioButton.Checked = m_parameterInfo.ParameterForType;
            instanceRadioButton.Checked = !m_parameterInfo.ParameterForType;

            // initialize TextBoxes
            nameTextBox.Text = m_parameterInfo.ParameterName;

            // initialize ComboBoxes
            groupComboBox.SelectedItem = m_parameterInfo.ParameterGroup;
            typeComboBox.SelectedItem = m_parameterInfo.ParameterType;

            // initialize CheckedListBox
            if (m_parameterInfo.Categories != null)
            {
                foreach (Category cat in m_parameterInfo.Categories)
                {
                    int index = categoryCheckedListBox.Items.IndexOf(cat);
                    categoryCheckedListBox.SetItemChecked(index, true);
                }
            }
        }

        /// <summary>
        /// Format a item in combo box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void groupComboBox_Format(object sender, ListControlConvertEventArgs e)
        {
            e.Value = RDBResource.GetString("IDS_" + e.ListItem.ToString());
        }

        /// <summary>
        /// Save parameter info and close the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void okButton_Click(object sender, EventArgs e)
        {
            // get categories
            CategorySet categories = new CategorySet();
            foreach (Category cat in categoryCheckedListBox.CheckedItems)
            {
                categories.Insert(cat);
            }
            m_parameterInfo.Categories = categories;

            string parameterName = nameTextBox.Text.Trim();

            // name should not be empty
            if(parameterName == string.Empty)
            {
                MessageBox.Show(RDBResource.GetString("MessageBox_Name_Shoud_Not_Be_Empty"));
                return;
            }

            // Only characters 'A-Z', 'a-z', '0-9' and '_' are available and 
            // numbers can't be at the beginning of a name
            if (char.IsNumber(parameterName[0]))
            {
                MessageBox.Show(RDBResource.GetString("MessageBox_Name_Should_Start_With_Letter"));
                return;
            }
            else
            {
                foreach (char ch in parameterName.ToCharArray())
                {
                    if (!(
                        ('0' <= ch && ch <= '9') ||
                        ('A' <= ch && ch <= 'Z') ||
                        ('a' <= ch && ch <= 'z') ||
                        ch == '_'))
                    {
                        MessageBox.Show(string.Format(
                            RDBResource.GetString("MessageBox_Name_Contains_Illegal_Characters")
                            , ch));
                        return;
                    }
                }
            }

            // there should be some categories selected
            if (categoryCheckedListBox.CheckedItems.Count == 0)
            {
                MessageBox.Show(RDBResource.GetString("MessageBox_Select_One_Category"));
                return;
            }

            m_parameterInfo.ParameterIsProject = projectParameterRadioButton.Checked;
            m_parameterInfo.ParameterForType = typeRadioButton.Checked;
            m_parameterInfo.ParameterName = parameterName;
            m_parameterInfo.ParameterGroup = (BuiltInParameterGroup)groupComboBox.SelectedItem;
            m_parameterInfo.ParameterType = (ParameterType)typeComboBox.SelectedItem;

            this.Close();
            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Close the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Check all categories
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkAllButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < categoryCheckedListBox.Items.Count; i++)
            {
                categoryCheckedListBox.SetItemChecked(i, true);
            }
        }

        /// <summary>
        /// Uncheck all categories
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkNoneButton_Click(object sender, EventArgs e)
        {
            foreach (int index in categoryCheckedListBox.CheckedIndices)
            {
                categoryCheckedListBox.SetItemChecked(index, false);
            }
        }
        
        #endregion
    }
}
