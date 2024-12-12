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
using System.ComponentModel;
using Autodesk.Revit.Utility;
using Autodesk.Revit.DB;

namespace Revit.SDK.Samples.Materials.CS
{
    /// <summary>
    /// A description of a property consists of a name, its attributes and value
    /// here AssetPropertyPropertyDescriptor is used to wrap AssetProperty 
    /// to display its name and value in PropertyGrid
    /// </summary>
    internal class AssetPropertyPropertyDescriptor : PropertyDescriptor
    {
        #region Fields
        /// <summary>
        /// A reference to an AssetProperty
        /// </summary>
        private AssetProperty m_assetProperty;
        
        /// <summary>
        /// The type of AssetProperty's property "Value"
        /// </summary>
        private Type m_valueType;

        /// <summary>
        /// The value of AssetProperty's property "Value"
        /// </summary>
        private Object m_value;
        #endregion

        #region Properties
        /// <summary>
        /// Property to get internal AssetProperty
        /// </summary>
        public AssetProperty AssetProperty
        {
            get { return m_assetProperty; }
        }
        #endregion

        #region override Properties
        /// <summary>
        /// Gets a value indicating whether this property is read-only
        /// </summary>
        public override bool IsReadOnly 
        { 
            get 
            { 
                return true;
            } 
        }

        /// <summary>
        /// Gets the type of the component this property is bound to. 
        /// </summary>
        public override Type ComponentType
        {
            get 
            { 
                return m_assetProperty.GetType();
            }
        }

        /// <summary>
        /// Gets the type of the property. 
        /// </summary>
        public override Type PropertyType 
        { 
            get 
            { 
                return m_valueType; 
            }
        }
        #endregion

        /// <summary>
        /// Public class constructor
        /// </summary>
        /// <param name="assetProperty">the AssetProperty which a AssetPropertyPropertyDescriptor instance describes</param>
        public AssetPropertyPropertyDescriptor(AssetProperty assetProperty)
            : base(assetProperty.Name, new Attribute[0])
        {
            m_assetProperty = assetProperty;
        }

        #region override methods
        /// <summary>
        /// Compares this to another object to see if they are equivalent
        /// </summary>
        /// <param name="obj">The object to compare to this AssetPropertyPropertyDescriptor. </param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            AssetPropertyPropertyDescriptor other = obj as AssetPropertyPropertyDescriptor;
            return other != null && other.AssetProperty.Equals(m_assetProperty);
        }

        /// <summary>
        /// Returns the hash code for this object.
        /// Here override the method "Equals", so it is necessary to override GetHashCode too.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() 
        { 
            return m_assetProperty.GetHashCode();
        }

        /// <summary>
        /// Resets the value for this property of the component to the default value. 
        /// </summary>
        /// <param name="component">The component with the property value that is to be reset to the default value.</param>
        public override void ResetValue(object component) 
        {

        }

        /// <summary>
        /// Returns whether resetting an object changes its value. 
        /// </summary>
        /// <param name="component">The component to test for reset capability.</param>
        /// <returns>true if resetting the component changes its value; otherwise, false.</returns>
        public override bool CanResetValue(object component) 
        { 
            return false;
        }

        /// <summary>
        /// Determines a value indicating whether the value of this property needs to be persisted.
        /// </summary>
        /// <param name="component">The component with the property to be examined for persistence.</param>
        /// <returns>true if the property should be persisted; otherwise, false.</returns>
        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }

        /// <summary>
        /// Gets the current value of the property on a component.
        /// </summary>
        /// <param name="component">The component with the property for which to retrieve the value.</param>
        /// <returns>The value of a property for a given component.</returns>
        public override object GetValue(object component)
        {
            //For each AssetProperty, it has different type and value
            //must deal with it separately
            try
            {
                if (m_assetProperty is AssetPropertyBoolean)
                {
                    AssetPropertyBoolean property = m_assetProperty as AssetPropertyBoolean;
                    m_valueType = typeof(Boolean);
                    m_value = property.Value;
                }
                else if (m_assetProperty is AssetPropertyDistance)
                {
                    AssetPropertyDistance property = m_assetProperty as AssetPropertyDistance;
                    m_valueType = typeof(Double);
                    m_value = property.Value;
                }
                else if (m_assetProperty is AssetPropertyDouble)
                {
                    AssetPropertyDouble property = m_assetProperty as AssetPropertyDouble;
                    m_valueType = typeof(Double);
                    m_value = property.Value;
                }
                else if (m_assetProperty is AssetPropertyDoubleArray2d)
                {
                    //Default, it is supported by PropertyGrid to display Double []
                    //Try to convert DoubleArray to Double []
                    AssetPropertyDoubleArray2d property = m_assetProperty as AssetPropertyDoubleArray2d;
                    m_valueType = typeof(Double[]);
                    m_value = GetSystemArray(property.Value);
                }
                else if (m_assetProperty is AssetPropertyDoubleArray3d)
                {
                    AssetPropertyDoubleArray3d property = m_assetProperty as AssetPropertyDoubleArray3d;
                    m_valueType = typeof(Double[]);
                    m_value = GetSystemArray(property.Value);
                }
                else if (m_assetProperty is AssetPropertyDoubleArray4d)
                {
                    AssetPropertyDoubleArray4d property = m_assetProperty as AssetPropertyDoubleArray4d;
                    m_valueType = typeof(Double[]);
                    m_value = GetSystemArray(property.Value);
                }
                else if (m_assetProperty is AssetPropertyDoubleMatrix44)
                {
                    AssetPropertyDoubleMatrix44 property = m_assetProperty as AssetPropertyDoubleMatrix44;
                    m_valueType = typeof(Double[]);
                    m_value = GetSystemArray(property.Value);
                }
                else if (m_assetProperty is AssetPropertyEnum)
                {
                    AssetPropertyEnum property = m_assetProperty as AssetPropertyEnum;
                    m_valueType = typeof(int);
                    m_value = property.Value;
                }
                else if (m_assetProperty is AssetPropertyFloat)
                {
                    AssetPropertyFloat property = m_assetProperty as AssetPropertyFloat;
                    m_valueType = typeof(float);
                    m_value = property.Value;
                }
                else if (m_assetProperty is AssetPropertyInteger)
                {
                    AssetPropertyInteger property = m_assetProperty as AssetPropertyInteger;
                    m_valueType = typeof(int);
                    m_value = property.Value;
                }
                else if (m_assetProperty is AssetPropertyReference)
                {
                    AssetPropertyReference property = m_assetProperty as AssetPropertyReference;
                    m_valueType = typeof(String);
                    m_value = property.Type;
                }
                else if (m_assetProperty is AssetPropertyString)
                {
                    AssetPropertyString property = m_assetProperty as AssetPropertyString;
                    m_valueType = typeof(String);
                    m_value = property.Value;
                }
                else if (m_assetProperty is AssetPropertyTime)
                {
                    AssetPropertyTime property = m_assetProperty as AssetPropertyTime;
                    m_valueType = typeof(DateTime);
                    m_value = property.Value;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
            return m_value;
        }

        /// <summary>
        /// Sets the value of the component to a different value.
        /// For AssetProperty, it is not allowed to set its value, so here just return.
        /// </summary>
        /// <param name="component">The component with the property value that is to be set. </param>
        /// <param name="value">The new value.</param>
        public override void SetValue(object component, object value)
        {
            return;
        }
        #endregion

        /// <summary>
        /// Convert Autodesk.Revit.DB.DoubleArray to Double [].
        /// For Double [] is supported by PropertyGrid.
        /// </summary>
        /// <param name="doubleArray">the original Autodesk.Revit.DB.DoubleArray </param>
        /// <returns>The converted Double []</returns>
        private Double[] GetSystemArray(DoubleArray doubleArray)
        {
            double [] values  = new double [doubleArray.Size];
            int index = 0;
            foreach(Double value in doubleArray)
            {
                values[index++] = value;
            }
            return values;
        }
    }

    /// <summary>
    /// supplies dynamic custom type information for an Asset while it is displayed in PropertyGrid.
    /// </summary>
    public class RenderAppearanceDescriptor : ICustomTypeDescriptor
    {
        #region Fields
        /// <summary>
        /// Reference to Asset
        /// </summary>
        Asset m_asset;

        /// <summary>
        /// Asset's property descriptors
        /// </summary>
        PropertyDescriptorCollection m_propertyDescriptors;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes Asset object
        /// </summary>
        /// <param name="asset">an Asset object</param>
        public RenderAppearanceDescriptor(Asset asset)
        {
            m_asset = asset;
            GetAssetProperties();
        }

        /// <summary>
        /// Initializes Asset object
        /// </summary>
        /// <param name="material">an Material where can get an Asset</param>
        public RenderAppearanceDescriptor(Material material)
        {
            m_asset = material.RenderAppearance;
            GetAssetProperties();
        }
        #endregion

        #region Methods
        #region ICustomTypeDescriptor Members

        /// <summary>
        /// Returns a collection of custom attributes for this instance of Asset.
        /// </summary>
        /// <returns>Asset's attributes</returns>
        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(m_asset, false);
        }

        /// <summary>
        /// Returns the class name of this instance of Asset.
        /// </summary>
        /// <returns>Asset's class name</returns>
        public string GetClassName()
        {
            return TypeDescriptor.GetClassName(m_asset, false);
        }

        /// <summary>
        /// Returns the name of this instance of Asset.
        /// </summary>
        /// <returns>The name of Asset</returns>
        public string GetComponentName()
        {
            return TypeDescriptor.GetComponentName(m_asset, false);
        }

        /// <summary>
        /// Returns a type converter for this instance of Asset.
        /// </summary>
        /// <returns>The converter of the Asset</returns>
        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(m_asset, false);
        }

        /// <summary>
        /// Returns the default event for this instance of Asset.
        /// </summary>
        /// <returns>An EventDescriptor that represents the default event for this object, 
        /// or null if this object does not have events.</returns>
        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(m_asset, false);
        }

        /// <summary>
        /// Returns the default property for this instance of Asset.
        /// </summary>
        /// <returns>A PropertyDescriptor that represents the default property for this object, 
        /// or null if this object does not have properties.</returns>
        public PropertyDescriptor GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(m_asset, false);
        }

        /// <summary>
        /// Returns an editor of the specified type for this instance of Asset.
        /// </summary>
        /// <param name="editorBaseType">A Type that represents the editor for this object. </param>
        /// <returns>An Object of the specified type that is the editor for this object, 
        /// or null if the editor cannot be found.</returns>
        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(m_asset, editorBaseType, false);
        }

        /// <summary>
        /// Returns the events for this instance of Asset using the specified attribute array as a filter.
        /// </summary>
        /// <param name="attributes">An array of type Attribute that is used as a filter. </param>
        /// <returns>An EventDescriptorCollection that represents the filtered events for this Asset instance.</returns>
        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(m_asset, attributes, false);
        }

        /// <summary>
        /// Returns the events for this instance of Asset.
        /// </summary>
        /// <returns>An EventDescriptorCollection that represents the events for this Asset instance.</returns>
        public EventDescriptorCollection GetEvents()
        {
            return TypeDescriptor.GetEvents(m_asset, false);
        }

        /// <summary>
        /// Returns the properties for this instance of Asset using the attribute array as a filter.
        /// </summary>
        /// <param name="attributes">An array of type Attribute that is used as a filter.</param>
        /// <returns>A PropertyDescriptorCollection that 
        /// represents the filtered properties for this Asset instance.</returns>
        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            return m_propertyDescriptors;
        }

        /// <summary>
        /// Returns the properties for this instance of Asset.
        /// </summary>
        /// <returns>A PropertyDescriptorCollection that represents the properties 
        /// for this Asset instance.</returns>
        public PropertyDescriptorCollection GetProperties()
        {            
            return m_propertyDescriptors;
        }

        /// <summary>
        /// Returns an object that contains the property described by the specified property descriptor.
        /// </summary>
        /// <param name="pd">A PropertyDescriptor that represents the property whose owner is to be found. </param>
        /// <returns>Asset object</returns>
        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return m_asset;
        }
        #endregion

        /// <summary>
        /// Get Asset's property descriptors
        /// </summary>
        private void GetAssetProperties()
        {
            if (null == m_propertyDescriptors)
            {
                m_propertyDescriptors = new PropertyDescriptorCollection(new AssetPropertyPropertyDescriptor[0]);
            }
            else
            {
                return;
            }

            //For each AssetProperty in Asset, create an AssetPropertyPropertyDescriptor.
            //It means that each AssetProperty will be a property of Asset
            for (int index = 0; index < m_asset.Size; index++)
            {
                AssetProperty assetProperty = m_asset[index];
                if(null != assetProperty)
                {
                    AssetPropertyPropertyDescriptor assetPropertyPropertyDescriptor = new AssetPropertyPropertyDescriptor(assetProperty);
                    m_propertyDescriptors.Add(assetPropertyPropertyDescriptor);  
                }              
            }
        }
        #endregion
    }

}
