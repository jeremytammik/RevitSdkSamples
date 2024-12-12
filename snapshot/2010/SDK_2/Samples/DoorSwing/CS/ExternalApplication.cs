//
// (C) Copyright 2003-2009 by Autodesk, Inc.
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
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using Autodesk.Revit;
using Autodesk.Revit.Events;
using System.Windows.Media.Imaging;


namespace Revit.SDK.Samples.DoorSwing.CS
{
   /// <summary>
   /// A class inherited IExternalApplication interface.
   /// This class subscribes to some application level events and 
   /// creates a custom Ribbon panel which contains three buttons.
   /// </summary
   public class ExternalApplication : IExternalApplication
   {
      #region "Members" 

      // An object that is passed to the external application which contains the controlled Revit application.
      ControlledApplication m_controlApp; 

      #endregion

      #region IExternalApplication Members

      /// <summary>
      /// Implement this method to implement the external application which should be called when 
      /// Revit starts before a file or default template is actually loaded.
      /// <param name="application">An object that is passed to the external application 
      /// which contains the controlled application.</param>
      /// <returns>Return the status of the external application. 
      /// A result of Succeeded means that the external application successfully started. 
      /// Cancelled can be used to signify that the user cancelled the external operation at 
      /// some point.
      /// If false is returned then Revit should inform the user that the external application 
      /// failed to load and the release the internal reference.</returns> 
      public IExternalApplication.Result OnStartup(ControlledApplication application)
      {
         m_controlApp = application; 

         #region Subscribe to related events

         // Doors are updated from the application level events. 
         // That will insure that the doc is correct when it is saved.
         // Subscribe to related events.
         application.DocumentSaving   += new EventHandler<DocumentSavingEventArgs>(DocumentSavingHandler);
         application.DocumentSavingAs += new EventHandler<DocumentSavingAsEventArgs>(DocumentSavingAsHandler);

         #endregion

         #region create a custom Ribbon panel which contains three buttons

         // The location of this command assembly
         string currentCommandAssemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;

         // The directory path of buttons' images
         string buttonImageDir = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName
                (Path.GetDirectoryName(currentCommandAssemblyPath))));
         
         // begin to create custom Ribbon panel and command buttons.
         // create a Ribbon panel.
         RibbonPanel doorPanel = application.CreateRibbonPanel("Door Swing");
         
         // the first button in the DoorSwing panel, use to invoke the InitializeCommand.
         PushButton initialCommandBut = doorPanel.AddPushButton("Customize Door Opening", 
                                                                "Customize Door Opening", 
                                                                currentCommandAssemblyPath, 
                                                                typeof(InitializeCommand).FullName);
         initialCommandBut.ToolTip    = "Customize the expression based on family's geometry and country's standard.";
         initialCommandBut.LargeImage = new BitmapImage(new Uri(Path.Combine(buttonImageDir, "InitialCommand_Large.bmp")));
         initialCommandBut.Image      = new BitmapImage(new Uri(Path.Combine(buttonImageDir, "InitialCommand_Small.bmp")));

         // the second button in the DoorSwing panel, use to invoke the UpdateParamsCommand.
         PushButton updateParamBut = doorPanel.AddPushButton("Update Door Properties",
                                                             "Update Door Properties",
                                                             currentCommandAssemblyPath, 
                                                             typeof(UpdateParamsCommand).FullName);
         updateParamBut.ToolTip    = "Update door properties based on geometry.";
         updateParamBut.LargeImage = new BitmapImage(new Uri(Path.Combine(buttonImageDir, "UpdateParameter_Large.bmp")));
         updateParamBut.Image      = new BitmapImage(new Uri(Path.Combine(buttonImageDir, "UpdateParameter_Small.bmp")));

         // the third button in the DoorSwing panel, use to invoke the UpdateGeometryCommand.
         PushButton updateGeoBut = doorPanel.AddPushButton("Update Door Geometry", 
                                                           "Update Door Geometry", 
                                                           currentCommandAssemblyPath, 
                                                           typeof(UpdateGeometryCommand).FullName);
         updateGeoBut.ToolTip    = "Update door geometry based on From/To room property.";
         updateGeoBut.LargeImage = new BitmapImage(new Uri(Path.Combine(buttonImageDir, "UpdateGeometry_Large.bmp")));
         updateGeoBut.Image      = new BitmapImage(new Uri(Path.Combine(buttonImageDir, "UpdateGeometry_Small.bmp")));

         #endregion

         return IExternalApplication.Result.Succeeded;
      }

      /// <summary>
      /// Implement this method to implement the external application which should be called when 
      /// Revit is about to exit,Any documents must have been closed before this method is called.
      /// </summary>
      /// <param name="application">An object that is passed to the external application 
      /// which contains the controlled application.</param>
      /// <returns>Return the status of the external application. 
      /// A result of Succeeded means that the external application successfully shutdown. 
      /// Cancelled can be used to signify that the user cancelled the external operation at some point.
      /// If false is returned then the Revit user should be warned of the failure of the external 
      /// application to shut down correctly.</returns>
      public IExternalApplication.Result OnShutdown(ControlledApplication application)
      {
         return IExternalApplication.Result.Succeeded;
      }

      #endregion

      /// <summary>
      /// This event is fired whenever a document is saved.
      /// Update door's information according to door's current geometry.
      /// </summary>
      /// <param name="sender">The source of the event.</param>
      /// <param name="args">An DocumentSavingEventArgs that contains the DocumentSaving event data.</param>
      private void DocumentSavingHandler(Object sender, DocumentSavingEventArgs args)
      {
         string message = "";
         try
         {
            if (DoorSwingData.UpdateDoorsInfo(m_controlApp.Create.Filter, args.Document, false, false, ref message) != IExternalCommand.Result.Succeeded)
               MessageBox.Show(message, "Door Swing");
         }
         catch (Exception ex)
         {
            // if there are something wrong, give error information message.
            MessageBox.Show(ex.Message, "Door Swing");
         }
      }

      /// <summary>
      /// This event is fired whenever a document is saved as.
      /// Update door's information according to door's current geometry.
      /// </summary>
      /// <param name="sender">The source of the event.</param>
      /// <param name="args">An DocumentSavingAsEventArgs that contains the DocumentSavingAs event data.</param>
      private void DocumentSavingAsHandler(Object sender, DocumentSavingAsEventArgs args)
      {
         string message = "";
         try
         {
             if (DoorSwingData.UpdateDoorsInfo(m_controlApp.Create.Filter, args.Document, false, false, ref message) != IExternalCommand.Result.Succeeded)
               MessageBox.Show(message, "Door Swing");
         }
         catch (Exception ex)
         {
            // if there are something wrong, give error message.
            MessageBox.Show(ex.Message, "Door Swing");
         } 
      }
   }
}
