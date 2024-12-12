//
// (C) Copyright 2003-2010 by Autodesk, Inc.
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
using System.IO;
using System.Linq;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Revit.Samples.DirectionCalculation
{
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Automatic)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
   public class FindSouthFacingWallsWithoutProjectLocation : FindSouthFacingWalls, IExternalCommand
   {
      static AddInId m_appId = new AddInId(new Guid("8B29D56B-7B9A-4c79-8A38-B1C13B921877"));
       /// <summary>
      /// The top level command.
      /// </summary>
      /// <param name="revit">An object that is passed to the external application 
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
      public Result Execute(ExternalCommandData revit,
                                                            ref string message,
                                                            ElementSet elements)
      {
         Application = revit.Application.Application;
         Document = revit.Application.ActiveUIDocument.Document;

         Execute(false);

         CloseFile();

         return Result.Succeeded;
      }

   }

   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Automatic)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   public class FindSouthFacingWallsWithProjectLocation : FindSouthFacingWalls, IExternalCommand
   {
      static AddInId m_appId = new AddInId(new Guid("6CADE602-7F32-496c-AA37-CEE4B0EE6087"));
       /// <summary>
       /// The top level command.
       /// </summary>
       /// <param name="revit">An object that is passed to the external application 
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
       public Result Execute(ExternalCommandData revit,
                                                             ref string message,
                                                             ElementSet elements)
       {
           Application = revit.Application.Application;
           Document = revit.Application.ActiveUIDocument.Document;

           Execute(true);

           CloseFile();

           return Result.Succeeded;
       }
   }

   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Automatic)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]

   public class FindSouthFacingWindowsWithoutProjectLocation : FindSouthFacingWindows, IExternalCommand
   {
      static AddInId m_appId = new AddInId(new Guid("AB3588F5-1CD1-4693-9DF0-C0890C811B21"));
       /// <summary>
      /// The top level command.
      /// </summary>
      /// <param name="revit">An object that is passed to the external application 
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
      public Result Execute(ExternalCommandData revit,
                                                            ref string message,
                                                            ElementSet elements)
      {
         Application = revit.Application.Application;
         Document = revit.Application.ActiveUIDocument.Document;

         Execute(false);

         CloseFile();

         return Result.Succeeded;
      }

   }

   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Automatic)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   public class FindSouthFacingWindowsWithProjectLocation : FindSouthFacingWindows, IExternalCommand
   {
      static AddInId m_appId = new AddInId(new Guid("BFECDEA2-C384-4bcc-965E-EA302BA309AA"));
       /// <summary>
       /// The top level command.
       /// </summary>
       /// <param name="revit">An object that is passed to the external application 
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
      public Result Execute(
        ExternalCommandData revit,
        ref string message,
        ElementSet elements )
      {
        Application = revit.Application.Application;
        Document = revit.Application.ActiveUIDocument.Document;

        Execute( true );

        CloseFile();

        return Result.Succeeded;
      }
   }
}
