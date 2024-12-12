using System;
using System.Collections.Generic;
using System.Linq;

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI.Selection;

/*
 * This command copies the picked group in a room to any other picked rooms. 
 * The group must be created before running this command.  
 * It can contain any kind of Revit elements, for instance furniture or
 * MEP ducts and pipes, and it should be in an existing room. 
 * The target room should also be created before running this command.
 */

[TransactionAttribute(TransactionMode.Automatic)]
[RegenerationAttribute(RegenerationOption.Automatic)]
public class BatchCopyFurniture : IExternalCommand
{
  public Result Execute( 
    ExternalCommandData commandData, 
    ref string message, 
    ElementSet elements )
  {
    UIApplication uiApp = commandData.Application;
    Application app = uiApp.Application;
    Document doc = uiApp.ActiveUIDocument.Document;

    Reference refPicked = null;
    
    try
    {
      Selection sel = uiApp.ActiveUIDocument.Selection;
      GroupPickFilter groupFilter = new GroupPickFilter();

      // need to add filter to restrict room only

      refPicked = sel.PickObject( ObjectType.Element, groupFilter, 
        "Please select a group in a room" );

      Element elem = refPicked.Element;
      Group group = elem as Group;
      if( group == null )
      {
        message = "No group was selected.";
        return Result.Failed;
      }

      //Get the group's Origin point coordinates.
      XYZ ptOrigin = GetGroupOrigin( group, doc );

      //Get the room that the picked group is located in.
      Room room = GetRoomOfGroup( doc, ptOrigin );

      //pick rooms interactively by user
      RoomPickFilter roomPickFilter = new RoomPickFilter();
      IList<Reference> refRooms = sel.PickObjects( ObjectType.Element, 
        roomPickFilter, "Please select rooms to duplicate furniture group" );

      //place furniture to these rooms
      PlaceFurnitureInRoom( refRooms, room, group.GroupType, ptOrigin, doc );
    }
    //If user right click or press ESC key, handle the exception
    catch( Autodesk.Revit.Exceptions.OperationCanceledException ex )
    {
      return Result.Cancelled;
    }
    //catch other errors.
    catch( Exception ex )
    {
      message = ex.Message;
      return Result.Failed;
    }
    return Result.Succeeded;
  }

  /// <summary>
  /// Return the room in which the given point 'p' is located.
  /// </summary>
  Room GetRoomOfGroup( Document doc, XYZ p )
  {
    FilteredElementCollector collector = new FilteredElementCollector( doc );
    collector.OfCategory( BuiltInCategory.OST_Rooms );
    Room room = null;
    foreach( Element elem in collector )
    {
      room = elem as Room;
      if( room != null )
      {
        //decide if this point is in the picked room                  
        if( room.IsPointInRoom( p ) )
        {
          break;
        }
      }
    }
    return room;
  }

  /// <summary>
  /// Return the center/origin point of a group.
  /// </summary>
  public XYZ GetGroupOrigin( Group group, Document doc )
  {
    XYZ pt = null;
    BoundingBoxXYZ bounding = group.get_BoundingBox( doc.ActiveView );
    pt = ( bounding.Max + bounding.Min ) * 0.5;
    return pt;
  }

  /// <summary>
  /// Duplicate furniture group in picked rooms according to the 
  /// room's center point. The position where  group is placed is 
  /// based on the target room's center point, the offset to the 
  /// room's center point is the offset between the source group 
  /// center to its owner room's center.
  /// </summary>
  /// <param name="roomReferences">selected rooms' references.</param>
  /// <param name="roomSource">the room that the source group is located in.</param>
  /// <param name="gt">group type of the source group.</param>
  /// <param name="ptGroupOrigin">source group's center/origin point coordinates.</param>
  /// <param name="doc">Document.</param>
  public void PlaceFurnitureInRoom( 
    IList<Reference> roomReferences, 
    Room roomSource, 
    GroupType gt, 
    XYZ ptGroupOrigin, 
    Document doc )
  {
    foreach( Reference r in roomReferences )
    {
      Room roomTarget = r.Element as Room;

      if( null != roomTarget )
      {
        XYZ ptRoom = GetRoomCenter( roomTarget, doc );
        XYZ offsetXYZ = ptGroupOrigin - GetRoomCenter( roomSource, doc );
        XYZ offsetXY = new XYZ( offsetXYZ.X, offsetXYZ.Y, 0 );

        Group g = doc.Create.PlaceGroup( ptRoom + offsetXY, gt );
      }
    }
  }

  /// <summary> 
  /// Return a room's center point coordinates. 
  /// Z value is equal to the bottom of the room
  /// </summary>
  public XYZ GetRoomCenter( Room room, Document doc )
  {
    LocationPoint ptLocation = room.Location as LocationPoint;
    //get the room center point.
    BoundingBoxXYZ bounding = room.get_BoundingBox( doc.ActiveView );
    XYZ xyzCenter = ( bounding.Max + bounding.Min ) * 0.5;
    XYZ ptRoom = new XYZ( xyzCenter.X, xyzCenter.Y, ptLocation.Point.Z );
    return ptRoom;
  }
}

/// <summary>
/// Filter to constrain picking to model groups. Only model groups 
/// are highlighted and can be selected when cursor is hovering.
/// </summary>
public class GroupPickFilter : ISelectionFilter
{
  public bool AllowElement( Element e )
  {
    return ( e.Category.Name == "Model Groups" );
  }

  public bool AllowReference( Reference r, XYZ p )
  {
    return false;
  }
}

/// <summary>
/// Filter to constrain picking to rooms only. Only rooms are 
/// highlighted and can be selected when cursor is hovering.
/// </summary>
public class RoomPickFilter : ISelectionFilter
{
  public bool AllowElement( Element e )
  {
    return ( e.Category.Name == "Rooms" );
  }

  public bool AllowReference( Reference r, XYZ p )
  {
    return false;
  }
}
