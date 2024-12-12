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
using System.Data.Odbc;
using System.Data.Common;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Revit.SDK.Samples.RDBLink.CS
{
    /// <summary>
    /// Main form, provides UI to export to or import from database
    /// </summary>
    public partial class RDBLinkForm : Form
    {
        #region Fields
        /// <summary>
        /// true indicates exporting, false indicates importing
        /// </summary>
        bool m_isExport;
        #endregion

        #region Properties
        /// <summary>
        /// Gets whether user wants to do export, 
        /// true to indicate exporting, false to indicate importing
        /// </summary>
        public bool IsExport
        {
            get { return m_isExport; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public RDBLinkForm()
        {
            InitializeComponent();
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Do export
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exportButton_Click(object sender, EventArgs e)
        {
            m_isExport = true;
            this.Close();
        }

        /// <summary>
        /// Do import
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void importButton_Click(object sender, EventArgs e)
        {
            m_isExport = false;
            this.Close();
		}

        /// <summary>
        /// Close the form when press Esc key. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RDBLinkForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();          
            }
        }
        #endregion
    }
}
