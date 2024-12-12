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
using System.Text;
using System.Collections;
using System.Windows.Forms;


using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Enums;
using Autodesk.Revit.Parameters;


namespace Revit.SDK.Samples.Materials.CS
{
    
    /// <summary>
    /// used to defined unit's type
    /// </summary>
    public enum UnitType
    {
        Temperature = 0,    //for ThermalExpansionCoefficientX(Y,Z)
        UnitWeight,         //for property UnitWeight
        Stress,             //for ShearModulusX(Y,Z) and so on
        Other,              //special for Smoothness and Transparency
        No                  //for PoissonRatioX(Y,Z)and so on,which have no unit
    }
    

    /// <summary>
    /// a struct define conversion detail
    /// </summary>
    public struct ConversionValue
    {
        private int m_precision;   //precision when value show in PropertyGrid
        private string m_unitName; //unit'name shown in PropertyGrid
        private double m_ratio;    //ratio between value obtain by API 
                                   //and value show in PropertyGrid 

        /// <summary>
        /// precision of value while display
        /// </summary>
        public int Precision
        {
            get
            {
                return m_precision;
            }
            set
            {
                m_precision = value;
            }
        }

        /// <summary>
        /// unit'name while display, e.g. show in PropertyGrid
        /// </summary>
        public string UnitName
        {
            get
            {
                return m_unitName;
            }
            set
            {
                m_unitName = value;
            }
        }

        /// <summary>
        /// ratio between value obtain by API 
        /// and value shown in UI
        /// </summary>
        public double Ratio
        {
            get
            {
                return m_ratio;
            }
            set
            {
                m_ratio = value;
            }
        }

        /// <summary>
        /// a constructor of struct ConversionValue
        /// </summary>
        /// <param name="precision"></param>
        /// <param name="unitName"></param>
        /// <param name="ratio"></param>
        public ConversionValue(int precision, string unitName, double ratio)
        {
            m_precision = precision;
            m_unitName = unitName;
            m_ratio = ratio;
        }
    }

    /// <summary>
    /// a class used to store information about conversion,
    /// can obtain these information by method "GetUnitValue"
    /// </summary>
    public class UnitConversion
    {
        private static DisplayUnit m_currentDisplayUnit; //used to store DisplayUnit of current document
        private static readonly Dictionary<UnitType, ConversionValue> m_metricUnitDictionary;
        //used to store UnitType and corresponding ConversionValue when DisplayUnit is metric
        private static readonly Dictionary<UnitType, ConversionValue> m_imperialUnitDictionary;
        //used to store UnitType and corresponding ConversionValue when DisplayUnit is imperial

        /// <summary>
        /// set current DisplayUnit of current document
        /// </summary>
        public static DisplayUnit DisplayUnitSystem
        {
            set
            {
                m_currentDisplayUnit = value;
            }
        }

        /// <summary>
        /// get conversionValue according to DisplayUnitType
        /// </summary>
        /// <param name="unitType"></param>
        /// <param name="conversionValue"></param>
        /// <returns>if success return true, else return false</returns>
        public static bool GetUnitValue(UnitType unitType,
            out ConversionValue conversionValue)
        {
            if (DisplayUnit.METRIC == m_currentDisplayUnit)
            {
                //try to get ConversionValue match UnitType in m_unitDictionary
                if (m_metricUnitDictionary.TryGetValue(unitType, out conversionValue))
                {
                    return true;
                }
                //if not found, set precision = 0, ratio = 1, unitName = null
                //and return false
                else
                {
                    conversionValue.Precision = 0;
                    conversionValue.Ratio = 1;
                    conversionValue.UnitName = "";
                    return false;
                }
            }
            else
            {
                //try to get ConversionValue match UnitType in m_unitDictionary
                if (m_imperialUnitDictionary.TryGetValue(unitType, out conversionValue))
                {
                    return true;
                }
                //if not found, set precision = 0, ratio = 1, unitName = null
                //and return false
                else
                {
                    conversionValue.Precision = 0;
                    conversionValue.Ratio = 1;
                    conversionValue.UnitName = "";
                    return false;
                }
            }
        }

        /// <summary>
        /// convert ito string according to unitType,
        /// then return a string include value and type
        /// </summary>
        /// <param name="value">value of property</param>
        /// <param name="unitType">
        /// unitType store UnitType of property
        /// </param>
        /// <returns></returns>
        public static string ConvertFrom(double value, UnitType unitType)
        {
            string displayText = null; // string included value and unit of parameter
            // will be shown in PropertyGrid
            
            //get convert value by method "GetUnitValue"
            ConversionValue conversionValue;
            GetUnitValue(unitType, out conversionValue);

            //if 0 == conversionValue.Ratio, set conversionValue.Ratio = 1
            if (0 == conversionValue.Ratio)
            {
                conversionValue.Ratio = 1;
            }
            double temp = DealPrecision(value / conversionValue.Ratio, conversionValue.Precision);
            //add UnitType after value if necessary
            if (null == conversionValue.UnitName || "" == conversionValue.UnitName ||
                " " == conversionValue.UnitName)
            {
                displayText = temp.ToString();
            }
            else
            {
                displayText = temp + " " + conversionValue.UnitName;
            }
            return displayText;
        }

        /// <summary>
        /// convert a string into double,
        /// </summary>
        /// <param name="value">value of property</param>
        /// <param name="result">
        /// the result of Method"ConvertTo"
        /// </param>
        /// <param name="unitType">
        /// unitType store UnitType of property
        /// </param>
        public static bool ConvertTo(string value, out double result, UnitType unitType)
        {
            //get convert value by class method "GetUnitValue"
            ConversionValue conversionValue;
            GetUnitValue(unitType, out conversionValue);

            //true if s is converted successfully; otherwise, false.
            if (ParseFromString(value, conversionValue, out result))
            {
                result *= conversionValue.Ratio;
                return true;
            }
            return false;
        }

        /// <summary>
        /// try to parse string to double
        /// </summary>
        /// <param name="value">input string</param>
        /// <param name="conversionValue">ConversionValue about value</param>
        /// <param name="result">the result of this method</param>
        /// <returns>successful return true; otherwise return false</returns>
        static bool ParseFromString(string value, ConversionValue conversionValue, out double result)
        {
            //try to get value from string. 
            string newValue = null;

            //if nothing, set result = 0;
            if (value.Length == 0)
            {
                result = 0;
                return true;
            }

            //check if contain string got by method "GetUnitValue",
            //first make sure conversionValue.UnitName is not null
            else if (null != conversionValue.UnitName && value.Contains(conversionValue.UnitName) &&
                "" != conversionValue.UnitName && " " != conversionValue.UnitName)
            {
                int index = value.IndexOf(conversionValue.UnitName);
                newValue = value.Substring(0, index);
            }
            //check if have string" " ,for there is string" " 
            //between value and unit when show in PropertyGrid
            else if (value.Contains(" "))
            {
                int index = value.IndexOf(" ");
                //if have " " at the beginning of string value
                if (0 == index)
                {
                    newValue = value;
                }
                else
                {
                    newValue = value.Substring(0, index);
                }
            }
            //final if value don't have unit name in it, set newValue = value
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
        /// deal with value according to precision
        /// </summary>
        /// <param name="value">original value will be dealt</param>
        /// <param name="precision">precision wanted to be set</param>
        /// <returns>return the dealt value</returns>
        static double DealPrecision(double value, int precision)
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
            
            if(firstNumberPos > 15)
            {
                firstNumberPos = 15;
            }

            //Math.Round: returns the number with the specified precision
            //nearest the specified value.
            newValue = Math.Round(value, firstNumberPos > precision ? firstNumberPos : precision);
            return newValue;
        }

        /// <summary>
        /// initialize all Dictionaries 
        /// </summary>
        static UnitConversion()
        {
            //initialize Dictionary m_metricUnitDictionary and  m_imperialUnitDictionary
            m_metricUnitDictionary = new Dictionary<UnitType, ConversionValue>();
            m_imperialUnitDictionary = new Dictionary<UnitType, ConversionValue>();

            m_metricUnitDictionary.Add(UnitType.Stress,
                new ConversionValue(6, "MPa", 304800));
            m_imperialUnitDictionary.Add(UnitType.Stress,
                new ConversionValue(2, "ksi", 2101522.0229577161));

            m_metricUnitDictionary.Add(UnitType.Temperature,
                new ConversionValue(8, "1/" + (char)0x00B0 + "C", 1));
            m_imperialUnitDictionary.Add(UnitType.Temperature,
                new ConversionValue(8, "1/" + (char)0x00B0 + "F", 1.8));

            m_metricUnitDictionary.Add(UnitType.UnitWeight,
                new ConversionValue(2, "kN/m" + (char)0x00B3, 92.90304));
            m_imperialUnitDictionary.Add(UnitType.UnitWeight,
                new ConversionValue(2, "lb/ft" + (char)0x00B3, 14.593902937206364));

            //special for Smoothness and Transparency
            m_metricUnitDictionary.Add(UnitType.Other, new ConversionValue(2, "", 0.01));
            m_imperialUnitDictionary.Add(UnitType.Other, new ConversionValue(2, "", 0.01));

            //others set default value as below
            m_metricUnitDictionary.Add(UnitType.No, new ConversionValue(2, "", 1));
            m_imperialUnitDictionary.Add(UnitType.No, new ConversionValue(2, "", 1));
        }
    }
}
