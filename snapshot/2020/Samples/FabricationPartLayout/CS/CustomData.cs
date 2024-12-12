//
// (C) Copyright 2003-2019 by Autodesk, Inc.
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
   /// Helper class to report custom data from fabrication part selection.
   /// </summary>
   public class CustomDataHelper
   {
      /// <summary>
      /// Report the custom data.
      /// </summary>
      /// <param name="doc"></param>
      /// <param name="uiDoc"></param>
      /// <param name="setNewValues"></param>
      /// <param name="message"></param>
      /// <returns></returns>
      public static Result ReportCustomData(Document doc, UIDocument uiDoc, bool setNewValues, ref string message)
      {
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
            Reference refObj = uiDoc.Selection.PickObject(ObjectType.Element, "Pick a fabrication part to start.");
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
            // get custom data from loaded fabrication config
            IList<int> customDataIds = config.GetAllPartCustomData();
            int customDataCount = customDataIds.Count;
            string results = string.Empty;

            // report custom data info
            if (customDataCount > 0)
            {
               StringBuilder resultsBuilder = new StringBuilder();
               resultsBuilder.AppendLine($"Fabrication config contains {customDataCount} custom data entries {Environment.NewLine}");

               foreach (var customDataId in customDataIds)
               {
                  FabricationCustomDataType customDataType = config.GetPartCustomDataType(customDataId);
                  string customDataName = config.GetPartCustomDataName(customDataId);

                  resultsBuilder.AppendLine($"Type: {customDataType.ToString()} Name: {customDataName}");
                  // check custom data exists on selected part
                  if (fabPart.HasCustomData(customDataId))
                  {
                     string fabPartCurrentValue = string.Empty;
                     string fabPartNewValue = string.Empty;

                     switch (customDataType)
                     {
                        case FabricationCustomDataType.Text:

                           fabPartCurrentValue = $"\"{fabPart.GetPartCustomDataText(customDataId)}\"";

                           if (setNewValues)
                           {
                              string installDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                              fabPart.SetPartCustomDataText(customDataId, installDateTime);
                              fabPartNewValue = installDateTime;
                           }

                           break;
                        case FabricationCustomDataType.Integer:

                           fabPartCurrentValue = fabPart.GetPartCustomDataInteger(customDataId).ToString();

                           if (setNewValues)
                           {
                              int installHours = new Random().Next(1, 10);
                              fabPart.SetPartCustomDataInteger(customDataId, installHours);
                              fabPartNewValue = installHours.ToString();
                           }

                           break;
                        case FabricationCustomDataType.Real:

                           fabPartCurrentValue = $"{fabPart.GetPartCustomDataReal(customDataId):0.##}";

                           if (setNewValues)
                           {
                              double installCost = new Random().NextDouble() * new Random().Next(100, 1000);
                              fabPart.SetPartCustomDataReal(customDataId, installCost);
                              fabPartNewValue = $"{installCost:0.##}";
                           }

                           break;
                     }

                     resultsBuilder.AppendLine("Current custom data entry value = "
                                             + $"{fabPartCurrentValue} {Environment.NewLine}");

                     if (setNewValues)
                     {
                        resultsBuilder.AppendLine("New custom data entry value = "
                                             + $"{fabPartNewValue} {Environment.NewLine}");
                     }
                  }
                  else
                  {
                     resultsBuilder.AppendLine($"Custom data entry is not set on the part {Environment.NewLine}");
                  }
               }
               results = resultsBuilder.ToString();
            }

            TaskDialog td = new TaskDialog("Custom Data")
            {
               MainIcon = TaskDialogIcon.TaskDialogIconInformation,
               TitleAutoPrefix = false,
               MainInstruction = $"{customDataCount} custom data entries found in the loaded fabrication config",
               MainContent = results
            };

            td.Show();
         }

         return Result.Succeeded;
      }
   }
   /// <summary>
   /// Implements the Revit add-in interface IExternalCommand
   /// </summary>
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   public class GetCustomData : IExternalCommand
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

         return CustomDataHelper.ReportCustomData(doc, uidoc, false, ref message);
      }
   }

   /// <summary>
   /// Implements the Revit add-in interface IExternalCommand
   /// </summary>
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   public class SetCustomData : IExternalCommand
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
         Result result;

         using (Transaction tr = new Transaction(doc, "Setting Custom Data"))
         {
            tr.Start();
            result = CustomDataHelper.ReportCustomData(doc, uidoc, true, ref message);

            if (result == Result.Succeeded)
            {
               tr.Commit();
            }
            else
            {
               tr.RollBack();
            }
         }

         return result;
      }
   }
}
