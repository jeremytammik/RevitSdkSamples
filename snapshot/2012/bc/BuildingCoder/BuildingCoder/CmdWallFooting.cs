#region Header
//
// CmdWallFooting.cs - determine wall footing from wall.
//
// Copyright (C) 2009-2011 by Jeremy Tammik,
// Autodesk Inc. All rights reserved.
//
#endregion // Header

#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

#endregion // Namespaces

namespace BuildingCoder
{
  [Transaction( TransactionMode.Automatic )]
  class CmdWallFooting : IExternalCommand
  {
    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      UIApplication app = commandData.Application;
      UIDocument uidoc = app.ActiveUIDocument;
      Document doc = uidoc.Document;

      ContFooting footing = null;

      Wall wall = Util.SelectSingleElementOfType(
        uidoc, typeof( Wall ), "a wall", false ) as Wall;

      if ( null == wall )
      {
        message = "Please select a single wall element.";
      }
      else
      {
        SubTransaction t = new SubTransaction( doc );
        ICollection<ElementId> delIds = null;
        try
        {
          t.Start();
          delIds = doc.Delete( wall );
          t.RollBack();
        }
        catch ( Exception ex )
        {
          message = "Deletion failed: " + ex.Message;
          t.RollBack();
          return Result.Failed;
        }

        foreach ( ElementId id in delIds )
        {
          Element elem = doc.get_Element( id );
          if( null != elem )
          {
            footing = elem as ContFooting;
            if( null != footing )
              break;
          }
        }
      }
      string s = Util.ElementDescription( wall );

      Util.InfoMsg( ( null == footing )
        ? string.Format( "No footing found for {0}.", s )
        : string.Format( "{0} has {1}.", s,
          Util.ElementDescription( footing ) ) );

      return Result.Failed;
    }
  }
}
