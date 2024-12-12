using System;
using System.ComponentModel;
using System.Linq;
using Autodesk.Revit.DB;

namespace Revit.SDK.Samples.ProjectInfo.CS
{
    /// <summary>
    /// Convert ElementIds with string
    /// </summary>
    /// <typeparam name="T">Element Type</typeparam>
    public class ElementIdConverter<T> : TypeConverter where T: Element
    {
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
            // using type filter to get the target type objects
            //Autodesk.Revit.DB.TypeFilter typeFilter = RevitStartInfo.RevitApp.Create.Filter.NewTypeFilter(targetType, true);
            //ElementIterator elementIterator = RevitStartInfo.RevitDoc.get_Elements(typeFilter);

            //// create a list
            //List<Element> list = new List<Element>();
            //elementIterator.Reset();
            //while (elementIterator.MoveNext())
            //{
            //    list.Add(elementIterator.Current as Element);
            //}
            var list = new FilteredElementCollector(RevitStartInfo.RevitDoc).OfClass(typeof(T));

            return new StandardValuesCollection(list.ToElementIds().ToList());
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
                StandardValuesCollection svc = GetStandardValues(context);
                foreach (ElementId elementId in svc)
                {
                    Element element = RevitStartInfo.GetElement(elementId);
                    if (element.Name == text)
                        return element.Id;
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
                ElementId elementId = value as ElementId;
                if (elementId != null)
                {
                    Element element = RevitStartInfo.GetElement(elementId);
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
                
            }
            return base.ConvertTo(context, culture, value, destinationType);
        } 
    };
}