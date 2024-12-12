//
// (C) Copyright 2003-2017 by Autodesk, Inc.
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
using System.Globalization;

namespace Revit.SDK.Samples.SharedCoordinateSystem.CS
{    
    /// <summary>
    /// define type of value
    /// </summary>
    public enum ValueType
    {
        /// <summary>
        /// general value
        /// </summary>
        General = 0,

        /// <summary>
        /// angle value
        /// </summary>
        Angle
    }


    /// <summary>
    /// a class used to deal with converting operation
    /// </summary>
    public class UnitConversion
    {       
        private static readonly int DefaultPrecision = 3;  //default precision 
        private static readonly double AngleRatio = 0.0174532925199433;  //ratio of Angle

        /// <summary>
        /// convert CityInfo into CityInfoString
        /// </summary>
        /// <param name="cityInfo">CityInfo need to convert</param>
        /// <returns>conversion result</returns>
        public static CityInfoString ConvertFrom(CityInfo cityInfo)
        {
            CityInfoString cityInfoString = new CityInfoString();            
            cityInfoString.Latitude = DoubleToString(cityInfo.Latitude, ValueType.Angle);
            cityInfoString.Longitude = DoubleToString(cityInfo.Longitude, ValueType.Angle);
            return cityInfoString;
        }

        /// <summary>
        /// convert CityInfoString into CityInfo
        /// </summary>
        /// <param name="cityInfoString">CityInfoString need to convert</param>
        /// <returns>conversion result</returns>
        public static CityInfo ConvertTo(CityInfoString cityInfoString)
        {
            CityInfo cityInfo = new CityInfo();
            double temp;

            //convert Latitude
            if (StringToDouble(cityInfoString.Latitude, ValueType.Angle, out temp))
            {
                cityInfo.Latitude = temp;
            }
            else
            {
                cityInfo.Latitude = Double.NaN;
            }

            //convert Longitude
            if (StringToDouble(cityInfoString.Longitude, ValueType.Angle, out temp))
            {
                cityInfo.Longitude = temp;
            }
            else
            {
                cityInfo.Longitude = Double.NaN;
            }

            return cityInfo;
        }

        /// <summary>
        /// deal with value according to precision
        /// </summary>
        /// <param name="value">original value will be dealed</param>
        /// <param name="precision">precision wanted to be set</param>
        /// <returns>return the dealed value</returns>
        public static double DealPrecision(double value, int precision)
        {
           //first make sure 0 =< precision <= 15
           if (precision < 0 && precision > 15)
           {
              return value;
           }

           //if >1 or < -1,just use Math.Round to deal with
           double newValue;
           if (value >= 1 || value <= -1 || 0 == value)
           {
              //Math.Round: returns the number with the specified precision
              //nearest the specified value.
              newValue = Math.Round(value, precision);
              return newValue;
           }

           //if -1 < value < 1, 
           //find first number which is not "0"
           //compare it with precision, then select
           //min of them as final precision
           int firstNumberPos = 0;
           double temp = Math.Abs(value);
           for (firstNumberPos = 1; ; firstNumberPos++)
           {
              temp *= 10;
              if (temp >= 1)
              {
                 break;
              }
           }

           //make sure firstNumberPos <= 15
           if (firstNumberPos > 15)
           {
               firstNumberPos = 15;
           }

           //Math.Round: returns the number with the specified precision
           //nearest the specified value.
           newValue = Math.Round(value, firstNumberPos > precision ? firstNumberPos : precision);
           return newValue;
        }            

        /// <summary>
        /// convert double into string
        /// </summary>
        /// <param name="value">double value need to convert</param>
        /// <param name="valueType">value type</param>
        /// <returns>conversion result</returns>
        public static string DoubleToString(double value, ValueType valueType)
        {
            string displayText = null; // string included value and unit of parameter
            double newValue;
            ValueConversion(value, ValueType.Angle, true, out newValue);
            value = newValue;
            newValue = DealPrecision(value, DefaultPrecision);

            //calculate the number after ".",if less than DecimalNumber
            // add some "0" after it
            displayText = DealDecimalNumber(newValue.ToString(), DefaultPrecision);            

            if(ValueType.Angle == valueType)
            {
                char degree = (char)0xb0;
                displayText += degree;          
            }
            
            return displayText;
        }

        /// <summary>
        /// deal with decimal number
        /// </summary>
        /// <param name="value">string wanted to deal with</param>
        /// <param name="number">number of decimal</param>
        /// <returns>result dealing with</returns>
        public static string DealDecimalNumber(string value, int number)
        {
            string newValue = value;
            int dist;
            if (newValue.Contains("."))
            {
                int index = newValue.IndexOf(".");
                dist = newValue.Length - (index + 1);
            }
            else
            {
                dist = 0;
                newValue += ".";
            }
            if (dist < number)
            {
                for (int i = 0; i < number - dist; i++)
                {
                    newValue += "0";
                }
            }
            return newValue;
        }

        /// <summary>
        /// convert string into double
        /// </summary>
        /// <param name="value">string value need to convert</param>
        /// <param name="valueType">value type</param>
        /// <param name="newValue">conversion result</param>
        /// <returns>if success, return true; otherwise, return false</returns>
        public static bool StringToDouble(string value, ValueType valueType, out double newValue)
        {
            newValue = 0;

            if (null == value)
            {
                return false;
            }

            //try to Parse double from string
            double result;
            if (ParseFromString(value, valueType, out result))
            {
                //deal with ratio
                ValueConversion(result, valueType, false, out newValue);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Parse double from string
        /// </summary>
        /// <param name="value">string value</param>
        /// <param name="valueType">value type</param>
        /// <param name="result">conversion result</param>
        /// <returns>if success, return true; otherwise, return false</returns>
        private static bool ParseFromString(string value, ValueType valueType, out double result)
        {
            string newValue = null;
            string degree = ((char)0xb0).ToString();

            //if nothing, set result = 0;
            if (value.Length == 0)
            {
                result = 0;
                return true;
            }
            else if (ValueType.General == valueType)
            {                 
            }
            //check if contain degree symbol
            else if (value.Contains(degree))
            {
                int index = value.IndexOf(degree);
                newValue = value.Substring(0, index);
            }
            //check if have string" " ,for there is string" " 
            //between value and unit when show in PropertyGrid
            else if (value.Contains(" "))
            {
                int index = value.IndexOf(" ");
                newValue = value.Substring(0, index);
            }
            //finally if don't have unit name in it 
            //other situation, set newValue = value
            else
            {
                newValue = value;
            }

            //double.TryParse's return value:
            //true if s is converted successfully; otherwise, false.
            if (double.TryParse(newValue, out result))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// deal with ratio
        /// </summary>
        /// <param name="value">value need to deal with</param>
        /// <param name="valueType">value type</param>
        /// <param name="isDoubleToString">
        /// figure out whether be called by function "DoubleToString"
        /// </param>
        /// <param name="newValue"></param>
        private static void ValueConversion(double value, ValueType valueType,
            bool isDoubleToString, out double newValue)
        {
            //ValueType.General == valueType,do nothing and return
            if (ValueType.General == valueType)
            {
                newValue = value;
                return;
            }

            //otherwise,check whether be called by function "DoubleToString"
            if (isDoubleToString)
            {
                newValue = value / AngleRatio;
            }
            else
            {
                newValue = value * AngleRatio;
            }
        }
    }
}
