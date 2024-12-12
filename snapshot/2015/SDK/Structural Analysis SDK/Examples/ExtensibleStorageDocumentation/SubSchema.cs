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

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.DB.ExtensibleStorage.Framework;
using Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes ;
using Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation  ;
using Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.Attributes;
namespace ExtensibleStorageDocumentation
{
    /// <summary>
    /// A simple SubSchema Class that contains 3 double values.
    /// </summary>
    [Schema("SimpleSchema", "eaec17a1-e7dd-4e46-9eac-2646d95d334c")]
    public class SubSchema : SchemaClass
    {
        public SubSchema()
        {
            subSchemaFirstValue = 1;
            subSchemaSecondValue = 2;
            subSchemaThirdValue = 3;
        }

        public SubSchema(Autodesk.Revit.DB.Document document)
        {
            subSchemaFirstValue = 1;
            subSchemaSecondValue = 2;
            subSchemaThirdValue = 3;
        }

        public SubSchema(Entity entity, Autodesk.Revit.DB.Document document)
            : base(entity, document)
        {
            subSchemaFirstValue = 1;
            subSchemaSecondValue = 2;
            subSchemaThirdValue = 3;
        }

        [Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.Attributes.DefaultElement]
        [Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes.SchemaProperty(UnitType.UT_Length, DisplayUnitType.DUT_METERS)]
        public double subSchemaFirstValue { get; set; }


        [Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.Attributes.DefaultElement]
        [Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes.SchemaProperty(UnitType.UT_Length, DisplayUnitType.DUT_METERS)]
        public double subSchemaSecondValue { get; set; }


        [Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.Attributes.DefaultElement]
        [Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes.SchemaProperty(UnitType.UT_Length, DisplayUnitType.DUT_METERS)]
        public double subSchemaThirdValue { get; set; }
    }
}