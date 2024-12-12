#region Header
// RstLink
//
// Copyright (C) 2010-2013 by Autodesk, Inc.
//
// Permission to use, copy, modify, and distribute this software
// for any purpose and without fee is hereby granted, provided
// that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
#endregion // Header

#region Namespaces
using System;
using System.Diagnostics;
using System.IO;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using RstLink;
#endregion // Namespaces

namespace RstLinkRevitClient
{
  [Transaction( TransactionMode.ReadOnly )]
  class CmdLiveLink : IExternalCommand
  {
    #region Global variables
    static string _filename = "RstLinkModel.xml";
    static DateTime _last_write_time;
    #endregion // Global variables

    #region Idling event handler
    void OnIdling( object sender, IdlingEventArgs e )
    {
      if( File.Exists( _filename ) )
      {
        DateTime dt = File.GetLastWriteTime(
          _filename );

        // check whether transfer files was updated 
        // since we last looked:

        try
        {
          if( !dt.Equals( _last_write_time ) )
          {
            // we should do something here to lock 
            // the file before processing it...

            FileStream f = File.OpenRead( _filename );
            f.Close();

            // Support both 2011, where sender is an 
            // Application instance, and 2012, where 
            // it is a UIApplication instance:

            UIApplication uiapp
              = sender is UIApplication
                ? sender as UIApplication
                : new UIApplication(
                  sender as Application );

            // Access active document from sender:

            Document doc
              = uiapp.ActiveUIDocument.Document;

            Transaction t = new Transaction(
              doc, "RstLiveLink Update" );

            if( TransactionStatus.Started == t.Start() )
            {
              int n = RsLinkImport.ImportMembers(
                _filename, doc, false );

              t.Commit();

              _last_write_time = dt;
            }
            else
            {
              RstLink.Util.InfoMsg( "Starting transaction"
                + " for RstLiveLink update failed" );
            }
          }
        }
        catch( System.IO.IOException ex )
        {
          // An error accessing the file is to be expected,
          // because the Idling event is triggered faster than
          // the external application can let go of the file.
          // The second or third time Idling is called after 
          // the file was modified we can access it all right.

          string s = string.Format( "The process cannot access the file '{0}' because it is being used by another process.", _filename );
          Debug.Assert( ex.Message.Equals( s ), string.Format( "expected error '{0}'", s ) );
        }
      }
    }
    #endregion // Idling event handler

    #region External command Execute mainline
    public Result Execute(
      ExternalCommandData commandData,
      ref string msg,
      ElementSet els )
    {
      Result result = Result.Failed;

      UIApplication app = commandData.Application;

      Document doc = app.ActiveUIDocument.Document;

      // export the structural elements to an external file:

      _filename = RsLinkExport.ExportMembers( doc, _filename );

      if( null != _filename )
      {
        _last_write_time = File.GetLastWriteTime( 
          _filename );

        // start up the external application 
        // processing the exported model:

        string aca_cwd = "C:/a/j/adn/train/revit/2011/src/rst/rvt";

        // AutoCAD Architecture 2011
        //string aca_path = "C:/Program Files/Autodesk/AutoCAD Architecture 2011/acad.exe";
        //string aca_args = "/ld \"C:/Program Files/Autodesk/AutoCAD Architecture 2011/AecBase.dbx\"" 
        //  + " /p \"AutoCAD Architecture (US Metric)\"";

        // AutoCAD MEP 2012

        string aca_path = "C:/Program Files/Autodesk/AutoCAD MEP 2012/acad.exe";
        string aca_args = " /p \"AutoCAD\"";

        ProcessStartInfo info = new ProcessStartInfo( aca_path, aca_args );
        info.WorkingDirectory = aca_cwd;

        Process.Start( info );

        string instruction = string.Format( 
          "Starting up external application... "
            + "Please load and save modified RstLiveLink file '{0}'.",
          _filename );

        RstLink.Util.InfoMsg( instruction );

        // set up an Idling event monitoring changes made to the file:

        app.Idling 
          += new EventHandler<IdlingEventArgs>( 
            OnIdling );

        result = Result.Succeeded;
      }
      return result;
    }
    #endregion // External command Execute mainline
  }
}
