#region Header
//
// CmdEditFloor.cs - read existing floor geometry and create a new floor
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
  class CmdEditFloor : IExternalCommand
  {
    /// <summary>
    /// Determine the uppermost horizontal face
    /// of a given "horizontal" solid object
    /// such as a floor slab. Currently only
    /// supports planar faces.
    /// </summary>
    /// <param name="verticalFaces">Return solid vertical boundary faces, i.e. 'sides'</param>
    /// <param name="solid">Input solid</param>
    PlanarFace GetTopFace( Solid solid )
    {
      PlanarFace topFace = null;
      FaceArray faces = solid.Faces;
      foreach( Face f in faces )
      {
        PlanarFace pf = f as PlanarFace;
        if( null != pf
          && Util.IsHorizontal( pf ) )
        {
          if( (null == topFace)
            || (topFace.Origin.Z < pf.Origin.Z) )
          {
            topFace = pf;
          }
        }
      }
      return topFace;
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
      //
      // determine top face of each selected floor:
      //
      int nNullFaces = 0;
      List<Face> topFaces = new List<Face>();
      Options opt = app.Create.NewGeometryOptions();

      foreach( Floor floor in floors )
      {
        GeoElement geo = floor.get_Geometry( opt );
        GeometryObjectArray objects = geo.Objects;
        foreach( GeometryObject obj in objects )
        {
          Solid solid = obj as Solid;
          if( solid != null )
          {
            PlanarFace f = GetTopFace( solid );
            if( null == f )
            {
              Debug.WriteLine(
                Util.ElementDescription( floor )
                + " has no top face." );
              ++nNullFaces;
            }
            topFaces.Add( f );
          }
        }
      }
      //
      // create new floors from the top faces found
      // before creating the new floor, we would obviously
      // apply whatever modifications are required to the
      // new floor profile:
      //
      Autodesk.Revit.Creation.Application creApp = app.Create;
      Autodesk.Revit.Creation.Document creDoc = doc.Create;

      int i = 0;
      int n = topFaces.Count - nNullFaces;

      Debug.Print(
        "{0} top face{1} found.",
        n, Util.PluralSuffix( n ) );

      foreach( Face f in topFaces )
      {
        if( null != f )
        {
          CurveArray profile = new CurveArray();
          EdgeArrayArray eaa = f.EdgeLoops;
          //
          // only use first edge array,
          // the outer boundary loop,
          // skip the further items
          // representing holes:
          //
          EdgeArray ea = eaa.get_Item( 0 );
          foreach( Edge e in ea )
          {
            XYZArray pts = e.Tessellate();
            int m = pts.Size;
            XYZ p = pts.get_Item( 0 );
            XYZ q = pts.get_Item( m - 1 );
            Line line = creApp.NewLineBound( p, q );
            profile.Append( line );
          }
          Floor floor = floors[i++] as Floor;
          floor = creDoc.NewFloor( profile, floor.FloorType, floor.Level, true );
          doc.Move( floor, new XYZ( 5, 5, 0 ) );
        }
      }
      return CmdResult.Succeeded;
    }
  }
}
