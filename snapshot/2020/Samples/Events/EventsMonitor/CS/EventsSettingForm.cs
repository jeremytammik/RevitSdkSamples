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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Autodesk.Revit;

namespace Revit.SDK.Samples.EventsMonitor.CS
{
    /// <summary>
    /// The UI to allow user to choose which event they want to subscribe.
    /// This class is not the main one, is just a assistant in this sample.
    /// If you just want to learn how to use Revit events, 
    /// please pay more attention to EventManager class.
    /// </summary>
    public partial class EventsSettingForm : System.Windows.Forms.Form
    {
        #region Class Member Variable
        /// <summary>
        /// A list to storage the selection user made
        /// </summary>
        private List<String> m_appSelection;
        #endregion

        #region Class property
        /// <summary>
        /// Property to get and set private member variables of SeletionMap
        /// </summary>
        public List<String> AppSelectionList
        {
            get
            {
                if (null == m_appSelection)
                {
                    m_appSelection = new List<string>();
                }
                return m_appSelection;
            }
            set
            {
                m_appSelection = value;
            }
        }

        #endregion

        #region Class Constructors
        /// <summary>
        /// Constructor without any argument
        /// </summary>
        public EventsSettingForm()
        {
            InitializeComponent();
            m_appSelection = new List<string>();
        }
        #endregion

        #region Class Events Handler
        /// <summary>
        /// Event handler for click OK button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FinishToggle_Click(object sender, EventArgs e)
        {
            // clear lists.
            m_appSelection.Clear();
            foreach (object item in AppEventsCheckedList.CheckedItems)
            {
                m_appSelection.Add(item.ToString());
            }
            this.DialogResult = DialogResult.OK;
            this.Hide();
        }

        /// <summary>
        /// Event handler for close windows
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToggleForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Hide();
        }

        /// <summary>
        /// Event handler for clicking check all button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkAllButton_Click(object sender, EventArgs e)
        {
                for (int i = 0; i < AppEventsCheckedList.Items.Count; i++)
                {
                    AppEventsCheckedList.SetItemChecked(i, true);
                }
           
        }

        /// <summary>
        /// Events handler for clicking check none button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkNoneButton_Click(object sender, EventArgs e)
        {
                for (int i = 0; i < AppEventsCheckedList.Items.Count; i++)
                {
                    AppEventsCheckedList.SetItemChecked(i, false);
                }
        }

        /// <summary>
        /// Events handler for clicking cancel button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Hide();
        }

        #endregion
    }
}