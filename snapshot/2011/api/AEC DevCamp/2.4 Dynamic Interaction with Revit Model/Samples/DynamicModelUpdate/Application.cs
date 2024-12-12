using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace DynamicModelUpdate
{

   ///////////////////////////////////////////////////////////////////////////////////////////////
   //
   // Application to setup our updaters, register them (on startup), and unregister it (on shutdown)
   //

   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   public class AssociativeSectionUpdater : Autodesk.Revit.UI.IExternalApplication
   {
      //////////////////////////////////////////////////////////////////
      // creating and registering our updaters on Revit startup
      
      public Result OnStartup(Autodesk.Revit.UI.UIControlledApplication app)
      {
         // this application's Id is the currently active AddIn Id:
         // (the Id is taken form the manifest file(AddIn) of this application)
         
         AddInId thisAppId = app.ActiveAddInId;
         
         // instantiate a section updater and register it with Revit

         m_sectionUpdater = new SectionUpdater(thisAppId);
         m_sectionUpdater.Register();

         // instantiate a wall updater and register it with Revit

         m_wallTypeUpdater = new WallTypeUpdater(thisAppId);
         m_wallTypeUpdater.Register();

         return Result.Succeeded;
      }


      //////////////////////////////////////////////////////////////////
      // Unregister our registered our updaters on Revit shutdown

      public Result OnShutdown(Autodesk.Revit.UI.UIControlledApplication application)
      {
         if (m_sectionUpdater != null)
         {
            UpdaterRegistry.UnregisterUpdater(m_sectionUpdater.GetUpdaterId());
            m_sectionUpdater = null;
         }

         if (m_wallTypeUpdater != null)
         {
            UpdaterRegistry.UnregisterUpdater(m_wallTypeUpdater.GetUpdaterId());
            m_wallTypeUpdater = null;
         }
         
         return Result.Succeeded;
      }

      // application's private data

      private static SectionUpdater m_sectionUpdater = null;
      private static WallTypeUpdater m_wallTypeUpdater = null;
   }

}
 