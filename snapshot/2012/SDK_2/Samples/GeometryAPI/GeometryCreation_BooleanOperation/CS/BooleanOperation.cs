//
// (C) Copyright 2003-2011 by Autodesk, Inc.
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

namespace Revit.SDK.Samples.GeometryCreation_BooleanOperation.CS
{
   static class BooleanOperation
   {
      /// <summary>
      /// Boolean intersect geometric operation, return a new solid as the result
      /// </summary>
      /// <param name="solid1">Operation solid 1</param>
      /// <param name="solid2">Operation solid 2</param>
      /// <returns>The operation result</returns>
      public static Solid BooleanOperation_Intersect(Solid solid1, Solid solid2)
      {
         return BooleanOperationsUtils.ExecuteBooleanOperation(solid1, solid2, BooleanOperationsType.Intersect);
      }

      /// <summary>
      /// Boolean union geometric operation, return a new solid as the result
      /// </summary>
      /// <param name="solid1">Operation solid 1</param>
      /// <param name="solid2">Operation solid 2</param>
      /// <returns>The operation result</returns>
      public static Solid BooleanOperation_Union(Solid solid1, Solid solid2)
      {
         return BooleanOperationsUtils.ExecuteBooleanOperation(solid1, solid2, BooleanOperationsType.Union);
      }

      /// <summary>
      /// Boolean difference geometric operation, return a new solid as the result
      /// </summary>
      /// <param name="solid1">Operation solid 1</param>
      /// <param name="solid2">Operation solid 2</param>
      /// <returns>The operation result</returns>
      public static Solid BooleanOperation_Difference(Solid solid1, Solid solid2)
      {
         return BooleanOperationsUtils.ExecuteBooleanOperation(solid1, solid2, BooleanOperationsType.Difference);
      }

      /// <summary>
      /// Boolean intersect geometric operation, modify the original solid as the result
      /// </summary>
      /// <param name="solid1">Operation solid 1 and operation result</param>
      /// <param name="solid2">Operation solid 2</param>
      public static void BooleanOperation_Intersect(ref Solid solid1, Solid solid2)
      {
         BooleanOperationsUtils.ExecuteBooleanOperationModifyingOriginalSolid(solid1, solid2, BooleanOperationsType.Intersect);
      }

      /// <summary>
      /// Boolean union geometric operation, modify the original solid as the result
      /// </summary>
      /// <param name="solid1">Operation solid 1 and operation result</param>
      /// <param name="solid2">Operation solid 2</param>
      public static void BooleanOperation_Union(ref Solid solid1, Solid solid2)
      {
         BooleanOperationsUtils.ExecuteBooleanOperationModifyingOriginalSolid(solid1, solid2, BooleanOperationsType.Union);
      }

      /// <summary>
      /// Boolean difference geometric operation, modify the original solid as the result
      /// </summary>
      /// <param name="solid1">Operation solid 1 and operation result</param>
      /// <param name="solid2">Operation solid 2</param>
      public static void BooleanOperation_Difference(ref Solid solid1, Solid solid2)
      {
         BooleanOperationsUtils.ExecuteBooleanOperationModifyingOriginalSolid(solid1, solid2, BooleanOperationsType.Difference);
      }
   }
}
