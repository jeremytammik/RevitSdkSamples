//
// (C) Copyright 2003-2008 by Autodesk, Inc.
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


namespace Revit.SDK.Samples.DoorSwing.CS
{
   /// <summary>
   /// A class inherited IExternalApplication interface.
   /// This class subscribes to some application level events and 
   /// creates a custom toolbar which contains three buttons.
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

         // Doors are updated from the application level events. 
         // That will insure that the doc is correct when it is saved, closed etc.
         // Subscribe to related events.
         application.OnDocumentSaved   += new DocumentSavedEventHandler(OnDocumentSaved);
         application.OnDocumentSavedAs += new DocumentSavedAsEventHandler(OnDocumentSaveAs);
         application.OnDocumentClosed  += new DocumentClosedEventHandler(OnDocumentClosed);

         // create custom Toolbar.
         Toolbar doorToolbar = application.CreateToolbar();
         doorToolbar.Name    = "DoorSwing";

         // The location of this command assembly
         string currentCommandAssemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;

         // The path of ourselves's DoorSwing.bmp
         string toolbarImagePath = Path.GetDirectoryName( Path.GetDirectoryName( Path.GetDirectoryName // jeremy: use GetFullPath( path + "../../.." ) instead!
                                   (Path.GetDirectoryName(currentCommandAssemblyPath)))); // jeremy: use GetFullPath( path + "../../.." ) instead!
         toolbarImagePath        = toolbarImagePath + "\\DoorSwing.bmp";
         doorToolbar.Image       = toolbarImagePath;

         // the first button in the DoorSwing toolbar, use to invoke the InitializeCommand.
         ToolbarItem iniButton = doorToolbar.AddItem(currentCommandAssemblyPath,typeof(InitializeCommand).FullName);
         iniButton.ItemType    = ToolbarItem.ToolbarItemType.BtnRText;
         iniButton.ItemText    = "Customize Door Opening Expression";
         iniButton.ToolTip     = "Customize the expression based on family's geometry and country's standard.";

         // the second button in the DoorSwing toolbar, use to invoke the UpdateParamsCommand.
         ToolbarItem updateParamButton = doorToolbar.AddItem(currentCommandAssemblyPath,typeof(UpdateParamsCommand).FullName);
         updateParamButton.ItemType    = ToolbarItem.ToolbarItemType.BtnRText;
         updateParamButton.ItemText    = "Update Door Properties";
         updateParamButton.ToolTip     = "Update door properties based on geometry.";

         // the third button in the DoorSwing toolbar, use to invoke the UpdateGeometryCommand.
         ToolbarItem updateGepButton = doorToolbar.AddItem(currentCommandAssemblyPath,typeof(UpdateGeometryCommand).FullName);
         updateGepButton.ItemType    = ToolbarItem.ToolbarItemType.BtnRText;
         updateGepButton.ItemText    = "Update Door Geometry";
         updateGepButton.ToolTip     = "Update door geometry based on From/To room property.";

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
      /// This event is fired whenever a document is closed.
      /// Update door's information according to door's current geometry.
      /// </summary>
      /// <param name="doc">The specific document which is closed<./param>
      private void OnDocumentClosed(Document doc)
      {
         string message = "";
         try
         {
            if (DoorSwingData.UpdateDoorsInfo(m_controlApp.Create.Filter, doc, false, false, ref message) != IExternalCommand.Result.Succeeded)
               MessageBox.Show(message, "Door Swing");
         }
         catch (Exception ex)
         {
            // if there are something wrong, give error message.
            MessageBox.Show(ex.Message, "Door Swing");
         }
      }

      /// <summary>
      /// This event is fired whenever a document is saved.
      /// Update door's information according to door's current geometry.
      /// </summary>
      /// <param name="doc">The specific document which is saved<./param>
      private void OnDocumentSaved(Document doc)
      {
         string message = "";
         try
         {
            if (DoorSwingData.UpdateDoorsInfo(m_controlApp.Create.Filter, doc, false, false, ref message) != IExternalCommand.Result.Succeeded)
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
      /// <param name="doc">The specific document which is saved as.<./param>
      private void OnDocumentSaveAs(Document doc)
      {
         string message = "";
         try
         {
            if (DoorSwingData.UpdateDoorsInfo(m_controlApp.Create.Filter, doc, false, false, ref message) != IExternalCommand.Result.Succeeded)
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
