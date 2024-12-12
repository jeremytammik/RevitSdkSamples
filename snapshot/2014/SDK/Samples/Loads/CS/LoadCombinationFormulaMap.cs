//
// (C) Copyright 2003-2013 by Autodesk, Inc. 
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

namespace Revit.SDK.Samples.Loads.CS
{
    /// <summary>
    /// The map class which store the data and display in formulaDataGridView
    /// </summary>
    public class FormulaMap
    {
        // Private Members
        Double m_factor;    // Indicate the factor column of Formula DataGridView control
        String m_caseName;  // Indicate case column of Formula DataGridView control

        /// <summary>
        /// Factor
        /// </summary>
        public Double Factor
        {
            get
            {
                return m_factor;
            }
            set
            {
                m_factor = value;
            }
        }

        /// <summary>
        /// Load Case
        /// </summary>
        public String Case
        {
            get
            {
                return m_caseName;
            }
            set
            {
                m_caseName = value;
            }
        }

        // Methods
        /// <summary>
        /// Default constructor of FormulaMap
        /// </summary>
        public FormulaMap()
        {
            m_factor = 0;
        }

        /// <summary>
        /// constructor with the case name
        /// </summary>
        /// <param name="caseName">The value set to Case Property</param>
        public FormulaMap(String caseName)
        {
            m_factor = 1;
            m_caseName = caseName;
        }

        /// <summary>
        /// Constructor with the factor and case name
        /// </summary>
        /// <param name="factor">The value set to Factor Property</param>
        /// <param name="caseName">The value set to Case Property</param>
        public FormulaMap(double factor, String caseName)
        {
            m_factor = factor;
            m_caseName = caseName;
        }
    }
}
