//
// (C) Copyright 2003-2021 by Autodesk, Inc.
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

using Autodesk.Revit;
using Autodesk.Revit.DB.Events;
using System.Windows.Forms;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.ApplicationServices;
using Autodesk.RevitAddIns;
using Autodesk.Revit.UI.Events;
using System.Linq;

namespace Revit.SDK.Samples.SelectionChanged.CS
{
   /// <summary>
   /// This class is an external application which monitors when the selection is changed.
   /// </summary>
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
   public class SelectionChanged : IExternalApplication
   {
      #region Class Member Variables
      /// <summary>
      /// A controlled application used to register the events. Because all trigger points
      /// in this sample come from UI, all events in application level must be registered 
      /// to ControlledApplication. If the trigger point is from API, user can register it 
      /// to Application or Document according to what level it is in. But then, 
      /// the syntax is the same in these three cases.
      /// </summary>
      private static UIControlledApplication m_ctrlApp;
      /// <summary>
      /// The window is used to show SelectionChanged event information.
      /// </summary>
      private static InfoWindow m_infoWindow;

      static string AddInPath = typeof(SelectionChanged).Assembly.Location;
      #endregion

      #region Class Static Property
      /// <summary>
      /// Property to get and set private member variable of InfoWindows
      /// </summary>
      public static InfoWindow InfoWindow
      {
         get;
         set;         
      }
      #endregion

      #region IExternalApplication Members

      /// <summary>
      /// Implement OnStartup method of IExternalApplication interface.
      /// This method subscribes to SelectionChanged event.        
      /// </summary>
      /// <param name="application">Controlled application to be loaded to Revit process.</param>
      /// <returns>The status of the external application</returns>
      public Autodesk.Revit.UI.Result OnStartup(UIControlledApplication application)
      {
         m_ctrlApp = application;
         
         RibbonPanel ribbonPanel = m_ctrlApp.CreateRibbonPanel("SelectionChanged Event");
         PushButtonData showInfoWindowButton = new PushButtonData("showInfoWindow", "Show Event Info", AddInPath, "Revit.SDK.Samples.SelectionChanged.CS.Command");
         showInfoWindowButton.ToolTip = "Show Event Monitor window";
         ribbonPanel.AddItem(showInfoWindowButton);
         

         // subscribe to SelectionChanged event
         application.SelectionChanged += new EventHandler<SelectionChangedEventArgs>(SelectionChangedHandler);

         return Autodesk.Revit.UI.Result.Succeeded;
      }

      /// <summary>
      /// Implement OnShutdown method of IExternalApplication interface. 
      /// This method unsubscribes from SelectionChanged event
      /// </summary>
      /// <param name="application">Controlled application to be shutdown.</param>
      /// <returns>The status of the external application.</returns>
      public Autodesk.Revit.UI.Result OnShutdown(UIControlledApplication application)
      {
         // unsubscribe to SelectionChanged event
         application.SelectionChanged -= new EventHandler<SelectionChangedEventArgs>(SelectionChangedHandler);

         // finalize the log file.
         LogManager.LogFinalize();

         if (m_infoWindow != null)
         {
            m_infoWindow.Close();
            m_infoWindow = null;
         }

         return Autodesk.Revit.UI.Result.Succeeded;
      }

      #endregion

      #region EventHandler

      /// <summary>
      /// Event handler method for SelectionChanged event.
      /// This method will check that the selection reported by event is the same with the actual Revit selection 
      /// and print the current selection in the log file.
      /// </summary>
      /// <param name="sender">The source of the event.</param>
      /// <param name="args">Event arguments that contains the event data.</param>
      private void SelectionChangedHandler(Object sender, SelectionChangedEventArgs args)
      {
         // The document associated with the event. 
         Document doc = args.GetDocument();
         
         //Check if Revit selection matches the selection reported by the event
         UIDocument uidoc = new UIDocument(doc);
         IList<Reference> currentSelection = uidoc.Selection.GetReferences();
         IList<Reference> reportedSelection = args.GetReferences();
         if (!currentSelection.All(i => null != reportedSelection.FirstOrDefault(r => r.EqualTo(i))))
         {
            LogManager.WriteLog("Error: Current selection is different from the selection reported by the selectionchanged event");
         }

         var currentSelectedElementIds = uidoc.Selection.GetElementIds();
         var reportedSelectedElementIds = args.GetSelectedElements();
         if (!currentSelectedElementIds.All(i => null != reportedSelectedElementIds.FirstOrDefault(r => r == i)))
         {
            LogManager.WriteLog("Error: Current selected ElementIds is different from the one reported by the selectionchanged event");
         }

         // write to log file. 
         LogManager.WriteLog(args.GetInfo());

         if(InfoWindow != null)
         {
            InfoWindow.RevitUIApp_SelectionChanged(sender, args);
         }

      }

      #endregion
   }
}
