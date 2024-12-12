//
// (C) Copyright 2003-2023 by Autodesk, Inc. All rights reserved.
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
using System.Windows.Forms;

using Autodesk.Revit.UI;

using TaskDialog = Autodesk.Revit.UI.TaskDialog;

namespace APIAppStartup
{
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
   public class AppSample : IExternalApplication
   {
      #region IExternalApplication Members

      public Autodesk.Revit.UI.Result OnShutdown(UIControlledApplication application)
      {
         TaskDialog.Show("Revit", "Quit External Application!");
         return Autodesk.Revit.UI.Result.Succeeded;
      }

       public Autodesk.Revit.UI.Result OnStartup(UIControlledApplication application)
      {
         String version = application.ControlledApplication.VersionName;

         //display splash window for 10 seconds
         SplashWindow.StartSplash();
         SplashWindow.ShowVersion(version);
         System.Threading.Thread.Sleep(10000);
         SplashWindow.StopSplash();

         return Autodesk.Revit.UI.Result.Succeeded;
      }

      #endregion
   }
}
