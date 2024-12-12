#region Header
//
// CmdDetailCurves.cs - create detail curves
//
// Copyright (C) 2009-2010 by Jeremy Tammik,
// Autodesk Inc. All rights reserved.
//
#endregion // Header

#region Namespaces
using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Geometry;
using CmdResult = Autodesk.Revit.IExternalCommand.Result;
#endregion // Namespaces

namespace BuildingCoder
{
  class CmdDetailCurves : IExternalCommand
  {
    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;
      Document doc = app.ActiveDocument;
      View view = doc.ActiveView;

      // Create a geometry line

      XYZ startPoint = new XYZ( 0, 0, 0 );
      XYZ endPoint = new XYZ( 10, 10, 0 );

      Line geomLine = app.Create.NewLine(
        startPoint, endPoint, true );

      // Create a geometry arc

      XYZ end0 = new XYZ( 1, 0, 0 );
      XYZ end1 = new XYZ( 10, 10, 10 );
      XYZ pointOnCurve = new XYZ( 10, 0, 0 );

      Arc geomArc = app.Create.NewArc(
        end0, end1, pointOnCurve );

      // Create a geometry plane

      XYZ origin = new XYZ( 0, 0, 0 );
      XYZ normal = new XYZ( 1, 1, 0 );

      Plane geomPlane = app.Create.NewPlane(
        normal, origin );

      // Create a sketch plane in current document

      SketchPlane sketch = doc.Create.NewSketchPlane(
        geomPlane );

      // Create a DetailLine element using the 
      // newly created geometry line and sketch plane

      DetailLine line = doc.Create.NewDetailCurve(
        view, geomLine ) as DetailLine;

      // Create a DetailArc element using the 
      // newly created geometry arc and sketch plane

      DetailArc arc = doc.Create.NewDetailCurve(
        view, geomArc ) as DetailArc;

      // Change detail curve colour.
      // Initially, this only affects the newly 
      // created curves. However, when the view 
      // is refreshed, all detail curves will 
      // be updated.

      GraphicsStyle gs = arc.LineStyle as GraphicsStyle;

      gs.GraphicsStyleCategory.LineColor 
        = new Color( 250, 10, 10 );

      return CmdResult.Succeeded;
    }
  }
}
