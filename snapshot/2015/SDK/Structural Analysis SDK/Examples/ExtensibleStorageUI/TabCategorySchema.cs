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
    [Schema("tabCategorySchema", "e7152376-4340-4b0c-969b-4cc89874d8fb")]
    public class TabCategorySchema : SchemaClass
    {

        # region CheckBox
        /// <summary>
        /// CheckBox unchecked.
        /// </summary>
        [SchemaProperty]
        [CheckBox(
            Category = "CheckBox",
            IsVisible = true,
            IsEnabled = true,
            Index = 0,
            Localizable = true,
            Description = "CheckBoxUnchecked",
            Tooltip = "CheckBoxUncheckedToolTips"
            )]
        public Boolean CheckBoxUnchecked { get; set; }

        /// <summary>
        /// CheckBox checked.
        /// </summary>
        [SchemaProperty]
        [CheckBox(
            Category = "CheckBox",
            IsVisible = true,
            IsEnabled = true,
            Index = 0,
            Localizable = true,
            Description = "CheckBoxChecked",
            Tooltip = "CheckBoxCheckedToolTips"
            )]
        public Boolean CheckBoxChecked { get; set; }

        # endregion CheckBox

        # region Category CheckBox

        /// <summary>
        ///  Category CheckBox drives the status of controls stacked to  this category.
        ///  Enable it will enable all controls, disable it will disable all controls. 
        /// </summary>
        [SchemaProperty]
        [CategoryCheckBox(
            Category = "CategoryCheckBox",
            IsVisible = true,
            IsEnabled = true,
            Index = 1,
            Localizable = true,
            Description = "CategoryCheckBox",
            Tooltip = "CategoryCheckBoxToolTips"
            )]
        public Boolean CategoryCheckBox { get; set; }

        /// <summary>
        /// Unit ComboBox supporting double values.
        /// The unit type is set to length (DUT_METERS). 
        /// Selected index value is stored in DUT_METERS.
        /// This comboBox is filled using "DataSourceDoubleList" data source.
        /// "DataSourceDoubleList" is based on doubleList values {10.0, 20.0, 30.0, 40.0}.
        /// </summary> 
        [SchemaProperty(Unit = UnitType.UT_Length, DisplayUnit = DisplayUnitType.DUT_METERS, FieldName = "")]
        [UnitComboBox(
            DataSourceKey = "DataSourceDoubleList",
            Category = "CategoryCheckBox",
            Description = "ComboBoxDouble",
            IsVisible = true,
            IsEnabled = true,
            Index = 3,
            Localizable = true,
            Tooltip = "ComboBoxDoubleToolTips"
            )]
        public Double ComboBoxDouble { get; set; }
 
        /// <summary>
        /// Unit Checklist supporting double values.
        /// The unit type is set to length (DUT_METERS). 
        /// Selected index value is stored in DUT_METERS.
        /// This Checklist is filled using "DataSourceDoubleList" data source.
        /// "DataSourceDoubleList" is based on doubleList values {10.0, 20.0, 30.0, 40.0}.
        /// </summary> 
        [SchemaProperty(Unit = UnitType.UT_Length, DisplayUnit = DisplayUnitType.DUT_METERS)]
        [UnitCheckedList(
            DataSourceKey = "DataSourceDoubleList",
            Category = "CategoryCheckBox",
            IsVisible = true,
            IsEnabled = true,
            Index = 2,
            Localizable = true,
            Description = "UnitCheckListDouble",
            Tooltip = "UnitCheckListDoubleToolTips"
            )]
        public List<Double> UnitCheckListDouble { get; set; }
        # endregion Category CheckBox

        # region Constructors

        /// <summary>
        /// 
        /// </summary>
        public TabCategorySchema()
        {
            CategoryCheckBox = true;
            CheckBoxChecked = true;
            CheckBoxUnchecked = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="document"></param>
        public TabCategorySchema(Document document)
        {
       
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="document"></param>
        public TabCategorySchema(Entity entity, Document document)
            : base(entity, document)
        {
 
        }

        #endregion Constructors
    }
}