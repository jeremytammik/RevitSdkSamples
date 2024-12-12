#region Header
//
// CmdGetMaterials.cs - determine element materials
// by iterating over its geometry faces
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
using Autodesk.Revit.Parameters;
using CmdResult = Autodesk.Revit.IExternalCommand.Result;
using RvtElement = Autodesk.Revit.Element;
using GeoElement = Autodesk.Revit.Geometry.Element;
using GeoInstance = Autodesk.Revit.Geometry.Instance;
#endregion // Namespaces

namespace BuildingCoder
{
  class CmdGetMaterials : IExternalCommand
  {
    /// <summary>
    /// Return family instance element material, either 
    /// for the given instance or the entire category.
    /// If no element material is specified and the 
    /// ByCategory material information is null, set 
    /// it to a valid value at the category level.
    /// </summary>
    public Material GetMaterial( 
      Document doc, 
      FamilyInstance fi )
    {
      Material material = null;

      foreach( Parameter p in fi.Parameters )
      {
        Definition def = p.Definition;

        // the material is stored as element id:

        if( p.StorageType == StorageType.ElementId
          && def.ParameterGroup == BuiltInParameterGroup.PG_MATERIALS
          && def.ParameterType == ParameterType.Material )
        {
          ElementId materialId = p.AsElementId();

          if( -1 == materialId.Value )
          {
            // invalid element id, so we assume 
            // the material is "By Category":

            if( null != fi.Category )
            {
              material = fi.Category.Material;

              if( null == material )
              {
                MaterialOther mat 
                  = doc.Settings.Materials.AddOther(
                    "GoodConditionMat" );

                mat.Color = new Color( 255, 0, 0 );

                fi.Category.Material = mat;

                material = fi.Category.Material;
              }
            }
          }
          else
          {
            material = doc.Settings.Materials.get_Item( 
              materialId );
          }
          break;
        }
      }
      return material;
    }

    /// <summary>
    /// Return a list of all materials by recursively traversing
    /// all the object's solids' faces, retrieving their face
    /// materials, and collecting them in a list.
    ///
    /// Original implementation, not always robust as noted by Andras Kiss in
    /// http://thebuildingcoder.typepad.com/blog/2008/10/family-instance-materials.html?cid=6a00e553e1689788330115713e2e3c970b#comment-6a00e553e1689788330115713e2e3c970b
    /// </summary>
    public List<string> GetMaterials1( GeoElement geo )
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
          materials.AddRange( GetMaterials1(
            i.SymbolGeometry ) );
        }
      }
      return materials;
    }

    /// <summary>
    /// Return a list of all materials by traversing
    /// all the object's and its instances' solids'
    /// faces, retrieving their face materials,
    /// and collecting them in a list.
    ///
    /// Enhanced more robust implementation suggested
    /// by Harry Mattison, but lacking the recursion
    /// of the first version.
    /// </summary>
    public List<string> GetMaterials( GeoElement geo )
    {
      List<string> materials = new List<string>();
      foreach( GeometryObject o in geo.Objects )
      {
        if( o is Solid )
        {
          Solid solid = o as Solid;
          if( null != solid )
          {
            foreach( Face face in solid.Faces )
            {
              string s = face.MaterialElement.Name;
              materials.Add( s );
            }
          }
        }
        else if( o is GeoInstance )
        {
          GeoInstance i = o as GeoInstance;
          foreach( Object geomObj in i.SymbolGeometry.Objects )
          {
            Solid solid = geomObj as Solid;
            if( solid != null )
            {
              foreach( Face face in solid.Faces )
              {
                string s = face.MaterialElement.Name;
                materials.Add( s );
              }
            }
          }
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
      Material mat;
      string msg = string.Empty;
      int i, n;

      foreach( RvtElement e in sel.Elements )
      {
        // for 310_ensure_material.htm:

        if( e is FamilyInstance )
        {
          mat = GetMaterial( doc, e as FamilyInstance );

          Util.InfoMsg( 
            "Family instance element material: " 
            + (null == mat ? "<null>" : mat.Name) );
        }

        GeoElement geo = e.get_Geometry( opt );
        //
        // if you are not interested in duplicate
        // materials, you can define a class that
        // overloads the Add method to only insert
        // a new entry if its value is not already
        // present in the list, instead of using
        // the standard List<> class:
        //
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
