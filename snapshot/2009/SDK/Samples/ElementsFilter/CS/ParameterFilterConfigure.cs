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
using Autodesk.Revit.Enums;
using Autodesk.Revit.Parameters;

namespace Revit.SDK.Samples.ElementsFilter.CS
{
    /// <summary>
    /// Parameter filter configure data management.
    /// </summary>
    public class ParameterFilterConfigure : FilterConfigure
    {
        private BuiltInParameter m_selectedBuiltInPara;
        private CriteriaFilterType m_selectedCriteriaFilterType;
        private StorageType m_selectedStorageType;
        private string m_inputValue;

        /// <summary>
        /// Generate a new FilterInfo object according to the configure.
        /// </summary>
        /// <param name="filterName"></param>
        /// <returns></returns>
        public override FilterInfo GenerateFilterInfo(string filterName)
        {
            FilterInfo newFilterInfo = new FilterInfo(filterName);
            if (null == m_inputValue)
                return null;

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0} {1} {2} {3} {4}"
                , newFilterInfo.Name
                , ResourceMgr.GetString("CommonDes")
                , m_selectedBuiltInPara.ToString()
                , m_selectedCriteriaFilterType.ToString()
                , m_inputValue);
            newFilterInfo.TipMessage = sb.ToString();

            switch (m_selectedStorageType)
            {
                case StorageType.Double:
                    {
                        newFilterInfo.Filter = m_commandData.Application.Create.Filter.NewParameterFilter(m_selectedBuiltInPara
                        , m_selectedCriteriaFilterType, double.Parse(m_inputValue));                        
                        break;
                    }

                case StorageType.ElementId:
                    {
                        Autodesk.Revit.ElementId id;
                        id.Value = Int32.Parse(m_inputValue);
                        newFilterInfo.Filter = m_commandData.Application.Create.Filter.NewParameterFilter(m_selectedBuiltInPara
                        , m_selectedCriteriaFilterType, id);
                        break;
                    }
                case StorageType.Integer:
                    {
                        newFilterInfo.Filter = m_commandData.Application.Create.Filter.NewParameterFilter(m_selectedBuiltInPara
                        , m_selectedCriteriaFilterType, Int32.Parse(m_inputValue));
                        break;
                    }
                case StorageType.String:
                    {
                        newFilterInfo.Filter = m_commandData.Application.Create.Filter.NewParameterFilter(m_selectedBuiltInPara
                        , m_selectedCriteriaFilterType, m_inputValue);
                        break;
                    }
                default:
                    break;
            }

            return (null == newFilterInfo.Filter) ? null : newFilterInfo;
        }

        /// <summary>
        /// Fill out the data to generate parameter filter.
        /// </summary>
        /// <param name="commandData">Revit command data</param>
        public override void Fill(Autodesk.Revit.ExternalCommandData commandData)
        {
            if (null != m_commandData)
                return;
            m_commandData = commandData;
        }

        /// <summary>
        /// Get all built-in parameters.
        /// </summary>
        public Array BuiltInParas
        {
            get
            {
                return Enum.GetNames(typeof(BuiltInParameter));
            }
        }

        /// <summary>
        /// Get/Set the selected built-in parameter.
        /// </summary>
        public object SelectedBuiltInPara
        {
            get
            {
                return m_selectedBuiltInPara;
            }
            set
            {
                m_selectedBuiltInPara = (BuiltInParameter)value;
            }
        }

        /// <summary>
        /// Get all the criteria
        /// </summary>
        public Array CriteriaFilterTypes
        {
            get
            {
                return Enum.GetValues(typeof(CriteriaFilterType));
            }
        }

        /// <summary>
        /// Get/Set the selected criteria.
        /// </summary>
        public object SelectedCriteriaFilterType
        {
            get
            {
                return m_selectedCriteriaFilterType;
            }
            set
            {
                m_selectedCriteriaFilterType = (CriteriaFilterType)value;
            }
        }

        /// <summary>
        /// Get all the storage types.
        /// </summary>
        public Array StorageTypes
        {
            get
            {
                return Enum.GetValues(typeof(StorageType));
            }
        }

        /// <summary>
        /// Get/Set the selected storage type.
        /// </summary>
        public object SelectedStorageType
        {
            get
            {
                return m_selectedStorageType;
            }
            set
            {
                m_selectedStorageType = (StorageType)value;
            }
        }

        /// <summary>
        /// Get/Set the input value string.
        /// </summary>
        public string InputValue
        {
            get
            {
                return m_inputValue;
            }
            set
            {
                m_inputValue = value;
            }
        }        
    }
}
