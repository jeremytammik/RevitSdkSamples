//
// (C) Copyright 2003-2013 by Autodesk, Inc.
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

namespace CodeCheckingConcreteExample
{
#pragma warning disable 1591
   public class RevitApplicationDB : Autodesk.Revit.DB.IExternalDBApplication
   {
      #region IExternalDBApplication Members

      public Autodesk.Revit.DB.ExternalDBApplicationResult OnShutdown(Autodesk.Revit.ApplicationServices.ControlledApplication application)
      {
         return Autodesk.Revit.DB.ExternalDBApplicationResult.Succeeded;
      }

      public Autodesk.Revit.DB.ExternalDBApplicationResult OnStartup(Autodesk.Revit.ApplicationServices.ControlledApplication application)
      {
         application.ApplicationInitialized += new EventHandler<Autodesk.Revit.DB.Events.ApplicationInitializedEventArgs>(application_ApplicationInitialized);


         return Autodesk.Revit.DB.ExternalDBApplicationResult.Succeeded;
      }

      void application_ApplicationInitialized(object sender, Autodesk.Revit.DB.Events.ApplicationInitializedEventArgs e)
      {
         Server.Server server = new Server.Server();

         Autodesk.Revit.DB.CodeChecking.Service.AddServer(server);

         //Updater updater = new Updater();
         //UpdaterRegistry.RegisterUpdater(updater);
         //UpdaterRegistry.AddTrigger(updater.GetUpdaterId(), new ElementClassFilter(typeof(FamilyInstance)), Element.GetChangeTypeGeometry());            

      }

      #endregion
   }
}
