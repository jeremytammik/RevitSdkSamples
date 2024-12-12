#region Header
//
// CmdNewDimensionLabel.cs - create a new dimension label in a family document
//
// Copyright (C) 2010 by Jeremy Tammik,
// Autodesk Inc. All rights reserved.
//
#endregion // Header

#region Namespaces
using System.Collections.Generic;
using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Geometry;
using Autodesk.Revit.Parameters;
using CmdResult = Autodesk.Revit.IExternalCommand.Result;
using Element = Autodesk.Revit.Element;
#endregion // Namespaces

namespace BuildingCoder
{
  class CmdNewDimensionLabel : IExternalCommand
  {
    /// <summary>
    /// Return a sketch plane from the given document with 
    /// the specified normal vector, if one exists, else null.
    /// </summary>
    static SketchPlane findSketchPlane( 
      Document doc, 
      XYZ normal )
    {
      SketchPlane result = null;

      List<Element> a = new List<Element>();

      int n = doc.get_Elements( 
        typeof( SketchPlane ), a );

      foreach( SketchPlane e in a )
      {
        if( e.Plane.Normal.AlmostEqual( normal ) )
        {
          result = e;
          break;
        }
      }
      return result;
    }

    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;
      Document doc = app.ActiveDocument;

      SketchPlane skplane = findSketchPlane( doc, XYZ.BasisZ );

      if( null == skplane )
      {
        Plane geometryPlane = app.Create.NewPlane(
          XYZ.BasisZ, XYZ.Zero );

        skplane = doc.FamilyCreate.NewSketchPlane(
          geometryPlane );
      }

      double length = 1.23;

      XYZ start = XYZ.Zero;
      XYZ end = app.Create.NewXYZ( 0, length, 0 );

      Line line = app.Create.NewLine( 
        start, end, true );

      ModelCurve modelCurve
        = doc.FamilyCreate.NewModelCurve(
          line, skplane );

      ReferenceArray ra = new ReferenceArray();

      ra.Append( modelCurve.GeometryCurve.Reference );

      start = app.Create.NewXYZ( length, 0, 0 );
      end = app.Create.NewXYZ( length, length, 0 );

      line = app.Create.NewLine( start, end, true );

      modelCurve = doc.FamilyCreate.NewModelCurve( 
        line, skplane );

      ra.Append( modelCurve.GeometryCurve.Reference );

      start = app.Create.NewXYZ( 0, 0.2 * length, 0 );
      end = app.Create.NewXYZ( length, 0.2 * length, 0 );

      line = app.Create.NewLine( start, end, true );

      Dimension dim 
        = doc.FamilyCreate.NewLinearDimension( 
          doc.ActiveView, line, ra );

      FamilyParameter familyParam
        = doc.FamilyManager.AddParameter(
          "length",
          BuiltInParameterGroup.PG_IDENTITY_DATA,
          ParameterType.Length, false );

      dim.Label = familyParam;

      return CmdResult.Succeeded;
    }
  }
}
