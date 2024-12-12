using System;
using System.Text;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Architecture;


namespace Rooms
{
    /// <summary>
    /// Iterates through the rooms in the project and get the information of all the rooms
    /// </summary>
    public class RoomsData
    {
        ThisApplication m_thisApp;

        List<Room> m_rooms = new List<Room>();                   //a list to store all rooms in the project
        List<RoomTag> m_roomTags = new List<RoomTag>();          //a list to store all room tags in the project
        List<Room> m_roomsWithTag = new List<Room>();            //a list to store all rooms with tag
        List<Room> m_roomsWithoutTag = new List<Room>();         //a list to store all rooms without tag
        List<DepartmentInfo> m_departmentInfos = new List<DepartmentInfo>();    //a list to store department


        /// <summary>
        /// a class to stor the value of property Area and Department
        /// </summary>
        public struct DepartmentInfo
        {
            String m_departmentName;
            double m_departmentAreaValue;

            /// <summary>
            /// constructor 
            /// </summary>
            public DepartmentInfo(String departmentName, double areaValue)
            {
                m_departmentName = departmentName;
                m_departmentAreaValue = areaValue;
            }


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
            /// the total area of the rooms in department
            /// </summary>
            public double DepartmentAreaValue
            {
                get
                {
                    return m_departmentAreaValue;
                }
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
        /// a list of the rooms without tags
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
        public RoomsData(ThisApplication application)
        {
            m_thisApp = application;

            ElementFilter filterRoom = new ElementClassFilter(typeof(SpatialElement));
            FilteredElementCollector roomCollector = new FilteredElementCollector(m_thisApp.ActiveUIDocument.Document);
            roomCollector.WherePasses(filterRoom);

            foreach (Autodesk.Revit.DB.Element ee in roomCollector)
            {
                Room tmpRoom = ee as Room;
                if (null != tmpRoom)
                {
                    m_rooms.Add(tmpRoom);
                    continue;
                }
            }

            ElementFilter filterRoomTag = new ElementClassFilter(typeof(SpatialElementTag));
            FilteredElementCollector roomTagCollector = new FilteredElementCollector(m_thisApp.ActiveUIDocument.Document);
            roomTagCollector.WherePasses(filterRoomTag);

            foreach (Autodesk.Revit.DB.Element ee in roomTagCollector)
            {
                // find the room tags
                RoomTag tmpTag = ee as RoomTag;
                if (null != tmpTag)
                {
                    m_roomTags.Add(tmpTag);
                    continue;
                }
            }

            //find out the rooms that without tag
            ClassifyRooms();
        }


        /// <summary>
        /// find out the rooms that without tag
        /// </summary>
        private void ClassifyRooms()
        {
            //copy the all the elements in list Rooms to list RoomsWithoutTag
            m_roomsWithoutTag.AddRange(m_rooms);

            //get the room id from room tag via room property
            //if find the room id in list RoomWithoutTag,
            //add it to the list RoomWithTag and delete it from list RoomWithoutTag
            foreach (RoomTag tmpTag in m_roomTags)
            {
                int idValue = tmpTag.Room.Id.IntegerValue;
                m_roomsWithTag.Add(tmpTag.Room);
                //search the id for list RoomWithoutTag
                foreach (Room tmpRoom in m_rooms)
                {
                    if (idValue == tmpRoom.Id.IntegerValue)
                    {
                        m_roomsWithoutTag.Remove(tmpRoom);
                    }
                }
            }
        }


        /// <summary>
        /// create the room tag for the rooms without tags
        /// </summary>
        public void CreateTags()
        {
            foreach (Room tmpRoom in m_roomsWithoutTag)
            {
                //get the location point of the room
                LocationPoint locPoint = tmpRoom.Location as LocationPoint;
                //create a instance of UV class
                double u = locPoint.Point.X;
                double v = locPoint.Point.Y;

                UV point = m_thisApp.Application.Create.NewUV(u, v);

                //create room tag
                m_thisApp.ActiveUIDocument.Document.Create.NewRoomTag(tmpRoom, point, null);
            }
        }


        /// <summary>
        /// sort all the rooms by ascending order according their coordinate
        /// </summary>
        private void SortRooms()
        {
            LocationPoint tmpPoint = null;
            LocationPoint roomPoint = null;
            Room listRoom = null;
            int result = 0;
            int flag = 0;
            int amount = m_rooms.Count;

            //sort the rooms according their location point 
            for (int i = 0; i < amount - 1; i++)
            {
                Room tmpRoom = m_rooms[i];
                for (int j = i + 1; j < amount; j++)
                {
                    tmpPoint = tmpRoom.Location as LocationPoint;
                    listRoom = m_rooms[j];
                    roomPoint = listRoom.Location as LocationPoint;

                    //rooms in different level
                    if (tmpPoint.Point.Z > roomPoint.Point.Z)
                    {
                        tmpRoom = listRoom;
                        result = j;
                        //if tmpRoom was changed,set flag to 1
                        flag = 1;
                    }
                    //the two rooms in the same level
                    else if (tmpPoint.Point.Z == roomPoint.Point.Z)
                    {
                        if (tmpPoint.Point.X > roomPoint.Point.X)
                        {
                            tmpRoom = listRoom;
                            result = j;
                            flag = 1;
                        }
                        else if (tmpPoint.Point.X == roomPoint.Point.X &&
                                 tmpPoint.Point.Y > roomPoint.Point.Y)
                        {
                            tmpRoom = listRoom;
                            result = j;
                            flag = 1;
                        }
                    }
                }

                //if flag equals 1 ,move the room to the front of list
                if (1 == flag)
                {
                    Room tempRoom = m_rooms[i];
                    m_rooms[i] = m_rooms[result];
                    m_rooms[result] = tempRoom;
                    flag = 0;
                }
            }
        }


        /// <summary>
        /// reorder all the rooms' number
        /// </summary>
        /// <param name="roomlist">a list of rooms</param>
        public void ReorderRooms()
        {
            //sort all the rooms by ascending order according their coordinate
            this.SortRooms();

            //to avoid revit display the warning message,
            //change the rooms' name to a temp name 
            foreach (Room tmpRoom in m_rooms)
            {
                tmpRoom.Number += "XXX";
            }

            //set the rooms number to a new order
            for (int i = 1; i <= m_rooms.Count; i++)
            {
                m_rooms[i - 1].Number = i.ToString();
            }
        }


        /// <summary>
        /// get the room property and Department property according the property name
        /// </summary>
        /// <param name="room">a instance of room class</param>
        /// <param name="propertyName">the property name</param>
        /// <param name="proValue">the value of property</param>
        public String GetProperty(Room room, BuiltInParameter paramEnum)
        {
            String propertyValue = null;  //the value of parameter 

            //get the parameter via the parameterId
            Parameter param = room.get_Parameter(paramEnum);
            //get the parameter's storage type
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
        /// <param name="deparName">the department name</param>
        /// <param name="areaValue">the value of room area</param>
        public void CalculateDepartmentArea(String deparName, Double areaValue)
        {
            //if the array list is empty add a new instance of DepartmentArea to the list
            if (0 == m_departmentInfos.Count)
            {
                //create a new instance of DepartmentArea struct and insert it to the list
                DepartmentInfo tmpDep = new DepartmentInfo(deparName, areaValue);
                m_departmentInfos.Add(tmpDep);
            }
            else
            {
                int flag = 0;
                //find whether the department exist in the project
                for (int i = 0; i < m_departmentInfos.Count; i++)
                {
                    if (deparName == m_departmentInfos[i].DepartmentName)
                    {
                        double tempValue = m_departmentInfos[i].DepartmentAreaValue + areaValue;
                        DepartmentInfo tempInstance = new DepartmentInfo(deparName, tempValue);
                        m_departmentInfos[i] = tempInstance;
                        flag = 1;
                    }
                }
                //if found a new department,
                //create a new instance of DepartmentArea struct and insert it to the list
                if (0 == flag)
                {
                    DepartmentInfo tmpDep = new DepartmentInfo(deparName, areaValue);
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
            //store all the information that to be exported
            String allData = "";

            //get the project title
            String projectTitle = m_thisApp.ActiveUIDocument.Document.Title;  //the name of the project
            allData += "Total Rooms area of " + projectTitle + "\n";
            allData += "Department" + "," + "Area" + "\n";

            foreach (DepartmentInfo tmp in m_departmentInfos)
            {
                allData += tmp.DepartmentName + "," + tmp.DepartmentAreaValue + " SF\n";
            }

            //save the information into a Excel file
            if (allData.Length > 0)
            {
                System.IO.StreamWriter exportinfo = new System.IO.StreamWriter(fileName);
                exportinfo.WriteLine(allData);
                exportinfo.Close();
            }
        }
    }
}
