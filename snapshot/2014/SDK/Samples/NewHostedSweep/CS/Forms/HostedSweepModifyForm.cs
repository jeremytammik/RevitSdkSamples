//
// (C) Copyright 2003-2013 by Autodesk, Inc.
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
using Autodesk.Revit.DB;
using System.Drawing.Drawing2D;

namespace Revit.SDK.Samples.NewHostedSweep.CS
{
    /// <summary>
    /// This form contains a property grid control to modify the property of hosted sweep.
    /// </summary>
    public partial class HostedSweepModifyForm : System.Windows.Forms.Form
    {        
        /// <summary>
        /// Data for modification.
        /// </summary>
        private ModificationData m_modificationData;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public HostedSweepModifyForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Customize constructor contains a parameter ModificationData.
        /// </summary>
        /// <param name="modificationData"></param>
        public HostedSweepModifyForm(ModificationData modificationData)
            : this()
        {
            m_modificationData = modificationData;
            this.Text = "Modify " + m_modificationData.CreatorName;
        }

        /// <summary>
        /// OK button, exit this form with DialogResult.OK.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// Load event, set the data source for property-grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HostedSweepModify_Load(object sender, EventArgs e)
        {
            propertyGrid.SelectedObject = m_modificationData;
            m_modificationData.ShowElement();
        } 
    }
}