using System;
using System.Collections.Generic;
using System.ComponentModel;
using Autodesk.Revit.DB;

namespace Revit.SDK.Samples.ProjectInfo.CS
{
    /// <summary>
    /// Converts ProjectLocation with string
    /// </summary>
    public class ProjectLocationConverter: TypeConverter
    {
        /// <summary>
        /// All project locations in current document
        /// </summary>
        public static List<ProjectLocation> ProjectLocations;
        /// <summary>
        /// User defined location
        /// </summary>
        public const string UserDefined = "User Defined";

        /// <summary>
        /// Initialize ProjectLocations
        /// </summary>
        static ProjectLocationConverter()
        {
            ProjectLocations = new List<ProjectLocation>();
            foreach (ProjectLocation city in RevitStartInfo.RevitDoc.ProjectLocations)
            {
                ProjectLocations.Add(city);
            }
        }

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
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(ProjectLocations);
        }

        /// <summary>
        /// Converts from string. 
        /// </summary>
        /// <returns>Converted string</returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }
        /// <summary>
        /// Converts to string. 
        /// </summary>
        /// <returns>Converted string</returns>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType.Equals(typeof(string)) || base.CanConvertTo(context, destinationType);
        }

        /// <summary>
        /// Converts from string. 
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
                foreach (ProjectLocation projectLocation in ProjectLocations)
                {
                    if (projectLocation.Name == text)
                        return projectLocation;
                }
            }
            return base.ConvertFrom(context, culture, value);
        }
        /// <summary>
        /// Converts to string. 
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
                if (value == null) return UserDefined;
                ProjectLocation projectLocation = value as ProjectLocation;
                if (projectLocation != null)
                    return projectLocation.Name;
                else
                    return null;
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
        #endregion
    }
}