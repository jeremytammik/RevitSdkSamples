#region Header
//
// CmdCoordsOfViewOnSheet.cs - retrieve coordinatess of view on sheet
//
// Copyright (C) 2009-2010 by Jeremy Tammik,
// Autodesk Inc. All rights reserved.
//
#endregion // Header

#region Namespaces
using System;
using System.Diagnostics;
using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Geometry;
using CmdResult
  = Autodesk.Revit.IExternalCommand.Result;
#endregion // Namespaces

namespace BuildingCoder
{
  class CmdCoordsOfViewOnSheet : IExternalCommand
  {
    public CmdResult Execute(
      ExternalCommandData commandData,
      ref String message,
      ElementSet elements )
    {
      Application app = commandData.Application;
      Document doc = app.ActiveDocument;

      ViewSheet currentSheet 
        = doc.ActiveView as ViewSheet;

      foreach( View v in currentSheet.Views )
      {
        // the values returned here do not seem to 
        // accurately reflect the positions of the 
        // views on the sheet:

        BoundingBoxUV loc = v.Outline;

        Debug.Print(
          "Coordinates of {0} view '{1}': {2}",
          v.ViewType, v.Name, 
          Util.PointString( loc.Min ) );
      }

      return CmdResult.Failed;
    }
  }
}
