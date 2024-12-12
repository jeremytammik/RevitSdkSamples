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

	using DocCreator = Autodesk.Revit.Creation.Document;


	/// <summary>
	/// main class to create simple AreaReinforcement on selected wall or floor
	/// </summary>
	public class Command : IExternalCommand
	{
		private Document m_currentDoc;
		private static ExternalCommandData m_revit;
		private AreaReinData m_data;

        ///<summary>
        /// Implement this method as an external command for Revit.
        /// </summary>
        /// <param name="commandData">An object that is passed to the external application 
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
		public IExternalCommand.Result Execute(ExternalCommandData revit, 
            ref string message, ElementSet elements)
		{
			//initialize necessary data
			m_revit = revit;
			m_currentDoc = revit.Application.ActiveDocument;
			if (!Initialize())
			{
				return IExternalCommand.Result.Failed;
			}

			//create AreaReinforcement
			try
			{
				if (Create())
				{
					return IExternalCommand.Result.Succeeded;
				}
			}
			catch (ApplicationException appEx)
			{
				MessageBox.Show(appEx.Message);
				return IExternalCommand.Result.Failed;
			}
			catch
			{
				MessageBox.Show("Unknow Errors.");
				return IExternalCommand.Result.Failed;
			}
			return IExternalCommand.Result.Cancelled;
		}

		/// <summary>
		/// ExternalCommandData
		/// </summary>
		public static ExternalCommandData CommandData
		{
			get
			{
				return m_revit;
			}
		}

		/// <summary>
		/// initialize member data, judge simple precondition
		/// </summary>
		private bool Initialize()
		{
			m_data = new AreaReinData();

			//iterate all AreaReinforcement Type in current project
			ElementIterator itor = m_currentDoc.Elements;
			itor.Reset();
			while (itor.MoveNext())
			{
				AreaReinforcementType symbol = itor.Current as AreaReinforcementType;
				if (null != symbol)
				{
					m_data.AreaReinTypes.Add(symbol);
				}
			}

			//no AreaReinForcementSymbol in current project
			if (m_data.AreaReinTypes.Count == 0)
			{
				MessageBox.Show(
                    "No Family of AreaReinForcement has been loaded in current project.", "Error");
				return false;
			}

			return true;
		}

		/// <summary>
		/// create simple AreaReinforcement on selected wall or floor
		/// </summary>
		/// <returns></returns>
		private bool  Create()
		{
			ElementSet elems = m_currentDoc.Selection.Elements;

			//selected 0 or more than 1 element
			if (elems.Size != 1)
			{
				MessageBox.Show("Please selecte exactly one wall or floor.", "Error");
				return false;
			}
			foreach (object o in elems)
			{
				//create on floor
				Floor floor = o as Floor;
				if (null != floor)
				{
					bool flag = CreateAreaReinOnFloor(floor);
					return flag;
				}

				//create on wall
				Wall wall = o as Wall;
				if (null != wall)
				{
					bool flag = CreateAreaReinOnWall(wall);
					return flag;
				}

				//selected element is neither wall nor floor
				MessageBox.Show("Please selecte exactly one wall or floor.", "Error");
			}
			return false;
		}

		/// <summary>
		/// create simple AreaReinforcement on horizontal floor
		/// </summary>
		/// <param name="floor"></param>
		/// <returns>is successful</returns>
		private bool CreateAreaReinOnFloor(Floor floor)
		{
			GeomHelper helper = new GeomHelper();
			Reference refer = null;
			CurveArray curves = null;

			//check whether floor is horizontal rectangular 
            //and prepare necessary to create AreaReinforcement
			if (!helper.GetFloorGeom(floor, ref refer, ref curves))
			{
				ApplicationException appEx = new ApplicationException(
                    "Your selection is not a horizontal rectangular sab.");
				throw appEx;
			}

			AreaReinDataOnFloor dataOnFloor = new AreaReinDataOnFloor();
			dataOnFloor.AreaReinTypes = m_data.AreaReinTypes;
			CreateSimpleAreaReinForm createForm = 
                new CreateSimpleAreaReinForm(dataOnFloor);

			//allow use select parameters to create
			if (createForm.ShowDialog() == DialogResult.OK)
			{
				AreaReinforcementType symbol = 
                    dataOnFloor.AreaReinType as AreaReinforcementType;

                //define the Major Direction of AreaReinforcement,
                //we get direction of first Line on the Floor as the Major Direction
                Line firstLine = (Line)(curves.get_Item(0));
                XYZ majorDirection = new XYZ(
                    firstLine.get_EndPoint(1).X - firstLine.get_EndPoint(0).X,
                    firstLine.get_EndPoint(1).Y - firstLine.get_EndPoint(0).Y,
                    firstLine.get_EndPoint(1).Z - firstLine.get_EndPoint(0).Z);

                //crete AreaReinforcement by NewAreaReinforcement function
				DocCreator creator = m_revit.Application.ActiveDocument.Create;
				AreaReinforcement areaRein = creator.NewAreaReinforcement(symbol, floor, curves, majorDirection);

                //set AreaReinforcement and it's AreaReinforcementCurves parameters
				dataOnFloor.FillIn(areaRein);
				return true;
			}
			return false;
		}

		/// <summary>
		/// create simple AreaReinforcement on vertical straight rectangular wall
		/// </summary>
		/// <param name="wall"></param>
		/// <returns>is successful</returns>
		private bool CreateAreaReinOnWall(Wall wall)
		{
			//make sure selected is basic wall
			if (wall.WallType.Kind != WallType.WallKind.Basic)
			{
				MessageBox.Show("Selected wall is not a basic wall.");
				return false;
			}

			GeomHelper helper = new GeomHelper();
			Reference refer = null;
			CurveArray curves = null;
			//check whether wall is vertical rectangular and analytical model shape is line
			if (!helper.GetWallGeom(wall, ref refer, ref curves))
			{
				ApplicationException appEx = new ApplicationException(
                    "Your selection is not a structural straight rectangular wall.");
				throw appEx;
			}

			AreaReinDataOnWall dataOnWall = new AreaReinDataOnWall();
			dataOnWall.AreaReinTypes = m_data.AreaReinTypes;
			CreateSimpleAreaReinForm createForm = new 
                CreateSimpleAreaReinForm(dataOnWall);

			//allow use select parameters to create
			if (createForm.ShowDialog() == DialogResult.OK)
			{
				AreaReinforcementType symbol = 
                    dataOnWall.AreaReinType as AreaReinforcementType;
				DocCreator creator = m_revit.Application.ActiveDocument.Create;

                //define the Major Direction of AreaReinforcement,
                //we get direction of first Line on the Floor as the Major Direction
                Line firstLine = (Line)(curves.get_Item(0));
                XYZ majorDirection = new XYZ(
                    firstLine.get_EndPoint(1).X - firstLine.get_EndPoint(0).X,
                    firstLine.get_EndPoint(1).Y - firstLine.get_EndPoint(0).Y,
                    firstLine.get_EndPoint(1).Z - firstLine.get_EndPoint(0).Z);

                //create AreaReinforcement by NewAreaReinforcement function
				AreaReinforcement areaRein = creator.NewAreaReinforcement(symbol, wall, curves, majorDirection);
				dataOnWall.FillIn(areaRein);
				return true;
			}
			return false;
		}
	}
}
