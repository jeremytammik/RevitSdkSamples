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
                return DoubleToString(m_materialWood.Bending, UnitType.Stress);
            }
            set
            {
                double newValue;
                if (StringToDouble(value, out newValue, UnitType.Stress))
                {
                    m_materialWood.Bending = newValue;
                }
            }
        }

        /// <summary>
        /// original Type : Double
        /// </summary>
        public string CompressionParallelToGrain
        {
            get
            {
                return DoubleToString(m_materialWood.CompressionParallel, UnitType.Stress);
            }
            set
            {
                double newValue;
                if (StringToDouble(value, out newValue, UnitType.Stress))
                {
                    m_materialWood.CompressionParallel = newValue;
                }
            }
        }

        /// <summary>
        /// original Type : Double
        /// </summary>
        public string CompressionPerpendicularToGrain 
        {
            get
            {
                return DoubleToString(m_materialWood.CompressionPerpendicular,
                                        UnitType.Stress);
            }
            set
            {
                double newValue;
                if (StringToDouble(value, out newValue, UnitType.Stress))
                {
                    m_materialWood.CompressionPerpendicular = newValue;
                }
            }
        }

        /// <summary>
        /// original Type : string
        /// </summary>
        public string Grade 
        {
            get
            {
                return m_materialWood.Grade;
            }
            set
            {
                m_materialWood.Grade = value;
            }
        }

        /// <summary>
        /// original Type : Double
        /// </summary>
        public string PoissonRatio 
        {
            get
            {
                return DoubleToString(m_materialWood.PoissonModulus, UnitType.No);
            }
            set
            {
                double newValue;
                if (StringToDouble(value, out newValue, UnitType.No))
                {
                    m_materialWood.PoissonModulus = newValue;
                }
            }
        }

        /// <summary>
        /// original Type : Double
        /// </summary>
        public string ShearModulus 
        {
            get
            {
                return DoubleToString(m_materialWood.ShearModulus, UnitType.Stress);
            }
            set
            {
                double newValue;
                if (StringToDouble(value, out newValue, UnitType.Stress))
                {
                    m_materialWood.ShearModulus = newValue;
                }
            }
        }

        /// <summary>
        /// original Type : Double
        /// </summary>
        public string ShearParallelToGrain
        {
            get
            {
                return DoubleToString(m_materialWood.ShearParallel, UnitType.Stress);
            }
            set
            {
                double newValue;
                if (StringToDouble(value, out newValue, UnitType.Stress))
                {
                    m_materialWood.ShearParallel = newValue;
                }
            }
        }

        /// <summary>
        /// original Type : Double
        /// </summary>
        public string TensionPerpendicularToGrain
        {
            get
            {
                return DoubleToString(m_materialWood.ShearPerpendicular, UnitType.Stress);
            }
            set
            {
                double newValue;
                if (StringToDouble(value, out newValue, UnitType.Stress))
                {
                    m_materialWood.ShearPerpendicular = newValue;
                }
            }
        }

        /// <summary>
        /// original Type : string
        /// </summary>
        public string Species 
        {
            get
            {
                return m_materialWood.Species;
            }
            set
            {
                m_materialWood.Species = value;
            }
        }

        /// <summary>
        /// original Type : Double
        /// </summary>
        public string ThermalExpansionCoefficient 
        {
            get
            {
                return DoubleToString(m_materialWood.ThermalExpansionCoefficient,
                                        UnitType.Temperature);
            }
            set
            {
                double newValue;
                if (StringToDouble(value, out newValue, UnitType.Temperature))
                {
                    m_materialWood.ThermalExpansionCoefficient = newValue;
                }
            }
        }

        /// <summary>
        /// original Type : Double
        /// </summary>
        public string UnitWeight
        {
            get
            {
                return DoubleToString(m_materialWood.UnitWeight, UnitType.UnitWeight);
            }
            set
            {
                double newValue;
                if (StringToDouble(value, out newValue, UnitType.UnitWeight))
                {
                    m_materialWood.UnitWeight = newValue;
                }
            }
        }

        /// <summary>
        /// original Type : Double
        /// </summary>
        public string YoungModulus 
        {
            get
            {
                return DoubleToString(m_materialWood.YoungModulus, UnitType.Stress);
            }
            set
            {
                double newValue;
                if (StringToDouble(value, out newValue, UnitType.Stress))
                {
                    m_materialWood.YoungModulus = newValue;
                }
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
