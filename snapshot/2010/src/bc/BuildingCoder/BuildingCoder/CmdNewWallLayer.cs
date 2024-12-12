#region Header
//
// CmdNewWallLayer.cs - create a new compound wall layer.
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
using Autodesk.Revit.Structural;
using Autodesk.Revit.Symbols;
using CmdResult
  = Autodesk.Revit.IExternalCommand.Result;
#endregion // Namespaces

namespace BuildingCoder
{
  class CmdNewWallLayer : IExternalCommand
  {
    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;
      Document doc = app.ActiveDocument;

      Debug.Assert( false,
        "Currently, no new wall layer can be created, because"
        + "there is no creation method available for it." );

      foreach ( WallType wallType in doc.WallTypes )
      {
        if ( 0 < wallType.CompoundStructure.Layers.Size )
        {
          CompoundStructureLayer oldLayer
            = wallType.CompoundStructure.Layers.get_Item( 0 );

          WallType newWallType
            = wallType.Duplicate( "NewWallType" ) as WallType;

          CompoundStructure structure
            = newWallType.CompoundStructure;

          CompoundStructureLayerArray layers
            = structure.Layers;

          //
          // from here on, nothing works, as expected:
          //
          CompoundStructureLayer newLayer
            = new CompoundStructureLayer(); // for internal use only

          newLayer.DeckProfile = oldLayer.DeckProfile;
          //newLayer.DeckUsage = oldLayer.DeckUsage; // read-only
          //newLayer.Function = oldLayer.Function; // read-only
          newLayer.Material = oldLayer.Material;
          newLayer.Thickness = oldLayer.Thickness;
          newLayer.Variable = oldLayer.Variable;
          layers.Append( newLayer );
        }
      }
      return CmdResult.Succeeded;
    }
  }
}
