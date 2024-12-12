#region Header
//
// CmdMirror.cs - mirror some elements.
//
// Copyright (C) 2009-2010 by Jeremy Tammik,
// Autodesk Inc. All rights reserved.
//
#endregion // Header

#region Namespaces
using System;
using System.Collections.Generic;
using Autodesk.Revit;
using Line = Autodesk.Revit.Geometry.Line;
using XYZ = Autodesk.Revit.Geometry.XYZ;
using CmdResult = Autodesk.Revit.IExternalCommand.Result;
#endregion // Namespaces

namespace BuildingCoder
{
  class CmdMirror : IExternalCommand
  {
    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;
      Document doc = app.ActiveDocument;

      ElementSet els = doc.Selection.Elements;

      Line line = app.Create.NewLine(
        XYZ.Zero, XYZ.BasisX, true );

      doc.Mirror( els, line );

      return CmdResult.Succeeded;
    }
  }

  class CmdMirrorListAdded : IExternalCommand
  {
    int GetElementCount( Document doc )
    {
      int count = 0;
      ElementIterator it = doc.Elements;
      while( it.MoveNext() ) 
      { 
        ++count; 
      }
      return count;
    }

    List<Element> GetElementsAfter( int n, Document doc )
    {
      List<Element> a = new List<Element>( n );
      ElementIterator it = doc.Elements;
      int i = 0;

      while( it.MoveNext() ) 
      {
        ++i;

        if( n < i )
        {
          a.Add( it.Current as Element );
        }
      }
      return a;
    }

    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;
      Document doc = app.ActiveDocument;

      ElementSet els = doc.Selection.Elements;

      Line line = app.Create.NewLine( 
        XYZ.Zero, XYZ.BasisX, true );

      int n = GetElementCount( doc );

      doc.Mirror( els, line );

      List<Element> a = GetElementsAfter( n, doc );

      string s = "The following elements were mirrored:\r\n";

      foreach( Element e in a )
      {
        s += string.Format( "\r\n  {0}",
          Util.ElementDescription( e ) );
      }
      Util.InfoMsg( s );

      return CmdResult.Succeeded;
    }
  }
}
