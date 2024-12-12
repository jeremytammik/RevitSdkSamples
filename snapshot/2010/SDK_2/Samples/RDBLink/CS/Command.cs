//
// (C) Copyright 2003-2009 by Autodesk, Inc.
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
using System.Text;
using System.Data;
using Autodesk.Revit;
using System.Windows.Forms;
using System.Data.Odbc;
using Autodesk.Revit.Parameters;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;
using System.Globalization;
using RevitApplication = Autodesk.Revit.Application;

namespace Revit.SDK.Samples.RDBLink.CS
{
    /// <summary>
    /// Implements the Revit add-in interface IExternalCommand
    /// </summary>
    public class Command : IExternalCommand
    {
        #region Fields

        /// <summary>
        /// Table schema and some meta data
        /// </summary>
        public static TableInfoSet TableInfoSet;

        /// <summary>
        /// Odbc connection
        /// </summary>
        public static OdbcConnection OdbcConnection;

        /// <summary>
        /// Revit application
        /// </summary>
        RevitApplication m_app;

        /// <summary>
        /// Current Revit Document
        /// </summary>
        Document m_doc;

        /// <summary>
        /// Table names that sorted by their relations
        /// </summary>
        TableLevelSet m_sortedTables;

        /// <summary>
        /// Form to show export or import progress
        /// </summary>
        ProgressForm m_progressForm;

        /// <summary>
        /// Approximate number of steps that the progress bar will go while exporting
        /// </summary>
        int tmpRatioForExport = 20;

        /// <summary>
        /// Approximate number of steps that the progress bar will go while clear garbage records
        /// </summary>
        int tmpRatioForClearGarbage = 200;


        /// <summary>
        /// Used to print time span for specific progress
        /// </summary>
        DateTime m_now = DateTime.Now;

        #endregion

        #region Interface Implementation
        /// <summary>
        /// Implement this method as an external command for Revit.
        /// </summary>
        /// <param name="commandData">An object that is passed to the external application 
        /// which contains data related to the command, 
        /// such as the application object and active view.</param>
        /// <param name="message">A message that can be set by the external application 
        /// which will be displayed if a failure or cancellation is returned by 
        /// the external command.</param>
        /// <param name="elements">A set of elements to which the external application 
        /// can add elements that are to be highlighted in case of failure or cancellation.</param>
        /// <returns>Return the status of the external command. 
        /// A result of Succeeded means that the API external method functioned as expected. 
        /// Cancelled can be used to signify that the user cancelled the external operation 
        /// at some point. Failure should be returned if the application is unable to proceed with 
        /// the operation.</returns>
        public IExternalCommand.Result Execute(ExternalCommandData commandData,
            ref string message, ElementSet elements)
        {
            IExternalCommand.Result result = IExternalCommand.Result.Failed;
            m_app = commandData.Application;
            m_doc = m_app.ActiveDocument;
            APIObjectList.ActiveDocument = m_doc;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CurrentCulture;

            //create log file
            //string log = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            //    + @"\RDBLink_log_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
            //if (File.Exists(log)) File.Delete(log);
            //TraceListener txtListener = new TextWriterTraceListener(log);
            //Trace.Listeners.Add(txtListener);

            SQLDriver sqlDriver = null;
            try
            {
                // show main form
                RDBLinkForm rdbLinkForm = new RDBLinkForm();
                rdbLinkForm.StartPosition = FormStartPosition.CenterParent;
                if (rdbLinkForm.ShowDialog() == DialogResult.OK)
                {
                    string connStr = null;
                    sqlDriver = new SQLDriver();

                    //prompt user to select a database
                    DatabaseType dbType = sqlDriver.SelectDatabase(ref connStr);
                    if (dbType == DatabaseType.Invalid)
                    {
                        sqlDriver.Terminate();
                        return IExternalCommand.Result.Cancelled;
                    }

                    DatabaseConfig.Initialize(dbType);

                    //initialize table schema
                    InitializeTableSchemaAndMetaData();

                    int max, eleNum = 0;

                    //count elements number in current document
                    ElementIterator eit = m_doc.Elements;
                    eit.Reset();
                    while (eit.MoveNext())
                    {
                        ++eleNum;
                    }

                    if (rdbLinkForm.IsExport)
                    {
                        tmpRatioForExport = 2 * eleNum / TableInfoSet.Count;
                        tmpRatioForClearGarbage = 10 * tmpRatioForExport;
                        max = eleNum  // categorize elements
                            + TableInfoSet.Count * tmpRatioForExport // export
                            + m_sortedTables.Count // commit changes to database
                            + tmpRatioForClearGarbage; // clear garbage records
                        BeginProgress(max);
                        m_progressForm.ProgressBarLableTitle = RDBResource.GetString("Progress_ExportData");
                        m_progressForm.Update();

                        //database manager to create database
                        DatabaseManager dbMan = new DatabaseManager(TableInfoSet);
                        dbMan.Create(sqlDriver, m_doc, TableInfoSet);
                        sqlDriver.Terminate();

                        //categorize all elements in current document
                        CategorizeElements();

                        //open database connection
                        OdbcConnection = OpenConnection(connStr);
                        if (OdbcConnection == null) return IExternalCommand.Result.Failed;

                        //begin export data
                        TransferData(OdbcConnection.CreateCommand(), rdbLinkForm.IsExport);
                    }
                    else
                    {
                        // verify database is suitable for importing
                        if (!IsDatabaseValid(sqlDriver))
                        {
                            //close sql driver
                            sqlDriver.Terminate();
                            MessageBox.Show(
                                RDBResource.GetString("MessageBox_Import_DatabaseInvalid"),
                                RDBResource.GetString("MessageBox_Import_Failed_Caption"),
                                MessageBoxButtons.OK);
                            return IExternalCommand.Result.Cancelled;
                        }

                        //close sql driver
                        sqlDriver.Terminate();
                        BeginProgress(eleNum);
                        m_progressForm.ProgressBarLableTitle = RDBResource.GetString("Progress_PrepareData");
                        m_progressForm.Update();

                        //categorize all elements in current document
                        CategorizeElements();

                        //open database connection
                        OdbcConnection = OpenConnection(connStr);
                        if (OdbcConnection == null) return IExternalCommand.Result.Failed;

                        //begin import data
                        TransferData(OdbcConnection.CreateCommand(), rdbLinkForm.IsExport);
                    }

                    rdbLinkForm.Dispose();
                }

                result = IExternalCommand.Result.Succeeded;
            }
            catch (Exception ex)
            {
                if (m_progressForm != null)
                    m_progressForm.Close();
                message = ex.Message + ex.StackTrace;
                Trace.WriteLine(ex.ToString());
                return IExternalCommand.Result.Failed;
            }
            finally
            {
                if (sqlDriver != null) sqlDriver.Terminate();
                CloseConnection(OdbcConnection);
                OdbcConnection = null;
                Trace.Flush();
                Trace.Close();
                //Trace.Listeners.Remove(txtListener);
            }

            return result;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Create the ProgressForm.
        /// </summary>
        /// <param name="maxAll"></param>
        private void BeginProgress(int maxAll)
        {
            m_progressForm = new ProgressForm();
            m_progressForm.ProgressStep = 1;
            m_progressForm.ProgressMaximum = maxAll;
            m_progressForm.Show();
        }

        /// <summary>
        /// Verify whether the database is valid or not.
        /// </summary>
        /// <param name="sqlDriver">SQLDriver</param>
        /// <returns>true if the schema of the database is compatible with 
        /// that defined in TableInfoSet, otherwise false</returns>
        private bool IsDatabaseValid(SQLDriver sqlDriver)
        {
            int countTableExist = 0;
            foreach (TableInfo tableInfo in TableInfoSet.Values)
            {
                string tableName = tableInfo.Name;
                if (sqlDriver.TableExist(tableName))
                {
                    ++countTableExist;
                    bool isBuiltInTable = tableInfo is InstanceTableInfo || tableInfo is SymbolTableInfo;
                    foreach (ColumnInfo colInfo in tableInfo)
                    {                       
                        string colName = colInfo.Name;
                        if (!sqlDriver.ColumnExist(tableName, colName))
                        {
                            // if the column is a shared parameter, create a corresponding column in database
                            if (isBuiltInTable && colInfo is CustomColumnInfo)
                            {
                                bool isOK = sqlDriver.ExecuteSQL(string.Format("ALTER TABLE {0} ADD [{1}] {2}",
                                    tableName, colInfo.Name,
                                    DatabaseConfig.Instance().GetDataType(colInfo.DataType)));
                                if (!isOK) return false;
                                continue;
                            }
                            return false;
                        }
                    }
                }
            }

            return countTableExist > 0;
        }

        /// <summary>
        /// Refresh the CommandBuilder to bind to a new table.
        /// </summary>
        /// <param name="cmdBuilder">CommandBuilder to be updated</param>
        /// <param name="tableName">Table name used to update the CommandBuilder</param>
        private void RefreshCmdBuilder(OdbcCommandBuilder cmdBuilder, string tableName)
        {
            cmdBuilder.DataAdapter.SelectCommand.CommandText = "SELECT * FROM " + tableName;
            cmdBuilder.RefreshSchema();
        }

        /// <summary>
        /// Load all tables from database into a DataSet
        /// </summary>
        /// <param name="dbCommand">OdbcCommand used to execute SQL statement</param>
        /// <param name="outCmdBuilder">CommandBuilder which will be created internally</param>
        /// <returns>a DataSet contains all tables of a database</returns>
        private DataSet BuildDataSet(OdbcCommand dbCommand, out OdbcCommandBuilder outCmdBuilder)
        {
            Trace.WriteLine("BuildDataSet...");

            // populate all tables in database to a DataSet
            DataSet dataSet = new DataSet("RDBLink");
            OdbcDataAdapter dataAdapter = new OdbcDataAdapter(dbCommand);
            OdbcCommandBuilder cmdBuilder = new OdbcCommandBuilder(dataAdapter);
            cmdBuilder.QuotePrefix = "[";
            cmdBuilder.QuoteSuffix = "]";
            Trace.Indent();
            string tableName = null;
            foreach (TableInfo tbInfo in TableInfoSet.Values)
            {
                if (!TableExists(tbInfo.Name)) continue;

                Trace.WriteLine(tbInfo.Name);
                tableName = tbInfo.Name;
                RefreshCmdBuilder(cmdBuilder, tableName);
                dataAdapter.Fill(dataSet, tableName);
            }
            Trace.Unindent();
            outCmdBuilder = cmdBuilder;
            return dataSet;
        }

        /// <summary>
        /// Transfer data from Revit to database.
        /// </summary>
        /// <param name="dbSet">DataSet use to export to</param>
        private void Export(DataSet dbSet)
        {
            Trace.WriteLine("Exporting ...");
            m_progressForm.ProgressStep = tmpRatioForExport;
            Trace.Indent();
            foreach (TableInfo tbInfo in TableInfoSet.Values)
            {
                Trace.WriteLine(tbInfo.Name);
                m_progressForm.PerformStep();
                m_progressForm.Update();

                DataTable dbTable = dbSet.Tables[tbInfo.Name];
                tbInfo.ObjectList.Export(dbTable);
            }
            Trace.Unindent();
            TraceTime();

            m_progressForm.ProgressStep = 1;
        }

        /// <summary>
        /// Whether a table exists.
        /// </summary>
        /// <param name="tableName">table name.</param>
        /// <returns>true if exists, otherwise false.</returns>
        public static bool TableExists(string tableName)
        {
            return OdbcConnection.GetSchema("Tables",
                new string[] { null, null, tableName }).Rows.Count > 0;
        }

        /// <summary>
        /// Execute NonQuery 
        /// </summary>
        /// <param name="sqlStmt">SQL statement</param>
        /// <returns></returns>
        public static bool ExecuteSQL(string sqlStmt)
        {
            try
            {
                IDbCommand cmd = OdbcConnection.CreateCommand();
                cmd.CommandText = sqlStmt;
                int retval = cmd.ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Whether a column table exists in a table
        /// </summary>
        /// <param name="tableName">table name</param>
        /// <param name="columnName">column name</param>
        /// <returns>true if exists, otherwise false</returns>
        public static bool ColumnExists(string tableName, string columnName)
        {
            return OdbcConnection.GetSchema("Columns",
                new string[] { null, null, tableName, columnName }).Rows.Count > 0;
        }

        /// <summary>
        /// Remove a column from a table
        /// </summary>
        /// <param name="columnName">column name</param>
        /// <param name="tableName">table name</param>
        /// <returns>true if success, otherwise false</returns>
        public static bool DropColumn(string columnName, string tableName)
        {
            if (tableName != null && ColumnExists(tableName, columnName))
            {
                // "ALTER TABLE tableName DROP COLUMN columnName"            
                // "ALTER TABLE tableName DROP columnName"
                //  Access can accept above two syntaxes.        
                //  SQLServer just accept the second one.                 
                return Command.ExecuteSQL(string.Format("ALTER TABLE {0} DROP [{1}]",
                    tableName, columnName));
            }
            return false;
        }

        /// <summary>
        /// Add a column to a table
        /// </summary>
        /// <param name="columnType">column type</param>
        /// <param name="columnName">column name</param>
        /// <param name="tableName">table name</param>
        public static bool AddColumn(string columnName, DataType columnType, string tableName)
        {
            // "ALTER TABLE tableName ADD COLUMN columnName columnType"            
            // "ALTER TABLE tableName ADD columnName columnType"
            //  Access can accept above two syntaxes.        
            //  SQLServer just accept the second one.
            return Command.ExecuteSQL(string.Format("ALTER TABLE {0} ADD [{1}] {2}",
               tableName, columnName, DatabaseConfig.Instance().GetDataType(columnType)));
        }

        /// <summary>
        /// Refresh data in DataTable
        /// </summary>
        /// <param name="dbTable">DataTable</param>
        public static void RefreshDataTable(DataTable dbTable)
        {
            OdbcCommand cmd = OdbcConnection.CreateCommand();
            cmd.CommandText = "SELECT * FROM " + dbTable.TableName;
            OdbcDataAdapter dbAdapter = new OdbcDataAdapter(cmd);
            dbAdapter.Fill(dbTable);
        }

        /// <summary>
        /// Transfer data from database to Revit.
        /// </summary>
        /// <param name="dbSet">DataSet use to import from</param>
        private void Import(DataSet dbSet)
        {
            Trace.WriteLine("Import...");



            List<ErrorTable> ets = new List<ErrorTable>();
            foreach (TableInfo tbInfo in TableInfoSet.Values)
            {
                Trace.WriteLine(tbInfo.Name);
                m_progressForm.PerformStep();
                m_progressForm.Update();

                DataTable dbTable = dbSet.Tables[tbInfo.Name];
                if (dbTable == null) continue;

                ErrorTable et = tbInfo.ObjectList.Import(dbTable);
                ets.Add(et);
            }

            // the following code will generate the import html formated report.

            // create report file
            string reportFile = @"ImportLog_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".htm";
            StreamWriter sw = new StreamWriter(reportFile, false, Encoding.Unicode);

            // HTML header, including title and css style.
            string header = "<head><meta content=\"text/html; charset=UTF-8\"> <title>RDB Import report - " + DateTime.Now.ToString() +
                "</title><style type=\"text/css\">" +
                @"
                .UnKnown{color:Orange;}
                .AssemblyCode{color:Gray;}
                .Success{color:Blue;}
                .Failed{color:Red;}
                .ReadOnlyFailed{color:Fuchsia;}
                .ParameterNull{color:DarkOrchid;}
                .ValueNull{color:Violet;}
                .Equals{color:Gray;}
                .Exception{color:Yellow;}
                " +
                "</style></head>";
            sw.WriteLine("<html>");
            sw.WriteLine(header);
            sw.WriteLine("<body>");
            
            // Font style indication
            sw.WriteLine(
                "<p><b>" + RDBResource.GetString("Report_Legend") + "</b></p>" +
                "<table border=\"1\"><tr align=\"left\"><th>" + RDBResource.GetString("Report_Appearance") + "</th><th>" + RDBResource.GetString("Report_Meaning") + "</th></tr>" +
                //"<tr><td class=\"AssemblyCode\">" +RDBResource.GetString("Report_Gray")+ "</td><td>" +RDBResource.GetString("Report_AssemblyCode")+ "</td></tr>" +
                "<tr><td class=\"Success\">" + RDBResource.GetString("Report_Blue") + "</td><td>" + RDBResource.GetString("Report_Success") + "</td></tr>" +
                "<tr><td class=\"Failed\">" + RDBResource.GetString("Report_Red") + "</td><td>" + RDBResource.GetString("Report_Failed") + "</td></tr>" +
                "<tr><td class=\"ReadOnlyFailed\">" + RDBResource.GetString("Report_Fuchsia") + "</td><td>" + RDBResource.GetString("Report_ReadOnlyFailed") + "</td></tr>" +
                "<tr><td class=\"ParameterNull\">" + RDBResource.GetString("Report_DarkOrchid") + "</td><td>" + RDBResource.GetString("Report_ParameterNull") + "</td></tr>" +
                "<tr><td class=\"ValueNull\">" + RDBResource.GetString("Report_Violet") + "</td><td>" + RDBResource.GetString("Report_ValueNull") + "</td></tr>" +
                "<tr><td class=\"Equals\">" + RDBResource.GetString("Report_Gray") + "</td><td>" + RDBResource.GetString("Report_Equals") + "</td></tr>" +
                "<tr><td class=\"Exception\">" + RDBResource.GetString("Report_Yellow") + "</td><td>" + RDBResource.GetString("Report_Exception") + "</td></tr>" +
                "<tr><td class=\"UnKnown\">" + RDBResource.GetString("Report_Orange") + "</td><td>" + RDBResource.GetString("Report_UnKnown") + "</td></tr>" +
                "<tr><td><del>" + RDBResource.GetString("Report_Deleted_Row") + "</del></td><td>" + RDBResource.GetString("Report_Deleted_Row") + "</td></tr>" +
                "<tr><td><i>" + RDBResource.GetString("Report_Added_Row") + "</i></td><td>" + RDBResource.GetString("Report_Added_Row") + "</td></tr>" +
                "</table>");
            sw.WriteLine("<hr>");
            //add index
            List<ErrorTable> tablesNotSupportImporting = new List<ErrorTable>();
            List<ErrorTable> tablesHasChanges = new List<ErrorTable>();
            foreach (ErrorTable et in ets)
            {
                if (et != null)
                {
                    if (et.NotSupportImporting)
                    {
                        tablesNotSupportImporting.Add(et);
                        continue;
                    }
                    if (et.Rows.Count == 0 || !et.HasChange) continue;
                    tablesHasChanges.Add(et);
                }
            }
            sw.WriteLine("<p><b>" + RDBResource.GetString("Report_Tables_that_does_not_support_importing") + "</b></p>");
            foreach (ErrorTable et in tablesNotSupportImporting)
            {
                sw.WriteLine("{0}&nbsp", et.TableName);
            }
            sw.WriteLine("<hr>");
            sw.WriteLine("<p><b>" + RDBResource.GetString("Report_Indexes_of_changed_tables") + "</b></p>");
            foreach (ErrorTable et in tablesHasChanges)
            {
                sw.WriteLine("<a href=\"#{0}\">{0}</a></br>", et.TableName);
            }
            sw.WriteLine("<hr>");

            //add tables
            sw.WriteLine("<p><b>" + RDBResource.GetString("Report_Tables") + "</b></p>");
            foreach (ErrorTable et in ets)
            {
                if (et != null)
                {
                    TableInfo tableInfo = TableInfoSet[et.TableId];
                    if (et.Rows.Count == 0 || !et.HasChange) continue;
                    sw.WriteLine("<P><a name=\"{0}\">{0}</P>", et.TableName);
                    StringBuilder htmlTable = new StringBuilder();
                    htmlTable.Append("<table border=\"1\">");
                    htmlTable.Append("<tr>");
                    
                    // column header
                    foreach (ColumnInfo columnInfo in tableInfo)
                    {
                        htmlTable.Append(string.Format("<th>{0}</th>", columnInfo.Name));
                    }
                    htmlTable.Append("</tr>");

                    // row data
                    foreach (ErrorRow er in et.Rows)
                    {
                        htmlTable.Append("<tr>");
                        string rowCode = "<td class=\"{0}\">{1}</td>";
                        switch (er.State)
                        {
                            case DataRowState.Added:
                                rowCode = "<td class=\"{0}\"><i>{1}</i></td>";
                                break;
                            case DataRowState.Deleted:
                                rowCode = "<td class=\"{0}\"><del>{1}</del></td>";
                                break;
                            case DataRowState.Modified:
                            case DataRowState.Unchanged:
                            case DataRowState.Detached:
                            default:
                                break;
                        }

                        for (int ii = 0; ii < tableInfo.Count; ii++)
                        {
                            string columnName = tableInfo[ii].Name;
                            object columnValue = er.DataRow[columnName];
                            htmlTable.Append(string.Format(rowCode,
                                er.Cells[ii].State,
                                (columnValue == DBNull.Value ?
                                "(null)" : (columnValue.ToString() == string.Empty ?
                                "&nbsp;" : columnValue))));
                        }
                        htmlTable.Append("</tr>");
                    }
                    htmlTable.Append("</table>");
                    sw.WriteLine(htmlTable);
                }
            }
            sw.WriteLine("</body>");
            sw.WriteLine("</html>");
            sw.Close();

            // open the report after import
            Process.Start("iexplore.exe", "-new " + Path.GetFullPath(reportFile));
        }

        /// <summary>
        /// Commit the changed data of dataset to database.
        /// </summary>
        /// <param name="dbSet">DataSet object</param>
        /// <param name="cmdBuilder">CommandBuilder object</param>
        private void UpdateDatabase(DataSet dbSet, OdbcCommandBuilder cmdBuilder)
        {
            Trace.WriteLine("UpdateDatabase...");

            string tableName = null;
            foreach (TableLevel tbLevel in m_sortedTables)
            {
                m_progressForm.PerformStep();
                m_progressForm.Update();
                tableName = RDBResource.GetTableName(tbLevel.TableId);

                DataTable dbTable = dbSet.Tables[tableName];
                if (dbTable == null) continue;

                RefreshCmdBuilder(cmdBuilder, tableName);

                DataRow[] updateRows = dbTable.Select(null, null,
                    DataViewRowState.Added | DataViewRowState.ModifiedOriginal | DataViewRowState.ModifiedCurrent | DataViewRowState.Deleted);
                // We just update Added and Modified rows of database.
                if (updateRows.Length == 0) continue;

                // Begin a transaction.
                OdbcTransaction trans = cmdBuilder.DataAdapter.SelectCommand.Connection.BeginTransaction();
                cmdBuilder.DataAdapter.SelectCommand.Transaction = trans;
                try
                {
                    string assemlyColumnName = RDBResource.GetColumnName("ColN_BP_UNIFORMAT_CODE");
                    if (dbTable.Columns.Contains(assemlyColumnName))
                    {
                        foreach (DataRow row in updateRows)
                        {
                            row[assemlyColumnName] = DBNull.Value;
                        }
                    }

                    cmdBuilder.DataAdapter.Update(updateRows);
                    trans.Commit();
                }
                catch (Exception exp)
                {
                    // Rollback the transaction.
                    Trace.WriteLine("[" + tableName +"]" + "UpdateDatabase Exception: " + exp.Message);
                    trans.Rollback();
                }
            }

            TraceTime();
        }

        /// <summary>
        /// Transfer data between Revit and Database. 
        /// </summary>
        /// <param name="dbCommand">OdbcCommand object, used to operate database</param>
        /// <param name="export">true if export data from Revit to database, vice versa</param>
        private void TransferData(OdbcCommand dbCommand, bool export)
        {
            Trace.WriteLine("Transfer Data...");
            OdbcCommandBuilder cmdBuilder = null;
            DataSet dataSet = BuildDataSet(dbCommand, out cmdBuilder);

            if (export)
            {
                // export all data, if record exists, update it, otherwise add a new one
                Export(dataSet);
                // commit all changes to database
                UpdateDatabase(dataSet, cmdBuilder);
                // remove records that there is no corresponding object exists in Revit.
                ClearGarbageRows(dataSet, cmdBuilder);
                m_progressForm.Close();
            }
            else
            {
                m_progressForm.Close();
                // if select import, prompt the edit form for user to edit data
                DbEditForm editForm = new DbEditForm(dataSet, TableInfoSet, m_app);

                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    BeginProgress(TableInfoSet.Values.Count + m_sortedTables.Count);
                    m_progressForm.ProgressBarLableTitle = RDBResource.GetString("Progress_ImportData");
                    m_progressForm.Update();
                    Import(dataSet);
                    UpdateDatabase(dataSet, cmdBuilder);
                    m_progressForm.Close();
                }
            }
        }

        /// <summary>
        /// Delete records that have no relation with Revit model.
        /// </summary>
        /// <param name="dataSet">DataSet contains all tables</param>
        /// <param name="cmdBuilder">CommandBuilder</param>
        private void ClearGarbageRows(DataSet dataSet, OdbcCommandBuilder cmdBuilder)
        {
            Trace.WriteLine("Clear Garbage Rows...");
            // delete records use reverse order
            for (
                int reverseIndex = m_sortedTables.Count - 1;
                reverseIndex >= 0;
                reverseIndex--)
            {
                TableLevel tbLevel = m_sortedTables[reverseIndex];
                TableInfo tbInfo = TableInfoSet[tbLevel.TableId];
                APIObjectList apiObjects = tbInfo.ObjectList;

                // get all records that there is no related object exists in Revit
                List<DataRow> garbageRows = apiObjects.CollectGarbageRows();

                if (garbageRows.Count > 0)
                {
                    // delete these rows
                    foreach (DataRow row in garbageRows)
                    {
                        row.Delete();
                    }
                    RefreshCmdBuilder(cmdBuilder, tbInfo.Name);

                    OdbcTransaction trans = cmdBuilder.DataAdapter.SelectCommand.Connection.BeginTransaction();
                    cmdBuilder.DataAdapter.SelectCommand.Transaction = trans;

                    try
                    {
                        cmdBuilder.DataAdapter.Update(garbageRows.ToArray());
                        trans.Commit();
                    }
                    catch (Exception exp)
                    {
                        Trace.WriteLine("ClearGarbageRows Exception: " + exp.Message);
                        trans.Rollback();
                    }
                }
            }

            m_progressForm.ProgressStep = tmpRatioForClearGarbage;
            m_progressForm.PerformStep();
            TraceTime();
        }

        /// <summary>
        /// Output span time to Trace
        /// </summary>
        private void TraceTime()
        {
            Trace.WriteLine("Time: [" + (DateTime.Now - m_now).ToString() + "]");
            m_now = DateTime.Now;
        }

        /// <summary>
        /// Categorize all elements in RevitAPI document to ObjectList.
        /// e.g. Symbols will be added into SymbolList.
        /// Instances will be added into InstanceList.
        /// </summary>
        private void CategorizeElements()
        {


            Trace.WriteLine("Categorize Elements ...");

            // get element iterator
            ElementIterator eit = m_doc.Elements;
            eit.Reset();

            int elemCount = 0;
            while (eit.MoveNext())
            {
                ++elemCount;
            }

            eit.Reset();
            while (eit.MoveNext())
            {
                m_progressForm.PerformStep();
                m_progressForm.Update();

                // using category and element type to categorize elements
                Element element = eit.Current as Element;
                BuiltInCategory categoryId = ElementList.GetCategoryId(element);

                ElementList relatedElementList = null;
                List<string> tableIds = ElementList.GenerateResourceIdsForTableName(element, categoryId);
                foreach (string tableId in tableIds)
                {
                    if (TableInfoSet.ContainsKey(tableId))
                    {
                        relatedElementList = TableInfoSet[tableId].ObjectList as ElementList;
                        relatedElementList.Append(element);
                    }
                }

                // append to custom tables
                foreach (string tbId in TableInfoSet.CustomTableIds)
                {
                    relatedElementList = TableInfoSet[tbId].ObjectList as ElementList;
                    if (relatedElementList != null)
                        relatedElementList.TryAppend(element);
                }
            }

            eit.Dispose();
            TraceTime();
        }

        /// <summary>
        /// Initialize TableInfoSet and TableLevelSet.
        /// </summary>
        private void InitializeTableSchemaAndMetaData()
        {
            Trace.WriteLine("Initialize Tables....");
            TableInfoSet = new TableInfoSet(m_doc);
            m_sortedTables = new TableLevelSet(TableInfoSet);
        }

        /// <summary>
        /// Release database connection.
        /// </summary>
        /// <param name="odbcConnection">OdbcConnection object to be released</param>
        private void CloseConnection(OdbcConnection odbcConnection)
        {
            //check whether the connection is opened. if not, close it.
            if (null != odbcConnection && odbcConnection.State != ConnectionState.Closed)
            {
                odbcConnection.Close();
            }
        }

        /// <summary>
        /// Open database connection with specified connection string.
        /// </summary>
        /// <param name="connStr">Connection string</param>
        /// <returns>OdbcConnection object</returns>
        private OdbcConnection OpenConnection(string connStr)
        {
            OdbcConnection dbConnection = new OdbcConnection();
            dbConnection.ConnectionString = connStr;
            try
            {
                dbConnection.Open();
            }
            catch (Exception)
            {
                CloseConnection(dbConnection);
                return null;
            }
            return dbConnection;
        }

        #endregion
   };
}
