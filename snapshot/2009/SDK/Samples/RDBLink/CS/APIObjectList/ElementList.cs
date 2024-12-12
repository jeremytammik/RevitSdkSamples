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
using Autodesk.Revit.Enums;
using Autodesk.Revit.Elements;

namespace Revit.SDK.Samples.RDBLink.CS
{
    /// <summary>
    /// Provides export and import functions for Revit elements.
    /// </summary>
    public abstract class ElementList : APIObjectList
    {
        #region Fields
        /// <summary>
        /// Element set which is related with a specific table in database.
        /// </summary>
        private ElementSet m_elements = new ElementSet();
        #endregion

        #region Properties
        /// <summary>
        /// Gets the element list.
        /// </summary>
        public ElementSet Elements
        {
            get { return m_elements; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get the category id of an element
        /// </summary>
        /// <param name="element">element to get category id</param>
        /// <returns></returns>
        public static BuiltInCategory GetCategoryId(Element element)
        {
            if (element != null)
            {
                Parameter
                    catParameter = element.get_Parameter(BuiltInParameter.ELEM_CATEGORY_PARAM);
                if (catParameter != null)
                {
                    return (BuiltInCategory)catParameter.AsElementId().Value;
                }
            }
            return BuiltInCategory.INVALID;
        }

        /// <summary>
        /// Generate resource id for table name
        /// </summary>
        /// <param name="element">element</param>
        /// <param name="categoryId">category id of the element</param>
        /// <returns>resource id for the table the element belongs to</returns>
        public static List<string> GenerateResourceIdsForTableName(Element element, BuiltInCategory categoryId)
        {
            List<string> ids = new List<string>();
            // using element type
            ElementType eleType = element is Symbol ? ElementType.SYMBOL : ElementType.INSTANCE;
            ids.Add(string.Format("TabN_{0}_{1}", categoryId, eleType));

            // using element host relationship
            BuiltInCategory hostCategoryId = BuiltInCategory.INVALID;

            // if the element is a FamilyInstance
            bool hasHost = false;
            FamilyInstance fi = element as FamilyInstance;
            if (fi != null && fi.Host != null)
            {
                hostCategoryId = GetCategoryId(fi.Host);
                hasHost = true;
            }
            else
            {
                // if the element is a LoadBase (LineLoad, AreaLoad etc.)
                LoadBase load = element as LoadBase;
                if (load != null && load.HostElement != null)
                {
                    hostCategoryId = GetCategoryId(load.HostElement);
                    hasHost = true;
                }
                else
                {
                    // if the element is an opening
                    Opening opening = element as Opening;
                    if (opening != null && opening.Host != null)
                    {
                        hostCategoryId = GetCategoryId(opening.Host);
                        ids.Add(string.Format("TabN_{0}_{1}", "OST_Type_Opening", hostCategoryId));
                        ids.Add("TabN_Type_" + opening.GetType().FullName.Replace('.', '_'));
                    }
                    else
                    {
                        // if the element is a Rebar
                        Rebar rebar = element as Rebar;
                        if (rebar != null && rebar.Host != null)
                        {
                            hostCategoryId = GetCategoryId(rebar.Host);
                            hasHost = true;
                        }
                    }
                }
            }
            if(hasHost)
                ids.Add(string.Format("TabN_{0}_{1}", categoryId, hostCategoryId));

            // using System.Type
            if(element is RoomTag)
            {
                ids.Add("TabN_Type_" + element.GetType().FullName.Replace('.', '_'));
            }

            if(element != null && element.Id.Value != -1)
            {
                if(element.Level != null)
                    ids.Add("TabN_CST_ElementLevel");
                if(element.PhaseCreated != null)
                    ids.Add("TabN_CST_ElementPhase");
            }
            return ids;
        }

        /// <summary>
        /// Get id of an element
        /// </summary>
        /// <param name="element">element to get id</param>
        /// <returns>-1 if the element is null, otherwise its id</returns>
        public static int GetElementId(Element element)
        {
            if (element != null)
            {
                Parameter idParameter = element.get_Parameter(BuiltInParameter.ID_PARAM);
                if (idParameter != null)
                {
                    return idParameter.AsElementId().Value;
                }
            }
            return -1;
        }

        /// <summary>
        /// If the element belongs to the related table, then add it to the element set.
        /// </summary>
        /// <param name="element">element to try to append.</param>
        /// <returns>True if append successfully, otherwise false.</returns>
        public bool TryAppend(Element element)
        {
            if (CanAppend(element))
            {
                Append(element);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Add an element to element set.
        /// </summary>
        /// <param name="element">element to append</param>
        public void Append(Element element)
        {
            m_elements.Insert(element);
        }

        /// <summary>
        /// Export all data in element set into a DataTable
        /// </summary>
        /// <param name="dataTable">DataTable which revit data will be exported into</param>
        public override void Export(DataTable dataTable)
        {
            DataTable = dataTable;
            foreach (Element element in m_elements)
            {
                InsertDbRow(element, false);
            }
        }

        /// <summary>
        /// Verify whether an element belongs to a specific table.
        /// </summary>
        /// <param name="element">element to verify.</param>
        /// <returns>true if the element belongs to the table, otherwise false.</returns>
        abstract protected bool CanAppend(Element element);

        /// <summary>
        /// Populate data of an element into a DataRow.
        /// </summary>
        /// <param name="element">element to retrieve data from.</param>
        /// <param name="row">DataRow to populate.</param>
        abstract protected void PopulateDbRow(Element element, DataRow row);

        /// <summary>
        /// Create a new DataRow preserves the element data, if the element is an existing one, 
        /// add the DataRow to DataTable.
        /// </summary>
        /// <param name="element">element used to populate DataRow.</param>
        /// <param name="isElementCreated">true if the element is a new created element by API.</param>
        /// <returns>Created DataRow</returns>
        private DataRow InsertDbRow(Element element, bool isElementCreated)
        {
            DataRow row = FindCorrespondingRow(element);
            if (row == null)
            {
                row = DataTable.NewRow();
                if (!isElementCreated) DataTable.Rows.Add(row);
            }

            PopulateDbRow(element, row);
            return row;
        }

        /// <summary>
        /// Get the value of the parameter which can be inserted into database.
        /// </summary>
        /// <param name="parameter">parameter to retrieve data.</param>
        /// <returns>value of the parameter</returns>
        protected object GetParameterDdValue(Parameter parameter)
        {
            if (parameter == null) return DBNull.Value;
            object retval = null;
            switch (parameter.StorageType)
            {
                case StorageType.Double:
                    double tmpval = parameter.AsDouble();
                    try
                    {
                        DisplayUnitType dut = parameter.DisplayUnitType;
                        retval = Unit.CovertFromAPI(dut, tmpval);
                    }
                    catch
                    {
                        retval = tmpval;
                    }
                    break;
                case StorageType.ElementId:
                    int id = parameter.AsElementId().Value;
                    retval = (id == -1) ? retval = DBNull.Value : id;
                    break;
                case StorageType.Integer:
                    retval = parameter.AsInteger();
                    break;
                case StorageType.None:
                    retval = parameter.AsValueString();
                    break;
                case StorageType.String:
                    retval = parameter.AsString();
                    // currently we can't get all "Assembly Codes" in document using RevitAPI
                    // so we just ignore it.
                    if (parameter.Definition.Name.Equals(ColumnRes("Definition_Assembly_Code")))
                    {
                        retval = DBNull.Value;
                    }
                    break;
                default:
                    break;
            }

            return retval == null ? DBNull.Value : retval;
        }

        /// <summary>
        /// Get the value of an element id.
        /// </summary>
        /// <param name="element">element to get id.</param>
        /// <returns>DBNull.Value if element id equals -1 or the element is null, 
        /// otherwise its id.</returns>
        protected static object GetIdDbValue(Element element)
        {
            int id = GetElementId(element);
            return id != -1 ? (object)id : DBNull.Value;
        }

        /// <summary>
        /// Get the value of an element name.
        /// </summary>
        /// <param name="element">element to get name.</param>
        /// <returns>DBNull.Value if the element or its name is null, otherwise its name.</returns>
        protected static object GetNameDbValue(Element element)
        {
            if (element == null) return DBNull.Value;
            try
            {
                if (element.Name != null)
                {
                    return element.Name;
                }
            }
            catch
            {
                // element.Name may cause exception
                // when exception throws, it means this element does not have name.
            }
            return DBNull.Value;
        }

        /// <summary>
        /// Get a parameter of an element by definition name.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="name">definition name of the parameter.</param>
        /// <returns>Parameter if found, otherwise null.</returns>
        public static Parameter GetParameterByDefinitionName(Element element, string name)
        {
            if (element != null && element.ParametersMap.Contains(name))
            {
                return element.ParametersMap.get_Item(name);
            }
            return null;
        }

        /// <summary>
        /// Gather all redundant records of which there is no related object found in Revit.
        /// These records will be deleted during exporting
        /// </summary>
        /// <returns>Redundant records</returns>
        public override List<DataRow> CollectGarbageRows()
        {
            List<DataRow> result = new List<DataRow>();
            if (Elements.Size == 0)
            {
                foreach (DataRow row in DataTable.Rows)
                {
                    result.Add(row);
                }
            }
            else
            {
                foreach (DataRow row in DataTable.Rows)
                {
                    Element elem = FindCorrespondingObject(row) as Element;
                    if (!Elements.Contains(elem))
                    {
                        result.Add(row);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Get Revit model object from a given row.
        /// </summary>
        /// <param name="dataRow">row contains information to find an Revit object</param>
        /// <returns>An APIObject if found, otherwise null</returns>
        public override APIObject FindCorrespondingObject(DataRow dataRow)
        {
            int id = (int)dataRow[ColumnRes(TableInfo.PrimaryKeys[0])];
            ElementId eid; eid.Value = id;
            return ActiveDocument.get_Element(ref eid);
        }

        /// <summary>
        /// Get a value array from an APIObject, used to find its corresponding DataRow in DataTable 
        /// </summary>
        /// <param name="apiObject">Revit API Object</param>
        /// <returns>Object array</returns>
        protected override object[] GetPrimaryKeyValues(APIObject apiObject)
        {
            Element element = apiObject as Element;
            if (element != null)
            {
                return new object[] { GetIdDbValue(element) };
            }
            return new object[] { DBNull.Value };
        }

        #endregion

    };
}
