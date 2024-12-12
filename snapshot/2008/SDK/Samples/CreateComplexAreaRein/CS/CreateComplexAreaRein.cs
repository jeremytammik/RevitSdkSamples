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
namespace Revit.SDK.Samples.CreateComplexAreaRein.CS
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
			//initialize members
			m_revit = revit;
			m_currentDoc = revit.Application.ActiveDocument;
			m_data = new AreaReinData();

			try
			{
				//check precondition and prepare necessary data to create AreaReinforcement.
				Reference refer = null;
				CurveArray curves = null;
				Floor floor = InitFloor(ref refer, ref curves);
				InitAreaReinType();

				//ask for user's input
				AreaReinData dataOnFloor = new AreaReinData();
				dataOnFloor.AreaReinTypes = m_data.AreaReinTypes;
				CreateComplexAreaReinForm createForm = new 
                    CreateComplexAreaReinForm(dataOnFloor);
				if (createForm.ShowDialog() == DialogResult.OK)
				{
					//create
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
					AreaReinforcement areaRein = creator.NewAreaReinforcement(symbol, 
                        floor, curves, majorDirection);

					//set AreaReinforcement and it's AreaReinforcementCurves parameters
					dataOnFloor.FillIn(areaRein);
					return IExternalCommand.Result.Succeeded;
				}
			}
			catch (ApplicationException appEx)
			{
				message = appEx.Message;
				return IExternalCommand.Result.Failed;
			}
			catch
			{
				message = "Unknow Errors.";
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
		/// iterate all AreaReinforcementTypes in current project
		/// </summary>
		private void InitAreaReinType()
		{
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
				string msg = "No Family of AreaReinForcement has been loaded in current project. ";
                msg += "You should draw an AreaReinforcement or Load a family in.";
				ApplicationException appEx = new ApplicationException(msg);
				throw appEx;
			}
		}

		/// <summary>
		/// initialize member data, judge simple precondition
		/// </summary>
		private Floor InitFloor(ref Reference refer, ref CurveArray curves)
		{
			ElementSet elems = m_currentDoc.Selection.Elements;
			//selected 0 or more than 1 element
			if (elems.Size != 1)
			{
                string msg = "Please select exactly one slab.";
				ApplicationException appEx = new ApplicationException(msg);
				throw appEx;
			}
			Floor floor = null;
			foreach (object o in elems)
			{
				//selected one floor
				floor = o as Floor;
				if (null == floor)
				{
                    string msg = "Please select exactly one slab.";
					ApplicationException appEx = new ApplicationException(msg);
					throw appEx;	
				}
			}
			//check the shape is rectangular and get its edges
			GeomHelper helper = new GeomHelper();
			if (!helper.GetFloorGeom(floor, ref refer, ref curves))
			{
				ApplicationException appEx = new 
                    ApplicationException(
                    "Your selection is not a structural rectangular horizontal slab.");
				throw appEx;
			}

			return floor;
		}
	}
}
