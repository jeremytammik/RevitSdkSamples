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
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.DB.ExtensibleStorage.Framework;
using Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes;
using Autodesk.Revit.UI.ExtensibleStorage.Framework.Attributes;

namespace ExtensibleStorageUI
{
    /// <summary>
    /// 
    /// </summary>
    [Schema("tabListTextBoxSchema", "ecf25bef-5ce6-422c-9b7f-630b9f8ced85")]
    public class TabListTextBoxSchema : SchemaClass
    {
        # region list
        /// <summary>
        /// Unit TextBox supporting a list of double value
        /// The separator is ";" character.
        /// The unit type is set to  length (DUT_METERS). 
        /// These values are stored in DUT_METERS.
        /// These values are initialized via the constructor to {10.0, 20.0} DUT_METERS.
        /// </summary>
        [SchemaProperty(Unit = UnitType.UT_Length, DisplayUnit = DisplayUnitType.DUT_METERS)]
        [ListUnitTextBox(
            Category = "List",
            IsVisible = true,
            IsEnabled = true,
            Index = 0,
            Localizable = true,
            Description = "ListUnitTextBoxDouble",
            Tooltip = "ListUnitTextBoxDoubleToolTips"
            )]
        public List<Double> ListUnitTextBoxDouble { get; set; }

        /// <summary>   /// Unit TextBox supporting a list of double value
        /// The separator is ";" character. 
        /// Number of items is limited to 5; 
        /// The unit type is set to  length (DUT_METERS). 
        /// These values are stored in DUT_METERS.
        /// These values are initialized via the constructor to {10.0, 20.0,30.0,40.0,50.0} DUT_METERS.

             /// </summary>
        [SchemaProperty(Unit = UnitType.UT_Length, DisplayUnit = DisplayUnitType.DUT_METERS)]
        [ListUnitTextBox(
            ValidateMaximumItemCount = true,
            Category = "List",
            IsVisible = true,
            IsEnabled = true,
            Index = 1,
            Localizable = true,
            MaximumItemCount = 5,
            Description = "ListUnitTextBoxMaxItemSet",
            Tooltip = "ListUnitTextBoxMaxItemSetToolTips"
            )]
        public List<Double> ListUnitTextBoxMaxItemSet { get; set; }


        /// <summary>
        /// Unit TextBox supporting a list of double value
        /// The separator is ";" character. 
        /// Number of items should be superior to 2; 
        /// The unit type is set to  length (DUT_METERS). 
        /// These values are stored in DUT_METERS.
        /// These values are initialized via the constructor to {10.0, 20.0} DUT_METERS.
        /// </summary>
        [SchemaProperty(Unit = UnitType.UT_Length, DisplayUnit = DisplayUnitType.DUT_METERS)]
        [ListUnitTextBox(
            ValidateMinimumItemCount = true,
            Category = "List",
            IsVisible = true,
            IsEnabled = true,
            Index = 2,
            Localizable = true,
            MinimumItemCount = 2,
            Description = "ListUnitTextBoxMinItemSet",
            Tooltip = "ListUnitTextBoxMinItemSetSetToolTips"
            )]
        public List<Double> ListUnitTextBoxMinItemSet { get; set; }

        /// <summary>
        /// Unit TextBox supporting a list of double value
        /// The separator is ";" character. 
        /// The unit type is set to  length (DUT_METERS). 
        /// These values are stored in DUT_METERS.
        /// These values should be lower than 100 in DUT_METERS.
        /// These values are initialized via the constructor to {10.0, 20.0} DUT_METERS.
        /// </summary>
        [SchemaProperty(Unit = UnitType.UT_Length, DisplayUnit = DisplayUnitType.DUT_METERS)]
        [ListUnitTextBox(
            ValidateMaximumValue = true,
            Category = "List",
            IsVisible = true,
            IsEnabled = true,
            Index = 3,
            Localizable = true,
            MaximumValue = 100,
            Description = "ListUnitTextBoxMaxValueSet",
            Tooltip = "ListUnitTextBoxMaxValueSetToolTips"
            )]
        public List<Double> ListUnitTextBoxMaxValueSet { get; set; }

        /// <summary>
        /// Unit TextBox supporting a list of double value
        /// The separator is ";" character. 
        /// The unit type is set to  length (DUT_METERS). 
        /// These values are stored in DUT_METERS.
        /// These values should be upper than 5 in DUT_METERS.
        /// These values are initialized via the constructor to {10.0, 20.0} DUT_METERS.
        /// </summary>
        [SchemaProperty(Unit = UnitType.UT_Length, DisplayUnit = DisplayUnitType.DUT_METERS)]
        [ListUnitTextBox(
            ValidateMinimumValue = true,
            Category = "List",
            IsVisible = true,
            IsEnabled = true,
            Index = 4,
            Localizable = true,
            MinimumValue = 5,
            Description = "ListUnitTextBoxMinValueSet",
            Tooltip = "ListUnitTextBoxMinValueSetToolTips"
            )]
        public List<Double> ListUnitTextBoxMinValueSet { get; set; }

        /// <summary>
        /// TextBox supporting a list of string values
        /// The separator is ";" character. 
        /// These values are initialized via the constructor to {"this is a first text", "this is a second text"}.
        /// </summary>
        [SchemaProperty()]
        [ListTextBox(
            Category = "List",
            IsVisible = true,
            IsEnabled = true,
            Index = 5,
            Localizable = true,
            Description = "ListTextBoxString",
            Tooltip = "ListTextBoxStringToolTips"
            )]
        public List<String> ListTextBoxString { get; set; }

        /// <summary>
        /// TextBox supporting a list of int16 values
        /// The separator is ";" character. 
        /// These values are initialized via the constructor to {1,2,3}.
        /// </summary>
        [SchemaProperty()]
        [ListTextBox(
            Category = "List",
            IsVisible = true,
            IsEnabled = true,
            Index = 6,
            Localizable = true,
            Description = "ListTextBoxInt16",
            Tooltip = "ListTextBoxInt16ToolTips"
            )]
        public List<Int16> ListTextBoxInt16 { get; set; }

        /// <summary>
        /// TextBox supporting a list of int32 values
        /// The separator is ";" character. 
        /// These values are initialized via the constructor to {1,2,3}.
        /// </summary>
        [SchemaProperty()]
        [ListTextBox(
            Category = "List",
            IsVisible = true,
            IsEnabled = true,
            Index = 7,
            Localizable = true,
            Description = "ListTextBoxInt32",
            Tooltip = "ListTextBoxInt32ToolTips"
            )]
        public List<Int32> ListTextBoxInt32 { get; set; }

        /// <summary>
        /// TextBox supporting a list of double values
        /// The separator is ";" character. 
        /// These values are initialized via the constructor to {10,20,30}.
        /// </summary>
        [SchemaProperty(Unit = UnitType.UT_Length, DisplayUnit = DisplayUnitType.DUT_METERS)]
        [ListTextBox(
            Category = "List",
            IsVisible = true,
            IsEnabled = true,
            Index = 8,
            Localizable = true,
            Description = "ListTextBoxDouble",
            Tooltip = "ListTextBoxDoubleToolTips"
            )]
        public List<double> ListTextBoxDouble { get; set; }

      

        /// <summary>
        /// TextBox supporting a list of XYZ values
        /// The separator is ";" character. 
        /// These values are initialized via the constructor to {XYZ(1,1,1),XYZ(2,2,2)}.
        /// </summary>
        [SchemaProperty(Unit = UnitType.UT_Length, DisplayUnit = DisplayUnitType.DUT_METERS, FieldName = "")]
        [ListTextBox(
            Category = "List",
            IsVisible = true,
            IsEnabled = true,
            Index = 9,
            Localizable = true,
            Description = "ListTextBoxXYZ",
            Tooltip = "ListTextBoxXYZToolTips",
            FieldFormat = typeof(ValueFormatListXYZ)
            )]
        public List<XYZ> ListTextBoxXYZ { get; set; }

        /// <summary>
        /// TextBox supporting a list of UV values
        /// The separator is ";" character. 
        /// These values are initialized via the constructor to {UV(1,1),UV(2,2)}.
        /// </summary>
        [SchemaProperty(Unit = UnitType.UT_Length, DisplayUnit = DisplayUnitType.DUT_METERS)]
        [ListTextBox(
            Category = "List",
            IsVisible = true,
            IsEnabled = true,
            Index = 10,
            Localizable = true,
            Description = "ListTextBoxUV",
            Tooltip = "ListTextBoxUVToolTips",
            FieldFormat = typeof(ValueFormatListUV)
            )]
        public List<UV> ListTextBoxUV { get; set; }

        /// <summary>
        /// TextBox supporting a list of Guid values
        /// The separator is ";" character. 
        /// These values are initialized via the constructor to {("6AED35BD-9143-4AAB-B568-7FC69C946824"),("F6F9D635-6AF3-4336-9D52-E734DFA9F97E"), ("E72993A5-CDFE-4501-9A34-D3A6DA407CD6")}.
        /// </summary>
        [SchemaProperty(FieldName = "")]
        [ListTextBox(
            Category = "List",
            IsVisible = true,
            IsEnabled = true,
            Index = 11,
            Localizable = true,
            Description = "ListTextBoxGuid",
            Tooltip = "ListTextBoxGuidToolTips",
            FieldFormat = typeof(ValueFormatListGuid)
            )]
        public List<Guid> ListTextBoxGuid { get; set; }


        /// <summary>
        /// TextBox supporting a list of boolean values
        /// The separator is ";" character. 
        /// These values are initialized via the constructor to {true,false}.
        /// </summary>
        [SchemaProperty()]
        [ListTextBox(
            Category = "List",
            IsVisible = true,
            IsEnabled = true,
            Index = 12,
            Localizable = true,
            Description = "ListTextBoxBool",
            Tooltip = "ListTextBoxBoolToolTips"
            )]
        public List<bool> ListTextBoxBool { get; set; }


        /// <summary>
        /// TextBox supporting a list of ElementId values
        /// The separator is ";" character. 
        /// These values are initialized via the constructor with the first 5 rebar type Elementd from project.
        /// </summary>
        [SchemaProperty()]
        [ListTextBox(
            Category = "List",
            IsVisible = true,
            IsEnabled = true,
            Index = 13,
            Localizable = true,
            Description = "ListTextBoxElementId",
            Tooltip = "ListTextBoxDoubleElementIdToolTips",
            FieldFormat = typeof(ValueFormatListElementId)
            )]
        public List<ElementId> ListTextBoxElementId { get; set; }

        # endregion list

        # region Constructors
        /// <summary>
        /// 
        /// </summary>
        public TabListTextBoxSchema()
        {
         

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="document"></param>
        public TabListTextBoxSchema(Document document)
        {
            ListUnitTextBoxDouble = new List<Double> { 10.0, 20.0 };
            ListUnitTextBoxMaxItemSet = new List<Double> { 10.0, 20.0, 30.0, 40.0, 50.0 };
            ListUnitTextBoxMinItemSet = new List<Double> { 10.0, 20.0 };
            ListUnitTextBoxMaxValueSet = new List<Double> { 10.0, 20.0 };
            ListUnitTextBoxMinValueSet = new List<Double> { 10.0, 20.0 };

            ListTextBoxString = new List<String> { "this is a first text", "this is a second text" };
            ListTextBoxBool = new List<bool> { true, false };
            ListTextBoxDouble = new List<Double> { 10.0, 20.0 };
            ListTextBoxInt16 = new List<short> { 1, 2, 3 };
            ListTextBoxInt32 = new List<int> { 1, 2, 3 };
            ListTextBoxUV = new List<UV> { new UV(1, 1), new UV(2, 2) };
            ListTextBoxXYZ = new List<XYZ> { new XYZ(1, 1, 1), new XYZ(2, 2, 2) };
            ListTextBoxGuid = new List<Guid> { new Guid("6AED35BD-9143-4AAB-B568-7FC69C946824"), new Guid("F6F9D635-6AF3-4336-9D52-E734DFA9F97E"), new Guid("E72993A5-CDFE-4501-9A34-D3A6DA407CD6") };

            ListTextBoxElementId = (new FilteredElementCollector(document).OfClass(typeof(Autodesk.Revit.DB.Structure.RebarBarType)).ToElementIds() as List<ElementId>).GetRange(0,5)   ;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="document"></param>
        public TabListTextBoxSchema(Entity entity, Document document)
            : base(entity, document)
        {

        }
        #endregion Constructors
    }
}