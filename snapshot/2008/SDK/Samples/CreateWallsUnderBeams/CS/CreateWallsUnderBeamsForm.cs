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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Revit.SDK.Samples.CreateWallsUnderBeams.CS
{
    public partial class CreateWallsUnderBeamsForm : Form
    {
        // Private members
        CreateWallsUnderBeams m_dataBuffer;

        /// <summary>
        /// Constructor of CreateWallsUnderBeamsForm
        /// </summary>
        /// <param name="dataBuffer"> A reference of CreateWallsUnderBeams class </param>
        public CreateWallsUnderBeamsForm(CreateWallsUnderBeams dataBuffer)
        {
            // Required for Windows Form Designer support
            InitializeComponent();

            //Get a reference of CreateWallsUnderBeams
            m_dataBuffer = dataBuffer; 
        }

        /// <summary>
        /// Initialize the data on the form
        /// </summary>
        private void CreateWallsUnderBeamsForm_Load(object sender, EventArgs e)
        {
            wallTypeComboBox.DataSource = m_dataBuffer.WallTypes;
            wallTypeComboBox.DisplayMember = "Name";

            structualCheckBox.Checked = m_dataBuffer.IsSturctual;
        }

        /// <summary>
        /// update the data to CreateWallsUnderBeams class
        /// </summary>
        private void OKButton_Click(object sender, EventArgs e)
        {
            m_dataBuffer.SelectedWallType = wallTypeComboBox.SelectedItem;

            m_dataBuffer.IsSturctual = structualCheckBox.Checked;
        }


    }
}