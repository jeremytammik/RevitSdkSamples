//
// (C) Copyright 2003-2019 by Autodesk, Inc.
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
    /// The dialog which lets user operate selected elements
    /// </summary>
    public partial class CircuitOperationForm : System.Windows.Forms.Form
    {
        /// <summary>
        /// Object of data class
        /// </summary>
        CircuitOperationData m_optionData;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="optionData"></param>
        public CircuitOperationForm(CircuitOperationData optionData)
        {
            m_optionData = optionData;
            InitializeComponent();
            InitializeButtons();
            // Add tool tips
            AddToolTips();
        }

        /// <summary>
        /// Initialize buttons
        /// </summary>
        private void InitializeButtons()
        {
            // Set enabled status
            buttonCreate.Enabled = m_optionData.CanCreateCircuit;
            buttonEdit.Enabled = m_optionData.HasCircuit;
            buttonSelectPanel.Enabled = m_optionData.HasCircuit;
            buttonDisconnectPanel.Enabled = m_optionData.HasPanel;
        }

        /// <summary>
        /// Add tool tips
        /// </summary>
        private void AddToolTips()
        {
            System.Resources.ResourceManager rsm = Properties.Resources.ResourceManager;
            toolTip.SetToolTip(buttonCreate, rsm.GetString("tipCreateCircuit"));
            toolTip.SetToolTip(buttonEdit, rsm.GetString("tipEditCircuit"));
            toolTip.SetToolTip(buttonSelectPanel, rsm.GetString("tipSelectPanel"));
            toolTip.SetToolTip(buttonDisconnectPanel, rsm.GetString("tipDisconnectPanel"));
            toolTip.SetToolTip(buttonCancel, rsm.GetString("tipCancel"));
        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            m_optionData.Operation = Operation.CreateCircuit;
            this.Close();
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            m_optionData.Operation = Operation.EditCircuit;
            this.Close();
        }

        private void buttonSelectPanel_Click(object sender, EventArgs e)
        {
            m_optionData.Operation = Operation.SelectPanel;
            this.Close();
        }

        private void buttonDisconnectPanel_Click(object sender, EventArgs e)
        {
            m_optionData.Operation = Operation.DisconnectPanel;
            this.Close();
        }
    }
}