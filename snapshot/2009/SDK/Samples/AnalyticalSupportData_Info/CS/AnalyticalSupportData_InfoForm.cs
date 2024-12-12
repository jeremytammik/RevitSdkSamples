//
// (C) Copyright 2003-2008 by Autodesk, Inc.
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

namespace Revit.SDK.Samples.AnalyticalSupportData_Info.CS
{
    /// <summary>
    /// UI which display the information
    /// </summary>
    public partial class AnalyticalSupportData_InfoForm : Form
    {
        // an instance of Command class which is prepared the displayed data.
        Command m_dataBuffer;

        /// <summary>
        /// Default constructor
        /// </summary>
        AnalyticalSupportData_InfoForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="dataBuffer"></param>
        public AnalyticalSupportData_InfoForm(Command dataBuffer) : this()
        {
            m_dataBuffer = dataBuffer;
            // display the elements information, which is prepared by Command class, in a grid.
            // set data source
            elementInfoDataGridView.AutoGenerateColumns = false;
            elementInfoDataGridView.DataSource = m_dataBuffer.ElementInformation;

            id.DataPropertyName = "Id";
            typeName.DataPropertyName = "Element Type";
            support.DataPropertyName = "Support Type";
            remark.DataPropertyName = "Remark";
        }

        /// <summary>
        /// exit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}