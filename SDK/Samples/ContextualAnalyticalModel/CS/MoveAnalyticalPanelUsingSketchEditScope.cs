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
   public class MoveAnalyticalPanelUsingSketchEditScope : IExternalCommand
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
         //Expected results: the Analytical Panel has been moved and the connection with the Analytical Member has been broken
         try
         {
            //Get the Document
            Document document = commandData.Application.ActiveUIDocument.Document;

            // Create Analytical Panel
            AnalyticalPanel analyticalPanel = CreateAnalyticalPanel.CreateAMPanel(document);

            // Create an Analytical Member connected with the Analytical Panel above
            AnalyticalMember analyticalMember = CreateAnalyticalMember.CreateMember(document);

            // Move the Analytical Panel using SketchEditScope 
            SketchEditScope sketchEditScope = new SketchEditScope(document, "Move panel with SketchEditScope");
            sketchEditScope.StartWithNewSketch(analyticalPanel.Id);

            // Start transaction
            using (Transaction transaction = new Transaction(document, "Offset panel"))
            {
               transaction.Start();

               // Get Sketch
               if (document.GetElement(analyticalPanel.SketchId) is Sketch sketch)
               {
                  foreach (CurveArray curveArray in sketch.Profile)
                  {
                     // Iterate through the Curves forming the Analytical Panel and 
                     // create new ones with a slight offset from the original ones before deleting them
                     foreach (Curve curve in curveArray)
                     {
                        Line line = curve as Line;
                        if (line != null)
                        {
                           // Create new offseted Start and End points from the original line coordinates
                           double offset = 5.0;
                           XYZ newLineStart = new XYZ(line.GetEndPoint(0).X + offset, line.GetEndPoint(0).Y + offset, 0);
                           XYZ newLineEnd = new XYZ(line.GetEndPoint(1).X + offset, line.GetEndPoint(1).Y + offset, 0);

                           // Define the new line with offseted coordinates
                           Curve offsetedLine = Line.CreateBound(newLineStart, newLineEnd);

                           // Remove the old line
                           document.Delete(line.Reference.ElementId);

                           // Create the new line
                           document.Create.NewModelCurve(offsetedLine, sketch.SketchPlane);
                        }
                     }
                  }
               }
               transaction.Commit();
            }
            sketchEditScope.Commit(new FailurePreproccessor());
            
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
