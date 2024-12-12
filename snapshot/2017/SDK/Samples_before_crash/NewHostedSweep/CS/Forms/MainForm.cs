//
// (C) Copyright 2003-2016 by Autodesk, Inc.
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

namespace Revit.SDK.Samples.NewHostedSweep.CS
{
    /// <summary>
    /// This is the main form. It is the entry to create a new hosted sweep or to modify
    /// a created hosted sweep.
    /// </summary>
    public partial class MainForm : System.Windows.Forms.Form
    {
        /// <summary>
        /// Encapsulates the data source for a form.
        /// </summary>
        BindingSource m_binding;

        /// <summary>
        /// Creation manager, which collects all the creators.
        /// </summary>
        private CreationMgr m_creationMgr;

        /// <summary>
        /// Default constructor
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Customize constructor.
        /// </summary>
        /// <param name="mgr"></param>
        public MainForm(CreationMgr mgr): this()
        {
            m_creationMgr = mgr;
            m_binding = new BindingSource();
        }

        /// <summary>
        /// Show a form to fetch edges for hosted-sweep creation, and then create 
        /// the hosted-sweep.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCreate_Click(object sender, EventArgs e)
        {
            HostedSweepCreator creator = 
                comboBoxHostedSweepType.SelectedItem as HostedSweepCreator;

            CreationData creationData = new CreationData(creator);

            using (EdgeFetchForm createForm = new EdgeFetchForm(creationData))
            {
                if (createForm.ShowDialog() == DialogResult.OK)
                {
                    creator.Create(creationData);
                    RefreshListBox();
                }
            } 
        }

        /// <summary>
        /// Show a form to modify the created hosted-sweep.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonModify_Click(object sender, EventArgs e)
        {
            ModificationData modificationData = listBoxCreatedHostedSweeps.SelectedItem as ModificationData;

            using(HostedSweepModifyForm modifyForm = new HostedSweepModifyForm(modificationData))
            {
                modifyForm.ShowDialog();
            }
        }

        /// <summary>
        /// Refresh list box data source.
        /// </summary>
        private void RefreshListBox()
        {
            HostedSweepCreator creator = 
                comboBoxHostedSweepType.SelectedItem as HostedSweepCreator;
            m_binding.DataSource = creator.CreatedHostedSweeps;
            listBoxCreatedHostedSweeps.DataSource = m_binding;
            listBoxCreatedHostedSweeps.DisplayMember = "Name";
            m_binding.ResetBindings(false);
        }

        /// <summary>
        /// Initialize combobox data source.
        /// </summary>
        private void InitializeComboBox()
        {
            comboBoxHostedSweepType.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxHostedSweepType.Items.Add(m_creationMgr.FasciaCreator);
            comboBoxHostedSweepType.Items.Add(m_creationMgr.GutterCreator);
            comboBoxHostedSweepType.Items.Add(m_creationMgr.SlabEdgeCreator);
            comboBoxHostedSweepType.SelectedIndex = 0;
            comboBoxHostedSweepType.DisplayMember = "Name";
        }

        /// <summary>
        /// Initialize combo-box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            InitializeComboBox();
        }

        /// <summary>
        /// Close this form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// Update "Modify" button status according to the list-box selection item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBoxHostedSweeps_SelectedValueChanged(object sender, EventArgs e)
        {
            if (listBoxCreatedHostedSweeps.SelectedItem != null)
                buttonModify.Enabled = true;
            else
                buttonModify.Enabled = false;
        }

        /// <summary>
        /// Update the list-box data source according to the combobox selection.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxHostedSweepType_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshListBox();
        }
    }
}