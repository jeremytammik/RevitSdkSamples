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
using Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation;
using Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.Attributes;


using Document = Autodesk.Revit.DB.Document;

namespace ExtensibleStorageDocumentation
{
    [Schema("SchemaClassDocumentation", "b22a7a96-928b-4ffd-819e-503e275101bd")]
    public class DocumentationSchema : SchemaClass
    {
        public int[] Columns = new[] {20, 30, 40};

        public DocumentationSchema()
        {
            Value = "Value";
            ValueWithName = "ValueWithName";
            ValueWithDescription = "ValueWithDescription";
            ValueString = "my simple string";
            ValueDouble = 10;
            Ratio1 = 0.9;
            Ratio2 = 1;
            Test = "remove canvas";
            LenghtMeter = 10;
            Table = new List<string>
                        {"Table 1", "Table 2"};

            ListDoubles = new List<double> {5, 10, 11, 12};
            TestDictionnary = new Dictionary<double, double>
                                  {{1, 1}, {2, 2}, {3, 3}, {4, 4}};
            SubLabel = new List<SimpleSchema>
                           {new SimpleSchema(), new SimpleSchema(), new SimpleSchema(), new SimpleSchema()};
            ListSimpleSchemas = new List<SimpleSchema>
                                    {new SimpleSchema(), new SimpleSchema(), new SimpleSchema(), new SimpleSchema(), new SimpleSchema()};
            TestDictionnary2 = new Dictionary<double, SimpleSchema>
                                   {{1, new SimpleSchema()}, {2, new SimpleSchema()}, {3, new SimpleSchema()}, {4, new SimpleSchema()}};
        }

        public DocumentationSchema(Autodesk.Revit.DB.Document document)
        {
            Value = "Value";
            ValueWithName = "ValueWithName";
            ValueWithDescription = "ValueWithDescription";
            Table = new List<string> {"Table 1", "Table 2"};
        }

        public DocumentationSchema(Entity entity, Autodesk.Revit.DB.Document document)
            : base(entity, document)
        {
            Value = "Value";
            ValueWithName = "ValueWithName";
            ValueWithDescription = "ValueWithDescription";
            Table = new List<string> {"Table 1", "Table 2"};
        }

        [SchemaProperty(FieldName = "SimpleDoubleFieldName", Unit = UnitType.UT_Length, DisplayUnit = DisplayUnitType.DUT_METERS)]
        [Value(
            LocalizableValue = false,
            Level = DetailLevel.General,
            Localizable = false,
            Index = -1
            )]
        public Double ValueDouble { get; set; }

        [SchemaProperty(FieldName = "SimpleStringFieldName")]
        [Value(
            LocalizableValue = false,
            Level = DetailLevel.General,
            Localizable = false,
            Index = -1
            )]
        public String ValueString { get; set; }

        [SchemaProperty(Unit = UnitType.UT_Length, DisplayUnit = DisplayUnitType.DUT_METERS)]
        [Table(
            Name = "ADictionnary",
            LocalizableValue = false,
            Level = DetailLevel.General,
            Localizable = false,
            Index = -1
            )]
        public Dictionary<Double, Double> TestDictionnary { get; set; }

        [SchemaProperty(Unit = UnitType.UT_Length, DisplayUnit = DisplayUnitType.DUT_METERS)]
        [Table(
            Name = "ADictionnary2",
            LocalizableValue = false,
            Level = DetailLevel.General,
            Localizable = false,
            Index = -1
            )]
        public Dictionary<Double, SimpleSchema> TestDictionnary2 { get; set; }


        [SchemaProperty(Unit = UnitType.UT_Length, DisplayUnit = DisplayUnitType.DUT_METERS)]
        [Table(
            Name = "ListDoublesName",
            LocalizableValue = false,
            Level = DetailLevel.General,
            Localizable = false,
            Index = -1
            )]
        public List<Double> ListDoubles { get; set; }

        [SchemaProperty(Unit = UnitType.UT_Length, DisplayUnit = DisplayUnitType.DUT_METERS)]
        [Table(
            Name = "ListSimpleSchemas",
            LocalizableValue = false,
            Level = DetailLevel.General,
            Localizable = false,
            Index = -1
            )]
        public List<SimpleSchema> ListSimpleSchemas { get; set; }

        [SchemaProperty(Unit = UnitType.UT_Length, DisplayUnit = DisplayUnitType.DUT_METERS)]
        [Value(
            LocalizableValue = false,
            Level = DetailLevel.General,
            Localizable = false,
            Index = 5
            )]
        public Double LenghtMeter { get; set; }


        [SchemaProperty]
        [Value(
            LocalizableValue = true,
            Level = DetailLevel.General,
            Localizable = true,
            Index = 1
            )]
        public String Value { get; set; }


        [SchemaProperty]
        [ValueWithName(
            LocalizableValue = true,
            Level = DetailLevel.General,
            Localizable = true,
            Name = "ValueWithName",
            Index = 3
            )]
        public String ValueWithName { get; set; }

        [SchemaProperty]
        [ValueWithDescription(
            LocalizableValue = true,
            Level = DetailLevel.General,
            Localizable = true,
            Name = "ValueWithName",
            Description = "ValueWithNameDescription",
            Note = "ValueWithNameNote",
            Index = 6
            )]
        public String ValueWithDescription { get; set; }


        [SchemaProperty]
        [Table(
            Level = DetailLevel.General,
            Index = 2,
            Localizable = false,
            Name = "TableName"
            )]
        public List<String> Table { get; set; }


        [SchemaProperty]
        [Value(
            LocalizableValue = true,
            Level = DetailLevel.General,
            Localizable = true,
            Index = 4
            )]
        public String Test { get; set; }

        [SubSchemaListTable(
            "ListTable", 10, 30, 60,
            AddHeader = true,
            Level = DetailLevel.Detail,
            Name = "ListTableName",
            Localizable = true
            )]
        [SchemaProperty]
        public List<SimpleSchema> SubLabel { get; set; }


        [SchemaProperty(Unit = UnitType.UT_Length, DisplayUnit = DisplayUnitType.DUT_METERS)]
        [Ratio(
            
            Level = DetailLevel.General,
            Localizable = false,
            Index = -1,
            Name = "Ratio1Name"
            )]
        public Double Ratio1 { get; set; }


        [SchemaProperty(Unit = UnitType.UT_Length, DisplayUnit = DisplayUnitType.DUT_METERS)]
        [Ratio(
     
            Level = DetailLevel.General,
            Localizable = false,
            Index = -1,
            Name = "Ratio2Name"
            )]
        public Double Ratio2 { get; set; }
    }
}