#region Header
// Revit API .NET Labs
//
// Copyright (C) 2007-2008 by Autodesk, Inc.
//
// Permission to use, copy, modify, and distribute this software
// for any purpose and without fee is hereby granted, provided
// that the above copyright notice appears in all copies and
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
#endregion // Header

#region Namespaces
using System;
using System.Collections;
using System.Collections.Generic;
using Autodesk;
using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Symbols;
using Autodesk.Revit.Parameters;
using Autodesk.Revit.Structural;
using Autodesk.Revit.Utility;
#endregion // Namespaces

namespace RstLabs
{
  /// <summary>
  /// Revit Structure utilities.
  /// </summary>
  static class RstUtils
  {
    #region Structural Element Description
    public static string StructuralElementDescription( FamilyInstance e )
    {
      BuiltInCategory bic = BuiltInCategory.OST_StructuralFraming;
      Category cat = e.Document.Settings.Categories.get_Item( bic );
      bool hasCat = ( null != e.Category );
      bool hasUsage = hasCat && e.Category.Id.Equals( cat.Id );
      return e.Name
        + " Id=" + e.Id.Value.ToString()
        + ( hasCat ? ", Category=" + e.Category.Name : string.Empty )
        + ", Type=" + e.Symbol.Name
        + ( hasUsage ? ", Struct.Usage=" + e.StructuralUsage.ToString() : string.Empty ) // can throw exception "only Beam and Brace support Structural Usage!"
        + ", Struct.Type=" + e.StructuralType.ToString()
        + ", Analytical Type=" + e.AnalyticalModel.GetType().Name;
    }

    public static string StructuralElementDescription( ContFooting e )
    {
      return e.Name
        + " Id=" + e.Id.Value.ToString()
        + ", Category=" + e.Category.Name
        + ", Type=" + e.FootingType.Name
        + ", Analytical Type=" + e.AnalyticalModel.GetType().Name;
    }

    public static string StructuralElementDescription( Floor e )
    {
      return e.Name
        + " Id=" + e.Id.Value.ToString()
        + ", Category=" + e.Category.Name
        + ", Type=" + e.FloorType.Name
        + ", Struct.Usage=" + e.StructuralUsage.ToString() // can throw exception "only Beam and Brace support Structural Usage!"
        + ", Analytical Type=" + e.AnalyticalModel.GetType().Name;
    }

    public static string StructuralElementDescription( Wall e )
    {
      return e.Name
        + " Id=" + e.Id.Value.ToString()
        + ", Category=" + e.Category.Name
        + ", Type=" + e.WallType.Name
        + ", Struct.Usage=" + e.StructuralUsage.ToString() // can throw exception "only Beam and Brace support Structural Usage!"
        + ", Analytical Type=" + e.AnalyticalModel.GetType().Name;
    }
    #endregion // Structural Element Description

    #region Retrieve specific element collections
    /// <summary>
    /// Return all LoadNature instances in the current active document.
    /// </summary>
    public static List<Element> GetAllLoadNatures( Application app )
    {
      // The following commented code is for Revit 2008 and previous version.
      // It works in Revit 2009 too. However this method has a low performance.
      // There is a new feature named Element filter in Revit 2009 API,
      // which can improve the performance.
      /*
      ElementSet natures = app.Create.NewElementSet();
      IEnumerator iter = app.ActiveDocument.Elements;
      while (iter.MoveNext())
      {
        Autodesk.Revit.Element elem = iter.Current as Autodesk.Revit.Element;
        if (elem is LoadNature)
        {
          natures.Insert(elem);
        }
      }
      return natures;
      */
      List<Element> a = new List<Element>();
      Filter filterType = app.Create.Filter.NewTypeFilter( typeof( LoadNature ) );
      app.ActiveDocument.get_Elements( filterType, a );
      return a;
    }

    /// <summary>
    /// Return all LoadCase instances in the current active document.
    /// </summary>
    public static List<Element> GetAllLoadCases( Application app )
    {
      // The following commented code is for Revit 2008 and previous version.
      // It works in Revit 2009 too. However this method has a low performance.
      // There is a new feature named Element filter in Revit 2009 API, 
      // which can improve the performance.
      /*
      ElementSet cases = app.Create.NewElementSet();
      IEnumerator iter = app.ActiveDocument.Elements;
      while (iter.MoveNext())
      {
        Autodesk.Revit.Element elem = iter.Current as Autodesk.Revit.Element;
        if (elem is LoadCase)
        {
          cases.Insert(elem);
        }
      }
      return cases;
      */
      List<Element> a = new List<Element>();
      Filter filterType = app.Create.Filter.NewTypeFilter( typeof( LoadCase ) );
      app.ActiveDocument.get_Elements( filterType, a );
      return a;
    }

    /// <summary>
    /// Return all LoadCombination instances in the current active document.
    /// </summary>
    public static List<Element> GetAllLoadCombinations( Application app )
    {
      // The following commented code is for Revit 2008 and previous version.
      // It works in Revit 2009 too. However this method has a low performance.
      // There is a new feature named Element filter in Revit 2009 API,
      // which can improve the performance.
      /*
      ElementSet combinations = app.Create.NewElementSet();
      IEnumerator iter = app.ActiveDocument.Elements;
      while (iter.MoveNext())
      {
        Autodesk.Revit.Element elem = iter.Current as Autodesk.Revit.Element;
        if (elem is LoadCombination)
        {
          combinations.Insert(elem);
        }
      }
      return combinations;
      */
      List<Element> a = new List<Element>();
      Filter filterType = app.Create.Filter.NewTypeFilter( typeof( LoadCombination ) );
      app.ActiveDocument.get_Elements( filterType, a );
      return a;
    }

    /// <summary>
    /// Return all LoadUsage instances in the current active document.
    /// </summary>
    public static List<Element> GetAllLoadUsages( Application app )
    {
      // The following commented code is for Revit 2008 and previous version.
      // It works in Revit 2009 too. However this method has a low performance.
      // There is a new feature named Element filter in Revit 2009 API,
      // which can improve the performance.
      /*
      ElementSet usages = app.Create.NewElementSet();
      IEnumerator iter = app.ActiveDocument.Elements;
      while (iter.MoveNext())
      {
        Autodesk.Revit.Element elem = iter.Current as Autodesk.Revit.Element;
        if (elem is LoadUsage)
        {
          usages.Insert(elem);
        }
      }
      return usages;
      */
      List<Element> a = new List<Element>();
      Filter filterType = app.Create.Filter.NewTypeFilter( typeof( LoadUsage ) );
      app.ActiveDocument.get_Elements(filterType, a);
      return a;
    }

    /// <summary>
    /// Return all loads in the current active document, 
    /// i.e. any objects derived from LoadBase.
    /// </summary>
    public static List<Element> GetAllLoads( Application app )
    {
      // The following commented code is for Revit 2008 and previous version.
      // It works in Revit 2009 too. However this method has a low performance.
      // There is a new feature named Element filter in Revit 2009 API, 
      // which can improve the performance.
      /*
      ElementSet loads = app.Create.NewElementSet();
      IEnumerator iter = app.ActiveDocument.Elements;
      while (iter.MoveNext())
      {
        Autodesk.Revit.Element elem = iter.Current as Autodesk.Revit.Element;
        if (elem is LoadBase)
        {
          loads.Insert(elem);
        }
      }
      return loads;
      */
      List<Element> a = new List<Element>();
      Filter filterType = app.Create.Filter.NewTypeFilter( typeof( LoadBase ), true );
      app.ActiveDocument.get_Elements( filterType, a );
      return a;
    }

    /// <summary>
    /// Return all loads in the current active document, 
    /// i.e. any objects derived from LoadBase, sorted by load type.
    /// </summary>
    public static void GetAllSpecificLoads(
      Application app, 
      ref ElementSet pointLoads,
      ref ElementSet lineLoads, 
      ref ElementSet areaLoads )
    {
      // More efficient if we loop only once and sort all in one go.
      // This was more important in 2008 and earlier, before the filtering
      // feature was introduced in Revit 2009.
      /*
      IEnumerator iter = app.ActiveDocument.Elements;
      while( iter.MoveNext() )
      {
        Autodesk.Revit.Element elem = iter.Current as Autodesk.Revit.Element;
        if( elem is PointLoad )
        {
          pointLoads.Insert( elem );
        }
        else if( elem is LineLoad )
        {
          lineLoads.Insert( elem );
        }
        else if( elem is AreaLoad )
        {
          areaLoads.Insert( elem );
        }
      }
      */
      List<Element> a = new List<Element>();
      Filter filterTypeLoad = app.Create.Filter.NewTypeFilter( typeof( LoadBase ), true );
      app.ActiveDocument.get_Elements( filterTypeLoad, a );
      foreach( Element elem in a )
      {
        if( elem is PointLoad )
        {
          pointLoads.Insert( elem );
        }
        else if( elem is LineLoad )
        {
          lineLoads.Insert( elem );
        }
        else if( elem is AreaLoad )
        {
          areaLoads.Insert( elem );
        }
      }
    }

    /// <summary>
    /// Return all load symbols in the current active document, 
    /// i.e. all objects derived from LoadTypeBase.
    /// </summary>
    public static List<Element> GetAllLoadSymbols( Application app )
    {
      // Revit 2009 added new classes to represent Load symbols,
      // such as LoadTypeBase, PointLoadType, LineLoadType and AreaLoadType.
      // to find out all load types, we can search for the base class 
      // instead of the category.
      /*
      ElementSet symbols = app.Create.NewElementSet();
      IEnumerator iter = app.ActiveDocument.Elements;
      while ((iter.MoveNext()))
      {
        LoadTypeBase loadType = iter.Current as LoadTypeBase;
        if (loadType is LoadTypeBase)
        {
          symbols.Insert(loadType);
        }
      }
      return symbols;
      */
      List<Element> a = new List<Element>();
      Filter filter = app.Create.Filter.NewTypeFilter( typeof( LoadTypeBase ), true );
      app.ActiveDocument.get_Elements( filter, a );
      return a;
    }

    /// <summary>
    /// Return all load symbols in active document.
    /// </summary>
    public static List<Element> GetPointLoadSymbols( Application app )
    {
      // The following commented code is for Revit 2008 and previous version.
      // It works in Revit 2009 too. However this method has a low performance.
      // There is a new feature named Element filter in Revit 2009 API, which can improve the performance.
      /*
      ElementSet symbols = app.Create.NewElementSet();

      ElementIterator iter = app.ActiveDocument.get_Elements(typeof(PointLoadType));
      while ((iter.MoveNext()))
      {
        Element elem = iter.Current as Element;
        if (elem is PointLoadType)
        {
          symbols.Insert(elem);
        }
      }
      return symbols;
      */
      Filter filter = app.Create.Filter.NewTypeFilter( typeof( PointLoadType ) );
      List<Element> a = new List<Element>();
      app.ActiveDocument.get_Elements( filter, a );
      return a;
    }

    /// <summary>
    /// Return all structural walls in active document.
    /// </summary>
    public static ElementSet GetAllStructuralWalls( Application app )
    {
      Filter filterWall = app.Create.Filter.NewTypeFilter( typeof( Wall ) );
      List<Element> walls = new List<Element>();
      app.ActiveDocument.get_Elements( filterWall, walls );
      ElementSet elems = app.Create.NewElementSet();
      foreach( Wall w in walls )
      {
        //
        // We could check if the wall is anything but non-bearing in one of the two following ways:
        // If Not w.Parameter(BuiltInParameter.WALL_STRUCTURAL_USAGE_TEXT_PARAM).AsString.Equals("Non-bearing") Then
        // If Not w.Parameter(BuiltInParameter.WALL_STRUCTURAL_USAGE_PARAM).AsInteger = 0 Then
        // ... but it is more generic and precise to make sure that analytical model exists
        // (in theory, one can set the wall to bearing and still uncheck Analytical):
        //
        if( null != w.AnalyticalModel )
        {
          elems.Insert( w );
        }
      }
      return elems;
    }

    /// <summary>
    /// Return all structural floors in active document.
    /// </summary>
    public static ElementSet GetAllStructuralFloors( Application app )
    {
      Filter filterFloors = app.Create.Filter.NewTypeFilter( typeof( Floor ) );
      List<Element> floors = new List<Element>();
      app.ActiveDocument.get_Elements( filterFloors, floors );
      ElementSet elems = app.Create.NewElementSet();
      foreach( Floor f in floors )
      {
        AnalyticalModel anaMod = f.AnalyticalModel;
        if( null != anaMod )
        {
          //
          // For floors, looks like we need to have additional check:
          // for non-structural floors anaMod is NOT null, but it IS empty!
          //
          AnalyticalModelFloor floorAnaMod = anaMod as AnalyticalModelFloor;
          if( 0 < floorAnaMod.Curves.Size )
          {
            elems.Insert( f );
          }
        }
      }
      return elems;
    }

    /// <summary>
    /// Return all structural continuous footings in active document.
    /// </summary>
    public static ElementSet GetAllStructuralContinuousFootings( Application app )
    {
      Filter filterFooting = app.Create.Filter.NewTypeFilter( typeof( ContFooting ) );
      List<Element> a = new List<Element>();
      app.ActiveDocument.get_Elements( filterFooting, a );
      ElementSet elems = app.Create.NewElementSet();
      foreach( ContFooting cf in a )
      {
        if( null != cf.AnalyticalModel )
        {
          elems.Insert( cf );
        }
      }
      return elems;
    }
    #endregion // Retrieve specific element collections
  }
}
