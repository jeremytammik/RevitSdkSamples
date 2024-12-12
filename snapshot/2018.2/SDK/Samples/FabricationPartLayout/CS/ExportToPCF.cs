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
using System.Windows.Forms;
using System.IO;
using System.Reflection;

namespace Revit.SDK.Samples.FabricationPartLayout.CS
{
   /// <summary>
   /// Implements the Revit add-in interface IExternalCommand
   /// </summary>
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   public class ExportToPCF : IExternalCommand
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
            var collection = uidoc.Selection.GetElementIds();
            var hasFabricationPart = false;

            using (var trans = new Transaction(doc, "Change Spool Name"))
            {
               trans.Start();

               foreach (var elementId in collection)
               {
                  var part = doc.GetElement(elementId) as FabricationPart;
                  if (part != null)
                  {
                     hasFabricationPart = true;
                     part.SpoolName = "My Spool";
                  }
               }

               trans.Commit();
            }


            if (hasFabricationPart == false)
            {
               message = "Select at least one fabrication part";
               return Result.Failed;
            }

            var callingFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var saveAsDlg = new FileSaveDialog("PCF Files (*.pcf)|*.pcf");

            saveAsDlg.InitialFileName = callingFolder + "\\pcfExport";
            saveAsDlg.Title = "Export To PCF";
            var result = saveAsDlg.Show();

            if (result == ItemSelectionDialogResult.Canceled)
               return Result.Cancelled;

            var fabParts = collection.ToList();
            var model = saveAsDlg.GetSelectedModelPath();

            string filename = ModelPathUtils.ConvertModelPathToUserVisiblePath(saveAsDlg.GetSelectedModelPath());

            FabricationUtils.ExportToPCF(doc, fabParts, filename);

            TaskDialog td = new TaskDialog("Export to PCF")
            {
               MainIcon = TaskDialogIcon.TaskDialogIconInformation,
               TitleAutoPrefix = false,
               MainInstruction = "Export to PCF was successful",
               MainContent = filename,
               AllowCancellation = false,
               CommonButtons = TaskDialogCommonButtons.Ok
            };

            td.Show();

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
