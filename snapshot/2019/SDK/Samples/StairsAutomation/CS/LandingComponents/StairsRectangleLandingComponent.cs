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
    /// A configuration for creation of a landing with a fixed cross section.
    /// </summary>
    public class StairsRectangleLandingComponent : IStairsLandingComponent
    {
        /// <summary>
        /// Creates a new StairsRectangleLandingConfiguration.
        /// </summary>
        /// <param name="stairsRunBoundary1">The end curve of the lower stair run.</param>
        /// <param name="stairsRunBoundary2">The start curve of the higher stair run.</param>
        /// <param name="runDirection">A vector representing the direction of the lower stair run.</param>
        /// <param name="elevation">The elevation of the landing.</param>
        /// <param name="width">The width of the landing.</param>
        public StairsRectangleLandingComponent(Line stairsRunBoundary1, Line stairsRunBoundary2, XYZ runDirection, double elevation, double width)
        {
            // offset to base elevation
            m_stairsRunBoundary1 = LandingComponentUtils.ProjectCurveToElevation(stairsRunBoundary1, elevation) as Line;
            m_stairsRunBoundary2 = LandingComponentUtils.ProjectCurveToElevation(stairsRunBoundary2, elevation) as Line;

            m_runDirection = runDirection;
            m_width = width;
            m_elevation = elevation;
        }

        private Line m_stairsRunBoundary1;
        private Line m_stairsRunBoundary2;
        private XYZ m_runDirection;
        private double m_width;
        private double m_elevation;

        #region IStairsLandingConfiguration Members

        /// <summary>
        /// Implements the interface method.
        /// </summary>
        public Autodesk.Revit.DB.CurveLoop GetLandingBoundary()
        {
            // TODO : What if not collinear 
            Line boundaryLine = LandingComponentUtils.FindLongestEndpointConnection(m_stairsRunBoundary1, m_stairsRunBoundary2);
    
            // offset by 1/2 of width
            Curve centerCurve = boundaryLine.CreateTransformed(Transform.CreateTranslation(m_runDirection * (m_width / 2.0)));

            // Create by Thicken
            CurveLoop curveLoop = CurveLoop.CreateViaThicken(centerCurve, m_width, XYZ.BasisZ);

            return curveLoop;
        }

        /// <summary>
        /// Implements the interface method.
        /// </summary>
        public double GetLandingBaseElevation()
        {
            return m_elevation;
        }

        /// <summary>
        /// Implements the interface method.
        /// </summary>
		public StairsLanding CreateLanding(Document document, ElementId stairsElementId)
		{
			return StairsLanding.CreateSketchedLanding(document, stairsElementId, GetLandingBoundary(), GetLandingBaseElevation());
        }

        #endregion
    }
}
