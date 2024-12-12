//
// (C) Copyright 2003-2014 by Autodesk, Inc. 
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
using System.Windows.Forms;

using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.Loads.CS
{
    /// <summary>
    /// The map class which store the data and display in usageDataGridView
    /// </summary>
    public class UsageMap
    {
        // Private Members
        Loads m_dataBuffer; // A reference of Loads
        Boolean m_set;      // Indicate the set column of Usage DataGridView control
        String m_name;      // Indicate the name column of Usage DataGridView control

        /// <summary>
        /// is selected in Usage DataGridView control
        /// </summary>
        public Boolean Set
        {
            get
            {
                return m_set;
            }
            set
            {
                m_set = value;
            }
        }

        /// <summary>
        /// usage name
        /// </summary>
        public String Name
        {
            get
            {
                return m_name;
            }
            set
            {
                if (null == value)
                {
                    TaskDialog.Show("Revit", "The usage name should not be null.");
                    return;
                }
                if (null == m_name)
                {
                    m_name = value;
                    return;
                }
                Boolean canModify = m_dataBuffer.ModifyUsageName(m_name, value);
                if (canModify)
                {
                    m_name = value;
                }
            }
        }

        /// <summary>
        /// Constructor with Set = false, Name="",
        /// This should not be called.
        /// </summary>
        /// <param name="dataBuffer">The reference of Loads</param>
        public UsageMap(Loads dataBuffer)
        {
            m_dataBuffer = dataBuffer;
        }

        /// <summary>
        /// constructor with Set = false
        /// </summary>
        /// <param name="dataBuffer">The reference of Loads</param>
        /// <param name="name">The value set to Name property</param>
        public UsageMap(Loads dataBuffer, String name)
        {
            m_dataBuffer = dataBuffer;
            m_set = false;
            m_name = name;
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="dataBuffer">The reference of Loads</param>
        /// <param name="set">The value set to Set property</param>
        /// <param name="name">The value set to Name property</param>
        public UsageMap(Loads dataBuffer, Boolean set, String name)
        {
            m_dataBuffer = dataBuffer;
            m_set = set;
            m_name = name;
        }
    }
}
