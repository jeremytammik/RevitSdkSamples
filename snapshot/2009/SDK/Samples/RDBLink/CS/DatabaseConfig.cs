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
using Autodesk.Revit.Parameters;

namespace Revit.SDK.Samples.RDBLink.CS
{
    /// <summary>
    /// Indicates data type of columns
    /// </summary>
    public enum DataType
    {
        /// <summary>
        /// Integer
        /// </summary>
        INTEGER, 

        /// <summary>
        /// Double
        /// </summary>
        DOUBLE, 

        /// <summary>
        /// String
        /// </summary>
        TEXT
    }

    /// <summary>
    /// Preserves Database configuration, such as InvalidCharacters and DataType and string map
    /// </summary>
    public class DatabaseConfig
    {
        #region Fields
        /// <summary>
        /// Unique instance of DatabaseConfig
        /// </summary>
        static DatabaseConfig s_instance = null;

        /// <summary>
        /// Invalid characters for table name and column name
        /// </summary>
        protected char[] m_invalidCharacters = new char[0];

        /// <summary>
        /// In order to avoid the table name equals keywords of database ,
        /// so map keywords to other words.
        /// </summary>
        protected Dictionary<string, string> m_keyWordsMap = new Dictionary<string, string>();        

        /// <summary>
        /// Data type map
        /// </summary>
        protected Dictionary<DataType, string> m_dataTypeMap = new Dictionary<DataType, string>();

        #endregion

        #region Properties

        /// <summary>
        /// Gets some invalid characters with could not appear in table and column names
        /// </summary>
        public char[] InvalidCharacters
        {
            get { return m_invalidCharacters; }
        }

        /// <summary>
        /// In order to avoid the table name equals keywords of database ,
        /// so map keywords to other words.
        /// </summary>
        public Dictionary<string, string> KeyWordsMap
        {
            get{ return m_keyWordsMap; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Protected to avoid instantiation via constructor
        /// </summary>
        protected DatabaseConfig()
        {
        } 
        #endregion

        #region Methods
        /// <summary>
        /// Get the unique instance
        /// </summary>
        /// <returns>DatabaseConfig unique instance</returns>
        public static DatabaseConfig Instance()
        {
            if (s_instance == null)
                s_instance = new DatabaseConfig();
            return s_instance;
        }

        /// <summary>
        /// Initialize instance for the first time
        /// </summary>
        /// <param name="databaseType">DatabaseType</param>
        public static void Initialize(DatabaseType databaseType)
        {
            switch (databaseType)
            {
                case DatabaseType.SQLServer:
                    s_instance = new SqlServerDatabaseConfig();
                    break;
                case DatabaseType.MSAccess:
                    s_instance = new AccessDatabaseConfig();
                    break;
                case DatabaseType.Invalid:
                default:
                    break;
            }
        }

        /// <summary>
        /// Get related data type string
        /// </summary>
        /// <param name="dataType">enumeration DataType</param>
        /// <returns>related data type string</returns>
        public string GetDataType(DataType dataType)
        {
            return m_dataTypeMap[dataType];
        }

        /// <summary>
        /// Get data type from ParameterType
        /// </summary>
        /// <param name="parameterType">ParameterType</param>
        /// <returns>related data type</returns>
        public static DataType GetDataType(ParameterType parameterType)
        {
            DataType dataType = DataType.TEXT;
            switch (parameterType)
            {
                case ParameterType.YesNo:
                case ParameterType.Integer:
                case ParameterType.Material:
                case ParameterType.Invalid:
                case ParameterType.Number:
                    dataType = DataType.INTEGER;
                    break;
                case ParameterType.URL:
                case ParameterType.Text:
                    dataType = DataType.TEXT;
                    break;
                case ParameterType.Angle:
                case ParameterType.Volume:
                case ParameterType.Area:
                case ParameterType.Force:
                case ParameterType.AreaForce:
                case ParameterType.Length:
                case ParameterType.LinearForce:
                case ParameterType.Moment:
                    dataType = DataType.DOUBLE;
                    break;
                default:
                    break;
            }
            return dataType;
        } 
        #endregion
    };

    /// <summary>
    /// Preserves SQL server database configuration
    /// </summary>
    internal class SqlServerDatabaseConfig : DatabaseConfig
    {
        #region Methods
        internal SqlServerDatabaseConfig()
        {
            m_invalidCharacters = new char[] { '-', '/', ' ' };
            m_keyWordsMap.Add("Columns", "Columns1");
            m_dataTypeMap.Add(DataType.DOUBLE, "float");
            m_dataTypeMap.Add(DataType.TEXT, "nvarchar(255)");
            m_dataTypeMap.Add(DataType.INTEGER, "int");
        } 
        #endregion
    };

    /// <summary>
    ///  Preserves Microsoft Access database configuration
    /// </summary>
    internal class AccessDatabaseConfig : DatabaseConfig
    {
        #region Methods
        internal AccessDatabaseConfig()
        {
            m_invalidCharacters = new char[] { ' ' };
            m_dataTypeMap.Add(DataType.DOUBLE, "double");
            m_dataTypeMap.Add(DataType.TEXT, "varchar");
            m_dataTypeMap.Add(DataType.INTEGER, "int");
        }
        #endregion
    };
}
