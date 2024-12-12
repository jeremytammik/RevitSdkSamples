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
using System.Text;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Windows.Forms;


namespace Revit.SDK.Samples.DoorSwing.CS
{
   /// <summary>
   /// A ExternalCommand class inherited IExternalCommand interface.
   /// This command will add needed shared parameters and initialize them. 
   /// It will initialize door opening parameter based on family's actual geometry and 
   /// country's standard. It will initialize each door instance's opening, ToRoom, FromRoom and 
   /// internal door flag values according to door's current geometry.
   /// </summary>
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
   public class InitializeCommand : IExternalCommand
   {
      #region IExternalCommand Members

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
      public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData, 
                                             ref string message, 
                                             ElementSet elements)
      {
         Autodesk.Revit.UI.Result returnCode = Autodesk.Revit.UI.Result.Cancelled;  

         Transaction tran = new Transaction(commandData.Application.ActiveUIDocument.Document, "Initialize Command");
         tran.Start();

         try
         {
            // one instance of DoorSwingData class.
            DoorSwingData databuffer = new DoorSwingData(commandData.Application);
   
            using (InitializeForm initForm = new InitializeForm(databuffer))
            {
               // Show UI
               DialogResult dialogResult = initForm.ShowDialog();

               if (DialogResult.OK == dialogResult)
               {
                  databuffer.DeleteTempDoorInstances();

                  // update door type's opening feature based on family's actual geometry and 
                  // country's standard.
                  databuffer.UpdateDoorFamiliesOpeningFeature();

                  // update each door instance's Opening feature and internal door flag
                  returnCode = DoorSwingData.UpdateDoorsInfo(commandData.Application.ActiveUIDocument.Document, false, true, ref message);
               }
            }
         }
         catch (Exception ex)
         {
            // if there is anything wrong, give error information and return failed.
            message    = ex.Message;
            returnCode = Autodesk.Revit.UI.Result.Failed;
         }

         if (Autodesk.Revit.UI.Result.Succeeded == returnCode)
         {
            tran.Commit();
         }
         else
         {
            tran.RollBack();
         }
         return returnCode;
      }

      #endregion
   }

   /// <summary>
   /// A ExternalCommand class inherited IExternalCommand interface.
   /// This command will update each door instance's opening, ToRoom, FromRoom and 
   /// internal door flag values according to door's current geometry.
   /// </summary>
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   public class UpdateParamsCommand : IExternalCommand
   {
      #region IExternalCommand Members

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
      public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData, ref string message, Autodesk.Revit.DB.ElementSet elements)
      {
         Autodesk.Revit.UI.Result returnCode = Autodesk.Revit.UI.Result.Succeeded;
         Autodesk.Revit.UI.UIApplication app = commandData.Application;
         UIDocument doc = app.ActiveUIDocument;
         Transaction tran = new Transaction(doc.Document, "Update Parameters Command");
         tran.Start();

         try
         {
            ElementSet elementSet = new ElementSet();
            foreach (ElementId elementId in doc.Selection.GetElementIds())
            {
               elementSet.Insert(doc.Document.GetElement(elementId));
            }
            if (elementSet.IsEmpty)
            {
               returnCode = DoorSwingData.UpdateDoorsInfo(doc.Document, false, true, ref message);
            }
            else
            {
               returnCode = DoorSwingData.UpdateDoorsInfo(doc.Document, true, true, ref message);
            }
         }
         catch (Exception ex)
         {
            // if there is anything wrong, give error information and return failed.
            message = ex.Message;
            returnCode = Autodesk.Revit.UI.Result.Failed;
         }

         if (Autodesk.Revit.UI.Result.Succeeded == returnCode)
         {
            tran.Commit();
         }
         else
         {
            tran.RollBack();
         }
         return returnCode;
      }

      #endregion
   }

   /// <summary>
   /// A ExternalCommand class inherited IExternalCommand interface.
   /// This command will update door instance's geometry according to door's 
   /// current To/From Room value.
   /// </summary>
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   public class UpdateGeometryCommand : IExternalCommand
   {
      #region IExternalCommand Members

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
      public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData, ref string message, Autodesk.Revit.DB.ElementSet elements)
      {
         Autodesk.Revit.UI.Result returnCode = Autodesk.Revit.UI.Result.Succeeded;
         Autodesk.Revit.UI.UIApplication app = commandData.Application;
         UIDocument doc = app.ActiveUIDocument;
         Transaction tran = new Transaction(doc.Document, "Update Geometry Command");
         tran.Start();

         try
         {
            ElementSet elementSet = new ElementSet();
            foreach (ElementId elementId in doc.Selection.GetElementIds())
            {
               elementSet.Insert(doc.Document.GetElement(elementId));
            }
            if (elementSet.IsEmpty)
            {
               DoorSwingData.UpdateDoorsGeometry(doc.Document, false);
            }
            else
            {
               DoorSwingData.UpdateDoorsGeometry(doc.Document, true);
            }

            returnCode =  Autodesk.Revit.UI.Result.Succeeded;
         }
         catch (Exception ex)
         {
            // if there is anything wrong, give error information and return failed.
            message = ex.Message;
            returnCode = Autodesk.Revit.UI.Result.Failed;
         }

         if (Autodesk.Revit.UI.Result.Succeeded == returnCode)
         {
            tran.Commit();
         }
         else
         {
            tran.RollBack();
         }

         return returnCode;
      }

      #endregion
   }
}
