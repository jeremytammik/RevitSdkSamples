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
        /// Used for the name suffix of type name when create a new type if 
        /// the type name is duplicated
        /// </summary>
        int m_numberSuffix = 2; 
        #endregion

        #region Properties
        /// <summary>
        /// whether user can create a new record in a table
        /// </summary>
        public override bool SupportCreate
        {
            get
            {
                if (Elements.IsEmpty)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Insert data of an element to a DataRow.
        /// </summary>
        /// <param name="element">element to retrieve data.</param>
        /// <param name="row">DataRow to populate.</param>
        protected override void PopulateDbRow(Element element, DataRow row)
        {
            foreach (ColumnInfo colInfo in TableInfo)
            {
                // if it is a built-in parameter, using BuiltInParameter to 
                // get data
                if (colInfo.BuiltInParameter != BuiltInParameter.INVALID)
                {
                    BuiltInParameter bip = colInfo.BuiltInParameter;
                    Parameter parameter = element.get_Parameter(bip);
                    if (parameter != null)
                    {
                        row[colInfo.Name] = GetParameterDdValue(parameter);
                    }
                }
                // if it is not a built-in parameter, using definition name 
                // to get data
                else
                {
                    Parameter param = GetParameterByDefinitionName(element, colInfo.Name);
                    row[colInfo.Name] = GetParameterDdValue(param);
                }
            }
        }

        /// <summary>
        /// Import data from dataTable
        /// </summary>
        /// <param name="dataTable">dataTable imported</param>
        /// <returns>ErrorTable which preserves the importing results</returns>
        public override ErrorTable Import(DataTable dataTable)
        {
            // create error data for table
            ErrorTable et = new ErrorTable(TableInfo.TableId, TableInfo.Name, dataTable);
            // iterate each row
            foreach (DataRow row in dataTable.Rows)
            {
                // skip if it is deleted
                if (row.RowState == DataRowState.Deleted) continue;
                // find the corresponding element in Revit
                Element elem = FindCorrespondingObject(row) as Element;

                ErrorRow er = null;
                // no corresponding element found
                if (elem == null)
                {
                    // symbol tables support create elements
                    elem = CreateNewElement(row);
                    // create failed, we need to delete the row
                    if (elem == null)
                    {
                        // create error data for row
                        er = new ErrorRow(row);
                        // set its state to deleted
                        er.State = DataRowState.Deleted;
                        // flag it with HasChange to be true
                        er.HasChange = true;
                        // flag each cell to Equals
                        for (int colIndex = 0; colIndex < dataTable.Columns.Count; colIndex++)
                        {
                            er.Cells.Add(new ErrorCell(dataTable.Columns[colIndex].ColumnName, UpdateResult.Equals));
                        }
                    }
                    // if create successfully
                    else
                    {
                        // update the new element
                        er = UpdateOneElement(elem, row);
                        // flag its state to new added
                        er.State = DataRowState.Added;
                        // flag it with HasChange to be true
                        er.HasChange = true;
                    }
                }
                // we found the corresponding element
                else
                {
                    // update the new element
                    er = UpdateOneElement(elem, row);
                }

                // add row error data to table error data
                et.Rows.Add(er);
                if (!et.HasChange && er.HasChange)
                {
                    // mark its change state
                    et.HasChange = true;
                }
            }

            return et;
        }

        /// <summary>
        /// Create an element in Revit using the information stored in a DataRow
        /// </summary>
        /// <param name="row">DataRow stores the creation information</param>
        /// <returns>An element if create successfully, otherwise null</returns>
        protected virtual Element CreateNewElement(DataRow row)
        {
            try
            {
                // get "FaimlyName" and "TypeName" and get their column name
                string colFamilyName = ColumnRes("ColN_BP_ALL_MODEL_FAMILY_NAME");
                string colTypeName = ColumnRes("ColN_BP_ALL_MODEL_TYPE_NAME");
                string familyName = row[colFamilyName].ToString();
                string typeName = row[colTypeName].ToString();

                // make sure family name and type name not be null or Empty.
                if (familyName == null || typeName == null || 
                    familyName == string.Empty || typeName == string.Empty) 
                    return null;

                // following code is to make sure family name and type name is unique
                string tmpTypeName = typeName;
                while(true)
                {
                    // replace "'" to "''" to avoid sql keywords
                    string sqlTypeName = typeName.Replace("'", "''");
                    string sqlFamilyName = familyName.Replace("'", "''");

                    // if a duplicated row exists, give another new name for type
                    if (row.Table.Select(string.Format("[{0}] = '{1}' and [{2}] = '{3}'",
                        colFamilyName, sqlFamilyName, colTypeName, sqlTypeName)).Length > 1)
                    {
                        typeName = tmpTypeName + " " + m_numberSuffix;
                        row[colTypeName] = typeName;
                        ++m_numberSuffix;
                    }
                    else
                        break;
                } 

                // find a symbol to duplicate
                Parameter familyNameParam = null;
                Element rightSymbol = null;
                foreach (Element elem in this.Elements)
                {
                    if (rightSymbol == null) rightSymbol = elem;

                    familyNameParam = elem.get_Parameter(BuiltInParameter.ALL_MODEL_FAMILY_NAME);
                    if (familyNameParam == null) continue;
                    if (familyNameParam.AsString().Equals(familyName))
                    {
                        rightSymbol = elem;
                        break;
                    }
                }

                // if can't find any Symbol to duplicate, null will be returned.
                if (rightSymbol == null) return null;

                Symbol symbolUsetoDuplicate = rightSymbol as Symbol;

                // duplicate the symbol with a new name
                Element duplicatedSymbol = symbolUsetoDuplicate.Duplicate(typeName);
                this.Elements.Insert(duplicatedSymbol);

                // update its new id to database
                row[ColumnRes(TableInfo.PrimaryKeys[0])] = duplicatedSymbol.Id.Value;                
                return duplicatedSymbol;
            }
            catch
            {
                // if data table haven't FamilyName column or the TypeName is repeated,
                // duplication will fail. so null will be returned.
                return null;
            }
        }

        /// <summary>
        /// Update an element with a DataRow
        /// </summary>
        /// <param name="elem">element to update</param>
        /// <param name="row">row used to update</param>
        /// <returns>ErrorRow which records the update results</returns>
        private ErrorRow UpdateOneElement(Element elem, DataRow row)
        {
            ErrorRow er = new ErrorRow(row);

            Parameter param = null;
            object paramValue = null;
            foreach (ColumnInfo colInfo in this.TableInfo)
            {
                // get parameter

                // for built-in parameter
                if (colInfo.BuiltInParameter != BuiltInParameter.INVALID)
                {
                    param = elem.get_Parameter(colInfo.BuiltInParameter);
                }
                // for shared or project parameter
                else
                {
                    param = GetParameterByDefinitionName(elem, colInfo.Name);
                }

                paramValue = row[colInfo.Name];

                // we can't get "Assembly Codes", deal with it separately
                UpdateResult ur = (colInfo.BuiltInParameter == BuiltInParameter.UNIFORMAT_CODE ?
                    UpdateResult.AssemblyCode : 
                    // update the parameter with the cell value
                    UpdateParameter(param, paramValue));
                // create cell error data
                ErrorCell ec = new ErrorCell(colInfo.Name, ur);
                er.Cells.Add(ec);

                // set HasChange property to row error data
                switch (ur)
                {
                    case UpdateResult.Unknown:
                    case UpdateResult.Success:                    
                    case UpdateResult.Failed:
                    case UpdateResult.Exception:
                    case UpdateResult.ParameterNull:
                    case UpdateResult.ReadOnlyFailed:
                        er.HasChange = true;
                        break;
                    case UpdateResult.AssemblyCode:
                    case UpdateResult.Equals:
                    
                    default:
                        break;
                }
            }

            return er;
        }

        /// <summary>
        /// Update a designated parameter with some value
        /// </summary>
        /// <param name="param">Parameter to update</param>
        /// <param name="paramValue">value used to update</param>
        /// <returns>updated result</returns>
        private UpdateResult UpdateParameter(Parameter param, object paramValue)
        {
            try
            {
                //whether parameter is null
                bool parameterIsNull = (param == null);
                //whether value is null or DBNull
                bool valueIsNull = IsNullOrDBNull(paramValue);

                //parameter is null
                if (parameterIsNull)
                {
                    if (valueIsNull) return UpdateResult.Equals;
                    else return UpdateResult.ParameterNull;
                }
                //parameter is not null
                else
                {
                    //whether parameter's value is empty
                    bool parameterValueIsEmpty = IsParameterValueEmpty(param);
                    //whether value is empty
                    bool valueIsEmpty = IsValueEmpty(param.StorageType, paramValue);

                    //parameter's value is empty
                    if (parameterValueIsEmpty)
                    {
                        if (valueIsNull || valueIsEmpty) return UpdateResult.Equals;

                        //parameter is read-only
                        if (param.IsReadOnly) return UpdateResult.ReadOnlyFailed;
                        
                        //value is not empty, set the parameter with the value
                        bool res1 = SetParameter(param, paramValue);
                        return res1 ? UpdateResult.Success : UpdateResult.Failed;
                    }
                    //parameter's value is not empty
                    else
                    {
                        //value is null or empty
                        if (valueIsNull || valueIsEmpty)
                        {
                            if (param.IsReadOnly) return UpdateResult.ReadOnlyFailed;

                            // value is empty or null, set the parameter value to empty
                            bool res1 = SetParameterValueToEmpty(param);
                            return res1 ? UpdateResult.Success : UpdateResult.Failed;
                        }
                        //value is not null or empty and parameter's value is not empty
                        else
                        {
                            //verify whether value equals to parameter's value
                            bool valueEquals = false;
                            switch (param.StorageType)
                            {
                                case StorageType.Double:
                                    double doubleValue = param.AsDouble();
                                    try
                                    {
                                        double unitValue = Unit.CovertFromAPI(param.DisplayUnitType, doubleValue);
                                        valueEquals = unitValue.Equals((Double)paramValue);
                                    }
                                    catch
                                    {
                                        valueEquals = doubleValue.Equals((Double)paramValue);
                                    }
                                    break;
                                case StorageType.ElementId:
                                    valueEquals = param.AsElementId().Value.Equals((int)paramValue);
                                    break;
                                case StorageType.Integer:
                                    valueEquals = param.AsInteger().Equals((int)paramValue);
                                    break;
                                case StorageType.None:
                                    string value = param.AsValueString();
                                    if (value == null)
                                        valueEquals = (paramValue == null || paramValue == DBNull.Value) ? true : false;
                                    else
                                        valueEquals = value.Equals(paramValue.ToString());
                                    break;
                                case StorageType.String:
                                    string value2 = param.AsString();
                                    if (value2 == null)
                                        valueEquals = (paramValue == null || paramValue == DBNull.Value) ? true : false;
                                    else
                                        valueEquals = value2.Equals(paramValue.ToString());
                                    break;
                                default:
                                    break;
                            }

                            // if equals return Equals
                            if (valueEquals) return UpdateResult.Equals;
                            // otherwise
                            else
                            {
                                //if it is read-only
                                if (param.IsReadOnly) return UpdateResult.ReadOnlyFailed;
                                //otherwise set the parameter
                                bool res2 = SetParameter(param, paramValue);
                                return res2 ? UpdateResult.Success : UpdateResult.Failed;
                            }
                        }
                    }
                }
            }
            catch
            {
                // if exception throws, set result to Exception
                return UpdateResult.Exception;
            }
        }

        /// <summary>
        /// Whether a value which belongs to a parameter equals some specific empty value
        /// </summary>
        /// <param name="storageType">StorageType of the corresponding parameter</param>
        /// <param name="paramValue">value of the parameter</param>
        /// <returns>true if yes, otherwise false</returns>
        private bool IsValueEmpty(StorageType storageType, object paramValue)
        {
            if (paramValue == null || paramValue == DBNull.Value) return true;
            switch (storageType)
            {
                case StorageType.Double:
                    return (double)paramValue == paraDoubleNull;
                case StorageType.ElementId:
                    return (int)paramValue == paraElementIdValueNull;
                case StorageType.Integer:
                    return (int)paramValue == paraIntegerNull;
                case StorageType.None:
                    return paramValue.ToString() == paraStringNull;
                case StorageType.String:
                    return string.IsNullOrEmpty(paramValue.ToString());
                default:
                    return false;
            }
        }

        /// <summary>
        /// Whether the value of a parameter equals some specific empty value
        /// </summary>
        /// <param name="parameter">Parameter to verify</param>
        /// <returns>true if yes, otherwise false</returns>
        private bool IsParameterValueEmpty(Parameter parameter)
        {
            switch (parameter.StorageType)
            {
                case StorageType.Double:
                    return parameter.AsDouble() == paraDoubleNull;
                case StorageType.ElementId:
                    return (parameter.AsElementId().Value == paraElementIdValueNull);
                case StorageType.Integer:
                    return (parameter.AsInteger() == paraIntegerNull);
                case StorageType.None:
                    string val = parameter.AsValueString();
                    return (string.IsNullOrEmpty(val));
                case StorageType.String:
                    val = parameter.AsString();
                    return (string.IsNullOrEmpty(val));
                default:
                    return false;
            }
        }

        /// <summary>
        /// Set parameter to some specific empty value
        /// </summary>
        /// <param name="param">Parameter to set</param>
        /// <returns>true if set successfully otherwise false</returns>
        private bool SetParameterValueToEmpty(Parameter param)
        {
            switch (param.StorageType)
            {
                case StorageType.Double:
                    return param.Set(paraDoubleNull);
                case StorageType.ElementId:
                    ElementId eid;
                    eid.Value = paraElementIdValueNull;
                    return param.Set(ref eid);
                case StorageType.Integer:
                    return param.Set(paraIntegerNull);
                case StorageType.None:
                    return param.Set(paraStringNull);
                case StorageType.String:
                    return param.Set(paraStringNull);
                default:
                    return false;
            }
        }

        /// <summary>
        /// Set parameter to a designated value
        /// </summary>
        /// <param name="parameter">Parameter to set</param>
        /// <param name="value">value to set to</param>
        /// <returns>true if set successfuly otherwise false</returns>
        private bool SetParameter(Parameter parameter, object value)
        {
            switch (parameter.StorageType)
            {
                case StorageType.Double:
                    try
                    {
                        Double apiValue = Unit.CovertToAPI((double)value, parameter.DisplayUnitType);
                        return parameter.Set(apiValue);
                    }
                    catch
                    {
                        return parameter.Set((Double)value);
                    }
                case StorageType.ElementId:
                    ElementId eId = new ElementId();
                    eId.Value = (int)value;
                    return parameter.Set(ref eId);
                case StorageType.Integer:
                    return parameter.Set((int)value);
                case StorageType.None:
                    return parameter.SetValueString(value.ToString());
                case StorageType.String:
                    return parameter.Set(value.ToString());
                default:
                    return false;
            }
        }

        /// <summary>
        /// Whether the parameter value is null or DBNull
        /// </summary>
        /// <param name="paramValue">Parameter value</param>
        /// <returns>true if yes otherwise false</returns>
        private bool IsNullOrDBNull(object paramValue)
        {
            return paramValue == null || paramValue == DBNull.Value;
        }

        /// <summary>
        /// Verify whether an element belongs to a specific table.
        /// </summary>
        /// <param name="element">element to verify.</param>
        /// <returns>true if the element belongs to the table, otherwise false.</returns>
        protected override bool CanAppend(Element element)
        {
            BuiltInCategory bic = GetCategoryId(element);
            ElementType et = element is Symbol ? ElementType.SYMBOL : ElementType.INSTANCE;
            return TableInfo.TableId.Equals("TabN_" + bic.ToString() + "_" + et.ToString());
        }
        #endregion
    };
}
