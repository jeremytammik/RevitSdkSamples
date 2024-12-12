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

namespace Revit.SDK.Samples.FrameBuilder.CS
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using System.Diagnostics;

	using Autodesk.Revit;
	using Autodesk.Revit.Symbols;
	using Autodesk.Revit.Elements;
	using Autodesk.Revit.Geometry;
	using Autodesk.Revit.Structural.Enums;
	using Autodesk.Revit.Parameters;

	using ModelElement = Autodesk.Revit.Element;

	/// <summary>
	/// create columns, beams and braces to create framing
	/// </summary>
	public class FrameBuilder
	{
		FrameData m_data;		// necessary data to create frame
		Autodesk.Revit.Creation.Document m_docCreator;		// buffer of API object
		Autodesk.Revit.Creation.Application m_appCreator;	// buffer of API object

		/// <summary>
		/// constructor
		/// </summary>
		/// <param name="data">data necessary to initialize object</param>
		public FrameBuilder(FrameData data)
		{
			// initialize members
			if (null == data)
			{
				throw new ArgumentNullException("data", 
					"constructor FrameBuilder(FrameData data)'s parameter shouldn't be null ");
			}
			m_data = data;

			m_appCreator = data.CommandData.Application.Create;
			m_docCreator = data.CommandData.Application.ActiveDocument.Create;
		}

		/// <summary>
		/// create framing according to FramingData
		/// </summary>
		/// <returns>columns, beams and braces</returns>
		public void CreateFraming()
		{
			m_data.UpdateLevels();
			List<FamilyInstance> frameElems = new List<FamilyInstance>();
			UV[,] matrixUV = CreateMatrix(m_data.XNumber, m_data.YNumber, m_data.Distance);
			// iterate levels from lower one to higher one by one according to FloorNumber
			for (int ii = 0; ii < (m_data.FloorNumber - 1); ii++)	
			{
				Level baseLevel = m_data.Levels.Values[ii];
				Level topLevel = m_data.Levels.Values[ii + 1];

				int matrixXSize = matrixUV.GetLength(0);	//length of matrix's x range
				int matrixYSize = matrixUV.GetLength(1);	//length of matrix's y range

				// insert columns in an array format according to the calculated matrix
				foreach (UV point2D in matrixUV)
				{
					frameElems.Add(NewColumn(point2D, baseLevel, topLevel));
				}

				// insert beams between the tops of each adjacent column in the X and Y direction
				for (int j = 0; j < matrixYSize; j++)
				{
					for (int i = 0; i < matrixXSize; i++)
					{
						//create beams in x direction
						if (i != (matrixXSize - 1))
						{
							frameElems.Add(NewBeam(matrixUV[i, j], matrixUV[i + 1, j], topLevel));
						}
						//create beams in y direction
						if (j != (matrixYSize - 1))
						{
							frameElems.Add(NewBeam(matrixUV[i, j], matrixUV[i, j + 1], topLevel));
						}
					}
				}

				// insert braces between the mid point of each column 
				// and the mid point of each adjoining beam
				for (int j = 0; j < matrixYSize; j++)
				{
					for (int i = 0; i < matrixXSize; i++)
					{
						//create braces in x direction
						if (i != (matrixXSize - 1))
						{
							frameElems.AddRange(
								NewBraces(matrixUV[i, j], matrixUV[i + 1, j], baseLevel, topLevel));
						}
						//create braces in y direction
						if (j != (matrixYSize - 1))
						{
							frameElems.AddRange(
								NewBraces(matrixUV[i, j], matrixUV[i, j + 1], baseLevel, topLevel));
						}
					}
				}
			}

			MoveRotateFrame(frameElems);
		}

		/// <summary>
		/// constructor without parameter is forbidden
		/// </summary>
		private FrameBuilder()
		{
		}

		/// <summary>
		/// create a 2D matrix of coordinates to form an array format
		/// </summary>
		/// <param name="xNumber">number of Columns in the X direction</param>
		/// <param name="yNumber">number of Columns in the Y direction</param>
		/// <param name="distance">distance between columns</param>
		private static UV[,] CreateMatrix(int xNumber, int yNumber, double distance)
		{
			UV[,] result = new UV[xNumber, yNumber];

			for (int i = 0; i < xNumber; i++)
			{
				for (int j = 0; j < yNumber; j++)
				{
					result[i, j] = new UV(i * distance, j * distance);
				}
			}
			return result;
		}

		/// <summary>
		/// create column of certain type in given position
		/// </summary>
		/// <param name="point2D">2D coordinate of the column</param>
		/// <param name="columnType">specified type of the column</param>
		/// <param name="baseLevel">base level of the column</param>
		/// <param name="topLevel">top level of the colunm</param>
		private FamilyInstance NewColumn(UV point2D, Level baseLevel, Level topLevel)
		{
			//create column of specified type with certain level and start point 
			XYZ point = new XYZ(point2D.U, point2D.V, 0);
			FamilyInstance column = 
				m_docCreator.NewFamilyInstance(ref point, m_data.ColumnSymbol, baseLevel, StructuralType.Column);

			//set baselevel & toplevel of the column
			SetParameter(column, BuiltInParameter.FAMILY_TOP_LEVEL_OFFSET_PARAM, 0.0);
			SetParameter(column, BuiltInParameter.FAMILY_BASE_LEVEL_OFFSET_PARAM, 0.0);
			SetParameter(column, BuiltInParameter.FAMILY_TOP_LEVEL_PARAM, topLevel.Id);
			SetParameter(column, BuiltInParameter.FAMILY_BASE_LEVEL_PARAM, baseLevel.Id);

			return column;
		}

		/// <summary>
		/// create beam of certain type in given position
		/// </summary>
		/// <param name="point2D1">first point of the location line in 2D</param>
		/// <param name="point2D2">second point of the location line in 2D</param>
		/// <param name="baseLevel">base level of the beam</param>
		/// <param name="topLevel">top level of the beam</param>
		/// <returns>nothing</returns>
		private FamilyInstance NewBeam(UV point2D1, UV point2D2, Level topLevel)
		{
			// calculate the start point and end point of Beam's location line in 3D
			double height = topLevel.Elevation;
			XYZ startPoint = new XYZ(point2D1.U, point2D1.V, height);
			XYZ endPoint = new XYZ(point2D2.U, point2D2.V, height);
			// create Beam and set its location
			FamilyInstance beam = 
				m_docCreator.NewFamilyInstance(ref startPoint, m_data.BeamSymbol, topLevel, StructuralType.Beam);
			SetLocationLine(beam, startPoint, endPoint);

			return beam;
		}

		/// <summary>
		/// create 2 braces between the mid point of 2 column and the mid point of adjoining beam
		/// </summary>
		/// <param name="point2D1">first point of the location line in 2D</param>
		/// <param name="point2D2">second point of the location line in 2D</param>
		/// <param name="baseLevel">the base level of the brace</param>
		/// <param name="topLevel">the top level of the brace</param>
		private List<FamilyInstance> NewBraces(UV point2D1, UV point2D2, Level baseLevel, Level topLevel)
		{
			// calculate the start point and end point of the location lines of two braces
			double topHeight = topLevel.Elevation;
			double baseHeight = baseLevel.Elevation;
			double middleElevation = (topHeight + baseHeight) / 2;
			XYZ startPoint = new XYZ(point2D1.U, point2D1.V, middleElevation);
			XYZ endPoint = new XYZ(point2D2.U, point2D2.V, middleElevation);
			XYZ middlePoint = new XYZ((point2D1.U + point2D2.U) / 2, (point2D1.V + point2D2.V) / 2, topHeight);

			ElementId levelId = topLevel.Id;
			// create two brace; then set their location line and reference level
			FamilyInstance firstBrace = 
				m_docCreator.NewFamilyInstance(ref startPoint, m_data.BraceSymbol, StructuralType.Brace);
			SetLocationLine(firstBrace, startPoint, middlePoint);
			SetParameter(firstBrace, BuiltInParameter.INSTANCE_REFERENCE_LEVEL_PARAM, levelId);

			FamilyInstance secondBrace = 
				m_docCreator.NewFamilyInstance(ref endPoint, m_data.BraceSymbol, StructuralType.Brace);
			SetLocationLine(secondBrace, endPoint, middlePoint);
			SetParameter(secondBrace, BuiltInParameter.INSTANCE_REFERENCE_LEVEL_PARAM, levelId);

			List<FamilyInstance> result = new List<FamilyInstance>();
			result.Add(firstBrace);
			result.Add(secondBrace);
			return result;
		}

		/// <summary>
		/// set parameter whose storage type is ElementId 
		/// </summary>
		/// <param name="elem">Element has parameter</param>
		/// <param name="builtInPara">BuiltInParameter to find parameter</param>
		/// <param name="value">value to set</param>
		/// <returns>is successful</returns>
		private static bool SetParameter(ModelElement elem, 
			BuiltInParameter builtInPara, ElementId value)
		{
			Parameter para = elem.get_Parameter(builtInPara);
			if (null != para && para.StorageType == StorageType.ElementId && !para.IsReadOnly)
			{
				return para.Set(ref value);
			}
			return false;
		}

		/// <summary>
		/// set parameter whose storage type is double
		/// </summary>
		/// <param name="elem">Element has parameter</param>
		/// <param name="builtInPara">BuiltInParameter to find parameter</param>
		/// <param name="value">value to set</param>
		/// <returns>is successful</returns>
		private static bool SetParameter(ModelElement elem, 
			BuiltInParameter builtInPara, double value)
		{
			Parameter para = elem.get_Parameter(builtInPara);
			if (null != para && para.StorageType == StorageType.Double && !para.IsReadOnly)
			{
				return para.Set(value);
			}
			return false;
		}

		/// <summary>
		/// set location line to the Element
		/// </summary>
		/// <param name="elem">goal Element</param>
		/// <param name="startPnt">start point of the line</param>
		/// <param name="endPnt">end point of the line</param>
		/// <returns>is successful</returns>
		private bool SetLocationLine(ModelElement elem, XYZ startPnt, XYZ endPnt)
		{
			LocationCurve elemCurve = elem.Location as LocationCurve;
			if (null != elemCurve && !elemCurve.IsReadOnly)
			{
				try
				{
					Line line = m_appCreator.NewLineBound(ref startPnt, ref endPnt);
					elemCurve.Curve = line;
					return true;
				}
				catch (Exception ex)
				{
					Debug.WriteLine(ex.ToString());
				}
			}
			return false;
		}


		/// <summary>
		/// move and rotate the Frame
		/// </summary>
		/// <param name="frameElems">columns, beams and braces included in frame</param>
		private void MoveRotateFrame(List<FamilyInstance> frameElems)
		{
            Application app = m_data.CommandData.Application;
			Document doc = m_data.CommandData.Application.ActiveDocument;
			if (m_data.IsSuspendUpdating)
			{
				using (SuspendUpdating aSuspendUpdating = new SuspendUpdating(doc))
				{
					foreach (FamilyInstance elem in frameElems)
					{
						MoveElement(doc, elem, m_data.FrameOrigin);
                        RotateElement(app, elem, m_data.FrameOrigin, m_data.FrameOriginAngle);
					}
				}
			}
			else
			{
				foreach (FamilyInstance elem in frameElems)
				{
					MoveElement(doc, elem, m_data.FrameOrigin);
                    RotateElement(app, elem, m_data.FrameOrigin, m_data.FrameOriginAngle);
				}
			}
		}

		/// <summary>
		/// move an element in horizontal plane
		/// </summary>
		/// <param name="elem">element to be moved</param>
		/// <param name="translation2D">the 2D vector by which the element is to be moved</param>
		/// <returns>is successful</returns>
		private bool MoveElement(Document doc, Autodesk.Revit.Element elem, UV translation2D)
		{
			XYZ translation3D = new XYZ(translation2D.U, translation2D.V, 0.0);
			return doc.Move(elem, ref translation3D);
		}

		/// <summary>
		/// rotate an element a specified number of degrees 
		/// around a given center in horizontal plane
		/// </summary>
		/// <param name="elem">element to be rotated</param>
		/// <param name="center">the center of rotation</param>
		/// <param name="angle">the number of degrees, in radians, 
		/// by which the element is to be rotated around the specified axis</param>
		/// <returns>is successful</returns>
		private bool RotateElement(Application app, Autodesk.Revit.Element elem, UV center, double angle)
		{
			XYZ axisPnt1 = new XYZ(center.U, center.V, 0.0);
			XYZ axisPnt2 = new XYZ(center.U, center.V, 1.0);           
			Line axis = app.Create.NewLine(ref axisPnt1, ref axisPnt2,true);
            //axis.
			return app.ActiveDocument.Rotate(elem, axis, angle);
		}
	}
}
