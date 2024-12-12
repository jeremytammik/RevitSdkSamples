//
// (C) Copyright 2003-2015 by Autodesk, Inc.
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted,
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE. AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
//

using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
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
 