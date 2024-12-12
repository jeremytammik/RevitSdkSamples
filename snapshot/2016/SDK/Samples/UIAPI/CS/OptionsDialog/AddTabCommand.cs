//
// (C) Copyright 2003-2015 by Autodesk, Inc.
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
using Autodesk.Revit.UI;
using RApplication = Autodesk.Revit.ApplicationServices.Application;

namespace Revit.SDK.Samples.UIAPI.CS
{
   public class AddTabCommand
   {

      public AddTabCommand(UIControlledApplication application)
      {
         _application = application;
      }

      public bool AddTabToOptionsDialog()
      {
         _application.DisplayingOptionsDialog += 
            new EventHandler<Autodesk.Revit.UI.Events.DisplayingOptionsDialogEventArgs>(Command_DisplayingOptionDialog);
         return true;
      }

      void Command_DisplayingOptionDialog(object sender, Autodesk.Revit.UI.Events.DisplayingOptionsDialogEventArgs e)
      {
          // Actual options
          Revit.SDK.Samples.UIAPI.CS.OptionsDialog.Options optionsControl = new Revit.SDK.Samples.UIAPI.CS.OptionsDialog.Options();
          ContextualHelp ch = new ContextualHelp(ContextualHelpType.Url, "http://www.autodesk.com/");
          TabbedDialogExtension extension = new TabbedDialogExtension(optionsControl, optionsControl.OnOK);
          extension.OnRestoreDefaultsAction = optionsControl.OnRestoreDefaults;
          extension.SetContextualHelp(ch);
          e.AddTab("Demo options", extension);

          // Demo options
          UserControl3 userControl3 = new UserControl3("Product Information");
          ContextualHelp ch3 = new ContextualHelp(ContextualHelpType.Url, "http://www.google.com/");
          TabbedDialogExtension tdext3 = new TabbedDialogExtension(userControl3,
             userControl3.OnOK);
          tdext3.OnCancelAction = userControl3.OnCancel;
          tdext3.OnRestoreDefaultsAction = userControl3.OnRestoreDefaults;
          tdext3.SetContextualHelp(ch);
          e.AddTab("Product Information", tdext3);
            
            UserControl2 userControl2 = new UserControl2("Copy of SteeringWheels");
            TabbedDialogExtension tdext2 = new TabbedDialogExtension(userControl2,
               userControl2.OnOK);
            tdext2.OnCancelAction = userControl2.OnCancel;
            e.AddTab("SteeringWheels(Copy)", tdext2);

            UserControl1 userControl1 = new UserControl1();
            ContextualHelp ch1 = new ContextualHelp(ContextualHelpType.Url, "http://www.google.com/");
            TabbedDialogExtension tdext1 = new TabbedDialogExtension(userControl1,
               userControl1.OnOK);
            tdext1.OnCancelAction = userControl1.OnCancel;
            tdext1.OnRestoreDefaultsAction = userControl1.OnRestoreDefaults;
            tdext1.SetContextualHelp(ch);
            e.AddTab("WPF components", tdext1);               
      }

      private UIControlledApplication _application = null;
   }
}
