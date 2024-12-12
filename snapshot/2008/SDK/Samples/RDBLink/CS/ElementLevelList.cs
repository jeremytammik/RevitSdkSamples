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

namespace Revit.SDK.Samples.RDBLink.CS
{
    /// <summary>
    /// Provides export and import functions for table ElementLevel.
    /// </summary>
    public class ElementLevelList : NonCreatableElementList
    {
        #region Constructors
        /// <summary>
        /// Initializes table name.
        /// </summary>
        public ElementLevelList()
            : base()
        {
            TableName = "ElementLevel";
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
            if (element != null && element.Id.Value != -1 && element.Level != null)
                return true;
            return false;
        }

        /// <summary>
        /// Populate data of an element into a DataRow.
        /// </summary>
        /// <param name="element">element to retrieve data from.</param>
        /// <param name="row">DataRow to populate.</param>
        protected override void PopulateDbRow(Autodesk.Revit.Element element, System.Data.DataRow row)
        {
            row["ElementId"] = GetIdDbValue(element);
            row["ElementName"] = GetNameDbValue(element);
            row["LevelId"] = GetIdDbValue(element.Level);
            row["LevelName"] = GetNameDbValue(element.Level);
        } 
        #endregion
    }
}
