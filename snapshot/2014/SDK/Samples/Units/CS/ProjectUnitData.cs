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
using System.Collections;
using System.Collections.Generic;
using System.Text;

using Autodesk.Revit.DB;

namespace Revit.SDK.Samples.Units.CS
{
    /// <summary>
    /// Project unit data class.
    /// </summary>
    public class ProjectUnitData
    {
        #region Fields
        //Required designer variable.
        private Autodesk.Revit.DB.Units m_units;
        private Dictionary<Autodesk.Revit.DB.UnitType, Autodesk.Revit.DB.FormatOptions> m_unitType_formatOptions =
            new Dictionary<Autodesk.Revit.DB.UnitType, Autodesk.Revit.DB.FormatOptions>();
        private Autodesk.Revit.DB.DecimalSymbol m_decimalSymbol;
        private Autodesk.Revit.DB.DigitGroupingSymbol m_digitGroupingSymbol;
        private Autodesk.Revit.DB.DigitGroupingAmount m_digitGroupingAmount;
        private Dictionary<Autodesk.Revit.DB.UnitType, string> m_unitType_label =
            new Dictionary<Autodesk.Revit.DB.UnitType, string>();
        private Dictionary<Autodesk.Revit.DB.DisplayUnitType, string> m_displayUnitType_label =
            new Dictionary<Autodesk.Revit.DB.DisplayUnitType, string>();
        private Dictionary<Autodesk.Revit.DB.UnitSymbolType, string> m_unitSymbolType_label =
            new Dictionary<Autodesk.Revit.DB.UnitSymbolType, string>();
        #endregion 

        #region Functions
        /// <summary>
        /// Initializes a new instance of ProjectUnitData 
        /// </summary>
        /// <param name="units">an object of Autodesk.Revit.DB.Units</param>
        public ProjectUnitData(Autodesk.Revit.DB.Units units)
        {
           m_unitType_formatOptions.Clear();
            m_units = units;
            foreach (Autodesk.Revit.DB.UnitType unittype in Enum.GetValues(typeof(
                Autodesk.Revit.DB.UnitType)))
            {
               try
               {
                   Autodesk.Revit.DB.FormatOptions formatOptions = m_units.GetFormatOptions(unittype);
                   m_unitType_formatOptions.Add(unittype, formatOptions);
                   m_unitType_label.Add(unittype, Autodesk.Revit.DB.LabelUtils.GetLabelFor(unittype));
               }
               catch
               {
                   continue;
               }
            }
            m_decimalSymbol = units.DecimalSymbol;
            m_digitGroupingSymbol = units.DigitGroupingSymbol;
            m_digitGroupingAmount = units.DigitGroupingAmount;
            foreach (Autodesk.Revit.DB.DisplayUnitType displayunittype in Enum.GetValues(typeof(
                Autodesk.Revit.DB.DisplayUnitType)))
            {
               try
               {
                  m_displayUnitType_label.Add(displayunittype, Autodesk.Revit.DB.LabelUtils.GetLabelFor(displayunittype));
               }
               catch
               {
                  continue;
               }
            }
            foreach (Autodesk.Revit.DB.UnitSymbolType unitsymboltype in Enum.GetValues(typeof(
                Autodesk.Revit.DB.UnitSymbolType)))
            {
               try
               {
                  m_unitSymbolType_label.Add(unitsymboltype, Autodesk.Revit.DB.LabelUtils.GetLabelFor(unitsymboltype));
               }
               catch
               {
                  if (unitsymboltype == Autodesk.Revit.DB.UnitSymbolType.UST_NONE)
                  {
                     m_unitSymbolType_label.Add(unitsymboltype, "(NONE)");
                  }
                  continue;
               }
            }
        }

        /// <summary>
        /// set the project unit Decimal Symbol And Grouping.
        /// </summary>
        public void SetDecimalSymbolAndGrouping(Autodesk.Revit.DB.DecimalSymbol decimalSymbol, Autodesk.Revit.DB.DigitGroupingSymbol digitGroupingSymbol, Autodesk.Revit.DB.DigitGroupingAmount digitGroupingAmount)
        {
            m_units.SetDecimalSymbolAndGrouping(decimalSymbol, digitGroupingSymbol, digitGroupingAmount);
            m_decimalSymbol = decimalSymbol;
            m_digitGroupingSymbol = digitGroupingSymbol;
            m_digitGroupingAmount = digitGroupingAmount;
        }

        /// <summary>
        /// set the format options for special unit type.
        /// </summary>
        public void SetFormatOptions(Autodesk.Revit.DB.UnitType unitType, Autodesk.Revit.DB.FormatOptions options)
        {
            m_units.SetFormatOptions(unitType, options);
            m_unitType_formatOptions[unitType] = options;
        }
        #endregion 

        #region Properties
        /// <summary>
        /// project unit Decimal Symbol property. 
        /// </summary>8-
        public Autodesk.Revit.DB.DecimalSymbol DecimalSymbol
        {
            get 
            {
                return this.m_units.DecimalSymbol;
            }
            set
            {
                m_decimalSymbol = value;
            }
        }

        /// <summary>
        /// project Digit Grouping Symbol property. 
        /// </summary>
        public Autodesk.Revit.DB.DigitGroupingSymbol DigitGroupingSymbol
        {
            get
            {
                return this.m_units.DigitGroupingSymbol;
            }
            set
            {
                m_digitGroupingSymbol = value;
            }
        }

        /// <summary>
        /// project Digit Grouping Amount  property. 
        /// </summary>
        public Autodesk.Revit.DB.DigitGroupingAmount DigitGroupingAmount
        {
            get
            {
                return this.m_units.DigitGroupingAmount;
            }
            set
            {
                m_digitGroupingAmount = value;
            }
        }

        /// <summary>
        /// project UnitType and its label dictionary property.
        /// </summary>
        public Dictionary<Autodesk.Revit.DB.UnitType, string> UnitType_Label
        {
           get
           {
              return this.m_unitType_label;
           }
        }

        /// <summary>
        /// project DisplayUnitType and its label dictionary property.
        /// </summary>
        public Dictionary<Autodesk.Revit.DB.DisplayUnitType, string> DisplayUnitType_Label
        {
           get
           {
              return this.m_displayUnitType_label;
           }
        }

        /// <summary>
        /// project UnitSymbolType and its label dictionary property.
        /// </summary>
        public Dictionary<Autodesk.Revit.DB.UnitSymbolType, string> UnitSymbolType_Label
        {
           get
           {
              return this.m_unitSymbolType_label;
           }
        }

        /// <summary>
        /// project UnitType FormatOptions dictionary property.
        /// </summary>
        public Dictionary<Autodesk.Revit.DB.UnitType, Autodesk.Revit.DB.FormatOptions> UnitType_FormatOptions
        {
            get
            {
                return this.m_unitType_formatOptions;
            }
        }

        /// <summary>
        /// project units. 
        /// </summary>
        public Autodesk.Revit.DB.Units Units
        {
           get
           {
              return this.m_units;
           }
        }

        #endregion         
    }
}
