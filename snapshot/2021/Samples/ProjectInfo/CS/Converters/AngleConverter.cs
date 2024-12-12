using System;
using System.ComponentModel;

namespace Revit.SDK.Samples.ProjectInfo.CS
{
    /// <summary>
    /// Converts angle with string
    /// </summary>
    public class AngleConverter : TypeConverter
    {
        #region Methods
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
                return AngleString2Double(text);
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
                if (value == null) return string.Empty;
                double angle = (double) value;
                return Double2AngleString(angle);
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        /// <summary>
        /// Convert angle string to double value
        /// </summary>
        /// <param name="value">Angle string</param>
        /// <returns>Double value</returns>
        private static double AngleString2Double(string value)
        {
            int n = value.Length - 1;
            if (!char.IsDigit(value[n]))
            {
                value = value.Substring(0, n);
            }
            return Double.Parse(value) * 0.0174532925199433;
        }

        /// <summary>
        /// Convert double value to angle string
        /// </summary>
        /// <param name="value">Angle value</param>
        /// <returns>Angle string, the unit is degree.</returns>
        private static string Double2AngleString(Double value)
        {
            // 0xb0 is ASCII for unit flag of "degree"
            return ((object)Math.Round(value / 0.0174532925199433, 3)).ToString() + (char)0xb0;
        } 
        
        #endregion
    }
}