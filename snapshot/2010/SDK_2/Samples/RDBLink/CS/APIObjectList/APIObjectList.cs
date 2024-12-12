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
using Autodesk.Revit;
using System.Data;
using System.Collections.ObjectModel;

namespace Revit.SDK.Samples.RDBLink.CS
{
    /// <summary>
    /// Provides an interface to export and import data.
    /// </summary>
    public abstract class APIObjectList
    {
        #region Fields
        /// <summary>
        /// Current document
        /// </summary>
        private static Document s_activeDocument;
        /// <summary>
        /// Corresponding DataTable
        /// </summary>
        private DataTable m_dataTable;
        /// <summary>
        /// Corresponding TableInfo
        /// </summary>
        private TableInfo m_tableInfo;

        /// <summary>
        /// Indicates double value of parameter which can be treated as empty
        /// </summary>
        protected const double paraDoubleNull = 0.0;
        /// <summary>
        /// Indicates int value of parameter which can be treated as empty
        /// </summary>
        protected const int paraIntegerNull = 0;
        /// <summary>
        /// Indicates element id value of parameter which can be treated as empty
        /// </summary>
        protected const int paraElementIdValueNull = -1;
        /// <summary>
        /// Indicates string value of parameter which can be treated as empty
        /// </summary>
        protected const string paraStringNull = null;

        #endregion

        #region Properties
        /// <summary>
        /// Gets active document
        /// </summary>
        public static Document ActiveDocument
        {
            get { return APIObjectList.s_activeDocument; }
            set { s_activeDocument = value;  }
        }

        /// <summary>
        /// Gets or sets the corresponding TableInfo
        /// </summary>
        public TableInfo TableInfo
        {
            get { return m_tableInfo; }
            set { m_tableInfo = value; }
        }

        /// <summary>
        /// Gets or sets the DataTable.
        /// </summary>
        public DataTable DataTable
        {
            get { return m_dataTable; }
            set { m_dataTable = value; }
        } 

        /// <summary>
        /// whether user can create a new record in a table
        /// </summary>
        public virtual bool SupportCreate
        {
            get{return false;}
        }
        #endregion

        #region Methods
        /// <summary>
        /// Export data to dataTable
        /// </summary>
        /// <param name="dataTable">dataTable which will be exported into</param>
        abstract public void Export(DataTable dataTable);

        /// <summary>
        /// Import data from DataTable
        /// </summary>
        /// <param name="dataTable">DataTable to import</param>
        /// <returns>ErrorTable which preserves the importing results</returns>
        virtual public ErrorTable Import(DataTable dataTable)
        {
            ErrorTable et = new ErrorTable(TableInfo.TableId, TableInfo.Name, dataTable);
            et.NotSupportImporting = true;
            return et;
        }

        /// <summary>
        /// Get column name in the resource file using resource id
        /// </summary>
        /// <param name="key">resource id</param>
        /// <returns>resource value</returns>
        protected static string ColumnRes(string key)
        {
            return RDBResource.GetColumnName(key);
        }

        /// <summary>
        /// Gather all redundant records of which there is no related object found in Revit.
        /// These records will be deleted during exporting
        /// </summary>
        /// <returns>Redundant records</returns>
        public abstract List<DataRow> CollectGarbageRows();

        /// <summary>
        /// Get Revit model object from a given row.
        /// </summary>
        /// <param name="dataRow">Row contains information used to find an Revit object</param>
        /// <returns>An APIObject if found, otherwise null</returns>
        abstract public APIObject FindCorrespondingObject(DataRow dataRow);

        /// <summary>
        /// Get database row from a APIObject
        /// </summary>
        /// <param name="apiObject">RevitAPI Object</param>
        /// <returns>DataRow</returns>
        public virtual DataRow FindCorrespondingRow(APIObject apiObject)
        {
            // build a select statement
            StringBuilder selectStatement = new StringBuilder();
            object[] itemValues = GetPrimaryKeyValues(apiObject);
            int count = TableInfo.PrimaryKeys.Count;

            if (count != itemValues.Length) 
                return null;

            for (int index = 0; index < count; index++)
            {
                string columnName = ColumnRes(TableInfo.PrimaryKeys[index]);
                object value = itemValues[index];
                selectStatement.AppendFormat("[{0}]={1}", columnName, value);
                if (index < count - 1)
                {
                    selectStatement.Append(" and ");
                }
            }

            // use DataTable.Select to find rows
            DataRow[] rows = DataTable.Select(selectStatement.ToString());
            if (rows.Length > 0)
                return rows[0];
            else
                return null;
        }

        /// <summary>
        /// Get a value array from an APIObject, used to find its corresponding DataRow in DataTable
        /// </summary>
        /// <param name="apiObject">Revit API Object</param>
        /// <returns>Object array</returns>
        protected abstract object[] GetPrimaryKeyValues(APIObject apiObject);

        #endregion
    };

    /// <summary>
    /// Provides export and import functions for table AssemblyCodes.
    /// </summary>
    public class AssemblyCodesList : APIObjectList
    {
        #region Methods
        /// <summary>
        /// Export data to dataTable
        /// </summary>
        /// <param name="dataTable">dataTable which will be exported into</param>
        public override void Export(System.Data.DataTable dataTable)
        {
        }

        /// <summary>
        /// Gather all redundant records of which there is no related object found in Revit.
        /// These records will be deleted during exporting
        /// </summary>
        /// <returns>Redundant records</returns>
        public override List<System.Data.DataRow> CollectGarbageRows()
        {
            return new List<System.Data.DataRow>();
        }

        /// <summary>
        /// Get Revit model object from a given row.
        /// </summary>
        /// <param name="dataRow">Row contains information used to find an Revit object</param>
        /// <returns>An APIObject if found, otherwise null</returns>
        public override Autodesk.Revit.APIObject FindCorrespondingObject(System.Data.DataRow dataRow)
        {
            return null;
        }

        /// <summary>
        /// Get a value array from an APIObject, used to find its corresponding DataRow in DataTable 
        /// </summary>
        /// <param name="apiObject">Revit API Object</param>
        /// <returns>Object array</returns>
        protected override object[] GetPrimaryKeyValues(APIObject apiObject)
        {
            return null;
        }
        #endregion
    }

    /// <summary>
    /// Provides export and import functions for Revit categories.
    /// </summary>
    public class CategoriesList : APIObjectList
    {
        #region Methods
        /// <summary>
        /// Export categories to a DataTable.
        /// </summary>
        /// <param name="dataTable">DataTable to export.</param>
        public override void Export(DataTable dataTable)
        {
            if (dataTable == null) return;
            DataTable = dataTable;

            foreach (Category category in ActiveDocument.Settings.Categories)
            {
                DataRow row = FindCorrespondingRow(category);
                if (row == null)
                {
                    row = dataTable.NewRow();
                    row[ColumnRes("ColN_CST_Id")] = category.Id.Value;
                    dataTable.Rows.Add(row);
                }

                row[ColumnRes("ColN_CST_Name")] = category.Name;
                if (category.Material != null)
                {
                    row[ColumnRes("ColN_CST_MaterialId")] = category.Material.Id.Value;
                }
                else
                {
                    row[ColumnRes("ColN_CST_MaterialId")] = DBNull.Value;
                }
            }
        }

        /// <summary>
        /// Gather all redundant records of which there is no related object found in Revit.
        /// These records will be deleted during exporting
        /// </summary>
        /// <returns>Redundant records</returns>
        public override List<DataRow> CollectGarbageRows()
        {
            List<DataRow> result = new List<DataRow>();
            foreach (DataRow row in DataTable.Rows)
            {
                object oId = row[ColumnRes("ColN_CST_Id")];
                if (oId == null) continue;

                if (FindCorrespondingObject(row) == null)
                {
                    result.Add(row);
                }
            }

            return result;
        }

        /// <summary>
        /// Get Revit model object from a given row.
        /// </summary>
        /// <param name="dataRow">Row contains information used to find an Revit object</param>
        /// <returns>An APIObject if found, otherwise null</returns>
        public override APIObject FindCorrespondingObject(DataRow dataRow)
        {
            string name = (string)dataRow[ColumnRes("ColN_CST_Name")];
            return ActiveDocument.Settings.Categories.get_Item(name);
        }

        /// <summary>
        /// Get a value array from an APIObject, used to find its corresponding DataRow in DataTable 
        /// </summary>
        /// <param name="apiObject">Revit API Object</param>
        /// <returns>Object array</returns>
        protected override object[] GetPrimaryKeyValues(APIObject apiObject)
        {
            Category category = apiObject as Category;
            if (category != null)
            {
                return new object[] { category.Id.Value };
            }
            return new object[] { DBNull.Value };
        }
        #endregion
    };

    /// <summary>
    /// Provides export and import functions for table MaterialQuantities.
    /// </summary>
    public class MaterialQuantitiesList : APIObjectList
    {
        #region Methods
        /// <summary>
        /// Export data to dataTable
        /// </summary>
        /// <param name="dataTable">dataTable which will be exported into</param>
        public override void Export(System.Data.DataTable dataTable)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// Gather all redundant records of which there is no related object found in Revit.
        /// These records will be deleted during exporting
        /// </summary>
        /// <returns>Redundant records</returns>
        public override List<System.Data.DataRow> CollectGarbageRows()
        {
            return new List<System.Data.DataRow>();
        }

        /// <summary>
        /// Get Revit model object from a given row.
        /// </summary>
        /// <param name="dataRow">Row contains information used to find an Revit object</param>
        /// <returns>An APIObject if found, otherwise null</returns>
        public override Autodesk.Revit.APIObject FindCorrespondingObject(System.Data.DataRow dataRow)
        {
            return null;
        }

        /// <summary>
        /// Get a value array from an APIObject, used to find its corresponding DataRow in DataTable 
        /// </summary>
        /// <param name="apiObject">Revit API Object</param>
        /// <returns>Object array</returns>
        protected override object[] GetPrimaryKeyValues(APIObject apiObject)
        {
            return null;
        }
        #endregion
    }
}
