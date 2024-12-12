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
using System.Collections;
using System.Collections.Generic;
using WinForms = System.Windows.Forms;
using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Geometry;
using Autodesk.Revit.Parameters;
using Autodesk.Revit.Rooms;
using Autodesk.Revit.Symbols;

using RvtElement = Autodesk.Revit.Element; // to avoid ambiguity with Geometry.Element
#endregion // Namespaces

namespace Labs
{
  #region Lab5_1_GroupsAndGroupTypes
  /// <summary>
  /// List all groups and group types in the model.
  /// </summary>
  public class Lab5_1_GroupsAndGroupTypes : IExternalCommand
  {
    public IExternalCommand.Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;

      // List all Group Elements
      List<RvtElement> groups = LabUtils.GetAllGroups( app );
      string sMsg = "All GROUPS in the doc are:\r\n";
      foreach( Group grp in groups )
      {
        sMsg += "\r\n  Id=" + grp.Id.Value.ToString() + "; Type=" + grp.GroupType.Name;
      }
      LabUtils.InfoMsg( sMsg );

      // List all Group Type Elements
      List<RvtElement> groupTypes = LabUtils.GetAllGroupTypes( app );
      sMsg = "All GROUP TYPES in the doc are:\r\n";
      foreach( GroupType grpTyp in groupTypes )
      {
        // determine the GroupType system family
        // (cf. Labs3 for standard symbols):
        Parameter p = grpTyp.get_Parameter(BuiltInParameter.SYMBOL_FAMILY_NAME_PARAM);
        string famName = null == p ? "?" : p.AsString();
        sMsg += "\r\n  Name=" + grpTyp.Name + "; Id=" + grpTyp.Id.Value.ToString() + "; Family=" + famName;
      }
      LabUtils.InfoMsg( sMsg );

      // Typically, only "Model" types will be needed, so makes sense to have a dedicated utility
      // Does not work any longer in 9.0, so it will be empty (see above) ! :-(
      // Works fine in 2008 and later.
      List<RvtElement> modelGroupTypes = LabUtils.GetAllModelGroupTypes( app );
      sMsg = "All *MODEL* GROUP TYPES in the doc are:\r\n";
      foreach( GroupType grpTyp in modelGroupTypes )
      {
        sMsg += "\r\n  Name=" + grpTyp.Name + "; Id=" + grpTyp.Id.Value.ToString();
      }
      LabUtils.InfoMsg( sMsg );
      return IExternalCommand.Result.Succeeded;
    }
  }
  #endregion // Lab5_1_GroupsAndGroupTypes

  #region Lab5_2_SwapGroupTypes
  /// <summary>
  /// Swap group types for selected groups.
  /// </summary>
  public class Lab5_2_SwapGroupTypes : IExternalCommand
  {
    public IExternalCommand.Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;
      List<RvtElement> gts = LabUtils.GetAllModelGroupTypes( app ); // Get all Group Types of Model Family
      if( 0 == gts.Count )
      {
        LabUtils.ErrorMsg( "No Model Group Types in this model." );
        return IExternalCommand.Result.Cancelled;
      }
      string sMsg;
      foreach( RvtElement elem in app.ActiveDocument.Selection.Elements )
      {
        // Check for Group instance
        if( elem is Group )
        {
          Group gp = elem as Group;

          // Offer simple message box to swap the type
          // (one-by-one, stop if user confirms the change)
          foreach( GroupType gt in gts )
          {
            sMsg = "Swap OLD Type=" + gp.GroupType.Name
              + " with NEW Type=" + gt.Name
              + " for Group Id=" + gp.Id.Value.ToString() + "?";
            switch( LabUtils.QuestionCancelMsg( sMsg ) )
            {
              case WinForms.DialogResult.Yes:
                gp.GroupType = gt;
                LabUtils.InfoMsg( "Group type successfully swapped." );
                return IExternalCommand.Result.Succeeded;
              case WinForms.DialogResult.Cancel:
                LabUtils.InfoMsg( "Command cancelled!" );
                return IExternalCommand.Result.Cancelled;
               // just continue with the For Loop
            }
          }
        }
      }

      /*
      //
      // cannot modify group members after creation:
      //
      RvtElement e = null;
      ElementArray els = new ElementArray();
      els.Append( e );
      Group g = new Group();
      g.Members = els; // Property or indexer 'Autodesk.Revit.Elements.Group.Members' cannot be assigned to -- it is read only
      RvtElement e2 = null;
      els.Append( e2 );
      g.Members = els;
      */

      return IExternalCommand.Result.Succeeded;
    }
  }
  #endregion // Lab5_2_SwapGroupTypes

  #region Lab5_3_Rooms
  /// <summary>
  /// List room boundaries.
  /// </summary>
  public class Lab5_3_Rooms : IExternalCommand
  {
    public IExternalCommand.Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;

      // List all Room Elements
      List<RvtElement> rooms = LabUtils.GetAllRooms(app);
      if( 0 == rooms.Count )
      {
        LabUtils.InfoMsg( "There are no rooms in this model!" );
      }
      else
      {
        foreach( Room room in rooms )
        {
          // Some identification parameters
          // (there are probably built-in Params for this, but this works :-)
          
          Parameter p = room.get_Parameter( "Name" );
          string roomName = null == p ? "?" : p.AsString();
          
          p = room.get_Parameter( "Number" );
          string roomNumber = null == p ? "?" : p.AsString();

          string sMsg = "Room Id=" + room.Id.Value.ToString()
            + " Name=" + roomName + " Number=" + roomNumber + "\r\n";

          // Loop all boundaries of this room
          BoundarySegmentArrayArray boundaries = room.Boundary;
          // Check to ensure room has boundary
          if( null != boundaries )
          {
            int iB = 0;
            foreach (BoundarySegmentArray boundary in boundaries)
            {
              ++iB;
              sMsg += "\r\n    Boundary " + iB + ":";
              int iSeg = 0;
              foreach (BoundarySegment segment in boundary)
              {
                ++iSeg;

                // Segment's curve
                Curve crv = segment.Curve;
                if (crv is Line) // LINE
                {
                  Line line = crv as Line;
                  XYZ ptS = line.get_EndPoint(0);
                  XYZ ptE = line.get_EndPoint(1);
                  sMsg += "\r\n        Segment " + iSeg + " is a LINE: "
                    + LabUtils.PointString(ptS) + " ; "
                    + LabUtils.PointString(ptE);
                }
                else if (crv is Arc) // ARC
                {
                  Arc arc = crv as Arc;
                  XYZ ptS = arc.get_EndPoint(0);
                  XYZ ptE = arc.get_EndPoint(1);
                  double r = arc.Radius;
                  sMsg += "\r\n        Segment " + iSeg + " is an ARC:"
                    + LabUtils.PointString(ptS) + " ; "
                    + LabUtils.PointString(ptE) + " ; R=" + r;
                }
              }
            }
          }
          LabUtils.InfoMsg( sMsg );
        }
      }
    return IExternalCommand.Result.Succeeded;
    }
  }
  #endregion // Lab5_3_Rooms
}
