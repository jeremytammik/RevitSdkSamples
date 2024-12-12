//
// (C) Copyright 2003-2017 by Autodesk, Inc.
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
using System.Collections;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Windows.Forms;


namespace Revit.SDK.Samples.DoorSwing.CS
{
    /// <summary>
    /// Stores all the needed data and operates RevitAPI.
    /// </summary>
    public class DoorSwingData
    {
        #region "Memebers"

        // store door-opening types: user to decide how he wants to identify 
        // the Left, Right or others information.
        public static List<String> OpeningTypes = new List<string>();

        // store current project's door families.
        List<DoorFamily> m_doorFamilies = new List<DoorFamily>();

        Autodesk.Revit.UI.UIApplication m_app;

        #endregion

        #region "Properties"

        // retrieves door families.
        public List<DoorFamily> DoorFamilies
        {
            get
            {
                return m_doorFamilies;
            }
        }

        #endregion

        #region "Methods"

        /// <summary>
        /// fill OpeningTypes static member variable.
        /// </summary>
        static DoorSwingData()
        {
            // fill door's opening types. User can modify the DoorSwingResource according to 
            // how he wants to identify the Left, Right or others opening information.
            OpeningTypes.Clear();

            // Undefined means this door family is insensible of door opening feature.
            // User didn't add the relevant parameters or just gave an invalid value.
            OpeningTypes.Add(DoorSwingResource.Undefined);
            OpeningTypes.Add(DoorSwingResource.LeftDoor);
            OpeningTypes.Add(DoorSwingResource.RightDoor);
            OpeningTypes.Add(DoorSwingResource.TwoLeaf);
            OpeningTypes.Add(DoorSwingResource.TwoLeafActiveLeafLeft);
            OpeningTypes.Add(DoorSwingResource.TwoLeafActiveLeafRight);
        }

        /// <summary>
        /// constructor.
        /// </summary>
        /// <param name="app"> Revit application</param>
        public DoorSwingData(Autodesk.Revit.UI.UIApplication app)
        {
            m_app = app;

            // store door families in m_doorFamilies.
            PrepareDoorFamilies();

            // add needed shared parameters
            // if the parameters already added will not add again.
            DoorSharedParameters.AddSharedParameters(app);
        }

        /// <summary>
        /// update door instances information: Left/Right information, related rooms information.
        /// </summary>
        /// <param name="creFilter">One element filter utility object.</param>
        /// <param name="doc">Revit project.</param>
        /// <param name="onlyUpdateSelect">
        /// true means only update selected doors' information otherwise false.
        /// </param>
        /// <param name="showUpdateResultMessage">
        /// this parameter is used for invoking this method in Application's events (document save and document saveAs).
        /// update door infos in Application level events should not show unnecessary messageBox.
        /// </param>
        public static Autodesk.Revit.UI.Result UpdateDoorsInfo(Document doc, bool onlyUpdateSelect,
                                           bool showUpdateResultMessage, ref string message)
        {
            if ((!AssignedAllRooms(doc)) && showUpdateResultMessage)
            {
                TaskDialogResult dialogResult = TaskDialog.Show("Door Swing", "One or more eligible areas of this level " +
                                           "have no assigned room(s). Doors bounding these areas " +
                                           "will be designated as external doors. Proceed anyway?",
                                           TaskDialogCommonButtons.Yes | TaskDialogCommonButtons.No);

                if (TaskDialogResult.No == dialogResult)
                {
                    message = "Update cancelled. Please assign rooms for all eligible areas first.";
                    return Autodesk.Revit.UI.Result.Cancelled;
                }
            }

            // begin update door parameters.
            IEnumerator iter;
            int doorCount = 0;
            bool checkSharedParameters = false;

            if (onlyUpdateSelect) // update doors in select elements
            {
                UIDocument newUIdoc = new UIDocument(doc);
                ElementSet es = new ElementSet();
                foreach (ElementId elementId in newUIdoc.Selection.GetElementIds())
                {
                   es.Insert(newUIdoc.Document.GetElement(elementId));
                }
                iter = es.GetEnumerator();
            }
            else // update all doors in current Revit project.
            {
                ElementClassFilter familyInstanceFilter = new ElementClassFilter(typeof(FamilyInstance));
                ElementCategoryFilter doorsCategoryfilter = new ElementCategoryFilter(BuiltInCategory.OST_Doors);
                LogicalAndFilter doorInstancesFilter = new LogicalAndFilter(familyInstanceFilter, doorsCategoryfilter);
                iter = new FilteredElementCollector(doc).WherePasses(doorInstancesFilter).GetElementIterator();
            }

            iter.Reset();
            while (iter.MoveNext())
            {
                // find door instance
                FamilyInstance door = iter.Current as FamilyInstance;

                if (onlyUpdateSelect)
                {
                    if (null == door)
                    {
                        continue;
                    }

                    if (null == door.Category)
                    {
                        continue;
                    }

                    if (!door.Category.Name.Equals("Doors"))
                    {
                        continue;
                    }
                }

                // check if has needed parameters.
                if (!checkSharedParameters)
                {
                    checkSharedParameters = true;

                    if (!(door.Symbol.ParametersMap.Contains("BasalOpening") &&
                          door.ParametersMap.Contains("InstanceOpening") &&
                          door.ParametersMap.Contains("Internal Door")))
                    {
                        message = "Cannot update door parameters. Please customize door opening expression first.";
                        return Autodesk.Revit.UI.Result.Failed;
                    }
                }

                // get one door.
                doorCount++;

                // update one door's Opening parameter value.
                if (UpdateOpeningFeatureOfOneDoor(door) == Autodesk.Revit.UI.Result.Failed)
                {
                    message = "Cannot update door parameters. Please customize door opening expression first.";
                    return Autodesk.Revit.UI.Result.Failed;
                }

                // update one door's from/to room.
                UpdateFromToRoomofOneDoor(door, false);

                // update one door's internalDoor flag
                UpdateInternalDoorFlagFeatureofOneDoor(door);
            }

            if (showUpdateResultMessage)
            {

                if (onlyUpdateSelect)
                {
                    Autodesk.Revit.UI.TaskDialog.Show("Door Swing", "Updated all selected doors of " + doc.Title +
                                                         " (" + doorCount + " doors).\r\n (Selection may " +
                                                         "include miscellaneous elements.)");
                }
                else
                {
                    Autodesk.Revit.UI.TaskDialog.Show("Door Swing", "Updated all doors of " + doc.Title + " (" +
                                                         doorCount + " doors).");
                }
            }

            return Autodesk.Revit.UI.Result.Succeeded;
        }

        /// <summary>
        /// Doors related rooms: update doors' geometry according to its To/From room information.
        /// </summary>
        /// <param name="creFilter">One element filter utility object.</param>
        /// <param name="doc">Revit project.</param>
        /// <param name="onlyUpdateSelect">
        /// true means only update selected doors' information else false.
        /// </param>
        public static void UpdateDoorsGeometry(Document doc, bool onlyUpdateSelect)
        {
            IEnumerator iter;
            int doorCount = 0;

            if (onlyUpdateSelect) // update doors in select elements
            {
                UIDocument newUIdoc = new UIDocument(doc);
                ElementSet es = new ElementSet();
                foreach (ElementId elementId in newUIdoc.Selection.GetElementIds())
                {
                   es.Insert(newUIdoc.Document.GetElement(elementId));
                }
                iter = es.GetEnumerator();
            }
            else // update all doors in current Revit document
            {
                ElementClassFilter familyInstanceFilter = new ElementClassFilter(typeof(FamilyInstance));
                ElementCategoryFilter doorsCategoryfilter = new ElementCategoryFilter(BuiltInCategory.OST_Doors);
                LogicalAndFilter doorInstancesFilter = new LogicalAndFilter(familyInstanceFilter, doorsCategoryfilter);
                iter = new FilteredElementCollector(doc).WherePasses(doorInstancesFilter).GetElementIterator();
            }

            iter.Reset();
            while (iter.MoveNext())
            {
                // find door instance
                FamilyInstance door = iter.Current as FamilyInstance;

                if (onlyUpdateSelect)
                {
                    if (null == door)
                    {
                        continue;
                    }

                    if (null == door.Category)
                    {
                        continue;
                    }

                    if (!door.Category.Name.Equals("Doors"))
                    {
                        continue;
                    }
                }

                // find one door.
                doorCount++;

                // update one door.
                UpdateFromToRoomofOneDoor(door, true);
                doc.Regenerate();
            }

            if (onlyUpdateSelect)
            {
                Autodesk.Revit.UI.TaskDialog.Show("Door Swing", "Updated all selected doors (" + doorCount +
                                                     " doors).\r\n (Selection may include miscellaneous elements.)");
            }
            else
            {
                Autodesk.Revit.UI.TaskDialog.Show("Door Swing", "Updated all doors of this project (" +
                                                     doorCount + " doors).");
            }
        }

        /// <summary>
        /// Update doors' Left/Right information.
        /// </summary>
        /// <param name="door">one door instance.</param>
        private static Autodesk.Revit.UI.Result UpdateOpeningFeatureOfOneDoor(FamilyInstance door)
        {
            // flag whether the opening value should switch from its corresponding family's basic opening value.
            bool switchesOpeningValueFlag = false;

            // When the door is being mirrored once, the door switches its direction; 
            // When the door is being flipped once, the door switches its direction.
            // When the door is being mirrored and flipped, the door's direction remains the same.
            if (door.FacingFlipped ^ door.HandFlipped)
            {
                switchesOpeningValueFlag = true;
            }

            // get door's Opening parameter which indicates whether the door is Left or Right.
            Parameter openingParam = door.ParametersMap.get_Item("InstanceOpening");

            // country's standard Left/Right opening for this door type.
            String basalOpeningValue = door.Symbol.ParametersMap.get_Item("BasalOpening").AsString();

            string rightOpeningValue; // actual opening value of the door.
            if (switchesOpeningValueFlag)
            {
                if (DoorSwingResource.LeftDoor.Equals(basalOpeningValue))
                {
                    rightOpeningValue = DoorSwingResource.RightDoor;
                }
                else if (DoorSwingResource.RightDoor.Equals(basalOpeningValue))
                {
                    rightOpeningValue = DoorSwingResource.LeftDoor;
                }
                else if (DoorSwingResource.TwoLeafActiveLeafLeft.Equals(basalOpeningValue))
                {
                    rightOpeningValue = DoorSwingResource.TwoLeafActiveLeafRight;
                }
                else if (DoorSwingResource.TwoLeafActiveLeafRight.Equals(basalOpeningValue))
                {
                    rightOpeningValue = DoorSwingResource.TwoLeafActiveLeafLeft;
                }
                else if (DoorSwingResource.TwoLeaf.Equals(basalOpeningValue))
                {
                    rightOpeningValue = DoorSwingResource.TwoLeaf;
                }
                else if (DoorSwingResource.Undefined.Equals(basalOpeningValue))
                {
                    rightOpeningValue = DoorSwingResource.Undefined;
                }
                else
                {
                    return Autodesk.Revit.UI.Result.Failed;
                }
            }
            else
            {
                if (OpeningTypes.Contains(basalOpeningValue))
                {
                    rightOpeningValue = basalOpeningValue;
                }
                else
                {
                    return Autodesk.Revit.UI.Result.Failed;
                }
            }

            // update door's Opening param.
            openingParam.Set(rightOpeningValue);
            return Autodesk.Revit.UI.Result.Succeeded;
        }

        /// <summary>
        /// Update one door's internalDoor flag which indicates the door is internal door or external door.
        /// </summary>
        /// <param name="door">one door instance.</param>
        private static void UpdateInternalDoorFlagFeatureofOneDoor(FamilyInstance door)
        {
            // get the "Internal Door" shared parameter. 
            Parameter internalDoorFlagParam = door.ParametersMap.get_Item("Internal Door");

            // "Internal Door" is decided based on whether door's ToRoom and FromRoom properties both have values.
            // 1 means internal door, 0 means external door.
            if (null != door.ToRoom && null != door.FromRoom) // considered as internal door.
            {
                internalDoorFlagParam.Set(1);
            }
            else
            {
                internalDoorFlagParam.Set(0); // considered as external door.
            }
        }

        /// <summary>
        /// Doors related rooms: update one door's To/From room information or geometry.
        /// </summary>
        /// <param name="door">one door instance.</param>
        /// <param name="updateGeo">
        /// true means update geometry else update To/From room information.
        /// </param>
        private static void UpdateFromToRoomofOneDoor(FamilyInstance door, bool updateGeo)
        {
            if (null == door.ToRoom && null == door.FromRoom)
            {
                return;
            }

            // update the door's geometry according to door's To/From room info.
            // standard: door.ToRoom should keep consistent with door.Room else need update.
            if ((null == door.Room) && (null == door.FromRoom))
            {
                // only external door may have this status.
                // door.Room are consistent with door.FromRoom, so need update.
                if (updateGeo) // update geometry
                {
                    door.flipHand();
                    door.flipFacing();
                }
                else // update To/From Room.
                {
                    door.FlipFromToRoom();
                }
            }
            else if ((null != door.Room) && (null != door.FromRoom))
            {
                // door.Room are consistent with door.FromRoom, so need update.
                if (door.Room.Id.IntegerValue.Equals(door.FromRoom.Id.IntegerValue))
                {
                    if (updateGeo) // update geometry
                    {
                        door.flipHand();
                        door.flipFacing();
                    }
                    else // update To/From Room.
                    {
                        door.FlipFromToRoom();
                    }
                }
            }
        }

        /// <summary>
        /// Iterate through plan topology to determine if all plan circuits have assigned rooms.
        /// </summary>
        /// <param name="doc">Revit project.</param>
        /// <returns> true means all plan circuits have assigned rooms else not.</returns>
        private static bool AssignedAllRooms(Document doc)
        {
            PlanTopologySet planTopologies = doc.PlanTopologies;

            // Iterate plan topology for each level.
            foreach (PlanTopology planTopology in planTopologies)
            {
                PlanCircuitSet circuits = planTopology.Circuits;

                // Iterate each circuit in this plan topology.
                foreach (PlanCircuit circuit in circuits)
                {
                    bool locatedRoom = circuit.IsRoomLocated;

                    if (!locatedRoom)
                    {
                        // If any circuit isn't assigned room, then method return false.
                        return locatedRoom;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Do Door symbols' Opening set based on family's basic geometry and country's standard.
        /// </summary>
        public void UpdateDoorFamiliesOpeningFeature()
        {
            for (int i = 0; i < m_doorFamilies.Count; i++)
            {
                m_doorFamilies[i].UpdateOpeningFeature();
            }
        }

        /// <summary>
        /// Delete temporarily created door instances which are used to retrieve geometry. Retrieved 
        /// geometry will shown to users. So they can initialize door opening parameter more visually. 
        /// </summary>
        public void DeleteTempDoorInstances()
        {
            for (int i = 0; i < m_doorFamilies.Count; i++)
            {
                m_doorFamilies[i].DeleteTempDoorInstance();
            }
        }

        /// <summary>
        /// get all the door families in the project. 
        /// And store them in two lists separately based on opening parameter.
        /// </summary>
        private void PrepareDoorFamilies()
        {
            // prepare DoorFamilies
            FilteredElementIterator familyIter = new FilteredElementCollector(m_app.ActiveUIDocument.Document).OfClass(typeof(Family)).GetElementIterator();

            while (familyIter.MoveNext())
            {
                Family doorFamily = familyIter.Current as Family;

                if (null == doorFamily.FamilyCategory) // some family.FamilyCategory is null
                {
                    continue;
                }

                if (doorFamily.FamilyCategory.Name !=
                    m_app.ActiveUIDocument.Document.Settings.Categories.get_Item(BuiltInCategory.OST_Doors).Name) // FamilyCategory.Name is not 'Doors'
                {
                    continue;
                }
                // create one instance of self class DoorFamily.
                DoorFamily tempDoorFamily = new DoorFamily(doorFamily, m_app);

                // store the created DoorFamily instance
                m_doorFamilies.Add(tempDoorFamily);
            }
        }

        #endregion
    }
}
