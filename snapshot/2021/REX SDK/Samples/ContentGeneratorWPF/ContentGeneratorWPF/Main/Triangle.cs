//
// (C) Copyright 2007-2011 by Autodesk, Inc. All rights reserved.
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM 'AS IS' AND WITH ALL ITS FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE. AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using REX.Common.Geometry;

namespace REX.ContentGeneratorWPF.Main
{
    /// <summary>
    /// Represents the geometry of triangle.
    /// </summary>
    public class Triangle
    {
        /// <summary>
        /// Gets or sets the first point.
        /// </summary>
        /// <value>The point.</value>
        public REXxyz Pt1 { get; set; }
        /// <summary>
        /// Gets or sets the second point.
        /// </summary>
        /// <value>The point.</value>
        public REXxyz Pt2 { get; set; }
        /// <summary>
        /// Gets or sets the third point.
        /// </summary>
        /// <value>The point.</value>
        public REXxyz Pt3 { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="Triangle"/> class.
        /// </summary>
        public Triangle()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Triangle"/> class.
        /// </summary>
        /// <param name="pt1">The first point.</param>
        /// <param name="pt2">The second point.</param>
        /// <param name="pt3">The third point.</param>
        public Triangle(REXxyz pt1, REXxyz pt2, REXxyz pt3)
        {
            Pt1 = pt1;
            Pt2 = pt2;
            Pt3 = pt3;
        }
    }
}
