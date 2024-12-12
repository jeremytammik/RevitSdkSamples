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
    /// A stairs run consisting of a linear straight run.
    /// </summary>
    public class StraightStairsRunComponent : TransformedStairsComponent, IStairsRunComponent
    {
        /// <summary>
        /// Creates a new straight stairs run in the default location and orientation.
        /// </summary>
        /// <param name="riserNumber">The number of risers.</param>
        /// <param name="bottomElevation">The bottom elevation of the run.</param>
        /// <param name="desiredTreadDepth">The desired tread depth.</param>
        /// <param name="width">The width of the run.</param>
    	public StraightStairsRunComponent(int riserNumber, double bottomElevation, double desiredTreadDepth, double width):
    		base()
        {
            m_riserNumber = riserNumber;
            m_bottomElevation = bottomElevation;
            m_desiredTreadDepth = desiredTreadDepth;
            m_width = width;
            m_runOffset = new XYZ(0, (riserNumber - 1) * desiredTreadDepth, 0);
            m_widthOffset = new XYZ(m_width, 0, 0);
        }

        /// <summary>
        /// Creates a new straight stairs run in the specified location and orientation.
        /// </summary>
        /// <param name="riserNumber">The number of risers.</param>
        /// <param name="bottomElevation">The bottom elevation of the run.</param>
        /// <param name="desiredTreadDepth">The desired tread depth.</param>
        /// <param name="width">The width of the run.</param>
        /// <param name="transform">The transform (orientation and location) for the run.</param>
        public StraightStairsRunComponent(int riserNumber, double bottomElevation, double desiredTreadDepth, double width, 
    	                                      Transform transform):
    		base (transform)
        {
            m_riserNumber = riserNumber;
            m_bottomElevation = bottomElevation;
            m_desiredTreadDepth = desiredTreadDepth;
            m_width = width;
            m_runOffset = new XYZ(0, (riserNumber - 1) * desiredTreadDepth, 0);
            m_widthOffset = new XYZ(m_width, 0, 0);
        }

        private int m_riserNumber;
        private double m_bottomElevation;
        private double m_desiredTreadDepth;
        private double m_width;
        private XYZ m_runOffset;
        private XYZ m_widthOffset;
        private StairsRun m_stairsRun = null;

        #region IStairsRunConfiguration Members

        /// <summary>
        /// Implements the interface method.
        /// </summary>
        public double GetRunElevation()
        {
            return m_bottomElevation;
        }

        /// <summary>
        /// Implements the interface method.
        /// </summary>
        public Line GetRunStairsPath()
        {
            XYZ start = new XYZ(m_width / 2.0, 0, m_bottomElevation);
            XYZ end = start + m_runOffset;
            Line curve1 = Line.CreateBound(start, end);

  			return curve1;
        }

        /// <summary>
        /// Implements the interface method.
        /// </summary>
        public double RunElevation
        {
        	get
        	{
        		return m_bottomElevation;
        	}
        }

        /// <summary>
        /// Implements the interface method.
        /// </summary>
        public double TopElevation
        {
        	get
        	{
        		if (m_stairsRun == null)
        		{
        			throw new NotSupportedException("Stairs run hasn't been constructed yet.");
        		}	
        		return m_stairsRun.TopElevation;
        	}
        }

        /// <summary>
        /// Implements the interface method.
        /// </summary>
        public IList<Curve> GetStairsPath()
        {
        	if (m_stairsRun == null)
        	{
        		throw new NotSupportedException("Stairs run hasn't been constructed yet.");
        	}
        	CurveLoop curveLoop = m_stairsRun.GetStairsPath();
        	return curveLoop.ToList();
        }

        /// <summary>
        /// Implements the interface method.
        /// </summary>
    	public Curve GetFirstCurve()
        {
        	return GetEndCurve(false);
        }

        /// <summary>
        /// Implements the interface method.
        /// </summary>
        public Curve GetLastCurve()
        {
        	return GetEndCurve(true);
        }

        /// <summary>
        /// Implements the interface method.
        /// </summary>
        public XYZ GetRunEndpoint()
        {
        	return GetLastCurve().GetEndPoint(0);
        }

        /// <summary>
        /// Implements the interface method.
        /// </summary>
		public Autodesk.Revit.DB.Architecture.StairsRun CreateStairsRun(Document document, ElementId stairsId)
		{
			m_stairsRun = StairsRun.CreateStraightRun(document, stairsId, 
			                                   Transform(GetRunStairsPath()), StairsRunJustification.Center);
			Width = m_width;
			document.Regenerate(); // to get updated width
			return m_stairsRun;
		}

        /// <summary>
        /// Implements the interface property.
        /// </summary>
		public double Width
		{
			get
			{
				return m_width;
			}
			set
			{
				m_width = value;
				if (m_stairsRun != null)
				{
					m_stairsRun.ActualRunWidth = m_width;
				}
			}
        }

        #endregion

        /// <summary>
        /// Gets the first or last riser curve of the run.
        /// </summary>
        /// <param name="last">True to get the last curve, false to get the first.</param>
        /// <returns>The curve.</returns>
        private Curve GetEndCurve(bool last)
        {
            if (m_stairsRun == null)
            {
                throw new NotSupportedException("Stairs run hasn't been constructed yet.");
            }

            // Obtain the footprint boundary of the run.
            CurveLoop boundary = m_stairsRun.GetFootprintBoundary();

            // Find the first or last point on the path
            CurveLoop path = m_stairsRun.GetStairsPath();
            Curve pathCurve = path.First<Curve>();
            XYZ pathPoint = pathCurve.GetEndPoint(last ? 1 : 0);

            // Walk the footprint boundary, and look for a curve whose projection of the target point is equal to the point.
            foreach (Curve boundaryCurve in boundary)
            {
                if (boundaryCurve.Project(pathPoint).XYZPoint.IsAlmostEqualTo(pathPoint))
                {
                    return boundaryCurve;
                }
            }

            throw new Exception("Unable to find an intersecting boundary curve in the run.");
        }
    }
}
