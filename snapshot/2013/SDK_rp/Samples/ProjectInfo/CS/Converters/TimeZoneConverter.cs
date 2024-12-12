using System.ComponentModel;

namespace Revit.SDK.Samples.ProjectInfo.CS
{
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
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(RevitStartInfo.TimeZones);
        }
        #endregion
    };
}