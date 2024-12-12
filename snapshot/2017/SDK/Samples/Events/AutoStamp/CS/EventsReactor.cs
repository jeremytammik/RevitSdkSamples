//
// (C) Copyright 2003-2016 by Autodesk, Inc.
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
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;

using Autodesk.Revit;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.DB;

namespace Revit.SDK.Samples.AutoStamp.CS
{
    /// <summary>
    /// This class consists of two handler methods which will be subscribed to ViewPrinting and ViewPrinted events separately,
    /// The pre-event handler will create TextNote element when event is raised, and the post-event handler will delete it.
    /// Meanwhile, these two handler methods will be used to dump designed information to log file.
    /// 
    /// It contains other methods which are used to dump related information(like events arguments and sequences) 
    /// to log file PrintEventsLog.txt.
    /// </summary>
    public sealed class EventsReactor
    {
        #region Class Member Variables
        /// <summary>
        /// This listener is used to monitor the events raising sequences and arguments of events.
        /// it will be bound to log file PrintEventsLog.txt, it will be added to Trace.Listeners.
        /// 
        /// This log file will only contain information of event raising sequence, event arguments, etc.
        /// This file can be used to check if events work well in different platforms, for example:
        /// By this sample, if user printed something, Revit journal will record all operation of users, 
        /// meanwhile PrintEventsLog.txt will be generated. If user run the journal in other machine, user will get another 
        /// PrintEventsLog.txt, by comparing the two files user can figure out easily if the two prints work equally.
        /// </summary>
        private TextWriterTraceListener m_eventsLog;

        /// <summary>
        /// Current assembly path
        /// </summary>
        String m_assemblyPath;

        /// <summary>
        /// Reserves the id of TextNote created by ViewPrinting and delete it in ViewPrinted event.
        /// </summary>
        Autodesk.Revit.DB.ElementId m_newTextNoteId; 
        #endregion


        #region Class Constructor Method
        /// <summary>
        /// Constructor method, it will only initialize m_assemblyPath.
        /// Notice that this method won't open log files at this time.
        /// </summary>
        public EventsReactor()
        {
            // Get assembly path 
            m_assemblyPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        }

        /// <summary>
        ///  Close log files now
        /// </summary>
        public void CloseLogFiles()
        {
            // Flush trace and close it
            Trace.Flush();
            Trace.Close();
            //
            // Close listeners 
            Trace.Flush();
            if (null != m_eventsLog)
            {
                Trace.Listeners.Remove(m_eventsLog);
                m_eventsLog.Flush();
                m_eventsLog.Close();
            }
        }
        #endregion


        #region Class Handler Methods
        /// <summary>
        /// Handler method for ViewPrinting event.
        /// This method will dump EventArgument information of event firstly and then create TextNote element for this view.
        /// View print will be canceled if TextNote creation failed.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments of ViewPrinting event.</param>
        public void AppViewPrinting(object sender, Autodesk.Revit.DB.Events.ViewPrintingEventArgs e)
        {
            // Setup log file if it still is empty
            if (null == m_eventsLog)
            {
                SetupLogFiles(); 
            } 
            //
            // header information
            Trace.WriteLine(System.Environment.NewLine + "View Print Start: ------------------------");
            //
            // Dump the events arguments
            DumpEventArguments(e);
            //
            // Create TextNote for current view, cancel the event if TextNote creation failed
            bool failureOccured = false; // Reserves whether failure occurred when create TextNote
            try
            {
                String strText = String.Format("Printer Name: {0}{1}User Name: {2}",
                    e.Document.PrintManager.PrinterName, System.Environment.NewLine, System.Environment.UserName);
                //
                // Use non-debug compile symbol to write constant text note
#if !(Debug || DEBUG)
                strText = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
#endif
                using( Transaction eventTransaction = new Transaction(e.Document, "External Tool"))
                {
                    eventTransaction.Start();
                    TextNoteOptions options = new TextNoteOptions();
                    options.HorizontalAlignment = HorizontalTextAlignment.Center;
                    options.TypeId = e.Document.GetDefaultElementTypeId(ElementTypeGroup.TextNoteType);
                    TextNote newTextNote = TextNote.Create(e.Document, e.View.Id, XYZ.Zero, strText, options);
                    eventTransaction.Commit();

                    // Check to see whether TextNote creation succeeded
                    if(null != newTextNote)
                    {
                        Trace.WriteLine("Create TextNote element successfully...");
                        m_newTextNoteId = newTextNote.Id;
                    }
                    else
                    {
                       failureOccured = true;
                    }
                }
            }
            catch (System.Exception ex)
            {
               failureOccured = true;
                Trace.WriteLine("Exception occurred when creating TextNote, print will be canceled, ex: " + ex.Message);
            }
            finally
            {
                // Cancel the TextNote creation when failure occurred, meantime the event is cancellable
               if (failureOccured && e.Cancellable)
                {
                    e.Cancel();  
                }
            }
        }

        /// <summary>
        /// Handler method for ViewPrinted event.
        /// This handler will dump information of printed view firstly and then delete the TextNote created in pre-event handler.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments of ViewPrinted event.</param>
        public void AppViewPrinted(object sender, Autodesk.Revit.DB.Events.ViewPrintedEventArgs e)
        {
            // header information
            Trace.WriteLine(System.Environment.NewLine + "View Print End: -------");
            //
            // Dump the events arguments 
            DumpEventArguments(e);
            //
            // Delete the TextNote element created in ViewPrinting only when view print process succeeded or failed.
            // We don't care about the delete when event status is Cancelled because no TextNote element
            // will be created when cancelling occurred.
            if(RevitAPIEventStatus.Cancelled != e.Status)
            {
               //now event framework will not provide transaction, user need start by self(2009/11/18)
               Transaction eventTransaction = new Transaction(e.Document, "External Tool");
               eventTransaction.Start();
               e.Document.Delete(m_newTextNoteId);
               eventTransaction.Commit();
               Trace.WriteLine("Succeeded to delete the created TextNote element.");
            }
        }               
        #endregion 


        #region Class Implementations
        /// <summary>
        /// For singleton consideration, setup log file only when ViewPrinting is raised.
        /// m_eventsLog will be initialized and added to Trace.Listeners, 
        /// PrintEventsLog.txt will be removed if it already existed.
        /// </summary>
        private void SetupLogFiles()
        {
            // singleton instance for log file
            if (null != m_eventsLog)
            {
                return;
            }
            //
            // delete existed log files
            String printEventsLogFile = Path.Combine(m_assemblyPath, "PrintEventsLog.txt");
            if (File.Exists(printEventsLogFile))
            {
                File.Delete(printEventsLogFile);
            }
            //
            // Create listener and add to Trace.Listeners to monitor the string to be emitted
            m_eventsLog = new TextWriterTraceListener(printEventsLogFile);
            Trace.Listeners.Add(m_eventsLog);
            Trace.AutoFlush = true; // set auto flush to ensure the emitted string can be dumped in time
        }

        /// <summary>
        /// Dump the events arguments to log file PrintEventsLog.txt.
        /// This method will only dump EventArguments of ViewPrint, two event arguments will be handled:
        /// ViewPrintingEventArgs and ViewPrintedEventArgs.
        /// Typical properties of EventArgs of them will be dumped to log file.
        /// </summary>
        /// <param name="eventArgs">Event argument to be dumped. </param>
        private static void DumpEventArguments(RevitAPIEventArgs eventArgs)
        {
            // Dump parameters now:
            // white space is for align purpose.
            if (eventArgs.GetType().Equals(typeof(ViewPrintingEventArgs)))
            {
                Trace.WriteLine("ViewPrintingEventArgs Parameters ------>");
                ViewPrintingEventArgs args = eventArgs as ViewPrintingEventArgs;
                Trace.WriteLine("    TotalViews          : " + args.TotalViews);
                Trace.WriteLine("    View Index          : " + args.Index);
                Trace.WriteLine("    View Information    :"); 
                DumpViewInfo(args.View, "      ");
            }
            else if (eventArgs.GetType().Equals(typeof(ViewPrintedEventArgs)))
            {
                Trace.WriteLine("ViewPrintedEventArgs Parameters ------>");
                ViewPrintedEventArgs args = eventArgs as ViewPrintedEventArgs;
                Trace.WriteLine("    Event Status        : " + args.Status);
                Trace.WriteLine("    TotalViews          : " + args.TotalViews);
                Trace.WriteLine("    View Index          : " + args.Index); 
                Trace.WriteLine("    View Information    :"); 
                DumpViewInfo(args.View, "      ");
            }
            else
            {
                // no handling for other arguments
            }
        }

        /// <summary>
        /// Dump information of  view(View name and type) to log file.
        /// </summary>
        /// <param name="view">View element to be dumped to log files.</param>
        /// <param name="prefix">Prefix mark for each line dumped to log files.</param>
        private static void DumpViewInfo(View view, String prefix)
        {
            Trace.WriteLine(String.Format("{0} ViewName: {1}, ViewType: {2}", 
                prefix, view.ViewName, view.ViewType));
        }
        #endregion
    }
}
