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
using System.ComponentModel;
using System.Globalization;

namespace Revit.SDK.Samples.NewRebar.CS
{
    /// <summary>
    /// Type converter between RebarShapeParameter and string is provided for property grid.
    /// </summary>
    class TypeConverterRebarShapeParameter : TypeConverter
    {
        /// <summary>
        /// RebarShape parameters list.
        /// </summary>
        public static List<RebarShapeParameter> RebarShapeParameters;

        /// <summary>
        /// Returns whether this converter can convert the object to the specified type.
        /// </summary>
        /// <param name="context">An System.ComponentModel.ITypeDescriptorContext that 
        /// provides a format context.</param>
        /// <param name="destinationType">A System.Type that represents the type you want
        /// to convert to.</param>
        /// <returns></returns>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(string);
        }

        /// <summary>
        /// Converts the given value object to the specified type, using the specified
        /// context and culture information.
        /// </summary>
        /// <param name="context">An System.ComponentModel.ITypeDescriptorContext that 
        /// provides a format context.</param>
        /// <param name="culture">A System.Globalization.CultureInfo. If null is passed,
        /// the current culture is assumed.</param>
        /// <param name="value">The System.Object to convert.</param>
        /// <param name="destinationType">The System.Type to convert the value parameter
        /// to.</param>
        /// <returns>An System.Object that represents the converted value.</returns>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture,
            object value, Type destinationType)
        {
            if (destinationType == typeof(String) && value is RebarShapeParameter)
            {
                RebarShapeParameter param = value as RebarShapeParameter;
                if (null != param)
                {
                    return param.Name;
                }
            }
            throw new Exception("Can't be converted to other types except string.");
        }

        /// <summary>       
        /// Returns whether this converter can convert an object of the given type to
        /// the type of this converter, using the specified context.
        /// </summary>
        /// <param name="context">An System.ComponentModel.ITypeDescriptorContext that 
        /// provides a format context.</param>
        /// <param name="sourceType">A System.Type that represents the type you want to 
        /// convert from.</param>
        /// <returns>true if this converter can perform the conversion; otherwise,
        /// false.</returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        /// <summary>
        /// Converts the given object to the type of this converter, using the specified
        /// context and culture information.
        /// </summary>
        /// <param name="context">An System.ComponentModel.ITypeDescriptorContext that 
        /// provides a format context.</param>
        /// <param name="culture">The System.Globalization.CultureInfo to use as the 
        /// current culture.</param>
        /// <param name="value">The System.Object to convert.</param>
        /// <returns>An System.Object that represents the converted value.</returns>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture,
            object value)
        {
            if(value is String)
            {
                foreach(RebarShapeParameter param in
                    TypeConverterRebarShapeParameter.RebarShapeParameters)
                {
                    if(param.Name.Equals(value))
                    {
                        return param;
                    }
                }
            }
            throw new Exception("Can't be converted from other types except from string.");
        }

        /// <summary>
        /// Returns whether this object supports a standard set of values that can be
        /// picked from a list.
        /// </summary>
        /// <param name="context"></param>
        /// <returns>true if System.ComponentModel.TypeConverter.GetStandardValues() should be
        /// called to find a common set of values the object supports; otherwise, false.</returns>
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        /// <summary>
        /// Returns a collection of standard values for the data type this type converter
        /// is designed for when provided with a format context.
        /// </summary>
        /// <param name="context">An System.ComponentModel.ITypeDescriptorContext that 
        /// provides a format context
        /// that can be used to extract additional information about the environment
        /// from which this converter is invoked. This parameter or properties of this
        /// parameter can be null.</param>
        /// <returns>A System.ComponentModel.TypeConverter.StandardValuesCollection that holds
        /// a standard set of valid values, or null if the data type does not support
        /// a standard set of values.</returns>
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(RebarShapeParameters);
        }

        /// <summary>
        /// Returns whether the collection of standard values returned from 
        /// System.ComponentModel.TypeConverter.GetStandardValues()
        /// is an exclusive list of possible values, using the specified context.
        /// </summary>
        /// <param name="context">An System.ComponentModel.ITypeDescriptorContext that 
        /// provides a format context.</param>
        /// <returns>true if the System.ComponentModel.TypeConverter.StandardValuesCollection
        /// returned from System.ComponentModel.TypeConverter.GetStandardValues() is
        /// an exhaustive list of possible values; false if other values are possible.</returns>
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }
    }
}
