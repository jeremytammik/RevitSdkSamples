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

namespace Revit.SDK.Samples.ElementsFilter.CS
{
    /// <summary>
    /// The boolean operator types.
    /// </summary>
    public enum BooleanOperator
    {
        /// <summary>
        /// Logic operator and
        /// </summary>
        And,
        /// <summary>
        /// Logic operator or
        /// </summary>
        Or,
        /// <summary>
        /// Logic operator not
        /// </summary>
        Not,
        /// <summary>
        /// Add a filter to expression
        /// </summary>
        Add,
        /// <summary>
        /// Empty expression
        /// </summary>
        None
    }

    /// <summary>
    /// Manages all filter and boolean operation.
    /// </summary>
    public class FilterMgr
    {
        private ExternalCommandData m_commandData;
        private List<FilterInfo> m_filters;
        private FilterInfo m_selectedFilterInfo;
        private Filter m_toBeExecutedFilter;
        private string m_booleanExp;
        private BooleanOperator m_booleanOperator;
        private bool m_canBeExecuted;

        /// <summary>
        /// Constructor, get the Revit commandData reference.
        /// </summary>
        /// <param name="commandData"></param>
        public FilterMgr(ExternalCommandData commandData)
        {
            m_commandData = commandData;
            m_filters = new List<FilterInfo>();
            m_booleanOperator = BooleanOperator.None;
            m_canBeExecuted = false;
        }

        /// <summary>
        /// Generate the logic filter via API, and set it to m_toBeExecutedFilter
        /// </summary>
        /// <param name="leftFilter"></param>
        /// <param name="rightFilter"></param>
        /// <param name="booleanOperator"></param>
        /// <returns></returns>
        private bool PureBooleanOperate(Filter leftFilter, Filter rightFilter, BooleanOperator booleanOperator)
        {
            Filter result;
            switch (booleanOperator)
            {
                case BooleanOperator.And:
                    result = m_commandData.Application.Create.Filter.NewLogicAndFilter(leftFilter, rightFilter);
                    break;
                case BooleanOperator.Or:
                    result = m_commandData.Application.Create.Filter.NewLogicOrFilter(leftFilter, rightFilter);
                    break;
                case BooleanOperator.Not:
                    result = m_commandData.Application.Create.Filter.NewLogicNotFilter(rightFilter);
                    break;
                case BooleanOperator.None:
                    result = rightFilter;
                    break;
                default:
                    return false;
            }

            m_toBeExecutedFilter = result;
            return true;
        }

        /// <summary>
        /// Form the expression string, and generate the logic filter.
        /// </summary>
        /// <param name="booleanOperator"></param>
        /// <returns></returns>
        public bool BooleanOperate(BooleanOperator booleanOperator)
        {
            StringBuilder sb = new StringBuilder();
            switch (booleanOperator)
            {
                case BooleanOperator.And:
                    if (null == m_toBeExecutedFilter
                        || m_booleanOperator != BooleanOperator.Add)
                    {
                        return false;
                    }

                    m_booleanOperator = BooleanOperator.And;
                    sb.AppendFormat("({0}) && ", m_booleanExp);
                    m_booleanExp = sb.ToString();
                    m_canBeExecuted = false;
                    return true;

                case BooleanOperator.Or:
                    if (null == m_toBeExecutedFilter
                        || m_booleanOperator != BooleanOperator.Add)
                    {
                        return false;
                    }

                    m_booleanOperator = BooleanOperator.Or;
                    sb.AppendFormat("({0}) || ", m_booleanExp);
                    m_booleanExp = sb.ToString();
                    m_canBeExecuted = false;
                    return true;
                case BooleanOperator.Not:
                    if (null == m_toBeExecutedFilter
                        || m_booleanOperator != BooleanOperator.Add)
                    {
                        return false;
                    }

                    m_booleanOperator = BooleanOperator.Add;//'Add' means right operator is ready
                    sb.AppendFormat("!({0})", m_booleanExp);
                    m_booleanExp = sb.ToString();
                    m_canBeExecuted = true;
                    return PureBooleanOperate(null, m_toBeExecutedFilter, BooleanOperator.Not);
                case BooleanOperator.Add:
                    if (m_booleanOperator == BooleanOperator.Add
                        || null == m_selectedFilterInfo)
                    {
                        return false;
                    }

                    bool result;
                    if (string.IsNullOrEmpty(m_booleanExp))
                    {
                        m_booleanExp = m_selectedFilterInfo.Name;
                        m_toBeExecutedFilter = m_selectedFilterInfo.Filter;
                        m_canBeExecuted = true;
                        result =  true;
                    }
                    else
                    {
                        sb.AppendFormat("{0} {1}", m_booleanExp, m_selectedFilterInfo.Name);
                        m_booleanExp = sb.ToString();
                        m_canBeExecuted = true;
                        result = PureBooleanOperate(m_toBeExecutedFilter, m_selectedFilterInfo.Filter, m_booleanOperator);
                    }

                    // this filter is used more.
                    m_selectedFilterInfo.UsedCount++;
                    m_booleanOperator = BooleanOperator.Add;
                    return result;

                case BooleanOperator.None:
                    m_booleanOperator = BooleanOperator.None;
                    m_toBeExecutedFilter = null;
                    m_booleanExp = string.Empty;
                    m_canBeExecuted = false;
                    // No filter is used.
                    foreach (FilterInfo fi in m_filters)
                    {
                        fi.UsedCount = 0;
                    }
                    break;
                default:
                    sb.AppendFormat("Invalid boolean operation!");
                    break;
            }
            return false;
        }

        /// <summary>
        /// All filters are in this list.
        /// </summary>
        public List<FilterInfo> Filters
        {
            get
            {
                return m_filters;
            }
        }

        /// <summary>
        /// The selected filter information.
        /// </summary>
        public FilterInfo SelectedFilterInfo
        {
            get
            {
                return m_selectedFilterInfo;
            }
            set
            {
                m_selectedFilterInfo = value;
            }
        }

        /// <summary>
        /// The logic filter to be executed.
        /// </summary>
        public Filter ToBeExecutedFilter
        {
            get
            {
                return m_toBeExecutedFilter;
            }
            set
            {
                m_toBeExecutedFilter = value;
            }
        }

        /// <summary>
        /// Get/Set the current operator.
        /// </summary>
        public BooleanOperator Operator
        {
            get
            {
                return m_booleanOperator;
            }
            set
            {
                m_booleanOperator = value;
            }
        }

        /// <summary>
        /// Return true if the current expression can be executed, or else return false.
        /// </summary>
        public bool CanBeExecuted
        {
            get
            {
                return m_canBeExecuted;
            }
        }

        /// <summary>
        /// Get the boolean expression string.
        /// </summary>
        public string BooleanExpression
        {
            get
            {
                return m_booleanExp;
            }
        }

        /// <summary>
        /// Application Unique Identifier.
        /// </summary>
        public string AUID
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                int count = m_filters.Count + 1;
                sb.AppendFormat("filter - {0}", count.ToString());
                
                while (IsUsedName(sb.ToString()))
                {
                    sb.Replace(count.ToString(), (++count).ToString());
                }
                
                return sb.ToString();
            }
        }

        /// <summary>
        /// Is the name has already been used.
        /// </summary>
        /// <param name="name">The name string to be checked.</param>
        /// <returns>Return true if this string is duplicated, or else return false.</returns>
        public bool IsUsedName(string name)
        {
            if (null == m_filters)
                return false;

            foreach (FilterInfo info in m_filters)
            {
                if (name.Equals(info.Name))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Return true if this name string is valid, or else return false.
        /// </summary>
        /// <param name="name">The name string to be checked.</param>
        /// <returns>Return true if this string is valid, or else return false.</returns>
        public bool IsValidName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }
            string invalidChars = "!@#$%^&*()|";
            foreach (char ch in invalidChars.ToCharArray())
            {
                if (name.Contains(ch.ToString()))
                    return false;
            }

            return true;
        }
    }
}
