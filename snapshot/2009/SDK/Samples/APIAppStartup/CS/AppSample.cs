//
// (C) Copyright 1994-2006 by Autodesk, Inc. All rights reserved.
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
using Autodesk.Revit.Elements;

namespace APIAppStartup
{
   public class AppSample : IExternalApplication
   {
      #region IExternalApplication Members

      public IExternalApplication.Result OnShutdown(ControlledApplication application)
      {
         MessageBox.Show("Quit External Application!");
         return IExternalApplication.Result.Succeeded;
      }

       public IExternalApplication.Result OnStartup(ControlledApplication application)
      {
         String version = application.VersionName;

         //display splash window for 10 seconds
         SplashWindow.StartSplash();
         SplashWindow.ShowVersion(version);
         System.Threading.Thread.Sleep(10000);
         SplashWindow.StopSplash();

         return IExternalApplication.Result.Succeeded;
      }

      #endregion
   }
}
