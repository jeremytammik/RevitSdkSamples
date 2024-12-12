//
// (C) Copyright 2003-2010 by Autodesk, Inc.
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
    using Autodesk.Revit.DB;
    using Autodesk.Revit.DB.Structure;

    using GeoElement = Autodesk.Revit.DB.GeometryElement;
    using Element = Autodesk.Revit.DB.Element;


    /// <summary>
    /// data and data manager of the AreaReinforcement
    /// </summary>
    public class AreaReinData
    {
        /// <summary>
        /// constructor
        /// </summary>
        public AreaReinData()
        {}

        private LayoutRules m_layoutRule = LayoutRules.Maximum_Spacing;

        /// <summary>
        /// Parameter LayoutRule of AreaReinforcement
        /// </summary>
        public LayoutRules LayoutRule
        {
            get
            {
                return m_layoutRule;
            }
            set
            {
                m_layoutRule = value;
            }
        }

        /// <summary>
        /// set the parameters to given AreaReinforcement
        /// </summary>
        /// <param name="areaRein"></param>
        public virtual void FillIn(AreaReinforcement areaRein)
        {
            int temp = (int)m_layoutRule;
            bool flag = ParameterUtil.SetParaInt(areaRein, 
                BuiltInParameter.REBAR_SYSTEM_LAYOUT_RULE, temp);
            //if BuiltInParameter doesn't work
            if(!flag)    
            {
                Parameter paraLayout = ParameterUtil.FindParaByName(
                    areaRein.Parameters, "Layout Rule");
                if (null != paraLayout)
                {
                    paraLayout.Set(temp);
                }
            }
            ChangeAreaReinCurves(areaRein);
        }

        /// <summary>
        /// On the interior 4 curves, set the override flag 
        /// and flip the hooks on the top 2 layers to "up"
        /// </summary>
        /// <param name="areaRein"></param>
        private void ChangeAreaReinCurves(AreaReinforcement areaRein)
        {
            //interior 4 curves are listed in the back of the curves, 
            //this order is decided when we create it
            for (int i = 4; i < 8; i++)
            {
                AreaReinforcementCurve areaReinCurve = 
                    areaRein.Curves.get_Item(i) as AreaReinforcementCurve;
                //remove hooks, set the hook the top 2 layers to 'up'
                ParameterUtil.SetParaInt(areaReinCurve, 
                    BuiltInParameter.REBAR_SYSTEM_OVERRIDE, -1);
                ParameterUtil.SetParaInt(areaReinCurve, 
                    BuiltInParameter.REBAR_SYSTEM_HOOK_ORIENT_TOP_DIR_1, 
                    (int)HookOrientation.Up);
                ParameterUtil.SetParaInt(areaReinCurve, 
                    BuiltInParameter.REBAR_SYSTEM_HOOK_ORIENT_TOP_DIR_2, 
                    (int)HookOrientation.Up);
            }
        }
    }
}
