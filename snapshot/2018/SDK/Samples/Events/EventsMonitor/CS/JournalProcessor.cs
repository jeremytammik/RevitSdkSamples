//
// (C) Copyright 2003-2017 by Autodesk, Inc.
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
using System.Xml.Serialization;

using Autodesk.Revit;
using Autodesk.Revit.DB.Events;

namespace Revit.SDK.Samples.EventsMonitor.CS
{
    /// <summary>
    /// This class is implemented to make sample autotest.
    /// It checks whether the external file exists or not.
    /// The sample can control the UI's display by this judgement.
    /// It can dump the seleted event list to file or commandData.Data
    /// and also can retrieve the list from file and commandData.Data.
    /// </summary>
    public class JournalProcessor
    {
        #region Class Member Variables
        /// <summary>
        /// xml file name.
        /// </summary>
        private string m_xmlFile;

        /// <summary>
        /// xml serializer.
        /// </summary>
        private XmlSerializer m_xs;

        /// <summary>
        /// using UI or playing journal.
        /// </summary>
        private bool m_isReplay;

        /// <summary>
        /// events deserialized from xml file.
        /// </summary>
        private List<String> m_eventsInFile;

        /// <summary>
        /// direcotory of xml file.
        /// </summary>
        private string m_directory;
        #endregion

        #region Class Property
        /// <summary>
        /// Property to get private member variables to check the stauts is playing or recording.
        /// </summary>
        public bool IsReplay
        {
            get
            {
                return m_isReplay;
            }
        }

        /// <summary>
        /// Property to get private member variables of Event list.
        /// </summary>
        public List<String> EventsList
        {
            get
            {
                return m_eventsInFile;
            }
        }
        #endregion

        #region Class Constructor and Destructor
        /// <summary>
        /// Constructor without argument.
        /// </summary>
        public JournalProcessor()
        {
            m_xs = new XmlSerializer(typeof(List<String>));
            m_directory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            m_xmlFile = Path.Combine(m_directory,"Current.xml");
            
            // if the external file is exist, it means playing journal now. No Setting Dialog will pop up.
            m_isReplay = CheckFileExistence();

            // get event list from xml file.
            GetEventsListFromFile();
        }
        #endregion

        #region Class Methods
        /// <summary>
        /// Check whether the xml file is exist or not.
        /// </summary>
        /// <returns></returns>
        private bool CheckFileExistence()
        {
            return File.Exists(m_xmlFile) ? true : false;
        }

        /// <summary>
        /// Get the event list from xml file.
        /// This method is used in ExternalApplication.
        /// </summary>
        private void GetEventsListFromFile()
        {
            if (m_isReplay)
            {
                Stream stream = new FileStream(m_xmlFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                m_eventsInFile = (List<String>)m_xs.Deserialize(stream);
                stream.Close();
            }
            else
            {
                m_eventsInFile = new List<string>();
            }
        }

        /// <summary>
        /// Dump the selected event list to xml file.
        /// This method is used in ExternalApplication.
        /// </summary>
        /// <param name="eventList"></param>
        public void DumpEventsListToFile(List<String> eventList)
        {
            if (!m_isReplay)
            {
                string fileName =  DateTime.Now.ToString("yyyyMMdd") + ".xml";
                string tempFile =Path.Combine(m_directory ,fileName);
                Stream stream = new FileStream(tempFile, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                m_xs.Serialize(stream, eventList);
                stream.Close();
            }
        }

        /// <summary>
        /// Get event list from commandData.Data.
        /// This method is used in ExternalCommmand.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<String> GetEventsListFromJournalData(IDictionary<String, String> data)
        {
            List<String> eventList = new List<string>();
            foreach (KeyValuePair<string, string> kvp in data)
            {
               eventList.Add(kvp.Key);
            }
            return eventList;
        }

        /// <summary>
        /// Dump the selected event list to commandData.Data.
        /// This method is used in ExternalCommand.
        /// </summary>
        /// <param name="eventList"></param>
        /// <param name="data"></param>
        public void DumpEventListToJournalData(List<String> eventList, ref IDictionary<String, String> data)
        {
            foreach (String eventname in eventList)
            {
                data.Add(eventname, "1");
            }
        }
        #endregion
    }
}
 