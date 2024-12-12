//
// (C) Copyright 2003-2014 by Autodesk, Inc.
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
    using Autodesk.Revit.DB;
    using Autodesk.Revit.DB.Structure;


    /// <summary>
    /// can be the datasource of propertygrid
    /// </summary>
    class WallAreaReinData : IAreaReinData
    {
        //member
        Parameter m_layoutRule;
        //exterior major layer
        Parameter m_exteriorMajorBarType = null;
        Parameter m_exteriorMajorHookType = null;
        Parameter m_exteriorMajorHookOrientation = null;

        //exterior minor layer
        Parameter m_exteriorMinorBarType = null;
        Parameter m_exteriorMinorHookType = null;
        Parameter m_exteriorMinorHookOrientation = null;

        //interior major layer
        Parameter m_interiorMajorBarType = null;
        Parameter m_interiorMajorHookType = null;
        Parameter m_interiorMajorHookOrientation = null;

        //interior minor layer
        Parameter m_interiorMinorBarType = null;
        Parameter m_interiorMinorHookType = null;
        Parameter m_interiorMinorHookOrientation = null;

        public bool FillInData(AreaReinforcement areaRein)
        {
            bool flag = true;

            //member
            m_layoutRule = areaRein.get_Parameter(
                BuiltInParameter.REBAR_SYSTEM_LAYOUT_RULE);
            flag = (m_layoutRule != null);

            ParameterSet paras = areaRein.Parameters;

            //exterior major layer
            m_exteriorMajorBarType = ParameterUtil.FindParaByName(paras, 
                "Exterior Major Bar Type");
            m_exteriorMajorHookType = ParameterUtil.FindParaByName(paras, 
                "Exterior Major Hook Type");
            m_exteriorMajorHookOrientation = ParameterUtil.FindParaByName(paras, 
                "Exterior Major Hook Orientation");
            flag &= (m_exteriorMajorBarType != null) && (m_exteriorMajorHookOrientation != null)
                && (m_exteriorMajorHookType != null);

            //exterior minor layer
            m_exteriorMinorBarType = ParameterUtil.FindParaByName(paras, 
                "Exterior Minor Bar Type");
            m_exteriorMinorHookType = ParameterUtil.FindParaByName(paras, 
                "Exterior Minor Hook Type");
            m_exteriorMinorHookOrientation = ParameterUtil.FindParaByName(paras, 
                "Exterior Minor Hook Orientation");
            flag &= (m_exteriorMinorBarType != null) && (m_exteriorMinorHookOrientation != null)
                && (m_exteriorMinorHookType != null);

            //interior major layer
            m_interiorMajorBarType = ParameterUtil.FindParaByName(paras, 
                "Interior Major Bar Type");
            m_interiorMajorHookType = ParameterUtil.FindParaByName(paras, 
                "Interior Major Hook Type");
            m_interiorMajorHookOrientation = ParameterUtil.FindParaByName(paras, 
                "Interior Major Hook Orientation");
            flag &= (m_interiorMajorBarType != null) && (m_interiorMajorHookOrientation != null)
                && (m_interiorMajorHookType != null);

            //interior minor layer
            m_interiorMinorBarType = ParameterUtil.FindParaByName(paras, 
                "Interior Minor Bar Type");
            m_interiorMinorHookType = ParameterUtil.FindParaByName(paras, 
                "Interior Minor Hook Type");
            m_interiorMinorHookOrientation = ParameterUtil.FindParaByName(paras, 
                "Interior Minor Hook Orientation");
            flag &= (m_interiorMinorBarType != null) && (m_interiorMinorHookOrientation != null)
                && (m_interiorMinorHookType != null);

            return flag;
        }

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

        #region exterior major layer
        [CategoryAttribute("Exterior Major Layers"), TypeConverter(typeof(BarTypeItem))]
        public Autodesk.Revit.DB.ElementId Exterior_Major_Bar_Type
        {
            get
            {
                return m_exteriorMajorBarType.AsElementId();
            }
            set
            {
                m_exteriorMajorBarType.Set(value);
            }
        }

        [CategoryAttribute("Exterior Major Layers"), TypeConverter(typeof(HookTypeItem))]
        public Autodesk.Revit.DB.ElementId Exterior_Major_Hook_Type
        {
            get
            {
                return m_exteriorMajorHookType.AsElementId();
            }
            set
            {
                m_exteriorMajorHookType.Set(value);
            }
        }

        [CategoryAttribute("Exterior Major Layers")]
        public WallHookOrientations Exterior_Major_Hook_Orientation
        {
            get
            {
                int index = m_exteriorMajorHookOrientation.AsInteger();
                return (WallHookOrientations)index;
            }
            set
            {
                int index = (int)value;
                m_exteriorMajorHookOrientation.Set(index);
            }
        }
        #endregion

        #region exterior minor layer
        [CategoryAttribute("Exterior Minor Layers"), TypeConverter(typeof(BarTypeItem))]
        public Autodesk.Revit.DB.ElementId Exterior_Minor_Bar_Type
        {
            get
            {
                return m_exteriorMinorBarType.AsElementId();
            }
            set
            {
                m_exteriorMinorBarType.Set(value);
            }
        }

        [CategoryAttribute("Exterior Minor Layers"), TypeConverter(typeof(HookTypeItem))]
        public Autodesk.Revit.DB.ElementId Exterior_Minor_Hook_Type
        {
            get
            {
                return m_exteriorMinorHookType.AsElementId();
            }
            set
            {
                m_exteriorMinorHookType.Set(value);
            }
        }

        [CategoryAttribute("Exterior Minor Layers")]
        public WallHookOrientations Exterior_Minor_Hook_Orientation
        {
            get
            {
                int index = m_exteriorMinorHookOrientation.AsInteger();
                return (WallHookOrientations)index;
            }
            set
            {
                int index = (int)value;
                m_exteriorMinorHookOrientation.Set(index);
            }
        }
        #endregion

        #region interior major layer
        [CategoryAttribute("Interior Major Layers"), TypeConverter(typeof(BarTypeItem))]
        public Autodesk.Revit.DB.ElementId Interior_Major_Bar_Type
        {
            get
            {
                return m_interiorMajorBarType.AsElementId();
            }
            set
            {
                m_interiorMajorBarType.Set(value);
            }
        }

        [CategoryAttribute("Interior Major Layers"), TypeConverter(typeof(HookTypeItem))]
        public Autodesk.Revit.DB.ElementId Interior_Major_Hook_Type
        {
            get
            {
                return m_interiorMajorHookType.AsElementId();
            }
            set
            {
                m_interiorMajorHookType.Set(value);
            }
        }

        [CategoryAttribute("Interior Major Layers")]
        public WallHookOrientations Interior_Major_Hook_Orientation
        {
            get
            {
                int index = m_interiorMajorHookOrientation.AsInteger();
                return (WallHookOrientations)index;
            }
            set
            {
                int index = (int)value;
                m_interiorMajorHookOrientation.Set(index);
            }
        }
        #endregion

        #region interior minor layer
        [CategoryAttribute("Interior Minor Layers"), TypeConverter(typeof(BarTypeItem))]
        public Autodesk.Revit.DB.ElementId Interior_Minor_Bar_Type
        {
            get
            {
                return m_interiorMinorBarType.AsElementId();
            }
            set
            {
                m_interiorMinorBarType.Set(value);
            }
        }

        [CategoryAttribute("Interior Minor Layers"), TypeConverter(typeof(HookTypeItem))]
        public Autodesk.Revit.DB.ElementId Interior_Minor_Hook_Type
        {
            get
            {
                return m_interiorMinorHookType.AsElementId();
            }
            set
            {
                m_interiorMinorHookType.Set(value);
            }
        }

        [CategoryAttribute("Interior Minor Layers"),]
        public WallHookOrientations Interior_Minor_Hook_Orientation
        {
            get
            {
                int index = m_interiorMinorHookOrientation.AsInteger();
                return (WallHookOrientations)index;
            }
            set
            {
                int index = (int)value;
                m_interiorMinorHookOrientation.Set(index);
            }
        }
        #endregion

    }
}
