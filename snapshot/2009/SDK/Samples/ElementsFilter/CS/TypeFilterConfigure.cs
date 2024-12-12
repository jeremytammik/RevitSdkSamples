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
using System.Reflection;

using Autodesk.Revit;

namespace Revit.SDK.Samples.ElementsFilter.CS
{
    /// <summary>
    /// Type filter configure data management.
    /// </summary>
    public class TypeFilterConfigure : FilterConfigure
    {
        private List<Type> m_revitAPITypes;
        private Type m_selectedType;

        /// <summary>
        /// Generate a new FilterInfo object according to the configure.
        /// </summary>
        /// <param name="filterName">What's the filter name</param>
        /// <returns></returns>
        public override FilterInfo GenerateFilterInfo(string filterName)
        {
            FilterInfo newFilterInfo = new FilterInfo(filterName);

            if (null == m_selectedType)
                return null;

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("The {0} {1} {2}"
                , newFilterInfo.Name
                , ResourceMgr.GetString("TypeDes")
                , m_selectedType.FullName);
            newFilterInfo.TipMessage = sb.ToString();
            newFilterInfo.Filter = m_commandData.Application.Create.Filter.NewTypeFilter(m_selectedType);

            return (null == newFilterInfo.Filter) ? null : newFilterInfo;
        }

        /// <summary>
        /// Fill out the data to generate type filter.
        /// </summary>
        /// <param name="commandData">Revit command data</param>
        public override void Fill(Autodesk.Revit.ExternalCommandData commandData)
        {
            if (null != m_commandData)
                return;
            m_commandData = commandData;

            if (null != m_revitAPITypes)
                return;

            m_revitAPITypes = new List<Type>();

            Type[] revitAPITypes = typeof(Element).Assembly.GetExportedTypes();
            foreach (Type type in revitAPITypes)
            {
                if (type.IsSubclassOf(typeof(Element)))
                {
                    m_revitAPITypes.Add(type);
                }
            }
        }

        /// <summary>
        /// Get all RevitAPI element types.
        /// </summary>
        public List<Type> RevitAPITypes
        {
            get
            {
                return m_revitAPITypes;
            }
        }

        /// <summary>
        /// Get/Set the selected type.
        /// </summary>
        public Type SelectedType
        {
            get
            {
                return m_selectedType;
            }
            set
            {
                m_selectedType = value;
            }
        }
    }
}
