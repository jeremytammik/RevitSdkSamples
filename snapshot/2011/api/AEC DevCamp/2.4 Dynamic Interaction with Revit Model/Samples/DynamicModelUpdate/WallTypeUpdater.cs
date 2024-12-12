using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;

namespace DynamicModelUpdate
{

   ///////////////////////////////////////////////////////////////////////////////////////////////
   //
   // The wall-type updater class - will move a section to follow a moved window
   //

   public class WallTypeUpdater : IUpdater
   {
      // constructor

      internal WallTypeUpdater(AddInId addinID)
      {
         m_updaterId = new UpdaterId(addinID, new Guid("FBF3F6B2-4C06-42d4-97C1-D1B4EB593EFE"));
      }

      // self-registering with Revit

      internal void Register()
      {
         // create a wall filter for the updater's scope

         ElementClassFilter wallFilter = new ElementClassFilter(typeof(Wall));

         // register and set a trigger for newly added walls

         UpdaterRegistry.RegisterUpdater(this);
         UpdaterRegistry.AddTrigger(m_updaterId, wallFilter, Element.GetChangeTypeElementAddition());
      }

      // implementing the interface

      public string GetAdditionalInformation()
      {
         return "Wall type updater example: updates all newly created walls to a special wall type.";
      }

      public ChangePriority GetChangePriority()
      {
         return ChangePriority.FloorsRoofsStructuralWalls;
      }

      public UpdaterId GetUpdaterId()
      {
         return m_updaterId;
      }

      public string GetUpdaterName()
      {
         return "Wall Type Updater";
      }

      public void Execute(UpdaterData data)
      {
         Document doc = data.GetDocument();
         ICollection<ElementId> wallIds = data.GetAddedElementIds();

         if (wallIds.Count == 0)  // this should not happen
         {
            return;
         }

         // if not done not have a wall type yet, we pick the first added wall and use it as a template

         if (m_wallType == null)
         {
            IEnumerator<ElementId> iter = wallIds.GetEnumerator();
            iter.Reset();
            iter.MoveNext();
            Wall wallTemplate = doc.get_Element(iter.Current) as Wall;

            m_wallType = wallTemplate.WallType;    // this is going to be our template type
         }

         // Change the wall to the template type

         if (m_wallType != null)
         {
            foreach (ElementId addedElemId in wallIds)
            {
               Wall wall = doc.get_Element(addedElemId) as Wall;

               if ((wall != null) && (wall.WallType != m_wallType))
               {
                  wall.WallType = m_wallType;
               }
            }
         }

         return;
      }

      // private data:

      private WallType m_wallType = null;
      private UpdaterId m_updaterId = null;
   }

}
