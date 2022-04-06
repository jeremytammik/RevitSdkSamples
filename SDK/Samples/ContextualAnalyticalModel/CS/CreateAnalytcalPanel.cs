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

namespace ContextualAnalyticalModel
{
   /// <summary>
   /// Implements the Revit add-in interface IExternalCommand
   /// </summary>
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   public class CreateAnalyticalPanel : IExternalCommand
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

            //create analytical panel
            AnalyticalPanel analyticalPanel = CreateAMPanel(document);
            if(analyticalPanel != null)
            {
               //create analytical opening on the panel we've just created
               CreateAMOpening(document, analyticalPanel.Id);
            }

            return Result.Succeeded;
         }
         catch (Exception ex)
         {
            message = ex.Message;
            return Result.Failed;
         }
      }

      /// <summary>
      /// Creates an Analytiocal Panel
      /// </summary>
      /// <param name="revitDoc">Revit documenr</param>
      /// <returns></returns>
      public static AnalyticalPanel CreateAMPanel(Document revitDoc)
      {
         AnalyticalPanel analyticalPanel = null;
         using (Transaction transaction = new Transaction(revitDoc, "Create Analytical Panel"))
         {
            transaction.Start();

            //create curveloop which will be assigned to the analytical panel
            CurveLoop profileloop = new CurveLoop();
            profileloop.Append(Line.CreateBound(
               new XYZ(0, 0, 0), new XYZ(5, 0, 0)));
            profileloop.Append(Line.CreateBound(
               new XYZ(5, 0, 0), new XYZ(5, 5, 0)));
            profileloop.Append(Line.CreateBound(
               new XYZ(5, 5, 0), new XYZ(0, 5, 0)));
            profileloop.Append(Line.CreateBound(
               new XYZ(0, 5, 0), new XYZ(0, 0, 0)));

            //create the AnalyticalPanel
            analyticalPanel = AnalyticalPanel.Create(revitDoc, profileloop);

            analyticalPanel.StructuralRole = AnalyticalStructuralRole.StructuralRoleFloor;
            analyticalPanel.AnalyzeAs = AnalyzeAs.SlabOneWay;

            transaction.Commit();
         }

         return analyticalPanel;
      }

      /// <summary>
      /// creates an AnalyticalOpening element which will be placed on the AnalyticalPanel
      /// with id = panelId
      /// </summary>
      public static AnalyticalOpening CreateAMOpening(Document revitDoc, ElementId panelId)
      {
         if (panelId == ElementId.InvalidElementId)
            return null;

         AnalyticalOpening opening = null;

         using (Transaction transaction = new Transaction(revitDoc, "Create Analytical Opening"))
         {
            transaction.Start();

            //create the curveLoop for the AnalyticalOpening element
            CurveLoop profileloop = new CurveLoop();
            profileloop.Append(Line.CreateBound(
               new XYZ(1, 1, 0), new XYZ(2, 1, 0)));
            profileloop.Append(Line.CreateBound(
               new XYZ(2, 1, 0), new XYZ(2, 2, 0)));
            profileloop.Append(Line.CreateBound(
               new XYZ(2, 2, 0), new XYZ(-1, 2, 0)));
            profileloop.Append(Line.CreateBound(
               new XYZ(-1, 2, 0), new XYZ(1, 1, 0)));

            if (AnalyticalOpening.IsCurveLoopValidForAnalyticalOpening(profileloop, revitDoc, panelId))
            {             
               //create the AnalyticalOpening
               opening = AnalyticalOpening.Create(revitDoc, profileloop, panelId);
            }

            transaction.Commit();
         }
         return opening;
      }
   }
}

