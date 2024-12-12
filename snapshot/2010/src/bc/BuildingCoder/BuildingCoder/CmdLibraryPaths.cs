#region Header
//
// CmdLibraryPaths.cs - update the application options library paths
//
// Copyright (C) 2009-2010 by Jeremy Tammik,
// Autodesk Inc. All rights reserved.
//
#endregion // Header

#region Namespaces
using System;
using System.Diagnostics;
using Autodesk.Revit;
using Autodesk.Revit.Collections;
using CmdResult = Autodesk.Revit.IExternalCommand.Result;
#endregion // Namespaces

namespace BuildingCoder
{
  class CmdLibraryPaths : IExternalCommand
  {
    void PrintMap( StringStringMap map, string description )
    {
      Debug.Print( "\n{0}:\n", description );

      StringStringMapIterator it = map.ForwardIterator();

      while( it.MoveNext() )
      {
        Debug.Print( "{0} -> {1}", it.Key, it.Current );
      }
    }

    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;

      StringStringMap map = app.Options.LibraryPaths;

      PrintMap( map, "Initial application options library paths" );

      string key = "ImperialTestCreate";
      string value = @"C:\Documents and Settings\All Users\Application Data\Autodesk\RAC 2010\Imperial Library\Detail Components";

      map.Insert( key, value );

      PrintMap( map, "After adding 'ImperialTestCreate' key" );

      map.set_Item( key, @"C:\Temp" );

      PrintMap( map, "After modifying 'ImperialTestCreate' key" );

      map.set_Item( "Metric Detail Library", @"C:\Temp" );

      PrintMap( map, "After modifying 'Metric Detail Library' key" );

      app.Options.LibraryPaths = map;

      return CmdResult.Succeeded;
    }
  }
}
