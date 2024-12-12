//
// (C) Copyright 2003-2015 by Autodesk, Inc.
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

namespace Revit.SDK.Samples.CreateWallinBeamProfile.CS
{
    public partial class CreateWallinBeamProfileForm : System.Windows.Forms.Form
    {
        // Private members
        CreateWallinBeamProfile m_dataBuffer;

        /// <summary>
        /// Constructor of CreateWallinBeamProfileForm
        /// </summary>
        /// <param name="dataBuffer"> A reference of CreateWallinBeamProfile class </param>
        public CreateWallinBeamProfileForm(CreateWallinBeamProfile dataBuffer)
        {
            // Required for Windows Form Designer support
            InitializeComponent();

            //Get a reference of CreateWallAndFloor
            m_dataBuffer = dataBuffer; 
        }

        /// <summary>
        /// Initialize the data on the form
        /// </summary>
        private void CreateWallinBeamProfileForm_Load(object sender, EventArgs e)
        {
            wallTypeComboBox.DataSource = m_dataBuffer.WallTypes;
            wallTypeComboBox.DisplayMember = "Name";

            structualCheckBox.Checked = m_dataBuffer.IsSturctual;
        }

        /// <summary>
        /// update the data to CreateWallinBeamProfile class
        /// </summary>
        private void okButton_Click(object sender, EventArgs e)
        {
            m_dataBuffer.SelectedWallType = wallTypeComboBox.SelectedItem;

            m_dataBuffer.IsSturctual = structualCheckBox.Checked;
        }
    }
}