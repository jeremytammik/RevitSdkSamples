#region Header
// Revit API .NET Labs
//
// Copyright (C) 2007-2009 by Autodesk, Inc.
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
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Linq;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
// todo: report and resolve this, this should not be required: 'RE: ambiguous BoundarySegmentArrayArray'
using BoundarySegmentArrayArray = Autodesk.Revit.DB.Architecture.BoundarySegmentArrayArray;
using BoundarySegmentArray = Autodesk.Revit.DB.Architecture.BoundarySegmentArray;
using BoundarySegment = Autodesk.Revit.DB.Architecture.BoundarySegment;
#endregion // Namespaces

namespace Labs
{
  #region Lab5_1_GroupsAndGroupTypes
  /// <summary>
  /// List all groups and group types in the model.
  /// </summary>
  [Transaction( TransactionMode.ReadOnly )]
  [Regeneration( RegenerationOption.Manual )]
  public class Lab5_1_GroupsAndGroupTypes : IExternalCommand
  {
    const string _groupTypeModel = "Model Group"; // BEWARE: In the browser, it says only "Model"
    const string _groupsTypeModel = "Model Groups";

    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      UIApplication app = commandData.Application;
      Document doc = app.ActiveUIDocument.Document;

      List<string> a = new List<string>();

      // list groups:

      FilteredElementCollector collector;
      collector = new FilteredElementCollector( doc );
      collector.OfClass( typeof( Group ) );

      foreach( Group g in collector )
      {
        a.Add( "Id=" + g.Id.IntegerValue.ToString() + "; Type=" + g.GroupType.Name );
      }
      LabUtils.InfoMsg( "{0} group{1} in the document{2}", a );

      // list groups types:

      BuiltInParameter bic = BuiltInParameter.SYMBOL_FAMILY_NAME_PARAM;

      a.Clear();

      collector = new FilteredElementCollector( doc );
      collector.OfClass( typeof( GroupType ) );

      foreach( GroupType g in collector )
      {
        // determine the GroupType system family
        // (cf. Labs3 for standard symbols):

        Parameter p = g.get_Parameter( bic );
        string famName = (null == p) ? "?" : p.AsString();

        a.Add( "Name=" + g.Name + "; Id=" + g.Id.IntegerValue.ToString() + "; Family=" + famName );
      }
      LabUtils.InfoMsg( "{0} group type{1} in the document{2}", a );

      // typically, only "Model" types will be needed.
      // create a filter by creating a provider and an evaluator.
      // we can reuse the collector we already set up for group
      // types, and just add another criteria to check to it:

      a.Clear();

      #region Failed attempts
      /*
       * this returns zero elements:
       *
      ParameterValueProvider provider
        = new ParameterValueProvider( new ElementId( ( int ) bic ) );

      FilterStringRuleEvaluator evaluator
        = new FilterStringEquals();

      string ruleString = _groupTypeModel;
      bool caseSensitive = true;

      FilterRule rule = new FilterStringRule( provider, evaluator, ruleString, caseSensitive );

      // Create an ElementParameter filter:

      ElementParameterFilter filter = new ElementParameterFilter( rule );

      // Apply the filter to the elements in the active collector:

      collector.WherePasses( filter ).ToElements();
      */

      /*
       * this returns false:
       *
      if( doc.Settings.Categories.Contains( _groupsTypeModel ) )
      {
        Category cat = doc.Settings.Categories.get_Item( _groupsTypeModel );

        foreach( GroupType g in collector )
        {
          a.Add( "Name=" + g.Name + "; Id=" + g.Id.IntegerValue.ToString() );
        }
      }
      */
      #endregion // Failed attempts

      collector.OfCategory( BuiltInCategory.OST_IOSModelGroups );

      foreach( GroupType g in collector )
      {
        a.Add( "Name=" + g.Name + "; Id=" + g.Id.IntegerValue.ToString() );
      }

      LabUtils.InfoMsg( "{0} *model* group type{1} in the document{2}", a );

      return Result.Failed;
    }
  }
  #endregion // Lab5_1_GroupsAndGroupTypes

  #region Lab5_2_SwapGroupTypes
  /// <summary>
  /// Swap group types for selected groups.
  /// </summary>
  [Transaction( TransactionMode.Automatic )]
  [Regeneration( RegenerationOption.Manual )]
  public class Lab5_2_SwapGroupTypes : IExternalCommand
  {
    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      UIApplication app = commandData.Application;
      UIDocument uidoc = app.ActiveUIDocument;
      Document doc = uidoc.Document;

      // Get all Group Types of Model Family:

      FilteredElementCollector modelGroupTypes
        = new FilteredElementCollector( doc );

      modelGroupTypes.OfClass( typeof( GroupType ) );
      modelGroupTypes.OfCategory( BuiltInCategory.OST_IOSModelGroups );

      if( 0 == modelGroupTypes.Count() )
      {
        message = "No model group types found in model.";
        return Result.Failed;
      }

      FilteredElementCollector groups;
      groups = new FilteredElementCollector( doc );
      groups.OfClass( typeof( Group ) );

      foreach( Group g in groups )
      {
        // Offer simple message box to swap the type
        // (one-by-one, stop if user confirms the change)

        foreach( GroupType gt in modelGroupTypes )
        {
          string msg = "Swap OLD Type=" + g.GroupType.Name
            + " with NEW Type=" + gt.Name
            + " for Group Id=" + g.Id.IntegerValue.ToString() + "?";

          TaskDialogResult r = LabUtils.QuestionCancelMsg( msg );

          switch( r )
          {
            case TaskDialogResult.Yes:
              g.GroupType = gt;
              LabUtils.InfoMsg( "Group type successfully swapped." );
              return Result.Succeeded;

            case TaskDialogResult.Cancel:
              LabUtils.InfoMsg( "Command cancelled." );
              return Result.Cancelled;

            // else continue...
          }
        }
      }

      /*
      //
      // cannot modify group members after creation:
      //
      Element e = null;
      ElementArray els = new ElementArray();
      els.Append( e );
      Group g = new Group();
      g.Members = els; // Property or indexer 'Autodesk.Revit.Elements.Group.Members' cannot be assigned to -- it is read only
      Element e2 = null;
      els.Append( e2 );
      g.Members = els;
      */

      return Result.Succeeded;
    }
  }
  #endregion // Lab5_2_SwapGroupTypes

  #region Lab5_3_Rooms
  /// <summary>
  /// List room boundaries.
  /// </summary>
  [Transaction( TransactionMode.ReadOnly )]
  [Regeneration( RegenerationOption.Manual )]
  public class Lab5_3_Rooms : IExternalCommand
  {
    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      UIApplication app = commandData.Application;
      Document doc = app.ActiveUIDocument.Document;

      FilteredElementCollector rooms
        = new FilteredElementCollector( doc );

      //
      // this is one way of obtaining rooms ... but see below for a better solution:
      //
      //rooms.OfClass( typeof( Room ) ); // Input type is of an element type that exists in the API, but not in Revit's native object model. Try using Autodesk.Revit.DB.Enclosure instead, and then postprocessing the results to find the elements of interest.
      //rooms.OfClass( typeof( Enclosure ) ); // this works but returns all Enclosure elements

      RoomFilter filter = new RoomFilter();
      rooms.WherePasses( filter );

      if( 0 == rooms.Count() )
      {
        LabUtils.InfoMsg( "There are no rooms in this model." );
      }
      else
      {
        List<string> a = new List<string>();

        /*
        foreach( Enclosure e in rooms ) // todo: remove this
        {
          Room room = e as Room; // todo: remove this
          if( null != room ) // todo: remove this
          {
        */

        foreach( Room room in rooms )
        {
          string roomName = room.Name;
          string roomNumber = room.Number;

          string s = "Room Id=" + room.Id.IntegerValue.ToString()
            + " Name=" + roomName + " Number=" + roomNumber + "\n";

          // Loop all boundaries of this room

          BoundarySegmentArrayArray boundaries = room.Boundary;

          // Check to ensure room has boundary

          if( null != boundaries )
          {
            int iB = 0;
            foreach( BoundarySegmentArray boundary in boundaries )
            {
              ++iB;
              s += "  Boundary " + iB + ":\n";
              int iSeg = 0;
              foreach( BoundarySegment segment in boundary )
              {
                ++iSeg;

                // Segment's curve
                Curve crv = segment.Curve;

                if( crv is Line )
                {
                  Line line = crv as Line;
                  XYZ ptS = line.get_EndPoint( 0 );
                  XYZ ptE = line.get_EndPoint( 1 );
                  s += "    Segment " + iSeg + " is a LINE: "
                    + LabUtils.PointString( ptS ) + " ; "
                    + LabUtils.PointString( ptE ) + "\n";
                }
                else if( crv is Arc )
                {
                  Arc arc = crv as Arc;
                  XYZ ptS = arc.get_EndPoint( 0 );
                  XYZ ptE = arc.get_EndPoint( 1 );
                  double r = arc.Radius;
                  s += "    Segment " + iSeg + " is an ARC:"
                    + LabUtils.PointString( ptS ) + " ; "
                    + LabUtils.PointString( ptE ) + " ; R="
                    + LabUtils.RealString( r ) + "\n";
                }
              }
            }
            a.Add( s );
          }
          LabUtils.InfoMsg( "{0} room{1} in the model{2}", a );
        }
      }
      return Result.Failed;
    }
  }
  #endregion // Lab5_3_Rooms
}
