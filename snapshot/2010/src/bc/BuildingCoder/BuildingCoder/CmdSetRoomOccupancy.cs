#region Header
//
// CmdSetRoomOccupancy.cs - read and set room occupancy
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
using Autodesk.Revit.Collections;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Parameters;
using CmdResult
  = Autodesk.Revit.IExternalCommand.Result;
#endregion // Namespaces

namespace BuildingCoder
{
  class CmdSetRoomOccupancy : IExternalCommand
  {
    static char[] _digits = null;

    /// <summary>
    /// Analyse the given string.
    /// If it ends in a sequence of digits representing a number, 
    /// return a string with the number oincremented by one.
    /// Otherwise, return a string with a suffix "1" appended.
    /// </summary>
    static string BumpStringSuffix( string s )
    {
      if( null == s || 0 == s.Length )
      {
        return "1";
      }
      if( null == _digits )
      {
        _digits = new char[] {
          '0', '1', '2', '3', '4', 
          '5', '6', '7', '8', '9'
        };
      }
      int n = s.Length;
      string t = s.TrimEnd( _digits );
      if( t.Length == n )
      {
        t += "1";
      }
      else
      {
        n = t.Length;
        n = int.Parse( s.Substring( n ) );
        ++n;
        t += n.ToString();
      }
      return t;
    }

    /// <summary>
    /// Read the value of the element ROOM_OCCUPANCY parameter.
    /// If it ends in a number, increment the number, else append "1".
    /// </summary>
    static void BumpOccupancy( Element e )
    {
      Parameter p = e.get_Parameter( 
        BuiltInParameter.ROOM_OCCUPANCY );

      if( null == p )
      {
        Debug.Print(
          "{0} has no room occupancy parameter.",
          Util.ElementDescription( e ) );
      }
      else
      {
        string occupancy = p.AsString();

        string newOccupancy = BumpStringSuffix( 
          occupancy );

        p.Set( newOccupancy );
      }
    }

    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;
      Document doc = app.ActiveDocument;

      List<Element> rooms = new List<Element>();
      if( !Util.GetSelectedElementsOrAll(
        rooms, doc, typeof( Room ) ) )
      {
        Selection sel = doc.Selection;
        message = ( 0 < sel.Elements.Size )
          ? "Please select some room elements."
          : "No room elements found.";
        return CmdResult.Failed;
      }
      foreach( Room room in rooms )
      {
        BumpOccupancy( room );
      }
      return CmdResult.Succeeded;
    }
  }
}
