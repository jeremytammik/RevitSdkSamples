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
  //using GeoSolid = Autodesk.Revit.Geometry.Solid; // jeremy
	using Element = Autodesk.Revit.Element;


	/// <summary>
	/// provide utility method to get geometry data for 
    /// creating AreaReinforcement on wall or floor
	/// </summary>
	class GeomHelper
	{
		private Document m_currentDoc;	//active document

		/// <summary>
		/// constructor, initialize m_currentDoc
		/// </summary>
		public GeomHelper()
		{
			m_currentDoc =  Command.CommandData.Application.ActiveDocument;
		}

		/// <summary>
		/// get necessary data when create AreaReinforcement on a straight wall
		/// </summary>
		/// <param name="wall">wall on which to create AreaReinforcemen</param>
		/// <param name="refer">reference of the vertical straight face on the wall</param>
		/// <param name="curves">curves compose the vertical face of the wall</param>
		/// <returns>is successful</returns>
		public bool GetWallGeom(Wall wall, ref Reference refer, ref CurveArray curves)
		{
			FaceArray faces = GeomUtil.GetFaces(wall);
			LocationCurve locCurve = wall.Location as LocationCurve;
			//unless API has bug, locCurve can't be null
			if (null == locCurve)
			{
				return false;
			}
			//check the location is line
			Line locLine = locCurve.Curve as Line;
			if (null == locLine)
			{
				return false;
			}

			//get the face reference
			foreach (Face face in faces)
			{
				if (GeomUtil.IsParallel(face, locLine))
				{
					refer = face.Reference;
					break;
				}
			}
			//can't find proper reference
			if (null == refer)
			{
				return false;
			}

			//check the analytical model profile is rectangular
			AnalyticalModelWall model = wall.AnalyticalModel as AnalyticalModelWall;
			if (null == model)
			{
				return false;
			}
			curves = model.Curves;
			if (!GeomUtil.IsRectangular(curves))
			{
				return false;
			}
			
			return true;
		}

		/// <summary>
		/// get necessary data when create AreaReinforcement on a horizontal floor
		/// </summary>
		/// <param name="floor">floor on which to create AreaReinforcemen</param>
		/// <param name="refer">reference of the horizontal face on the floor</param>
		/// <param name="curves">curves compose the horizontal face of the floor</param>
		/// <returns>is successful</returns>
		public bool GetFloorGeom(Floor floor, ref Reference refer, ref CurveArray curves)
		{
			//get horizontal face reference
			FaceArray faces = GeomUtil.GetFaces(floor);
			foreach (Face face in faces)
			{
				if (GeomUtil.IsHorizontalFace(face))
				{
					refer = face.Reference;
					break;
				}
			}
			//no proper reference
			if (null == refer)
			{
				return false;
			}

			//check the analytical model profile is rectangular
			AnalyticalModelFloor model = floor.AnalyticalModel as AnalyticalModelFloor;
			if (null == model)
			{
				return false;
			}
			curves = model.Curves;
			if (!GeomUtil.IsRectangular(curves))
			{
				return false;
			}
		
			return true;
		}
	}
}
