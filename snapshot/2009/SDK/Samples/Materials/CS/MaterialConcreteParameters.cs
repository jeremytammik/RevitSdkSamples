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
        
        private List<String> m_behaviorRelativeProperties;//list stores name of properties which is relavite with Behavior
        
        
        /// <summary>
        /// original Type : MaterialBehaviourType
        /// </summary>
        public LocalMaterialBehaviourType Behavior
        {
            get
            {
                LocalMaterialBehaviourType curBehavior = (LocalMaterialBehaviourType)GetParameterValue(BuiltInParameter.PHY_MATERIAL_PARAM_BEHAVIOR);
                if (m_behavior != curBehavior)
                {
                    bool isReadonly = false;
                    if (LocalMaterialBehaviourType.Isotropic == curBehavior)
                    {
                        isReadonly = true;
                    }
                    foreach (String propertyName in m_behaviorRelativeProperties)
                    {
                        SetPropertyReadOnly(this, propertyName, isReadonly);
                    }
                    m_behavior = curBehavior;
                }
                return m_behavior;
            }
            set
            {
                SetParameterValue(BuiltInParameter.PHY_MATERIAL_PARAM_BEHAVIOR, value);
            }
        }

        /// <summary>
        /// original Type : double
        /// </summary>
        [DisplayName("Concrete Compression")]
        public string ConcreteCompression
        {
            get
            {
                return GetParameterValueString(BuiltInParameter.PHY_MATERIAL_PARAM_CONCRETE_COMPRESSION);
            }
            set
            {
                SetParameterValueString(BuiltInParameter.PHY_MATERIAL_PARAM_CONCRETE_COMPRESSION, value);
            }
        }

        /// <summary>
        /// original Type : double
        /// </summary>
        [DisplayName("Damping Ratio")]
        public string DampingRatio
        {
            get
            {
                return GetParameterValueString(BuiltInParameter.PHY_MATERIAL_PARAM_DAMPING_RATIO);
            }
            set
            {
                SetParameterValueString(BuiltInParameter.PHY_MATERIAL_PARAM_DAMPING_RATIO, value);
            }
        }

        /// <summary>
        /// original Type : bool
        /// </summary>
        [DisplayName("Light Weight")]
        public bool LightWeight
        {
            get
            {
                int lightWeightValue = (int)GetParameterValue(BuiltInParameter.PHY_MATERIAL_PARAM_LIGHT_WEIGHT);
                bool lightWeight;
                if(0 == lightWeightValue)
                {
                    lightWeight = false;
                }
                else
                {
                    lightWeight = true;
                }
                if (m_isLightWeight != lightWeight)
                {
                    m_isLightWeight = m_materialConcrete.LightWeight;
                    SetPropertyReadOnly(this, "ShearStrengthModification", !m_isLightWeight);
                    m_isLightWeight = lightWeight;
                }
                return m_isLightWeight;
            }
            set
            {
                if (value)
                {
                    SetParameterValue(BuiltInParameter.PHY_MATERIAL_PARAM_LIGHT_WEIGHT, 1);
                }
                else
                {
                    SetParameterValue(BuiltInParameter.PHY_MATERIAL_PARAM_LIGHT_WEIGHT, 0);
                }
            }
        }

        #region PoissonRatio
        /// <summary>
        /// original Type : Double
        /// </summary>
        [RefreshPropertiesAttribute(RefreshProperties.All), DisplayName("Poisson Ratio X")]
        public string PoissonRatioX
        {
            get
            {
                return GetParameterValueString(BuiltInParameter.PHY_MATERIAL_PARAM_POISSON_MOD1);
            }
            set
            {
                SetParameterValueString(BuiltInParameter.PHY_MATERIAL_PARAM_POISSON_MOD1, value);
            }
        }

        /// <summary>
        /// original Type : Double
        /// </summary>
        [ReadOnlyAttribute(true), DisplayName("Poisson Ratio Y")]
        public string PoissonRatioY
        {
            get
            {
                return GetParameterValueString(BuiltInParameter.PHY_MATERIAL_PARAM_POISSON_MOD2);
            }
            set
            {
                SetParameterValueString(BuiltInParameter.PHY_MATERIAL_PARAM_POISSON_MOD2, value);
            }
        }

        /// <summary>
        /// original Type : Double
        /// </summary>
        [ReadOnlyAttribute(true), DisplayName("Poisson Ratio Z")]
        public string PoissonRatioZ
        {
            get
            {
                return GetParameterValueString(BuiltInParameter.PHY_MATERIAL_PARAM_POISSON_MOD3);
            }
            set
            {
                SetParameterValueString(BuiltInParameter.PHY_MATERIAL_PARAM_POISSON_MOD3, value);
            }
        }
        #endregion

        #region ShearModulus
        /// <summary>
        /// original Type : Double
        /// </summary>
        [RefreshPropertiesAttribute(RefreshProperties.All), DisplayName("Shear Modulus X")]
        public string ShearModulusX
        {
            get
            {
                return GetParameterValueString(BuiltInParameter.PHY_MATERIAL_PARAM_SHEAR_MOD1);
            }
            set
            {
                SetParameterValueString(BuiltInParameter.PHY_MATERIAL_PARAM_SHEAR_MOD1, value);
            }
        }

        /// <summary>
        /// original Type : Double
        /// </summary>
        [ReadOnlyAttribute(true), DisplayName("Shear Modulus Y")]
        public string ShearModulusY
        {
            get
            {
                return GetParameterValueString(BuiltInParameter.PHY_MATERIAL_PARAM_SHEAR_MOD2);
            }
            set
            {
                SetParameterValueString(BuiltInParameter.PHY_MATERIAL_PARAM_SHEAR_MOD2, value);
            }
        }

        /// <summary>
        /// original Type : Double
        /// </summary>
        [ReadOnlyAttribute(true), DisplayName("Shear Modulus Z")]
        public string ShearModulusZ
        {
            get
            {
                return GetParameterValueString(BuiltInParameter.PHY_MATERIAL_PARAM_SHEAR_MOD3);
            }
            set
            {
                SetParameterValueString(BuiltInParameter.PHY_MATERIAL_PARAM_SHEAR_MOD3, value);
            }
        }
        #endregion

        /// <summary>
        /// original Type : Double
        /// </summary>
        [ReadOnlyAttribute(true), DisplayName("Shear Strength Modification")]
        public string ShearStrengthModification
        {
            get
            {
                return GetParameterValueString(BuiltInParameter.PHY_MATERIAL_PARAM_SHEAR_STRENGTH_REDUCTION);
            }
            set
            {
                SetParameterValueString(BuiltInParameter.PHY_MATERIAL_PARAM_SHEAR_STRENGTH_REDUCTION, value);
            }
        }

        #region ThermalExpansionCoefficient
        /// <summary>
        /// original Type : Double
        /// </summary>
        [RefreshPropertiesAttribute(RefreshProperties.All), DisplayName("Thermal Expansion Coefficient X")]
        public string ThermalExpansionCoefficientX
        {
            get
            {
                return GetParameterValueString(BuiltInParameter.PHY_MATERIAL_PARAM_EXP_COEFF1);
            }
            set
            {
                SetParameterValueString(BuiltInParameter.PHY_MATERIAL_PARAM_EXP_COEFF1, value);
            }
        }

        /// <summary>
        /// original Type : Double
        /// </summary>
        [ReadOnlyAttribute(true), DisplayName("Thermal Expansion Coefficient Y")]
        public string ThermalExpansionCoefficientY
        {
            get
            {
                return GetParameterValueString(BuiltInParameter.PHY_MATERIAL_PARAM_EXP_COEFF2);
            }
            set
            {
                SetParameterValueString(BuiltInParameter.PHY_MATERIAL_PARAM_EXP_COEFF2, value);
            }
        }

        /// <summary>
        /// original Type : Double
        /// </summary>
        [ReadOnlyAttribute(true), DisplayName("Thermal Expansion Coefficient Z")]
        public string ThermalExpansionCoefficientZ
        {
            get
            {
                return GetParameterValueString(BuiltInParameter.PHY_MATERIAL_PARAM_EXP_COEFF3);
            }
            set
            {
                SetParameterValueString(BuiltInParameter.PHY_MATERIAL_PARAM_EXP_COEFF3, value);
            }
        }
        #endregion

        /// <summary>
        /// original Type : Double
        /// </summary>
        [DisplayName("Unit Weight")]
        public string UnitWeight
        {
            get
            {
                return GetParameterValueString(BuiltInParameter.PHY_MATERIAL_PARAM_UNIT_WEIGHT);
            }
            set
            {
                SetParameterValueString(BuiltInParameter.PHY_MATERIAL_PARAM_UNIT_WEIGHT, value);
            }
        }

        #region YoungModulus
        /// <summary>
        /// original Type : Double
        /// </summary>
        [RefreshPropertiesAttribute(RefreshProperties.All), DisplayName("Young Modulus X")]
        public string YoungModulusX
        {
            get
            {
                return GetParameterValueString(BuiltInParameter.PHY_MATERIAL_PARAM_YOUNG_MOD1);
            }
            set
            {
                SetParameterValueString(BuiltInParameter.PHY_MATERIAL_PARAM_YOUNG_MOD1, value);
            }
        }

        /// <summary>
        /// original Type : Double
        /// </summary>
        [ReadOnlyAttribute(true), DisplayName("Young Modulus Y")]
        public string YoungModulusY
        {
            get
            {
                return GetParameterValueString(BuiltInParameter.PHY_MATERIAL_PARAM_YOUNG_MOD2);
            }
            set
            {
                SetParameterValueString(BuiltInParameter.PHY_MATERIAL_PARAM_YOUNG_MOD2, value);
            }
        }

        /// <summary>
        /// original Type : Double
        /// </summary>
        [ReadOnlyAttribute(true), DisplayName("Young Modulus Z")]
        public string YoungModulusZ
        {
            get
            {
                return GetParameterValueString(BuiltInParameter.PHY_MATERIAL_PARAM_YOUNG_MOD3);
            }
            set
            {
                SetParameterValueString(BuiltInParameter.PHY_MATERIAL_PARAM_YOUNG_MOD3, value);
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
            m_behaviorRelativeProperties = new List<string>();
            m_behaviorRelativeProperties.AddRange(new String[] { "ShearModulusY", "ShearModulusZ", 
                "PoissonRatioY","PoissonRatioZ", "YoungModulusY","YoungModulusZ",
            "ThermalExpansionCoefficientY", "ThermalExpansionCoefficientZ"});
        }
    }
}
