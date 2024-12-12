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


namespace Revit.SDK.Samples.AreaReinParameters.CS
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Globalization;

    using Autodesk.Revit;
    using Autodesk.Revit.Parameters;
    using Autodesk.Revit.Elements;

    /// <summary>
    /// converter between ElementId and Element's name
    /// </summary>
    public abstract class ParameterConverter : TypeConverter
    {
        /// <summary>
        /// hash table save
        /// </summary>
        protected Hashtable m_hash = null;
        
        /// <summary>
        /// initialize m_hash
        /// </summary>
        protected ParameterConverter()
        {
            m_hash = new Hashtable();
            GetConvertHash();
        }

        /// <summary>
        /// fill m_hashtable
        /// </summary>
        public abstract void GetConvertHash();

        /// <summary>
        /// always return true
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        /// <summary>
        /// provide ElementId collection
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override StandardValuesCollection GetStandardValues(
            ITypeDescriptorContext context)
        {
            ParameterConverter.StandardValuesCollection standardValues= null;

            ElementId[] Ids = new ElementId[m_hash.Values.Count];
            int i = 0;

            foreach (DictionaryEntry de in m_hash)
            {
                Ids[i++] = (ElementId)(de.Value);
            }
            standardValues = new StandardValuesCollection(Ids);

            return standardValues;
        }

        /// <summary>
        /// whether conversion is allowable
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sourceType"></param>
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
        /// convert from Name to ElementId
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="v">Name</param>
        /// <returns>ElementId</returns>
        public override object ConvertFrom(ITypeDescriptorContext context, 
            CultureInfo culture, object v)
        {
            if (v is string)
            {
                foreach (DictionaryEntry de in m_hash)
                {
                    if (de.Key.Equals(v.ToString()))
                    {
                        return de.Value;
                    }
                }
            }

            return base.ConvertFrom(context, culture, v);
        }

        /// <summary>
        /// convert from ElementId to Name
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="v">ElementId</param>
        /// <param name="destinationType">String</param>
        /// <returns>Name</returns>
        public override object ConvertTo(ITypeDescriptorContext context, 
            CultureInfo culture, object v, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                foreach (DictionaryEntry de in m_hash)
                {
                    ElementId tmpId = (ElementId)de.Value;
                    ElementId cmpId = (ElementId)v;

                    if (tmpId.Value == cmpId.Value)
                    {
                        return de.Key.ToString();
                    }
                }
                return "";
            }
            return base.ConvertTo(context, culture, v, destinationType);
        }

        /// <summary>
        /// always return true so that user can't input unexpected text
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }
    }

    /// <summary>
    /// Hook Type
    /// </summary>
    public class HookTypeItem : ParameterConverter
    {
        public override void GetConvertHash()
        {
            m_hash = Command.HookTypes;
            ElementId id = new ElementId();
            id.Value = -1;
            if (!m_hash.ContainsKey("None"))
            {
                m_hash.Add("None", id);
            }
        }
    }

    /// <summary>
    /// Bar Type
    /// </summary>
    public class BarTypeItem : ParameterConverter
    {
        public override void GetConvertHash()
        {
            m_hash = Command.BarTypes;
        }
    }
}