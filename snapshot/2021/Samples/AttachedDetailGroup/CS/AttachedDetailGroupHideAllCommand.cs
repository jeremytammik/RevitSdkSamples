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

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.AttachedDetailGroup.CS
{
   /// <summary>
   /// The external command that hides all of the selected group's attached detail groups.
   /// </summary>
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   class AttachedDetailGroupHideAllCommand : IExternalCommand
   {
      #region IExternalCommand Members

      /// <summary>
      /// The implementation of the command.
      /// </summary>
      /// <param name="commandData"></param>
      /// <param name="message"></param>
      /// <param name="elements"></param>
      /// <returns></returns>
      public Result Execute(ExternalCommandData commandData, ref string message, Autodesk.Revit.DB.ElementSet elements)
      {
         UIDocument uiDoc = commandData.Application.ActiveUIDocument;
         Document doc = uiDoc.Document;
         View activeView = commandData.View;
         GroupHelper groupHelper = new GroupHelper();
         Group selectedModelGroup;

         if (!groupHelper.getSelectedModelGroup(uiDoc, out selectedModelGroup, out message))
         {
            return Result.Cancelled;
         }

         groupHelper.HideAllAttachedDetailGroups(selectedModelGroup, doc, activeView);
         return Result.Succeeded;
      }

      #endregion
   }
}
