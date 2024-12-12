#region Header
//
// CmdRelationshipInverter.cs
//
// Determine door and window to wall relationships,
// i.e. hosted --> host, and invert it to obtain
// a map host --> list of hosted elements.
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
using Autodesk.Revit.Parameters;
using CmdResult
  = Autodesk.Revit.IExternalCommand.Result;
#endregion // Namespaces

namespace BuildingCoder
{
  public class CmdRelationshipInverter : IExternalCommand
  {
    private Document m_doc;

    string ElementDescription( ElementId id )
    {
      Element e = m_doc.get_Element( ref id );
      return Util.ElementDescription( e );
    }

    /// <summary>
    /// From a list of openings, determine
    /// the wall hoisting each and return a mapping
    /// of element ids from host to all hosted.
    /// </summary>
    /// <param name="elements">Hosted elements</param>
    /// <returns>Map of element ids from host to
    /// hosted</returns>
    private Dictionary<ElementId, List<ElementId>>
      getElementIds( List<Element> elements )
    {
      Dictionary<ElementId, List<ElementId>> dict =
        new Dictionary<ElementId, List<ElementId>>();

      string fmt = "{0} is hosted by {1}";

      foreach( FamilyInstance fi in elements )
      {
        ElementId id = fi.Id;
        ElementId idHost = fi.Host.Id;

        Debug.Print( fmt,
          Util.ElementDescription( fi ),
          ElementDescription( idHost ) );

        if( !dict.ContainsKey( idHost ) )
        {
          dict.Add( idHost, new List<ElementId>() );
        }
        dict[idHost].Add( id );
      }
      return dict;
    }

    private void dumpHostedElements(
      Dictionary<ElementId, List<ElementId>> ids )
    {
      foreach( ElementId idHost in ids.Keys )
      {
        string s = string.Empty;

        foreach( ElementId id in ids[idHost] )
        {
          if( 0 < s.Length )
          {
            s += ", ";
          }
          s += ElementDescription( id );
        }

        int n = ids[idHost].Count;

        Debug.Print(
          "{0} hosts {1} opening{2}: {3}",
          ElementDescription( idHost ),
          n, Util.PluralSuffix( n ), s );
      }
    }

    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;
      m_doc = app.ActiveDocument;

      // f5 = f1 && f4
      //    = f1 && (f2 || f3)
      //    = family instance and (door or window)
      Autodesk.Revit.Creation.Filter cf
        = app.Create.Filter;

      Filter f1 = cf.NewTypeFilter(
        typeof( FamilyInstance ) );

      Filter f2 = cf.NewCategoryFilter(
        BuiltInCategory.OST_Doors );
      Filter f3 = cf.NewCategoryFilter(
        BuiltInCategory.OST_Windows );

      Filter f4 = cf.NewLogicOrFilter( f2, f3 );
      Filter f5 = cf.NewLogicAndFilter( f1, f4 );

      List<Element> openings = new List<Element>();
      int n = m_doc.get_Elements( f5, openings );

      // map with key = host element id and
      // value = list of hosted element ids:
      Dictionary<ElementId, List<ElementId>> ids =
        getElementIds( openings );

      dumpHostedElements( ids );
      m_doc = null;

      return CmdResult.Succeeded;
    }
  }
}
