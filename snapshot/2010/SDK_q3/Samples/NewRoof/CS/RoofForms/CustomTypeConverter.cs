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
using System.ComponentModel;
using System.Collections.ObjectModel;
using Autodesk.Revit.Elements;

namespace Revit.SDK.Samples.NewRoof.RoofForms.CS
{
    /// <summary>
    /// The LevelConverter class is inherited from the TypeConverter class which is used to
    /// show the property which returns Level type as like a combo box in the PropertyGrid control.
    /// </summary>
    public class LevelConverter : TypeConverter
    {
        /// <summary>
        /// To store the levels element
        /// </summary>
        static private Dictionary<String, Level> m_levels = new Dictionary<String, Level>();

        /// <summary>
        /// Initialize the levels data.
        /// </summary>
        /// <param name="levels"></param>
        static public  void SetStandardValues(ReadOnlyCollection<Level> levels)
        {
            m_levels.Clear();
            foreach (Level level in levels)
            {
                m_levels.Add(level.Id.Value.ToString(), level);
            }
        }

        /// <summary>
        /// Get a level by a level id.
        /// </summary>
        /// <param name="id">The id of the level</param>
        /// <returns>Returns a level which id equals the specified id.</returns>
        static public Level GetLevelByID(int id)
        {            
            return m_levels[id.ToString()];
        }

        /// <summary>
        /// Override the CanConvertTo method.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(Level))
                return true;
            return base.CanConvertTo(context, destinationType);
        }

        /// <summary>
        ///  Override the ConvertTo method, convert a level type value to a string type value for displaying in the PropertyGrid.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(String) && value is Level)
            {
                Level level = (Level)value;
                return level.Name + "[" + level.Id.Value.ToString() + "]";
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        /// <summary>
        /// Override the CanConvertFrom method.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(String))
                return true;
            return base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        /// Override the ConvertFrom method, convert a string type value to a level type value.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is String)
            {
                try
                {
                    String levelString = (String)value;

                    int leftBracket = levelString.IndexOf('[');
                    int rightBracket = levelString.IndexOf(']');

                    String idString = levelString.Substring(leftBracket + 1, rightBracket - leftBracket - 1);

                    return m_levels[idString];
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }
            }
            return base.ConvertFrom(context, culture, value);
        }

        /// <summary>
        /// Override the GetStandardValuesSupported method for displaying a level list in the PropertyGrid.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        /// <summary>
        /// Override the StandardValuesCollection method for suppling a level list in the PropertyGrid.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(m_levels.Values);
        }
    }

    /// <summary>
    /// The FootPrintRoofLineConverter class is inherited from the ExpandableObjectConverter class which is used to
    /// expand the property which returns FootPrintRoofLine type as like a tree view in the PropertyGrid control.
    /// </summary>
    public class FootPrintRoofLineConverter : ExpandableObjectConverter
    {
        // To store the FootPrintRoofLines data.
        static private Dictionary<String, FootPrintRoofLine> m_footPrintLines = new Dictionary<String, FootPrintRoofLine>();
        
        /// <summary>
        /// Initialize the FootPrintRoofLines data. 
        /// </summary>
        /// <param name="footPrintRoofLines"></param>
        static public void SetStandardValues(List<FootPrintRoofLine> footPrintRoofLines)
        {
            m_footPrintLines.Clear();
            foreach (FootPrintRoofLine footPrintLine in footPrintRoofLines)
            {
                if (m_footPrintLines.ContainsKey(footPrintLine.Id.ToString()))
                    continue;
                m_footPrintLines.Add(footPrintLine.Id.ToString(), footPrintLine);
            }
        }

        /// <summary>
        /// Override the CanConvertTo method.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(FootPrintRoofLine))
                return true;
            return base.CanConvertTo(context, destinationType);
        }

        /// <summary>
        ///  Override the ConvertTo method, convert a FootPrintRoofLine type value to a string type value for displaying in the PropertyGrid.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(System.String) && value is FootPrintRoofLine)
            {
                FootPrintRoofLine footPrintLine = (FootPrintRoofLine)value;
                return footPrintLine.Name + "[" + footPrintLine.Id.ToString() + "]";
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        /// <summary>
        /// Override the CanConvertFrom method.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;
            return base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        /// Override the ConvertFrom method, convert a string type value to a FootPrintRoofLine type value.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is String)
            {
                try
                {
                    String footPrintLineString = (String)value;

                    int leftBracket = footPrintLineString.IndexOf('[');
                    int rightBracket = footPrintLineString.IndexOf(']');

                    String idString = footPrintLineString.Substring(leftBracket + 1, rightBracket - leftBracket - 1);

                    return m_footPrintLines[idString];
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }
            }
            
            return base.ConvertFrom(context, culture, value);
        }

        /// <summary>
        /// Override the GetStandardValuesSupported method for displaying a FootPrintRoofLine list in the PropertyGrid.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override bool  GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        /// <summary>
        /// Override the StandardValuesCollection method for suppling a FootPrintRoofLine list in the PropertyGrid.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(m_footPrintLines.Values);
        }
    };
}