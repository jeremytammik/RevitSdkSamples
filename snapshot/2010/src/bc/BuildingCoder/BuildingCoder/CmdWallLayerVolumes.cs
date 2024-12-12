#region Header
//
// CmdWallLayerVolumes.cs - determine
// compound wall layer volumes
//
// Copyright (C) 2009-2010 by Jeremy Tammik,
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
using Autodesk.Revit.Structural;
using Autodesk.Revit.Symbols;
using CmdResult
  = Autodesk.Revit.IExternalCommand.Result;
#endregion // Namespaces

namespace BuildingCoder
{
  class CmdWallLayerVolumes : IExternalCommand
  {
    /// <summary>
    /// Enhance the standard Dictionary
    /// class to have a Cumulate method.
    /// </summary>
    class MapLayerToVolume
      : Dictionary<string, double>
    {
      /// <summary>
      /// Add cumulated value.
      /// If seen for the first time for
      /// this key, initialise with zero.
      /// </summary>
      public void Cumulate(
        string key,
        double value )
      {
        if( !ContainsKey( key ) )
        {
          this[key] = 0.0;
        }
        this[key] += value;
      }
    }

    const BuiltInParameter _bipArea
      = BuiltInParameter.HOST_AREA_COMPUTED;

    const BuiltInParameter _bipVolume
      = BuiltInParameter.HOST_VOLUME_COMPUTED;

    /// <summary>
    /// Return the specified double parameter
    /// value for the given wall.
    /// </summary>
    double GetWallParameter(
      Wall wall,
      BuiltInParameter bip )
    {
      Parameter p = wall.get_Parameter( bip );

      Debug.Assert( null != p,
        "expected wall to have "
        + "HOST_AREA_COMPUTED and "
        + "HOST_VOLUME_COMPUTED parameters" );

      return p.AsDouble();
    }

    /// <summary>
    /// Cumulate the compound wall layer volumes for the given wall.
    /// </summary>
    void GetWallLayerVolumes(
      Wall wall,
      ref MapLayerToVolume totalVolumes )
    {
      WallType wt = wall.WallType;

      CompoundStructure structure
        = wt.CompoundStructure;

      CompoundStructureLayerArray layers
        = structure.Layers;

      int i, n = layers.Size;
      double area = GetWallParameter( wall, _bipArea );
      double volume = GetWallParameter( wall, _bipVolume );
      double thickness = wt.Width;

      string desc = Util.ElementDescription( wall );

      Debug.Print(
        "{0} with thickness {1}"
        + " and volume {2}"
        + " has {3} layer{4}{5}",
        desc,
        Util.MmString( thickness ),
        Util.RealString( volume ),
        n, Util.PluralSuffix( n ),
        Util.DotOrColon( n ) );

      //
      // volume for entire wall:
      //
      string key = wall.WallType.Name;
      totalVolumes.Cumulate( key, volume );

      //
      // volume for compound wall layers:
      //
      if( 0 < n )
      {
        i = 0;
        double total = 0.0;
        double layerVolume;
        foreach( CompoundStructureLayer
          layer in layers )
        {
          key = wall.WallType.Name + " : "
            + layer.Function;

          layerVolume = area * layer.Thickness;

          totalVolumes.Cumulate( key, layerVolume );
          total += layerVolume;

          Debug.Print(
            "  Layer {0}: function {1}, "
            + "thickness {2}, volume {3}",
            ++i, layer.Function,
            Util.MmString( layer.Thickness ),
            Util.RealString( layerVolume ) );
        }

        Debug.Print( "Wall volume = {0},"
          + " total layer volume = {1}",
          Util.RealString( volume ),
          Util.RealString( total ) );

        if( !Util.IsEqual( volume, total ) )
        {
          Debug.Print( "Wall host volume parameter"
            + " value differs from sum of all layer"
            + " volumes: {0}",
            volume - total );
        }
      }
    }

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
      List<Element> walls = new List<Element>();
      if( !Util.GetSelectedElementsOrAll(
        walls, doc, typeof( Wall ) ) )
      {
        Selection sel = doc.Selection;
        message = ( 0 < sel.Elements.Size )
          ? "Please select some wall elements."
          : "No wall elements found.";
        return CmdResult.Failed;
      }

      //
      // mapping of compound wall layers to total cumulated volume;
      // the key is "<wall type name> : <wall layer function>",
      // the value is its cumulated volume across all selected walls:
      //
      MapLayerToVolume totalVolumes
        = new MapLayerToVolume();

      foreach( Wall wall in walls )
      {
        GetWallLayerVolumes( wall, ref totalVolumes );
      }

      string msg
        = "Compound wall layer volumes formatted as '"
        + "wall type : layer function :"
        + " volume in cubic meters':\n";

      List<string> keys = new List<string>(
        totalVolumes.Keys );

      keys.Sort();

      foreach( string key in keys )
      {
        msg += "\n" + key + " : "
          + Util.RealString(
            Util.CubicFootToCubicMeter(
            totalVolumes[key] ) );
      }

      Util.InfoMsg( msg );

      return CmdResult.Cancelled;
    }
  }
}