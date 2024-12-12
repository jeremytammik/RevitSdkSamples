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
using System.Collections.Generic;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Element = Autodesk.Revit.DB.Element;
using GElement = Autodesk.Revit.DB.GeometryElement;

namespace Revit.SDK.Samples.WindowWizard.CS
{
   /// <summary>
   /// A object to help locating with geometry data.
   /// </summary>
   public class GeoHelper
   {
      /// <summary>
      /// store the const precision
      /// </summary>
      private const double Precision = 0.0001;

      /// <summary>
      /// The method is used to get the wall face along the specified parameters
      /// </summary>
      /// <param name="wall">the wall</param>
      /// <param name="view">the options view</param>
      /// <param name="ExtOrInt">if true indicate that get exterior wall face, else false get the interior wall face</param>
      /// <returns>the face</returns>
      static public Face GetWallFace(Wall wall, View view, bool ExtOrInt)
      {
         FaceArray faces = null;
         Face face = null;
         Options options = new Options();
         options.ComputeReferences = true;
         options.View = view;
         if (wall != null)
         {
            //GeometryObjectArray geoArr = wall.get_Geometry(options).Objects;
            IEnumerator<GeometryObject> Objects = wall.get_Geometry(options).GetEnumerator();
            //foreach (GeometryObject geoObj in geoArr)
            while (Objects.MoveNext())
            {
               GeometryObject geoObj = Objects.Current;

               if (geoObj is Solid)
               {
                  Solid s = geoObj as Solid;
                  faces = s.Faces;
               }
            }
         }
         if (ExtOrInt)
            face = GetExteriorFace(faces);
         else
            face = GetInteriorFace(faces);
         return face;
      }

      /// <summary>
      /// The method is used to get extrusion's face along to the specified parameters
      /// </summary>
      /// <param name="extrusion">the extrusion</param>
      /// <param name="view">options view</param>
      /// <param name="ExtOrInt">If true indicate getting exterior extrusion face, else getting interior extrusion face</param>
      /// <returns>the face</returns>
      static public Face GetExtrusionFace(Extrusion extrusion, View view, bool ExtOrInt)
      {
         Face face = null;
         FaceArray faces = null;
         if (extrusion.IsSolid)
         {
            Options options = new Options();
            options.ComputeReferences = true;
            options.View = view;
            //GeometryObjectArray geoArr = extrusion.get_Geometry(options).Objects;
            IEnumerator<GeometryObject> Objects = extrusion.get_Geometry(options).GetEnumerator();
            //foreach (GeometryObject geoObj in geoArr)
            while (Objects.MoveNext())
            {
               GeometryObject geoObj = Objects.Current;

               if (geoObj is Solid)
               {
                  Solid s = geoObj as Solid;
                  faces = s.Faces;
               }
            }
            if (ExtOrInt)
               face = GetExteriorFace(faces);
            else
               face = GetInteriorFace(faces);
         }
         return face;
      }

      /// <summary>
      /// The assistant method is used for getting wall face and getting extrusion face
      /// </summary>
      /// <param name="faces">faces array</param>
      /// <returns>the face</returns>
      static private Face GetExteriorFace(FaceArray faces)
      {
         double elevation = 0;
         double tempElevation = 0;
         Mesh mesh = null;
         Face face = null;
         foreach (Face f in faces)
         {
            tempElevation = 0;
            mesh = f.Triangulate();
            foreach (Autodesk.Revit.DB.XYZ xyz in mesh.Vertices)
            {
               tempElevation = tempElevation + xyz.Y;
            }
            tempElevation = tempElevation / mesh.Vertices.Count;
            if (elevation < tempElevation || null == face)
            {
               face = f;
               elevation = tempElevation;
            }
         }
         return face;
      }

      /// <summary>
      /// The assistant method is used for getting wall face and getting extrusion face
      /// </summary>
      /// <param name="faces">faces array</param>
      /// <returns>the face</returns>
      static private Face GetInteriorFace(FaceArray faces)
      {
         double elevation = 0;
         double tempElevation = 0;
         Mesh mesh = null;
         Face face = null;
         foreach (Face f in faces)
         {
            tempElevation = 0;
            mesh = f.Triangulate();
            foreach (Autodesk.Revit.DB.XYZ xyz in mesh.Vertices)
            {
               tempElevation = tempElevation + xyz.Y;
            }
            tempElevation = tempElevation / mesh.Vertices.Count;
            if (elevation > tempElevation || null == face)
            {
               face = f;
               elevation = tempElevation;
            }
         }
         return face;
      }

      /// <summary>
      /// Find out the three points which made of a plane.
      /// </summary>
      /// <param name="mesh">A mesh contains many points.</param>
      /// <param name="startPoint">Create a new instance of ReferencePlane.</param>
      /// <param name="endPoint">The free end apply to reference plane.</param>
      /// <param name="thirdPnt">A third point needed to define the reference plane.</param>
      static public void Distribute(Mesh mesh, ref Autodesk.Revit.DB.XYZ startPoint, ref Autodesk.Revit.DB.XYZ endPoint, ref Autodesk.Revit.DB.XYZ thirdPnt)
      {
         int count = mesh.Vertices.Count;
         startPoint = mesh.Vertices[0];
         endPoint = mesh.Vertices[(int)(count / 3)];
         thirdPnt = mesh.Vertices[(int)(count / 3 * 2)];
      }

      /// <summary>
      /// Determines whether a edge is vertical.
      /// </summary>
      /// <param name="edge">The edge to be determined.</param>
      /// <returns>Return true if this edge is vertical, or else return false.</returns>
      static public bool IsVerticalEdge(Edge edge)
      {
         List<XYZ> polyline = edge.Tessellate() as List<XYZ>;
         Autodesk.Revit.DB.XYZ verticalVct = new Autodesk.Revit.DB.XYZ(0, 0, 1);
         Autodesk.Revit.DB.XYZ pointBuffer = polyline[0];

         for (int i = 1; i < polyline.Count; i = i + 1)
         {
            Autodesk.Revit.DB.XYZ temp = polyline[i];
            Autodesk.Revit.DB.XYZ vector = GetVector(pointBuffer, temp);
            if (Equal(vector, verticalVct))
            {
               return true;
            }
            else
            {
               continue;
            }
         }
         return false;
      }

      /// <summary>
      /// Get the vector between two points.
      /// </summary>
      /// <param name="startPoint">The start point.</param>
      /// <param name="endPoint">The end point.</param>
      /// <returns>The vector between two points.</returns>
      static public Autodesk.Revit.DB.XYZ GetVector(Autodesk.Revit.DB.XYZ startPoint, Autodesk.Revit.DB.XYZ endPoint)
      {
         return new Autodesk.Revit.DB.XYZ(endPoint.X - startPoint.X,
             endPoint.Y - startPoint.Y, endPoint.Z - startPoint.Z);
      }

      /// <summary>
      /// Determines whether two vector are equal in x and y axis.
      /// </summary>
      /// <param name="vectorA">The vector A.</param>
      /// <param name="vectorB">The vector B.</param>
      /// <returns>Return true if two vector are equals, or else return false.</returns>
      static public bool Equal(Autodesk.Revit.DB.XYZ vectorA, Autodesk.Revit.DB.XYZ vectorB)
      {
         bool isNotEqual = (Precision < Math.Abs(vectorA.X - vectorB.X)) ||
             (Precision < Math.Abs(vectorA.Y - vectorB.Y));
         return isNotEqual ? false : true;
      }
   }
}
