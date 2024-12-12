//
// (C) Copyright 2003-2017 by Autodesk, Inc.
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

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Fabrication;

namespace Revit.SDK.Samples.FabricationPartLayout.CS
{
   /// <summary>
   /// Implements the Revit add-in interface IExternalCommand
   /// </summary>
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   public class StretchAndFit : IExternalCommand
   {
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
      public virtual Result Execute(ExternalCommandData commandData
          , ref string message, ElementSet elements)
      {
         try
         {
            Document doc = commandData.Application.ActiveUIDocument.Document;

            // check user selection
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            ICollection<ElementId> collection = uidoc.Selection.GetElementIds();
            if (collection.Count > 0)
            {
               List<ElementId> selIds = new List<ElementId>();
               foreach (ElementId id in collection)
                  selIds.Add(id);

               if (selIds.Count != 2)
               {
                  message = "Select a fabrication part to stretch and fit from and an element to connect to.";
                  return Result.Cancelled;
               }

               Connector connFrom = GetValidConnectorToStretchAndFitFrom(doc, selIds.ElementAt(0));
               Connector connTo = GetValidConnectorToStretchAndFitTo(doc, selIds.ElementAt(1));

               FabricationPartRouteEnd toEnd = FabricationPartRouteEnd.CreateFromConnector(connTo);

               if (connFrom == null || connTo == null)
               {
                  message = "Invalid fabrication parts to stretch and fit";
                  return Result.Cancelled;
               }

               using (Transaction tr = new Transaction(doc, "Stretch and Fit"))
               {
                  tr.Start();

                  ISet<ElementId> newPartIds;
                  FabricationPartFitResult result = FabricationPart.StretchAndFit(doc, connFrom, toEnd, out newPartIds);
                  if (result != FabricationPartFitResult.Success)
                  {
                     message = result.ToString();
                     return Result.Failed;
                  }

                  doc.Regenerate();

                  tr.Commit();
               }

               return Result.Succeeded;
            }
            else
            {
               // inform user they need to select at least one element
               message = "Select a fabrication part to stretch and fit from and an element to connect to.";
            }

            return Result.Failed;
         }
         catch (Exception ex)
         {
            message = ex.Message;
            return Result.Failed;
         }
      }

      private Connector GetValidConnectorToStretchAndFitFrom(Document doc, ElementId elementId)
      {
         // must be a fabrication part
         FabricationPart part = doc.GetElement(elementId) as FabricationPart;
         if (part == null)
            return null;

         // must not be a straight, hanger or tap
         if (part.IsAStraight() || part.IsATap() || part.IsAHanger())
            return null;

         // part must be connected at one end and have one unoccupied connector
         int numUnused = part.ConnectorManager.UnusedConnectors.Size;
         int numConns = part.ConnectorManager.Connectors.Size;

         if (numConns - numUnused != 1)
            return null;

         foreach (Connector conn in part.ConnectorManager.UnusedConnectors)
         {
            // return the first unoccupied connector
            return conn;
         }

         return null;
      }

      private Connector GetValidConnectorToStretchAndFitTo(Document doc, ElementId elementId)
      {
         // connect to another fabrication part - will work also with families.
         FabricationPart part = doc.GetElement(elementId) as FabricationPart;
         if (part == null)
            return null;

         // must not be a fabrication part hanger
         if (part.IsAHanger())
            return null;

         foreach (Connector conn in part.ConnectorManager.UnusedConnectors)
         {
            // return the first unoccupied connector
            return conn;
         }

         return null;
      }
   }
}
