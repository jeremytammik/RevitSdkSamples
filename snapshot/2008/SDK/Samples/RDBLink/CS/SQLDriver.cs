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
    class SQLDriver
    {
        #region Fields
        //driver completion modes
        public const ushort SQL_DRIVER_NOPROMPT = 0;
        public const ushort SQL_DRIVER_COMPLETE = 1;
        public const ushort SQL_DRIVER_PROMPT = 2;
        public const ushort SQL_DRIVER_COMPLETE_REQUIRED = 3;

        //return values
        public const short SQL_SUCCESS = 0;
        public const short SQL_SUCCESS_WITH_INFO = 1;
        public const short SQL_STILL_EXECUTING = 2;
        public const short SQL_NEED_DATA = 99;
        public const short SQL_NO_DATA = 100;
        public const short SQL_ERROR = (-1);
        public const short SQL_INVALID_HANDLE = (-2);

        //handles
        static IntPtr environmentHandle = IntPtr.Zero;
        static IntPtr connectionHandle = IntPtr.Zero;

        //allocated flag
        static bool isAllocated; 
        #endregion

        #region Methods
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
        [DllImport("odbc32.dll", CharSet = CharSet.Ansi)]
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
        [DllImport("odbc32.dll", CharSet = CharSet.Ansi)]
        static extern short SQLFreeConnect(IntPtr ConnectionHandle);

        /// <summary>
        /// closes the connection associated with a specific connection handle. 
        /// </summary>
        /// <param name="ConnectionHandle">ConnectionHandle</param>
        /// <returns>SQL_SUCCESS, SQL_SUCCESS_WITH_INFO, SQL_ERROR, or SQL_INVALID_HANDLE</returns>
        [DllImport("odbc32.dll", CharSet = CharSet.Ansi)]
        static extern short SQLDisconnect(IntPtr ConnectionHandle);

        /// <summary>
        /// Frees an given ODBC Environment handle 
        /// </summary>
        /// <param name="EnvironmentHandle">EnvironmentHandle</param>
        /// <return>SQL_SUCCESS, SQL_SUCCESS_WITH_INFO, SQL_INVALID_HANDLE, or SQL_ERROR</return>
        [DllImport("odbc32.dll", CharSet = CharSet.Ansi)]
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
        /// Connect a database and return the connection string
        /// </summary>
        /// <param name="connstr">A full connection string, a partial connection string, 
        /// or an empty string.</param>
        /// <param name="driverCompletion">flag that indicates whether the Driver Manager or 
        /// driver must prompt for more connection information.</param>
        /// <param name="retConnStr">The completed connection string</param>
        /// <returns>SQL_SUCCESS, SQL_SUCCESS_WITH_INFO, SQL_NO_DATA, SQL_ERROR, 
        /// or SQL_INVALID_HANDLE.</returns>
        public static int Connect(string connstr, ushort driverCompletion, ref string retConnStr)
        {
            retConnStr = string.Empty;
            const short MAX_CONNECT_LEN = 1024;
            StringBuilder out_connect = new StringBuilder(MAX_CONNECT_LEN);
            string in_connect = connstr;
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
            return retval;
        }

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
        /// Close connection and free memory
        /// </summary>
        public static void Terminate()
        {
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
        #endregion
    }
}
