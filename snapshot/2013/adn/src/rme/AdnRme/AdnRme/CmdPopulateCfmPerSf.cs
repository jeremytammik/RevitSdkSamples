#region Header
// Revit MEP API sample application
//
// Copyright (C) 2007-2013 by Jeremy Tammik, Autodesk, Inc.
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
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  
// AUTODESK, INC. DOES NOT WARRANT THAT THE OPERATION OF THE 
// PROGRAM WILL BE UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject
// to restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
#endregion // Header

#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.UI;
#endregion // Namespaces

namespace AdnRme
{
  [Transaction( TransactionMode.Automatic )]
  class CmdPopulateCfmPerSf : IExternalCommand
  {
    #region Set CFM/SF
    /// <summary>
    /// Populate the value of the 'CFM per SF' variable on the given space.
    /// Throws an exception if something goes wrong.
    /// </summary>
    /// <param name="space">Given space element</param>
    static void SetCfmPerSf( Space space )
    {
      double flow = Util.GetSpaceParameterValue( space, Bip.Airflow, "Actual Supply Airflow" );
      double area = Util.GetSpaceParameterValue( space, Bip.Area, "Area" );
      double cfm = Const.SecondsPerMinute * flow;
      double cfmPerSf = cfm / area;
      Debug.WriteLine( string.Format( "Space {0} flow {1} CFM / area {2} f^2 --> {3} CFM/SF",
        space.Number, Util.RealString( cfm ), Util.RealString( area ), Util.RealString( cfmPerSf ) ) );
      Parameter pCfmPerSf = Util.GetSpaceParameter( space, ParameterName.CfmPerSf );
      pCfmPerSf.Set( cfmPerSf );
    }
    #endregion // Set CFM/SF

    #region Execute Command
    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      try
      {
        WaitCursor waitCursor = new WaitCursor();

        UIApplication app = commandData.Application;
        Document doc = app.ActiveUIDocument.Document;

        //ElementIterator it = doc.get_Elements( typeof( Room ) );
        //ElementIterator it = doc.get_Elements( typeof( Space ) ); // changed Room to Space
        //FilteredElementCollector spaces = new FilteredElementCollector( doc );
        //spaces.OfClass( typeof( Space ) );

        List<Space> spaces = Util.GetSpaces( doc );

        foreach( Space space in spaces )
        {
          SetCfmPerSf( space ); // set CFM/SF on the space AFTER assigning flow to the terminals
        }
        return Result.Succeeded;
      }
      catch( Exception ex )
      {
        message = ex.Message;
        return Result.Failed;
      }
    }
    #endregion // Execute Command
  }
}
