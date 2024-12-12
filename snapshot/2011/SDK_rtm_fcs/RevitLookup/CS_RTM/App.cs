//
// Copyright 2003-2010 by Autodesk, Inc.
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
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC. 
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
using System.Text;
using System.Windows.Forms;

using Autodesk.Revit;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;

namespace RvtMgdDbg
{

   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   public class App : IExternalApplication
   {

      // get the absolute path of this assembly
      static string ExecutingAssemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
      private AppDocEvents m_appDocEvents;

      public Autodesk.Revit.UI.Result OnStartup(UIControlledApplication application)
      {
         AddMenu(application);
         AddAppDocEvents(application.ControlledApplication);

         return Autodesk.Revit.UI.Result.Succeeded;
      }

      public Autodesk.Revit.UI.Result OnShutdown(UIControlledApplication application)
      {
         RemoveAppDocEvents();

         return Autodesk.Revit.UI.Result.Succeeded;
      }

      private void AddMenu(UIControlledApplication app)
      {
         Autodesk.Revit.UI.RibbonPanel rvtRibbonPanel = app.CreateRibbonPanel("Revit Lookup");
         PulldownButtonData data = new PulldownButtonData("Options", "Revit Lookup");

         RibbonItem item = rvtRibbonPanel.AddItem(data);
         PulldownButton optionsBtn = item as PulldownButton;

         optionsBtn.AddPushButton(new PushButtonData("HelloWorld", "Hello World...", ExecutingAssemblyPath, "RvtMgdDbg.HelloWorld"));
         optionsBtn.AddPushButton(new PushButtonData("Snoop Db..", "Snoop DB...", ExecutingAssemblyPath, "RvtMgdDbg.CmdSnoopDb"));
         optionsBtn.AddPushButton(new PushButtonData("Snoop Current Selection...", "Snoop Current Selection...", ExecutingAssemblyPath, "RvtMgdDbg.CmdSnoopModScope"));
         optionsBtn.AddPushButton(new PushButtonData("Snoop Application...", "Snoop Application...", ExecutingAssemblyPath, "RvtMgdDbg.CmdSnoopApp"));
         optionsBtn.AddPushButton(new PushButtonData("Test Framework...", "Test Framework...", ExecutingAssemblyPath, "RvtMgdDbg.CmdTestShell"));
         optionsBtn.AddPushButton(new PushButtonData("Events...", "Events...", ExecutingAssemblyPath, "RvtMgdDbg.CmdEvents"));
      }

      private void AddAppDocEvents(Autodesk.Revit.ApplicationServices.ControlledApplication app)
      {
         m_appDocEvents = new AppDocEvents(app);
         m_appDocEvents.EnableEvents();
      }

      private void RemoveAppDocEvents()
      {
         m_appDocEvents.DisableEvents();
      }
   }
}
