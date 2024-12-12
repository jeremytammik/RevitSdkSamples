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
    /// this class explains how to define a schema and associated UIField related to ComboBox.
    /// ComboBox Items could be defined directly, based on data sources or on an enum. 
    /// </summary>
    [Schema("tabComboBoxSchema", "8c1bdb2d-75bf-4d56-bfd5-fbafd30d18d0")]
    public class TabComboBoxSchema : SchemaClass
    {
        # region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public TabComboBoxSchema()
        {
            this.ComboBoxString = "Choice 1";
            this.ComboBoxBool = true;
            this.ComboBoxInt16  = 1;
            this.ComboBoxInt32  = 2;
            this.ComboBoxGuid = new Guid("E72993A5-CDFE-4501-9A34-D3A6DA407CD6");
            this.ComboBoxDouble = 10.0;
            this.ComboBoxEnumImage = EnumLocalized.Choice1;
            this.ComboBoxEnumImageText = EnumLocalized.Choice2;
            this.ComboBoxEnumText = EnumLocalized.Choice3;
            this.ComboBoxEnumTextNotLocalized = EnumNotLocalized.Item1;
            this.ComboBoxEnumImageTextNotLocalized = EnumNotLocalized.Item3;
            this.UnitComboBoxDouble = 20;
            // Value will be set on layoutInitialized when 
            this.UnitComboBoxDoubleConstructor = 0;
            this.ComboBoxRebar = null;
            this.ComboBoxElementId = null;
            this.ComboBoxUV = null; ;
            this.ComboBoxXYZ = null; 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="document"></param>
        public TabComboBoxSchema(Document document)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="document"></param>
        public TabComboBoxSchema(Entity entity, Document document)
            : base(entity, document)
        {
        
        }

        #endregion Constructors

        # region UnitComboBox

        /// <summary>
        /// Unit ComboBox supporting double values.
        /// The unit type is set to length (DUT_METERS). 
        /// Selected index value is stored in DUT_METERS.
        /// This comboBox is filled using "DataSourceDoubleList" data source.
        /// "DataSourceDoubleList" is based on doubleList values {10.0, 20.0, 30.0, 40.0}.
        /// </summary> 
        [SchemaProperty(Unit = UnitType.UT_Length, DisplayUnit = DisplayUnitType.DUT_METERS )]
        [UnitComboBox(
            DataSourceKey = "DataSourceDoubleList",
            Category = "UnitComboBox",
            Description = "UnitComboBoxDouble",
            IsVisible = true,
            IsEnabled = true,
            Index = 0,
            Localizable = true,
            Tooltip = "UnitComboBoxDoubleToolTips"
            )]
        public Double UnitComboBoxDouble { get; set; }

        /// <summary>
        /// Unit ComboBox supporting double values.
        /// The unit type is set to length (DUT_METERS). 
        /// Selected index value is stored in DUT_METERS.
        /// This comboBox is filled inline using {5,10,15,20)  
        /// </summary>
        [SchemaProperty(Unit = UnitType.UT_Length, DisplayUnit = DisplayUnitType.DUT_METERS)]
        [UnitComboBox(
            5.0, 10.0, 15.0, 20.0,
            Category = "UnitComboBox",
            Description = "UnitComboBoxDoubleConstructor",
            IsVisible = true,
            IsEnabled = true,
            Index = 1,
            Localizable = true,
            Tooltip = "UnitComboBoxDoubleConstructorToolTips"
            )]
        public Double UnitComboBoxDoubleConstructor { get; set; }

        # endregion UnitComboBox

        # region EnumComboBox
        /// <summary>
        /// EnumControl supporting  strings values presented as ComboBox.
        /// This comboBox is filled using AnEnumLocalized.
        /// Strings associated to [Choice1,Choice2,Choice3] are part of the resources file.
        /// On the UI are visible strings "This is my choice 1","This is my choice 2","This is my choice 3"
        /// Enum field is stored as integer with default enumerator values.
        /// </summary>
        [SchemaProperty]
        [EnumControl(
            Description = "ComboBoxEnumText",
            Presentation = PresentationMode.Combobox,
            Item = PresentationItem.Text,
            Category = "EnumComboBox",
            IsVisible = true,
            IsEnabled = true,
            Index = 0,
            Localizable = true,
            Tooltip = "ComboBoxEnumTextToolTips"
            )]
        public EnumLocalized ComboBoxEnumText { get; set; }

        /// <summary>
        /// EnumControl supporting  strings values presented as ComboBox.
        /// This comboBox is filled using AnEnumNotLocalized.
        /// Strings associated to [Item1,Item2,Item3] are not part of the resources file.
        /// On the UI are visible strings "Item1","Item2","Item3".
        /// Enum field is stored as integer with defined enumerator values.
        /// </summary>
        [SchemaProperty]
        [EnumControl(
            Description = "ComboBoxEnumTextNotLocalized",
            Presentation = PresentationMode.Combobox,
            Item = PresentationItem.Text,
            Category = "EnumComboBox",
            IsVisible = true,
            IsEnabled = true,
            Index = 3,
            Localizable = true,
            Tooltip = "ComboBoxEnumTextNotLocalizedToolTips"
            )]
        public EnumNotLocalized ComboBoxEnumTextNotLocalized { get; set; }

        /// <summary>
        /// EnumControl supporting  strings values and images presented as ComboBox.
        /// This comboBox is filled using AnEnumLocalized.
        /// Strings associated to AnEnumLocalized.Choice1, AnEnumLocalized.Choice2, AnEnumLocalized.Choice3 are part of the resources file.
        /// On the UI are visible strings "This is my choice 1","This is my choice 2","This is my choice 3"
        /// Enum field is stored as integer with default enumerator values.
        /// Uri for associated images are returned by the GetResourceImage(string key) function.
        /// </summary>
        [SchemaProperty]
        [EnumControl(
            Description = "ComboBoxEnumImageText",
            Presentation = PresentationMode.Combobox,
            Item = PresentationItem.ImageWithText,
            Category = "EnumComboBox",
            IsVisible = true,
            IsEnabled = true,
            Index = 4,
            Localizable = true,
            Tooltip = "ComboBoxEnumImageTextToolTips"
            )]
        public EnumLocalized ComboBoxEnumImageText { get; set; }

        /// <summary>
        /// EnumControl supporting  images and text presented as OptionList(List).
        /// Uri for associated images are returned by the GetResourceImage(string key) function.
        /// Text are coming from the not transalted enum
        /// </summary>
        [SchemaProperty]
        [EnumControl(
            Presentation = PresentationMode.Combobox,
            Item = PresentationItem.ImageWithText,
            Category = "EnumComboBox",
            IsVisible = true,
            IsEnabled = true,
            Index = 2,
            Localizable = true,
            Description = "ComboBoxEnumImageTextNotLocalized",
            Tooltip = "ComboBoxEnumImageTextNotLocalizedTooltips"
            )]
        public EnumNotLocalized ComboBoxEnumImageTextNotLocalized { get; set; }

        /// <summary>
        /// EnumControl supporting  images presented as ComboBox.
        /// This comboBox is filled using AnEnumLocalized.
        /// Uri for associated images are returned by the GetResourceImage(string key) function.
        /// </summary>
        [SchemaProperty]
        [EnumControl(
            Description = "ComboBoxEnumImage",
            Presentation = PresentationMode.Combobox,
            Item = PresentationItem.Image,
            Category = "EnumComboBox",
            IsVisible = true,
            IsEnabled = true,
            Index = 5,
            Localizable = true,
            Tooltip = "ComboBoxEnumImageToolTips"
            )]
        public EnumLocalized ComboBoxEnumImage { get; set; }

        # endregion EnumComboBox

        # region ComboBox Category

        /// <summary>
        /// ComboBox supporting int16 values.
        /// This comboBox is filled using "DataSourceInt16List" data source.
        /// "DataSourceInt16List" is based on int16List values {1,2,3,4}.
        /// </summary> 
        [SchemaProperty]
        [ComboBox(
            DataSourceKey = "DataSourceInt16List",
            Category = "ComboBox",
            Description = "ComboBoxInt16",
            IsVisible = true,
            IsEnabled = true,
            Index = 1,
            Localizable = true,
            Tooltip = "ComboBoxInt16ToolTips"
            )]
        public Int16 ComboBoxInt16 { get; set; }

        /// <summary>
        /// ComboBox supporting int32 values.
        /// This comboBox is filled using "DataSourceInt32List" data source.
        /// "DataSourceInt32List" is based on int32List values {1,2,3,4}.
        /// </summary> 
        [SchemaProperty]
        [ComboBox(
            DataSourceKey = "DataSourceInt32List",
            Category = "ComboBox",
            Description = "ComboBoxInt32",
            IsVisible = true,
            IsEnabled = true,
            Index = 1,
            Localizable = true,
            Tooltip = "ComboBoxInt32ToolTips"
            )]
        public Int32 ComboBoxInt32 { get; set; }

        /// <summary>
        /// ComboBox supporting bool values.
        /// This comboBox is filled using "DataSourceBoolList" data source.
        /// "DataSourceBoolList" is based on boolList values {true,false}.
        /// </summary> 
        [SchemaProperty]
        [ComboBox(
            DataSourceKey = "DataSourceBoolList",
            Category = "ComboBox",
            Description = "ComboBoxBool",
            IsVisible = true,
            IsEnabled = true,
            Index = 1,
            Localizable = true,
            Tooltip = "ComboBoxBoolToolTips"
            )]
        public bool ComboBoxBool { get; set; }

        /// <summary>
        /// ComboBox supporting UV values.
        /// This comboBox is filled using "DataSourceUVList" data source.
        /// "DataSourceUVList" is based on uvList values {(1,1),(2,2),(3,3)}.
        /// </summary> 
        [SchemaProperty(Unit = UnitType.UT_Length, DisplayUnit = DisplayUnitType.DUT_METERS)]
        [ComboBox(
            DataSourceKey = "DataSourceUVList",
            Category = "ComboBox",
            Description = "ComboBoxUV",
            IsVisible = true,
            IsEnabled = true,
            Index = 1,
            Localizable = true,
            Tooltip = "ComboBoxUVToolTips"
            )]
        public UV ComboBoxUV { get; set; }

        /// <summary>
        /// ComboBox supporting XYZ values.
        /// This comboBox is filled using "DataSourceXYZList" data source.
        /// "DataSourceXYZList" is based on xyzList values {(1,1,1),(2,2,2),(3,3,3)}.
        /// </summary> 
        [SchemaProperty(Unit = UnitType.UT_Length, DisplayUnit = DisplayUnitType.DUT_METERS)]
        [ComboBox(
            DataSourceKey = "DataSourceXYZList",
            Category = "ComboBox",
            Description = "ComboBoxXYZ",
            IsVisible = true,
            IsEnabled = true,
            Index = 1,
            Localizable = true,
            Tooltip = "ComboBoxXYZToolTips"
            )]
        public XYZ ComboBoxXYZ { get; set; }

        /// <summary>
        /// ComboBox supporting string values.
        /// This comboBox is filled using "DataSourceStringList" data source.
        /// "DataSourceStringList" is based on stringList values {"Choice 1", "Choice 2", "Choice 3", "Choice 4"}.
        /// </summary> 
        [SchemaProperty]
        [ComboBox(
            DataSourceKey = "DataSourceStringList",
            Category = "ComboBox",
            Description = "ComboBoxString",
            IsVisible = true,
            IsEnabled = true,
            Index = 1,
            Localizable = true,
            Tooltip = "ComboBoxStringToolTips"
            )]
        public string ComboBoxString { get; set; }

        /// <summary>
        /// ComboBox supporting ElementId values.
        /// Selected index value is stored as ElementId.
        /// This comboBox is filled using "DataSourceElementIdList" data source.
        /// "DataSourceElementIdList" is the list of ElementID for the first five rebars form the current document 
        /// </summary> 
        [SchemaProperty]
        [ComboBox(
            Description = "ComboBoxElementId",
            Category = "ComboBox",
            IsVisible = true,
            IsEnabled = true,
            Index = 6,
            Localizable = true,
            DataSourceKey = "DataSourceElementIdList",
            Tooltip = "ComboElementIDToolTips"
            )]
        public ElementId ComboBoxElementId { get; set; }

        /// <summary>
        /// ComboBox supporting Rebar type values.
        /// This Element ComboBox is filled using "DataSourceRebarList" data source.
        /// "DataSourceRebarList" is the list of Rebars from current document.
        /// </summary> 
        [SchemaProperty]
        [ElementComboBox(
            Description = "ComboBoxRebar",
            Category = "ComboBoxRebar",
            IsVisible = true,
            IsEnabled = true,
            Index = 6,
            Localizable = true,
            DataSourceKey = "DataSourceRebarList",
            Tooltip = "ComboBoxRebarToolTips"
            )]
        public Autodesk.Revit.DB.Structure.RebarBarType  ComboBoxRebar { get; set; }

        /// <summary>
        /// ComboBox supporting guid values.
        /// This comboBox is filled using "DataSourceGuidList" data source.
        /// "DataSourceGuidList" is based on guidList values {"6AED35BD-9143-4AAB-B568-7FC69C946824"),"F6F9D635-6AF3-4336-9D52-E734DFA9F97E", "E72993A5-CDFE-4501-9A34-D3A6DA407CD6" ;.
        /// </summary> 
        [SchemaProperty]
        [ComboBox(
            DataSourceKey = "DataSourceGuidList",
            Category = "ComboBox",
            Description = "ComboBoxGuid",
            IsVisible = true,
            IsEnabled = true,
            Index = 1,
            Localizable = true,
            Tooltip = "ComboBoxGuidToolTips"
            )]
        public Guid ComboBoxGuid { get; set; }

        /// <summary>
        /// ComboBox supporting double values.
        /// The unit type is set to length (DUT_METERS). 
        /// Selected index value is stored in DUT_METERS.
        /// This comboBox is filled using "DataSourceDoubleList" data source.
        /// "DataSourceDoubleList" is based on doubleList values {10.0, 20.0, 30.0, 40.0}.
        /// </summary> 
        [SchemaProperty(Unit = UnitType.UT_Length, DisplayUnit = DisplayUnitType.DUT_METERS)]
        [ComboBox(
            DataSourceKey = "DataSourceDoubleList",
            Category = "ComboBox",
            Description = "ComboBoxDouble",
            IsVisible = true,
            IsEnabled = true,
            Index = 0,
            Localizable = true,
            Tooltip = "ComboBoxDoubleToolTips"
            )]
        public Double ComboBoxDouble { get; set; }

        # endregion ComboBox Category

    }
}