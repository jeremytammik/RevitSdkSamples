#region Header
//
// CmdSlabSides.cs - determine vertical slab 'side' faces
//
// Copyright (C) 2008-2010 by Jeremy Tammik,
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
using RvtElement = Autodesk.Revit.Element;
using GeoElement = Autodesk.Revit.Geometry.Element;
using CmdResult = Autodesk.Revit.IExternalCommand.Result;
#endregion // Namespaces

namespace BuildingCoder
{
  class CmdSlabSides : IExternalCommand
  {
    /// <summary>
    /// Determine the vertical boundary faces
    /// of a given "horizontal" solid object
    /// such as a floor slab. Currently only
    /// supports planar and cylindrical faces.
    /// </summary>
    /// <param name="verticalFaces">Return solid vertical boundary faces, i.e. 'sides'</param>
    /// <param name="solid">Input solid</param>
    void GetSideFaces(
      List<Face> verticalFaces,
      Solid solid )
    {
      FaceArray faces = solid.Faces;
      foreach( Face f in faces )
      {
        if( f is PlanarFace )
        {
          if( Util.IsVertical( f as PlanarFace ) )
          {
            verticalFaces.Add( f );
          }
        }
        if( f is CylindricalFace )
        {
          if( Util.IsVertical( f as CylindricalFace ) )
          {
            verticalFaces.Add( f );
          }
        }
      }
    }

    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;
      Document doc = app.ActiveDocument;

      List<RvtElement> floors = new List<RvtElement>();
      if( !Util.GetSelectedElementsOrAll(
        floors, doc, typeof( Floor ) ) )
      {
        Selection sel = doc.Selection;
        message = ( 0 < sel.Elements.Size )
          ? "Please select some floor elements."
          : "No floor elements found.";
        return CmdResult.Failed;
      }

      List<Face> faces = new List<Face>();
      Options opt = app.Create.NewGeometryOptions();

      foreach( Floor floor in floors )
      {
        GeoElement geo = floor.get_Geometry( opt );
        GeometryObjectArray objects = geo.Objects;
        foreach( GeometryObject obj in objects )
        {
          Solid solid = obj as Solid;
          if( solid != null )
          {
            GetSideFaces( faces, solid );
          }
        }
      }

      int n = faces.Count;

      Debug.Print(
        "{0} side face{1} found.",
        n, Util.PluralSuffix( n ) );

      Creator creator = new Creator( app );
      foreach( Face f in faces )
      {
        creator.DrawFaceTriangleNormals( f );
      }
      return CmdResult.Succeeded;
    }
  }
}
