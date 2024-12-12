#region Header
//
// CmdDuplicateElement.cs - duplicate selected elements
//
// Copyright (C) 2010 by Jeremy Tammik,
// Autodesk Inc. All rights reserved.
//
#endregion // Header

#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
#endregion // Namespaces

namespace BuildingCoder
{
  [Transaction( TransactionMode.Manual )]
  [Regeneration( RegenerationOption.Manual )]
  class CmdDuplicateElements : IExternalCommand
  {
    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      UIApplication app = commandData.Application;
      UIDocument uidoc = app.ActiveUIDocument;
      Document doc = uidoc.Document;

      Transaction trans = new Transaction( doc, 
        "Duplicate Elements" );

      trans.Start();

      Group group = doc.Create.NewGroup( 
        uidoc.Selection.Elements );

      LocationPoint location = group.Location 
        as LocationPoint;

      XYZ p = location.Point;
      XYZ newPoint = new XYZ( p.X, p.Y + 10, p.Z );

      Group newGroup = doc.Create.PlaceGroup( 
        newPoint, group.GroupType );

      group.Ungroup();

      ElementSet eSet = newGroup.Ungroup();

      // change the property or parameter values 
      // of elements in eSet as required...

      trans.Commit();

      return Result.Succeeded;
    }
  }
}
