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
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
#endregion // Namespaces

namespace AdnRme
{
  [Transaction( TransactionMode.ReadOnly )]
  class CmdUnhostedElements : IExternalCommand
  {
    #region Determine unhosted elements
    static bool IsValidHost( string hostValue )
    {
      bool rc = ( "<not associated>" != hostValue )
        && ( "None" != hostValue );
      return rc;
    }

    /// <summary>
    /// Determine unhosted elements.
    /// </summary>
    static bool DetermineUnhostedElements( Document doc )
    {
      int nHosted = 0;
      int nUnhosted = 0;
      bool needHeader = true;
      List<string> unhosted = new List<string>();
      {
        WaitCursor waitCursor = new WaitCursor();
        FilteredElementCollector collector = new FilteredElementCollector( doc );
        collector.OfClass( typeof( FamilyInstance ) );
        //int j = 0;

        // temporary problem: running this in rac_basic_sample_project.rvt throws an exception
        // Unable to cast object of type 'Autodesk.Revit.DB.Panel' to type 'Autodesk.Revit.DB.FamilyInstance'.
        // fixed in http://srd.autodesk.com/srdapp/ReportForm.asp?number=176141 

        foreach( FamilyInstance inst in collector )
        {
          //Debug.WriteLine( ++j );
          Parameter p = inst.get_Parameter( Bip.Host );
          if( null != p )
          {
            if( needHeader )
            {
              Debug.WriteLine( "\nHosted and unhosted elements:" );
              needHeader = false;
            }
            string description = Util.ElementDescriptionAndId( inst );
            string hostValue = p.AsString();
            bool hosted = IsValidHost( hostValue );
            if( hosted ) { ++nHosted; }
            else
            {
              ++nUnhosted;
              //if( null == unhosted )
              //{
              //  unhosted = new string[1];
              //  unhosted[0] = description;
              //}
              //else
              //{
              //  unhosted.se
              //}
              unhosted.Add( description );
            }
            Debug.WriteLine( string.Format( "{0} {1} host is '{2}' --> {3}hosted",
              description, inst.Id.IntegerValue, hostValue, hosted ? "" : "un" ) );
          }
        }
      }
      if( 0 < nHosted + nUnhosted )
      {
        Debug.WriteLine( string.Format( "{0} hosted and {1} unhosted elements.", nHosted, nUnhosted ) );
      }
      if( 0 < nUnhosted )
      {
        string[] a = new string[unhosted.Count];
        int i = 0;
        foreach( string s in unhosted )
        {
          a[i++] = s;
        }
        // todo: present the element ids in a separete edit box for easier copy and paste:
        string msg = string.Format( "{0} unhosted element{1}:\n\n", nUnhosted, Util.PluralSuffix( nUnhosted ) )
          + string.Join( "\n", a );
        Util.InfoMsg( msg );
      }
      return true;
    }
    #endregion // Determine unhosted elements

    #region Execute Command
    public Result Execute(
      ExternalCommandData commandData,
      ref String message,
      ElementSet elements )
    {
      try
      {
        UIApplication app = commandData.Application;
        Document doc = app.ActiveUIDocument.Document;
        //
        // 5. determine unhosted elements (cf. SPR 134098).
        // list all hosted versus unhosted elements:
        //
        bool rc = DetermineUnhostedElements( doc );
        return Result.Cancelled;
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
