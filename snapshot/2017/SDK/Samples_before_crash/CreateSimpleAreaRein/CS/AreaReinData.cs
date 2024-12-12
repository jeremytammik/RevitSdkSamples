//
// (C) Copyright 2003-2016 by Autodesk, Inc.
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
    using System.ComponentModel;

    using Autodesk.Revit;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.DB.Structure;

    using GeoElement = Autodesk.Revit.DB.GeometryElement;
    using Element = Autodesk.Revit.DB.Element;


    /// <summary>
    /// data of the AreaReinforcement
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
        [CategoryAttribute("Construction"), DefaultValueAttribute(
            LayoutRules.Maximum_Spacing)]
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

            if(!flag)    
            {
                Parameter paraLayout = ParameterUtil.FindParaByName(
                    areaRein.Parameters, "Layout Rule");
                if (null != paraLayout)
                {
                    paraLayout.Set(temp);
                }
            }
        }
    }

    /// <summary>
    /// data of AreaReinforcement which created on wall
    /// </summary>
    public class AreaReinDataOnWall:AreaReinData
    {
        private bool m_exteriorMajorDirection = true;
        private bool m_exteriorMinorDirection = true;
        private bool m_interiorMajorDirection = true;
        private bool m_interiorMinorDirection = true;

        /// <summary>
        /// Parameter of AreaReinforcement
        /// </summary>
        [CategoryAttribute("Layers")]
        public bool ExteriorMajorDirection
        {
            get
            {
                return m_exteriorMajorDirection;
            }
            set
            {
                m_exteriorMajorDirection = value;
            }
        }

        /// <summary>
        /// Parameter of AreaReinforcement
        /// </summary>
        [CategoryAttribute("Layers")]
        public bool ExteriorMinorDirection
        {
            get
            {
                return m_exteriorMinorDirection;
            }
            set
            {
                m_exteriorMinorDirection = value;
            }
        }

        /// <summary>
        /// Parameter of AreaReinforcement
        /// </summary>
        [CategoryAttribute("Layers")]
        public bool InteriorMajorDirection
        {
            get
            {
                return m_interiorMajorDirection;
            }
            set
            {
                m_interiorMajorDirection = value;
            }
        }

        /// <summary>
        /// Parameter of AreaReinforcement
        /// </summary>
        [CategoryAttribute("Layers")]
        public bool InteriorMinorDirection
        {
            get
            {
                return m_interiorMinorDirection;
            }
            set
            {
                m_interiorMinorDirection = value;
            }
        }

        /// <summary>
        /// set the parameters to given AreaReinforcement
        /// </summary>
        /// <param name="areaRein"></param>
        public override void FillIn(AreaReinforcement areaRein)
        {
            base.FillIn(areaRein);


            foreach (Parameter para in areaRein.Parameters)
            {
                if (para.Definition.Name == "Exterior Major Direction")
                {
                    para.Set(Convert.ToInt32(m_exteriorMajorDirection));
                }

                if (para.Definition.Name == "Interior Major Direction")
                {
                    para.Set(Convert.ToInt32(m_interiorMajorDirection));
                }

                if (para.Definition.Name == "Exterior Minor Direction")
                {
                    para.Set(Convert.ToInt32(m_exteriorMinorDirection));
                }

                if (para.Definition.Name == "Interior Minor Direction")
                {
                    para.Set(Convert.ToInt32(m_interiorMinorDirection));
                }
            }
        }
    }

    /// <summary>
    /// data of AreaReinforcement which created on floor
    /// </summary>
    public class AreaReinDataOnFloor : AreaReinData
    {
        private bool m_topMajorDirection = true;
        private bool m_topMinorDirection = true;
        private bool m_bottomMajorDirection = true;
        private bool m_bottomMinorDirection = true;

        /// <summary>
        /// Parameter of AreaReinforcement
        /// </summary>
        [CategoryAttribute("Layers")]
        public bool TopMajorDirection
        {
            get
            {
                return m_topMajorDirection;
            }
            set
            {
                m_topMajorDirection = value;
            }
        }

        /// <summary>
        /// Parameter of AreaReinforcement
        /// </summary>
        [CategoryAttribute("Layers")]
        public bool TopMinorDirection
        {
            get
            {
                return m_topMinorDirection;
            }
            set
            {
                m_topMinorDirection = value;
            }
        }

        /// <summary>
        /// Parameter of AreaReinforcement
        /// </summary>
        [CategoryAttribute("Layers")]
        public bool BottomMajorDirection
        {
            get
            {
                return m_bottomMajorDirection;
            }
            set
            {
                m_bottomMajorDirection = value;
            }
        }

        /// <summary>
        /// Parameter of AreaReinforcement
        /// </summary>
        [CategoryAttribute("Layers")]
        public bool BottomMinorDirection
        {
            get
            {
                return m_bottomMinorDirection;
            }
            set
            {
                m_bottomMinorDirection = value;
            }
        }

        /// <summary>
        /// set the parameters to given AreaReinforcement
        /// </summary>
        /// <param name="areaRein"></param>
        public override void FillIn(AreaReinforcement areaRein)
        {
            base.FillIn(areaRein);

            ParameterUtil.SetParaInt(areaRein, 
                BuiltInParameter.REBAR_SYSTEM_ACTIVE_BOTTOM_DIR_1, 
                Convert.ToInt32(m_bottomMajorDirection));
            ParameterUtil.SetParaInt(areaRein, 
                BuiltInParameter.REBAR_SYSTEM_ACTIVE_BOTTOM_DIR_2,
                Convert.ToInt32(m_bottomMinorDirection));
            ParameterUtil.SetParaInt(areaRein, 
                BuiltInParameter.REBAR_SYSTEM_ACTIVE_TOP_DIR_1, 
                Convert.ToInt32(m_topMajorDirection));
            ParameterUtil.SetParaInt(areaRein, 
                BuiltInParameter.REBAR_SYSTEM_ACTIVE_TOP_DIR_2, 
                Convert.ToInt32(m_topMinorDirection));
        }
    }
}
