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

namespace Revit.SDK.Samples.RDBLink.CS
{
    /// <summary>
    /// Preserves error information of a table after importing
    /// </summary>
    public class ErrorTable
    {
        #region Fields
        /// <summary>
        /// The table name
        /// </summary>
        string m_tableName;
        /// <summary>
        /// The DataTable
        /// </summary>
        DataTable m_dataTable;
        /// <summary>
        /// Indicates if there are any changes made to Revit
        /// </summary>
        bool m_hasChange;
        /// <summary>
        /// Indicates whether the table supports importing or not
        /// </summary>
        bool m_notSupportImporting;
        /// <summary>
        /// Table resource id
        /// </summary>
        string m_tableId;

        /// <summary>
        /// ErrorRow list
        /// </summary>
        List<ErrorRow> m_rows; 
        #endregion

        #region Properties

        /// <summary>
        /// Gets ErrorRow list
        /// </summary>
        public List<ErrorRow> Rows
        {
            get { return m_rows; }
            //set { m_rows = value; }
        }

        /// <summary>
        /// Gets or sets whether there are any changes made to Revit
        /// </summary>
        public bool HasChange
        {
            get { return m_hasChange; }
            set { m_hasChange = value; }
        }

        /// <summary>
        /// Gets or sets whether the table supports importing or not
        /// </summary>
        public bool NotSupportImporting
        {
            get { return m_notSupportImporting; }
            set { m_notSupportImporting = value; }
        }

        /// <summary>
        /// Gets the corresponding DataTable
        /// </summary>
        public DataTable DataTable
        {
            get { return m_dataTable; }
            //set { m_dataTable = value; }
        }

        /// <summary>
        /// Gets the table name
        /// </summary>
        public string TableName
        {
            get { return m_tableName; }
            //set { m_tableName = value; }
        }

        /// <summary>
        /// Gets table resource id
        /// </summary>
        public string TableId
        {
            get { return m_tableId; }
            /*set { m_tableKey = value; }*/
        }
        #endregion

        #region Constructors

        /// <summary>
        /// Constructor, initialize variables
        /// </summary>
        /// <param name="tableId">table name resource id</param>
        /// <param name="tableName">table name</param>
        /// <param name="dataTable">DataTable</param>
        public ErrorTable(string tableId, string tableName, DataTable dataTable)
        {
            m_rows = new List<ErrorRow>();
            m_tableName = tableName;
            m_tableId = tableId;
            m_dataTable = dataTable;
        } 
        #endregion
    };

    /// <summary>
    /// Preserves error information of a row after importing
    /// </summary>
    public class ErrorRow
    {
        #region Fields
        /// <summary>
        /// Corresponding DataRow
        /// </summary>
        DataRow m_dataRow;
        /// <summary>
        /// ErrorCell list
        /// </summary>
        List<ErrorCell> m_cells;
        /// <summary>
        /// Indicates if there are any changes made to Revit related with this row
        /// </summary>
        bool m_hasChange;
        /// <summary>
        /// Row state
        /// </summary>
        DataRowState m_state; 
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the state of the ErrorRow
        /// </summary>
        public DataRowState State
        {
            get { return m_state; }
            set { m_state = value; }
        }

        /// <summary>
        /// Gets or sets whether there are any changes made to Revit related with this row
        /// </summary>
        public bool HasChange
        {
            get { return m_hasChange; }
            set { m_hasChange = value; }
        }

        /// <summary>
        /// Gets the corresponding DataRow
        /// </summary>
        public DataRow DataRow
        {
            get { return m_dataRow; }
            //set { m_dataRow = value; }
        }
        /// <summary>
        /// Gets ErrorCell list
        /// </summary>
        public List<ErrorCell> Cells
        {
            get { return m_cells; }
            //set { m_cells = value; }
        } 
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor, initialize variables
        /// </summary>
        /// <param name="dataRow">The corresponding DataRow</param>
        public ErrorRow(DataRow dataRow)
        {
            m_dataRow = dataRow;
            m_cells = new List<ErrorCell>();
        } 
        #endregion
    };

    /// <summary>
    /// Preserves error information of a cell after importing
    /// </summary>
    public class ErrorCell
    {
        #region Fields
        /// <summary>
        /// Column name
        /// </summary>
        string m_columnName;
        /// <summary>
        /// Importing result
        /// </summary>
        UpdateResult m_state; 
        #endregion

        #region Properties
        /// <summary>
        /// Gets the column name
        /// </summary>
        public string ColumnName
        {
            get { return m_columnName; }
            //set { m_columnName = value; }
        }
        /// <summary>
        /// Gets or sets the result state
        /// </summary>
        public UpdateResult State
        {
            get { return m_state; }
            set { m_state = value; }
        } 
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="columnName">Column name</param>
        /// <param name="state">Importing result</param>
        public ErrorCell(string columnName, UpdateResult state)
        {
            m_columnName = columnName;
            m_state = state;
        } 
        #endregion
    };

    /// <summary>
    /// Indicates single cell importing result.
    /// </summary>
    public enum UpdateResult
    {
        /// <summary>
        /// An unknown result
        /// </summary>
        Unknown,
        /// <summary>
        /// Can't deal with AssemblyCode
        /// </summary>
        AssemblyCode,
        /// <summary>
        /// Updated successfully
        /// </summary>
        Success,
        /// <summary>
        /// Updated failed
        /// </summary>
        Failed,
        /// <summary>
        /// Updated failed because the corresponding field is readonly
        /// </summary>
        ReadOnlyFailed,
        /// <summary>
        /// Updated failed because the corresponding parameter is null
        /// </summary>
        ParameterNull,        
        /// <summary>
        /// Updated failed because there is an exception
        /// </summary>
        Exception,
        /// <summary>
        /// Nothing happened when import, because the value of the corresponding 
        /// parameter and the cell are equal
        /// </summary>
        Equals
    }
}
