//
// (C) Copyright 2003-2012 by Autodesk, Inc.
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

using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.PrintLog.CS
{
    /// <summary>
    /// This class implements the methods of interface IExternalApplication and register Print related events.
    /// OnStartUp method will register ViewPrint and DocumentPrint events and unregister them in OnShutDown method.
    /// The registered events will help implements the sample functionalities. 
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class Application : IExternalApplication
    {
        #region Class Member Variable
        /// <summary>
        /// Events reactor for ViewPrint and DocumentPrint related
        /// </summary>
        EventsReactor m_eventsReactor = null;
        #endregion


        #region IExternalApplication Members
        /// <summary>
        /// Implement the OnStartup method to register events when Revit starts.
        /// </summary>
        /// <param name="application">Controlled application of to be loaded to Revit process.</param>
        /// <returns>Return the status of the external application.</returns>
        public Autodesk.Revit.UI.Result OnStartup(UIControlledApplication application)
        {
            // Register related events
            m_eventsReactor = new EventsReactor();
            application.ControlledApplication.ViewPrinting += new EventHandler<Autodesk.Revit.DB.Events.ViewPrintingEventArgs>(m_eventsReactor.AppViewPrinting);
            application.ControlledApplication.ViewPrinted += new EventHandler<Autodesk.Revit.DB.Events.ViewPrintedEventArgs>(m_eventsReactor.AppViewPrinted);
            application.ControlledApplication.DocumentPrinting += new EventHandler<Autodesk.Revit.DB.Events.DocumentPrintingEventArgs>(m_eventsReactor.AppDocumentPrinting);
            application.ControlledApplication.DocumentPrinted += new EventHandler<Autodesk.Revit.DB.Events.DocumentPrintedEventArgs>(m_eventsReactor.AppDocumentPrinted);
            return Autodesk.Revit.UI.Result.Succeeded;
        }

        /// <summary>
        /// Implement this method to unregister the subscribed events when Revit exits.
        /// </summary>
        /// <param name="application">Controlled application to be shutdown.</param>
        /// <returns>Return the status of the external application.</returns>
        public Autodesk.Revit.UI.Result OnShutdown(UIControlledApplication application)
        {
            // just close log file and return success
            m_eventsReactor.CloseLogFiles();
            //
            // unregister events
            application.ControlledApplication.ViewPrinting -= new EventHandler<Autodesk.Revit.DB.Events.ViewPrintingEventArgs>(m_eventsReactor.AppViewPrinting);
            application.ControlledApplication.ViewPrinted -= new EventHandler<Autodesk.Revit.DB.Events.ViewPrintedEventArgs>(m_eventsReactor.AppViewPrinted);
            application.ControlledApplication.DocumentPrinting -= new EventHandler<Autodesk.Revit.DB.Events.DocumentPrintingEventArgs>(m_eventsReactor.AppDocumentPrinting);
            application.ControlledApplication.DocumentPrinted -= new EventHandler<Autodesk.Revit.DB.Events.DocumentPrintedEventArgs>(m_eventsReactor.AppDocumentPrinted);
            return Autodesk.Revit.UI.Result.Succeeded;
        }
        #endregion
    }
}
