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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Revit.SDK.Samples.Loads.CS
{
    public partial class LoadsForm : System.Windows.Forms.Form
    {
        // Private members
        Loads m_dataBuffer;   // A reference of Loads.

        /// <summary>
        /// Constructor of LoadsForm
        /// </summary>
        /// <param name="dataBuffer"> A reference of Loads class </param>
        public LoadsForm(Loads dataBuffer)
        {
            // Required for Windows Form Designer support
            InitializeComponent();

            //Get a reference of LoadsForm
            m_dataBuffer = dataBuffer; 
        }

        /// <summary>
        /// Initialize the data on the form
        /// </summary>
        private void LoadsForm_Load(object sender, EventArgs e)
        {
            // Initialize the data of loadCaseTabPage 
            InitializeLoadCasePage();

            // Initialize the data of LoadCombinationsTabPage
            InitializeLoadCombinationPage();
        }     

        /// <summary>
        /// Respond the ok button click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void okButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// Respond the cancel button click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}