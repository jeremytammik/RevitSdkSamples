#region Header
//
// Creator.cs - model line creator helper class
//
// Copyright (C) 2008-2010 by Jeremy Tammik,
// Autodesk Inc. All rights reserved.
//
#endregion // Header

#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit.Creation;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Geometry;
#endregion // Namespaces

namespace BuildingCoder
{
  class Creator
  {
    // these are
    // Autodesk.Revit.Creation
    // objects!
    Autodesk.Revit.Creation.Application _app;
    Document _doc;

    public Creator( Autodesk.Revit.Application app )
    {
      _app = app.Create;
      _doc = app.ActiveDocument.Create;
    }

    /// <summary>
    /// Miroslav Schonauer's model line creation method.
    /// A utility function to create an arbitrary sketch
    /// plane given the model line end points.
    /// </summary>
    /// <param name="app">Revit application</param>
    /// <param name="p">Model line start point</param>
    /// <param name="q">Model line end point</param>
    /// <returns></returns>
    public static ModelLine CreateModelLine(
      Autodesk.Revit.Application app,
      XYZ p,
      XYZ q )
    {
      if( p.Distance( q ) < Util.MinLineLength ) return null;
      //
      // Create sketch plane; for non-vertical lines,
      // use Z-axis to span the plane, otherwise Y-axis:
      //
      XYZ v = q - p;

      double dxy = Math.Abs( v.X ) + Math.Abs( v.Y );

      XYZ w = ( dxy > Util.TolPointOnPlane )
        ? XYZ.BasisZ
        : XYZ.BasisY;

      XYZ norm = v.Cross( w ).Normalized;

      Plane plane = app.Create.NewPlane( norm, p );

      Autodesk.Revit.Creation.Document creDoc
        = app.ActiveDocument.Create;

      SketchPlane sketchPlane = creDoc.NewSketchPlane( plane );

      return creDoc.NewModelCurve(
        app.Create.NewLine( p, q, true ),
        sketchPlane ) as ModelLine;
    }

    SketchPlane NewSketchPlanePassLine(
      Line line )
    {
      XYZ p = line.get_EndPoint( 0 );
      XYZ q = line.get_EndPoint( 1 );
      XYZ norm;
      if( p.X == q.X )
      {
        norm = XYZ.BasisX;
      }
      else if( p.Y == q.Y )
      {
        norm = XYZ.BasisY;
      }
      else
      {
        norm = XYZ.BasisZ;
      }
      Plane plane = _app.NewPlane(
        norm, p );

      return _doc.NewSketchPlane( plane );
    }

    public void CreateModelLine( XYZ p, XYZ q )
    {
      if( p.AlmostEqual( q ) )
      {
        throw new ArgumentException(
          "Expected two different points." );
      }
      Line line = _app.NewLine( p, q, true );
      if( null == line )
      {
        throw new Exception(
          "Geometry line creation failed." );
      }
      _doc.NewModelCurve( line,
        NewSketchPlanePassLine( line ) );
    }

    public void DrawPolygons(
      List<List<XYZ>> loops )
    {
      XYZ p1 = XYZ.Zero;
      XYZ q = XYZ.Zero;
      bool first;
      foreach( List<XYZ> loop in loops )
      {
        first = true;
        foreach( XYZ p in loop )
        {
          if( first )
          {
            p1 = p;
            first = false;
          }
          else
          {
            CreateModelLine( p, q );
          }
          q = p;
        }
        CreateModelLine( q, p1 );
      }
    }

    public void DrawFaceTriangleNormals( Face f )
    {
      Mesh mesh = f.Triangulate();
      int n = mesh.NumTriangles;

      string s = "{0} face triangulation returns "
        + "mesh triangle{1} and normal vector{1}:";

      Debug.Print(
        s, n, Util.PluralSuffix( n ) );

      for( int i = 0; i < n; ++i )
      {
        MeshTriangle t = mesh.get_Triangle( i );

        XYZ p = ( t.get_Vertex( 0 )
          + t.get_Vertex( 1 )
          + t.get_Vertex( 2 ) ) / 3;

        XYZ v = t.get_Vertex( 1 )
          - t.get_Vertex( 0 );

        XYZ w = t.get_Vertex( 2 )
          - t.get_Vertex( 0 );

        XYZ normal = v.Cross( w ).Normalized;

        Debug.Print(
          "{0} {1} --> {2}", i,
          Util.PointString( p ),
          Util.PointString( normal ) );

        CreateModelLine( p, p + normal );
      }
    }
  }
}
