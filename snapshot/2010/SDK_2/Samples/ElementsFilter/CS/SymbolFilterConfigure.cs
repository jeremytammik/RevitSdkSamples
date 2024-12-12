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
using System.Collections.Generic;
using System.Text;

using Autodesk.Revit;
using Autodesk.Revit.Symbols;

namespace Revit.SDK.Samples.ElementsFilter.CS
{

    /// <summary>
    /// Friendly symbol display name in UI
    /// </summary>
    public class FriendlySymbolName
    {
        private string m_prefixCategoryName;
        private string m_prefixFamilyName;
        private string m_value;

        /// <summary>
        /// UI friendly name of a symbol.
        /// </summary>
        /// <param name="symbol"></param>
        public FriendlySymbolName(Symbol symbol)
        {
            if (null != symbol.Category)
            {
                m_prefixCategoryName = symbol.Category.Name;
            }
            else
            {
                m_prefixCategoryName = null;
            }

            FamilySymbol fs = symbol as FamilySymbol;
            if (null != fs)
            {
                m_prefixFamilyName = fs.Family.Name;
            }
            else
            {
                m_prefixFamilyName = null;
            }
            
            m_value = symbol.Name;
        }

        /// <summary>
        /// The friendly name string.
        /// </summary>
        public string FriendlyName
        {
            get
            {
                if (null == m_prefixCategoryName && null == m_prefixFamilyName)
                {
                    return m_value;
                }

                if (null == m_prefixFamilyName)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("{0} : {1}", m_prefixCategoryName, m_value);
                    return sb.ToString();
                }

                if (null == m_prefixCategoryName)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("{0} : {1}", m_prefixFamilyName, m_value);
                    return sb.ToString();
                }

                StringBuilder fullSb = new StringBuilder();
                fullSb.AppendFormat("{0} : {1} : {2}", m_prefixCategoryName, m_prefixFamilyName, m_value);
                return fullSb.ToString();
                
            }
        }

        /// <summary>
        /// The symbol name.
        /// </summary>
        public string Value
        {
            get
            {
                return m_value;
            }
        }
    }

    /// <summary>
    /// Symbol filter configure data management.
    /// </summary>
    public class SymbolFilterConfigure : FilterConfigure
    {
        private List<FriendlySymbolName> m_symbolNames;
        private FriendlySymbolName m_selectedSymbolName;

        /// <summary>
        /// Generate a new FilterInfo object according to the configure.
        /// </summary>
        /// <param name="filterName">What's the filter name</param>
        /// <returns></returns>
        public override FilterInfo GenerateFilterInfo(string filterName)
        {
            FilterInfo newFilterInfo = new FilterInfo(filterName);

            if (null == m_selectedSymbolName)
                return null;

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("The {0} {1} {2}"
                , newFilterInfo.Name
                , ResourceMgr.GetString("SymbolDes")
                , m_selectedSymbolName.FriendlyName);
            newFilterInfo.TipMessage = sb.ToString();
            Filter newFilter = m_commandData.Application.Create.Filter.NewSymbolFilter(m_selectedSymbolName.Value);
            if (null == newFilter)
                return null;

            newFilterInfo.Filter = newFilter;
            return newFilterInfo;
        }

        /// <summary>
        /// Fill out the data to generate symbol filter.
        /// </summary>
        /// <param name="commandData">Revit command data</param>
        public override void Fill(Autodesk.Revit.ExternalCommandData commandData)
        {
            if (null != m_commandData)
                return;
            m_commandData = commandData;

            if (null != m_symbolNames)
                return;

            m_symbolNames = new List<FriendlySymbolName>();
            List<string> nameList = new List<string>();

            Filter filter = commandData.Application.Create.Filter.NewTypeFilter(typeof(Symbol));
            List<Element> symbols = new List<Element>();
            commandData.Application.ActiveDocument.get_Elements(filter, symbols);

            foreach (Element element in symbols)
            {
                Symbol symbol = element as Symbol;
                if (null == symbol)
                    continue;

                FriendlySymbolName friendlyName = new FriendlySymbolName(symbol);
                if (!nameList.Contains(friendlyName.FriendlyName))
                {
                    nameList.Add(friendlyName.FriendlyName);
                    m_symbolNames.Add(friendlyName);
                }
            }
        }

        /// <summary>
        /// Get all symbol names.
        /// </summary>
        public List<FriendlySymbolName> SymbolNames
        {
            get
            {
                return m_symbolNames;
            }
        }

        /// <summary>
        /// Get/Set the selected symbol name.
        /// </summary>
        public FriendlySymbolName SelectedSymbolName
        {
            get
            {
                return m_selectedSymbolName;
            }
            set
            {
                m_selectedSymbolName = value;
            }
        }
    }
}
