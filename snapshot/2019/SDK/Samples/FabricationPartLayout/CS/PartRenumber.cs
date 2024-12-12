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

namespace Revit.SDK.Samples.FabricationPartLayout.CS
{
   /// <summary>
   /// Implements the Revit add-in interface IExternalCommand
   /// </summary>
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   public class PartRenumber : IExternalCommand
   {
      #region region Member Variables
      private int m_ductNum = 1;
      private int m_ductCouplingNum = 1;
      private int m_pipeNum = 1;
      private int m_pipeCouplingNum = 1;
      private int m_hangerNum = 1;
      private int m_otherNum = 1;
      #endregion

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

            using (var trans = new Transaction(doc, "Part Renumber"))
            {
               trans.Start();

               var fabParts = new List<FabricationPart>();
               foreach (var elementId in collection)
               {
                  var part = doc.GetElement(elementId) as FabricationPart;
                  if (part != null)
                  {
                     part.ItemNumber = string.Empty; // wipe the item number
                     fabParts.Add(part);
                  }
               }

               if (fabParts.Count == 0)
               {
                  message = "Select at least one fabrication part";
                  return Result.Failed;
               }

               // ignore certain fields
               var ignoreFields = new List<FabricationPartCompareType>();
               ignoreFields.Add(FabricationPartCompareType.Notes);
               ignoreFields.Add(FabricationPartCompareType.OrderNo);
               ignoreFields.Add(FabricationPartCompareType.Service);

               for (int i = 0; i < fabParts.Count; i++)
               {
                  var part1 = fabParts[i];
                  if (string.IsNullOrWhiteSpace(part1.ItemNumber))
                  {
                     // part has not already been checked
                     if (IsADuct(part1))
                     {
                        if (IsACoupling(part1))
                           part1.ItemNumber = "DUCT COUPLING: " + m_ductCouplingNum++;
                        else
                           part1.ItemNumber = "DUCT: " + m_ductNum++;
                     }
                     else if (IsAPipe(part1))
                     {
                        if (IsACoupling(part1))
                           part1.ItemNumber = "PIPE COUPLING: " + m_pipeCouplingNum++;
                        else
                           part1.ItemNumber = "PIPE: " + m_pipeNum++;
                     }
                     else if (part1.IsAHanger())
                        part1.ItemNumber = "HANGER: " + m_hangerNum++;
                     else
                        part1.ItemNumber = "MISC: " + m_otherNum++;

                  }

                  for (int j = i + 1; j < fabParts.Count; j++)
                  {
                     var part2 = fabParts[j];
                     if (string.IsNullOrWhiteSpace(part2.ItemNumber))
                     {
                        // part2 has not been checked
                        if (part1.IsSameAs(part2, ignoreFields))
                        {
                           // items are the same, so give them the same item number
                           part2.ItemNumber = part1.ItemNumber;
                        }
                     }
                  }
               }

               trans.Commit();
            }

            TaskDialog td = new TaskDialog("Fabrication Part Renumber")
            {
               MainIcon = TaskDialogIcon.TaskDialogIconInformation,
               TitleAutoPrefix = false,
               MainInstruction = "Renumber was successful",
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

      /// <summary>
      /// Checks if the given part is fabrication ductwork.
      /// </summary>
      /// <param name="fabPart">The part to check.</param>
      /// <returns>True if the part is fabrication ductwork.</returns>
      private bool IsADuct(FabricationPart fabPart)
      {
         return (fabPart != null && (fabPart.Category.Id.IntegerValue == (int)BuiltInCategory.OST_FabricationDuctwork));
      }

      /// <summary>
      /// Checks if the part is fabrication pipework.
      /// </summary>
      /// <param name="fabPart">The part to check.</param>
      /// <returns>True if the part is fabrication pipework.</returns>
      private bool IsAPipe(FabricationPart fabPart)
      {
         return (fabPart != null && (fabPart.Category.Id.IntegerValue == (int)BuiltInCategory.OST_FabricationPipework));
      }

      /// <summary>
      /// Checks if the part is a coupling. 
      /// The CID's (the fabrication part item customer Id) that are recognized internally as couplings are:
      ///   CID 522, 1112 - Round Ductwork
      ///   CID 1522 - Oval Ductwork
      ///   CID 4522 - Rectangular Ductwork
      ///   CID 2522 - Pipe Work
      ///   CID 3522 - Electrical
      /// </summary>
      /// <param name="fabPart">The part to check.</param>
      /// <returns>True if the part is a coupling.</returns>
      private bool IsACoupling(FabricationPart fabPart)
      {
         if (fabPart != null)
         {
            int CID = fabPart.ItemCustomId;
            if (CID == 522 || CID == 1522 || CID == 2522 || CID == 3522 || CID == 1112)
            {
               return true;
            }
         }
         return false;
      }
   }
}
