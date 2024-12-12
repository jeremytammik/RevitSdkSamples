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
using Autodesk.Revit.Elements;

namespace Revit.SDK.Samples.AddSpaceAndZone.CS
{
    /// <summary>
    /// The MainForm Class is the main user interface to manage the Space elements and Zone elements
    /// </summary>
    public partial class MainForm : System.Windows.Forms.Form
    {
        /// <summary>
        /// The default constructor of MainForm.
        /// </summary>
        private MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The constructor of MainForm.
        /// </summary>
        public MainForm(DataManager dataManager)
        {
            m_dataManager = dataManager;
            InitializeComponent();
        }

        /// <summary>
        /// When the levelComboBox selected index is changed, update the spacesListView and zonesTreeView.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void levelComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Update(levelComboBox.SelectedItem as Level);
        }

        /// <summary>
        /// When the MainForm is loaded, initialize related controls.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            // levelComboBox
            this.levelComboBox.DataSource = m_dataManager.Levels;
            this.levelComboBox.DisplayMember = "Name";
            this.levelComboBox.DropDownStyle = ComboBoxStyle.DropDownList;

            // EditZoneButton
            this.editZoneButton.Enabled = false;
        }

        /// <summary>
        /// When createSpacesButton is clicked, then create spaces and update the spacesListView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void createSpacesButton_Click(object sender, EventArgs e)
        {
            m_dataManager.CreateSpaces();
            Update(levelComboBox.SelectedItem as Level);
        }

        /// <summary>
        /// When editZoneButton is clicked, show the ZoneEditorForm to edit the selected ZoneNode.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void editZoneButton_Click(object sender, EventArgs e)
        {
            ZoneNode zoneNode = this.zonesTreeView.SelectedNode as ZoneNode;
            if (zoneNode != null)
            {
                using (ZoneEditorForm zoneEditorForm = new ZoneEditorForm(m_dataManager, zoneNode))
                {
                    zoneEditorForm.ShowDialog();
                }
            }

            Update(this.levelComboBox.SelectedItem as Level);
        }   

        /// <summary>
        /// Update the spacesListView and spacesListView.
        /// </summary>
        /// <param name="level"></param>
        private void Update(Level level)
        {
            this.spacesListView.Items.Clear();
            this.zonesTreeView.Nodes.Clear();
            
            // DataManager
            m_dataManager.Update(level);

            // spacesListView
            List<Space> spaces = m_dataManager.GetSpaces();
            foreach (Space space in spaces)
            {
                this.spacesListView.Items.Add(new SpaceItem(space));
            }

            // zonesTreeView
            List<Zone> zones = m_dataManager.GetZones();
            foreach (Zone zone in zones)
            {
                int nodeIndex = this.zonesTreeView.Nodes.Add(new ZoneNode(zone));
                foreach (Space spaceInZone in zone.Spaces)
                {
                    this.zonesTreeView.Nodes[nodeIndex].Nodes.Add(new SpaceNode(spaceInZone));
                }
            }
            this.zonesTreeView.ExpandAll();

            this.zonesTreeView.Update();
            this.spacesListView.Update();          
        }

        /// <summary>
        /// When createZoneButton is clicked, then create a new Zone element.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void createZoneButton_Click(object sender, EventArgs e)
        {
            this.m_dataManager.CreateZone();
            Update(levelComboBox.SelectedItem as Level);
        }

        private void zonesTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node is ZoneNode)
            {
                this.editZoneButton.Enabled = true;
            }
            else
            {
                this.editZoneButton.Enabled = false;
            }
        }
    }
}