using System;
using System.ComponentModel;
using Autodesk.Revit.ApplicationServices;

namespace Revit.SDK.Samples.ProjectInfo.CS
{
    /// <summary>
    /// Wrapper class for ProjectInfo
    /// </summary>
    public class ProjectInfoWrapper : IWrapper
    {
        #region Fields
        /// <summary>
        /// ProjectInfo
        /// </summary>
        private Autodesk.Revit.DB.ProjectInfo m_projectInfo; 
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes private variables.
        /// </summary>
        /// <param name="projectInfo">ProjectInfo</param>
        public ProjectInfoWrapper(Autodesk.Revit.DB.ProjectInfo projectInfo)
        {
            m_projectInfo = projectInfo;
        } 
        #endregion

        #region Properties
        /// <summary>
        /// Gets gbXMLSettings
        /// </summary>
        [Category("Energy Analysis"), DisplayName("Energy Settings")]
        [TypeConverter(typeof(WrapperConverter))]
        [RevitVersion(ProductType.MEP, ProductType.Architecture)]
        public ICustomTypeDescriptor EnergyDataSettings
        {
            get
            {
                return new WrapperCustomDescriptor(new EnergyDataSettingsWrapper(m_projectInfo.Document));
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
}