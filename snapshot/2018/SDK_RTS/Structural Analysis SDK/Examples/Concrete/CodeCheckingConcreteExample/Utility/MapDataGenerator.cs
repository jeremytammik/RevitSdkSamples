//
// (C) Copyright 2003-2013 by Autodesk, Inc.
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
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE. AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Revit.DB;

/// <structural_toolkit_2015>
namespace CodeCheckingConcreteExample.Utility
{
   /// <summary>
   /// Simple utility class for creation maps in the report.
   /// </summary>
   class MapDataGenerator
   {
      /// <summary>
      /// Transformation flag
      /// </summary>
      bool isTransformed = false;
      /// <summary>
      ///  List of values
      /// </summary>
      private List<double> _Values = new List<double>();
      /// <summary>
      /// List of oryginal 3D points
      /// </summary>
      private List<XYZ> _Points = new List<XYZ>();
      /// <summary>
      /// List of indexes of points on the contour (map edge)
      /// </summary>
      private List<int> _PointsOnContour = new List<int>();
      /// <summary>
      /// List of indexes of points on the edge of holes
      /// </summary>
      private List<List<int>> _PointsOnHoles = new List<List<int>>();
      /// <summary>
      /// List of transformed 3D points
      /// </summary>
      private List<XYZ> _vPointsTransformed = new List<XYZ>();
      /// <summary>
      /// List of transformed 2D points
      /// </summary>
      private List<UV> _vPoints2d = new List<UV>();
      
      /// <summary>
      /// Returns result values for all points
      /// </summary>
      public List<double> Values          { get { return _Values; } }
      
      /// <summary>
      /// Returns number of holes
      /// </summary>
      public int HolesCount               { get { return _PointsOnHoles.Count; } }
      
      /// <summary>
      /// returns number of points
      /// </summary>
      public int Count                    { get { return _Values.Count; } }
      
      /// <summary>
      /// Returns indexes of points for all holes
      /// </summary>
      public List<List<int>>Holes         { get { return _PointsOnHoles; } }
      /// <summary>
      /// Returns indexes of points for external contour (edge of map)
      /// </summary>
      public List<int> Contour            { get { return _PointsOnContour; } }

      /// <summary>
      /// Creates default MapDataGenerator
      /// </summary>
      public MapDataGenerator() {}

      /// <summary>
      /// Clear all internal data
      /// </summary>
      public void Clear()
      {
         _Points.Clear();
         _Values.Clear();
         _PointsOnContour.Clear();
         _PointsOnHoles.Clear();
         isTransformed = false;
      }
      /// <summary>
      /// Adds new point with value to the colection.
      /// </summary>
      /// <param name="point">Position of point</param>
      /// <param name="value">Result values in the point</param>
      public void AddPoint (XYZ point, double value)
      {
         _Points.Add(point);
         _Values.Add(value);
         isTransformed = false;
      }
      /// <summary>
      /// Adds new points to the colection of external contour (map edge).
      /// </summary>
      /// <param name="points">Position of points</param>
      public void AddContour(List<XYZ> points)
      {
         foreach (XYZ point in points)
         {
            AddPointToContour(point);
         }
      }
      /// <summary>
      /// Adds new point to the colection of external contour (map edge).
      /// </summary>
      /// <param name="point">Position of point</param>
      /// <remarks>If point exist in the points collection is taken into accout as edge of hole. If not new point is adds and set zero value for it.</remarks>
      /// <remarks>Sets the transformation flag on "false" if necessary</remarks>
      public void AddPointToContour(XYZ point)
      {
         int indexPoints = _PointsOnContour.FindIndex(o => _Points[o].IsAlmostEqualTo(point, 1e-6));
         if (indexPoints < 0)
         {
            indexPoints = _Points.FindIndex(o => o.IsAlmostEqualTo(point, 1e-6));
            if (indexPoints < 0)
            {
               _PointsOnContour.Add(_Points.Count());
               _Points.Add(point);
               _Values.Add(0.0);
               isTransformed = false;
            }
            else
               _PointsOnContour.Add(indexPoints);
         }
      }
      /// <summary>
      /// Creates new hole and adds new points to this hole
      /// </summary>
      /// <param name="points">Position of points</param>
      public void AddHole(List<XYZ> points)
      {
         int nextHole = _PointsOnHoles.Count;
         foreach (XYZ point in points)
         {
            AddPointToHole(point,nextHole);
         }
      }
      /// <summary>
      /// Creates new or modifies existing hole and adds new point to this hole
      /// </summary>
      /// <param name="point">Position of point</param>
      /// <param name="holeItem">Hole number</param>
      /// <remarks>If point exist in the points collection is taken into accout as edge of hole. If not new point is adds and set zero value for it.</remarks>
      /// <remarks>Sets the transformation flag on "false" if necessary</remarks>
      public void AddPointToHole(XYZ point, int holeItem)
      {
         if (holeItem > _PointsOnHoles.Count())
            throw new ArgumentException("Item out of range", "holeItem");
         List<int> hole = (holeItem == _PointsOnHoles.Count()) ? new List<int>() : _PointsOnHoles[holeItem];
         int indexPoints = hole.FindIndex(o => _Points[o].IsAlmostEqualTo(point, 1e-6));
         if (indexPoints < 0)
         {
            indexPoints = _Points.FindIndex(o => o.IsAlmostEqualTo(point, 1e-6));
            if (indexPoints < 0)
            {
               hole.Add(_Points.Count());
               _Points.Add(point);
               _Values.Add(0.0);
               isTransformed = false;
            }
            else
               hole.Add(indexPoints);
         }
         if (holeItem == _PointsOnHoles.Count())
            _PointsOnHoles.Add(hole);
      }
      /// <summary>
      /// Returns 2D position for point on the map
      /// </summary>
      /// <param name="item">Points number</param>
      public UV GetPoint(int item)
      {
         if (!isTransformed)
            transform();
         return _vPoints2d[item];
      }
      /// <summary>
      /// Returns value for point on the map
      /// </summary>
      /// <param name="item">Points number</param>
      public double GetValue(int item)
      {
         if (!isTransformed)
            transform();
         return _Values[item];
      }
      /// <summary>
      /// Transform if necessary, and returns transformed points.
      /// </summary>
      /// <returns>Transformed points to maps LCS</returns>
      public List<XYZ> PointsTransformed() 
      {
         if (!isTransformed)
            transform();
         return _vPointsTransformed; 
      }
      /// <summary>
      /// Transform if necessary, and returns 2D transformed points.
      /// </summary>
      /// <returns>Transformed points to maps LCS in 2D</returns>
      public List<UV> Points2d()
      { 
         if (!isTransformed)
            transform();
         return _vPoints2d; 
      }
      /// <summary>
      /// Transform all points.
      /// </summary>
      /// <remarks>Sets the transformation flag on "true"</remarks>
      private void transform()
      {
         transform2Plane();
         transform22D();
         isTransformed = true;
      }
      /// <summary>
      /// Transform all 3D points to the plane.
      /// </summary>
      private void transform2Plane()
      {
         if (_Points.Count > 2)
         {
            if ((_Points.Max(s => s.Z) - _Points.Min(s => s.Z)) < 1.0e-6)
            {
               _vPointsTransformed = new List<XYZ>(_Points);
            }
            else
            {
               XYZ v1 = _Points[1] - _Points[0],
                   v2 = _Points[2] - _Points[1],
                   n = v1.CrossProduct(v2).Normalize(),
                   p0 = _Points[0];

               for (int i = 0; i < 2; i++)
               {
                  bool bContour = i == 0;
                  List<XYZ> vSrc = _Points,
                            vTar = _vPointsTransformed;

                  foreach (XYZ pt in vSrc)
                  {
                     XYZ u = pt - p0,
                         u1 = u - n.Multiply(u.DotProduct(n));
                     vTar.Add(p0 + u1);
                  }
               }
            }
         }
      }
      /// <summary>
      /// Transform all 3D points to the 2D points.
      /// </summary>
      private void transform22D()
      {
         if (_Points.Count > 2)
         {
            var zdist = _Points.Select(s => s.Z).Distinct();
            if ((_Points.Max(s => s.Z) - _Points.Min(s => s.Z)) < 1.0e-6 )
            {
               _vPoints2d = _Points.Select(s => new UV(s.X, s.Y)).ToList();
               UV move = new UV(_vPoints2d.Min(s => s.U), _vPoints2d.Min(s => s.V));
               _vPoints2d = _vPoints2d.Select( s=>( new UV( s.U-move.U, s.V-move.V))).ToList();
            }
            else
            {
               int indLast = _Points.Count - 1;
               XYZ vX = null;
               for (int i = 0; i <= indLast; i++)
               {
                  vX = (_Points[i] - _Points[0]).Normalize();
                  if (!vX.IsZeroLength())
                  {
                     break;
                  }
               }

               XYZ vY = null;
               for (int i = indLast; i >= 0; i--)
               {

                  XYZ v2 = _Points[0] - _Points[i],
                      vZ = vX.CrossProduct(v2).Normalize();
                  if (!vZ.IsZeroLength())
                  {
                     vY = vZ.CrossProduct(vX);
                     break;
                  }

               }

               {
                  XYZ p0 = _Points[0];
                  List<XYZ> vSrc = _vPointsTransformed;
                  List<UV> vTar = _vPoints2d;

                  foreach (XYZ pt in vSrc)
                  {
                     XYZ u = pt - p0;
                     vTar.Add( new UV(u.DotProduct(vX), u.DotProduct(vY) ) );
                  }
               }
            }
         }

      }
   }
}
/// </structural_toolkit_2015>

