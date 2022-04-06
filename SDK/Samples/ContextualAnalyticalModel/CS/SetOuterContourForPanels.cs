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
   class SetOuterContourForPanels : IExternalCommand
   {
      public virtual Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
      {
         try
         {
            Document document = commandData.Application.ActiveUIDocument.Document;

            //create analytical panel
            AnalyticalPanel analyticalPanel = CreateAnalyticalPanel.CreateAMPanel(document);
            if (analyticalPanel != null)
            {
               using (Transaction transaction = new Transaction(document, "Edit Analytical Panel outer contour"))
               {
                  transaction.Start();

                  //create a new curve loop
                  CurveLoop profileloop = new CurveLoop();
                  profileloop.Append(Line.CreateBound(
                     new XYZ(0, 0, 0), new XYZ(5, 0, 0)));
                  profileloop.Append(Line.CreateBound(
                     new XYZ(5, 0, 0), new XYZ(5, 5, 0)));
                  profileloop.Append(Line.CreateBound(
                     new XYZ(5, 5, 0), new XYZ(-2, 5, 0)));
                  profileloop.Append(Line.CreateBound(
                     new XYZ(-2, 5, 0), new XYZ(0, 0, 0)));

                  //Sets the new contour for analytical panel
                  analyticalPanel.SetOuterContour(profileloop);

                  transaction.Commit();
               }
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
