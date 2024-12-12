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

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.ComponentModel;
using System.Windows.Forms;


using Autodesk.Revit;
using Autodesk.Revit.DB;

namespace Revit.SDK.Samples.Materials.CS
{
    /// <summary>
    /// Local material behaviour type enums
    /// </summary>
    public enum LocalMaterialBehaviourType
    {
        /// <summary>
        /// Isotropic for LocalMaterialBehaviourType
        /// </summary>
        Isotropic = 0,
        /// <summary>
        /// Orthotropic for LocalMaterialBehaviourType
        /// </summary>
        Orthotropic = 1,
    }

    /// <summary>
    /// base class which has a member Material
    /// and expose Material's properties according 
    /// to Material's type
    /// </summary>
    public class MaterialParameters
    {
        private Material m_material; // reference to Material

        /// <summary>
        /// Local material behaviour type
        /// </summary>
        protected LocalMaterialBehaviourType m_behavior; 

        /// <summary>
        /// delegate to reload material parameters
        /// </summary>
        /// <param name="materialParameters">Material parameter to be reloaded.</param>
        public delegate void ReloadMaterailParametersHandle(MaterialParameters materialParameters);

        /// <summary>
        /// event to reload material parameters
        /// </summary>
        public static event ReloadMaterailParametersHandle ReloadMaterialParametersEvent;

        #region General        
        /// <summary>
        /// original Type : Autodesk.Revit.Color
        /// </summary>
        [Category("General")]
        public System.Drawing.Color Color
        {
            get
            {
                System.Drawing.Color color =
                        System.Drawing.Color.FromArgb((int)GetParameterValue(BuiltInParameter.MATERIAL_PARAM_COLOR));
                return color; 
            }
            set
            {
                SetParameterValue(BuiltInParameter.MATERIAL_PARAM_COLOR,value.ToArgb());
            }
        }

        /// <summary>
        /// original Type : Autodesk.Revit.Color
        /// Use property to get and set for can not find corresponding BuiltInParameter
        /// </summary>
        [Category("General"),DisplayName("Cut Pattern")]
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
        /// original Type : Color
        /// Use property to get and set for can not find corresponding BuiltInParameter
        /// </summary>
        [Category("General"), DisplayName("Surface Pattern")]
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
                return (int)GetParameterValue(BuiltInParameter.MATERIAL_PARAM_TRANSPARENCY);
            }
            set
            {
                SetParameterValue(BuiltInParameter.MATERIAL_PARAM_TRANSPARENCY, value);
            }  
        }
        #endregion

        /// <summary>
        /// Get parameter's value
        /// </summary>
        /// <param name="builtInParameter">BuiltInParameter which need to get value</param>
        /// <returns>Object about parameter's value</returns>
        protected Object GetParameterValue(BuiltInParameter builtInParameter)
        {
            try
            {
                Parameter parameter = m_material.get_Parameter(builtInParameter);
                if (null != parameter)
                {
                    switch(parameter.StorageType)
                    {
                        case StorageType.Double:
                            return parameter.AsDouble();
                        case StorageType.ElementId:
                            return parameter.AsElementId();
                        case StorageType.Integer:
                            return parameter.AsInteger();
                        case StorageType.None:
                            return parameter.AsValueString();
                        case StorageType.String:
                            return parameter.AsString();
                        default:
                            break;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Set parameter's value
        /// </summary>
        /// <param name="builtInParameter">BuiltInParameter which should be set new value</param>
        /// <param name="newValue">new parameter value</param>
        protected void SetParameterValue(BuiltInParameter builtInParameter, Object newValue)
        {
            try
            {
                Parameter parameter = m_material.get_Parameter(builtInParameter);
                if (null != parameter)
                {
                    bool flag = false;
                    switch (parameter.StorageType)
                    {
                        case StorageType.Double:
                            Double newDouble = (Double)newValue;
                            flag = parameter.Set(newDouble);
                            break;
                        case StorageType.ElementId:
                            Autodesk.Revit.DB.ElementId newElementId = (Autodesk.Revit.DB.ElementId)newValue;
                            flag = parameter.Set(newElementId);
                            break;
                        case StorageType.Integer:
                            int newInt = (int)newValue;
                            flag = parameter.Set(newInt);
                            break;
                        case StorageType.None:
                            String newStringValue = newValue as String;
                            flag = parameter.Set(newStringValue);
                            break;
                        case StorageType.String:
                            String newString = newValue as String;
                            flag = parameter.Set(newString);
                            break;
                    }
                    if (!flag)
                    {
                        MessageBox.Show("Property value is not valid.", "Properties Window", MessageBoxButtons.OK, MessageBoxIcon.Warning);                       
                    }
                }
            }
            catch
            {
                return;
            }
        }

        /// <summary>
        /// Get parameter's value and convert to String
        /// </summary>
        /// <param name="builtInParameter">BuiltInParameter which need to get value</param>
        /// <returns>String about parameter's value</returns>
        protected String GetParameterValueString(BuiltInParameter builtInParameter)
        {
            try
            {
                Parameter parameter = m_material.get_Parameter(builtInParameter);
                if(null != parameter)
                {
                    return parameter.AsValueString();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Set parameter's value with String
        /// </summary>
        /// <param name="builtInParameter">BuiltInParameter which should be set new value</param>
        /// <param name="newStringValue">new parameter value</param>
        protected void SetParameterValueString(BuiltInParameter builtInParameter, String newStringValue)
        {
            try
            {
                Parameter parameter = m_material.get_Parameter(builtInParameter);
                if (null != parameter)
                {
                    if(!parameter.SetValueString(newStringValue))
                    {
                        MessageBox.Show("Property value is not valid.", "Properties Window", MessageBoxButtons.OK, MessageBoxIcon.Warning);                                             
                    }
                }
            }
            catch
            {
                return;
            }
        }

        /// <summary>
        /// Set property's ReadOnlyAttribute's value
        /// </summary>
        /// <param name="materialParameters">the instance of which property's ReadOnlyAttribute need to change </param>
        /// <param name="propertyName">name of property of which ReadOnlyAttribute need to change</param>
        /// <param name="readOnly">indicate ReadOnlyAttribute's value</param>
        protected void SetPropertyReadOnly(MaterialParameters materialParameters, string propertyName, bool readOnly)
        {
            if (null == materialParameters || null == propertyName)
            {
                return;
            }

            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(materialParameters);
            PropertyDescriptor propertyDescriptor = props[propertyName];

            Type attributeType = typeof(System.ComponentModel.ReadOnlyAttribute);
            Attribute readonlyAttri = propertyDescriptor.Attributes[attributeType];

            FieldInfo readOnlyField = attributeType.GetField("isReadOnly", BindingFlags.Instance | BindingFlags.CreateInstance | BindingFlags.NonPublic);
            readOnlyField.SetValue(readonlyAttri, readOnly);
            if (null != ReloadMaterialParametersEvent)
            {
                ReloadMaterialParametersEvent(materialParameters);
            }
        }

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
