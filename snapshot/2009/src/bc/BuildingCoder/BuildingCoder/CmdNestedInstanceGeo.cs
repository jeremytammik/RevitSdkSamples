#region Header
//
// CmdNestedInstanceGeo.cs - analyse 
// nested instance geometry and structure
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
using Autodesk.Revit.Geometry;
using CmdResult
  = Autodesk.Revit.IExternalCommand.Result;
using RvtElement = Autodesk.Revit.Element;
using GeoElement = Autodesk.Revit.Geometry.Element;
using GeoInstance = Autodesk.Revit.Geometry.Instance;
#endregion // Namespaces

namespace BuildingCoder
{
  class CmdNestedInstanceGeo : IExternalCommand
  {
    /// <summary>
    /// Define equality between XYZ objects, ensuring that almost equal points compare equal.
    /// </summary>
    class XyzEqualityComparer : IEqualityComparer<XYZ>
    {
      public bool Equals( XYZ p, XYZ q )
      {
        return p.AlmostEqual( q );
      }

      public int GetHashCode( XYZ p )
      {
        return Util.PointString( p ).GetHashCode();
      }
    }

    static void GetVertices( XYZArray vertices, Solid s )
    {
      Debug.Assert( 0 < s.Edges.Size, 
        "expected a non-empty solid" );

      Dictionary<XYZ, int> a 
        = new Dictionary<XYZ, int>( 
          new XyzEqualityComparer() );

      foreach( Face f in s.Faces )
      {
        Mesh m = f.Triangulate();
        foreach( XYZ p in m.Vertices )
        {
          if( !a.ContainsKey( p ) )
          {
            a.Add( p, 1 );
          }
          else
          {
            ++a[p];
          }
        }
      }
      List<XYZ> keys = new List<XYZ>( a.Keys );

      Debug.Assert( 8 == keys.Count, 
        "expected eight vertices for a rectangular column" );

      keys.Sort( Util.Compare );

      foreach( XYZ p in keys )
      {
        Debug.Assert( 3 == a[p], 
          "expected every vertex of solid to appear in exactly three faces" );

        vertices.Append( p );
      }
    }

    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;
      Document doc = app.ActiveDocument;

      List<RvtElement> a = new List<RvtElement>();

      if( !Util.GetSelectedElementsOrAll( a, doc, 
        typeof( FamilyInstance ) ) )
      {
        Selection sel = doc.Selection;
        message = ( 0 < sel.Elements.Size )
          ? "Please select some family instances."
          : "No family instances found.";
        return CmdResult.Failed;
      }
      FamilyInstance inst = a[0] as FamilyInstance;

      // Here are two ways to traverse the nested instance geometry.
      // The first way can get the right position, but can't get the right structure.
      // The second way can get the right structure, but can't get the right position.
      // What I want is the right structure and right position.
      //
      // First way:
      //
      // In the current project project1.rvt, I can get myFamily3 instance via API, 
      // the class is Autodesk.Revit.Elements.FamilyInstance.
      // Then i try to get its geometry:
      //
      Options opts = app.Create.NewGeometryOptions();
      GeoElement geoElement = inst.get_Geometry( opts );

      GeometryObjectArray a1 = geoElement.Objects;
      int n = a1.Size;

      Debug.Print( 
        "Family instance geometry has {0} geometry object{1}{2}",
        n, Util.PluralSuffix( n ), Util.DotOrColon( n ) );

      int i = 0;
      foreach( GeometryObject o1 in a1 )
      {
        GeoInstance geoInstance = o1 as GeoInstance;
        if( null != geoInstance )
        {
          //
          // geometry includes one instance, so get its geometry:
          //
          GeoElement symbolGeo = geoInstance.SymbolGeometry;
          GeometryObjectArray a2 = symbolGeo.Objects;
          //
          // the symbol geometry contains five solids.
          // how can I find out which solid belongs to which column?
          // how to relate the solid to the family instance?
          //
          foreach( GeometryObject o2 in a2 )
          {
            Solid s = o2 as Solid;
            if( null != s && 0 < s.Edges.Size )
            {
              XYZArray vertices = app.Create.NewXYZArray();
              GetVertices( vertices, s );
              n = vertices.Size;

              Debug.Print( "Solid {0} has {1} vertices{2} {3}",
                i++, n, Util.DotOrColon( n ), 
                Util.PointArrayString( vertices ) );
            }
          }
        }
      }
      //
      // Another way, using 
      // FamilyInstance.Symbol.Family.Components
      // to obtain FamilyInstance:
      //
      ElementSet components = inst.Symbol.Family.Components;
      n = components.Size;

      Debug.Print(
        "Family instance symbol family has {0} component{1}{2}",
        n, Util.PluralSuffix( n ), Util.DotOrColon( n ) );

      foreach( RvtElement e in components )
      {
        //
        // there are 3 FamilyInstance: Column, myFamily1, myFamily2
        // then we can loop myFamily1, myFamily2 also.
        // then get all the Column geometry
        // But all the Column's position is the same, 
        // because the geometry is defined by the Symbol.
        // Not the actually position in project1.rvt
        //
        LocationPoint lp = e.Location as LocationPoint;
        Debug.Print( "{0} at {1}",
          Util.ElementDescription( e ),
          Util.PointString( lp.Point ) );
      }
      return CmdResult.Failed;
    }
  }
}
