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
namespace Revit.SDK.Samples.CreateComplexAreaRein.CS
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using System.Windows.Forms;
	using System.ComponentModel;

	using Autodesk.Revit;
	using Autodesk.Revit.Parameters;
	using Autodesk.Revit.Elements;
	using Autodesk.Revit.Geometry;
	using Autodesk.Revit.Symbols;

	using GeoElement = Autodesk.Revit.Geometry.Element;
	using Element = Autodesk.Revit.Element;


	/// <summary>
	/// enum of AreaReinforcement's parameter Layout Rules
	/// </summary>
	public enum LayoutRules
	{
		Fixed_Number = 2,
		Maximum_Spacing = 3
	}

	/// <summary>
	/// enum of AreaReinforcementCurve's parameter Hook Orientation
	/// </summary>
	public enum HookOrientation
	{
		Up = 0,
		Down = 2
	}

	/// <summary>
	/// contain utility methods find or set certain parameter
	/// </summary>
	public class ParameterUtil
	{
		/// <summary>
		/// find certain parameter in a set
		/// </summary>
		/// <param name="paras"></param>
		/// <param name="name">find by name</param>
		/// <returns>found parameter</returns>
		public static Parameter FindParaByName(ParameterSet paras, string name)
		{
			Parameter findPara = null;

			foreach (Parameter para in paras)
			{
				if (para.Definition.Name == name)
				{
					findPara = para;
				}
			}

			return findPara;
		}

		/// <summary>
		/// set certain parameter of given element to int value
		/// </summary>
		/// <param name="elem">given element</param>
		/// <param name="paraIndex">BuiltInParameter</param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static bool SetParaInt(Element elem, BuiltInParameter paraIndex, int value)
		{
			Parameter para = elem.get_Parameter(paraIndex);
			if (null == para)
			{
				return false;
			}

			para.Set(value);
			return true;
		}
	}
}
