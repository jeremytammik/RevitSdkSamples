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
   public class ConvertToFabrication : IExternalCommand
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
               // DesignToFabrication needs an ISet<ElementId>
               ISet<ElementId> selIds = new HashSet<ElementId>();
               foreach (ElementId id in collection)
               {
                  selIds.Add(id);
               }

               using (Transaction tr = new Transaction(doc, "Convert To Fabrication Parts"))
               {
                  tr.Start();

                  FabricationConfiguration config = FabricationConfiguration.GetFabricationConfiguration(doc);

                  // get all loaded fabrication services and attempt to convert the design elements 
                  // to the first loaded service
                  IList<FabricationService> allLoadedServices = config.GetAllLoadedServices();

                  DesignToFabricationConverter converter = new DesignToFabricationConverter(doc);
                  DesignToFabricationConverterResult result = converter.Convert(selIds, allLoadedServices[0].ServiceId);
                  
                  if (result != DesignToFabricationConverterResult.Success)
                  {
                     message = "There was a problem with the conversion.";
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
