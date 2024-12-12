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
using Autodesk.Revit.UI.Selection;

namespace Revit.SDK.Samples.FabricationPartLayout.CS
{
   /// <summary>
   /// Implements the Revit add-in interface IExternalCommand
   /// </summary>
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   public class Ancillaries : IExternalCommand
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

         Document doc = commandData.Application.ActiveUIDocument.Document;
         UIDocument uidoc = commandData.Application.ActiveUIDocument;

         FabricationPart fabPart = null;
         FabricationConfiguration config = null;

         try
         {
            // check for a load fabrication config
            config = FabricationConfiguration.GetFabricationConfiguration(doc);

            if (config == null)
            {
               message = "No fabrication configuration loaded.";
               return Result.Failed;
            }

            // pick a fabrication part
            Reference refObj = uidoc.Selection.PickObject(ObjectType.Element, "Pick a fabrication part to start.");
            fabPart = doc.GetElement(refObj) as FabricationPart;
         }
         catch (Autodesk.Revit.Exceptions.OperationCanceledException)
         {
            return Result.Cancelled;
         }

         if (fabPart == null)
         {
            message = "The selected element is not a fabrication part.";
            return Result.Failed;
         }
         else
         {
            // get ancillary data from selected part and report to user
            IList<FabricationAncillaryUsage> ancillaries = fabPart.GetPartAncillaryUsage();
            List<string> ancillaryDescriptions = new List<string>();

            // create list of ancillary descriptions using the Ancillary UseageType and Name
            foreach (var ancillaryUsage in ancillaries)
            {
               FabricationAncillaryType ancilType = ancillaryUsage.Type;
               FabricationAncillaryUsageType usageType = ancillaryUsage.UsageType;
               ancillaryDescriptions.Add($"{ancilType.ToString()}: {usageType.ToString()} - "
                                + $"{config.GetAncillaryName(ancillaryUsage.AncillaryId)}");
            }

            string results = string.Empty;

            // group and quantify 
            if (ancillaryDescriptions.Count > 0)
            {
               ancillaryDescriptions.Sort();
               StringBuilder resultsBuilder = new StringBuilder();
               string currentAncillary = string.Empty;

               foreach (var ancillaryName in ancillaryDescriptions)
               {
                  if (ancillaryName != currentAncillary)
                  {
                     resultsBuilder.AppendLine($"{ancillaryName} x {ancillaryDescriptions.Count(x => x == ancillaryName)}");
                     currentAncillary = ancillaryName;
                  }
               }
               results = resultsBuilder.ToString();
            }

            TaskDialog td = new TaskDialog("Ancillaries")
            {
               MainIcon = TaskDialogIcon.TaskDialogIconInformation,
               TitleAutoPrefix = false,
               MainInstruction = ancillaryDescriptions.Count > 0 ?
                                   $"{ancillaryDescriptions.Count} ancillaries found on selected part"
                                 : $"No ancillaries found on selected part",
               MainContent = results
            };

            td.Show();
         }

         return Result.Succeeded;
      }
   }
}
