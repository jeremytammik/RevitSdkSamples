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
    public class MaterialSteelParameters : MaterialParameters
    {
        private MaterialSteel m_materialSteel;// reference to MaterialSteel


        /// <summary>
        /// original Type : MaterialBehaviourType
        /// </summary>
        public MaterialBehaviourType Behavior
        {
            get
            {
                m_behavior = m_materialSteel.Behavior;
                return m_behavior;
            }
            set
            {
                if (MaterialBehaviourType.Undefined == value)
                {
                    return;
                }
                m_materialSteel.Behavior = value;
            }
        }

        /// <summary>
        /// original Type : Double
        /// </summary>
        public string DampingRatio
        {
            get
            {
                return DoubleToString(m_materialSteel.DampingRatio, UnitType.No);
            }
            set
            {
                double newValue;
                if (StringToDouble(value, out newValue, UnitType.No))
                {
                    m_materialSteel.DampingRatio = newValue;
                }
            }
        }

        /// <summary>
        /// original Type : Double
        /// </summary>
        public string MinimumTensileStrength
        {
            get
            {
                return DoubleToString(m_materialSteel.MinimumTensileStrength, UnitType.Stress);
            }
            set
            {
                double newValue;
                if (StringToDouble(value, out newValue, UnitType.Stress))
                {
                    m_materialSteel.MinimumTensileStrength = newValue;
                }
            }
        }

        /// <summary>
        /// original Type : Double
        /// </summary>
        public string MinimumYieldStress
        {
            get
            {
                return DoubleToString(m_materialSteel.MinimumYieldStress, UnitType.Stress);
            }
            set
            {
                double newValue;
                if (StringToDouble(value, out newValue, UnitType.Stress))
                {
                    m_materialSteel.MinimumYieldStress = newValue;
                }
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
                return DoubleToString(m_materialSteel.PoissonModulusX, UnitType.No);
            }
            set
            {
                double newValue;
                if (StringToDouble(value, out newValue, UnitType.No))
                {
                    m_materialSteel.PoissonModulusX = newValue;
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
                return DoubleToString(m_materialSteel.PoissonModulusY, UnitType.No);
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
                        m_materialSteel.PoissonModulusY = newValue;
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
                return DoubleToString(m_materialSteel.PoissonModulusZ, UnitType.No);
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
                        m_materialSteel.PoissonModulusZ = newValue;
                    }
                }
            }
        }
        #endregion

        /// <summary>
        /// original Type : Double
        /// </summary>
        public string ReductionFactorForShear
        {
            get
            {
                return DoubleToString(m_materialSteel.ReductionFactor, UnitType.No);
            }
            set
            {
                double newValue;
                if (StringToDouble(value, out newValue, UnitType.No))
                {
                    m_materialSteel.ReductionFactor = newValue;
                }
            }
        }

        #region ShearModulus
        /// <summary>
        /// original Type : Double
        /// </summary>
        [RefreshPropertiesAttribute(RefreshProperties.All)]
        public string ShearModulusX
        {
            get
            {
                return DoubleToString(m_materialSteel.ShearModulusX, UnitType.Stress);
            }
            set
            {
                double newValue;
                if (StringToDouble(value, out newValue, UnitType.Stress))
                {
                    m_materialSteel.ShearModulusX = newValue;
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
                return DoubleToString(m_materialSteel.ShearModulusY, UnitType.Stress);
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
                        m_materialSteel.ShearModulusY = newValue;
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
                return DoubleToString(m_materialSteel.ShearModulusZ, UnitType.Stress);
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
                        m_materialSteel.ShearModulusZ = newValue;
                    }
                }
            }
        }
        #endregion

        #region ThermalExpansionCoefficient
        /// <summary>
        /// original Type : Double
        /// </summary>
        [RefreshPropertiesAttribute(RefreshProperties.All)]
        public string ThermalExpansionCoefficientX
        {
            get
            {
                return DoubleToString(m_materialSteel.ThermalExpansionCoefficientX,
                                        UnitType.Temperature);
            }
            set
            {
                double newValue;
                if (StringToDouble(value, out newValue, UnitType.Temperature))
                {
                    m_materialSteel.ThermalExpansionCoefficientX = newValue;
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
                return DoubleToString(m_materialSteel.ThermalExpansionCoefficientY,
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
                        m_materialSteel.ThermalExpansionCoefficientY = newValue;
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
                return DoubleToString(m_materialSteel.ThermalExpansionCoefficientZ,
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
                        m_materialSteel.ThermalExpansionCoefficientZ = newValue;
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
                return DoubleToString(m_materialSteel.UnitWeight, UnitType.UnitWeight);
            }
            set
            {
                double newValue;
                if (StringToDouble(value, out newValue, UnitType.UnitWeight))
                {
                    m_materialSteel.UnitWeight = newValue;
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
                return DoubleToString(m_materialSteel.YoungModulusX, UnitType.Stress);
            }
            set
            {
                double newValue;
                if (StringToDouble(value, out newValue, UnitType.Stress))
                {
                    m_materialSteel.YoungModulusX = newValue;
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
                return DoubleToString(m_materialSteel.YoungModulusY, UnitType.Stress);
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
                        m_materialSteel.YoungModulusY = newValue;
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
                return DoubleToString(m_materialSteel.YoungModulusZ, UnitType.Stress);
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
                        m_materialSteel.YoungModulusZ = newValue;
                    }
                }
            }
        }
        #endregion


        /// <summary>
        /// MaterialSteelParameters's constructor 
        /// </summary>
        /// <param name="materialSteel">a instance of MaterialSteel</param>
        public MaterialSteelParameters(MaterialSteel materialSteel)
            : base(materialSteel)
        {
            m_materialSteel = materialSteel;
        }
    }
}
