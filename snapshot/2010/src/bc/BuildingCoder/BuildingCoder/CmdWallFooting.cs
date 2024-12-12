#region Header
//
// CmdWallFooting.cs - determine wall footing from wall.
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
using CmdResult
  = Autodesk.Revit.IExternalCommand.Result;
#endregion // Namespaces

namespace BuildingCoder
{
  class CmdWallFooting : IExternalCommand
  {
    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;
      Document doc = app.ActiveDocument;

      ContFooting footing = null;

      Wall wall = Util.SelectSingleElementOfType(
        doc, typeof( Wall ), "a wall" ) as Wall;

      if ( null == wall )
      {
        message = "Please select a single wall element.";
      }
      else
      {
        doc.BeginTransaction();
        ElementIdSet delIds = null;
        try
        {
          delIds = doc.Delete( wall );
        }
        catch ( System.Exception )
        {
          message = "Deletion failed.";
          doc.AbortTransaction();
          return CmdResult.Failed;
        }
        doc.AbortTransaction();

        foreach ( ElementId id in delIds )
        {
          ElementId refId = id;
          Element elem = doc.get_Element( ref refId );
          if ( null == elem )
            continue;
          footing = elem as ContFooting;
          if ( null != footing )
            break;
        }
      }
      string s = Util.ElementDescription( wall );

      Util.InfoMsg( ( null == footing )
        ? string.Format( "No footing found for {0}.", s )
        : string.Format( "{0} has {1}.", s,
          Util.ElementDescription( footing ) ) );

      return CmdResult.Failed;
    }
  }
}
