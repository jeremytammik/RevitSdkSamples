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
   public class AnalyticalElementLocalCoordinateSystem : IExternalCommand
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
         //Expected results: The Local Coordinate System X axis is align to Line direction.
         try
         {
            // Get the Document
            Document document = commandData.Application.ActiveUIDocument.Document;

            // Create an Analytical Panel
            AnalyticalPanel analyticalPanel = CreateAnalyticalPanel.CreateAMPanel(document);

            // Start transaction
            using (Transaction transaction = new Transaction(document, "Align X axis"))
            {
               transaction.Start();

               Transform trf = analyticalPanel.GetTransform();

               Line line = Line.CreateBound(new XYZ(0, 0, 0), new XYZ(10, 10, 0));

               trf.BasisX = line.Direction.Normalize();
               trf.BasisY = trf.BasisZ.CrossProduct(trf.BasisX);

               if (analyticalPanel.IsValidTransform(trf))
                  analyticalPanel.SetTransform(trf);

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
