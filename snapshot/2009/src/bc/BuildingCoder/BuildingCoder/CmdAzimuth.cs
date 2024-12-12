#region Header
//
// CmdAzimuth.cs - determine direction
// of a line with regard to the north
//
// Copyright (C) 2008 by Jeremy Tammik,
// Autodesk Inc. All rights reserved.
//
#endregion // Header

#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit;
using Autodesk.Revit.Site;
using CmdResult
  = Autodesk.Revit.IExternalCommand.Result;
using XYZ = Autodesk.Revit.Geometry.XYZ;
#endregion // Namespaces

namespace BuildingCoder
{
  class CmdAzimuth : IExternalCommand
  {
    public CmdResult Execute(
      ExternalCommandData commandData,
      ref String message,
      ElementSet elements )
    {
      Application app = commandData.Application;
      Document doc = app.ActiveDocument;
      Element e = Util.SelectSingleElement( doc, "a line" );

      if( null == e )
      {
        message = "No element selected";
        return CmdResult.Failed;
      }

      LocationCurve curve
        = e.Location as LocationCurve;

      if( null == curve )
      {
        message = "No curve available";
        return CmdResult.Failed;
      }

      XYZ p = curve.Curve.get_EndPoint( 0 );
      XYZ q = curve.Curve.get_EndPoint( 1 );

      Debug.WriteLine( "Start point "
        + Util.PointString( p ) );

      Debug.WriteLine( "End point "
        + Util.PointString( q ) );

      double a = p.Angle( q );
      Debug.WriteLine(
        "Angle between start and end points = "
        + Util.AngleString( a ) );

      XYZ v = q - p;
      a = XYZ.BasisX.Angle( v );
      Debug.WriteLine(
        "Angle between points measured from X axis = "
        + Util.AngleString( a ) );

      XYZ z = XYZ.BasisZ;
      a = XYZ.BasisX.AngleAround( v, z );
      Debug.WriteLine(
        "Angle around measured from X axis = "
        + Util.AngleString( a ) );

      foreach( ProjectLocation location
        in doc.ProjectLocations )
      {
        ProjectPosition projectPosition
          = location.get_ProjectPosition( XYZ.Zero );
        double pna = projectPosition.Angle;
        Debug.WriteLine(
          "Angle between project north and true north "
          + Util.AngleString( pna ) );
      }
      return CmdResult.Succeeded;
    }
  }
}
