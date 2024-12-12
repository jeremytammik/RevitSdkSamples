#region Header
//
// CmdRotatedBeamLocation.cs - determine location of rotated beam
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
//using Autodesk.Revit.Geometry;
using Autodesk.Revit.Structural;
using Autodesk.Revit.Symbols;
using Curve = Autodesk.Revit.Geometry.Curve;
using Line = Autodesk.Revit.Geometry.Line;
using Plane = Autodesk.Revit.Geometry.Plane;
using XYZ = Autodesk.Revit.Geometry.XYZ;
using XYZArray = Autodesk.Revit.Geometry.XYZArray;
using CmdResult = Autodesk.Revit.IExternalCommand.Result;
#endregion // Namespaces

// C:\a\doc\revit\blog\img\three_beams.png
// C:\a\doc\revit\blog\img\rotated_beam.jpg

namespace BuildingCoder
{
  class CmdRotatedBeamLocation : IExternalCommand
  {
    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;
      Document doc = app.ActiveDocument;

      FamilyInstance beam = Util.SelectSingleElementOfType(
        doc, typeof( FamilyInstance ), "a beam" ) as FamilyInstance;

      BuiltInCategory bic 
        = BuiltInCategory.OST_StructuralFraming;

      if( null == beam
        || null == beam.Category 
        || !beam.Category.Id.Value.Equals( (int) bic ) )
      {
        message = "Please select a single beam element.";
      }
      else
      {
        LocationCurve curve
          = beam.Location as LocationCurve;

        if( null == curve )
        {
          message = "No curve available";
          return CmdResult.Failed;
        }

        XYZ p = curve.Curve.get_EndPoint( 0 );
        XYZ q = curve.Curve.get_EndPoint( 1 );
        XYZ v = 0.1 * (q - p);
        p = p - v;
        q = q + v;

        Creator creator = new Creator( app );
        creator.CreateModelLine( p, q );
      }
      return CmdResult.Succeeded;
    }
  }
}
