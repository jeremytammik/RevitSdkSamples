#region Header
//
// CmdRoomWallAdjacency.cs - determine part
// of wall face area that bounds a room.
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
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
// todo: report and resolve this, this should not be required: 'RE: ambiguous BoundarySegmentArrayArray'
using BoundarySegmentArrayArray = Autodesk.Revit.DB.Architecture.BoundarySegmentArrayArray;
using BoundarySegmentArray = Autodesk.Revit.DB.Architecture.BoundarySegmentArray;
using BoundarySegment = Autodesk.Revit.DB.Architecture.BoundarySegment;
#endregion // Namespaces

namespace BuildingCoder
{
  /// <summary>
  /// Determine part of wall face area that bounds a room.
  /// </summary>
  [Transaction( TransactionMode.ReadOnly )]
  [Regeneration( RegenerationOption.Manual )]
  class CmdRoomWallAdjacency : IExternalCommand
  {
    void DetermineAdjacentElementLengthsAndWallAreas(
      Room room )
    {
      BoundarySegmentArrayArray boundaries
        = room.Boundary;

      // a room may have a null boundary property:

      int n = (null == boundaries)
        ? 0
        : boundaries.Size;

      Debug.Print(
        "{0} has {1} boundar{2}{3}",
        Util.ElementDescription( room ),
        n, Util.PluralSuffixY( n ),
        Util.DotOrColon( n ) );

      if( 0 < n )
      {
        int iBoundary = 0, iSegment;

        foreach( BoundarySegmentArray b in boundaries )
        {
          ++iBoundary;
          iSegment = 0;
          foreach( BoundarySegment s in b )
          {
            ++iSegment;
            Element neighbour = s.Element;
            Curve curve = s.Curve;
            double length = curve.Length;

            Debug.Print(
              "  Neighbour {0}:{1} {2} has {3}"
              + " feet adjacent to room.",
              iBoundary, iSegment,
              Util.ElementDescription( neighbour ),
              Util.RealString( length ) );

            if( neighbour is Wall )
            {
              Wall wall = neighbour as Wall;

              Parameter p = wall.get_Parameter(
                BuiltInParameter.HOST_AREA_COMPUTED );

              double area = p.AsDouble();

              LocationCurve lc
                = wall.Location as LocationCurve;

              double wallLength = lc.Curve.Length;

              Level bottomLevel = wall.Level;
              double bottomElevation = bottomLevel.Elevation;
              double topElevation = bottomElevation;

              p = wall.get_Parameter(
                BuiltInParameter.WALL_HEIGHT_TYPE );

              if( null != p )
              {
                ElementId id = p.AsElementId();
                Level topLevel = wall.Document.get_Element( id ) as Level;
                topElevation = topLevel.Elevation;
              }

              double height = topElevation - bottomElevation;

              Debug.Print(
                "    This wall has a total length,"
                + " height and area of {0} feet,"
                + " {1} feet and {2} square feet.",
                Util.RealString( wallLength ),
                Util.RealString( height ),
                Util.RealString( area ) );
            }
          }
        }
      }
    }

    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      UIApplication app = commandData.Application;
      UIDocument uidoc = app.ActiveUIDocument;
      Document doc = uidoc.Document;

      List<Element> rooms = new List<Element>();
      if( !Util.GetSelectedElementsOrAll(
        rooms, uidoc, typeof( Room ) ) )
      {
        Selection sel = uidoc.Selection;
        message = ( 0 < sel.Elements.Size )
          ? "Please select some room elements."
          : "No room elements found.";
        return Result.Failed;
      }
      foreach( Room room in rooms )
      {
        DetermineAdjacentElementLengthsAndWallAreas(
          room );
      }
      return Result.Failed;
    }
  }
}
