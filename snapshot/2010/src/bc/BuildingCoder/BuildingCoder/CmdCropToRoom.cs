#region Header
//
// CmdCropToRoom.cs - set 3D view crop box to room extents
//
// Copyright (C) 2009-2010 by Jeremy Tammik,
// Autodesk Inc. All rights reserved.
//
#endregion // Header

#region Namespaces
using System;
using System.Collections.Generic;
using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Geometry;
using CmdResult = Autodesk.Revit.IExternalCommand.Result;
using GeoElement = Autodesk.Revit.Geometry.Element;
using RvtElement = Autodesk.Revit.Element;
#endregion // Namespaces

namespace BuildingCoder
{
  class CmdCropToRoom : IExternalCommand
  {
    static int _i = -1;

    /// <summary>
    /// Increment and return the current room index.
    /// Every call to this method increments the current room index by one.
    /// If it exceeds the number of rooms in the model, loop back to zero.
    /// </summary>
    /// <param name="room_count">Number of rooms in the model.</param>
    /// <returns>Incremented current room index, looping around to zero when max room count is reached.</returns>
    static int BumpRoomIndex( int room_count )
    {
      ++_i;

      if( _i >= room_count )
      {
        _i = 0;
      }
      return _i;
    }

    public CmdResult Execute( 
      ExternalCommandData commandData, 
      ref string message, 
      ElementSet elements )
    {
      Application app = commandData.Application;
      Document doc = app.ActiveDocument;
      View3D view3d = doc.ActiveView as View3D;

      if( null == view3d )
      {
        message = "Please activate a 3D view"
          + " before running this command.";

        return CmdResult.Failed;
      }

      // get the 3d view crop box:

      BoundingBoxXYZ bb = view3d.CropBox;

      // get the transform from the current view 
      // to the 3D model:

      Transform transform = bb.Transform;

      // get the transform from the 3D model 
      // to the current view:

      Transform transformInverse = transform.Inverse;

      // get all rooms in the model:

      List<RvtElement> rooms = new List<RvtElement>();
      int n = doc.get_Elements( typeof( Room ), rooms );

      Room room = (0 < n) 
        ? rooms[BumpRoomIndex( n )] as Room 
        : null;

      if( null == room )
      {
        message = "No room element found in project.";
        return CmdResult.Failed;
      }

      // collect all vertices of room closed shell 
      // to determine its extents:

      GeoElement e = room.ClosedShell;
      XYZArray vertices = app.Create.NewXYZArray();

      foreach( GeometryObject o in e.Objects )
      {
        if( o is Solid )
        {
          // iterate over all the edges of all solids:

          Solid solid = o as Solid;

          foreach( Edge edge in solid.Edges )
          {
            foreach( XYZ p in edge.Tessellate() )
            {
              // collect all vertices, 
              // including duplicates:

              vertices.Append( p );
            }
          }
        }
      }

      XYZArray verticesIn3dView 
        = app.Create.NewXYZArray();

      foreach( XYZ p in vertices )
      {
        verticesIn3dView.Append( 
          transformInverse.OfPoint( p ) );
      }

      // ignore the Z coorindates and find the 
      // min and max X and Y in the 3d view:

      double xMin = 0, yMin = 0, xMax = 0, yMax = 0;

      bool first = true;
      foreach( XYZ p in verticesIn3dView )
      {
        if( first )
        {
          xMin = p.X;
          yMin = p.Y;
          xMax = p.X;
          yMax = p.Y;
          first = false;
        }
        else
        {
          if( xMin > p.X )
            xMin = p.X;
          if( yMin > p.Y )
            yMin = p.Y;
          if( xMax < p.X )
            xMax = p.X;
          if( yMax < p.Y )
            yMax = p.Y;
        }
      }

      // grow the crop box by one twentieth of its 
      // size to include the walls of the room:

      double d = 0.05 * ( xMax - xMin );
      xMin = xMin - d;
      xMax = xMax + d;

      d = 0.05 * ( yMax - yMin );
      yMin = yMin - d;
      yMax = yMax + d;

      bb.Max = new XYZ( xMax, yMax, bb.Max.Z );
      bb.Min = new XYZ( xMin, yMin, bb.Min.Z );

      view3d.CropBox = bb;

      // change the crop view setting manually or 
      // programmatically to see the result:

      view3d.CropBoxActive = true;
      view3d.CropBoxVisible = true;

      return CmdResult.Succeeded;
    }
  }
}
