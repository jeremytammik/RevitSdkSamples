#region Header
//
// CmdColumnRound.cs - determine whether a
// selected column instance is cylindrical
//
// Copyright (C) 2009-2010 by Jeremy Tammik,
// Autodesk Inc. All rights reserved.
//
#endregion // Header

#region Namespaces
using System;
using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Geometry;
using Autodesk.Revit.Structural;
using Autodesk.Revit.Symbols;
using RvtElement = Autodesk.Revit.Element;
using GeoElement = Autodesk.Revit.Geometry.Element;
using GeoInstance = Autodesk.Revit.Geometry.Instance;
using CmdResult
  = Autodesk.Revit.IExternalCommand.Result;
#endregion // Namespaces

namespace BuildingCoder
{
  class CmdColumnRound : IExternalCommand
  {

#if REQUIRES_REVIT_2009_API
    //
    // works in Revit Structure 2009 API, but not in 2010:
    //
    bool IsColumnRound(
      FamilySymbol symbol )
    {
      GenericFormSet solid = symbol.Family.SolidForms;
      GenericFormSetIterator i = solid.ForwardIterator();
      i.MoveNext();
      Extrusion extr = i.Current as Extrusion;
      CurveArray cr = extr.Sketch.CurveLoop;
      CurveArrayIterator i2 = cr.ForwardIterator();
      i2.MoveNext();
      String s = i2.Current.GetType().ToString();
      return s.Contains( "Arc" );
    }
#endif // REQUIRES_REVIT_2009_API

    //
    // works in Revit Structure, but not in other flavours of Revit:
    //
    bool ContainsArc( AnalyticalModelFrame a )
    {
      bool rc = false;
      AnalyticalModelProfile amp = a.Profile;
      Profile p = amp.SweptProfile;
      foreach( Curve c in p.Curves )
      {
        if( c is Arc )
        {
          rc = true;
          break;
        }
      }
      return rc;
    }

    /// <summary>
    /// Return true if the given Revit element looks
    /// like it might be a column family instance.
    /// </summary>
    bool IsColumn( RvtElement e )
    {
      return e is FamilyInstance
        && null != e.Category
        && e.Category.Name.ToLower().Contains( "column" );
    }

    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;
      Document doc = app.ActiveDocument;

      Selection sel = doc.Selection;
      RvtElement column = null;

      if( 1 == sel.Elements.Size )
      {
        foreach( RvtElement e in sel.Elements )
        {
          column = e;
        }
        if( !IsColumn( column ) )
        {
          column = null;
        }
      }

      if( null == column )
      {
        sel.Elements.Clear();
        sel.StatusbarTip = "Please select a column";
        if( sel.PickOne() )
        {
          ElementSetIterator i
            = sel.Elements.ForwardIterator();
          i.MoveNext();
          column = i.Current as RvtElement;
        }
        if( !IsColumn( column ) )
        {
          message = "Please select a single column instance";
        }
      }

      if( null != column )
      {
        Options opt = app.Create.NewGeometryOptions();
        GeoElement geo = column.get_Geometry( opt );
        GeometryObjectArray objects = geo.Objects;
        GeoInstance i = null;
        foreach( GeometryObject obj in objects )
        {
          i = obj as GeoInstance;
          if( null != i )
          {
            break;
          }
        }
        if( null == i )
        {
          message = "Unable to obtain geometry instance";
        }
        else
        {
          bool isCylindrical = false;
          geo = i.SymbolGeometry;
          objects = geo.Objects;
          foreach( GeometryObject obj in objects )
          {
            Solid solid = obj as Solid;
            if( null != solid )
            {
              foreach( Face face in solid.Faces )
              {
                if( face is CylindricalFace )
                {
                  isCylindrical = true;
                  break;
                }
              }
            }
          }
          message = string.Format(
            "Selected column instance is{0} cylindrical",
            ( isCylindrical ? "" : " NOT" ) );
        }
      }
      return CmdResult.Failed;
    }
  }
}
