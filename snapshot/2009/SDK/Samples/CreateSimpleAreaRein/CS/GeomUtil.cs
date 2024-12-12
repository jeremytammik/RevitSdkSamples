//
// (C) Copyright 2003-2008 by Autodesk, Inc.
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
namespace Revit.SDK.Samples.CreateSimpleAreaRein.CS
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using System.Windows.Forms;

	using Autodesk.Revit;
	using Autodesk.Revit.Parameters;
	using Autodesk.Revit.Elements;
	using Autodesk.Revit.Geometry;
	using Autodesk.Revit.Symbols;
	using Autodesk.Revit.Structural;

	using GeoElement = Autodesk.Revit.Geometry.Element;
	using GeoSolid = Autodesk.Revit.Geometry.Solid;
	using Element = Autodesk.Revit.Element;

	/// <summary>
	/// provide some common geometry judgement and calculate method
	/// </summary>
	class GeomUtil
	{
		const double PRECISION = 0.00001;	//precision when judge whether two doubles are equal

		/// <summary>
		/// get all faces that compose the geometry solid of given element
		/// </summary>
		/// <param name="elem">element to be calculated</param>
		/// <returns>all faces</returns>
		public static FaceArray GetFaces(Element elem)
		{
			List<Face> faces = new List<Face>();

			Autodesk.Revit.Geometry.Options geoOptions = 
                Command.CommandData.Application.Create.NewGeometryOptions();
			geoOptions.ComputeReferences = true;

			GeoElement geoElem = elem.get_Geometry(geoOptions);
			GeometryObjectArray geoElems = geoElem.Objects;

			foreach (object o in geoElems)
			{
				GeoSolid geoSolid = o as GeoSolid;
				if (null == geoSolid)
				{
					continue;
				}

				return geoSolid.Faces;
			}

			return null;
		}

		/// <summary>
		/// get all points proximate to the given face
		/// </summary>
		/// <param name="face">face to be calculated</param>
		/// <returns></returns>
		public static List<XYZ> GetPoints(Face face)
		{
			List<XYZ> points = new List<XYZ>();
			XYZArray XYZs = face.Triangulate().Vertices;

			foreach (XYZ point in XYZs)
			{
				points.Add(point);
			}

			return points;
		}

		/// <summary>
		/// judge whether the given face is horizontal
		/// </summary>
		/// <param name="face">face to be judged</param>
		/// <returns>is horizontal</returns>
		public static bool IsHorizontalFace(Face face)
		{
			List<XYZ> points = GetPoints(face);
			double z1 = points[0].Z;
			double z2 = points[1].Z;
			double z3 = points[2].Z;
			double z4 = points[3].Z;
			bool flag = IsEqual(z1, z2);
			flag = flag && IsEqual(z2, z3);
			flag = flag && IsEqual(z3, z4);
			flag = flag && IsEqual(z4, z1);

			return flag;
		}

		/// <summary>
		/// judge whether a face and a line are parallel
		/// </summary>
		/// <param name="face"></param>
		/// <param name="line"></param>
		/// <returns></returns>
		public static bool IsParallel(Face face, Line line)
		{
			List<XYZ> points = GetPoints(face);
			XYZ vector1 = SubXYZ(points[0], points[1]);
			XYZ vector2 = SubXYZ(points[1], points[2]);
			XYZ refer = SubXYZ(line.get_EndPoint(0), line.get_EndPoint(1));

			XYZ cross = CrossMatrix(vector1, vector2);
			double result = DotMatrix(cross, refer);

			if (result < PRECISION)
			{
				return true;
			}
			return false;
		}

		/// <summary>
		/// judge whether given 4 lines can form a rectangular
		/// </summary>
		/// <param name="lines"></param>
		/// <returns>is rectangular</returns>
		public static bool IsRectangular(CurveArray curves)
		{
			//make sure the CurveArray contains 4 line
			if (curves.Size != 4)
			{
				return false;
			}
			Line[] lines = new Line[4];
			for (int i = 0; i < 4; i++)
			{
				lines[i] = curves.get_Item(i) as Line;
				if (null == lines[i])
				{
					return false;
				}
			}

			//make sure the first line is vertical to 2 lines and parallel to another line
			Line iniLine = lines[0];
			Line[] verticalLines = new Line[2];
			Line paraLine = null;
			int index = 0;
			for (int i = 1; i < 4; i++)
			{
				if (IsVertical(lines[0], lines[i]))
				{
					verticalLines[index] = lines[i];
					index++;
				}
				else
				{
					paraLine = lines[i];
				}
			}
			if (index != 2)
			{
				return false;
			}
			bool flag = IsVertical(paraLine, verticalLines[0]);
			return flag;
		}

		/// <summary>
		/// judge whether two lines are vertical
		/// </summary>
		/// <param name="line1"></param>
		/// <param name="line2"></param>
		/// <returns></returns>
		private static bool IsVertical(Line line1, Line line2)
		{
			XYZ vector1 = SubXYZ(line1.get_EndPoint(0), line1.get_EndPoint(1));
			XYZ vector2 = SubXYZ(line2.get_EndPoint(0), line2.get_EndPoint(1));

			double result = DotMatrix(vector1, vector2);

			if (Math.Abs(result) < PRECISION)
			{
				return true;
			}
			return false;
		}

		/// <summary>
		/// subtraction of two XYZ as Matrix
		/// </summary>
		/// <param name="p1"></param>
		/// <param name="p2"></param>
		/// <returns></returns>
		private static XYZ SubXYZ(XYZ p1, XYZ p2)
		{
			double x = p1.X - p2.X;
			double y = p1.Y - p2.Y;
			double z = p1.Z - p2.Z;

			XYZ result = new XYZ(x, y, z);
			return result;
		}

		/// <summary>
		/// multiplication cross of two XYZ as Matrix
		/// </summary>
		/// <param name="p1"></param>
		/// <param name="p2"></param>
		/// <returns></returns>
		private static XYZ CrossMatrix(XYZ p1, XYZ p2)
		{
			double v1 = p1.X;
			double v2 = p1.Y;
			double v3 = p1.Z;

			double u1 = p2.X;
			double u2 = p2.Y;
			double u3 = p2.Z;

			double x = v3 * u2 - v2 * u3;
			double y = -v3 * u1 + v1 * u3;
			double z = v2 * u1 - v1 * u2;

			XYZ point = new XYZ(x, y, z);
			return point;
		}

		/// <summary>
		/// dot product of two XYZ as Matrix
		/// </summary>
		/// <param name="p1"></param>
		/// <param name="p2"></param>
		/// <returns></returns>
		private static double DotMatrix(XYZ p1, XYZ p2)
		{
			double v1 = p1.X;
			double v2 = p1.Y;
			double v3 = p1.Z;

			double u1 = p2.X;
			double u2 = p2.Y;
			double u3 = p2.Z;

			double result = v1 * u1 + v2 * u2 + v3 * u3;

			return result;
		}

		/// <summary>
		/// judge whether the subtraction of two doubles is less than 
        /// the internal dicided precision
		/// </summary>
		/// <param name="d1"></param>
		/// <param name="d2"></param>
		/// <returns></returns>
		private static bool IsEqual(double d1, double d2)
		{
			double diff = Math.Abs(d1 - d2);
			if (diff < PRECISION)
			{
				return true;
			}
			return false;
		}
	}
}
