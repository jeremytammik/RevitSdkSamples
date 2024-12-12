//
// (C) Copyright 2003-2010 by Autodesk, Inc.
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

using Autodesk.Revit;
using Autodesk.Revit.DB;

namespace Revit.SDK.Samples.SharedCoordinateSystem.CS
{
    /// <summary>
    /// coordinate system data form
    /// </summary>
    public partial class CoordinateSystemDataForm : System.Windows.Forms.Form
    {
        CoordinateSystemData m_data; //the reference of the CoordinateSystemData class
        string m_currentName; //the current project location's name;
        string m_angle;    //the value of angle
        string m_eastWest; //the value of the east to west offset
        string m_northSouth; //the value of the north to south offset
        string m_elevation; //the value of the elevation from ground level
        string m_newLocationName; //the name of the duplicated location

        private PlaceInfo m_placeInfo;           //store all cities' information
        private SiteLocation m_siteLocation;     //reference to SiteLocation
        private CityInfo m_currentCityInfo;      //current CityInfo
        private const int DecimalNumber = 3;     //number of decimal
        private bool m_isFormLoading = true;     //indicate whether called when Form loading
        private bool m_isLatitudeChanged = false;//indicate whether user change Latitude value
        private bool m_isLongitudeChanged = false;//indicate whether user change Longitude value

        /// <summary>
        /// get and set the new location's name
        /// </summary>
        public string NewLocationName
        {
            get
            {
                return m_newLocationName;
            }
            set
            {
                m_newLocationName = value;
            }
        }

        /// <summary>
        /// constructor of form
        /// </summary>
        private CoordinateSystemDataForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// override constructor
        /// </summary>
        /// <param name="data">a instance of CoordinateSystemData class</param>
        public CoordinateSystemDataForm(CoordinateSystemData data, CitySet citySet, SiteLocation siteLocation)
        {
            m_data = data;
            m_currentName = null;

            //create new members about place information
            m_placeInfo = new PlaceInfo(citySet);
            m_siteLocation = siteLocation;
            m_currentCityInfo = new CityInfo();
            InitializeComponent();
        }

        /// <summary>
        /// display the location information on the form
        /// </summary>
        private void DisplayInformation()
        {
            //initialize the listbox
            locationListBox.Items.Clear();
            foreach (string itemName in m_data.LocationNames)
            {
                if (itemName == m_data.LocationName)
                {
                    m_currentName = itemName + " (current)"; //indicate the current project location
                    locationListBox.Items.Add(m_currentName);
                }
                else
                {
                    locationListBox.Items.Add(itemName);
                }
            }

            //set the selected item to current location
            for (int i = 0; i < locationListBox.Items.Count; i++)
            {
                string itemName = null;
                itemName = locationListBox.Items[i].ToString();
                if (itemName.Contains("(current)"))
                {
                    locationListBox.SelectedIndex = i;
                }
            }

            //get the offset values of the selected item 
            string selecteName = locationListBox.SelectedItem.ToString();
            m_data.GetOffset(selecteName);
            this.ShowOffsetValue();

            //set control in placeTabPage
            //convert values get from API and set them to controls
            CityInfo cityInfo = new CityInfo(m_siteLocation.Latitude, m_siteLocation.Longitude);
            CityInfoString cityInfoString = UnitConversion.ConvertFrom(cityInfo);

            //set Text of Latitude and Longitude TextBox
            latitudeTextBox.Text = cityInfoString.Latitude;
            longitudeTextBox.Text = cityInfoString.Longitude;

            //set DataSource of CitiesName ComboBox and TimeZones ComboBox
            cityNameComboBox.DataSource = m_placeInfo.CitiesName;
            timeZoneComboBox.DataSource = m_placeInfo.TimeZones;

            //try use Method DoTextBoxChanged to Set CitiesName ComboBox
            DoTextBoxChanged();
            m_isFormLoading = false;

            //get timezone from double value and set control
            string timeZoneString = m_placeInfo.TryGetTimeZoneString(m_siteLocation.TimeZone);

            //set selectItem of TimeZones ComboBox
            timeZoneComboBox.SelectedItem = timeZoneString;
            timeZoneComboBox.Enabled = false;
        }


        /// <summary>
        /// load the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CoordinateSystemDataForm_Load(object sender, EventArgs e)
        {
            this.DisplayInformation();

            this.CheckSelecteCurrent();
        }

        /// <summary>
        /// display the duplicate form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void duplicateButton_Click(object sender, EventArgs e)
        {
            using (DuplicateForm duplicateForm = new DuplicateForm(m_data,
                                                                     this,
                                                locationListBox.SelectedItem.ToString()))
            {

                if (DialogResult.OK != duplicateForm.ShowDialog())
                {
                    return;
                }
            }
            //refresh the form
            locationListBox.Items.Clear();
            m_data.GatData();
            this.DisplayInformation();

            //make the new project location is the selected item after it was duplicated
            for (int i = 0; i < locationListBox.Items.Count; i++)
            {
                if (m_newLocationName == locationListBox.Items[i].ToString())
                {
                    locationListBox.SelectedIndex = i;
                }
            }
        }

        /// <summary>
        /// when the selected item is the current location,make the button to disable
        /// </summary>
        private void CheckSelecteCurrent()
        {
            if (locationListBox.SelectedItem.ToString() == m_currentName)
            {
                makeCurrentButton.Enabled = false;
            }
            else
            {
                makeCurrentButton.Enabled = true;
            }
            //get the offset values of the selected item 
            string selecteName = locationListBox.SelectedItem.ToString();
            m_data.GetOffset(selecteName);
            this.ShowOffsetValue();
        }

        /// <summary>
        /// show the offset values on the form
        /// </summary>
        private void ShowOffsetValue()
        {
            //show the angle value
            char degree = (char)0xb0;
            angleTextBox.Text = m_data.AngleOffset.ToString() + degree;
            m_angle = m_data.AngleOffset.ToString();

            //show the value of the east to west offset
            eatWestTextBox.Text = m_data.EastWestOffset.ToString();
            m_eastWest = m_data.EastWestOffset.ToString();

            //show the value of the north to south offset
            northSouthTextBox.Text = m_data.NorthSouthOffset.ToString();
            m_northSouth = m_data.NorthSouthOffset.ToString();

            //show the value of the elevation
            elevationTextBox.Text = m_data.PositionElevation.ToString();
            m_elevation = m_data.PositionElevation.ToString();
        }


        /// <summary>
        /// the function will be invoked when the selected item changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void locationListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.CheckSelecteCurrent();
        }

        /// <summary>
        /// close the form and return true
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void okButton_Click(object sender, EventArgs e)
        {
            if (!this.CheckModify())
            {
                return;
            }
            SaveSiteLocation();
            this.DialogResult = DialogResult.OK;    // set dialog result
            this.Close();                           // close the form
        }

        /// <summary>
        /// set the selected item of the listbox to be the current project location
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void makeCurrentButton_Click(object sender, EventArgs e)
        {
            int selectIndex = locationListBox.SelectedIndex; //get selected index
            string newCurrentName = locationListBox.SelectedItem.ToString();//get location name
            m_data.ChangeCurrentLocation(newCurrentName);
            //refresh the form
            this.DisplayInformation();
            locationListBox.SelectedIndex = selectIndex;
        }

        /// <summary>
        /// check whether user modify the offset value
        /// </summary>
        private bool CheckModify()
        {
            try
            {
                if (m_angle != angleTextBox.Text ||
                            m_eastWest != eatWestTextBox.Text ||
                            m_northSouth != northSouthTextBox.Text ||
                            m_elevation != elevationTextBox.Text)
                {
                    string newValue = angleTextBox.Text;
                    string degree = ((char)0xb0).ToString();
                    if (newValue.Contains(degree))
                    {
                        int index = newValue.IndexOf(degree);
                        newValue = newValue.Substring(0, index);
                    }
                    double newAngle = Convert.ToDouble(newValue);
                    double newEast = Convert.ToDouble(eatWestTextBox.Text);
                    double newNorth = Convert.ToDouble(northSouthTextBox.Text);
                    double newElevation = Convert.ToDouble(elevationTextBox.Text);
                    string positionName = locationListBox.SelectedItem.ToString();
                    m_data.EditPosition(positionName, newAngle, newEast, newNorth, newElevation);
                }
            }
            catch (FormatException)
            {
                // spacing text boxes should only input number information
                MessageBox.Show("Please input double number in TextBox.", "Revit",
                                     MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            catch (Exception ex)
            {
                // if other unexpected error, just show the information
                MessageBox.Show(ex.Message, "Revit",
                                      MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            return true;
        }


        /// <summary>
        /// be invoked when SelectedValue of control cityNameComboBox changed 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cityNameComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            //check whether is Focused
            if (!cityNameComboBox.Focused)
            {
                return;
            }
            DoCityNameChanged();
        }

        /// <summary>
        /// be invoked when SelectValue of control timeZoneComboBox changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timeZoneComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            //check whether is Focused
            if (!timeZoneComboBox.Focused)
            {
                return;
            }
            m_currentCityInfo.TimeZone = m_placeInfo.TryGetTimeZoneNumber(
                timeZoneComboBox.SelectedItem as string);
        }

        /// <summary>
        /// be invoked when text changed in control latitudeTextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void latitudeTextBox_TextChanged(object sender, EventArgs e)
        {
            if(latitudeTextBox.Focused)
            {
                m_isLatitudeChanged = true;
            }
        }

        /// <summary>
        /// be invoked when text changed in control longitudeTextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void longitudeTextBox_TextChanged(object sender, EventArgs e)
        {
            if(longitudeTextBox.Focused)
            {
                m_isLongitudeChanged = true;
            }

        }

        /// <summary>
        /// be invoked when focus leave control latitudeTextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void latitudeTextBox_Leave(object sender, EventArgs e)
        {
            if (m_isLatitudeChanged)
            {
                DoTextBoxChanged();
                string text = DealDecimalNumber(latitudeTextBox.Text);
                latitudeTextBox.Text = text;
                m_isLatitudeChanged = false;
            }
        }

        /// <summary>
        /// be invoked when focus leave control longitudeTextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void longitudeTextBox_Leave(object sender, EventArgs e)
        {
            if(m_isLongitudeChanged)
            {
                DoTextBoxChanged();
                string text = DealDecimalNumber(longitudeTextBox.Text);
                longitudeTextBox.Text = text;
                m_isLongitudeChanged = false;
            }
        }

        /// <summary>
        /// deal with decimal number
        /// </summary>
        /// <param name="value">string wanted to deal with</param>
        /// <returns>result dealing with</returns>
        private string DealDecimalNumber(string value)
        {
            string result;
            double doubleValue;
            //try to get double value from string
            if (!UnitConversion.StringToDouble(value, ValueType.Angle, out doubleValue))
            {
                string degree = ((char)0xb0).ToString();
                if (!value.Contains(degree))
                {
                    result = value + degree;
                    return result;
                }
            }
            //try to convert double into string
            result = UnitConversion.DoubleToString(doubleValue, ValueType.Angle);
            return result;
        }

        /// <summary>
        /// call by CitiesNameSelectedChanged,when CitiesName ComboBox selected changed
        /// </summary>
        private void DoCityNameChanged()
        {
            //disable timezone ComboBox
            timeZoneComboBox.Enabled = false;
            CityInfoString cityInfoString = new CityInfoString();

            //get new CityInfoString
            if (GetCityInfo(cityNameComboBox.SelectedItem as string, out cityInfoString))
            {
                //use new CityInfoString to set TextBox and ComboBox
                latitudeTextBox.Text = cityInfoString.Latitude;
                longitudeTextBox.Text = cityInfoString.Longitude;

                //set control timeZonesComboBox
                if (null != cityInfoString.TimeZone)
                {
                    timeZoneComboBox.Text = null;
                    timeZoneComboBox.SelectedItem = cityInfoString.TimeZone;
                }
                else
                {
                    timeZoneComboBox.SelectedIndex = -1;
                }
            }
            //if failed, set control with nothing 
            else
            {
                latitudeTextBox.Text = null;
                longitudeTextBox.Text = null;
                timeZoneComboBox.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// called by some functions to do same operation
        /// </summary>
        private void DoTextBoxChanged()
        {
            //enable timezone ComboBox
            timeZoneComboBox.Enabled = true;
            CityInfoString cityInfoString = new CityInfoString(latitudeTextBox.Text, longitudeTextBox.Text);
            string cityName;
            string timeZone;

            //get new CityName and TimeZone
            GetCityNameTimeZone(cityInfoString, out cityName, out timeZone);

            //use new CityName to set ComboBox
            if (null != cityName)
            {
                cityNameComboBox.Text = null;
                cityNameComboBox.SelectedItem = cityName;
                timeZoneComboBox.Enabled = false;
            }
            else
            {
                if(m_isFormLoading)
                {
                    string userDefinedCity = "User Defined\r";
                    if (!m_placeInfo.CitiesName.Contains(userDefinedCity))
                    {
                        cityNameComboBox.DataSource = null;
                        m_placeInfo.CitiesName.Add(userDefinedCity);
                        m_placeInfo.CitiesName.Sort();
                        cityNameComboBox.DataSource = m_placeInfo.CitiesName;
                        CityInfo cityInfo = UnitConversion.ConvertTo(cityInfoString);
                        cityInfo.CityName = userDefinedCity;
                        cityInfo.TimeZone = m_siteLocation.TimeZone;
                        m_placeInfo.AddCityInfo(cityInfo);
                    }
                    cityNameComboBox.SelectedItem = userDefinedCity;
                }
                else
                {
                    cityNameComboBox.SelectedIndex = -1;
                }
            }

            //after get timeZone,set control timeZonesComboBox
            if (null != timeZone)
            {
                timeZoneComboBox.Text = null;
                timeZoneComboBox.SelectedItem = timeZone;
            }
        }

        /// <summary>
        /// used when city information changed
        /// </summary>
        /// <param name="cityInfoString">city information which changed</param>
        /// <param name="cityName">city name want to get according to information</param>
        /// <param name="timeZone">city time zone gotten according to information</param>
        private void GetCityNameTimeZone(CityInfoString cityInfoString,
            out string cityName, out string timeZone)
        {
            CityInfo cityInfo = UnitConversion.ConvertTo(cityInfoString);
            string tempName;
            double tempTime;

            //try to get city name and timezone according to cityInfo
            if (m_placeInfo.TryGetCityNameTimeZone(cityInfo, out tempName, out tempTime))
            {
                cityName = tempName;

                //try to get string representing timezone according to a number 
                timeZone = m_placeInfo.TryGetTimeZoneString(tempTime);

                //set current CityInfo
                m_currentCityInfo.Latitude = cityInfo.Latitude;
                m_currentCityInfo.Longitude = cityInfo.Longitude;
                m_currentCityInfo.TimeZone = tempTime;
                m_currentCityInfo.CityName = tempName;
            }
            else
            {
                //set current CityInfo
                cityName = null;
                timeZone = null;
                m_currentCityInfo.Latitude = cityInfo.Latitude;
                m_currentCityInfo.Longitude = cityInfo.Longitude;
                m_currentCityInfo.CityName = null;
            }
        }

        /// <summary>
        /// used when city name changed
        /// </summary>
        /// <param name="cityName">city name which changed</param>
        /// <param name="cityInfoString">city information want to get according to city name</param>
        /// <returns>check whether is successful</returns>
        private bool GetCityInfo(string cityName, out CityInfoString cityInfoString)
        {
            CityInfo cityInfo = new CityInfo();

            //try to get CityInfo according to cityName
            if (m_placeInfo.TryGetCityInfo(cityName, out cityInfo))
            {
                //do conversion from CityInfo to CityInfoString
                cityInfoString = UnitConversion.ConvertFrom(cityInfo);

                //do TimeZone conversion from double to string
                cityInfoString.TimeZone = m_placeInfo.TryGetTimeZoneString(cityInfo.TimeZone);

                //set current CityInfo
                m_currentCityInfo = cityInfo;
                m_currentCityInfo.CityName = cityName;
                return true;
            }

            //if failed, also set current CityInfo            
            m_currentCityInfo.CityName = null;
            cityInfoString = new CityInfoString();
            return false;
        }

        /// <summary>
        /// save siteLocation to Revit
        /// </summary>
        private void SaveSiteLocation()
        {
            if (null == m_siteLocation)
            {
                return;
            }

            //change SiteLocation of Revit          
            m_siteLocation.Latitude = m_currentCityInfo.Latitude;
            m_siteLocation.Longitude = m_currentCityInfo.Longitude;
            m_siteLocation.TimeZone = m_currentCityInfo.TimeZone;
        }


        /// <summary>
        /// check the format of the user's input and add a degree symbol behind the angle value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void angleTextBox_Leave(object sender, EventArgs e)
        {
            try
            {
                //check is there any symbol exist in the behind of the value
                //and check whether the user's input is number 
                string degree = ((char)0xb0).ToString();
                if (!angleTextBox.Text.Contains(degree))
                {
                    double value = Convert.ToDouble(angleTextBox.Text);
                    angleTextBox.AppendText(degree);
                }
                else
                {
                    string tempName = angleTextBox.Text;
                    int index = tempName.IndexOf(degree);
                    tempName = tempName.Substring(0, index);
                    double value = Convert.ToDouble(tempName);
                }
            }
            catch (FormatException)
            {
                //angle text boxes should only input number information
                MessageBox.Show("Please input double number in TextBox.", "Revit",
                                     MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            catch (Exception ex)
            {
                // if other unexpected error, just show the information
                MessageBox.Show(ex.Message, "Revit",
                                      MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
        }
    }
}