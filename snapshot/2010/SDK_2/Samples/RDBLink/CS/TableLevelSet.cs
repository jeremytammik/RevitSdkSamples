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

namespace Revit.SDK.Samples.RDBLink.CS
{
    /// <summary>
    /// Preserves sorted tables by their relations
    /// </summary>
    class TableLevelSet : List<TableLevel>
    {
        #region Fields
        /// <summary>
        /// TableInfoSet which contains all database information
        /// </summary>
        TableInfoSet m_tableInfoSet; 
        #endregion

        #region Constructors
        /// <summary>
        /// Initialize TableInfoSet and create the list of TableLevel
        /// </summary>
        /// <param name="tableInfoSet">TableInfoSet</param>
        public TableLevelSet(TableInfoSet tableInfoSet)
            : base()
        {
            m_tableInfoSet = tableInfoSet;

            //create the list of TableLevel
            foreach (string key in m_tableInfoSet.Keys)
            {
                this.Add(new TableLevel(key, GetLevel(key)));
            }
            //sort the list
            this.Sort();
        } 
        #endregion

        #region Methods
        /// <summary>
        /// Get the level of a table which indicates the position in the 
        /// relation hierarchy of all tables
        /// </summary>
        /// <param name="tableId">Table id</param>
        /// <returns>Level of the table</returns>
        private int GetLevel(string tableId)
        {
            int maxlevel = 0;
            TableInfo ti = m_tableInfoSet[tableId];
            if (ti.ForeignKeys.Count > 0) maxlevel += 1;
            foreach (ForeignKey fKey in ti.ForeignKeys)
            {
                int level = GetLevel(fKey.RefTableId) + 1;
                maxlevel = level > maxlevel ? level : maxlevel;
            }
            return maxlevel;
        } 
        #endregion
    };

    /// <summary>
    /// Wrapper the table id and its level which is used to sort all tables
    /// </summary>
    class TableLevel : IComparable
    {
        #region Fields
        /// <summary>
        /// Table id
        /// </summary>
        string m_tableId;
        /// <summary>
        /// Table level
        /// </summary>
        int m_level; 
        #endregion

        #region Properties
        /// <summary>
        /// Gets table id
        /// </summary>
        public string TableId
        {
            get { return m_tableId; }
            //set { m_tableId = value; }
        }
        /// <summary>
        /// Gets table level
        /// </summary>
        public int Level
        {
            get { return m_level; }
            //set { m_level = value; }
        } 
        #endregion

        #region Constructors
        /// <summary>
        /// Initialize Table id and level
        /// </summary>
        /// <param name="tableId">Table id</param>
        /// <param name="level">Table level</param>
        public TableLevel(string tableId, int level)
        {
            m_tableId = tableId;
            m_level = level;
        } 
        #endregion

        #region Methods
        /// <summary>
        /// Compare two TableLevel object
        /// </summary>
        /// <param name="obj">TableLevel to compare</param>
        /// <returns>zero if this table's level is equal to the other, 
        /// bigger then zero if this table's level is bigger than the other,
        /// bigger then zero if this table's level is smaller than the other</returns>
        public int CompareTo(object obj)
        {
            TableLevel tl = obj as TableLevel;
            if (tl != null)
            {
                return this.Level.CompareTo(tl.Level);
            }
            return -1;
        } 
        #endregion
    };
}
