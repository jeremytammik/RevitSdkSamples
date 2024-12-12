#region Header
// Revit API .NET Labs
//
// Copyright (C) 2007-2010 by Autodesk, Inc.
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
using System.IO;
using System.Linq;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
#endregion // Namespaces

namespace Labs
{
  #region Lab2_0_CreateLittleHouse
  /// <summary>
  /// Create a little house with some sample building elements.
  /// We create a simple building consisting of four walls,
  /// a door, two windows, a floor, a roof, a room and a room tag.
  /// <include file='../doc/labs.xml' path='labs/lab[@name="2-0"]/*' />
  /// </summary>
  [Transaction( TransactionMode.Automatic )]
  [Regeneration( RegenerationOption.Manual )]
  public class Lab2_0_CreateLittleHouse : IExternalCommand
  {
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
        Autodesk.Revit.Creation.Application createApp = app.Application.Create;
        Autodesk.Revit.Creation.Document createDoc = doc.Create;
        //
        // determine the four corners of the rectangular house:
        //
        double width = 7 * LabConstants.MeterToFeet;
        double depth = 4 * LabConstants.MeterToFeet;

        List<XYZ> corners = new List<XYZ>( 4 );

        corners.Add( XYZ.Zero );
        corners.Add( new XYZ( width, 0, 0 ) );
        corners.Add( new XYZ( width, depth, 0 ) );
        corners.Add( new XYZ( 0, depth, 0 ) );

        #region Test creating two levels
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
        #endregion // Test creating two levels

        //
        // determine the levels where the walls will be located:
        //
        Level levelBottom = null;
        Level levelTop = null;

        if( !LabUtils.GetBottomAndTopLevels( doc, ref levelBottom, ref levelTop ) )
        {
          message = "Unable to determine wall bottom and top levels";
          return Result.Failed;
        }
        Debug.Print( string.Format( "Drawing walls on '{0}' up to '{1}'",
          levelBottom.Name, levelTop.Name ) );
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
          param.Set( topLevelId );
          walls.Add( wall );
        }
        //
        // determine wall thickness for tag offset and profile growth:
        //
        double wallThickness = walls[0].WallType.CompoundStructure.Layers.get_Item( 0 ).Thickness;
        //
        // add door and windows to the first wall;
        // note that the NewFamilyInstance() api method does not automatically add door
        // and window tags, like the ui command does. we add tags here by making additional calls
        // to NewTag():
        //
        FamilySymbol door = LabUtils.GetFirstFamilySymbol( doc, BuiltInCategory.OST_Doors );
        if( null == door )
        {
          LabUtils.InfoMsg( "No door symbol found." );
          return Result.Failed;
        }
        FamilySymbol window = LabUtils.GetFirstFamilySymbol( doc, BuiltInCategory.OST_Windows );
        if( null == window )
        {
          LabUtils.InfoMsg( "No window symbol found." );
          return Result.Failed;
        }
        XYZ midpoint = LabUtils.Midpoint( corners[0], corners[1] );
        XYZ p = LabUtils.Midpoint( corners[0], midpoint );
        XYZ q = LabUtils.Midpoint( midpoint, corners[1] );
        double tagOffset = 3 * wallThickness;
        //double windowHeight = 1 * LabConstants.MeterToFeet;
        double windowHeight = levelBottom.Elevation + 0.3 * ( levelTop.Elevation - levelBottom.Elevation );
        p = new XYZ( p.X, p.Y, windowHeight );
        q = new XYZ( q.X, q.Y, windowHeight );
        View view = doc.ActiveView;
        FamilyInstance inst = createDoc.NewFamilyInstance(
          midpoint, door, walls[0], levelBottom, StructuralType.NonStructural );
        midpoint += tagOffset * XYZ.BasisY;
        IndependentTag tag = createDoc.NewTag(
          view, inst, false, TagMode.TM_ADDBY_CATEGORY, TagOrientation.TAG_HORIZONTAL, midpoint );
        inst = createDoc.NewFamilyInstance( p, window, walls[0], levelBottom, StructuralType.NonStructural );
        p += tagOffset * XYZ.BasisY;
        tag = createDoc.NewTag( view, inst, false, TagMode.TM_ADDBY_CATEGORY, TagOrientation.TAG_HORIZONTAL, p );
        inst = createDoc.NewFamilyInstance( q, window, walls[0], levelBottom, StructuralType.NonStructural );
        q += tagOffset * XYZ.BasisY;
        tag = createDoc.NewTag( view, inst, false, TagMode.TM_ADDBY_CATEGORY, TagOrientation.TAG_HORIZONTAL, q );
        //
        // grow the profile out by half the wall thickness,
        // so the floor and roof do not stop halfway through the wall:
        //
        double w = 0.5 * wallThickness;
        corners[0] -= w * ( XYZ.BasisX + XYZ.BasisY );
        corners[1] += w * ( XYZ.BasisX - XYZ.BasisY );
        corners[2] += w * ( XYZ.BasisX + XYZ.BasisY );
        corners[3] -= w * ( XYZ.BasisX - XYZ.BasisY );
        CurveArray profile = new CurveArray();
        for( int i = 0; i < 4; ++i )
        {
          Line line = createApp.NewLineBound( corners[i], corners[3 == i ? 0 : i + 1] );
          profile.Append( line );
        }
        //
        // add a floor, a roof, the roof slope, a room and a room tag:
        //
        bool structural = false;
        Floor floor = createDoc.NewFloor( profile, structural );
        List<Element> roofTypes = new List<Element>( LabUtils.GetElementsOfType(
          doc, typeof( RoofType ), BuiltInCategory.OST_Roofs ) );
        Debug.Assert( 0 < roofTypes.Count, "expected at least one roof type to be loaded into project" );
        RoofType roofType = roofTypes[0] as RoofType;
        ModelCurveArray modelCurves = new ModelCurveArray();
        FootPrintRoof roof = createDoc.NewFootPrintRoof(
          profile, levelTop, roofType, out modelCurves );
        //
        // regenerate the model after roof creation, otherwise both the calls
        // to set_DefinesSlope and set_SlopeAngle throwing the exception
        // "Unable to access curves from the roof sketch."
        //
        doc.Regenerate();
        // 
        // the argument to set_SlopeAngle is NOT an angle, it is
        // really a slope, i.e. relation of height to distance,
        // e.g. 0.5 = 6" / 12", 0.75  = 9" / 12", etc.
        //
        //double slopeAngle = 30 * LabConstants.DegreesToRadians;
        double slope = 0.3;
        foreach( ModelCurve curve in modelCurves )
        {
          roof.set_DefinesSlope( curve, true );
          roof.set_SlopeAngle( curve, slope );
        }
        Room room = createDoc.NewRoom( levelBottom, new UV( 0.5 * width, 0.5 * depth ) );
        RoomTag roomTag = createDoc.NewRoomTag( room, new UV( 0.5 * width, 0.7 * depth ), null );

        //doc.AutoJoinElements(); // todo: remove this, the transaction should perform this automatically

        //LabUtils.InfoMsg( "Little house was created successfully." );
        return Result.Succeeded;
      }
      catch( Exception ex )
      {
        message = ex.Message;
        return Result.Failed;
      }
    }
  }
  #endregion // Lab2_0_CreateLittleHouse

  #region Lab2_1_Elements
  // Cf. C:\a\doc\revit\2011\constellation\get_all_elements.txt
  /// <summary>
  /// List all document elements.
  /// This is not recommended for normal use!
  /// <include file='../doc/labs.xml' path='labs/lab[@name="2-1"]/*' />
  /// </summary>
  [Transaction( TransactionMode.ReadOnly )]
  [Regeneration( RegenerationOption.Manual )]
  public class Lab2_1_Elements : IExternalCommand
  {
    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      #region 2.1.a. Access Revit doc and open output file:
      UIApplication app = commandData.Application;
      Document doc = app.ActiveUIDocument.Document;

      // .NET exception handling should be done everywhere,
      // but will sometimes be omitted for clarity in the
      // following labs unless we expect exceptions:

      StreamWriter sw;

      try
      {
        sw = new StreamWriter( LabConstants.FilePath );
      }
      catch( Exception e )
      {
        LabUtils.ErrorMsg( string.Format( "Cannot open '{0}': {1}",
          LabConstants.FilePath, e.Message ) );
        return Result.Failed;
      }
      #endregion // 2.1.a. Access Revit doc and open output file

      try
      {
        WaitCursor waitCursor = new WaitCursor();

        #region 2.1.b. Set up element collector to retrieve all elements:
        // the Revit API does not expect an application
        // ever to need to iterate over all elements.
        // To do so, we need to use a trick: ask for all
        // elements fulfilling a specific criteria and
        // unite them with all elements NOT fulfilling
        // the same criteria:

        FilteredElementCollector collector
          = new FilteredElementCollector( doc )
            .WhereElementIsElementType();

        FilteredElementCollector collector2
          = new FilteredElementCollector( doc )
            .WhereElementIsNotElementType();

        collector.UnionWith( collector2 );
        #endregion // 2.1.b. Set up element collector to retrieve all elements

        #region 2.1.c. Loop over the elements, list their data, and close the file:
        string s, line;

        foreach( Element e in collector )
        {
          line = "Id=" + e.Id.IntegerValue.ToString(); // element id
          line += "; Class=" + e.GetType().Name; // element class, i.e. System.Type

          // The element category is not implemented for all classes,
          // and may return null; for Family elements, one can sometimes
          // use the FamilyCategory property instead.

          s = string.Empty;

          if( null != e.Category )
          {
            s = e.Category.Name;
          }
          if( 0 == s.Length && e is Family && null != ((Family) e).FamilyCategory )
          {
            s = ((Family) e).FamilyCategory.Name;
          }
          if( 0 == s.Length )
          {
            s = "?";
          }
          line += "; Category=" + s;

          // The element Name has a different meaning for different classes,
          // but is mostly implemented "logically". More precise info on elements
          // can be obtained in class-specific ways.

          line += "; Name=" + e.Name;
          line += "; UniqueId=" + e.UniqueId;
          //line += "; Guid=" + GetGuid( e.UniqueId );
          sw.WriteLine( line );
        }
        sw.Close();

        LabUtils.InfoMsg( "Element list has been written to "
          + LabConstants.FilePath + "." );

        #endregion // 2.1.c. Loop over the elements, list their data, and close the output file
      }
      catch( Exception e )
      {
        message = e.Message;
      }
      return Result.Failed;
    }
  }
  #endregion // Lab2_1_Elements

  #region Lab2_2_ModelElements
  /// <summary>
  /// List all model elements.
  /// <include file='../doc/labs.xml' path='labs/lab[@name="2-2"]/*' />
  /// </summary>
  /// <include file='../doc/labs.xml' path='labs/lab[@name="2-2-remarks"]/*' />
  [Transaction( TransactionMode.ReadOnly )]
  [Regeneration( RegenerationOption.Manual )]
  public class Lab2_2_ModelElements : IExternalCommand
  {
    #region Konstanty
    Dictionary<string, Category> GetAllCategories1( 
      List<Document> allDocuments )
    {
      Dictionary<string, Category> categories 
        = new Dictionary<string, Category>();

      Dictionary<int, Element> elemnts 
        = new Dictionary<int, Element>();

      foreach( Document doc in allDocuments )
      {
        FilteredElementCollector collector 
          = new FilteredElementCollector( doc );

        IList<Element> found 
          = collector
            .WhereElementIsNotElementType()
            .WhereElementIsViewIndependent()
            .WherePasses( new LogicalOrFilter( 
              new ElementIsElementTypeFilter( false ), 
              new ElementIsElementTypeFilter( true ) ) )
            .ToElements();

        var disElems = (from elem in found select elem)
          .Distinct();

        foreach( Element element in disElems )
        {
          if( element.Category != null )
          {
            if( element.Parameters.Size > 0 )
            {
              if( element.PhaseCreated != null )
              {
                if( !categories.ContainsKey( 
                  element.Category.Name ) )
                {
                  categories.Add( 
                    element.Category.Name, 
                    element.Category );

                  elemnts.Add( 
                    element.Category.Id.IntegerValue, 
                    element );
                }
              }
              else if( element.Location != null )
              {
                LocationPoint point = null;
                LocationCurve curve = null;
                try
                {
                  point = element.Location 
                    as LocationPoint;
                }
                catch { }

                try
                {
                  curve = element.Location 
                    as LocationCurve;
                }
                catch { }

                if( curve != null || point != null )
                {
                  if( !categories.ContainsKey( 
                    element.Category.Name ) )
                  {
                    categories.Add( 
                      element.Category.Name, 
                      element.Category );

                    elemnts.Add( 
                      element.Category.Id.IntegerValue, 
                      element );
                  }
                }
              }
            }
          }
        }
      }
      return categories;
    }

    private void GetAllCategories( 
      List<Document> allDocuments )
    {
      Dictionary<string, Category> categories 
        = new Dictionary<string, Category>();

      List<Element> elements = new List<Element>();

      foreach( Document doc in allDocuments )
      {
        FilteredElementCollector collector 
          = new FilteredElementCollector( doc );

        collector
          .WhereElementIsNotElementType()
          .WhereElementIsViewIndependent()
          .ToElements();

        foreach( Element element in collector )
        {
          if( null != element.Category
            && 0 < element.Parameters.Size
            && (element.Category.HasMaterialQuantities 
              || null != element.PhaseCreated) )
          {
            if( !categories.ContainsKey( 
              element.Category.Name ) )
            {
              categories.Add( 
                element.Category.Name, 
                element.Category );
            }
            elements.Add( element );
          }
        }
      }
    }
    #endregion // Konstanty

    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      #region 2.2 List all model elements:

      UIApplication app = commandData.Application;
      Document doc = app.ActiveUIDocument.Document;

      FilteredElementCollector collector
        = new FilteredElementCollector( doc )
          .WhereElementIsNotElementType();

      List<string> a = new List<string>();

      // we could use a LINQ query here instead:

      foreach( Element e in collector )
      {
        //  && null != e.Materials
        //  && 0 < e.Materials.Size

        if( null != e.Category
          && e.Category.HasMaterialQuantities )
        {
          a.Add( string.Format(
            "Category={0}; Name={1}; Id={2}",
            e.Category.Name, e.Name, 
            e.Id.IntegerValue ) );
        }
      }

      LabUtils.InfoMsg( 
        "Project contains {0} model element{1}{2}", a );

      return Result.Failed;

      #endregion // 2.2 List all model elements
    }
  }
  #endregion // Lab2_2_ModelElements

  #region Lab2_3_ListWallsAndDoors
  /// <summary>
  /// Retrieving family instances: list all walls and doors.
  ///
  /// These represent two different kinds of elements:
  /// walls are represented by their own specialised
  /// System.Type Wall, whereas doors are represented
  /// by family instances and need to be identified by
  /// additionally checking the category.
  /// <include file='../doc/labs.xml' path='labs/lab[@name="2-3"]/*' />
  /// </summary>
  [Transaction( TransactionMode.ReadOnly )]
  [Regeneration( RegenerationOption.Manual )]
  public class Lab2_3_ListWallsAndDoors : IExternalCommand
  {
    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      UIApplication app = commandData.Application;
      Document doc = app.ActiveUIDocument.Document;

      #region 2.3.a Filter to retrieve and list all walls:
      // get all wall elements:
      //
      // we could also call
      //
      // FilteredElementCollector walls = LabUtils.GetElementsOfType(
      //   doc, typeof( Wall ), BuiltInCategory.OST_Walls );

      FilteredElementCollector walls = new FilteredElementCollector( doc );
      walls.OfClass( typeof( Wall ) );

      List<string> a = new List<string>();

      foreach( Wall wall in walls )
      {
        a.Add( string.Format( "Id={0}; Kind={1}; Type={2}",
          wall.Id.IntegerValue,
          wall.WallType.Kind.ToString(),
          wall.WallType.Name ) );
      }

      LabUtils.InfoMsg( "{0} wall{1} in the model{2}", a );
      #endregion // 2.3.a Filter to retrieve and list all walls

      a.Clear();

      #region 2.3.b Filter to retrieve and list all doors:
      // get all door family instances:
      //
      // we could also call
      //
      // FilteredElementCollector doors = LabUtils.GetElementsOfType(
      //   doc, typeof( FamilyInstance ), BuiltInCategory.OST_Doors );
      //
      // or
      //
      // FilteredElementCollector doors = LabUtils.GetFamilyInstances(
      //   doc, BuiltInCategory.OST_Doors );

      FilteredElementCollector doors = new FilteredElementCollector( doc );
      doors.OfCategory( BuiltInCategory.OST_Doors );
      doors.OfClass( typeof( FamilyInstance ) );

      foreach( FamilyInstance door in doors )
      {
        // For family instances, the element name property
        // returns the type name:

        a.Add( string.Format( "Id={0}; Type={1}",
          door.Id.IntegerValue, door.Name ) );
      }

      LabUtils.InfoMsg( "{0} door family instance{1} in the model{2}", a );
      #endregion // 2.3.b Filter to retrieve and list all doors

      return Result.Failed;
    }
  }
  #endregion // Lab2_3_ListWallsAndDoors

  #region Lab2_4_EditFamilyInstance
  /// <summary>
  /// Demonstrate access to family instance parameters
  /// and modification of family instance location.
  ///
  /// Edit all doors in the current project.
  /// Move the doors up 0.2 feet via the Document.Move method
  /// and widen them 1 foot by changing the WINDOW_WIDTH parameter value.
  ///
  /// <include file='../doc/labs.xml' path='labs/lab[@name="2-4"]/*' />
  /// </summary>
  [Transaction( TransactionMode.Automatic )]
  [Regeneration( RegenerationOption.Manual )]
  public class Lab2_4_EditFamilyInstance : IExternalCommand
  {
    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      UIApplication app = commandData.Application;
      Document doc = app.ActiveUIDocument.Document;

      try
      {
        #region 2.4 Retrieve all doors, move them and widen them:
        FilteredElementCollector doors = LabUtils.GetFamilyInstances(
          doc, BuiltInCategory.OST_Doors );

        // move doors up 0.2 feet:

        XYZ v = 0.2 * XYZ.BasisZ;

        foreach( FamilyInstance door in doors )
        {
          doc.Move( door, v );

          // widen doors by one foot by changing parameter value:

          Parameter p = door.Symbol.get_Parameter(
            BuiltInParameter.WINDOW_WIDTH );

          if( null != p )
          {
            double width = p.AsDouble();
            width += 1.0;
            p.Set( width );
          }
        }
        return Result.Succeeded;
        #endregion // 2.4 Retrieve all doors, move them and widen them
      }
      catch( Exception ex )
      {
        message = ex.Message;
        return Result.Failed;
      }
    }
  }
  #endregion // Lab2_4_EditFamilyInstance

  #region Lab2_5_SelectAndMoveWallAndAddColumns
  /// <summary>
  /// Demonstrate creation of new family instance elements
  /// by querying an existing wall for its location and parameters,
  /// modifying it, and inserting column elements.
  /// <include file='../doc/labs.xml' path='labs/lab[@name="2-5"]/*' />
  /// </summary>
  /// <remarks>
  /// Note: the column can be seen in 3D view by setting argument to StructuralType.Column,
  /// but cannot by StructuralType.NonStructural, since the latter is only visible in plan view.
  /// This is a temporary problem, NewFamilyInstance identifies the nonstructural instance
  /// as an annotation instance, so only shows them in plan view.
  /// </remarks>
  [Transaction( TransactionMode.Automatic )]
  [Regeneration( RegenerationOption.Manual )]
  public class Lab2_5_SelectAndMoveWallAndAddColumns : IExternalCommand
  {
    /// <summary>
    /// A selection filter for wall elements.
    /// </summary>
    class WallSelectionFilter : ISelectionFilter
    {
      //const BuiltInCategory _bic = BuiltInCategory.OST_Walls;

      /// <summary>
      /// Allow wall to be selected.
      /// </summary>
      /// <param name="element">A candidate element in selection operation.</param>
      /// <returns>Return true for wall, false for all other elements.</returns>
      public bool AllowElement( Element e )
      {
        //return null != e.Category
        // && e.Category.Id.IntegerValue == ( int ) _bic;

        return e is Wall;
      }

      /// <summary>
      /// Allow all the reference to be selected
      /// </summary>
      /// <param name="refer">A candidate reference in selection operation.</param>
      /// <param name="point">The 3D position of the mouse on the candidate reference.</param>
      /// <returns>Return true to allow the user to select this candidate reference.</returns>
      public bool AllowReference( Reference r, XYZ p )
      {
        return true;
      }
    }

    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      UIApplication app = commandData.Application;
      UIDocument uidoc = app.ActiveUIDocument;
      Document doc = uidoc.Document;

      ElementSet ss = uidoc.Selection.Elements;

      Wall wall = null;

      if( 0 < ss.Size )
      {
        // old pre-selection handling:

        // must be one single element only:

        if( 1 != ss.Size )
        {
          message = "Please pre-select a single wall element.";
          return Result.Failed;
        }

        // must be a wall:

        ElementSetIterator it = ss.ForwardIterator();
        it.MoveNext();
        Element e = it.Current as Element;

        if( !( e is Wall ) )
        {
          message = "Selected element is NOT a wall.";
          return Result.Failed;
        }
        wall = e as Wall;
      }
      else
      {
        // new prompt for filtered selection allowing only walls:

        Reference r = uidoc.Selection.PickObject(
          ObjectType.Element, new WallSelectionFilter(),
          "Please pick a wall" );

        wall = r.Element as Wall;
      }

      // wall must be constrained to a level at the top (more on parameters later):

      Level topLev = null;

      try
      {
        ElementId id = wall.get_Parameter( BuiltInParameter.WALL_HEIGHT_TYPE ).AsElementId();
        topLev = doc.get_Element( id ) as Level;
      }
      catch( Exception )
      {
        topLev = null;
      }

      if( null == topLev )
      {
        message = "Selected wall is not constrained to a level at the top.";
        return Result.Failed;
      }

      // get the bottom level as well (this should never fail):

      Level botLev = null;

      try
      {
        ElementId id = wall.get_Parameter( BuiltInParameter.WALL_BASE_CONSTRAINT ).AsElementId();
        botLev = doc.get_Element( id ) as Level;
      }
      catch( Exception )
      {
        botLev = null;
      }

      if( null == botLev )
      {
        message = "Selected wall is not constrained to a level at the bottom.";
        return Result.Failed;
      }

      // Calculate the location points for the 3 columns (assuming straight wall)
      LocationCurve locCurve = wall.Location as LocationCurve;

      XYZ ptStart = locCurve.Curve.get_EndPoint( 0 );
      XYZ ptEnd = locCurve.Curve.get_EndPoint( 1 );
      XYZ ptMid = 0.5 * ( ptStart + ptEnd );

      List<XYZ> locations = new List<XYZ>( 3 );
      locations.Add( ptStart );
      locations.Add( ptMid );
      locations.Add( ptEnd );

      string s = "{0} location{1} for the new columns in raw database coordinates, e.g. feet{2}";
      List<string> a = new List<string>();
      a.Add( "Start: " + LabUtils.PointString( ptStart ) );
      a.Add( "Mid  : " + LabUtils.PointString( ptMid ) );
      a.Add( "End  : " + LabUtils.PointString( ptEnd ) );
      LabUtils.InfoMsg( s, a );

      // retrieve the family type for the new instances.
      // if needed, change the names to match a column
      // type available in the model:

      string family_name = "M_Wood Timber Column";
      string type_name = "191 x 292mm";

      FilteredElementCollector collector = new FilteredElementCollector( doc );
      collector.OfCategory( BuiltInCategory.OST_Columns );
      collector.OfClass( typeof( FamilySymbol ) );

      // LINQ query to find element with given name:
      //
      // ... note that this could also be achieved by
      // filtering for the element name parameter value.

      var column_types = from element in collector
        //where ((FamilySymbol)element).Family.Name == family_name
        where element.Name == type_name
        select element;

      FamilySymbol symbol = null;

      try
      {
        symbol = column_types.Cast<FamilySymbol>().First<FamilySymbol>();
      }
      catch
      {
      }

      if( null == symbol )
      {
        message = string.Format(
          "Cannot find type '{0}' in family '{1}' in the current model - please load it first.",
          type_name, family_name );
        return Result.Failed;
      }

      // insert column family instances:

      foreach( XYZ p in locations )
      {
        try
        {
          // Note: Currently there is a problem.
          // If we set the type as NonStructural, it is treated as Annotation instance,
          // and it shows only in plan view.
          // FamilyInstance column = doc.Create.NewFamilyInstance( p, symbol, botLev, StructuralType.NonStuctural );

          FamilyInstance column = doc.Create.NewFamilyInstance( 
            p, symbol, botLev, StructuralType.Column );

          Parameter paramTopLevel = column.get_Parameter( 
            BuiltInParameter.FAMILY_TOP_LEVEL_PARAM );

          ElementId id = topLev.Id;
 
          paramTopLevel.Set( id );
        }
        catch( Exception )
        {
          LabUtils.ErrorMsg( "Failed to create or adjust column." );
        }
      }

      // Finally, move the wall so the columns are visible.
      // We move the wall perpendicularly to its location
      // curve by one tenth of its length:

      XYZ v = new XYZ(
        -0.1 * ( ptEnd.Y - ptStart.Y ),
        0.1 * ( ptEnd.X - ptStart.X ),
        0 );

      if( !wall.Location.Move( v ) )
      {
        LabUtils.ErrorMsg( "Failed to move the wall." );
      }
      return Result.Succeeded;
    }
  }
  #endregion // Lab2_5_SelectAndMoveWallAndAddColumns
}
