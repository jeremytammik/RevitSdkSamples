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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB.Architecture;

namespace Revit.SDK.Samples.AutoTagRooms.CS
{
    /// <summary>
    /// This class can get all the rooms, rooms tags, room tag types and levels
    /// </summary>
    public class RoomsData
    {
        // Store the reference of the application in revit
        UIApplication m_revit;
        // Store all levels which have rooms in the current document
        List<Level> m_levels = new List<Level>();
        // Store all the rooms
        List<Room> m_rooms = new List<Room>();
        // Store all the RoomTagTypes
        List<RoomTagType> m_roomTagTypes = new List<RoomTagType>();
        // Store the room ID and all the tags which tagged to that room
        Dictionary<ElementId, List<RoomTag>> m_roomWithTags = new Dictionary<ElementId, List<RoomTag>>();

        /// <summary>
        /// Constructor of RoomsData
        /// </summary>
        /// <param name="commandData">The data source of RoomData class</param>
        public RoomsData(ExternalCommandData commandData)
        {
            m_revit = commandData.Application;
            GetRooms();
            GetRoomTagTypes();
            GetRoomWithTags();
        }

        /// <summary>
        /// Get all the rooms in the current document
        /// </summary>
        public ReadOnlyCollection<Room> Rooms
        {
            get
            {
                return new ReadOnlyCollection<Room>(m_rooms);
            }
        }

        /// <summary>
        /// Get all the levels which have rooms in the current document
        /// </summary>
        public ReadOnlyCollection<Level> Levels
        {
            get
            {
                return new ReadOnlyCollection<Level>(m_levels);
            }
        }

        /// <summary>
        /// Get all the RoomTagTypes in the current document
        /// </summary>
        public ReadOnlyCollection<RoomTagType> RoomTagTypes
        {
            get
            {
                return new ReadOnlyCollection<RoomTagType>(m_roomTagTypes);
            }
        }

        /// <summary>
        /// Find all the rooms in the current document
        /// </summary>
        private void GetRooms()
        {
            Document document = m_revit.ActiveUIDocument.Document;
            foreach (PlanTopology planTopology in document.PlanTopologies)
            {
                if (planTopology.GetRoomIds().Count != 0 && planTopology.Level != null)
                {
                    m_levels.Add(planTopology.Level);
                    foreach (ElementId eid in planTopology.GetRoomIds())
                    {
                        Room tmpRoom = document.GetElement(eid) as Room;

                        if (document.GetElement(tmpRoom.LevelId) != null && m_roomWithTags.ContainsKey(tmpRoom.Id) == false)
                        {
                            m_rooms.Add(tmpRoom);
                            m_roomWithTags.Add(tmpRoom.Id, new List<RoomTag>());
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get all the RoomTagTypes in the current document
        /// </summary>
        private void GetRoomTagTypes()
        {
            FilteredElementCollector filteredElementCollector = new FilteredElementCollector(m_revit.ActiveUIDocument.Document);
            filteredElementCollector.OfClass(typeof(FamilySymbol));
            filteredElementCollector.OfCategory(BuiltInCategory.OST_RoomTags);
            m_roomTagTypes = filteredElementCollector.Cast<RoomTagType>().ToList<RoomTagType>();
        }

        /// <summary>
        /// Get all the room tags which tagged rooms
        /// </summary>
        private void GetRoomWithTags()
        {
            Document document = m_revit.ActiveUIDocument.Document;
            IEnumerable<RoomTag> roomTags = from elem in ((new FilteredElementCollector(document)).WherePasses(new RoomTagFilter()).ToElements())
                                            let roomTag = elem as RoomTag
                                            where (roomTag != null) && (roomTag.Room != null)
                                            select roomTag;

            foreach (RoomTag roomTag in roomTags)
            {
                if (m_roomWithTags.ContainsKey(roomTag.Room.Id))
                {
                    List<RoomTag> tmpList = m_roomWithTags[roomTag.Room.Id];
                    tmpList.Add(roomTag);
                }
            }
        }

        /// <summary>
        /// Auto tag rooms with specified RoomTagType in a level
        /// </summary>
        /// <param name="level">The level where rooms will be auto tagged</param>
        /// <param name="tagType">The room tag type</param>
        public void AutoTagRooms(Level level, RoomTagType tagType)
        {
            PlanTopology planTopology = m_revit.ActiveUIDocument.Document.get_PlanTopology(level);

            SubTransaction subTransaction = new SubTransaction(m_revit.ActiveUIDocument.Document);
            subTransaction.Start();
            foreach (ElementId eid in planTopology.GetRoomIds())
            {
                Room tmpRoom = m_revit.ActiveUIDocument.Document.GetElement(eid) as Room;

                if (m_revit.ActiveUIDocument.Document.GetElement(tmpRoom.LevelId) != null && tmpRoom.Location != null)
                {
                    // Create a specified type RoomTag to tag a room
                    LocationPoint locationPoint = tmpRoom.Location as LocationPoint;
                    Autodesk.Revit.DB.UV point = new Autodesk.Revit.DB.UV(locationPoint.Point.X, locationPoint.Point.Y);
                    RoomTag newTag = m_revit.ActiveUIDocument.Document.Create.NewRoomTag(new LinkElementId(tmpRoom.Id), point, null);
                    newTag.RoomTagType = tagType;

                    List<RoomTag> tagListInTheRoom = m_roomWithTags[newTag.Room.Id];
                    tagListInTheRoom.Add(newTag);
                }

            }
            subTransaction.Commit();
        }

        /// <summary>
        /// Get the amount of room tags in a room with the specified RoomTagType
        /// </summary>
        /// <param name="room">A specified room</param>
        /// <param name="tagType">A specified tag type</param>
        /// <returns></returns>
        public int GetTagNumber(Room room, RoomTagType tagType)
        {
            int count = 0;
            List<RoomTag> tagListInTheRoom = m_roomWithTags[room.Id];
            foreach (RoomTag roomTag in tagListInTheRoom)
            {
                if (roomTag.RoomTagType.Id == tagType.Id)
                {
                    count++;
                }
            }
            return count;
        }
    }
}
