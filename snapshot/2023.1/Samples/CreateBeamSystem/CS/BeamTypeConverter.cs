//
// (C) Copyright 2003-2019 by Autodesk, Inc.
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


namespace Revit.SDK.Samples.CreateBeamSystem.CS
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Collections;
    using System.ComponentModel;
    using System.Globalization;

    using Autodesk.Revit;
    using Autodesk.Revit.DB;

    /// <summary>
    /// base class of converting types of FamilySymbol to string
    /// Code here have nothing with Revit API
    /// it's only for PropertyGrid and its SelectedObject
    /// </summary>
    public abstract class ParameterConverter : TypeConverter
    {
        /// <summary>
        /// hashtable of FamilySymbol and its Name
        /// </summary>
        protected Dictionary<string, FamilySymbol> m_hash;

        /// <summary>
        /// subclass must implement to initialize m_hash
        /// </summary>
        public abstract void GetConvertHash();

        /// <summary>
        /// returns whether this object supports a standard set of values that can be picked from a list
        /// </summary>
        /// <param name="context">provides a format context</param>
        /// <returns>true if GetStandardValues should be called to find a common set of values the object supports; 
        /// otherwise, false</returns>
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        /// <summary>
        /// returns a collection of FamilySymbol
        /// </summary>
        /// <param name="context">provides a format context</param>
        /// <returns>collection of FamilySymbol</returns>
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(m_hash.Values);
        }

        /// <summary>
        /// whether this converter can convert an object of the given type to string
        /// </summary>
        /// <param name="context">provides a format context</param>
        /// <param name="sourceType">a Type that represents the type you want to convert from</param>
        /// <returns></returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        /// converts the Name to corresponding FamilySymbol
        /// </summary>
        /// <param name="context">provides a format context</param>
        /// <param name="culture">the CultureInfo to use as the current culture</param>
        /// <param name="value">the Object to convert</param>
        /// <returns>an FamilySymbol object</returns>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return m_hash[value.ToString()];
        }

        /// <summary>
        /// converts the given FamilySymbol to the Name, using the specified context and culture information
        /// </summary>
        /// <param name="context">provides a format context</param>
        /// <param name="culture">the CultureInfo to use as the current culture</param>
        /// <param name="v">the Object to convert</param>
        /// <param name="destinationType">should be string</param>
        /// <returns></returns>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object v, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                FamilySymbol symbol = v as FamilySymbol;
                if (null == symbol)
                {
                    return "";
                }

                foreach (KeyValuePair<string, FamilySymbol> kvp in m_hash)
                {
                    if (kvp.Value.Id == symbol.Id)
                    {
                        return kvp.Key;
                    }
                }
                return "";
            }

            return base.ConvertTo(context, culture, v, destinationType);
        }

        /// <summary>
        /// whether the collection of standard values returned 
        /// from GetStandardValues is an exclusive list of possible values
        /// </summary>
        /// <param name="context">provides a format context</param>
        /// <returns></returns>
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }
        
        /// <summary>
        /// constructor initialize m_hash
        /// </summary>
        protected ParameterConverter()
        {
            GetConvertHash();
        }
    }

    /// <summary>
    /// converting types of FamilySymbol to string
    /// Code here have nothing with Revit API
    /// it's only for PropertyGrid and its SelectedObject
    /// </summary>
    public class BeamTypeItem : ParameterConverter
    {
        /// <summary>
        /// override the base type's GetConvertHash method
        /// </summary>
        public override void GetConvertHash()
        {
            m_hash = BeamSystemData.GetBeamTypes();
        }
    }
}
