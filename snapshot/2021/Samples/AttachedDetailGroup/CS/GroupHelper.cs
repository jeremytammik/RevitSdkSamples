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
using System.Linq;

namespace Revit.SDK.Samples.AttachedDetailGroup.CS
{
   /// <summary>
   /// Utility class for accessing group API functionality.
   /// </summary>
   class GroupHelper
   {
      /// <summary>
      /// Utility method for retrieving the currently selected model group.
      /// </summary>
      /// <param name="uiDoc">The UIDocument that gives access to the selected elements.</param>
      /// <param name="modelGroup">The selected model group or null if no model group was selected.</param>
      /// <param name="errorMessage">The appropriate error message if a model group was not selected by the user.</param>
      public bool getSelectedModelGroup(UIDocument uiDoc, out Group modelGroup, out string errorMessage)
      {
         modelGroup = null;

         var sel = uiDoc.Selection.GetElementIds();
         if (sel.Count < 1)
         {
            errorMessage = "Please select a single model group.";
            return false;
         }
         else if (sel.Count > 1)
         {
            errorMessage = "Too many selected elements. Please select a single model group.";
            return false;
         }

         Group selectedGroup = uiDoc.Document.GetElement(sel.First<ElementId>()) as Group;
         if (selectedGroup == null)
         {
            errorMessage = "The selected element is not a group. Please select a single model group.";
            return false;
         }

         if (selectedGroup.AttachedParentId != ElementId.InvalidElementId)
         {
            errorMessage = "The selected element is an attached detail group. Please select a single model group.";
            return false;
         }

         errorMessage = "";
         modelGroup = selectedGroup;
         return true;
      }

      /// <summary>
      /// Utility method for creating a transaction and showing the input model group's associated detail groups
      /// that are compatible with the input view.
      /// </summary>
      /// <param name="modelGroup">The selected model group or null if no model group was selected.</param>
      /// <param name="doc">The current Document.</param>
      /// <param name="view">The view in which the attached detail groups will be shown.</param>
      public void ShowAllAttachedDetailGroups(Group modelGroup, Document doc, View view)
      {
         using (Transaction transaction = new Transaction(doc, "ShowAllAttachedDetailGroups"))
         {
            transaction.Start("Show All Attached Detail Groups");

            modelGroup.ShowAllAttachedDetailGroups(view);

            transaction.Commit();
         }
      }

      /// <summary>
      /// Utility method for creating a transaction and hiding the input model group's associated detail groups
      /// that are compatible with the input view.
      /// </summary>
      /// <param name="modelGroup">The selected model group or null if no model group was selected.</param>
      /// <param name="doc">The current Document.</param>
      /// <param name="view">The view in which the attached detail groups will be hidden.</param>
      public void HideAllAttachedDetailGroups(Group modelGroup, Document doc, View view)
      {
         using (Transaction transaction = new Transaction(doc, "HideAllAttachedDetailGroups"))
         {
            transaction.Start("Hide All Attached Detail Groups");

            modelGroup.HideAllAttachedDetailGroups(view);

            transaction.Commit();
         }
      }
   }
}
