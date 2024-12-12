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
using System.Collections;
using System.Collections.Generic;
using System.Linq; 

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.DB.ExtensibleStorage.Framework;
using Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes;
using Autodesk.Revit.UI.ExtensibleStorage.Framework.Attributes;
using Autodesk.Revit.DB.Structure ;
namespace ExtensibleStorageUI
{
    /// <summary>
    /// 
    /// </summary>
    [Schema("tabGridSchema", "02639b0b-d52a-453c-be68-a4a98aa81945")]
    public class TabGridSchema : SchemaClass
    {
 
        public RebarBarType rbt =  null; 
  
        # region Constructors
        /// <summary>
        /// Initialize 
        /// </summary>
        public TabGridSchema()
        {
            # region GridTextBox
            GridTextBoxDouble = new List<Double>();
            GridTextBoxXYZ = new List<XYZ>();
            GridTextBoxUV = new List<UV>();
            GridTextBoxBool = new List<bool>();
            GridTextBoxInt16 = new List<short>();
            GridTextBoxInt32 = new List<int>();
            GridTextBoxString = new List<string>();
            GridTextBoxGuid = new List<Guid>();
            GridTextBoxXYZRemoveNotAllow = new List<XYZ> { new XYZ(1, 1, 1), new XYZ(2, 2, 2) };
            # endregion GridTextBox

            # region GridComboBox
            GridComboBoxDouble = new List<Double>();
            GridComboBoxXYZ = new List<XYZ>();
            GridComboBoxUV = new List<UV>();
            GridComboBoxBool = new List<bool>();
            GridComboBoxInt16 = new List<short>();
            GridComboBoxInt32 = new List<int>();
            GridComboBoxString = new List<string>();
            GridComboBoxGuid = new List<Guid>();
            # endregion GridComboBox

            GridUnitTextBoxDouble = new List<Double>();
            GridXYZTextBox = new List<XYZ>();
            GridCheckBoxBool = new List<bool>();
     
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="document"></param>
        public TabGridSchema(Document document)
        {
         
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="document"></param>
        public TabGridSchema(Entity entity, Document document)
            : base(entity, document)
        {
             
        }
        #endregion Constructors 

        # region GridTextBox 
        /// <summary>
        /// Grid  TextBox supporting a value defined as double. 
        /// </summary>
        [SchemaProperty(Unit = UnitType.UT_Area, DisplayUnit = DisplayUnitType.DUT_SQUARE_CENTIMETERS)]
        [GridTextBox(
            AllowToAddElements = true,
            AllowToRemoveElements = true,
            AttributeUnit = DisplayUnitType.DUT_SQUARE_CENTIMETERS,
            Category = "Grid TextBox",
            IsVisible = true,
            IsEnabled = true,
            Index = 0,
            Localizable = true,
            Description = "GridTextBoxDouble",
            Tooltip = "GridTextBoxDoubleToolTips"
            )]
        public List<Double> GridTextBoxDouble { get; set; }

        /// <summary>
        /// Grid TextBox supporting a value defined as XYZ. 
        /// Default XYZ is provided by the XYZDefaultValueProvider (1,1,1).
        /// </summary>
        [SchemaProperty(Unit = UnitType.UT_Length , DisplayUnit = DisplayUnitType.DUT_METERS)]
        [GridTextBox(
            AllowToAddElements = true,
            AllowToRemoveElements = true,
            AttributeUnit = DisplayUnitType.DUT_METERS,
            Category = "Grid TextBox",
            IsVisible = true,
            IsEnabled = true,
            Index = 0,
            Localizable = true,
            Description = "GridTextBoxXYZ",
            Tooltip = "GridTextBoxXYZToolTips",
            DefaultValueProvider = typeof(XYZDefaultValueProvider)
            )]
        public List<XYZ> GridTextBoxXYZ { get; set; }

        /// <summary>
        /// Grid TextBox supporting a value defined as UV. 
        /// Default UV is provided by the UVDefaultValueProvider (1,1).
        /// </summary>
        [SchemaProperty(Unit = UnitType.UT_Length, DisplayUnit = DisplayUnitType.DUT_METERS)]
        [GridTextBox(
            AllowToAddElements = true,
            AllowToRemoveElements = true,
            AttributeUnit = DisplayUnitType.DUT_METERS,
            Category = "Grid TextBox",
            IsVisible = true,
            IsEnabled = true,
            Index = 0,
            Localizable = true,
            Description = "GridTextBoxUV",
            Tooltip = "GridTextBoxUVToolTips",
            DefaultValueProvider = typeof(UVDefaultValueProvider)
            )]
        public List<UV> GridTextBoxUV { get; set; }
        
        /// <summary>
        /// Grid TextBox supporting a value defined as integer16. 
        /// Default integer is provided by the IntDefaultValueProvider (2).
        /// </summary>
        [SchemaProperty()]
        [GridTextBox(
            AllowToAddElements = true,
            AllowToRemoveElements = true,
            Category = "Grid TextBox",
            IsVisible = true,
            IsEnabled = true,
            Index = 0,
            Localizable = true,
            Description = "GridTextBoxInt16",
            Tooltip = "GridTextBoxInt16ToolTips",
            DefaultValueProvider = typeof(Int16DefaultValueProvider)
            )]
        public List<Int16> GridTextBoxInt16 { get; set; }

        /// <summary>
        /// Grid TextBox supporting a value defined as integer32.
        /// integer is set to 5 per default.
        /// </summary>
        [SchemaProperty()]
        [GridTextBox(
            AllowToAddElements = true,
            AllowToRemoveElements = true,
            Category = "Grid TextBox",
            IsVisible = true,
            IsEnabled = true,
            Index = 0,
            Localizable = true,
            Description = "GridTextBoxInt32",
            Tooltip = "GridTextBoxInt32ToolTips",
            DefaultValue = 5
            )]
        public List<Int32> GridTextBoxInt32 { get; set; }

        /// <summary>
        /// Grid TextBox supporting a value defined as string.
        /// String is set to "Choice 1" per default.
        /// </summary>
        [SchemaProperty()]
        [GridTextBox(
            AllowToAddElements = true,
            AllowToRemoveElements = true,
            Category = "Grid TextBox",
            IsVisible = true,
            IsEnabled = true,
            Index = 0,
            Localizable = true,
            Description = "GridTextBoxString",
            Tooltip = "GridTextBoxStringToolTips",
            DefaultValue = "Choice 1"
            )]
        public List<string> GridTextBoxString { get; set; }

        /// <summary>
        /// Grid TextBox supporting a value defined as boolean.
        /// Boolean is set to true per default. 
        /// </summary>
        [SchemaProperty()]
        [GridTextBox(
            AllowToAddElements = true,
            AllowToRemoveElements = true,
            Category = "Grid TextBox",
            IsVisible = true,
            IsEnabled = true,
            Index = 0,
            Localizable = true,
            Description = "GridTextBoxBool",
            Tooltip = "GridTextBoxBoolToolTips",
            DefaultValue =  true
            )]
        public List<bool> GridTextBoxBool { get; set; }
 
        /// <summary>
        /// Grid TextBox supporting a value defined as guid.
        /// Default guid is porvided by the GUIDDefaultValueProvider
        /// </summary>
        [SchemaProperty()]
        [GridTextBox(
            AllowToAddElements = true,
            AllowToRemoveElements = true,
            Category = "Grid TextBox",
            IsVisible = true,
            IsEnabled = true,
            Index = 0,
            Localizable = true,
            Description = "GridTextBoxGuid",
            Tooltip = "GridTextBoxGuidToolTips" ,
            DefaultValueProvider = typeof(GUIDDefaultValueProvider)  
            )]
        public List<Guid> GridTextBoxGuid { get; set; }
        # endregion GridTextBox

        # region GridComboBox
        /// <summary>
        /// Grid ComboBox supporting some double values. 
        /// ComboBox is filled using  "ComboBoxDouble" data source [10,20,30,40]. 
        /// </summary>
        [SchemaProperty(Unit = UnitType.UT_Length, DisplayUnit = DisplayUnitType.DUT_CENTIMETERS)]
        [GridComboBox(
            AllowToAddElements = true,
            AllowToRemoveElements = true,
            AttributeUnit = DisplayUnitType.DUT_METERS,
            Category = "Grid ComboBox",
            IsVisible = true,
            IsEnabled = true,
            Index = 1,
            Localizable = true,
            Description = "GridComboBoxDouble",
            Tooltip = "GridComboBoxDoubleToolTips",
            DataSourceKey = "DataSourceDoubleList",
            DefaultValue = 10
            )]
        public List<Double> GridComboBoxDouble { get; set; }

        /// <summary>
        /// Grid ComboBox supporting some XYZ values. 
        /// ComboBox is filled using  "DataSourceXYZList" data source. 
        /// Default value is set using XYZDefaultValueProvider
        /// </summary>
        [SchemaProperty(Unit = UnitType.UT_Length, DisplayUnit = DisplayUnitType.DUT_CENTIMETERS)]
        [GridComboBox(
            AllowToAddElements = true,
            AllowToRemoveElements = true,
            AttributeUnit = DisplayUnitType.DUT_METERS,
            Category = "Grid ComboBox",
            IsVisible = true,
            IsEnabled = true,
            Index = 1,
            Localizable = true,
            Description = "GridComboBoxXYZ",
            Tooltip = "GridComboBoxXYZToolTips",
            DataSourceKey = "DataSourceXYZList",
            DefaultValueProvider = typeof(XYZDefaultValueProvider) 
            )]
        public List<XYZ> GridComboBoxXYZ { get; set; }

        /// <summary>
        /// Grid ComboBox supporting UV values. 
        /// ComboBox is filled using  "DataSourceUVList" data source. 
        /// Default value is set using UVDefaultValueProvider
        /// </summary>
        [SchemaProperty(Unit = UnitType.UT_Length, DisplayUnit = DisplayUnitType.DUT_CENTIMETERS)]
        [GridComboBox(
            AllowToAddElements = true,
            AllowToRemoveElements = true,
            AttributeUnit = DisplayUnitType.DUT_METERS,
            Category = "Grid ComboBox",
            IsVisible = true,
            IsEnabled = true,
            Index = 1,
            Localizable = true,
            Description = "GridComboBoxUV",
            Tooltip = "GridComboBoxUVToolTips",
            DataSourceKey = "DataSourceUVList",
            DefaultValueProvider = typeof(UVDefaultValueProvider) 
            )]
        public List<UV> GridComboBoxUV { get; set; }

        /// <summary>
        /// Grid ComboBox supporting integer16 values. 
        /// ComboBox is filled using  "DataSourceInt16List" data source.
        /// Default value is set using Int16DefaultValueProvider
        /// </summary>
        [SchemaProperty()]
        [GridComboBox(
            AllowToAddElements = true,
            AllowToRemoveElements = true,
            Category = "Grid ComboBox",
            IsVisible = true,
            IsEnabled = true,
            Index = 1,
            Localizable = true,
            Description = "GridComboBoxInt16",
            Tooltip = "GridComboBoxInt16ToolTips",
            DataSourceKey = "DataSourceInt16List",
            DefaultValueProvider = typeof(Int16DefaultValueProvider)
            )]
        public List<Int16 > GridComboBoxInt16 { get; set; }

        /// <summary>
        /// Grid ComboBox supporting integer32 values. 
        /// ComboBox is filled using  "DataSourceInt32List" data source.
        /// Default value is set to 1.
        [SchemaProperty()]
        [GridComboBox(
            AllowToAddElements = true,
            AllowToRemoveElements = true,
            Category = "Grid ComboBox",
            IsVisible = true,
            IsEnabled = true,
            Index = 1,
            Localizable = true,
            Description = "GridComboBoxInt32",
            Tooltip = "GridComboBoxInt32ToolTips",
            DataSourceKey = "DataSourceInt32List",
            DefaultValue = 1
            )]
        public List<Int32> GridComboBoxInt32 { get; set; }

        /// <summary>
        /// Grid ComboBox supporting guid values. 
        /// ComboBox is filled using  "DataSourceGuidList" data source.
        /// Default value is set using GUIDDefaultValueProvider
        [SchemaProperty()]
        [GridComboBox(
            AllowToAddElements = true,
            AllowToRemoveElements = true,
            Category = "Grid ComboBox",
            IsVisible = true,
            IsEnabled = true,
            Index = 1,
            Localizable = true,
            Description = "GridComboBoxGuid",
            Tooltip = "GridComboBoxGuidToolTips",
            DataSourceKey = "DataSourceGuidList",
            DefaultValueProvider = typeof(GUIDDefaultValueProvider)  
            )]
        public List<Guid> GridComboBoxGuid { get; set; }

        /// <summary>
        /// Grid ComboBox supporting boolean values. 
        /// ComboBox is filled using  "DataSourceBoolList" data source.
        /// Default value is set to true.
        [SchemaProperty()]
        [GridComboBox(
            AllowToAddElements = true,
            AllowToRemoveElements = true,
            Category = "Grid ComboBox",
            IsVisible = true,
            IsEnabled = true,
            Index = 1,
            Localizable = true,
            Description = "GridComboBoxBool",
            Tooltip = "GridComboBoxBoolToolTips",
            DataSourceKey = "DataSourceBoolList",
            DefaultValue = true
            )]
        public List<bool> GridComboBoxBool { get; set; }

        /// <summary>
        /// Grid ComboBox supporting string values. 
        /// ComboBox is filled using  "DataSourceStringList" data source.
        /// Default value is set to "Choice 1".
        [SchemaProperty()]
        [GridComboBox(
            AllowToAddElements = true,
            AllowToRemoveElements = true,
            Category = "Grid ComboBox",
            IsVisible = true,
            IsEnabled = true,
            Index = 1,
            Localizable = true,
            Description = "GridComboBoxString",
            Tooltip = "GridComboBoxStringToolTips",
            DataSourceKey = "DataSourceStringList",
            DefaultValue = "Choice 1"
            )]
        public List<string> GridComboBoxString { get; set; }

        # endregion GridComboBox

        # region UnitTextBox
        /// <summary>
        /// Grid  TextBox supporting a value defined as double.
        /// Default value is set to 10.0.
        /// </summary>
        [SchemaProperty(Unit = UnitType.UT_Area, DisplayUnit = DisplayUnitType.DUT_SQUARE_CENTIMETERS)]
        [GridUnitTextBox(
            AllowToAddElements = true,
            AllowToRemoveElements = true,
            Category = "Grid UnitTextBox",
            AttributeUnit = DisplayUnitType.DUT_SQUARE_CENTIMETERS,
            IsVisible = true,
            IsEnabled = true,
            Index = 2,
            Localizable = true,
            Description = "GridUnitTextBoxDouble",
            Tooltip = "GridUnitTextBoxDoubleToolTips",
            DefaultValue = 10.0)]
        public List<Double> GridUnitTextBoxDouble { get; set; }
        # endregion UnitTextBox

        # region XYZTextBox
        /// <summary>
        /// Grid TextBox supporting a value defined as XYZ. 
        /// Default XYZ is provided by the XYZDefaultValueProvider (1,1,1).
        /// </summary>
        [SchemaProperty(Unit = UnitType.UT_Length, DisplayUnit = DisplayUnitType.DUT_METERS)]
        [GridXYZTextBox(
            AllowToAddElements = true,
            AllowToRemoveElements = true,
            AttributeUnit = DisplayUnitType.DUT_METERS,
            Category = "Grid XYZTextBox",
            IsVisible = true,
            IsEnabled = true,
            Index = 7,
            Localizable = true,
            Description = "GridXYZTextBox",
            Tooltip = "GridXYZTextBoxToolTips",
            DefaultValueProvider = typeof(XYZDefaultValueProvider)  
            )]
        public List<XYZ> GridXYZTextBox { get; set; }

        /// <summary>
        /// Grid  TextBox supporting a key defined as string and value defined as XYZ. 
        /// Default values are for the first point 1 (1,1,1) and for the point 2 (2,2,2).
        /// Point 1 and 2 are defined on the constructor.
        /// </summary>
        [SchemaProperty(Unit = UnitType.UT_Length, DisplayUnit = DisplayUnitType.DUT_METERS)]
        [GridXYZTextBox(
            AllowToAddElements = false,
            AllowToRemoveElements = false,
            AttributeUnit = DisplayUnitType.DUT_METERS,
            Category = "XYZTextBox",
            IsVisible = true,
            IsEnabled = true,
            Index = 7,
            Localizable = true,
            Description = "GridTextBoxXYZRemoveNotAllow",
            Tooltip = "GridTextBoxXYZRemoveNotAllowToolTips"
            )]
        public List<XYZ> GridTextBoxXYZRemoveNotAllow { get; set; }
        # endregion XYZTextBox

        # region GridCheckBox
        /// <summary>
        /// Grid  CheckBox supporting a  boolean.
        ////// </summary>
        [SchemaProperty()]
        [GridCheckBox(
            AllowToAddElements = true,
            AllowToRemoveElements = true,
            AttributeUnit = DisplayUnitType.DUT_METERS,
            Category = "Grid CheckBox",
            IsVisible = true,
            IsEnabled = true,
            Index = 1,
            Localizable = true,
            Description = "GridCheckBoxBool",
            Tooltip = "GridCheckBoxBoolToolTips",
            DefaultValue  = true
            )]
        public List<bool> GridCheckBoxBool { get; set; }
        # endregion GridCheckBox

        # region GridEnumCombobox
        /// <summary>
        /// Grid Enum ComboBox supporting image. 
        /// Default value is set to Choice 3 
        /// <summary>
        [SchemaProperty()]
        [GridEnumControl(
            AllowToAddElements = true,
            AllowToRemoveElements = true,
            EnumType = typeof(EnumLocalized),
            Presentation = PresentationMode.Combobox,
            Item = PresentationItem.Image,
            Description = "GridEnumComboboxImage",
            Category = "Grid Enum Combobox",
            IsVisible = true,
            IsEnabled = true,
            Index = 6,
            Localizable = true,
            Tooltip = "GridEnumComboboxImageToolTips",
            DefaultValue = EnumLocalized.Choice3
            )]
        public List<EnumLocalized> GridEnumComboboxImage { get; set; }
        
        /// <summary>
        /// Grid Enum ComboBox supporting text. 
        /// Default value is set to Choice 2 
        /// <summary>
        [SchemaProperty()]
        [GridEnumControl(
            AllowToAddElements = true,
            AllowToRemoveElements = true,
            EnumType = typeof(EnumLocalized),
            Presentation = PresentationMode.Combobox,
            Item = PresentationItem.Text,
            Description = "GridEnumComboboxText",
            Category = "Grid Enum Combobox",
            IsVisible = true,
            IsEnabled = true,
            Index = 6,
            Localizable = true,
            Tooltip = "GridEnumComboboxTextToolTips",
            DefaultValue = EnumLocalized.Choice2 
            )]
        public List<EnumLocalized> GridEnumComboboxText { get; set; }

        /// <summary>
        /// Grid Enum ComboBox supporting image and text. 
        /// Default value is set to Choice 1 
        /// <summary>
        [SchemaProperty()]
        [GridEnumControl(
            AllowToAddElements = true,
            AllowToRemoveElements = true,
            EnumType=   typeof(EnumLocalized), 
            Presentation = PresentationMode.Combobox,
            Item = PresentationItem.ImageWithText ,
            Description = "GridEnumComboboxImageText",
            Category = "Grid Enum Combobox",
            IsVisible = true,
            IsEnabled = true,
            Index = 6,
            Localizable = true,
            Tooltip = "GridEnumComboboxImageTextToolTips",
            DefaultValue = EnumLocalized.Choice1
            )]
        public List<EnumLocalized> GridEnumComboboxImageText { get; set; }
        # endregion GridEnumCombobox

        # region GridEnumOptionList
        /// <summary>
        /// Grid Enum OptionList supporting image. 
        /// Default value is set to Choice 3. 
        /// <summary>
        [SchemaProperty()]
        [GridEnumControl(
            AllowToAddElements = true,
            AllowToRemoveElements = true,
            EnumType = typeof(EnumLocalized),
            Presentation = PresentationMode.OptionList ,
            Item = PresentationItem.Image,
            Description = "GridEnumOptionListImage",
            Category = "Grid Enum OptionList",
            IsVisible = true,
            IsEnabled = true,
            Index = 6,
            Localizable = true,
            Tooltip = "GridEnumOptionListImageToolTips",
            DefaultValue = EnumLocalized.Choice3
            )]
        public List<EnumLocalized> GridEnumOptionListImage { get; set; }
        
        /// <summary>
        /// Grid Enum OptionList supporting text. 
        /// Default value is set to Choice 2. 
        /// <summary>
        [SchemaProperty()]
        [GridEnumControl(
            AllowToAddElements = true,
            AllowToRemoveElements = true,
            EnumType = typeof(EnumLocalized),
            Presentation = PresentationMode.OptionList,
            Item = PresentationItem.Text,
            Description = "GridEnumOptionListText",
            Category = "Grid Enum OptionList",
            IsVisible = true,
            IsEnabled = true,
            Index = 6,
            Localizable = true,
            Tooltip = "GridEnumOptionListTextToolTips",
            DefaultValue = EnumLocalized.Choice2
            )]
        public List<EnumLocalized> GridEnumOptionListText { get; set; }
        
        /// <summary>
        /// Grid Enum OptionList supporting image and text. 
        /// Default value is set to Choice 1. 
        /// <summary>
        [SchemaProperty()]
        [GridEnumControl(
            AllowToAddElements = true,
            AllowToRemoveElements = true,
            //  EnumType=   typeof(AnEnumLocalized), 
            Presentation = PresentationMode.OptionList,
            Item = PresentationItem.ImageWithText,
            Description = "GridEnumOptionListImageText",
            Category = "Grid Enum OptionList",
            IsVisible = true,
            IsEnabled = true,
            Index = 6,
            Localizable = true,
            Tooltip = "GridEnumOptionListImageTextToolTips",
            DefaultValue = EnumLocalized.Choice1
            )]
        public List<EnumLocalized> GridEnumOptionListImageText { get; set; }
        # endregion GridEnumOptionList

        # region GridEnumToggleButton
        /// <summary>
        /// Grid Enum ToggleButton supporting image. 
        /// Default value is set to Choice 3.
        /// <summary>
        [SchemaProperty()]
        [GridEnumControl(
            AllowToAddElements = true,
            AllowToRemoveElements = true,
            EnumType = typeof(EnumLocalized),
            Presentation = PresentationMode.ToggleButton,
            Item = PresentationItem.Image,
            Description = "GridEnumToggleButtonImage",
            Category = "Grid Enum ToggleButton",
            IsVisible = true,
            IsEnabled = true,
            Index = 6,
            Localizable = true,
            Tooltip = "GridEnumToggleButtonImageToolTips",
            DefaultValue = EnumLocalized.Choice3
            )]
        public List<EnumLocalized> GridEnumToggleButtonImage { get; set; }
        
        /// <summary>
        /// Grid Enum ToggleButton supporting text. 
        /// Default value is set to Choice 2. 
        /// <summary>
        [SchemaProperty()]
        [GridEnumControl(
            AllowToAddElements = true,
            AllowToRemoveElements = true,
            EnumType = typeof(EnumLocalized),
            Presentation = PresentationMode.ToggleButton ,
            Item = PresentationItem.Text,
            Description = "GridEnumToggleButtonText",
            Category = "Grid Enum ToggleButton",
            IsVisible = true,
            IsEnabled = true,
            Index = 6,
            Localizable = true,
            Tooltip = "GridEnumToggleButtonTextToolTips",
            DefaultValue = EnumLocalized.Choice2
            )]
        public List<EnumLocalized> GridEnumToggleButtonText { get; set; }

        /// <summary>
        /// Grid Enum ToggleButton supporting image and text. 
        /// Default value is set to Choice 1.
        /// <summary>
        [SchemaProperty()]
        [GridEnumControl(
            AllowToAddElements = true,
            AllowToRemoveElements = true,
            EnumType=   typeof(EnumLocalized), 
            Presentation = PresentationMode.ToggleButton ,
            Item = PresentationItem.ImageWithText,
            Description = "GridEnumToggleButtonImageText",
            Category = "Grid Enum ToggleButton",
            IsVisible = true,
            IsEnabled = true,
            Index = 6,
            Localizable = true,
            Tooltip = "GridEnumToggleButtonImageTextToolTips",
            DefaultValue = EnumLocalized.Choice1
            )]
        public List<EnumLocalized> GridEnumToggleButtonImageText{ get; set; }

        # endregion GridEnumToggleButton


    }
}