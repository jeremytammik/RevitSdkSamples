//
// (C) Copyright 2003-2015 by Autodesk, Inc.
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
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;

namespace Revit.SDK.Samples.RoomSchedule
{
    /// <summary>
    /// Iterates through the rooms in the project and get the information of all the rooms
    /// </summary>
    public class RoomsData
    {
        #region Class Constant Variables
        /// <summary>
        /// Constant name for RoomID, this column must exist in first row.
        /// </summary>
        public const String RoomID = "ID";

        /// <summary>
        /// Constant name for room area, this column must exist in first row
        /// </summary>
        public const String RoomArea = "Room Area";

        /// <summary>
        /// Constant named for room name, this column must exist in first row
        /// </summary>
        public const String RoomName = "Room Name";

        /// <summary>
        /// Constant name for room number, this column must exist in first row
        /// </summary>
        public const String RoomNumber = "Room Number";

        /// <summary>
        /// Constant name for room number, this column must exist in first row
        /// </summary>
        public const String RoomComments = "Room Comments";

        /// <summary>
        /// Constant name for shared parameter, 
        /// the mapped room id of spread sheet will saved in this parameter.
        /// </summary>
        public const String SharedParam = "External Room ID";
        #endregion


        #region Class Member Variables
        /// <summary>
        /// Active document to which this RoomsData instance belongs
        /// </summary>
        Document m_activeDocument;

        /// <summary>
        /// a list to store all rooms in the project
        /// </summary>
        List<Room> m_rooms = new List<Room>();

        /// <summary>
        /// parameters which will be displayed in DataGridView
        /// </summary>
        List<BuiltInParameter> m_parameters = new List<BuiltInParameter>();

        /// <summary>
        /// a list to store column names of Rooms
        /// </summary>
        List<String> m_columnNames = new List<string>();
        #endregion


        #region Class Properties
        /// <summary>
        /// A list of all the rooms in the project
        /// </summary>
        public ReadOnlyCollection<Room> Rooms
        {
            get
            {
                return new ReadOnlyCollection<Room>(m_rooms);
            }
        }
        #endregion


        #region Class Constructor Method
        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="activeDocument">Revit project.</param>
        public RoomsData(Document activeDocument)
        {
            m_activeDocument = activeDocument;

            // initialize the output parameters
            InitializeParameters();

            // get all the rooms in the project
            GetAllRooms(activeDocument);
        }
        #endregion


        #region Class Public Methods
        /// <summary>
        /// Update rooms data after room creation happens in Revit
        /// </summary>
        public void UpdateRoomsData()
        {
            // clear all rooms and re-retrieve data from Revit
            m_rooms.Clear();
            GetAllRooms(m_activeDocument);
        }


        /// <summary>
        /// Get all parameters to be displayed in DataGridView.
        /// </summary>
        /// <param name="specifiedParams">all parameters specified by user.</param>
        public void UpdateParameters(ReadOnlyCollection<BuiltInParameter> specifiedParams)
        {
            // if there is no instance, parameter setting is not allowed
            if (m_rooms.Count <= 0)
            {
                throw new Exception("No element instance to set parameters");
            }

            // clear old parameters data
            m_parameters.Clear();
            m_columnNames.Clear();

            // get column names of room by specified parameters
            Room firstRoom = m_rooms[0];
            foreach (BuiltInParameter param in specifiedParams)
            {
                // add this parameter
                m_parameters.Add(param);

                // store all specified parameter names of room.
                Parameter toomPara = firstRoom.get_Parameter(param);
                m_columnNames.Add(toomPara.Definition.Name);
            }
        }


        /// <summary>
        /// Generate all rooms which are located in specified level.
        /// A DataTable data object will be generated after this method call.
        /// </summary>
        /// <param name="level">the specified level to retrieve rooms</param>
        /// <returns>DataTable generated from rooms</returns>
        public DataTable GenRoomsDataTable(Level level)
        {
            // get all Rooms information and generate a DataTable 
            if (m_rooms.Count == 0)
            {
                return null;
            }

            // generate columns by all parameters            
            DataTable newTable = new DataTable();
            foreach (String col in m_columnNames)
            {
                DataColumn column = new DataColumn();
                column.ColumnName = col;
                column.ReadOnly = true;
                column.DataType = System.Type.GetType("System.String");
                newTable.Columns.Add(column);
            }

            // add constant column: External Room ID
            DataColumn constantCol = new DataColumn();
            constantCol.ColumnName = SharedParam;
            constantCol.ReadOnly = true;
            constantCol.DataType = System.Type.GetType("System.String");
            newTable.Columns.Add(constantCol);

            // filter rooms by level
            foreach (Room room in m_rooms)
            {
                // check whether room is located at specified level 
                if ((null == level) || (m_activeDocument.GetElement(room.LevelId) != null && room.LevelId.IntegerValue == level.Id.IntegerValue))
                {
                    DataRow dataRow = newTable.NewRow();
                    for (int i = 0; i < m_parameters.Count; i++)
                    {
                        dataRow[i] = GetProperty(m_activeDocument, room, m_parameters[i], true);
                    }

                    // add constant column value: External Room ID
                    Parameter param = null;
                    bool bExist = ShareParameterExists(room, SharedParam, ref param);
                    if (bExist && null != param && false == String.IsNullOrEmpty(param.AsString()))
                    {
                        dataRow[m_parameters.Count] = param.AsString();
                    }
                    else
                    {
                        dataRow[m_parameters.Count] = "<null>";
                    }

                    // add this row
                    newTable.Rows.Add(dataRow);
                }
            }

            return newTable;
        }


        /// <summary>
        /// Get the room property value according the parameter name
        /// </summary>
        /// <param name="activeDoc">Current active document.</param>
        /// <param name="room">an instance of room class</param>
        /// <param name="paraEnum">the parameter used to get parameter value</param>
        /// <param name="useValue">convert parameter to value type or not.
        /// if true, the value of parameter will be with unit.
        /// if false, the value of parameter will be without unit.</param>
        /// <returns>the string value of property specified by shared parameter</returns>
        public static String GetProperty(Document activeDoc, Room room, BuiltInParameter paraEnum, bool useValue)
        {
            String propertyValue = null;  //the value of parameter 

            // Assuming the build in parameter is legal for room.
            // if the room is not placed, some properties are not available, i.g. Level name, Area ...
            // trying to retrieve them will throw exception; 
            // however some parameters are available, e.g.: name, number
            Parameter param;
            try
            {
                param = room.get_Parameter(paraEnum);
            }
            catch (Exception)
            {
                // throwing exception for this parameter is acceptable if it's a unplaced room
                if (null == room.Location)
                {
                    propertyValue = "Not Placed";
                    return propertyValue;
                }
                else
                {
                    throw new Exception("Illegal built in parameter.");
                }
            }

            // get the parameter via the built in parameter
            if (null == param)
            {
                return "";
            }

            // get the parameter's storage type and convert parameter to string 
            StorageType storageType = param.StorageType;
            switch (storageType)
            {
                case StorageType.Integer:
                    int iVal = param.AsInteger();
                    propertyValue = iVal.ToString();
                    break;
                case StorageType.String:
                    propertyValue = param.AsString();
                    break;
                case StorageType.Double:
                    // AsValueString will make the return string with unit, it's appreciated.
                    if (useValue)
                    {
                        propertyValue = param.AsValueString();
                    }
                    else
                    {
                        propertyValue = param.AsDouble().ToString();
                    }
                    break;
                case StorageType.ElementId:
                    Autodesk.Revit.DB.ElementId elemId = param.AsElementId();
                    Element elem = activeDoc.GetElement(elemId);
                    propertyValue = elem.Name;
                    break;
                default:
                    propertyValue = param.AsString();
                    break;
            }

            return propertyValue;
        }

        /// <summary>
        /// Check to see whether specified parameter exists in room object.
        /// </summary>
        /// <param name="roomObj">Room object used to get parameter</param>
        /// <param name="paramName">parameter name to be checked</param>
        /// <param name="sharedParam">shared parameter returned</param>
        /// <returns>true, the parameter exists; false, the parameter doesn't exist</returns>
        public static bool ShareParameterExists(Room roomObj, String paramName, ref Parameter sharedParam)
        {
            // get the parameter
            try
            {
               sharedParam = roomObj.LookupParameter(paramName);
            }
            catch
            {
            }
            return (null != sharedParam);
        }
        #endregion


        #region Class Implementation
        /// <summary>
        /// Get all rooms in current Revit project
        /// </summary>
        private void GetAllRooms(Document activeDoc)
        {
            // get all room elements
            // try to find all rooms in the project and add to the list
            RoomFilter filter = new RoomFilter();
            FilteredElementCollector collector = new FilteredElementCollector(activeDoc);
            m_rooms = collector.WherePasses(filter).ToElements().Cast<Room>().ToList<Room>();
            // sort rooms by number
            m_rooms.Sort(CompRoomByNumber);
        }

        /// <summary>
        /// Sort the rooms by number
        /// </summary>
        /// <param name="room1"></param>
        /// <param name="room2"></param>
        /// <returns></returns>
        private static int CompRoomByNumber(Room room1, Room room2)
        {
            if (null == room1 || null == room2)
            {
                return -1;
            }
            return room1.Number.CompareTo(room2.Number);
        }

        /// <summary>
        /// Initialize the parameters displayed in DataGridView control
        /// </summary>
        private void InitializeParameters()
        {
            // Room name
            m_parameters.Add(BuiltInParameter.ROOM_NAME);
            m_columnNames.Add("Name");

            // Room Number
            m_parameters.Add(BuiltInParameter.ROOM_NUMBER);
            m_columnNames.Add("Number");

            // Room Area
            m_parameters.Add(BuiltInParameter.ROOM_AREA);
            m_columnNames.Add("Area");

            // Room Comments 
            m_parameters.Add(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS);
            m_columnNames.Add("Comments");

            // Level 
            m_parameters.Add(BuiltInParameter.LEVEL_NAME);
            m_columnNames.Add("Level");

            // Phase 
            m_parameters.Add(BuiltInParameter.ROOM_PHASE);
            m_columnNames.Add("Phase");
        }
        #endregion
    }
}
