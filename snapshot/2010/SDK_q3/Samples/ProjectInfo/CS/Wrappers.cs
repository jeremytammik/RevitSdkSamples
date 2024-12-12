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
using System.Text;

using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Parameters;
using Autodesk.Revit.Symbols;
using Autodesk.Revit.Site;

namespace Revit.SDK.Samples.ProjectInfo.CS
{
    /// <summary>
    /// wrapper interface
    /// </summary>
    public interface IWrapper
    {
        #region Properties
        /// <summary>
        /// Gets the handle object.
        /// </summary>
        object Handle
        {
            get;
        }

        /// <summary>
        /// Gets the name of the handle.
        /// </summary>
        string Name
        {
            get;
        } 
        #endregion
    }

    /// <summary>
    /// Wrapper class for ProjectInfo
    /// </summary>
    public class ProjectInfoWrapper : IWrapper
    {
        #region Fields
        /// <summary>
        /// ProjectInfo
        /// </summary>
        private Autodesk.Revit.Elements.ProjectInfo m_projectInfo; 
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes private variables.
        /// </summary>
        /// <param name="projectInfo">ProjectInfo</param>
        public ProjectInfoWrapper(Autodesk.Revit.Elements.ProjectInfo projectInfo)
        {
            m_projectInfo = projectInfo;
        } 
        #endregion

        #region Properties
        /// <summary>
        /// Gets gbXMLSettings
        /// </summary>
        [Category("Energy Analysis"), DisplayName("Energy Data"), TypeConverter(typeof(WrapperConverter)),
        RevitVersion(RevitStartInfo.RME, RevitStartInfo.RAC)]
        public ICustomTypeDescriptor gbXMLSettings
        {
            get
            {
                return new WrapperCustomDescriptor(new gbXMLParamElemWrapper(m_projectInfo.gbXMLSettings));
            }
        }

        /// <summary>
        /// Gets or sets Project Issue Data
        /// </summary>
        [Category("Other"), DisplayName("Project Issue Data")]
        public String IssueDate
        {
            get
            {
                return m_projectInfo.IssueDate;
            }
            set
            {
                m_projectInfo.IssueDate = value;
            }
        }

        /// <summary>
        /// Gets or sets Project Status
        /// </summary>
        [Category("Other"), DisplayName("Project Status")]
        public String Status
        {
            get
            {
                return m_projectInfo.Status;
            }
            set
            {
                m_projectInfo.Status = value;
            }
        }

        /// <summary>
        /// Gets or sets Client Name
        /// </summary>
        [Category("Other"), DisplayName("Client Name")]
        public String ClientName
        {
            get
            {
                return m_projectInfo.ClientName;
            }
            set
            {
                m_projectInfo.ClientName = value;
            }
        }

        /// <summary>
        /// Gets or sets Project Address
        /// </summary>
        [Category("Other"), DisplayName("Project Address")]
        public String Address
        {
            get
            {
                return m_projectInfo.Address;
            }
            set
            {
                m_projectInfo.Address = value;
            }
        }

        /// <summary>
        /// Gets or sets Project Number
        /// </summary>
        [Category("Other"), DisplayName("Project Number")]
        public String Number
        {
            get
            {
                return m_projectInfo.Number;
            }
            set
            {
                m_projectInfo.Number = value;
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
                return m_projectInfo;
            }
        }

        /// <summary>
        /// Gets the name of the handle.
        /// </summary>
        [Category("Other"), DisplayName("Project Name")]
        public String Name
        {
            get
            {
                return m_projectInfo.Name;
            }
            set
            {
                m_projectInfo.Name = value;
            }
        }
        #endregion 
        #endregion
    }

    /// <summary>
    /// Wrapper class for gbXMLParamElem
    /// </summary>
    public class gbXMLParamElemWrapper : IWrapper
    {
        #region Fields
        /// <summary>
        /// gbXMLParamElem
        /// </summary>
        private gbXMLParamElem m_gbXMLParamElem;

        /// <summary>
        /// SliverSpaceTolerance Parameter
        /// </summary>
        private Parameter m_sliverSpaceToleranceParameter;

        /// <summary>
        /// Export Default Values Parameter
        /// </summary>
        private Parameter m_exportDefaultValuesParameter; 
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes private variables.
        /// </summary>
        /// <param name="gbXMLParamElem">gbXMLParamElem</param>
        public gbXMLParamElemWrapper(gbXMLParamElem gbXMLParamElem)
        {
            m_gbXMLParamElem = gbXMLParamElem;
            m_sliverSpaceToleranceParameter = m_gbXMLParamElem.get_Parameter(BuiltInParameter.RBS_ENERGY_ANALYSIS_SLIVER_SPACE_TOLERANCE);
            m_exportDefaultValuesParameter = m_gbXMLParamElem.get_Parameter(BuiltInParameter.RBS_ENERGY_ANALYSIS_EXPORT_GBXML_DEFAULTS_PARAM);
        } 
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets Project Phase
        /// </summary>
        [DisplayName("Project Phase"), TypeConverter(typeof(ElementConverter))]
        public Phase ProjectPhase
        {
            get
            {
                return m_gbXMLParamElem.ProjectPhase;
            }
            set
            {
                m_gbXMLParamElem.ProjectPhase = value;
            }
        }

        /// <summary>
        /// Gets or sets Export Complexity
        /// </summary>
        [DisplayName("Export Complexity"), TypeConverter(typeof(ExportComplexityConverter)), RevitVersion(RevitStartInfo.RME, RevitStartInfo.RAC)]
        public ExportComplexity ExportComplexity
        {
            get
            {
                return m_gbXMLParamElem.ExportComplexity;
            }
            set
            {
                m_gbXMLParamElem.ExportComplexity = value;
            }
        }

        /// <summary>
        /// Gets or sets Export Default Values
        /// </summary>
        [DisplayName("Export Default Values"), RevitVersion(RevitStartInfo.RME)]
        public bool ExportDefaultValues
        {
            get
            {
                return m_exportDefaultValuesParameter.AsInteger() == 1;
            }
            set
            {
                if(!m_exportDefaultValuesParameter.Set(value ? 1 : 0))
                {
                    throw new ArgumentOutOfRangeException("value");
                }
            }
        }

        /// <summary>
        /// Gets or sets Ground Plane
        /// </summary>
        [DisplayName("Ground Plane"), TypeConverter(typeof(ElementConverter)), RevitVersion(RevitStartInfo.RME, RevitStartInfo.RAC)]
        public Level GroundPlane
        {
            get
            {
                return m_gbXMLParamElem.GroundPlane;
            }
            set
            {
                m_gbXMLParamElem.GroundPlane = value;
            }
        }

        /// <summary>
        /// Gets Building Construction
        /// </summary>
        [DisplayName("Building Construction"), TypeConverter(typeof(WrapperConverter)), RevitVersion(RevitStartInfo.RME)]
        public MEPBuildingConstructionWrapper BuildingConstruction
        {
            get
            {
                return new MEPBuildingConstructionWrapper(m_gbXMLParamElem.BuildingConstruction);
            }
        }

        /// <summary>
        /// Gets or sets Building Service
        /// </summary>
        [DisplayName("Building Service"), TypeConverter(typeof(ServiceTypeConverter)), RevitVersion(RevitStartInfo.RME)]
        public ServiceType BuildingService
        {
            get
            {
                return m_gbXMLParamElem.BuildingService;
            }
            set
            {
                m_gbXMLParamElem.BuildingService = value;
            }
        }

        /// <summary>
        /// Gets Location
        /// </summary>
        [DisplayName("Location"), TypeConverter(typeof(WrapperConverter)), RevitVersion(RevitStartInfo.RME)]
        public ProjectLocationWrapper ProjectLocation
        {
            get
            {
                return new ProjectLocationWrapper(m_gbXMLParamElem.ProjectLocation);
            }
        }

        /// <summary>
        /// Gets or sets Postal Code
        /// </summary>
        [DisplayName("Postal Code")]
        public String PostalCode
        {
            get
            {
                return m_gbXMLParamElem.PostalCode;
            }
            set
            {
                m_gbXMLParamElem.PostalCode = value;
            }
        }

        /// <summary>
        /// Gets or sets Building Type
        /// </summary>
        [DisplayName("Building Type"), TypeConverter(typeof(BuildingTypeConverter))]
        public BuildingType BuildingType
        {
            get
            {
                return m_gbXMLParamElem.BuildingType;
            }
            set
            {
                m_gbXMLParamElem.BuildingType = value;
            }
        }

        /// <summary>
        /// Gets or sets Sliver Space Tolerance
        /// </summary>
        [DisplayName("Sliver Space Tolerance")]
        public String SliverSpaceTolerance
        {
            get
            {
                return m_sliverSpaceToleranceParameter.AsValueString();
            }
            set
            {
                if(!m_sliverSpaceToleranceParameter.SetValueString(value))
                {
                    throw new ArgumentOutOfRangeException("value");
                }

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
                return m_gbXMLParamElem;
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
                return m_gbXMLParamElem.Name;
            }
            set
            {
                m_gbXMLParamElem.Name = value;
            }
        }
        #endregion 
        #endregion
    }

    /// <summary>
    /// Wrapper class for MEPBuildingConstruction
    /// </summary>
    public class MEPBuildingConstructionWrapper : IWrapper
    {
        #region Fields
        /// <summary>
        /// MEPBuildingConstruction
        /// </summary>
        private MEPBuildingConstruction m_mEPBuildingConstruction; 
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes private variables.
        /// </summary>
        /// <param name="mEPBuildingConstruction">MEPBuildingConstruction</param>
        public MEPBuildingConstructionWrapper(MEPBuildingConstruction mEPBuildingConstruction)
        {
            m_mEPBuildingConstruction = mEPBuildingConstruction;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets Exterior Walls
        /// </summary>
        [DisplayName("Exterior Walls")]
        public ConstructionWrapper ExteriorWalls
        {
            get
            {
                return new ConstructionWrapper(m_mEPBuildingConstruction.get_BuildingConstruction(ConstructionType.ExteriorWalls));
            }
            set
            {
                m_mEPBuildingConstruction.set_BuildingConstruction(ConstructionType.ExteriorWalls, value.Handle as Construction);
            }
        }

        /// <summary>
        /// Gets or sets Interior Walls
        /// </summary>
        [DisplayName("Interior Walls")]
        public ConstructionWrapper InteriorWalls
        {
            get
            {
                return new ConstructionWrapper(m_mEPBuildingConstruction.get_BuildingConstruction(ConstructionType.InteriorWalls));
            }
            set
            {
                m_mEPBuildingConstruction.set_BuildingConstruction(ConstructionType.InteriorWalls, value.Handle as Construction);
            }
        }

        /// <summary>
        /// Gets or sets Slabs
        /// </summary>
        [DisplayName("Slabs")]
        public ConstructionWrapper Slabs
        {
            get
            {
                return new ConstructionWrapper(m_mEPBuildingConstruction.get_BuildingConstruction(ConstructionType.Slabs));
            }
            set
            {
                m_mEPBuildingConstruction.set_BuildingConstruction(ConstructionType.Slabs, value.Handle as Construction);
            }
        }

        /// <summary>
        /// Gets or sets Roofs
        /// </summary>
        [DisplayName("Roofs")]
        public ConstructionWrapper Roofs
        {
            get
            {
                return new ConstructionWrapper(m_mEPBuildingConstruction.get_BuildingConstruction(ConstructionType.Roofs));
            }
            set
            {
                m_mEPBuildingConstruction.set_BuildingConstruction(ConstructionType.Roofs, value.Handle as Construction);
            }
        }

        /// <summary>
        /// Gets or sets Floors
        /// </summary>
        [DisplayName("Floors")]
        public ConstructionWrapper Floors
        {
            get
            {
                return new ConstructionWrapper(m_mEPBuildingConstruction.get_BuildingConstruction(ConstructionType.Floors));
            }
            set
            {
                m_mEPBuildingConstruction.set_BuildingConstruction(ConstructionType.Floors, value.Handle as Construction);
            }
        }

        /// <summary>
        /// Gets or sets Doors
        /// </summary>
        [DisplayName("Doors")]
        public ConstructionWrapper Doors
        {
            get
            {
                return new ConstructionWrapper(m_mEPBuildingConstruction.get_BuildingConstruction(ConstructionType.Doors));
            }
            set
            {
                m_mEPBuildingConstruction.set_BuildingConstruction(ConstructionType.Doors, value.Handle as Construction);
            }
        }

        /// <summary>
        /// Gets or sets Exterior Windows
        /// </summary>
        [DisplayName("Exterior Windows")]
        public ConstructionWrapper ExteriorWindows
        {
            get
            {
                return new ConstructionWrapper(m_mEPBuildingConstruction.get_BuildingConstruction(ConstructionType.ExteriorWindows));
            }
            set
            {
                m_mEPBuildingConstruction.set_BuildingConstruction(ConstructionType.ExteriorWindows, value.Handle as Construction);
            }
        }

        /// <summary>
        /// Gets or sets Interior Windows
        /// </summary>
        [DisplayName("Interior Windows")]
        public ConstructionWrapper InteriorWindows
        {
            get
            {
                return new ConstructionWrapper(m_mEPBuildingConstruction.get_BuildingConstruction(ConstructionType.InteriorWindows));
            }
            set
            {
                m_mEPBuildingConstruction.set_BuildingConstruction(ConstructionType.InteriorWindows, value.Handle as Construction);
            }
        }

        /// <summary>
        /// Gets or sets Skylights
        /// </summary>
        [DisplayName("Skylights")]
        public ConstructionWrapper Skylights
        {
            get
            {
                return new ConstructionWrapper(m_mEPBuildingConstruction.get_BuildingConstruction(ConstructionType.Skylights));
            }
            set
            {
                m_mEPBuildingConstruction.set_BuildingConstruction(ConstructionType.Skylights, value.Handle as Construction);
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
                return m_mEPBuildingConstruction;
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
                return m_mEPBuildingConstruction.Name;
            }
            set
            {
                m_mEPBuildingConstruction.Name = value;
            }
        }
        #endregion
        #endregion

        #region Methods
        /// <summary>
        /// Get constructions
        /// </summary>
        /// <param name="constructionType">ConstructionType</param>
        /// <returns>Related Constructions specified by constructionTypes</returns>
        public Autodesk.Revit.Collections.Set get_Constructions(ConstructionType constructionType)
        {
            return m_mEPBuildingConstruction.get_Constructions(constructionType);
        } 
        #endregion
    }

    /// <summary>
    /// Wrapper class for Construction
    /// </summary>
    [TypeConverter(typeof(ConstructionWrapperConverter))]
    public class ConstructionWrapper : IComparable, IWrapper
    {
        #region Fields
        /// <summary>
        /// Construction
        /// </summary>
        private Construction m_construction;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes private variables.
        /// </summary>
        /// <param name="construction">Construction</param>
        public ConstructionWrapper(Construction construction)
        {
            m_construction = construction;
        } 
        #endregion

        #region Properties
        #region IComparable Members

        /// <summary>
        /// Compares the names of Constructions.
        /// </summary>
        /// <param name="obj">ConstructionWrapper used to compare</param>
        /// <returns>A 32-bit signed integer that indicates the relative order of the objects
        /// being compared. The return value has these meanings:
        /// Value Condition Less than zero This instance is less than value.
        /// Zero This instance is equal to value. Greater than zero This instance is
        /// greater than value.-or- value is null.</returns>
        public int CompareTo(object obj)
        {
            ConstructionWrapper wrapper = obj as ConstructionWrapper;
            if (wrapper != null)
            {
                return this.Name.CompareTo(wrapper.Name);
            }
            return 1;
        }

        #endregion

        #region IWrapper Members

        /// <summary>
        /// Gets the handle object.
        /// </summary>
        [Browsable(false)]
        public object Handle
        {
            get { return m_construction; }
        }

        /// <summary>
        /// Gets the name of the handle.
        /// </summary>
        [Browsable(false)]
        public String Name
        {
            get
            {
                return m_construction.Name;
            }
        }
        #endregion 
        #endregion
    }

    /// <summary>
    /// Wrapper class for ProjectLocation
    /// </summary>
    public class ProjectLocationWrapper : IWrapper
    {
        #region Fields
        /// <summary>
        /// ProjectLocation
        /// </summary>
        private ProjectLocation m_projectLocation; 
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes private variables.
        /// </summary>
        /// <param name="projectLocation">ProjectLocation</param>
        public ProjectLocationWrapper(ProjectLocation projectLocation)
        {
            m_projectLocation = projectLocation;
        } 
        #endregion

        #region Properties
        /// <summary>
        /// Gets Site Location
        /// </summary>
        [DisplayName("Site Location"), TypeConverter(typeof(WrapperConverter))]
        public SiteLocationWrapper SiteLocation
        {
            get
            {
                return new SiteLocationWrapper(m_projectLocation.SiteLocation);
            }
        }

        #region IWrapper Members

        /// <summary>
        /// Gets the handle object.
        /// </summary>
        [Browsable(false)]
        public object Handle
        {
            get { return m_projectLocation; }
        }

        /// <summary>
        /// Gets the name of the handle.
        /// </summary>
        public String Name
        {
            get
            {
                return m_projectLocation.Name;
            }
            set
            {
                m_projectLocation.Name = value;
            }
        }
        #endregion 
        #endregion
    }

    /// <summary>
    /// Wrapper class for SiteLocation
    /// </summary>
    public class SiteLocationWrapper : IWrapper
    {
        #region Fields
        /// <summary>
        /// SiteLocation
        /// </summary>
        private SiteLocation m_siteLocation; 
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes private variables.
        /// </summary>
        /// <param name="siteLocation"></param>
        public SiteLocationWrapper(SiteLocation siteLocation)
        {
            m_siteLocation = siteLocation;
        } 
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets TimeZone
        /// </summary>
        [DisplayName("Time Zone"), TypeConverter(typeof(TimeZoneConverter))]
        public String TimeZone
        {
            get
            {
                return GetTimeZoneFromDouble(m_siteLocation.TimeZone);
            }
            set
            {
                m_siteLocation.TimeZone = GetTimeZoneFromString(value);
            }
        }

        /// <summary>
        /// Gets or sets Longitude
        /// </summary>
        [DisplayName("Longitude")]
        public String Longitude
        {
            get
            {
                return Double2AngleString(m_siteLocation.Longitude);
            }
            set
            {
                m_siteLocation.Longitude = AngleString2Double(value);
            }
        }

        /// <summary>
        /// Gets or sets Latitude
        /// </summary>
        [DisplayName("Latitude")]
        public String Latitude
        {
            get
            {
                return Double2AngleString(m_siteLocation.Latitude);
            }
            set
            {
                m_siteLocation.Latitude = AngleString2Double(value);
            }
        }

        #region IWrapper Members

        /// <summary>
        /// Gets the handle object.
        /// </summary>
        [Browsable(false)]
        public object Handle
        {
            get { return m_siteLocation; }
        }

        /// <summary>
        /// Gets the name of the handle.
        /// </summary>
        [Browsable(false)]
        public String Name
        {
            get
            {
                return m_siteLocation.Name;
            }
            set
            {
                m_siteLocation.Name = value;
            }
        }
        #endregion 

        #endregion

        #region Methods

        /// <summary>
        /// Get time zone double value from time zone string
        /// </summary>
        /// <param name="value">time zone string</param>
        /// <returns>the value of time zone</returns>
        private double GetTimeZoneFromString(string value)
        {
            //i.e. convert "(GMT-12:00) International Date Line West" to 12.0
            //i.e. convert "(GMT-03:30) Newfoundland" to 3.30
            string timeZoneDouble = value.Substring(4, value.IndexOf(')') - 4).Replace(':', '.').Trim();
            if (string.IsNullOrEmpty(timeZoneDouble))
                return 0d;
            else
                return Double.Parse(timeZoneDouble);
        }

        /// <summary>
        /// Get time zone display string from time zone value
        /// </summary>
        /// <param name="timeZone">zone value</param>
        /// <returns>display string</returns>
        private string GetTimeZoneFromDouble(double timeZone)
        {
            // i.e. get "(GMT-04:00) Santiago" from double number 4.0
            // should find the last one who matches the time zone
            string lastTimeZone = null;
            foreach (string tmpTimeZone in RevitStartInfo.TimeZones)
            {
                object tmpZone = this.GetTimeZoneFromString(tmpTimeZone);
                if ((double)tmpZone == timeZone)
                    lastTimeZone = tmpTimeZone;
            }
            return lastTimeZone;
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
        private string Double2AngleString(Double value)
        {
            // 0xb0 is ASCII for unit flag of "degree"
            return ((object)Math.Round(value / 0.0174532925199433, 3)).ToString() + (char)0xb0;
        } 
        #endregion
    }
}
