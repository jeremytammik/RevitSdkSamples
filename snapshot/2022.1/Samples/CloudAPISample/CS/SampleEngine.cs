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
using System.Collections.Generic;
using Autodesk.Revit.UI;
using Revit.SDK.Samples.CloudAPISample.CS.Coroutine;
using Revit.SDK.Samples.CloudAPISample.CS.View;

namespace Revit.SDK.Samples.CloudAPISample.CS
{
   internal class SampleEngine : IDisposable
   {
      private readonly UIApplication application;
      private readonly List<SampleContext> allContext = new List<SampleContext>();
      private ViewSamplePortal viewPortal;

      public SampleEngine(UIApplication app)
      {
         application = app;
         viewPortal = new ViewSamplePortal(app);

         CoroutineScheduler.Run();
      }

      /// <inheritdoc />
      public void Dispose()
      {
         foreach (var context in allContext)
         {
            context.Terminate();
            context.Application = null;
         }

         allContext.Clear();
         viewPortal?.Close();
         viewPortal = null;

         CoroutineScheduler.Stop();
      }

      public void RegisterSample(string name, SampleContext sampleContext)
      {
         sampleContext.Application = application;
         allContext.Add(sampleContext);

         viewPortal.AddTab(name, sampleContext.View);
      }

      public void Run()
      {
         viewPortal.ShowDialog();
      }
   }
}