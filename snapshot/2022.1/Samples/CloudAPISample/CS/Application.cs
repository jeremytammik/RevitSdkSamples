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
using System.IO;
using System.Windows.Media.Imaging;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Revit.SDK.Samples.CloudAPISample.CS.Migration;

namespace Revit.SDK.Samples.CloudAPISample.CS
{
   /// <summary>
   ///    Main external application class.
   ///    A generic menu generator application.
   ///    Read a text file and add entries to the Revit menu.
   ///    Any number and location of entries is supported.
   /// </summary>
   [Transaction(TransactionMode.Manual)]
   [Regeneration(RegenerationOption.Manual)]
   public class Application : IExternalApplication
   {
      #region IExternalApplication Members

      /// <summary>
      ///    Implement this method to implement the external application which should be called when
      ///    Revit is about to exit,Any documents must have been closed before this method is called.
      /// </summary>
      /// <param name="application">
      ///    An object that is passed to the external application
      ///    which contains the controlled application.
      /// </param>
      /// <returns>
      ///    Return the status of the external application.
      ///    A result of Succeeded means that the external application successfully shutdown.
      ///    Cancelled can be used to signify that the user cancelled the external operation at
      ///    some point.
      ///    If false is returned then the Revit user should be warned of the failure of the external
      ///    application to shut down correctly.
      /// </returns>
      public Result OnShutdown(UIControlledApplication application)
      {
         return Result.Succeeded;
      }

      /// <summary>
      ///    Implement this method to implement the external application which should be called when
      ///    Revit starts before a file or default template is actually loaded.
      /// </summary>
      /// <param name="application">
      ///    An object that is passed to the external application
      ///    which contains the controlled application.
      /// </param>
      /// <returns>
      ///    Return the status of the external application.
      ///    A result of Succeeded means that the external application successfully started.
      ///    Cancelled can be used to signify that the user cancelled the external operation at
      ///    some point.
      ///    If false is returned then Revit should inform the user that the external application
      ///    failed to load and the release the internal reference.
      /// </returns>
      public Result OnStartup(UIControlledApplication application)
      {
         // Register this addon to Revit.
         var ribbonPanel = application.CreateRibbonPanel("Tutorials");
         var assemblyPath = typeof(Application).Assembly.Location;
         var pushButton = ribbonPanel.AddItem(new PushButtonData("CloudAPI Tutorial", "CloudAPI Tutorial", assemblyPath,
            "Revit.SDK.Samples.CloudAPISample.CS.RunSampleCommand")) as PushButton;
         pushButton.Enabled = true;

         pushButton.LargeImage =
            new BitmapImage(new Uri(Path.Combine(Path.GetDirectoryName(assemblyPath) ?? string.Empty, "icon.ico")));
         pushButton.AvailabilityClassName = " Revit.SDK.Samples.CloudAPISample.CS.RunSampleCommand";

         return Result.Succeeded;
      }

      #endregion
   }

   /// <summary>
   /// </summary>
   [Transaction(TransactionMode.Manual)]
   public class RunSampleCommand : IExternalCommand, IExternalCommandAvailability
   {
      /// <inheritdoc />
      public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
      {
         // Start to run samples.
         var engine = new SampleEngine(commandData.Application);
         engine.RegisterSample("Model Migration", new MigrationToBim360());
         engine.Run();

         return Result.Succeeded;
      }

      /// <inheritdoc />
      public bool IsCommandAvailable(UIApplication applicationData, CategorySet selectedCategories)
      {
         return true;
      }
   }
}