//
// (C) Copyright 2003-2019 by Autodesk, Inc.
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
using System.IO;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RApplication = Autodesk.Revit.ApplicationServices.Application;
using TaskDialog = Autodesk.Revit.UI.TaskDialog;

namespace Revit.SDK.Samples.UIAPI.CS 
{
   public class ExternalApp : IExternalApplication
    {

        static String addinAssmeblyPath = typeof(ExternalApp).Assembly.Location;

        /// <summary>
        /// Loads the default Mass template automatically rather than showing UI.
        /// </summary>
        /// <param name="application">An object that is passed to the external application 
        /// which contains the controlled application.</param>
        void createCommandBinding(UIControlledApplication application)
        {
           RevitCommandId wallCreate = RevitCommandId.LookupCommandId("ID_NEW_REVIT_DESIGN_MODEL");
           AddInCommandBinding binding = application.CreateAddInCommandBinding(wallCreate);
           binding.Executed += new EventHandler<Autodesk.Revit.UI.Events.ExecutedEventArgs>(binding_Executed);
           binding.CanExecute += new EventHandler<Autodesk.Revit.UI.Events.CanExecuteEventArgs>(binding_CanExecute);
        }


        BitmapSource convertFromBitmap(System.Drawing.Bitmap bitmap)
        {
            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                bitmap.GetHbitmap(),
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
        }

        void createRibbonButton(UIControlledApplication application)
        {
            application.CreateRibbonTab("AddIn Integration");
            RibbonPanel rp = application.CreateRibbonPanel("AddIn Integration", "Testing");

            PushButtonData pbd = new PushButtonData("Wall", "Goto WikiHelp for wall creation",
                    addinAssmeblyPath,
                    "Revit.SDK.Samples.UIAPI.CS.CalcCommand");
            ContextualHelp ch = new ContextualHelp(ContextualHelpType.ContextId, "HID_OBJECTS_WALL");
            pbd.SetContextualHelp(ch);
            pbd.LongDescription = "We redirect the wiki help for this button to Wall creation.";
            pbd.LargeImage = convertFromBitmap(Revit.SDK.Samples.UIAPI.CS.Properties.Resources.StrcturalWall);
            pbd.Image = convertFromBitmap(Revit.SDK.Samples.UIAPI.CS.Properties.Resources.StrcturalWall_S);
            
            PushButton pb = rp.AddItem(pbd) as PushButton;
            pb.Enabled = true;
            pb.AvailabilityClassName = "Revit.SDK.Samples.UIAPI.CS.ApplicationAvailabilityClass";

            PushButtonData pbd1 = new PushButtonData("GotoGoogle", "Go to Google",
                    addinAssmeblyPath,
                    "Revit.SDK.Samples.UIAPI.CS.CalcCommand");
            ContextualHelp ch1 = new ContextualHelp(ContextualHelpType.Url, "http://www.google.com/");
            pbd1.SetContextualHelp(ch1);
            pbd1.LongDescription = "Go to google.";
            pbd1.LargeImage = convertFromBitmap(Revit.SDK.Samples.UIAPI.CS.Properties.Resources.StrcturalWall);
            pbd1.Image = convertFromBitmap(Revit.SDK.Samples.UIAPI.CS.Properties.Resources.StrcturalWall_S);
            PushButton pb1 = rp.AddItem(pbd1) as PushButton;
            pb1.AvailabilityClassName = "Revit.SDK.Samples.UIAPI.CS.ApplicationAvailabilityClass";

            PushButtonData pbd2 = new PushButtonData("GotoRevitAddInUtilityHelpFile", "Go to Revit Add-In Utility",
                addinAssmeblyPath,
                "Revit.SDK.Samples.UIAPI.CS.CalcCommand");

            ContextualHelp ch2 = new ContextualHelp(ContextualHelpType.ChmFile, Path.GetDirectoryName(addinAssmeblyPath) + @"\RevitAddInUtility.chm");
            ch2.HelpTopicUrl = @"html/3374f8f0-dccc-e1df-d269-229ed8c60e93.htm";    
            pbd2.SetContextualHelp(ch2);
            pbd2.LongDescription = "Go to Revit Add-In Utility.";
            pbd2.LargeImage = convertFromBitmap(Revit.SDK.Samples.UIAPI.CS.Properties.Resources.StrcturalWall);
            pbd2.Image = convertFromBitmap(Revit.SDK.Samples.UIAPI.CS.Properties.Resources.StrcturalWall_S);
            PushButton pb2 = rp.AddItem(pbd2) as PushButton;
            pb2.AvailabilityClassName = "Revit.SDK.Samples.UIAPI.CS.ApplicationAvailabilityClass";


            PushButtonData pbd3 = new PushButtonData("PreviewControl", "Preview all views",
                addinAssmeblyPath,
                "Revit.SDK.Samples.UIAPI.CS.PreviewCommand");
            pbd3.LargeImage = convertFromBitmap(Revit.SDK.Samples.UIAPI.CS.Properties.Resources.StrcturalWall);
            pbd3.Image = convertFromBitmap(Revit.SDK.Samples.UIAPI.CS.Properties.Resources.StrcturalWall_S);
            PushButton pb3 = rp.AddItem(pbd3) as PushButton;
            pb3.AvailabilityClassName = "Revit.SDK.Samples.UIAPI.CS.ApplicationAvailabilityClass";


            PushButtonData pbd5 = new PushButtonData("Drag_And_Drop", "Drag and Drop", addinAssmeblyPath,
                                                     "Revit.SDK.Samples.UIAPI.CS.DragAndDropCommand");
            pbd5.LargeImage = convertFromBitmap(Revit.SDK.Samples.UIAPI.CS.Properties.Resources.StrcturalWall);
            pbd5.Image = convertFromBitmap(Revit.SDK.Samples.UIAPI.CS.Properties.Resources.StrcturalWall_S);
            PushButton pb5 = rp.AddItem(pbd5) as PushButton;
            pb5.AvailabilityClassName = "Revit.SDK.Samples.UIAPI.CS.ApplicationAvailabilityClass";
        }


        /// <summary>
        /// Implement this method to implement the external application which should be called when 
        /// Revit is about to exit,Any documents must have been closed before this method is called.
        /// </summary>
        /// <param name="application">An object that is passed to the external application 
        /// which contains the controlled application.</param>
        /// <returns>Return the status of the external application.
        /// A result of Succeeded means that the external application successfully shutdown. 
        /// Cancelled can be used to signify that the user cancelled the external operation at 
        /// some point.                                                                        
        /// If false is returned then the Revit user should be warned of the failure of the external 
        /// application to shut down correctly.</returns> 
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        /// Implement this method to implement the external application which should be called when 
        /// Revit starts before a file or default template is actually loaded.
        /// </summary>
        /// <param name="application">An object that is passed to the external application
        /// which contains the controlled application.</param>
        /// <returns>Return the status of the external application.
        /// A result of Succeeded means that the external application successfully started.
        /// Cancelled can be used to signify that the user cancelled the external operation at
        /// some point.
        /// If false is returned then Revit should inform the user that the external application
        /// failed to load and the release the internal reference.</returns> 
        public Result OnStartup(UIControlledApplication application)
        {
            s_uiApplication = application;
            ApplicationOptions.Initialize(this);

            createCommandBinding(application);
            createRibbonButton(application);
            
            // add custom tabs to options dialog.
            AddTabCommand addTabCommand = new AddTabCommand(application);
            addTabCommand.AddTabToOptionsDialog();
            return Result.Succeeded;
        }

        public UIControlledApplication UIControlledApplication
        {
            get { return s_uiApplication; }
        }

        private UIControlledApplication s_uiApplication;

        void binding_CanExecute(object sender, Autodesk.Revit.UI.Events.CanExecuteEventArgs e)
        {
            e.CanExecute = true;
        }

        void binding_Executed(object sender, Autodesk.Revit.UI.Events.ExecutedEventArgs e)
        {
           UIApplication uiApp = sender as UIApplication;
           if (uiApp == null)
              return;

           String famTemplatePath = uiApp.Application.FamilyTemplatePath;
           String conceptualmassTemplatePath = famTemplatePath + @"\Conceptual Mass\Mass.rft";
           if (System.IO.File.Exists(conceptualmassTemplatePath))
           {
              //uiApp.OpenAndActivateDocument(conceptualmassTemplatePath);
              Document familyDocument = uiApp.Application.NewFamilyDocument(conceptualmassTemplatePath);
              if (null == familyDocument)
              {
                 throw new Exception("Cannot open family document");
              }

              String fileName = Guid.NewGuid().ToString() + ".rfa";
              familyDocument.SaveAs(fileName);
              familyDocument.Close();

              uiApp.OpenAndActivateDocument(fileName);

              FilteredElementCollector collector = new FilteredElementCollector(uiApp.ActiveUIDocument.Document);
              collector = collector.OfClass(typeof(View3D));

              var query = from element in collector

                          where element.Name == "{3D}"

                          select element; // Linq query  

              List<Autodesk.Revit.DB.Element> views = query.ToList<Autodesk.Revit.DB.Element>();

              View3D view3D = views[0] as View3D;
              if(view3D != null)
               uiApp.ActiveUIDocument.ActiveView = view3D;



           }
        }
    }

   /// <summary>
   /// Implement this method as an external command for Revit.
   /// </summary>
   /// <param name="commandData">An object that is passed to the external application
   /// which contains data related to the command,
   /// such as the application object and active view.</param>
   /// <param name="message">A message that can be set by the external application
   /// which will be displayed if a failure or cancellation is returned by
   /// the external command.</param>
   /// <param name="elements">A set of elements to which the external application
   /// can add elements that are to be highlighted in case of failure or cancellation.</param>
   /// <returns>Return the status of the external command.
   /// A result of Succeeded means that the API external method functioned as expected.
   /// Cancelled can be used to signify that the user cancelled the external operation 
   /// at some point. Failure should be returned if the application is unable to proceed with
   /// the operation.</returns>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class CalcCommand : IExternalCommand
    {

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            TaskDialog.Show("Dummy command", "This is a dummy command for buttons associated to contextual help.");
            return Result.Succeeded;
        }
    }
}
