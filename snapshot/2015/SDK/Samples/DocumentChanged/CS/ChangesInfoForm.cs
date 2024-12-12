//
// (C) Copyright 2003-2014 by Autodesk, Inc.
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
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Revit.SDK.Samples.ChangesMonitor.CS
{
    /// <summary>
    /// The UI to show the change history logs. This class is not the main one just a assistant
    /// in this sample. If you just want to learn how to use DocumentChanges event,
    /// please pay more attention to ExternalApplication class.
    /// </summary>
    public partial class ChangesInformationForm : Form
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public ChangesInformationForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor with one argument
        /// </summary>
        /// <param name="dataBuffer">prepare the informations which is shown in this UI</param>
        public ChangesInformationForm(DataTable dataBuffer)
            : this()
        {
            changesdataGridView.DataSource = dataBuffer;
            changesdataGridView.AutoGenerateColumns = false;
        }


        /// <summary>
        /// windows shown event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangesInfoForm_Shown(object sender, EventArgs e)
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
        private void changesdataGridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            changesdataGridView.CurrentCell = changesdataGridView.Rows[changesdataGridView.Rows.Count - 1].Cells[0];
        }
      
    }
}
