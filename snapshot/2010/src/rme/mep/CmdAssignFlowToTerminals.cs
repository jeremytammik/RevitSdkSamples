#region Header
// Revit MEP API sample application
//
// Copyright (C) 2007-2009 by Jeremy Tammik, Autodesk, Inc.
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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using W = System.Windows.Forms;
using Autodesk.Revit;
using Autodesk.Revit.Collections; // Map
using Autodesk.Revit.Elements;
using Autodesk.Revit.Parameters;
using CmdResult = Autodesk.Revit.IExternalCommand.Result;
#endregion // Namespaces

namespace mep
{
  class CmdAssignFlowToTerminals : IExternalCommand
  {
    #region Get Terminals Per Space

    #region Obsolete GetTerminalsPerRoomUsingRevitMap
    static Map GetTerminalsPerRoomUsingRevitMap( Application app )
    {
      Document doc = app.ActiveDocument;
      ElementSet terminals = Util.GetSupplyAirTerminals( app );
      Map terminalsPerRoom = app.Create.NewMap();
      foreach( FamilyInstance terminal in terminals )
      {
        string spaceNr = terminal.Room.Number;
        ElementSet elementSet;
        if( terminalsPerRoom.Contains( spaceNr ) )
        {
          elementSet = terminalsPerRoom.get_Item( spaceNr ) as ElementSet;
        }
        else
        {
          elementSet = app.Create.NewElementSet();
          terminalsPerRoom.Insert( spaceNr, elementSet );
        }
        elementSet.Insert( terminal );
      }

#if LIST_ROOMS_USING_REVIT_MAP
      MapIterator it2 = terminalsPerRoom.ForwardIterator();
      while( it2.MoveNext() )
      {
        ElementSet elementSet = it2.Current as ElementSet;
        int n = elementSet.Size;
        Debug.WriteLine( string.Format( "Room {0} contains {1} air terminal{2}{3} {4}", it2.Key, n, PluralSuffix( n ), DotOrColon( n ), IdList( elementSet ) ) );
      }
#endif // LIST_ROOMS_USING_REVIT_MAP

      return terminalsPerRoom;
    }
    #endregion // Obsolete GetTerminalsPerRoomUsingRevitMap

    /// <summary>
    /// Helper class for sorting the spaces by space number before listing them
    /// </summary>
    class NumericalComparer : IComparer
    {
      public int Compare( object x, object y )
      {
        return int.Parse( x as string ) - int.Parse( y as string );
      }
    }

    static Hashtable GetTerminalsPerSpace( Application app )
    {
      Document doc = app.ActiveDocument;
      ElementSet terminals = Util.GetSupplyAirTerminals( app );
      // todo: use Dictionary<string,List<FamilyInstance>> instead of hashtable:
      Hashtable terminalsPerSpace = new Hashtable();
      foreach( FamilyInstance terminal in terminals )
      {
        //string roomNr = terminal.Room.Number;
        string spaceNr = terminal.Space.Number; // changed Room to Space
        if( !terminalsPerSpace.ContainsKey( spaceNr ) )
        {
          terminalsPerSpace.Add( spaceNr, new ArrayList() );
        }
        ( (ArrayList) terminalsPerSpace[spaceNr] ).Add( terminal );
      }
      ArrayList keys = new ArrayList( terminalsPerSpace.Keys );
      keys.Sort( new NumericalComparer() );
      string ids;
      ArrayList spaceTerminals;
      int n, nTerminals = 0;
      foreach( string key in keys )
      {
        spaceTerminals = terminalsPerSpace[key] as ArrayList;
        n = spaceTerminals.Count;
        ids = Util.IdList( spaceTerminals );
        Debug.WriteLine( string.Format( "Space {0} contains {1} air terminal{2}{3} {4}", key, n, Util.PluralSuffix( n ), Util.DotOrColon( n ), ids ) );
        nTerminals += n;
      }
      n = terminalsPerSpace.Count;
      Debug.WriteLine( string.Format( "Processing a total of {0} space{1} containing {2} air terminal{3}.", n, Util.PluralSuffix( n ), nTerminals, Util.PluralSuffix( nTerminals ) ) );
      return terminalsPerSpace;
    }
    #endregion // Get Terminals Per Space

    #region AssignFlowToTerminals

    static double RoundFlowTo( double a )
    {
      a = a / Const.RoundTerminalFlowTo;
      a = Math.Round( a, 0, MidpointRounding.AwayFromZero );
      a = a * Const.RoundTerminalFlowTo;
      return a;
    }

    static void AssignFlowToTerminals( ArrayList terminals, double flow )
    {
      foreach( FamilyInstance terminal in terminals )
      {
        Parameter p = Util.GetTerminalFlowParameter( terminal );
        p.Set( flow );
      }
    }

    static void AssignFlowToTerminalsForSpace( Hashtable terminalsPerSpace, Space space )
    {
      ArrayList terminals = terminalsPerSpace[space.Number] as ArrayList;
      if( null != terminals )
      {
        int n = terminals.Count;
        double calculatedSupplyAirFlow = Util.GetSpaceParameterValue( space, Bip.CalculatedSupplyAirFlow, ParameterName.CalculatedSupplyAirFlow );
        double flowCfm = calculatedSupplyAirFlow * Const.SecondsPerMinute;
        double flowCfmPerOutlet = flowCfm / n;
        double flowCfmPerOutletRounded = RoundFlowTo( flowCfmPerOutlet );
        double flowPerOutlet = flowCfmPerOutletRounded / Const.SecondsPerMinute;
        string format = "Space {0} has calculated supply airflow {1} f^3/s = {2} CFM and {3} terminal{4}"
          + " --> flow {5} CFM per terminal, rounded to {6} = {7} f^3/s";
        Debug.WriteLine( string.Format( format,
          space.Number, Util.RealString( calculatedSupplyAirFlow ), Util.RealString( flowCfm ),
          n, Util.PluralSuffix( n ), Util.RealString( flowCfmPerOutlet ),
          Util.RealString( flowCfmPerOutletRounded ), Util.RealString( flowPerOutlet ) ) );
        AssignFlowToTerminals( terminals, flowPerOutlet );
      }
    }
    #endregion // AssignFlowToTerminals

    #region Execute Command
    public CmdResult Execute(
      ExternalCommandData commandData,
      ref String message,
      ElementSet elements )
    {
      try
      {
        WaitCursor waitCursor = new WaitCursor();
        Application app = commandData.Application;
        Document doc = app.ActiveDocument;
        //
        // 1. determine air terminals for each space.
        // determine the relationship between all air terminals and all spaces:
        // extract and group all air terminals per space 
        // (key=space, val=set of air terminals)
        //
        Debug.WriteLine( "\nDetermining terminals per space..." );
        //Map terminalsPerRoomMap = GetTerminalsPerRoomUsingRevitMap( app );
        Hashtable terminalsPerSpace = GetTerminalsPerSpace( app );
        //
        // 2. assign flow to the air terminals depending on the space's calculated supply air flow.
        //
        //ElementFilterIterator it = doc.get_Elements( typeof( Room ) ); // 2008
        //ElementIterator it = doc.get_Elements( typeof( Space ) ); // 2009
        List<Element> spaces = new List<Element>();
        doc.get_Elements( typeof( Space ), spaces );
        int n = spaces.Count;
        string s = "{0} of " + n.ToString() + " spaces processed...";
        string caption = "Assign Flow to Terminals";
        using( ProgressForm pf = new ProgressForm( caption, s, n ) )
        {
          foreach( Space space in spaces )
          {
            AssignFlowToTerminalsForSpace( terminalsPerSpace, space );
            pf.Increment();
          }
        }
        Debug.WriteLine( "Completed." );
        return CmdResult.Succeeded;
      }
      catch( Exception ex )
      {
        message = ex.Message;
        return CmdResult.Failed;
      }
    }
    #endregion // Execute Command
  }
}
