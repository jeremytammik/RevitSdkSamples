//
// (C) Copyright 2003-2010 by Autodesk, Inc.
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
using System.Linq;
using System.Text;

using Autodesk.Revit;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;

namespace Revit.SDK.Samples.Selections.CS
{
    /// <summary>
    /// A default filter.
    /// All objects are allowed to be picked.
    /// </summary>
    public class DefaultElementsFilter : ISelectionFilter
    {
        /// <summary>
        /// Allow all the element to be selected
        /// </summary>
        /// <param name="element">A candidate element in selection operation.</param>
        /// <returns>Return true to allow the user to select this candidate element.</returns>
        public bool AllowElement(Element element)
        {
            return true;
        }

        /// <summary>
        /// Allow all the reference to be selected
        /// </summary>
        /// <param name="refer">A candidate reference in selection operation.</param>
        /// <param name="point">The 3D position of the mouse on the candidate reference.</param>
        /// <returns>Return true to allow the user to select this candidate reference.</returns>
        public bool AllowReference(Reference refer, XYZ point)
        {
            return true;
        }
    }

    /// <summary>
    /// A Filter for Wall Face.
    /// Only wall faces are allowed to be picked.
    /// </summary>
    public class WallFaceFilter : ISelectionFilter
    {
        /// <summary>
        /// Allow wall to be selected
        /// </summary>
        /// <param name="element">A candidate element in selection operation.</param>
        /// <returns>Return true for wall. Return false for non wall element.</returns>
        public bool AllowElement(Element element)
        {
            return element is Wall;
        }

        /// <summary>
        /// Allow face reference to be selected
        /// </summary>
        /// <param name="refer">A candidate reference in selection operation.</param>
        /// <param name="point">The 3D position of the mouse on the candidate reference.</param>
        /// <returns>Return true for face reference. Return false for non face reference.</returns>
        public bool AllowReference(Reference refer, XYZ point)
        {
            return refer.GeometryObject != null && refer.GeometryObject is Face;
        }
    }

    /// <summary>
    /// A Filter for planar face.
    /// Only planar faces are allowed to be picked.
    /// </summary>
    public class PlanarFaceFilter : ISelectionFilter
    {
        /// <summary>
        /// Allow all the element to be selected
        /// </summary>
        /// <param name="element">A candidate element in selection operation.</param>
        /// <returns>Return true to allow the user to select this candidate element.</returns>
        public bool AllowElement(Element element)
        {
            return true;
        }

        /// <summary>
        /// Allow planar face reference to be selected
        /// </summary>
        /// <param name="refer">A candidate reference in selection operation.</param>
        /// <param name="point">The 3D position of the mouse on the candidate reference.</param>
        /// <returns>Return true for planar face reference. Return false for non planar face reference.</returns>
        public bool AllowReference(Reference refer, XYZ point)
        {
            return refer.GeometryObject != null && refer.GeometryObject is PlanarFace;
        }
    }
}
