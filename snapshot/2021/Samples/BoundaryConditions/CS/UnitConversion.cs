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

namespace Revit.SDK.Samples.BoundaryConditions.CS
{
    /// <summary>
    /// a structure about the conversion rule between display value and inside value
    /// </summary>
    public struct ConversionValue
    {
        // members
        private int m_precision;   // the precision of the diaplay value
        private string m_unitName; // the unit of the display value
        private double m_ratio;    // the radio of inside value to display value

        /// <summary>
        /// constructor, using to initialize the members
        /// </summary>
        /// <param name="precision">the precision of the diaplay value</param>
        /// <param name="ratio">the unit of the display value</param>
        /// <param name="unitName">the radio of inside value to display valuea</param>
        public ConversionValue(int precision, double ratio, string unitName)
        {

            m_precision = precision;
            m_ratio     = ratio;
            m_unitName  = unitName;
        }

        /// <summary>
        /// get the precision of the diaplay value
        /// </summary>
        public int Precision
        {
            get 
            {
                return m_precision;
            }
        }

        /// <summary>
        /// get the unit of the display value
        /// </summary>
        public string UnitName
        {
            get 
            {
                return m_unitName;
            }
        }

        /// <summary>
        /// get the radio of inside value to display value
        /// </summary>
        public double Ratio
        {
            get 
            {
                return m_ratio;
            }
        }
    };

    /// <summary>
    /// value conversion dictionary, in this class the data we gave only fit for Imperial unit
    /// the relationship about dislay paramete values and inside parameter values
    /// </summary>
    public static class UnitConversion
    {
        // member: dictionary
        private static Dictionary<string, ConversionValue> m_unitDictionary;

        /// <summary>
        /// get the value dictionary
        /// </summary>
        public static Dictionary<string, ConversionValue> UnitDictionary
        {
            get
            {
                return m_unitDictionary;
            }
        }

        /// <summary>
        /// static constructor to initialize the value conversion dictionary which we will used 
        /// according to the difference unit
        /// </summary>
        static UnitConversion()
        {
            m_unitDictionary = new Dictionary<string, ConversionValue>();

            #region "fill my units dictionary"
            char degree = (char)0xb0;
            char square = (char)0xB2;
            char cube = (char)0xB3;

            // about point BC's translation spring modulus
            AddNewUnit(1, 175126.835246476, "kip/in", "PTSpringModulusConver");

            // about point BC's rotation spring modulus
            AddNewUnit(1, 47880.2589803358, "kip-f/" + degree + "F", "PRSpringModulusConver");

            // about Line BC's translation spring modulus
            AddNewUnit(4, 14593.9029372064, "kip/ft" + square, "LTSpringModulusConver");

            // about Line BC's rotation spring modulus
            AddNewUnit(1, 47880.2589803358, "kip-f/" + degree + "F/ft", "LRSpringModulusConver");

            // about Area BC's translation spring modulus
            AddNewUnit(1, 14593.9029372064, "kip/ft" + cube, "ATSpringModulusConver");

            #endregion
        }

        /// <summary>
        /// add a new unit conversion item
        /// </summary>
        /// <param name="precision"> the precision of the display value</param>
        /// <param name="ratio"> the radio of inside value to display value about current unit</param>
        /// <param name="unitName"> the unit of the display value</param>
        /// <param name="key"></param>
        private static void AddNewUnit(int precision, double ratio, string unitName, string key)
        {
            // create a ConversionValue instance
            ConversionValue conversionValue = new ConversionValue(precision, ratio, unitName);

            // add this item to ourself dictionary
            m_unitDictionary.Add(key, conversionValue);
        }
    }
}
