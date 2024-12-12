#region Header
//
// CmdBoundingBox.cs - eplore element bounding box
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
using BoundingBoxXYZ = Autodesk.Revit.Geometry.BoundingBoxXYZ;
using Transform = Autodesk.Revit.Geometry.Transform;
using XYZ = Autodesk.Revit.Geometry.XYZ;
#endregion // Namespaces

namespace BuildingCoder
{
  class CmdBoundingBox : IExternalCommand
  {
    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;
      Document doc = app.ActiveDocument;

      Element e = Util.SelectSingleElement(
        doc, "an element" );

      if( null == e )
      {
        message = "No element selected";
        return CmdResult.Failed;
      }

      //
      // trying to call this property returns the
      // compile time error: Property, indexer, or
      // event 'BoundingBox' is not supported by
      // the language; try directly calling
      // accessor method 'get_BoundingBox( View )'
      //
      //BoundingBoxXYZ b = e.BoundingBox[null];

      BoundingBoxXYZ b = e.get_BoundingBox( null );

      Debug.Assert( b.Transform.IsIdentity,
        "expected identity Element bounding box transform" );

      Debug.Print(
        "Element bounding box of {0}"
        + " extends from {1} to {2}.",
        Util.ElementDescription( e ),
        Util.PointString( b.Min ),
        Util.PointString( b.Max ) );

      return CmdResult.Succeeded;
    }
  }
}
