#region Header
//
// CmdLinkedFileElements.cs - list elements in linked files
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
using Autodesk.Revit.Structural.Enums;
using Autodesk.Revit.Symbols;
using CmdResult
  = Autodesk.Revit.IExternalCommand.Result;
using CreationFilter
  = Autodesk.Revit.Creation.Filter;
using XYZ = Autodesk.Revit.Geometry.XYZ;
#endregion // Namespaces

namespace BuildingCoder
{
  public class ElementData
  {
    string _document;
    string _elementName;
    int _id;
    double _x;
    double _y;
    double _z;
    string _uniqueId;
    string _folder;

    public ElementData(
      string path,
      string elementName,
      int id,
      double x,
      double y,
      double z,
      string uniqueId )
    {
      int i = path.LastIndexOf( "\\" );
      _document = path.Substring( i + 1 );
      _elementName = elementName;
      _id = id;
      _x = x;
      _y = y;
      _z = z;
      _uniqueId = uniqueId;
      _folder = path.Substring( 0, i );
    }

    public string Document {
      get { return _document; }
    }
    public string Element {
      get { return _elementName; }
    }
    public int Id {
      get { return _id; }
    }
    public string X {
      get { return Util.RealString( _x ); }
    }
    public string Y {
      get { return Util.RealString( _y ); }
    }
    public string Z {
      get { return Util.RealString( _z ); }
    }
    public string UniqueId {
      get { return _uniqueId; }
    }
    public string Folder {
      get { return _folder; }
    }
  }

  class CmdLinkedFileElements : IExternalCommand
  {
    /// <summary>
    /// Retrieve elements fitting given category
    /// and type from the given document.
    /// </summary>
    public List<Element> GetElements(
      BuiltInCategory bic,
      Type elemType,
      Application app,
      Document doc )
    {
      CreationFilter cf = app.Create.Filter;
      Filter f1 = cf.NewCategoryFilter( bic );
      Filter f2 = cf.NewTypeFilter( elemType );
      Filter f3 = cf.NewLogicAndFilter( f1, f2 );
      List<Element> elements = new List<Element>();
      doc.get_Elements( f3, elements );
      return elements;
    }

    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet highlightElements )
    {
      /*
      //
      // retrieve all link elements:
      //
      Document doc = app.ActiveDocument;
      List<Element> links = GetElements(
        BuiltInCategory.OST_RvtLinks,
        typeof( Instance ), app, doc );
      //
      // determine the link paths:
      //
      DocumentSet docs = app.Documents;
      int n = docs.Size;
      Dictionary<string, string> paths
        = new Dictionary<string, string>( n );

      foreach( Document d in docs )
      {
        string path = d.PathName;
        int i = path.LastIndexOf( "\\" ) + 1;
        string name = path.Substring( i );
        paths.Add( name, path );
      }
      */

      //
      // retrieve lighting fixture element
      // data from linked documents:
      //
      List<ElementData> data = new List<ElementData>();
      Application app = commandData.Application;
      DocumentSet docs = app.Documents;

      foreach( Document doc in docs )
      {
        List<Element> elements = GetElements(
          BuiltInCategory.OST_LightingFixtures,
          typeof( FamilyInstance ), app, doc );

        foreach( FamilyInstance e in elements )
        {
          string name = e.Name;
          LocationPoint lp = e.Location as LocationPoint;
          if( null != lp )
          {
            XYZ p = lp.Point;
            data.Add( new ElementData( doc.PathName, e.Name,
              e.Id.Value, p.X, p.Y, p.Z, e.UniqueId ) );
          }
        }
      }
      //
      // display data:
      //
      using( CmdLinkedFileElementsForm dlg = new CmdLinkedFileElementsForm( data ) )
      {
        dlg.ShowDialog();
      }
      //
      // this command does not modify the Revit document:
      //
      return CmdResult.Cancelled;
    }
  }
}
