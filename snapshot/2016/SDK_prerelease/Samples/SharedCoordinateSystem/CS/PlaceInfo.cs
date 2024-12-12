//
// (C) Copyright 2003-2014 by Autodesk, Inc.
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
using System.Text;
using System.Collections;
using System.IO;
using System.Windows.Forms;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.SharedCoordinateSystem.CS
{
    /// <summary>
    /// a struct used to describe information about city
    /// </summary>
    public struct CityInfo
    {
        double m_timeZone;  //Timezone in which the city resides
        double m_latitude;  //Latitude of the city 
        double m_longitude; //Longitude of the city
        string m_cityName;  //name of city


        /// <summary>
        /// property used to get and set TimeZone
        /// </summary>
        public double TimeZone
        {
            get
            {
                return m_timeZone;
            }
            set
            {
                m_timeZone = value;
            }
        }

        /// <summary>
        /// property used to get and set Latitude
        /// </summary>
        public double Latitude
        {
            get
            {
                return m_latitude;
            }
            set
            {
                m_latitude = value;
            }
        }

        /// <summary>
        /// property used to get and set Longitude
        /// </summary>
        public double Longitude
        {
            get
            {
                return m_longitude;
            }
            set
            {
                m_longitude = value;
            }
        }

        /// <summary>
        /// property used to get and set city name
        /// </summary>
        public string CityName
        {
            get
            {
                return m_cityName;
            }
            set
            {
                m_cityName = value;
            }
        }

        /// <summary>
        /// class CityInfo's constructor
        /// </summary>
        /// <param name="latitude">latitude of city</param>
        /// <param name="longitude">longitude of city</param>
        public CityInfo(double latitude, double longitude)
        {
            m_latitude = latitude;
            m_longitude = longitude;
            m_timeZone = PlaceInfo.InvalidTimeZone;
            m_cityName = null;
        }

        /// <summary>
        /// class CityInfo's constructor
        /// </summary>
        /// <param name="latitude">latitude of city</param>
        /// <param name="longitude">longitude of city</param>
        /// <param name="timeZone">timezone of city</param>
        /// <param name="cityName">city name</param>
        public CityInfo(double latitude, double longitude, double timeZone, string cityName)
        {
            m_timeZone = timeZone;
            m_latitude = latitude;
            m_longitude = longitude;
            m_cityName = cityName;
        }
    }

    /// <summary>
    /// a struct used to describe information about city
    /// displayed in Form and it's members are string type
    /// </summary>
    public struct CityInfoString
    {
        string m_timeZone; //Timezone in which the city resides
        string m_latitude; //Latitde of the city 
        string m_longitude; //Longitude of the city

        /// <summary>
        /// property used to get and set TimeZone
        /// </summary>
        public string TimeZone
        {
            get
            {
                return m_timeZone;
            }
            set
            {
                m_timeZone = value;
            }
        }

        /// <summary>
        /// property used to get and set Latitude
        /// </summary>
        public string Latitude
        {
            get
            {
                return m_latitude;
            }
            set
            {
                m_latitude = value;
            }
        }

        /// <summary>
        /// property used to get and set Longitude
        /// </summary>
        public string Longitude
        {
            get
            {
                return m_longitude;
            }
            set
            {
                m_longitude = value;
            }
        }

        /// <summary>
        /// class CityInfo's constructor
        /// </summary>
        /// <param name="latitude">latitude of city</param>
        /// <param name="longitude">longitude of city</param>
        public CityInfoString(string latitude, string longitude)
        {
            m_latitude = latitude;
            m_longitude = longitude;
            m_timeZone = null;
        }

        /// <summary>
        /// class CityInfo's constructor
        /// </summary>
        /// <param name="latitude">latitude of city</param>
        /// <param name="longitude">longitude of city</param>
        /// <param name="timeZone">timezone of city</param>
        public CityInfoString(string latitude, string longitude, string timeZone)
        {
            m_timeZone = timeZone;
            m_latitude = latitude;
            m_longitude = longitude;
        }
    }

    /// <summary>
    /// a class used to store information of all city
    /// include it's name,Latitude,longitude,timezone
    /// </summary>
    public class PlaceInfo
    {
        private List<string> m_citiesName;    //city's name
        private List<CityInfo> m_citiesInfo;  //information of all cities,such Latitude,longitude
        private List<string> m_timeZones;     //timezone information of all cities 
        private bool m_isTimeZonesValid;      //figure out whether can get timezone information
        private const double Diff = 0.0001;  //used to check whether two double values are equal
        private static readonly double m_invalidTimeZone = -13;  //value used when can't get timezone


        /// <summary>
        /// property used to get and set all cities' name
        /// </summary>
        public List<string> CitiesName
        {
            get
            {
                return m_citiesName;
            }
            set
            {
                m_citiesName = value;
            }
        }

        /// <summary>
        /// property used to get and set all timezone
        /// </summary>
        public List<string> TimeZones
        {
            get
            {
                return m_timeZones;
            }
            set
            {
                m_timeZones = value;
            }
        }

        /// <summary>
        /// property used to get Invalid timezone
        /// </summary>
        public static double InvalidTimeZone
        {
            get
            {
                return m_invalidTimeZone;
            }
        }

        /// <summary>
        /// class PlaceInfo's constructor
        /// </summary>
        /// <param name="cities"></param>
        public PlaceInfo(CitySet cities)
        {
            Initialize(cities);
        }

        /// <summary>
        /// initialize function 
        /// </summary>
        /// <param name="cities">a set store all cities</param>
        /// <returns></returns>
        public bool Initialize(CitySet cities)
        {
            m_citiesInfo = new List<CityInfo>();
            m_citiesName = new List<string>();
            m_timeZones = new List<string>();

            if (InitCities(cities) && InitTimeZone())
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Add a city info to city info List
        /// </summary>
        /// <param name="cityInfo">the city info need to add</param>
        public void AddCityInfo(CityInfo cityInfo)
        {
            if (m_citiesInfo.Contains(cityInfo))
            {
                return;
            }
            m_citiesInfo.Add(cityInfo);
        }

        /// <summary>
        /// try to get city name according to CityInfo
        /// </summary>
        /// <param name="cityInfo">store information about city</param>
        /// <param name="cityName">city's name</param>
        /// <param name="timeZone">city's timezone</param>
        /// <returns>figure out whether this function successful</returns>
        public bool TryGetCityNameTimeZone(CityInfo cityInfo, out string cityName, out double timeZone)
        {
            cityName = null;
            timeZone = m_invalidTimeZone;

            //loop to find cityinfo matched
            foreach (CityInfo temp in m_citiesInfo)
            {
                //compare Latitude and longitude,and if difference < Diff 
                // the two CityInfo are equal
                if (Math.Abs(temp.Latitude - cityInfo.Latitude) < Diff &&
                    Math.Abs(temp.Longitude - cityInfo.Longitude) < Diff)
                {
                    cityName = temp.CityName;
                    timeZone = temp.TimeZone;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// try to get city info according to city name
        /// </summary>
        /// <param name="cityName">city name</param>
        /// <param name="cityInfo">city's information</param>
        /// <returns>figure out whether this function successful</returns>
        public bool TryGetCityInfo(string cityName, out CityInfo cityInfo)
        {
            //compare cityName with element's CityName of m_citiesInfo
            //if they are equal, that element is matched
            foreach (CityInfo temp in m_citiesInfo)
            {
                if (cityName == temp.CityName)
                {
                    cityInfo = temp;
                    return true;
                }
            }
            cityInfo = new CityInfo();
            return false;
        }

        /// <summary>
        /// try to get city's timezone
        /// </summary>
        /// <param name="timeZoneNumber">time zone</param>
        /// <returns>figure out whether this function successful</returns>
        public string TryGetTimeZoneString(double timeZoneNumber)
        {
            //if Initialize faied or timeZoneNumber is not in range -12 to 12, return null
            if (!m_isTimeZonesValid || timeZoneNumber > 13 || timeZoneNumber < -12)
            {
                return null;
            }
            string timeZoneString = null;
            string temp = null;

            //try to get a string like "(GMT+08:00)",
            //the number in string associate with timeZoneNumber           
            //first if timeZoneNumber is 0
            if (0 == timeZoneNumber)
            {
                temp = "(GMT)";
            }
            else
            {
                //if timeZoneNumber > 0
                if (timeZoneNumber > 0)
                {
                    if (timeZoneNumber > 9)
                    {
                        temp = "(GMT+";
                    }
                    else
                    {
                        temp = "(GMT+0";
                    }
                }
                //if timeZoneNumber < 0
                else
                {
                    if (timeZoneNumber < -9)
                    {
                        temp = "(GMT-";
                    }
                    else
                    {
                        temp = "(GMT-0";
                    }
                }

                //if timeZoneNumber is not int, append ":30" to string
                int intNumber = (int)timeZoneNumber;
                if (0.5 == Math.Abs(timeZoneNumber - intNumber))
                {
                    temp += Math.Abs(intNumber) + ":30)";
                }
                else
                {
                    temp += Math.Abs(intNumber) + ":00)";
                }
            }

            //try to find string in list m_timeZones contains string get above
            for (int i = 0; i < m_timeZones.Count; i++)
            {
                if (m_timeZones[i].Contains(temp))
                {
                    //here, use last member of list contains that string as result.
                    timeZoneString = m_timeZones[i];
                }
            }
            return timeZoneString;
        }

        /// <summary>
        /// try to get TimeZone's number from a string
        /// </summary>
        /// <param name="timeZoneString">a string store TimeZone</param>
        /// <returns>result Parse from string</returns>
        public double TryGetTimeZoneNumber(string timeZoneString)
        {
            bool isPlus;
            double result = 0;
            //check timezone is plus, zero or negative
            if (timeZoneString.Contains("+"))
            {
                isPlus = true;
            }
            else if (timeZoneString.Contains("-"))
            {
                isPlus = false;
            }
            else
            {
                return result;
            }

            //get int and decimal part of timezone
            string intString = timeZoneString.Substring(5, 2);
            string decimalString = timeZoneString.Substring(8, 1);
            double intNumber;
            double decimalNumber;

            //try to get double from string using method Double.TryParse
            if (Double.TryParse(intString, out intNumber) &&
               Double.TryParse(decimalString, out decimalNumber))
            {
                //if decimal part is not zero, add 0.5 after int part
                if (0 != decimalNumber)
                {
                    result = intNumber + 0.5;
                }
                else
                {
                    result = intNumber;
                }
            }

            //if timezone is negative, add minus
            if (!isPlus)
            {
                result *= -1;
            }
            return result;
        }

        /// <summary>
        /// initialize cities
        /// </summary>
        /// <param name="cities"></param>
        /// <returns></returns>
        private bool InitCities(CitySet cities)
        {
            if (null == cities)
            {
                return false;
            }

            //add all element of CitySet cities to List m_cities
            CitySetIterator iter = cities.ForwardIterator();
            iter.Reset();
            for (; iter.MoveNext(); )
            {
                City city = iter.Current as City;
                if (null == city)
                {
                    continue;
                }
                m_citiesName.Add(city.Name);
                m_citiesInfo.Add(new CityInfo(city.Latitude, city.Longitude, city.TimeZone, city.Name));
            }
            //sort list according to first char of element
            m_citiesName.Sort();
            return true;
        }

        /// <summary>
        /// initialize timezone
        /// </summary>
        /// <returns></returns>
        private bool InitTimeZone()
        {
            StreamReader streamReader = null;
            try
            {
                //open file store timezone
                string filepath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                if (!filepath.EndsWith("\\"))
                {
                    filepath += "\\";
                }
                filepath += "timezone.txt";
                streamReader = File.OpenText(filepath);

                //add timezone to m_timeZones
                while (!streamReader.EndOfStream)
                {
                    string text = streamReader.ReadLine();
                    if (null != text)
                    {
                        m_timeZones.Add(text);
                    }
                }
            }
            catch (Exception e)
            {
                m_isTimeZonesValid = false;

                //show message to tell user that initialize failed
                TaskDialog.Show("Revit", e.Message);
                return false;
            }
            finally
            {
                //close file resource
                if (null != streamReader)
                {
                    streamReader.Close();
                }
            }
            m_isTimeZonesValid = true;
            return true;
        }
    }
}
