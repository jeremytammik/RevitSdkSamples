#region Header
//
// CmdWallLayers.cs - analyse wall compound
// layer structure and geometry
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
using Autodesk.Revit.Structural;
using Autodesk.Revit.Structural.Enums;
using RvtElement = Autodesk.Revit.Element;
using GeoElement = Autodesk.Revit.Geometry.Element;
using CmdResult
  = Autodesk.Revit.IExternalCommand.Result;
#endregion // Namespaces

namespace BuildingCoder
{
  class CmdWallLayers : IExternalCommand
  {
    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;
      Document doc = app.ActiveDocument;
      //
      // retrieve selected walls, or all walls,
      // if nothing is selected:
      //
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

      int i, n;
      double halfThickness, layerOffset;
      Creator creator = new Creator( app );
      XYZ lcstart, lcend, v, w, p, q;

      foreach( Wall wall in walls )
      {
        string desc = Util.ElementDescription( wall );

        LocationCurve curve
          = wall.Location as LocationCurve;

        if( null == curve )
        {
          message = desc + ": No wall curve found.";
          return CmdResult.Failed;
        }
        //
        // wall centre line and thickness:
        //
        lcstart = curve.Curve.get_EndPoint( 0 );
        lcend = curve.Curve.get_EndPoint( 1 );
        halfThickness = 0.5 * wall.WallType.Width;
        v = lcend - lcstart;
        v = v.Normalized; // one foot long
        w = XYZ.BasisZ.Cross( v ).Normalized;
        if( wall.Flipped ) { w = -w; }

        p = lcstart - 2 * v;
        q = lcend + 2 * v;
        creator.CreateModelLine( p, q );

        q = p + halfThickness * w;
        creator.CreateModelLine( p, q );

        // exterior edge
        p = lcstart - v + halfThickness * w;
        q = lcend + v + halfThickness * w;
        creator.CreateModelLine( p, q );

        CompoundStructure structure
          = wall.WallType.CompoundStructure;

        CompoundStructureLayerArray layers
          = structure.Layers;

        i = 0;
        n = layers.Size;
        Debug.Print(
          "{0} with thickness {1}"
          + " has {2} layer{3}{4}",
          desc,
          Util.MmString( 2 * halfThickness ),
          n, Util.PluralSuffix( n ),
          Util.DotOrColon( n ) );

        if( 0 == n )
        {
          // interior edge
          p = lcstart - v - halfThickness * w;
          q = lcend + v - halfThickness * w;
          creator.CreateModelLine( p, q );
        }
        else
        {
          layerOffset = halfThickness;
          foreach( CompoundStructureLayer layer
            in layers )
          {
            Debug.Print(
              "  Layer {0}: function {1}, "
              + "thickness {2}",
              ++i, layer.Function,
              Util.MmString( layer.Thickness ) );

            layerOffset -= layer.Thickness;
            p = lcstart - v + layerOffset * w;
            q = lcend + v + layerOffset * w;
            creator.CreateModelLine( p, q );
          }
        }
      }
      return CmdResult.Succeeded;
    }
  }
}
