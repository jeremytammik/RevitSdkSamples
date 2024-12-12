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

namespace Revit.SDK.Samples.RDBLink.CS
{
    /// <summary>
    /// Provides export and import functions for Revit instances.
    /// </summary>
    public class InstanceList : SymbolList
    {
        /// <summary>
        /// Initializes table name and TableInfo.
        /// </summary>
        /// <param name="tableInfo"></param>
        public InstanceList(TableInfo tableInfo)
            : base(tableInfo)
        {
        }

        /// <summary>
        /// Check whether an element belongs to the specific table.
        /// </summary>
        /// <param name="element">element to check</param>
        /// <returns>true if the element belongs to the table, otherwise false.</returns>
        protected override bool CanAppend(Element element)
        {
            return (TableInfo.Filter.Category == GetCategoryId(element)
                && TableInfo.Filter.ElementType != ElementType.SYMBOL);
        }

        /// <summary>
        /// Do nothing and return null.
        /// </summary>
        /// <param name="row">DataRow which contains the information to create element.</param>
        /// <returns></returns>
        protected override Element CreateElement(DataRow row)
        {
            return null;
        }
    };
}
