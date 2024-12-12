//
// (C) Copyright 2003-2009 by Autodesk, Inc.
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

namespace Revit.SDK.Samples.PowerCircuit.CS
{
    /// <summary>
    /// The dialog which provides the options of editing circuit
    /// </summary>
    public partial class EditCircuitForm : Form
    {
        /// <summary>
        /// data class object
        /// </summary>
        private CircuitOperationData m_data;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="data">Data class object</param>
        public EditCircuitForm(CircuitOperationData data)
        {
            m_data = data;

            InitializeComponent();
            // Add tool tips
            AddToolTips();
        }

        /// <summary>
        /// Add tool tips
        /// </summary>
        private void AddToolTips()
        {
            System.Resources.ResourceManager rsm = Properties.Resources.ResourceManager;
            toolTip.SetToolTip(buttonAdd, rsm.GetString("tipAddToCircuit"));
            toolTip.SetToolTip(buttonRemove, rsm.GetString("tipRemoveFromCircuit"));
            toolTip.SetToolTip(buttonSelectPanel, rsm.GetString("tipSelectPanel"));
            toolTip.SetToolTip(buttonCancel, "tipCancel");
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            m_data.EditOption = EditOption.Add;
            this.Close();           
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            m_data.EditOption = EditOption.Remove;
            this.Close();
        }

        private void buttonSelectPanel_Click(object sender, EventArgs e)
        {
            m_data.EditOption = EditOption.SelectPanel;
            this.Close();
        }
    }
}