using System;
using System.Collections.Generic;
using System.Text;
using Autodesk.Revit.Elements;
using Autodesk.Revit;

namespace Revit.SDK.Samples.RDBLink.CS
{
    /// <summary>
    /// Provides export and import functions for table RoomFromToAssociations.
    /// </summary>
    public class RoomFromToAssociationList : NonCreatableElementList
    {
        #region Constructors
        /// <summary>
        /// Initializes table name.
        /// </summary>
        public RoomFromToAssociationList()
        {
            TableName = "RoomFromToAssociations";
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
            // element should be door or window
            try
            {
                FamilyInstance familylInstance = element as FamilyInstance;
                if (familylInstance != null &&
                    (familylInstance.FromRoom != null ||
                    familylInstance.ToRoom != null))
                {
                    BuiltInCategory catId = GetCategoryId(element);
                    if (catId == BuiltInCategory.OST_Doors
                        || catId == BuiltInCategory.OST_Windows)
                        return true;
                }
            }
            catch
            {
                // familylInstance.FromRoom or familylInstance.ToRoom may cause exception
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
            row["Id"] = GetIdDbValue(element);
            row["PhaseId"] = GetIdDbValue(element.PhaseCreated);
            row["DesignOptionId"] = element.DesignOption == null ? -1 : element.DesignOption.Id.Value;
            row["FromRoom"] = GetIdDbValue(familyInstance.FromRoom);
            row["ToRoom"] = GetIdDbValue(familyInstance.ToRoom);
        } 
        #endregion
    }
}
