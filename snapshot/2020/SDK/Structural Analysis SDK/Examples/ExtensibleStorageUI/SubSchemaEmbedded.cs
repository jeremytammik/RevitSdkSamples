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
using Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes;
using Autodesk.Revit.UI.ExtensibleStorage.Framework.Attributes;
using Autodesk.Revit.DB.ExtensibleStorage.Framework;
namespace ExtensibleStorageUI
{
    [Schema("SubSchemaEmbedded", "8f253922-1dfa-4d6b-84b8-bb695b2b1780")]
    public class SubSchemaEmbedded : SchemaClass
    {
        public SubSchemaEmbedded()
        {
             
            ValueDouble = 100;
            ValueString = "A string";  
        }

        public SubSchemaEmbedded(Document document)
        {

        }

        public SubSchemaEmbedded(Entity entity, Document document)
            : base(entity, document)
        {
             
     
        }



        [SchemaProperty(Unit = UnitType.UT_Length, DisplayUnit = DisplayUnitType.DUT_METERS)]
        [UnitTextBox()]
        public Double ValueDouble { get; set; }

        [SchemaProperty]
        [TextBox()]
        public String ValueString { get; set; }
        
        [SchemaProperty(Unit = UnitType.UT_Length, DisplayUnit = DisplayUnitType.DUT_METERS)]
        [ComboBox(
            DataSourceKey = "DataSourceDoubleList"
            )]
        public Double ValueDoubleList { get; set; }

        [SchemaProperty]
        [CheckBox()]
        public Boolean CheckBoxUnchecked { get; set; }
        
        [SchemaProperty]
        [ComboBox(
            "Item1",
            "Item2",
            "Item3",
            Category = "",
            IsVisible = true,
            IsEnabled = true,
            Index = -1,
            Localizable = true
            )]
        public String ComboBoxString { get; set; }

        
    }
}
