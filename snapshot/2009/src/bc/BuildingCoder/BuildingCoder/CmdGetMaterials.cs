#region Header
//
// CmdGetMaterials.cs - determine element materials
// by iterating over its geometry faces
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
using Autodesk.Revit.Elements;
using Autodesk.Revit.Geometry;
using CmdResult = Autodesk.Revit.IExternalCommand.Result;
using RvtElement = Autodesk.Revit.Element;
using GeoElement = Autodesk.Revit.Geometry.Element;
using GeoInstance = Autodesk.Revit.Geometry.Instance;
#endregion // Namespaces

namespace BuildingCoder
{
  class CmdGetMaterials : IExternalCommand
  {
    public List<string> GetMaterials( GeoElement geo )
    {
      List<string> materials = new List<string>();
      foreach( GeometryObject o in geo.Objects )
      {
        if( o is Solid )
        {
          Solid solid = o as Solid;
          foreach( Face face in solid.Faces )
          {
            string s = face.MaterialElement.Name;
            materials.Add( s );
          }
        }
        else if( o is GeoInstance )
        {
          GeoInstance i = o as GeoInstance;
          materials.AddRange( GetMaterials(
            i.SymbolGeometry ) );
        }
      }
      return materials;
    }

    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;
      Document doc = app.ActiveDocument;
      Selection sel = doc.Selection;
      Options opt = app.Create.NewGeometryOptions();
      string msg = string.Empty;
      int i, n;

      foreach( RvtElement e in sel.Elements )
      {
        GeoElement geo = e.get_Geometry( opt );
        List<string> materials = GetMaterials( geo );

        msg += "\n" + Util.ElementDescription( e );

        n = materials.Count;
        if( 0 == n )
        {
          msg += " has no materials.";
        }
        else
        {
          i = 0;
          msg += string.Format(
            " has {0} material{1}:",
            n, Util.PluralSuffix( n ) );

          foreach( string s in materials )
          {
            msg += string.Format(
              "\n  {0} {1}", i++, s );
          }
        }
      }
      if( 0 == msg.Length )
      {
        msg = "Please select some elements.";
      }
      Util.InfoMsg( msg );
      return CmdResult.Succeeded;
    }
  }
}
