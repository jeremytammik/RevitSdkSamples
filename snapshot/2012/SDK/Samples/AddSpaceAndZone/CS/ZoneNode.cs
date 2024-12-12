//
// (C) Copyright 2003-2011 by Autodesk, Inc.
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
using System.Windows.Forms;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;

namespace Revit.SDK.Samples.AddSpaceAndZone.CS
{
    /// <summary>
    /// The ZoneNode class inherit TreeNode Class, it is used
    /// to display the Zones is a TreeView, each ZoneNode contains
    /// a Zone element.
    /// </summary>
    public class ZoneNode : TreeNode
    {
        Zone m_zone;

        /// <summary>
        /// The constructor of ZoneNode class
        /// </summary>
        /// <param name="zone"></param>
        public ZoneNode(Zone zone)
            : base(zone.Name)
        {
            m_zone = zone;
            base.Text = m_zone.Name;
            base.ToolTipText = "Phase: " + m_zone.Phase.Name;
        }

        /// <summary>
        /// Get the Zone element in the ZoneNode.
        /// </summary>
        public Zone Zone
        {
            get
            {
                return m_zone;
            }
        }
    }
}
