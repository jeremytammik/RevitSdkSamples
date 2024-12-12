//
// (C) Copyright 2003-2007 by Autodesk, Inc.
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
using System.Data;
using System.Collections.Generic;
using System.Text;

using Autodesk.Revit;
using Autodesk.Revit.Events;

namespace Revit.SDK.Samples.ApplicationEvents.CS
{
    /// <summary>
    /// A class inherits ApplicationEvents interface that supports the events from the Revit Application object. 
    /// </summary>
    public class RevitApplicationEvents : Autodesk.Revit.Events.ApplicationEvents
    {
        #region Class Member Variable
        // a data table used store the events history log and these data is the data source of dataGridView in UI
        private DataTable m_eventsLog;
        #endregion


        #region Class Property
        /// <summary>
        /// property to get and set private member variable m_eventsLog
        /// </summary>
        public DataTable EventsLog
        {
            get 
            { 
                return m_eventsLog;
            }
            set 
            { 
                m_eventsLog = value;
            }
        }
        #endregion 


        #region Class Constructor
        /// <summary>
        /// Subscribe some application events (document save, save as, open and close)
        /// </summary>
        /// <param name="app">Revit application to be subscribed</param>
        public RevitApplicationEvents(Application app)
        {
            if (null != app)
            {
                app.OnDocumentClosed  += new DocumentClosedEventHandler(OnDocumentClosed);
                app.OnDocumentOpened  += new DocumentOpenedEventHandler(OnDocumentOpened);
                app.OnDocumentSaved   += new DocumentSavedEventHandler(OnDocumentSaved);
                app.OnDocumentSavedAs += new DocumentSavedAsEventHandler(OnDocumentSavedAs);
            }

            m_eventsLog = CreateEventsLogTable();
        }
        #endregion


        #region ApplicationEvents Methods Implementation

        /// <summary>
        /// This event is fired whenever a Revit dialog box is displayed that 
        /// requires user interaction during an external api command. 
        /// </summary>
        /// <param name="dialogBoxData">
        /// An object that describes the dialog box. 
        /// </param>
        public void OnDialogBox(DialogBoxData dialogBoxData)
        {
            // no implementation for this method
        }

        /// <summary>
        /// This event is fired whenever a document is closed that 
        /// requires user interaction during an external api command. 
        /// </summary>
        /// <param name="document">The specified document which is closed</param>
        public void OnDocumentClosed(Document document)
        {
            WriteEventsLog("Document Closed", document);
        }

        /// <summary>
        /// This event is fired whenever a document is saved that 
        /// requires user interaction during an external api command. 
        /// </summary>
        /// <param name="document">The specified document which is saved</param>
        public void OnDocumentSaved(Document document)
        {
            WriteEventsLog("Document Saved", document);
        }

        /// <summary>
        /// This event is fired whenever a document is saved as another file that 
        /// requires user interaction during an external api command. 
        /// </summary>
        /// <param name="document">The specified document which is saved as</param>
        public void OnDocumentSavedAs(Document document)
        {
            WriteEventsLog("Document Saved as", document);
        }

        /// <summary>
        /// This event is fired whenever a document is opened that 
        /// requires user interaction during an external api command. 
        /// </summary>
        /// <param name="document">The specified document which is opened</param>
        public void OnDocumentOpened(Document document)
        {
            WriteEventsLog("Document Opened", document);
        }

        /// <summary>
        /// This event is fired whenever a new document is created that 
        /// requires user interaction during an external api command. 
        /// </summary>
        /// <param name="document">The specified document which is created</param>
        public void OnDocumentNewed(Document document)
        {
            // no implementation for this method
        }
        #endregion


        #region Class Implementation
        /// <summary>
        /// Generate a data table with four columns for display in window
        /// </summary>
        /// <returns>The DataTable to be displayed in window</returns>
        static private DataTable CreateEventsLogTable()
        {
            // create a new dataTable
            DataTable eventsInfoLogTable = new DataTable("EventsLogInfoTable");

            // Create a Machine column and add to the DataTable.
            DataColumn machineiColumn = new DataColumn("Machine", typeof(System.String));
            machineiColumn.Caption    = "Machine";
            eventsInfoLogTable.Columns.Add(machineiColumn);

            // Create a User column
            DataColumn userColumn = new DataColumn("User", typeof(System.String));
            userColumn.Caption    = "User";
            eventsInfoLogTable.Columns.Add(userColumn);

            // Create a "Time" column
            DataColumn timeColumn = new DataColumn("Time", typeof(System.String));
            timeColumn.Caption    = "Time";
            eventsInfoLogTable.Columns.Add(timeColumn);

            // Create a "Event" column
            DataColumn eventColum = new DataColumn("Event", typeof(System.String));
            eventColum.Caption    = "Event";
            eventsInfoLogTable.Columns.Add(eventColum);

            // return this data table 
            return eventsInfoLogTable;
        }

        /// <summary>
        /// When any event which has been subscribed is fired, log its information.
        /// the information includes: machine name, user, time and event
        /// </summary>
        /// <param name="eventType"> string to indicates the event type</param>
        /// <param name="doc">document which rises subscribed events</param>
        private void WriteEventsLog(string eventType, Document doc)
        {
            DataRow newRow = m_eventsLog.NewRow();

            // set the relative information of this event into the table.
            newRow["Machine"] = System.Environment.MachineName;
            newRow["User"]    = System.Environment.UserName;
            newRow["Time"]    = System.DateTime.Now.ToString();
            newRow["Event"]   = eventType + ": " + doc.Title;

            m_eventsLog.Rows.Add(newRow);
        }
        #endregion
    }
}
