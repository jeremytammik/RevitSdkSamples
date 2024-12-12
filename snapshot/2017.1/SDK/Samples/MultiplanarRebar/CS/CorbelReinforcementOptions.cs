//
// (C) Copyright 2003-2016 by Autodesk, Inc.
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
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;

namespace Revit.SDK.Samples.MultiplanarRebar.CS
{
    /// <summary>
    /// Represent the reinforcement options of corbel.
    /// The options include bar type and bar counts which are collected from user via UI input.
    /// </summary>
    class CorbelReinforcementOptions
    {
        /// <summary>
        /// Active Revit DB Document.
        /// </summary>
        public Document RevitDoc { get; set; }

        /// <summary>
        /// List of RebarBarTypes in active document.
        /// </summary>
        public List<RebarBarType> RebarBarTypes { get; set; }

        /// <summary>
        /// RebarBarType for corbel top straight bars.
        /// </summary>
        public RebarBarType TopBarType { get; set; }

        /// <summary>
        /// RebarBarType for corbel stirrup bars.
        /// </summary>
        public RebarBarType StirrupBarType { get; set; }

        /// <summary>
        /// RebarBarType for corbel multi-planar bar.
        /// </summary>
        public RebarBarType MultiplanarBarType { get; set; }

        /// <summary>
        /// RebarBarType for corbel host straight bars.
        /// </summary>
        public RebarBarType HostStraightBarType { get; set; }

        /// <summary>
        /// Count of corbel straight bars.
        /// </summary>
        public int TopBarCount { get; set; }

        /// <summary>
        /// Count of corbel stirrup bars.
        /// </summary>
        public int StirrupBarCount { get; set; }

        /// <summary>
        /// Constructor to initialize the fields.
        /// </summary>
        /// <param name="revitDoc">Revit DB Document</param>
        public CorbelReinforcementOptions(Document revitDoc)
        {
            RevitDoc = revitDoc;
            FilteredElementCollector filteredElementCollector = new FilteredElementCollector(RevitDoc);
            filteredElementCollector.OfClass(typeof(RebarBarType));
            RebarBarTypes = filteredElementCollector.Cast<RebarBarType>().ToList<RebarBarType>();
        }
    }
}
