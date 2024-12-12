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
using System.Text;

using Autodesk.Revit;

namespace Revit.SDK.Samples.AutoStamp.CS
{
    /// <summary>
    /// This class implements the methods of interface IExternalApplication and register View Print related events.
    /// OnStartUp method will register ViewPrinting and ViewPrinted events and unregister them in OnShutDown method.
    /// The registered events will help implement the sample functionalities. 
    /// </summary>
    public class Application : IExternalApplication
    {
        #region Class Member Variable
        /// <summary>
        /// Events reactor for ViewPrint related events
        /// </summary>
        EventsReactor m_eventsReactor;
        #endregion


        #region IExternalApplication Members
        /// <summary>
        /// Implements the OnStartup method to register events when Revit starts.
        /// </summary>
        /// <param name="application">Controlled application of to be loaded to Revit process.</param>
        /// <returns>Return the status of the external application.</returns>
        public IExternalApplication.Result OnStartup(ControlledApplication application)
        {
            // Register related events
            m_eventsReactor = new EventsReactor();
            application.ViewPrinting += new EventHandler<Autodesk.Revit.Events.ViewPrintingEventArgs>(m_eventsReactor.AppViewPrinting);
            application.ViewPrinted += new EventHandler<Autodesk.Revit.Events.ViewPrintedEventArgs>(m_eventsReactor.AppViewPrinted);
            return IExternalApplication.Result.Succeeded;
        }

        /// <summary>
        /// Implements this method to unregister the subscribed events when Revit exits.
        /// </summary>
        /// <param name="application">Controlled application to be shutdown.</param>
        /// <returns>Return the status of the external application.</returns>
        public IExternalApplication.Result OnShutdown(ControlledApplication application)
        {
            // just close log file
            m_eventsReactor.CloseLogFiles();
            //
            // unregister events
            application.ViewPrinting -= new EventHandler<Autodesk.Revit.Events.ViewPrintingEventArgs>(m_eventsReactor.AppViewPrinting);
            application.ViewPrinted -= new EventHandler<Autodesk.Revit.Events.ViewPrintedEventArgs>(m_eventsReactor.AppViewPrinted);
            return IExternalApplication.Result.Succeeded;
        }
        #endregion
    }
}
