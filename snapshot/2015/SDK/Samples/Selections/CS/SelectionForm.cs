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

namespace Revit.SDK.Samples.Selections.CS
{
    /// <summary>
    /// A Form to show selection from dialog.
    /// </summary>
    public partial class SelectionForm : Form
    {
        SelectionManager m_manager;

        /// <summary>
        /// Form initialize.
        /// </summary>
        /// <param name="manager"></param>
        public SelectionForm(SelectionManager manager)
        {
            InitializeComponent();
            m_manager = manager;
        }

        /// <summary>
        /// Set the selection type for picking element.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PickElementButton_Click(object sender, EventArgs e)
        {
            m_manager.SelectionType = SelectionType.Element;
            this.Close();
        }

        /// <summary>
        /// Set the selection type for picking point.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoveToButton_Click(object sender, EventArgs e)
        {
            m_manager.SelectionType = SelectionType.Point;
            this.Close();
        }
    }
}
