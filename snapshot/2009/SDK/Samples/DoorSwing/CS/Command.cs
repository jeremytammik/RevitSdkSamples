//
// (C) Copyright 2003-2008 by Autodesk, Inc.
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
      public IExternalCommand.Result Execute(ExternalCommandData commandData, 
                                             ref string message, 
                                             ElementSet elements)
      {
         try
         {
            Autodesk.Revit.Application app = commandData.Application;

            // one instance of DoorSwingData class.
            DoorSwingData databuffer       = new DoorSwingData(app);
   
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
                  return DoorSwingData.UpdateDoorsInfo(app.Create.Filter, app.ActiveDocument, false, true, ref message);
               }
            }

            // user cancel the setting.
            return IExternalCommand.Result.Cancelled;
         }
         catch (Exception ex)
         {
            // if there is anything wrong, give error information and return failed.
            message = ex.Message;
            return IExternalCommand.Result.Failed;
         }
      }

      #endregion
   }

   /// <summary>
   /// A ExternalCommand class inherited IExternalCommand interface.
   /// This command will update each door instance's opening, ToRoom, FromRoom and 
   /// internal door flag values according to door's current geometry.
   /// </summary>
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
      public IExternalCommand.Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
      {
         try
         {
            Autodesk.Revit.Application app           = commandData.Application;
            Document doc                             = app.ActiveDocument;
            Autodesk.Revit.Creation.Filter creFilter = app.Create.Filter;
            IExternalCommand.Result returnCode = IExternalCommand.Result.Succeeded;

            if (doc.Selection.Elements.IsEmpty)
            {
               returnCode = DoorSwingData.UpdateDoorsInfo(creFilter, doc, false, true, ref message);
            }
            else
            {
               returnCode = DoorSwingData.UpdateDoorsInfo(creFilter, doc, true, true, ref message);
            }

            return returnCode;
         }
         catch (Exception ex)
         {
            // if there is anything wrong, give error information and return failed.
            message = ex.Message;
            return IExternalCommand.Result.Failed;
         }
      }

      #endregion
   }

   /// <summary>
   /// A ExternalCommand class inherited IExternalCommand interface.
   /// This command will update door instance's geometry according to door's 
   /// current To/From Room value.
   /// </summary>
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
      public IExternalCommand.Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
      {
         try
         {
            Autodesk.Revit.Application app           = commandData.Application;
            Document doc                             = app.ActiveDocument;
            Autodesk.Revit.Creation.Filter creFilter = app.Create.Filter;

            if (doc.Selection.Elements.IsEmpty)
            {
               DoorSwingData.UpdateDoorsGeometry(creFilter, doc, false);
            }
            else
            {
               DoorSwingData.UpdateDoorsGeometry(creFilter, doc, true);
            }

            return IExternalCommand.Result.Succeeded;
         }
         catch (Exception ex)
         {
            // if there is anything wrong, give error information and return failed.
            message = ex.Message;
            return IExternalCommand.Result.Failed;
         }
      }

      #endregion
   }
}
