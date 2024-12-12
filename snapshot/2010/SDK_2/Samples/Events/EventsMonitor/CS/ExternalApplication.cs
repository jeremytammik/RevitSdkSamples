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
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

using Autodesk.Revit;

namespace Revit.SDK.Samples.EventsMonitor.CS
{
    /// <summary>
    /// A class inherits IExternalApplication interface and provide an entry of the sample.
    /// This class controls other function class and plays the bridge role in this sample.
    /// It create a custom menu "Track Revit Events" of which the corresponding 
    /// external command is the command in this project.
    /// </summary>
    public class ExternalApplication : IExternalApplication
    {
        #region Class Member Variables
        /// <summary>
        /// A controlled application used to register the events. Because all trigger points
        /// in this sample come from UI, all events in application level must be registered 
        /// to ControlledApplication. If the trigger point is from API, user can register it 
        /// to Application or Document according to what level it is in. But then, 
        /// the syntax is the same in these three cases.
        /// </summary>
        private static ControlledApplication m_ctrlApp;

        /// <summary>
        /// A common object used to write the log file and generate the data table 
        /// for event information.
        /// </summary>
        private static LogManager m_logManager;

        /// <summary>
        /// The window is used to show events' information.
        /// </summary>
        private static EventsInfoWindows m_infWindows;

        /// <summary>
        /// The window is used to choose what event to be subscribed.
        /// </summary>
        private static EventsSettingForm m_settingDialog;

        /// <summary>
        /// This list is used to store what user selected.
        /// It can be got from the selectionDialog.
        /// </summary>
        private static List<String> m_appEventsSelection;

        /// <summary>
        /// This object is used to manager the events in application level.
        /// It can be updated according to what user select.
        /// </summary>
        private static EventManager m_appEventMgr;

        #if (JOURNAL_ENABLED)
        /// <summary>
        /// This object is used to make the sample can be autotest.
        /// It can dump the event list to file or commandData.Data
        /// and also can retrieval the list from file and commandData.Data.
        /// If you just pay attention to how to use events, 
        /// please skip over this class and related sentence.
        /// </summary>
        private static JournalProcessor m_journalProcessor;
        #endif
        #endregion

        #region Class Static Property
        /// <summary>
        /// Property to get and set private member variable of InfoWindows
        /// </summary>
        public static EventsInfoWindows InfoWindows
        {
            get
            {
                if (null == m_infWindows)
                {
                    m_infWindows = new EventsInfoWindows(EventLogManager);
                }
                return m_infWindows;
            }
            set
            {
                m_infWindows = value;
            }
        }

        /// <summary>
        /// Property to get private member variable of SeletionDialog
        /// </summary>
        public static EventsSettingForm SettingDialog
        {
            get
            {
                if (null == m_settingDialog)
                {
                    m_settingDialog = new EventsSettingForm();
                }
                return m_settingDialog;
            }
        }

        /// <summary>
        /// Property to get and set private member variable of log data
        /// </summary>
        public static LogManager EventLogManager
        {
            get
            {
                if (null == m_logManager)
                {
                    m_logManager = new LogManager();
                }
                return m_logManager;
            }
        }

        /// <summary>
        /// Property to get and set private member variable of application events selection.
        /// </summary>
        public static List<String> ApplicationEvents
        {
            get
            {
                if (null == m_appEventsSelection)
                {
                    m_appEventsSelection = new List<string>();
                }
                return m_appEventsSelection;
            }
            set
            {
                m_appEventsSelection = value;
            }
        }

        /// <summary>
        /// Property to get private member variable of application events manager.
        /// </summary>
        public static EventManager AppEventMgr
        {
            get
            {
                if (null == m_appEventMgr)
                {
                    m_appEventMgr = new EventManager(m_ctrlApp);
                }
                return m_appEventMgr;
            }
        }

        #if (JOURNAL_ENABLED)
        /// <summary>
        /// Property to get private member variable of journal processor.
        /// </summary>
        public static JournalProcessor JnlProcessor
        {
            get
            {
                if (null == m_journalProcessor)
                {
                    m_journalProcessor = new JournalProcessor();
                }
                return m_journalProcessor;
            }
        }
        #endif
        #endregion

        #region IExternalApplication Members

        /// <summary>
        /// Implement this method to implement the external application which should be called when 
        /// Revit starts before a file or default template is actually loaded.
        /// </summary>
        /// <param name="application">An object that is passed to the external application 
        /// which contains the controlled application.</param> 
        /// <returns>Return the status of the external application. 
        /// A result of Succeeded means that the external application successfully started. 
        /// Cancelled can be used to signify that the user cancelled the external operation at 
        /// some point.
        /// If false is returned then Revit should inform the user that the external application 
        /// failed to load and the release the internal reference.</returns>
        public IExternalApplication.Result OnStartup(ControlledApplication application)
        {
            // initialize member variables.
            m_ctrlApp = application;
            m_logManager = new LogManager();
            m_infWindows = new EventsInfoWindows(m_logManager);
            m_settingDialog = new EventsSettingForm();
            m_appEventsSelection = new List<string>();
            m_appEventMgr = new EventManager(m_ctrlApp);

            #if (JOURNAL_ENABLED)
            m_journalProcessor = new JournalProcessor();
            #endif

            try
            {
                #if (JOURNAL_ENABLED)
                // playing journal.
                if (m_journalProcessor.IsReplay)
                {
                    m_appEventsSelection = m_journalProcessor.EventsList;
                }
                
                // running the sample form UI.
                else
                {
                #endif
                    m_settingDialog.ShowDialog();
                    if (DialogResult.OK == m_settingDialog.DialogResult)
                    {
                        //get what user select.
                        m_appEventsSelection = m_settingDialog.AppSelectionList;
                    }
                #if (JOURNAL_ENABLED)
                    // dump what user select to a file in order to autotesting.
                    m_journalProcessor.DumpEventsListToFile(m_appEventsSelection);
                }
                #endif
                
                // update the events according to the selection.
                m_appEventMgr.Update(m_appEventsSelection);

                // track the selected events by showing the information in the information windows.
                m_infWindows.Show();
              
                // add menu item in Revit menu bar to provide an approach to 
                // retrieve events setting form. User can change his choices 
                // by calling the setting form again.
                AddCustomPanel(application);
            }
            catch (Exception)
            {
                return IExternalApplication.Result.Failed;
            }

            return IExternalApplication.Result.Succeeded;
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
        public IExternalApplication.Result OnShutdown(ControlledApplication application)
        {
            // dispose some resource.
            Dispose();
            return IExternalApplication.Result.Succeeded;
        }
        #endregion

        #region Class Methods
        /// <summary>
        /// Dispose some resource.
        /// </summary>
        public static void Dispose()
        {
            if (m_infWindows != null)
            {
                m_infWindows.Close();
                m_infWindows = null;
            }
            if (m_settingDialog != null)
            {
                m_settingDialog.Close();
                m_settingDialog = null;
            }
            m_appEventMgr = null;
            m_logManager = null;
        }

        /// <summary>
        /// Add custom menu.
        /// </summary>
        static private void AddCustomPanel(ControlledApplication application)
        {
            // create a panel named "Events Monitor";
            string panelName = "Events Monitor";
            // create a button on the panel.
            RibbonPanel ribbonPanelPushButtons = application.CreateRibbonPanel(panelName);
            PushButton pushButtonCreateWall = ribbonPanelPushButtons.AddPushButton("EventsSetting", 
                "Set Events", System.Reflection.Assembly.GetExecutingAssembly().Location, 
                "Revit.SDK.Samples.EventsMonitor.CS.Command");
            pushButtonCreateWall.ToolTip = "Setting Events";
  
        }
        #endregion
    }
}
