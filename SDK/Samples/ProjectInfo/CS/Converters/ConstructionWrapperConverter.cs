using System;
using System.Collections.Generic;
using System.ComponentModel;
using Autodesk.Revit.DB;
using ConstructionType = Autodesk.Revit.DB.Analysis.ConstructionType;

namespace Revit.SDK.Samples.ProjectInfo.CS
{
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
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            List<ConstructionWrapper> list = new List<ConstructionWrapper>();
            // convert property name to ConstructionType
            ConstructionType constructionType = (ConstructionType)Enum.Parse(typeof(ConstructionType), context.PropertyDescriptor.Name);

            // convert instance to MEPBuildingConstructionWrapper
            MEPBuildingConstructionWrapper mEPBuildingConstruction = context.Instance as MEPBuildingConstructionWrapper;

            // get all Constructions from MEPBuildingConstructionWrapper and add them to a list
            foreach (Construction con in mEPBuildingConstruction.GetConstructions(constructionType))
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
}