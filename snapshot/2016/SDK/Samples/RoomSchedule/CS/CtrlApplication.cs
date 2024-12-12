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

using Autodesk.Revit;
using Autodesk.Revit.UI;
using Autodesk.Revit.ApplicationServices;

namespace Revit.SDK.Samples.RoomSchedule
{
    /// <summary>
    /// This class implements the IExternalApplication interface, 
    /// OnStartup will subscribe Save/SaveAs and DocumentClose events when Revit starts and OnShutdown will unregister these events when Revit exists.
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class CrtlApplication : IExternalApplication
    {
        #region Class Members 
        /// <summary>
        /// The events reactor for this application. 
        /// </summary>
        static private EventsReactor m_eventReactor;

        /// <summary>
        /// Access the event reactor instance
        /// </summary>
        public static EventsReactor EventReactor
        {
            get 
            {
                if (null == m_eventReactor)
                {
                    throw new ArgumentException("External application was not loaded yet, please make sure you register external application by correct full path of dll.", "EventReactor");
                }
                else
                {
                    return CrtlApplication.m_eventReactor;
                }
            }
        }
        #endregion


        #region IExternalApplication Implementations
        /// <summary>
        /// Implement OnStartup method to subscribe related events.
        /// </summary>
        /// <param name="application">Current loaded application.</param>
        /// <returns></returns>
        public Autodesk.Revit.UI.Result OnStartup(UIControlledApplication application)
        {
            // specify the log
            string assemblyName = this.GetType().Assembly.Location;
            m_eventReactor = new EventsReactor(assemblyName.Replace(".dll", ".log"));
            //
            // subscribe events
            application.ControlledApplication.DocumentSaving += new EventHandler<Autodesk.Revit.DB.Events.DocumentSavingEventArgs>(EventReactor.DocumentSaving);
            application.ControlledApplication.DocumentSavingAs += new EventHandler<Autodesk.Revit.DB.Events.DocumentSavingAsEventArgs>(EventReactor.DocumentSavingAs);
            application.ControlledApplication.DocumentClosed += new EventHandler<Autodesk.Revit.DB.Events.DocumentClosedEventArgs>(EventReactor.DocumentClosed);
            return Autodesk.Revit.UI.Result.Succeeded;
        }

        /// <summary>
        /// Unregister subscribed events when Revit exists
        /// </summary>
        /// <param name="application">Current loaded application.</param>
        /// <returns></returns>
        public Autodesk.Revit.UI.Result OnShutdown(UIControlledApplication application)
        {
            m_eventReactor.Dispose();
            application.ControlledApplication.DocumentSaving -= new EventHandler<Autodesk.Revit.DB.Events.DocumentSavingEventArgs>(EventReactor.DocumentSaving);
            application.ControlledApplication.DocumentSavingAs -= new EventHandler<Autodesk.Revit.DB.Events.DocumentSavingAsEventArgs>(EventReactor.DocumentSavingAs);
            application.ControlledApplication.DocumentClosed -= new EventHandler<Autodesk.Revit.DB.Events.DocumentClosedEventArgs>(EventReactor.DocumentClosed);
            return Autodesk.Revit.UI.Result.Succeeded;
        }
        #endregion
    }
}
