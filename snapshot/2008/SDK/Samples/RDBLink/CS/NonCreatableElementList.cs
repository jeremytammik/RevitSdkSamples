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
using System.Data;

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
        /// Clear a DataTable and export all related elements data into it.
        /// </summary>
        /// <param name="dataTable">DataTable</param>
        public override void Import(System.Data.DataTable dataTable)
        {
            if (dataTable == null) return;
            DataTable = dataTable;
            foreach (DataRow row in dataTable.Rows)
            {
                row.Delete();
            }
            Export(dataTable);
        }

        /// <summary>
        /// Do nothing.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="rowIndex"></param>
        protected override void UpdateElement(Autodesk.Revit.Element element, int rowIndex)
        {
        } 
        #endregion
    }
}
