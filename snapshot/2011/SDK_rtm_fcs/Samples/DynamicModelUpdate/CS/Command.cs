using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.DynamicModelUpdate.CS
{

   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Automatic)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   public class AssociativeSectionUpdater : Autodesk.Revit.UI.IExternalApplication
   {
      public Result OnStartup(Autodesk.Revit.UI.UIControlledApplication app)
      {
         SectionUpdater updater = new SectionUpdater(app.ActiveAddInId);
         UpdaterRegistry.RegisterUpdater(updater);
         ElementClassFilter classFilter = new ElementClassFilter(typeof(FamilyInstance));
         ElementCategoryFilter categoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_Windows);
         LogicalAndFilter filter = new LogicalAndFilter(classFilter, categoryFilter);
         UpdaterRegistry.AddTrigger(updater.GetUpdaterId(), filter, Element.GetChangeTypeGeometry());

         FailureDefinitionId warnId = new FailureDefinitionId(new Guid("FB4F5AF3-42BB-4371-B559-FB1648D5B4D1"));
         updater.SetWarnId(warnId);
         FailureDefinition failDef = FailureDefinition.CreateFailureDefinition(warnId, FailureSeverity.Warning, "Window section updater cannot work with this window because it is missing required parameters.");

         return Result.Succeeded;
      }
      public Result OnShutdown(Autodesk.Revit.UI.UIControlledApplication application) {return Result.Succeeded;}
   }
   
   public class SectionUpdater : IUpdater
   {
      AddInId addinID = null;
      UpdaterId updaterID = null;
      static FailureDefinitionId warnId = null;
      public SectionUpdater(AddInId id) { addinID = id; updaterID = new UpdaterId(addinID, new Guid("FBF3F6B2-4C06-42d4-97C1-D1B4EB593EFF")); }
      public void Execute(UpdaterData data)
      {
         Document doc = data.GetDocument();
         foreach (ElementId id in data.GetModifiedElementIds())
         {
            Element element = doc.get_Element(id);

            Autodesk.Revit.DB.Parameter xParam = element.get_Parameter("X");
            Autodesk.Revit.DB.Parameter yParam = element.get_Parameter("Y");
            Autodesk.Revit.DB.Parameter zParam = element.get_Parameter("Z");
            if (xParam == null || yParam == null || zParam == null)
            {
               FailureMessage failMessage = new FailureMessage(GetWarnId());
               failMessage.SetFailingElement(id);
               doc.PostFailure(failMessage);
               return;
            }
            double xOld = xParam.AsDouble();
            double yOld = yParam.AsDouble();
            double zOld = zParam.AsDouble();

            LocationPoint lp = element.Location as LocationPoint;
            double xNew = lp.Point.X;
            double yNew = lp.Point.Y;
            double zNew = lp.Point.Z;
            XYZ translationVec = new XYZ(xNew - xOld, yNew - yOld, zNew - zOld);

            if (!translationVec.IsZeroLength())
            {
               bool move = doc.Move(doc.get_Element(new ElementId(156136)), translationVec);
            }

            xParam.Set(xNew);
            yParam.Set(yNew);
            zParam.Set(zNew);
         }
      }
      public string GetAdditionalInformation(){return "Automatically move a section to maintain its position relative to a window";}
      public ChangePriority GetChangePriority() {return ChangePriority.Views;}
      public UpdaterId GetUpdaterId() { return updaterID; }
      public void SetWarnId(FailureDefinitionId id) { warnId = id; }
      public FailureDefinitionId GetWarnId() { return warnId; }
      public string GetUpdaterName() { return "Associative Section Updater"; }
   }

}
 