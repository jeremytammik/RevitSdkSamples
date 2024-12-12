//
// (C) Copyright 2003-2009 by Autodesk, Inc.
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
    public class MaterialWoodParameters : MaterialParameters
    {
        private MaterialWood m_materialWood;// reference to MaterialWood


        /// <summary>
        /// original Type : Double
        /// </summary>
        public string Bending 
        {
            get
            {
                return GetParameterValueString(BuiltInParameter.PHY_MATERIAL_PARAM_BENDING);
            }
            set
            {
                SetParameterValueString(BuiltInParameter.PHY_MATERIAL_PARAM_BENDING, value);
            }
        }

        /// <summary>
        /// original Type : Double
        /// </summary>
        [DisplayName("Compression Parallel To Grain")]
        public string CompressionParallelToGrain
        {
            get
            {
                return GetParameterValueString(BuiltInParameter.PHY_MATERIAL_PARAM_COMPRESSION_PARALLEL);
            }
            set
            {
                SetParameterValueString(BuiltInParameter.PHY_MATERIAL_PARAM_COMPRESSION_PARALLEL, value);
            }
        }

        /// <summary>
        /// original Type : Double
        /// </summary>
        [DisplayName("Compression Perpendicular To Grain")]
        public string CompressionPerpendicularToGrain 
        {
            get
            {
                return GetParameterValueString(BuiltInParameter.PHY_MATERIAL_PARAM_COMPRESSION_PERPENDICULAR);
            }
            set
            {
                SetParameterValueString(BuiltInParameter.PHY_MATERIAL_PARAM_COMPRESSION_PERPENDICULAR, value);
            }
        }

        /// <summary>
        /// original Type : string
        /// </summary>
        public string Grade 
        {
            get
            {
                return GetParameterValueString(BuiltInParameter.PHY_MATERIAL_PARAM_GRADE);
            }
            set
            {
                SetParameterValueString(BuiltInParameter.PHY_MATERIAL_PARAM_GRADE, value);
            }
        }

        /// <summary>
        /// original Type : Double
        /// </summary>
        [DisplayName("Poisson Ratio")]
        public string PoissonRatio 
        {
            get
            {
                return GetParameterValueString(BuiltInParameter.PHY_MATERIAL_PARAM_POISSON_MOD);
            }
            set
            {
                SetParameterValueString(BuiltInParameter.PHY_MATERIAL_PARAM_POISSON_MOD, value);
            }
        }

        /// <summary>
        /// original Type : Double
        /// </summary>
        [DisplayName("Shear Modulus")]
        public string ShearModulus 
        {
            get
            {
                return GetParameterValueString(BuiltInParameter.PHY_MATERIAL_PARAM_SHEAR_MOD);
            }
            set
            {
                SetParameterValueString(BuiltInParameter.PHY_MATERIAL_PARAM_SHEAR_MOD, value);
            }
        }

        /// <summary>
        /// original Type : Double
        /// </summary>
        [DisplayName("Shear Parallel To Grain")]
        public string ShearParallelToGrain
        {
            get
            {
                return GetParameterValueString(BuiltInParameter.PHY_MATERIAL_PARAM_SHEAR_PARALLEL);
            }
            set
            {
                SetParameterValueString(BuiltInParameter.PHY_MATERIAL_PARAM_SHEAR_PARALLEL, value);
            }
        }

        /// <summary>
        /// original Type : Double
        /// </summary>
        [DisplayName("Tension Perpendicular To Grain")]
        public string TensionPerpendicularToGrain
        {
            get
            {
                return GetParameterValueString(BuiltInParameter.PHY_MATERIAL_PARAM_SHEAR_PERPENDICULAR);
            }
            set
            {
                SetParameterValueString(BuiltInParameter.PHY_MATERIAL_PARAM_SHEAR_PERPENDICULAR, value);
            }
        }

        /// <summary>
        /// original Type : string
        /// </summary>
        public string Species 
        {
            get
            {
                return GetParameterValueString(BuiltInParameter.PHY_MATERIAL_PARAM_SPECIES);
            }
            set
            {
                SetParameterValueString(BuiltInParameter.PHY_MATERIAL_PARAM_SPECIES, value);
            }
        }

        /// <summary>
        /// original Type : Double
        /// </summary>
        [DisplayName("Thermal Expansion Coefficient")]
        public string ThermalExpansionCoefficient 
        {
            get
            {
                return GetParameterValueString(BuiltInParameter.PHY_MATERIAL_PARAM_EXP_COEFF);
            }
            set
            {
                SetParameterValueString(BuiltInParameter.PHY_MATERIAL_PARAM_EXP_COEFF, value);
            }
        }

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

        /// <summary>
        /// original Type : Double
        /// </summary>
        [DisplayName("Young Modulus")]
        public string YoungModulus 
        {
            get
            {
                return GetParameterValueString(BuiltInParameter.PHY_MATERIAL_PARAM_YOUNG_MOD);
            }
            set
            {
                SetParameterValueString(BuiltInParameter.PHY_MATERIAL_PARAM_YOUNG_MOD, value);
            }
        }

        /// <summary>
        /// MaterialWoodParameters's constructor 
        /// </summary>
        /// <param name="materialWood">a instance of MaterialWood</param>
        public MaterialWoodParameters(MaterialWood materialWood)
            : base(materialWood)
        {
            m_materialWood = materialWood;
        }
    }
}
