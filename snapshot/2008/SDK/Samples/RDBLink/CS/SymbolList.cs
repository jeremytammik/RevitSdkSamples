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
using Autodesk.Revit.Parameters;

namespace Revit.SDK.Samples.RDBLink.CS
{
    /// <summary>
    /// Provides export and import functions for Revit symbols.
    /// </summary>
    public class SymbolList : ElementList
    {
        #region Fields
        /// <summary>
        /// The TableInfo related with this type of symbols
        /// </summary>
        private TableInfo m_tableInfo;

        /// <summary>
        /// Counter of duplicated symbols
        /// </summary>
        static int duplicatedCount = 0;

        /// <summary>
        /// A symbol which is to be duplicated when create new symbols
        /// </summary>
        Symbol m_symbolToDuplicate; 
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes table name and TableInfo.
        /// </summary>
        /// <param name="tableInfo"></param>
        public SymbolList(TableInfo tableInfo)
        {
            if(tableInfo != null)
            {
                m_tableInfo = tableInfo;
                TableName = m_tableInfo.Name;
            }
        } 
        #endregion

        #region Properties

        /// <summary>
        /// Gets TableInfo
        /// </summary>
        protected TableInfo TableInfo
        {
            get { return m_tableInfo; }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Check whether an element belongs to the specific table.
        /// </summary>
        /// <param name="element">element to check</param>
        /// <returns>true if the element belongs to the table, otherwise false.</returns>
        protected override bool CanAppend(Element element)
        {
            return (m_tableInfo.Filter.Category == GetCategoryId(element)
                && m_tableInfo.Filter.ElementType == ElementType.SYMBOL
                && element is Symbol);
        }

        /// <summary>
        /// Create a symbol using a given symbol name.
        /// </summary>
        /// <param name="row">DataRow which contains the information to create symbol.</param>
        /// <returns>Created symbol.</returns>
        protected override Element CreateElement(DataRow row)
        {
            string typeName = row["TypeName"].ToString();
            string familyName = row["FamilyName"].ToString();
            Symbol newSymbol = null;
            m_symbolToDuplicate = GetOneSymbolToDuplicate(familyName);
            if (m_symbolToDuplicate != null)
            {
                try
                {
                    typeName = GetNewSymbolName(typeName);
                    newSymbol = m_symbolToDuplicate.Duplicate(typeName);
                }
                catch
                {
                    // if exception throws, it means we can't create this type of elements.
                }
            }
            return newSymbol;
        }

        /// <summary>
        /// Insert data of an element to a DataRow.
        /// </summary>
        /// <param name="element">element to retrieve data.</param>
        /// <param name="row">DataRow to populate.</param>
        protected override void PopulateDbRow(Element element, DataRow row)
        {
            foreach (ColumnInfo colInfo in m_tableInfo.Columns)
            {
                if (colInfo.BuiltInParameter != BuiltInParameter.INVALID)
                {
                    BuiltInParameter bip = colInfo.BuiltInParameter;
                    Parameter parameter = element.get_Parameter(bip);
                    if (parameter != null)
                    {
                        row[colInfo.Name] = GetParameterDdValue(parameter);
                    }
                }
            }
            // populate extra columns
            foreach (string columnName in ExtraColumns)
            {
                Parameter param = GetParameterByDefinitionName(element, columnName);
                row[columnName] = GetParameterDdValue(param);
            }
        }

        /// <summary>
        /// Update an element with the data in a DataRow.
        /// </summary>
        /// <param name="element">Element to update.</param>
        /// <param name="rowIndex">Index of the DataRow in DataTable.</param>
        protected override void UpdateElement(Element element, int rowIndex)
        {
            DataRow row = DataTable.Rows[rowIndex];
            Parameter param = null;
            foreach (ColumnInfo col in m_tableInfo.Columns)
            {
                // element id is read only
                if (col.Name.Equals("Id"))
                {
                    row["Id"] = element.Id.Value;
                    continue;
                }

                param = element.get_Parameter(col.BuiltInParameter);
                SynParamAndDbCell(param, row, col.Name);
            }

            foreach (string extCol in ExtraColumns)
            {
                param = GetParameterByDefinitionName(element, extCol);
                SynParamAndDbCell(param, row, extCol);
            }
        }

        /// <summary>
        /// Update a parameter of an element using a DBCell, if update failed, update the DBCell.
        /// </summary>
        /// <param name="parameter">Parameter to update.</param>
        /// <param name="row">DataRow contains the DBCell.</param>
        /// <param name="colName">Column name of the DBCell.</param>
        private void SynParamAndDbCell(Parameter parameter, DataRow row, string colName)
        {
            // if parameter is null, set the db cell to null
            if(parameter == null)
            {
                row[colName] = DBNull.Value;
                return;
            }

            object dbCell = row[colName];
            try
            {
                // if parameter is read only, set db cell to the value of the parameter
                if (parameter.IsReadOnly)
                {
                    UpdateDBCell(parameter, row, colName);
                }
                else
                {
                    // if update failed, set db cell to the value of the parameter
                    if (!UpdateParameter(parameter, dbCell))
                    {
                        UpdateDBCell(parameter, row, colName);
                    }
                }
            }
            catch
            {
                // if exception occurs, set db cell to the value of the parameter
                UpdateDBCell(parameter, row, colName);
            }
        }

        /// <summary>
        /// Update a DBCell using a given Parameter.
        /// </summary>
        /// <param name="parameter">Parameter to retrieve data.</param>
        /// <param name="row">DataRow contains the DBCell.</param>
        /// <param name="colName">Column name of the DBCell.</param>
        private void UpdateDBCell(Parameter parameter, DataRow row, string colName)
        {
            // ignore assembly code
            if (colName.Equals("AssemblyCode"))
            {
                row[colName] = DBNull.Value;
                return;
            }

            StorageType type = parameter.StorageType;
            switch (type)
            {
                case StorageType.Integer:
                    row[colName] = parameter.AsInteger();
                    break;

                case StorageType.Double:
                    try
                    {
                        row[colName] = Unit.CovertFromAPI(parameter.DisplayUnitType, parameter.AsDouble());
                    }
                    catch
                    {
                        row[colName] = parameter.AsDouble();
                    }
                    break;

                case StorageType.String:
                    row[colName] = parameter.AsString();
                    break;

                case StorageType.ElementId:
                    ElementId id = parameter.AsElementId();
                    if (id.Value == -1)
                    {
                        row[colName] = DBNull.Value;
                    }
                    else
                    {
                        row[colName] = id.Value;
                    }
                    break;

                default:
                    row[colName] = parameter.AsValueString();
                    break;
            }
        }

        /// <summary>
        /// Update a Parameter using a given DBCell.
        /// </summary>
        /// <param name="parameter">Parameter to update.</param>
        /// <param name="dbCell">DBCell to get data.</param>
        /// <returns>true if update success, otherwise false.</returns>
        private bool UpdateParameter(Parameter parameter, object dbCell)
        {
            if (dbCell == DBNull.Value) return false;
            bool retval = false;
            StorageType type = parameter.StorageType;
            switch (type)
            {
                case StorageType.Integer:
                    retval = parameter.Set((int)dbCell);
                    break;

                case StorageType.Double:
                    double newValue = (Double)dbCell;
                    try
                    {
                        newValue = Unit.CovertToAPI(newValue, parameter.DisplayUnitType);
                    }
                    catch
                    {
                    }
                    retval = parameter.Set(newValue);
                    break;

                case StorageType.String:
                    retval = parameter.Set(dbCell.ToString());
                    break;

                case StorageType.ElementId:
                    ElementId id = new ElementId();
                    id.Value = (int)(dbCell);
                    if (ActiveDocument.get_Element(ref id) == null)
                        return false;
                    retval = parameter.Set(ref id);
                    break;

                default:
                    retval = parameter.SetValueString(dbCell.ToString());
                    break;
            }
            return retval;
        }

        /// <summary>
        /// Get a symbol to duplicate.
        /// </summary>
        /// <param name="familyName">Family name used to find the best matching symbol.</param>
        /// <returns>A symbol can be duplicated.</returns>
        private Symbol GetOneSymbolToDuplicate(string familyName)
        {
            Symbol symbol = null;
            Symbol firstSymbol = null;
            familyName = familyName.Trim();                
            ElementSetIterator eit = Elements.ForwardIterator();
            eit.Reset();
            while (eit.MoveNext())
            {
                symbol = eit.Current as Symbol;
                if (symbol != null)
                {
                    // if family name is empty, return the first symbol we find
                    if(firstSymbol == null) firstSymbol = symbol;
                    if(familyName == string.Empty)
                    {
                        return firstSymbol;
                    }
                    
                    //get symbol family name.
                    Parameter familyParam = symbol.get_Parameter(BuiltInParameter.SYMBOL_FAMILY_NAME_PARAM);
                    if(familyParam != null)
                    {
                        string symbolName = familyParam.AsString(); 
                        // find the designated symbol
                        if(symbolName.Equals(familyName))
                        {
                            return symbol;
                        }
                    }
                }
            }
            // return the first symbol.
            return firstSymbol;
        }

        /// <summary>
        /// Get a non-duplicated name.
        /// </summary>
        /// <param name="typeName">original name.</param>
        /// <returns>new name which is not duplicated with other symbol's name.</returns>
        private string GetNewSymbolName(string typeName)
        {
            if(typeName == String.Empty)
            {
                typeName = "Default" + duplicatedCount++;
            }

            bool found = true;
            while (found)
            {
                found = false;
                foreach (Element element in Elements)
                {
                    if (element.Name.Equals(typeName))
                    {
                        found = true;
                        break;
                    }
                }
                // if the name has already existed, assign a new name to it.
                if (found)
                {
                    typeName = typeName + duplicatedCount++;
                }
            }
            return typeName;
        } 
        #endregion

       
    };
}
