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
using System.IO;
using System.Reflection;

namespace Revit.SDK.Samples.FabricationPartLayout.CS
{
   /// <summary>
   /// Implements the Revit add-in interface IExternalCommand
   /// </summary>
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   public class ExportToMAJ : IExternalCommand
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
            // check user selection
            var uidoc = commandData.Application.ActiveUIDocument;
            var doc = uidoc.Document;
            var elementIds = new HashSet<ElementId>();
            uidoc.Selection.GetElementIds().ToList().ForEach( x => elementIds.Add(x) );

            var hasFabricationParts = false;
            foreach (var elementId in elementIds)
            {
               var part = doc.GetElement(elementId) as FabricationPart;
               if (part != null)
               {
                  hasFabricationParts = true;
                  break;
               }
            }

            if (hasFabricationParts == false)
            {
               message = "Select at least one fabrication part";
               return Result.Failed;
            }

            var callingFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var saveAsDlg = new FileSaveDialog("MAJ Files (*.maj)|*.maj");
            saveAsDlg.InitialFileName = callingFolder + "\\majExport";
            saveAsDlg.Title = "Export To MAJ";
            var result = saveAsDlg.Show();

            if (result == ItemSelectionDialogResult.Canceled)
               return Result.Cancelled;

            string filename = ModelPathUtils.ConvertModelPathToUserVisiblePath(saveAsDlg.GetSelectedModelPath());

            ISet<ElementId> exported = FabricationPart.SaveAsFabricationJob(doc, elementIds, filename, new FabricationSaveJobOptions(true));
            if (exported.Count > 0)
            {
               TaskDialog td = new TaskDialog("Export to MAJ")
               {
                  MainIcon = TaskDialogIcon.TaskDialogIconInformation,
                  TitleAutoPrefix = false,
                  MainInstruction = string.Concat("Export to MAJ was successful - ", exported.Count.ToString(), " Parts written"),
                  MainContent = filename,
                  AllowCancellation = false,
                  CommonButtons = TaskDialogCommonButtons.Ok
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
