#region Header
//
// CmdWallProfileAreas.cs - determine wall
// elevation profile boundary loop polygon areas
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
using Autodesk.Revit.Geometry;
using RvtElement = Autodesk.Revit.Element;
using GeoElement = Autodesk.Revit.Geometry.Element;
using CmdResult
  = Autodesk.Revit.IExternalCommand.Result;
#endregion // Namespaces

namespace BuildingCoder
{
  class CmdWallProfileArea : IExternalCommand
  {
    #region Three-Dimensional Polygon Area
    /*
    /// <summary>
    /// Return the average of a list of values.
    /// Prerequisite: the underlying class T must supply
    /// operator*(double) and operator+(const T &).
    /// </summary>
    T Average<T>( List<T> a )
    {
      T result;
      bool first = true;
      foreach( T x in a )
      {
        if( first )
        {
          result = x;
        }
        else
        {
          result += x;
        }
      }
      return result * ( 1.0 / a.Count );
    }

    XYZ Sum( List<XYZ> a )
    {
      XYZ sum = XYZ.Zero;
      foreach( XYZ x in a )
      {
        sum += x;
      }
      return sum;
    }

    XYZ Average( List<XYZ> a )
    {
      return Sum( a ) * (1.0 / a.Count);
    }

    XYZ TriangleCenter( List<XYZ> pts )
    {
      Debug.Assert( 3 == pts.Count, "expected three points in triangle" );
      return Average( pts );
    }
    */

    /// <summary>
    /// Return the plane properties of a given polygon,
    /// i.e. the plane normal, area, and its distance
    /// from the origin.
    /// </summary>
    bool GetPolygonPlane(
      List<XYZ> polygon,
      out XYZ normal,
      out double dist,
      out double area )
    {
      normal = XYZ.Zero;
      dist = area = 0.0;
      int n = ( null == polygon ) ? 0 : polygon.Count;
      bool rc = ( 2 < n );
      if( 3 == n )
      {
        //
        // the general case returns a wrong result for the triangle
        // ((-1 -1 -1) (1 -1 -1) (-1 -1 1)), so implement specific
        // code for triangle:
        //
        XYZ a = polygon[0];
        XYZ b = polygon[1];
        XYZ c = polygon[2];
        XYZ v = b - a;
        normal = v.Cross( c - a );
        dist = normal.Dot( a );
      }
      else if( 4 == n )
      {
        //
        // more efficient code for 4-sided polygons
        //
        XYZ a = polygon[0];
        XYZ b = polygon[1];
        XYZ c = polygon[2];
        XYZ d = polygon[3];

        normal.X = ( c.Y - a.Y ) * ( d.Z - b.Z )
          + ( c.Z - a.Z ) * ( b.Y - d.Y );
        normal.Y = ( c.Z - a.Z ) * ( d.X - b.X )
          + ( c.X - a.X ) * ( b.Z - d.Z );
        normal.Z = ( c.X - a.X ) * ( d.Y - b.Y )
          + ( c.Y - a.Y ) * ( b.X - d.X );

        dist = 0.25 *
          ( normal.X * ( a.X + b.X + c.X + d.X )
          + normal.Y * ( a.Y + b.Y + c.Y + d.Y )
          + normal.Z * ( a.Z + b.Z + c.Z + d.Z ) );
      }
      else if( 4 < n )
      {
        //
        // general case for n-sided polygons
        //
        XYZ a;
        XYZ b = polygon[n - 2];
        XYZ c = polygon[n - 1];
        XYZ s = XYZ.Zero;

        for( int i = 0; i < n; ++i )
        {
          a = b;
          b = c;
          c = polygon[i];

          normal.X += b.Y * ( c.Z - a.Z );
          normal.Y += b.Z * ( c.X - a.X );
          normal.Z += b.X * ( c.Y - a.Y );

          s += c;
        }
        dist = s.Dot( normal ) / n;
      }
      if( rc )
      {
        //
        // the polygon area is half of the length
        // of the non-normalized normal vector of the plane:
        //
        double length = normal.Length;
        rc = !Util.IsZero( length );
        Debug.Assert( rc );

        if( rc )
        {
          normal /= length;
          dist /= length;
          area = 0.5 * length;
        }
      }
      return rc;
    }

    double[] GetPolygonAreas( List<List<XYZ>> polygons )
    {
      int i = 0, n = polygons.Count;
      double[] areas = new double[n];
      double dist, area;
      XYZ normal;
      foreach( List<XYZ> polygon in polygons )
      {
        if( GetPolygonPlane( polygon, out normal, out dist, out area ) )
        {
          areas[i++] = area;
        }
      }
      return areas;
    }
    #endregion // Three-Dimensional Polygon Area

    #region Transform 3D plane to horizontal
    Transform GetTransformToZ( XYZ v )
    {
      Transform t;

      double a = XYZ.BasisZ.Angle( v );

      if( Util.IsZero( a ) )
      {
        t = Transform.Identity;
      }
      else
      {
        XYZ axis = Util.IsEqual( a, Math.PI )
          ? XYZ.BasisX
          : v.Cross( XYZ.BasisZ );

        t = Transform.get_Rotation( XYZ.Zero,
          axis, a );
      }
      return t;
    }

    List<XYZ> ApplyTransform(
      List<XYZ> polygon,
      Transform t )
    {
      int n = polygon.Count;

      List<XYZ> polygonTransformed
        = new List<XYZ>( n );

      foreach( XYZ p in polygon )
      {
        polygonTransformed.Add( t.OfPoint( p ) );
      }
      return polygonTransformed;
    }
    #endregion // Transform 3D plane to horizontal

    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;
      Document doc = app.ActiveDocument;

      List<RvtElement> walls = new List<RvtElement>();
      if( !Util.GetSelectedElementsOrAll(
        walls, doc, typeof( Wall ) ) )
      {
        Selection sel = doc.Selection;
        message = ( 0 < sel.Elements.Size )
          ? "Please select some wall elements."
          : "No wall elements found.";
        return CmdResult.Failed;
      }

      List<List<XYZ>> polygons
        = CmdWallProfile.GetWallProfilePolygons(
          app, walls );

      int i = 0, n = polygons.Count;
      double[] areas = new double[n];
      double d, a, maxArea = 0.0;
      XYZ normal;
      foreach( List<XYZ> polygon in polygons )
      {
        GetPolygonPlane( polygon,
          out normal, out d, out a );
        if( Math.Abs( maxArea ) < Math.Abs( a ) )
        {
          maxArea = a;
        }
        areas[i++] = a;

#if DEBUG
      //
      // transform the 3D polygon into a horizontal plane
      // so we can use the 2D GetSignedPolygonArea() and
      // compare its results with the 3D calculation.
      //
      // todo: compare the relative speed of
      // transforming 3d to 2d and using 2d area
      // calculation versus direct 3d area calculation.
      //

      Transform t = GetTransformToZ( normal );

      List<XYZ> polygonHorizontal
        = ApplyTransform( polygon, t );

      List<UV> polygon2d
        = CmdSlabBoundaryArea.Flatten(
          polygonHorizontal );

      double a2
        = CmdSlabBoundaryArea.GetSignedPolygonArea(
          polygon2d );

      Debug.Assert( Util.IsEqual( a, a2 ),
        "expected same area from 2D and 3D calculations" );
#endif

      }

      Debug.Print(
        "{0} boundary loop{1} found.",
        n, Util.PluralSuffix( n ) );

      for( i = 0; i < n; ++i )
      {
        Debug.Print(
          "  Loop {0} area is {1} square feet{2}",
          i,
          Util.RealString( areas[i] ),
          ( areas[i].Equals( maxArea )
            ? ", outer loop of largest wall"
            : "" ) );
      }

      Creator creator = new Creator( app );
      creator.DrawPolygons( polygons );

      return CmdResult.Succeeded;
    }
  }
}
