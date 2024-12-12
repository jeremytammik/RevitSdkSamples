//
// (C) Copyright 2003-2023 by Autodesk, Inc.
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
    /// The SpaceNode class inherit TreeNode Class, it is used
    /// to display the Spaces is a TreeView, each SpaceNode contains
    /// a Space element.
    /// </summary>
    class SpaceNode : TreeNode
    {
        Space m_space;

        /// <summary>
        /// The constructor of SpaceNode class
        /// </summary>
        /// <param name="space"></param>
        public SpaceNode(Space space)
            : base(space.Name)
        {
            m_space = space;
            base.Text = space.Name;
        }

        /// <summary>
        /// Get the Zone element in the ZoneNode.
        /// </summary>
        public Space Space
        {
            get
            {
                return m_space;
            }
        }
    }
}
