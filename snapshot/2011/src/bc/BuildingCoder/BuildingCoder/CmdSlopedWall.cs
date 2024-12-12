#region Header
//
// CmdSlopedWall.cs - create a sloped wall
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
  class CmdSlopedWall : IExternalCommand
  {
    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      UIApplication app = commandData.Application;

      Autodesk.Revit.Creation.Application ac
        = app.Application.Create;

      CurveArray profile = ac.NewCurveArray();

      double length = 10;
      double heightStart = 5;
      double heightEnd = 8;

      XYZ p = XYZ.Zero;
      XYZ q = ac.NewXYZ( length, 0.0, 0.0 );

      profile.Append( ac.NewLineBound( p, q ) );

      p = q;
      q += heightEnd * XYZ.BasisZ;

      profile.Append( ac.NewLineBound( p, q ) );

      p = q;
      q = ac.NewXYZ( 0.0, 0.0, heightStart );

      profile.Append( ac.NewLineBound( p, q ) );

      p = q;
      q = XYZ.Zero;

      profile.Append( ac.NewLineBound( p, q ) );

      Document doc = app.ActiveUIDocument.Document;

      Wall wall = doc.Create.NewWall( profile, false );

      return Result.Succeeded;
    }
  }
}
