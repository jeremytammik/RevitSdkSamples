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
using System.Data.Odbc;
using Autodesk.Revit;
using Autodesk.Revit.Collections;
using Autodesk.Revit.Parameters;
using RevitApplication = Autodesk.Revit.Application;

namespace Revit.SDK.Samples.RDBLink.CS
{
    /// <summary>
    /// Provides user interface to edit database before importing
    /// </summary>
    public partial class DbEditForm : Form
    {
        #region Fields
        
        /// <summary>
        /// Revit document
        /// </summary>
        Document m_revitDoc;

        /// <summary>
        /// Revit application
        /// </summary>
        RevitApplication m_revitApp;

        /// <summary>
        /// DataSet contains all tables in database
        /// </summary>
        DataSet m_dataSet;

        /// <summary>
        /// Database command object, used to execute SQL statement.
        /// </summary>
        IDbCommand m_command;

        /// <summary>
        /// Latest new row of DataGridView, indicates last new row added to DataGridView.
        /// </summary>
        DataGridViewRow m_newDataGridViewRow = null;

        /// <summary>
        /// TableInfoSet contains all table schema information
        /// </summary>
        TableInfoSet m_tableInfoSet;

        /// <summary>
        /// BindingSource used to bind with DataGridView
        /// </summary>
        BindingSource m_bindingSource;

        /// <summary>
        /// A cell style to indicate read-only state
        /// </summary>
        DataGridViewCellStyle m_readOnlyStyle = new DataGridViewCellStyle();

        /// <summary>
        /// A cell style to indicate records that is user added but failed to import
        /// </summary>
        DataGridViewCellStyle m_toBeDeleteStyle = new DataGridViewCellStyle();

        /// <summary>
        /// A cell style to indicate row header
        /// </summary>
        DataGridViewCellStyle m_headerCellStyle = new DataGridViewCellStyle();

        /// <summary>
        /// Used to automatically set a value to "Id" column for new user added row
        /// </summary>
        int m_autoNegativeNewId;


        /// <summary>
        /// Resource id of "Id" column
        /// </summary>
        const string IdColumnResId = "ColN_BP_ID_PARAM";
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor, initialize all controls
        /// </summary>
        private DbEditForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialize extra variables
        /// </summary>
        /// <param name="dataSet">DataSet which contains all tables in database</param>
        /// <param name="tableInfoSet">TableInfoSet which contains all table schema information</param>
        /// <param name="revitApp">Revit Application</param>
        public DbEditForm(DataSet dataSet, TableInfoSet tableInfoSet, RevitApplication revitApp)
            : this()
        {
            m_dataSet = dataSet;
            m_command = Command.OdbcConnection.CreateCommand();
            m_tableInfoSet = tableInfoSet;
            m_revitApp = revitApp;
            m_revitDoc = m_revitApp.ActiveDocument;
            m_bindingSource = new BindingSource();
            m_bindingSource.DataSource = m_dataSet;
            tableDataGridView.AutoGenerateColumns = false;
            tableDataGridView.VirtualMode = true;
            tableDataGridView.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;

            CreatePrimaryKeyConstraints(dataSet);

            m_readOnlyStyle.ForeColor = System.Drawing.SystemColors.GrayText;
            m_headerCellStyle.BackColor = System.Drawing.Color.Pink;
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// End DataGridView edit state and close the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void okButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.BindingContext[this.tableDataGridView.DataSource].EndCurrentEdit();
            }
            catch (NoNullAllowedException ex)
            {
                MessageBox.Show(ex.Message);
            }

            this.Close();
        }

        /// <summary>
        /// Populate all table names into list box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DbEditForm_Load(object sender, EventArgs e)
        {
            InitializeListBox();
        }

        /// <summary>
        /// Update the content of DataGridView with the selected table in list box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitializeToolstripStatus();
            UpdateDataGridView();
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
        /// Show the error when it occurs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tableDataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show(String.Format("Error happened : row[{0}], column[{1}]; Error context: {2}", e.RowIndex, e.ColumnIndex, e.Context));
        }

        /// <summary>
        /// Set the id cell of the new user added row to a negative unique number.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tableDataGridView_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            DataGridViewCell cell;
            try
            {
                int i = e.Row.Index;
                cell = tableDataGridView.Rows[i - 1].Cells[IdColumnResId];
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.ToString());
                return;
            }
            cell.Value = --m_autoNegativeNewId;
        }

        /// <summary>
        /// Set the id cell to read only style when new row is needed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tableDataGridView_NewRowNeeded(object sender, DataGridViewRowEventArgs e)
        {
            DataGridViewCell cell;
            try
            {
                cell = e.Row.Cells[IdColumnResId];
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.ToString());
                return;
            }
            cell.ReadOnly = true;
            cell.Style = m_readOnlyStyle;
        }


        /// <summary>
        /// Set the id cell to read only style when a row is added.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tableDataGridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            m_newDataGridViewRow = tableDataGridView.Rows[e.RowIndex];
            m_newDataGridViewRow.Tag = RowTag.ANewRow;
        }

        /// <summary>
        /// Copy content of selected rows to clipboard.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void copyToolStripButton_Click(object sender, EventArgs e)
        {
            Clipboard.Clear();
            Clipboard.SetDataObject(tableDataGridView.GetClipboardContent());
        }

        /// <summary>
        /// Cut the selected rows.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cutToolStripButton_Click(object sender, EventArgs e)
        {
            //Copy to clip board
            Clipboard.Clear();
            Clipboard.SetDataObject(tableDataGridView.GetClipboardContent());

            //remove the cut rows in dataGridView
            foreach (DataGridViewRow row in tableDataGridView.SelectedRows)
            {
                if (!row.IsNewRow)
                    tableDataGridView.Rows.Remove(row);
            }
        }

        /// <summary>
        /// Paste to selected rows with the content in clipboard.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pasteToolStripButton_Click(object sender, EventArgs e)
        {
            // Paste on a dataGridView from the Clipboard a string copied from say an Excel sheet or database(Access).
            // Clipboard text format as below :
            // First row string (Header text): "\tId\tKeynote\tModel\tManufacturer\tTypeComments\tURL\tDescription\tAssemblyCode\tFamilyName\tTypeName\tTypeMark\tFireRating\tCost\tWidth".

            // Other rows string (Data text):  "\t83171\t\t\t\t\t\t\t\tBasic Wall\tExterior - CMU on Mtl. Stud\t\t\t0\t1.4895833333333333"
            if (tableDataGridView.SelectedRows.Count == 0)
                return;
            string pasteText = Clipboard.GetText();

            string[] splitter = { "\r\n" }; //splitter for rows

            string[] splitterC = { "\t" }; //splitter for cells

            //get rows
            string[] rows = pasteText.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
            int rowCount = rows.Length - 1;

            //get number of items per row
            string[] cells = rows[0].Split(splitterC, StringSplitOptions.None);
            bool isBeginTab = cells[0].Equals("");

            int selectCount = tableDataGridView.SelectedRows.Count;

            int updateCount = selectCount < rowCount ? selectCount : rowCount;

            // Use paste text to update the selection rows.
            for (int i = 0; i < updateCount; i++)
            {
                // Get the selection row
                DataGridViewRow dgvRow = tableDataGridView.SelectedRows[i];
                String[] rowValues = rows[i + 1].Split(splitterC, StringSplitOptions.None);

                if (isBeginTab) // start with '\t'
                {
                    String[] newRow = new string[rowValues.Length - 1];
                    System.Array.Copy(rowValues, 1, newRow, 0, newRow.Length);
                    rowValues = newRow;
                }

                //paste the rows on clipboard, new a row in dataTable and update dataGridView 
                //when the selected row is new row
                if (dgvRow.IsNewRow)
                {
                    tableDataGridView.CancelEdit();
                    DataTable dt = m_dataSet.Tables[m_bindingSource.DataMember.ToString()];
                    DataRow dr = dt.NewRow();
                    for (int cellIndex = 0; cellIndex < dr.ItemArray.Length; cellIndex++)
                    {
                        if (rowValues[cellIndex] != "")
                        {
                            dr[cellIndex] = rowValues[cellIndex];
                        }
                        else
                        {
                            dr[cellIndex] = DBNull.Value;
                        }
                        if (dt.Columns[cellIndex].ColumnName == RDBResource.GetColumnName(IdColumnResId))
                        {
                            //set the the id cell to a negative unique number
                            dr[cellIndex] = --m_autoNegativeNewId;
                        }
                    }
                    dt.Rows.Add(dr);
                    UpdateNewRowStatus();
                }
                else
                {
                    //cover the rows with the copy rows on clipboard when the selected rows aren't new row. 
                    for (int j = 0; j < dgvRow.Cells.Count; j++)
                    {
                        if (dgvRow.Cells[j].OwningColumn.Name == IdColumnResId)
                        {
                            if (dgvRow.Cells[j].Value.ToString() != rowValues[j])
                            {
                                dgvRow.Cells[j].Value = --m_autoNegativeNewId;
                            }
                        }
                        else
                        {
                            if (rowValues[j] != "")
                            {
                                dgvRow.Cells[j].Value = rowValues[j];
                            }
                            else
                            {
                                dgvRow.Cells[j].Value = DBNull.Value;
                            }
                        }
                    }
                }
            }

            // if the rows count greater than selected rows count, paste the clipboard text
            // to new rows.
            if (rowCount > selectCount)
            {
                for (int k = updateCount; k < rowCount; k++)
                {
                    String[] rowValues = rows[k + 1].Split(splitterC, StringSplitOptions.None);
                    DataTable dt = m_dataSet.Tables[m_bindingSource.DataMember.ToString()];
                    DataRow dr = dt.NewRow();

                    if (isBeginTab)
                    {
                        String[] newRow = new string[rowValues.Length - 1];
                        System.Array.Copy(rowValues, 1, newRow, 0, newRow.Length);
                        rowValues = newRow;
                    }

                    for (int cellIndex = 0; cellIndex < dr.ItemArray.Length; cellIndex++)
                    {
                        if (rowValues[cellIndex] != "")
                        {
                            dr[cellIndex] = rowValues[cellIndex];
                        }
                        else
                        {
                            dr[cellIndex] = DBNull.Value;
                        }
                        if (dt.Columns[cellIndex].ColumnName == RDBResource.GetColumnName(IdColumnResId))
                        {
                            dr[cellIndex] = --m_autoNegativeNewId;
                        }
                    }
                    dt.Rows.Add(dr);

                    UpdateNewRowStatus();
                }
            }

            tableDataGridView.Update();
        }

        /// <summary>
        /// create a row in the dataTable and update the DataGridView.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newRecordToolStripButton_Click(object sender, EventArgs e)
        {

            if (tableDataGridView.CurrentRow.IsNewRow)
            {
                tableDataGridView.CancelEdit();
            }

            DataTable dt = m_dataSet.Tables[m_bindingSource.DataMember.ToString()];
            DataRow dr = dt.NewRow();
            string idName = RDBResource.GetColumnName(IdColumnResId);
            if (dt.Columns.Contains(idName))
            {
                dr[idName] = --m_autoNegativeNewId;
                dt.Rows.Add(dr);
                UpdateNewRowStatus();
                tableDataGridView.Update();
            }
        }

        /// <summary>
        /// Remove the selected rows in DataGridView.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteToolStripButton_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in tableDataGridView.SelectedRows)
            {
                if (!row.IsNewRow)
                    tableDataGridView.Rows.Remove(row);
            }
        }

        /// <summary>
        /// Set status of ToolStripButtons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tableDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            bool selectionIsNew = tableDataGridView.SelectedRows.Count > 0;
            foreach (DataGridViewRow row in tableDataGridView.SelectedRows)
            {
                RowTag tag = row.Tag as RowTag;
                if (tag != null)
                {
                    if (tag.Equals(RowTag.AnOldRowExist))
                    {
                        selectionIsNew = false;
                    }
                }
            }
            
            // user selected some rows, copying is allowed
            copyToolStripButton.Enabled = tableDataGridView.SelectedRows.Count > 0;
            
            // if the selection are all new records, it is allowed to paste, cut and delete
            if (selectionIsNew)
            {
                pasteToolStripButton.Enabled = Clipboard.ContainsText();
                cutToolStripButton.Enabled = true;
                deleteToolStripButton.Enabled = true;
            }
            else
            {
                pasteToolStripButton.Enabled = false;
                cutToolStripButton.Enabled = false;
                deleteToolStripButton.Enabled = false;
            }
        }

        /// <summary>
        /// Commits and ends the edit operation when the form is closed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DbEditForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            tableDataGridView.EndEdit();
        }

        /// <summary>
        /// List custom parameters.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void parametersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (ParameterListForm parameterListForm = new ParameterListForm(m_revitApp, m_dataSet))
            {
                if(parameterListForm.ShowDialog() == DialogResult.OK)
                {
                    UpdateDataGridView();
                }
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Add primary key constraints to the data set
        /// </summary>
        /// <param name="dataSet"></param>
        private void CreatePrimaryKeyConstraints(DataSet dataSet)
        {
            foreach (TableInfo tbInfo in m_tableInfoSet.Values)
            {
                DataTable dbTable = dataSet.Tables[tbInfo.Name];
                if (dbTable == null)
                    continue;

                //Build primary keys
                PrimaryKeys pks = tbInfo.PrimaryKeys;
                DataColumn[] keys = new DataColumn[pks.Count];
                for (int i = 0; i < pks.Count; ++i)
                {
                    keys[i] = dbTable.Columns[tbInfo.PrimaryKeys.ColumnName(i)];
                }
                dbTable.PrimaryKey = keys;
            }
        }

        /// <summary>
        /// Add all table names to list box
        /// </summary>
        private void InitializeListBox()
        {
            // use TableInfo list as list box data source
            List<TableInfo> tableInfos = new List<TableInfo>();
            DataTable dbTable = null;
            foreach (TableInfo tableInfo in m_tableInfoSet.Values)
            {
                dbTable = m_dataSet.Tables[tableInfo.Name];
                if (dbTable == null) continue;
                tableInfos.Add(tableInfo);
            }
            listBox.Sorted = true;
            listBox.DataSource = tableInfos;
            listBox.DisplayMember = "Name";
        }

        /// <summary>
        /// Update DataGridView with the selected table in the list box
        /// </summary>
        private void UpdateDataGridView()
        {
            m_autoNegativeNewId = 0;

            //get selection
            TableInfo tableInfo = listBox.SelectedItem as TableInfo;

            //clear all columns
            tableDataGridView.Columns.Clear();

            // bind source to data grid view
            m_bindingSource.DataMember = tableInfo.Name;
            tableDataGridView.DataSource = m_bindingSource;

            if (tableInfo != null)
            {
                //put all keys to a dictionary
                Dictionary<string, ForeignKey> fkCols = new Dictionary<string, ForeignKey>();
                foreach (ForeignKey fk in tableInfo.ForeignKeys)
                {
                    fkCols.Add(fk.ColumnId, fk);
                }
                //deal with each column, verify which type of column should be used
                foreach (ColumnInfo ci in tableInfo)
                {
                    DataGridViewColumn dgvc;
                    string columnName = ci.Name;

                    //if it is foreign key, ComboBoxColumn is used
                    if (fkCols.ContainsKey(ci.ColumnId))
                    {
                        DataGridViewComboBoxColumn tmp = new DataGridViewComboBoxColumn();
                        tmp.DataSource = GetDataTable(fkCols[ci.ColumnId].RefTableName);
                        tmp.DisplayMember = fkCols[ci.ColumnId].RefColumnName;
                        dgvc = tmp;
                    }
                    //otherwise, TextBoxColumn is used
                    else
                    {
                        DataGridViewTextBoxColumn tmp = new DataGridViewTextBoxColumn();
                        dgvc = tmp;
                    }

                    //set column properties
                    dgvc.Name = ci.ColumnId;
                    dgvc.SortMode = DataGridViewColumnSortMode.NotSortable;
                    dgvc.DataPropertyName = columnName;
                    dgvc.HeaderText = columnName;

                    //add new column to DataGridView
                    tableDataGridView.Columns.Add(dgvc);
                }

            }

            SetDataGridViewTags(tableInfo);
            SetDataGridViewAppearence();
            InitializeAutoNegativeNewId();
        }

        /// <summary>
        /// Initialize value of m_autoNegativeNewId
        /// </summary>
        private void InitializeAutoNegativeNewId()
        {
            if ((tableDataGridView.Columns.Contains(IdColumnResId)) && (tableDataGridView.Rows.Count != 0))
            {
                foreach (DataGridViewRow row in tableDataGridView.Rows)
                {
                    if ((!row.IsNewRow) && (m_autoNegativeNewId > (int)row.Cells[IdColumnResId].Value))
                        m_autoNegativeNewId = (int)row.Cells[IdColumnResId].Value;
                }
            }
        }

        /// <summary>
        /// Set primary status of tool strip
        /// </summary>
        private void InitializeToolstripStatus()
        {
            Clipboard.Clear();
            cutToolStripButton.Enabled = false;
            copyToolStripButton.Enabled = false;
            pasteToolStripButton.Enabled = false;
            newRecordToolStripButton.Enabled = false;
            deleteToolStripButton.Enabled = false;
        }

        /// <summary>
        /// Set all rows' styles or cells' styles
        /// </summary>
        private void SetDataGridViewAppearence()
        {
            // get table tag
            TableTag tableTag = tableDataGridView.Tag as TableTag;
            if (tableTag != null)
            {
                foreach (DataGridViewRow row in tableDataGridView.Rows)
                {
                    // get row tag
                    RowTag rowTag = row.Tag as RowTag;
                    // if this row has no corresponding object in Revit
                    if (rowTag != null && rowTag.NotExist)
                    {
                        // if elements can't be create via API, e.g. walls
                        // this row will import failed, because it can neither
                        // be created nor be updated.
                        if (!tableTag.IsAllowCreate)
                        {
                            row.DefaultCellStyle = m_toBeDeleteStyle;
                        }
                        // if elements can be create via API, e.g. wall types
                        // this row can be created and its Id column will be updated.
                        else
                        {
                            // set primary columns to read-only
                            SetAllCellsState(row);
                        }
                    }
                    // if the row has corresponding object in Revit
                    else
                    {
                        // if it is custom table e.g. Categories
                        if (tableTag.IsCustomTable)
                        {
                            // set its read-only to true and text to gray
                            row.ReadOnly = true;
                            row.DefaultCellStyle = m_readOnlyStyle;
                        }
                        // if it is not custom table e.g. walls
                        else
                        {
                            // set read-only fields to read-only
                            SetAllCellsState(row);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Set each cell's style of a row
        /// </summary>
        /// <param name="row">DataGridViewRow to set each cell</param>
        private void SetAllCellsState(DataGridViewRow row)
        {
            foreach (DataGridViewCell cell in row.Cells)
            {
                CellTag cellTag = cell.Tag as CellTag;
                if (cellTag != null)
                {
                    // if cell tag is read-only, set cell style to read-only
                    if (cellTag.IsReadOnly)
                    {
                        cell.ReadOnly = true;
                        cell.Style = m_readOnlyStyle;
                    }
                }
            }
        }

        /// <summary>
        /// Detect and preserve all rows or cells' status in DataGridView
        /// </summary>
        /// <param name="tableInfo"></param>
        private void SetDataGridViewTags(TableInfo tableInfo)
        {
            //Allow user to add rows if support element creation
            if (tableInfo.ObjectList.SupportCreate)
            {
                tableDataGridView.AllowUserToAddRows = true;
                newRecordToolStripButton.Enabled = true;
            }
            else
            {
                tableDataGridView.AllowUserToAddRows = false;
                newRecordToolStripButton.Enabled = false;
            }

            // default is allow-create
            tableDataGridView.Tag = tableInfo.ObjectList.SupportCreate
                ? TableTag.AllowCreate : TableTag.NotAllowCreate;

            // it is a custom table, set tag to custom table and not allow to create
            if (m_tableInfoSet.CustomTableIds.Contains(tableInfo.TableId))
            {
                tableDataGridView.Tag = TableTag.CustomTable;
            }

            // get DataTable
            DataTable dt = m_dataSet.Tables[tableInfo.Name];

            // iterate all rows to detect whether there are related object exist in Revit
            for (int ii = 0; ii < dt.Rows.Count; ii++)
            {
                DataRow dr = dt.Rows[ii];
                APIObject apiObject = tableInfo.ObjectList.FindCorrespondingObject(dr);
                Element element = apiObject as Element;

                DataGridViewRow row = tableDataGridView.Rows[ii];

                // we can find the object
                if (apiObject != null)
                {
                    row.Tag = RowTag.AnOldRowExist;

                    // the object is an element
                    if (element != null)
                    {
                        for (int jj = 0; jj < tableInfo.Count; jj++)
                        {
                            ColumnInfo ci = tableInfo[jj];
                            DataGridViewCell cell = row.Cells[jj];

                            Parameter par = null;
                            // if it is invalid, maybe it is a shared or project parameter
                            if(ci.BuiltInParameter == BuiltInParameter.INVALID)
                            {
                                par = ElementList.GetParameterByDefinitionName(element, ci.Name);
                            }
                            else
                            {
                                par = element.get_Parameter(ci.BuiltInParameter);
                            }

                            // if the column is null or read-only, set cell tag to indicate it is read-only
                            if (par == null || par.IsReadOnly)
                            {
                                cell.Tag = CellTag.ReadOnly;
                            }
                        }
                    }
                }
                // if we can not find the object
                else
                {
                    row.Tag = RowTag.ANewRow;

                    // if this table support element creation e.g. WallTypes
                    // set its primary columns to read-only
                    if (tableInfo.ObjectList.SupportCreate)
                    {
                        for (int jj = 0; jj < tableInfo.Count; jj++)
                        {
                            ColumnInfo ci = tableInfo[jj];
                            DataGridViewCell cell = tableDataGridView.Rows[ii].Cells[jj];
                            // all primary columns can't be modified, so set it to read-only
                            if (tableInfo.PrimaryKeys.Contains(ci.ColumnId))
                            {
                                cell.Tag = CellTag.ReadOnly;
                                continue;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Set id column style to read-only
        /// </summary>
        private void UpdateNewRowStatus()
        {
            if (tableDataGridView.Columns.Contains(IdColumnResId))
            {
                DataGridViewCell cell = m_newDataGridViewRow.Cells[IdColumnResId];
                cell.Tag = CellTag.ReadOnly;
                cell.ReadOnly = true;
                cell.Style = m_readOnlyStyle;
            }
        }

        /// <summary>
        /// Get a DataTable from DataSet
        /// </summary>
        /// <param name="tableName">Table name to get</param>
        /// <returns>DataTable</returns>
        private DataTable GetDataTable(string tableName)
        {
            DataTable dataTable = m_dataSet.Tables[tableName];
            return dataTable;
        }
        #endregion
    }
}
