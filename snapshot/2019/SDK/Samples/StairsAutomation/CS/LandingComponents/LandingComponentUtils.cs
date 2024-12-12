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
using Autodesk.Revit.DB;

namespace Revit.SDK.Samples.StairsAutomation.CS
{
   /// <summary>
   /// Utilities used in processing of landings.
   /// </summary>
   public class LandingComponentUtils
   {
      /// <summary>
      /// Returns a new set of curves created as a projection of the input curves to the provided elevation.
      /// </summary>
      /// <remarks>Assumes, but does not validate, that the curves are in the XY plane.</remarks>
      /// <param name="curves">The curves.</param>
      /// <param name="elevation">The target elevation.</param>
      /// <returns>The projected curves.</returns>
      public static IList<Curve> ProjectCurvesToElevation(IList<Curve> curves, double elevation)
      {
         List<Curve> ret = new List<Curve>();
         foreach (Curve curve in curves)
         {
            ret.Add(ProjectCurveToElevation(curve, elevation));
         }
         return ret;
      }

      /// <summary>
      /// Returns a new curve created as a projection of the input curve to the provided elevation.
      /// </summary>
      /// <remarks>Assumes, but does not validate, that the curve is in the XY plane.</remarks>
      /// <param name="curve">The curve.</param>
      /// <param name="elevation">The target elevation.</param>
      /// <returns>The projected curve.</returns>
      public static Curve ProjectCurveToElevation(Curve curve, double elevation)
      {
         double offset1 = elevation - curve.GetEndPoint(0).Z;
         Curve projectedCurve = curve.CreateTransformed(Transform.CreateTranslation(new XYZ(0, 0, offset1)));
         return projectedCurve;
      }

      /// <summary>
      /// Given two linear inputs, finds the combination of endpoints from each line which are farthest from each other, and returns a line
      /// connecting those points.
      /// </summary>
      /// <param name="line1">The first line.</param>
      /// <param name="line2">The second line.</param>
      /// <returns>The longest line connecting one endpoint from each line. </returns>
      public static Line FindLongestEndpointConnection(Line line1, Line line2)
      {
         double maxDistance = 0.0;
         XYZ endPoint1 = null;
         XYZ endPoint2 = null;

         for (int i = 0; i < 2; i++)
         {
            for (int j = 0; j < 2; j++)
            {
               double distance = line1.GetEndPoint(j).DistanceTo(line2.GetEndPoint(i));
               if (distance > maxDistance)
               {
                  maxDistance = distance;
                  endPoint1 = line1.GetEndPoint(j);
                  endPoint2 = line2.GetEndPoint(i);
               }
            }
         }

         return Line.CreateBound(endPoint1, endPoint2);
      }

      /// <summary>
      /// Computes the parameter of the point projected onto the curve.
      /// </summary>
      /// <param name="curve">The curve.</param>
      /// <param name="point">The point.</param>
      /// <returns>The parameter of the projected point.</returns>
      public static double ComputeParameter(Curve curve, XYZ point)
      {
         IntersectionResult result = curve.Project(point);
         return result.Parameter;
      }
   }
}
