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
   /// A stairs run consisting of a single sketched curved run.
   /// </summary>
   /// <remarks>Because this is sketched, the maximum covered angle for the stair is 360 degrees.</remarks>
   class SketchedCurvedStairsRunComponent : TransformedStairsComponent, IStairsRunComponent
   {

      /// <summary>
      /// Creates a new SketchedCurvedStairsRunConfiguration at the default location and orientation.
      /// </summary>
      /// <param name="riserNumber">The number of risers in the run.</param>
      /// <param name="bottomElevation">The bottom elevation.</param>
      /// <param name="desiredTreadDepth">The desired tread depth.</param>
      /// <param name="width">The width.</param>
      /// <param name="innerRadius">The radius of the innermost edge of the run.</param>
      /// <param name="appCreate">The Revit API application creation object.</param>
      public SketchedCurvedStairsRunComponent(int riserNumber, double bottomElevation,
                                      double desiredTreadDepth, double width,
                                      double innerRadius, Autodesk.Revit.Creation.Application appCreate) :
         base()
      {
         m_riserNumber = riserNumber;
         m_bottomElevation = bottomElevation;
         m_innerRadius = innerRadius;
         m_outerRadius = innerRadius + width;
         m_incrementAngle = desiredTreadDepth / (m_innerRadius + width / 2.0);
         m_includedAngle = m_incrementAngle * (riserNumber - 1);
         if (m_includedAngle > 2 * Math.PI)
            throw new Exception("Arguments provided require an included angle of more than 360 degrees");

         m_center = new XYZ(0, 0, bottomElevation);
         m_appCreate = appCreate;
      }

      /// <summary>
      /// Creates a new SketchedCurvedStairsRunConfiguration at the specified location and orientation.
      /// </summary>
      /// <param name="riserNumber">The number of risers in the run.</param>
      /// <param name="bottomElevation">The bottom elevation.</param>
      /// <param name="desiredTreadDepth">The desired tread depth.</param>
      /// <param name="width">The width.</param>
      /// <param name="innerRadius">The radius of the innermost edge of the run.</param>
      /// <param name="appCreate">The Revit API application creation object.</param>
      /// <param name="transform">The transformation (location and orientation).</param>
      public SketchedCurvedStairsRunComponent(int riserNumber, double bottomElevation,
                                        double desiredTreadDepth, double width,
                                        double innerRadius, Autodesk.Revit.Creation.Application appCreate,
                                        Transform transform) :
         base(transform)
      {
         m_riserNumber = riserNumber;
         m_bottomElevation = bottomElevation;
         m_innerRadius = innerRadius;
         m_outerRadius = innerRadius + width;
         m_incrementAngle = desiredTreadDepth / (m_innerRadius + width / 2.0);
         m_includedAngle = m_incrementAngle * (riserNumber - 1);
         if (m_includedAngle > 2 * Math.PI)
            throw new Exception("Arguments provided require an included angle of more than 360 degrees");

         m_center = new XYZ(0, 0, bottomElevation);
         m_appCreate = appCreate;
      }

      private int m_riserNumber;
      private double m_bottomElevation;
      private double m_innerRadius;
      private double m_outerRadius;
      private double m_incrementAngle;
      private double m_includedAngle;
      private XYZ m_center;
      private Autodesk.Revit.Creation.Application m_appCreate;
      private StairsRun m_stairsRun;

      #region IStairsConfiguration members
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
         List<Curve> ret = new List<Curve>();
         Arc arc = Arc.Create(m_center, (m_innerRadius + m_outerRadius) / 2.0, 0, m_includedAngle, XYZ.BasisX, XYZ.BasisY);
         ret.Add(arc);

         return ret;
      }

      /// <summary>
      /// Implements the interface method.
      /// </summary>
      public Curve GetFirstCurve()
      {
         return GetRunRiserCurves().First<Curve>();
      }

      /// <summary>
      /// Implements the interface method.
      /// </summary>
      public Curve GetLastCurve()
      {
         return GetRunRiserCurves().Last<Curve>();
      }

      /// <summary>
      /// Implements the interface method.
      /// </summary>
      public IList<Autodesk.Revit.DB.Curve> GetRunRiserCurves()
      {
         double incAngle = 0.0;
         XYZ center = XYZ.Zero;
         List<Curve> ret = new List<Curve>();

         // Run riser curves are linear and radial with respect to the center of curvature.
         for (int i = 0; i < m_riserNumber - 1; i++)
         {
            XYZ start = center + new XYZ(m_innerRadius * Math.Cos(incAngle), m_innerRadius * Math.Sin(incAngle), 0);
            XYZ end = center + new XYZ(m_outerRadius * Math.Cos(incAngle), m_outerRadius * Math.Sin(incAngle), 0);
            ret.Add(Line.CreateBound(start, end));

            incAngle += m_incrementAngle;
         }

         // last one handled manually to ensure that it intersects end curves
         IList<Curve> boundaryCurves = GetRunBoundaryCurves();

         ret.Add(Line.CreateBound(boundaryCurves[0].Evaluate(1.0, true), boundaryCurves[1].Evaluate(1.0, true)));

         return ret;
      }

      /// <summary>
      /// Implements the interface method.
      /// </summary>
      public XYZ GetRunEndpoint()
      {
         return GetRunBoundaryCurves()[1].GetEndPoint(1);
      }

      /// <summary>
      /// Implements the interface method.
      /// </summary>
      public IList<Autodesk.Revit.DB.Curve> GetRunBoundaryCurves()
      {
         List<Curve> ret = new List<Curve>();
         Arc arc = Arc.Create(m_center, m_innerRadius, 0, m_includedAngle, XYZ.BasisX, XYZ.BasisY);
         ret.Add(arc);

         arc = Arc.Create(m_center, m_outerRadius, 0, m_includedAngle, XYZ.BasisX, XYZ.BasisY);
         ret.Add(arc);

         return ret;
      }

      /// <summary>
      /// Implements the interface method.
      /// </summary>
      public Autodesk.Revit.DB.Architecture.StairsRun CreateStairsRun(Document document, ElementId stairsId)
      {
         m_stairsRun = StairsRun.CreateSketchedRun(document, stairsId, GetRunElevation(),
                                            Transform(GetRunBoundaryCurves()), Transform(GetRunRiserCurves()),
                                            Transform(GetStairsPath()));
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
            return m_outerRadius - m_innerRadius;
         }
         set
         {
            if (m_stairsRun != null)
            {
               throw new InvalidOperationException("Cannot change width of already existing sketched run.");
            }
            m_outerRadius = value + m_innerRadius;
         }
      }

      #endregion

   }
}
