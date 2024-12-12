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

using Autodesk.Revit;
using Autodesk.Revit.DB;

namespace Revit.SDK.Samples.ModelLines.CS
{
    /// <summary>
    /// The map class which store the data and display in informationDataGridView
    /// </summary>
    public class ModelCurveCounter
    {
        // Private members
        String m_typeName; // type name
        int m_number;               // the number of corresponding type

        // Properties
        /// <summary>
        /// Indicate the type name, such ModelArc, ModelLine, etc
        /// </summary>
        public String TypeName
        {
            get
            {
                return m_typeName;
            }
        }

        /// <summary>
        /// Indicate the number of the corresponding type which name stored in type name
        /// </summary>
        public int Number
        {
            get
            {
                return m_number;
            }
            set
            {
                m_number = value;
            }
        }

        // Methods
        /// <summary>
        /// The constructor of ModelCurveCounter
        /// </summary>
        /// <param name="typeName">The type name</param>
        public ModelCurveCounter(String typeName)
        {
            m_typeName = typeName;
        }
    }


    /// <summary>
    /// The map class which store the information used in elementIdComboBox comboBox in UI
    /// </summary>
    public class IdInfo
    {
        // Private members
        String m_text;     // The display text
        int m_id;          // The real value - id

        // Properties
        /// <summary>
        /// The text displayed in the comboBox, as the DisplayMember
        /// </summary>
        public String DisplayText
        {
            get
            {
                return m_text;
            }
        }

        /// <summary>
        /// The real value of the comboBox, as the ValueMember
        /// </summary>
        public int Id
        {
            get
            {
                return m_id;
            }
        }

        // Methods
        /// <summary>
        /// The constructor of CreateInfo
        /// </summary>
        /// <param name="typeName">indicate model curve type</param>
        /// <param name="id">the element id</param>
        public IdInfo(String typeName, int id)
        {
            m_id = id;          // Store the element id
            
            // Generate the display text
            m_text = typeName + " : " + id.ToString();
        }
    }

}
