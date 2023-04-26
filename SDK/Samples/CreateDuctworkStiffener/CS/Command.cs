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

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;


namespace Revit.SDK.Samples.CreateDuctworkStiffener.CS
{
   /// <summary>
   /// Implements the Revit add-in interface IExternalCommand
   /// </summary>
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   public class Command : IExternalCommand
   {
      /// <summary>
      /// The current document of the application
      /// </summary>
      private static Document m_document;

      /// <summary>
      /// The ductwork to host stiffeners
      /// </summary>
      private FabricationPart m_ductwork;

      /// <summary>
      /// The type of the stiffeners
      /// </summary>
      private FamilySymbol m_stiffenerType = null;

      /// <summary>
      /// The distance from ductwork start point to stiffener position
      /// valid range: [0, m_ductwork.CenterlineLength]
      /// </summary>
      private Double m_distanceToHostEnd;

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
      public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData, ref string message,
            ElementSet elements)
      {
         m_document = commandData.Application.ActiveUIDocument.Document;
         if (m_document.IsFamilyDocument)
         {
            message = "Cannot create ductwork stiffener in family document";
            return Result.Failed;
         }

         //Get the ductwork in current project
         FilteredElementCollector ductCollector = new FilteredElementCollector(m_document);
         ductCollector.OfCategory(BuiltInCategory.OST_FabricationDuctwork).OfClass(typeof(FabricationPart));
         if (ductCollector.GetElementCount() == 0)
         {
            message = "The document does not contain fabrication ductwork";
            return Result.Failed;
         }
         m_ductwork = ductCollector.FirstElement() as FabricationPart;

         //Get the ductwork stiffener type
         FilteredElementCollector stiffenerTypeCollector = new FilteredElementCollector(m_document);
         stiffenerTypeCollector.OfCategory(BuiltInCategory.OST_FabricationDuctworkStiffeners).OfClass(typeof(FamilySymbol));
         if (stiffenerTypeCollector.GetElementCount() == 0)
         {
            message = "The document does not contain stiffener family symbol";
            return Result.Failed;
         }
         IList<Element> allStiffenerTypes = stiffenerTypeCollector.ToElements();
         String stiffenerTypeName = "Duct Stiffener - External Rectangular Angle Iron: L Angle";
         foreach (Element element in allStiffenerTypes)
         {
            FamilySymbol f = element as FamilySymbol;
            String name = f.Family.Name + ": " + f.Name;
            if (name == stiffenerTypeName)
            {
               m_stiffenerType = f;
               break;
            }
         }
         if (m_stiffenerType == null)
         {
            message = "The stiffener type cannot be found in this document";
            return Result.Failed;
         }

         //Place the stiffener at ductwork middle point
         m_distanceToHostEnd = 0.5 * m_ductwork.CenterlineLength;

         try
         {
            using (Transaction transaction = new Transaction(m_document, "Sample_CreateDuctworkStiffener"))
            {
               transaction.Start();
               if (!m_stiffenerType.IsActive)
               {
                  m_stiffenerType.Activate();
                  m_document.Regenerate();
               }
               FamilyInstance stiffener = MEPSupportUtils.CreateDuctworkStiffener(m_document, m_stiffenerType.Id, m_ductwork.Id, m_distanceToHostEnd);
               transaction.Commit();
            }
            return Result.Succeeded;
         }
         catch (Exception ex)
         {
            message = ex.Message;
            return Result.Failed;
         }
      }

      #endregion
   }
}

