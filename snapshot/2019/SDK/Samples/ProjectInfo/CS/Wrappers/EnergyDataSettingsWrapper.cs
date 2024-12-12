using System;
using System.ComponentModel;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Analysis;
using Autodesk.Revit.DB.Mechanical;

namespace Revit.SDK.Samples.ProjectInfo.CS
{
    /// <summary>
    /// Wrapper class for gbXMLParamElem
    /// </summary>
    public class EnergyDataSettingsWrapper : IWrapper
    {
        #region Fields
        /// <summary>
        /// gbXMLParamElem
        /// </summary>
        private EnergyDataSettings m_energyDataSettings;
        /// <summary>
        /// Revit Document
        /// </summary>
        private Document m_document;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes private variables.
        /// </summary>
        /// <param name="gbXMLParamElem">gbXMLParamElem</param>
        public EnergyDataSettingsWrapper(Document document)
        {
            m_document = document;
            m_energyDataSettings = EnergyDataSettings.GetFromDocument(document);
        } 
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets Building Type
        /// </summary>
        [Category("Common"), DisplayName("Building Type")]
        [TypeConverter(typeof(BuildingTypeConverter))]
        public gbXMLBuildingType BuildingType
        {
            get
            {
                return m_energyDataSettings.BuildingType;
            }
            set
            {
                m_energyDataSettings.BuildingType = value;
            }
        }

        /// <summary>
        /// Gets or sets Ground Plane
        /// </summary>
        [Category("Common"), DisplayName("Ground Plane")]
        [TypeConverter(typeof(ElementIdConverter<Level>))]
        public ElementId GroundPlane
        {
            get
            {
                return m_energyDataSettings.GroundPlane;
            }
            set
            {
                m_energyDataSettings.GroundPlane = value;
            }
        }

        /// <summary>
        /// Gets or sets Building Service
        /// </summary>
        [Category("Detailed Model"), DisplayName("Building Service")]
        [TypeConverter(typeof(ServiceTypeConverter))]
        [RevitVersion(ProductType.MEP)]
        public gbXMLServiceType BuildingService
        {
            get
            {
                return m_energyDataSettings.ServiceType;
            }
            set
            {
                m_energyDataSettings.ServiceType = value;
            }
        }

        /// <summary>
        /// Gets Building Construction
        /// </summary>
        [Category("Detailed Model"), DisplayName("Building Construction"), TypeConverter(typeof(WrapperConverter)), RevitVersion(ProductType.MEP)]
        public MEPBuildingConstructionWrapper BuildingConstruction
        {
            get
            {
                ElementId eid = EnergyDataSettings.GetBuildingConstructionSetElementId(m_document);
                MEPBuildingConstruction mEPBuildingConstruction = RevitStartInfo.GetElement(eid) as MEPBuildingConstruction;
                //MEPBuildingConstruction mEPBuildingConstruction = RevitStartInfo.GetElement(m_energyDataSettings.ConstructionSetElementId) as MEPBuildingConstruction;
                if(mEPBuildingConstruction != null)
                    return new MEPBuildingConstructionWrapper(mEPBuildingConstruction);
                return null;
            }
        }

        /// <summary>
        /// Gets and Sets BuildingConstructionClass
        /// </summary>
        [Category("Detailed Model"), DisplayName("Building Infiltration Class")]
        [TypeConverter(typeof(HVACLoadConstructionClassConverter))]
        [RevitVersion(ProductType.MEP)]
        public HVACLoadConstructionClass BuildingConstructionClass
        {
            get
            {
                return m_energyDataSettings.BuildingConstructionClass;
            }
            set
            {
                m_energyDataSettings.BuildingConstructionClass = value;
            }
        }

        /// <summary>
        /// Gets or sets Project Phase
        /// </summary>
        [Category("Detailed Model"), DisplayName("Project Phase")]
        [TypeConverter(typeof(ElementIdConverter<Phase>))]
        public ElementId ProjectPhase
        {
            get
            {
                return m_energyDataSettings.ProjectPhase;
            }
            set
            {
                m_energyDataSettings.ProjectPhase = value;
            }
        }

        /// <summary>
        /// Gets or sets Sliver Space Tolerance
        /// </summary>
        [Category("Detailed Model"), DisplayName("Sliver Space Tolerance")]
        public Double SliverSpaceTolerance
        {
            get
            {
                return m_energyDataSettings.SliverSpaceTolerance;
            }
            set
            {
                m_energyDataSettings.SliverSpaceTolerance = value;
            }
        }

        /// <summary>
        /// Gets or sets Export Complexity
        /// </summary>
        [Category("Detailed Model"), DisplayName("Export Complexity")]
        [TypeConverter(typeof(ExportComplexityConverter))]
        public gbXMLExportComplexity ExportComplexity
        {
            get
            {
                return m_energyDataSettings.ExportComplexity;
            }
            set
            {
                m_energyDataSettings.ExportComplexity = value;
            }
        }

        /// <summary>
        /// Gets or sets Export Default Values
        /// </summary>
        [Category("Detailed Model"), DisplayName("Export Default Values")]
        [RevitVersion(ProductType.MEP)]
        public bool ExportDefaultValues
        {
            get
            {
                return m_energyDataSettings.ExportDefaults;
            }
            set
            {
                m_energyDataSettings.ExportDefaults = value;
            }
        }

        /// <summary>
        /// Gets and Sets ProjectReportType
        /// </summary>
        [Category("Detailed Model"), DisplayName("Report Type")]
        [TypeConverter(typeof(HVACLoadLoadsReportTypeConverter))]
        [RevitVersion(ProductType.MEP)]
        public HVACLoadLoadsReportType ProjectReportType
        {
            get
            {
                return m_energyDataSettings.ProjectReportType;
            }
            set
            {
                m_energyDataSettings.ProjectReportType = value;
            }
        }

        /// <summary>
        /// Gets Project Location
        /// </summary>
        [DisplayName("Project Location"), TypeConverter(typeof(ProjectLocationConverter)), RevitVersion(ProductType.MEP, ProductType.Architecture)]
        public ProjectLocation ProjectLocation
        {
            get
            {
                return m_document.ActiveProjectLocation;
            }
            set
            {
                m_document.ActiveProjectLocation = value;
            }
        }

        /// <summary>
        /// Gets Site Location
        /// </summary>
        [DisplayName("Site Location"), TypeConverter(typeof(WrapperConverter)), RevitVersion(ProductType.MEP, ProductType.Architecture)]
        public SiteLocationWrapper SiteLocation
        {
            get
            {
                return new SiteLocationWrapper(m_document.SiteLocation);
            }
        }

        #region IWrapper Members

        /// <summary>
        /// Gets the handle object.
        /// </summary>
        [Browsable(false)]
        public object Handle
        {
            get
            {
                return m_energyDataSettings;
            }
        }

        /// <summary>
        /// Gets the name of the handle.
        /// </summary>
        [Browsable(false)]
        public string Name
        {
            get
            {
                return "";
            }
            set
            {
            }
        }
        #endregion 
        #endregion
    }
}