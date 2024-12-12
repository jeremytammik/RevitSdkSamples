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
using System.Text;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;

namespace Revit.SDK.Samples.DividedSurfaceByIntersects.CS
{
   /// <summary>
   /// the entry point of the sample
   /// </summary>
   [Transaction(TransactionMode.Manual)]
   [Regeneration(RegenerationOption.Manual)]
   [Journaling(JournalingMode.NoCommandData)]
   public class Command : IExternalCommand
   {
      /// <summary>
      /// The active Revit document
      /// </summary>
      Document m_document;

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
         // store the active Revit document
         m_document = commandData.Application.ActiveUIDocument.Document;

         DividedSurface ds = GetDividedSurface();
         if (null == ds)
         {
            message = "Open the family file from the sample folder first.";
            return Result.Failed;
         }
         IEnumerable<ElementId> planes = GetPlanes();
         IEnumerable<ElementId> lines = GetLines();

         Transaction act = new Transaction(m_document);
         act.Start("AddRemoveIntersects");
         try
         {
            // step 1: divide the surface with reference planes and levels
            foreach (ElementId id in planes)
            {
               if (ds.CanBeIntersectionElement(id))
               {
                  ds.AddIntersectionElement(id);
               }
            }

            // step 2: remove all the reference planes and level intersection elements
            IEnumerable<ElementId> intersects = ds.GetAllIntersectionElements();

            foreach (ElementId id in intersects)
            {
               ds.RemoveIntersectionElement(id);
            }
            
            // step 3: divide the surface with model lines instead
            foreach (ElementId id in lines)
            {
               if (ds.CanBeIntersectionElement(id))
               {
                  ds.AddIntersectionElement(id);
               }
            }
         }
         catch (Exception)
         {
            act.RollBack();
         }
         finally
         {
            act.Commit();
         }

         return Autodesk.Revit.UI.Result.Succeeded;
      }

      private DividedSurface GetDividedSurface()
      {
         return m_document.GetElement(new ElementId(31519)) as DividedSurface;
      }

      private IEnumerable<ElementId> GetPlanes()
      {
         // 1027, 1071 & 1072 are ids of the reference planes and levels drawn in the family file
         yield return new ElementId(1027); 
         yield return new ElementId(1071);
         yield return new ElementId(1072);
      }

      private IEnumerable<ElementId> GetLines()
      {
         // the "31xxx" numberic values are ids of the model lines drawn in the family file
         yield return new ElementId(31170);
         yield return new ElementId(31206);
         yield return new ElementId(31321);
         yield return new ElementId(31343);
         yield return new ElementId(31377);
         yield return new ElementId(31395);
      }

      /// <summary>
      /// Get element by its Id
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <param name="eid"></param>
      /// <returns></returns>
      public T GetElement<T>(int eid) where T : Element
      {
         return m_document.GetElement(new ElementId(eid)) as T;
      }
   }
}
