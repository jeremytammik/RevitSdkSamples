#region Header
//
// CmdFilterPerformance.cs
//
// Compare TypeFilter versus using an
// anonymous method to filter elements.
// By Guy Robinson, info@r-e-d.co.nz.
//
// Copyright (C) 2008-2010 by Jeremy Tammik,
// Autodesk Inc. All rights reserved.
//
#endregion // Header

#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Parameters;
using Autodesk.Revit.Symbols;
using CmdResult
  = Autodesk.Revit.IExternalCommand.Result;
#endregion // Namespaces

namespace BuildingCoder
{
  class CmdFilterPerformance : IExternalCommand
  {
    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      try
      {
        Application app = commandData.Application;
        Document doc = app.ActiveDocument;

        Stopwatch sw = Stopwatch.StartNew();

        // f5 = f1 && f4
        //    = f1 && (f2 || f3)
        //    = family instance and (door or window)

        Autodesk.Revit.Creation.Filter cf
          = app.Create.Filter;

        //Filter f1 = cf.NewTypeFilter(
        //  typeof( FamilyInstance ) );

        Filter f2 = cf.NewCategoryFilter(
          BuiltInCategory.OST_Doors );
        Filter f3 = cf.NewCategoryFilter(
          BuiltInCategory.OST_Windows );

        Filter f4 = cf.NewLogicOrFilter( f2, f3 );
        //Filter f5 = cf.NewLogicAndFilter( f1, f4 );

        //int n = doc.get_Elements( f5, openings );

        List<Element> openings = new List<Element>();
        doc.get_Elements( f4, openings );

        List<Element> openingInstances
          = openings.FindAll( delegate( Element element )
            {
              return !( element is Symbol );
            } );
        int n = openingInstances.Count;

        sw.Stop();

        Debug.Print(
          "Time to get {0} elements {1}",
          n, sw.ElapsedMilliseconds );

        return CmdResult.Succeeded;
      }
      catch( Exception ex )
      {
        message = ex.Message + ex.StackTrace;
        return CmdResult.Failed;
      }
    }

    #region Code from Guy in response to Ralf
    void CodeFromGuy1()
    {
      Document doc = null;
      Autodesk.Revit.Creation.Filter cf = null;

      Filter f1 = cf.NewCategoryFilter(
        BuiltInCategory.OST_Doors );

      Filter f2 = cf.NewCategoryFilter(
        BuiltInCategory.OST_Windows );

      Filter f3 = cf.NewLogicOrFilter( f1, f2 );

      ElementIterator itor = doc.get_Elements( f3 );
      int n = 0;
      while( itor.MoveNext() )
      {
        if( itor.Current is FamilyInstance )
        {
          n++;
          FamilyInstance elem = itor.Current
            as FamilyInstance;
        }
      }
    }

    string ElementDescription( Element e )
    {
      return null;
    }

    string ElementDescription( ElementId id )
    {
      return null;
    }

    private Dictionary<ElementId, List<ElementId>>
      getElementIds( ElementIterator itor)
    {
      Dictionary<ElementId, List<ElementId>> dict
        = new Dictionary<ElementId, List<ElementId>>();

      string fmt = "{0} is hosted by {1}";

      while( itor.MoveNext() )
      {
        object elem = itor.Current;
        if( elem is FamilyInstance )
        {
          FamilyInstance fi = elem as FamilyInstance;
          ElementId id = fi.Id;

          ElementId idHost = fi.Host.Id;

          Debug.Print( fmt,
            ElementDescription( fi ),
            ElementDescription( idHost ) );

          if( !dict.ContainsKey( idHost ) )
          {
            dict.Add( idHost, new List<ElementId>() );
          }
          dict[idHost].Add( id );
        }
      }
      return dict;
    }
    #endregion // Code from Guy in response to Ralf

    #region Sample code for 114_filter_for_family.htm
    void test1()
    {
      Application app = null;
      Document doc = null;

      Autodesk.Revit.Creation.Filter cf
        = app.Create.Filter;

      Filter f1 = cf.NewTypeFilter(
        typeof( Family ) );

      Filter f2 = cf.NewCategoryFilter(
        BuiltInCategory.OST_LightingFixtures );

      LogicAndFilter f3 = cf.NewLogicAndFilter(
        f1, f2 );

      List<Element> a = new List<Element>();

      doc.get_Elements( f3, a );

      foreach( Family f in a )
      {
        Util.InfoMsg( f.Name );
      }
    }
    #endregion // Sample code for 114_filter_for_family.htm
  }
}
