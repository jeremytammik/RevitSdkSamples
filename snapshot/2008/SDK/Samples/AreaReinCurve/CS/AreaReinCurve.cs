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
namespace Revit.SDK.Samples.AreaReinCurve.CS
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

    public class Command : IExternalCommand
	{
		AreaReinforcement m_areaRein;
		List<AreaReinforcementCurve> m_areaReinCurves;

		/// <summary>
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
			ElementSet selected = revit.Application.ActiveDocument.Selection.Elements;

			try
			{
				//selected is not one rectangular AreaReinforcement
				if (!PreData(selected))
				{
					message = "Please select only one rectangular AreaReinforcement.";
					return IExternalCommand.Result.Failed;
				}

				//fail to turn off layers
				if (!TurnOffLayers())
				{
					message = "Can't turn off layers as expected or can't find these layers.";
					return IExternalCommand.Result.Failed;
				}

				//fail to remove hooks
				if (!ChangeHookType())
				{
					message = "Can't remove HookTypes as expected.";
					return IExternalCommand.Result.Failed;
				}
			}
			catch (ApplicationException appEx)
			{
				message = appEx.ToString();
				return IExternalCommand.Result.Failed;
			}
			catch
			{
				message = "Unexpected error happens.";
				return IExternalCommand.Result.Failed;
			}

			//command is successful
            string msg = "All layers but Major Direction Layer or Exterior Direction Layer ";
            msg += "have been turn off; ";
            msg += "Removed the Hooks from one boundary curve of the Major Direction Layer ";
            msg += "or Exterior Direction Layer.";
			MessageBox.Show(msg, "Revit");

			return IExternalCommand.Result.Succeeded;
		}

		/// <summary>
		/// check whether the selected is expected, prepare necessary data
		/// </summary>
		/// <param name="selected">selected elements</param>
		/// <returns>whether the selected AreaReinforcement is expected</returns>
		private bool PreData(ElementSet selected)
		{
			//selected is not only one AreaReinforcement
			if (selected.Size != 1)
			{
				return false;
			}
			foreach (Object o in selected)
			{
				m_areaRein = o as AreaReinforcement;
				if (null == m_areaRein)
				{
					return false;
				}
			}

			//whether the selected AreaReinforcement is rectangular
			CurveArray curves = new CurveArray();
			m_areaReinCurves = new List<AreaReinforcementCurve>();
			foreach (object o in m_areaRein.Curves)
			{
				AreaReinforcementCurve areaCurve = o as AreaReinforcementCurve;
				if (null == areaCurve)
				{
					ApplicationException appEx = new ApplicationException
                        ("There is unexpected error with selected AreaReinforcement.");
					throw appEx;
				}
				m_areaReinCurves.Add(areaCurve);
				curves.Append(areaCurve.Curve);
			}
			bool flag = GeomUtil.IsRectangular(curves);

			return flag;
		}

		/// <summary>
		/// turn off all layers but the Major Direction Layer or Exterior Direction Layer
		/// </summary>
		/// <returns>whether the command is successful</returns>
		private bool TurnOffLayers()
		{
			//AreaReinforcement is on the floor or slab
			bool flag = true;
			flag = ParameterUtil.SetParaInt(m_areaRein, 
                BuiltInParameter.REBAR_SYSTEM_ACTIVE_BOTTOM_DIR_1, 0);
			flag &= ParameterUtil.SetParaInt(m_areaRein, 
                BuiltInParameter.REBAR_SYSTEM_ACTIVE_BOTTOM_DIR_2, 0);
			flag &= ParameterUtil.SetParaInt(m_areaRein, 
                BuiltInParameter.REBAR_SYSTEM_ACTIVE_TOP_DIR_2, 0);

			//AreaReinforcement is on the wall
			if (!flag)
			{
				flag = true;
				flag &= ParameterUtil.SetParaInt(m_areaRein, "Interior Major Direction", 0);
				flag &= ParameterUtil.SetParaInt(m_areaRein, "Exterior Minor Direction", 0);
				flag &= ParameterUtil.SetParaInt(m_areaRein, "Interior Minor Direction", 0);
			}

			return flag;
		}

		/// <summary>
		/// remove the hooks from one boundary curve of the Major Direction Layer 
        /// or Exterior Direction Layer
		/// </summary>
		/// <returns>whether the command is successful</returns>
		private bool ChangeHookType()
		{
			//find two vertical AreaReinforcementCurve
			Line line0 = m_areaReinCurves[0].Curve as Line;
			Line line1 = m_areaReinCurves[1].Curve as Line;
			Line line2 = m_areaReinCurves[2].Curve as Line;
			AreaReinforcementCurve temp = null;
			if (GeomUtil.IsVertical(line0, line1))
			{
				temp = m_areaReinCurves[1];
			}
			else
			{
				temp = m_areaReinCurves[2];
			}

			//remove hooks
			ParameterUtil.SetParaInt(m_areaReinCurves[0], 
                BuiltInParameter.REBAR_SYSTEM_OVERRIDE, -1);
			Parameter para = m_areaReinCurves[0].get_Parameter(
                BuiltInParameter.REBAR_SYSTEM_HOOK_TYPE_TOP_DIR_1);
			bool flag = ParameterUtil.SetParaNullId(para);

			ParameterUtil.SetParaInt(temp, BuiltInParameter.REBAR_SYSTEM_OVERRIDE, -1);
			para = temp.get_Parameter(
                BuiltInParameter.REBAR_SYSTEM_HOOK_TYPE_TOP_DIR_1);
			flag &= ParameterUtil.SetParaNullId(para);

			return flag;
		}
	}
}
