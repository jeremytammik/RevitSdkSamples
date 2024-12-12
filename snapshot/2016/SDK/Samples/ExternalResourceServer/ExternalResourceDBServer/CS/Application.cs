//
// (C) Copyright 2003-2015 by Autodesk, Inc. All rights reserved.
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
using Autodesk.Revit.ApplicationServices;

namespace Revit.SDK.Samples.ExternalResourceDBServer.CS
{
   /// <summary>
   /// <para>Implements the Revit add-in interface IExternalDBApplication.</para>
   /// <para>An IExternalResourceServer can be registered at any time during a Revit session.
   /// However, the most straightforward approach is to register during start-up, in the
   /// OnStartUp method of a Revit external application.  Since IExternalResourceServers
   /// do not implement any UI directly, they can be registered in a DB-only context,
   /// using an IExternalDBApplication.</para>
   /// </summary>
   public class DBApplication : IExternalDBApplication
   {
      #region IExternalDBApplication Members
      /// <summary>
      /// Registers an instance of a SampleExternalResourceDBServer with the ExternalService
      /// of type ExternalResourceService. 
      /// </summary>
      /// <param name="application">An object that is passed to the external application
      /// which contains the controlled application.</param>
      /// <returns>Return the status of the external application.  A result of Succeeded
      /// means that the external application was able to register the IExternalResourceServer.
      /// </returns>
      public ExternalDBApplicationResult OnStartup(ControlledApplication application)
      {
         // Get Revit's ExternalResourceService.
         ExternalService externalResourceService = ExternalServiceRegistry.GetService(ExternalServices.BuiltInExternalServices.ExternalResourceService);

         if (externalResourceService == null)
            return ExternalDBApplicationResult.Failed;

         // Create an instance of your IExternalResourceServer and register it with the ExternalResourceService.
         IExternalResourceServer sampleServer = new SampleExternalResourceDBServer();
         externalResourceService.AddServer(sampleServer);
         return ExternalDBApplicationResult.Succeeded;
      }


      /// <summary>
      /// <para>Implements the OnShutdown event.</para>
      /// <para>The server implementer may wish to perform clean-up tasks here.  However, in the
      /// simplest case, no server-related code is required, and the server will be
      /// destroyed as Revit shuts down.</para>
      /// </summary>
      /// <param name="application">An object that is passed to the external application
      /// which contains the controlled application.</param>
      /// <returns>Return the status of the external application.</returns>
      public ExternalDBApplicationResult OnShutdown(ControlledApplication application)
      {
         return ExternalDBApplicationResult.Succeeded;
      }

      #endregion IExternalDBApplication Members
   }
}
