using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.DynamicModelUpdate.CS
{

   ///////////////////////////////////////////////////////////////////////////////////////////////
   //
   // Application to setup our updaters, register them (on startup), and unregister it (on shutdown)
   //

   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   public class AssociativeSectionUpdater : Autodesk.Revit.UI.IExternalApplication
   {
       private AddInId m_thisAppId;

      //////////////////////////////////////////////////////////////////
      // creating and registering our updaters on Revit startup
      
      public Result OnStartup(Autodesk.Revit.UI.UIControlledApplication app)
      {
         // this application's Id is the currently active AddIn Id:
         // (the Id is taken form the manifest file(AddIn) of this application)
         
         m_thisAppId = app.ActiveAddInId;
         
         app.ControlledApplication.DocumentOpened += RegisterSectionUpdaterOnOpen;

         return Result.Succeeded;
      }

       private void RegisterSectionUpdaterOnOpen(object source, DocumentOpenedEventArgs args)
       {
          if (args.Document.Title.StartsWith("AssociativeSection"))
          {
             m_sectionUpdater = new SectionUpdater(m_thisAppId);
             m_sectionUpdater.Register(args.Document);

             bool enableSecondUpdate = false;
             if (enableSecondUpdate)
             {
                m_sectionUpdater.UpdateInitialParameters(args.Document);
             }
          }

           args.Document.DocumentClosing += UnregisterSectionUpdaterOnClose;
       }

       private void UnregisterSectionUpdaterOnClose(object source, DocumentClosingEventArgs args)
       {
           if (m_sectionUpdater != null)
           {
               UpdaterRegistry.UnregisterUpdater(m_sectionUpdater.GetUpdaterId());
               m_sectionUpdater = null;
           }
       }

      //////////////////////////////////////////////////////////////////
      // Unregister our registered our updater on Revit shutdown
      public Result OnShutdown(Autodesk.Revit.UI.UIControlledApplication application)
      {
         return Result.Succeeded;
      }

      

      // application's private data

      private static SectionUpdater m_sectionUpdater = null;
 
   }

}
 