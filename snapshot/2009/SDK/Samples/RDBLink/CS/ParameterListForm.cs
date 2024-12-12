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
using Autodesk.Revit.Collections;
using Autodesk.Revit.Parameters;
using Autodesk.Revit;
using System.Data.Odbc;
using RevitApplication = Autodesk.Revit.Application;

namespace Revit.SDK.Samples.RDBLink.CS
{
    /// <summary>
    /// Lists current custom parameters and provides some operations for them
    /// </summary>
    public partial class ParameterListForm : Form
    {
        #region Fields
        /// <summary>
        /// RevitApplication
        /// </summary>
        RevitApplication m_revitApp;

        /// <summary>
        /// In charge of parameter creation
        /// </summary>
        ParameterCreation m_parameterCreation;

        /// <summary>
        /// TableInfoSet
        /// </summary>
        TableInfoSet m_tableInfoSet = null;
        #endregion

        #region Constructors
        DataSet m_dataSet = null;

        /// <summary>
        /// Default constructor
        /// </summary>
        private ParameterListForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialize all controls and some variables
        /// </summary>
        /// <param name="revitApp">RevitApplication</param>
        /// <param name="dataSet">DataSet used by current GridView</param>
        public ParameterListForm(RevitApplication revitApp, DataSet dataSet)
            : this()
        {
            m_revitApp = revitApp;
            m_tableInfoSet = Command.TableInfoSet;
            m_parameterCreation = new ParameterCreation(m_revitApp);
            m_dataSet = dataSet;
            parameterListBox.DisplayMember = "ParameterName";
            parameterListBox.Sorted = true;

            // initialize the list of IParameterInfo contains all custom parameters
            DefinitionBindingMapIterator bmIt = m_revitApp.ActiveDocument.ParameterBindings.ForwardIterator();
            bmIt.Reset();
            while (bmIt.MoveNext())
            {
                Definition definition = bmIt.Key;
                string definitionName = definition.Name;
                if (!ParameterExists(definitionName))
                    parameterListBox.Items.Add(definitionName);
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Add a parameter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addButton_Click(object sender, EventArgs e)
        {
            using (ParameterPropertyForm parameterPropertyForm = new ParameterPropertyForm(m_revitApp))
            {
                // show the parameter creation dialog to get input from user
                if (parameterPropertyForm.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // create a parameter
                        if(!m_parameterCreation.CreateUserDefinedParameter(parameterPropertyForm.ParameterInfo))
                        {
                            MessageBox.Show(RDBResource.GetString("MessageBox_Import_CreateParameterFailed"));
                            return;
                        }
                        ParameterInfo parameterInfo = parameterPropertyForm.ParameterInfo;

                        // add the new created parameter to ListBox
                        parameterListBox.Items.Add(parameterInfo.ParameterName);
                        parameterListBox.SelectedItem = parameterInfo.ParameterName;

                        // add shared parameter related columns to database
                        foreach (Category category in parameterInfo.Categories)
                        {
                            if (category != null)
                            {
                                string tableKey = RDBResource.GetTableKey(category, parameterInfo.ParameterForType);
                                string tableName = RDBResource.GetTableName(tableKey);

                                // if this column does not exist, create it
                                if (tableName != null && !Command.ColumnExists(tableName, parameterInfo.ParameterName))
                                {
                                    // create a new column to database
                                    bool isSuccess = Command.AddColumn(parameterInfo.ParameterName, 
                                        DatabaseConfig.GetDataType(parameterInfo.ParameterType), tableName);

                                    if (isSuccess)
                                    {
                                        // update table schema
                                        // add the column info to table info
                                        bool notExist = true;
                                        TableInfo tableInfo = m_tableInfoSet[tableKey];
                                        foreach (ColumnInfo colinfo in tableInfo)
                                        {
                                            if (colinfo.Name == parameterInfo.ParameterName)
                                            {
                                                notExist = false;
                                                break;
                                            }
                                        }
                                        if(notExist)
                                            tableInfo.Add(new CustomColumnInfo(parameterInfo.ParameterName,
                                                DatabaseConfig.GetDataType(parameterInfo.ParameterType)));
                                        
                                        // add the column to dataset
                                        Command.RefreshDataTable(m_dataSet.Tables[tableName]);
                                    }                                    
                                }
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        System.Diagnostics.Trace.WriteLine(ex.ToString());
                    }
                }
            }
        }

        /// <summary>
        /// Close this form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Create new columns to tables related with the new added parameters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void okButton_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// If the parameter is already in the parameter list
        /// </summary>
        /// <param name="paramInfo">parameter info</param>
        /// <returns>true if exists otherwise false</returns>
        private bool ParameterExists(string definitionName)
        {
            foreach (string definitionTmp in parameterListBox.Items)
            {
                if (definitionTmp == definitionName)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion
    }
}
