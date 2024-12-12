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
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.DB.ExtensibleStorage.Framework;
using Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes;
using Autodesk.Revit.UI.ExtensibleStorage.Framework.Attributes;

namespace ExtensibleStorageUI
{
    /// <summary>
    /// The usage of this class demonstrate how to use NumericUpDown controls and associated features
    /// </summary>
    [Schema("tabNumericUpDownSchema", "0dc91899-8fe3-4e90-84b1-a48f3b6c8e11")]
    public class TabNumericUpDownSchema : SchemaClass
    {
        # region Constructors

        public TabNumericUpDownSchema()
        {
            NumericUpDownDoubleUnit = 10;
            NumericUpDownDoubleUnitVolume = 2;
            NumericUpDownInt = 15;
            NumericUpDownDouble = 5;
        }

        public TabNumericUpDownSchema(Document document)
        {
        }

        public TabNumericUpDownSchema(Entity entity, Document document)
            : base(entity, document)
        {
        }

        #endregion Constructors

        # region NumericUpDown

        /// <summary>
        /// Unit NumericUpDown supporting a double value.
        /// The unit type is set to  length (DUT_FEET_FRACTIONAL_INCHES). 
        /// This value is stored in DUT_FEET_FRACTIONAL_INCHES.
        /// The step is set to 5 DUT_FEET_FRACTIONAL_INCHES.
        /// The minimal value is set to 0 DUT_FEET_FRACTIONAL_INCHES. 
        /// The maximal value is set to 100 DUT_FEET_FRACTIONAL_INCHES.
        /// </summary>
        [SchemaProperty(Unit = UnitType.UT_Length, DisplayUnit = DisplayUnitType.DUT_FEET_FRACTIONAL_INCHES)]
        [UnitNumericUpDown(
            Description = "NumericUpDownDoubleUnit",
            Category = "NumericUpDown",
            AttributeUnit = DisplayUnitType.DUT_FEET_FRACTIONAL_INCHES,
            IsVisible = true,
            IsEnabled = true,
            Index = 0,
            Localizable = true,
            Step = 5,
            MinimumValue = 0,
            MaximumValue = 100,
            Tooltip = "NumericUpDownDoubleUnitToolTips"
            )]
        public Double NumericUpDownDoubleUnit { get; set; }

        /// <summary>
        /// Unit NumericUpDown supporting a double value.
        /// The unit type is set to volume (DUT_CUBIC_METERS)
        /// This value is stored in DUT_CUBIC_METERS
        /// Validation value and step are set in DUT_CUBIC_FEET.
        /// The step is set to 1 DUT_CUBIC_FEET.
        /// The minimal value is set to 0 DUT_CUBIC_FEET. 
        /// The maximal value is set to 100 DUT_CUBIC_FEET.
        /// Default value is set to 2m³ and 1m³ = 35.3146670ft³
        /// </summary>
        [SchemaProperty(Unit = UnitType.UT_Volume, DisplayUnit = DisplayUnitType.DUT_CUBIC_METERS  )]
        [UnitNumericUpDown(
            Description = "NumericUpDownDoubleUnitVolume",
            AttributeUnit = DisplayUnitType.DUT_CUBIC_FEET,
            Category = "NumericUpDown",
            IsVisible = true,
            IsEnabled = true,
            Index = 1,
            Localizable = true,
            Step = 1,
            MinimumValue = 0,
            MaximumValue = 100,
            Tooltip = "NumericUpDownDoubleUnitVolumeToolTips"
            )]
        public Double NumericUpDownDoubleUnitVolume { get; set; }

        /// <summary>
        /// NumericUpDown supporting a double value.
        /// The unit type is set to  length (DUT_METERS). 
        /// This value is stored in DUT_METERS.
        /// No Unit are displayed on the UI
        /// The step is set to 5 DUT_METERS.
        /// The minimal value is set to -100 DUT_METERS. 
        /// The maximal value is set to 100 DUT_METERS.
        /// </summary>
        [SchemaProperty(Unit = UnitType.UT_Length, DisplayUnit = DisplayUnitType.DUT_METERS)]
        [UnitNumericUpDown(
            Description = "NumericUpDownDouble",
            Category = "NumericUpDown",
            AttributeUnit = DisplayUnitType.DUT_METERS,
            IsVisible = true,
            IsEnabled = true,
            Index = 2,
            Localizable = true,
            Step = 5,
            MinimumValue = -100,
            MaximumValue = 100,
            Tooltip = "NumericUpDownDoubleToolTips"
            )]
        public Double NumericUpDownDouble { get; set; }

        /// <summary>
        /// NumericUpDown supporting an integer value.
        /// No Unit are displayed on the UI.
        /// The step is set to 1.
        /// The minimal value is set to 10. 
        /// The maximal value is set to 20.
        /// </summary>
        [SchemaProperty()]
        [IntNumericUpDown(
            Description = "NumericUpDownInt",
            Category = "NumericUpDown",
            IsVisible = true,
            IsEnabled = true,
            Index = 3,
            Localizable = true,
            Step = 1,
            MinimumValue = 10,
            MaximumValue = 20,
            Tooltip = "NumericUpDownIntToolTips"
            )]
        public Int32 NumericUpDownInt { get; set; }

        # endregion NumericUpDown
    }
}