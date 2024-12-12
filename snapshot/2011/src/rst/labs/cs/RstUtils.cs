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
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.ApplicationServices;
#endregion // Namespaces

namespace RstLabs
{
  /// <summary>
  /// Revit Structure utilities.
  /// </summary>
  static class RstUtils
  {
    #region Structural element description
    public static string StructuralElementDescription( FamilyInstance e )
    {
      BuiltInCategory bic = BuiltInCategory.OST_StructuralFraming;
      Category cat = e.Document.Settings.Categories.get_Item( bic );
      bool hasCat = ( null != e.Category );
      bool hasUsage = hasCat && e.Category.Id.Equals( cat.Id );
      return e.Name
        + " Id=" + e.Id.IntegerValue.ToString()
        + ( hasCat ? ", Category=" + e.Category.Name : string.Empty )
        + ", Type=" + e.Symbol.Name
        + ( hasUsage ? ", Struct.Usage=" + e.StructuralUsage.ToString() : string.Empty ) // can throw exception "only Beam and Brace support Structural Usage!"
        + ", Struct.Type=" + e.StructuralType.ToString();
    }

    public static string StructuralElementDescription( ContFooting e )
    {
      return e.Name
        + " Id=" + e.Id.IntegerValue.ToString()
        + ", Category=" + e.Category.Name
        + ", Type=" + e.FootingType.Name;
    }

    public static string StructuralElementDescription( Floor e )
    {
      return e.Name
        + " Id=" + e.Id.IntegerValue.ToString()
        + ", Category=" + e.Category.Name
        + ", Type=" + e.FloorType.Name
        + ", Struct.Usage=" + e.StructuralUsage.ToString(); // can throw exception "only Beam and Brace support Structural Usage!"
    }

    public static string StructuralElementDescription( Wall e )
    {
      return e.Name
        + " Id=" + e.Id.IntegerValue.ToString()
        + ", Category=" + e.Category.Name
        + ", Type=" + e.WallType.Name
        + ", Struct.Usage=" + e.StructuralUsage.ToString(); // can throw exception "only Beam and Brace support Structural Usage!"
    }
    #endregion // Structural element description

    #region Retrieve specific element collections
    /// <summary>
    /// Return all instances of the specified class..
    /// </summary>
    public static IList<Element> GetInstanceOfClass( 
      Document doc, 
      Type type )
    {
      FilteredElementCollector collector = new FilteredElementCollector( doc );
      return collector.OfClass( type ).ToElements();
    }

    /// <summary>
    /// Return all instances of the specified class,including the derevided class instances.
    /// </summary>
    public static IList<Element> GetInstanceOfClass(
      Document doc,
      Type type,
      bool bInverted )
    {
      FilteredElementCollector collector = new FilteredElementCollector( doc );
      ElementClassFilter classFilter = new ElementClassFilter( type, bInverted );
      return collector.WherePasses(classFilter).ToElements();      
    }

    /// <summary>
    /// Return all loads in the current active document,
    /// i.e. any objects derived from LoadBase, sorted by load type.
    /// </summary>
    public static void GetAllSpecificLoads(
      Document doc,
      ref List<Element> pointLoads,
      ref List<Element> lineLoads,
      ref List<Element> areaLoads )
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
      IList<Element> a = GetInstanceOfClass( doc, typeof( LoadBase ), false );
      foreach( Element elem in a )
      {
        if( elem is PointLoad )
        {
          pointLoads.Add( elem );
        }
        else if( elem is LineLoad )
        {
          lineLoads.Add( elem );
        }
        else if( elem is AreaLoad )
        {
          areaLoads.Add( elem );
        }
      }
    }


    /// <summary>
    /// Return all structural walls in active document.
    /// </summary>
    public static List<Element> GetAllStructuralWalls( Document doc )
    {
      /*
      IList<Element> walls = GetInstanceOfClass( doc, typeof( Wall ) );
      List<Element> elems = new List<Element>();
      foreach( Wall w in walls )
      {
        //
        // We could check if the wall is anything but non-bearing in one of the two following ways:
        // If Not w.Parameter(BuiltInParameter.WALL_STRUCTURAL_USAGE_TEXT_PARAM).AsString.Equals("Non-bearing") Then
        // If Not w.Parameter(BuiltInParameter.WALL_STRUCTURAL_USAGE_PARAM).AsInteger = 0 Then
        // ... but it is more generic and precise to make sure that analytical model exists
        // (in theory, one can set the wall to bearing and still uncheck Analytical):
        //
        if( null != w.GetAnalyticalModel())
        {
          elems.Add(w);
        }
      }
      return elems;
      */

      FilteredElementCollector collector = new FilteredElementCollector( doc );
      collector.OfClass( typeof( Wall ) );
      
      return new List<Element>(
        from wall in collector
        where null != wall.GetAnalyticalModel()
        select wall );
    }

    /// <summary>
    /// Return all structural floors in active document.
    /// </summary>
    public static List<Element> GetAllStructuralFloors( Document doc)
    {
      //List<Element> floors = new List<Element>();
      //Filter filterFloors = app.Create.Filter.NewTypeFilter( typeof( Floor ) );
      //List<Element> floors = new List<Element>();
      //app.ActiveDocument.get_Elements( filterFloors, floors );

      /*
      IList<Element> floors = GetInstanceOfClass( doc, typeof( Floor ) );
      List<Element> elems = new List<Element>();
      foreach( Floor f in floors )
      {
        AnalyticalModel anaMod = f.GetAnalyticalModel();
        if( null != anaMod )
        {
          //
          // For floors, looks like we need to have additional check:
          // for non-structural floors anaMod is NOT null, but it IS empty!
          //
          //          AnalyticalModelFloor floorAnaMod = anaMod as AnalyticalModelFloor;
          if( 0 < anaMod.GetCurves( AnalyticalCurveType.RawCurves ).Count )
          {
            elems.Add( f );
          }
        }
      }
      return elems;
      */

      FilteredElementCollector collector = new FilteredElementCollector( doc );
      collector.OfClass( typeof( Floor ) );

      AnalyticalModel am;

      return new List<Element>(
        from floor in collector
        where ( null != ( am = floor.GetAnalyticalModel() ) 
          && ( 0 < am.GetCurves( AnalyticalCurveType.RawCurves ).Count ) )
        select floor );
    }

    /// <summary>
    /// Return all structural continuous footings in active document.
    /// </summary>
    public static List<Element> GetAllStructuralContinuousFootings( Document doc )
    {
      //List<Element> a = new List<Element>();
      //Filter filterFooting = app.Create.Filter.NewTypeFilter( typeof( ContFooting ) );
      //List<Element> a = new List<Element>();
      //app.ActiveDocument.get_Elements( filterFooting, a );

      /*
      IList<Element> a = GetInstanceOfClass( doc, typeof( ContFooting ) );
      List<Element> elems = new List<Element>();
      foreach( ContFooting cf in a )
      {
        if( null != cf.GetAnalyticalModel())
        {
          elems.Add(cf);
        }
      }
      return elems;
      */

      FilteredElementCollector collector = new FilteredElementCollector( doc );
      collector.OfClass( typeof( ContFooting ) );

      return new List<Element>(
        from cf in collector
        where null != cf.GetAnalyticalModel()
        select cf );
    }
    #endregion // Retrieve specific element collections
  }
}
