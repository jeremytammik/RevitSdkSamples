//
// (C) Copyright 2003-2016 by Autodesk, Inc.
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

using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using System.Timers;
using Autodesk.Revit.UI.Events;
using System.Windows.Media.Imaging;
using System.Windows;

using Autodesk.Revit;
using System.Net;
using System.IO;
using System.Reflection;

namespace Revit.SDK.Samples.GetSetDefaultTypes.CS
{
   /// <summary>
   /// Implements the Revit add-in interface IExternalApplication
   /// </summary>
   public class ThisApplication : IExternalApplication
   {
      #region IExternalApplication Members

      public Result OnShutdown(UIControlledApplication application)
      {
         return Result.Succeeded;
      }

      public Result OnStartup(UIControlledApplication application)
      {
         try
         {
            string str = "Default Type Selector";
            RibbonPanel panel = application.CreateRibbonPanel(str);
            string directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            PushButtonData data = new PushButtonData("Default Type Selector", "Default Type Selector", directoryName + @"\GetSetDefaultTypes.dll", "Revit.SDK.Samples.GetSetDefaultTypes.CS.ThisCommand");
            PushButton button = panel.AddItem(data) as PushButton;
            button.LargeImage = new BitmapImage(new Uri(directoryName + "\\Resources\\type.png"));

            // register dockable Windows on startup.
            DefaultFamilyTypesPane = new DefaultFamilyTypes();
            DefaultElementTypesPane = new DefaultElementTypes();
            application.RegisterDockablePane(DefaultFamilyTypes.PaneId, "Default Family Types", DefaultFamilyTypesPane);
            application.RegisterDockablePane(DefaultElementTypes.PaneId, "Default Element Types", DefaultElementTypesPane);

            // register view active event
            application.ViewActivated += new EventHandler<ViewActivatedEventArgs>(application_ViewActivated);

            return Result.Succeeded;
         }
         catch (Exception exception)
         {
            MessageBox.Show(exception.ToString(), "Default Type Selector");
            return Result.Failed;
         }
      }

      #endregion

      public static DefaultFamilyTypes DefaultFamilyTypesPane;
      public static DefaultElementTypes DefaultElementTypesPane;


      /// <summary>
      /// Show dockable panes when view active.
      /// </summary>
      void application_ViewActivated(object sender, Autodesk.Revit.UI.Events.ViewActivatedEventArgs e)
      {
         if (!DockablePane.PaneExists(DefaultFamilyTypes.PaneId) ||
             !DockablePane.PaneExists(DefaultElementTypes.PaneId))
            return;

         UIApplication uiApp = sender as UIApplication;
         if (uiApp == null)
            return;

         if (DefaultFamilyTypesPane != null)
            DefaultFamilyTypesPane.SetDocument(e.Document);
         if (DefaultElementTypesPane != null)
            DefaultElementTypesPane.SetDocument(e.Document);
      }

   }
}
