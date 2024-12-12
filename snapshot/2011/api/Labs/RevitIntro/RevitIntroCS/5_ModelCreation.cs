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
using Autodesk.Revit.Attributes; //' specific this if you want to save typing for attributes. e.g.,
using Autodesk.Revit.DB.Structure; //' added for Lab5.
using System.Collections.Generic;
// added for Lab4.

#endregion

#region Description
// Revit Intro Lab 5
//
// In this lab, you will learn how to create revit models.
// To test this, use "DefaultMetric" template.
//
// Disclaimer: minimum error checking to focus on the main topic.
//
// MH: my scale(77)
//-------1---------2---------3---------4---------5---------6---------7-------
//-------1---------2---------3---------4---------5---------6---------
#endregion

namespace RevitIntroCS
{
  // Element Creation.
  //
  // add Shared parameter here?
  // importing a family symbol. Maybe do it earlier?
  //
  [Transaction( TransactionMode.Automatic )]
  [Regeneration( RegenerationOption.Manual )]
  public class ModelCreation : IExternalCommand
  {
    // member variables
    Application _rvtApp;
    Document _rvtDoc;

    public Autodesk.Revit.UI.Result Execute( ExternalCommandData commandData, ref string message, ElementSet elements )
    {
      // Get the acess to the top most objects.
      UIApplication rvtUIApp = commandData.Application;
      UIDocument rvtUIDoc = rvtUIApp.ActiveUIDocument;
      _rvtApp = rvtUIApp.Application;
      _rvtDoc = rvtUIDoc.Document;

      // Let's make a simple "house" composed of four walls, a window
      // and a door.
      CreateHouse();

      return Result.Succeeded;
    }

    public void CreateHouse()
    {
      // simply create four walls with rectangular profile.
      List<Wall> walls = CreateWalls();

      // add a door to the second wall
      AddDoor( walls[0] );

      // add windows to the rest of the walls.
      for( int i = 1; i <= 3; i++ )
      {
        AddWindow( walls[i] );
      }

      // (optional) add a roof over the walls' rectangular profile.

      addRoof( walls );
    }

    // There are five override methods for creating walls.
    // We assume you are using metric template, where you have
    // "Level 1" and "Level 2"
    // cf. Developer Guide page 117
    //
    public List<Wall> CreateWalls()
    {
      // hard coding the size of the house for simplicity
      double width = mmToFeet( 10000.0 );
      double depth = mmToFeet( 5000.0 );

      // get the levels we want to work on.
      // Note: hard coding for simplicity. Modify here you use a different template.
      Level level1 = ( Level ) ElementFiltering.FindElement( _rvtDoc, typeof( Level ), "Level 1", null );
      if( level1 == null )
      {
        TaskDialog.Show( "Revit Intro Lab", "Cannot find (Level 1). Maybe you use a different template? Try with DefaultMetric.rte." );
        return null;
      }

      Level level2 = ( Level ) ElementFiltering.FindElement( _rvtDoc, typeof( Level ), "Level 2", null );
      if( level2 == null )
      {
        TaskDialog.Show( "Revit Intro Lab", "Cannot find (Level 2). Maybe you use a different template? Try with DefaultMetric.rte." );
        return null;
      }

      // set four corner of walls.
      // 5th point is for combenience to loop through.
      double dx = width / 2.0;
      double dy = depth / 2.0;

      List<XYZ> pts = new List<XYZ>( 5 );
      pts.Add( new XYZ( -dx, -dy, 0.0 ) );
      pts.Add( new XYZ( dx, -dy, 0.0 ) );
      pts.Add( new XYZ( dx, dy, 0.0 ) );
      pts.Add( new XYZ( -dx, dy, 0.0 ) );
      pts.Add( pts[0] );

      // flag for structural wall or not.
      bool isStructural = false;

      // save walls we create.
      List<Wall> walls = new List<Wall>( 4 );

      // loop through list of points and define four walls.
      for( int i = 0; i <= 3; i++ )
      {
        // define a base curve from two points.
        Line baseCurve = _rvtApp.Create.NewLineBound( pts[i], pts[i + 1] );
        // create a wall using the one of overloaded methods.
        Wall aWall = _rvtDoc.Create.NewWall( baseCurve, level1, isStructural );
        // set the Top Constraint to Level 2
        aWall.get_Parameter( BuiltInParameter.WALL_HEIGHT_TYPE ).Set( level2.Id );
        // save the wall.
        walls.Add( aWall );
      }
      // This is important. we need these lines to have shrinkwrap working.
      _rvtDoc.Regenerate();
      _rvtDoc.AutoJoinElements();

      return walls;
    }

    // add a door to the center of the given wall.
    // cf. Developer Guide p137. NewFamilyInstance() for Doors and Window.
    //
    public void AddDoor( Wall hostWall )
    {
      // hard coding the door type we will use.
      // e.g., "M_Single-Flush: 0915 x 2134mm
      const string doorFamilyName = "Single-Flush"; // "M_Single-Flush"
      const string doorTypeName = "30\" x 80\""; // "0915 x 2134mm"
      const string doorFamilyAndTypeName = doorFamilyName + ": " + doorTypeName;

      // get the door type to use.
      FamilySymbol doorType = ( FamilySymbol ) ElementFiltering.FindFamilyType( _rvtDoc, typeof( FamilySymbol ), doorFamilyName, doorTypeName, BuiltInCategory.OST_Doors );
      if( doorType == null )
      {
        TaskDialog.Show( "Revit Intro Lab", "Cannot find (" + doorFamilyAndTypeName + "). Maybe you use a different template? Try with DefaultMetric.rte." );
      }

      // get the start and end points of the wall.
      LocationCurve locCurve = ( LocationCurve ) hostWall.Location;
      XYZ pt1 = locCurve.Curve.get_EndPoint( 0 );
      XYZ pt2 = locCurve.Curve.get_EndPoint( 1 );
      // calculate the mid point.
      XYZ pt = ( pt1 + pt2 ) / 2.0;

      // one more thing - we want to set the reference as a bottom of the wall or level1.
      ElementId idLevel1 = hostWall.get_Parameter( BuiltInParameter.WALL_BASE_CONSTRAINT ).AsElementId();
      Level level1 = ( Level ) _rvtDoc.get_Element( idLevel1 );

      // finally, create a door.

      FamilyInstance aDoor = _rvtDoc.Create.NewFamilyInstance( pt, doorType, hostWall, level1, StructuralType.NonStructural );
    }

    // add a window to the center of the wall given.
    // cf. Developer Guide p137. NewFamilyInstance() for Doors and Window.
    // Basically the same idea as a door except that we need to set sill hight.
    //
    public void AddWindow( Wall hostWall )
    {
      // hard coding the window type we will use.
      // e.g., "M_Fixed: 0915 x 1830mm
      const string windowFamilyName = "Fixed"; // "M_Fixed"
      const string windowTypeName = "16\" x 24\""; // "0915 x 1830mm"
      const string windowFamilyAndTypeName = windowFamilyName + ": " + windowTypeName;
      double sillHeight = mmToFeet( 915 );

      // get the door type to use.
      FamilySymbol windowType = ( FamilySymbol ) ElementFiltering.FindFamilyType( _rvtDoc, typeof( FamilySymbol ), windowFamilyName, windowTypeName, BuiltInCategory.OST_Windows );
      if( windowType == null )
      {
        TaskDialog.Show( "Revit Intro Lab", "Cannot find (" + windowFamilyAndTypeName + "). Maybe you use a different template? Try with DefaultMetric.rte." );
      }

      // get the start and end points of the wall.
      LocationCurve locCurve = ( LocationCurve ) hostWall.Location;
      XYZ pt1 = locCurve.Curve.get_EndPoint( 0 );
      XYZ pt2 = locCurve.Curve.get_EndPoint( 1 );
      // calculate the mid point.
      XYZ pt = ( pt1 + pt2 ) / 2.0;

      // one more thing - we want to set the reference as a bottom of the wall or level1.
      ElementId idLevel1 = hostWall.get_Parameter( BuiltInParameter.WALL_BASE_CONSTRAINT ).AsElementId();
      Level level1 = ( Level ) _rvtDoc.get_Element( idLevel1 );

      // finally create a window.
      FamilyInstance aWindow = _rvtDoc.Create.NewFamilyInstance( pt, windowType, hostWall, level1, StructuralType.NonStructural );

      aWindow.get_Parameter( BuiltInParameter.INSTANCE_SILL_HEIGHT_PARAM ).Set( sillHeight );
    }

    // add a roof over the rectangular profile of the walls we created earlier.
    //
    public void addRoof( List<Wall> walls )
    {
      // hard coding the roof type we will use.
      // e.g., "Basic Roof: Generic - 400mm"
      const string roofFamilyName = "Basic Roof";
      const string roofTypeName = "Generic - 9\""; // "Generic - 400mm"
      const string roofFamilyAndTypeName = roofFamilyName + ": " + roofTypeName;

      // find the roof type
      RoofType roofType = ( RoofType ) ElementFiltering.FindFamilyType( _rvtDoc, typeof( RoofType ), roofFamilyName, roofTypeName, null );
      if( roofType == null )
      {
        TaskDialog.Show( "Revit Intro Lab", "Cannot find (" + roofFamilyAndTypeName + "). Maybe you use a different template? Try with DefaultMetric.rte." );
      }

      // wall thickness to adjust the footprint of the walls
      // to the outer most lines.
      // Note: this may not be the best way.
      // but we will live with this for this exercise.
      double wallThickness = walls[0].WallType.CompoundStructure.Layers.get_Item( 0 ).Thickness;
      double dt = wallThickness / 2.0;
      List<XYZ> dts = new List<XYZ>( 5 );
      dts.Add( new XYZ( -dt, -dt, 0.0 ) );
      dts.Add( new XYZ( dt, -dt, 0.0 ) );
      dts.Add( new XYZ( dt, dt, 0.0 ) );
      dts.Add( new XYZ( -dt, dt, 0.0 ) );
      dts.Add( dts[0] );

      // set the profile from four walls
      CurveArray footPrint = new CurveArray();
      for( int i = 0; i <= 3; i++ )
      {
        LocationCurve locCurve = ( LocationCurve ) walls[i].Location;
        XYZ pt1 = locCurve.Curve.get_EndPoint( 0 ) + dts[i];
        XYZ pt2 = locCurve.Curve.get_EndPoint( 1 ) + dts[i + 1];
        Line line = _rvtApp.Create.NewLineBound( pt1, pt2 );
        footPrint.Append( line );
      }

      // get the level2 from the wall
      ElementId idLevel2 = walls[0].get_Parameter( BuiltInParameter.WALL_HEIGHT_TYPE ).AsElementId();
      Level level2 = ( Level ) _rvtDoc.get_Element( idLevel2 );

      // footprint to morel curve mapping
      ModelCurveArray mapping = new ModelCurveArray();

      // create a roof.
      FootPrintRoof aRoof = _rvtDoc.Create.NewFootPrintRoof( footPrint, level2, roofType, out mapping );

      //
      foreach( ModelCurve modelCurve in mapping )
      {
        aRoof.set_DefinesSlope( modelCurve, true );
        aRoof.set_SlopeAngle( modelCurve, 0.5 );
      }
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