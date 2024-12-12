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
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.DB.ExtensibleStorage.Framework;
using Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes;
using Autodesk.Revit.UI.ExtensibleStorage.Framework.Attributes;

namespace ExtensibleStorageUI
{
    /// <summary>
    /// 
    /// </summary>
    [Schema("tabListSchema", "f39afc35-6c0d-4e9e-9f23-eba3f1419940")]
    public class TabListSchema : SchemaClass
    {
        # region checkedlist  
        /// <summary>
        /// Element CheckedList supporting Rebar type values.
        /// This Element CheckedList is filled using "DataSourceRebarList" data source.
        /// "DataSourceRebarList" is the list of Rebars from current document.
        /// </summary> 
        [SchemaProperty(FieldName = "")]
        [ElementCheckedList(
            DataSourceKey = "DataSourceRebarList",
            Category = "CheckedList",
            IsVisible = true,
            IsEnabled = true,
            Index = 0,
            Localizable = true,
            Description = "CheckListRebar",
            Tooltip = "CheckListRebarToolTips"
            )]
        public List<RebarBarType> CheckListRebar { get; set; }

        /// <summary>
        /// CheckedList supporting string values.
        /// This CheckedList is filled using "DataSourceStringList" data source.
        /// "DataSourceStringList" is based on stringList values {"Choice 1", "Choice 2", "Choice 3", "Choice 4"}.
        /// </summary> 
        [SchemaProperty()]
        [CheckedList(
            DataSourceKey = "DataSourceStringList",
            Category = "CheckedList",
            IsVisible = true,
            IsEnabled = true,
            Index = 1,
            Localizable = true,
            Description = "CheckListString",
            Tooltip = "CheckListStringToolTips"
            )]
        public List<String> CheckListString { get; set; }

        /// <summary>
        /// CheckedList supporting double values.
        /// The unit type is set to length (DUT_METERS). 
        /// Checked items values are stored in DUT_METERS.
        /// This comboBox is filled using "DataSourceDoubleList" data source.
        /// "DataSourceDoubleList" is based on doubleList values {10.0, 20.0, 30.0, 40.0}.
        /// Values are exposed on the UI based on Revit project units settings.  
        /// Revit project unit formatting won't be applyed on the UI (use Unit CheckedList to achieve this). 
        /// </summary> 
        [SchemaProperty(Unit = UnitType.UT_Length, DisplayUnit = DisplayUnitType.DUT_METERS)]
        [CheckedList(
            DataSourceKey = "DataSourceDoubleList",
            Category = "CheckedList",
            IsVisible = true,
            IsEnabled = true,
            Index = 2,
            Localizable = true,
            Description = "CheckListDouble",
            Tooltip = "CheckListDoubleToolTips"
            )]
        public List<Double> CheckListDouble { get; set; }

        /// <summary>
        /// CheckedList supporting XYZ values.
        /// This CheckedList is filled using "DataSourceXYZList" data source.
        /// "DataSourceXYZList" is based on xyzList values {(1, 1, 1),(2, 2, 2), (3, 3, 3)}.
        /// </summary> 
        [SchemaProperty(Unit = UnitType.UT_Length, DisplayUnit = DisplayUnitType.DUT_METERS)]
        [CheckedList(
            DataSourceKey = "DataSourceXYZList",
            Category = "CheckedList",
            IsVisible = true,
            IsEnabled = true,
            Index = 2,
            Localizable = true,
            Description = "CheckListXYZ",
            Tooltip = "CheckListXYZToolTips"
            )]
        public List<XYZ> CheckListXYZ { get; set; }

        /// <summary>
        /// CheckedList supporting UV values.
        /// This CheckedList is filled using "DataSourceUVList" data source.
        /// "DataSourceUVList" is based on uvList values {(1, 1),(2, 2), (3, 3)}.
        /// </summary> 
        [SchemaProperty(Unit = UnitType.UT_Length, DisplayUnit = DisplayUnitType.DUT_METERS)]
        [CheckedList(
            DataSourceKey = "DataSourceUVList",
            Category = "CheckedList",
            IsVisible = true,
            IsEnabled = true,
            Index = 2,
            Localizable = true,
            Description = "CheckListUV",
            Tooltip = "CheckListUVToolTips"
            )]
        public List<UV> CheckListUV { get; set; }

        /// <summary>
        /// CheckedList supporting boolean values.
        /// This CheckedList is filled using "DataSourceBoolList" data source.
        /// "DataSourceBoolList" is based on boolList values {true,false}.
        /// </summary> 
        [SchemaProperty()]
        [CheckedList(
            DataSourceKey = "DataSourceBoolList",
            Category = "CheckedList",
            IsVisible = true,
            IsEnabled = true,
            Index = 2,
            Localizable = true,
            Description = "CheckListBool",
            Tooltip = "CheckListBoolToolTips"
            )]
        public List<bool> CheckListBool { get; set; }

        /// <summary>
        /// CheckedList supporting int16 values.
        /// This CheckedList is filled using "DataSourceInt16List" data source.
        /// "DataSourceInt16List" is based on int16List values {1,2,3,4}.
        /// </summary> 
        [SchemaProperty()]
        [CheckedList(
            DataSourceKey = "DataSourceInt16List",
            Category = "CheckedList",
            IsVisible = true,
            IsEnabled = true,
            Index = 2,
            Localizable = true,
            Description = "CheckListInt16",
            Tooltip = "CheckListInt16ToolTips"
            )]
        public List<Int16> CheckListInt16 { get; set; }

        /// <summary>
        /// CheckedList supporting int32 values.
        /// This CheckedList is filled using "DataSourceInt32List" data source.
        /// "DataSourceInt32List" is based on int32List values {1,2,3,4}.
        /// </summary> 
        [SchemaProperty()]
        [CheckedList(
            DataSourceKey = "DataSourceInt32List",
            Category = "CheckedList",
            IsVisible = true,
            IsEnabled = true,
            Index = 2,
            Localizable = true,
            Description = "CheckListInt32",
            Tooltip = "CheckListInt32ToolTips"
            )]
        public List<Int32> CheckListInt32 { get; set; }

        /// <summary>
        /// CheckedList supporting boolean values.
        /// This CheckedList is filled using "DataSourceGuidList" data source.
        /// "DataSourceGuidList" is based on guidList values {"6AED35BD-9143-4AAB-B568-7FC69C946824"), ("F6F9D635-6AF3-4336-9D52-E734DFA9F97E"), ("E72993A5-CDFE-4501-9A34-D3A6DA407CD6") }
        /// </summary> 
        [SchemaProperty()]
        [CheckedList(
            DataSourceKey = "DataSourceGuidList",
            Category = "CheckedList",
            IsVisible = true,
            IsEnabled = true,
            Index = 2,
            Localizable = true,
            Description = "CheckListGuid",
            Tooltip = "CheckListGuidToolTips"
            )]
        public List<Guid> CheckListGuid { get; set; }

        /// <summary>
        /// Unit CheckedList supporting double values.
        /// The unit type is set to length (DUT_METERS). 
        /// Checked item values are stored in DUT_METERS.
        /// This CheckedList is filled using "DataSourceDoubleList" data source.
        /// "DataSourceDoubleList" is based on doubleList values {10.0, 20.0, 30.0, 40.0}.
        /// </summary> 
        [SchemaProperty(Unit = UnitType.UT_Length, DisplayUnit = DisplayUnitType.DUT_METERS)]
        [UnitCheckedList(
            DataSourceKey = "DataSourceDoubleList",
            Category = "CheckedList",
            IsVisible = true,
            IsEnabled = true,
            Index = 3,
            Localizable = true,
            Description = "UnitCheckListDouble",
            Tooltip = "UnitCheckListDoubleToolTips"
            )]
        public List<Double> UnitCheckListDouble { get; set; }


        # endregion checkedlist

        # region Constructors
        /// <summary>
        /// 
        /// </summary>
        public TabListSchema()
        {
            CheckListRebar = new List<RebarBarType>();
            CheckListDouble = new List<Double>();
            CheckListDouble.Add(10);  
            CheckListString = new List<String>();
            CheckListString.Add("Choice 1"); 
            UnitCheckListDouble = new List<Double>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="document"></param>
        public TabListSchema(Document document)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="document"></param>
        public TabListSchema(Entity entity, Document document)
            : base(entity, document)
        {
   
        }
        #endregion Constructors
    }
}