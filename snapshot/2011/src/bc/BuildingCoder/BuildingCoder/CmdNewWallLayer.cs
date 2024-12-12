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
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
#endregion // Namespaces

namespace BuildingCoder
{
  [Transaction( TransactionMode.Automatic )]
  [Regeneration( RegenerationOption.Manual )]
  class CmdNewWallLayer : IExternalCommand
  {
    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      UIApplication app = commandData.Application;
      Document doc = app.ActiveUIDocument.Document;

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


          // from here on, nothing works, as expected:
          // in the Revir 2010 API, we could call the constructor
          // even though it is for internal use only.
          // in 2011, it is not possible to call it either.

          CompoundStructureLayer newLayer = null;
          //  = new CompoundStructureLayer(); // for internal use only

          newLayer.DeckProfile = oldLayer.DeckProfile;
          //newLayer.DeckUsage = oldLayer.DeckUsage; // read-only
          //newLayer.Function = oldLayer.Function; // read-only
          newLayer.Material = oldLayer.Material;
          newLayer.Thickness = oldLayer.Thickness;
          newLayer.Variable = oldLayer.Variable;
          layers.Append( newLayer );

        }
      }
      return Result.Succeeded;
    }
  }
}
