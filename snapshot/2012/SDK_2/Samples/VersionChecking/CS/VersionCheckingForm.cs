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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Revit.SDK.Samples.VersionChecking.CS
{
    /// <summary>
    /// UI that display the version information
    /// </summary>
    public partial class versionCheckingForm : System.Windows.Forms.Form
    {
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="dataBuffer">a instance of Command class</param>
        public versionCheckingForm(Command dataBuffer)
        {
            InitializeComponent();
            m_dataBuffer = dataBuffer;
        }

        // a instance of Command class
        Command m_dataBuffer;

        /// <summary>
        /// display the version information in a multiline text box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VersionCheckingForm_Load(object sender, EventArgs e)
        {
            versionInformationTextBox.ReadOnly = true;

            string productName    = "Product Name:    " + m_dataBuffer.ProductName + "\r\n";
            string productVersion = "Product Version: " + m_dataBuffer.ProductVersion + "\r\n";
            string buildNumber    = "Build Number:    " + m_dataBuffer.BuildNumner + "\r\n";
            
            versionInformationTextBox.AppendText(productName);
            versionInformationTextBox.AppendText(productVersion);
            versionInformationTextBox.AppendText(buildNumber);

            SetDialogLocation();
        }

        /// <summary>
        /// close UI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// set dialog's display location.
        /// </summary>
        private void SetDialogLocation()
        {
           // set dialog's display location
           int left             = (Screen.PrimaryScreen.WorkingArea.Right - this.Width) / 2;
           int top              = (Screen.PrimaryScreen.WorkingArea.Bottom - this.Height) / 2;
           Point windowLocation = new Point(left, top);
           this.Location        = windowLocation;
        }
    }
}