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
using System.Text;
using Autodesk.Revit;
using System.Data;
using System.Collections.ObjectModel;

namespace Revit.SDK.Samples.RDBLink.CS
{
    /// <summary>
    /// Provides generic methods to export and import data.
    /// </summary>
    public abstract class APIObjectList
    {
        #region Fields
        /// <summary>
        /// Revit Document object
        /// </summary>
        private static Document s_activeDocument;

        /// <summary>
        /// Extra columns which do not exist in the original database.
        /// </summary>
        private List<string> m_extraColumns = new List<string>();

        /// <summary>
        /// DataTable which represents the real table in database.
        /// </summary>
        private DataTable m_dataTable;

        /// <summary>
        /// Table name.
        /// </summary>
        private string m_tableName;

        /// <summary>
        /// DataRow collection which will be deleted from data table at the end of importing.
        /// </summary>
        private List<DataRow> m_garbageRows = new List<DataRow>();

        /// <summary>
        /// DataRow collection which will be updated at the end of importing.
        /// </summary>
        private Dictionary<DataRow, DataRow> m_pendingRows = new Dictionary<DataRow, DataRow>();

        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the table name.
        /// </summary>
        public string TableName
        {
            get { return m_tableName; }
            set { m_tableName = value; }
        }

        /// <summary>
        /// Gets or sets DataTable.
        /// </summary>
        public DataTable DataTable
        {
            get { return m_dataTable; }
            set { m_dataTable = value; }
        }

        /// <summary>
        /// Gets active document
        /// </summary>
        protected static Document ActiveDocument
        {
            get { return APIObjectList.s_activeDocument; }
        }

        /// <summary>
        /// Gets extra column list.
        /// </summary>
        protected List<string> ExtraColumns
        {
            get { return m_extraColumns; }
        }

        /// <summary>
        /// Gets garbage rows which will be deleted from data table at the end of importing.
        /// </summary>
        protected List<DataRow> GarbageRows
        {
            get { return m_garbageRows; }
        }

        /// <summary>
        /// Gets pending rows which will be updated at the end of importing.
        /// </summary>
        protected Dictionary<DataRow, DataRow> PendingRows
        {
            get { return m_pendingRows; }
        } 
        #endregion

        #region Methods
        /// <summary>
        /// Sets Revit Document object.
        /// </summary>
        public static void SetRevitDocument(Document activeDocument)
        {
            s_activeDocument = activeDocument;
        }

        /// <summary>
        /// Exports data to dataTable
        /// </summary>
        /// <param name="dataTable">DataTable which revit data will be exported into</param>
        abstract public void Export(DataTable dataTable);

        /// <summary>
        /// Imports data from dataTable
        /// </summary>
        /// <param name="dataTable">DataTable to import</param>
        abstract public void Import(DataTable dataTable);

        /// <summary>
        /// Delete the rows in garbage row collection.
        /// </summary>
        public void ClearGarbageRows()
        {
            foreach (DataRow row in m_garbageRows)
            {
                row.Delete();
            }
        }

        /// <summary>
        /// Update the rows in pending row collection
        /// </summary>
        public void UpdatePendingRows()
        {
            foreach (KeyValuePair<DataRow, DataRow> pair in m_pendingRows)
            {
                pair.Value.ItemArray = pair.Key.ItemArray;
            }
        }

        /// <summary>
        /// Add a new column which need to deal with.
        /// </summary>
        /// <param name="columnName">column name that added</param>
        public void AppendColumn(string columnName)
        {
            if (!m_extraColumns.Contains(columnName))
            {
                m_extraColumns.Add(columnName);
            }
        }

        /// <summary>
        /// Add some new columns which need to deal with.
        /// </summary>
        /// <param name="columnNames">column name list that added</param>
        public void AppendColumns(List<string> columnNames)
        {
            foreach (string columnName in columnNames)
            {
                AppendColumn(columnName);
            }
        }

        /// <summary>
        /// Clear extra columns
        /// </summary>
        public void ClearExtraColumns()
        {
            m_extraColumns.Clear();
        } 
        #endregion
    };
}
