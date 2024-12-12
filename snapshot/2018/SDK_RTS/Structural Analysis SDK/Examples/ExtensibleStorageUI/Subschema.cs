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
    /// Simple subschema 
    /// </summary>
    /// 


    [Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes.Schema("SubSchema", "8a89c457-ae12-45ce-b1d2-fa19be3dbf82")]
    public class SubSchema : Autodesk.Revit.DB.ExtensibleStorage.Framework.SchemaClass
    {

        # region Constructors
        /// <summary>
       /// 
       /// </summary>
        public SubSchema()
        {
            UnitTextBoxDouble = 10;
            CheckBoxChecked = true;
            ComboBoxInt32 = 1;
            UnitComboBoxDouble = 10;
  
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="document"></param>
        //public SubSchema(Document document)
        //{
        //    UnitTextBoxDouble = 10;
        //    CheckBoxChecked = true;
        //    ComboBoxInt32 = 1;
        //    UnitComboBoxDouble = 10;
  
    

        //}
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="entity"></param>
        ///// <param name="document"></param>
        public SubSchema(Entity entity, Document document)
         : base(entity, document)
        {

  
        }
        #endregion Constructors

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
        /// Unit ComboBox supporting double values.
        /// The unit type is set to length (DUT_METERS). 
        /// Selected index value is stored in DUT_METERS.
        /// This comboBox is filled using "DataSourceDoubleList" data source.
        /// "DataSourceDoubleList" is based on doubleList values {10.0, 20.0, 30.0, 40.0}.
        /// </summary> 
        [SchemaProperty(Unit = UnitType.UT_Length, DisplayUnit = DisplayUnitType.DUT_METERS)]
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
     
    }
}