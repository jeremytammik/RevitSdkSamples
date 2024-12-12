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
using System.IO;
using System.Data.Odbc;
using System.Windows.Forms;
using System.Resources;
using System.Collections;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Data;

namespace Revit.SDK.Samples.RDBLink.CS
{
    /// <summary>
    /// Provides method to pop up ODBC data source selection dialog.
    /// </summary>
    public class SQLDriver
    {
        #region Fields
        //driver completion modes
        const ushort SQL_DRIVER_NOPROMPT = 0;
        const ushort SQL_DRIVER_COMPLETE = 1;
        const ushort SQL_DRIVER_PROMPT = 2;
        const ushort SQL_DRIVER_COMPLETE_REQUIRED = 3;
        const short SQL_NTS = -3;

        /* FreeStmt() options */
        const short SQL_CLOSE = 0;
        const short SQL_DROP = 1;
        const short SQL_UNBIND = 2;
        const short SQL_RESET_PARAMS = 3;

        //return values
        const short SQL_SUCCESS = 0;
        const short SQL_SUCCESS_WITH_INFO = 1;
        const short SQL_STILL_EXECUTING = 2;
        const short SQL_NEED_DATA = 99;
        const short SQL_NO_DATA = 100;
        const short SQL_ERROR = (-1);
        const short SQL_INVALID_HANDLE = (-2);

        //handles
        static IntPtr environmentHandle = IntPtr.Zero;
        static IntPtr connectionHandle = IntPtr.Zero;
        static IntPtr stmtHandle = IntPtr.Zero;

        //allocated flag
        static bool isAllocated;
        #endregion

        #region Methods

        #region Win32API

        /// <summary>
        /// Get current window handle
        /// </summary>
        /// <returns>Current window handle</returns>
        [DllImport("user32.dll")]
        static extern IntPtr GetActiveWindow();

        /// <summary>
        /// SQLDriverConnect is an alternative to SQLConnect. It supports data sources that 
        /// require more connection information than the three arguments in SQLConnect, 
        /// dialog boxes to prompt the user for all connection information, 
        /// and data sources that are not defined in the system information. 
        /// </summary>
        /// <param name="hdbc">Connection handle.</param>
        /// <param name="hwnd">Window handle.</param>
        /// <param name="szConnStrIn">A full connection string, a partial connection string, 
        /// or an empty string. </param>
        /// <param name="cbConnStrIn">Length of *InConnectionString, in characters. </param>
        /// <param name="szConnStrOut">A buffer for the completed connection string.</param>
        /// <param name="cbConnStrOutMax">Length of the *OutConnectionString buffer.</param>
        /// <param name="pcbConnStrOut">A buffer in which to return the total number of characters 
        /// (excluding the null-termination character) 
        /// available to return in *OutConnectionString.</param>
        /// <param name="fDriverCompletion">flag that indicates whether the Driver Manager or 
        /// driver must prompt for more connection information.</param>
        /// <returns>SQL_SUCCESS, SQL_SUCCESS_WITH_INFO, SQL_NO_DATA, SQL_ERROR, 
        /// or SQL_INVALID_HANDLE.</returns>
        [DllImport("odbc32.dll", CharSet = CharSet.Unicode)]
        static extern short SQLDriverConnect(IntPtr hdbc,
            IntPtr hwnd,
            string szConnStrIn,
            short cbConnStrIn,
            StringBuilder szConnStrOut,
            short cbConnStrOutMax,
            out short pcbConnStrOut,
            ushort fDriverCompletion);

        /// <summary>
        /// Releases a connection handle and frees all memory associated with the handle. 
        /// </summary>
        /// <param name="ConnectionHandle">ConnectionHandle</param>
        /// <returns>SQL_SUCCESS, SQL_ERROR, or SQL_INVALID_HANDLE.</returns>
        [DllImport("odbc32.dll", CharSet = CharSet.Unicode)]
        static extern short SQLFreeConnect(IntPtr ConnectionHandle);

        /// <summary>
        /// closes the connection associated with a specific connection handle. 
        /// </summary>
        /// <param name="ConnectionHandle">ConnectionHandle</param>
        /// <returns>SQL_SUCCESS, SQL_SUCCESS_WITH_INFO, SQL_ERROR, or SQL_INVALID_HANDLE</returns>
        [DllImport("odbc32.dll", CharSet = CharSet.Unicode)]
        static extern short SQLDisconnect(IntPtr ConnectionHandle);

        /// <summary>
        /// Frees an given ODBC Environment handle 
        /// </summary>
        /// <param name="EnvironmentHandle">EnvironmentHandle</param>
        /// <return>SQL_SUCCESS, SQL_SUCCESS_WITH_INFO, SQL_INVALID_HANDLE, or SQL_ERROR</return>
        [DllImport("odbc32.dll", CharSet = CharSet.Unicode)]
        static extern short SQLFreeEnv(IntPtr EnvironmentHandle);

        /// <summary>
        /// Allocates memory for an environment handle and initializes the ODBC call level 
        /// interface for use by an application. 
        /// </summary>
        /// <param name="EnvironmentHandle">EnvironmentHandle</param>
        /// <return>SQL_SUCCESS, SQL_SUCCESS_WITH_INFO, SQL_INVALID_HANDLE, or SQL_ERROR</return>
        [DllImport("odbc32.dll")]
        static extern short SQLAllocEnv(out IntPtr EnvironmentHandle);

        /// <summary>
        /// Allocates memory for a connection handle within the environment. 
        /// </summary>
        /// <param name="EnvironmentHandle">EnvironmentHandle</param>
        /// <param name="ConnectionHandle">ConnectionHandle</param>
        [DllImport("odbc32.dll")]
        static extern short SQLAllocConnect(IntPtr EnvironmentHandle, out IntPtr ConnectionHandle);

        /// <summary>
        /// Allocates a new statement handle and associates it with the connection 
        /// specified by the connection handle. 
        /// </summary>
        /// <param name="ConnectionHandle">Connection handle</param>
        /// <param name="StmtHandle">Statement handle</param>
        /// <returns>SQL_SUCCESS, SQL_SUCCESS_WITH_INFO, SQL_INVALID_HANDLE, or SQL_ERROR</returns>
        [DllImport("odbc32.dll")]
        static extern short SQLAllocStmt(IntPtr ConnectionHandle, out IntPtr StmtHandle);

        /// <summary>
        /// Free (or Reset) a Statement Handle,
        /// Ends processing on the statement referenced by the statement handle.
        /// Use this function to: Close a cursor,Reset parameters 
        /// and Unbind columns from variables. 
        /// </summary>
        /// <param name="StmtHandle">Statement handle.</param>
        /// <param name="Option">Option specifying the manner
        /// of freeing the statement handle.</param>
        /// <returns>SQL_SUCCESS, SQL_SUCCESS_WITH_INFO, SQL_INVALID_HANDLE, or SQL_ERROR</returns>
        [DllImport("odbc32.dll")]
        static extern short SQLFreeStmt(IntPtr StmtHandle, short Option);

        /// <summary>
        /// Execute SQL statement directly.
        /// </summary>
        /// <param name="StatementHandle">Statement handle</param>
        /// <param name="StatementText">Statement content</param>
        /// <param name="TextLength">Statement text length</param>
        /// <returns>SQL_SUCCESS, SQL_SUCCESS_WITH_INFO, SQL_INVALID_HANDLE, or SQL_ERROR</returns>
        [DllImport("odbc32.dll", CharSet = CharSet.Unicode)]
        static extern short SQLExecDirect(IntPtr StatementHandle,
            string StatementText, int TextLength);

        /// <summary>
        /// Returns a list of table names and associated information
        /// stored in the system catalogs of the connected data source. 
        /// </summary>
        /// <param name="StatementHandle">Statement handle</param>
        /// <param name="CatalogName">Buffer that may contain a pattern-value
        /// to qualify the result set. Catalog is the first part of a three-part table name. 
        /// This must be a NULL pointer or a zero length string. </param>       
        /// <param name="NameLength1">Length of CatalogName. This must be set to 0.</param>
        /// <param name="SchemaName">Buffer that may contain a pattern-value to qualify 
        /// the result set by schema name.</param>
        /// <param name="NameLength2">Length of SchemaName.</param>
        /// <param name="TableName">Buffer that may contain a pattern-value 
        /// to qualify the result set by table name.</param>
        /// <param name="NameLength3">Length of TableName.</param>
        /// <param name="TableType">Buffer that may contain a value list to qualify 
        /// the result set by table type. </param>
        /// <param name="NameLength4">Size of Table type.</param>
        /// <returns>SQL_SUCCESS, SQL_SUCCESS_WITH_INFO, SQL_INVALID_HANDLE, or SQL_ERROR</returns>
        [DllImport("odbc32.dll", CharSet =  CharSet.Unicode)]
        static extern short SQLTables(IntPtr StatementHandle, string CatalogName,
            short NameLength1, string SchemaName, short NameLength2, string TableName,
           short NameLength3, string TableType, short NameLength4);

        /// <summary>
        /// Returns a list of columns in the specified tables.
        /// </summary>
        /// <param name="StatementHandle">Statement handle.</param>
        /// <param name="CatalogName">Catalog name</param>
        /// <param name="NameLength1">Catalog name length</param>
        /// <param name="SchemaName">Schema name</param>
        /// <param name="NameLength2">Schema name length</param>
        /// <param name="TableName">Table name</param>
        /// <param name="NameLength3">Table name length</param>
        /// <param name="ColumnName">Column name</param>
        /// <param name="NameLength4">Column name length</param>
        /// <returns></returns>
        [DllImport("odbc32.dll", CharSet = CharSet.Unicode)]
        static extern short SQLColumns(IntPtr StatementHandle,
            string CatalogName, short NameLength1, string SchemaName, short NameLength2,
                string TableName, short NameLength3, string ColumnName, short NameLength4);

        /// <summary>
        /// Advances the cursor to the next row of the result set, and retrieves any bound columns.
        /// </summary>
        /// <param name="StatementHandle">Statement handle.</param>
        /// <returns>SQL_SUCCESS, SQL_SUCCESS_WITH_INFO, SQL_INVALID_HANDLE,
        /// SQL_NO_DATA_FOUND, or SQL_ERROR.</returns>
        [DllImport("odbc32.dll")]
        static extern short SQLFetch(IntPtr StatementHandle);

        /// <summary>
        /// Closes the open cursor on a statement handle.
        /// </summary>
        /// <param name="StatementHandle">Statement handle.</param>
        /// <returns>SQL_SUCCESS, SQL_INVALID_HANDLE, or SQL_ERROR</returns>
        [DllImport("odbc32.dll")]
        static extern short SQLCloseCursor(IntPtr StatementHandle);

        /// <summary>
        /// Returns general information (including supported data conversions) 
        /// about the DBMS to which the application is connected.
        /// </summary>
        /// <param name="sqlHdbc">Database connection handle</param>
        /// <param name="fInfoType">The type of information desired. The argument
        /// must be one of the values in the first column of the tables in Data 
        /// Types and Data Conversion. </param>
        /// <param name="rgbInfoValue">Pointer to buffer where this function stores
        /// the necessary information.</param>
        /// <param name="cbInfoValueMax">Maximum size of the buffer pointed to by 
        /// rgbInfoValue. </param>
        /// <param name="pcbInfoValue">Pointer to location where this function returns
        /// the total number of bytes available to return the desired information.</param>
        /// <returns>SQL_SUCCESS, SQL_SUCCESS_WITH_INFO , SQL_INVALID_HANDLE, 
        /// or SQL_ERROR</returns>
        [DllImport("odbc32.dll", EntryPoint = "SQLGetInfo")]
        static extern short SQLGetInfo(IntPtr sqlHdbc, short fInfoType, 
            [Out] StringBuilder rgbInfoValue, short cbInfoValueMax, out short pcbInfoValue);

        #endregion

        /// <summary>
        /// Verify value is SQL_SUCCESS or SQL_SUCCESS_WITH_INFO
        /// </summary>
        /// <param name="value">value to verify</param>
        /// <returns>true if value is SQL_SUCCESS or SQL_SUCCESS_WITH_INFO</returns>
        static bool IsOK(int value)
        {
            return value == SQL_SUCCESS || value == SQL_SUCCESS_WITH_INFO;
        }

        /// <summary>
        /// Initializes environment, allocate memory for environment and connection
        /// </summary>
        /// <returns>true if initialize successfully</returns>
        static bool Initialize()
        {
            if (!isAllocated)
            {
                if (!IsOK(SQLAllocEnv(out environmentHandle)))
                {
                    return false;
                }
                if (!IsOK(SQLAllocConnect(environmentHandle, out connectionHandle)))
                {
                    return false;
                }
                isAllocated = true;
            }
            return true;
        }

        /// <summary>
        /// Connect a database and return the connection string
        /// </summary>
        /// <param name="in_connect">A full connection string, a partial connection string, 
        /// or an empty string.</param>
        /// <param name="driverCompletion">flag that indicates whether the Driver Manager or 
        /// driver must prompt for more connection information.</param>
        /// <param name="retConnStr">The completed connection string</param>
        /// <returns>SQL_SUCCESS, SQL_SUCCESS_WITH_INFO, SQL_NO_DATA, SQL_ERROR, 
        /// or SQL_INVALID_HANDLE.</returns>
        static int Connect(string in_connect, ushort driverCompletion, ref string retConnStr)
        {
            retConnStr = string.Empty;
            const short MAX_CONNECT_LEN = 1024;
            StringBuilder out_connect = new StringBuilder(MAX_CONNECT_LEN);
            short len = 0;
            int retval = SQL_ERROR;

            // initializes environment
            if (!Initialize())
                return retval;

            // pop up dialog for user to select data source
            retval = SQLDriverConnect(connectionHandle,
                GetActiveWindow(),
                in_connect,
                (short)in_connect.Length,
                out_connect,
                MAX_CONNECT_LEN,
                out len,
                driverCompletion);
            retConnStr = out_connect.ToString();

            if (IsOK(SQLAllocStmt(connectionHandle, out stmtHandle)))
            {

            }
            return retval;
        }

        /// <summary>
        /// Execute SQL statement.
        /// </summary>
        /// <param name="sqlStmt">SQL statement.</param>
        public bool ExecuteSQL(string sqlStmt)
        {
            short status = SQLExecDirect(stmtHandle, sqlStmt, sqlStmt.Length);

            return IsOK(status);
        }

        /// <summary>
        /// Return whether the table exist in database.
        /// </summary>
        /// <param name="tableName">Table name.</param>
        /// <returns>True of False.</returns>
        public bool TableExist(string tableName)
        {
            short status = SQLTables(stmtHandle, null, 0, null, 0, tableName, SQL_NTS, null, 0);

            if (!IsOK(status))
            {
                SQLCloseCursor(stmtHandle);
                return false;
            }
            else
            {
                status = SQLFetch(stmtHandle);
                if (IsOK(status))
                {
                    SQLCloseCursor(stmtHandle);
                    return true;
                }
                else
                {
                    SQLCloseCursor(stmtHandle);
                    return false;
                }
            }
        }

        /// <summary>
        /// Judge whether the column exist in data table or not.
        /// </summary>
        /// <param name="tableName">Data table name</param>
        /// <param name="columnName">Column name</param>
        /// <returns>True or False</returns>
        public bool ColumnExist(string tableName, string columnName)
        {
            short status = SQLColumns(stmtHandle, null, 0, null, 0, 
                tableName, SQL_NTS, columnName, SQL_NTS);

            if (!IsOK(status))
            {
                SQLCloseCursor(stmtHandle);
                return false;
            }
            else
            {
                status = SQLFetch(stmtHandle);
                if (IsOK(status))
                {
                    SQLCloseCursor(stmtHandle);
                    return true;
                }
                else
                {
                    SQLCloseCursor(stmtHandle);
                    return false;
                }
            }
        }

        /// <summary>
        /// Close connection and free memory
        /// </summary>
        public void Terminate()
        {
            if (stmtHandle.ToInt32() != 0)
            {
                SQLFreeStmt(stmtHandle, SQL_CLOSE);
                stmtHandle = IntPtr.Zero;
            }
            if (connectionHandle.ToInt32() != 0)
            {
                SQLDisconnect(connectionHandle);
            }
            if (connectionHandle.ToInt32() != 0)
            {
                SQLFreeConnect(connectionHandle);
                connectionHandle = IntPtr.Zero;
            }
            if (environmentHandle.ToInt32() != 0)
            {
                SQLFreeEnv(environmentHandle);
                environmentHandle = IntPtr.Zero;
            }
            isAllocated = false;
        }

        /// <summary>
        /// Pop up data source dialog to select a data source and return connection string.
        /// </summary>
        /// <param name="connectStr">Connection string of selected data source</param>
        /// <returns>Selected database type</returns>
        public DatabaseType SelectDatabase(ref string connectStr)
        {
            int state = SQLDriver.Connect("", SQLDriver.SQL_DRIVER_PROMPT, ref connectStr);
            if (state != SQLDriver.SQL_SUCCESS && state != SQLDriver.SQL_SUCCESS_WITH_INFO)
            {
                return DatabaseType.Invalid;
            }

            short MaxLength = 1000;
            StringBuilder value = new StringBuilder(MaxLength);
            short SQL_DBMS_NAME = 17;
            short retv = 0;
            SQLGetInfo(connectionHandle, SQL_DBMS_NAME, value, MaxLength, out retv);

            switch (value.ToString())
            {
                case "Microsoft SQL Server":
                    return DatabaseType.SQLServer;
                case "ACCESS":
                    return DatabaseType.MSAccess;
                default:
                    return DatabaseType.Invalid;
            }
        }
        #endregion
    }

    /// <summary>
    /// Indicates supported databases
    /// </summary>
    public enum DatabaseType
    {
        /// <summary>
        /// Default
        /// </summary>
        Invalid,
        /// <summary>
        /// SQLServer is supported
        /// </summary>
        SQLServer,
        /// <summary>
        /// Microsoft Access is supported
        /// </summary>
        MSAccess
    }
}
