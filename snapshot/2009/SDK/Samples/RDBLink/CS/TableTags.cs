//
// (C) Copyright 2003-2008 by Autodesk, Inc.
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
    /// A tag used to set Tag property of DataGridView, indicates its display status
    /// </summary>
    public class TableTag
    {
        #region Fields
        /// <summary>
        /// Whether the table support object creation
        /// </summary>
        bool m_allowCreate;
        /// <summary>
        /// Whether the table is custom table
        /// </summary>
        bool m_customTable; 

        /// <summary>
        /// An instance indicates the table which supports object creation
        /// </summary>
        static TableTag s_allowCreate;
        /// <summary>
        /// An instance indicates the table which does not support object creation
        /// </summary>
        static TableTag s_notAllowCreate;
        /// <summary>
        /// An instance indicates the table which is custom table
        /// </summary>
        static TableTag s_customTable;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets whether the table supports object creation or not
        /// </summary>
        public bool IsAllowCreate
        {
            get { return m_allowCreate; }
            set { m_allowCreate = value; }
        }
        /// <summary>
        /// Gets or sets whether the table is custom table or not
        /// </summary>
        public bool IsCustomTable
        {
            get { return m_customTable; }
            set { m_customTable = value; }
        }

        /// <summary>
        /// Gets the instance which indicates the table which supports object creation
        /// </summary>
        public static TableTag AllowCreate
        {
            get { return s_allowCreate; }
        }
        /// <summary>
        /// Gets the instance which indicates the table which does not support object creation
        /// </summary>
        public static TableTag NotAllowCreate
        {
            get { return s_notAllowCreate; }
        }
        /// <summary>
        /// Gets the instance which indicates the table which is custom table
        /// </summary>
        public static TableTag CustomTable
        {
            get { return s_customTable; }
        } 
        #endregion

        #region Constructors

        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="allowCreate">Is allow object creation</param>
        /// <param name="customTable">Is custom table</param>
        public TableTag(bool allowCreate, bool customTable)
        {
            m_allowCreate = allowCreate;
            m_customTable = customTable;
        }
        /// <summary>
        /// Initialize static instances.
        /// </summary>
        static TableTag()
        {
            s_allowCreate = new TableTag(true, false);
            s_notAllowCreate = new TableTag(false, false);
            s_customTable = new TableTag(false, true);
        } 
        #endregion
    };

    /// <summary>
    /// A tag used to set Tag property of DataGridViewRow, indicates its display status
    /// </summary>
    public class RowTag
    {
        #region Fields
        /// <summary>
        /// Indicates whether the row has corresponding object exists in Revit
        /// </summary>
        bool m_notExist;
        /// <summary>
        /// Indicates whether it is allowed to cover (paste) this row
        /// </summary>
        bool m_pastable;
        /// <summary>
        /// Indicates whether it is allowed to delete this row
        /// </summary>
        bool m_deletable;

        /// <summary>
        /// An instance indicates a new user added row
        /// </summary>
        static RowTag s_aNewRow;
        /// <summary>
        /// An instance indicates a row which has corresponding object exist in Revit
        /// </summary>
        static RowTag s_anOldRowExist;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets whether corresponding object of this row exists in Revit
        /// </summary>
        public bool NotExist
        {
            get { return m_notExist; }
            set { m_notExist = value; }
        }
        /// <summary>
        /// Gets or sets whether it is allowed to cover (paste) this row
        /// </summary>
        public bool Pastable
        {
            get { return m_pastable; }
            set { m_pastable = value; }
        }
        /// <summary>
        /// Gets or sets whether it is allowed to delete this row
        /// </summary>
        public bool Deletable
        {
            get { return m_deletable; }
            set { m_deletable = value; }
        }

        /// <summary>
        /// Gets the instance which indicates a new user added row
        /// </summary>
        public static RowTag ANewRow
        {
            get { return s_aNewRow; }
        }
        /// <summary>
        /// Gets the instance which indicates a row which has not corresponding object exist in Revit
        /// </summary>
        public static RowTag AnOldRowExist
        {
            get { return s_anOldRowExist; }
        } 
        #endregion

        #region Constructors
        /// <summary>
        /// Initialize static instances.
        /// </summary>
        static RowTag()
        {
            s_aNewRow = new RowTag(true, true, true);
            s_anOldRowExist = new RowTag(false, false, false);
        }

        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="notExist">Does its corresponding object exist in Revit</param>
        /// <param name="pastable">Is it allowed to paste</param>
        /// <param name="deletable">Is it allowed to delete</param>
        public RowTag(bool notExist, bool pastable, bool deletable)
        {
            m_notExist = notExist;
            m_pastable = pastable;
            m_deletable = deletable;
        } 
        #endregion
    };

    /// <summary>
    /// A tag used to set Tag property of DataGridViewCell, indicates its display status
    /// </summary>
    public class CellTag
    {
        #region Fields
        /// <summary>
        /// Indicates whether the cell is read-only
        /// </summary>
        bool m_readOnly;

        /// <summary>
        /// An instance indicates a read-only cell
        /// </summary>
        static CellTag s_readOnly;
        /// <summary>
        /// An instance indicates a non read-only cell
        /// </summary>
        static CellTag s_notReadOnly; 
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets whether this cell is read-only or not
        /// </summary>
        public bool IsReadOnly
        {
            get { return m_readOnly; }
            set { m_readOnly = value; }
        }

        /// <summary>
        /// Gets the instance which indicates a read-only cell
        /// </summary>
        public static CellTag ReadOnly
        {
            get { return s_readOnly; }
        }

        /// <summary>
        /// Gets the instance which indicates a non read-only cell
        /// </summary>
        public static CellTag NotReadOnly
        {
            get { return s_notReadOnly; }
        } 
        #endregion

        #region Constructors
        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="readOnly">Is it read-only</param>
        public CellTag(bool readOnly)
        {
            m_readOnly = readOnly;
        }

        /// <summary>
        /// Initialize static instances.
        /// </summary>
        static CellTag()
        {
            s_readOnly = new CellTag(true);
            s_notReadOnly = new CellTag(false);
        } 
        #endregion
    };


}
