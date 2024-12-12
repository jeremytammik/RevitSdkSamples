//
// (C) Copyright 2003-2017 by Autodesk, Inc.
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
using Autodesk.Revit.DB.Architecture;

namespace Revit.SDK.Samples.StairsAutomation.CS
{
    /// <summary>
    /// An interface that represents a landing.
    /// </summary>
    public interface IStairsLandingComponent
    {
        /// <summary>
        /// Obtains the curves that bound the landing.
        /// </summary>
        /// <returns>The boundary curves.</returns>
        CurveLoop GetLandingBoundary();

        /// <summary>
        /// Obtains the elevation of the landing.
        /// </summary>
        /// <returns>The elevation.</returns>
        double GetLandingBaseElevation();

        /// <summary>
        /// Creates the landing component represented by this configuration.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="stairsElementId">The stairs element id.</param>
        /// <returns>The new landing.</returns>
        StairsLanding CreateLanding(Document document, ElementId stairsElementId);
    }
}
