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
   /// A stairs component consisting of a single curved run.
   /// </summary>
   class CurvedStairsRunComponent : TransformedStairsComponent, IStairsRunComponent
   {
      /// <summary>
      /// Creates a new CurvedStairsRunConfiguration at the default location and orientation.
      /// </summary>
      /// <param name="riserNumber">The number of risers in the run.</param>
      /// <param name="bottomElevation">The bottom elevation.</param>
      /// <param name="desiredTreadDepth">The desired tread depth.</param>
      /// <param name="width">The width.</param>
      /// <param name="innerRadius">The radius of the innermost edge of the run.</param>
      /// <param name="appCreate">The Revit API application creation object.</param>
      public CurvedStairsRunComponent(int riserNumber, double bottomElevation,
                                      double desiredTreadDepth, double width,
                                      double innerRadius, Autodesk.Revit.Creation.Application appCreate) :
         base()
      {
         m_riserNumber = riserNumber;
         m_bottomElevation = bottomElevation;
         m_innerRadius = innerRadius;
         m_outerRadius = innerRadius + width;
         m_incrementAngle = desiredTreadDepth / (m_innerRadius + width / 2.0);
         m_desiredTreadDepth = desiredTreadDepth;
         m_includedAngle = m_incrementAngle * (riserNumber - 1);

         m_center = new XYZ(0, 0, bottomElevation);
         m_appCreate = appCreate;
      }

      /// <summary>
      /// Creates a new CurvedStairsRunConfiguration at the specified location and orientation.
      /// </summary>
      /// <param name="riserNumber">The number of risers in the run.</param>
      /// <param name="bottomElevation">The bottom elevation.</param>
      /// <param name="desiredTreadDepth">The desired tread depth.</param>
      /// <param name="width">The width.</param>
      /// <param name="innerRadius">The radius of the innermost edge of the run.</param>
      /// <param name="appCreate">The Revit API application creation object.</param>
      /// <param name="transform">The transformation (location and orientation).</param>
      public CurvedStairsRunComponent(int riserNumber, double bottomElevation,
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
         m_desiredTreadDepth = desiredTreadDepth;
         m_center = new XYZ(0, 0, bottomElevation);
         m_appCreate = appCreate;
      }

      private int m_riserNumber;
      private double m_bottomElevation;
      private double m_innerRadius;
      private double m_outerRadius;
      private double m_incrementAngle;
      private double m_includedAngle;
      private double m_desiredTreadDepth;
      private XYZ m_center;
      private Autodesk.Revit.Creation.Application m_appCreate;
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
      /// Implements the interface property.
      /// </summary>
      private double Radius
      {
         get
         {
            return (m_innerRadius + m_outerRadius) / 2.0;
         }
      }

      /// <summary>
      /// Implements the interface method.
      /// </summary>
      public Autodesk.Revit.DB.Architecture.StairsRun CreateStairsRun(Document document, ElementId stairsId)
      {
         m_stairsRun = StairsRun.CreateSpiralRun(document, stairsId, TransformPoint(m_center),
                                          Radius, 0, m_includedAngle, true, StairsRunJustification.Center);
         Width = m_outerRadius - m_innerRadius;
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
            return m_outerRadius - m_innerRadius;
         }
         set
         {
            m_outerRadius = value + m_innerRadius;
            if (m_stairsRun != null)
            {
               m_stairsRun.ActualRunWidth = m_outerRadius - m_innerRadius;
               m_incrementAngle = m_desiredTreadDepth / (m_innerRadius + value / 2.0);
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

         // Obtain the footprint boundary
         CurveLoop boundary = m_stairsRun.GetFootprintBoundary();

         // Obtain the endpoint of the stairs path matching the desired end,
         // and find out which curve contains this point.
         CurveLoop path = m_stairsRun.GetStairsPath();
         Curve pathCurve = path.First<Curve>();
         XYZ pathPoint = pathCurve.GetEndPoint(last ? 1 : 0);

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
