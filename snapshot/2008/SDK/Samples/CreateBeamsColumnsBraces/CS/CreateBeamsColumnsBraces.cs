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
using System.IO;
using System.Collections;
using System.Windows.Forms;

using Autodesk.Revit;
using Autodesk.Revit.Parameters;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Collections;
using Autodesk.Revit.Events;
using Autodesk.Revit.Symbols;
using Autodesk.Revit.Structural;
using Autodesk.Revit.Structural.Enums;
using Autodesk.Revit.Creation;
using Autodesk.Revit.Geometry;

using ELEMENT        = Autodesk.Revit.Element;
using STRUCTURALTYPE = Autodesk.Revit.Structural.Enums.StructuralType;

namespace Revit.SDK.Samples.CreateBeamsColumnsBraces.CS
{
	/// <summary>
	/// Create Beams, Columns and Braces according to user's input information
	/// </summary>
	public class Command : IExternalCommand
	{
		Autodesk.Revit.Application m_revit = null;

		ArrayList m_columnMaps = new ArrayList();		//list of columns' type
		ArrayList m_beamMaps   = new ArrayList();		//list of beams' type
		ArrayList m_braceMaps  = new ArrayList();		//list of braces' type
		SortedList levels      = new SortedList();		//list of list sorted by their elevations

		UV[,] m_matrixUV;		//2D coordinates of matrix

		/// <summary>
		/// list of all type of columns
		/// </summary>
		public ArrayList ColumnMaps
		{
			get
			{
				return m_columnMaps;
			}
		}

		/// <summary>
		/// list of all type of beams
		/// </summary>
		public ArrayList BeamMaps
		{
			get
			{
				return m_beamMaps;
			}
		}

		/// <summary>
		/// list of all type of braces
		/// </summary>
		public ArrayList BraceMaps
		{
			get
			{
				return m_braceMaps;
			}
		}

        /// <summary>
        /// Implement this method as an external command for Revit.
        /// </summary>
        /// <param name="revit">An object that is passed to the external application 
        /// which contains data related to the command, 
        /// such as the application object and active view.</param>
        /// <param name="message">A message that can be set by the external application 
        /// which will be displayed if a failure or cancellation is returned by 
        /// the external command.</param>
        /// <param name="elements">A set of elements to which the external application 
        /// can add elements that are to be highlighted in case of failure or cancellation.</param>
        /// <returns>Return the status of the external command. 
        /// A result of Succeeded means that the API external method functioned as expected. 
        /// Cancelled can be used to signify that the user cancelled the external operation 
        /// at some point. Failure should be returned if the application is unable to proceed with 
        /// the operation.</returns>
        public IExternalCommand.Result Execute(Autodesk.Revit.ExternalCommandData revit, ref string message, ElementSet elements)
		{
			m_revit = revit.Application;	

			//if initialize failed return Result.Failed
			bool initializeOK = Initialize();
			if (!initializeOK)
			{
				return Autodesk.Revit.IExternalCommand.Result.Failed;
			}

            using (CreateBeamsColumnsBracesForm displayForm = new CreateBeamsColumnsBracesForm(this))
            {
                if (displayForm.ShowDialog() != DialogResult.OK)
                {
                    return Autodesk.Revit.IExternalCommand.Result.Cancelled;
                }
            }

			return Autodesk.Revit.IExternalCommand.Result.Succeeded;
		}

		/// <summary>
		/// check the number of floors is less than the number of levels
		/// create beams, columns and braces according to selected types
		/// </summary>
        /// <param name="columnObject">type of column</param>
        /// <param name="beamObject">type of beam</param>
        /// <param name="braceObject">type of brace</param>
		/// <param name="floorNumber">number of floor</param>
		/// <returns>number of floors is less than the number of levels and create successfully then return true</returns>
		public bool AddInstance(object columnObject, object beamObject, object braceObject, int floorNumber)
		{
			//whether floor number less than levels number
			if (floorNumber >= levels.Count)
			{
				MessageBox.Show("The number of levels must be added.","Revit");
				return false;
			}

			FamilySymbol columnSymbol = columnObject as FamilySymbol;
			FamilySymbol beamSymbol   = beamObject as FamilySymbol;
			FamilySymbol braceSymbol  = braceObject as FamilySymbol;

			//any symbol is null then the command failed
			if (null == columnSymbol || null == beamSymbol || null == braceSymbol)
			{
				return false;
			}

			try
			{
				for (int k = 0; k < floorNumber; k++)	//iterate levels from lower one to higher
				{
					Level baseLevel = levels.GetByIndex(k) as Level;
					Level topLevel  = levels.GetByIndex(k + 1) as Level;

					int matrixXSize = m_matrixUV.GetLength(0);	//length of matrix's x range
					int matrixYSize = m_matrixUV.GetLength(1);	//length of matrix's y range

					//iterate coordinate both in x direction and y direction and create beams and braces
					for(int j = 0; j < matrixYSize; j++)
					{
						for(int i = 0; i < matrixXSize; i++)
						{
							//create beams and braces in x direction
							if(i != (matrixXSize - 1))
							{
								PlaceBrace(m_matrixUV[i,j], m_matrixUV[i+1, j], baseLevel, topLevel, beamSymbol, true);
							}
							//create beams and braces in y direction
							if(j != (matrixYSize - 1))
							{
								PlaceBrace(m_matrixUV[i,j], m_matrixUV[i, j+1], baseLevel, topLevel, beamSymbol, false);
							}
						}
					}
					for(int j = 0; j < matrixYSize; j++)
					{
						for(int i = 0; i < matrixXSize; i++)
						{
							//create beams and braces in x direction
							if(i != (matrixXSize - 1))
							{
								PlaceBeam(m_matrixUV[i,j], m_matrixUV[i+1, j], baseLevel, topLevel, beamSymbol);
							}
							//create beams and braces in y direction
							if(j != (matrixYSize - 1))
							{
								PlaceBeam(m_matrixUV[i,j], m_matrixUV[i, j+1], baseLevel, topLevel, beamSymbol);
							}
						}
					}
					//place column of this level
					foreach(UV point2D in m_matrixUV)
					{
						PlaceColumn(point2D, columnSymbol, baseLevel, topLevel);
					}
				}
			}
			catch
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// generate 2D coordinates of matrix according to parameters
		/// </summary>
		/// <param name="xNumber">Number of Columns in the X direction</param>
		/// <param name="yNumber">Number of Columns in the Y direction</param>
		/// <param name="distance">Distance between columns</param>
		public void CreateMatrix(int xNumber, int yNumber, double distance)
		{
			m_matrixUV = new UV[xNumber, yNumber];

			for(int i = 0; i < xNumber; i++)
			{
				for(int j = 0; j < yNumber; j++)
				{
					m_matrixUV[i, j] = new UV(i * distance, j * distance);
				}
			}
		}
	
		/// <summary>
		/// iterate all the symbols of levels, columns, beams and braces
		/// </summary>
		/// <returns>A value that signifies if the initialization was successful for true or failed for false</returns>
		private bool Initialize()
		{
			try
			{
				ElementIterator i = m_revit.ActiveDocument.Elements;
				i.Reset();
				bool moreElement = i.MoveNext();
				while (moreElement)
				{
					object o = i.Current;

					//add level to list
					Level level = o as Level;
					if (null != level)
					{
						levels.Add(level.Elevation, level);
						goto nextLoop;
					}

					Family f = o as Family;
					if (null == f)
					{
						goto nextLoop;
					}

					foreach (object symbol in f.Symbols)
					{
						FamilySymbol familyType = symbol as FamilySymbol;
						if (null == familyType)
						{
							goto nextLoop;
						}
						if (null == familyType.Category)
						{
							goto nextLoop;
						}

						//add symbols of beams and braces to lists 
						string categoryName = familyType.Category.Name;
						if ("Structural Framing" == categoryName)
						{
							m_beamMaps.Add(new SymbolMap(familyType));
							m_braceMaps.Add(new SymbolMap(familyType));
						}
						else if ("Structural Columns" == categoryName)
						{
							m_columnMaps.Add(new SymbolMap(familyType));
						}
					}
				nextLoop:
					moreElement = i.MoveNext();
				}
			}
			catch
			{
				return false;
			}
			return true;
		}

		/// <summary>
		/// create column of certain type in certain position
		/// </summary>
		/// <param name="point2D">2D coordinate of the column</param>
		/// <param name="columnType">type of column</param>
		/// <param name="baseLevel">the base level of the column</param>
		/// <param name="topLevel">the top level of the colunm</param>
		private void PlaceColumn(UV point2D, FamilySymbol columnType, Level baseLevel, Level topLevel)
		{
			//create column of certain type in certain level and start point 
			XYZ point = new XYZ(point2D.U, point2D.V, 0);
			STRUCTURALTYPE structuralType;
			structuralType = Autodesk.Revit.Structural.Enums.StructuralType.Column;
			FamilyInstance column = m_revit.ActiveDocument.Create.NewFamilyInstance(ref point, columnType, topLevel, structuralType);

			//set baselevel & toplevel of the column
			if (null != column)
			{
				Parameter baseLevelParameter = column.get_Parameter(Autodesk.Revit.Parameters.BuiltInParameter.FAMILY_BASE_LEVEL_PARAM);
				Parameter topLevelParameter = column.get_Parameter(Autodesk.Revit.Parameters.BuiltInParameter.FAMILY_TOP_LEVEL_PARAM);
				Parameter topOffsetParameter = column.get_Parameter(BuiltInParameter.FAMILY_TOP_LEVEL_OFFSET_PARAM);
				Parameter baseOffsetParameter = column.get_Parameter(BuiltInParameter.FAMILY_BASE_LEVEL_OFFSET_PARAM);

				if (null != baseLevelParameter)
				{
					Autodesk.Revit.ElementId baseLevelId;
					baseLevelId = baseLevel.Id;
					baseLevelParameter.Set(ref baseLevelId);
				}

				if (null != topLevelParameter)
				{
					Autodesk.Revit.ElementId topLevelId;
					topLevelId = topLevel.Id;
					topLevelParameter.Set(ref topLevelId);
				}

				if (null != topOffsetParameter)
				{
					topOffsetParameter.Set(0.0);
				}

				if (null != baseOffsetParameter)
				{
					baseOffsetParameter.Set(0.0);
				}
			}
		}

		/// <summary>
		/// create beam of certain type in certain position
		/// </summary>
		/// <param name="point2D1">one point of the location line in 2D</param>
		/// <param name="point2D2">another point of the location line in 2D</param>
		/// <param name="baseLevel">the base level of the beam</param>
		/// <param name="topLevel">the top level of the beam</param>
		/// <param name="beamType">type of beam</param>
		/// <returns>nothing</returns>
		private void PlaceBeam(UV point2D1, UV point2D2, Level baseLevel, Level topLevel, FamilySymbol beamType)
		{
			double height	     = topLevel.Elevation;
			XYZ startPoint	     = new XYZ(point2D1.U, point2D1.V, height);
			XYZ endPoint	     = new XYZ(point2D2.U, point2D2.V, height);
			ElementId topLevelId = topLevel.Id;
        
            STRUCTURALTYPE structuralType = Autodesk.Revit.Structural.Enums.StructuralType.Beam;
			FamilyInstance beam = m_revit.ActiveDocument.Create.NewFamilyInstance(ref startPoint, beamType, topLevel, structuralType);

			LocationCurve beamCurve = beam.Location as LocationCurve;
			if (null != beamCurve)
			{
				Line line = m_revit.Create.NewLineBound(ref startPoint, ref endPoint);
				beamCurve.Curve = line;
			}
		}

		/// <summary>
		/// create brace of certain type in certain position between two adjacent columns
		/// </summary>
		/// <param name="point2D1">one point of the location line in 2D</param>
		/// <param name="point2D2">another point of the location line in 2D</param>
		/// <param name="baseLevel">the base level of the brace</param>
		/// <param name="topLevel">the top level of the brace</param>
        /// <param name="braceType">type of beam</param>
		/// <param name="isXDirection">whether the location line is in x direction</param>
		private void PlaceBrace(UV point2D1, UV point2D2, Level baseLevel, Level topLevel, FamilySymbol braceType, bool isXDirection)
		{
			//get the start points and end points of location lines of two braces
			double topHeight       = topLevel.Elevation;
			double baseHeight      = baseLevel.Elevation;
			double middleElevation = (topHeight + baseHeight) / 2;
			double middleHeight    = (topHeight - baseHeight) / 2;
			XYZ startPoint         = new XYZ(point2D1.U, point2D1.V, middleElevation);
			XYZ endPoint           = new XYZ(point2D2.U, point2D2.V, middleElevation);
			XYZ middlePoint;

			if (isXDirection)
			{
				middlePoint = new XYZ((point2D1.U + point2D2.U) / 2, point2D2.V, topHeight);
			}
			else
			{
				middlePoint = new XYZ(point2D2.U, (point2D1.V + point2D2.V) / 2, topHeight);
			}

			//create two brace and set their location line
            STRUCTURALTYPE structuralType = Autodesk.Revit.Structural.Enums.StructuralType.Brace;
			ElementId levelId             = topLevel.Id;
			ElementId startLevelId        = baseLevel.Id;
			ElementId endLevelId          = topLevel.Id;

			FamilyInstance firstBrace = m_revit.ActiveDocument.Create.NewFamilyInstance(ref startPoint, braceType, structuralType);
			LocationCurve braceCurve1 = firstBrace.Location as LocationCurve;
			if (null != braceCurve1)
			{
				Line line = m_revit.Create.NewLineBound(ref startPoint, ref middlePoint);
				braceCurve1.Curve = line;
			}

			Parameter referenceLevel1 = firstBrace.get_Parameter(BuiltInParameter.INSTANCE_REFERENCE_LEVEL_PARAM);
			if (null != referenceLevel1)
			{
				referenceLevel1.Set(ref levelId);
			}
			
			FamilyInstance secondBrace = m_revit.ActiveDocument.Create.NewFamilyInstance(ref endPoint, braceType, baseLevel, structuralType);
			LocationCurve braceCurve2  = secondBrace.Location as LocationCurve;
			if (null != braceCurve2)
			{
				Line line = m_revit.Create.NewLineBound(ref endPoint, ref middlePoint);
				braceCurve2.Curve = line;
			}

			Parameter referenceLevel2 = secondBrace.get_Parameter(BuiltInParameter.INSTANCE_REFERENCE_LEVEL_PARAM);
			if (null != referenceLevel2)
			{
				referenceLevel2.Set(ref levelId);
			}
		}
	}

	/// <summary>
	/// assistant class contains symbol and it's name
	/// </summary>
	public class SymbolMap
	{
		string m_symbolName   = "";
		FamilySymbol m_symbol = null;

		/// <summary>
		/// constructor without parameter is forbidden
		/// </summary>
		private SymbolMap()
		{
		}

		/// <summary>
		/// constructor
		/// </summary>
		/// <param name="symbol">family symbol</param>
		public SymbolMap(FamilySymbol symbol)
		{
			m_symbol          = symbol;
			string familyName = "";
			if (null != symbol.Family)
			{
				familyName = symbol.Family.Name;
			}
			m_symbolName = familyName + " : " + symbol.Name;
		}

		/// <summary>
		/// SymbolName property
		/// </summary>
		public string SymbolName
		{
			get
			{
				return m_symbolName;
			}
		}
		/// <summary>
		/// ElementType property
		/// </summary>
		public FamilySymbol ElementType
		{
			get
			{
				return m_symbol;
			}
		}
	}
}