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
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.?AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
//


using System;
using System.Data;
using System.Collections.ObjectModel;
using System.Windows.Forms;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;


namespace Revit.SDK.Samples.Rooms.CS
{
    /// <summary>
    /// UI to display the rooms information
    /// </summary>
    public partial class roomsInformationForm : System.Windows.Forms.Form
    {
        #region Class member variables
        RoomsData m_data; // Room's data for current active document

        #endregion

        /// <summary>
        /// constructor
        /// </summary>
        public roomsInformationForm()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Overload the constructor
        /// </summary>
        /// <param name="data">an instance of Data class</param>
        public roomsInformationForm(RoomsData data)
        {
            m_data = data;
            InitializeComponent();
        }


        /// <summary>
        /// add rooms of list roomsWithTag to the listview
        /// </summary>
        private void DisplayRooms(ReadOnlyCollection<Room> roomList, bool isHaveTag)
        {
            String propertyValue = null;   //value of department
            String departmentName = null;  //department name 
            Double areaValue = 0.0;        //room area

            // add rooms to the listview
            foreach (Room tmpRoom in roomList)
            {
                // make sure the room has Level, that's it locates at level.
                if (tmpRoom.Document.GetElement(tmpRoom.LevelId) == null)
                {
                    continue;
                }

                string roomId = tmpRoom.Id.ToString();

                // create a list view Item
                ListViewItem tmpItem = new ListViewItem(roomId);
                tmpItem.SubItems.Add(tmpRoom.Name);       //display room name.
                tmpItem.SubItems.Add(tmpRoom.Number);     //display room number.
                tmpItem.SubItems.Add((tmpRoom.Document.GetElement(tmpRoom.LevelId) as Level).Name); //display the level

                // get department name from Department property 
                departmentName = m_data.GetProperty(tmpRoom, BuiltInParameter.ROOM_DEPARTMENT);
                tmpItem.SubItems.Add(departmentName);

                // get property value 
                propertyValue = m_data.GetProperty(tmpRoom, BuiltInParameter.ROOM_AREA);

                // get the area value
                areaValue = Double.Parse(propertyValue);
                tmpItem.SubItems.Add(propertyValue + " SF");

                // display whether the room with tag or not
                if (isHaveTag)
                {
                    tmpItem.SubItems.Add("Yes");
                }
                else
                {
                    tmpItem.SubItems.Add("No");
                }

                // add the item to the listview
                roomsListView.Items.Add(tmpItem);

                // add the area to the department
                m_data.CalculateDepartmentArea(departmentName, areaValue);
            }
        }


        /// <summary>
        /// when the form was loaded, display the information of rooms 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RoomInfoForm_Load(object sender, EventArgs e)
        {
            roomsListView.Items.Clear();

            // add rooms in the list roomsWithoutTag to the listview
            this.DisplayRooms(m_data.RoomsWithoutTag, false);

            // add rooms in the list roomsWithTag to the listview
            this.DisplayRooms(m_data.RoomsWithTag, true);

            // display the total area of each department
            this.DisplayDartmentsInfo();

            // if all the rooms have tags ,the button will be set to disable
            if (0 == m_data.RoomsWithoutTag.Count)
            {
                addTagsButton.Enabled = false;
            }
        }


        /// <summary>
        /// create room tags for the rooms which are lack of tags
        /// </summary>
        private void addTagButton_Click(object sender, EventArgs e)
        {
            m_data.CreateTags();

            roomsListView.Items.Clear();
            this.DisplayRooms(m_data.RoomsWithTag, true);
            this.DisplayRooms(m_data.RoomsWithoutTag, false);

            // if all the rooms have tags ,the button will be set to disable
            if (0 == m_data.RoomsWithoutTag.Count)
            {
                addTagsButton.Enabled = false;
            }
        }


        /// <summary>
        /// reorder rooms' number
        /// </summary>
        private void reorderButton_Click(object sender, EventArgs e)
        {
            m_data.ReorderRooms();

            // refresh the listview
            roomsListView.Items.Clear();
            this.DisplayRooms(m_data.RoomsWithTag, true);
            this.DisplayRooms(m_data.RoomsWithoutTag, false);
        }


        /// <summary>
        /// display total rooms' information for each department
        /// </summary>
        private void DisplayDartmentsInfo()
        {
            for (int i = 0; i < m_data.DepartmentInfos.Count; i++)
            {
                // create a listview item
                ListViewItem tmpItem = new ListViewItem(m_data.DepartmentInfos[i].DepartmentName);
                tmpItem.SubItems.Add(m_data.DepartmentInfos[i].RoomsAmount.ToString());
                tmpItem.SubItems.Add(m_data.DepartmentInfos[i].DepartmentAreaValue.ToString() +
                                     " SF");
                departmentsListView.Items.Add(tmpItem);
            }
        }


        /// <summary>
        /// Save the information into an Excel file
        /// </summary>
        private void exportButton_Click(object sender, EventArgs e)
        {
            // create a save file dialog
            using (SaveFileDialog sfdlg = new SaveFileDialog())
            {
                sfdlg.Title = "Export area of department to Excel file";
                sfdlg.Filter = "CSV(command delimited)(*.csv)|*.csv";
                sfdlg.RestoreDirectory = true;

                if (DialogResult.OK == sfdlg.ShowDialog())
                {
                    m_data.ExportFile(sfdlg.FileName);
                }
            }
        }
    }
}