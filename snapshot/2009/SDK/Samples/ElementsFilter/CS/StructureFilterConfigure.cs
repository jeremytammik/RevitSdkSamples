//
// (C) Copyright 2003-2008 by Autodesk, Inc.
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
using System.Collections.Generic;
using System.Text;

using Autodesk.Revit;
using Autodesk.Revit.Structural.Enums;

namespace Revit.SDK.Samples.ElementsFilter.CS
{
    /// <summary>
    /// List all classifications.
    /// </summary>
    public enum ClassificationFilter
    {
        /// <summary>
        /// classify by instance usage
        /// </summary>
        InstanceUsage,
        /// <summary>
        /// classify by wall usage
        /// </summary>
        WallUsage,
        /// <summary>
        /// classify by material usage
        /// </summary>
        Material,
        /// <summary>
        /// classify by structural type
        /// </summary>
        StructuralType
    }

    /// <summary>
    /// Structure filter configure data management.
    /// </summary>
    public class StructureFilterConfigure : FilterConfigure
    {
        private ClassificationFilter m_selectedClassificationFilter;
        private InstanceUsage m_selectedInstanceUsage;
        private WallUsage m_selectedWallUsage;
        private Material m_selectedMaterial;
        private StructuralType m_selectedStructuralType;

        /// <summary>
        /// Generate a new FilterInfo object according to the configure.
        /// </summary>
        /// <param name="filterName"></param>
        /// <returns></returns>
        public override FilterInfo GenerateFilterInfo(string filterName)
        {
            FilterInfo newFilterInfo = new FilterInfo(filterName);
            
            switch (m_selectedClassificationFilter)
            {
                case ClassificationFilter.InstanceUsage:
                    newFilterInfo.Filter = m_commandData.Application.Create.Filter.NewInstanceUsageFilter(m_selectedInstanceUsage);
                    break;
                case ClassificationFilter.Material:
                    newFilterInfo.Filter = m_commandData.Application.Create.Filter.NewMaterialFilter(m_selectedMaterial);
                    break;
                case ClassificationFilter.StructuralType:
                    newFilterInfo.Filter = m_commandData.Application.Create.Filter.NewStructuralTypeFilter(m_selectedStructuralType);
                    break;
                case ClassificationFilter.WallUsage:
                    newFilterInfo.Filter = m_commandData.Application.Create.Filter.NewWallUsageFilter(m_selectedWallUsage);
                    break;
                default:
                    break;
            }

            newFilterInfo.TipMessage = string.Format("The {0} {1} {2} == {3}."
                            , newFilterInfo.Name
                            , ResourceMgr.GetString("CommonDes")
                            , m_selectedClassificationFilter.ToString()
                            , m_selectedWallUsage.ToString());
            return (null == newFilterInfo.Filter) ? null : newFilterInfo;
        }

        /// <summary>
        /// Fill out the data to generate structure filter.
        /// </summary>
        /// <param name="commandData"></param>
        public override void Fill(Autodesk.Revit.ExternalCommandData commandData)
        {
            if (null != m_commandData)
                return;
            m_commandData = commandData;
        }

        /// <summary>
        /// Get all classifications.
        /// </summary>
        public Array ClassificationFilters
        {
            get
            {
                return Enum.GetValues(typeof(ClassificationFilter));
            }
        }

        /// <summary>
        /// Get/Set the selected classification.
        /// </summary>
        public object SelectedClassificationFilter
        {
            get
            {
                return m_selectedClassificationFilter;
            }
            set
            {
                m_selectedClassificationFilter = (ClassificationFilter)value;
            }
        }

        /// <summary>
        /// Get all instance usages.
        /// </summary>
        public Array InstanceUsages
        {
            get
            {
                return Enum.GetValues(typeof(InstanceUsage));
            }
        }


        /// <summary>
        /// Get/Set the selected instance usage.
        /// </summary>
        public object SelectedInstanceUsage
        {
            get
            {
                return m_selectedInstanceUsage;
            }
            set
            {
                m_selectedInstanceUsage = (InstanceUsage)value;
            }
        }

        /// <summary>
        /// Get all wall usages.
        /// </summary>
        public Array WallUsages
        {
            get
            {
                return Enum.GetValues(typeof(WallUsage));
            }
        }

        /// <summary>
        /// Get/Set the selected wall usage.
        /// </summary>
        public object SelectedWallUsage
        {
            get
            {
                return m_selectedWallUsage;
            }
            set
            {
                m_selectedWallUsage = (WallUsage)value;
            }
        }

        /// <summary>
        /// Get all materials.
        /// </summary>
        public Array Materials
        {
            get
            {
                return Enum.GetValues(typeof(Material));
            }
        }

        /// <summary>
        /// Get/Set the selected material.
        /// </summary>
        public object SelectedMaterial
        {
            get
            {
                return m_selectedMaterial;
            }
            set
            {
                m_selectedMaterial = (Material)value;
            }
        }

        /// <summary>
        /// Get all structure types.
        /// </summary>
        public Array StructuralTypes
        {
            get
            {
                return Enum.GetValues(typeof(StructuralType));
            }
        }

        /// <summary>
        /// Get/Set the selected structure type.
        /// </summary>
        public object SelectedStructuralType
        {
            get
            {
                return m_selectedStructuralType;
            }
            set
            {
                m_selectedStructuralType = (StructuralType)value;
            }
        }        
    }
}
