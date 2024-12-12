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
using System.Collections;
using System.Collections.Generic;
using System.Text;

using Autodesk.Revit;
using Autodesk.Revit.Symbols;
using Autodesk.Revit.Parameters;
using Autodesk.Revit.Elements;

namespace Revit.SDK.Samples.CurtainWallGrid.CS
{
    /// <summary>
    /// compare 2 wall types and sort them by their type names
    /// </summary>
    class WallTypeComparer : IComparer<WallType>
    {
        #region IComparer<WallType> Members

        /// <summary>
        /// compare 2 walltypes by their names
        /// </summary>
        /// <param name="x">
        /// wall type to be compared
        /// </param>
        /// <param name="y">
        /// wall type to be compared
        /// </param>
        /// <returns>
        /// returns the result by comparing their names with CaseInsensitiveComparer
        /// </returns>
        public int Compare(WallType x, WallType y)
        {
            string xName = x.Name;
            string yName = y.Name;

            IComparer comp = new CaseInsensitiveComparer();
            return comp.Compare(xName, yName);
        }

        #endregion
    }

    /// <summary>
    /// compare 2 views and sort them by their type names
    /// </summary>
    class ViewComparer : IComparer<Autodesk.Revit.Elements.View>
    {
        #region IComparer<View> Members

        /// <summary>
        /// compare 2 views by their names
        /// </summary>
        /// <param name="x">
        /// view to be compared
        /// </param>
        /// <param name="y">
        /// view to be compared
        /// </param>
        /// <returns>
        /// returns the result by comparing their names with CaseInsensitiveComparer
        /// </returns>
        public int Compare(Autodesk.Revit.Elements.View x, Autodesk.Revit.Elements.View y)
        {
            string xName = x.ViewType.ToString() + " : " + x.Name;
            string yName = y.ViewType.ToString() + " : " + y.Name;

            IComparer comp = new CaseInsensitiveComparer();
            return comp.Compare(xName, yName);
        }

        #endregion
    }
}
