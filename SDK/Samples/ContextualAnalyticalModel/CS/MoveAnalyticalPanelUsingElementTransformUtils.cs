using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
   public class MoveAnalyticalPanelUsingElementTransformUtils : IExternalCommand
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
      public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
      {
         //Expected results: The Analytical Panel has been moved and the connection with the Analytical Member was kept
         try
         {
            // Get the Document
            Document document = commandData.Application.ActiveUIDocument.Document;

            // Create Analytical Panel
            AnalyticalPanel analyticalPanel = CreateAnalyticalPanel.CreateAMPanel(document);

            // Create an Analytical Member connected with the Analytical Panel above
            AnalyticalMember analyticalMember = CreateAnalyticalMember.CreateMember(document);

            // Move the Analytical Panel using ElementTransformUtils
            using (Transaction transaction = new Transaction(document, "Move panel with ElementTransformUtils"))
            {
               transaction.Start();                            
               ElementTransformUtils.MoveElement(document, analyticalPanel.Id, new XYZ(5, 5, 0));
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
   }
}
