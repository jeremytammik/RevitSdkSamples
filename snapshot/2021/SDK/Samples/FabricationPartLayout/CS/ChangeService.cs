//
// (C) Copyright 2003-2011 by Autodesk, Inc.
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
   public class ChangeService : IExternalCommand
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

            // get the user selection
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            ICollection<ElementId> collection = uidoc.Selection.GetElementIds();

            if (collection.Count > 0)
            {
               // FabricationNetworkChangeService needs an ISet<ElementId>
               ISet<ElementId> selIds = new HashSet<ElementId>();
               foreach (ElementId id in collection)
               {
                  selIds.Add(id);
               }

               using (Transaction tr = new Transaction(doc, "Change Service of Fabrication Parts"))
               {
                  tr.Start();

                  FabricationConfiguration config = FabricationConfiguration.GetFabricationConfiguration(doc);

                  // Get all loaded fabrication services
                  IList<FabricationService> allLoadedServices = config.GetAllLoadedServices();

                  FabricationNetworkChangeService changeservice = new FabricationNetworkChangeService(doc);

                  // Change the fabrication parts to the first loaded service and group
                  FabricationNetworkChangeServiceResult result = changeservice.ChangeService(selIds, allLoadedServices[0].ServiceId, 0);

                  if (result != FabricationNetworkChangeServiceResult.Success)
                  {
                     message = "There was a problem with the change service.";
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
               message = "Please select at least one element.";
            }

            return Result.Failed;
         }
         catch (Exception ex)
         {
            message = ex.Message;
            return Result.Failed;
         }
      }
   }

   /// <summary>
   /// Implements the Revit add-in interface IExternalCommand
   /// </summary>
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   public class ChangeSize : IExternalCommand
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

            // get the user selection
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            ICollection<ElementId> collection = uidoc.Selection.GetElementIds();

            if (collection.Count > 0)
            {
               // FabricationNetworkChangeService needs an ISet<ElementId>
               ISet<ElementId> selIds = new HashSet<ElementId>();
               foreach (ElementId id in collection)
               {
                  selIds.Add(id);
               }

               using (Transaction tr = new Transaction(doc, "Change Size of Fabrication Parts"))
               {
                  tr.Start();

                  FabricationConfiguration config = FabricationConfiguration.GetFabricationConfiguration(doc);

                  // Get all loaded fabrication services
                  IList<FabricationService> allLoadedServices = config.GetAllLoadedServices();

                  // Create a map of sizes to swap the current sizes to a new size
                  var sizeMappings = new HashSet<Autodesk.Revit.DB.Fabrication.FabricationPartSizeMap>();
                  var mapping = new Autodesk.Revit.DB.Fabrication.FabricationPartSizeMap("12x12", 1.0, 1.0, false, ConnectorProfileType.Rectangular, allLoadedServices[0].ServiceId, 0 );
                  mapping.MappedWidthDiameter = 1.5;
                  mapping.MappedDepth = 1.5;
                  sizeMappings.Add( mapping );
                  var mapping1 = new Autodesk.Revit.DB.Fabrication.FabricationPartSizeMap("18x18", 1.5, 1.5, false, ConnectorProfileType.Rectangular, allLoadedServices[0].ServiceId, 0 );
                  mapping1.MappedWidthDiameter = 2.0;
                  mapping1.MappedDepth = 2.0;
                  sizeMappings.Add( mapping1 );

                  FabricationNetworkChangeService changesize = new FabricationNetworkChangeService(doc);

                  // Change the size of the fabrication parts in the selection to the new sizes
                  FabricationNetworkChangeServiceResult result = changesize.ChangeSize(selIds, sizeMappings );

                  if (result != FabricationNetworkChangeServiceResult.Success)
                  {
                     // Get the collection of element identifiers for parts that had errors posted against them
                     ICollection<ElementId> errorIds = changesize.GetElementsThatFailed();
                     if ( errorIds.Count > 0 )
                     {
                        message = "There was a problem with the change size.";
                        return Result.Failed;
                     }
                  }

                  doc.Regenerate();

                  tr.Commit();
               }

               return Result.Succeeded;
            }
            else
            {
               // inform user they need to select at least one element
               message = "Please select at least one element.";
            }

            return Result.Failed;
         }
         catch (Exception ex)
         {
            message = ex.Message;
            return Result.Failed;
         }
      }
   }
   
   /// <summary>
   /// Implements the Revit add-in interface IExternalCommand
   /// </summary>
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   public class ApplyChange : IExternalCommand
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

            // get the user selection
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            ICollection<ElementId> collection = uidoc.Selection.GetElementIds();

            if (collection.Count > 0)
            {
               // FabricationNetworkChangeService needs an ISet<ElementId>
               ISet<ElementId> selIds = new HashSet<ElementId>();
               foreach (ElementId id in collection)
               {
                  selIds.Add(id);
               }

               using (Transaction tr = new Transaction(doc, "Appply Change Service and Size of Fabrication Parts"))
               {
                  tr.Start();

                  FabricationConfiguration config = FabricationConfiguration.GetFabricationConfiguration(doc);

                  // Get all loaded fabrication services
                  IList<FabricationService> allLoadedServices = config.GetAllLoadedServices();

                  FabricationNetworkChangeService applychange = new FabricationNetworkChangeService(doc);

                  // Set the selection of element identifiers to be changed
                  applychange.SetSelection( selIds );
                  // Set the service to the second service in the list (ductwork exhaust service)
                  applychange.SetServiceId( allLoadedServices[1].ServiceId );
                  // Set the group to the second in the list (round)
                  applychange.SetGroupId( 1 );

                  // Get the sizes of all the straights that was in the selection of elements that was added to FabricationNetworkChangeService
                  ISet<Autodesk.Revit.DB.Fabrication.FabricationPartSizeMap> sizeMappings = applychange.GetMapOfAllSizesForStraights();
                  foreach (Autodesk.Revit.DB.Fabrication.FabricationPartSizeMap sizemapping in sizeMappings)
                  {
                     if (sizemapping != null)
                     {
                        // Testing round so ignoring the depth and adding 6" to the current size so all straights will be updated to a new size
                        var widthDia = sizemapping.WidthDiameter + 0.5;
                        sizemapping.MappedWidthDiameter = widthDia;
                     }
                  }
                  applychange.SetMapOfSizesForStraights( sizeMappings );

                  // Get the in-line element type identiers
                  var inlineRevIds = new HashSet<Autodesk.Revit.DB.ElementId>();
                  ISet<Autodesk.Revit.DB.ElementId> inlineIds = applychange.GetInLinePartTypes();
                  for ( var ii = inlineIds.Count() - 1; ii > -1; ii-- )
                  {
                     var elemId = inlineIds.ElementAt( ii );
                     if (elemId != null)
                        inlineRevIds.Add(elemId);
                  }
                  // Set the in-line element type identiers by swapping them out by reversing the order to keep it simple but still exercise the code
                  IDictionary<ElementId, ElementId> swapinlineIds = new Dictionary<ElementId, ElementId>();
                  for ( var ii = inlineIds.Count() - 1; ii > -1; ii-- )
                  {
                     var elemId = inlineIds.ElementAt( ii );
                     var elemIdother = inlineRevIds.ElementAt( ii );
                     if ((elemId != null) && (elemId != null))
                        swapinlineIds.Add(elemId, elemIdother);
                  }
                  applychange.SetMapOfInLinePartTypes( swapinlineIds );

                  // Apply the changes
                  FabricationNetworkChangeServiceResult result = applychange.ApplyChange();

                  if (result != FabricationNetworkChangeServiceResult.Success)
                  {
                     message = "There was a problem with the apply change.";
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
               message = "Please select at least one element.";
            }

            return Result.Failed;
         }
         catch (Exception ex)
         {
            message = ex.Message;
            return Result.Failed;
         }
      }
   }
}
