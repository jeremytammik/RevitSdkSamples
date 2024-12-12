#region Header
//
// CmdNewArea.cs - create a new area element
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
using Autodesk.Revit.Enums;
using CmdResult
  = Autodesk.Revit.IExternalCommand.Result;
using UV = Autodesk.Revit.Geometry.UV;
using XYZ = Autodesk.Revit.Geometry.XYZ;
#endregion // Namespaces

namespace BuildingCoder
{
  class CmdNewArea : IExternalCommand
  {
    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      CmdResult rc = CmdResult.Failed;

      ViewPlan view = commandData.View as ViewPlan;

      if( null == view
        || view.ViewType != ViewType.AreaPlan )
      {
        message = "Please run this command in an area plan view.";
        return rc;
      }

      Application app = commandData.Application;
      Document doc = app.ActiveDocument;

      Element room = Util.GetSingleSelectedElement( doc );

      if( null == room || !(room is Room) )
      {
        room = Util.SelectSingleElement( doc, "a room" );
      }

      if( null == room || !( room is Room ) )
      {
        message = "Please select a single room element.";
      }
      else
      {
        Location loc = room.Location;
        LocationPoint lp = loc as LocationPoint;
        XYZ p = lp.Point;
        UV q = new UV( p.X, p.Y );
        Area area = doc.Create.NewArea( view, q );
        rc = CmdResult.Succeeded;
      }
      return rc;
    }
  }
}
