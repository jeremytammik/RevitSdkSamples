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
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Parameters;
using Autodesk.Revit.Enums;

namespace Revit.SDK.Samples.Materials.CS
{

    /// <summary>
    /// derived class of MaterialParameters 
    /// </summary>
    public class MaterialConcreteParameters : MaterialParameters
    {
        private MaterialConcrete m_materialConcrete;// reference to MaterialConcrete
        private bool m_isLightWeight;//if LightWeight == true, m_lightWeiht == true; otherwise false

        /// <summary>
        /// original Type : MaterialBehaviourType
        /// </summary>
        public MaterialBehaviourType Behavior
        {
            get
            {
                m_behavior = m_materialConcrete.Behavior;
                return m_behavior;
            }
            set
            {
                if (MaterialBehaviourType.Undefined == value)
                {
                    return;
                }
                m_materialConcrete.Behavior = value;                
            }
        }

        /// <summary>
        /// original Type : double
        /// </summary>
        public string ConcreteCompression
        {
            get
            {
                return DoubleToString(m_materialConcrete.ConcreteCompression, UnitType.Stress);
            }
            set
            {
                double newValue;
                if (StringToDouble(value, out newValue, UnitType.Stress))
                {
                    m_materialConcrete.ConcreteCompression = newValue;
                }
            }
        }

        /// <summary>
        /// original Type : double
        /// </summary>
        public string DampingRatio
        {
            get
            {
                return DoubleToString(m_materialConcrete.DampingRatio, UnitType.No);
            }
            set
            {
                double newValue;
                if (StringToDouble(value, out newValue, UnitType.No))
                {
                    m_materialConcrete.DampingRatio = newValue;
                }
            }
        }

        /// <summary>
        /// original Type : bool
        /// </summary>
        public bool LightWeight
        {
            get
            {
                m_isLightWeight = m_materialConcrete.LightWeight;
                return m_isLightWeight;                
            }
            set
            {
                m_materialConcrete.LightWeight = value;
            }

        }

        #region PoissonRatio
        /// <summary>
        /// original Type : Double
        /// </summary>
        [RefreshPropertiesAttribute(RefreshProperties.All)]
        public string PoissonRatioX
        {
            get
            {
                return DoubleToString(m_materialConcrete.PoissonModulusX, UnitType.No);
            }
            set
            {
                double newValue;
                if (StringToDouble(value, out newValue, UnitType.No))
                {
                    m_materialConcrete.PoissonModulusX = newValue;
                }
            }
        }

        /// <summary>
        /// original Type : Double
        /// </summary>
        public string PoissonRatioY
        {
            get
            {
                return DoubleToString(m_materialConcrete.PoissonModulusY, UnitType.No);
            }
            set
            {
                //when property "Behavior" is MaterialBehaviourType.Orthotropic,can set;
                //otherwise do nothing
                if (MaterialBehaviourType.Orthotropic == m_behavior)
                {
                    double newValue;
                    if (StringToDouble(value, out newValue, UnitType.No))
                    {
                        m_materialConcrete.PoissonModulusY = newValue;
                    }
                }
            }
        }

        /// <summary>
        /// original Type : Double
        /// </summary>
        public string PoissonRatioZ
        {
            get
            {
                return DoubleToString(m_materialConcrete.PoissonModulusZ, UnitType.No);
            }
            set
            {
                //when property "Behavior" is MaterialBehaviourType.Orthotropic,can set;
                //otherwise do nothing
                if (MaterialBehaviourType.Orthotropic == m_behavior)
                {
                    double newValue;
                    if (StringToDouble(value, out newValue, UnitType.No))
                    {
                        m_materialConcrete.PoissonModulusZ = newValue;
                    }
                }
            }
        }
        #endregion

        #region ShearModulus
        /// <summary>
        /// original Type : Double
        /// </summary>
        [RefreshPropertiesAttribute(RefreshProperties.All)]
        public string ShearModulusX
        {
            get
            {
                return DoubleToString(m_materialConcrete.ShearModulusX, UnitType.Stress);
            }
            set
            {
                double newValue;
                if (StringToDouble(value, out newValue, UnitType.Stress))
                {
                    m_materialConcrete.ShearModulusX = newValue;
                }
            }
        }

        /// <summary>
        /// original Type : Double
        /// </summary>
        public string ShearModulusY
        {
            get
            {
                return DoubleToString(m_materialConcrete.ShearModulusY, UnitType.Stress);
            }
            set
            {
                //when property "Behavior" is MaterialBehaviourType.Orthotropic,can set;
                //otherwise do nothing
                if (MaterialBehaviourType.Orthotropic == m_behavior)
                {
                    double newValue;
                    if (StringToDouble(value, out newValue, UnitType.Stress))
                    {
                        m_materialConcrete.ShearModulusY = newValue;
                    }
                }
            }
        }

        /// <summary>
        /// original Type : Double
        /// </summary>
        public string ShearModulusZ
        {
            get
            {
                return DoubleToString(m_materialConcrete.ShearModulusZ, UnitType.Stress);
            }
            set
            {
                //when property "Behavior" is MaterialBehaviourType.Orthotropic,can set;
                //otherwise do nothing
                if (MaterialBehaviourType.Orthotropic == m_behavior)
                {
                    double newValue;
                    if (StringToDouble(value, out newValue, UnitType.Stress))
                    {
                        m_materialConcrete.ShearModulusZ = newValue;
                    }
                }
            }
        }
        #endregion

        /// <summary>
        /// original Type : Double
        /// </summary>
        public string ShearStrengthModification
        {
            get
            {
                return DoubleToString(m_materialConcrete.ShearStrengthReduction, UnitType.No);
            }
            set
            {
                if (m_isLightWeight)
                {
                    double newValue;
                    if (StringToDouble(value, out newValue, UnitType.No))
                    {
                        m_materialConcrete.ShearStrengthReduction = newValue;
                    }
                }
            }
        }

        #region ThermalExpansionCoefficient
        /// <summary>
        /// original Type : Double
        /// </summary>
        [RefreshPropertiesAttribute(RefreshProperties.All)]
        public string ThermalExpansionCoefficientX
        {
            get
            {
                return DoubleToString(m_materialConcrete.ThermalExpansionCoefficientX,
                                        UnitType.Temperature);
            }
            set
            {
                double newValue;
                if (StringToDouble(value, out newValue, UnitType.Temperature))
                {
                    m_materialConcrete.ThermalExpansionCoefficientX = newValue;
                }
            }
        }

        /// <summary>
        /// original Type : Double
        /// </summary>
        public string ThermalExpansionCoefficientY
        {
            get
            {
                return DoubleToString(m_materialConcrete.ThermalExpansionCoefficientY, UnitType.Temperature);
            }
            set
            {
                //when property "Behavior" is MaterialBehaviourType.Orthotropic,can set;
                //otherwise do nothing
                if (MaterialBehaviourType.Orthotropic == m_behavior)
                {
                    double newValue;
                    if (StringToDouble(value, out newValue, UnitType.Temperature))
                    {
                        m_materialConcrete.ThermalExpansionCoefficientY = newValue;
                    }
                }
            }
        }

        /// <summary>
        /// original Type : Double
        /// </summary>
        public string ThermalExpansionCoefficientZ
        {
            get
            {
                return DoubleToString(m_materialConcrete.ThermalExpansionCoefficientZ,
                                        UnitType.Temperature);
            }
            set
            {
                //when property "Behavior" is MaterialBehaviourType.Orthotropic,can set;
                //otherwise do nothing
                if (MaterialBehaviourType.Orthotropic == m_behavior)
                {
                    double newValue;
                    if (StringToDouble(value, out newValue, UnitType.Temperature))
                    {
                        m_materialConcrete.ThermalExpansionCoefficientZ = newValue;
                    }
                }
            }
        }
        #endregion

        /// <summary>
        /// original Type : Double
        /// </summary>
        public string UnitWeight
        {
            get
            {
                return DoubleToString(m_materialConcrete.UnitWeight, UnitType.UnitWeight);
            }
            set
            {
                double newValue;
                if (StringToDouble(value, out newValue, UnitType.UnitWeight))
                {
                    m_materialConcrete.UnitWeight = newValue;
                }
            }
        }

        #region YoungModulus
        /// <summary>
        /// original Type : Double
        /// </summary>
        [RefreshPropertiesAttribute(RefreshProperties.All)]
        public string YoungModulusX
        {
            get
            {
                return DoubleToString(m_materialConcrete.YoungModulusX, UnitType.Stress);
            }
            set
            {
                double newValue;
                if (StringToDouble(value, out newValue, UnitType.Stress))
                {
                    m_materialConcrete.YoungModulusX = newValue;
                }
            }
        }

        /// <summary>
        /// original Type : Double
        /// </summary>
        public string YoungModulusY
        {
            get
            {
                return DoubleToString(m_materialConcrete.YoungModulusY, UnitType.Stress);
            }
            set
            {
                //when property "Behavior" is MaterialBehaviourType.Orthotropic,can set;
                //otherwise do nothing
                if (MaterialBehaviourType.Orthotropic == m_behavior)
                {
                    double newValue;
                    if (StringToDouble(value, out newValue, UnitType.Stress))
                    {
                        m_materialConcrete.YoungModulusY = newValue;
                    }
                }
            }
        }

        /// <summary>
        /// original Type : Double
        /// </summary>
        public string YoungModulusZ
        {
            get
            {
                return DoubleToString(m_materialConcrete.YoungModulusZ, UnitType.Stress);
            }
            set
            {
                //when property "Behavior" is MaterialBehaviourType.Orthotropic,can set;
                //otherwise do nothing
                if (MaterialBehaviourType.Orthotropic == m_behavior)
                {
                    double newValue;
                    if (StringToDouble(value, out newValue, UnitType.Stress))
                    {
                        m_materialConcrete.YoungModulusZ = newValue;
                    }
                }
            }
        }
        #endregion


        /// <summary>
        /// MaterialConcreteParameters's constructor 
        /// </summary>
        /// <param name="materialConcrete">a instance of MaterialConcrete</param>
        public MaterialConcreteParameters(MaterialConcrete materialConcrete)
            : base(materialConcrete)
        {
            m_materialConcrete = materialConcrete;
        }
    }
}
