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

namespace Revit.SDK.Samples.FabricationPartLayout.CS
{
   /// <summary>
   /// Implements the Revit add-in interface IExternalCommand
   /// </summary>
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   public class ButtonPaletteExclusions : IExternalCommand
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

            using (Transaction tr = new Transaction(doc, "Set button and palette exclusions"))
            {
               tr.Start();

               FabricationConfiguration config = FabricationConfiguration.GetFabricationConfiguration(doc);

               if (config == null)
               {
                  message = "No fabrication configuration loaded.";
                  return Result.Failed;
               }

               // get all loaded fabrication services
               IList<FabricationService> allLoadedServices = config.GetAllLoadedServices();
               // get the "ADSK - HVAC:Supply Air" service
               string serviceName = "ADSK - HVAC: Supply Air";
               FabricationService selectedService = allLoadedServices.FirstOrDefault(x => x.Name == serviceName);

               if (selectedService == null)
               {
                  message = $"Could not find fabrication service {serviceName}";
                  return Result.Failed;
               }

               string rectangularPaletteName = "Rectangular";
               string roundPaletteName = "Round Bought Out";
               string excludeButtonName = "Square Bend";

               int rectangularPaletteIndex = -1;
               int roundPaletteIndex = -1;

               // find Rectangular and Round palettes in service
               for (int i = 0; i < selectedService.PaletteCount; i++)
               {
                  if (selectedService.GetPaletteName(i) == rectangularPaletteName)
                  {
                     rectangularPaletteIndex = i;
                  }

                  if (selectedService.GetPaletteName(i) == roundPaletteName)
                  {
                     roundPaletteIndex = i;
                  }

                  if (rectangularPaletteIndex > -1 && roundPaletteIndex > -1)
                  {
                     break;
                  }
               }

               if (rectangularPaletteIndex > -1)
               {
                  // exclude square bend in Rectangular palette
                  for (int i = 0; i < selectedService.GetButtonCount(rectangularPaletteIndex); i++)
                  {
                     if (selectedService.GetButton(rectangularPaletteIndex, i).Name == excludeButtonName)
                     {
                        selectedService.OverrideServiceButtonExclusion(rectangularPaletteIndex, i, true);
                        break;
                     }
                  }
               }
               else
               {
                  message = $"Unable to locate {excludeButtonName} button to exclude.";
                  return Result.Failed;
               }

               // exclude entire Round Bought Out service palette
               if (roundPaletteIndex > -1)
               {
                  selectedService.SetServicePaletteExclusions(new List<int>() { roundPaletteIndex });
               }
               else
               {
                  message = $"Unable to locate {roundPaletteName} service palette to exclude.";
                  return Result.Failed;
               }

               tr.Commit();

               TaskDialog td = new TaskDialog("Button and Palette Exclsuions")
               {
                  MainIcon = TaskDialogIcon.TaskDialogIconInformation,
                  TitleAutoPrefix = false,
                  MainInstruction = "Operation Successful",
                  MainContent = $"Excluded {excludeButtonName} button from {serviceName} {rectangularPaletteName} Palette {Environment.NewLine}"
                                 + $"Excluded {roundPaletteName} Palette from {serviceName}"
               };

               td.Show();
            }


            return Result.Succeeded;
         }
         catch (Exception ex)
         {
            message = ex.Message;
            return Result.Failed;
         }
      }
   }
}
