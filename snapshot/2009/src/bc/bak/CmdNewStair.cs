#region Header
//
// CmdNewStair.cs - create a new stair instance
//
// Copyright (C) 2009 by Jeremy Tammik,
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
using XYZ = Autodesk.Revit.Geometry.XYZ;
#endregion // Namespaces

namespace BuildingCoder
{
  class CmdNewStair : IExternalCommand
  {
    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;
      Document doc = app.ActiveDocument;
      //
      // get all stair symbols in current document:
      //
      Autodesk.Revit.Creation.Filter cf
        = app.Create.Filter;

      List<Element> symbols = new List<Element>();
      
      Filter f1 = cf.NewCategoryFilter( 
        BuiltInCategory.OST_Stairs );

      Filter f2 = cf.NewTypeFilter( 
        typeof( FamilySymbol ) );

      Filter f = cf.NewLogicAndFilter( f1, f2 );
      
      doc.get_Elements( f, symbols );
      
      if( 0 == symbols.Count )
      {
        message = "No stair symbols defined";
      }

      // no overload available for specifying a sketch:
      //doc.Create.NewFamilyInstance()

      return CmdResult.Failed;
    }
  }
}
