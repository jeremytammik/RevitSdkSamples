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
using Autodesk.Revit.Parameters;
using Autodesk.Revit.Collections;
using System.Diagnostics;

namespace Revit.SDK.Samples.RDBLink.CS
{
    /// <summary>
    /// Provides function to create database tables and their relations.
    /// </summary>
    public class DatabaseManager
    {
        #region Fields
        /// <summary>
        /// TableInfoSet which preserves database schema and other information
        /// </summary>
        private TableInfoSet m_tableInfoSet; 
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tableInfoSet">TableInfoSet object</param>
        public DatabaseManager(TableInfoSet tableInfoSet)
        {
            m_tableInfoSet = tableInfoSet;
        } 
        #endregion

        #region Methods
        /// <summary>
        /// Build the SQL statement to create a table
        /// </summary>
        /// <param name="tableInfo">TableInfo indicates table to create</param>
        /// <returns>SQL statement to create table</returns>
        private string GetTableCreationSqlStmt(TableInfo tableInfo)
        {
            StringBuilder builder = new StringBuilder(100);
            builder.Append("CREATE TABLE ");
            builder.Append(tableInfo.Name);
            builder.Append(" (");

            int ii = 0;
            foreach (ColumnInfo col in tableInfo)
            {
                ++ii;
                builder.Append("[" + col.Name + "]");
                builder.Append(" ");
                builder.Append(DatabaseConfig.Instance().GetDataType(col.DataType));
                if (ii < tableInfo.Count)
                    builder.Append(",");
            }

            if (tableInfo.PrimaryKeys.Count > 0)
            {
                builder.Append(", CONSTRAINT ");
                builder.Append("PK_" + tableInfo.Name);
                builder.Append(" PRIMARY KEY (");

                for (int i = 0; i < tableInfo.PrimaryKeys.Count; i++)
                {
                    builder.Append(tableInfo.PrimaryKeys.ColumnName(i));
                    if (i < tableInfo.PrimaryKeys.Count - 1)
                        builder.Append(",");
                }
                builder.Append(")");
            }
            builder.Append(")");
            return builder.ToString();
        }

        /// <summary>
        /// Build SQL statement to create foreign keys of a table
        /// </summary>
        /// <param name="tableInfo">TableInfo indicates table to create</param>
        /// <returns>SQL statement to create foreign keys</returns>
        private string[] GetTableForeignKeysSqlStmt(TableInfo tableInfo)
        {
            StringBuilder builder = null;
            string[] result = new string[tableInfo.ForeignKeys.Count];
            for (int i = 0; i < tableInfo.ForeignKeys.Count; i++)
            {
                builder = new StringBuilder(100);
                ForeignKey fKey = tableInfo.ForeignKeys[i];
                builder.Append("ALTER TABLE ");
                builder.Append(tableInfo.Name);
                builder.Append(" ADD CONSTRAINT ");
                builder.Append("FK_" + tableInfo.Name + "_" + fKey.ColumnName);
                builder.Append(" FOREIGN KEY(");
                builder.Append(fKey.ColumnName);
                builder.Append(") REFERENCES ");
                builder.Append(fKey.RefTableName);
                builder.Append("(");
                builder.Append(fKey.RefColumnName);
                builder.Append(")");
                result[i] = builder.ToString();
            }
            return result;
        }

        /// <summary>
        /// Build SQL statement to add columns not exist in table.
        /// </summary>
        /// <param name="tableName">Table name</param>
        /// <param name="columns">column name</param>
        /// <returns>SQL statement</returns>
        private string GetAddColumnSqlStmt(String tableName, List<ColumnInfo> columns)
        {
            StringBuilder sqlBuilder = new StringBuilder(100);
            sqlBuilder.AppendFormat("ALTER TABLE {0} ADD ", tableName);
            for(int ii = 0; ii < columns.Count; ii++)
            {
                sqlBuilder.AppendFormat("{0} {1}", columns[ii].Name, 
                    DatabaseConfig.Instance().GetDataType(columns[ii].DataType));
                if (ii < columns.Count - 1)
                    sqlBuilder.Append(",");
            }
            return sqlBuilder.ToString();
        }

        /// <summary>
        /// Create all database tables.
        /// </summary>
        /// <param name="sqlDriver">SQLDriver object, use to execute SQL statement</param>
        /// <param name="revitDocument">RevitAPI Document</param>
        /// <param name="tableInfoSet">TableInfoSet</param>
        public void Create(SQLDriver sqlDriver, Document revitDocument, TableInfoSet tableInfoSet)
        {
            Trace.WriteLine("Create all database tables ...");
            Trace.Indent();
            // Create tables.
            foreach (TableInfo ti in m_tableInfoSet.Values)
            {
                Trace.WriteLine(string.Format("{0},{1}", ti.TableId, ti.Name));
                if (sqlDriver.TableExist(ti.Name))
                {
                    // Table exists, we check the table columns, if there are columns 
                    // not exist in the table, we add it.
                    List<ColumnInfo> needAddedColumns = new List<ColumnInfo>();
                    foreach(ColumnInfo ci in ti)
                    {
                        if(!sqlDriver.ColumnExist(ti.Name, ci.Name))
                        {
                            needAddedColumns.Add(ci);
                        }
                    }

                    if (needAddedColumns.Count > 0)
                    {
                        sqlDriver.ExecuteSQL(GetAddColumnSqlStmt(ti.Name, needAddedColumns));
                    }
                }
                else
                {
                    string tableCreationSqlStmt = GetTableCreationSqlStmt(ti);
                    // Table doesn't exist, we create it.
                    if (!sqlDriver.ExecuteSQL(tableCreationSqlStmt))
                    {
                        Trace.WriteLine("Failed: " + tableCreationSqlStmt);
                    }
                }
            }

            // Add foreign keys.
            foreach (TableInfo ti in m_tableInfoSet.Values)
            {
                string[] foreignKeysStmt = GetTableForeignKeysSqlStmt(ti);
                foreach (string fKeyStmt in foreignKeysStmt)
                {
                    sqlDriver.ExecuteSQL(fKeyStmt);
                }
            }
            Trace.Unindent();
        }
        #endregion

    }
}
