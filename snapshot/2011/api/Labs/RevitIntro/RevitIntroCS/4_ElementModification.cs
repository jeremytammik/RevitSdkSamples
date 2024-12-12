#region Copyright
//
// (C) Copyright 2010 by Autodesk, Inc.
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted,
// provided that the above copyright notice appears in all copies and
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
//
// Migrated to C# by Adam Nagy
//
#endregion // Copyright

#region Imports
// Import the following name spaces in the project properties/references.
// Note: VB.NET has a slighly different way of recognizing name spaces than C#.
// if you explicitely set them in each .vb file, you will need to specify full name spaces.

using System;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI.Selection; //' specific this if you want to save typing for attributes. e.g.,
// added for Lab4.

#endregion

#region Description
// Revit Intro Lab 4
//
// In this lab, you will learn how to modify elements.
// There are two places to look at when you want to modify an element.
// (1) at each element level, such as by modifying each properties, parameters, and location.
// (2) use document methods, such as move, rorat, mirror, array and delete.
//
// for #2, document.move, rotate, etc., see from pp105 of developer guide.
//
//
// Disclaimer: minimum error checking to focus on the main topic.
//
#endregion

namespace RevitIntroCS
{
  // Element Modification
  //
  [Transaction( TransactionMode.Automatic )]
  [Regeneration( RegenerationOption.Manual )]
  public class ElementModification : IExternalCommand
  {
    // member variables
    Application _rvtApp;
    Document _rvtDoc;

    public Result Execute( ExternalCommandData commandData, ref string message, ElementSet elements )
    {
      // Get the access to the top most objects.
      UIApplication rvtUIApp = commandData.Application;
      UIDocument rvtUIDoc = rvtUIApp.ActiveUIDocument;
      _rvtApp = rvtUIApp.Application;
      _rvtDoc = rvtUIDoc.Document;

      // select an object on a screen. (We'll come back to the selection in the UI Lab later.)
      Reference r = rvtUIDoc.Selection.PickObject( ObjectType.Element, "Pick an element" );
      // we have picked something.
      Element elem = r.Element;

      // modify element's properties, parameters, location. use document.move, rotation, mirror.
      // In eailer lab, CommandData command, we learned how to access to the wallType. i.e.,
      // here we'll take a look at more on the topic of accessing to elements in the interal rvt project database.
      // (1) element level modification
      //ModifyElementPropertiesWall(elem)
      ModifyElementPropertiesDoor( elem );
      _rvtDoc.Regenerate();

      // select an object on a screen. (We'll come back to the selection in the UI Lab later.)
      Reference ref2 = rvtUIDoc.Selection.PickObject( ObjectType.Element, "Pick another element" );
      // we have picked something.
      Element elem2 = ref2.Element;

      // (2) you can also use document level move, rotation, mirror.
      ModifyElementByDocumentMethods( elem2 );

      return Result.Succeeded;
    }

    //
    // A sampler function to demonstrate how to modify an element through its prooperties.
    // Using a wall as an example here.
    //
    public void ModifyElementPropertiesWall( Element elem )
    {
      // Constant to this function.
      // this is for wall. e.g., "Basic Wall: Exterior - Brick on CMU"
      // you can modify this to fit your need.
      //
      const string wallFamilyName = "Basic Wall";
      const string wallTypeName = "Exterior - Brick on CMU";
      const string wallFamilyAndTypeName = wallFamilyName + ": " + wallTypeName;

      // for simplicity, we assume we can only modify a wall
      if( !( elem is Wall ) )
      {
        TaskDialog.Show( "Revit Intro Lab", "Sorry, I only know how to modify a wall. Please select a wall." );
        return;
      }
      Wall aWall = ( Wall ) elem;

      string msg = "Wall changed: " + "\n" + "\n";
      //keep the message to the user.

      // (1) change its family type to a different one.
      // To Do: change this to enhance import symbol later.
      //
      Element newWallType = ElementFiltering.FindFamilyType( _rvtDoc, typeof( WallType ), wallFamilyName, wallTypeName, null );

      if( newWallType != null )
      {
        aWall.WallType = ( WallType ) newWallType;
        msg = msg + "Wall type to: " + wallFamilyAndTypeName + "\n";
      }
      //TaskDialog.Show("Revit Intro Lab", msg)

      // (2) change its parameters.
      // as a way of exercise, let's constrain top of the wall to the level1 and set an offset.

      // find the level 1 using the helper function we defined in the lab3.
      Level level1 = ( Level ) ElementFiltering.FindElement( _rvtDoc, typeof( Level ), "Level 1", null );
      if( level1 != null )
      {
        aWall.get_Parameter( BuiltInParameter.WALL_HEIGHT_TYPE ).Set( level1.Id );
        // Top Constraint
        msg = msg + "Top Constraint to: Level 1" + "\n";
      }

      // hard coding for simplisity here.
      double topOffset = mmToFeet( 5000.0 );
      // Top Offset Double
      aWall.get_Parameter( BuiltInParameter.WALL_TOP_OFFSET ).Set( topOffset );
      // Structural Usage = Bearing(1)
      aWall.get_Parameter( BuiltInParameter.WALL_STRUCTURAL_USAGE_PARAM ).Set( 1 );
      // Comments - String
      aWall.get_Parameter( BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS ).Set( "Modified by API" );

      msg = msg + "Top Offset to: 5000.0" + "\n";
      msg = msg + "Structural Usage to: Bearing" + "\n";
      msg = msg + "Comments added: Modified by API" + "\n";
      //TaskDialog.Show("Revit Intro Lab", msg)

      // (3) Optional: change its location, using location curve
      // LocationCurve also has move and rotation methods.
      // Note: constaints affect the result.
      // Effect will be more visible with disjoined wall.
      // To test this, you may want to draw a single standing wall, and run this command.
      //
      LocationCurve wallLocation = ( LocationCurve ) aWall.Location;
      XYZ pt1 = wallLocation.Curve.get_EndPoint( 0 );
      XYZ pt2 = wallLocation.Curve.get_EndPoint( 1 );
      // hard coding the displacement value for simility here.
      double dt = mmToFeet( 1000.0 );
      var newPt1 = new XYZ( pt1.X - dt, pt1.Y - dt, pt1.Z );
      var newPt2 = new XYZ( pt2.X - dt, pt2.Y - dt, pt2.Z );
      // create a new line bound.
      Line newWallLine = _rvtApp.Create.NewLineBound( newPt1, newPt2 );
      // finally change the curve.
      wallLocation.Curve = newWallLine;

      msg = msg + "Location: start point moved -1000.0 in X-direction" + "\n";
      // message to the user.

      TaskDialog.Show( "Revit Intro Lab", msg );
    }

    //
    // A sampler function to demonstrate how to modify an element through its prooperties.
    // Using a door as an example here.
    //
    public void ModifyElementPropertiesDoor( Element elem )
    {
      // Constant to this function.
      // this is for a door. e.g., "M_Single-Flush: 0762 x 2032mm"
      // you can modify this to fit your need.
      //
      const string doorFamilyName = "M_Single-Flush";
      const string doorTypeName = "0762 x 2032mm";
      const string doorFamilyAndTypeName = doorFamilyName + ": " + doorTypeName;

      // for simplicity, we assume we can only modify a door
      if( !( elem is FamilyInstance ) )
      {
        TaskDialog.Show( "Revit Intro Lab", "Sorry, I only know how to modify a door. Please select a door." );
        return;
      }
      FamilyInstance aDoor = ( FamilyInstance ) elem;

      string msg = "Door changed: " + "\n" + "\n";
      //keep the message to the user.

      // (1) change its family type to a different one.
      // To Do: change this to enhance import symbol later.
      //
      Element newDoorType = ElementFiltering.FindFamilyType( _rvtDoc, typeof( FamilySymbol ), doorFamilyName, doorTypeName, BuiltInCategory.OST_Doors );

      if( newDoorType != null )
      {
        aDoor.Symbol = ( FamilySymbol ) newDoorType;
        msg = msg + "Door type to: " + doorFamilyAndTypeName + "\n";
      }
      //TaskDialog.Show("Revit Intro Lab", msg)

      // (2) change its parameters.
      // as a way of exercise, let's constrain top of the wall to the level1 and set an offset.

      // '' find the level 1 using the helper function we defined in the lab3.
      //Dim level1 As Level = ElementFiltering.FindElement(_rvtDoc, GetType(Level), "Level 1")
      //If level1 IsNot Nothing Then
      // aWall.Parameter(BuiltInParameter.WALL_HEIGHT_TYPE).Set(level1.Id) '' Top Constraint
      // msg = msg + "Top Constraint to: Level 1" + vbCr
      //End If

      //Double topOffset = mmToFeet(5000.0); //' hard coding for simplisity here.
      //aWall.Parameter(BuiltInParameter.WALL_TOP_OFFSET).Set(topOffset); // '' Top Offset Double
      //aWall.Parameter(BuiltInParameter.WALL_STRUCTURAL_USAGE_PARAM).Set(1) '' Structural Usage = Bearing(1)
      //aWall.Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS).Set("Modified by API") '' Comments - String

      //msg = msg + "Top Offset to: 5000.0" + vbCr
      //msg = msg + "Structural Usage to: Bearing" + vbCr
      //msg = msg + "Comments added: Modified by API" + vbCr
      //TaskDialog.Show("Revit Intro Lab", msg)

      // (3) Optional: change its location, using location curve
      // LocationCurve also has move and rotation methods.
      // Note: constaints affect the result.
      // Effect will be more visible with disjoined wall.
      // To test this, you may want to draw a single standing wall, and run this command.
      //
      //Dim wallLocation As LocationCurve = aWall.Location
      //Dim pt1 As XYZ = wallLocation.Curve.EndPoint(0)
      //Dim pt2 As XYZ = wallLocation.Curve.EndPoint(1)
      // '' hard coding the displacement value for simility here.
      //Dim dt As Double = mmToFeet(1000.0)
      //Dim newPt1 = New XYZ(pt1.X - dt, pt1.Y - dt, pt1.Z)
      //Dim newPt2 = New XYZ(pt2.X - dt, pt2.Y - dt, pt2.Z)
      // '' create a new line bound.
      //Dim newWallLine As Line = _rvtApp.Create.NewLineBound(newPt1, newPt2)
      // '' finally change the curve.
      //wallLocation.Curve = newWallLine

      //msg = msg + "Location: start point moved -1000.0 in X-direction" + vbCr

      // message to the user.

      TaskDialog.Show( "Revit Intro Lab", msg );
    }

    //
    // A sampler function that demonstrates how to modify an element through document methods.
    //
    public void ModifyElementByDocumentMethods( Element elem )
    {
      string msg = "The element changed: " + "\n" + "\n";
      //keep the message to the user.

      // try move
      double dt = mmToFeet( 1000.0 );
      // hard cording for simplicity.
      XYZ v = new XYZ( dt, dt, 0.0 );
      _rvtDoc.Move( elem, v );

      msg = msg + "move by (1000, 1000, 0)" + "\n";

      // try rotate: 15 degree around z-axis.
      var pt1 = XYZ.Zero;
      var pt2 = XYZ.BasisZ;
      Line axis = _rvtApp.Create.NewLineBound( pt1, pt2 );
      _rvtDoc.Rotate( elem, axis, Math.PI / 12.0 );

      msg = msg + "rotate by 15 degree around Z-axis" + "\n";

      // message to the user.

      TaskDialog.Show( "Revit Intro Lab", msg );
    }

    #region Helper Functions

    //=============================================
    // Helper Functions
    //=============================================

    // convert millimeter to feet
    //
    public double mmToFeet( double mmVal )
    {
      // * 0.00328;

      return mmVal / 304.8;
    }

    #endregion
  }
}