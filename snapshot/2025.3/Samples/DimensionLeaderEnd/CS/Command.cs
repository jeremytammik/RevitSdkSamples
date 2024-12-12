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
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

using TaskDialog = Autodesk.Revit.UI.TaskDialog;

namespace Revit.SDK.Samples.DimensionLeaderEnd.CS
{
   /// <summary>
   /// Implements the Revit add-in interface IExternalCommand
   /// </summary>
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   public class MoveHorizontally : IExternalCommand
   {
      private double m_delta = -10;
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
         // Get the handle of current document.
         UIDocument uidoc = commandData.Application.ActiveUIDocument;
         Document doc = uidoc.Document;

         using (Transaction _transaction_ = new Transaction(doc))
         {
            // Get the element selection of current document.
            Selection selection = uidoc.Selection;
            ICollection<ElementId> selectedIds = uidoc.Selection.GetElementIds();

            if (0 == selectedIds.Count)
            {
               // If no elements selected.
               TaskDialog.Show("Revit", "You haven't selected any elements.");
            }
            else
            {
               foreach (ElementId id in selectedIds)
               {
                  Dimension dim = doc.GetElement(id) as Dimension;

                  if (null != dim)
                  {
                     Line dimLine = dim.Curve as Line;
                     if (dimLine != null)
                     {
                        _transaction_.Start("Set leader end position.");
                        try
                        {
                           XYZ dir = dimLine.Direction;
                           if (dim.Segments.IsEmpty)
                           {
                              XYZ leaderPos = ComputeLeaderPosition(dir, dim.Origin);
                              dim.LeaderEndPosition = leaderPos;
                           }
                           else
                           {
                              foreach (DimensionSegment ds in dim.Segments)
                              {
                                 XYZ leaderPos = ComputeLeaderPosition(dir, ds.Origin);
                                 ds.LeaderEndPosition = leaderPos;
                              }
                           }
                           _transaction_.Commit();
                        }
                        catch (System.Exception ex)
                        {
                           TaskDialog.Show("Can't set dimension leader end point: {0}", ex.Message);
                           _transaction_.RollBack();
                        }
                     }
                  }
               }
            }

            return Autodesk.Revit.UI.Result.Succeeded;
         }
      }

      private XYZ ComputeLeaderPosition(XYZ dir, XYZ origin)
      {
         XYZ leaderPos = new XYZ();
         leaderPos = dir * m_delta;
         leaderPos = leaderPos.Add(origin);
         return leaderPos;
      }
   }

   /// <summary>
   /// Implements the Revit add-in interface IExternalCommand
   /// </summary>
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   public class MoveToPickedPoint : IExternalCommand
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
         // Get the handle of current document.
         UIDocument uidoc = commandData.Application.ActiveUIDocument;
         Document doc = uidoc.Document;
         using (Transaction _transaction_ = new Transaction(doc))
         {

            // Get the element selection of current document.
            Selection selection = uidoc.Selection;
            ICollection<ElementId> selectedIds = uidoc.Selection.GetElementIds();

            if (0 == selectedIds.Count)
            {
               // If no elements selected.
               TaskDialog.Show("Revit", "You haven't selected any elements.");
            }
            else
            {
               foreach (ElementId id in selectedIds)
               {
                  Dimension dim = doc.GetElement(id) as Dimension;
                  if (null != dim)
                  {
                     XYZ startPoint = selection.PickPoint(ObjectSnapTypes.None, "Pick start");
                      _transaction_.Start("Set leader end point");
                     try
                     {
                        if (dim.Segments.IsEmpty)
                        {
                           dim.LeaderEndPosition = startPoint;
                        }
                        else
                        {
                           XYZ deltaVec = dim.Segments.get_Item(1).Origin.Subtract(dim.Segments.get_Item(0).Origin);
                           XYZ offset = new XYZ();
                           foreach (DimensionSegment ds in dim.Segments)
                           {
                              ds.LeaderEndPosition = startPoint.Add(offset);
                              offset = offset.Add(deltaVec);
                           }
                        }
                        _transaction_.Commit();
                     }
                     catch (System.Exception ex)
                     {
                        TaskDialog.Show("Can't set dimension leader end point: {0}", ex.Message);
                        _transaction_.RollBack();
                     }
                  }
               }
            }

            return Autodesk.Revit.UI.Result.Succeeded;
         }
      }
   }
}

