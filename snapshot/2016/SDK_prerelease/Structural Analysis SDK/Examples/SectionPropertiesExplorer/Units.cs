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
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Autodesk.Revit.DB;

namespace SectionPropertiesExplorer
{
    internal class UnitsTypeAssignment
    {
        public UnitsTypeAssignment(UnitSymbolType symbolType, DisplayUnitType displayType)
        {
            SymbolType = symbolType;
            DisplayType = displayType;
        }

        public UnitSymbolType SymbolType { get; private set; }
        public DisplayUnitType DisplayType { get; private set; }
    }

    internal class UnitsAssignment
    {
        static UnitsAssignment()
        {
            imperialSettings = new Dictionary<UnitType, UnitsTypeAssignment>();
            imperialSettings.Add(UnitType.UT_Length, new UnitsTypeAssignment(UnitSymbolType.UST_IN_HG, DisplayUnitType.DUT_DECIMAL_INCHES));
            imperialSettings.Add(UnitType.UT_Section_Property, new UnitsTypeAssignment(UnitSymbolType.UST_IN_HG, DisplayUnitType.DUT_DECIMAL_INCHES));
            imperialSettings.Add(UnitType.UT_Section_Dimension, new UnitsTypeAssignment(UnitSymbolType.UST_IN_HG, DisplayUnitType.DUT_DECIMAL_INCHES));
            imperialSettings.Add(UnitType.UT_Section_Area, new UnitsTypeAssignment(UnitSymbolType.UST_IN_SUP_2, DisplayUnitType.DUT_SQUARE_INCHES));
            imperialSettings.Add(UnitType.UT_Moment_of_Inertia, new UnitsTypeAssignment(UnitSymbolType.UST_IN_SUP_4, DisplayUnitType.DUT_INCHES_TO_THE_FOURTH_POWER));
            imperialSettings.Add(UnitType.UT_Mass_per_Unit_Length, new UnitsTypeAssignment(UnitSymbolType.UST_LB_MASS_PER_FT, DisplayUnitType.DUT_POUNDS_MASS_PER_FOOT));
            imperialSettings.Add(UnitType.UT_Stress, new UnitsTypeAssignment(UnitSymbolType.UST_PSF, DisplayUnitType.DUT_POUNDS_FORCE_PER_SQUARE_FOOT));
            imperialSettings.Add(UnitType.UT_UnitWeight, new UnitsTypeAssignment(UnitSymbolType.UST_LBF_PER_CU_FT, DisplayUnitType.DUT_POUNDS_FORCE_PER_CUBIC_FOOT));
            imperialSettings.Add(UnitType.UT_Structural_Velocity, new UnitsTypeAssignment(UnitSymbolType.UST_FPS, DisplayUnitType.DUT_FEET_PER_SECOND));

            metricSettings = new Dictionary<UnitType, UnitsTypeAssignment>();
            metricSettings.Add(UnitType.UT_Length, new UnitsTypeAssignment(UnitSymbolType.UST_M, DisplayUnitType.DUT_METERS));
            metricSettings.Add(UnitType.UT_Section_Property, new UnitsTypeAssignment(UnitSymbolType.UST_M, DisplayUnitType.DUT_METERS));
            metricSettings.Add(UnitType.UT_Section_Dimension, new UnitsTypeAssignment(UnitSymbolType.UST_M, DisplayUnitType.DUT_METERS));
            metricSettings.Add(UnitType.UT_Section_Area, new UnitsTypeAssignment(UnitSymbolType.UST_M_SUP_2, DisplayUnitType.DUT_SQUARE_METERS));
            metricSettings.Add(UnitType.UT_Moment_of_Inertia, new UnitsTypeAssignment(UnitSymbolType.UST_M_SUP_4, DisplayUnitType.DUT_METERS_TO_THE_FOURTH_POWER));
            metricSettings.Add(UnitType.UT_Mass_per_Unit_Length, new UnitsTypeAssignment(UnitSymbolType.UST_KGM_PER_M, DisplayUnitType.DUT_KILOGRAMS_MASS_PER_METER));
            metricSettings.Add(UnitType.UT_Stress, new UnitsTypeAssignment(UnitSymbolType.UST_N_PER_M_SUP_2, DisplayUnitType.DUT_NEWTONS_PER_SQUARE_METER));
            metricSettings.Add(UnitType.UT_UnitWeight, new UnitsTypeAssignment(UnitSymbolType.UST_KN_PER_M_SUP_3, DisplayUnitType.DUT_KILONEWTONS_PER_CUBIC_METER));
            metricSettings.Add(UnitType.UT_Structural_Velocity, new UnitsTypeAssignment(UnitSymbolType.UST_M_PER_S, DisplayUnitType.DUT_METERS_PER_SECOND));

            RevitUnits = null;
        }

        public UnitsAssignment(string valueName, UnitType unitType)
        {
            ValueName = valueName;
            this.unitType = unitType;
        }

        static public Units RevitUnits { set; private get; }

        static public string GetUnitSymbol(string valueName, UnitsAssignment[] Assignments, Autodesk.Revit.DB.DisplayUnit unitSystem)
        {
            foreach (UnitsAssignment ua in Assignments)
            {
                if (ua.ValueName.CompareTo(valueName) == 0)
                {
                    return ua.GetUnitSymbol(unitSystem);
                }
            }
            return "";
        }

        static public string GetUnitSymbol(string valueName, UnitsAssignment[] Assignments)
        {
            foreach (UnitsAssignment ua in Assignments)
            {
                if (ua.ValueName.CompareTo(valueName) == 0)
                {
                    FormatOptions fo = RevitUnits.GetFormatOptions(ua.unitType);
                    if (UnitSymbolType.UST_NONE == fo.UnitSymbol)
                        return "";
                    return LabelUtils.GetLabelFor(fo.UnitSymbol);
                }
            }
            return "";
        }

        static public double ConvertToRevitUI(string valueName, double value, UnitsAssignment[] Assignments, DisplayUnit unitSystem)
        {
            foreach (UnitsAssignment ua in Assignments)
            {
                if (ua.ValueName.CompareTo(valueName) == 0)
                {
                    UnitsTypeAssignment currentAssignment;
                    switch (unitSystem)
                    {
                        case DisplayUnit.IMPERIAL:
                            if (!imperialSettings.TryGetValue(ua.unitType, out currentAssignment))
                                return value;
                            break;
                        case DisplayUnit.METRIC:
                            if (!metricSettings.TryGetValue(ua.unitType, out currentAssignment))
                                return value;
                            break;
                        default:
                            return value;
                    }
                    value = UnitUtils.ConvertFromInternalUnits(value, currentAssignment.DisplayType);
                    break;
                }
            }
            return value;
        }

        static public string FormatToRevitUI(string valueName, double value, UnitsAssignment[] Assignments, bool appendUnitSymbol = false)
        {
            string formatedValue = value.ToString();

            if (null == RevitUnits)
                return formatedValue;

            foreach (UnitsAssignment ua in Assignments)
            {
                if (ua.ValueName.CompareTo(valueName) == 0)
                {
                    FormatOptions fo = RevitUnits.GetFormatOptions(ua.unitType);

                    FormatValueOptions formatValueOptions = new FormatValueOptions();
                    formatValueOptions.SetFormatOptions(fo);
                    formatValueOptions.AppendUnitSymbol = appendUnitSymbol;

                    formatedValue = UnitFormatUtils.Format(RevitUnits, ua.unitType, value, false, false, formatValueOptions);

                    string unitSymbol = GetUnitSymbol(valueName, Assignments);
                    if (unitSymbol.Length > 0)
                        formatedValue = Regex.Replace(formatedValue, unitSymbol, "");
                    break;
                }
            }

            return formatedValue;
        }

        public string ValueName { get; private set; }

        private string GetUnitSymbol(DisplayUnit unitSystem)
        {
            UnitsTypeAssignment assignment;
            switch (unitSystem)
            {
                case DisplayUnit.IMPERIAL:
                    if (!imperialSettings.TryGetValue(unitType, out assignment))
                        return "";
                    break;
                case DisplayUnit.METRIC:
                    if (!metricSettings.TryGetValue(unitType, out assignment))
                        return "";
                    break;
                default:
                    return "";
            }

            if (UnitSymbolType.UST_NONE == assignment.SymbolType)
                return "";
            return LabelUtils.GetLabelFor(assignment.SymbolType);
        }

        private UnitType unitType;

        static private Dictionary<UnitType, UnitsTypeAssignment> imperialSettings;
        static private Dictionary<UnitType, UnitsTypeAssignment> metricSettings;
    }

    internal class ElementInfoUnits
    {
        public static UnitsAssignment[] Assignments = { 
                                                        new UnitsAssignment("GeomLength", UnitType.UT_Length),
                                                        new UnitsAssignment("LayerThickness", UnitType.UT_Length),
                                                        new UnitsAssignment("LayerOffset", UnitType.UT_Length),
                                                        new UnitsAssignment("SlabThickness", UnitType.UT_Length),
                                                        new UnitsAssignment("SlabDimension", UnitType.UT_Length),
                                                        new UnitsAssignment("WallThickness", UnitType.UT_Length),
                                                        new UnitsAssignment("WallDimension", UnitType.UT_Length)
                                                      };
    }

    internal class SectionDimensionsUnits
    {
        public static UnitsAssignment[] Assignments = { 
                                                        new UnitsAssignment("h", UnitType.UT_Section_Dimension),
                                                        new UnitsAssignment("hw", UnitType.UT_Section_Dimension),
                                                        new UnitsAssignment("tw", UnitType.UT_Section_Dimension),
                                                        new UnitsAssignment("t", UnitType.UT_Section_Dimension),
                                                        new UnitsAssignment("hw1", UnitType.UT_Section_Dimension),
                                                        new UnitsAssignment("tw1", UnitType.UT_Section_Dimension),
                                                        new UnitsAssignment("hw2", UnitType.UT_Section_Dimension),
                                                        new UnitsAssignment("tw2", UnitType.UT_Section_Dimension),
                                                        new UnitsAssignment("h1", UnitType.UT_Section_Dimension),
                                                        new UnitsAssignment("h2", UnitType.UT_Section_Dimension),
                                                        new UnitsAssignment("b", UnitType.UT_Section_Dimension),
                                                        new UnitsAssignment("tf", UnitType.UT_Section_Dimension),
                                                        new UnitsAssignment("bf", UnitType.UT_Section_Dimension),
                                                        new UnitsAssignment("bf1", UnitType.UT_Section_Dimension),
                                                        new UnitsAssignment("tf1", UnitType.UT_Section_Dimension),
                                                        new UnitsAssignment("bf2", UnitType.UT_Section_Dimension),
                                                        new UnitsAssignment("tf2", UnitType.UT_Section_Dimension),
                                                        new UnitsAssignment("l1", UnitType.UT_Section_Dimension),
                                                        new UnitsAssignment("l2", UnitType.UT_Section_Dimension),
                                                        new UnitsAssignment("s", UnitType.UT_Section_Dimension),
                                                        new UnitsAssignment("vy", UnitType.UT_Section_Dimension),
                                                        new UnitsAssignment("vpy", UnitType.UT_Section_Dimension),
                                                        new UnitsAssignment("vz", UnitType.UT_Section_Dimension),
                                                        new UnitsAssignment("vpz", UnitType.UT_Section_Dimension)
                                                      };
    }

    internal class SectionCharacteristicsUnits
    {
        public static UnitsAssignment[] Assignments = { 
                                                        new UnitsAssignment("A", UnitType.UT_Section_Area),
                                                        new UnitsAssignment("Iy", UnitType.UT_Moment_of_Inertia),
                                                        new UnitsAssignment("Iz", UnitType.UT_Moment_of_Inertia),
                                                        new UnitsAssignment("ry", UnitType.UT_Section_Property),
                                                        new UnitsAssignment("rz", UnitType.UT_Section_Property),
                                                        new UnitsAssignment("mass", UnitType.UT_Mass_per_Unit_Length)
                                                      };
    }

    internal class MaterialCharacteristicsUnits
    {
        public static UnitsAssignment[] Assignments = { 
                                                        new UnitsAssignment("YoungModulus", UnitType.UT_Stress),
                                                        new UnitsAssignment("PoissonRatio", UnitType.UT_Number),
                                                        new UnitsAssignment("ThermalExpansionCoefficient", UnitType.UT_ThermalExpansion),
                                                        new UnitsAssignment("UnitWeight", UnitType.UT_UnitWeight),
                                                        new UnitsAssignment("DampingRatio", UnitType.UT_Number),
                                                        new UnitsAssignment("ShearModulusG", UnitType.UT_Stress) 
                                                      };
    }

    internal class MaterialConcreteCharacteristicsUnits
    {
        public static UnitsAssignment[] Assignments = { 
                                                        new UnitsAssignment("Compression", UnitType.UT_Stress),
                                                        new UnitsAssignment("BendingReinforcement", UnitType.UT_Stress),
                                                        new UnitsAssignment("ShearReinforcement", UnitType.UT_Stress),
                                                        new UnitsAssignment("ShearStrengthReduction", UnitType.UT_Number)
                                                      };
    }

    internal class MaterialMetalCharacteristicsUnits
    {
        public static UnitsAssignment[] Assignments = { 
                                                        new UnitsAssignment("MinimumYieldStress", UnitType.UT_Stress),
                                                        new UnitsAssignment("MinimumTensileStrength", UnitType.UT_Stress),
                                                        new UnitsAssignment("ReductionShearFactor", UnitType.UT_Number)
                                                      };
    }

    internal class MaterialTimberCharacteristicsUnits
    {
        public static UnitsAssignment[] Assignments = { 
                                                        new UnitsAssignment("ParallelCompressionStrength", UnitType.UT_Stress),
                                                        new UnitsAssignment("ParallelShearStrength", UnitType.UT_Stress), 
                                                        new UnitsAssignment("AverageModulusG", UnitType.UT_Stress),
                                                        new UnitsAssignment("BurningVelocity", UnitType.UT_Structural_Velocity),
                                                        new UnitsAssignment("BendingStrength", UnitType.UT_Stress),
                                                        new UnitsAssignment("PerpendicularShearStrength", UnitType.UT_Stress),
                                                        new UnitsAssignment("PerpendicularCompressionStrength", UnitType.UT_Stress) 
                                                      };
    }
}
