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
using Autodesk.Revit.Enums;

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
        protected ElementSet Elements
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
        /// Clear the element list.
        /// </summary>
        public void Clear()
        {
            m_elements.Clear();
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
        /// Import data from DataTable
        /// </summary>
        /// <param name="dataTable">DataTable to import</param>
        public override void Import(DataTable dataTable)
        {
            if (dataTable == null) return;
            DataTable = dataTable;

            //represents elements that appears in Revit but not in database
            ElementSet elementsNotInDb = new ElementSet();
            foreach (Element elem in m_elements)
            {
                elementsNotInDb.Insert(elem);
            }

            ElementId eId;
            DataRow row = null;

            //the following code is to iterate all rows to find existing records to modify elements
            //and new records to create elements.
            for (int rowIndex = 0; rowIndex < DataTable.Rows.Count; rowIndex++)
            {
                row = DataTable.Rows[rowIndex];
                //get id of an element
                int idInDb = (int)row["Id"];
                eId.Value = idInDb;
                //get element using its id
                Element element = ActiveDocument.get_Element(ref eId);
                //verify whether the element exists
                if (element != null && CanAppend(element))
                {
                    // if this element exists, remove it from elementsNotInDb.
                    elementsNotInDb.Erase(element);
                    // And update it. If update element failed, update database.
                    UpdateElement(element, rowIndex);
                }
                else
                {
                    // if this element does not exist, create one
                    try
                    {
                        // just symbol can be duplicated.
                        element = CreateElement(row);
                    }
                    catch
                    {
                        element = null;
                    }
                    // if create successfully
                    if (element != null)
                    {
                        // update the row and add it to the pending row list which will be updated
                        // at the end of importing.
                        AddToUpdateList(element, rowIndex);
                        // add it to the element set
                        m_elements.Insert(element);
                    }
                    else
                    {
                        // if create failed, remove the record to garbage row list which will be 
                        // deleted at the end of importing.
                        DeleteDbRow(rowIndex);
                    }
                }
                if (element != null)
                {
                    if (elementsNotInDb.Contains(element))
                    {
                        elementsNotInDb.Erase(element);
                    }
                }
            }
            //if there are some elements don't exist in database, insert them to database
            if (elementsNotInDb.Size > 0)
            {
                foreach (Element elem in elementsNotInDb)
                {
                    InsertDbRow(elem, false);
                }
            }
            elementsNotInDb.Dispose();
        }

        /// <summary>
        /// Verify whether an element belongs to the specific table.
        /// </summary>
        /// <param name="element">element to verify.</param>
        /// <returns>true if the element belongs to the table, otherwise false.</returns>
        abstract protected bool CanAppend(Element element);

        /// <summary>
        /// Update element. if update element failed, update database.
        /// </summary>
        /// <param name="element">element to update</param>
        /// <param name="rowIndex">index of DataRow in DataTable related with the element</param>
        abstract protected void UpdateElement(Element element, int rowIndex);

        /// <summary>
        /// Populate data of an element into a DataRow.
        /// </summary>
        /// <param name="element">element to retrieve data from.</param>
        /// <param name="row">DataRow to populate.</param>
        abstract protected void PopulateDbRow(Element element, DataRow row);

        /// <summary>
        /// Create an element.
        /// </summary>
        /// <param name="row">DataRow which contains the information to create element.</param>
        /// <returns>the created element.</returns>
        virtual protected Element CreateElement(DataRow row)
        {
            return null;
        }

        /// <summary>
        /// Delete database row.
        /// </summary>
        /// <param name="rowIndex">row index</param>
        protected void DeleteDbRow(int rowIndex)
        {
            GarbageRows.Add(DataTable.Rows[rowIndex]);
        }

        /// <summary>
        /// Update the row and add it to the pending row list which will be updated
        /// at the end of importing.
        /// </summary>
        /// <param name="element">element used to update DataRow</param>
        /// <param name="rowIndex">index of DataRow in DataTable</param>
        private void AddToUpdateList(Element element, int rowIndex)
        {
            DataRow row = InsertDbRow(element, true);
            PendingRows.Add(row, DataTable.Rows[rowIndex]);
        }

        /// <summary>
        /// Create a new DataRow preserves the element data, if the element is an existing one, 
        /// add the DataRow to DataTable.
        /// </summary>
        /// <param name="element">element used to populate DataRow.</param>
        /// <param name="isElementCreated">true if the element is a new created element by API.</param>
        /// <returns>Created DataRow</returns>
        private DataRow InsertDbRow(Element element, bool isElementCreated)
        {
            DataRow row = DataTable.NewRow();
            if (!isElementCreated) DataTable.Rows.Add(row);
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
                    if (parameter.Definition.Name.Equals("Assembly Code"))
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
            if (element != null) return DBNull.Value;
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
        protected static Parameter GetParameterByDefinitionName(Element element, string name)
        {
            if (element == null) return null;
            foreach (Parameter param in element.Parameters)
            {
                if (param.Definition.Name.Equals(name))
                {
                    return param;
                }
            }
            return null;
        } 
        #endregion
    };
}
