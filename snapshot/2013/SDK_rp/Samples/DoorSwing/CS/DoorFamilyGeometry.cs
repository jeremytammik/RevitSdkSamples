//
// (C) Copyright 2003-2012 by Autodesk, Inc.
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
using System.Text;
using System.Linq;

using Autodesk.Revit;
using Autodesk.Revit.DB;


namespace Revit.SDK.Samples.DoorSwing.CS
{
   /// <summary>
   /// The DoorGeometry object is used to transform Revit geometry data
   /// to appropriate format for GDI.
   /// </summary>
   public class DoorGeometry
   {
      #region "Members"

      // User preferences for parsing of geometry.
      Options m_options; 
      // boundingBox of the geometry. 
      BoundingBoxXYZ m_bbox;  
      // curves can represent the wireFrame of the door's geometry.
      List<List<XYZ>> m_curve3Ds = new List<List<XYZ>>();

      #endregion

      #region "Properties"

      /// <summary>
      /// BoundingBox of the 2D geometry.
      /// </summary>
      public System.Drawing.RectangleF BBOX2D
      {
         get
         {
            return new System.Drawing.RectangleF((float)m_bbox.Min.X, (float)m_bbox.Min.Y,
                  (float)(m_bbox.Max.X - m_bbox.Min.X), (float)(m_bbox.Max.Y - m_bbox.Min.Y));
         }
      }

      #endregion

      #region "Methods"

      /// <summary>
      /// construct function.
      /// </summary>
      /// <param name="door">of which geometry data is wanted.</param>
      public DoorGeometry(Autodesk.Revit.DB.Element door)
      {
         m_options                              = new Options();
         m_options.View                         = GetPlanform2DView(door);
         m_options.ComputeReferences            = false;
         Autodesk.Revit.DB.GeometryElement geoEle = door.get_Geometry(m_options);
         AddGeometryElement(geoEle);

         m_bbox = door.get_BoundingBox(m_options.View);
      }

      /// <summary>
      /// Draw the line contains in m_curve3Ds in 2d Preview.Drawn as top view.
      /// </summary>
      /// <param name="graphics">Graphics to draw</param>
      /// <param name="drawPen">The pen to draw curves.</param>
      public void DrawGraphics(System.Drawing.Graphics graphics, System.Drawing.Pen drawPen)
      {
         for (int i = 0; i < m_curve3Ds.Count; i++)
         {
            List<XYZ> points = m_curve3Ds[i];

            for (int j = 0; j < (points.Count - 1); j++)
            {
               // ignore xyz.Z value, drawn as top view.
               System.Drawing.PointF startPoint = new System.Drawing.PointF((float)points[j].X, (float)points[j].Y);
               System.Drawing.PointF endPoint   = new System.Drawing.PointF((float)points[j + 1].X, (float)points[j + 1].Y);
               graphics.DrawLine(drawPen, startPoint, endPoint);
            }
         }
      }

      /// <summary>
      /// Retrieve the ViewPlan corresponding to the door's level. 
      /// </summary>
      /// <param name="door">
      /// one door whose level is corresponding to the retrieved ViewPlan.
      /// </param>
      /// <returns>One ViewPlan</returns>
      static private ViewPlan GetPlanform2DView(Autodesk.Revit.DB.Element door)
      {
          IEnumerable<ViewPlan> viewPlans = from elem in
                                                new FilteredElementCollector(door.Document).OfClass(typeof(ViewPlan)).ToElements()
                                            let viewPlan = elem as ViewPlan
                                            where viewPlan != null && !viewPlan.IsTemplate && viewPlan.GenLevel.Id.IntegerValue == door.Level.Id.IntegerValue
                                            select viewPlan;
          if (viewPlans.Count() > 0)
          {
              return viewPlans.First();
          }
          else
              return null;
      }

      /// <summary>
      /// iterate GeometryObject in GeometryObjectArray and generate data accordingly.
      /// </summary>
      /// <param name="geoEle">a geometry object of element</param>
      private void AddGeometryElement(Autodesk.Revit.DB.GeometryElement geoEle)
      {
         // get all geometric primitives contained in the Geometry Element
         GeometryObjectArray geoObjArray = geoEle.Objects;

         // iterate each Geometry Object and generate data accordingly.
         foreach (GeometryObject geoObj in geoObjArray)
         {
            if (geoObj is Curve)
            {
               AddCurve(geoObj);
            }
            else if (geoObj is Edge)
            {
               AddEdge(geoObj);
            }
            else if (geoObj is Autodesk.Revit.DB.GeometryElement)
            {
               AddElement(geoObj);
            }
            else if (geoObj is Face)
            {
               AddFace(geoObj);
            }
            else if (geoObj is Autodesk.Revit.DB.GeometryInstance)
            {
               AddInstance(geoObj);
            }
            else if (geoObj is Mesh)
            {
               AddMesh(geoObj);
            }
            else if (geoObj is Profile)
            {
               AddProfile(geoObj);
            }
            else if (geoObj is Solid)
            {
               AddSolid(geoObj);
            }
         }
      }

      /// <summary>
      /// generate data of a Curve.
      /// </summary>
      /// <param name="obj">a geometry object of element.</param>
      private void AddCurve(GeometryObject obj)
      {
         Curve curve = obj as Curve;

         if (!curve.IsBound)
         {
            return;
         }

         // get a polyline approximation to the curve.
         List<XYZ> points = curve.Tessellate() as List<XYZ>;

         m_curve3Ds.Add(points);
      }

      /// <summary>
      /// generate data of an Edge.
      /// </summary>
      /// <param name="obj">a geometry object of element.</param>
      private void AddEdge(GeometryObject obj)
      {
         Edge edge       = obj as Edge;

         // get a polyline approximation to the edge.
         List<XYZ> points = edge.Tessellate() as List<XYZ>;

         m_curve3Ds.Add(points);
      }

      /// <summary>
      /// generate data of a Geometry Element.
      /// </summary>
      /// <param name="obj">a geometry object of element.</param>
      private void AddElement(GeometryObject obj)
      {
         Autodesk.Revit.DB.GeometryElement geoEle = obj as Autodesk.Revit.DB.GeometryElement;
         AddGeometryElement(geoEle);
      }

      /// <summary>
      /// generate data of a Face.
      /// </summary>
      /// <param name="obj">a geometry object of element.</param>
      private void AddFace(GeometryObject obj)
      {
         Face face = obj as Face;

         // get a triangular mesh approximation to the face.
         Mesh mesh = face.Triangulate();
         if (null != mesh)
         {
            AddMesh(mesh);
         }
      }

      /// <summary>
      /// generate data of a Geometry Instance.
      /// </summary>
      /// <param name="obj">a geometry object of element.</param>
      private void AddInstance(GeometryObject obj)
      {
          Autodesk.Revit.DB.GeometryInstance instance = obj as Autodesk.Revit.DB.GeometryInstance;
         Autodesk.Revit.DB.GeometryElement geoElement = instance.SymbolGeometry;

         AddGeometryElement(geoElement);
      }

      /// <summary>
      /// generate data of a Mesh.
      /// </summary>
      /// <param name="obj">a geometry object of element.</param>
      private void AddMesh(GeometryObject obj)
      {
         Mesh mesh = obj as Mesh;
         List<XYZ> points = new List<XYZ>();

         // get all triangles of the mesh.
         for (int i = 0; i < mesh.NumTriangles; i++)
         {
            MeshTriangle trigangle = mesh.get_Triangle(i);

            for (int j = 0; j < 3; j++)
            {
               // A vertex of the triangle.
               Autodesk.Revit.DB.XYZ point = trigangle.get_Vertex(j);

               double x = point.X;
               double y = point.Y;
               double z = point.Z;

               points.Add(point);
            }

            Autodesk.Revit.DB.XYZ iniPoint = points[0];
            points.Add(iniPoint);

            m_curve3Ds.Add(points);
         }
      }

      /// <summary>
      /// generate data of a Profile.
      /// </summary>
      /// <param name="obj">a geometry object of element.</param>
      private void AddProfile(GeometryObject obj)
      {
         Profile profile = obj as Profile;

         // get the curves that make up the boundary of the profile.
         CurveArray curves = profile.Curves;

         foreach (Curve curve in curves)
         {
            AddCurve(curve);
         }
      }

      /// <summary>
      /// generate data of a Solid.
      /// </summary>
      /// <param name="obj">a geometry object of element.</param>
      private void AddSolid(GeometryObject obj)
      {
         Solid solid = obj as Solid;

         // get the faces that belong to the solid.
         FaceArray faces = solid.Faces;

         foreach (Face face in faces)
         {
            AddFace(face);
         }
      }

      #endregion
   }
}
