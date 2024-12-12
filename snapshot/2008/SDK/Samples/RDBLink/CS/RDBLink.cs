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
using System.Data.Odbc;
using Autodesk.Revit;
using Autodesk.Revit.Parameters;
using Autodesk.Revit.Collections;
using System.Data;
using System.Diagnostics;

namespace Revit.SDK.Samples.RDBLink.CS
{
    /// <summary>
    /// Preserves all APIObjectLists and can do export/import.
    /// </summary>
    public class RDBLink
    {
        #region Fields
        /// <summary>
        /// TableManager to do table creation, column creation and table validation tasks.
        /// </summary>
        TableManager m_tableManger;

        /// <summary>
        /// RoomAssociationList to do export and import related with the table "RoomAssociations".
        /// </summary>
        RoomAssociationList m_roomAssociationList = new RoomAssociationList();

        /// <summary>
        /// RoomFromToAssociationList to do export and import related with the table "RoomFromToAssociations".
        /// </summary>
        RoomFromToAssociationList m_roomFromToAssociationList = new RoomFromToAssociationList();

        /// <summary>
        /// OpeningList to do export and import related with the table "Openings".
        /// </summary>
        OpeningList m_openingList = new OpeningList();

        /// <summary>
        /// CategoryList to do export and import related with the table "Categories".
        /// </summary>
        CategoryList m_categoryList = new CategoryList();

        /// <summary>
        /// ElementLevelList to do export and import related with the table "ElementLevel".
        /// </summary>
        ElementLevelList m_elementLevelList = new ElementLevelList();

        /// <summary>
        /// ElementPhaseList to do export and import related with the table "ElementPhase".
        /// </summary>
        ElementPhaseList m_elementPhaseList = new ElementPhaseList();

        /// <summary>
        /// AreaLoadOnSlabList to do export and import related with the table "AreaLoadOnSlab".
        /// </summary>
        AreaLoadOnSlabList m_areaLoadOnSlabList = new AreaLoadOnSlabList();

        /// <summary>
        /// DoorWallList to do export and import related with the table "DoorWall".
        /// </summary>
        DoorWallList m_doorWallList = new DoorWallList();

        /// <summary>
        /// LineLoadOnBeamList to do export and import related with the table "LineLoadOnBeam".
        /// </summary>
        LineLoadOnBeamList m_lineLoadOnBeamList = new LineLoadOnBeamList();

        /// <summary>
        /// RoomTagList to do export and import related with the table "RoomTags".
        /// </summary>
        RoomTagList m_roomTagList = new RoomTagList();

        /// <summary>
        /// WindowWallList to do export and import related with the table "WindowWall".
        /// </summary>
        WindowWallList m_windowWallList = new WindowWallList();

        /// <summary>
        /// Dictionary stores Filter and ELementList map, which is used to quickly find 
        /// which element belongs to which table.
        /// </summary>
        Dictionary<Filter, ElementList>
            m_fileterElementListMap = new Dictionary<Filter, ElementList>();

        /// <summary>
        /// Dictionary stores table name and APIObjectList map, which is used to quickly find the
        /// APIObjectList using its table name.
        /// </summary>
        Dictionary<string, APIObjectList>
            m_tableNameObjectListMap = new Dictionary<string, APIObjectList>();

        /// <summary>
        /// List to store APIObjectLists, which is sorted by their table relationships, 
        /// the less dependencies it has, the topper position it will be.
        /// </summary>
        List<APIObjectList> m_objectLists = new List<APIObjectList>();

        /// <summary>
        /// DataSet to store data in database.
        /// </summary>
        DataSet m_dataSet = new DataSet("RevitDb");
        #endregion

        #region Properties
        /// <summary>
        /// Sets TableManager.
        /// </summary>
        public TableManager TableManger
        {
            set { m_tableManger = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes some APIObjectLists and some dictionaries.
        /// </summary>
        public RDBLink()
        {
            ElementList elementList = null;

            //using TableInfos to initializes APIObjectLists
            foreach (TableInfo tableInfo in TableInfoSet.Instance)
            {
                if (tableInfo.Filter.ElementType == ElementType.SYMBOL)
                {
                    elementList = new SymbolList(tableInfo);
                }
                else
                {
                    elementList = new InstanceList(tableInfo);
                }
                if (!m_tableNameObjectListMap.ContainsKey(tableInfo.Name))
                {
                    m_tableNameObjectListMap.Add(tableInfo.Name, elementList);
                    m_objectLists.Add(elementList);
                }
                if (!m_fileterElementListMap.ContainsKey(tableInfo.Filter))
                {
                    m_fileterElementListMap.Add(tableInfo.Filter, elementList);
                    m_objectLists.Add(elementList);
                }
            }

            //Add some APIObjectLists to maps.
            m_tableNameObjectListMap.Add(m_roomAssociationList.TableName, m_roomAssociationList);
            m_objectLists.Add(m_roomAssociationList);
            m_tableNameObjectListMap.Add(m_roomFromToAssociationList.TableName, m_roomFromToAssociationList);
            m_objectLists.Add(m_roomFromToAssociationList);
            m_tableNameObjectListMap.Add(m_categoryList.TableName, m_categoryList);
            m_objectLists.Add(m_categoryList);
            m_tableNameObjectListMap.Add(m_openingList.TableName, m_openingList);
            m_objectLists.Add(m_openingList);
            m_tableNameObjectListMap.Add(m_elementLevelList.TableName, m_elementLevelList);
            m_objectLists.Add(m_elementLevelList);
            m_tableNameObjectListMap.Add(m_elementPhaseList.TableName, m_elementPhaseList);
            m_objectLists.Add(m_elementPhaseList);
            m_tableNameObjectListMap.Add(m_areaLoadOnSlabList.TableName, m_areaLoadOnSlabList);
            m_objectLists.Add(m_areaLoadOnSlabList);
            m_tableNameObjectListMap.Add(m_doorWallList.TableName, m_doorWallList);
            m_objectLists.Add(m_doorWallList);
            m_tableNameObjectListMap.Add(m_lineLoadOnBeamList.TableName, m_lineLoadOnBeamList);
            m_objectLists.Add(m_lineLoadOnBeamList);
            m_tableNameObjectListMap.Add(m_roomTagList.TableName, m_roomTagList);
            m_objectLists.Add(m_roomTagList);
            m_tableNameObjectListMap.Add(m_windowWallList.TableName, m_windowWallList);
            m_objectLists.Add(m_windowWallList);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Iterates Revit elements to categorize them, also deals with some shared parameter columns.
        /// </summary>
        /// <param name="revitDocument">Revit Document.</param>
        /// <param name="tableNameExtraColumnsMap">a map stores information about which
        /// table has which extra columns appended.</param>
        public void PrepareRevitData(Document revitDocument,
            Dictionary<string, List<string>> tableNameExtraColumnsMap)
        {
            APIObjectList.SetRevitDocument(revitDocument);
            // iterate elements to categorize them to different APIObjectList.
            ElementIterator eit = revitDocument.Elements;

            // clear elements in the list.
            m_roomAssociationList.Clear();
            m_roomFromToAssociationList.Clear();
            m_elementLevelList.Clear();
            m_elementPhaseList.Clear();
            m_areaLoadOnSlabList.Clear();
            m_doorWallList.Clear();
            m_lineLoadOnBeamList.Clear();
            m_windowWallList.Clear();
            m_roomTagList.Clear();
            m_openingList.Clear();
            foreach(KeyValuePair<Filter, ElementList> pair in m_fileterElementListMap)
            {
                pair.Value.Clear();
            }

            eit.Reset();
            while (eit.MoveNext())
            {
                Element element = eit.Current as Element;
                // try to append this element to table "RoomAssociations".
                m_roomAssociationList.TryAppend(element);
                // try to append this element to table "RoomFromToAssociations".
                m_roomFromToAssociationList.TryAppend(element);
                // try to append this element to table "ElementLevel".
                m_elementLevelList.TryAppend(element);
                // try to append this element to table "ElementPhase".
                m_elementPhaseList.TryAppend(element);
                // try to append this element to table "AreaLoadOnSlab".
                m_areaLoadOnSlabList.TryAppend(element);
                // try to append this element to table "DoorWall".
                m_doorWallList.TryAppend(element);
                // try to append this element to table "LineLoadOnBeam".
                m_lineLoadOnBeamList.TryAppend(element);
                // try to append this element to table "WindowWall".
                m_windowWallList.TryAppend(element);



                // get the Filter related with the element.
                Filter filter = new Filter(ElementList.GetCategoryId(element),
                    element is Symbol ? ElementType.SYMBOL : ElementType.INSTANCE);
                // using this Filter to get the related APIObjectList.
                if (m_fileterElementListMap.ContainsKey(filter))
                {
                    // append the element into the APIObjectList.
                    m_fileterElementListMap[filter].Append(element);
                    continue;
                }

                // try to append this element to table "RoomTags".
                if (m_roomTagList.TryAppend(element)) continue;
                // try to append this element to table "Opening".
                if (m_openingList.TryAppend(element)) continue;
            }

            // In MS-Access, table name "Columns" is allowed but not in SQLServer
            // so we modify it to "Columns1" if using SQLServer.
            ElementList columnList = m_fileterElementListMap[
                new Filter(BuiltInCategory.OST_Columns, ElementType.INSTANCE)];
            if (m_tableManger.IsSQLServer)
            {
                columnList.TableName = "Columns1";
            }
            else
            {
                columnList.TableName = "Columns";
            }

            // notify the APIObjectLists that they have extra columns to populate.
            if (tableNameExtraColumnsMap != null)
            {
                // first, clear the map.
                foreach (KeyValuePair<string, APIObjectList> pair in m_tableNameObjectListMap)
                {
                    pair.Value.ClearExtraColumns();
                }
                // find the corresponding APIObjectList and 
                //add the columns to its extra column list.
                foreach (KeyValuePair<string, List<string>> pair in tableNameExtraColumnsMap)
                {
                    if (m_tableNameObjectListMap.ContainsKey(pair.Key))
                    {
                        m_tableNameObjectListMap[pair.Key].AppendColumns(pair.Value);
                    }
                }
            }
        }

        /// <summary>
        /// Begins import or export data.
        /// </summary>
        /// <param name="dbCommand">OdbcCommand used to create OdbcDataAdapter</param>
        /// <param name="export">true if export data, otherwise import.</param>
        public void BeginTransfer(OdbcCommand dbCommand, bool export)
        {
            // clear dataset.
            m_dataSet.Clear();
            // create DataAdapter
            OdbcDataAdapter dataAdapter = new OdbcDataAdapter(dbCommand);
            dataAdapter.RowUpdated += new OdbcRowUpdatedEventHandler(dataAdapter_RowUpdated);
            dataAdapter.RowUpdating += new OdbcRowUpdatingEventHandler(dataAdapter_RowUpdating);
            // binding a CommandBuilder to the DataAdapter
            OdbcCommandBuilder cmdBuilder = new OdbcCommandBuilder(dataAdapter);
            // add QuotePrefix and QuoteSuffix to avoid conflict with keywords in SQL statements.
            cmdBuilder.QuotePrefix = "["; cmdBuilder.QuoteSuffix = "]";

            foreach (KeyValuePair<string, APIObjectList> pair in m_tableNameObjectListMap)
            {
                APIObjectList apiObjectList = pair.Value;
                // select all data in a table.
                dbCommand.CommandText = "SELECT * FROM " + apiObjectList.TableName;
                // notify CommandBuilder that the CommandText has been changed.
                cmdBuilder.RefreshSchema();
                // initialize the dataAdapter, fill the data set of selected table.
                dataAdapter.Fill(m_dataSet, apiObjectList.TableName);
                // get the DataTable related with the APIObjectList.
                DataTable dt = m_dataSet.Tables[apiObjectList.TableName];
                if (dt == null) continue;

                // if export then do export otherwise do import.
                if (export)
                {
                    apiObjectList.Export(dt);
                }
                else
                {
                    apiObjectList.Import(dt);
                }
                // update the table in database.
                dataAdapter.Update(m_dataSet, apiObjectList.TableName);
            }
            //Commit all the changes to the DataSet
            m_dataSet.AcceptChanges();

            //Import only
            if (!export)
            {
                // clear tables that we would not deal with, which can remove its dependencies on 
                // other tables to make them updatable.
                if (m_tableManger != null) { m_tableManger.ClearIgnoredTables(); }

                // delete and add some DataRows by descending order.
                // because when delete and update, we have to begin from the tables that
                // have most dependencies.
                for (int reverseIndex = m_objectLists.Count - 1; reverseIndex >= 0;
                    reverseIndex--)
                {
                    // update some rows.
                    m_objectLists[reverseIndex].UpdatePendingRows();
                    // delete some rows.
                    m_objectLists[reverseIndex].ClearGarbageRows();

                    // commit the changes to the DataSet.
                    dbCommand.CommandText
                        = "SELECT * FROM " + m_objectLists[reverseIndex].TableName;
                    cmdBuilder.RefreshSchema();
                    dataAdapter.Update(m_dataSet, m_objectLists[reverseIndex].TableName);
                    Trace.Flush();
                }
                m_dataSet.AcceptChanges();
            }
        }

        /// <summary>
        /// Trace information before row updating.
        /// </summary>
        void dataAdapter_RowUpdating(object sender, OdbcRowUpdatingEventArgs e)
        {
            if (e.Row != null)
            {
                string msg = string.Format("... {0} {1} {2} ...",
                    (e.Row.Table == null ? "" : e.Row.Table.TableName),
                    e.Status,
                    e.StatementType);
                Trace.WriteLine(msg);
            }
        }

        /// <summary>
        /// Trace information after row updating.
        /// </summary>
        void dataAdapter_RowUpdated(object sender, OdbcRowUpdatedEventArgs e)
        {
            if (e.Row != null)
            {
                string msg = string.Format("[{0} {1}]",
                    e.Row.RowState,
                    e.Row.RowState == DataRowState.Deleted ? "": e.Row[0]);
                Trace.WriteLine(msg);
            }
        }
        #endregion
    }
}
