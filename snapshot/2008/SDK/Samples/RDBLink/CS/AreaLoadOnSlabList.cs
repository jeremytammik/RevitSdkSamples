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

using Autodesk.Revit;
using System.Data;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Structural.Enums;

namespace Revit.SDK.Samples.RDBLink.CS
{
    /// <summary>
    /// Provides export and import functions for table AreaLoadOnSlab.
    /// </summary>
    public class AreaLoadOnSlabList : NonCreatableElementList
    {
        #region Constructors
        /// <summary>
        /// Initializes table name.
        /// </summary>
        public AreaLoadOnSlabList()
            : base()
        {
            TableName = "AreaLoadOnSlab";
        } 
        #endregion

        #region Methods
        /// <summary>
        /// Verify whether an element belongs to the specific table.
        /// </summary>
        /// <param name="element">element to verify.</param>
        /// <returns>true if the element belongs to the table, otherwise false.</returns>
        protected override bool CanAppend(Element element)
        {
            AreaLoad areaLoad = element as AreaLoad;
            if (areaLoad != null)
            {
                Floor hostElement = areaLoad.HostElement as Floor;
                if (null != hostElement &&
                    SlabFoundationType.Slab == hostElement.StructuralUsage)
                {
                    return true;
                }
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
            row["AreaLoadId"] = GetIdDbValue(areaLoad);
            row["SlabId"] = GetIdDbValue(areaLoad.HostElement);
        } 
        #endregion
    }
}
