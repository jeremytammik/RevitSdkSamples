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
using Autodesk.Revit.UI;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB.ExternalService;
using Autodesk.Revit.DB.Structure;

namespace Revit.SDK.Samples.RebarFreeForm.CS
{
   /// <summary>
   /// Implements the Revit add-in interface IExternalApplication
   /// </summary>
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   public class Application : IExternalApplication
   {
      #region IExternalApplication Members

      RebarUpdateServer m_server = new RebarUpdateServer();

      #endregion

      #region IExternalApplication Interface Implementation
      /// <summary>
      /// Implements the OnShutdown event
      /// </summary>
      /// <param name="application"></param>
      /// <returns></returns>
      public Result OnShutdown(UIControlledApplication application)
      {
         return Result.Succeeded;
      }

      /// <summary>
      /// Implements the OnStartup event
      /// 
      /// </summary>
      /// <param name="application"></param>
      /// <returns></returns>
      public Result OnStartup(UIControlledApplication application)
      {
         // Register CurveElement updater with revit to trigger regen in rebar for selected lines
         CurveElementRegenUpdater updater = new CurveElementRegenUpdater(application.ActiveAddInId);
         UpdaterRegistry.RegisterUpdater(updater);
         ElementClassFilter modelLineFilter = new ElementClassFilter(typeof(CurveElement));
         UpdaterRegistry.AddTrigger(updater.GetUpdaterId(), modelLineFilter, Element.GetChangeTypeAny());

         //Register the RebarUpdateServer
         ExternalService service = ExternalServiceRegistry.GetService(m_server.GetServiceId());
         if (service != null)
         {
            service.AddServer(m_server);
            return Result.Succeeded;
         }
         return Result.Succeeded;
      }
      #endregion
   }
}
