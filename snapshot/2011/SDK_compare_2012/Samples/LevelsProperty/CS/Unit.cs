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
using System.Text;
using Autodesk.Revit;
using Autodesk.Revit.DB;

namespace Revit.SDK.Samples.LevelsProperty.CS
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
            switch (to)
            {
                case DisplayUnitType.DUT_FAHRENHEIT:
                    return value * ImperialDutRatio(to) - 459.67;
                case DisplayUnitType.DUT_CELSIUS:
                    return value - 273.15;
                default:
                    return value *= ImperialDutRatio(to);
            }
        }

        /// <summary>
        /// Convert a value indicated by DisplayUnitType to the value used by RevitAPI
        /// </summary>
        /// <param name="value">Value to be converted</param>
        /// <param name="from">DisplayUnitType indicates the unit of the value to be converted</param>
        /// <returns>Target value</returns>
        public static double CovertToAPI(double value, DisplayUnitType from)
        {
            switch (from)
            {
                case DisplayUnitType.DUT_FAHRENHEIT:
                    return (value + 459.67) / ImperialDutRatio(from);
                case DisplayUnitType.DUT_CELSIUS:
                    return value + 273.15;
                default:
                    return value /= ImperialDutRatio(from);
            }
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
                case DisplayUnitType.DUT_ACRES: return 2.29568411386593E-05;
                case DisplayUnitType.DUT_AMPERES: return 1;
                case DisplayUnitType.DUT_ATMOSPHERES: return 3.23793722675857E-05;
                case DisplayUnitType.DUT_BARS: return 3.28083989501312E-05;
                case DisplayUnitType.DUT_BRITISH_THERMAL_UNITS: return 8.80550918411529E-05;
                case DisplayUnitType.DUT_BRITISH_THERMAL_UNITS_PER_HOUR: return 0.316998330628151;
                case DisplayUnitType.DUT_BRITISH_THERMAL_UNITS_PER_SECOND: return 8.80550918411529E-05;
                case DisplayUnitType.DUT_CALORIES: return 0.0221895098882201;
                case DisplayUnitType.DUT_CALORIES_PER_SECOND: return 0.0221895098882201;
                case DisplayUnitType.DUT_CANDELAS: return 1;
                case DisplayUnitType.DUT_CANDELAS_PER_SQUARE_METER: return 10.7639104167097;
                case DisplayUnitType.DUT_CANDLEPOWER: return 1;
                case DisplayUnitType.DUT_CELSIUS: return 1;
                case DisplayUnitType.DUT_CENTIMETERS: return 30.48;
                case DisplayUnitType.DUT_CENTIMETERS_PER_MINUTE: return 1828.8;
                case DisplayUnitType.DUT_CENTIPOISES: return 3280.83989501312;
                case DisplayUnitType.DUT_CUBIC_CENTIMETERS: return 28316.846592;
                case DisplayUnitType.DUT_CUBIC_FEET: return 1;
                case DisplayUnitType.DUT_CUBIC_FEET_PER_KIP: return 14593.9029372064;
                case DisplayUnitType.DUT_CUBIC_FEET_PER_MINUTE: return 60;
                case DisplayUnitType.DUT_CUBIC_INCHES: return 1728;
                case DisplayUnitType.DUT_CUBIC_METERS: return 0.028316846592;
                case DisplayUnitType.DUT_CUBIC_METERS_PER_HOUR: return 101.9406477312;
                case DisplayUnitType.DUT_CUBIC_METERS_PER_KILONEWTON: return 92.90304;
                case DisplayUnitType.DUT_CUBIC_METERS_PER_SECOND: return 0.028316846592;
                case DisplayUnitType.DUT_CUBIC_MILLIMETERS: return 28316846.592;
                case DisplayUnitType.DUT_CUBIC_YARDS: return 0.037037037037037;
                case DisplayUnitType.DUT_CYCLES_PER_SECOND: return 1;
                case DisplayUnitType.DUT_DECANEWTONS: return 0.03048;
                case DisplayUnitType.DUT_DECANEWTONS_PER_METER: return 0.1;
                case DisplayUnitType.DUT_DECANEWTONS_PER_SQUARE_METER: return 0.328083989501312;
                case DisplayUnitType.DUT_DECANEWTON_METERS: return 0.009290304;
                case DisplayUnitType.DUT_DECANEWTON_METERS_PER_METER: return 0.03048;
                case DisplayUnitType.DUT_DECIMAL_DEGREES: return 57.2957795130823;
                case DisplayUnitType.DUT_DECIMAL_FEET: return 1;
                case DisplayUnitType.DUT_DECIMAL_INCHES: return 12;
                case DisplayUnitType.DUT_DEGREES_AND_MINUTES: return 57.2957795130823;
                case DisplayUnitType.DUT_FAHRENHEIT: return 1.8;
                case DisplayUnitType.DUT_FEET_FRACTIONAL_INCHES: return 1;
                case DisplayUnitType.DUT_FEET_OF_WATER: return 0.00109764531546318;
                case DisplayUnitType.DUT_FEET_OF_WATER_PER_100FT: return 0.109761336731934;
                case DisplayUnitType.DUT_FEET_PER_KIP: return 14593.9029372064;
                case DisplayUnitType.DUT_FEET_PER_MINUTE: return 60;
                case DisplayUnitType.DUT_FEET_PER_SECOND: return 1;
                case DisplayUnitType.DUT_FIXED: return 1;
                case DisplayUnitType.DUT_FOOTCANDLES: return 1.0000000387136;
                case DisplayUnitType.DUT_FOOTLAMBERTS: return 3.1415927449471;
                case DisplayUnitType.DUT_FRACTIONAL_INCHES: return 12;
                case DisplayUnitType.DUT_GALLONS_US: return 7.48051905367236;
                case DisplayUnitType.DUT_GALLONS_US_PER_HOUR: return 26929.8685932205;
                case DisplayUnitType.DUT_GALLONS_US_PER_MINUTE: return 448.831143220342;
                case DisplayUnitType.DUT_GENERAL: return 1;
                case DisplayUnitType.DUT_HECTARES: return 9.290304E-06;
                case DisplayUnitType.DUT_HERTZ: return 1;
                case DisplayUnitType.DUT_HORSEPOWER: return 0.00012458502883053;
                case DisplayUnitType.DUT_INCHES_OF_MERCURY: return 0.000968831370233344;
                case DisplayUnitType.DUT_INCHES_OF_WATER: return 0.0131845358262865;
                case DisplayUnitType.DUT_INCHES_OF_WATER_PER_100FT: return 1.31845358262865;
                case DisplayUnitType.DUT_INV_CELSIUS: return 1;
                case DisplayUnitType.DUT_INV_FAHRENHEIT: return 0.555555555555556;
                case DisplayUnitType.DUT_INV_KILONEWTONS: return 3280.83989501312;
                case DisplayUnitType.DUT_INV_KIPS: return 14593.9029372064;
                case DisplayUnitType.DUT_JOULES: return 0.09290304;
                case DisplayUnitType.DUT_KELVIN: return 1;
                case DisplayUnitType.DUT_KILOAMPERES: return 0.001;
                case DisplayUnitType.DUT_KILOCALORIES: return 2.21895098882201E-05;
                case DisplayUnitType.DUT_KILOCALORIES_PER_SECOND: return 2.21895098882201E-05;
                case DisplayUnitType.DUT_KILOGRAMS_FORCE: return 0.0310810655372411;
                case DisplayUnitType.DUT_KILOGRAMS_FORCE_PER_METER: return 0.101971999794098;
                case DisplayUnitType.DUT_KILOGRAMS_FORCE_PER_SQUARE_METER: return 0.334553805098747;
                case DisplayUnitType.DUT_KILOGRAMS_PER_CUBIC_METER: return 35.3146667214886;
                case DisplayUnitType.DUT_KILOGRAM_FORCE_METERS: return 0.00947350877575109;
                case DisplayUnitType.DUT_KILOGRAM_FORCE_METERS_PER_METER: return 0.0310810655372411;
                case DisplayUnitType.DUT_KILONEWTONS: return 0.0003048;
                case DisplayUnitType.DUT_KILONEWTONS_PER_CUBIC_METER: return 0.0107639104167097;
                case DisplayUnitType.DUT_KILONEWTONS_PER_METER: return 0.001;
                case DisplayUnitType.DUT_KILONEWTONS_PER_SQUARE_METER: return 0.00328083989501312;
                case DisplayUnitType.DUT_KILONEWTON_METERS: return 9.290304E-05;
                case DisplayUnitType.DUT_KILONEWTON_METERS_PER_DEGREE: return 9.290304E-05;
                case DisplayUnitType.DUT_KILONEWTON_METERS_PER_DEGREE_PER_METER: return 0.0003048;
                case DisplayUnitType.DUT_KILONEWTON_METERS_PER_METER: return 0.0003048;
                case DisplayUnitType.DUT_KILOPASCALS: return 0.00328083989501312;
                case DisplayUnitType.DUT_KILOVOLTS: return 9.290304E-05;
                case DisplayUnitType.DUT_KILOVOLT_AMPERES: return 9.290304E-05;
                case DisplayUnitType.DUT_KILOWATTS: return 9.290304E-05;
                case DisplayUnitType.DUT_KILOWATT_HOURS: return 2.58064E-08;
                case DisplayUnitType.DUT_KIPS: return 0.224808943099711;
                case DisplayUnitType.DUT_KIPS_PER_CUBIC_FOOT: return 6.85217658567918E-05;
                case DisplayUnitType.DUT_KIPS_PER_CUBIC_INCH: return 3.96537996856434E-08;
                case DisplayUnitType.DUT_KIPS_PER_FOOT: return 6.85217658567918E-05;
                case DisplayUnitType.DUT_KIPS_PER_INCH: return 5.71014715473265E-06;
                case DisplayUnitType.DUT_KIPS_PER_SQUARE_FOOT: return 6.85217658567918E-05;
                case DisplayUnitType.DUT_KIPS_PER_SQUARE_INCH: return 4.75845596227721E-07;
                case DisplayUnitType.DUT_KIP_FEET: return 6.85217658567918E-05;
                case DisplayUnitType.DUT_KIP_FEET_PER_DEGREE: return 2.08854342331501E-05;
                case DisplayUnitType.DUT_KIP_FEET_PER_DEGREE_PER_FOOT: return 2.08854342331501E-05;
                case DisplayUnitType.DUT_KIP_FEET_PER_FOOT: return 6.85217658567918E-05;
                case DisplayUnitType.DUT_LITERS: return 28.316846592;
                case DisplayUnitType.DUT_LITERS_PER_SECOND: return 28.316846592;
                case DisplayUnitType.DUT_LUMENS: return 1;
                case DisplayUnitType.DUT_LUX: return 10.7639104167097;
                case DisplayUnitType.DUT_MEGANEWTONS: return 3.048E-07;
                case DisplayUnitType.DUT_MEGANEWTONS_PER_METER: return 1E-06;
                case DisplayUnitType.DUT_MEGANEWTONS_PER_SQUARE_METER: return 3.28083989501312E-06;
                case DisplayUnitType.DUT_MEGANEWTON_METERS: return 9.290304E-08;
                case DisplayUnitType.DUT_MEGANEWTON_METERS_PER_METER: return 3.048E-07;
                case DisplayUnitType.DUT_MEGAPASCALS: return 3.28083989501312E-06;
                case DisplayUnitType.DUT_METERS: return 0.3048;
                case DisplayUnitType.DUT_METERS_CENTIMETERS: return 0.3048;
                case DisplayUnitType.DUT_METERS_PER_KILONEWTON: return 1000;
                case DisplayUnitType.DUT_METERS_PER_SECOND: return 0.3048;
                case DisplayUnitType.DUT_MILLIAMPERES: return 1000;
                case DisplayUnitType.DUT_MILLIMETERS: return 304.8;
                case DisplayUnitType.DUT_MILLIMETERS_OF_MERCURY: return 0.0246083170946002;
                case DisplayUnitType.DUT_MILLIVOLTS: return 92.90304;
                case DisplayUnitType.DUT_NEWTONS: return 0.3048;
                case DisplayUnitType.DUT_NEWTONS_PER_METER: return 1;
                case DisplayUnitType.DUT_NEWTONS_PER_SQUARE_METER: return 3.28083989501312;
                case DisplayUnitType.DUT_NEWTON_METERS: return 0.09290304;
                case DisplayUnitType.DUT_NEWTON_METERS_PER_METER: return 0.3048;
                case DisplayUnitType.DUT_PASCALS: return 3.28083989501312;
                case DisplayUnitType.DUT_PASCALS_PER_METER: return 10.7639104167097;
                case DisplayUnitType.DUT_PASCAL_SECONDS: return 3.28083989501312;
                case DisplayUnitType.DUT_PERCENTAGE: return 100;
                case DisplayUnitType.DUT_POUNDS_FORCE: return 224.80894309971;
                case DisplayUnitType.DUT_POUNDS_FORCE_PER_CUBIC_FOOT: return 0.0685217658567918;
                case DisplayUnitType.DUT_POUNDS_FORCE_PER_FOOT: return 0.0685217658567918;
                case DisplayUnitType.DUT_POUNDS_FORCE_PER_SQUARE_FOOT: return 0.0685217658567917;
                case DisplayUnitType.DUT_POUNDS_FORCE_PER_SQUARE_INCH: return 0.000475845616460903;
                case DisplayUnitType.DUT_POUNDS_MASS_PER_CUBIC_FOOT: return 2.20462262184878;
                case DisplayUnitType.DUT_POUNDS_MASS_PER_CUBIC_INCH: return 0.00127582327653286;
                case DisplayUnitType.DUT_POUNDS_MASS_PER_FOOT_HOUR: return 7936.64143865559;
                case DisplayUnitType.DUT_POUNDS_MASS_PER_FOOT_SECOND: return 2.20462262184878;
                case DisplayUnitType.DUT_POUND_FORCE_FEET: return 0.0685217658567918;
                case DisplayUnitType.DUT_POUND_FORCE_FEET_PER_FOOT: return 0.0685217658567918;
                case DisplayUnitType.DUT_RANKINE: return 1.8;
                case DisplayUnitType.DUT_SQUARE_CENTIMETERS: return 929.0304;
                case DisplayUnitType.DUT_SQUARE_FEET: return 1;
                case DisplayUnitType.DUT_SQUARE_FEET_PER_KIP: return 14593.9029372064;
                case DisplayUnitType.DUT_SQUARE_INCHES: return 144;
                case DisplayUnitType.DUT_SQUARE_METERS: return 0.09290304;
                case DisplayUnitType.DUT_SQUARE_METERS_PER_KILONEWTON: return 304.8;
                case DisplayUnitType.DUT_SQUARE_MILLIMETERS: return 92903.04;
                case DisplayUnitType.DUT_THERMS: return 8.80547457016663E-10;
                case DisplayUnitType.DUT_TONNES_FORCE: return 3.10810655372411E-05;
                case DisplayUnitType.DUT_TONNES_FORCE_PER_METER: return 0.000101971999794098;
                case DisplayUnitType.DUT_TONNES_FORCE_PER_SQUARE_METER: return 0.000334553805098747;
                case DisplayUnitType.DUT_TONNE_FORCE_METERS: return 9.47350877575109E-06;
                case DisplayUnitType.DUT_TONNE_FORCE_METERS_PER_METER: return 3.10810655372411E-05;
                case DisplayUnitType.DUT_VOLTS: return 0.09290304;
                case DisplayUnitType.DUT_VOLT_AMPERES: return 0.09290304;
                case DisplayUnitType.DUT_WATTS: return 0.09290304;
                case DisplayUnitType.DUT_WATTS_PER_SQUARE_FOOT: return 0.09290304;
                case DisplayUnitType.DUT_WATTS_PER_SQUARE_METER: return 1;
                default: return 1;
            }
        }
        #endregion
    }
}
