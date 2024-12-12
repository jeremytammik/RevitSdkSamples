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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Revit.SDK.Samples.EventsMonitor.CS
{
    /// <summary>
    /// The UI to show the events history logs. This class is not the main one just a assistant
    /// in this sample. If you just want to learn how to use Revit events,
    /// please pay more attention to EventManager class.
    /// </summary>
    public partial class EventsInfoWindows : System.Windows.Forms.Form
    {
        #region Class Member Variable
        /// <summary>
        /// An instance of RevitApplicationEvents class
        /// Which prepares the informations which is shown in this UI
        /// </summary>
        private LogManager m_dataBuffer;
        #endregion

        #region Class Constructors
        /// <summary>
        /// Constructor without any argument
        /// </summary>
        public EventsInfoWindows()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor with one argument
        /// </summary>
        /// <param name="dataBuffer">prepare the informations which is shown in this UI</param>
        public EventsInfoWindows(LogManager dataBuffer)
            : this()
        {
            m_dataBuffer = dataBuffer;
            Initialize();
        }
        #endregion 

        #region Class Implementation
        /// <summary>
        /// Initialize the DataGridView property
        /// </summary>
        private void Initialize()
        {            
            // set dataSource
            appEventsLogDataGridView.AutoGenerateColumns = false;
            appEventsLogDataGridView.DataSource          = m_dataBuffer.EventsLog;
            timeColumn.DataPropertyName                  = "Time";
            eventColumn.DataPropertyName                 = "Event";
            typeColumn.DataPropertyName                  = "Type";
        }
        #endregion 

        #region Class Events Handler
        /// <summary>
        /// form closed event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InformationWindows_FormClosed(object sender, FormClosedEventArgs e)
        {
            // when form closed the relevant resource will be set free. 
            // Then the instance InfoWindows become invalid, so we set InfoWindows with null.
            ExternalApplication.InfoWindows = null;
        }

        /// <summary>
        /// windows shown event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void applicationEventsInfoWindows_Shown(object sender, EventArgs e)
        {
            // set window's display location
            int left             = Screen.PrimaryScreen.WorkingArea.Right - this.Width - 5;
            int top              = Screen.PrimaryScreen.WorkingArea.Bottom - this.Height;
            Point windowLocation = new Point(left, top);
            this.Location        = windowLocation;
        }

        /// <summary>
        /// Scroll to last line when add new log lines
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void appEventsLogDataGridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            appEventsLogDataGridView.CurrentCell = appEventsLogDataGridView.Rows[appEventsLogDataGridView.Rows.Count - 1].Cells[0];
        }
        #endregion
    }
}