#region Header
//
// CmdIdling.cs - subscribe to the Idling event
//
// Copyright (C) 2010 by Jeremy Tammik, Autodesk Inc. All rights reserved.
//
#endregion // Header

#region Namespaces
using System;
using System.Diagnostics;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
#endregion // Namespaces

namespace BuildingCoder
{
  [Transaction( TransactionMode.ReadOnly )]
  [Regeneration( RegenerationOption.Manual )]
  class CmdIdling : IExternalCommand
  {
    void Log( string msg )
    {
      string dt = DateTime.Now.ToString( "u" );
      Debug.Print( dt + " " + msg );
    }

    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Log( "Execute begin" );

      UIApplication uiapp = commandData.Application;

      uiapp.Idling 
        += new EventHandler<IdlingEventArgs>( 
          OnIdling );

      Log( "Execute end" );

      return Result.Succeeded;
    }

    void OnIdling( object sender, IdlingEventArgs e )
    {
      // access active document from sender:

      Application app = sender as Application;

      Debug.Assert( null != app, 
        "expected a valid Revit application instance" );

      if( app != null )
      {
        UIApplication uiapp = new UIApplication( app );
        UIDocument uidoc = uiapp.ActiveUIDocument;
        Document doc = uidoc.Document;

        Log( "OnIdling with active document " 
          + doc.Title );
      }
    }
  }
}
