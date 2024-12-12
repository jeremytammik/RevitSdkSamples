using System;
using System.Collections.Generic;
using System.Text;
using Autodesk.Revit.Elements;
using Autodesk.Revit;

namespace Revit.SDK.Samples.RDBLink.CS
{
    /// <summary>
    /// Provides export and import functions for table RoomAssociations.
    /// </summary>
    public class RoomAssociationList : NonCreatableElementList
    {
        #region Constructors
        /// <summary>
        /// Initializes table name.
        /// </summary>
        public RoomAssociationList()
        {
            TableName = "RoomAssociations";
        }
        #endregion

        #region Methods
        /// <summary>
        /// Verify whether an element belongs to the specific table.
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
                    BuiltInCategory catId = GetCategoryId(familyInstance);
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
            catch
            {
                // familyInstance.Room may cause exception
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
            row["Id"] = GetIdDbValue(element);
            row["PhaseId"] = GetIdDbValue(element.PhaseCreated);
            row["DesignOptionId"] = element.DesignOption == null ? -1 : element.DesignOption.Id.Value;
            row["RoomId"] = GetIdDbValue((element as FamilyInstance).Room);
        }
        #endregion
    }
}
