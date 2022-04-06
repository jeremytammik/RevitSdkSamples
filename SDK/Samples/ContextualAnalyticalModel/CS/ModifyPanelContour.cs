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
   /// Implements the interface IFailuresPreprocessor
   /// </summary>
   public class FailurePreproccessor : IFailuresPreprocessor
   {
      /// <summary>
      /// This method is called when there have been failures found at the end of a transaction and Revit is about to start processing them. 
      /// </summary>
      /// <param name="failuresAccessor">The Interface class that provides access to the failure information. </param>
      /// <returns></returns>
      public FailureProcessingResult PreprocessFailures(FailuresAccessor failuresAccessor)
      {
         return FailureProcessingResult.Continue;
      }
   }

   /// <summary>
   /// Implements the Revit add-in interface IExternalCommand
   /// Modify Analytical Panel contour using SketchEditScope
   /// </summary>
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   class ModifyPanelContour : IExternalCommand
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
         try
         {
            Document document = commandData.Application.ActiveUIDocument.Document;

            //create analytical panel
            AnalyticalPanel analyticalPanel = CreateAnalyticalPanel.CreateAMPanel(document);
            if (analyticalPanel != null)
            {
               // Start a sketch edit scope
               SketchEditScope sketchEditScope = new SketchEditScope(document, "Replace line with an arc");
               sketchEditScope.StartWithNewSketch(analyticalPanel.Id);

               using (Transaction transaction = new Transaction(document, "Modify sketch"))
               {
                  transaction.Start();

                  //replace a boundary line with an arc
                  Line line = null;
                  Sketch sketch = document.GetElement(analyticalPanel.SketchId) as Sketch;
                  if (sketch != null)
                  {
                     foreach (CurveArray curveArray in sketch.Profile)
                     {
                        foreach (Curve curve in curveArray)
                        {
                           line = curve as Line;
                           if (line != null)
                           {
                              break;
                           }
                        }
                        if (line != null)
                        {
                           break;
                        }
                     }
                  }

                  // Create arc
                  XYZ normal = line.Direction.CrossProduct(XYZ.BasisZ).Normalize().Negate();
                  XYZ middle = line.GetEndPoint(0).Add(line.Direction.Multiply(line.Length / 2));
                  Curve arc = Arc.Create(line.GetEndPoint(0), line.GetEndPoint(1), middle.Add(normal.Multiply(20)));

                  // Remove element referenced by the found line. 
                  document.Delete(line.Reference.ElementId);

                  // Model curve creation automatically puts the curve into the sketch, if sketch edit scope is running.
                  document.Create.NewModelCurve(arc, sketch.SketchPlane);

                  transaction.Commit();
               }

               sketchEditScope.Commit(new FailurePreproccessor());

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
