//
// (C) Copyright 2003-2019 by Autodesk, Inc.
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
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Architecture;

namespace Revit.SDK.Samples.Rooms.CS
{
    /// <summary>
    /// Iterates through the rooms in the project and get the information of all the rooms
    /// </summary>
    public class RoomsData
    {
        #region class member variables
        Autodesk.Revit.UI.UIApplication m_revit;  // Store the reference of the application in revit

        List<Room> m_rooms = new List<Room>();    // a list to store all rooms in the project
        List<RoomTag> m_roomTags = new List<RoomTag>(); // a list to store all room tags
        List<Room> m_roomsWithTag = new List<Room>();   // a list to store all rooms with tag
        List<Room> m_roomsWithoutTag = new List<Room>(); // a list to store all rooms without tag
        // a list to store the information of departments
        List<DepartmentInfo> m_departmentInfos = new List<DepartmentInfo>();
        #endregion


        /// <summary>
        /// a struct to store the value of property Area and Department name
        /// </summary>
        public struct DepartmentInfo
        {
            String m_departmentName;
            int m_roomAmount;
            double m_departmentAreaValue;

            /// <summary>
            /// the name of department
            /// </summary>
            public String DepartmentName
            {
                get
                {
                    return m_departmentName;
                }
            }


            /// <summary>
            /// get the amount of rooms in the department
            /// </summary>
            public int RoomsAmount
            {
                get
                {
                    return m_roomAmount;
                }
            }

            /// <summary>
            /// the total area of the rooms in department
            /// </summary>
            public double DepartmentAreaValue
            {
                get
                {
                    return m_departmentAreaValue;
                }
            }

            /// <summary>
            /// constructor
            /// </summary>
            public DepartmentInfo(String departmentName, int roomAmount, double areaValue)
            {
                m_departmentName = departmentName;
                m_roomAmount = roomAmount;
                m_departmentAreaValue = areaValue;
            }
        }


        /// <summary>
        /// a list of all department
        /// </summary>
        public ReadOnlyCollection<DepartmentInfo> DepartmentInfos
        {
            get
            {
                return new ReadOnlyCollection<DepartmentInfo>(m_departmentInfos);
            }
        }


        /// <summary>
        /// a list of all the rooms in the project
        /// </summary>
        public ReadOnlyCollection<Room> Rooms
        {
            get
            {
                return new ReadOnlyCollection<Room>(m_rooms);
            }
        }


        /// <summary>
        /// a list of all the room tags in the project
        /// </summary>
        public ReadOnlyCollection<RoomTag> RoomTags
        {
            get
            {
                return new ReadOnlyCollection<RoomTag>(m_roomTags);
            }
        }


        /// <summary>
        /// a list of the rooms that had tag
        /// </summary>
        public ReadOnlyCollection<Room> RoomsWithTag
        {
            get
            {
                return new ReadOnlyCollection<Room>(m_roomsWithTag);
            }
        }


        /// <summary>
        /// a list of the rooms which lack room tag
        /// </summary>
        public ReadOnlyCollection<Room> RoomsWithoutTag
        {
            get
            {
                return new ReadOnlyCollection<Room>(m_roomsWithoutTag);
            }
        }


        /// <summary>
        ///constructor 
        /// </summary>
        public RoomsData(ExternalCommandData commandData)
        {
            m_revit = commandData.Application;

            // get all the rooms and room tags in the project
            GetAllRoomsAndTags();

            // find out the rooms that without room tag
            ClassifyRooms();
        }


        /// <summary>
        /// create the room tags for the rooms which lack room tag
        /// </summary>
        public void CreateTags()
        {
            try
            {
                foreach (Room tmpRoom in m_roomsWithoutTag)
                {
                    // get the location point of the room
                    LocationPoint locPoint = tmpRoom.Location as LocationPoint;
                    if (null == locPoint)
                    {
                        String roomId = "Room Id:  " + tmpRoom.Id.ToString();
                        String errMsg = roomId + "\r\nFault to create room tag," +
                                                   "can't get the location point!";
                        throw new Exception(errMsg);
                    }

                    // create a instance of Autodesk.Revit.DB.UV class
                    Autodesk.Revit.DB.UV point = new Autodesk.Revit.DB.UV(locPoint.Point.X, locPoint.Point.Y);

                    //create room tag
                    RoomTag tmpTag;
                    tmpTag = m_revit.ActiveUIDocument.Document.Create.NewRoomTag(new LinkElementId(tmpRoom.Id), point, null);
                    if (null != tmpTag)
                    {
                        m_roomTags.Add(tmpTag);
                    }
                }

                // classify rooms
                ClassifyRooms();

                // display a message box
                TaskDialog.Show("Revit", "Add room tags complete!");
            }
            catch (Exception exception)
            {
                TaskDialog.Show("Revit", exception.Message);
            }
        }


        /// <summary>
        /// reorder all the rooms' number
        /// </summary>
        public void ReorderRooms()
        {
            bool result = false;

            // sort all the rooms by ascending order according their coordinate
            result = this.SortRooms();

            // fault to reorder rooms' number
            if (!result)
            {
                TaskDialog.Show("Revit", "Fault to reorder rooms' number,can't get location point!");
                return;
            }

            // to avoid revit display the warning message,
            // change the rooms' name to a temp name 
            foreach (Room tmpRoom in m_rooms)
            {
                tmpRoom.Number += "XXX";
            }

            // set the tag number of rooms in order
            for (int i = 1; i <= m_rooms.Count; i++)
            {
                m_rooms[i - 1].Number = i.ToString();
            }

            // display a message box
            TaskDialog.Show("Revit", "Reorder room's number complete!");
        }


        /// <summary>
        /// get the room property and Department property according the property name
        /// </summary>
        /// <param name="room">a instance of room class</param>
        /// <param name="paraEnum">the property name</param>
        public String GetProperty(Room room, BuiltInParameter paraEnum)
        {
            String propertyValue = null;  //the value of parameter 

            // get the parameter via the parameterId
            Parameter param = room.get_Parameter(paraEnum);
            if (null == param)
            {
                return "";
            }
            // get the parameter's storage type
            StorageType storageType = param.StorageType;
            switch (storageType)
            {
                case StorageType.Integer:
                    int iVal = param.AsInteger();
                    propertyValue = iVal.ToString();
                    break;
                case StorageType.String:
                    String stringVal = param.AsString();
                    propertyValue = stringVal;
                    break;
                case StorageType.Double:
                    Double dVal = param.AsDouble();
                    dVal = Math.Round(dVal, 2);
                    propertyValue = dVal.ToString();
                    break;
                default:
                    break;
            }
            return propertyValue;
        }


        /// <summary>
        /// calculate the area of rooms for each department
        /// </summary>
        /// <param name="departName">the department name</param>
        /// <param name="areaValue">the value of room area</param>
        public void CalculateDepartmentArea(String departName, Double areaValue)
        {
            //if the list is empty, add a new  DepartmentArea instance
            if (0 == m_departmentInfos.Count)
            {
                // create a new instance of DepartmentArea struct and insert it to the list
                DepartmentInfo tmpDep = new DepartmentInfo(departName, 1, areaValue);
                m_departmentInfos.Add(tmpDep);
            }
            else
            {
                bool flag = false;
                // find whether the department exist in the project
                for (int i = 0; i < m_departmentInfos.Count; i++)
                {
                    if (departName == m_departmentInfos[i].DepartmentName)
                    {
                        int newAmount = m_departmentInfos[i].RoomsAmount + 1;
                        double tempValue = m_departmentInfos[i].DepartmentAreaValue + areaValue;
                        DepartmentInfo tempInstance = new DepartmentInfo(departName, newAmount, tempValue);
                        m_departmentInfos[i] = tempInstance;
                        flag = true;
                    }
                }

                // if a new department is found,
                // create a new instance of DepartmentArea struct and insert it to the list
                if (!flag)
                {
                    DepartmentInfo tmpDep = new DepartmentInfo(departName, 1, areaValue);
                    m_departmentInfos.Add(tmpDep);
                }
            }
        }


        /// <summary>
        /// export data into an Excel file
        /// </summary>
        /// <param name="fileName"></param>
        public void ExportFile(String fileName)
        {
            // store all the information that to be exported
            String allData = "";

            // get the project title
            String projectTitle = m_revit.ActiveUIDocument.Document.Title;  //the name of the project
            allData += "Total Rooms area of " + projectTitle + "\r\n";
            allData += "Department" + "," + "Rooms Amount" + "," + "Total Area" + "\r\n";

            foreach (DepartmentInfo tmp in m_departmentInfos)
            {
                allData += tmp.DepartmentName + "," + tmp.RoomsAmount +
                                        "," + tmp.DepartmentAreaValue + " SF\r\n";
            }

            // save the information into a Excel file
            if (0 < allData.Length)
            {
                System.IO.StreamWriter exportinfo = new System.IO.StreamWriter(fileName);
                exportinfo.WriteLine(allData);
                exportinfo.Close();
            }
        }


        /// <summary>
        /// get all the rooms and room tags in the project
        /// </summary>
        private void GetAllRoomsAndTags()
        {
            // get the active document 
            Document document = m_revit.ActiveUIDocument.Document;
            RoomFilter roomFilter = new RoomFilter();
            RoomTagFilter roomTagFilter = new RoomTagFilter();
            LogicalOrFilter orFilter = new LogicalOrFilter(roomFilter, roomTagFilter);

            FilteredElementIterator elementIterator =
                (new FilteredElementCollector(document)).WherePasses(orFilter).GetElementIterator();
            elementIterator.Reset();

            // try to find all the rooms and room tags in the project and add to the list
            while (elementIterator.MoveNext())
            {
                object obj = elementIterator.Current;

                // find the rooms, skip those rooms which don't locate at Level yet.
                Room tmpRoom = obj as Room;
                if (null != tmpRoom && null != document.GetElement(tmpRoom.LevelId))
                {
                    m_rooms.Add(tmpRoom);
                    continue;
                }

                // find the room tags
                RoomTag tmpTag = obj as RoomTag;
                if (null != tmpTag)
                {
                    m_roomTags.Add(tmpTag);
                    continue;
                }
            }
        }


        /// <summary>
        /// find out the rooms that without room tag
        /// </summary>
        private void ClassifyRooms()
        {
            m_roomsWithoutTag.Clear();
            m_roomsWithTag.Clear();

            // copy the all the elements in list Rooms to list RoomsWithoutTag
            m_roomsWithoutTag.AddRange(m_rooms);

            // get the room id from room tag via room property
            // if find the room id in list RoomWithoutTag,
            // add it to the list RoomWithTag and delete it from list RoomWithoutTag
            foreach (RoomTag tmpTag in m_roomTags)
            {
                ElementId idValue = tmpTag.Room.Id;
                m_roomsWithTag.Add(tmpTag.Room);

                // search the id for list RoomWithoutTag
                foreach (Room tmpRoom in m_rooms)
                {
                    if (idValue == tmpRoom.Id)
                    {
                        m_roomsWithoutTag.Remove(tmpRoom);
                    }
                }
            }
        }


        /// <summary>
        /// sort all the rooms by ascending order according their coordinate
        /// </summary>
        private bool SortRooms()
        {
            LocationPoint tmpPoint = null;
            LocationPoint roomPoint = null;
            Room listRoom = null;
            int result = 0;    //a temp variable
            int amount = m_rooms.Count; //the number of rooms
            bool flag = false;

            // sort the rooms according their location point 
            for (int i = 0; i < amount - 1; i++)
            {
                Room tmpRoom = m_rooms[i];
                for (int j = i + 1; j < amount; j++)
                {
                    tmpPoint = tmpRoom.Location as LocationPoint;
                    listRoom = m_rooms[j];
                    roomPoint = listRoom.Location as LocationPoint;

                    // if can't get location point, return false;
                    if (null == tmpPoint || null == roomPoint)
                    {
                        return false;
                    }

                    // rooms in different level
                    if (tmpPoint.Point.Z > roomPoint.Point.Z)
                    {
                        tmpRoom = listRoom;
                        result = j;

                        // if tmpRoom was changed, set flag to 1
                        flag = true;
                    }
                    // the two rooms in the same level
                    else if (tmpPoint.Point.Z == roomPoint.Point.Z)
                    {
                        if (tmpPoint.Point.X > roomPoint.Point.X)
                        {
                            tmpRoom = listRoom;
                            result = j;
                            flag = true;
                        }
                        else if (tmpPoint.Point.X == roomPoint.Point.X &&
                                 tmpPoint.Point.Y > roomPoint.Point.Y)
                        {
                            tmpRoom = listRoom;
                            result = j;
                            flag = true;
                        }
                    }
                }

                // if flag equals 1 ,move the room to the front of list
                if (flag)
                {
                    Room tempRoom = m_rooms[i];
                    m_rooms[i] = m_rooms[result];
                    m_rooms[result] = tempRoom;
                    flag = false;
                }
            }
            return true;
        }
    }
}
