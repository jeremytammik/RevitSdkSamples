#region Header
//
// CmdNewLightingFixture.cs - insert new lighting fixture family instance
//
// Copyright (C) 2009-2010 by Jeremy Tammik,
// Autodesk Inc. All rights reserved.
//
#endregion // Header

#region Namespaces
using System;
using System.Collections.Generic;
using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Structural.Enums;
using Autodesk.Revit.Symbols;
using CmdResult = Autodesk.Revit.IExternalCommand.Result;
using XYZ = Autodesk.Revit.Geometry.XYZ;
#endregion // Namespaces

namespace BuildingCoder
{
  class CmdNewLightingFixture : IExternalCommand
  {
    public CmdResult Execute( 
      ExternalCommandData commandData, 
      ref string message, 
      ElementSet elements )
    {
      Application app = commandData.Application;
      Document doc = app.ActiveDocument;

      Autodesk.Revit.Creation.Filter cf
        = app.Create.Filter;

      // get a lighting fixture family symbol:

      Filter f1 = cf.NewTypeFilter( 
        typeof( FamilySymbol ) );

      Filter f2 = cf.NewCategoryFilter( 
        BuiltInCategory.OST_LightingFixtures );

      Filter f = cf.NewLogicAndFilter( f1, f2 );

      List<Element> symbols = new List<Element>();
      doc.get_Elements( f, symbols );

      FamilySymbol sym = null;

      foreach( Element elem in symbols )
      {
        sym = elem as FamilySymbol;  //get the first one.
        break;
      }

      if( null == sym )
      {
        message = "No lighting fixture symbol found.";
        return CmdResult.Failed;
      }

      // pick the ceiling:

      doc.Selection.StatusbarTip 
        = "Please select ceiling to host lighting fixture";

      doc.Selection.PickOne();

      Element ceiling = null;

      foreach( Element elem in doc.Selection.Elements )
      {
        ceiling = elem as Element;
        break;
      }

      // get the level 1:

      ElementIterator it = doc.get_Elements( 
        typeof( Level ) );

      Level level = null;

      while( it.MoveNext() )
      {
        level = it.Current as Level;

        if( level.Name.Equals( "Level 1" ) )
          break;
      }

      if( null == level )
      {
        message = "Level 1 not found.";
        return CmdResult.Failed;
      }

      // create the family instance:

      XYZ p = app.Create.NewXYZ( -43, 28, 0 );

      FamilyInstance instLight 
        = doc.Create.NewFamilyInstance( 
          p, sym, ceiling, level,
          StructuralType.NonStructural );

      return CmdResult.Succeeded;
    }
  }
}
