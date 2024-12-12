using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;

namespace DynamicModelUpdate
{

   ///////////////////////////////////////////////////////////////////////////////////////////////
   //
   // The section updater class - will move a section to follow a moved window
   //

   public class SectionUpdater : IUpdater
   {
      // constructor

      internal SectionUpdater(AddInId addinID)
      {
         m_updaterId = new UpdaterId(addinID, new Guid("FBF3F6B2-4C06-42d4-97C1-D1B4EB593EFF"));
      }

      // self-registering with Revit

      internal void Register()
      {
         // create a window filter for the updater's scope

         ElementClassFilter classFilter = new ElementClassFilter(typeof(FamilyInstance));
         ElementCategoryFilter categoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_Windows);
         LogicalAndFilter filter = new LogicalAndFilter(classFilter, categoryFilter);

         // register and set a trigger for the section updater

         UpdaterRegistry.RegisterUpdater(this);
         UpdaterRegistry.AddTrigger(m_updaterId, filter, Element.GetChangeTypeGeometry());
      }

      // implementing the interface

      public UpdaterId GetUpdaterId()
      {
         return m_updaterId;
      }

      public string GetUpdaterName()
      {
         return "Associative Section Updater";
      }

      public string GetAdditionalInformation()
      {
         return "Automatically moves a section to maintain its position relative to a window";
      }

      public ChangePriority GetChangePriority()
      {
         return ChangePriority.Views;   // there is no exact priority for sections, but we can choose something equally low
      }

      // The Execute method does the actual work

      public void Execute(UpdaterData data)
      {
         Document doc = data.GetDocument();

         // we are only interested in two elements, one window and one section
         // (for the sake of simplicity, we hard-coded the element's unique Id)

         String windowUniqueId = "7f04899c-797b-4ad1-a9da-c6de1243ff83-00025ff6";
         String sectionUniqueId = "4ca42fe1-e3e2-4bee-ab19-7d6e95cdf1bc-000261e8";

         // make sure our section line is in the document

         Element sectionLine = doc.get_Element(sectionUniqueId);
         if (sectionLine == null)
         {
            return;  // nothing to do
         }

         // iterate through modified windows to find the one we want the section to follow

         foreach (ElementId id in data.GetModifiedElementIds())
         {
            Element window = doc.get_Element(id);

            // we skip other windows, except the one we want to follow
            if (window.UniqueId != windowUniqueId)
            {
               continue;
            }

            // first we look-up X-Y-Z parameters, which we know are set on our windows
            // and the values are set to the current coordinates of the window instance
            Autodesk.Revit.DB.Parameter xParam = window.get_Parameter("X");
            Autodesk.Revit.DB.Parameter yParam = window.get_Parameter("Y");
            Autodesk.Revit.DB.Parameter zParam = window.get_Parameter("Z");

            // something is wrong if any of the parameters was not found
            if (xParam == null || yParam == null || zParam == null)
            {
               break;
            }

            // establish the translation from the old to the new location

            double xOld = xParam.AsDouble();
            double yOld = yParam.AsDouble();
            double zOld = zParam.AsDouble();

            LocationPoint lp = window.Location as LocationPoint;

            double xNew = lp.Point.X;
            double yNew = lp.Point.Y;
            double zNew = lp.Point.Z;

            XYZ translationVec = new XYZ(xNew - xOld, yNew - yOld, zNew - zOld);

            // move the section by the same vector

            if (!translationVec.IsZeroLength())
            {
               bool move = doc.Move(sectionLine, translationVec);
            }

            // update the parameters on the window instance (to be the current position)

            xParam.Set(xNew);
            yParam.Set(yNew);
            zParam.Set(zNew);

            break;  // we only expect to find one window, thus we can leave the loop now
         }

         return;
      }

      // private data:

      private UpdaterId m_updaterId = null;
   }

}