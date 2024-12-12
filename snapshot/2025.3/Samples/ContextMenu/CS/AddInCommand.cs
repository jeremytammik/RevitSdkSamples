//
// (C) Copyright 2003-2022 by Autodesk, Inc.
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
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace Revit.SDK.Samples.ContextMenu.CS
{
   /// <summary>
   /// Implements the Revit add-in interface IExternalCommand, show the user selection.
   /// Implements the Revit add-in interface IExternalCommandAvailability, enable/disable the command.
   /// </summary>
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]

   public class ShowSelection : IExternalCommand, IExternalCommandAvailability
   {
      public Result Execute(ExternalCommandData revit,
                                             ref string message,
                                             ElementSet elements)
      {
         try
         {
            Selection selection = revit.Application.ActiveUIDocument.Selection;
            System.Collections.Generic.ICollection<ElementId> elementIds = selection.GetElementIds();
            if (elementIds.Count == 0)
            {
               TaskDialog.Show("Revit", "No Element Selected");
            }
            else
            {
               string info = "Selected Element Id: ";
               foreach (ElementId elemId in elementIds)
               {
                  info += "\n\t" + elemId.ToString();
               }

               TaskDialog.Show("Revit", info);
            }
         }
         catch (Exception e)
         {
            message = e.Message;
            return Result.Failed;
         }

         return Result.Succeeded;
      }

      public bool IsCommandAvailable(UIApplication applicationData, CategorySet selectedCategories)
      {
         return true;
      }
   }
}
