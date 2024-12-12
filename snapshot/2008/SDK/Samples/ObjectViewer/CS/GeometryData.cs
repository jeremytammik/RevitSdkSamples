//
// (C) Copyright 2003-2007 by Autodesk, Inc.
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
using System.Collections.ObjectModel;

using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Geometry;
using Autodesk.Revit.Parameters;

using Element = Autodesk.Revit.Element;
using GeometryElement = Autodesk.Revit.Geometry.Element;
using GeometryOptions = Autodesk.Revit.Geometry.Options;
using GeometryInstance = Autodesk.Revit.Geometry.Instance;

namespace Revit.SDK.Samples.ObjectViewer.CS
{
	/// <summary>
	/// The GeometryDatafactory object is used to transform Revit geometry data
	/// to appropriate format for GDI. 
	/// </summary>
	public class GeometryData
	{
		// boundingBox of the geometry
		private BoundingBoxXYZ m_bbox;
		// curves can represent the wireframe of the geometry
		private List<XYZArray> m_curve3Ds = new List<XYZArray>();
	
		/// <summary>
		/// 3D graphics data of the geometry
		/// </summary>
		public Graphics3DData Data3D
		{
			get
			{
				return new Graphics3DData(new List<XYZArray>(m_curve3Ds), m_bbox);
			}
		}

        /// <summary>
        /// create 3D and 2D data of given GeometryElement
        /// </summary>
        /// <param name="elem"></param>
        /// <param name="detail"></param>
        /// <param name="currentView"></param>
        public GeometryData(Element elem, Options.DetailLevels detail, View currentView)
		{
			Options opt = Command.CommandData.Application.Create.NewGeometryOptions();
			opt.DetailLevel = detail;           		
			opt.ComputeReferences = false;
			GeometryElement geoElement = elem.get_Geometry(opt);

            XYZ xyz = new XYZ(0, 0, 0);
			Transform transform = Transform.get_Translation(ref xyz);
			AddGeoElement(geoElement, transform);

			m_bbox = elem.get_BoundingBox(currentView);
		}

        /// <summary>
        /// create 3D and 2D data of given GeometryElement
        /// </summary>
        /// <param name="elem">of which geometry data be gotten</param>
        /// <param name="currentView">current view of Revit</param>
        public GeometryData(Element elem, View currentView)
        {
            Options opt = Command.CommandData.Application.Create.NewGeometryOptions();
            opt.View = currentView;
            opt.ComputeReferences = false;
            GeometryElement geoElement = elem.get_Geometry(opt);

            XYZ xyz = new XYZ(0, 0, 0);
            Transform transform = Transform.get_Translation(ref xyz);
            AddGeoElement(geoElement, transform);

            m_bbox = elem.get_BoundingBox(currentView);
        }

        /// <summary>
        /// get the solids in a Geometric primitive
        /// </summary>
        /// <param name="obj">a geometry object of element</param>
        /// <param name="transform"></param>
        private void AddGeoElement(GeometryObject obj, Transform transform)
		{
			GeometryElement geometry = obj as GeometryElement;
			if (null == geometry)
			{
				return;
			}

			//get all geometric primitives contained in the GeometryElement
			GeometryObjectArray geometries = geometry.Objects;

			AddGeometryObjects(geometries, transform);
		}

		/// <summary>
		/// iterate GeometryObject in GeometryObjectArray and generate data accordingly
		/// </summary>
		/// <param name="objects"></param>
		/// <param name="transform"></param>
		private void AddGeometryObjects(GeometryObjectArray objects, Transform transform)
		{
			foreach (GeometryObject o in objects)
			{
				//if the type of the geometric primitive is Solid
				string geoType = o.GetType().Name;
				switch (geoType)
				{
					case "Solid":
						AddSolid(o, transform);
						break;
					case "Face":
						AddFace(o, transform);
						break;
					case "Mesh":
						AddMesh(o, transform);
						break;
					case "Curve":
					case "Line":
					case "Arc":
						AddCurve(o, transform);
						break;
					case "Profile":
						AddProfile(o, transform);
						break;
					case "Element":
						AddGeoElement(o, transform);
						break;
					case "Instance":
						AddInstance(o, transform);
						break;
					case "Edge":
						AddEdge(o, transform);
						break;
					default:
						break;
				}
			}
		}

		/// <summary>
		/// generate data of a Solid
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="transform"></param>
		private void AddSolid(GeometryObject obj, Transform transform)
		{
			Solid solid = obj as Solid;
			if (null == solid)
			{
				return;
			}

			//a solid has many faces
			FaceArray faces = solid.Faces;
			if (faces.Size == 0)
			{
				return;
			}

			foreach (Face face in faces)
			{
				AddFace(face, transform);
			}
		}

		/// <summary>
		/// generate data of a Face
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="transform"></param>
		private void AddFace(GeometryObject obj, Transform transform)
		{
			Face face = obj as Face;
			if (null == face)
			{
				return;
			}

			Mesh mesh = face.Triangulate();
			if (null == mesh)
			{
				return;
			}
			AddMesh(mesh, transform);
		}

		/// <summary>
		/// generate data of a Profile
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="transform"></param>
		private void AddProfile(GeometryObject obj, Transform transform)
		{
			Profile profile = obj as Profile;
			if (null == profile)
			{
				return;
			}

			foreach (Curve curve in profile.Curves)
			{
				AddCurve(curve, transform);
			}
		}

		/// <summary>
		/// generate data of a Mesh
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="transform"></param>
		private void AddMesh(GeometryObject obj, Transform transform)
		{
			Mesh mesh = obj as Mesh;
			if (null == mesh)
			{
				return;
			}

			//a face has a mesh, all meshes are made of triangles
			for (int i = 0; i < mesh.NumTriangles; i++)
			{
				MeshTriangle triangular = mesh.get_Triangle(i);
				XYZArray points = new XYZArray();
				try
				{
					for (int n = 0; n < 3; n++)
					{
						XYZ point = triangular.get_Vertex(n);
						XYZ newPoint = MathUtil.GetBasis(point, transform);
						points.Append(ref newPoint);
					}
					XYZ iniPoint = points.get_Item(0);
					points.Append(ref iniPoint);
					m_curve3Ds.Add(points);
				}
				catch
				{
				}
			}
		}

		/// <summary>
		/// generate data of a Curve
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="transform"></param>
		private void AddCurve(GeometryObject obj, Transform transform)
		{
			Curve curve = obj as Curve;
			if (null == curve)
			{
				return;
			}

			if (curve.IsBound)
			{
				XYZArray points = curve.Tessellate();
				XYZArray result = new XYZArray();
				foreach (XYZ point in points)
				{
					XYZ newPoint = MathUtil.GetBasis(point, transform);
					result.Append(ref newPoint);
				}
				m_curve3Ds.Add(result);
			}
		}

		/// <summary>
		/// generate data of a Instance
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="transform"></param>
		private void AddInstance(GeometryObject obj, Transform transform)
		{
			GeometryInstance instance = obj as GeometryInstance;
			if (null == instance)
			{
				return;
			}
			//get a transformation of the affine 3-space
			Transform allTransform = AddTransform(transform, instance.Transform);

			GeometryElement instanceGeometry = instance.SymbolGeometry;
			if (null == instanceGeometry)
			{
				return;
			}
			//get all geometric primitives contained in the GeometryElement
			GeometryObjectArray instanceGeometries = instanceGeometry.Objects;
			AddGeometryObjects(instanceGeometries, allTransform);
		}

		/// <summary>
		/// generate data of a Edge
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="transform"></param>
		private void AddEdge(GeometryObject obj, Transform transform)
		{
			Edge edge = obj as Edge;
			if (null == edge)
			{
				return;
			}

			XYZArray points = edge.Tessellate();
			XYZArray result = new XYZArray();
			foreach (XYZ point in points)
			{
				XYZ newPoint = MathUtil.GetBasis(point, transform);
				result.Append(ref newPoint);
			}
			m_curve3Ds.Add(result);
		}

		/// <summary>
		/// Add 2 Transform Matrix
		/// </summary>
		/// <param name="tran1"></param>
		/// <param name="tran2"></param>
		/// <returns></returns>
		private Transform AddTransform(Transform tran1, Transform tran2)
		{
            XYZ xyz = new XYZ(0, 0, 0);
            Transform result = Transform.get_Translation(ref xyz);
			result.Origin = MathUtil.AddXYZ(tran1.Origin, tran2.Origin);

			XYZ[] left = new XYZ[3];
			XYZ[] right = new XYZ[3];

			for (int i = 0; i < 3; i++)
			{
				left[i] = tran1.get_Basis(i);
				right[i] = tran2.get_Basis(i);
			}

			XYZ[] temp = MathUtil.MultiCross(left, right);

			for (int i = 0; i < 3; i++)
			{
				result.set_Basis(i, temp[i]);
			}

			return result;
		}
	}
}
