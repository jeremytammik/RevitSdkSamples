//
// (C) Copyright 2003-2017 by Autodesk, Inc. All rights reserved.
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

/*
 * This sample demonstrates how to use DirectContext3D to draw graphics. The graphics primitives that
 * are needed for DirectContext3D are taken from the contents of existing Revit elements. The result of
 * the macro is to duplicate the graphics of existing Revit elements by using DirectContext3D. No new
 * elements are created to contain the graphics produced with DirectContext3D.
 * 
 * The following are the main steps for using DirectContext3D:
 *  1) create a DirectContext3D server (derived from Autodesk.Revit.DB.DirectContext3D.IDirectContext3DServer)
 *     a. register the server with the DirectContext3D service
 *  2) use the server to submit geometry for drawing
 *     a. represent geometry primitives using pairs of vertex and index buffers
 *     b. determine when to submit opaque/transparent geometry
 *     c. flush the buffers
 *     d. update the buffers when necessary
 */


using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExternalService;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;

namespace Revit.SDK.Samples.DuplicateGraphics.CS
{
    /// <summary>
    /// Implements the Revit add-in interface IExternalApplication
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class Application : IExternalApplication
   {
      #region IExternalApplication Members

      /// <summary>
      /// Implements the OnStartup event
      /// </summary>
      /// <param name="application"></param>
      /// <returns>Result that indicates whether the external application has completed its work successfully.</returns>
      public Result OnStartup(UIControlledApplication application)
      {
         try
         {
            // Register events. 
            application.ControlledApplication.DocumentClosing += new EventHandler
                <Autodesk.Revit.DB.Events.DocumentClosingEventArgs>(OnDocumentClosing);
            m_servers = new List<RevitElementDrawingServer>();
            m_documents = new HashSet<Document>();

            s_applicationInstance = this;

         }
         catch (Exception)
         {
            return Result.Failed;
         }

         return Result.Succeeded;
      }

      /// <summary>
      /// Implements the OnShutdown event
      /// </summary>
      /// <param name="application"></param>
      /// <returns>Result that indicates whether the external application has completed its work successfully.</returns>
      public Result OnShutdown(UIControlledApplication application)
      {
         // remove the event.
         application.ControlledApplication.DocumentClosing -= OnDocumentClosing;
         return Result.Succeeded;
      }

      /// <summary>
      /// Implements the OnDocumentClosing event
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="args"></param>
      /// <returns></returns>
      public void OnDocumentClosing(object sender, DocumentClosingEventArgs args)
      {
         unregisterServers(args.Document, false);
      }

      #endregion

      /// <summary>
      /// Responds to the external command CommandDuplicateGraphics.
      /// </summary>
      /// <param name="document"></param>
      public static void ProcessCommandDuplicateGraphics(Document document)
      {
         if (s_applicationInstance != null)
         {
            s_applicationInstance.AddMultipleRevitElementServers(new UIDocument(document));
         }
      }

      /// <summary>
      /// Responds to the external command CommandClearExternalGraphics.
      /// </summary>
      /// <param name="document"></param>
      public static void ProcessCommandClearExternalGraphics(Document document)
      {
         if (s_applicationInstance != null)
         {
            s_applicationInstance.unregisterServers(null, true);
         }
      }

      /// <summary>
      /// Picks a Revit element and creates a corresponding DirectContext3D server that will draw the element's graphics at a fixed offset from the original location.
      /// </summary>
      /// <param name="uidoc"></param>
      public void AddRevitElementServer(UIDocument uidoc)
      {
         Reference reference = uidoc.Selection.PickObject(ObjectType.Element, "Select an element to duplicate with DirectContext3D");
         Element elem = uidoc.Document.GetElement(reference);

         // Create the server and register it with the DirectContext3D service.
         ExternalService directContext3DService = ExternalServiceRegistry.GetService(ExternalServices.BuiltInExternalServices.DirectContext3DService);
         RevitElementDrawingServer revitServer = new RevitElementDrawingServer(uidoc, elem, m_offset);
         directContext3DService.AddServer(revitServer);
         m_servers.Add(revitServer);

         MultiServerService msDirectContext3DService = directContext3DService as MultiServerService;

         IList<Guid> serverIds = msDirectContext3DService.GetActiveServerIds();

         serverIds.Add(revitServer.GetServerId());

         // Add the new server to the list of active servers.
         msDirectContext3DService.SetActiveServers(serverIds);

         m_documents.Add(uidoc.Document);
         uidoc.UpdateAllOpenViews();
      }

      /// <summary>
      /// Same as AddRevitElementServer(), but for multiple elements.
      /// </summary>
      /// <param name="uidoc"></param>
      public void AddMultipleRevitElementServers(UIDocument uidoc)
      {
         IList<Reference> references = uidoc.Selection.PickObjects(ObjectType.Element, "Select elements to duplicate with DirectContext3D");

         ExternalService directContext3DService = ExternalServiceRegistry.GetService(ExternalServices.BuiltInExternalServices.DirectContext3DService);
         MultiServerService msDirectContext3DService = directContext3DService as MultiServerService;
         IList<Guid> serverIds = msDirectContext3DService.GetActiveServerIds();

         // Create one server per element.
         foreach (Reference reference in references)
         {
            Element elem = uidoc.Document.GetElement(reference);

            RevitElementDrawingServer revitServer = new RevitElementDrawingServer(uidoc, elem, m_offset);
            directContext3DService.AddServer(revitServer);
            m_servers.Add(revitServer);

            serverIds.Add(revitServer.GetServerId());
         }

         msDirectContext3DService.SetActiveServers(serverIds);

         m_documents.Add(uidoc.Document);
         uidoc.UpdateAllOpenViews();
      }

      /// <summary>
      /// Cleans up by unregistering the servers corresponding to the specified document, or all servers if the document is not provided.
      /// </summary>
      /// <param name="document">The document whose servers should be removed, or null.</param>
      /// <param name="updateViews">Update views of the affected document(s).</param>
      public void unregisterServers(Document document, bool updateViews)
      {
         ExternalServiceId externalDrawerServiceId = ExternalServices.BuiltInExternalServices.DirectContext3DService;
         var externalDrawerService = ExternalServiceRegistry.GetService(externalDrawerServiceId) as MultiServerService;
         if (externalDrawerService == null)
            return;

         foreach (var registeredServerId in externalDrawerService.GetRegisteredServerIds())
         {
            var externalDrawServer = externalDrawerService.GetServer(registeredServerId) as RevitElementDrawingServer;
            if (externalDrawServer == null)
               continue;
            if (document != null && !document.Equals(externalDrawServer.Document))
               continue;
            externalDrawerService.RemoveServer(registeredServerId);
         }

         if (document != null)
         {
            m_servers.RemoveAll(server => document.Equals(server.Document));

            if (updateViews)
            {
               UIDocument uidoc = new UIDocument(document);
               uidoc.UpdateAllOpenViews();
            }

            m_documents.Remove(document);
         }
         else
         {
            m_servers.Clear();

            if (updateViews)
               foreach (var doc in m_documents)
               {
                  UIDocument uidoc = new UIDocument(doc);
                  uidoc.UpdateAllOpenViews();
               }

            m_documents.Clear();
         }
      }

      List<RevitElementDrawingServer> m_servers;
      XYZ m_offset = new XYZ(0, 0, 45);
      HashSet<Document> m_documents;
      static Application s_applicationInstance;
   }
}
