#region Header
//
// CmdLinkedFiles.cs - retrieve linked files
// in current project
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
using Autodesk.Revit.Parameters;
using CmdResult
  = Autodesk.Revit.IExternalCommand.Result;
#endregion // Namespaces

namespace BuildingCoder
{
  class CmdLinkedFiles : IExternalCommand
  {
    List<Element> GetLinkedFiles( Application app )
    {
      Autodesk.Revit.Creation.Filter cf 
        = app.Create.Filter;

      BuiltInCategory bic 
        = BuiltInCategory.OST_RvtLinks;

      Filter f1 
        = cf.NewCategoryFilter( bic );

      Filter f2 
        = cf.NewTypeFilter( typeof( Instance ) );

      Filter f3 
        = cf.NewLogicAndFilter( f1, f2 );

      List<Element> links = new List<Element>();

      Document doc = app.ActiveDocument;
      doc.get_Elements( f3, links );
      return links;
    }

    List<Element> GetLinkedFiles_2008( Application app )
    {
      BuiltInParameter bip 
        = BuiltInParameter.ELEM_TYPE_PARAM;

      Document doc = app.ActiveDocument;
      ElementIterator it = doc.Elements;

      List<Element> links = new List<Element>();

      while( it.MoveNext() )
      {
        Instance inst = it.Current as Instance;
        if( null != inst )
        {
          Parameter p = inst.get_Parameter( bip );
          ElementId id = p.AsElementId();
          Element e = doc.get_Element( ref id );
          string n = e.Name;
          string s = n.Substring( n.Length - 4 );
          if( s.ToLower().Equals( ".rvt" ) )
          {
            links.Add( inst );
          }
        }
      }
      return links;
    }

    Dictionary<string,string> GetFilePaths( 
      Application app,
      bool onlyImportedFiles )
    {
      DocumentSet docs = app.Documents;
      int n = docs.Size;

      Dictionary<string, string> dict 
        = new Dictionary<string, string>( n );

      foreach( Document doc in docs )
      {
        if( !onlyImportedFiles
          || ( null == doc.ActiveView ) )
        {
          string path = doc.PathName;
          int i = path.LastIndexOf( "\\" ) + 1;
          string name = path.Substring( i );
          dict.Add( name, path );
        }
      }
      return dict;
    }

    public CmdResult Execute( 
      ExternalCommandData commandData, 
      ref string message, 
      ElementSet elements )
    {
      Application app = commandData.Application;

      Dictionary<string, string> dict
        = GetFilePaths( app, true );

      List<Element> links
        = GetLinkedFiles( app );

      int n = links.Count;
      Debug.Print(
        "There {0} {1} linked Revit model{2}.",
        ( 1 == n ? "is" : "are" ), n,
        Util.PluralSuffix( n ) );

      string name;
      char[] sep = new char[] { ':' };
      string[] a;

      foreach( Element link in links )
      {
        name = link.Name;
        a = name.Split( sep );
        name = a[0].Trim();

        Debug.Print(
          "Link '{0}' full path is '{1}'.",
          name, dict[name] );

        #region Explore Location
        Location loc = link.Location; // unknown content in here
        LocationPoint lp = loc as LocationPoint;
        if( null != lp )
        {
          Autodesk.Revit.Geometry.XYZ p = lp.Point;
        }
        Autodesk.Revit.Geometry.Element e = link.get_Geometry( new Autodesk.Revit.Geometry.Options() );
        if( null != e ) // no geometry defined
        {
          Autodesk.Revit.Geometry.GeometryObjectArray objects = e.Objects;
          n = objects.Size;
        }
        #endregion // Explore Location
      }
      return CmdResult.Succeeded;
    }
  }
}
