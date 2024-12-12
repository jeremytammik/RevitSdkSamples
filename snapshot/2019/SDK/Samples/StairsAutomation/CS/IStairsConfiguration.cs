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

namespace Revit.SDK.Samples.StairsAutomation.CS
{
    /// <summary>
    /// This interface represents a configuration of stairs runs and landings to be created.
    /// </summary>
    public interface IStairsConfiguration
    {
        /// <summary>
        /// Returns the number of stairs runs in the stairs assembly.
        /// </summary>
        /// <returns>The number of runs.</returns>
        int GetNumberOfRuns();

        /// <summary>
        /// Creates the stairs run with the given index.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="stairsElementId">The id of the stairs element.</param>
        /// <param name="runIndex">The run index.</param>
        void CreateStairsRun(Document document, ElementId stairsElementId, int runIndex);
        
        /// <summary>
        /// Returns the number of landings in the stairs assembly.
        /// </summary>
        /// <returns>The number of landings.</returns>
        int GetNumberOfLandings();

        /// <summary>
        /// Creates the stairs landing with the given index.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="stairsElementId">The id of the stairs element.</param>
        /// <param name="landingIndex">The landing index.</param>
        void CreateLanding(Document document, ElementId stairsElementId, int landingIndex);
        
        /// <summary>
        /// Assigns the width of the stairs runs.
        /// </summary>
        /// <param name="width">The width.</param>
        void SetRunWidth(double width);
    }
}
