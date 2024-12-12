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


namespace Revit.SDK.Samples.AreaReinParameters.CS
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


	/// <summary>
	/// can be the datasource of propertygrid
	/// </summary>
	class FloorAreaReinData : IAreaReinData
	{
		//member
		Parameter m_layoutRule;
		//top major layer
		Parameter m_topMajorBarType = null;
		Parameter m_topMajorHookType = null;
		Parameter m_topMajorHookOrientation = null;

		//top minor layer
		Parameter m_topMinorBarType = null;
		Parameter m_topMinorHookType = null;
		Parameter m_topMinorHookOrientation = null;

		//bottom major layer
		Parameter m_bottomMajorBarType = null;
		Parameter m_bottomMajorHookType = null;
		Parameter m_bottomMajorHookOrientation = null;

		//bottom minor layer
		Parameter m_bottomMinorBarType = null;
		Parameter m_bottomMinorHookType = null;
		Parameter m_bottomMinorHookOrientation = null;

		/// <summary>
		/// fill in data with given AreaReinforcement
		/// </summary>
		/// <param name="areaRein"></param>
		/// <returns></returns>
		public bool FillInData(AreaReinforcement areaRein)
		{
			bool flag = false;

			//member
			m_layoutRule = areaRein.get_Parameter(
                BuiltInParameter.REBAR_SYSTEM_LAYOUT_RULE);
			flag = (m_layoutRule != null);

			//top major layer
			m_topMajorBarType = areaRein.get_Parameter(
                BuiltInParameter.REBAR_SYSTEM_BAR_TYPE_TOP_DIR_1);
			m_topMajorHookType = areaRein.get_Parameter(
                BuiltInParameter.REBAR_SYSTEM_HOOK_TYPE_TOP_DIR_1);
			m_topMajorHookOrientation = areaRein.get_Parameter(
                BuiltInParameter.REBAR_SYSTEM_HOOK_ORIENT_TOP_DIR_1);
			flag &= (m_topMajorBarType != null) && (m_topMajorHookOrientation != null)
                && (m_topMajorHookType != null);

			//top minor layer
			m_topMinorBarType = areaRein.get_Parameter(
                BuiltInParameter.REBAR_SYSTEM_BAR_TYPE_TOP_DIR_2);
			m_topMinorHookType = areaRein.get_Parameter(
                BuiltInParameter.REBAR_SYSTEM_HOOK_TYPE_TOP_DIR_2);
			m_topMinorHookOrientation = areaRein.get_Parameter(
                BuiltInParameter.REBAR_SYSTEM_HOOK_ORIENT_TOP_DIR_2);
			flag &= (m_topMinorBarType != null) && (m_topMinorHookOrientation != null) 
                && (m_topMinorHookType != null);

			//bottom major layer
			m_bottomMajorBarType = areaRein.get_Parameter(
                BuiltInParameter.REBAR_SYSTEM_BAR_TYPE_BOTTOM_DIR_1);
			m_bottomMajorHookType = areaRein.get_Parameter(
                BuiltInParameter.REBAR_SYSTEM_HOOK_TYPE_BOTTOM_DIR_1);
			m_bottomMajorHookOrientation = areaRein.get_Parameter(
                BuiltInParameter.REBAR_SYSTEM_HOOK_ORIENT_BOTTOM_DIR_1);
			flag &= (m_bottomMajorBarType != null) && (m_bottomMajorHookOrientation != null)
                && (m_bottomMajorHookType != null);

			//bottom minor layer
			m_bottomMinorBarType = areaRein.get_Parameter(
                BuiltInParameter.REBAR_SYSTEM_BAR_TYPE_BOTTOM_DIR_2);
			m_bottomMinorHookType = areaRein.get_Parameter(
                BuiltInParameter.REBAR_SYSTEM_HOOK_TYPE_BOTTOM_DIR_2);
			m_bottomMinorHookOrientation = areaRein.get_Parameter(
                BuiltInParameter.REBAR_SYSTEM_HOOK_ORIENT_BOTTOM_DIR_2);
			flag &= (m_bottomMinorBarType != null) && (m_bottomMinorHookOrientation != null) 
                && (m_bottomMinorHookType != null);

			return flag;
		}

		/// <summary>
		/// layout rule
		/// </summary>
		[Category("Construction")]
		public LayoutRules Layout_Rule
		{
			get
			{
				int index = m_layoutRule.AsInteger();
				return (LayoutRules)index;
			}
			set
			{
				int index = (int)value;
				m_layoutRule.Set(index);
			}
		}

		#region top major layer
		[CategoryAttribute("Top Major Layer"), TypeConverter(typeof(BarTypeItem))]
		public ElementId Top_Major_Bar_Type
		{
			get
			{ 
				return m_topMajorBarType.AsElementId();
			}
			set
			{
				m_topMajorBarType.Set(ref value); 
			}
		}

		[CategoryAttribute("Top Major Layer"), TypeConverter(typeof(HookTypeItem))]
		public ElementId Top_Major_Hook_Type
		{
			get
			{ 
				return m_topMajorHookType.AsElementId();
			}
			set
			{ 
				m_topMajorHookType.Set(ref value);
			}
		}

		[CategoryAttribute("Top Major Layer")]
		public FloorHookOrientations Top_Major_Hook_Orientation
		{
			get
			{
				int index = m_topMajorHookOrientation.AsInteger();
				return (FloorHookOrientations)index;
			}
			set 
			{
				int index = (int)value;
				m_topMajorHookOrientation.Set(index);
			}
		}
		#endregion

		#region top minor layer
		[CategoryAttribute("Top Minor Layer"), TypeConverter(typeof(BarTypeItem))]
		public ElementId Top_Minor_Bar_Type
		{
			get
			{
				return m_topMinorBarType.AsElementId();
			}
			set
			{
				m_topMinorBarType.Set(ref value);
			}
		}

		[CategoryAttribute("Top Minor Layer"), TypeConverter(typeof(HookTypeItem))]
		public ElementId Top_Minor_Hook_Type
		{
			get
			{
				return m_topMinorHookType.AsElementId();
			}
			set
			{
				m_topMinorHookType.Set(ref value);
			}
		}

		[CategoryAttribute("Top Minor Layer")]
		public FloorHookOrientations Top_Minor_Hook_Orientation
		{
			get
			{
				int index = m_topMinorHookOrientation.AsInteger();
				return (FloorHookOrientations)index;
			}
			set
			{
				int index = (int)value;
				m_topMinorHookOrientation.Set(index);
			}
		}
		#endregion

		#region bottom major layer
		[CategoryAttribute("Bottom Major Layer"), TypeConverter(typeof(BarTypeItem))]
		public ElementId Bottom_Major_Bar_Type
		{
			get
			{
				return m_bottomMajorBarType.AsElementId();
			}
			set
			{
				m_bottomMajorBarType.Set(ref value);
			}
		}

		[CategoryAttribute("Bottom Major Layer"), TypeConverter(typeof(HookTypeItem))]
		public ElementId Bottom_Major_Hook_Type
		{
			get
			{
				return m_bottomMajorHookType.AsElementId();
			}
			set
			{
				m_bottomMajorHookType.Set(ref value);
			}
		}

		[CategoryAttribute("Bottom Major Layer")]
		public FloorHookOrientations Bottom_Major_Hook_Orientation
		{
			get
			{
				int index = m_bottomMajorHookOrientation.AsInteger();
				return (FloorHookOrientations)index;
			}
			set
			{
				int index = (int)value;
				m_bottomMajorHookOrientation.Set(index);
			}
		}
		#endregion

		#region bottom minor layer
		[CategoryAttribute("Bottom Minor Layer"), TypeConverter(typeof(BarTypeItem))]
		public ElementId Bottom_Minor_Bar_Type
		{
			get
			{
				return m_bottomMinorBarType.AsElementId();
			}
			set
			{
				m_bottomMinorBarType.Set(ref value);
			}
		}

		[CategoryAttribute("Bottom Minor Layer"), TypeConverter(typeof(HookTypeItem))]
		public ElementId Bottom_Minor_Hook_Type
		{
			get
			{
				return m_bottomMinorHookType.AsElementId();
			}
			set
			{
				m_bottomMinorHookType.Set(ref value);
			}
		}

		[CategoryAttribute("Bottom Minor Layer")]
		public FloorHookOrientations Bottom_Minor_Hook_Orientation
		{
			get
			{
				int index = m_bottomMinorHookOrientation.AsInteger();
				return (FloorHookOrientations)index;
			}
			set
			{
				int index = (int)value;
				m_bottomMinorHookOrientation.Set(index);
			}
		}
		#endregion

	}
}
