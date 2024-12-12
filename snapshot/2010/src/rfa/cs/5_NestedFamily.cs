#region Copyright
//
// (C) Copyright 2009 by Autodesk, Inc.
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
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
//
#endregion // Copyright

#region Namespaces
using System;
using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Enums;
using Autodesk.Revit.Geometry;
using Autodesk.Revit.Parameters;
using Autodesk.Revit.Structural.Enums;
using Autodesk.Revit.Symbols;
using RvtElement = Autodesk.Revit.Element;
using GeoElement = Autodesk.Revit.Geometry.Element;
#endregion // Namespaces

#region Description
/// <summary>
/// Revit Family Creation API Lab - 5
///
/// Create a nested family.
/// </summary>
#endregion // Description

namespace LabsCs
{
  class RvtCmd_FamilyCreateNestedFamily : IExternalCommand
  {
    StructuralType _non_rst 
      = StructuralType.NonStructural;

    FamilyInstance InsertFamilySymbolFromRfa( 
      string filename, 
      Document doc )
    {
      FamilyInstance fi = null;
      //
      // load family file:
      //
      Family f;
      if( doc.LoadFamily( filename, out f ) )
      {
        //
        // retrieve first family symbol:
        //
        FamilySymbol symbol = null;
        foreach( FamilySymbol s in f.Symbols )
        {
          symbol = s;
          break;
        }
        //
        // create family instance:
        //
        if( null != symbol )
        {
          if( doc.IsFamilyDocument )
          {
            fi = doc.FamilyCreate.NewFamilyInstance(
              XYZ.Zero, symbol, _non_rst );
          }
          else
          {
            fi = doc.Create.NewFamilyInstance(
              XYZ.Zero, symbol, _non_rst );
          }
        }
      }
      return fi;
    }

    public IExternalCommand.Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;
      Document doc = app.ActiveDocument;

      FamilyInstance a = InsertFamilySymbolFromRfa( 
        "C:/tmp/column.rfa", doc );

      FamilyInstance b = InsertFamilySymbolFromRfa( 
        "C:/tmp/shelf.rfa", doc );

      /*
      Parameter p = (null == a) ? null : a.get_Parameter( 
        BuiltInParameter.FAMILY_TOP_LEVEL_OFFSET_PARAM );

      if( null != p )
      {
        p.Set( 1000.0 ); // InvalidOperationException "Operation is not valid due to the current state of the object."
      }
      */

      return IExternalCommand.Result.Succeeded;
    }
  }
}
