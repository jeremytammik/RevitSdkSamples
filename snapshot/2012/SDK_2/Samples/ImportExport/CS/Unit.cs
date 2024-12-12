//
// (C) Copyright 2003-2011 by Autodesk, Inc.
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
using Autodesk.Revit.DB;
using System.Configuration;
using System.Reflection;

namespace Revit.SDK.Samples.ImportExport.CS
{
    /// <summary>
    /// Provides static functions to convert unit
    /// </summary>
    static class Unit
    {
        #region Methods
        /// <summary>
        /// Convert the value get from RevitAPI to the value indicated by DisplayUnitType
        /// </summary>
        /// <param name="to">DisplayUnitType indicates unit of target value</param>
        /// <param name="value">value get from RevitAPI</param>
        /// <returns>Target value</returns>
        public static double CovertFromAPI(DisplayUnitType to, double value)
        {
            return value *= ImperialDutRatio(to);
        }

        /// <summary>
        /// Convert a value indicated by DisplayUnitType to the value used by RevitAPI
        /// </summary>
        /// <param name="value">Value to be converted</param>
        /// <param name="from">DisplayUnitType indicates the unit of the value to be converted</param>
        /// <returns>Target value</returns>
        public static double CovertToAPI(double value, DisplayUnitType from )
        {
            return value /= ImperialDutRatio(from);
        }

        /// <summary>
        /// Get ratio between value in RevitAPI and value to display indicated by DisplayUnitType
        /// </summary>
        /// <param name="dut">DisplayUnitType indicates display unit type</param>
        /// <returns>Ratio </returns>
        private static double ImperialDutRatio(DisplayUnitType dut) 
        {
            switch (dut)
            {
                case DisplayUnitType.DUT_DECIMAL_FEET: return 1;
                case DisplayUnitType.DUT_FEET_FRACTIONAL_INCHES: return 1;
                case DisplayUnitType.DUT_DECIMAL_INCHES: return 12;
                case DisplayUnitType.DUT_FRACTIONAL_INCHES: return 12;
                case DisplayUnitType.DUT_METERS: return 0.3048;
                case DisplayUnitType.DUT_CENTIMETERS: return 30.48;
                case DisplayUnitType.DUT_MILLIMETERS: return 304.8;
                case DisplayUnitType.DUT_METERS_CENTIMETERS: return 0.3048;
                default: return 1;
            }
        }
        #endregion
    }
}
