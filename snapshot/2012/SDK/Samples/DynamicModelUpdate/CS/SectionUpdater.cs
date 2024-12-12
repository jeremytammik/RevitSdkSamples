using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.DynamicModelUpdate.CS
{

   /// <summary>
   /// Updater to automatically move a section in conjunction with the location of a window
   /// </summary>
   public class SectionUpdater : IUpdater
   {
      internal SectionUpdater(AddInId addinID)
      {
         m_updaterId = new UpdaterId(addinID, new Guid("FBF3F6B2-4C06-42d4-97C1-D1B4EB593EFF"));
         m_schemaId = new Guid("4DE4BE80-0857-4785-A7DF-8A8918851CB2");
      }

      // Registers itself with Revit
      internal void Register(Document doc)
      {
         // create a window filter for the updater's scope
          // we are only interested in two elements, one window and one section
          // (for the sake of simplicity, we hard-coded the elements' unique Ids)
          String windowUniqueId = "7f04899c-797b-4ad1-a9da-c6de1243ff83-00025ff6";
          String sectionUniqueId = "4ca42fe1-e3e2-4bee-ab19-7d6e95cdf1bc-000261e8";

          Element window = doc.get_Element(windowUniqueId);
          if (window == null)
              throw new Exception("Window was not found in the document.");
          Element section = doc.get_Element(sectionUniqueId);
          if (section == null)
              throw new Exception("Section was not found in the document.");

          // Setup collection of ids for the updater
          List<ElementId> idsToWatch = new List<ElementId>();
          m_windowId = window.Id;
          idsToWatch.Add(m_windowId);
          m_sectionId = section.Id;

          UpdateInitialParameters(doc);
 
         // Register and set a trigger for the section updater when the window changes
         UpdaterRegistry.RegisterUpdater(this, doc);
         UpdaterRegistry.AddTrigger(m_updaterId, doc, idsToWatch, Element.GetChangeTypeGeometry());
      }

      #region IUpdater members

      // The Execute method for the updater
      public void Execute(UpdaterData data)
      {
          try
          {
              Document doc = data.GetDocument();
              FamilyInstance window = doc.get_Element(m_windowId) as FamilyInstance;
              Element section = doc.get_Element(m_sectionId);

              // iterate through modified elements to find the one we want the section to follow
              foreach (ElementId id in data.GetModifiedElementIds())
              {
                  if (id == m_windowId)
                  {
                     //Let's take this out temporarily.
                     bool enableLookup = false;
                     if (enableLookup)
                        m_schema = Schema.Lookup(m_schemaId); // (new Guid("{4DE4BE80-0857-4785-A7DF-8A8918851CB2}"));

                     Entity storedEntity = null;
                     storedEntity = window.GetEntity(m_schema);
              
                     // 
                     // first we look-up X-Y-Z parameters, which we know are set on our windows
                     // and the values are set to the current coordinates of the window instance
                     Field fieldPosition = m_schema.GetField("Position");
                     XYZ oldPosition = storedEntity.Get<XYZ>(fieldPosition, DisplayUnitType.DUT_FEET_FRACTIONAL_INCHES);

                     TaskDialog.Show("Old position", oldPosition.ToString());

                     LocationPoint lp = window.Location as LocationPoint;
                     XYZ newPosition = lp.Point;

                     // XYZ has operator overloads 
                     XYZ translationVec = newPosition - oldPosition;

                     // move the section by the same vector
                     if (!translationVec.IsZeroLength())
                     {
                        ElementTransformUtils.MoveElement(doc, section.Id, translationVec);
                     }
                     TaskDialog.Show("Moving", "Moving");

                     // Lookup the normal vector (i,j,we assume k=0)
                     Field fieldOrientation = m_schema.GetField("Orientation");
                     // Establish the old and new orientation vectors
                     XYZ oldNormal = storedEntity.Get<XYZ>(fieldOrientation, DisplayUnitType.DUT_FEET_FRACTIONAL_INCHES);


                     XYZ newNormal = window.FacingOrientation;

                     // If different, rotate the section by the angle around the location point of the window

                     double angle = oldNormal.AngleTo(newNormal);

                     // Need to adjust the rotation angle based on the direction of rotation (not covered by AngleTo)
                     XYZ cross = oldNormal.CrossProduct(newNormal).Normalize();
                     double sign = 1.0;
                     if (!cross.IsAlmostEqualTo(XYZ.BasisZ))
                     {
                        sign = -1.0;
                     }
                     angle *= sign;
                     if (Math.Abs(angle) > 0)
                     {
                        Line axis = doc.Application.Create.NewLineBound(newPosition, newPosition + XYZ.BasisZ);
                        ElementTransformUtils.RotateElement(doc, section.Id, axis, angle);
                     }

                     // update the parameters on the window instance (to be the current position and orientation)
                     storedEntity.Set<XYZ>(fieldPosition, newPosition, DisplayUnitType.DUT_FEET_FRACTIONAL_INCHES);


                     storedEntity.Set<XYZ>(fieldOrientation, newNormal, DisplayUnitType.DUT_FEET_FRACTIONAL_INCHES);
                     window.SetEntity(storedEntity);

                  }
              }

          }
          catch (System.Exception ex)
          {
              TaskDialog.Show("Exception", ex.ToString());
          }


         return;
      }

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
          return ChangePriority.Views;
      }

      #endregion


       // Setup routine: updates parameters of the window to the appropriate values on load
       internal void UpdateInitialParameters(Document doc)
       {
    
          Transaction t = new Transaction(doc, "Update parameters");
           t.Start();


           SchemaBuilder builder = new SchemaBuilder(m_schemaId); //(new Guid("{4DE4BE80-0857-4785-A7DF-8A8918851CB2}"));
           builder.AddSimpleField("Position", typeof(XYZ)).SetUnitType(UnitType.UT_Length);
           builder.AddSimpleField("Orientation", typeof(XYZ)).SetUnitType(UnitType.UT_Length);
           builder.SetSchemaName("WallPositionData");
           builder.SetDocumentation("Two points in a Window element that assist in placing a section view.");
           builder.SetVendorId("adsk");
           builder.SetApplicationGUID(doc.Application.ActiveAddInId.GetGUID());

           m_schema = builder.Finish();

           t.Commit();

           t.Start();
           Field fieldPosition = m_schema.GetField("Position");
           Field fieldOrientation = m_schema.GetField("Orientation");

           FamilyInstance window = doc.get_Element(m_windowId) as FamilyInstance;

           Entity storageEntity = new Entity(m_schema);
     
           LocationPoint lp = window.Location as LocationPoint;
           XYZ location = lp.Point;

           storageEntity.Set<XYZ>(fieldPosition, location, DisplayUnitType.DUT_FEET_FRACTIONAL_INCHES);

           XYZ orientation = window.FacingOrientation;
           storageEntity.Set<XYZ>(fieldOrientation, orientation, DisplayUnitType.DUT_FEET_FRACTIONAL_INCHES);  
           window.SetEntity(storageEntity);
    
           
           t.Commit();
       }

      // private data:

      private Guid m_schemaId;
      private UpdaterId m_updaterId = null;
      private ElementId m_windowId = null;
      private ElementId m_sectionId = null;
      private Schema m_schema; 
   }

}