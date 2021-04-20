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

using System;
using System.Collections;

namespace Revit.SDK.Samples.CloudAPISample.CS.Coroutine
{
   /// <summary>
   ///    A simple coroutine scheduler.
   ///    This coroutine scheduler allows for control over the execution regime
   ///    of a set of coroutines.
   /// </summary>
   public class CoroutineScheduler
   {
      private static CoroutineScheduler instance;

      private Coroutine coroutines;

      private System.Windows.Threading.DispatcherTimer dispatcherTimer;

      private CoroutineScheduler()
      {
      }

      private void Attach()
      {
         dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
         dispatcherTimer.Tick += new EventHandler(Update);
         dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
         dispatcherTimer.Start();
      }

      private void Detach()
      {
         dispatcherTimer?.Stop();
         dispatcherTimer = null;
      }

      private void AddCoroutine(Coroutine coroutine)
      {
         if (coroutines != null)
         {
            coroutine.Next = coroutines;
            coroutines.Previous = coroutine;
         }

         coroutines = coroutine;
      }

      private void RemoveCoroutine(Coroutine coroutine)
      {
         if (coroutines == coroutine)
         {
            coroutines = coroutines.Next;
         }
         else
         {
            if (coroutine.Next != null)
            {
               coroutine.Previous.Next = coroutine.Next;
               coroutine.Next.Previous = coroutine.Previous;
            }
            else if (coroutine.Previous != null)
            {
               coroutine.Previous.Next = null;
            }
         }

         coroutine.Previous = null;
         coroutine.Next = null;
      }

      private void Update(object sender, EventArgs eventArgs)
      {
         UpdateAllCoroutines();
      }

      private void UpdateAllCoroutines()
      {
         var iter = coroutines;

         while (iter != null)
         {
            if (!iter.ExecuteOnStep())
            {
               iter.IsFinished = true;
               RemoveCoroutine(iter);
            }

            iter = iter.Next;
         }
      }

      /// <summary>
      ///    Attach a scheduler to render loop.
      ///    Must be called before using coroutine.
      /// </summary>
      public static void Run()
      {
         if (instance != null)
            return;
         instance = new CoroutineScheduler();
         instance.Attach();
      }

      /// <summary>
      ///    Stop a scheduler. All coroutines will be released.
      /// </summary>
      public static void Stop()
      {
         instance?.Detach();
         instance = null;
      }

      /// <summary>
      ///    Start a new coroutine with an enumerator
      /// </summary>
      /// <returns></returns>
      public static Coroutine StartCoroutine(IEnumerator enumerator)
      {
         if (enumerator == null || instance == null) return null;

         var coroutine = new Coroutine(enumerator);
         instance.AddCoroutine(coroutine);
         return coroutine;
      }
   }
}