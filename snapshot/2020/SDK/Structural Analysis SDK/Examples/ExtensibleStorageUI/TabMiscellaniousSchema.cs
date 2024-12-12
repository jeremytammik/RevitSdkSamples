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
    /// 
    [Schema("tabMiscellaniousSchema", "0934f0c3-05df-4512-9c3f-fb6637a88c65")]
    public class TabMiscellaniousSchema : SchemaClass
    {
        # region Constructors
        /// <summary>
        /// 
        /// </summary>
        public TabMiscellaniousSchema()
        {
            SubSchemaSimple = new SubSchema();
            SubSchemaEmbedded = new SubSchema();
            SubSchemaList = new List<SubSchema>();
            SubSchemaDictionary = new Dictionary<int, SubSchema>();
            SubSchemaTable = new List<SubSchema>();
            DoubleNotSerialized = 5;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="document"></param>
        public TabMiscellaniousSchema(Document document)
        {
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="entity"></param>
        ///// <param name="document"></param>
        public TabMiscellaniousSchema(Entity entity, Document document)
            : base(entity, document)
        {
        }
        #endregion Constructors
        /// <summary>
        /// SubSchema embedded on the main layout
        /// </summary>
        [SchemaProperty()]
        [SubSchemaEmbeddedControl(
            Category = "SubSchema Embedded",
            Description="SubSchemaEmbedded",
            Localizable =  true
            )]
        public SubSchema SubSchemaEmbedded { get; set; }

        /// <summary>
        /// Schema embedded on an additional dialog launch by clicking the ellipse button 
        /// </summary>
        [SchemaProperty()]
        [SubSchemaControl(
            Category = "SubSchema",
            DialogTitle = "SubSchemaSimpleDialogTitle",
            Description = "SubSchemaSimple",
            Text = "SubSchemaSimpleText",
            Tooltip = "SubSchemaSimpleToolTips"
            )]
        public SubSchema SubSchemaSimple { get; set; }

        /// <summary>
        /// List of schemas embedded on an additional dialog launch by clicking the ellipse button
        /// </summary>
        [SchemaProperty()]
        [SubSchemaControl(
            Category = "SubSchema",
            DialogTitle = "SubSchemaListDialogTitle",
            Description = "SubSchemaList",
            Text = "SubSchemaListText",
            Tooltip = "SubSchemaListToolTips"
            )]
        public List<SubSchema> SubSchemaList { get; set; }

        /// <summary>
        /// Dictionary of schemas embedded on an additional dialog launch by clicking the ellipse button
        /// </summary>
        [SchemaProperty()]
        [Autodesk.Revit.UI.ExtensibleStorage.Framework.Attributes.SubSchemaControl(
            Category = "SubSchema",
            DialogTitle = "SubSchemaDictionary",
            Description = "SubSchemaDictionary",
            Text = "SubSchemaDictionaryText",
            Tooltip = "SubSchemaDictionaryToolTips"
            )]
        public Dictionary<int, SubSchema> SubSchemaDictionary { get; set; }

        /// <summary>
        /// List of schemas embedded on a table
        /// </summary>
        [SchemaProperty()]
        [SubSchemaListTable(
            Category = "SubSchema Table",
            Description = "SubSchemaTable",
            Tooltip = "SubSchemaToolTips"
            )]
        public List<SubSchema> SubSchemaTable { get; set; }

        # region without serialization


        /// <summary>
        /// Unit TextBox supporting a double value.
        /// The unit type is set to  length (DUT_METERS). 
        /// This value is not serialized  
        /// this value is initialized via the constructor to 5 DUT_METERS in the range [4;6].
        /// </summary>
        [Unit(Unit = UnitType.UT_Length, DisplayUnit = DisplayUnitType.DUT_METERS)]
        [UnitTextBox(
            ValidateMinimumValue = true,
            ValidateMaximumValue = true,
            MinimumValue = 4,
            MaximumValue = 6,
            AttributeUnit = DisplayUnitType.DUT_METERS,
            Category = "Field Not Serialized",
            IsVisible = true,
            IsEnabled = true,
            Index = -1,
            Localizable = true,
            Description = "DoubleNotSerialized",
            Tooltip = "DoubleNotSerializedToolTips"
            )]
        public Double DoubleNotSerialized { get; set; }

        # endregion without serialization
    }
}