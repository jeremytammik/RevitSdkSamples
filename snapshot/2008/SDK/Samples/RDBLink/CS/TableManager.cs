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
    /// Provides functions to validate table existence, create tables and append new columns.
    /// </summary>
    public class TableManager
    {
        #region Fields
        /// <summary>
        /// New tables will be created.
        /// </summary>
        string[] m_newTables;

        /// <summary>
        /// Old tables that has already been created.
        /// </summary>
        string[] m_oldTables;

        /// <summary>
        /// Old tables which has already existed and we will not export data into.
        /// </summary>
        string[] m_ignoredTables;

        /// <summary>
        /// Indicate SQL server or other database.
        /// </summary>
        bool m_isSQLServer;

        /// <summary>
        /// Ignored tables which also exist in database
        /// </summary>
        List<string> m_ignoredButExistedTables = new List<string>();

        /// <summary>
        /// Connection to database.
        /// </summary>
        OdbcConnection m_connection;

        /// <summary>
        /// OdbcCommand.
        /// </summary>
        OdbcCommand m_dbCommand; 
        #endregion

        #region Properties
        /// <summary>
        /// Indicate SQL server or other database.
        /// </summary>
        public bool IsSQLServer
        {
            get 
            { 
                return m_isSQLServer; 
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes variables.
        /// </summary>
        public TableManager()
        {
            m_newTables = new string[]{
                "ElementLevel",
                "ElementPhase",
                "DoorWall",
                "WindowWall",
                "RoomTags",
                "Categories",
                "Openings",
                "LineLoadOnBeam",
                "AreaLoadOnSlab"
            };

            m_oldTables = new string[]{
                "Columns",//will be "Columns1" in SQLServer
                "AreaSchemes",
                "AssemblyCodes",
                "Constructions1",
                "DesignOptions",
                "Materials",
                "Phases",
                "ProjectInformation",
                "AirTerminalTypes",
                "AreaLoads",
                "CaseworkTypes",
                "CeilingTypes",
                "ColumnTypes",
                "CommunicationDeviceTypes",
                "Constructions",
                "CurtainPanelTypes",
                "CurtainSystemTypes",
                "CurtainWallMullionTypes",
                "DataDeviceTypes",
                "DemandFactorTypes",
                "DesignOptionSets",
                "DistributionSystemTypes",
                "DoorTypes",
                "DuctAccessoryTypes",
                "DuctFittingTypes",
                "DuctSystems",
                "DuctTypes",
                "ElectricalCircuits",
                "ElectricalEquipmentTypes",
                "ElectricalFixtureTypes",
                "FireAlarmDeviceTypes",
                "FlexDuctTypes",
                "FlexPipeTypes",
                "FloorTypes",
                "FurnitureSystemTypes",
                "FurnitureTypes",
                "GenericModelTypes",
                "InternalAreaLoads",
                "InternalLineLoads",
                "InternalPointLoads",
                "Levels",
                "LightingDeviceTypes",
                "LightingFixtureTypes",
                "LineLoads",
                "MaterialQuantities",
                "MechanicalEquipmentTypes",
                "NurseCallDeviceTypes",
                "ParkingTypes",
                "PipeAccessoryTypes",
                "PipeFittingTypes",
                "PipeTypes",
                "PipingSystems",
                "PlantingTypes",
                "PlumbingFixtureTypes",
                "PointLoads",
                "Profiles",
                "PropertyLineTypes",
                "RailingTypes",
                "RampTypes",
                "RoofTypes",
                "SecurityDeviceTypes",
                "SiteTypes",
                "SpecialtyEquipmentTypes",
                "SprinklerTypes",
                "StairTypes",
                "StructuralColumnTypes",
                "StructuralFoundationTypes",
                "StructuralFramingTypes",
                "StructuralFramingTypes1",
                "StructuralRebarTypes",
                "StructuralStiffenerTypes",
                "SwitchSystem",
                "TelephoneDeviceTypes",
                "TopographyTypes",
                "VoltageTypes",
                "WallTypes",
                "WindowTypes",
                "WireTypes",
                "AirTerminals",
                "Areas",
                "Casework",
                "Ceilings",
                "CommunicationDevices",
                "CurtainPanels",
                "CurtainSystems",
                "CurtainWallMullions",
                "DataDevices",
                "DemandFactors",
                "DistributionSystems",
                "Doors",
                "DuctAccessories",
                "DuctFittings",
                "Ducts",
                "ElectricalEquipment",
                "ElectricalFixtures",
                "FasciaTypes",
                "FireAlarmDevices",
                "FlexDucts",
                "FlexPipes",
                "Floors",
                "Furniture",
                "FurnitureSystems",
                "GenericModels",
                "GutterTypes",
                "LightingDevices",
                "LightingFixtures",
                "MechanicalEquipment",
                "NurseCallDevices",
                "Parking",
                "PipeAccessories",
                "PipeFittings",
                "Pipes",
                "Planting",
                "PlumbingFixtures",
                "PropertyLines",
                "Railings",
                "Ramps",
                "Roofs",
                "Rooms",
                "SecurityDevices",
                "Site",
                "SlabEdgeTypes",
                "SpecialtyEquipment",
                "Sprinklers",
                "Stairs",
                "StructuralColumns",
                "StructuralFoundations",
                "StructuralFraming",
                "StructuralRebar",
                "StructuralStiffeners",
                "StructuralTrusses",
                "TelephoneDevices",
                "Topography",
                "Voltages",
                "Walls",
                "WallSweeps",
                "WallSweepTypes",
                "Windows",
                "Wires",
                "Fascias",
                "Gutters",
                "RoomAssociations",
                "RoomFromToAssociations",
                "SlabEdges",
            };

            m_ignoredTables = new string[]{
                "MaterialQuantities"
            };
        } 
        #endregion

        #region Methods
        /// <summary>
        /// Whether a table exists.
        /// </summary>
        /// <param name="connection">OdbcConnection which connects a database.</param>
        /// <param name="tableName">table name.</param>
        /// <returns>true if exists, otherwise false.</returns>
        static public bool TableExists(OdbcConnection connection, string tableName)
        {
            if (connection == null || connection.State != ConnectionState.Open) return false;
            return connection.GetSchema("Tables",
                new string[] { null, null, tableName }).Rows.Count > 0;
        }

        /// <summary>
        /// Sets the OdbcConnection and OdbcCommand. Sets some table names and columns
        /// according to different data source.
        /// </summary>
        /// <param name="dbConnection">OdbcConnection</param>
        /// <param name="dbCommand">OdbcCommand</param>
        public void SetConnection(OdbcConnection dbConnection, OdbcCommand dbCommand)
        {
            m_connection = dbConnection;
            m_dbCommand = dbCommand;

            // In MS-Access, table name "Columns" is allowed but not in SQLServer
            // we modify it to "Columns1" if using SQLServer.
            m_isSQLServer = TableExists(m_connection, "Columns1");

            // get the columns whose name need to change according to different data source
            List<ColumnInfo> sprinklerTypesCols = 
                TableInfoSet.FilterTableInfoMap()[new Filter(BuiltInCategory.OST_Sprinklers, ElementType.SYMBOL)].Columns;
            List<ColumnInfo> topographyCols = 
                TableInfoSet.FilterTableInfoMap()[new Filter(BuiltInCategory.OST_Topography, ElementType.INSTANCE)].Columns;
            TableInfo columnsTableInfo =
                TableInfoSet.FilterTableInfoMap()[new Filter(BuiltInCategory.OST_Columns, ElementType.INSTANCE)];

            if (m_isSQLServer)
            {
                m_oldTables[0] = "Columns1";
                columnsTableInfo.Name = "Columns1";

                // In MS-Access, the column name "K-Factor" is available 
                // but in SQLServer, we have to remove the characters '-' and '/' etc.
                foreach (ColumnInfo col in sprinklerTypesCols)
                {
                    if (col.Name.Equals("K-Factor"))
                    {
                        col.Name = "KFactor";
                    }
                }

                foreach (ColumnInfo col in topographyCols)
                {
                    if (col.Name.Equals("Netcut/fill"))
                    {
                        col.Name = "Netcutfill";
                    }
                }
            }
            else
            {
                m_oldTables[0] = "Columns";
                columnsTableInfo.Name = "Columns";

                foreach (ColumnInfo col in sprinklerTypesCols)
                {
                    if (col.Name.Equals("KFactor"))
                    {
                        col.Name = "K-Factor";
                    }
                }

                foreach (ColumnInfo col in topographyCols)
                {
                    if (col.Name.Equals("Netcutfill"))
                    {
                        col.Name = "Netcut/fill";
                    }
                }
            }
        }

        /// <summary>
        /// Checks whether original needed tables are existent.
        /// </summary>
        /// <returns>True if check successfully, otherwise false</returns>
        public bool CheckOldTables()
        {
            // get all existent tables from database.
            List<string> allTables = new List<string>();
            DataTable dt = m_connection.GetSchema("Tables", new string[] { null, null, null });
            foreach (DataRow row in dt.Rows)
            {
                allTables.Add((string)row["TABLE_NAME"]);
            }

            // check whether old tables exist.
            foreach (string table in m_oldTables)
            {
                if (!allTables.Contains(table))
                {
                    if(table.Equals("Constructions1") && !allTables.Contains("ConstructionTypes"))
                    {
                        Trace.WriteLine(string.Format("Table '{0}' does not exist", table));
                        return false;
                    }
                }
            }

            // find ignored but existent tables.
            foreach (string table in m_ignoredTables)
            {
                if (allTables.Contains(table))
                {
                    m_ignoredButExistedTables.Add(table);
                }
            }
            return true;
        }

        /// <summary>
        /// Check new tables, if not exist create them.
        /// </summary>
        /// <returns>true if all new tables exist, otherwise false.</returns>
        public bool CheckNewTables()
        {
            if (!TableExists(m_connection, "ElementLevel"))
                CreateTable("CREATE TABLE ElementLevel (ElementId INTEGER, ElementName TEXT, LevelId INTEGER, LevelName TEXT, PRIMARY KEY(ElementId))");

            if (!TableExists(m_connection, "ElementPhase"))
                CreateTable("CREATE TABLE ElementPhase (ElementId INTEGER, ElementName TEXT, PhaseCreatedId INTEGER, PhaseName TEXT, PRIMARY KEY(ElementId))");

            if (!TableExists(m_connection, "DoorWall"))
                CreateTable("CREATE TABLE DoorWall (DoorId INTEGER, DoorName TEXT, WallId INTEGER, WallName TEXT, PRIMARY KEY(DoorId), CONSTRAINT constraint_DoorWall_DoorId FOREIGN KEY(DoorId) REFERENCES Doors (Id),CONSTRAINT constraint_DoorWall_WallId FOREIGN KEY(WallId) REFERENCES Walls (Id))");

            if (!TableExists(m_connection, "WindowWall"))
                CreateTable("CREATE TABLE WindowWall (WindowId INTEGER, WindowName TEXT, WallId INTEGER, WallName TEXT, PRIMARY KEY(WindowId), CONSTRAINT constraint_WindowWall_WindowId FOREIGN KEY(WindowId) REFERENCES Windows (Id),CONSTRAINT constraint_WindowWall_WallId FOREIGN KEY(WallId) REFERENCES Walls (Id))");

            if (!TableExists(m_connection, "RoomTags"))
                CreateTable("CREATE TABLE RoomTags (Id INTEGER, RoomTagType TEXT, RoomId INTEGER, ViewId INTEGER, PRIMARY KEY(Id))");

            if (!TableExists(m_connection, "Categories"))
                CreateTable("CREATE TABLE Categories (Id INTEGER, Name TEXT, MaterialId INTEGER, PRIMARY KEY(Id),CONSTRAINT constraint_Categories_MaterialId FOREIGN KEY(MaterialId) REFERENCES Materials (Id))");

            if (!TableExists(m_connection, "Openings"))
                CreateTable("CREATE TABLE Openings (Id INTEGER, Name TEXT, LevelId INTEGER, HostId INTEGER, PRIMARY KEY(Id))");

            if (!TableExists(m_connection, "LineLoadOnBeam"))
                CreateTable("CREATE TABLE LineLoadOnBeam (LineLoadId INTEGER, BeamId INTEGER, PRIMARY KEY(LineLoadId),CONSTRAINT constraint_LineLoadOnBeam_LineLoadId FOREIGN KEY(LineLoadId) REFERENCES LineLoads (Id),CONSTRAINT constraint_LineLoadOnBeam_BeamId FOREIGN KEY(BeamId) REFERENCES StructuralFraming (Id))");

            if (!TableExists(m_connection, "AreaLoadOnSlab"))
                CreateTable("CREATE TABLE AreaLoadOnSlab (AreaLoadId INTEGER, SlabId INTEGER, PRIMARY KEY(AreaLoadId),CONSTRAINT constraint_AreaLoadOnSlab_AreaLoadId FOREIGN KEY(AreaLoadId) REFERENCES AreaLoads (Id),CONSTRAINT constraint_AreaLoadOnSlab_SlabId FOREIGN KEY(SlabId) REFERENCES Floors (Id))");

            return true;
        }

        /// <summary>
        /// Check shared or project parameter columns, if not exist create them.
        /// </summary>
        /// <param name="revitDocument">Revit Document object.</param>
        /// <returns>A map that list table name and its shared parameter column names.</returns>
        public Dictionary<string, List<string>> CheckSharedParameterColumns(Document revitDocument)
        {
            Dictionary<string, List<string>>
                tableNameExtraColumnsMap = new Dictionary<string, List<string>>();
            BindingMap bindingMap = revitDocument.ParameterBindings;
            //m_sharedParas.Clear();
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
                string columnType = "TEXT";
                ParameterType paraType = ParameterType.Invalid;
                try
                {
                    paraType = itor.Key.ParameterType;
                }
                catch
                {
                    continue;
                }

                switch (paraType)
                {
                    case ParameterType.YesNo:
                    case ParameterType.Integer:
                    case ParameterType.Material:
                    case ParameterType.Invalid:
                    case ParameterType.Number:
                        columnType = ColumnInfo.INTEGER;
                        break;
                    case ParameterType.URL:
                    case ParameterType.Text:
                        columnType = ColumnInfo.VARCHAR;
                        break;
                    case ParameterType.Angle:
                    case ParameterType.Volume:
                    case ParameterType.Area:
                    case ParameterType.Force:
                    case ParameterType.AreaForce:
                    case ParameterType.Length:
                    case ParameterType.LinearForce:
                    case ParameterType.Moment:
                        columnType = ColumnInfo.DOUBLE;
                        break;
                    default:
                        break;
                }

                // get all categories which this parameter binds to
                CategorySet categories = binding.Categories;
                foreach (Category category in categories)
                {
                    // this line is needed, if not, it will throw exception when get its id.
                    if (category == null) continue;

                    Filter filter = new Filter((BuiltInCategory)category.Id.Value,
                        binding is TypeBinding ? ElementType.SYMBOL : ElementType.INSTANCE);
                    if (TableInfoSet.FilterTableInfoMap().ContainsKey(filter))
                    {
                        // if this column conflict with built-in-columns, ignore this column
                        TableInfo tableInfo = TableInfoSet.FilterTableInfoMap()[filter];
                        bool columnExists = false;
                        foreach (ColumnInfo colInfo in tableInfo.Columns)
                        {
                            if (colInfo.Name.Equals(columnName))
                            {
                                columnExists = true;
                                break;
                            }
                        }
                        if (columnExists) continue;

                        // add an extra column to a table
                        string tableName = tableInfo.Name;
                        if (!tableNameExtraColumnsMap.ContainsKey(tableName))
                        {
                            tableNameExtraColumnsMap.Add(tableName, new List<string>());
                        }
                        tableNameExtraColumnsMap[tableName].Add(columnName);

                        // if column related with this parameter does not exist, create it
                        if (!ColumnExists(tableName, columnName))
                        {
                            string cmdStr = "ALTER  TABLE " + tableName +
                                " ADD " + columnName + " " + columnType;
                            m_dbCommand.CommandText = cmdStr;
                            m_dbCommand.ExecuteNonQuery();
                        }
                    }
                }
            }
            return tableNameExtraColumnsMap;
        }

        /// <summary>
        /// Clears all ignored tables.
        /// </summary>
        /// <returns></returns>
        public bool ClearIgnoredTables()
        {
            // get deleteableCount
            int deleteableCount = m_ignoredButExistedTables.Count;
            // begin delete
            int deletedCount = 0;
            int lastDeletedCount = 0;
            while (deletedCount != deleteableCount)
            {
                lastDeletedCount = deletedCount;
                deletedCount = 0;
                foreach (string table in m_ignoredButExistedTables)
                {
                    try
                    {
                        m_dbCommand.CommandText = "DELETE FROM " + table;
                        m_dbCommand.ExecuteNonQuery();
                        deletedCount++;
                    }
                    catch
                    {
                        // deleting table that has relationship with other tables may cause exception,
                        // we just ignore it.
                    }
                }
                // going to limitless loops
                if (lastDeletedCount == deletedCount && deletedCount != deleteableCount)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Clears all tables related with Revit in database.
        /// </summary>
        /// <returns>true if clear success, otherwise false.</returns>
        public bool ClearTables()
        {
            // get deleteableCount
            int deleteableCount = m_newTables.Length + m_oldTables.Length + m_ignoredTables.Length;
            // begin delete
            int deletedCount = 0;
            int lastDeletedCount = 0;
            while (deletedCount != deleteableCount)
            {
                lastDeletedCount = deletedCount;
                deletedCount = 0;
                foreach (string tableName in m_newTables)
                {
                    try
                    {
                        m_dbCommand.CommandText = "DELETE FROM " + tableName;
                        m_dbCommand.ExecuteNonQuery();
                        deletedCount++;
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(ex.ToString());
                    }
                }

                foreach (string tableName in m_oldTables)
                {
                    try
                    {
                        m_dbCommand.CommandText = "DELETE FROM " + tableName;
                        m_dbCommand.ExecuteNonQuery();
                        deletedCount++;
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(ex.ToString());
                    }
                }

                foreach (string tableName in m_ignoredTables)
                {
                    try
                    {
                        m_dbCommand.CommandText = "DELETE FROM " + tableName;
                        m_dbCommand.ExecuteNonQuery();
                        deletedCount++;
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(ex.ToString());
                    }
                }

                // going to limitless loops
                if (lastDeletedCount == deletedCount && deletedCount != deleteableCount)
                {
                    return false;
                }
            }
            return true;
        } 

        /// <summary>
        /// Creates a table using the creation sql statement.
        /// </summary>
        /// <param name="creationCommand">sql statement to create a new table in database.</param>
        private void CreateTable(string creationCommand)
        {
            m_dbCommand.CommandText = creationCommand;
            m_dbCommand.ExecuteNonQuery();
        }

        /// <summary>
        /// string only contains "A-Z, a-z, 0-9" and does not start with numbers is allowed
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
        /// Whether there is a column belongs to a table in database.
        /// </summary>
        /// <param name="tableName">table name.</param>
        /// <param name="columnName">column name.</param>
        /// <returns>true if exists, otherwise false.</returns>
        private bool ColumnExists(string tableName, string columnName)
        {
            return m_connection.GetSchema("Columns",
                new string[] { null, null, tableName, columnName }).Rows.Count > 0;
        }

        #endregion
    }
}
