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


using Autodesk.Revit.UI.Selection;
using Autodesk.AdvanceSteel.CADAccess;
using Autodesk.Revit.DB.Steel;
using Autodesk.AdvanceSteel.Geometry;
using Autodesk.AdvanceSteel.Modelling;
using RvtDwgAddon;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;


namespace Revit.SDK.Samples.SampleCommandsSteelElements.CreateWeldPoint.CS
{
   /// <summary>
   /// Implements the Revit add-in interface IExternalCommand
   /// </summary>
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
   public class Command : IExternalCommand
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
         // Get the document from external command data.
         UIDocument activeDoc = commandData.Application.ActiveUIDocument;
         Autodesk.Revit.DB.Document doc = activeDoc.Document;

         if (null == doc)
         {
            return Result.Failed;
         }
         try
         {
            // Starting the transaction, using CYBORG's FabricationTransaction class
            using (FabricationTransaction trans = new FabricationTransaction(doc, false, "Create weld point"))
            {
               // for more details, please consult http://www.autodesk.com/adv-steel-api-walkthroughs-2019-enu
               Reference eRef = activeDoc.Selection.PickObject(ObjectType.Edge, "Pick an edge");

               Element elem = null;
               if (eRef != null && eRef.ElementId != ElementId.InvalidElementId)
               {
                  elem = (doc.GetElement(eRef.ElementId));
               }

               if (null == elem)
               {
                  return Result.Failed;
               }

               // adding fabrication data, if the element doesn't already have it.
               SteelElementProperties cell = SteelElementProperties.GetSteelElementProperties(elem);
               if (null == cell)
               {
                  List<ElementId> elemsIds = new List<ElementId>();
                  elemsIds.Add(elem.Id);
                  SteelElementProperties.AddFabricationInformationForRevitElements(doc, elemsIds);
               }

               FilerObject filerObj = Utilities.Functions.GetFilerObject(doc, eRef);

               if (null == filerObj)
               {
                  return Result.Failed;
               }

               // Point of reference for the weld point. We use GlobalPoint to create the weld point on the selected edge. GlobalPoint is the point where the edge is being hit when selected.
               Point3d p1 = new Point3d(eRef.GlobalPoint.X, eRef.GlobalPoint.Y, eRef.GlobalPoint.Z);
               // Creating a weld point, using AdvanceSteel's WeldPoint class
               Vector3d vX = new Vector3d(100, 100, 100);
               Vector3d vY = new Vector3d(100, 100, 100);
               WeldPoint weld = new WeldPoint(p1 * Utilities.Functions.FEET_TO_MM, vX, vY);

               if (null != weld)
               {
                  weld.WriteToDb();
                  HashSet<FilerObject> objsToConnect = new HashSet<FilerObject>();
                  objsToConnect.Add(filerObj);
                  weld.Connect(objsToConnect, Autodesk.AdvanceSteel.ConstructionTypes.AtomicElement.eAssemblyLocation.kInShop);
               }
            }
         }

         catch (Autodesk.Revit.Exceptions.OperationCanceledException)
         {
            return Result.Cancelled;
         }
         return Result.Succeeded;
      }
   }
}

