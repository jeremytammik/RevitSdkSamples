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
    /// A stairs run consisting of a single sketched straight run.
    /// </summary>
    public class SketchedStraightStairsRunComponent : TransformedStairsComponent, IStairsRunComponent
    {
        /// <summary>
        /// Creates a new sketched run configuration at the default location and orientation.
        /// </summary>
        /// <remarks>Stairs run boundary curves can be calculated before the run is created.</remarks>
        /// <param name="riserNumber">The number of risers.</param>
        /// <param name="bottomElevation">The bottom elevation.</param>
        /// <param name="desiredTreadDepth">The desired tread depth.</param>
        /// <param name="width">The width of the run.</param>
    	public SketchedStraightStairsRunComponent(int riserNumber, double bottomElevation, double desiredTreadDepth, double width):
    		base()
        {
            m_riserNumber = riserNumber;
            m_bottomElevation = bottomElevation;
            m_desiredTreadDepth = desiredTreadDepth;
            m_width = width;
            m_runExtent = new XYZ(0, (riserNumber - 1) * desiredTreadDepth, 0);
            m_widthOffset = new XYZ(m_width, 0, 0);
        }

        /// <summary>
        /// Creates a new sketched run configuration at the specified location and orientation.
        /// </summary>
        /// <param name="riserNumber">The number of risers.</param>
        /// <param name="bottomElevation">The bottom elevation.</param>
        /// <param name="desiredTreadDepth">The desired tread depth.</param>
        /// <param name="width">The width of the run.</param>
        /// <param name="transform">The transform (location and orientation).</param>
    	public SketchedStraightStairsRunComponent(int riserNumber, double bottomElevation, double desiredTreadDepth, double width, Transform transform):
    		base(transform)
        {
            m_riserNumber = riserNumber;
            m_bottomElevation = bottomElevation;
            m_desiredTreadDepth = desiredTreadDepth;
            m_width = width;
            // This is the full extent of the run across all treads
            m_runExtent = new XYZ(0, (riserNumber - 1) * desiredTreadDepth, 0);
            m_widthOffset = new XYZ(m_width, 0, 0);
        }


        private int m_riserNumber;
        private double m_bottomElevation;
        private double m_desiredTreadDepth;
        private double m_width;
        private XYZ m_runExtent;
        private XYZ m_widthOffset;
        private StairsRun m_stairsRun;

        #region IStairsRunConfiguration members

        /// <summary>
        /// Implements the interface property.
        /// </summary>
        public double RunElevation
        {
        	get
        	{
        		return m_bottomElevation;
        	}
        }

        /// <summary>
        /// Implements the interface property.
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
        public double GetRunElevation()
        {
            return m_bottomElevation;
        }

        /// <summary>
        /// Implements the interface method.
        /// </summary>
        public IList<Autodesk.Revit.DB.Curve> GetStairsPath()
        {
            // Proceed up the middle of the run to the run extent
            XYZ start = new XYZ(m_width / 2.0, 0, m_bottomElevation);
            XYZ end = start + m_runExtent;
            Line curve1 = Line.CreateBound(start, end);

            List<Curve> ret = new List<Curve>();
            ret.Add(curve1);
            return Transform(ret);
        }

        /// <summary>
        /// Implements the interface method.
        /// </summary>
        public Curve GetFirstCurve()
        {
        	return GenerateRunRiserCurves().First<Curve>();
        }

        /// <summary>
        /// Implements the interface method.
        /// </summary>
        public Curve GetLastCurve()
        {
        	return GenerateRunRiserCurves().Last<Curve>();
        }

        /// <summary>
        /// Implements the interface method.
        /// </summary>
        public XYZ GetRunEndpoint()
        {
        	return GetLastCurve().GetEndPoint(1);
        }

        /// <summary>
        /// Implements the interface method.
        /// </summary>
        public IList<Autodesk.Revit.DB.Curve> GetRunBoundaryCurves()
        {
        	return Transform(GenerateUntransformedRunBoundaryCurves());
        }

        /// <summary>
        /// Implements the interface method.
        /// </summary>
		public Autodesk.Revit.DB.Architecture.StairsRun CreateStairsRun(Document document, ElementId stairsId)
		{
			m_stairsRun = StairsRun.CreateSketchedRun(document, stairsId, GetRunElevation(), 
			                                   GetRunBoundaryCurves(), GenerateRunRiserCurves(), 
			                                   GetStairsPath());
			document.Regenerate(); 
			
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
				if (m_stairsRun != null)
				{
					throw new InvalidOperationException("Cannot change width of sketched run.");
				}
				m_width =  value;
			}
		}
        #endregion

        /// <summary>
        /// Generates the run boundary curves (not transformed by the stored transformation).
        /// </summary>
        /// <returns></returns>
        private IList<Autodesk.Revit.DB.Curve> GenerateUntransformedRunBoundaryCurves()
        {
            // Start at 0, 0 and extend to the run extent
            XYZ start = new XYZ(0, 0, m_bottomElevation);
            XYZ end = start + m_runExtent;
            Line curve1 = Line.CreateBound(start, end);

            List<Curve> ret = new List<Curve>();
            ret.Add(curve1);

            // Start offset along the width and extend to the run extent
            start = new XYZ(m_width, 0, m_bottomElevation);
            end = start + m_runExtent;
            Line curve2 = Line.CreateBound(start, end);
            ret.Add(curve2);

            return ret;
        }

        /// <summary>
        /// Generates the riser curves for the sketch.
        /// </summary>
        /// <returns></returns>
        private IList<Autodesk.Revit.DB.Curve> GenerateRunRiserCurves()
        {
            List<Curve> ret = new List<Curve>();

            // Generate all curves but the last by incrementing along the tread depth
            for (int i = 0; i < m_riserNumber - 1; i++)
            {
                XYZ start = new XYZ(0, i * m_desiredTreadDepth, m_bottomElevation);
                XYZ end = start + m_widthOffset;

                ret.Add(Line.CreateBound(start, end));
            }

            // last one handled manually to ensure that it intersects the endpoints of the stairs curves
            // Untransformed curves are used because transformation happens below
            IList<Curve> boundaryCurves = GenerateUntransformedRunBoundaryCurves();

            ret.Add(Line.CreateBound(boundaryCurves[0].Evaluate(1.0, true), boundaryCurves[1].Evaluate(1.0, true)));

            return Transform(ret);
        }
    }
}
