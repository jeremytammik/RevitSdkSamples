#region Header
//
// CmdListViews.cs - determine all the view 
// ports of a drawing sheet and vice versa
//
// Copyright (C) 2009 by Jeremy Tammik,
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
  class CmdListViews : IExternalCommand
  {
    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;
      Document doc = app.ActiveDocument;

      List<Element> sheets = new List<Element>();

      doc.get_Elements( typeof( ViewSheet ), sheets );

      // map with key = sheet element id and
      // value = list of viewport element ids:

      Dictionary<ElementId, List<ElementId>> 
        mapSheetToViewport =
        new Dictionary<ElementId, List<ElementId>>();

      // map with key = viewport element id and
      // value = sheet element id:

      Dictionary<ElementId, ElementId> 
        mapViewportToSheet =
        new Dictionary<ElementId, ElementId>();

      foreach( ViewSheet sheet in sheets )
      {
        int n = sheet.Views.Size;

        Debug.Print( 
          "Sheet {0} contains {1} view{2}: ",
          Util.ElementDescription( sheet ),
          n, Util.PluralSuffix( n ) );

        ElementId idSheet = sheet.Id;

        foreach( View v in sheet.Views )
        {
          Debug.WriteLine( "  Viewport " 
            + Util.ElementDescription( v ) );

          if( !mapSheetToViewport.ContainsKey( idSheet ) )
          {
            mapSheetToViewport.Add( idSheet, 
              new List<ElementId>() );
          }
          mapSheetToViewport[idSheet].Add( v.Id );

          Debug.Assert( 
            !mapViewportToSheet.ContainsKey( v.Id ), 
            "expected viewport to be contained"
            + " in only one single sheet" );

          mapViewportToSheet.Add( v.Id, idSheet );
        }
      }
      return CmdResult.Cancelled;
    }
  }
}
