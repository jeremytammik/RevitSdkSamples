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
using System.ComponentModel;
using System.Collections.Generic;
using System.Reflection;
using Autodesk.Revit;

namespace Revit.SDK.Samples.ProjectInfo.CS
{
    /// <summary>
    /// Type converter for wrapper classes
    /// </summary>
    public class WrapperConverter : ExpandableObjectConverter
    {
        #region Methods
        /// <summary>
        /// Can be converted to string
        /// </summary>
        /// <returns>true if destinationType is string, otherwise false</returns>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType.Equals(typeof(System.String)) || base.CanConvertTo(context, destinationType);
        }

        /// <summary>
        /// Converts to string. If value is null, convert it to "(null)". 
        /// if value has a "Name" property, returns its name. otherwise, returns "(...)".
        /// </summary>
        /// <returns>Converted string</returns>
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == null)
            {
                throw new ArgumentNullException("destinationType");
            }
            if (destinationType == typeof(string))
            {
                if (value == null) return "(null)";

                // get its name
                Type type = value.GetType();
                string wrapperType = type.ToString();
                MethodInfo mi = type.GetMethod("get_Name", new Type[0]);
                if (mi != null)
                {
                    return mi.Invoke(value, new object[0]).ToString();
                }

                // if no name
                return "(...)";
            }
            return base.ConvertTo(context, culture, value, destinationType);
        } 
        #endregion
    };
}

