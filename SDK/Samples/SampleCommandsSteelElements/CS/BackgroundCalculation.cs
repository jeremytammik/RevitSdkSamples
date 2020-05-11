//
// (C) Copyright 2003-2018 by Autodesk, Inc.
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
using Autodesk.Revit.UI.Events;
using Autodesk.Revit.DB.Steel;
using Autodesk.SteelConnectionsDB;
using Autodesk.AdvanceSteel.Geometry;
using Autodesk.AdvanceSteel.Modelling;
using Autodesk.AdvanceSteel.CADLink.Database;
using Autodesk.AdvanceSteel.CADAccess;

namespace Revit.SDK.Samples.SampleCommandsSteelElements.BackgroundCalculation.CS
{
   /// <summary>
   /// Implements the Revit add-in interface IExternalCommand
   /// </summary>
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
   public class Command : IExternalCommand
   {
      // Revit Id of a steel plate.
      private ElementId _plateId;
      // Current Revit document.
      private Document _doc;

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
         UIApplication uiApp = commandData.Application;
         //Get the document from external command data.
         UIDocument uiDoc = commandData.Application.ActiveUIDocument;
         _doc = uiDoc.Document;

         if (null == _doc)
         {
            return Result.Failed;
         }

         try
         {
            // Create a steel plate. The view should be in Fine mode to see the plate on screen.
            // It's better to switch to 3d view (current view could be Level view and the plate could not be visible on that level)
            _plateId = CreatePlate(XYZ.Zero);

            // This would generate background calculation (You can see the task running in the Revit "Background processes" window).
            MovePlate();

            // Check for active background calculations.
            bool backgroundCalc = _doc.IsBackgroundCalculationInProgress(); // It should be true.

            // Try to create another plate while background calculation are in progress. This should not succeed !
           CreatePlate(new XYZ(10, 10, 0));

            // Register for the idling event and wait for the background calculations to finish.
            uiApp.Idling += IdlingHandler;
         }
         catch (Autodesk.Revit.Exceptions.OperationCanceledException)
         {
            return Result.Cancelled;
         }
         return Result.Succeeded;
      }

      /// <summary>
      /// Handle the idling event.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="args"></param>
      public void IdlingHandler(object sender, IdlingEventArgs args)
      {
         UIApplication uiApp = sender as UIApplication;
         if (uiApp != null)
         {
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            if (uiDoc != null)
            {
               Document doc = uiDoc.Document;
               // If we still have background calculation then return and wait for them to finish.
               if (!doc.IsBackgroundCalculationInProgress())
               {
                  // Now we can safely create a new plate.
                  CreatePlate(new XYZ(-10, -10, 0));

                  // Deregister from idling event.
                  uiApp.Idling -= IdlingHandler;
               }
            }
         }
         return;
      }

      /// <summary>
      ///  Creates steel plate.
      /// </summary>
      /// <returns>Returns Revit id of the created plate.</returns>
      private ElementId CreatePlate(XYZ plateCenter)
      {
         ElementId ret = ElementId.InvalidElementId;

         Guid plateUniqueId = Guid.Empty;

         try
         {
            // Start detailed steel modeling transaction
            using (FabricationTransaction trans = new FabricationTransaction(_doc, false, "Create structural plate"))
            {
               // Create a plate using Advance Steel API (internal Advance Steel units are in mm)
               Point3d orig = new Point3d(Utilities.Functions.FEET_TO_MM * plateCenter.X,
                                          Utilities.Functions.FEET_TO_MM * plateCenter.Y,
                                          Utilities.Functions.FEET_TO_MM * plateCenter.Z);
               Plate plate = new Plate(new Autodesk.AdvanceSteel.Geometry.Plane(Point3d.kOrigin, Vector3d.kZAxis), orig, 2000, 1000);
               plate.Thickness = 10;

               // Write plate to database.
               plate.WriteToDb();

               plateUniqueId = plate.GetUniqueId();

               trans.Commit();
            }
         }
         catch (System.InvalidOperationException ex)
         {
            //Cannot copy plate due to background calculation in progress !
            System.Diagnostics.Debug.WriteLine(ex.Message);
         }

         // Get Revit element id from Advance Steel plate unique id.         
         Reference elem = SteelElementProperties.GetReference(_doc, plateUniqueId);
         if(elem != null)
         {
            ret = elem.ElementId;
         }

         return ret;
      }

      /// <summary>
      /// Move steel plate.
      /// </summary>
      private void MovePlate()
      {
         // Start Revit transaction.
         using (Autodesk.Revit.DB.Transaction trans = new Autodesk.Revit.DB.Transaction(_doc, "Move plate"))
         {
            trans.Start();

            // internal Revit units are in feet
            ElementTransformUtils.MoveElement(_doc, _plateId, new XYZ(-50, 0, 0));

            trans.Commit();
         }
      }
   }
}
