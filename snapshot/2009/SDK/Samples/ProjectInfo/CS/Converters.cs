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
using System.ComponentModel;
using System.Collections.Generic;
using System.Reflection;

using Autodesk.Revit;
using Autodesk.Revit.Parameters;

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

    /// <summary>
    /// Type converter for Element
    /// </summary>
    public class ElementConverter : TypeConverter
    {
        #region Methods
        /// <summary>
        /// Returns whether this object supports a standard set of values that can be
        /// picked from a list, using the specified context.
        /// </summary>
        /// <returns>true</returns>
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        /// <summary>
        /// Returns whether the collection of standard values returned from System.ComponentModel.TypeConverter.GetStandardValues()
        /// is an exclusive list.
        /// </summary>
        /// <returns>true</returns>
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }

        /// <summary>
        /// Returns a collection of standard values from the default context for the
        /// data type this type converter is designed for.
        /// </summary>
        /// <returns>Element collection retrieved through filtering current Revit document elements</returns>
        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            Type targetType = context.PropertyDescriptor.PropertyType;

            // using type filter to get the target type objects
            Autodesk.Revit.TypeFilter typeFilter = RevitStartInfo.RevitApp.Create.Filter.NewTypeFilter(targetType, true);
            ElementIterator elementIterator = RevitStartInfo.RevitDoc.get_Elements(typeFilter);

            // create a list
            List<Element> list = new List<Element>();
            elementIterator.Reset();
            while (elementIterator.MoveNext())
            {
                list.Add(elementIterator.Current as Element);
            }

            return new StandardValuesCollection(list);
        }

        /// <summary>
        /// Can convert from string
        /// </summary>
        /// <param name="context">An ITypeDescriptorContext that provides a format context. </param>
        /// <param name="sourceType">A Type that represents the type you want to convert from. </param>
        /// <returns>true if sourceType is string, otherwise false</returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        /// Can be converted to string
        /// </summary>
        /// <param name="context"></param>
        /// <param name="destinationType"></param>
        /// <returns>true if destinationType is string, otherwise false</returns>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType.Equals(typeof(System.String)) || base.CanConvertTo(context, destinationType);
        }

        /// <summary>
        /// Returns an element from a string contains its id
        /// </summary>
        /// <param name="context">An System.ComponentModel.ITypeDescriptorContext 
        /// that provides a format context.</param>
        /// <param name="culture">An optional System.Globalization.CultureInfo. 
        /// If not supplied, the current culture is assumed.</param>
        /// <param name="value">string to be converted to an element</param>
        /// <returns>An element if the element exists, otherwise null</returns>
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            string text = value as string;
            if (!string.IsNullOrEmpty(text))
            {
                Type targetType = context.PropertyDescriptor.PropertyType;

                // using type filter to get the target type objects
                Autodesk.Revit.TypeFilter typeFilter = RevitStartInfo.RevitApp.Create.Filter.NewTypeFilter(targetType, true);
                ElementIterator elementIterator = RevitStartInfo.RevitDoc.get_Elements(typeFilter);
                elementIterator.Reset();
                // find one element using its name
                while (elementIterator.MoveNext())
                {
                    if((elementIterator.Current as Element).Name == text)
                    {
                        return elementIterator.Current;
                    }
                }
            }
            return base.ConvertFrom(context, culture, value);
        }

        /// <summary>
        /// Returns element name.
        /// returns empty string if value is null or Element.Name throws an exception.
        /// </summary>
        /// <param name="context">An ITypeDescriptorContext that provides a format context. </param>
        /// <param name="culture">A CultureInfo. If null is passed, the current culture is assumed. </param>
        /// <param name="value">The Object to convert. </param>
        /// <param name="destinationType">The Type to convert the value parameter to. </param>
        /// <returns>Converted string</returns>
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == null)
            {
                throw new ArgumentNullException("destinationType");
            }
            if (destinationType == typeof(string))
            {
                if (value == null) return string.Empty;
                Element element = value as Element;
                if (element != null)
                {
                    string elementName = string.Empty;
                    try
                    {
                        elementName = element.Name;
                    }
                    catch
                    {
                    }
                    return elementName;
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        } 
        #endregion
    };

    /// <summary>
    /// Converter for ConstructionWrapper
    /// </summary>
    public class ConstructionWrapperConverter : TypeConverter
    {
        #region Methods
        /// <summary>
        /// Returns whether this object supports a standard set of values that can be
        /// picked from a list, using the specified context.
        /// </summary>
        /// <returns>true</returns>
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        /// <summary>
        /// Returns whether the collection of standard values returned from System.ComponentModel.TypeConverter.GetStandardValues()
        /// is an exclusive list.
        /// </summary>
        /// <returns>true</returns>
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }

        /// <summary>
        /// Returns a collection of standard values from the default context for the
        /// data type this type converter is designed for.
        /// </summary>
        /// <returns>ConstructionWrapper collection depends on current context</returns>
        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            List<ConstructionWrapper> list = new List<ConstructionWrapper>();
            // convert property name to ConstructionType
            ConstructionType constructionType = (ConstructionType)Enum.Parse(typeof(ConstructionType), context.PropertyDescriptor.Name);

            // convert instance to MEPBuildingConstructionWrapper
            MEPBuildingConstructionWrapper mEPBuildingConstruction = context.Instance as MEPBuildingConstructionWrapper;

            // get all Constructions from MEPBuildingConstructionWrapper and add them to a list
            foreach (Construction con in mEPBuildingConstruction.get_Constructions(constructionType))
            {
                list.Add(new ConstructionWrapper(con));
            }

            // sort the list
            list.Sort();
            return new StandardValuesCollection(list);
        }

        /// <summary>
        /// Can convert from string
        /// </summary>
        /// <param name="context">An ITypeDescriptorContext that provides a format context. </param>
        /// <param name="sourceType">A Type that represents the type you want to convert from. </param>
        /// <returns>true if sourceType is string, otherwise false</returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        /// Can be converted to string
        /// </summary>
        /// <returns>true if destinationType is string, otherwise false</returns>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType.Equals(typeof(System.String)) || base.CanConvertTo(context, destinationType);
        }

        /// <summary>
        /// Returns a ConstructionWrapper from a string
        /// </summary>
        /// <param name="context">An System.ComponentModel.ITypeDescriptorContext 
        /// that provides a format context.</param>
        /// <param name="culture">An optional System.Globalization.CultureInfo. 
        /// If not supplied, the current culture is assumed.</param>
        /// <param name="value">string to be converted to</param>
        /// <returns>A ConstructionWrapper from the StandardValues</returns>
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            string text = value as string;
            if (!string.IsNullOrEmpty(text))
            {
                foreach (ConstructionWrapper con in this.GetStandardValues(context))
                {
                    if (con.Name == text)
                    {
                        return con;
                    }
                }
            }
            return base.ConvertFrom(context, culture, value);
        }

        /// <summary>
        /// Convert object to string
        /// </summary>
        /// <param name="context">An ITypeDescriptorContext that provides a format context. </param>
        /// <param name="culture">A CultureInfo. If null is passed, the current culture is assumed. </param>
        /// <param name="value">The Object to convert. </param>
        /// <param name="destinationType">The Type to convert the value parameter to. </param>
        /// <returns>empty string if current construction is null, otherwise construction name</returns>
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == null)
            {
                throw new ArgumentNullException("destinationType");
            }
            if (destinationType == typeof(string))
            {
                if (value == null) return string.Empty;
                ConstructionWrapper construction = value as ConstructionWrapper;
                if (construction != null)
                {
                    return construction.Name;
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        } 
        #endregion
    };

    /// <summary>
    /// Converter for Enumeration types of RevitAPI
    /// </summary>
    public abstract class RevitEnumConverter : EnumConverter
    {
        #region Fields
        /// <summary>
        /// Dictionary contains enum and string map
        /// </summary>
        Dictionary<object, string> m_map = null;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the enum-string map
        /// </summary>
        protected abstract Dictionary<object, string> EnumMap
        {
            get;
        } 
        #endregion

        #region Constructors
        /// <summary>
        /// Initialize private variables
        /// </summary>
        /// <param name="type">Enumeration type</param>
        public RevitEnumConverter(Type type)
            : base(type)
        {
            m_map = this.EnumMap;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns a collection of standard values from the default context for the
        /// data type this type converter is designed for.
        /// </summary>
        /// <returns>All enum items</returns>
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(m_map.Keys);
        }

        /// <summary>
        /// Returns an enum item from a string
        /// </summary>
        /// <param name="context">An System.ComponentModel.ITypeDescriptorContext 
        /// that provides a format context.</param>
        /// <param name="culture">An optional System.Globalization.CultureInfo. 
        /// If not supplied, the current culture is assumed.</param>
        /// <param name="value">string to be converted to</param>
        /// <returns>An enum item</returns>
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            object enumValue = value;
            string valueText = value.ToString();
            foreach (KeyValuePair<object, string> pair in m_map)
            {
                if (pair.Value == valueText)
                {
                    enumValue = pair.Key.ToString();
                }
            }
            return base.ConvertFrom(context, culture, enumValue);
        }

        /// <summary>
        /// Convert enum item to string
        /// </summary>
        /// <param name="context">An ITypeDescriptorContext that provides a format context. </param>
        /// <param name="culture">A CultureInfo. If null is passed, the current culture is assumed. </param>
        /// <param name="value">The Object to convert. </param>
        /// <param name="destinationType">The Type to convert the value parameter to. </param>
        /// <returns>Corresponding string related with the enum item</returns>
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            object enumValue = base.ConvertTo(context, culture, value, destinationType);
            object enumObject = Enum.Parse(this.EnumType, enumValue.ToString());
            return m_map[enumObject];
        } 
        #endregion
    };

    /// <summary>
    /// Converter for BuildingType
    /// </summary>
    public class BuildingTypeConverter : RevitEnumConverter
    {
        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type">enumeration type</param>
        public BuildingTypeConverter(Type type) : base(type) { } 
        #endregion

        #region Properties
        /// <summary>
        /// Gets the enum-string map
        /// </summary>
        protected override Dictionary<object, string> EnumMap
        {
            get
            {
                return RevitStartInfo.BuildingTypeMap;
            }
        } 
        #endregion
    };

    /// <summary>
    /// Converter for ServiceType
    /// </summary>
    public class ServiceTypeConverter : RevitEnumConverter
    {
        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type">enumeration type</param>
        public ServiceTypeConverter(Type type) : base(type) { } 
        #endregion

        #region Properties
        /// <summary>
        /// Gets the enum-string map
        /// </summary>
        protected override Dictionary<object, string> EnumMap
        {
            get
            {
                return RevitStartInfo.ServiceTypeMap;
            }
        } 
        #endregion
    };

    /// <summary>
    /// Converter used to convert TimeZone
    /// </summary>
    public class TimeZoneConverter : TypeConverter
    {
        #region Methods
        /// <summary>
        /// Returns whether this object supports a standard set of values that can be
        /// picked from a list, using the specified context.
        /// </summary>
        /// <returns>true</returns>
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        /// <summary>
        /// Returns whether the collection of standard values returned from System.ComponentModel.TypeConverter.GetStandardValues()
        /// is an exclusive list.
        /// </summary>
        /// <returns>true</returns>
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }

        /// <summary>
        /// Returns a collection of standard values from the default context for the
        /// data type this type converter is designed for.
        /// </summary>
        /// <returns>Element collection retrieved through filtering current Revit document elements</returns>
        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(RevitStartInfo.TimeZones);
        }
        #endregion
    };
}
