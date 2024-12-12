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

namespace Revit.SDK.Samples.StairsAutomation.CS
{
	/// <summary>
	/// A specific implementation of IStairsConfiguration with some default storage included.
	/// </summary>
	public class StairsConfiguration : IStairsConfiguration
	{
	    /// <summary>
	    /// The run configurations.
	    /// </summary>
		protected List<IStairsRunComponent> m_runConfigurations = new List<IStairsRunComponent>();

        /// <summary>
        /// The landing configurations.
        /// </summary>
		protected List<IStairsLandingComponent> m_landingConfigurations = new List<IStairsLandingComponent>();

        #region IStairsConfiguration members
        /// <summary>
        /// Implements the interface method.
        /// </summary>
        public int GetNumberOfRuns()
        {
            return m_runConfigurations.Count;
        }

        /// <summary>
        /// Implements the interface method.
        /// </summary>
		public void CreateStairsRun(Autodesk.Revit.DB.Document document, Autodesk.Revit.DB.ElementId stairsElementId, int runIndex)
		{
			m_runConfigurations[runIndex].CreateStairsRun(document, stairsElementId);
		}

        /// <summary>
        /// Implements the interface method.
        /// </summary>
		public int GetNumberOfLandings()
        {
            return m_landingConfigurations.Count;
        }

        /// <summary>
        /// Implements the interface method.
        /// </summary>
		public void CreateLanding(Autodesk.Revit.DB.Document document, Autodesk.Revit.DB.ElementId stairsElementId, int landingIndex)
		{
			m_landingConfigurations[landingIndex].CreateLanding(document, stairsElementId);
		}

        /// <summary>
        /// Implements the interface method.
        /// </summary>
		public void SetRunWidth(double width)
		{
			foreach (var config in m_runConfigurations)
			{
				config.Width = width;
			}
        }
        #endregion
    }
}
