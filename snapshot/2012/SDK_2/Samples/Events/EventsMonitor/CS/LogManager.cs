//
// (C) Copyright 2003-2011 by Autodesk, Inc.
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
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Reflection;

using Autodesk.Revit;
using Autodesk.Revit.DB.Events;

namespace Revit.SDK.Samples.EventsMonitor.CS
{
    /// <summary>
    /// This class implements all operators about writing log file and generating event 
    /// information for showing in the information window. This class is not the main one 
    /// just a assistant in this sample. If you just want to learn how to use Revit events,
    /// please pay more attention to EventManager class.
    /// </summary>
    public class LogManager
    {
        #region Class Member Variables
        /// <summary>
        /// data table for information windows.
        /// </summary>
        private DataTable m_eventsLog;

        /// <summary>
        /// add a trace listener.
        /// </summary>
        private TraceListener m_txtListener;

        /// <summary>
        /// path of temp file and log file
        /// </summary>
        private string m_filePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        /// <summary>
        /// a temp file to record log and before revit closed the temp file will be change to log file.
        /// This strategy can make the log file alway can be accessable.
        /// </summary>
        private string m_tempFile;
        #endregion

        #region Class Property
        /// <summary>
        /// Property to get and set private member variables of Event log information.
        /// </summary>
        public DataTable EventsLog
        {
            get
            {
                return m_eventsLog;
            }
        }
        #endregion

        #region Class Constructor and Destructor
        /// <summary>
        /// Constructor without argument.
        /// </summary>
        public LogManager()
        {
            //m_filePath = ;
            CreateLogFile();
            m_eventsLog = CreateEventsLogTable();
        }

        #endregion

        #region Class Methods
        /// <summary>
        /// Create a log file to track the subscribed events' work process.
        /// </summary>
        private void CreateLogFile()
        {
            m_tempFile = Path.Combine(m_filePath, "Temp.log");
            if (File.Exists(m_tempFile)) File.Delete(m_tempFile);
            m_txtListener = new TextWriterTraceListener(m_tempFile);
            Trace.Listeners.Add(m_txtListener);
        }

        /// <summary>
        /// Close the log file and remove the corresponding listener.
        /// </summary>
        public void CloseLogFile()
        {
            Trace.Flush();
            Trace.Listeners.Remove(m_txtListener);
            Trace.Close();
            m_txtListener.Close();

            string log = Path.Combine(m_filePath, "EventsMonitor.log");
            if (File.Exists(log)) File.Delete(log);
            File.Copy(m_tempFile, log);
            File.Delete(m_tempFile);
        }

        /// <summary>
        /// Generate a data table with four columns for display in window
        /// </summary>
        /// <returns>The DataTable to be displayed in window</returns>
        private DataTable CreateEventsLogTable()
        {
            // create a new dataTable
            DataTable eventsInfoLogTable = new DataTable("EventsLogInfoTable");

            // Create a "Time" column
            DataColumn timeColumn = new DataColumn("Time", typeof(System.String));
            timeColumn.Caption = "Time";
            eventsInfoLogTable.Columns.Add(timeColumn);

            // Create a "Event" column
            DataColumn eventColum = new DataColumn("Event", typeof(System.String));
            eventColum.Caption = "Event";
            eventsInfoLogTable.Columns.Add(eventColum);

            // Create a "Type" column
            DataColumn typeColum = new DataColumn("Type", typeof(System.String));
            typeColum.Caption = "Type";
            eventsInfoLogTable.Columns.Add(typeColum);

            // return this data table 
            return eventsInfoLogTable;
        }

        /// <summary>
        /// When any event which has been subscribed is fired, log its information.
        /// the information includes: machine name, user, time and event
        /// </summary>
        /// <param name="sender"> Event sender.</param>
        /// <param name="args">EventArgs of this event.</param>
        public void TrackEvent(Object sender, EventArgs args)
        {
            DataRow newRow = m_eventsLog.NewRow();

            // set the relative information of this event into the table.
            newRow["Time"] = System.DateTime.Now.ToString();
            newRow["Event"] = GetEventsName(args.GetType());
            newRow["Type"] = sender.GetType().ToString();

            m_eventsLog.Rows.Add(newRow);
        }

        /// <summary>
        /// Write log to file, event name and type will be dumped
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="args">EventArgs of this event.</param>
        public void WriteLogFile(Object sender, EventArgs args)
        {
            Trace.WriteLine("*********************************************************");
            if (null == args)
            {
                return;
            }
            // get this eventArgs's runtime type.
            Type type = args.GetType();
            String eventName = GetEventsName(type);
            Trace.WriteLine("Raised " + sender.GetType().ToString() + "." + eventName);
            Trace.WriteLine("---------------------------------------------------------");
            
            // 1: output sender's information
            Trace.WriteLine("  Start to dump Sender and EventAgrs of Event...\n");
            if (null != sender)
            {
                // output the type of sender
                Trace.WriteLine("    [Event Sender]: " + sender.GetType().FullName);
            }
            else
            {
                Trace.WriteLine("      Sender is null, it's unexpected!!!");
            }

            // 2: Output event argument
            // get all properties info of this argument.
            PropertyInfo[] propertyInfos = type.GetProperties();

            // output some typical property's name and value. (for example, Cancelable, Cancel,etc)
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                try
                {
                    if (!propertyInfo.CanRead)
                    {
                        continue;
                    }
                    else
                    {
                        Object propertyValue;
                        String propertyName = propertyInfo.Name;
                        switch(propertyName)
                        {
                            case "Document":
                            case "Cancellable":
                            case "Cancel":
                            case "Status":
                            case "DocumentType":
                            case "Format":
                            propertyValue = propertyInfo.GetValue(args, null);
                            // Dump current property value
                            Trace.WriteLine("    [Property]: " + propertyInfo.Name);
                            Trace.WriteLine("    [Value]: " + propertyValue.ToString());
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Unexpected exception
                    Trace.WriteLine("    [Property Exception]: " + propertyInfo.Name + ", " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Get event name from its EventArgs, without namespace prefix
        /// </summary>
        /// <param name="type">Generic event type.</param>
        /// <returns></returns>
        private String GetEventsName(Type type)
        {
            String argName = type.ToString();
            String tail = "EventArgs";
            String head = "Autodesk.Revit.DB.Events.";
            int firstIndex = head.Length;
            int length = argName.Length - head.Length - tail.Length;
            String eventName = argName.Substring(firstIndex, length);
            return eventName;
        }
        #endregion
    }
}
