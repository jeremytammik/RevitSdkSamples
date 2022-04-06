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
   public class MemberForcesAnalyticalMember : IExternalCommand
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
         try
         {
            // Get the Document
            Document document = commandData.Application.ActiveUIDocument.Document;

            // Create an Analytical Member
            AnalyticalMember analyticalMember = CreateAnalyticalMember.CreateMember(document);

            // Start transaction
            using (Transaction transaction = new Transaction(document, "Member Forces"))
            {
               transaction.Start();

               // Get member forces of analytical member
               IList<MemberForces> memberForces = analyticalMember.GetMemberForces();
               foreach(MemberForces mf in memberForces)
               {
                  Console.WriteLine("Position: " + mf.Start + "Force: " + mf.Force.ToString() + "Moment: " + mf.Moment.ToString());
               }

               // Change some values
               analyticalMember.SetMemberForces(true, new XYZ(10000, 5000, 0), new XYZ(0, 0, 0));
               analyticalMember.SetMemberForces(new MemberForces(false, new XYZ(5000, 5000, 5000), new XYZ(10000, 10000, 10000)));

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
