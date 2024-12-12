//
// (C) Copyright 2003-2019 by Autodesk, Inc.
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
using System.Collections;
using System.Collections.Generic;
using System.Text;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;

namespace Revit.SDK.Samples.Reinforcement.CS
{
   /// <summary>
   /// The geometry support for reinforcement creation on conlumn.
   /// It can prepare the geometry information for transverse and vertical rebar creation
   /// </summary>
   class ColumnGeometrySupport : GeometrySupport
   {
      // Private members
      double m_columnLength; //the length of the column
      double m_columnWidth;  //the width of the column
      double m_columnHeight; //the height of the column

      /// <summary>
      /// constructor for the ColumnGeometrySupport
      /// </summary>
      /// <param name="element">the column which the rebars are placed on</param>
      /// <param name="geoOptions">the geometry option</param>
      public ColumnGeometrySupport(FamilyInstance element, Options geoOptions)
         : base(element, geoOptions)
      {
         // assert the host element is a column
         if (!element.StructuralType.Equals(StructuralType.Column))
         {
            throw new Exception("ColumnGeometrySupport can only work for column instance.");
         }

         // Get the length, width and height of the column.
         m_columnHeight = GetDrivingLineLength();
         m_columnLength = GetColumnLength();
         m_columnWidth = GetColumnWidth();
      }

      /// <summary>
      /// Get the geometry information of the transverse rebar
      /// </summary>
      /// <param name="location">the location of transverse rebar</param>
      /// <param name="spacing">the spacing value of the rebar</param>
      /// <returns>the gotted geometry information</returns>
      public RebarGeometry GetTransverseRebar(TransverseRebarLocation location, double spacing)
      {
         // sort the points of the swept profile
         XYZHeightComparer comparer = new XYZHeightComparer();
         m_points.Sort(comparer);

         // the offset from the column surface to the rebar
         double offset = ColumnRebarData.TransverseOffset;
         //the length of the transverse rebar
         double rebarLength = 0;

         // get the origin and normal parameter for rebar creation
         Autodesk.Revit.DB.XYZ normal = m_drivingVector;
         double curveOffset = 0;

         //set rebar length and origin according to the location of rebar
         switch (location)
         {
            case TransverseRebarLocation.Start:     // start transverse rebar
               rebarLength = m_columnHeight / 4;
               break;
            case TransverseRebarLocation.Center:    // center transverse rebar
               rebarLength = m_columnHeight / 2;
               curveOffset = m_columnHeight / 4 + (rebarLength % spacing) / 2;
               break;
            case TransverseRebarLocation.End:       // end transverse rebar
               rebarLength = m_columnHeight / 4;
               curveOffset = m_columnHeight - rebarLength + (rebarLength % spacing);
               break;
            default:
               throw new Exception("The program should never go here.");
         }

         // the number of the transverse rebar
         int rebarNumber = (int)(rebarLength / spacing) + 1;
         // get the profile of the transverse rebar
         List<Autodesk.Revit.DB.XYZ> movedPoints = OffsetPoints(offset);

         List<Autodesk.Revit.DB.XYZ> translatedPoints = new List<Autodesk.Revit.DB.XYZ>();
         foreach (Autodesk.Revit.DB.XYZ point in movedPoints)
         {
            translatedPoints.Add(GeomUtil.OffsetPoint(point, m_drivingVector, curveOffset));
         }

         IList<Curve> curves = new List<Curve>(); //the profile of the transverse rebar
         Autodesk.Revit.DB.XYZ first = translatedPoints[0];
         Autodesk.Revit.DB.XYZ second = translatedPoints[1];
         Autodesk.Revit.DB.XYZ third = translatedPoints[2];
         Autodesk.Revit.DB.XYZ fourth = translatedPoints[3];
         curves.Add(Line.CreateBound(first, second));
         curves.Add(Line.CreateBound(second, fourth));
         curves.Add(Line.CreateBound(fourth, third));
         curves.Add(Line.CreateBound(third, first));

         // return the rebar geometry information
         return new RebarGeometry(normal, curves, rebarNumber, spacing);
      }


      /// <summary>
      /// Get the geometry information of vertical rebar
      /// </summary>
      /// <param name="location">the location of vertical rebar</param>
      /// <param name="rebarNumber">the spacing value of the rebar</param>
      /// <returns>the gotted geometry information</returns>
      public RebarGeometry GetVerticalRebar(VerticalRebarLocation location, int rebarNumber)
      {
         // sort the points of the swept profile
         XYZHeightComparer comparer = new XYZHeightComparer();
         m_points.Sort(comparer);

         // Get the offset and rebar length of rebar
         double offset = ColumnRebarData.VerticalOffset;
         double rebarLength = m_columnHeight + 3; //the length of rebar

         // Get the start point of the vertical rebar curve
         Autodesk.Revit.DB.XYZ startPoint = m_drivingLine.GetEndPoint(0);

         List<Autodesk.Revit.DB.XYZ> movedPoints = OffsetPoints(offset);
         movedPoints.Sort(comparer);

         Autodesk.Revit.DB.XYZ normal = new Autodesk.Revit.DB.XYZ(); // the normal parameter
         double rebarOffset = 0; // rebar offset, equal to rebarNumber* spacing 
         // get the normal, start point and rebar offset of vertical rebar
         switch (location)
         {
            case VerticalRebarLocation.East:   //vertical rebar in east 
               normal = new Autodesk.Revit.DB.XYZ(0, 1, 0);
               rebarOffset = m_columnWidth - 2 * offset;
               startPoint = movedPoints[1];
               break;
            case VerticalRebarLocation.North: //vertical rebar in north
               normal = new Autodesk.Revit.DB.XYZ(-1, 0, 0);
               rebarOffset = m_columnLength - 2 * offset;
               startPoint = movedPoints[3];
               break;
            case VerticalRebarLocation.West: //vertical rebar in west
               normal = new Autodesk.Revit.DB.XYZ(0, -1, 0);
               rebarOffset = m_columnWidth - 2 * offset;
               startPoint = movedPoints[2];
               break;
            case VerticalRebarLocation.South: //vertical rebar in south
               normal = new Autodesk.Revit.DB.XYZ(1, 0, 0);
               rebarOffset = m_columnLength - 2 * offset;
               startPoint = movedPoints[0];
               break;
            default:
               break;
         }

         double spacing = rebarOffset / rebarNumber; //spacing value of the rebar
         Autodesk.Revit.DB.XYZ endPoint = GeomUtil.OffsetPoint(startPoint, m_drivingVector, rebarLength);

         IList<Curve> curves = new List<Curve>();       //profile of the rebar
         curves.Add(Line.CreateBound(startPoint, endPoint));

         // return the rebar geometry information
         return new RebarGeometry(normal, curves, rebarNumber, spacing);

      }

      /// <summary>
      /// Get the length of the column
      /// </summary>
      /// <returns>the length data</returns>
      private double GetColumnLength()
      {
         XYZHeightComparer comparer = new XYZHeightComparer();
         m_points.Sort(comparer);

         Autodesk.Revit.DB.XYZ refPoint = m_points[0];
         List<Autodesk.Revit.DB.XYZ> directions = GetRelatedVectors(refPoint);
         directions.Sort(comparer);

         return GeomUtil.GetLength(directions[0]);
      }

      /// <summary>
      /// Get the width of the column
      /// </summary>
      /// <returns>the width data</returns>
      private double GetColumnWidth()
      {
         XYZHeightComparer comparer = new XYZHeightComparer();
         m_points.Sort(comparer);

         Autodesk.Revit.DB.XYZ refPoint = m_points[0];
         List<Autodesk.Revit.DB.XYZ> directions = GetRelatedVectors(refPoint);
         directions.Sort(comparer);

         return GeomUtil.GetLength(directions[1]);
      }
   }
}
