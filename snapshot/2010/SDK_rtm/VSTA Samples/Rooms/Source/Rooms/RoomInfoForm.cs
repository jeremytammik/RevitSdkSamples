using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Forms;

using Autodesk.Revit;
using Autodesk.Revit.Symbols;
using Autodesk.Revit.Geometry;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Parameters;

using Rooms;

namespace Revit.SDK.Samples.Rooms.CS
{
    /// <summary>
    /// UI to display the rooms information
    /// </summary>
    public partial class roomsInformationForm : Form
    {
        RoomsData m_data;

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
        /// <param name="data">an instanc of Data class</param>
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

            //add rooms to the listview
            foreach (Room tmpRoom in roomList)
            {
                int idValue = tmpRoom.Id.Value;
                string roomId = idValue.ToString();
                //create a list view Item
                ListViewItem tmpItem = new ListViewItem(roomId);
                tmpItem.SubItems.Add(tmpRoom.Name);       //display room name.
                tmpItem.SubItems.Add(tmpRoom.Number);     //display room number.
                tmpItem.SubItems.Add(tmpRoom.Level.Name); //display the level

                //get department name from Department property 
                departmentName = m_data.GetProperty(tmpRoom, BuiltInParameter.ROOM_DEPARTMENT);
                tmpItem.SubItems.Add(departmentName);

                //get property value 
                propertyValue = m_data.GetProperty(tmpRoom, BuiltInParameter.ROOM_AREA);
                //get the area value
                areaValue = Double.Parse(propertyValue);
                tmpItem.SubItems.Add(propertyValue + " SF");
                //display whether the room with tag or not
                if (isHaveTag)
                {
                    tmpItem.SubItems.Add("Yes");
                }
                else
                {
                    tmpItem.SubItems.Add("No");
                }

                //add the item to the listview
                roomsListView.Items.Add(tmpItem);

                //add the area to the department
                m_data.CalculateDepartmentArea(departmentName, areaValue);
            }
        }

        /// <summary>
        /// when the form was loaded, display the room information
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RoomInfoForm_Load(object sender, EventArgs e)
        {
            roomsListView.Items.Clear();

            //add rooms in the list roomsWithoutTag to the listview
            this.DisplayRooms(m_data.RoomsWithoutTag, false);
            //add rooms in the list roomsWithTag to the listview
            this.DisplayRooms(m_data.RoomsWithTag, true);

            //display the amount of the rooms
            String numberOfRooms = "The number of rooms:  " + m_data.Rooms.Count.ToString();
            allRoomLabel.Text = numberOfRooms;

            //display the amount of the rooms without tags
            String roomsWithoutTag = "The number of rooms without tags:  " +
                                       m_data.RoomsWithoutTag.Count.ToString();
            tagLabel.Text = roomsWithoutTag;

            // if all the rooms have tags ,the button will be set to disable
            if (0 == m_data.RoomsWithoutTag.Count)
            {
                addTagsButton.Enabled = false;
            }

            //display the total area of each department
            this.DisplayDartmentsInfo();
        }


        /// <summary>
        /// create room tags for the rooms without tags
        /// </summary>
        private void addTagButton_Click(object sender, EventArgs e)
        {
            //close the form
            m_data.CreateTags();
            MessageBox.Show("Add tags to rooms successfully", "VSTA Sample", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }


        /// <summary>
        /// reorder room number
        /// </summary>
        private void reorderButton_Click(object sender, EventArgs e)
        {
            m_data.ReorderRooms();
            MessageBox.Show("Reoder rooms successfully", "VSTA Sample", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }


        /// <summary>
        /// display total room informations for each department
        /// </summary>
        private void DisplayDartmentsInfo()
        {
            for (int i = 0; i < m_data.DepartmentInfos.Count; i++)
            {
                //create a listview item
                ListViewItem tmpItem = new ListViewItem(m_data.DepartmentInfos[i].DepartmentName);
                tmpItem.SubItems.Add(m_data.DepartmentInfos[i].DepartmentAreaValue.ToString() + " SF");
                departmentsListView.Items.Add(tmpItem);
            }
        }


        /// <summary>
        /// export the total area of each department to a Excel file
        /// </summary>
        private void exportButton_Click(object sender, EventArgs e)
        {
            //create a save file dialog
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