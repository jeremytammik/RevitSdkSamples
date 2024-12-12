using System.Collections.Generic;
using System.ComponentModel;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using ConstructionType = Autodesk.Revit.DB.Analysis.ConstructionType;

namespace Revit.SDK.Samples.ProjectInfo.CS
{
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
        /// Gets or sets Roofs
        /// </summary>
        [DisplayName("Roofs")]
        public ConstructionWrapper Roof
        {
            get
            {
                return new ConstructionWrapper(m_mEPBuildingConstruction.GetBuildingConstruction(ConstructionType.Roof));
            }
            set
            {
                m_mEPBuildingConstruction.SetBuildingConstruction(ConstructionType.Roof, value.Handle as Construction);
            }
        }

        /// <summary>
        /// Gets or sets Exterior Walls
        /// </summary>
        [DisplayName("Exterior Walls")]
        public ConstructionWrapper ExteriorWall
        {
            get
            {
                return new ConstructionWrapper(m_mEPBuildingConstruction.GetBuildingConstruction(ConstructionType.ExteriorWall));
            }
            set
            {
                m_mEPBuildingConstruction.SetBuildingConstruction(ConstructionType.ExteriorWall, value.Handle as Construction);
            }
        }

        /// <summary>
        /// Gets or sets Interior Walls
        /// </summary>
        [DisplayName("Interior Walls")]
        public ConstructionWrapper InteriorWall
        {
            get
            {
                return new ConstructionWrapper(m_mEPBuildingConstruction.GetBuildingConstruction(ConstructionType.InteriorWall));
            }
            set
            {
                m_mEPBuildingConstruction.SetBuildingConstruction(ConstructionType.InteriorWall, value.Handle as Construction);
            }
        }

        /// <summary>
        /// Gets or sets Ceilings
        /// </summary>
        [DisplayName("Ceilings")]
        public ConstructionWrapper Ceiling
        {
            get
            {
                return new ConstructionWrapper(m_mEPBuildingConstruction.GetBuildingConstruction(ConstructionType.Ceiling));
            }
            set
            {
                m_mEPBuildingConstruction.SetBuildingConstruction(ConstructionType.Ceiling, value.Handle as Construction);
            }
        }

        /// <summary>
        /// Gets or sets Doors
        /// </summary>
        [DisplayName("Doors")]
        public ConstructionWrapper Door
        {
            get
            {
                return new ConstructionWrapper(m_mEPBuildingConstruction.GetBuildingConstruction(ConstructionType.Door));
            }
            set
            {
                m_mEPBuildingConstruction.SetBuildingConstruction(ConstructionType.Door, value.Handle as Construction);
            }
        }

        /// <summary>
        /// Gets or sets Slabs
        /// </summary>
        [DisplayName("Slabs")]
        public ConstructionWrapper Slab
        {
            get
            {
                return new ConstructionWrapper(m_mEPBuildingConstruction.GetBuildingConstruction(ConstructionType.Slab));
            }
            set
            {
                m_mEPBuildingConstruction.SetBuildingConstruction(ConstructionType.Slab, value.Handle as Construction);
            }
        }

        /// <summary>
        /// Gets or sets Floors
        /// </summary>
        [DisplayName("Floors")]
        public ConstructionWrapper Floor
        {
            get
            {
                return new ConstructionWrapper(m_mEPBuildingConstruction.GetBuildingConstruction(ConstructionType.Floor));
            }
            set
            {
                m_mEPBuildingConstruction.SetBuildingConstruction(ConstructionType.Floor, value.Handle as Construction);
            }
        }

        /// <summary>
        /// Gets or sets Exterior Windows
        /// </summary>
        [DisplayName("Exterior Windows")]
        public ConstructionWrapper ExteriorWindow
        {
            get
            {
                return new ConstructionWrapper(m_mEPBuildingConstruction.GetBuildingConstruction(ConstructionType.ExteriorWindow));
            }
            set
            {
                m_mEPBuildingConstruction.SetBuildingConstruction(ConstructionType.ExteriorWindow, value.Handle as Construction);
            }
        }

        /// <summary>
        /// Gets or sets Interior Windows
        /// </summary>
        [DisplayName("Interior Windows")]
        public ConstructionWrapper InteriorWindow
        {
            get
            {
                return new ConstructionWrapper(m_mEPBuildingConstruction.GetBuildingConstruction(ConstructionType.ExteriorWindow));
            }
            set
            {
                m_mEPBuildingConstruction.SetBuildingConstruction(ConstructionType.ExteriorWindow, value.Handle as Construction);
            }
        }

        /// <summary>
        /// Gets or sets Skylights
        /// </summary>
        [DisplayName("Skylights")]
        public ConstructionWrapper Skylight
        {
            get
            {
                return new ConstructionWrapper(m_mEPBuildingConstruction.GetBuildingConstruction(ConstructionType.Skylight));
            }
            set
            {
                m_mEPBuildingConstruction.SetBuildingConstruction(ConstructionType.Skylight, value.Handle as Construction);
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
        public ICollection<Construction> GetConstructions(ConstructionType constructionType)
        {
            return m_mEPBuildingConstruction.GetConstructions(constructionType);
        }
        #endregion
    }
}