//
// (C) Copyright 2003-2020 by Autodesk, Inc. All rights reserved.
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.

//
// AUTODESK PROVIDES THIS PROGRAM 'AS IS' AND WITH ALL ITS FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE. AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable. 

using System.Collections;

namespace Revit.SDK.Samples.CloudAPISample.CS.Coroutine
{
   /// <summary>
   ///    Represent a coroutine instance.
   /// </summary>
   public class Coroutine
   {
      private readonly IEnumerator innerEnumerator;

      /// <summary>
      ///    Create a coroutine with a enumerator
      /// </summary>
      /// <param name="coroutine"></param>
      public Coroutine(IEnumerator coroutine)
      {
         innerEnumerator = coroutine;
      }

      /// <summary>
      ///    Indicates if this coroutine is finished.
      /// </summary>
      public bool IsFinished { get; set; }

      /// <summary>
      ///    The previous coroutine in this double linked list
      /// </summary>
      public Coroutine Previous { get; set; }

      /// <summary>
      ///    The next coroutine in this double linked list
      /// </summary>
      public Coroutine Next { get; set; }

      /// <summary>
      ///    Execute one step of the enumerator
      /// </summary>
      /// <returns></returns>
      public bool ExecuteOnStep()
      {
         return innerEnumerator.MoveNext();
      }
   }
}