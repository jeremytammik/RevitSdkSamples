#region Header
//
// CmdPlanTopology.cs - test PlanTopology class
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

#endregion // Namespaces

namespace BuildingCoder
{
  [Transaction( TransactionMode.Automatic )]
  [Regeneration( RegenerationOption.Manual )]
  class CmdPlanTopology : IExternalCommand
  {
    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      UIApplication app = commandData.Application;
      Document doc = app.ActiveUIDocument.Document;

      FilteredElementCollector levels = Util.GetElementsOfType(
        doc, typeof( Level ), BuiltInCategory.OST_Levels );

      Level level = levels.FirstElement() as Level;

      PlanTopology pt = doc.get_PlanTopology( level );

      // collect some data on each room in the circuit
      string output = "Rooms on "
        + level.Name + ":"
        + "\n  Name and Number : Area";

      foreach( Room r in pt.Rooms )
      {
        output += "\n  " + r.Name + " : "
          + Util.RealString( r.Area ) + " sqf";
      }
      Util.InfoMsg( output );

      output = "Circuits without rooms:"
        + "\n  Number of Sides : Area";

      foreach( PlanCircuit pc in pt.Circuits )
      {
        if( !pc.IsRoomLocated ) // this circuit has no room, create one
        {
          output += "\n  " + pc.SideNum + " : "
            + Util.RealString( pc.Area ) + " sqf";

          // pass null to create a new room;
          // to place an existing unplaced room,
          // pass it in instead of null:

          Room r = doc.Create.NewRoom( null, pc );
        }
      }
      Util.InfoMsg( output );

      return Result.Succeeded;
    }
  }
}
