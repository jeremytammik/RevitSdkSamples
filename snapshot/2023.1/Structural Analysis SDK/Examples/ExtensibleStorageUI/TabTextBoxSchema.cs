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
    [Categories(
        new string[] { "DefaultTextBox", "XYZTextBox", "UnitTextBox" },
        new int[]{1,2,3},
        Localizable =  true
        )]
    [Schema("tabTextBoxSchema", "632f1c20-2c8c-4fb2-87d7-fdbc1dcb3b79")]
    public class TabTextBoxSchema : SchemaClass
    {
        # region Constructors

        public TabTextBoxSchema()
        {
            TextBoxInteger32 = 2;
            TextBoxInteger16 = 2;
            TextBoxGuid = new Guid("632f1c20-2c8c-4fb2-87d7-fdbc1dcb3b79");
            TextBoxBoolean = true; 
            TextBoxString = "A string for TextBoxString control";
            TextBoxUV = new UV(5, 5);
            TextBoxXYZ = new XYZ(5, 5, 5);
            TextBoxDouble = 25;
            TextBoxDoubleCalculated = TextBoxInteger32 * TextBoxDouble;
            UnitTextBoxDouble = 10.0;
            UnitTextBoxDoubleMinValueSet = 15;
            UnitTextBoxDoubleMaxValueSet = 100;
            UnitTextBoxDoubleIsNotEnabled = 5 * 10;
            XYZTextBox = new XYZ(5, 5, 5);
        }

        public TabTextBoxSchema(Document document)
        {
        }

        public TabTextBoxSchema(Entity entity, Document document): base(entity, document)
        {
        }

        #endregion Constructors

        # region UnitTextBox

        /// <summary>
        /// Unit TextBox supporting a double value.
        /// The unit type is set to  length (DUT_METERS). 
        /// This value is stored in DUT_METERS.
        /// this value is initialized via the constructor to 10.0 DUT_METERS.
        /// </summary>
        [SchemaProperty(Unit = UnitType.UT_Length, DisplayUnit = DisplayUnitType.DUT_METERS)]
        [UnitTextBox(
            Category = "UnitTextBox",
            AttributeUnit = DisplayUnitType.DUT_METERS,
            IsVisible = true,
            IsEnabled = true,
            Index = 0,
            Localizable = true,
            Description = "UnitTextBoxDouble",
            Tooltip = "UnitTextBoxDoubleToolTips"
            )]
        public Double UnitTextBoxDouble { get; set; }

        /// <summary>
        /// Unit TextBox supporting a double value.
        /// The unit type is set to length (DUT_METERS). 
        /// This value is stored in DUT_METERS.
        /// this value is initialized via the constructor to 5 x10 in DUT_FRACTIONAL_INCHES.
        /// This control is disabled on the UI. 
        /// </summary>
        [SchemaProperty(Unit = UnitType.UT_Length, DisplayUnit = DisplayUnitType.DUT_METERS)]
        [UnitTextBox(
            ValidateMinimumValue = false,
            ValidateMaximumValue = false,
            AttributeUnit = DisplayUnitType.DUT_FRACTIONAL_INCHES,
            Category = "UnitTextBox",
            IsVisible = true,
            IsEnabled = false,
            Index = 1,
            Localizable = true,
            Description = "UnitTextBoxDoubleIsNotEnabled",
            Tooltip = "UnitTextBoxDoubleIsNotEnabledToolTips"
            )]
        public Double UnitTextBoxDoubleIsNotEnabled { get; set; }

        /// <summary>
        /// Unit TextBox supporting a double value.
        /// The unit type is set to  length (DUT_METERS). 
        /// This value is stored in DUT_METERS.
        /// this value is initialized via the constructor to 15.0 DUT_METERS.
        /// The minimal value is set to 10 DUT_METERS. 
        /// </summary>
        [SchemaProperty(Unit = UnitType.UT_Length, DisplayUnit = DisplayUnitType.DUT_METERS)]
        [UnitTextBox(
            ValidateMinimumValue = true,
            AttributeUnit = DisplayUnitType.DUT_METERS, 
            MinimumValue = 10,
            Category = "UnitTextBox",
            IsVisible = true,
            IsEnabled = true,
            Index = 2,
            Localizable = true,
            Description = "UnitTextBoxDoubleMinValueSet",
            Tooltip = "UnitTextBoxDoubleMinValueSetToolTips"
            )]
        public Double UnitTextBoxDoubleMinValueSet { get; set; }

        /// <summary>
        /// Unit TextBox supporting a double value.
        /// The unit type is set to  length (DUT_DECIMAL_FEET). 
        /// This value is stored in DUT_DECIMAL_FEET.
        /// This value is initialized via the constructor to 100.0 DUT_DECIMAL_FEET.
        /// The maximal value is set to 100 DUT_METERS. 
        /// </summary>
        [SchemaProperty(Unit = UnitType.UT_Length, DisplayUnit = DisplayUnitType.DUT_DECIMAL_FEET)]
        [UnitTextBox(
            ValidateMaximumValue = true,
            MaximumValue = 100,
            AttributeUnit = DisplayUnitType.DUT_METERS,
            Category = "UnitTextBox",
            IsVisible = true,
            IsEnabled = true,
            Index = 3,
            Localizable = true,
            Description = "UnitTextBoxDoubleMaxValueSet",
            Tooltip = "UnitTextBoxDoubleMaxValueSetToolTips"
            )]
        public Double UnitTextBoxDoubleMaxValueSet { get; set; }
        # endregion UnitTextBox

        # region OtherTextBox
        /// <summary>
        /// TextBox supporting a XYZ value.
        /// The unit type is set to  length (DUT_METERS). 
        /// This value is stored in DUT_METERS.
        /// this value is initialized via the constructor to (5, 5, 5) DUT_METERS.
        /// On the UI semi colomns as X;Y;Z separator 
        /// </summary>
        [SchemaProperty(Unit = UnitType.UT_Length, DisplayUnit = DisplayUnitType.DUT_METERS)]
        [XYZTextBox(
            Category = "XYZTextBox",
            IsVisible = true,
            IsEnabled = true,
            Index = 4,
            Localizable = true,
            Description = "XYZTextBox",
            Tooltip = "XYZTextBoxToolTips"

            
            )]
        public XYZ XYZTextBox { get; set; }



        # endregion OtherTextBox

        # region DefaultTextBox
        /// <summary>
        /// TextBox supporting a string value.
        /// This value is initialized via the constructor to "A string for TextBoxString control".
        /// </summary>
        [SchemaProperty(FieldName = "")]
        [TextBox(
            Category = "DefaultTextBox",
            IsVisible = true,
            IsEnabled = true,
            Index = 1,
            Localizable = true,
            Description = "TextBoxString",
            Tooltip = "TextBoxStringToolTips"
            )]
        public String TextBoxString { get; set; }


        /// <summary>
        /// TextBox supporting an integer value (int16).
        /// This value is initialized via the constructor to 2.
        /// </summary>
        [SchemaProperty(FieldName = "")]
        [TextBox(
            Category = "DefaultTextBox",
            IsVisible = true,
            IsEnabled = true,
            Index = 2,
            Localizable = true,
            Description = "TextBoxInteger16",
            Tooltip = "TextBoxInteger16ToolTips"
            )]
        public Int16 TextBoxInteger16 { get; set; }

        /// <summary>
        /// TextBox supporting an integer value (int32).
        /// This value is initialized via the constructor to 2.
        /// </summary>
        [SchemaProperty(FieldName = "")]
        [TextBox(
            Category = "DefaultTextBox",
            IsVisible = true,
            IsEnabled = true,
            Index = 3,
            Localizable = true,
            Description = "TextBoxInteger32",
            Tooltip = "TextBoxInteger32ToolTips"
            )]
        public Int32 TextBoxInteger32 { get; set; }


        /// <summary>
        /// TextBox supporting a guid.
        /// This value is initialized via the constructor to {00000000-0000-0000-0000-000000000000}.
        /// </summary>
        [SchemaProperty(FieldName = "")]
        [TextBox(
            Category = "DefaultTextBox",
            IsVisible = true,
            IsEnabled = true,
            Index = 4,
            Localizable = true,
            Description = "TextBoxGuid",
            Tooltip = "TextBoxGuidToolTips",
            FieldFormat = typeof(ValueFormatGuid) 
            )]
        public Guid TextBoxGuid { get; set; }

        /// <summary>
        /// TextBox supporting a boolean.
        /// This value is initialized via the constructor to true.
        /// </summary>
        [SchemaProperty(FieldName = "")]
        [TextBox(
            Category = "DefaultTextBox",
            IsVisible = true,
            IsEnabled = true,
            Index = 5,
            Localizable = true,
            Description = "TextBoxBoolean",
            Tooltip = "TextBoxBooleanToolTips"
            )]
        public bool TextBoxBoolean { get; set; }

        /// <summary>
        /// TextBox supporting a double value.
        /// The unit type is set to  length (DUT_METERS). 
        /// This value is stored in DUT_METERS.
        /// This value is initialized via the constructor to 25.0 DUT_METERS.
        /// This value will be exposed on the UI based on Revit project units settings.  
        /// Revit project unit formatting won't be appled on the UI (use UnitTextBox to achieve this).     
        /// </summary>

        [SchemaProperty(Unit = UnitType.UT_Length, DisplayUnit = DisplayUnitType.DUT_METERS, FieldName = "")]
        [TextBox(
            Category = "DefaultTextBox",
            IsVisible = true,
            IsEnabled = true,
            Index = 6,
            Localizable = true,
            Description = "TextBoxDouble",
            Tooltip = "TextBoxDoubleToolTips"
            )]
        public Double TextBoxDouble { get; set; }

        /// <summary>
        /// TextBox supporting a double value.
        /// The unit type is set to  length (DUT_METERS). 
        /// This value is stored in DUT_METERS.
        /// This value is initialized and calculated via the constructor in DUT_METERS.
        /// This control is disabled on the UI.
        /// </summary>
        [SchemaProperty(Unit = UnitType.UT_Length, DisplayUnit = DisplayUnitType.DUT_METERS, FieldName = "")]
        [TextBox(
            Category = "DefaultTextBox",
            IsVisible = true,
            IsEnabled = false,
            Index = 7,
            Localizable = true,
            Description = "TextBoxDoubleCalculated",
            Tooltip = "TextBoxDoubleCalculatedToolTips"
       
            )]
        public Double TextBoxDoubleCalculated { get; set; }

        /// <summary>
        /// TextBox supporting an ElementId.
        /// This value is initialized to the active element. 
        /// </summary>
        [SchemaProperty()]
        [TextBox(
            Category = "DefaultTextBox", 
            IsVisible = true,
            IsEnabled = true,
            Index = 8,
            Localizable = true,
            Description = "TextBoxElementId",
            Tooltip = "TextBoxElementIdToolTips",
            FieldFormat = typeof(ValueFormatElementId)  
            )]

        public ElementId TextBoxElementId { get; set; }


        /// <summary>
        /// TextBox supporting a UV value.
        /// The unit type is set to  length (DUT_METERS). 
        /// This value is stored in DUT_METERS.
        /// this value is initialized via the constructor to (5, 5) DUT_METERS.
        /// On the UI the format  is (U,V)
        /// Revit project unit formatting won't be appled on the UI (use UnitTextBox to achieve this).     
        /// </summary>
        [SchemaProperty(Unit = UnitType.UT_Length, DisplayUnit = DisplayUnitType.DUT_METERS)]
        [TextBox(
            Category = "DefaultTextBox",
            IsVisible = true,
            IsEnabled = true,
            Index = 9, 
            Localizable = true,
            Description = "TextBoxUV",
            Tooltip = "TextBoxUVToolTips",
            FieldFormat = typeof(ValueFormatUV) 
            )]
         public UV TextBoxUV { get; set; }

        /// <summary>
        /// TextBox supporting a XYZ value.
        /// The unit type is set to  length (DUT_METERS). 
        /// This value is stored in DUT_METERS.
        /// this value is initialized via the constructor to (5, 5, 5) DUT_METERS.
        /// On the UI the format  is (X,Y,Z)
        /// Revit project unit formatting won't be appled on the UI (use XYZTextBox to achieve this).     
        /// </summary>
        [SchemaProperty(Unit = UnitType.UT_Length, DisplayUnit = DisplayUnitType.DUT_METERS)]
        [TextBox(
            Category = "DefaultTextBox",
            IsVisible = true,
            IsEnabled = true,
            Index = 10,
            Localizable = true,
            Description = "TextBoxXYZ",
            Tooltip = "TextBoxXYZToolTips",
            FieldFormat = typeof(ValueFormatXYZ) 
            )]
        public XYZ TextBoxXYZ { get; set; }


        # endregion Default TextBox
    }
}