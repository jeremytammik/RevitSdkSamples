#region Header
//
// CmdNewRailing.cs - insert a new railing instance,
// in response to queries from Berria at
// http://thebuildingcoder.typepad.com/blog/2009/02/list-railing-types.html#comments
//
// Copyright (C) 2009-2010 by Jeremy Tammik,
// Autodesk Inc. All rights reserved.
//
#endregion // Header

#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Structural.Enums;
using Autodesk.Revit.Symbols;
using CmdResult
  = Autodesk.Revit.IExternalCommand.Result;
using CreationFilter
  = Autodesk.Revit.Creation.Filter;
using XYZ = Autodesk.Revit.Geometry.XYZ;
using Line = Autodesk.Revit.Geometry.Line;
#endregion // Namespaces

namespace BuildingCoder
{
  /// <summary>
  /// Currently, it is not possible to create a new railing instance:
  /// http://thebuildingcoder.typepad.com/blog/2009/02/list-railing-types.html#comments
  /// SPR #134260 [API - New Element Creation: Railing]
  /// </summary>
  class CmdNewRailing : IExternalCommand
  {
    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;
      Document doc = app.ActiveDocument;
      CreationFilter cf = app.Create.Filter;
      List<Element> els = new List<Element>();
      //
      // get first level:
      //
      Filter f = cf.NewTypeFilter( typeof( Level ) );
      doc.get_Elements( f, els );
      if( 0 == els.Count )
      {
        message = "No level found.";
        return CmdResult.Failed;
      }
      Level level = els[0] as Level;
      els.Clear();
      //
      // get symbol to use:
      //
      BuiltInCategory bic;
      Type t;
      //
      // this retrieves the railing baluster symbols
      // but they cannot be used to create a railing:
      //
      bic = BuiltInCategory.OST_StairsRailingBaluster;
      t = typeof( FamilySymbol );
      //
      // this retrieves all railing symbols,
      // but they are just Symbol instances,
      // not FamilySymbol ones:
      //
      bic = BuiltInCategory.OST_StairsRailing;
      t = typeof( Symbol );

      Filter f1 = cf.NewCategoryFilter( bic );
      Filter f2 = cf.NewTypeFilter( t );
      f = cf.NewLogicAndFilter( f1, f2 );
      doc.get_Elements( f, els );

      FamilySymbol sym = null;

      foreach( Symbol s in els )
      {
        FamilySymbol fs = s as FamilySymbol;

        Debug.Print(
          "Family name={0}, symbol name={1},"
          + " category={2}",
          null == fs ? "<none>" : fs.Family.Name,
          s.Name,
          s.Category.Name );

        if( null == sym && s is Symbol )
        {
          // this does not work, of course:
          sym = s as FamilySymbol;
        }
      }
      if( null == sym )
      {
        message = "No railing family symbols found.";
        return CmdResult.Failed;
      }
      XYZ p1 = new XYZ( 17, 0, 0 );
      XYZ p2 = new XYZ( 33, 0, 0 );
      Line line = app.Create.NewLineBound( p1, p2 );
      // we need a FamilySymbol instance here, but only have a Symbol:
      FamilyInstance Railing1 = doc.Create.NewFamilyInstance( line, sym, level, StructuralType.NonStructural );
      return CmdResult.Succeeded;
    }
  }
}
