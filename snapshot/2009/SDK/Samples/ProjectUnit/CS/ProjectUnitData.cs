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
using System.Collections;
using System.Collections.Generic;
using System.Text;

using Autodesk.Revit.Elements;

namespace Revit.SDK.Samples.ProjectUnit.CS
{
    /// <summary>
    /// Project unit data class.
    /// </summary>
    public class ProjectUnitData
    {
        #region Fields
        //Required designer variable.
        private Autodesk.Revit.Elements.ProjectUnit m_projectUnit;
        private List<Autodesk.Revit.FormatOptions> m_formatOptionList = 
            new List<Autodesk.Revit.FormatOptions>();
        private Autodesk.Revit.Enums.DecimalSymbolType m_decimalSymbolType;
        private Autodesk.Revit.Enums.DigitGroupingAmount m_digitGroupingAmount;
        private Autodesk.Revit.Enums.DigitGroupingSymbolType m_digitGroupingSymbolType;
        #endregion 

        #region Functions
        /// <summary>
        /// Initializes a new instance of ProjectUnitData 
        /// </summary>
        /// <param name="projectUnit">an object of Autodesk.Revit.Elements.projectUnit</param>
        public ProjectUnitData(Autodesk.Revit.Elements.ProjectUnit projectUnit)
        {
            m_formatOptionList.Clear();
            m_projectUnit = projectUnit;
            foreach (Autodesk.Revit.Enums.UnitType unittype in Enum.GetValues(typeof(
                Autodesk.Revit.Enums.UnitType)))
            {
               try
               {
                   Autodesk.Revit.FormatOptions formatOption = m_projectUnit.get_FormatOptions(unittype);                 
               }
               catch (System.Exception /*e*/)
               {
                   continue;
               }

               if (m_projectUnit.get_FormatOptions(unittype) == null)
               {
                   continue;
               }

               m_formatOptionList.Add(m_projectUnit.get_FormatOptions(unittype));
            }
        }

        /// <summary>
        /// set the project unit Digit grouping type.
        /// </summary>
        public void SetDigitGroupingType()
        {
            m_projectUnit.setDigitGroupingType(m_decimalSymbolType, m_digitGroupingSymbolType, m_digitGroupingAmount);
        }
        #endregion 

        #region Properties
        /// <summary>
        /// project unit decimal symbol type  property. 
        /// </summary>
        public Autodesk.Revit.Enums.DecimalSymbolType DecimalSyType
        {
            get 
            {
                return this.m_projectUnit.DecimalSymbolType;
            }
            set
            {
                m_decimalSymbolType = value;
            }
        }

        /// <summary>
        /// project unit Digit grouping amount property. 
        /// </summary>
        public Autodesk.Revit.Enums.DigitGroupingAmount DigitGroupingAmount
        {
            get
            {
                return this.m_projectUnit.DigitGroupingAmount;
            }
            set
            {
                m_digitGroupingAmount = value;
            }
        }

        /// <summary>
        /// project unit Digit grouping symbol type  property. 
        /// </summary>
        public Autodesk.Revit.Enums.DigitGroupingSymbolType DigitGroupingSymbolType
        {
            get
            {
                return this.m_projectUnit.DigitGroupingSymbolType;
            }
            set
            {
                m_digitGroupingSymbolType = value;
            }
        }

        /// <summary>
        /// project unit format option list property.
        /// </summary>
        public List<Autodesk.Revit.FormatOptions> FormatOptionList
        {
            get
            {
                return this.m_formatOptionList;
            }
        }
        #endregion         
    }
}
