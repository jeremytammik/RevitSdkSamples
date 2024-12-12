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
using System.Linq;
using System.Text;
using Autodesk.Revit.DB;

namespace Revit.SDK.Samples.GeometryCreation_BooleanOperation.CS
{
   class GeometryCreation
   {
      /// <summary>
      /// The singleton instance of GeometryCreation
      /// </summary>
      private static GeometryCreation Instance;

      /// <summary>
      /// revit application
      /// </summary>
      private Autodesk.Revit.ApplicationServices.Application m_app;

      /// <summary>
      /// The direction of cylinder, only on X, Y, Z three axes forward direction
      /// </summary>
      public enum CylinderDirection
      {
         BasisX,
         BasisY,
         BasisZ
      };

      /// <summary>
      /// Constructor
      /// </summary>
      /// <param name="app">Revit application</param>
      private GeometryCreation(Autodesk.Revit.ApplicationServices.Application app)
      {
         m_app = app;
      }

      /// <summary>
      /// Get the singleton instance of GeometryCreation
      /// </summary>
      /// <param name="app">Revit application</param>
      /// <returns>The singleton instance of GeometryCreation</returns>
      public static GeometryCreation getInstance(Autodesk.Revit.ApplicationServices.Application app)
      {
         if (Instance == null)
         {
            Instance = new GeometryCreation(app);
         }
         return Instance;
      }

      /// <summary>
      /// Create an extrusion geometry
      /// </summary>
      /// <param name="profileLoops">The profile loops to be extruded</param>
      /// <param name="extrusionDir">The direction in which to extrude the profile loops</param>
      /// <param name="extrusionDist">The positive distance by which the loops are to be extruded</param>
      /// <returns>The created solid</returns>
      private Solid CreateExtrusion(List<CurveLoop> profileLoops, XYZ extrusionDir, double extrusionDist)
      {
         return GeometryCreationUtilities.CreateExtrusionGeometry(profileLoops, extrusionDir, extrusionDist);
      }

      /// <summary>
      /// Create a revolved geometry
      /// </summary>
      /// <param name="coordinateFrame">A right-handed orthonormal frame of vectors</param>
      /// <param name="profileLoops">The profile loops to be revolved</param>
      /// <param name="startAngle">The start angle for the revolution</param>
      /// <param name="endAngle">The end angle for the revolution</param>
      /// <returns>The created solid</returns>
      private Solid CreateRevolved(Autodesk.Revit.DB.Frame coordinateFrame, List<CurveLoop> profileLoops, double startAngle, double endAngle)
      {
         return GeometryCreationUtilities.CreateRevolvedGeometry(coordinateFrame, profileLoops, startAngle, endAngle);
      }

      /// <summary>
      /// Create a swept geometry
      /// </summary>
      /// <param name="sweepPath">The sweep path, consisting of a set of contiguous curves</param>
      /// <param name="pathAttachmentCrvIdx">The index of the curve in the sweep path where the profile loops are situated</param>
      /// <param name="pathAttachmentParam">Parameter of the path curve specified by pathAttachmentCrvIdx</param>
      /// <param name="profileLoops">The curve loops defining the planar domain to be swept along the path</param>
      /// <returns>The created solid</returns>
      private Solid CreateSwept(CurveLoop sweepPath, int pathAttachmentCrvIdx, double pathAttachmentParam, List<CurveLoop> profileLoops)
      {
         return GeometryCreationUtilities.CreateSweptGeometry(sweepPath, pathAttachmentCrvIdx, pathAttachmentParam, profileLoops);
      }

      /// <summary>
      /// Create a blend geometry
      /// </summary>
      /// <param name="firstLoop">The first curve loop</param>
      /// <param name="secondLoop">The second curve loop</param>
      /// <param name="vertexPairs">This input specifies how the two profile loops should be connected</param>
      /// <returns>The created solid</returns>
      private Solid CreateBlend(CurveLoop firstLoop, CurveLoop secondLoop, List<VertexPair> vertexPairs)
      {
         return GeometryCreationUtilities.CreateBlendGeometry(firstLoop, secondLoop, vertexPairs);
      }

      /// <summary>
      /// Create a swept and blend geometry
      /// </summary>
      /// <param name="pathCurve">The sweep path</param>
      /// <param name="pathParams">An increasing sequence of parameters along the path curve</param>
      /// <param name="profileLoops">Closed, planar curve loops arrayed along the path</param>
      /// <param name="vertexPairs">This input specifies how adjacent profile loops should be connected</param>
      /// <returns>The created solid</returns>
      private Solid CreateSweptBlend(Curve pathCurve, List<double> pathParams, List<CurveLoop> profileLoops, List<ICollection<VertexPair>> vertexPairs)
      {
         return GeometryCreationUtilities.CreateSweptBlendGeometry(pathCurve, pathParams, profileLoops, vertexPairs);
      }

      /// <summary>
      /// Create a centerbased box
      /// </summary>
      /// <param name="center">The given box center</param>
      /// <param name="edgelength">The given box's edge length</param>
      /// <returns>The created box</returns>
      public Solid CreateCenterbasedBox(XYZ center, double edgelength)
      {
         double halfedgelength = edgelength / 2.0;

         List<CurveLoop> profileloops = new List<CurveLoop>();
         CurveLoop profileloop = new CurveLoop();
         profileloop.Append(Line.CreateBound(
            new XYZ(center.X - halfedgelength, center.Y - halfedgelength, center.Z - halfedgelength),
            new XYZ(center.X - halfedgelength, center.Y + halfedgelength, center.Z - halfedgelength)));
         profileloop.Append(Line.CreateBound(
            new XYZ(center.X - halfedgelength, center.Y + halfedgelength, center.Z - halfedgelength),
            new XYZ(center.X + halfedgelength, center.Y + halfedgelength, center.Z - halfedgelength)));
         profileloop.Append(Line.CreateBound(
            new XYZ(center.X + halfedgelength, center.Y + halfedgelength, center.Z - halfedgelength),
            new XYZ(center.X + halfedgelength, center.Y - halfedgelength, center.Z - halfedgelength)));
         profileloop.Append(Line.CreateBound(
            new XYZ(center.X + halfedgelength, center.Y - halfedgelength, center.Z - halfedgelength),
            new XYZ(center.X - halfedgelength, center.Y - halfedgelength, center.Z - halfedgelength)));
         profileloops.Add(profileloop);

         XYZ extrusiondir = new XYZ(0, 0, 1); // orthogonal

         double extrusiondist = edgelength;

         return GeometryCreationUtilities.CreateExtrusionGeometry(profileloops, extrusiondir, extrusiondist);
      }

      /// <summary>
      /// Create a centerbased sphere
      /// </summary>
      /// <param name="center">The given sphere center</param>
      /// <param name="radius">The given sphere's radius</param>
      /// <returns>The created sphere</returns>
      public Solid CreateCenterbasedSphere(XYZ center, double radius)
      {
         Frame frame = new Frame(center,
            Autodesk.Revit.DB.XYZ.BasisX,
            Autodesk.Revit.DB.XYZ.BasisY,
            Autodesk.Revit.DB.XYZ.BasisZ);

         List<CurveLoop> profileloops = new List<CurveLoop>();
         CurveLoop profileloop = new CurveLoop();
         Curve cemiEllipse = Ellipse.CreateCurve(center, radius, radius,
            Autodesk.Revit.DB.XYZ.BasisX,
            Autodesk.Revit.DB.XYZ.BasisZ,
            -Math.PI / 2.0, Math.PI / 2.0);
         profileloop.Append(cemiEllipse);
         profileloop.Append(Line.CreateBound(
            new XYZ(center.X, center.Y, center.Z + radius),
            new XYZ(center.X, center.Y, center.Z - radius)));
         profileloops.Add(profileloop);

         return GeometryCreationUtilities.CreateRevolvedGeometry(frame, profileloops, -Math.PI, Math.PI);
      }

      /// <summary>
      /// Create a centerbased cylinder, only on X, Y, Z three axes forward direction
      /// </summary>
      /// <param name="center">The given cylinder center</param>
      /// <param name="bottomradius">The given cylinder's bottom radius</param>
      /// <param name="height">The given cylinder's height</param>
      /// <param name="cylinderdirection">Cylinder's extrusion direction</param>
      /// <returns>The created cylinder</returns>
      public Solid CreateCenterbasedCylinder(XYZ center, double bottomradius, double height, CylinderDirection cylinderdirection)
      {
         double halfheight = height / 2.0;
         XYZ bottomcenter = new XYZ(
            cylinderdirection == CylinderDirection.BasisX ? center.X - halfheight : center.X,
            cylinderdirection == CylinderDirection.BasisY ? center.Y - halfheight : center.Y,
            cylinderdirection == CylinderDirection.BasisZ ? center.Z - halfheight : center.Z);
         XYZ topcenter = new XYZ(
            cylinderdirection == CylinderDirection.BasisX ? center.X + halfheight : center.X,
            cylinderdirection == CylinderDirection.BasisY ? center.Y + halfheight : center.Y,
            cylinderdirection == CylinderDirection.BasisZ ? center.Z + halfheight : center.Z);

         CurveLoop sweepPath = new CurveLoop();
         sweepPath.Append(Line.CreateBound(bottomcenter,
            topcenter));

         List<CurveLoop> profileloops = new List<CurveLoop>();
         CurveLoop profileloop = new CurveLoop();
            Curve cemiEllipse1 = Ellipse.CreateCurve(bottomcenter, bottomradius, bottomradius,
            cylinderdirection == CylinderDirection.BasisX ? Autodesk.Revit.DB.XYZ.BasisY : Autodesk.Revit.DB.XYZ.BasisX,
            cylinderdirection == CylinderDirection.BasisZ ? Autodesk.Revit.DB.XYZ.BasisY : Autodesk.Revit.DB.XYZ.BasisZ,
            -Math.PI, 0);
         Curve cemiEllipse2 = Ellipse.CreateCurve(bottomcenter, bottomradius, bottomradius,
            cylinderdirection == CylinderDirection.BasisX ? Autodesk.Revit.DB.XYZ.BasisY : Autodesk.Revit.DB.XYZ.BasisX,
            cylinderdirection == CylinderDirection.BasisZ ? Autodesk.Revit.DB.XYZ.BasisY : Autodesk.Revit.DB.XYZ.BasisZ,
            0, Math.PI);
         profileloop.Append(cemiEllipse1);
         profileloop.Append(cemiEllipse2);
         profileloops.Add(profileloop);

         return GeometryCreationUtilities.CreateSweptGeometry(sweepPath, 0, 0, profileloops);
      }
   }
}
