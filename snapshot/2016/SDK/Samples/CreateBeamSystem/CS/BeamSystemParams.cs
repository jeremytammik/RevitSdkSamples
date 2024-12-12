//
// (C) Copyright 2003-2015 by Autodesk, Inc.
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


namespace Revit.SDK.Samples.CreateBeamSystem.CS
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.ComponentModel;

    using Autodesk.Revit;
    using Autodesk.Revit.DB;

    /// <summary>
    /// describes the type of beam layout method in beam system
    /// </summary>
    public enum LayoutMethod
    {
        /// <summary>
        /// the beam's layout method in beam System is clear the spacing among beams
        /// </summary>
        ClearSpacing,

        /// <summary>
        /// maximum the space among beams
        /// </summary>
        MaximumSpacing,

        /// <summary>
        /// has fixed beams number and user appoint the number
        /// </summary>
        FixedNumber,

        /// <summary>
        /// has fixed distance among beams and user appoint this distance
        /// </summary>
        FixedDistance
    }

    /// <summary>
    /// declares a delegate for a method that takes in a LayoutMethod
    /// </summary>
    /// <param name="layoutMethod"></param>
    public delegate void LayoutRuleChangedHandler(ref LayoutMethod layoutMethod);

    /// <summary>
    /// the properties of beam system;
    /// can be displayed in PropertyGrid
    /// </summary>
    public abstract class BeamSystemParam
    {
        /// <summary>
        /// layout method
        /// </summary>
        protected LayoutMethod m_layoutType;

        /// <summary>
        /// space between beams; buffer for subclass
        /// </summary>
        protected double m_fixedSpacing;
        
        /// <summary>
        /// justify type; buffer for subclass
        /// </summary>
        protected BeamSystemJustifyType m_justifyType;    
        
        /// <summary>
        /// number of beams
        /// </summary>
        protected int m_numberOfLines;    
                
        private LayoutRuleChangedHandler m_layoutRuleChanged;
        private FamilySymbol m_beamType;                // beam type of beam system

        /// <summary>
        /// layout method of beam system is changed
        /// </summary>
        [Browsable(false)]
        public LayoutRuleChangedHandler LayoutRuleChanged
        {
            get
            {
                return m_layoutRuleChanged; 
            }
            set
            {
                m_layoutRuleChanged = value;
            }
        }

        /// <summary>
        /// kind of layout rule
        /// </summary>
        [Category("Pattern"),
        Description("Specify the layout rule")]
        public LayoutMethod LayoutRuleMethod
        {
            get
            {
                return m_layoutType;
            }
            set
            {
                if (m_layoutType != value)
                {
                    // invokes the delegate
                    LayoutRuleChanged(ref value);
                }
            }
        }

        /// <summary>
        /// type of beam used to create beam system
        /// </summary>
        [Category("Pattern"), TypeConverter(typeof(BeamTypeItem)),
        Description("Select a value for the Beam Type used in the beam system")]
        public FamilySymbol BeamType
        {
            get
            {
                return m_beamType;
            }
            set
            {
                m_beamType = value;
            }
        }

        /// <summary>
        /// initial general members for its subclass
        /// </summary>
        protected BeamSystemParam()
        {
            m_fixedSpacing = 2000.0;
            m_justifyType = BeamSystemJustifyType.Center;
            m_numberOfLines = 6;
        }

        /// <summary>
        /// create BeamSystemParam's subclass according to LayoutMethod
        /// </summary>
        /// <param name="layoutType">LayoutMethod</param>
        /// <returns>created BeamSystemParam's subclass</returns>
        public static BeamSystemParam CreateInstance(LayoutMethod layoutType)
        {
            BeamSystemParam param = null;
            switch (layoutType)
            {
                case LayoutMethod.ClearSpacing:
                    param = new ClearSpacingParam();
                    break;
                case LayoutMethod.FixedDistance:
                    param = new FixedDistanceParam();
                    break;
                case LayoutMethod.FixedNumber:
                    param = new FixedNumberParam();
                    break;
                case LayoutMethod.MaximumSpacing:
                    param = new MaximumSpacingParam();
                    break;
                default:
                    break;
            }
            // it is absolutely impossible unless layoutType is wrong
            Debug.Assert(null != param);
            return param;
        }

        /// <summary>
        /// clone BeamSystemParam to one of its subclass according to LayoutMethod
        /// </summary>
        /// <param name="layoutType">LayoutMethod</param>
        /// <returns>cloned BeamSystemParam's subclass</returns>
        public BeamSystemParam CloneInstance(LayoutMethod layoutType)
        {
            // create a BeamSystemParam instance and set its properties
            BeamSystemParam param = CreateInstance(layoutType);
            param.m_fixedSpacing  = m_fixedSpacing;
            param.m_justifyType   = m_justifyType;
            param.m_numberOfLines = m_numberOfLines;
            param.m_beamType      = m_beamType;
            return param;            
        }

        /// <summary>
        /// subclass of LayoutRule
        /// </summary>
        [Browsable(false)]
        public abstract LayoutRule Layout
        {
            get;
        }

        /// <summary>
        /// properties related to LayoutRule when it's clear spacing
        /// only visible for class BeamSystemParam
        /// </summary>
        class ClearSpacingParam : BeamSystemParam
        {
            protected LayoutRuleClearSpacing m_layout;

            /// <summary>
            /// wrapped LayoutRuleClearSpacing object
            /// </summary>
            public override LayoutRule Layout
            {
                get
                {
                    return m_layout;
                }
            }

            /// <summary>
            /// FixedSpacing value of beam system
            /// </summary>
            [Category("Pattern"),
            Description("representing the distance between each beam")]
            public double ClearSpacing
            {
                get
                {
                    return m_fixedSpacing;
                }
                set
                {
                    try
                    {
                        m_layout.Spacing = value;
                        m_fixedSpacing   = value;
                    }
                    catch
                    {        
                    }
                }
            }

            /// <summary>
            /// JustifyType value of beam system
            /// </summary>
            [Category("Pattern"),
            Description("This value determines the placement of the first beam"
            + " and each subsequent beam is spaced a fixed distance from it.")]
            public BeamSystemJustifyType JustifyType
            {
                get
                {
                    return m_justifyType;
                }
                set
                {
                    m_layout.JustifyType = value;
                    m_justifyType        = value;
                }
            }

            /// <summary>
            /// constructor
            /// </summary>
            public ClearSpacingParam()
                : base()
            {
                m_layout     = new LayoutRuleClearSpacing(m_fixedSpacing, m_justifyType);
                m_layoutType = LayoutMethod.ClearSpacing;
            }
        }

        /// <summary>
        /// properties related to LayoutRule when it's fixed distance
        /// only visible for class BeamSystemParam
        /// </summary>
        class FixedDistanceParam : BeamSystemParam
        {
            protected LayoutRuleFixedDistance m_layout;

            /// <summary>
            /// wrapped LayoutRuleFixedDistance object
            /// </summary>
            public override LayoutRule Layout
            {
                get
                {
                    return m_layout;
                }
            }

            /// <summary>
            /// constructor
            /// </summary>
            public FixedDistanceParam()
                : base()
            {
                m_layout     = new LayoutRuleFixedDistance(m_fixedSpacing, m_justifyType);
                m_layoutType = LayoutMethod.FixedDistance;
            }

            /// <summary>
            /// FixedSpacing value of beam system
            /// </summary>
            [Category("Pattern"),
            Description("allows you to specify the distance between beams"
            + " based on the justification you specify.")]
            public double FixedSpacing
            {
                get
                {
                    return m_fixedSpacing;
                }
                set
                {
                    try
                    {
                        m_layout.Spacing = value;
                        m_fixedSpacing   = value;
                    }
                    catch
                    {
                    }
                }
            }

            /// <summary>
            /// JustifyType value of beam system
            /// </summary>
            [Category("Pattern"),
            Description("determines the placement of the first beam in the system"
            + " and each subsequent beam is spaced a fixed distance from that point.")]
            public BeamSystemJustifyType JustifyType
            {
                get
                {
                    return m_justifyType;
                }
                set
                {
                    m_layout.JustifyType = value;
                    m_justifyType        = value;
                }
            }
        }

        /// <summary>
        /// properties related to LayoutRule when it's fixed number
        /// only visible for class BeamSystemParam
        /// </summary>
        class FixedNumberParam : BeamSystemParam
        {
            protected LayoutRuleFixedNumber m_layout;

            /// <summary>
            /// NumberOfLines value of beam system
            /// </summary>
            [Category("Pattern"),
            Description("allows you to specify the number of beams within the beam system.")]
            public int NumberOfLines
            {
                get
                {
                    return m_numberOfLines;
                }
                set
                {
                    try
                    {
                        m_layout.NumberOfLines = value;
                        m_numberOfLines        = value;
                    }
                    catch
                    {
                    }
                }
            }

            /// <summary>
            /// wrapped LayoutRuleFixedNumber object
            /// </summary>
            public override LayoutRule Layout
            {
                get
                {
                    return m_layout;
                }
            }

            /// <summary>
            /// constructor
            /// </summary>
            public FixedNumberParam()
                : base()
            {
                m_layout     = new LayoutRuleFixedNumber(m_numberOfLines);
                m_layoutType = LayoutMethod.FixedNumber;
            }
        }

        /// <summary>
        /// properties related to LayoutRule when it's maximum spacing
        /// only visible for class BeamSystemParam
        /// </summary>
        class MaximumSpacingParam : BeamSystemParam
        {
            protected LayoutRuleMaximumSpacing m_layout;

            /// <summary>
            /// FixedSpacing value of beam system
            /// </summary>
            [Category("Pattern"), 
            Description("allows you to specify the maximum distance between beams.")]
            public double MaximumSpacing
            {
                get
                {
                    return m_fixedSpacing;
                }
                set
                {
                    try
                    {
                        m_layout.Spacing = value;
                        m_fixedSpacing   = value;
                    }
                    catch 
                    {
                    }
                }
            }

            /// <summary>
            /// wrapped LayoutRuleMaximumSpacing object
            /// </summary>
            public override LayoutRule Layout
            {
                get
                {
                    return m_layout;
                }
            }

            /// <summary>
            /// constructor
            /// </summary>
            public MaximumSpacingParam()
                : base()
            {
                m_layout     = new LayoutRuleMaximumSpacing(m_fixedSpacing);
                m_layoutType = LayoutMethod.MaximumSpacing;
            }
        }
    }
}
