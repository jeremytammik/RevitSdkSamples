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
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;

namespace Revit.SDK.Samples.AddSpaceAndZone.CS
{
    /// <summary>
    /// The ZoneEditorForm Class the user interface to edit a Zone element.
    /// </summary>
    public partial class ZoneEditorForm : System.Windows.Forms.Form
    {
        /// <summary>
        /// The default constructor of ZoneEditorForm class.
        /// </summary>
        private ZoneEditorForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The constructor of ZoneEditorForm class.
        /// </summary>
        /// <param name="dataManager"></param>
        /// <param name="zoneNode"></param>
        public ZoneEditorForm(DataManager dataManager, ZoneNode zoneNode)
        {
            m_dataManager = dataManager;
            m_zoneNode = zoneNode;
            m_zone = m_zoneNode.Zone;
            InitializeComponent();
        }

        /// <summary>
        /// When the addSpace Button is clicked, the selected spaces will be added to the
        /// current Zone element.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addSpaceButton_Click(object sender, EventArgs e)
        {
            SpaceSet set = new SpaceSet();
            foreach (SpaceItem item in this.availableSpacesListView.SelectedItems)
            {
                set.Insert(item.Space);
            }

            m_zone.AddSpaces(set);

            UpdateSpaceList();
        }


        /// <summary>
        /// When the removeSpace Button is clicked, the selected spaces will be removed from the
        /// current Zone element.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void removeSpaceButton_Click(object sender, EventArgs e)
        {
            SpaceSet set = new SpaceSet();
            foreach (SpaceItem item in this.currentSpacesListView.SelectedItems)
            {
                set.Insert(item.Space);
            }

            m_zone.RemoveSpaces(set);
            UpdateSpaceList();
        }

        /// <summary>
        /// When the ZoneEditorForm is loaded, update the AvailableSpacesListView and CurrentSpacesListView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ZoneEditorForm_Load(object sender, EventArgs e)
        {
            this.Text = "Edit Zone : " + m_zone.Name;
            UpdateSpaceList();
        }

        /// <summary>
        /// Update the AvailableSpacesListView and CurrentSpacesListView 
        /// </summary>
        private void UpdateSpaceList()
        {
            this.availableSpacesListView.Items.Clear();
            this.currentSpacesListView.Items.Clear();
           
            // AvailableSpacesListView
            foreach (Space space in m_dataManager.GetSpaces())
            {
                if (m_zone.Spaces.Contains(space) == false)
                {
                    this.availableSpacesListView.Items.Add(new SpaceItem(space));
                }
            }

            // CurrentSpacesListView
            foreach (Space space in m_zone.Spaces)
            {
                this.currentSpacesListView.Items.Add(new SpaceItem(space));
            }

            this.availableSpacesListView.Update();
            this.currentSpacesListView.Update();
        }

        /// <summary>
        /// When OK button is clicked, close the ZoneEditorForm.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void okButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}