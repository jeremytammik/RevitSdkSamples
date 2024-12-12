#region Header
//
// CmdWallBottomFace.cs - determine the bottom face of a wall
//
// Copyright (C) 2009-2010 by Jeremy Tammik,
// Autodesk Inc. All rights reserved.
//
#endregion // Header

#region Namespaces
using System;
using System.Diagnostics;
using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Geometry;
using GeoElement = Autodesk.Revit.Geometry.Element;
using CmdResult = Autodesk.Revit.IExternalCommand.Result;
#endregion // Namespaces

namespace BuildingCoder
{
  class CmdWallBottomFace : IExternalCommand
  {
    const double _tolerance = 0.001;

    public CmdResult Execute( 
      ExternalCommandData commandData, 
      ref string message, 
      ElementSet elements )
    {
      Application app = commandData.Application;
      Document doc = app.ActiveDocument;

      string s = "a wall, to retrieve its bottom face";

      Wall wall = Util.SelectSingleElementOfType(
        doc, typeof( Wall ), s ) as Wall;

      if( null == wall )
      {
        message = "Please select a wall.";
      }
      else
      {
        Options opt = app.Create.NewGeometryOptions();
        GeoElement e = wall.get_Geometry( opt );

        foreach( GeometryObject obj in e.Objects )
        {
          Solid solid = obj as Solid;
          if( null != solid )
          {
            foreach( Face face in solid.Faces )
            {
              PlanarFace pf = face as PlanarFace;
              if( null != pf )
              {
                if( Util.IsVertical( pf.Normal, _tolerance )
                  && pf.Normal.Z < 0 )
                {
                  Util.InfoMsg( string.Format(
                    "The bottom face area is {0},"
                    + " and its origin is at {1}.",
                    Util.RealString( pf.Area ),
                    Util.PointString( pf.Origin ) ) );
                  break;
                }
              }
            }
          }
        }
      }
      return CmdResult.Failed;
    }
  }
}
