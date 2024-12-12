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
using Autodesk.Revit.Parameters;
using Autodesk.Revit;
using System.IO;
using System.Xml.Serialization;

namespace Revit.SDK.Samples.RDBLink.CS
{
    /// <summary>
    /// Container of TableInfo.
    /// </summary>
    sealed public partial class TableInfoSet : List<TableInfo>
    {
        #region Fields
        /// <summary>
        /// an instance of TableInfoSet.
        /// </summary>
        static TableInfoSet s_tableInfoSet = new TableInfoSet();

        /// <summary>
        /// map of Filter and a TableInfo.
        /// </summary>
        static Dictionary<Filter, TableInfo> s_filterTableInfoMap = new Dictionary<Filter,TableInfo>();
        /// <summary>
        /// Singleton of TableInfoSet.
        /// </summary>
        public static readonly TableInfoSet Instance = s_tableInfoSet;
        #endregion

        #region Methods

        /// <summary>
        /// Stores the map of Filter and TableInfo, using this map we can quickly find
        /// which element belongs to which table.
        /// </summary>
        /// <returns>Filter and TableInfo map</returns>
        public static Dictionary<Filter, TableInfo> FilterTableInfoMap()
        {
            return s_filterTableInfoMap;
        }
        #endregion
    };

    /// <summary>
    /// Table - Column - BuiltInParameter map. Since each column of each table in database
    /// is related with a BuiltInParameter which is used to retrieve related data from an Element 
    /// using get_Parameter(BuiltInParameter).
    /// </summary>
    public class TableInfo
    {
        #region Fields
        /// <summary>
        /// table name
        /// </summary>
        string m_name;

        /// <summary>
        /// filter to filter elements.
        /// </summary>
        Filter m_filter;

        /// <summary>
        /// ColumnInfo Set.
        /// </summary>
        List<ColumnInfo> m_columns = new List<ColumnInfo>();
        #endregion

        #region Properties

        /// <summary>
        /// Gets or Sets the name of the corresponding table.
        /// </summary>
        public string Name
        {
            get
            {
                return m_name;
            }
            set
            {
                m_name = value;
            }
        }

        /// <summary>
        /// Gets or Sets the filter.
        /// </summary>
        public Filter Filter
        {
            get
            {
                return m_filter;
            }
            set
            {
                m_filter = value;
            }
        }

        /// <summary>
        /// Gets the ColumnInfoSet
        /// </summary>
        public List<ColumnInfo> Columns
        {
            get
            {
                return m_columns;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initialize the table name.
        /// </summary>
        /// <param name="name">table name</param>
        /// <param name="filter">table identifier</param>
        public TableInfo(string name, Filter filter)
        {
            m_name = name;
            m_filter = filter;
        }
        #endregion
    };
   
    /// <summary>
    /// Contains column name, column type and the corresponding BuiltInParameter which
    /// is used to retrieve a Parameter.
    /// </summary>
    public class ColumnInfo
    {
        #region Fields
        /// <summary>
        /// Integer data type in database 
        /// </summary>
        public const string INTEGER = "INTEGER";

        /// <summary>
        /// Double data type in database 
        /// </summary>
        public const string DOUBLE = "DOUBLE";

        /// <summary>
        /// String data type in database 
        /// </summary>
        public const string VARCHAR = "VARCHAR(255)";

        /// <summary>
        /// Name of the column
        /// </summary>
        string m_name;

        /// <summary>
        /// BuiltInParameter related with the column.
        /// </summary>
        BuiltInParameter m_builtInParameter = BuiltInParameter.INVALID;

        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the name of the column.
        /// </summary>
        public string Name
        {
            get
            {
                return m_name;
            }
            set
            {
                m_name = value;
            }
        }

        /// <summary>
        /// Gets or sets the related BuiltInParameter of the column.
        /// </summary>
        public BuiltInParameter BuiltInParameter
        {
            get
            {
                return m_builtInParameter;
            }
            set
            {
                m_builtInParameter = value;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes the name and the BuiltInParameter for the column.
        /// </summary>
        /// <param name="name">name of the column.</param>
        /// <param name="builtInParameter">BuiltInParameter related with the column.</param>
        public ColumnInfo(string name, BuiltInParameter builtInParameter)
        {
            m_name = name;
            m_builtInParameter = builtInParameter;
        } 
        #endregion
        
    };

    /// <summary>
    /// Element type "INSTANCE" or "SYMBOL"
    /// </summary>
    public enum ElementType 
    { 
        /// <summary>
        /// Element instance
        /// </summary>
        INSTANCE = 0, 

        /// <summary>
        /// Element symbol
        /// </summary>
        SYMBOL 
    }

    /// <summary>
    /// Contains the information of category an element belongs to and the type of the element.
    /// </summary>
    public struct Filter
    {
        #region Fields
        /// <summary>
        /// Category the element belongs to.
        /// </summary>
        BuiltInCategory m_category;

        /// <summary>
        /// type of the element, either "INSTANCE" or "SYMBOL".
        /// </summary>
        ElementType m_elementType; 
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        public BuiltInCategory Category
        {
            get
            {
                return m_category;
            }
            set
            {
                m_category = value;
            }
        }

        /// <summary>
        /// Gets or sets the element type.
        /// </summary>
        public ElementType ElementType
        {
            get
            {
                return m_elementType;
            }
            set
            {
                m_elementType = value;
            }
        } 
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes category and element type.
        /// </summary>
        /// <param name="category">BuiltInCategory.</param>
        /// <param name="elementType">ElementType.</param>
        public Filter(BuiltInCategory category, ElementType elementType)
        {
            m_category = category;
            m_elementType = elementType;
        } 
        #endregion
    };
}
