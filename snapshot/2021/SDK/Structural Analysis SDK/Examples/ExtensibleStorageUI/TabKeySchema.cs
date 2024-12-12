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
    [Schema("tabKeySchema", "02639b0b-d52a-453c-be68-a4a98aa81946")]
    public class TabKeySchema : SchemaClass
    {
        # region Constructors

  /// <summary>
  /// Initialize 
  /// </summary>
        public TabKeySchema()
        {
            GridKeyComboBoxString = new Dictionary<String, String>();
            GridKeyTextBoxString = new Dictionary<String, String>();
            GridKeyTextBoxStringDouble = new Dictionary<String, Double>();
            GridKeyTextBoxStringUnitDouble = new Dictionary<String, Double>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="document"></param>
        public TabKeySchema(Document document)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="document"></param>
        public TabKeySchema(Entity entity, Document document)
            : base(entity, document)
        {
     
        }
        #endregion Constructors 


        # region GridKey
        /// <summary>
        /// Grid Key TextBox supporting a key defined as string and a value defined as double.
        /// Default value is set to 10. Default key is set to 1 and will be incremented after each addition.
        /// Key are ckecked and validate on runtime to avoid duplication.    
        /// </summary>
        [SchemaProperty(Unit = UnitType.UT_Length, DisplayUnit = DisplayUnitType.DUT_CENTIMETERS)]
        [GridKeyTextBox(
            AttributeUnit = DisplayUnitType.DUT_METERS,
            IsVisible = true,
            IsEnabled = true,
            Category = "GridKey",
            Index = 2,
            Localizable = true,
            DefaultValue = "1")]
        [GridTextBox(
            AllowToAddElements = true,
            AllowToRemoveElements = true,
            AttributeUnit = DisplayUnitType.DUT_METERS,
            Category = "GridKey",
            IsVisible = true,
            IsEnabled = true,
            Index = 3,
            Localizable = true,
            Description = "GridKeyTextBoxStringDouble",
            Tooltip = "GridKeyTextBoxStringDoubleToolTips",
            DefaultValue = 10.0)]
        public Dictionary<String, Double> GridKeyTextBoxStringDouble { get; set; }


        /// <summary>
        /// Grid Key TextBox supporting a key defined as string and value defined as double, the unit type is set to length (meters). 
        /// Default value is set to 10.0 . Default key is set to 1 and will be incremented after each addition.
        /// </summary>
        [SchemaProperty(Unit = UnitType.UT_Length, DisplayUnit = DisplayUnitType.DUT_CENTIMETERS)]
        [GridKeyTextBox(
            AttributeUnit = DisplayUnitType.DUT_METERS,
            Category = "GridKey",
            IsVisible = true,
            IsEnabled = true,
            Index = 2,
            Localizable = true,
            DefaultValue = "1")]
        [GridUnitTextBox(
            AllowToAddElements = true,
            AllowToRemoveElements = true,
            AttributeUnit = DisplayUnitType.DUT_METERS,
            Category = "GridKey",
            IsVisible = true,
            IsEnabled = true,
            Index = 4,
            Localizable = true,
            Description = "GridKeyTextBoxStringUnitDouble",
            Tooltip = "GridKeyTextBoxStringUnitDoubleToolTips",
            DefaultValue = 10.0)]
        public Dictionary<String, Double> GridKeyTextBoxStringUnitDouble { get; set; }


        /// <summary>
        /// Grid Key TextBox supporting a key defined as string and value defined as string. 
        /// Default value is set to "Choice 1" . 
        /// Default key is set to 1 and will be incremented after each addition.
        /// Key are ckecked and validate on runtime to avoid duplication.    
        /// </summary>
        [SchemaProperty]
        [GridKeyTextBox(
            AttributeUnit = DisplayUnitType.DUT_METERS,
            Category = "GridKey",
            IsVisible = true,
            IsEnabled = true,
            Index = 3,
            Localizable = true,
            DefaultValue = "1")]
        [GridComboBox(
            AllowToAddElements = true,
            AllowToRemoveElements = true,
            AttributeUnit = DisplayUnitType.DUT_METERS,
            Category = "GridKey",
            IsVisible = true,
            IsEnabled = true,
            Index = 5,
            Localizable = true,
            Description = "GridKeyTextBoxString",
            Tooltip = "GridKeyTextBoxStringToolTips",
            DataSourceKey = "DataSourceStringList",
            DefaultValue = "Choice 1")]
        public Dictionary<String, String> GridKeyTextBoxString { get; set; }


        /// <summary>
        /// Grid Key ComboBox supporting a  key defined as a string and a value defined as string. 
        /// ComboBox is filled using  "ComboBoxString" data source (Choice 1,Choice 2,Choice 3, Choice 4). 
        /// </summary>
        [SchemaProperty]
        [GridKeyComboBox(
            AttributeUnit = DisplayUnitType.DUT_METERS,
            Category = "GridKey",
            IsVisible = true,
            IsEnabled = true,
            Index = 4,
            Localizable = true,
            DefaultValue = "Choice 1",
            DataSourceKey = "DataSourceStringList")]
        [GridComboBox(
            AllowToAddElements = true,
            AllowToRemoveElements = true,
            AttributeUnit = DisplayUnitType.DUT_METERS,
            Category = "GridKey",
            IsVisible = true,
            IsEnabled = true,
            Index = 6,
            Localizable = true,
            Description = "GridKeyComboBoxString",
            Tooltip = "GridKeyComboBoxStringToolTips",
            DefaultValue = "Choice 1",
            DataSourceKey = "DataSourceStringList")]
        public Dictionary<String, String> GridKeyComboBoxString { get; set; }
       
        # endregion GridKey
    }
}