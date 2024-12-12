//
// (C) Copyright 2003-2010 by Autodesk, Inc.
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
using System.IO;

using Autodesk.Revit.DB;

namespace Revit.SDK.Samples.ViewFilters.CS
{
    /// <summary>
    /// Form used to create new filter
    /// </summary>
    public partial class NewFilterForm : System.Windows.Forms.Form
    {
        #region Class Memeber
        /// <summary>
        /// In-use filter names
        /// </summary>
        ICollection<String> m_inUseFilterNames;

        /// <summary>
        /// New filter name 
        /// </summary>
        private String m_filterName;

        /// <summary>
        /// Get new filter name
        /// </summary>
        public String FilterName
        {
            get { return m_filterName; }
        }
        #endregion

        /// <summary>
        /// Show form for new filter name
        /// </summary>
        /// <param name="inUseNames">Filter names should be excluded.</param>
        public NewFilterForm(ICollection<String> inUseNames)
        {
            InitializeComponent();
            m_inUseFilterNames = inUseNames;
        }

        /// <summary>
        /// Check if input name is valid for new filter, the name should be unique
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void okButton_Click(object sender, EventArgs e)
        {
            // Check name is not empty
            String newName = newFilterNameTextBox.Text.Trim();
            if (String.IsNullOrEmpty(newName))
            {
                ViewFiltersForm.MyMessageBox("Filter name is empty!");
                newFilterNameTextBox.Focus();
                return;
            }
            //
            // Check if filter name contains invalid characters
            // These character are different from Path.GetInvalidFileNameChars()
            char[] invalidFileChars = { '\\', ':', '{', '}', '[', ']', '|', ';', '<', '>', '?', '\'', '~' };
            foreach (char invalidChr in invalidFileChars)
            {
                if (newName.Contains(invalidChr))
                {
                    ViewFiltersForm.MyMessageBox("Filter name contains invalid character: " + invalidChr);
                    return;
                }
            }
            // 
            // Check if name is used
            // check if name is already used by other filters
            bool inUsed = m_inUseFilterNames.Contains(newName, StringComparer.OrdinalIgnoreCase);
            if (inUsed)
            {
                ViewFiltersForm.MyMessageBox("The name you supplied is already in use. Enter a unique name please.");
                newFilterNameTextBox.Focus();
                return;
            }
            m_filterName = newName;
            this.Close();
            this.DialogResult = DialogResult.OK;
        }
    }
}
