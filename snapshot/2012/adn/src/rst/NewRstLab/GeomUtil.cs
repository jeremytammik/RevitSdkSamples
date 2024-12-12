#region Header
// Revit Structure API Labs
//
// Copyright (C) 2010 by Autodesk, Inc.
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
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
#endregion // Namespaces

namespace NewRstLab
{
  /// <summary>
  /// Compute geometry information and store geometry information.
  /// </summary>
  public class GeometrySupport
  {
    /// <summary>
    /// precision when judge whether two doubles are equal
    /// </summary>
    const double Precision = 0.00001;

    /// <summary>
    /// store the solid of beam or column
    /// </summary>
    protected Solid m_solid;

    /// <summary>
    /// the extend or sweep path of the beam or column
    /// </summary>
    protected Line m_drivingLine;

    /// <summary>
    /// the director vector of beam or column
    /// </summary>
    protected XYZ m_drivingVector;

    /// <summary>
    /// a list to store the edges 
    /// </summary>
    protected List<Line> m_edges = new List<Line>();

    /// <summary>
    /// a list to store the point
    /// </summary>
    private List<XYZ> m_points = new List<XYZ>();

    /// <summary>
    /// Return profile points
    /// </summary>
    public List<XYZ> ProfilePoints
    {
      get { return m_points; }
      set { m_points = value; }
    }

    /// <summary>
    /// the transform value of the solid
    /// </summary>
    protected Transform m_transform;

    /// <summary>
    /// Driving length field
    /// </summary>
    private double m_drivingLength;

    /// <summary>
    /// Return driving length
    /// </summary>
    public double DrivingLength
    {
      get
      {
        return m_drivingLength;
      }
    }

    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="element">The host object, must be family instance</param>
    public GeometrySupport( FamilyInstance element )
    {
      // get the geometry element of the selected element
      GeometryElement geoElement = element.get_Geometry( new Options() );
      if( null == geoElement || 0 == geoElement.Objects.Size )
      {
        throw new Exception( "Can't get the geometry of selected element." );
      }

      // get the driving path and vector of the beam or column

      AnalyticalModel analyticalLine = element.GetAnalyticalModel();
      Line line = analyticalLine.GetCurve() as Line;      
      if (null != line)
      {
          m_drivingLine = line;   // driving path
          m_drivingVector = line.get_EndPoint(1) - line.get_EndPoint(0);
          m_drivingLength = m_drivingVector.GetLength();          
      }



      //get the geometry object
      foreach( GeometryObject geoObject in geoElement.Objects )
      {
                //get the geometry instance which contains the geometry information
        GeometryInstance instance = geoObject as GeometryInstance;
        if( null != instance )
        {
          foreach( GeometryObject o in instance.SymbolGeometry.Objects )
          {
            // get the solid of beam or column
            Solid solid = o as Solid;

            // do some checks.
            if( null == solid )
            {
              continue;
            }
            if( 0 == solid.Faces.Size || 0 == solid.Edges.Size )
            {
              continue;
            }

            m_solid = solid;
            //get the transform value of instance
            m_transform = instance.Transform;

            // Get the swept profile curves information
            if( !GetSweptProfile( solid ) )
            {
              throw new Exception( "Can't get the swept profile curves." );
            }
            break;
          }
        }

      }

      // do some checks about profile curves information
      if( null == m_edges )
      {
        throw new Exception( "Can't get the geometry edge information." );
      }
      if( 4 != m_points.Count )
      {
        throw new Exception( "The sample only works for vertical rectangle column." );
      }
    }


    /// <summary>
    /// Transform the point to new coordinates
    /// </summary>
    /// <param name="point">The point need to transform</param>
    /// <returns>The changed point</returns>
    protected XYZ Transform( XYZ point )
    {
      // only invoke the TransformPoint() method.
      return TransformPoint( point, m_transform );
    }

    public static XYZ TransformPoint( XYZ point, Transform transform )
    {
      //get the coordinate value in X, Y, Z axis
      double x = point.X;
      double y = point.Y;
      double z = point.Z;

      //transform basis of the old coordinate system in the new coordinate system
      XYZ b0 = transform.get_Basis( 0 );
      XYZ b1 = transform.get_Basis( 1 );
      XYZ b2 = transform.get_Basis( 2 );
      XYZ origin = transform.Origin;

      //transform the origin of the old coordinate system in the new coordinate system
      double xTemp = x * b0.X + y * b1.X + z * b2.X + origin.X;
      double yTemp = x * b0.Y + y * b1.Y + z * b2.Y + origin.Y;
      double zTemp = x * b0.Z + y * b1.Z + z * b2.Z + origin.Z;

      return new XYZ( xTemp, yTemp, zTemp );
    }

    /// <summary>
    /// Judge whether the two vectors have the same direction
    /// </summary>
    /// <param name="firstVec">The first vector</param>
    /// <param name="secondVec">The second vector</param>
    /// <returns>True if the two vector is in same direction, otherwise false</returns>
    public static bool IsSameDirection( XYZ firstVec, XYZ secondVec )
    {
      // get the unit vector for two vectors
      XYZ first = UnitVector( firstVec );
      XYZ second = UnitVector( secondVec );

      // if the dot product of two unit vectors is equal to 1, return true
      double dot = DotMatrix( first, second );
      return ( IsEqual( dot, 1 ) );
    }

    /// <summary>
    /// Judge whether the two vectors have the opposite direction
    /// </summary>
    /// <param name="firstVec">The first vector</param>
    /// <param name="secondVec">The second vector</param>
    /// <returns>True if the two vector is in opposite direction, otherwise false</returns>
    public static bool IsOppositeDirection( XYZ firstVec, XYZ secondVec )
    {
      // get the unit vector for two vectors
      XYZ first = UnitVector( firstVec );
      XYZ second = UnitVector( secondVec );

      // if the dot product of two unit vectors is equal to -1, return true
      double dot = DotMatrix( first, second );
      return ( IsEqual( dot, -1 ) );
    }

    /// <summary>
    /// Offset the points of the swept profile to make the points inside swept profile
    /// </summary>
    /// <param name="offset">Indicate how long to offset on two directions</param>
    /// <returns>The offset points</returns>
    public List<XYZ> OffsetPoints( double offset )
    {
      // Initialize the offset point list.
      List<XYZ> points = new List<XYZ>();

      // Get all points of the swept profile, and offset it in two related directions
      foreach( XYZ point in m_points )
      {
        // Get two related directions
        List<XYZ> directions = GetRelatedVectors( point );
        XYZ firstDir = directions[0];
        XYZ secondDir = directions[1];

        // offset the point in two directions
        XYZ movedPoint = OffsetPoint( point, firstDir, offset );
        movedPoint = OffsetPoint( movedPoint, secondDir, offset );

        // add the offset point into the array
        points.Add( movedPoint );
      }

      return points;
    }

    /// <summary>
    /// Move a point a give offset along a given direction
    /// </summary>
    /// <param name="point">The point need to move</param>
    /// <param name="direction">The direction the point move to</param>
    /// <param name="offset">Tndicate how long to move</param>
    /// <returns>The moved point</returns>
    public static XYZ OffsetPoint( XYZ point, XYZ direction, double offset )
    {
      XYZ directUnit = UnitVector( direction );
      XYZ offsetVect = MultiplyVector( directUnit, offset );
      return AddXYZ( point, offsetVect );
    }

    /// <summary>
    /// Multiply a vector with a number
    /// </summary>
    /// <param name="vector">A vector</param>
    /// <param name="rate">The rate number</param>
    /// <returns></returns>
    public static XYZ MultiplyVector( XYZ vector, double rate )
    {
      double x = vector.X * rate;
      double y = vector.Y * rate;
      double z = vector.Z * rate;

      return new XYZ( x, y, z );
    }

    /// <summary>
    /// Add of two points(or vectors), get a new point(vector) 
    /// </summary>
    /// <param name="p1">The first point(vector)</param>
    /// <param name="p2">The first point(vector)</param>
    /// <returns>A new vector(point)</returns>
    public static XYZ AddXYZ( XYZ p1, XYZ p2 )
    {
      double x = p1.X + p2.X;
      double y = p1.Y + p2.Y;
      double z = p1.Z + p2.Z;

      return new XYZ( x, y, z );
    }

    /// <summary>
    /// Get two vectors, which indicate some edge direction which contain given point, 
    /// set the given point as the start point, the other end point of the edge as end
    /// </summary>
    /// <param name="point">A point of the swept profile</param>
    /// <returns>Two vectors indicate edge direction</returns>
    protected List<XYZ> GetRelatedVectors( XYZ point )
    {
      // Initialize the return vector list.
      List<XYZ> vectors = new List<XYZ>();

      // Get all the edges which contain this point.
      // And get the vector from this point to another point
      foreach( Line line in m_edges )
      {
        if( IsEqual( point, line.get_EndPoint( 0 ) ) )
        {
          XYZ vector = SubXYZ( line.get_EndPoint( 1 ), line.get_EndPoint( 0 ) );
          vectors.Add( vector );
        }
        if( IsEqual( point, line.get_EndPoint( 1 ) ) )
        {
          XYZ vector = SubXYZ( line.get_EndPoint( 0 ), line.get_EndPoint( 1 ) );
          vectors.Add( vector );
        }
      }

      // only two vectors(directions) should be found
      if( 2 != vectors.Count )
      {
        throw new Exception( "A point on swept profile should have only two directions." );
      }

      return vectors;
    }

    /// <summary>
    /// Set the vector into unit length
    /// </summary>
    /// <param name="vector">The input vector</param>
    /// <returns>The vector in unit length</returns>
    public static XYZ UnitVector( XYZ vector )
    {
      // calculate the distance from grid origin to the XYZ
      double length = GetLength( vector );

      // changed the vector into the unit length
      double x = vector.X / length;
      double y = vector.Y / length;
      double z = vector.Z / length;
      return new XYZ( x, y, z );
    }

    /// <summary>
    /// Calculate the distance from grid origin to the XYZ(vector length)
    /// </summary>
    /// <param name="vector">The input vector</param>
    /// <returns>The length of the vector</returns>
    public static double GetLength( XYZ vector )
    {
      double x = vector.X;
      double y = vector.Y;
      double z = vector.Z;
      return Math.Sqrt( x * x + y * y + z * z );
    }

    /// <summary>
    /// Judge whether the two double data are equal
    /// </summary>
    /// <param name="d1">The first double data</param>
    /// <param name="d2">The second double data</param>
    /// <returns>true if two double data is equal, otherwise false</returns>
    public static bool IsEqual( double d1, double d2 )
    {
      //get the absolute value;
      double diff = Math.Abs( d1 - d2 );
      return diff < Precision;
    }

    /// <summary>
    /// Judge whether the two XYZ point are equal
    /// </summary>
    /// <param name="first">The first XYZ point</param>
    /// <param name="second">The second XYZ point</param>
    /// <returns>true if two XYZ point is equal, otherwise false</returns>
    public static bool IsEqual( XYZ first, XYZ second )
    {
      bool flag = true;
      flag = flag && IsEqual( first.X, second.X );
      flag = flag && IsEqual( first.Y, second.Y );
      flag = flag && IsEqual( first.Z, second.Z );
      return flag;
    }

    /// <summary>
    /// Dot product of two XYZ as Matrix
    /// </summary>
    /// <param name="p1">The first XYZ</param>
    /// <param name="p2">The second XYZ</param>
    /// <returns>The cosine value of the angle between vector p1 an p2</returns>
    private static double DotMatrix( XYZ p1, XYZ p2 )
    {
      //get the coordinate of the XYZ 
      double v1 = p1.X;
      double v2 = p1.Y;
      double v3 = p1.Z;

      double u1 = p2.X;
      double u2 = p2.Y;
      double u3 = p2.Z;

      return v1 * u1 + v2 * u2 + v3 * u3;
    }

    /// <summary>
    /// Subtraction of two points(or vectors), get a new vector 
    /// </summary>
    /// <param name="p1">The first point(vector)</param>
    /// <param name="p2">The second point(vector)</param>
    /// <returns>Return a new vector from point p2 to p1</returns>
    public static XYZ SubXYZ( XYZ p1, XYZ p2 )
    {
      double x = p1.X - p2.X;
      double y = p1.Y - p2.Y;
      double z = p1.Z - p2.Z;

      return new XYZ( x, y, z );
    }

    /// <summary>
    /// Judge whether the line is perpendicular to the face
    /// </summary>
    /// <param name="face">The face reference</param>
    /// <param name="line">The line reference</param>
    /// <param name="faceTrans">The transform for the face</param>
    /// <param name="lineTrans">The transform for the line</param>
    /// <returns>True if line is perpendicular to the face, otherwise false</returns>
    public static bool IsVertical( Face face, Line line,
                        Transform faceTrans, Transform lineTrans )
    {
      //get points which the face contains
      List<XYZ> points = face.Triangulate().Vertices as List<XYZ>;
      if( 3 > points.Count )  // face's point number should be above 2
      {
        return false;
      }

      // get three points from the face points
      XYZ first = points[0];
      XYZ second = points[1];
      XYZ third = points[2];

      // get start and end point of line
      XYZ lineStart = line.get_EndPoint( 0 );
      XYZ lineEnd = line.get_EndPoint( 1 );

      // transForm the three points if necessary
      if( null != faceTrans )
      {
        first = TransformPoint( first, faceTrans );
        second = TransformPoint( second, faceTrans );
        third = TransformPoint( third, faceTrans );
      }

      // transform the start and end points if necessary
      if( null != lineTrans )
      {
        lineStart = TransformPoint( lineStart, lineTrans );
        lineEnd = TransformPoint( lineEnd, lineTrans );
      }

      // form two vectors from the face and a vector stand for the line
      // Use SubXYZ() method to get the vectors
      XYZ vector1 = SubXYZ( first, second );  // first vector of face
      XYZ vector2 = SubXYZ( first, third );   // second vector of face
      XYZ vector3 = SubXYZ( lineStart, lineEnd );   // line vector

      // get two dot products of the face vectors and line vector
      double result1 = DotMatrix( vector1, vector3 );
      double result2 = DotMatrix( vector2, vector3 );

      // if two dot products are all zero, the line is perpendicular to the face
      return ( IsEqual( result1, 0 ) && IsEqual( result2, 0 ) );
    }


    /// <summary>
    /// Find the information of the swept profile(face), 
    /// and store the points and edges of the profile(face) 
    /// </summary>
    /// <param name="solid">The solid reference</param>
    /// <returns>True if the swept profile can be gotten, otherwise false</returns>
    private bool GetSweptProfile( Solid solid )
    {
      // get the swept face
      Face sweptFace = GetSweptProfileFace( solid );
      // do some checks
      if( null == sweptFace || 1 != sweptFace.EdgeLoops.Size )
      {
        return false;
      }

      // get the points of the swept face
      foreach( XYZ point in sweptFace.Triangulate().Vertices )
      {
        m_points.Add( Transform( point ) );
      }

      // get the edges of the swept face
      m_edges = ChangeEdgeToLine( sweptFace.EdgeLoops.get_Item( 0 ) );

      // do some checks
      return ( null != m_edges );
    }

    /// <summary>
    /// Get the swept profile(face) of the host object(family instance)
    /// </summary>
    /// <param name="solid">The solid reference</param>
    /// <returns>The swept profile</returns>
    private Face GetSweptProfileFace( Solid solid )
    {
      // Get a point on the swept profile from all points in solid
      XYZ refPoint = new XYZ();   // the point on swept profile
      foreach( Edge edge in solid.Edges )
      {
        List<XYZ> points = edge.Tessellate() as List<XYZ>;  //get end points of the edge
        if( 2 != points.Count )           // make sure all edges are lines
        {
          throw new Exception( "Each edge should be a line." );
        }

        // get two points of the edge. All points in solid should be transformed first
        XYZ first = Transform( points[0] );  // start point of edge
        XYZ second = Transform( points[1] ); // end point of edge

        // some edges should be paralleled with the driving line,
        // and the start point of that edge should be the wanted point
        XYZ edgeVector = second - first;
        if( IsSameDirection( edgeVector, m_drivingVector ) )
        {
          refPoint = first;
          break;
        }
        if( IsOppositeDirection( edgeVector, m_drivingVector ) )
        {
          refPoint = second;
          break;
        }
      }

      // Find swept profile(face)
      Face sweptFace = null;  // define the swept face
      foreach( Face face in solid.Faces )
      {
        if( null != sweptFace )
        {
          break;
        }
        // the swept face should be perpendicular with the driving line
        if( !IsVertical( face, m_drivingLine, m_transform, null ) )
        {
          continue;
        }
        // use the point to get the swept face
        foreach( XYZ point in face.Triangulate().Vertices )
        {
          XYZ pnt = Transform( point ); // all points in solid should be transformed
          if( IsEqual( refPoint, pnt ) )
          {
            sweptFace = face;
            break;
          }
        }
      }

      return sweptFace;
    }

    /// <summary>
    /// Change the swept profile edges from EdgeArray type to line list
    /// </summary>
    /// <param name="edges">The swept profile edges</param>
    /// <returns>The line list which stores the swept profile edges</returns>
    private List<Line> ChangeEdgeToLine( EdgeArray edges )
    {
      // create the line list instance.
      List<Line> edgeLines = new List<Line>();

      // get each edge from swept profile,
      // and change the geometry information in line list
      foreach( Edge edge in edges )
      {
        //get the two points of each edge
        List<XYZ> points = edge.Tessellate() as List<XYZ>;
        XYZ first = Transform( points[0] );
        XYZ second = Transform( points[1] );

        // create new line and add them into line list
        edgeLines.Add( Line.get_Bound( first, second ) );
      }

      return edgeLines;
    }
  }
}
