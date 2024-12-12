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
   /// Description of GeometryUtils.
   /// </summary>
   public static class GeometryUtils
   {
      /// <summary>
      /// Creates a duplicate of a set of curves, applying the specified transform to each.
      /// </summary>
      /// <param name="inputs">The inputs.</param>
      /// <param name="trf">The transformation.</param>
      /// <returns>The copy of the curves.</returns>
      public static IList<Curve> TransformCurves(IList<Curve> inputs, Transform trf)
      {
         int numCurves = inputs.Count;
         List<Curve> result = new List<Curve>(numCurves);

         for (int i = 0; i < numCurves; i++)
         {
            result.Add(TransformCurve(inputs[i], trf));
         }
         return result;
      }

      /// <summary>
      /// Creates a duplicate curve, applying the specified transform to each.
      /// </summary>
      /// <param name="input">The input.</param>
      /// <param name="trf">The transformation.</param>
      /// <returns>The copy of the curve.</returns>
      public static Curve TransformCurve(Curve input, Transform trf)
      {
         Curve trfCurve = input.CreateTransformed(trf);
         return trfCurve;
      }
   }
}
