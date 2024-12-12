//
// (C) Copyright 2003-2009 by Autodesk, Inc.
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
using System.Data;
using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Structural.Enums;

namespace Revit.SDK.Samples.RDBLink.CS
{
    /// <summary>
    /// Provides export and import functions for specific tables
    /// of which we can ignore the modification when do import.
    /// </summary>
    public abstract class NonCreatableElementList : ElementList
    {
        #region Methods
        /// <summary>
        /// Populate id and host id column
        /// </summary>
        /// <param name="element">an element based on host element</param>
        /// <param name="host">host of the element</param>
        /// <param name="dataRow">data row to populate</param>
        protected void SetDbRowIDAndHostId(Element element, Element host, DataRow dataRow)
        {
            dataRow[ColumnRes("ColN_CST_Id")] = GetIdDbValue(element);
            dataRow[ColumnRes("ColN_CST_Name")] = GetNameDbValue(element);
            dataRow[ColumnRes("ColN_CST_HostId")] = GetIdDbValue(host);
            dataRow[ColumnRes("ColN_CST_HostName")] = GetNameDbValue(host);
        } 
        #endregion
    };

    /// <summary>
    /// Provides export and import functions for table Openings.
    /// </summary>
    public class OpeningsList : NonCreatableElementList
    {
        #region Methods
        /// <summary>
        /// Verify whether an element belongs to a specific table.
        /// </summary>
        /// <param name="element">element to verify.</param>
        /// <returns>true if the element belongs to the table, otherwise false.</returns>
        protected override bool CanAppend(Element element)
        {
            return element is Opening;
        }

        /// <summary>
        /// Populate data of an element to a DataRow.
        /// </summary>
        /// <param name="element">element to retrieve data from.</param>
        /// <param name="row">DataRow to populate.</param>
        protected override void PopulateDbRow(Element element, DataRow row)
        {
            row[ColumnRes("ColN_CST_Id")] = GetIdDbValue(element);
            row[ColumnRes("ColN_CST_Name")] = GetNameDbValue(element);
            row[ColumnRes("ColN_CST_LevelId")] = GetIdDbValue(element.Level);
            row[ColumnRes("ColN_CST_HostId")] = GetIdDbValue((element as Opening).Host);
        }
        #endregion
    };

    /// <summary>
    /// Provides export and import functions for table RoomAssociations.
    /// </summary>
    public class RoomAssociationsList : NonCreatableElementList
    {
        #region Methods
        /// <summary>
        /// Verify whether an element belongs to a specific table.
        /// </summary>
        /// <param name="element">element to verify.</param>
        /// <returns>true if the element belongs to the table, otherwise false.</returns>
        protected override bool CanAppend(Autodesk.Revit.Element element)
        {
            try
            {
                FamilyInstance familyInstance = element as FamilyInstance;
                if (familyInstance != null &&
                    familyInstance.Room != null)
                {
                    BuiltInCategory catId = GetCategoryId(element);
                    switch (catId)
                    {
                        case BuiltInCategory.OST_Casework:
                        case BuiltInCategory.OST_ElectricalEquipment:
                        case BuiltInCategory.OST_ElectricalFixtures:
                        case BuiltInCategory.OST_Furniture:
                        case BuiltInCategory.OST_FurnitureSystems:
                        case BuiltInCategory.OST_GenericModel:
                        case BuiltInCategory.OST_LightingFixtures:
                        case BuiltInCategory.OST_MechanicalEquipment:
                        case BuiltInCategory.OST_PlumbingFixtures:
                        case BuiltInCategory.OST_SpecialityEquipment:
                        case BuiltInCategory.OST_DuctTerminal:
                        case BuiltInCategory.OST_TelephoneDevices:
                        case BuiltInCategory.OST_NurseCallDevices:
                        case BuiltInCategory.OST_SecurityDevices:
                        case BuiltInCategory.OST_CommunicationDevices:
                        case BuiltInCategory.OST_DataDevices:
                        case BuiltInCategory.OST_FireAlarmDevices:
                        case BuiltInCategory.OST_LightingDevices:
                        case BuiltInCategory.OST_Sprinklers:
                            return true;
                        default:
                            break;
                    }
                }
            }
            catch (Exception)
            {
                // when exception throws, it means this element does not belong to this table.
            }
            return false;
        }

        /// <summary>
        /// Populate data of an element to a DataRow.
        /// </summary>
        /// <param name="element">element to retrieve data from.</param>
        /// <param name="row">DataRow to populate.</param>
        protected override void PopulateDbRow(Autodesk.Revit.Element element, System.Data.DataRow row)
        {
            row[ColumnRes("ColN_CST_Id")] = GetIdDbValue(element);
            row[ColumnRes("ColN_CST_PhaseId")] = GetElementId(element.PhaseCreated);
            row[ColumnRes("ColN_CST_DesignOptionId")] = GetElementId(element.DesignOption);
            row[ColumnRes("ColN_CST_RoomId")] = GetIdDbValue((element as FamilyInstance).Room);
        }
        #endregion
    };

    /// <summary>
    /// Provides export and import functions for table RoomFromToAssociations.
    /// </summary>
    public class RoomFromToAssociationsList : NonCreatableElementList
    {
        #region Methods
        /// <summary>
        /// Verify whether an element belongs to a specific table.
        /// </summary>
        /// <param name="element">element to verify.</param>
        /// <returns>true if the element belongs to the table, otherwise false.</returns>
        protected override bool CanAppend(Autodesk.Revit.Element element)
        {
            // element should be door or window
            try
            {
                FamilyInstance familylInstance = element as FamilyInstance;
                if (familylInstance != null &&
                    (familylInstance.FromRoom != null ||
                    familylInstance.ToRoom != null))
                {
                    BuiltInCategory catId = GetCategoryId(element);
                    if (catId == BuiltInCategory.OST_Doors ||
                        catId == BuiltInCategory.OST_Windows)
                    {
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                // when exception throws, it means this element does not belong to this table.
            }
            return false;
        }

        /// <summary>
        /// Populate data of an element to a DataRow.
        /// </summary>
        /// <param name="element">element to retrieve data from.</param>
        /// <param name="row">DataRow to populate.</param>
        protected override void PopulateDbRow(Autodesk.Revit.Element element, System.Data.DataRow row)
        {
            FamilyInstance familyInstance = element as FamilyInstance;
            row[ColumnRes("ColN_CST_Id")] = GetIdDbValue(element);
            row[ColumnRes("ColN_CST_PhaseId")] = GetElementId(element.PhaseCreated);
            row[ColumnRes("ColN_CST_DesignOptionId")] = GetElementId(element.DesignOption);
            row[ColumnRes("ColN_CST_FromRoom")] = GetIdDbValue(familyInstance.FromRoom);
            row[ColumnRes("ColN_CST_ToRoom")] = GetIdDbValue(familyInstance.ToRoom);
        }
        #endregion
    };

    /// <summary>
    /// Provides export and import functions for table RoomTags.
    /// </summary>
    public class RoomTagsList : NonCreatableElementList
    {
        #region Methods
        /// <summary>
        /// Verify whether an element belongs to a specific table.
        /// </summary>
        /// <param name="element">element to verify.</param>
        /// <returns>true if the element belongs to the table, otherwise false.</returns>
        protected override bool CanAppend(Element element)
        {
            return element is RoomTag;
        }

        /// <summary>
        /// Populate data of an element to a DataRow.
        /// </summary>
        /// <param name="element">element to retrieve data from.</param>
        /// <param name="row">DataRow to populate.</param>
        protected override void PopulateDbRow(Element element, DataRow row)
        {
            RoomTag rt = element as RoomTag;
            row[ColumnRes("ColN_CST_Id")] = GetIdDbValue(rt);
            row[ColumnRes("ColN_CST_RoomTagType")] = GetNameDbValue(rt.ObjectType);
            row[ColumnRes("ColN_CST_RoomId")] = GetIdDbValue(rt.Room);
            row[ColumnRes("ColN_CST_ViewId")] = GetIdDbValue(rt.View);
        }
        #endregion
    };

    /// <summary>
    /// Provides export and import functions for table LineLoadOnBeam.
    /// </summary>
    public class LineLoadOnBeamList : NonCreatableElementList
    {
        #region Methods
        /// <summary>
        /// Verify whether an element belongs to a specific table.
        /// </summary>
        /// <param name="element">element to verify.</param>
        /// <returns>true if the element belongs to the table, otherwise false.</returns>
        protected override bool CanAppend(Element element)
        {
            LineLoad lineLoad = element as LineLoad;
            if (lineLoad != null)
            {
                return GetCategoryId(lineLoad.HostElement) == BuiltInCategory.OST_StructuralFraming;
            }
            return false;
        }

        /// <summary>
        /// Populate data of an element to a DataRow.
        /// </summary>
        /// <param name="element">element to retrieve data from.</param>
        /// <param name="row">DataRow to populate.</param>
        protected override void PopulateDbRow(Element element, DataRow row)
        {
            LineLoad lineLoda = element as LineLoad;
            row[ColumnRes("ColN_CST_LineLoadId")] = GetIdDbValue(lineLoda);
            row[ColumnRes("ColN_CST_BeamId")] = GetIdDbValue(lineLoda.HostElement);
        }
        #endregion
    };

    /// <summary>
    /// Provides export and import functions for table AreaLoadOnSlab.
    /// </summary>
    public class AreaLoadOnSlabList : NonCreatableElementList
    {
        #region Methods

        /// <summary>
        /// Verify whether an element belongs to a specific table.
        /// </summary>
        /// <param name="element">element to verify.</param>
        /// <returns>true if the element belongs to the table, otherwise false.</returns>
        protected override bool CanAppend(Element element)
        {
            AreaLoad areaLoad = element as AreaLoad;
            if (areaLoad != null)
            {
                return GetCategoryId(areaLoad.HostElement) == BuiltInCategory.OST_Floors;
            }
            return false;
        }

        /// <summary>
        /// Populate DataRow with data of an element.
        /// </summary>
        /// <param name="element">element to retrieve data from.</param>
        /// <param name="row">DataRow to populate.</param>
        protected override void PopulateDbRow(Element element, DataRow row)
        {
            AreaLoad areaLoad = element as AreaLoad;
            row[ColumnRes("ColN_CST_AreaLoadId")] = GetIdDbValue(areaLoad);
            row[ColumnRes("ColN_CST_SlabId")] = GetIdDbValue(areaLoad.HostElement);
        }
        #endregion
    };

    /// <summary>
    /// Provides export and import functions for table DoorWall.
    /// </summary>
    public class DoorWallList : NonCreatableElementList
    {
        #region Methods
        /// <summary>
        /// Verify whether an element belongs to a specific table.
        /// </summary>
        /// <param name="element">element to verify.</param>
        /// <returns>true if the element belongs to the table, otherwise false.</returns>
        protected override bool CanAppend(Element element)
        {
            if(GetCategoryId(element) == BuiltInCategory.OST_Doors)
            {
                FamilyInstance fi = element as FamilyInstance;
                if(fi != null)
                {
                    return GetCategoryId(fi) == BuiltInCategory.OST_Walls;
                }
            }
            return false;
        }

        /// <summary>
        /// Populate data of an element to a DataRow.
        /// </summary>
        /// <param name="element">element to retrieve data from.</param>
        /// <param name="row">DataRow to populate.</param>
        protected override void PopulateDbRow(Element element, DataRow row)
        {
            FamilyInstance fi = element as FamilyInstance;
            row[ColumnRes("ColN_CST_DoorId")] = GetIdDbValue(element);
            row[ColumnRes("ColN_CST_DoorName")] = GetNameDbValue(element);
            row[ColumnRes("ColN_CST_WallId")] = GetIdDbValue(fi.Host);
            row[ColumnRes("ColN_CST_WallName")] = GetNameDbValue(fi.Host);
        }
        #endregion
    };

    /// <summary>
    /// Provides export and import functions for table ElementLevel.
    /// </summary>
    public class ElementLevelList : NonCreatableElementList
    {
        #region Methods

        /// <summary>
        /// Verify whether an element belongs to a specific table.
        /// </summary>
        /// <param name="element">element to verify.</param>
        /// <returns>true if the element belongs to the table, otherwise false.</returns>
        protected override bool CanAppend(Autodesk.Revit.Element element)
        {
            return element != null && element.Id.Value != -1 
                && element.Level != null;
        }

        /// <summary>
        /// Populate data of an element to a DataRow.
        /// </summary>
        /// <param name="element">element to retrieve data from.</param>
        /// <param name="row">DataRow to populate.</param>
        protected override void PopulateDbRow(Autodesk.Revit.Element element, System.Data.DataRow row)
        {
            row[ColumnRes("ColN_CST_ElementId")] = GetIdDbValue(element);
            row[ColumnRes("ColN_CST_ElementName")] = GetNameDbValue(element);
            row[ColumnRes("ColN_CST_LevelId")] = GetIdDbValue(element.Level);
            row[ColumnRes("ColN_CST_LevelName")] = GetNameDbValue(element.Level);
        }
        #endregion
    };

    /// <summary>
    /// Provides export and import functions for table ElementPhase.
    /// </summary>
    public class ElementPhaseList : NonCreatableElementList
    {
        #region Methods
        /// <summary>
        /// Verify whether an element belongs to a specific table.
        /// </summary>
        /// <param name="element">element to verify.</param>
        /// <returns>true if the element belongs to the table, otherwise false.</returns>
        protected override bool CanAppend(Element element)
        {
            return element != null && element.Id.Value != -1 
                && element.PhaseCreated != null;
        }

        /// <summary>
        /// Populate data of an element to a DataRow.
        /// </summary>
        /// <param name="element">element to retrieve data from.</param>
        /// <param name="row">DataRow to populate.</param>
        protected override void PopulateDbRow(Element element, DataRow row)
        {
            row[ColumnRes("ColN_CST_ElementId")] = GetIdDbValue(element);
            row[ColumnRes("ColN_CST_ElementName")] = GetNameDbValue(element);
            row[ColumnRes("ColN_CST_PhaseCreatedId")] = GetIdDbValue(element.PhaseCreated);
            row[ColumnRes("ColN_CST_PhaseName")] = GetNameDbValue(element.PhaseCreated);
        }
        #endregion
    };

    /// <summary>
    /// Provides export and import functions for table WindowWall.
    /// </summary>
    public class WindowWallList : NonCreatableElementList
    {
        #region Methods
        /// <summary>
        /// Verify whether an element belongs to a specific table.
        /// </summary>
        /// <param name="element">element to verify.</param>
        /// <returns>true if the element belongs to the table, otherwise false.</returns>
        protected override bool CanAppend(Element element)
        {
            if(GetCategoryId(element) == BuiltInCategory.OST_Windows)
            {
                FamilyInstance fi = element as FamilyInstance;
                if(fi != null)
                {
                    return GetCategoryId(fi.Host) == BuiltInCategory.OST_Walls;
                }
            }
            return false;
        }

        /// <summary>
        /// Populate data of an element to a DataRow.
        /// </summary>
        /// <param name="element">element to retrieve data from.</param>
        /// <param name="row">DataRow to populate.</param>
        protected override void PopulateDbRow(Element element, DataRow row)
        {
            FamilyInstance fi = element as FamilyInstance;
            row[ColumnRes("ColN_CST_WindowId")] = GetIdDbValue(element);
            row[ColumnRes("ColN_CST_WindowName")] = GetNameDbValue(element);
            row[ColumnRes("ColN_CST_WallId")] = GetIdDbValue(fi.Host);
            row[ColumnRes("ColN_CST_WallName")] = GetNameDbValue(fi.Host);
        }
        #endregion
    };

    /// <summary>
    /// Provides export and import functions for table RebarOnFloor.
    /// </summary>
    public class RebarOnFloorList : NonCreatableElementList
    {
        #region Methods
        /// <summary>
        /// Verify whether an element belongs to a specific table.
        /// </summary>
        /// <param name="element">element to verify.</param>
        /// <returns>true if the element belongs to the table, otherwise false.</returns>
        protected override bool CanAppend(Element element)
        {
            Rebar rebar = element as Rebar;
            if (rebar != null)
            {
                return GetCategoryId(rebar.Host) == BuiltInCategory.OST_Floors;
            }
            return false;
        }

        /// <summary>
        /// Populate data of an element to a DataRow.
        /// </summary>
        /// <param name="element">element to retrieve data from.</param>
        /// <param name="row">DataRow to populate.</param>
        protected override void PopulateDbRow(Element element, DataRow row)
        {
            Rebar rebar = element as Rebar;
            SetDbRowIDAndHostId(rebar, rebar.Host, row);
        }
        #endregion
    };

    /// <summary>
    /// Provides export and import functions for table RebarOnColumn.
    /// </summary>
    public class RebarOnColumnList : NonCreatableElementList
    {
        #region Methods
        /// <summary>
        /// Verify whether an element belongs to a specific table.
        /// </summary>
        /// <param name="element">element to verify.</param>
        /// <returns>true if the element belongs to the table, otherwise false.</returns>
        protected override bool CanAppend(Element element)
        {
            Rebar rebar = element as Rebar;
            if(rebar != null)
            {
                return GetCategoryId(rebar.Host) == BuiltInCategory.OST_StructuralColumns;
            }
            return false;
        }

        /// <summary>
        /// Populate data of an element to a DataRow.
        /// </summary>
        /// <param name="element">element to retrieve data from.</param>
        /// <param name="row">DataRow to populate.</param>
        protected override void PopulateDbRow(Element element, DataRow row)
        {
            Rebar rebar = element as Rebar;
            SetDbRowIDAndHostId(rebar, rebar.Host, row);
        }
        #endregion
    };

    /// <summary>
    /// Provides export and import functions for table CurtainWallPanelOnRoof.
    /// </summary>
    public class CurtainWallPanelOnRoofList : NonCreatableElementList
    {
        #region Methods
        /// <summary>
        /// Verify whether an element belongs to a specific table.
        /// </summary>
        /// <param name="element">element to verify.</param>
        /// <returns>true if the element belongs to the table, otherwise false.</returns>
        protected override bool CanAppend(Element element)
        {
            if (GetCategoryId(element) == BuiltInCategory.OST_CurtainWallPanels)
            {
                FamilyInstance fi = element as FamilyInstance;
                if (fi != null && GetCategoryId(fi.Host) == BuiltInCategory.OST_Roofs)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Populate data of an element to a DataRow.
        /// </summary>
        /// <param name="element">element to retrieve data from.</param>
        /// <param name="row">DataRow to populate.</param>
        protected override void PopulateDbRow(Element element, DataRow row)
        {
            FamilyInstance fi = element as FamilyInstance;
            SetDbRowIDAndHostId(fi, fi.Host, row);
        }
        #endregion
    };

    /// <summary>
    /// Provides export and import functions for table GenericModelOnRoof.
    /// </summary>
    public class GenericModelOnRoofList : NonCreatableElementList
    {
        #region Methods
        /// <summary>
        /// Verify whether an element belongs to a specific table.
        /// </summary>
        /// <param name="element">element to verify.</param>
        /// <returns>true if the element belongs to the table, otherwise false.</returns>
        protected override bool CanAppend(Element element)
        {
            if(GetCategoryId(element) == BuiltInCategory.OST_GenericModel)
            {
                FamilyInstance fi = element as FamilyInstance;
                if(fi != null && GetCategoryId(fi.Host) == BuiltInCategory.OST_Roofs)
                {
                    return true;
                }                
            }            
            return false;
        }

        /// <summary>
        /// Populate data of an element to a DataRow.
        /// </summary>
        /// <param name="element">element to retrieve data from.</param>
        /// <param name="row">DataRow to populate.</param>
        protected override void PopulateDbRow(Element element, DataRow row)
        {
            FamilyInstance fi = element as FamilyInstance;
            SetDbRowIDAndHostId(fi, fi.Host, row);
        }
        #endregion
    };

    /// <summary>
    /// Provides export and import functions for table LightingFixtureOnCeiling.
    /// </summary>
    public class LightingFixtureOnCeilingList : NonCreatableElementList
    {
        #region Methods
        /// <summary>
        /// Verify whether an element belongs to a specific table.
        /// </summary>
        /// <param name="element">element to verify.</param>
        /// <returns>true if the element belongs to the table, otherwise false.</returns>
        protected override bool CanAppend(Element element)
        {
            if (GetCategoryId(element) == BuiltInCategory.OST_LightingFixtures)
            {
                FamilyInstance fi = element as FamilyInstance;
                if (fi != null && GetCategoryId(fi.Host) == BuiltInCategory.OST_Ceilings)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Populate data of an element to a DataRow.
        /// </summary>
        /// <param name="element">element to retrieve data from.</param>
        /// <param name="row">DataRow to populate.</param>
        protected override void PopulateDbRow(Element element, DataRow row)
        {
            FamilyInstance fi = element as FamilyInstance;
            SetDbRowIDAndHostId(fi, fi.Host, row);
        }
        #endregion
    };

    /// <summary>
    /// Provides export and import functions for table MechanicalEquipmentOnWall.
    /// </summary>
    public class MechanicalEquipmentOnWallList : NonCreatableElementList
    {
        #region Methods
        /// <summary>
        /// Verify whether an element belongs to a specific table.
        /// </summary>
        /// <param name="element">element to verify.</param>
        /// <returns>true if the element belongs to the table, otherwise false.</returns>
        protected override bool CanAppend(Element element)
        {
            if(GetCategoryId(element) == BuiltInCategory.OST_MechanicalEquipment)
            {
                FamilyInstance fi = element as FamilyInstance;
                if(fi != null && fi.Host is Wall)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Populate data of an element to a DataRow.
        /// </summary>
        /// <param name="element">element to retrieve data from.</param>
        /// <param name="row">DataRow to populate.</param>
        protected override void PopulateDbRow(Element element, DataRow row)
        {
            FamilyInstance fi = element as FamilyInstance;
            SetDbRowIDAndHostId(fi, fi.Host, row);
        }
        #endregion
    };

    /// <summary>
    /// Provides export and import functions for table CurtainWallPanelOnWall.
    /// </summary>
    public class CurtainWallPanelOnWallList : NonCreatableElementList
    {
        #region Methods
        /// <summary>
        /// Verify whether an element belongs to a specific table.
        /// </summary>
        /// <param name="element">element to verify.</param>
        /// <returns>true if the element belongs to the table, otherwise false.</returns>
        protected override bool CanAppend(Element element)
        {
            if(GetCategoryId(element) == BuiltInCategory.OST_CurtainWallPanels)
            {
                FamilyInstance fi = element as FamilyInstance;
                if(fi != null && fi.Host is Wall)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Populate data of an element to a DataRow.
        /// </summary>
        /// <param name="element">element to retrieve data from.</param>
        /// <param name="row">DataRow to populate.</param>
        protected override void PopulateDbRow(Element element, DataRow row)
        {
            FamilyInstance fi = element as FamilyInstance;
            SetDbRowIDAndHostId(fi, fi.Host, row);
        }
        #endregion
    };

    /// <summary>
    /// Provides export and import functions for table LightingFixtureOnWall.
    /// </summary>
    public class LightingFixtureOnWallList : NonCreatableElementList
    {
        #region Methods
        /// <summary>
        /// Verify whether an element belongs to a specific table.
        /// </summary>
        /// <param name="element">element to verify.</param>
        /// <returns>true if the element belongs to the table, otherwise false.</returns>
        protected override bool CanAppend(Element element)
        {
            if (GetCategoryId(element) == BuiltInCategory.OST_LightingFixtures)
            {
                FamilyInstance fi = element as FamilyInstance;
                if (fi != null && fi.Host is Wall)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Populate data of an element to a DataRow.
        /// </summary>
        /// <param name="element">element to retrieve data from.</param>
        /// <param name="row">DataRow to populate.</param>
        protected override void PopulateDbRow(Element element, DataRow row)
        {
            FamilyInstance fi = element as FamilyInstance;
            SetDbRowIDAndHostId(fi, fi.Host, row);
        }
        #endregion
    };

    /// <summary>
    /// Provides export and import functions for table PlumbingFixtureOnWall.
    /// </summary>
    public class PlumbingFixtureOnWallList : NonCreatableElementList
    {
        #region Methods
        /// <summary>
        /// Verify whether an element belongs to a specific table.
        /// </summary>
        /// <param name="element">element to verify.</param>
        /// <returns>true if the element belongs to the table, otherwise false.</returns>
        protected override bool CanAppend(Element element)
        {
            if (GetCategoryId(element) == BuiltInCategory.OST_PlumbingFixtures)
            {
                FamilyInstance fi = element as FamilyInstance;
                if (fi != null && fi.Host is Wall)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Populate data of an element to a DataRow.
        /// </summary>
        /// <param name="element">element to retrieve data from.</param>
        /// <param name="row">DataRow to populate.</param>
        protected override void PopulateDbRow(Element element, DataRow row)
        {
            FamilyInstance fi = element as FamilyInstance;
            SetDbRowIDAndHostId(fi, fi.Host, row);
        }
        #endregion
    };

    /// <summary>
    /// Provides export and import functions for table OpeningOnWall.
    /// </summary>
    public class OpeningOnWallList : NonCreatableElementList
    {
        #region Methods
        /// <summary>
        /// Verify whether an element belongs to a specific table.
        /// </summary>
        /// <param name="element">element to verify.</param>
        /// <returns>true if the element belongs to the table, otherwise false.</returns>
        protected override bool CanAppend(Element element)
        {
            BuiltInCategory biCat = GetCategoryId(element);
            if (biCat == BuiltInCategory.OST_ArcWallRectOpening ||
                biCat == BuiltInCategory.OST_SWallRectOpening)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Populate data of an element to a DataRow.
        /// </summary>
        /// <param name="element">element to retrieve data from.</param>
        /// <param name="row">DataRow to populate.</param>
        protected override void PopulateDbRow(Element element, DataRow row)
        {
            Opening opening = element as Opening;
            SetDbRowIDAndHostId(opening, opening.Host, row);
        }
        #endregion
    };

    /// <summary>
    /// Provides export and import functions for table OpeningOnFloor.
    /// </summary>
    public class OpeningOnFloorList : NonCreatableElementList
    {
        #region Methods
        /// <summary>
        /// Verify whether an element belongs to a specific table.
        /// </summary>
        /// <param name="element">element to verify.</param>
        /// <returns>true if the element belongs to the table, otherwise false.</returns>
        protected override bool CanAppend(Element element)
        {
            return GetCategoryId(element) == BuiltInCategory.OST_FloorOpening;
        }

        /// <summary>
        /// Populate data of an element to a DataRow.
        /// </summary>
        /// <param name="element">element to retrieve data from.</param>
        /// <param name="row">DataRow to populate.</param>
        protected override void PopulateDbRow(Element element, DataRow row)
        {
            Opening opening = element as Opening;
            Floor host = opening.Host as Floor;
            SetDbRowIDAndHostId(opening, host, row);
        }
        #endregion
    };

    /// <summary>
    /// Provides export and import functions for table GenericModel.
    /// </summary>
    public class GenericModelOnWallList : NonCreatableElementList
    {
        #region Methods
        /// <summary>
        /// Verify whether an element belongs to a specific table.
        /// </summary>
        /// <param name="element">element to verify.</param>
        /// <returns>true if the element belongs to the table, otherwise false.</returns>
        protected override bool CanAppend(Element element)
        {
            if (GetCategoryId(element) == BuiltInCategory.OST_GenericModel)
            {
                FamilyInstance fi = element as FamilyInstance;
                if (fi != null && fi.Host is Wall)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Populate data of an element to a DataRow.
        /// </summary>
        /// <param name="element">element to retrieve data from.</param>
        /// <param name="row">DataRow to populate.</param>
        protected override void PopulateDbRow(Element element, DataRow row)
        {
            FamilyInstance fi = element as FamilyInstance;
            SetDbRowIDAndHostId(fi, fi.Host, row);
        }
        #endregion
    };

    /// <summary>
    /// Provides export and import functions for table CaseworkOnWall.
    /// </summary>
    public class CaseworkOnWallList : NonCreatableElementList
    {
        #region Methods
        /// <summary>
        /// Verify whether an element belongs to a specific table.
        /// </summary>
        /// <param name="element">element to verify.</param>
        /// <returns>true if the element belongs to the table, otherwise false.</returns>
        protected override bool CanAppend(Element element)
        {
            if (GetCategoryId(element) == BuiltInCategory.OST_Casework)
            {
                FamilyInstance fi = element as FamilyInstance;
                if (fi != null && fi.Host is Wall)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Populate data of an element to a DataRow.
        /// </summary>
        /// <param name="element">element to retrieve data from.</param>
        /// <param name="row">DataRow to populate.</param>
        protected override void PopulateDbRow(Element element, DataRow row)
        {
            FamilyInstance fi = element as FamilyInstance;
            SetDbRowIDAndHostId(fi, fi.Host, row);
        }
        #endregion
    };
}
