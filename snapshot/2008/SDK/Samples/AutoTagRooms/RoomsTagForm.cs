//
// (C) Copyright 2003-2007 by Autodesk, Inc.
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
using Autodesk.Revit.Symbols;

namespace Revit.SDK.Samples.RoomsTag.CS
{
    /// <summary>
    /// The graphic user interface of auto tag rooms
    /// </summary>
    public partial class RoomsTagForm : Form
    {
        /// <summary>
        /// Default constructor of RoomsTagForm
        /// </summary>
        private RoomsTagForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor of RoomsTagForm
        /// </summary>
        /// <param name="roomsData">The data source of RoomsTagForm</param>
        public RoomsTagForm(RoomsData roomsData) : this()
        {
            m_roomsData = roomsData;
            InitRoomListView();
        }

        /// <summary>
        /// When the RoomsTagForm is loading, initialize the levelsComboBox and tagTypesComboBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RoomsTagForm_Load(object sender, EventArgs e)
        {
            // levelsComboBox
            this.levelsComboBox.DataSource = m_roomsData.Levels;
            this.levelsComboBox.DisplayMember = "Name";
            this.levelsComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            this.levelsComboBox.Sorted = true;

            // tagTypesComboBox
            this.tagTypesComboBox.DataSource = m_roomsData.RoomTagTypes;
            this.tagTypesComboBox.DisplayMember = "Name";
            this.tagTypesComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        /// <summary>
        /// Initialize the roomsListView 
        /// </summary>
        private void InitRoomListView()
        {
            this.roomsListView.Columns.Clear();

            // Create the columns of the roomsListView
            this.roomsListView.Columns.Add("Room Name");
            foreach (RoomTagType type in m_roomsData.RoomTagTypes)
            {
                this.roomsListView.Columns.Add(type.Name);
            }

            this.roomsListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            this.roomsListView.FullRowSelect = true;
        }

        /// <summary>
        /// Update the rooms information in the specified level
        /// </summary>
        private void UpdateRoomsList()
        {
            // when update the RoomsListView, clear all the items first
            this.roomsListView.Items.Clear();

            foreach (Room tmpRoom in m_roomsData.Rooms)
            {
                Level level = this.levelsComboBox.SelectedItem as Level;
                
                if (tmpRoom.Level.Id.Value == level.Id.Value)
                {
                    ListViewItem item = new ListViewItem(tmpRoom.Name);

                    // Shows the number of each type of RoomTags that the room has
                    foreach (RoomTagType type in m_roomsData.RoomTagTypes)
                    {
                        int count = m_roomsData.GetTagNumber(tmpRoom, type);
                        string str = count.ToString();
                        item.SubItems.Add(str);
                    }

                    this.roomsListView.Items.Add(item);
                }
            }
        }

        /// <summary>
        /// When clicked the autoTag button, then tag all rooms in the specified level. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void autoTagButton_Click(object sender, EventArgs e)
        {
            Level level = this.levelsComboBox.SelectedItem as Level;
            RoomTagType tagType = this.tagTypesComboBox.SelectedItem as RoomTagType;
            if (level != null && tagType != null)
	        {
                m_roomsData.AutoTagRooms(level, tagType);
	        }

            UpdateRoomsList();
        }

        /// <summary>
        /// When selected different level, then update the roomsListView.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void levelsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateRoomsList();
        }
    }
}