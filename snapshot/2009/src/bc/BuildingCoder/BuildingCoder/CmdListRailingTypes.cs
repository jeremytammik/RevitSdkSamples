#region Header
//
// CmdListRailings.cs - list all railing types,
// in response to queries from Berria at
// http://thebuildingcoder.typepad.com/blog/2009/02/inserting-a-column.html#comments
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
//using Autodesk.Revit.Enums;
//using Autodesk.Revit.Parameters;
using Autodesk.Revit.Symbols;
using CmdResult
  = Autodesk.Revit.IExternalCommand.Result;
using CreationFilter
  = Autodesk.Revit.Creation.Filter;
#endregion // Namespaces

namespace BuildingCoder
{
  class CmdListRailingTypes : IExternalCommand
  {
    /*
    public IExternalCommand.Result Execute2(
      ExternalCommandData commandData,
      ref string messages,
      ElementSet elements )
    {
      Application app = commandData.Application;
      Document doc = app.ActiveDocument;
      CreationFilter cf = app.Create.Filter;

      Filter f1 = cf.NewParameterFilter( 
        BuiltInParameter.DESIGN_OPTION_PARAM, 
        CriteriaFilterType.Equal, "Main Model" );

      Filter f2 = cf.NewTypeFilter( typeof( Wall ) );
      Filter f = cf.NewLogicAndFilter( f1, f2 );

      List<Element> a = new List<Element>();

      doc.get_Elements( f, a );

      Util.InfoMsg( "There are " 
        + a.Count.ToString() 
        + " main model wall elements" );

      return IExternalCommand.Result.Succeeded;
    }
    */

    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;
      Document doc = app.ActiveDocument;
      CreationFilter cf = app.Create.Filter;

      List<Element> symbols = new List<Element>();

      // this returns zero symbols:
      //BuiltInCategory bic = BuiltInCategory.OST_StairsRailing;
      //Filter f1 = cf.NewCategoryFilter( bic );
      //Filter f2 = cf.NewTypeFilter( typeof( FamilySymbol ) );
      //Filter f = cf.NewLogicAndFilter( f1, f2 );

      // this returns zero families:
      // we already know why, because family does not implement the Category property, c.f.
      // http://thebuildingcoder.typepad.com/blog/2009/01/family-category-and-filtering.html
      //Filter f1 = cf.NewCategoryFilter( bic );
      //Filter f2 = cf.NewTypeFilter( typeof( Family ) );
      //Filter f = cf.NewLogicAndFilter( f1, f2 );

      BuiltInCategory bic 
        = BuiltInCategory.OST_StairsRailingBaluster;

      Filter f1 
        = cf.NewCategoryFilter( bic );

      Filter f2 
        = cf.NewTypeFilter( typeof( FamilySymbol ) );

      Filter f 
        = cf.NewLogicAndFilter( f1, f2 );

      doc.get_Elements( f, symbols );

      int n = symbols.Count;

      Debug.Print( "\n{0}"
        + " OST_StairsRailingBaluster"
        + " family symbol{1}:",
        n, Util.PluralSuffix( n ) );

      foreach( FamilySymbol s in symbols )
      {
        Debug.Print(
          "Family name={0}, symbol name={1}",
          s.Family.Name, s.Name );
      }

      bic = BuiltInCategory.OST_StairsRailing;
      f1 = cf.NewCategoryFilter( bic );
      f2 = cf.NewTypeFilter( typeof( Symbol ) );
      f = cf.NewLogicAndFilter( f1, f2 );

      doc.get_Elements( f, symbols );

      n = symbols.Count;

      Debug.Print( "\n{0}"
        + " OST_StairsRailing symbol{1}:",
        n, Util.PluralSuffix( n ) );

      foreach( Symbol s in symbols )
      {
        FamilySymbol fs = s as FamilySymbol;

        Debug.Print(
          "Family name={0}, symbol name={1}",
          null == fs ? "<none>" : fs.Family.Name, 
          s.Name );
      }
      return CmdResult.Failed;
    }
  }
}
