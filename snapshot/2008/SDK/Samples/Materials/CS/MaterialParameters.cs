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
using System.Reflection;
using System.Diagnostics;
using System.ComponentModel;


using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Parameters;
using Autodesk.Revit.Enums;

namespace Revit.SDK.Samples.Materials.CS
{
 
    /// <summary>
    /// base class which has a member Material
    /// and expose Material's properties according 
    /// to Material's type
    /// </summary>
    public class MaterialParameters
    {
        private Material m_material; // reference to Material

        protected MaterialBehaviourType m_behavior; //if Behavior == Orthotropic,true;
                                                    //otherwise false


        /// <summary>
        /// convert double into string include dealing with precision, ratio
        /// and adding unit after value
        /// </summary>
        protected string DoubleToString(double value)
        {
            return DoubleToString(value, UnitType.No);
        }

        /// <summary>
        /// convert double into string include dealing with precision, ratio 
        /// and add unit after value
        /// </summary>
        protected string DoubleToString(double value, UnitType unitType)
        {
            //use UnitConversion.ConvertTo to deal with.
            string displayText = UnitConversion.ConvertFrom(value, unitType);
            return displayText;
        }

        /// <summary>
        /// convert string into double include dealing with ratio
        /// </summary>
        protected bool StringToDouble(string value, out double newValue)
        {
            return StringToDouble(value, out newValue, UnitType.No);
        }

        /// <summary>
        /// convert string into double include dealing with ratio
        /// </summary>
        protected bool StringToDouble(string value, out double newValue, UnitType unitType)
        {
            //use UnitConversion.ConvertTo to deal with.
            //if success,return true,otherwise, return false
            if (UnitConversion.ConvertTo(value, out newValue, unitType))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #region General        
        /// <summary>
        /// original Type : Autodesk.Revit.Color
        /// </summary>
        [Category("General")]
        public System.Drawing.Color Color
        {
            get
            {
                try
                {
                    //convert Autodesk.Revit.Color into System.Drawing.Color
                    System.Drawing.Color color =
                        System.Drawing.Color.FromArgb(m_material.Color.Red,
                        m_material.Color.Green, m_material.Color.Blue);                    
                    return color;                   
                }
                catch (Exception)
                {
                    return System.Drawing.Color.Empty;
                }
            }
            set
            {
                //convert System.Drawing.Color into Autodesk.Revit.Color
                Color color = new Color(value.R, value.G, value.B);
                if (null == color)
                {
                    return;
                }
                m_material.Color = color;
            }
        }

        /// <summary>
        /// original Type : Autodesk.Revit.Color
        /// </summary>
        [Category("General")]
        public System.Drawing.Color CutPatternColor
        {   
            get
            {
                try
                {
                    //convert Autodesk.Revit.Color into System.Drawing.Color
                    System.Drawing.Color color =
                        System.Drawing.Color.FromArgb(m_material.CutPatternColor.Red,
                        m_material.CutPatternColor.Green, m_material.CutPatternColor.Blue);                    
                    return color;                   
                }
                catch (Exception)
                {
                    return System.Drawing.Color.Empty;
                }
            }
            set
            {
                //convert System.Drawing.Color into Autodesk.Revit.Color
                Color color = new Color(value.R, value.G, value.B);
                if (null == color)
                {
                    return;
                }
                m_material.CutPatternColor = color;

            }
        }

        /// <summary>
        /// original Type : bool
        /// </summary>
        [Category("General")]
        public bool Glow
        {
            get
            {
                return m_material.Glow;
            }
            set
            {
                m_material.Glow = value;
            }
        }

        /// <summary>
        /// original Type : int
        /// </summary> 
        [Category("General")]
        public int Shininess
        {
            get
            {
                return m_material.Shininess;                
            }
            set
            {
                m_material.Shininess = value;
            }           
        }

        /// <summary>
        /// original Type : int
        /// </summary>
        [Category("General")]
        public int Smoothness
        {
            get
            {
                return m_material.Smoothness; 
            }
            set
            {
                m_material.Smoothness = value;
            }
        }

        /// <summary>
        /// original Type : Color
        /// </summary>
        [Category("General")]
        public System.Drawing.Color SurfacePatternColor
        {
            get
            {
                try
                {
                    System.Drawing.Color color =
                        System.Drawing.Color.FromArgb(m_material.SurfacePatternColor.Red,
                        m_material.SurfacePatternColor.Green, m_material.SurfacePatternColor.Blue);
                    return color;
                }
                catch (Exception)
                {
                    return System.Drawing.Color.Empty;
                }
            }
            set
            {
                Color color = new Color(value.R, value.G, value.B);
                if (null == color)
                {
                    return;
                }
                m_material.SurfacePatternColor = color;
            }
        }

        /// <summary>
        /// original Type : int
        /// </summary>
        [Category("General")]
        public int Transparency
        {
            get
            {
                return m_material.Transparency;
            }
            set
            {                
                                       
                m_material.Transparency = value;               
            }
        }
        #endregion


        /// <summary>
        /// Material's constructor 
        /// </summary>
        /// <param name="material">a instance of Material's derived classes</param>
        public MaterialParameters(Material material)
        {
            m_material = material;
        }        
    }
}
