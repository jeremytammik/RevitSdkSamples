//
// (C) Copyright 2003-2013 by Autodesk, Inc.
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
using Autodesk.Revit.DB.Architecture;

namespace Revit.SDK.Samples.AutoTagRooms.CS
{
    /// <summary>
    /// The graphic user interface of auto tag rooms
    /// </summary>
    public partial class AutoTagRoomsForm : System.Windows.Forms.Form
    {
        /// <summary>
        /// Default constructor of AutoTagRoomsForm
        /// </summary>
        private AutoTagRoomsForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor of AutoTagRoomsForm
        /// </summary>
        /// <param name="roomsData">The data source of AutoTagRoomsForm</param>
        public AutoTagRoomsForm(RoomsData roomsData) : this()
        {
            m_roomsData = roomsData;
            InitRoomListView();
        }

        /// <summary>
        /// When the AutoTagRoomsForm is loading, initialize the levelsComboBox and tagTypesComboBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AutoTagRoomsForm_Load(object sender, EventArgs e)
        {
            // levelsComboBox
            this.levelsComboBox.DataSource = m_roomsData.Levels;
            this.levelsComboBox.DisplayMember = "Name";
            this.levelsComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            this.levelsComboBox.Sorted = true;
            this.levelsComboBox.DropDown += new EventHandler(levelsComboBox_DropDown);

            // tagTypesComboBox
            this.tagTypesComboBox.DataSource = m_roomsData.RoomTagTypes;
            this.tagTypesComboBox.DisplayMember = "Name";
            this.tagTypesComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            this.tagTypesComboBox.DropDown += new EventHandler(tagTypesComboBox_DropDown);
        }

        /// <summary>
        /// When the tagTypesComboBox drop down, adjust its width
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tagTypesComboBox_DropDown(object sender, EventArgs e)
        {
            AdjustWidthComboBox_DropDown(sender, e);
        }

        /// <summary>
        /// When the levelsComboBox drop down, adjust its width
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void levelsComboBox_DropDown(object sender, EventArgs e)
        {
            AdjustWidthComboBox_DropDown(sender, e);
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
                
                if (tmpRoom.LevelId.IntegerValue == level.Id.IntegerValue)
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


        /// <summary>
        /// Adjust combo box drop down list width to longest string width
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AdjustWidthComboBox_DropDown(object sender, System.EventArgs e)
        {
            ComboBox senderComboBox = (ComboBox)sender;
            int width = senderComboBox.DropDownWidth;
            Graphics g = senderComboBox.CreateGraphics();
            Font font = senderComboBox.Font;
            int vertScrollBarWidth =
                (senderComboBox.Items.Count > senderComboBox.MaxDropDownItems)
                ? SystemInformation.VerticalScrollBarWidth : 0;

            int newWidth;
            foreach (Autodesk.Revit.DB.Element element in ((ComboBox)sender).Items)
            {
                string s = element.Name;
                newWidth = (int)g.MeasureString(s, font).Width
                    + vertScrollBarWidth;
                if (width < newWidth)
                {
                    width = newWidth;
                }
            }
            senderComboBox.DropDownWidth = width;
        }
    }
}