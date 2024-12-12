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
using Autodesk.Revit.Elements;

namespace Revit.SDK.Samples.ElementsFilter.CS
{
    /// <summary>
    /// User friendly family display name in UI.
    /// </summary>
    public class FriendlyFamilyName
    {
        private string m_prefix;
        private string m_value;

        /// <summary>
        /// UI friendly name for a family.
        /// </summary>
        /// <param name="family"></param>
        public FriendlyFamilyName(Family family)
        {
            if (null != family.FamilyCategory)
            {
                m_prefix = family.FamilyCategory.Name;
            }
            else
            {
                m_prefix = null;
            }

            m_value = family.Name;
        }

        /// <summary>
        /// Prefix of the friendly name, it is the category of this family.
        /// </summary>
        public string Prefix
        {
            get
            {
                return m_prefix;
            }
        }

        /// <summary>
        /// The family name.
        /// </summary>
        public string Value
        {
            get
            {
                return m_value;
            }
        }

        /// <summary>
        /// The friendly name.
        /// </summary>
        public string FriendlyName
        {
            get
            {
                if (null == m_prefix)
                {
                    return m_value;
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("{0} : {1}", m_prefix, m_value);
                    return sb.ToString();
                }
            }
        }        
    }
    /// <summary>
    /// Family filter configure data management.
    /// </summary>
    public class FamilyFilterConfigure : FilterConfigure
    {
        private List<FriendlyFamilyName> m_familyNames;
        private FriendlyFamilyName m_selectedName;

        /// <summary>
        /// Generate a new FilterInfo object according to the configure.
        /// </summary>
        /// <param name="filterName"></param>
        /// <returns></returns>
        public override FilterInfo GenerateFilterInfo(string filterName)
        {
            FilterInfo newFilterInfo = new FilterInfo(filterName);

            if (null == m_selectedName)
                return null;

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0} {1} {2}", newFilterInfo.Name, ResourceMgr.GetString("FamilyDes")
                , m_selectedName.FriendlyName);
            newFilterInfo.TipMessage = sb.ToString();
            newFilterInfo.Filter = m_commandData.Application.Create.Filter.NewFamilyFilter(m_selectedName.Value);

            return (null == newFilterInfo.Filter) ? null : newFilterInfo;
        }

        /// <summary>
        /// Fill out the data to generate family filter.
        /// </summary>
        /// <param name="commandData"></param>
        public override void Fill(ExternalCommandData commandData)
        {
            if (null != m_commandData)
                return;
            m_commandData = commandData;

            if (null != m_familyNames)
                return;

            m_familyNames = new List<FriendlyFamilyName>();
            Filter filter = commandData.Application.Create.Filter.NewTypeFilter(typeof(Family));
            List<Element> families = new List<Element>();
            commandData.Application.ActiveDocument.get_Elements(filter, families);


            foreach (Element element in families)
            {
                try
                {
                    Family family = element as Family;
                    if (null == family)
                        continue;

                    FriendlyFamilyName friendlyName = new FriendlyFamilyName(family);
                    m_familyNames.Add(friendlyName);
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }

        /// <summary>
        /// Get all family names
        /// </summary>
        public List<FriendlyFamilyName> FamilyNames
        {
            get
            {
                return m_familyNames;
            }
        }

        /// <summary>
        /// Get/Set the selected family name.
        /// </summary>
        public FriendlyFamilyName SelectedName
        {
            get
            {
                return m_selectedName;
            }
            set
            {
                m_selectedName = value;
            }
        }
    }
}
