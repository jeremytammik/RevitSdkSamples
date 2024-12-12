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
using System.Data.Odbc;
using System.Data.Common;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Autodesk.Revit;
using Autodesk.Revit.Collections;
using Autodesk.Revit.Parameters;
using System.Diagnostics;

namespace Revit.SDK.Samples.RDBLink.CS
{
    /// <summary>
    /// Main form, provide UI for user to use this software.
    /// </summary>
    public partial class RDBLinkForm : Form
    {
        #region Fields
        /// <summary>
        /// Odbc database command object.
        /// </summary>
        OdbcCommand m_dbCommand;

        /// <summary>
        /// Odbc database connection object.
        /// </summary>
        OdbcConnection m_dbConnection = new OdbcConnection();

        /// <summary>
        /// RevitAPI document object.
        /// </summary>
        Document m_revitDocument;

        /// <summary>
        /// Table manager used to operate database table. 
        /// </summary>
        TableManager m_tableManager = new TableManager();

        /// <summary>
        /// RDBLink object used to transfer data between Revit and database.
        /// </summary>
        RDBLink m_rdbLink;
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor.
        /// </summary>
        public RDBLinkForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Overloaded constructor.
        /// </summary>
        /// <param name="revitData">RevitAPI ExternalCommandData object</param>
        public RDBLinkForm(ExternalCommandData revitData)
            : this()
        {
            m_revitDocument = revitData.Application.ActiveDocument;
            m_dbCommand = m_dbConnection.CreateCommand();
        } 
        #endregion

        #region Functions
        /// <summary>
        /// Export button event handle.
        /// </summary>
        /// <param name="sender">Who sends this event</param>
        /// <param name="e">Event arguments</param>
        private void exportButton_Click(object sender, EventArgs e)
        {
            Run(true);
        }

        /// <summary>
        /// Import button event handle.
        /// </summary>
        /// <param name="sender">Who sends this event</param>
        /// <param name="e">Event arguments</param>
        private void importButton_Click(object sender, EventArgs e)
        {
            Run(false);
        }

        /// <summary>
        /// Integrate export and import two functions.
        /// </summary>
        /// <param name="export">Indicate export or import</param>
        private void Run(bool export)
        {
            // When do import, we will modify or create some elements,
            // if import failed, we have to undo all the changes made to Revit.
            // so Transaction is used here.
            if (!export)
                m_revitDocument.BeginTransaction();

            if(m_rdbLink == null) m_rdbLink = new RDBLink();

            // get connection string
            string connStr = string.Empty;
            int state = SelectDatabase(ref connStr);
            if (state != SQLDriver.SQL_SUCCESS && state != SQLDriver.SQL_SUCCESS_WITH_INFO)
                return;

            string msg = (export ? "Export" : "Import") + " successfully!";

            //set log file
            // if you want to log the trace information, just uncomment the follow 4 lines and 1 line
            // in the finally clause to remove the TraceListener
            //string log = "RDBLink_log_" + GetTimeString() + ".txt";
            //if (System.IO.File.Exists(log)) System.IO.File.Delete(log);
            //TraceListener txtListener = new TextWriterTraceListener(log);
            //Trace.Listeners.Add(txtListener);
            try
            {
                #region BeginTransfer
                // get the connection to database
                OpenConnection(connStr);
                if (m_dbConnection.State != ConnectionState.Open)
                {
                    return;
                }

                m_tableManager.SetConnection(m_dbConnection, m_dbCommand);

                // check whether the database is valid
                if (m_tableManager.CheckOldTables() &&
                    m_tableManager.CheckNewTables())
                {
                    // get all columns contain shared and project parameters and the related tables
                    Dictionary<string, List<string>> tableNameExtraColumnsMap =
                        m_tableManager.CheckSharedParameterColumns(m_revitDocument);

                    // when do export, we clear the tables first
                    if (export)
                    {
                        if (!m_tableManager.ClearTables())
                        {
                            MessageBox.Show("The database can't be clear, make sure it is not read only.");
                            return;
                        }
                    }

                    m_rdbLink.TableManger = m_tableManager;
                    // categorizes Revit elements to different lists
                    m_rdbLink.PrepareRevitData(m_revitDocument, tableNameExtraColumnsMap);

                    // begin import or export
                    m_rdbLink.BeginTransfer(m_dbCommand, export);

                    // end transaction when do import
                    if (!export)
                        m_revitDocument.EndTransaction();

                    // show success message
                    MessageBox.Show(msg);
                }
                else
                {
                    MessageBox.Show("The database used should be an existing database that ever exported through Revit");
                } 
                #endregion
            }
            catch (Exception ex)
            {
                // abort transaction when exception happens
                if (!export)
                    m_revitDocument.AbortTransaction();
                Trace.WriteLine("[ex]: " + ex.ToString());
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                //Close the connection
                CloseConnection();
                Trace.Flush();
                Trace.Close();
                //Trace.Listeners.Remove(txtListener);
            }
        }

        /// <summary>
        /// Catenate date and time string
        /// </summary>
        /// <returns>Time string</returns>
        private string GetTimeString()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmss");
        }

        /// <summary>
        /// Open database connection.
        /// </summary>
        /// <param name="connectionString">Database connection string</param>
        private void OpenConnection(string connectionString)
        {
            if (m_dbConnection.ConnectionString != connectionString ||
                m_dbConnection.State != ConnectionState.Open)
            {
                m_dbConnection.Close();
                m_dbConnection.ConnectionString = connectionString;
                try
                {
                    m_dbConnection.Open();
                }
                catch
                {
                    CloseConnection();
                }
            }
        }

        /// <summary>
        /// Close database connection.
        /// </summary>
        public void CloseConnection()
        {
            //check whether the connection is opened. if so, close it.
            if (null != m_dbConnection || ConnectionState.Closed == m_dbConnection.State)
            {
                m_dbConnection.Close();
            }
        }

        /// <summary>
        /// Prompt user to select a data source from the pop up dialog.
        /// </summary>
        /// <param name="connectStr">Output selected connection string</param>
        /// <returns>state value</returns>
        public int SelectDatabase(ref string connectStr)
        {
            int state = SQLDriver.Connect("", SQLDriver.SQL_DRIVER_PROMPT, ref connectStr);
            SQLDriver.Terminate();
            return state;
        } 
        #endregion
    }
}
