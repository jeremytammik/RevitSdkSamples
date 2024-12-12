//
// (C) Copyright 2003-2016 by Autodesk, Inc.
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
using System.Threading.Tasks;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Architecture;
using MultistoryStairs = Autodesk.Revit.DB.Architecture.MultistoryStairs;

namespace Revit.SDK.Samples.MSOperation.CS
{
   /// <summary>
   /// A command that add the stairs into multistory stairs by picking the levels (which can align stairs to)
   /// </summary>
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
   public class AddStairsCommand : IExternalCommand
   {
      #region IExternalCommand Members Implementation
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
      /// Cancelled can be used to signify that the user canceled the external operation 
      /// at some point. Failure should be returned if the application is unable to proceed with 
      /// the operation.</returns>
      public Autodesk.Revit.UI.Result Execute(Autodesk.Revit.UI.ExternalCommandData commandData,
          ref string message, Autodesk.Revit.DB.ElementSet elements)
      {
         Transaction newTran = null;
         try
         {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            if (null == uiDoc)
            {
               message = "this command needs to be run in an active document.";
               return Result.Failed;
            }
            Document doc = uiDoc.Document;

            ICollection<ElementId> selectedId = uiDoc.Selection.GetElementIds();
            if (1 != selectedId.Count)
            {
               message = "Please select a multistory stairs before running this command.";
               return Result.Failed;
            }
            MultistoryStairs mStairs = doc.GetElement(selectedId.ElementAt(0)) as MultistoryStairs;
            if (null == mStairs)
            {
               message = "Please select a multistory stairs before running this command.";
               return Result.Failed;
            }

            View currentView = doc.ActiveView;
            if (null == currentView || currentView.ViewType != ViewType.Elevation || !currentView.CanBePrinted)
            {
               message = "The current view should be an elevation view to allow user to select levels.";
               return Result.Failed;
            }

            // allow the user to select the stairs to add stairs.
            LevelSelectionFilter selectionFilter = new LevelSelectionFilter(mStairs, OperationAction.Add);
            IList<Reference> userSelectedRefs = uiDoc.Selection.PickObjects(Autodesk.Revit.UI.Selection.ObjectType.Element, selectionFilter);
            IEnumerable<ElementId> userSelectedIds = from refer in userSelectedRefs select refer.ElementId;

            // start the transaction and add the stairs into the multistory stairs.
            newTran = new Transaction(doc, "Add Stairs to Multistory Stairs");
            newTran.Start();
            mStairs.AddStairsByLevelIds(new HashSet<ElementId>(userSelectedIds));
            newTran.Commit();

            return Autodesk.Revit.UI.Result.Succeeded;
         }
         catch (Exception e)
         {
            message = e.Message;
            if ((newTran != null) && newTran.HasStarted() && !newTran.HasEnded())
               newTran.RollBack();
            return Autodesk.Revit.UI.Result.Failed;
         }
      }
      #endregion IExternalCommand Members Implementation
   }


   /// <summary>
   /// A command that remove the stairs from multistory stairs by picking the levels (have aligned stairs to)
   /// </summary>
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
   public class RemoveStairsCommand : IExternalCommand
   {
      #region IExternalCommand Members Implementation
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
      /// Cancelled can be used to signify that the user canceled the external operation 
      /// at some point. Failure should be returned if the application is unable to proceed with 
      /// the operation.</returns>
      public Autodesk.Revit.UI.Result Execute(Autodesk.Revit.UI.ExternalCommandData commandData,
          ref string message, Autodesk.Revit.DB.ElementSet elements)
      {
         Transaction newTran = null;
         try
         {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            if (null == uiDoc)
            {
               message = "this command needs to be run in an active document.";
               return Result.Failed;
            }
            Document doc = uiDoc.Document;

            ICollection<ElementId> selectedId = uiDoc.Selection.GetElementIds();
            if (1 != selectedId.Count)
            {
               message = "Please select a multistory stairs before running this command.";
               return Result.Failed;
            }
            MultistoryStairs mStairs = doc.GetElement(selectedId.ElementAt(0)) as MultistoryStairs;
            if (null == mStairs)
            {
               message = "Please select a multistory stairs before running this command.";
               return Result.Failed;
            }

            View currentView = doc.ActiveView;
            if (null == currentView || currentView.ViewType != ViewType.Elevation || !currentView.CanBePrinted)
            {
               message = "The current view should be an elevation view to allow user to select levels.";
               return Result.Failed;
            }

            // allow the user to select the levels to remove stairs.
            LevelSelectionFilter selectionFilter = new LevelSelectionFilter(mStairs, OperationAction.Remove);
            IList<Reference> userSelectedRefs = uiDoc.Selection.PickObjects(Autodesk.Revit.UI.Selection.ObjectType.Element, selectionFilter);
            IEnumerable<ElementId> userSelectedIds = from refer in userSelectedRefs select refer.ElementId;

            // start the transaction and remove stairs from the multistory stairs.
            newTran = new Transaction(doc, "Remove Stairs from Multistory Stairs");
            newTran.Start();
            mStairs.RemoveStairsByLevelIds(new HashSet<ElementId>(userSelectedIds));
            newTran.Commit();

            return Autodesk.Revit.UI.Result.Succeeded;
         }
         catch (Exception e)
         {
            message = e.Message;
            if ((newTran != null) && newTran.HasStarted() && !newTran.HasEnded())
               newTran.RollBack();
            return Autodesk.Revit.UI.Result.Failed;
         }
      }
      #endregion IExternalCommand Members Implementation
   }
}
