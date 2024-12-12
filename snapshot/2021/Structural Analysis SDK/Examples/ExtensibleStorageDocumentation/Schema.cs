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
    [Schema("Schema", "b22a7a96-928b-4ffd-819e-503e275101bd")]
    public class Schema : SchemaClass
    {
 
        public Schema()
        {
            stringValue = "A string value ";
            stringWithNameValue = "A string value with name";
            stringWithDescriptionValue = "A string value with description";
            stringValue = "A string value ";
            doubleValue = Math.PI;
            ratioValue = 0.9;
            ratioWithNameValue = Math.Sqrt(2)  ;
            stringListValue = new List<string> { "A first string", "A second string" };
            doubleListValue = new List<double> { 5.0, 10.0, 11.0, 12.0 };
            doubleDoubledictionaryValue = new Dictionary<double, double> { { 1.0, 1.0 }, { 2.0, 2.0 }, { 3.0, 3.0 }, { 4.0, 4.0 } };
            subLabelListValue = new List<SubSchema>{new SubSchema(), new SubSchema(), new SubSchema(), new SubSchema()};
            subSchemaListValue = new List<SubSchema> {new SubSchema(), new SubSchema(), new SubSchema(), new SubSchema(), new SubSchema()};
            doubleSubSchemaDictionaryValue = new Dictionary<double, SubSchema> { { 1.0, new SubSchema() }, { 2.0, new SubSchema() }, { 3.0, new SubSchema() }, { 4.0, new SubSchema() } };
        }

        public Schema(Autodesk.Revit.DB.Document document)
        {
        }

        public Schema(Entity entity, Autodesk.Revit.DB.Document document)
            : base(entity, document)
        {
        }
        
        # region base type
        /// <summary>
        /// A double value
        /// </summary>
        [Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes.SchemaProperty(
            Unit = UnitType.UT_Length,
            DisplayUnit = DisplayUnitType.DUT_METERS)]
        [Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.Attributes.Value(
            LocalizableValue = false,
            Level = DetailLevel.General,
            Localizable = false,
            Index = 1)]
        public Double doubleValue { get; set; }

        /// <summary>
        /// A string value
        /// </summary>
        [Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes.SchemaProperty()]
        [Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.Attributes.Value(
            LocalizableValue = false,
            Level = DetailLevel.General,
            Localizable = false,
            Index = 2
            )]
        public String stringValue { get; set; }

        /// <summary>
        /// A string value with name set
        /// </summary>
        [Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes.SchemaProperty]
        [Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.Attributes.ValueWithName(
            LocalizableValue = true,
            Level = DetailLevel.General,
            Localizable = true,
            Name = "Name",
            Index = 3
            )]
        public String stringWithNameValue { get; set; }

        /// <summary>
        /// A string value with name, description and note set.
        /// </summary>
        [Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes.SchemaProperty]
        [Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.Attributes.ValueWithDescription(
            LocalizableValue = true,
            Level = DetailLevel.General,
            Localizable = true,
            Name = "Name",
            Description = "Description",
            Note = "Note",
            Index = 4
            )]
        public String stringWithDescriptionValue { get; set; }
        # endregion base type

        # region ratio
      
        /// <summary>
        /// A ratio
        /// </summary>
        [Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes.SchemaProperty(
            Unit = UnitType.UT_Length,
            DisplayUnit = DisplayUnitType.DUT_METERS)]
        [Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.Attributes.Ratio(
            Level = DetailLevel.General,
            Localizable = false,
            Index = 5
            )]
        public Double ratioValue { get; set; }

        /// <summary>
        /// A ratio with name
        /// </summary>
        [Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes.SchemaProperty(
            Unit = UnitType.UT_Length,
            DisplayUnit = DisplayUnitType.DUT_METERS)]
        [Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.Attributes.Ratio(
            Level = DetailLevel.General,
            Localizable = false,
            Index = 6,
            Name = "Ratio"
            )]
        public Double ratioWithNameValue { get; set; }
        # endregion ratio

        # region list

        /// <summary>
        /// A list of string 
        /// </summary>
        [Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes.SchemaProperty]
        [Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.Attributes.Table(
            Level = DetailLevel.General,
            Index = 7,
            Localizable = false,
            Name = "Title of a table containing string values"
            )]
        public List<String> stringListValue { get; set; }

        /// <summary>
        /// A list of double 
        /// </summary>
        [Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes.SchemaProperty(
            Unit = UnitType.UT_Length,
            DisplayUnit = DisplayUnitType.DUT_METERS)]
        [Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.Attributes.Table(
            Name = "Title of a table containing double values",
            LocalizableValue = false,
            Level = DetailLevel.General,
            Localizable = false,
            Index = 8
            )]
        public List<Double> doubleListValue { get; set; }

        /// <summary>
        /// A list of subSchemas
        /// </summary>
        [Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes.SchemaProperty(
            Unit = UnitType.UT_Length,
            DisplayUnit = DisplayUnitType.DUT_METERS)]
        [Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.Attributes.Table(
            Name = "Title of a table containing subschemas",
            LocalizableValue = false,
            Level = DetailLevel.General,
            Localizable = false,
            Index = 9
            )]
        public List<SubSchema> subSchemaListValue { get; set; }

        /// <summary>
        /// List  of subSchemas
        /// The size of columns is set to 10%,10% and 20 %
        /// </summary>
        [Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes.SchemaProperty]
        [Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.Attributes.SubSchemaListTable(
            "ListTable", 10, 10, 20,
            AddHeader = true,
            Level = DetailLevel.Detail,
            Name = "Title of a table containing subschemas",
            Localizable = true,
            Index =10
            )]
        public List<SubSchema> subLabelListValue { get; set; }
        # endregion list
        
        # region dictionary 
        /// <summary>
        /// A dictionary of double 
        /// </summary>
        [Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes.SchemaProperty(
            Unit = UnitType.UT_Length,
            DisplayUnit = DisplayUnitType.DUT_METERS)]
        [Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.Attributes.Table( 
            Name = "Title of a table containing dictionary of double",
            LocalizableValue = false,
            Level = DetailLevel.General,
            Localizable = false,
            Index = 12
            )]
        public Dictionary<Double, Double> doubleDoubledictionaryValue { get; set; }

        /// <summary>
        /// A dictionnary of SubSchema (see SubSchema.cs)     
        /// </summary>
        [Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes.SchemaProperty(
            Unit = UnitType.UT_Length,
            DisplayUnit = DisplayUnitType.DUT_METERS)]
        [Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.Attributes.Table(
            Name = "Title of a table containing dictionary of subSchema",
            LocalizableValue = false,
            Level = DetailLevel.General,
            Localizable = false,
            Index = 13
            )]
        public Dictionary<Double, SubSchema> doubleSubSchemaDictionaryValue { get; set; }
        # endregion dictionary

    }
}