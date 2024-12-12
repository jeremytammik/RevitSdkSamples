//
// (C) Copyright 2003-2016 by Autodesk, Inc. All rights reserved.
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
using System.Text;
using System.Windows.Forms;
using System.IO;

using Autodesk;
using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExternalService;
using Autodesk.Revit.UI;
using Autodesk.Revit.ApplicationServices;

namespace Revit.SDK.Samples.ExternalResourceUIServer.CS
{
   /// <summary>
   /// <para>Implements the Revit add-in interface IExternalApplication.</para>
   /// <para>An IExternalResourceUIServer can be registered at any time during a Revit session.
   /// However, the most straightforward approach is to register during start-up, in the
   /// OnStartUp method of a Revit external application.  This should be a (UI-level)
   /// IExternalApplication, and NOT an IExternalDBApplication.</para>
   /// </summary>
   class UIServerApplication : IExternalApplication
   {
      #region IExternalApplication Members
      /// <summary>
      /// Registers an instance of a SampleExternalResourceUIServer with the ExternalService
      /// of type ExternalResourceUIService. 
      /// </summary>
      /// <param name="application">An object that is passed to the external application
      /// which contains the controlled application.</param>
      /// <returns>Return the status of the external application.  A result of Succeeded
      /// means that the external application was able to register the IExternalResourceUIServer.
      /// </returns>
      public Result OnStartup(UIControlledApplication application)
      {
         ExternalService externalResourceUIService = ExternalServiceRegistry.GetService(ExternalServices.BuiltInExternalServices.ExternalResourceUIService);
         if (externalResourceUIService == null)
            return Result.Failed;

         // Create an instance of your IExternalResourceUIServer and register it with the ExternalResourceUIService.
         IExternalResourceUIServer sampleUIServer = new SampleExternalResourceUIServer();
         externalResourceUIService.AddServer(sampleUIServer);
         return Result.Succeeded;
      }

      /// <summary>
      /// Implements the OnShutdown event.
      /// <para>The server implementer may wish to perform clean-up tasks here.  However, in the
      /// simplest case, no server-related code is required, and the server will be
      /// destroyed as Revit shuts down.</para>
      /// </summary>
      /// <param name="application">An object that is passed to the external application
      /// which contains the controlled application.</param>
      /// <returns>Return the status of the external application.</returns>
      public Result OnShutdown(UIControlledApplication application)
      {
         return Result.Succeeded;
      }

      #endregion IExternalApplication Members
   }
 }
