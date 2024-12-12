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
using Autodesk.Revit.Parameters;
using Autodesk.Revit;
using Autodesk.Revit.Collections;

namespace Revit.SDK.Samples.RDBLink.CS
{
    /// <summary>
    /// Indicates Revit Element type
    /// </summary>
    public enum ElementType
    {
        /// <summary>
        /// Element is Instance
        /// </summary>
        INSTANCE, 
        /// <summary>
        /// Element is Symbol
        /// </summary>
        SYMBOL
    }

    /// <summary>
    /// Stores primary keys (column ids) of a table
    /// </summary>
    public class PrimaryKeys : List<string>
    {
        #region Methods
        /// <summary>
        /// Get column name of a key
        /// </summary>
        /// <param name="index">index of the key</param>
        /// <returns>column name of the key</returns>
        public string ColumnName(int index)
        {
            return RDBResource.GetColumnName(this[index]);
        } 
        #endregion
    };

    /// <summary>
    /// Represents a foreign key of a table
    /// </summary>
    public class ForeignKey
    {
        #region Fields
        /// <summary>
        /// Column id of the foreign key
        /// </summary>
        string m_columnId;
        /// <summary>
        /// Related table id
        /// </summary>
        string m_refTableId;
        /// <summary>
        /// Related column id
        /// </summary>
        string m_refColumnId; 
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the column id of the key
        /// </summary>
        public string ColumnId
        {
            get { return m_columnId; }
            set { m_columnId = value; }
        }
        /// <summary>
        /// Gets or sets the related table id
        /// </summary>
        public string RefTableId
        {
            get { return m_refTableId; }
            set { m_refTableId = value; }
        }
        /// <summary>
        /// Gets or sets the related column id
        /// </summary>
        public string RefColumnId
        {
            get { return m_refColumnId; }
            set { m_refColumnId = value; }
        }
        /// <summary>
        /// Gets the column name
        /// </summary>
        public string ColumnName
        {
            get
            {
                return RDBResource.GetColumnName(m_columnId);
            }
        }
        /// <summary>
        /// Gets the related table name
        /// </summary>
        public string RefTableName
        {
            get
            {
                return RDBResource.GetTableName(m_refTableId);
            }
        }
        /// <summary>
        /// Gets the related column name
        /// </summary>
        public string RefColumnName
        {
            get
            {
                return RDBResource.GetColumnName(m_refColumnId);
            }
        } 
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor, initialize column id, related table and column id
        /// </summary>
        /// <param name="columnId">Column id of the foreign key</param>
        /// <param name="refTableId">Related table id</param>
        /// <param name="refColumnId">Related column id</param>
        public ForeignKey(string columnId, string refTableId, string refColumnId)
        {
            m_columnId = columnId;
            m_refTableId = refTableId;
            m_refColumnId = refColumnId;
        } 
        #endregion
    };

    /// <summary>
    /// Preserves all database schema information
    /// </summary>
    public partial class TableInfoSet : Dictionary<string, TableInfo>
    {
        #region Fields
        /// <summary>
        /// Stores all custom tables
        /// </summary>
        List<string> m_customTableIds = new List<string>(); 
        #endregion

        #region Properties
        /// <summary>
        /// Gets a list of all custom table ids
        /// </summary>
        public List<string> CustomTableIds
        {
            get { return m_customTableIds; }
            set { m_customTableIds = value; }
        } 
        #endregion

        #region Constructors
        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="document">Revit Document</param>
        public TableInfoSet(Document document)
        {
            InitializeAllTableInfos();
            InitializeExtraColumns(document);
        } 
        #endregion

        #region Methods
        /// <summary>
        /// Strings only contain "A-Z, a-z, 0-9" and does not start with numbers is allowed
        /// </summary>
        /// <param name="columnName">column name</param>
        private bool IsSQLAllowed(string columnName)
        {
            if (columnName == null || columnName.Length == 0 || char.IsNumber(columnName[0]))
                return false;
            bool allowed = true;
            foreach (char ch in columnName.ToCharArray())
            {
                if (!(
                    ('0' <= ch && ch <= '9') ||
                    ('A' <= ch && ch <= 'Z') ||
                    ('a' <= ch && ch <= 'z') ||
                    ch == '_'))
                    allowed = false;
            }
            return allowed;
        }

        /// <summary>
        /// Create shared or project parameter columns.
        /// </summary>
        /// <param name="revitDocument">RevitAPI Document</param>
        private void InitializeExtraColumns(Document revitDocument)
        {
            BindingMap bindingMap = revitDocument.ParameterBindings;
            DefinitionBindingMapIterator itor = bindingMap.ForwardIterator();
            itor.Reset();

            while (itor.MoveNext())
            {
                ElementBinding binding = itor.Current as ElementBinding;
                if (binding == null) continue;

                string columnName = itor.Key.Name;

                //get related column name of this parameter
                if (!IsSQLAllowed(columnName)) continue;

                //get column type using ParameterType
                DataType columnType = DataType.TEXT;
                ParameterType paraType = ParameterType.Invalid;
                try
                {
                    paraType = itor.Key.ParameterType;
                }
                catch
                {
                    continue;
                }

                columnType = DatabaseConfig.GetDataType(paraType);

                // get all categories which this parameter binds to
                CategorySet categories = binding.Categories;
                foreach (Category category in categories)
                {
                    // this line is needed, if not, it will throw exception when get its id.
                    if (category == null) continue;

                    string key = RDBResource.GetTableKey(category, binding is TypeBinding);
                    if (this.ContainsKey(key))
                    {
                        // if this column conflicts with built-in-columns, ignore this column
                        TableInfo tableInfo = this[key];
                        bool columnExists = false;
                        foreach (ColumnInfo colInfo in tableInfo)
                        {
                            if (colInfo.Name.Equals(columnName))
                            {
                                columnExists = true;
                                break;
                            }
                        }
                        if (columnExists) continue;
                        tableInfo.Add(new CustomColumnInfo(columnName, columnType));
                    }
                }
            }
        } 
        #endregion
    };

    /// <summary>
    /// Provides database schema and interface to populate table with Revit model data 
    /// </summary>
    public class TableInfo : List<ColumnInfo>
    {
        #region Fields
        /// <summary>
        /// All foreign keys
        /// </summary>
        List<ForeignKey> m_foreignKeys = new List<ForeignKey>();
        /// <summary>
        /// All primary keys
        /// </summary>
        PrimaryKeys m_primaryKey = new PrimaryKeys();
        /// <summary>
        /// APIObjectList which used to transfer data between Revit and database
        /// </summary>
        APIObjectList m_objectList;
        /// <summary>
        /// Table id
        /// </summary>
        string m_tableId;
        /// <summary>
        /// Table name
        /// </summary>
        string m_name; 
        #endregion

        #region Properties
        /// <summary>
        /// Gets corresponding APIObjectList
        /// </summary>
        public APIObjectList ObjectList
        {
            get { return m_objectList; }
            //set { m_objectList = value; }
        }

        /// <summary>
        /// Gets table resource id
        /// </summary>
        public string TableId
        {
            get { return m_tableId; }
            //set { m_tableId = value; }
        }

        /// <summary>
        /// Gets table name
        /// </summary>
        public string Name
        {
            get { return m_name; }
            //set { m_name = value; }
        } 
        #endregion

        #region Properties
        /// <summary>
        /// Gets PrimaryKey list
        /// </summary>
        public PrimaryKeys PrimaryKeys
        {
            get { return m_primaryKey; }
            //set { m_primaryKey = value; }
        }

        /// <summary>
        /// Gets ForeignKey list
        /// </summary>
        public List<ForeignKey> ForeignKeys
        {
            get { return m_foreignKeys; }
            set { m_foreignKeys = value; }
        } 
        #endregion

        #region Constructors
        /// <summary>
        /// Initialize table id and table name
        /// </summary>
        /// <param name="tableId">table id</param>
        public TableInfo(string tableId)
        {
            m_tableId = tableId;
            m_name = RDBResource.GetTableName(m_tableId);
        }

        /// <summary>
        /// Initialize table id, table name and APIObjectList
        /// </summary>
        /// <param name="tableId">table id</param>
        /// <param name="list">APIObjectList</param>
        public TableInfo(string tableId, APIObjectList list)
            : this(tableId)
        {
            m_objectList = list;
            list.TableInfo = this;
        } 
        #endregion
    }

    /// <summary>
    /// Subclass of TableInfo, provides constructors for "instance" tables
    /// </summary>
    public class InstanceTableInfo : TableInfo
    {
        #region Constructors
        /// <summary>
        /// Initialize resource id and APIObjectList with a new InstanceList
        /// </summary>
        /// <param name="resId">Resource id</param>
        public InstanceTableInfo(string resId)
            : base(resId, new InstanceList())
        {
        } 



        #endregion
    };

    /// <summary>
    /// Subclass of TableInfo, provides constructors for "symbol" tables
    /// </summary>
    public class SymbolTableInfo : TableInfo
    {
        #region Constructors
        /// <summary>
        /// Initialize resource id and APIObjectList with a new SymbolList
        /// </summary>
        /// <param name="resId">Resource id</param>
        public SymbolTableInfo(string resId)
            : base(resId, new SymbolList())
        {
        }
        #endregion
    };

    /// <summary>
    /// Provides column schema
    /// </summary>
    public class ColumnInfo
    {
        #region Fields
        /// <summary>
        /// Column id
        /// </summary>
        string m_columnId;
        /// <summary>
        /// Column name
        /// </summary>
        string m_name;
        /// <summary>
        /// corresponding BuiltInParameter
        /// </summary>
        BuiltInParameter m_builtInParameter;
        /// <summary>
        /// Column data type
        /// </summary>
        DataType m_dataType;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets Column DataType
        /// </summary>
        public DataType DataType
        {
            get { return m_dataType; }
            set { m_dataType = value; }
        }
        /// <summary>
        /// Gets or sets Column id
        /// </summary>
        public string ColumnId
        {
            get { return m_columnId; }
            set { m_columnId = value; }
        }
        /// <summary>
        /// Gets or sets Column name
        /// </summary>
        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        } 
        /// <summary>
        /// Gets or sets BuiltInParameter
        /// </summary>
        public BuiltInParameter BuiltInParameter
        {
            get { return m_builtInParameter; }
            set { m_builtInParameter = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public ColumnInfo()
        {
        }

        /// <summary>
        /// Initialize BuiltInParameter, DataType, column id and name
        /// </summary>
        /// <param name="parameterId">corresponding BuiltInParameter</param>
        /// <param name="dataType">Column DataType</param>
        public ColumnInfo(BuiltInParameter parameterId, DataType dataType)
            : this()
        {
            m_builtInParameter = parameterId;
            m_dataType = dataType;

            m_columnId = "ColN_BP_" + Enum.GetName(typeof(BuiltInParameter), parameterId);
            m_name = RDBResource.GetColumnName(m_columnId);
        }

        /// <summary>
        /// Initialize BuiltInParameter, DataType, column id and name
        /// </summary>
        /// <param name="columnId">Column id</param>
        /// <param name="dataType">Column DataType</param>
        public ColumnInfo(string columnId, DataType dataType)
            : this()
        {
            m_columnId = columnId;
            m_dataType = dataType;
            m_builtInParameter = BuiltInParameter.INVALID;
            m_name = RDBResource.GetColumnName(m_columnId);
        } 
        #endregion
    }
    
    /// <summary>
    /// Subclass of ColumnInfo, provides constructors for custom tables
    /// </summary>
    public class CustomColumnInfo : ColumnInfo
    {
        #region Constructors
        /// <summary>
        /// Do initialization
        /// </summary>
        /// <param name="columnId">Column id</param>
        /// <param name="dataType">Column DataType</param>
        public CustomColumnInfo(string columnId, DataType dataType)
            : base()
        {
            ColumnId = columnId;
            DataType = dataType;
            BuiltInParameter = BuiltInParameter.INVALID;
            Name = columnId;
        }
        #endregion
    };

    
}
