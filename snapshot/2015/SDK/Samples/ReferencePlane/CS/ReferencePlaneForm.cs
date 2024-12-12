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
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Revit.SDK.Samples.ReferencePlane.CS
{
    /// <summary>
    /// A form display all reference planes, and allow user to create
    /// reference plane with a button.
    /// </summary>
    public partial class ReferencePlaneForm : System.Windows.Forms.Form
    {
        //A object to manage reference plane.
        private ReferencePlaneMgr m_refPlaneMgr;

        /// <summary>
        /// A form object constructor.
        /// </summary>
        /// <param name="refPlaneMgr">A ReferencePlaneMgr buffer.</param>
        public ReferencePlaneForm(ReferencePlaneMgr refPlaneMgr)
        {
            Debug.Assert(null != refPlaneMgr);            
            InitializeComponent();

            m_refPlaneMgr = refPlaneMgr;

            // Set up the data source.
            refPlanesDataGridView.DataSource = m_refPlaneMgr.ReferencePlanes;

            refPlanesDataGridView.Columns[0].Width = (int)(refPlanesDataGridView.Width * 0.13);
            refPlanesDataGridView.Columns[1].Width = (int)(refPlanesDataGridView.Width * 0.29);
            refPlanesDataGridView.Columns[2].Width = (int)(refPlanesDataGridView.Width * 0.29);
            refPlanesDataGridView.Columns[3].Width = (int)(refPlanesDataGridView.Width * 0.29);
        }

        /// <summary>
        /// Notify revit to generate a reference plane.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void okButton_Click(object sender, EventArgs e)
        {
            m_refPlaneMgr.Create();
        }
    }
}