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
    public class MaterialGenericParameters : MaterialParameters
    {
        private MaterialGeneric m_materialGeneric;// reference to MaterialGeneric

        /// <summary>
        /// original Type : MaterialBehaviourType
        /// </summary>
        public MaterialBehaviourType Behavior
        {
            get
            {
                m_behavior = m_materialGeneric.Behavior;
                return m_behavior;
            }
            set
            {
                if (MaterialBehaviourType.Undefined == value)
                {
                    return;
                }
                m_materialGeneric.Behavior = value;
            }
        }

        /// <summary>
        /// original Type : Double
        /// </summary>
        public string MinimumYieldStress
        {
            get
            {
                return DoubleToString(m_materialGeneric.MinimumYieldStress, UnitType.Stress);
            }
            set
            {
                double newValue;
                if (StringToDouble(value, out newValue, UnitType.Stress))
                {
                    m_materialGeneric.MinimumYieldStress = newValue;
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
                return DoubleToString(m_materialGeneric.PoissonModulusX, UnitType.No);
            }
            set
            {
                double newValue;
                if (StringToDouble(value, out newValue, UnitType.No))
                {
                    m_materialGeneric.PoissonModulusX = newValue;
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
                return DoubleToString(m_materialGeneric.PoissonModulusY, UnitType.No);
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
                        m_materialGeneric.PoissonModulusY = newValue;
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
                return DoubleToString(m_materialGeneric.PoissonModulusZ, UnitType.No);
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
                        m_materialGeneric.PoissonModulusZ = newValue;
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
                return DoubleToString(m_materialGeneric.ReductionFactor, UnitType.No);
            }
            set
            {
                double newValue;
                if (StringToDouble(value, out newValue, UnitType.No))
                {
                    m_materialGeneric.ReductionFactor = newValue;
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
                return DoubleToString(m_materialGeneric.ShearModulusX, UnitType.Stress);
            }
            set
            {
                double newValue;
                if (StringToDouble(value, out newValue, UnitType.Stress))
                {
                    m_materialGeneric.ShearModulusX = newValue;
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
                return DoubleToString(m_materialGeneric.ShearModulusY, UnitType.Stress);
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
                        m_materialGeneric.ShearModulusY = newValue;
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
                return DoubleToString(m_materialGeneric.ShearModulusZ, UnitType.Stress);
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
                        m_materialGeneric.ShearModulusZ = newValue;
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
                return DoubleToString(m_materialGeneric.ThermalExpansionCoefficientX,
                                        UnitType.Temperature);
            }
            set
            {
                double newValue;
                if (StringToDouble(value, out newValue, UnitType.Temperature))
                {
                    m_materialGeneric.ThermalExpansionCoefficientX = newValue;
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
                return DoubleToString(m_materialGeneric.ThermalExpansionCoefficientY,
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
                        m_materialGeneric.ThermalExpansionCoefficientY = newValue;
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
                return DoubleToString(m_materialGeneric.ThermalExpansionCoefficientZ,
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
                        m_materialGeneric.ThermalExpansionCoefficientZ = newValue;
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
                return DoubleToString(m_materialGeneric.UnitWeight, UnitType.UnitWeight);
            }
            set
            {
                double newValue;
                if (StringToDouble(value, out newValue, UnitType.UnitWeight))
                {
                    m_materialGeneric.UnitWeight = newValue;
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
                return DoubleToString(m_materialGeneric.YoungModulusX, UnitType.Stress);
            }
            set
            {
                double newValue;
                if (StringToDouble(value, out newValue, UnitType.Stress))
                {
                    m_materialGeneric.YoungModulusX = newValue;
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
                return DoubleToString(m_materialGeneric.YoungModulusY, UnitType.Stress);
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
                        m_materialGeneric.YoungModulusY = newValue;
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
                return DoubleToString(m_materialGeneric.YoungModulusZ, UnitType.Stress);
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
                        m_materialGeneric.YoungModulusZ = newValue;
                    }
                }
            }
        }
        #endregion


        /// <summary>
        /// MaterialGenericParameters's constructor 
        /// </summary>
        /// <param name="materialGeneric">a instance of MaterialGeneric</param>
        public MaterialGenericParameters(MaterialGeneric materialGeneric)
            :base(materialGeneric)
        {
            m_materialGeneric = materialGeneric;
        }
    }
}
