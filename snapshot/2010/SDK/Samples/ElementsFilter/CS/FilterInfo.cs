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
    /// Represent a filter's information
    /// </summary>
    public class FilterInfo
    {
        private string m_name;
        private string m_tipMessage;
        private Filter m_filter;
        private int m_usedCount;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">What's the filter name</param>
        public FilterInfo(string name)
        {
            m_name = name;
            m_usedCount = 0;
        }

        /// <summary>
        /// Get/Set the name
        /// </summary>
        public string Name
        {
            get
            {
                return m_name;
            }
            set
            {
                m_name = value;
            }
        }

        /// <summary>
        /// Get/Set the Autodesk.Revit.Filter object.
        /// </summary>
        public Filter Filter
        {
            get
            {
                return m_filter;
            }
            set
            {
                m_filter = value;
            }
        }

        /// <summary>
        /// Get/Set the tip message of this object.
        /// </summary>
        public string TipMessage
        {
            get
            {
                return m_tipMessage;
            }
            set
            {
                m_tipMessage = value;
            }
        }

        /// <summary>
        /// Get/Set this filter usage count in the boolean expression
        /// </summary>
        public int UsedCount
        {
            get
            {
                return m_usedCount;
            }
            set
            {
                m_usedCount = value;
            }
        }
    }
}
