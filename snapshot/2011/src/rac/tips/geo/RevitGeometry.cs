#region Header
// Copyright (c) 2007-2009 by Autodesk, Inc.
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
//
// Created by Joe Ye and Jeremy Tammik, Autodesk Inc., 2007-10-20.
#endregion // Header

#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
#endregion // Namespaces

namespace RevitGeometry
{
  #region 1. XYZ and Transform
  /// <summary>
  /// 1. XYZ and Transform
  ///
  /// This section demonstrates how to use the basic point,
  /// vector and transform geometry types: UV/XYZ and Transform.
  /// The first command defined in the sample code demonstrates the XYZ
  /// and Transform classes, and their properties and member functions.
  /// The sample does not require any preparation in Revit, nor does it
  /// generate any visible output in Revit. You can view variable values
  /// and other results in the watch window of the Visual Studio debugger.
  /// </summary>
  [Transaction( TransactionMode.Automatic )]
  [Regeneration( RegenerationOption.Manual )]
  public class CmdXyzAndTransform : IExternalCommand
  {
    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      //
      // Basic Point, Vector and Matrix Geometry Types: UV/XYZ and Transform
      //
      // To explore the geometry functionality provided in the Revit API,
      // we have migrated some samples from the ObjectARX AcGe sections.
      // The following examples show some of the most commonly used functions
      // and operators of the point, vector, and transformation classes.
      // These examples use the 3D classes, but most of them apply to the
      // 2D ones as well.
      //
      // Creation application, used to create new non-database object instances:
      //
      Autodesk.Revit.Creation.Application creApp = commandData.Application.Application.Create;
      //
      // The default constructor for points and vectors initializes all coordinates to 0.
      // Points and vectors may also be constructed by specifying their coordinates:
      //
      XYZ p1 = new XYZ( 2.0, 5.0, -7.5 ), p2, p3 = new XYZ( 1.0, 2.0, 3.0 );
      XYZ v1 = new XYZ( 1.0, 4.0, 5.0 ), v2 = new XYZ( 0.0, 1.0, -1.0 ), v3;
      //
      // The point and vector classes provide Add and Subtract methods.
      // Examples of adding and subtracting points and vectors:
      //
      p2 = p1 + v1; // Set p2 to sum of p1 and v1.
      p1 = p1 + v1; // Add v1 to p1.
      p3 = p3 - v1; // Subtract v1 from p3.
      v3 = v1 + v2; // Set v3 to sum of v1 and v2.
      v1 = v1 + v2; // Add v2 to v1.
      v3 = v1 - v2; // Set v3 to the difference of v1 and v2.
      //
      // Examples of how to obtain the negative of a vector:
      //
      v2 = v1.Negate(); // Set v2 to negative of v1.
      v1 = v1.Negate(); // This is equivalent to v1 = -v1.
      //
      // Examples of different ways to scale a vector:
      //
      v1 = v1.Multiply( 2.0 ); // Doubles the length of v1.
      v3 = v1.Divide( 2.0 ); // Set v3 to half the length of v1.
      v1 = v1.Normalize(); // Make v1 a unit vector.
      //
      // The point classes contain a number of query
      // functions for computing distances and lengths:
      //
      double len = v2.GetLength(); // Length of v2.
      len = p1.DistanceTo( p2 ); // Distance from p1 to p2.
      //
      // Compute the angle between two 3D vectors:
      //
      Double angle = v1.AngleTo( v2 );
      //
      // Check for zero length:
      //
      // Note: Revit API does not provide straightforward functions
      // to test if two vectors are perpendicular or parallel.
      // Simple helper methods to fufill these tasks can be created.
      //
      if( v1.IsZeroLength() )
      {
        // v1 is zero
      }
      //
      // The vector class provides the standard multiplication operations:
      //
      len = v1.DotProduct( v2 );
      v3 = v1.CrossProduct( v2 );
      //
      // The Creation.Application does not provide any methods to create transform objects.
      // A Transform instace can be created using the new operator as a copy constructor,
      // or by using static methods of the Transform class.
      // The transform default constructor initializes it to identity:
      //
      Transform trans1, trans2, trans3;
      XYZ ptOrigin = XYZ.Zero;
      XYZ ptXAxis = XYZ.BasisX;
      XYZ ptYAxis = XYZ.BasisY;

      trans1 = Transform.get_Rotation( ptOrigin, ptXAxis, 90 ); // get rotation transform
      p3 = trans1.OfPoint( p2 );

      trans2 = Transform.get_Translation( ptXAxis ); // get translation transform
      p3 = trans2.OfPoint( p2 );

      Plane plane1 = creApp.NewPlane( ptXAxis, ptYAxis );
      trans3 = Transform.get_Reflection( plane1 ); // get mirror transform, requires plane

      p3 = trans1.OfPoint( p2 ); // translate point
      //
      // Test the transform determinant for zero.
      // If the determinant is zero, it cannot be inverted:
      //
      if( 0.0001 < trans2.Determinant )
      {
        trans3 = trans2.Inverse;
      }
      //
      // The * operator concatenates matrices, just like the Multiply() method:
      //
      trans3 = trans1 * trans2;
      trans3 = trans1.Multiply(trans2);
      //
      // Scale the basis vectors of this transformation:
      //
      trans1 = trans2.ScaleBasis( 2.0 );
      //
      // Note: There is no straightforward method in Transform
      // to test whether it is uniform, i.e. does not change
      // the shape of an entity to which it is applied.
      //
      return Result.Succeeded;
    }
  }
  #endregion // 1. XYZ and Transform

  #region 2. Curve and Line
  /// <summary>
  /// 2. Curve and Line
  ///
  /// This command demonstrates the Curve and Line classes.
  /// The sample does not require any preparation in Revit, nor does it
  /// generate any visible output in Revit. You can view variable values
  /// and other results in the watch window of the Visual Studio debugger.
  /// </summary>
  [Transaction( TransactionMode.Automatic )]
  [Regeneration( RegenerationOption.Manual )]
  public class CmdCurve : IExternalCommand
  {
    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      //
      // Using the line and plane classes
      //
      // The following examples show some of the commonly used line functions.
      // These examples show how to use the line class for basic linear algebra operations.
      // Although the examples use the line classes, many of the methods can be used by other
      // curve classes as well.
      //
      Autodesk.Revit.Creation.Application creApp = commandData.Application.Application.Create;
      //
      // Create a Plane instance:
      //
      XYZ xyzNorm = new XYZ( 0, 0, 1 );
      XYZ ptOrigin = new XYZ( 0, 0, 0 );
      Plane plane1 = creApp.NewPlane( xyzNorm, ptOrigin );
      //
      // Create a Line instance:
      //
      XYZ xyzSta = new XYZ( 2, 1, 0 );
      XYZ xyzEnd = new XYZ( 2, 5, 0 );
      Line line1 = creApp.NewLineBound( xyzSta, xyzEnd );
      //
      // Get line properties:
      //
      Double dLen = line1.Length; // length
      XYZ xyz1 = line1.get_EndPoint( 0 ); // start point
      XYZ xyz2 = line1.get_EndPoint( 1 ); // end point
      //
      // Note: the Line class does not provide methods to test if a
      // point is on it, and to obtain the line direction of line.
      // These properties can be directy calculated from the
      // distance of the point to the line and as the normalised
      // difference between the end and start points.
      //
      // Get distance to a point:
      //
      XYZ xyzPoint = new XYZ( 5, 6, 0 );
      double dist = line1.Distance( xyzPoint );
      //
      // There is no method to get the closest point on the line.
      //
      // Test if the line is restricted to an interval:
      //
      bool isBounded = line1.IsBound;
      //
      // Note: the Line class does not provide methods to
      // test if two lines are parallel or perpendicular.
      //
      // Get intersection points with another line:
      //
      xyzSta = new XYZ( 1, 1, 0 );
      xyzEnd = new XYZ( 5, 1, 0 );
      Line line2 = creApp.NewLineBound( xyzSta, xyzEnd );
      IntersectionResultArray interArray = null;
      line1.Intersect( line2, out interArray );
      //
      // Clone line:
      // An instance of a Curve derived class can also be
      // created by methods in the Creation.Application class.
      //
      Curve curve1 = line1.Clone();
      //
      // Project the given point onto this curve:
      //
      IntersectionResult interResult = line1.Project( xyzEnd );
      return Result.Succeeded;
    }
  }
  #endregion // 2. Curve and Line

  #region 3. Circle
  /// <summary>
  /// 3. Circle
  ///
  /// This command demonstrates how to create a circle in Revit.
  /// There is no necessary preparation for this command.
  /// After launching this command, a circle is created in the current Revit view.
  /// </summary>
  [Transaction( TransactionMode.Automatic )]
  [Regeneration( RegenerationOption.Manual )]
  public class CmdCircle : IExternalCommand
  {
    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      //
      // The following sample code shows how to create a circle in Revit.
      // First create a temporary Arc object using Application.NewArc(),
      // then use it to create the model circle that can be seen in Revit
      // using Document.NewModelCurve():
      //
      UIApplication app = commandData.Application;
      Autodesk.Revit.Creation.Application creApp = app.Application.Create;
      Document doc = app.ActiveUIDocument.Document;
      XYZ xyzNorm = new XYZ( 0, 0, 1 );
      XYZ xyzOrigin = new XYZ( 0, 0, 0 );
      Plane basePlane = creApp.NewPlane( xyzNorm, xyzOrigin );
      double kPi = Math.Atan( 1 );
      Arc arc1 = creApp.NewArc( basePlane, 10, 0, 2 * kPi );
      SketchPlane plane = doc.Create.NewSketchPlane( basePlane );
      doc.Create.NewModelCurve( arc1, plane );
      return Result.Succeeded;
    }
  }
  #endregion // 3. Circle

  #region 4. Face
  /// <summary>
  /// 4. Face
  ///
  /// This command demonstrates accessing the wall geometry and face data.
  /// Before lauching this command, please select a wall.
  /// </summary>
  [Transaction( TransactionMode.Automatic )]
  [Regeneration( RegenerationOption.Manual )]
  public class CmdWallFace : IExternalCommand
  {
    Autodesk.Revit.Creation.Application _createApp;
    Autodesk.Revit.Creation.Document _createDoc;
    //SketchPlane _workPlane;

    static string PluralSuffix( int n )
    {
      return 1 == n ? "" : "s";
    }

    static string RealString( double a )
    {
      return a.ToString( "0.##" );
    }

    static string PointString( XYZ p )
    {
      return string.Format( "({0},{1},{2})", RealString( p.X ), RealString( p.Y ), RealString( p.Z ) );
    }

    /// <summary>
    /// Create a SketchPlane passing through the given line.
    /// </summary>
    private SketchPlane NewSketchPlanePassLine( Line aline )
    {
      XYZ norm;
      if( aline.get_EndPoint( 0 ).X == aline.get_EndPoint( 1 ).X )
      {
        norm = new XYZ( 1, 0, 0 );
      }
      else if( aline.get_EndPoint( 0 ).Y == aline.get_EndPoint( 1 ).Y )
      {
        norm = new XYZ( 0, 1, 0 );
      }
      else
      {
        norm = new XYZ( 0, 0, 1 );
      }
      XYZ point = aline.get_EndPoint( 0 );
      Plane plane = _createApp.NewPlane( norm, point );
      SketchPlane sketchPlane = _createDoc.NewSketchPlane( plane );
      return sketchPlane;
    }

    /// <summary>
    /// Create ModelLine instance.
    /// </summary>
    /// <param name="startPoint">Start point of the line</param>
    /// <param name="endPoint">End point of the line</param>
    public void CreateModelLine( XYZ startPoint, XYZ endPoint )
    {
      if( startPoint.Equals( endPoint ) )
      {
        throw new ArgumentException( "Start and end point should not be the same." );
      }
      Line geometryLine = _createApp.NewLine( startPoint, endPoint, true );
      if( null == geometryLine )
      {
        throw new Exception( "Creation of geometry line failed." );
      }
      SketchPlane workPlane = NewSketchPlanePassLine( geometryLine );
      ModelLine line = _createDoc.NewModelCurve( geometryLine, workPlane ) as ModelLine;
      if( null == line )
      {
        throw new Exception( "Creation of model line failed." );
      }
    }

    /// <summary>
    /// List face normals, also for non-planar faces.
    /// If two faces are given, print the normals at the first face's triangulation vertices projected onto both faces.
    /// The normal returned by ComputeDerivatives() is not guaranteed to point out of the solid.
    /// For a planar face, you can use the PlanarFace.Normal property.
    /// Please refer to TS88534 [Face planarity, points and normal].
    /// </summary>
    void ListFaceNormals( Face face, Face face2 )
    {
      //
      // get the normals for a non-planar face.
      // We have triangulated a non-planar face and have vertices of the face mesh.
      // Here are a few lines of code showing how the face normal at a
      // vertex point (XYZ point) on the face can be determined.
      // This code applies to all faces, regardless of whether they are planar or not.
      //
      Mesh mesh = face.Triangulate();
      IList<XYZ> vertices = mesh.Vertices;
      int i = 0, n = vertices.Count;
      IntersectionResult ir;
      Transform derivatives;
      string s = "{0} face triangulation mesh vertice{1} and normal vector{1} on the face itself and its opposite face:";
      Debug.WriteLine( string.Format( s, n, PluralSuffix( n ) ) );
      s = string.Empty;
      foreach( XYZ p in vertices )
      {
        ir = face.Project( p );
        derivatives = face.ComputeDerivatives( ir.UVPoint );
        XYZ normal = derivatives.BasisZ;
        if( null != face2 )
        {
          ir = face2.Project( p );
          derivatives = face2.ComputeDerivatives( ir.UVPoint );
          XYZ normal2 = derivatives.BasisZ;
          s = PointString( normal2 );
        }
        Debug.WriteLine( string.Format( "{0} {1} --> {2} {3}", i++, PointString( p ), PointString( normal ), s ) );
      }
    }

    /// <summary>
    /// List face triangle normals, also for non-planar faces.
    /// These normals are calculated from the vertices of the triangles
    /// returned by the face triangulation. The triangle vertices are
    /// ordered in counter-clockwise direction, so that a normal
    /// pointing out of the solid can be calculated.
    /// </summary>
    void ListFaceTriangleNormals( Face face )
    {
      Mesh mesh = face.Triangulate();
      int n = mesh.NumTriangles;
      string s = "{0} face triangulation mesh triangle{1} and normal vector{1} of face:";
      Debug.WriteLine( string.Format( s, n, PluralSuffix( n ) ) );
      for( int i = 0; i < n; ++i )
      {
        MeshTriangle t = mesh.get_Triangle( i );
        XYZ p = (t.get_Vertex( 0 ) + t.get_Vertex( 1 ) + t.get_Vertex( 2 )) / 3;
        XYZ v = t.get_Vertex( 1 ) - t.get_Vertex( 0 );
        XYZ w = t.get_Vertex( 2 ) - t.get_Vertex( 0 );
        XYZ normal = v.CrossProduct( w ).Normalize();
        Debug.WriteLine( string.Format( "{0} {1} --> {2}", i, PointString( p ), PointString( normal ) ) );
        CreateModelLine( p, p + normal );
      }
    }

    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      //
      // A face is defined by a surface bounded by edge loops.
      // A Face object can be obtained from a solid instance.
      // The following sample code shows some of the Face class functionality.
      // We cannot create a face object directly so far.
      // There is no available API for doing this.
      // We can however obtain a face object from an existing solid or
      // edge instance, then get the properties and use its methods.
      // In this example, we select a wall to obtain a face instance
      // from its solid geometry.
      //
      UIApplication app = commandData.Application;
      _createApp = app.Application.Create;
      UIDocument uidoc = app.ActiveUIDocument;
      Document doc = uidoc.Document;
      _createDoc = doc.Create;
      ElementSet ss = uidoc.Selection.Elements;
      //
      // Must be one single element:
      //
      if( 1 != ss.Size )
      {
        message = "Please select a single wall element.";
        return Result.Cancelled;
      }
      //
      // Must be a wall:
      //
      ElementSetIterator iter = ss.ForwardIterator();
      iter.MoveNext();
      Wall wall = iter.Current as Wall;
      if( null == wall )
      {
        message = "Selected element is not a wall; please select a single wall element.";
        return Result.Cancelled;
      }
      //
      // Obtain face instance from solid:
      //
      Solid solid = null;
      Options opt = app.Application.Create.NewGeometryOptions();
      opt.DetailLevel = DetailLevels.Coarse;
      GeometryElement geoElement = wall.get_Geometry( opt );
      foreach( GeometryObject geoObj in geoElement.Objects )
      {
        solid = geoObj as Solid;
        if( null != solid )
        {
          break;
        }
      }
      if( null == solid )
      {
        message = "There is no solid in the selected wall.";
        return Result.Failed;
      }
      //
      // Get first face:
      //
      bool testOpposingCurvedWallFaceNormals = false;
      Face face1 = null, face2 = null;
      foreach( Face face in solid.Faces )
      {
        //
        // additional testing code to explore normal vectors of non-planar faces;
        // to use, select an arc wall, which contains two cylindrical surfaces:
        //
        if( testOpposingCurvedWallFaceNormals )
        {
          if( !(face is PlanarFace) )
          {
            if( null == face1 )
            {
              face1 = face;
            }
            else
            {
              face2 = face;
              break;
            }
          }
        }
        else
        {
          face1 = face;
          break;
        }
      }
      //
      // Get the face area:
      //
      double dArea = face1.Area;
      EdgeArrayArray edges = face1.EdgeLoops;
      //
      // Test if the face is periodic in the U direction:
      //
      bool  bPeriodic = face1.get_IsCyclic( 0 );
      //
      // Calculate the intersection point of a face and a curve:
      //
      Curve curve1;
      XYZ ptStart = new XYZ( 0, 10, 10 );
      XYZ ptEnd = new XYZ( 1, 5, 10 );
      curve1 = app.Application.Create.NewLine( ptStart, ptEnd, true );
      IntersectionResultArray resultArray = null;
      face1.Intersect( curve1, out resultArray );
      int nPoints = resultArray.Size;
      //
      // Calculate the projected point of xyz1 on face1:
      //
      IntersectionResult intRes;
      XYZ xyz1 = new XYZ( 10, 10, 10 );
      UV uv1 = new UV( 10, 10 );
      intRes = face1.Project( xyz1 );
      //
      // Test whether a point is within a face:
      //
      bool bInside = face1.IsInside( uv1 );
      //
      // list face normals:
      //
      ListFaceNormals( face1, face2 );
      //
      // list face triangle normals:
      //
      ListFaceTriangleNormals( face1 );
      if( testOpposingCurvedWallFaceNormals )
      {
        ListFaceTriangleNormals( face2 );
      }
      return Result.Succeeded;
    }
  }
  #endregion // 4. Face

  #region 5. References and Dimensioning
  /// <summary>
  /// 5. References and Dimensioning - add dimensioning to architectural wall
  ///
  /// This command demonstrates dimensioning an architectural wall,
  /// i.e. a wall which is not necessarily structural. To do so, we need
  /// to ask the API to compute the references in the wall element geometry,
  /// which is done by setting Options.ComputeReferences = true before
  /// retrieving it. Before launching this command, please select a wall.
  /// </summary>
  [Transaction( TransactionMode.Automatic )]
  [Regeneration( RegenerationOption.Manual )]
  public class CmdAddDimensionForRacWall : IExternalCommand
  {
    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      UIApplication uiapp = commandData.Application;
      UIDocument uidoc = uiapp.ActiveUIDocument;
      Document doc = uidoc.Document;
      Wall wall = null;
      if( 1 != uidoc.Selection.Elements.Size )
      {
        message = "Please select one single straight wall.";
        return Result.Failed;
      }
      foreach( Element e in uidoc.Selection.Elements )
      {
        wall = e as Wall;
        if( null == wall )
        {
          message = "The selected element is not a wall. Please select a wall.";
          return Result.Failed;
        }
      }
      Options opt = uiapp.Application.Create.NewGeometryOptions();
      opt.ComputeReferences = true;
      opt.DetailLevel = DetailLevels.Fine;
      GeometryElement geoElement = wall.get_Geometry( opt );
      Solid solid = null;
      foreach( GeometryObject geoObj in geoElement.Objects )
      {
        solid = geoObj as Solid;
        if( null != solid )
        {
          break;
        }
      }
      if( null == solid )
      {
        message = "There is no solid in the selected wall.";
        return Result.Failed;
      }
      //
      // Get wall location:
      //
      LocationCurve wallLocation = wall.Location as LocationCurve;
      Line wallLine = wallLocation.Curve as Line;
      if( wallLine == null )
      {
        message = "Please select a straight wall";
        return Result.Failed;
      }
      //
      // Wall location line start and end point:
      //
      XYZ xyzStart = wallLine.get_EndPoint( 0 );
      XYZ xyzEnd = wallLine.get_EndPoint( 1 );
      //
      // Vector of the wall location:
      //
      XYZ xyzWallVector = xyzStart - xyzEnd;
      XYZ xyzWallVectorNormalized = xyzWallVector.Normalize();
      //
      // Z axis vector:
      //
      XYZ xyzZAxis = XYZ.BasisZ;
      //
      // Find the two reference faces:
      //
      ReferenceArray referenceArray = new ReferenceArray();
      if( null != solid )
      {
        FaceArrayIterator faceItor = solid.Faces.ForwardIterator();
        while( faceItor.MoveNext() )
        {
          PlanarFace faceOfWall = faceItor.Current as PlanarFace;
          if( null != faceOfWall )
          {
            XYZ faceNormalUnit = faceOfWall.Normal.Normalize();
            if( xyzWallVectorNormalized.IsAlmostEqualTo( faceNormalUnit ) )
            {
              referenceArray.Append( faceOfWall.Reference );
            }
            else
            {
              XYZ faceNormalUnitNegate = faceNormalUnit.Negate();
              if( xyzWallVectorNormalized.IsAlmostEqualTo( faceNormalUnitNegate ) )
              {
                referenceArray.Append( faceOfWall.Reference );
              }
            }
          }
        }
      }
      //
      // Create dimensioning:
      //
      xyzStart += 10 * XYZ.BasisY;
      xyzEnd += 10 * XYZ.BasisY;
      View view = doc.ActiveView;
      Line lineDimension = uiapp.Application.Create.NewLine( xyzStart, xyzEnd, true );
      Dimension newDimension = doc.Create.NewDimension( view, lineDimension, referenceArray );
      return Result.Succeeded;
    }
  }

  public class CmdIsDimensionLocked : IExternalCommand
  {
    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      UIApplication uiapp = commandData.Application;
      Document doc = uiapp.ActiveUIDocument.Document;

      FilteredElementCollector collector = new FilteredElementCollector( doc );
      collector.OfCategory( BuiltInCategory.OST_Dimensions );
      collector.OfClass( typeof( Dimension ) );

      Dimension d = collector.FirstElement() as Dimension;

      Debug.Assert( null != d, 
        "expected to find a dimension element" );

      collector = new FilteredElementCollector( doc );
      collector.OfCategory( BuiltInCategory.OST_Constraints );
      collector.OfClass( typeof( Dimension ) );

      Dimension c = collector.FirstElement() as Dimension;

      Debug.Assert( null != c, 
        "expected to find a constraint element" );

      // both locations have no valid information:
      Location locc = c.Location; 
      Location locd = d.Location;

      // both lines have no valid information:
      Line linc = c.Curve as Line; 
      Line lind = d.Curve as Line;

      // this throws an exception:
      //XYZ pc = linc.get_EndPoint( 0 ); 
      //XYZ qc = linc.get_EndPoint( 1 );
      //XYZ pd = lind.get_EndPoint( 0 );
      //XYZ qd = lind.get_EndPoint( 1 );

      // this cast returns null:
      LocationCurve locc2 = c.Location as LocationCurve;
      LocationCurve locd2 = d.Location as LocationCurve;

      ReferenceArray rc = c.References;
      ReferenceArray rd = d.References;

      if( rc.Size == rd.Size )
      {
        ReferenceArrayIterator ic = rc.ForwardIterator();
        ReferenceArrayIterator id = rd.ForwardIterator();
        while( ic.MoveNext() && id.MoveNext() )
        {
          Reference r1 = ic.Current as Reference;
          Reference r2 = id.Current as Reference;
          if( r1.Equals( r2 ) )
          {
            // this never happens:
            Debug.Print( "Equal" );
          }
        }
      }
      return Result.Failed;
    }
  }
  #endregion // 5. References and Dimensioning
}
