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
    /// Provides export and import functions for Revit categories.
    /// </summary>
    public class CategoryList : APIObjectList
    {
        #region Constructors
        /// <summary>
        /// Initializes table name.
        /// </summary>
        public CategoryList()
            : base()
        {
            TableName = "Categories";
        } 
        #endregion

        #region Methods
        /// <summary>
        /// Export categories to a DataTable.
        /// </summary>
        /// <param name="dataTable">DataTable which revit data will be exported into</param>
        public override void Export(DataTable dataTable)
        {
            if (dataTable == null) return;
            DataTable = dataTable;
            foreach (Category category in ActiveDocument.Settings.Categories)
            {
                DataRow row = dataTable.NewRow();
                dataTable.Rows.Add(row);
                row["Id"] = category.Id.Value;
                row["Name"] = category.Name;
                if (category.Material != null)
                {
                    row["MaterialId"] = category.Material.Id.Value;
                }
                else
                {
                    row["MaterialId"] = DBNull.Value;
                }
            }
        }

        /// <summary>
        /// Clear a DataTable and export all categories into it.
        /// </summary>
        /// <param name="dataTable">categories table.</param>
        public override void Import(System.Data.DataTable dataTable)
        {
            if (dataTable == null) return;
            DataTable = dataTable;

            // clear the DataTable
            foreach (DataRow row in dataTable.Rows)
            {
                row.Delete();
            }
            // export all categories into the DataTable
            Export(dataTable);
        } 
        #endregion
    };
}
