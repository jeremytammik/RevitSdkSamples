#region Header
//
// CmdSlopedWall.cs - create a sloped wall
//
// Copyright (C) 2009 by Jeremy Tammik,
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
using CmdResult
  = Autodesk.Revit.IExternalCommand.Result;
#endregion // Namespaces

namespace BuildingCoder
{
  class CmdSlopedWall : IExternalCommand
  {
    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;

      Autodesk.Revit.Creation.Application ac 
        = app.Create;

      CurveArray profile = ac.NewCurveArray();

      double length = 10;
      double heightStart = 5;
      double heightEnd = 8;

      XYZ p = ac.NewXYZ( 0.0, 0.0, 0.0 );
      XYZ q = ac.NewXYZ( length, 0.0, 0.0 );
        
      profile.Append( ac.NewLineBound( p, q ) );

      p.X = q.X;
      q.Z = heightEnd;

      profile.Append( ac.NewLineBound( p, q ) );

      p.Z = q.Z;
      q.X = 0.0;
      q.Z = heightStart;

      profile.Append( ac.NewLineBound( p, q ) );

      p.X = q.X;
      p.Z = q.Z;
      q.Z = 0.0;

      profile.Append( ac.NewLineBound( p, q ) );

      Document doc = app.ActiveDocument;

      Wall wall = doc.Create.NewWall( profile, 
        false );

      return CmdResult.Succeeded;
    }
  }
}
