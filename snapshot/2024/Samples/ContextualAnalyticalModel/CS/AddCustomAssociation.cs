

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI.Selection;

namespace ContextualAnalyticalModel
{
   /// <summary>
   /// Implements the Revit add-in interface IExternalCommand
   /// Adds new relation between physical and analytical elements using AnalyticalToPhysicalAssociationManager
   /// The relation can be between multiple physical and analytical elements
   /// </summary>
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   public class AddCustomAssociation : IExternalCommand
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
         UIDocument activeDoc = commandData.Application.ActiveUIDocument;
         Autodesk.Revit.DB.Document doc = activeDoc.Document;

         if (null == doc)
         {
            return Result.Failed;
         }

         using (Transaction trans = new Transaction(doc, "Revit.SDK.Samples.AddRelationBetweenPhysicalAndAnalyticalElements"))
         {
            trans.Start();

            ISet<ElementId> analyticalElementIds = ContextualAnalyticalModel.Utilities.GetSelectedObjects(activeDoc, "Please select analytical elements");
            ISet<ElementId> physicalElementIds = ContextualAnalyticalModel.Utilities.GetSelectedObjects(activeDoc, "Please select physical elements");

            //gets the AnalyticalToPhysicalAssociationManager for the current document
            AnalyticalToPhysicalAssociationManager analyticalToPhysicalmanager = AnalyticalToPhysicalAssociationManager.GetAnalyticalToPhysicalAssociationManager(doc);
            if (analyticalToPhysicalmanager == null)
               return Result.Failed;

            //creates a new relation between physical and analytical selected elements
            analyticalToPhysicalmanager.AddAssociation(analyticalElementIds, physicalElementIds);

            trans.Commit();
         }
         return Result.Succeeded;
      }
   }
}
