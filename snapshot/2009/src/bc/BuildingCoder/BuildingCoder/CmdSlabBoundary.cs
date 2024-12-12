#region Header
//
// CmdSlabBoundary.cs - determine polygonal slab boundary loops
//
// Copyright (C) 2008 by Jeremy Tammik,
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
using CmdResult = Autodesk.Revit.IExternalCommand.Result;
#endregion // Namespaces

namespace BuildingCoder
{
  class CmdSlabBoundary : IExternalCommand
  {
    /// <summary>
    /// Offset the generated boundary polygon loop
    /// model lines downwards to separate them from
    /// the slab edge.
    /// </summary>
    const double _offset = 0.1;

    /// <summary>
    /// Determine the boundary polygons of the lowest
    /// horizontal planar face of the given solid.
    /// </summary>
    /// <param name="polygons">Return polygonal boundary
    /// loops of lowest horizontal face, i.e. profile of
    /// circumference and holes</param>
    /// <param name="solid">Input solid</param>
    /// <returns>False if no horizontal planar face was
    /// found, else true</returns>
    static bool GetBoundary(
      List<List<XYZ>> polygons,
      Solid solid )
    {
      PlanarFace lowest = null;
      FaceArray faces = solid.Faces;
      foreach( Face f in faces )
      {
        PlanarFace pf = f as PlanarFace;
        if( null != pf && Util.IsHorizontal( pf ) )
        {
          if( ( null == lowest )
            || ( pf.Origin.Z < lowest.Origin.Z ) )
          {
            lowest = pf;
          }
        }
      }
      if( null != lowest )
      {
        XYZ p, q = XYZ.Zero;
        bool first;
        int i, n;
        EdgeArrayArray loops = lowest.EdgeLoops;
        foreach( EdgeArray loop in loops )
        {
          List<XYZ> vertices = new List<XYZ>();
          first = true;
          foreach( Edge e in loop )
          {
            XYZArray points = e.Tessellate();
            p = points.get_Item( 0 );
            if( !first )
            {
              Debug.Assert( p.AlmostEqual( q ),
                "expected subsequent start point"
                + " to equal previous end point" );
            }
            n = points.Size;
            q = points.get_Item( n - 1 );
            for( i = 0; i < n - 1; ++i )
            {
              XYZ v = points.get_Item( i );
              v.Z -= _offset;
              vertices.Add( v );
            }
          }
          q.Z -= _offset;
          Debug.Assert( q.AlmostEqual( vertices[0] ),
            "expected last end point to equal"
            + " first start point" );
          polygons.Add( vertices );
        }
      }
      return null != lowest;
    }

    /// <summary>
    /// Return all floor slab boundary loop polygons
    /// for the given floors, downwards from the bottom
    /// floor faces by a certain amount.
    /// </summary>
    static public List<List<XYZ>> GetFloorBoundaryPolygons(
      Application app,
      List<RvtElement> floors )
    {
      List<List<XYZ>> polygons = new List<List<XYZ>>();
      Options opt = app.Create.NewGeometryOptions();
      //opt.DetailLevel = Options.DetailLevels.Medium;
      //opt.ComputeReferences = true;

      foreach( Floor floor in floors )
      {
        GeoElement geo = floor.get_Geometry( opt );
        GeometryObjectArray objects = geo.Objects;
        foreach( GeometryObject obj in objects )
        {
          Solid solid = obj as Solid;
          if( solid != null )
          {
            GetBoundary( polygons, solid );
          }
        }
      }
      return polygons;
    }

    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;
      Document doc = app.ActiveDocument;
      //
      // retrieve selected floors, or all floors, if nothing is selected:
      //
      List<RvtElement> floors = new List<RvtElement>();
      if( !Util.GetSelectedElementsOrAll(
        floors, doc, typeof( Floor ) ) )
      {
        Selection sel = doc.Selection;
        message = ( 0 < sel.Elements.Size )
          ? "Please select some floor elements."
          : "No floor elements found.";
        return CmdResult.Failed;
      }

      List<List<XYZ>> polygons
        = GetFloorBoundaryPolygons( app, floors );

      int n = polygons.Count;

      Debug.Print(
        "{0} boundary loop{1} found.",
        n, Util.PluralSuffix( n ) );

      Creator creator = new Creator( app );
      creator.DrawPolygons( polygons );

      return CmdResult.Succeeded;
    }
  }
}
