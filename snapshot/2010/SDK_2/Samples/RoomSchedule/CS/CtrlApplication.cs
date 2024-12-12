//
// (C) Copyright 2003-2009 by Autodesk, Inc.
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

namespace Revit.SDK.Samples.RoomSchedule
{
    /// <summary>
    /// This class implements the IExternalApplication interface, 
    /// OnStartup will subscribe Save/SaveAs and DocumentClose events when Revit starts and OnShutdown will unregister these events when Revit exists.
    /// </summary>
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
                    throw new ArgumentException("You need to ensure controlled application was loaded successfully.", "EventReactor");
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
        public IExternalApplication.Result OnStartup(ControlledApplication application)
        {
            // specify the log
            string assemblyName = this.GetType().Assembly.Location;
            m_eventReactor = new EventsReactor(assemblyName.Replace(".dll", ".log"));
            //
            // subscribe events
            application.DocumentSaving += new EventHandler<Autodesk.Revit.Events.DocumentSavingEventArgs>(EventReactor.DocumentSaving);
            application.DocumentSavingAs += new EventHandler<Autodesk.Revit.Events.DocumentSavingAsEventArgs>(EventReactor.DocumentSavingAs);
            application.DocumentClosed += new EventHandler<Autodesk.Revit.Events.DocumentClosedEventArgs>(EventReactor.DocumentClosed);
            return IExternalApplication.Result.Succeeded;
        }

        /// <summary>
        /// Unregister subscribed events when Revit exists
        /// </summary>
        /// <param name="application">Current loaded application.</param>
        /// <returns></returns>
        public IExternalApplication.Result OnShutdown(ControlledApplication application)
        {
            m_eventReactor.Dispose();
            application.DocumentSaving -= new EventHandler<Autodesk.Revit.Events.DocumentSavingEventArgs>(EventReactor.DocumentSaving);
            application.DocumentSavingAs -= new EventHandler<Autodesk.Revit.Events.DocumentSavingAsEventArgs>(EventReactor.DocumentSavingAs);
            application.DocumentClosed -= new EventHandler<Autodesk.Revit.Events.DocumentClosedEventArgs>(EventReactor.DocumentClosed);
            return IExternalApplication.Result.Succeeded;
        }
        #endregion
    }
}