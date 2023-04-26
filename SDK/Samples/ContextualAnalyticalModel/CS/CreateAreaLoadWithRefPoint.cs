//
// (C) Copyright 2003-2021 by Autodesk, Inc.
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
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI.Selection;

namespace ContextualAnalyticalModel
{
   /// <summary>
   /// Implements the Revit add-in interface IExternalCommand
   /// </summary>
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   public class CreateAreaLoadWithRefPoint : IExternalCommand
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
      public virtual Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
      {
         try
         {
            Document document = commandData.Application.ActiveUIDocument.Document;
            UIDocument activeDoc = commandData.Application.ActiveUIDocument;

            //select object for adding a line load
            Reference eRef = activeDoc.Selection.PickObject(ObjectType.Element, "Please select the analytical element");
            ElementId selectedElementId = null;
            if (eRef != null && eRef.ElementId != ElementId.InvalidElementId)
               selectedElementId = eRef.ElementId;

            XYZ start = activeDoc.Selection.PickPoint("start");
            XYZ end = activeDoc.Selection.PickPoint("end");

            //create curveloop which will be assigned to the analytical panel
            CurveLoop profileloop = new CurveLoop();
            profileloop.Append(Line.CreateBound(
              start, new XYZ(end.X, start.Y, 0)));
            profileloop.Append(Line.CreateBound(
               new XYZ(end.X, start.Y, 0), end));
            profileloop.Append(Line.CreateBound(
               end, new XYZ(start.X, end.Y, 0)));
            profileloop.Append(Line.CreateBound(
               new XYZ(start.X, end.Y, 0), start));

            List<CurveLoop> loops = new List<CurveLoop>();
            loops.Add(profileloop);

            using (Transaction transaction = new Transaction(document, "Create custom AreaLoad"))
            {
               transaction.Start();

               IList<int> refPointsIndexes = new List<int>() { 0, 2, 2};
               IList<int> refPointsCurveEnds = new List<int>() { 0, 1, 0};

               IList<XYZ> forceVector = new List<XYZ>() { new XYZ(0, 0, -10000), new XYZ(0, 0, 0), new XYZ(0, 0, 0) };

               AreaLoad areaLoad = null;
               if (AreaLoad.IsCurveLoopsInsideHostBoundaries(document, selectedElementId, loops))
                  areaLoad = AreaLoad.Create(document, selectedElementId, loops, forceVector, refPointsIndexes, refPointsCurveEnds, null);

               transaction.Commit();
            }

         }
         catch (Exception ex)
         {
            message = ex.Message;
            return Result.Failed;
         }

         return Result.Succeeded;
      }
   }
}

