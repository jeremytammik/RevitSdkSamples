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
   public class MoveAnalyticalMemberUsingSetCurve : IExternalCommand
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
         //Expected results: The first Analytical Member has been moved and the connection with the second Analytical Member was lost
         try
         {
            // Get the document
            Document document = commandData.Application.ActiveUIDocument.Document;

            // Create the first Analytical Member
            AnalyticalMember analyticalMember = CreateAnalyticalMember.CreateMember(document);

            // Create the second Analytical Member
            AnalyticalMember convergentAnalyticalMember = CreateAnalyticalMember.CreateConvergentMember(document);

            // Start transaction
            using (Transaction transaction = new Transaction(document, "Offset member"))
            {
               transaction.Start();
              
               // Get the original curve and it's ends
               Curve originalCurve = analyticalMember.GetCurve();
               XYZ originalCurveStart = originalCurve.GetEndPoint(0);
               XYZ originalCurveEnd = originalCurve.GetEndPoint(1);

               // Create new start and end with offset value
               double offset = 15;
               XYZ newLineStart = new XYZ(originalCurveStart.X + offset, 0, 0);
               XYZ newLineEnd = new XYZ(originalCurveEnd.X + offset, 0, 0);

               // Create a new bounded line using the previous coordiantes
               Line line = Line.CreateBound(newLineStart, newLineEnd);
             
               // Set the member's curve to the newly created line
               analyticalMember.SetCurve(line);    

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
