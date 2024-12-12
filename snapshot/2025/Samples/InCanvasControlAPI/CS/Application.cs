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

using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;

namespace Revit.SDK.Samples.InCanvasControlAPI.CS
{
   /// <summary>
   /// Implements the Revit add-in interface IExternalApplication
   /// </summary>
   public class Application : IExternalApplication
   {
      const string TabLabel = "Issues";

      #region Class implementation

      /// <summary>
      /// Creates external application object and initializes event handlers.
      /// </summary>
      public Application()
      {
         IssueMarkerTrackingManager issueMarkerTrackingManager = IssueMarkerTrackingManager.GetInstance();

         // This event handler moves or deletes the markers based on changes to the tracked elements
         updateHandler = (sender, data) =>
         {
            IssueMarkerUpdater.Execute(data);
         };

         // This event handler initiates data for the opened document
         openHandler = (sender, data) =>
         {
            issueMarkerTrackingManager.AddTracking(data.Document);
         };

         // This event handler initiates data for the newly-created document
         createHandler = (sender, data) =>
         {
            issueMarkerTrackingManager.AddTracking(data.Document);
         };

         // This event handler prepares marker data for the document to be cleaned
         closingHandler = (closingSender, closeData) =>
         {
            IssueMarkerTracking track = issueMarkerTrackingManager.GetTracking(closeData.Document);
            if(!closingDocumentIdToIssueTrackingPairs.ContainsKey(closeData.DocumentId) && !closeData.IsCancelled())
               closingDocumentIdToIssueTrackingPairs.Add(closeData.DocumentId, track.Id);
         };

         // This event handler cleans marker data after the document is closed
         closedHandler = (closedSender, closedData) =>
         {
            issueMarkerTrackingManager.DeleteTracking(closingDocumentIdToIssueTrackingPairs[closedData.DocumentId]);
            closingDocumentIdToIssueTrackingPairs.Remove(closedData.DocumentId);
         };
      }

      #endregion

      #region IExternalApplication Members

      /// <summary>
      /// Implements the OnShutdown event. It cleans up events and IssueMarkerTrackingManager
      /// </summary>
      /// <param name="application"></param>
      /// <returns></returns>
      public Result OnShutdown(UIControlledApplication application)
      {
         application.ControlledApplication.DocumentChanged -= updateHandler;
         application.ControlledApplication.DocumentOpened -= openHandler;
         application.ControlledApplication.DocumentCreated -= createHandler;
         application.ControlledApplication.DocumentClosing -= closingHandler;
         application.ControlledApplication.DocumentClosed -= closedHandler;

         IssueMarkerTrackingManager.GetInstance().ClearTrackings();

         return Result.Succeeded;
      }

      /// <summary>
      /// Implements the OnStartup event. It adds a server to listen for clicks on issue markers, events that manage issue marker data based on changes in document, 
      /// and a button that lets user create an issue marker.
      /// </summary>
      /// <param name="application"></param>
      /// <returns></returns>
      public Result OnStartup(UIControlledApplication application)
      {
         IssueSelectHandler click = new IssueSelectHandler();

         //This registers a service. On success, we register a button or event as well.
         Autodesk.Revit.DB.ExternalService.ExternalService service = Autodesk.Revit.DB.ExternalService.ExternalServiceRegistry.GetService(click.GetServiceId());
         if (service != null)
         {
            service.AddServer(click);
            (service as Autodesk.Revit.DB.ExternalService.MultiServerService).SetActiveServers(new List<Guid>() { click.GetServerId() });

            RibbonPanel ribbonPanel = application.GetRibbonPanels(Tab.AddIns).Find(x => x.Name == TabLabel);
            if (ribbonPanel == null)
               ribbonPanel = application.CreateRibbonPanel(Tab.AddIns, TabLabel);

            RibbonItemData ribbonItemData = new PushButtonData("Create marker", "Create issue marker on an element",
                System.Reflection.Assembly.GetExecutingAssembly().Location, typeof(Command).FullName);

            PushButton pushButton = (PushButton)ribbonPanel.AddItem(ribbonItemData);

            application.ControlledApplication.DocumentChanged += updateHandler;
            application.ControlledApplication.DocumentOpened += openHandler;
            application.ControlledApplication.DocumentCreated += createHandler;
            application.ControlledApplication.DocumentClosing += closingHandler;
            application.ControlledApplication.DocumentClosed += closedHandler;

            return Result.Succeeded;

         }
         return Result.Failed;
      }

      #endregion

      #region Class members

      private Dictionary<int, Guid> closingDocumentIdToIssueTrackingPairs = new Dictionary<int, Guid>();

      private EventHandler<Autodesk.Revit.DB.Events.DocumentChangedEventArgs> updateHandler;

      private EventHandler<Autodesk.Revit.DB.Events.DocumentOpenedEventArgs> openHandler;

      private EventHandler<Autodesk.Revit.DB.Events.DocumentCreatedEventArgs> createHandler;

      private EventHandler<Autodesk.Revit.DB.Events.DocumentClosedEventArgs> closedHandler;
      
      private EventHandler<Autodesk.Revit.DB.Events.DocumentClosingEventArgs> closingHandler;
     
      #endregion
   }

}