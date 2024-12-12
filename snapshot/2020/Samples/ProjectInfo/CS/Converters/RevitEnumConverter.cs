using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Revit.SDK.Samples.ProjectInfo.CS
{
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
            m_map = EnumMap;
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
    /// Converter for ExportComplexityConverter
    /// </summary>
    public class ExportComplexityConverter : RevitEnumConverter
    {
        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type">enumeration type</param>
        public ExportComplexityConverter(Type type) : base(type) { }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the enum-string map
        /// </summary>
        protected override Dictionary<object, string> EnumMap
        {
            get
            {
                return RevitStartInfo.ExportComplexityMap;
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
    /// Converter for HVACLoadLoadsReportType
    /// </summary>
    public class HVACLoadLoadsReportTypeConverter : RevitEnumConverter
    {
        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type">enumeration type</param>
        public HVACLoadLoadsReportTypeConverter(Type type) : base(type) { }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the enum-string map
        /// </summary>
        protected override Dictionary<object, string> EnumMap
        {
            get
            {
                return RevitStartInfo.HVACLoadLoadsReportTypeMap;
            }
        }
        #endregion
    };

    /// <summary>
    /// Converter for HVACLoadConstructionClass
    /// </summary>
    public class HVACLoadConstructionClassConverter : RevitEnumConverter
    {
        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type">enumeration type</param>
        public HVACLoadConstructionClassConverter(Type type) : base(type) { }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the enum-string map
        /// </summary>
        protected override Dictionary<object, string> EnumMap
        {
            get
            {
                return RevitStartInfo.HVACLoadConstructionClassMap;
            }
        }
        #endregion
    };
    
}