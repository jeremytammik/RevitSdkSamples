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
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Linq;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.BoundaryConditions.CS
{
    /// <summary>
    /// A ExternalCommand class inherited IExternalCommand interface
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class Command : IExternalCommand
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
        public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData,
                                               ref string message,
                                               ElementSet elements)
        {
            try
            {
                //Retrieves the currently active project.
                UIDocument doc = commandData.Application.ActiveUIDocument;

                // must select a element first
                ElementSet elementSet = new ElementSet();
                foreach (ElementId elementId in doc.Selection.GetElementIds())
                {
                   elementSet.Insert(doc.Document.GetElement(elementId));
                }
                if (1 != elementSet.Size)
                {
                    message = "Please select one structural element which is listed as follows: \r\n" +
                              "Columns/braces/Beams/Walls/Wall Foundations/Slabs/Foundation Slabs";
                    return Autodesk.Revit.UI.Result.Cancelled;
                }


                Transaction tran = new Transaction(doc.Document, "BoundaryConditions");
                tran.Start();

                // deal with the selected element
                foreach (Element element in elementSet)
                {
                    // the selected element must be a structural element
                    if (!IsExpectedElement(element))
                    {
                        message = "Please select one structural element which is listed as follows: \r\n" +
                                  "Columns/braces/Beams/Walls/Wall Foundations/ \r\n" +
                                  "Slabs/Foundation Slabs";
                        return Autodesk.Revit.UI.Result.Cancelled;
                    }

                    // prepare the relative data
                    BoundaryConditionsData dataBuffer = new BoundaryConditionsData(element);

                    // show UI
                    using (BoundaryConditionsForm displayForm = new BoundaryConditionsForm(dataBuffer))
                    {
                        DialogResult result = displayForm.ShowDialog();
                        if (DialogResult.OK == result)
                        {
                            tran.Commit();
                            return Autodesk.Revit.UI.Result.Succeeded;
                        }
                        else if (DialogResult.Retry == result)
                        {
                            message = "failed to create BoundaryConditions.";
                            tran.RollBack();
                            return Autodesk.Revit.UI.Result.Failed;
                        }
                    }    
                }

                tran.RollBack();
                // user cancel the operation
                return Autodesk.Revit.UI.Result.Cancelled;                
            }
            catch (Exception e)
            {
                message = e.Message;
                return Autodesk.Revit.UI.Result.Failed;
            }
        }

      /// <summary>
      /// the selected element must be a structural Column/brace/Beam/Wall/Wall Foundation/Slab/Foundation Slab.
      /// </summary>
      /// <returns></returns>
      private bool IsExpectedElement(Element element)
      {
         // judge the element's type. If it is any type of FamilyInstance, Wall, Floor or 
         // WallFoundation, then get judge if it has a AnalyticalModel.
         AnalyticalToPhysicalAssociationManager assocManager = AnalyticalToPhysicalAssociationManager.GetAnalyticalToPhysicalAssociationManager(element.Document);
         AnalyticalElement elemAnalytical = null;
         if (assocManager != null)
         {
            ElementId associatedElementId = assocManager.GetAssociatedElementId(element.Id);
            if (associatedElementId != ElementId.InvalidElementId)
            {
               Element associatedElement = element.Document.GetElement(associatedElementId);
               if (associatedElement != null && associatedElement is AnalyticalElement)
               {
                  elemAnalytical = associatedElement as AnalyticalElement;
               }
            }
         }
         if (null == elemAnalytical)
         {
            return false;
         }
         FamilyInstance familyInstance = element as FamilyInstance;
         if ((null != familyInstance) && (StructuralType.Footing == familyInstance.StructuralType))
         {
            return false; // if selected a isolated foundation not create BC
         }

         if (element is FamilyInstance || element is Wall || element is Floor || element is WallFoundation)
         {
            return true;
         }

         return false;
      }
   }
}
