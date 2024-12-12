#region Header
//
// CmdWallNeighbours.cs - determine wall
// neighbours, i.e. walls joined at end points
//
// Copyright (C) 2008 by Jeremy Tammik,
// Autodesk Inc. All rights reserved.
//
#endregion // Header

#region Namespaces
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit;
using Autodesk.Revit.Elements;
using CmdResult
  = Autodesk.Revit.IExternalCommand.Result;
#endregion // Namespaces

namespace BuildingCoder
{
  class CmdWallNeighbours : IExternalCommand
  {
    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;
      Document doc = app.ActiveDocument;

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

      int i, n;
      string desc, s = null;
      List<Element> neighbours;

      foreach( Wall wall in walls )
      {
        desc = Util.ElementDescription( wall );

        LocationCurve c
          = wall.Location as LocationCurve;

        if( null == c )
        {
          s = desc + ": No wall curve found.";
        }
        else
        {
          s = string.Empty;

          for( i = 0; i < 2; ++i )
          {
            neighbours = c.get_ElementsAtJoin( i );
            n = neighbours.Count;

            s += string.Format(
              "\n\n{0} {1} point has {2} neighbour{3}{4}",
              desc,
              (0 == i ? "start" : "end"),
              n,
              Util.PluralSuffix( n ),
              Util.DotOrColon( n ) );

            foreach( Wall nb in neighbours )
            {
              s += "\n  " +
                Util.ElementDescription( nb );
            }
          }
        }
        Util.InfoMsg( s );
      }
      return CmdResult.Failed;
    }
  }
}
