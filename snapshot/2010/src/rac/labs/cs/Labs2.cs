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
using System.Diagnostics;
using System.IO;
using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Enums;
using Geo = Autodesk.Revit.Geometry;
using Autodesk.Revit.Parameters;
using Autodesk.Revit.Structural.Enums;
using Autodesk.Revit.Symbols;
using CmdResult = Autodesk.Revit.IExternalCommand.Result;
using Line = Autodesk.Revit.Geometry.Line;
using UV = Autodesk.Revit.Geometry.UV;
using XYZ = Autodesk.Revit.Geometry.XYZ;
using XYZArray = Autodesk.Revit.Geometry.XYZArray;
#endregion // Namespaces

namespace Labs
{
  #region Lab2_0_CreateLittleHouse
  /// <summary>
  /// Create some sample elements.
  /// We create a simple building consisting of four walls, 
  /// a door, two windows, a floor, a roof, a room and a room tag.
  /// </summary>
  public class Lab2_0_CreateLittleHouse : IExternalCommand
  {
    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      try
      {
        WaitCursor waitCursor = new WaitCursor();
        Application app = commandData.Application;
        Document doc = app.ActiveDocument;
        Autodesk.Revit.Creation.Application createApp = app.Create;
        Autodesk.Revit.Creation.Document createDoc = doc.Create;
        //
        // determine the four corners of the rectangular house:
        //
        double width = 7 * LabConstants.MeterToFeet;
        double depth = 4 * LabConstants.MeterToFeet;
        List<XYZ> corners = new List<XYZ>( 4 );
        corners.Add( new XYZ( 0, 0, 0 ) );
        corners.Add( new XYZ( width, 0, 0 ) );
        corners.Add( new XYZ( width, depth, 0 ) );
        corners.Add( new XYZ( 0, depth, 0 ) );

        #region TEST_CREATING_TWO_LEVELS
#if CREATE_TWO_LEVELS
        Level levelBottom = null;
        Level levelMiddle = null;
        Level levelTop = null;
        List<Element> levels = new List<Element>();

        Filter filterType 
          = createApp.Filter.NewTypeFilter( 
            typeof( Level ) );

        doc.get_Elements( filterType, levels );
        foreach( Element e in levels )
        {
          if( null == levelBottom )
          {
            levelBottom = e as Level;
          }
          else if( null == levelMiddle )
          {
            levelMiddle = e as Level;
          }
          else if( null == levelTop )
          {
            levelTop = e as Level;
          }
          else
          {
            break;
          }
        }

        BuiltInParameter topLevelParam 
          = BuiltInParameter.WALL_HEIGHT_TYPE;

        Line line;
        Wall wall;
        Parameter param;

        ElementId topId = levelMiddle.Id;
        List<Wall> walls = new List<Wall>( 8 );
        for( int i = 0; i < 4; ++i )
        {
          line = createApp.NewLineBound( 
            corners[i], corners[3 == i ? 0 : i + 1] );

          wall = createDoc.NewWall( 
            line, levelBottom, false );

          param = wall.get_Parameter( topLevelParam );
          param.Set( ref topId );
          walls.Add( wall );
        }

        topId = levelTop.Id;
        for( int i = 0; i < 4; ++i )
        {
          line = createApp.NewLineBound( 
            corners[i], corners[3 == i ? 0 : i + 1] );

          wall = createDoc.NewWall( 
            line, levelMiddle, false );

          param = wall.get_Parameter( topLevelParam );
          param.Set( ref topId );
          walls.Add( wall );
        }

        List<Element> doorSymbols 
          = LabUtils.GetAllFamilySymbols( 
            app, BuiltInCategory.OST_Doors );

        Debug.Assert( 
          0 < doorSymbols.Count, 
          "expected at least one door symbol"
          + " to be loaded into project" );

        FamilySymbol door 
          = doorSymbols[0] as FamilySymbol;

        XYZ midpoint = LabUtils.Midpoint( 
          corners[0], corners[1] );

        FamilyInstance inst0 
          = createDoc.NewFamilyInstance(
            midpoint, door, walls[0], levelBottom, 
            StructuralType.NonStructural );

        midpoint.Z = levelMiddle.Elevation;

        FamilyInstance inst1 
          = createDoc.NewFamilyInstance(
            midpoint, door, walls[4], levelMiddle, 
            StructuralType.NonStructural );

#endif // CREATE_TWO_LEVELS
        #endregion // TEST_CREATING_TWO_LEVELS

        //
        // determine the levels where the walls will be located:
        //
        Level levelBottom = null;
        Level levelTop = null;
        if( !LabUtils.GetBottomAndTopLevels( app, ref levelBottom, ref levelTop ) )
        {
          message = "Unable to determine wall bottom and top levels";
          return CmdResult.Failed;
        }
        Debug.Print( string.Format( "Drawing walls on '{0}' up to '{1}'", levelBottom.Name, levelTop.Name ) );
        //
        // create the walls:
        //
        BuiltInParameter topLevelParam = BuiltInParameter.WALL_HEIGHT_TYPE;
        ElementId topLevelId = levelTop.Id;
        List<Wall> walls = new List<Wall>( 4 );
        for( int i = 0; i < 4; ++i )
        {
          Line line = createApp.NewLineBound( corners[i], corners[3 == i ? 0 : i + 1] );
          Wall wall = createDoc.NewWall( line, levelBottom, false );
          Parameter param = wall.get_Parameter( topLevelParam );
          param.Set( ref topLevelId );
          walls.Add( wall );
        }
        // determine wall thickness for tag offset and profile growth:
        double wallThickness = walls[0].WallType.CompoundStructure.Layers.get_Item( 0 ).Thickness;
        //
        // add door and windows to the first wall;
        // note that the NewFamilyInstance() api method does not automatically add door 
        // and window tags, like the ui command does. we add tags here by making additional calls
        // to NewTag():
        //
        List<Element> doorSymbols = LabUtils.GetAllFamilySymbols( app, BuiltInCategory.OST_Doors );
        List<Element> windowSymbols = LabUtils.GetAllFamilySymbols( app, BuiltInCategory.OST_Windows );
        Debug.Assert( 0 < doorSymbols.Count, "expected at least one door symbol to be loaded into project" );
        Debug.Assert( 0 < windowSymbols.Count, "expected at least one window symbol to be loaded into project" );
        FamilySymbol door = doorSymbols[0] as FamilySymbol;
        FamilySymbol window = windowSymbols[0] as FamilySymbol;
        XYZ midpoint = LabUtils.Midpoint( corners[0], corners[1] );
        XYZ p = LabUtils.Midpoint( corners[0], midpoint );
        XYZ q = LabUtils.Midpoint( midpoint, corners[1] );
        double tagOffset = 3 * wallThickness;
        //double windowHeight = 1 * LabConstants.MeterToFeet;
        double windowHeight = levelBottom.Elevation + 0.3 * ( levelTop.Elevation - levelBottom.Elevation );
        p.Z = q.Z = windowHeight;
        View view = doc.ActiveView;
        FamilyInstance inst = createDoc.NewFamilyInstance( 
          midpoint, door, walls[0], levelBottom, StructuralType.NonStructural );
        midpoint.Y = midpoint.Y + tagOffset;
        IndependentTag tag = createDoc.NewTag( 
          view, inst, false, TagMode.TM_ADDBY_CATEGORY, TagOrientation.TAG_HORIZONTAL, midpoint );
        inst = createDoc.NewFamilyInstance( p, window, walls[0], levelBottom, StructuralType.NonStructural );
        p.Y = p.Y + tagOffset;
        tag = createDoc.NewTag( view, inst, false, TagMode.TM_ADDBY_CATEGORY, TagOrientation.TAG_HORIZONTAL, p );
        inst = createDoc.NewFamilyInstance( q, window, walls[0], levelBottom, StructuralType.NonStructural );
        q.Y = q.Y + tagOffset;
        tag = createDoc.NewTag( view, inst, false, TagMode.TM_ADDBY_CATEGORY, TagOrientation.TAG_HORIZONTAL, q );
        //
        // grow the profile out by half the wall thickness, 
        // so the floor and roof do not stop halfway through the wall:
        //
        double w = 0.5 * wallThickness;
        corners[0].X -= w;
        corners[0].Y -= w;
        corners[1].X += w;
        corners[1].Y -= w;
        corners[2].X += w;
        corners[2].Y += w;
        corners[3].X -= w;
        corners[3].Y += w;
        Geo.CurveArray profile = new Geo.CurveArray();
        for( int i = 0; i < 4; ++i )
        {
          Geo.Line line = createApp.NewLineBound( corners[i], corners[3 == i ? 0 : i + 1] );
          profile.Append( line );
        }
        //
        // add a floor, a roof, the roof slope, a room and a room tag:
        //
        bool structural = false;
        Floor floor = createDoc.NewFloor( profile, structural );
        List<Element> roofTypes = LabUtils.GetAllTypes( 
          app, typeof( RoofType ), BuiltInCategory.OST_Roofs );
        Debug.Assert( 0 < roofTypes.Count, "expected at least one roof type to be loaded into project" );
        RoofType roofType = roofTypes[0] as RoofType;
        ElementIdSet footPrintToModelCurvesMapping = new ElementIdSet();
        FootPrintRoof roof = createDoc.NewFootPrintRoof( 
          profile, levelTop, roofType, footPrintToModelCurvesMapping );
        double slopeAngle = 30 * LabConstants.DegreesToRadians;
        foreach( ElementId id in footPrintToModelCurvesMapping )
        {
          ElementId id2 = id;
          ModelCurve line = doc.get_Element( ref id2 ) as ModelCurve;
          roof.set_DefinesSlope( line, true );
          roof.set_SlopeAngle( line, slopeAngle );
        }
        Room room = createDoc.NewRoom( levelBottom, new UV( 0.5 * width, 0.5 * depth ) );
        RoomTag roomTag = createDoc.NewRoomTag( room, new UV( 0.5 * width, 0.7 * depth ), null );
        //LabUtils.InfoMsg( "Little house was created successfully." );
        return CmdResult.Succeeded;
      }
      catch( Exception ex )
      {
        message = ex.Message;
        return CmdResult.Failed;
      }
    }
  }
  #endregion // Lab2_0_CreateLittleHouse

  #region Lab2_1_Elements
  /// <summary>
  /// List all document elements.
  /// </summary>
  public class Lab2_1_Elements : IExternalCommand
  {
    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;
      Document doc = app.ActiveDocument;

      // Typical .NET error checking (should be done everywhere,
      // but will be omitted for clarity in some of the following
      // labs unless we expect exceptions):
      StreamWriter sw;
      try
      {
        sw = new StreamWriter( LabConstants.FilePath );
      }
      catch( Exception e )
      {
        LabUtils.ErrorMsg( "Cannot open " + LabConstants.FilePath 
          + "; Exception=" + e.Message );
        return CmdResult.Failed;
      }
      try
      {
        // *ALL* elements are bundled together and accessible via Document's ElementIterator
        WaitCursor waitCursor = new WaitCursor();
        string line;
        Element e;
        // IEnumerator it = doc.Elements // this would also be sufficient
        ElementIterator it = doc.Elements;
        while( it.MoveNext() )
        {
          e = it.Current as Element;
          line = "Id=" + e.Id.Value.ToString(); // Element Id
          line += "; Class=" + e.GetType().Name; // Element class (System.Type)
          //Debug.WriteLine( line  );

          // The element category is not implemented for all classes, 
          // it may return null; for Family elements, one can sometimes 
          // use the FamilyCategory property instead.
          string s = string.Empty;
          if( e is Family && null != ((Family) e).FamilyCategory )
          {
            s = ((Family) e).FamilyCategory.Name;
          }
          if( 0 == s.Length && null != e.Category )
          {
            s = e.Category.Name;
          }
          if( 0 == s.Length )
          {
            s = "?";
          }
          line += "; Category=" + s;

          // Element Name (different meaning for different classes, but mostly implemented "logically")
          // Later, we'll see that more precise info on elements can be obtained in class-specific ways...
          line += "; Name=" + e.Name;
          line += "; UniqueId=" + e.UniqueId;
          //line += "; Guid=" + GetGuid( e.UniqueId );
          sw.WriteLine( line );
        }
        sw.Close();
        LabUtils.InfoMsg( "Element list has been written to " + LabConstants.FilePath + "." );
      }
      catch( Exception e )
      {
        message = e.Message;
      }
      return CmdResult.Failed;
    }
  }
  #endregion // Lab2_1_Elements

  #region Lab2_2_ModelElements

  #region Prior to using a type filter
#if PRIOR_TO_USING_A_TYPE_FILTER
  /// <summary>
  /// List all model elements.
  /// </summary>
  public class Lab2_2_ModelElements : IExternalCommand
  {
    // do not use the language dependent localised category name string:
    //const string _CategoryNameLegendComponents = "Legend Components";
    //BuiltInCategory _bicLegendComponent = BuiltInCategory.OST_LegendComponents;

    BuiltInCategory _bicPreviewLegendComponent 
      = BuiltInCategory.OST_PreviewLegendComponents;

    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;
      Document doc = app.ActiveDocument;

      ElementIterator it = doc.Elements;
      string s = string.Empty;
      int count = 0;

      //Geo.Options opt = null; // this causes an exception in get_Geometry()
      Geo.Options opt = app.Create.NewGeometryOptions(); // this is ok
      //opt.DetailLevel = Geo.Options.DetailLevels.Fine; // this is ok as well

      int iBic = (int) _bicPreviewLegendComponent;

      while( it.MoveNext() )
      {
        Element e = it.Current as Element;

        // This single line would probably work if all system families
        // were exposed as HostObjects, but they are not:
        //if( e is FamilyInstance || e is HostObject )

        // we used to check for 
        // !(e is Symbol)
        // && !(e is FamilyBase)
        // && (null != e.Category)
        // && (null != e.get_Geometry(opt))
        //
        // in Revit 2010, this also admits legend component elements,
        // which previously did not have any geometry.
        // to eliminate these, we can either use 
        //
        // !(e is Symbol)
        // && !(e is FamilyBase)
        // && (null != e.Category)
        // && (_CategoryNameLegendComponents != e.Category.Name)
        // && (null != e.get_Geometry(opt))
        //
        // simply check that the level is not null and
        // altogether skip the checks on the object type;
        // nope, this does not work, it unfortunately misses 
        // certain model elements which do not have a level 
        // asssigned:
        //
        // (null != e.Category)
        // && (null != e.Level)
        // && (null != e.get_Geometry(opt))
        //
        if ( !(e is Symbol)
          && !(e is FamilyBase)
          && (null != e.Category)
          && ( iBic != e.Category.Id.Value )
          && (null != e.get_Geometry( opt )) )
        {
          ++count;
          s += string.Format( 
            "\r\n  Category={0}; Name={1}; Id={2}",
            e.Category.Name, e.Name, 
            e.Id.Value.ToString() );
        }
      }

      s = "There are " + count.ToString() 
        + " model elements:" + s;

      LabUtils.InfoMsg( s );

      return CmdResult.Failed;
    }
  }
#endif // PRIOR_TO_USING_A_TYPE_FILTER
  #endregion // Prior to using a type filter

  /// <summary>
  /// List all model elements.
  /// </summary>
  public class Lab2_2_ModelElements : IExternalCommand
  {
    /// <summary>
    /// Types of elements which are not model
    /// elements and need to be eliminated.
    /// </summary>
    static Type[] _types_to_skip = new Type[] {
      typeof( BasePoint ),
      typeof( DetailLine ),
      typeof( Dimension ),
      typeof( Family ),
      typeof( FamilyBase ),
      typeof( FillPattern ),
      typeof( gbXMLParamElem ),
      typeof( GraphicsStyle ),
      typeof( Level ),
      typeof( LinePattern ),
      typeof( MaterialOther ),
      typeof( ModelLine ),
      typeof( Phase ),
      typeof( PrintSetting ),
      typeof( ProjectInfo ),
      typeof( ProjectUnit ),
      typeof( ReferencePlane ),
      typeof( Room ),
      typeof( RoomTag ),
      typeof( Sketch ),
      typeof( SketchPlane ),
      // typeof( Symbol ), // this is done by first filter
      typeof( TextNote ),
      typeof( ViewDrafting ),
      typeof( ViewPlan ),
      typeof( ViewSheet ),
      typeof( WallType ),
    };

    /// <summary>
    /// Built-in categories that characterise non-model
    /// elements which need to be eliminated.
    /// </summary>
    static int[] _bics_to_skip = new int[] {
      ( int ) BuiltInCategory.OST_PreviewLegendComponents,
      ( int ) BuiltInCategory.OST_IOSSketchGrid,
      ( int ) BuiltInCategory.OST_Cameras,
    };

    static bool SkipThisBic( int bic )
    {
      return Array.Exists<int>(
        _bics_to_skip,
        delegate( int i )
        {
          return i == bic;
        } );
    }

    /// <summary>
    /// Given a type filter, add another type to it.
    /// </summary>
    /// <param name="f">Existing type filter</param>
    /// <param name="t">New type to add to filter</param>
    /// <param name="cf">Filter creation object</param>
    /// <returns></returns>
    static Filter OrType(
      Filter f,
      Type t,
      Autodesk.Revit.Creation.Filter cf )
    {
      Filter f2 = cf.NewTypeFilter( t );
      return cf.NewLogicOrFilter( f, f2 );
    }

    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;
      Document doc = app.ActiveDocument;

      Autodesk.Revit.Creation.Filter cf
        = app.Create.Filter;

      List<Element> els = new List<Element>();
      Filter f = cf.NewTypeFilter( typeof( Symbol ) );
      foreach( Type t in _types_to_skip )
      {
        f = OrType( f, t, cf );
      }
      f = cf.NewLogicNotFilter( f );
      int n = doc.get_Elements( f, els );

      string s = string.Empty;
      n = 0;

      //Geo.Options opt = null; // this causes an exception in get_Geometry()
      Geo.Options opt = app.Create.NewGeometryOptions(); // this is ok
      //opt.DetailLevel = Geo.Options.DetailLevels.Fine; // this is ok as well

      foreach( Element e in els )
      {
        if( (null != e.Category)
          && !SkipThisBic( e.Category.Id.Value )
          && (null != e.get_Geometry( opt )) )
        {
          ++n;
          s += string.Format( 
            "\r\n  Category={0}; Name={1}; Id={2}",
            e.Category.Name, e.Name, e.Id.Value );
        }
      }

      s = string.Format(
        "Project contains {0} model element{1}{2}",
        n,
        ( 1 == n ? "" : "s" ),
        ( 0 == n ? "." : ":" ) )
      + s;

      LabUtils.InfoMsg( s );

      return CmdResult.Failed;
    }
  }
  #endregion // Lab2_2_ModelElements

  #region Todo: Element Id and Fake Element Id
#if ADD_EXAMPLE_CODE_FOR_THIS
  //
  // todo: add some samples of accessing individual elements by element id.
  // explain the existence of fake element ids, e.g.
  //
  enum AnalyticalVerticalProjection_e
  {
    AVP_AUTODETECT = -1,
    AVP_TOP_OF_PHYSICAL = -2,
    AVP_CENTER_OF_PHYSICAL = -3,
    AVP_BOTTOM_OF_PHYSICAL = -4,
    AVP_DEFAULT_WALL_TOP = -5,
    AVP_DEFAULT_WALL_BOTTOM = -6,
    AVP_DEFAULT_COLUMN_TOP = -7,
    AVP_DEFAULT_COLUMN_BOTTOM = -8,
    AVP_DEFAULT_BEAM = -9,
    AVP_DEFAULT_SLAB = -10,
    AVP_INVALID = -11,
  };

  Is there a way to programatically change the value of the parameter "vertical projection" of a floor element to any of the values : "c", "center of the slab" or "bottom of the slab"?

  There is a enum type used to set the parameter value for 'Vertical Projection'
  We can use it in the following way:
  Construct a ElementId object, for instance, id , set id.value to one of the member of the above enum.
  Then set id to parameter of  'Vertical Projection'. The parameter value can be set successfully to the value that is not a real element id.
  Please see the code snippet below how to set value to 'top of the slab'

  <code_begin>
    Element ele = SelectOne( "Please select a slab", commandData, typeof( Floor ) );
    if( ele == null )
        return CmdResult.Failed;
    Floor floor = ele as Floor;
    //get the AnalyticalModel of the column;
    Parameter paraVerticalProject = floor.get_Parameter(BuiltInParameter.STRUCTURAL_ANALYTICAL_PROJECT_FLOOR_PLANE);
    ElementId id = paraVerticalProject.AsElementId(); // If we check the value of id.value here, it is negative number if the parameter value is not a real element id.
    id.Value = -2; // top of the slab
    paraVerticalProject.Set( ref id );
  <code_end>
#endif // ADD_EXAMPLE_CODE_FOR_THIS
  #endregion // Todo: Element Id and Fake Element Id

  #region Lab2_3_AllWallsAndDoorFamilyInstances
  /// <summary>
  /// List all walls and family instances.
  /// </summary>
  public class Lab2_3_AllWallsAndDoorFamilyInstances : IExternalCommand
  {
    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;
      Document doc = app.ActiveDocument;

      // get all walls:
      List<Element> walls = new List<Element>();
      Filter f = app.Create.Filter.NewTypeFilter( typeof( Wall ) );
      doc.get_Elements( f, walls );

      string s = "All walls in the model:";
      foreach ( Wall wall in walls )
      {
        s += string.Format( "\r\n  Id={0}; Kind={1}; Type={2}",
          wall.Id.Value.ToString(), 
          wall.WallType.Kind.ToString(),
          wall.WallType.Name );
      }
      LabUtils.InfoMsg( s );

      BuiltInCategory bic = BuiltInCategory.OST_Doors;
      string catName = app.ActiveDocument.Settings.Categories.get_Item( bic ).Name;
      List<Element> familyInstances = LabUtils.GetAllStandardFamilyInstancesForACategory( app, bic );

      List<Element> doors = new List<Element>();
      Filter fType = app.Create.Filter.NewTypeFilter( typeof( FamilyInstance ) );
      Filter fCategory = app.Create.Filter.NewCategoryFilter( bic );
      f = app.Create.Filter.NewLogicAndFilter( fType, fCategory);
      doc.get_Elements( f, doors );

      s = "All " + catName + " instances in the model:";
      foreach( FamilyInstance door in doors )
      {
        // For FamilyInstances, the element name property returns the type name:
        s += "\r\n  Id=" + door.Id.Value.ToString() + "; Type=" + door.Name;
      }
      LabUtils.InfoMsg( s );
      return CmdResult.Failed;
    }
  }
  #endregion // Lab2_3_AllWallsAndDoorFamilyInstances

  #region Lab2_4_EditFamilyInstance

  #region Obsolete Lab2_4_AddColumnsAndMoveWall
  /// <summary>
  /// Demonstrate access to family instance parameters,
  /// modification of family instance location, and
  /// creation of new family instance elements by
  /// querying an existing wall for its location and
  /// parameters, modifying it, and inserting column elements.
  /// </summary>
  //
  // note: the column can be seen in 3D view by setting argument to StructuralType.Column,
  // but cannot by StructuralType.NonStructural, the latter is only visible in plan view.
  // This is a temporary problem, NewFamilyInstance identifies the nonstructural instance
  // as an annotation instance, so it only shows it in plan view.
  //
  public class Lab2_4_AddColumnsAndMoveWall : IExternalCommand
  {
    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string msg, ElementSet els )
    {
      Application app = commandData.Application;
      Document doc = app.ActiveDocument;
      ElementSet ss = doc.Selection.Elements;

      // must have single element only
      if( 1 != ss.Size )
      {
        LabUtils.ErrorMsg( "Please pre-select a single wall element." );
        return CmdResult.Cancelled;
      }

      // must be a wall
      ElementSetIterator it = ss.ForwardIterator();
      it.MoveNext();
      Element e = it.Current as Element;
      if( !( e is Wall ) )
      {
        LabUtils.ErrorMsg( "Selected element is NOT a wall." );
        return CmdResult.Cancelled;
      }
      Wall wall = e as Wall;

      // wall must be constrained to a level at the top (more on parameters later...)
      Level topLev = null;
      try
      {
        ElementId id = wall.get_Parameter( BuiltInParameter.WALL_HEIGHT_TYPE ).AsElementId();
        topLev = doc.get_Element( ref id ) as Level;
      }
      catch( Exception )
      {
        topLev = null;
      }
      if( null == topLev )
      {
        LabUtils.ErrorMsg( "Selected wall is NOT constrained to a level at the top." );
        return CmdResult.Cancelled;
      }

      // get the bottom level as well (this should never fail)
      Level botLev = null;
      try
      {
        ElementId id = wall.get_Parameter( BuiltInParameter.WALL_BASE_CONSTRAINT ).AsElementId();
        botLev = doc.get_Element( ref id ) as Level;
      }
      catch( Exception )
      {
        botLev = null;
      }
      if( null == botLev )
      {
        LabUtils.ErrorMsg( "Selected wall is NOT constrained to a level at the bottom." );
        return CmdResult.Cancelled;
      }

      // Calculate the location points for the 3 columns (assuming straight wall)
      LocationCurve locCurve = wall.Location as LocationCurve;
      XYZ ptStart = locCurve.Curve.get_EndPoint( 0 );
      XYZ ptEnd = locCurve.Curve.get_EndPoint( 1 );
      XYZ ptMid = new XYZ( 0.5 * ( ptStart.X + ptEnd.X ), 0.5 * ( ptStart.Y + ptEnd.Y ), 0.5 * ( ptStart.Z + ptEnd.Z ) );
      XYZArray locations = new XYZArray();
      locations.Append( ptStart );
      locations.Append( ptMid );
      locations.Append( ptEnd );
      string s = "Locations for the new Columns (ALWAYS reported \'raw\', in inches) are:";
      s += "\r\n  Start: " + LabUtils.PointString( ptStart );
      s += "\r\n  Mid  : " + LabUtils.PointString( ptMid );
      s += "\r\n  End  : " + LabUtils.PointString( ptEnd );
      LabUtils.InfoMsg( s );

      // Get family type for the new instances.
      // If needed, change the names to match a column type available in the model
      string sFamilyName = "M_Wood Timber Column";
      string sTypeName = "191 x 292mm";
      FamilySymbol symbol = LabUtils.GetFamilySymbol( app, sFamilyName, sTypeName );
      if( null == symbol )
      {
        LabUtils.ErrorMsg( "Cannot find Family=" + sFamilyName + " Type=" + sTypeName + " in the current model - please load it first." );
        return CmdResult.Cancelled;
      }

      // Insert columns as family instances
      foreach( XYZ pt in locations )
      {
        try
        {
          XYZ p = new XYZ( pt.X, pt.Y, pt.Z );
          // Note: Currently there is a problem.  If we set the type as NonStructural, it is treated as Annotation instance, and it shows only in plan view.
          // FamilyInstance column = doc.Create.NewFamilyInstance( ref p, symbol, botLev, Autodesk.Revit.Structural.Enums.StructuralType.NonStuctural );
          FamilyInstance column = doc.Create.NewFamilyInstance( p, symbol, botLev, StructuralType.Column );
          Parameter paramTopLevel = column.get_Parameter( BuiltInParameter.FAMILY_TOP_LEVEL_PARAM );
          ElementId id = topLev.Id;
          paramTopLevel.Set( ref id );
        }
        catch( Exception )
        {
          LabUtils.ErrorMsg( "Failed to create or adjust this column." );
        }
      }

      // Finally, move the wall so the columns are surely visible.
      // For simplicity, move the wall "perpendicularly" to the
      // location curve by one tenth of its length.
      XYZ wallPerpAxis = new XYZ( -0.1 * ( ptEnd.Y - ptStart.Y ),
        0.1 * ( ptEnd.X - ptStart.X ), 0 );
      if( !wall.Location.Move( wallPerpAxis ) )
      {
        LabUtils.ErrorMsg( "Failed to move the wall." );
      }
      return CmdResult.Succeeded;
    }
  }
  #endregion // Obsolete Lab2_4_AddColumnsAndMoveWall

  /// <summary>
  /// Edit all doors in the current project.
  /// Move the doors up 0.2 feet via Document.Move() method.
  /// and widen the door 1 foot by changing the Parameter value.
  /// </summary>
  public class Lab2_4_EditFamilyInstance : IExternalCommand
  {
    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string msg, ElementSet els )
    {
      Application app = commandData.Application;
      Document doc = app.ActiveDocument;
      try
      {
        // find doors in the model by filter:
        Filter filterCategory = app.Create.Filter.NewCategoryFilter( BuiltInCategory.OST_Doors );
        Filter filterType = app.Create.Filter.NewTypeFilter( typeof( FamilyInstance ) );
        Filter f = app.Create.Filter.NewLogicAndFilter( filterCategory, filterType );
        List<Element> doors = new List<Element>();
        doc.get_Elements( f, doors );

        // move doors up 0.2 feet:
        XYZ xyzVector = new XYZ( 0, 0, 0.2 );

        foreach( FamilyInstance door in doors )
        {
          doc.Move( door, xyzVector );

          // widen doors by one foot by changing parameter value:
          Parameter par = door.Symbol.get_Parameter( BuiltInParameter.WINDOW_WIDTH );
          if( null != par )
          {
            double width = par.AsDouble();
            width += 1.0;
            par.Set( width );              
          }            
        }
      }
      catch( Exception ex )
      {
        LabUtils.InfoMsg( ex.Message );
      }
      return CmdResult.Succeeded;
    }
  }
  #endregion // Lab2_4_EditFamilyInstance

  #region Lab2_5_Categories
  /// <summary>
  /// List all built-in categories and the entire category tree.
  /// Some of the results:
  /// - not all built-in categories have a corresponding document category.
  /// - not all document categories have a corresponding built-in category.
  /// There are 645 built-in categories.
  /// 419 of them have associated document categories.
  /// 199 of these are top level parents.
  /// These lead to 444 top-level and children categories.
  /// </summary>
  public class Lab2_5_Categories : IExternalCommand
  {
    Document _doc;
    const int _indentDepth = 2;
    Hashtable _bicForCategory;
    //Hashtable _categoryForBic;

    /// <summary>
    /// Check whether the given category is one of the built-in ones.
    /// This implementation is slow in 2008, and very very very slow in 2009,
    /// so it has been replaced by the _bicForCategory hash table solution below.
    /// Furthermore, categories cannot reliably be compared by value using 
    /// Equals() in 2009, you have to compare their element ids instead.
    /// </summary>
    /// <returns>True if the given category is one of the built-in ones.</returns>
    bool IsBuiltInCategoryObsoleteSlowVersion( Category c )
    {
      bool rc = false;
      ElementId id = c.Id;
      foreach( BuiltInCategory a in Enum.GetValues( typeof( BuiltInCategory ) ) )
      {
        Category c2 = _doc.Settings.Categories.get_Item( a );
        //if( c.Equals( c2 ) )
        if( id.Equals( c2.Id ) )
        {
          rc = true;
          break;
        }
      }
      return rc;
    }

    /// <summary>
    /// Check whether the given category is one of the built-in ones.
    /// </summary>
    /// <returns>True if the given category is one of the built-in ones.</returns>
    bool IsBuiltInCategory( Category c )
    {
      return _bicForCategory.ContainsKey( c );
    }

    int ListCategoryAndSubCategories( Category c, int level )
    {
      int n = 1;
      string indent = new string( ' ', level * _indentDepth );
      Debug.WriteLine( indent + c.Name + ( IsBuiltInCategory( c ) ? "" : " - not built in" ) );
      foreach( Category sub in c.SubCategories )
      {
        n += ListCategoryAndSubCategories( sub, level + 1 );
      }
      return n;
    }

    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      WaitCursor waitCursor = new WaitCursor();
      Application app = commandData.Application;
      _doc = app.ActiveDocument;
      Categories categories = _doc.Settings.Categories;
      Array bics = Enum.GetValues( typeof( BuiltInCategory ) );
      _bicForCategory = new Hashtable( bics.GetLength( 0 ) );
      //_categoryForBic = new Hashtable( bics.GetLength( 0 ) );
      int nNullCategoriesForBic = 0;
      foreach( BuiltInCategory bic in bics )
      {
        Category c = categories.get_Item( bic );
        if( null == c )
        {
          ++nNullCategoriesForBic;
          Debug.WriteLine( string.Format( "Built-in category '{0}' has a null category.", bic.ToString() ) );
        }
        else
        {
          //_categoryForBic.Add( bic, c );
          _bicForCategory.Add( c, bic );
        }
      }
      //
      // list and count all the built-in categoreies:
      //
      int nBuiltInCategories = 0;
      int nDocumentCategories = 0;
      string indent = new string( ' ', _indentDepth );
      CategorySet topLevelCategories = app.Create.NewCategorySet();
      Debug.WriteLine( "\nBuilt-in categories:" );
      foreach( BuiltInCategory a in Enum.GetValues( typeof( BuiltInCategory ) ) )
      {
        Category c = categories.get_Item( a );
        if( null != c )
        {
          ++nDocumentCategories;
          if( null == c.Parent )
          {
            topLevelCategories.Insert( c );
          }
        }
        Debug.WriteLine( indent + a.ToString() + ( ( null == c ) ? "" : ( ": " + c.Name ) ) );
        ++nBuiltInCategories;
      }
      Debug.WriteLine( string.Format( "There are {0} built-in categories. {1} of these have a null category in document. {2} of them have associated document categories. {3} are top level parents.", nBuiltInCategories, nNullCategoriesForBic, nDocumentCategories, topLevelCategories.Size ) );
      //
      // list the entire category hierarchy:
      //
      int nPrinted = 0;
      Debug.WriteLine( "\nDocument categories that we reached so far:" );
      foreach( Category c in topLevelCategories )
      {
        nPrinted += ListCategoryAndSubCategories( c, 1 );
      }
      Debug.WriteLine( string.Format( "{0} top-level and children categories printed.", nPrinted ) );

      nPrinted = 0;
      Debug.WriteLine( "\nDocument categories from scratch:" );
      foreach( Category c in categories )
      {
        nPrinted += ListCategoryAndSubCategories( c, 1 );
      }
      Debug.WriteLine( string.Format( "{0} top-level and children categories printed.", nPrinted ) );
      return CmdResult.Succeeded;
    }
  }
  #endregion // Lab2_5_Categories

  #region Lab2_6_CategoryComparison
  /// <summary>
  /// Category comparison.
  ///
  /// For devnote ts88704 [Comparing Revit Categories].
  ///
  /// Categories could be directly compared in 2008. but this does not
  /// work in 2009. In 2009, you have to compare the category ids instead.
  /// However, please note that in general, category comparison can be
  /// completely avoided by using filters in 2009, which are significantly
  /// faster. 
  /// </summary>
  public class Lab2_6_CategoryComparison : IExternalCommand
  {
    /// <summary>
    /// Category comparison.
    ///
    /// Categories could be directly compared in 2008. but this does not
    /// work in 2009. In 2009, you have to compare the category ids instead.
    /// However, please note that in general, category comparison can be
    /// completely avoided by using filters in 2009, which are significantly
    /// faster. 
    /// </summary>
    void test_category_comparison()
    {
      //
      // here is the current document that we are working on, and
      // a query element whose category we want to compare:
      //
      Document doc = null;
      Categories categories = doc.Settings.Categories;
      Element e = null;
      //
      // comparison by name is both slow and language
      // dependant, so it should be avoided at all costs:
      //
      if( e.Category.Name.Equals( "Doors" ) )
      {
        // do something
      }
      //
      // obtaining the category from the document is extremely slow,
      // so ensure that this value is queried once only and then cached 
      // for future use. This single line walks the whole element tree,
      // so using it will be as slow as iterating over doc.Elements,
      //
      Category cat = categories.get_Item( BuiltInCategory.OST_Doors );
      //
      // direct comparison of categories worked in 2008, but fails in 2009:
      //
      if( e.Category.Equals( cat ) )
      {
        // do something
      }
      //
      // comparing the category ids works reliably and
      // is fast and language independent in all versions:
      //
      if( e.Category.Id.Equals( cat.Id ) )
      {
        // do something
      }
    }

    /// <summary>
    /// Category comparison benchmark provided by Guy Robinson.
    /// Cf. http://forums.augi.com/showthread.php?t=82239
    /// and case 1241716. Simply avoid comparing categories 
    /// at all, use a filter instead.
    /// </summary>
    public IExternalCommand.Result Execute( 
      ExternalCommandData commandData, 
      ref string message, 
      ElementSet elements )
    {
      Application app = commandData.Application;
      Stopwatch sw = Stopwatch.StartNew();
      //ElementIterator itor = app.ActiveDocument.Elements;
      //ElementId category = app.ActiveDocument.Settings.Categories.get_Item(Autodesk.Revit.BuiltInCategory.OST_Doors).Id;
      Filter doorFilter = app.Create.Filter.NewCategoryFilter( BuiltInCategory.OST_Doors );
      ElementIterator itor = app.ActiveDocument.get_Elements( doorFilter );
      while( itor.MoveNext() )
      {
        Element element = itor.Current as Element;
        if( element != null )
        {
          //if (element.Category != null)
          //	if (element.Category.Id.Equals(category))
          //	{
          //		// do something
          //	}
        }
      }
      sw.Stop();
      LabUtils.InfoMsg( string.Format( "Time: {0} ms.", sw.ElapsedMilliseconds.ToString() ) );
      return IExternalCommand.Result.Succeeded;
    }
  }
  #endregion // Lab2_6_CategoryComparison

  #region Lab2_7_ListLinkedFiles
  /// <summary>
  /// List all linked files.
  /// </summary>
  public class Lab2_7_ListLinkedFiles : IExternalCommand
  {
    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;
      //List<Element> instances = LabUtils.GetAllImportScaleInstances_Crash( app );
      List<Element> instances = LabUtils.GetAllImportScaleInstances( app );
      string s = "There are " + instances.Count.ToString() + " import scale instances:\r\n";
      foreach( Instance i in instances )
      {
        s += string.Format( "\r\n  Category={0}; Id={1}; Name={2}",
          i.Category.Name, i.Id.Value.ToString(), i.Name );
      }
      LabUtils.InfoMsg( s );
      return CmdResult.Succeeded;
    }

    private void HideLinkedFiles( Application app )
    {
      Document doc = app.ActiveDocument;
      Categories categories = doc.Settings.Categories;

      // get category for linked files
      Category linkedRevitCat
        = categories.get_Item(
          BuiltInCategory.OST_RvtLinks );

      // loop through all categories in document
      foreach( Category c in categories )
      {
        // if they end with dwg or rvt toggle
        // their current visibility in current view
        if( c.Name.ToLower().EndsWith( ".dwg" )
          || c.Name.ToLower().Contains( ".rvt" )
          || c.Name.ToLower().EndsWith( ".dwf" )
          || c.Name.ToLower().EndsWith( ".dxf" )
          || c.Name.ToLower().EndsWith( ".dwfx" )
          || ( linkedRevitCat != null
            && c.Id.Equals( linkedRevitCat.Id ) ) )
        {
          // toggle visibility 
          doc.ActiveView.setVisibility( c,
            !c.get_Visible( doc.ActiveView ) );
        }
      }
    }

    private void CheckLinkedBic( Document doc )
    {
      BuiltInCategory bic = BuiltInCategory.OST_RvtLinks;
      ElementIterator i = doc.Elements;
      Element e;
      while( i.MoveNext() )
      {
        e = i.Current as Element;
        if( e.Name.Contains( ".rvt" )
          && e.Category != null
          && e.Category.Id.Value.Equals( (int) bic ) )
        {
          break;
        }
      }
    }
  }
  #endregion // Lab2_7_ListLinkedFiles

  #region Lab2_8_VisibleElements
  /// <summary>
  /// List all model elements.
  /// </summary>
  public class Lab2_8_VisibleElements : IExternalCommand
  {
    //int _bic_lghting_fixtures 
    //  = ( int ) BuiltInCategory.OST_LightingFixtures;

    static int[] _bics_to_skip = new int[] {
      ( int ) BuiltInCategory.OST_PreviewLegendComponents,
      ( int ) BuiltInCategory.OST_IOSSketchGrid,
      ( int ) BuiltInCategory.OST_Cameras,
    };

    static bool SkipThisBic( int bic )
    {
      bool exists = Array.Exists<int>(
        _bics_to_skip,
        delegate( int i )
        {
          return i == bic;
        } );

      return exists;
    }

    static Filter OrType(
      Filter f,
      Type t,
      Autodesk.Revit.Creation.Filter cf )
    {
      Filter f2 = cf.NewTypeFilter( t );
      return cf.NewLogicOrFilter( f, f2 );
    }

    /// <summary>
    /// Return true if the given element is hidden in this view,
    /// either as an individual element, or because its category
    /// or one of its parent categories is hidden.
    /// </summary>
    static bool IsHiddenElementOrCategory(
      Element e,
      View v )
    {
      bool hidden = e.IsHidden( v );

      if( !hidden )
      {
        Category cat = e.Category;
        while( null != cat && !hidden )
        {
          hidden = !cat.get_Visible( v );
          cat = cat.Parent;
        }
      }
      return hidden;
    }

    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;
      Document doc = app.ActiveDocument;
      View view = doc.ActiveView;

      Autodesk.Revit.Creation.Filter cf
        = app.Create.Filter;

      List<Element> els = new List<Element>();
      Filter f = cf.NewTypeFilter( typeof( Symbol ) );
      f = OrType( f, typeof( FamilyBase ), cf );
      f = OrType( f, typeof( WallType ), cf );
      f = cf.NewLogicNotFilter( f );
      doc.get_Elements( f, els );

      Geo.Options opt = app.Create.NewGeometryOptions();

      string s = string.Empty;
      int count = 0, countHiddenElement = 0, countHiddenCategory = 0;
      bool hiddenElement, hiddenCategory, hidden;

      //
      // lighting fixture category is visible:
      //
      BuiltInCategory bicLf = BuiltInCategory.OST_LightingFixtures;
      Category catLf = doc.Settings.Categories.get_Item( bicLf );
      hiddenCategory = !catLf.get_Visible( view );

      //
      // light source category is hidden:
      //
      BuiltInCategory bicLfs = BuiltInCategory.OST_LightingFixtureSource;
      Category catLfs = doc.Settings.Categories.get_Item( bicLfs );
      hiddenCategory = !catLfs.get_Visible( view );

      foreach( Element e in els )
      {
        if( null != e.Category )
        {
          //
          // both lighting fixtures and light sources category 
          // is returned as OST_LightingFixtures, never 
          // OST_LightingFixtureSource:
          //
          int bic = e.Category.Id.Value;
          if( !SkipThisBic( bic )
            && ( null != e.get_Geometry( opt ) ) )
          {
            hiddenElement = e.IsHidden( view );
            countHiddenElement += hiddenElement ? 1 : 0;

            hiddenCategory = false;

            //
            // workaround to fix the wrong category returned:
            //
            // The category returned by the Category property 
            // on the light source elements named 
            // "Eclipse Linsenwandfluter 75381000_Lichtkörper" 
            // is Lighting Fixture instead of the correct 
            // subcategory Light Source.
            //
            // Luckily, the element name of all the light source 
            // elements contains the substring "_Lichtkörper".
            //
            Category cat = e.Category;
            if( catLf.Id.Equals( cat.Id )
              && e.Name.Contains( "_Lichtkörper" ) )
            {
              cat = catLfs;
            }

            try
            {
              hiddenCategory = !cat.get_Visible( view );

              /*CategoryNameMap subcats = e.Category.SubCategories;
              CategoryNameMapIterator it = subcats.ForwardIterator();
              while( it.MoveNext() )
              {
                Category c = it.Current as Category;
                hiddenCategory = !c.get_Visible( view );
              }*/

              countHiddenCategory += hiddenCategory ? 1 : 0;
            }
            catch( Exception ex )
            {
              Debug.Print( "{0} cat {1}: {2}",
                LabUtils.ElementDescription( e ),
                e.Category.Id.Value,
                ex.Message );
            }

            //
            // todo: check parent category visibility as well!
            //

            hidden = hiddenElement || hiddenCategory;

            bool hidden2 = IsHiddenElementOrCategory( e, view );

            s += string.Format(
              "\r\n  Category={0}; Name={1}; Id={2}; Hidden = element {3} + category {4} = {5} = {6}",
              e.Category.Name, e.Name,
              e.Id.Value.ToString(),
              ( hiddenElement ? "Yes" : "No" ),
              ( hiddenCategory ? "Yes" : "No" ),
              ( hidden ? "Yes" : "No" ),
              ( hidden2 ? "Yes" : "No" ) );

            if( !hidden )
            {
              ++count;
            }
          }
        }
      }

      s = "There are " + count.ToString()
        + " visible elements:" + s;

      LabUtils.InfoMsg( s );

      return CmdResult.Failed;
    }
  }
  #endregion // Lab2_8_VisibleElements
}
