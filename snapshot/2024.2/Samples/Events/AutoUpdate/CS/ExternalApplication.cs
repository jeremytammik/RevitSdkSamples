//
// (C) Copyright 2003-2019 by Autodesk, Inc.
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
using System.Text;
using System.IO;
using System.Diagnostics;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.AutoUpdate.CS
{
    /// <summary>
    /// This class implements IExternalApplication and demonstrate how to subscribe event 
    /// and modify model in event handler. We just demonstrate changing 
    /// the ProjectInformation.Address property, but user can do other modification for 
    /// current project, like delete element, create new element, etc.
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class ExternalApplication : IExternalApplication
    {
        #region Class Member Variables
        /// <summary>
        /// This listener is used to monitor the events arguments and the result of the sample.
        /// It will be bound to log file AutoUpdate.log, it will be added to Trace.Listeners.
        /// </summary>
        private TextWriterTraceListener m_txtListener;
        
        /// <summary>
        /// get assembly path.
        /// </summary>
        private static String m_directory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        
        /// <summary>
        /// create the temp file name.
        /// </summary>
        private String m_tempFile = Path.Combine(m_directory, "temp.log");
 
        #endregion

        #region IExternalApplication Members
        /// <summary>
        /// Implement this method to subscribe event.
        /// </summary>
        /// <param name="application">the controlled application.</param>
        /// <returns>Return the status of the external application. 
        /// A result of Succeeded means that the external application successfully started. 
        /// Cancelled can be used to signify that the user cancelled the external operation at 
        /// some point.
        /// If false is returned then Revit should inform the user that the external application 
        /// failed to load and the release the internal reference.</returns>
        public Autodesk.Revit.UI.Result OnStartup(UIControlledApplication application)
        {
            try
            {
                // Create a temp file to dump the log, and copy this file to log file 
                // at the end of the sample.
                CreateTempFile();

                // Register event. In this sample, we trigger this event from UI, so it must 
                // be registered on ControlledApplication. 
                application.ControlledApplication.DocumentOpened += new EventHandler
                    <Autodesk.Revit.DB.Events.DocumentOpenedEventArgs>(application_DocumentOpened);            
            }
            catch (Exception)
            {
                return Autodesk.Revit.UI.Result.Failed;
            }

            return Autodesk.Revit.UI.Result.Succeeded;
        }

        /// <summary>
        /// Implement this method to implement the external application which should be called when 
        /// Revit is about to exit,Any documents must have been closed before this method is called.
        /// </summary>
        /// <param name="application">An object that is passed to the external application 
        /// which contains the controlled application.</param>
        /// <returns>Return the status of the external application. 
        /// A result of Succeeded means that the external application successfully shutdown. 
        /// Cancelled can be used to signify that the user cancelled the external operation at 
        /// some point.
        /// If false is returned then the Revit user should be warned of the failure of the external 
        /// application to shut down correctly.</returns>
        public Autodesk.Revit.UI.Result OnShutdown(UIControlledApplication application)
        {
            // remove the event.
            application.ControlledApplication.DocumentOpened -= application_DocumentOpened;
            CloseLogFile();
            return Autodesk.Revit.UI.Result.Succeeded;
        }
        #endregion

        #region Event Handler
        /// <summary>
        /// This is the event handler. We modify the model and dump log in this method.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void application_DocumentOpened(object sender, DocumentOpenedEventArgs args)
        {
            // dump the args to log file.
            DumpEventArgs(args);

            // get document from event args.
            Document doc = args.Document;

            if (doc.IsFamilyDocument) 
            {
                return; 
            }            
            try
            {
               //now event framework will not provide transaction,user need start by self(2009/11/18)
               Transaction eventTransaction = new Transaction(doc, "Event handler modify project information");
               eventTransaction.Start();
               // assign specified value to ProjectInformation.Address property. 
               // User can change another properties of document or create(delete) something as he likes.
               // Please pay attention that ProjectInformation property is empty for family document.
               // But it isn't the correct usage. So please don't run this sample on family document.
               doc.ProjectInformation.Address =
                   "United States - Massachusetts - Waltham - 610 Lincoln St";
               eventTransaction.Commit();
            }
            catch (Exception ee)
            {
               Trace.WriteLine("Failed to modify project information!-"+ee.Message);
            }
            // write the value to log file to check whether the operation is successful.
            Trace.WriteLine("The value after running the sample ------>");
            Trace.WriteLine("    [Address]         :" + doc.ProjectInformation.Address);
        }
        #endregion

        #region Class Methods
        /// <summary>
        /// Dump the events arguments to log file: AutoUpdat.log.
        /// </summary>
        /// <param name="args"></param>
        private void DumpEventArgs(DocumentOpenedEventArgs args)
        {
            Trace.WriteLine("DocumentOpenedEventArgs Parameters ------>");
            Trace.WriteLine("    Event Cancel      : " + args.IsCancelled()); // is it be cancelled?
            Trace.WriteLine("    Event Cancellable : " + args.Cancellable); // Cancellable
            Trace.WriteLine("    Status            : " + args.Status); // Status
        }

        /// <summary>
        /// Create a log file to track the subscribed events' work process.
        /// </summary>
        private void CreateTempFile()
        {
            if (File.Exists(m_tempFile)) File.Delete(m_tempFile);
            m_txtListener = new TextWriterTraceListener(m_tempFile);
            Trace.AutoFlush = true;
            Trace.Listeners.Add(m_txtListener);
        }

        /// <summary>
        /// Close the log file and remove the corresponding listener.
        /// </summary>
        private void CloseLogFile()
        {
            // close listeners now.
            Trace.Flush();
            Trace.Listeners.Remove(m_txtListener);
            Trace.Close();
            m_txtListener.Close();

            // copy temp file to log file and delete the temp file.
            String logFile = Path.Combine(m_directory, "AutoUpdate.log");
            if (File.Exists(logFile)) File.Delete(logFile);
            File.Copy(m_tempFile, logFile);
            File.Delete(m_tempFile);
        }
        #endregion
    }
}
