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
using System.Reflection;

namespace Revit.SDK.Samples.ProjectInfo.CS
{
    /// <summary>
    /// 
    /// </summary>
    public class WrapperCustomDescriptor : ICustomTypeDescriptor, IWrapper
    {
        #region Fields
        /// <summary>
        /// Handle object
        /// </summary>
        object m_handle = null; 
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes handle object
        /// </summary>
        /// <param name="handle">Handle object</param>
        public WrapperCustomDescriptor(object handle)
        {
            m_handle = handle;
        } 
        #endregion

        #region Properties

        /// <summary>
        /// Gets handle object
        /// </summary>
        public object Handle
        {
            get { return m_handle; }
        }

        /// <summary>
        /// Gets the name of the handle object if it has the Name property,
        /// otherwise returns Handle.ToString().
        /// </summary>
        public string Name
        {
            get
            {
                MethodInfo mi = this.Handle.GetType().GetMethod("get_Name", new Type[0]);
                if (mi != null)
                {
                    object name = mi.Invoke(this.Handle, new object[0]);
                    if (name != null)
                    {
                        return name.ToString();
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
                return Handle.ToString();
            }
        }

        #endregion

        #region Methods
        #region ICustomTypeDescriptor Members

        /// <summary>
        /// Returns a collection of custom attributes for this instance of a component.
        /// </summary>
        /// <returns>Handle's attributes</returns>
        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(m_handle, false);
        }

        /// <summary>
        /// Returns the class name of this instance of a component.
        /// </summary>
        /// <returns>Handle's class name</returns>
        public string GetClassName()
        {
            return TypeDescriptor.GetClassName(m_handle, false);
        }

        /// <summary>
        /// Returns the name of this instance of a component.
        /// </summary>
        /// <returns>The name of handle object</returns>
        public string GetComponentName()
        {
            return TypeDescriptor.GetComponentName(m_handle, false);
        }

        /// <summary>
        /// Returns a type converter for this instance of a component.
        /// </summary>
        /// <returns>The converter of the handle</returns>
        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(m_handle, false);
        }

        /// <summary>
        /// Returns the default event for this instance of a component.
        /// </summary>
        /// <returns>An EventDescriptor that represents the default event for this object, 
        /// or null if this object does not have events.</returns>
        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(m_handle, false);
        }

        /// <summary>
        /// Returns the default property for this instance of a component.
        /// </summary>
        /// <returns>A PropertyDescriptor that represents the default property for this object, 
        /// or null if this object does not have properties.</returns>
        public PropertyDescriptor GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(m_handle, false);
        }

        /// <summary>
        /// Returns an editor of the specified type for this instance of a component.
        /// </summary>
        /// <param name="editorBaseType">A Type that represents the editor for this object. </param>
        /// <returns>An Object of the specified type that is the editor for this object, 
        /// or null if the editor cannot be found.</returns>
        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(m_handle, editorBaseType, false);
        }

        /// <summary>
        /// Returns the events for this instance of a component using the specified attribute array as a filter.
        /// </summary>
        /// <param name="attributes">An array of type Attribute that is used as a filter. </param>
        /// <returns>An EventDescriptorCollection that represents the filtered events for this component instance.</returns>
        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(m_handle, attributes, false);
        }

        /// <summary>
        /// Returns the events for this instance of a component.
        /// </summary>
        /// <returns>An EventDescriptorCollection that represents the events for this component instance.</returns>
        public EventDescriptorCollection GetEvents()
        {
            return TypeDescriptor.GetEvents(m_handle, false);
        }

        /// <summary>
        /// Returns the properties for this instance of a component using the attribute array as a filter.
        /// </summary>
        /// <param name="attributes">An array of type Attribute that is used as a filter.</param>
        /// <returns>A PropertyDescriptorCollection that 
        /// represents the filtered properties for this component instance.</returns>
        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            // get handle's properties
            PropertyDescriptorCollection collection = TypeDescriptor.GetProperties(m_handle, attributes, false);
            // create empty collection
            PropertyDescriptorCollection collection2 = new PropertyDescriptorCollection(new PropertyDescriptor[0]);

            // filter properties by RevitVersionAttribute.
            // if there is RevitVersionAttribute specified and the designated names does not 
            // contain current Revit version, the property will not be exposed.
            foreach (PropertyDescriptor pd in collection)
            {
                bool matchRevitVersion = true;
                foreach (Attribute att in pd.Attributes)
                {
                    RevitVersionAttribute pfa = att as RevitVersionAttribute;
                    if (pfa != null)
                    {
                        if (!pfa.Names.Contains(RevitStartInfo.RevitProduct))
                            matchRevitVersion = false;
                        break;
                    }
                }
                if (matchRevitVersion)
                    collection2.Add(pd);
            }
            return collection2;
        }

        /// <summary>
        /// Returns the properties for this instance of a component.
        /// </summary>
        /// <returns>A PropertyDescriptorCollection that represents the properties 
        /// for this component instance.</returns>
        public PropertyDescriptorCollection GetProperties()
        {
            return TypeDescriptor.GetProperties(m_handle, false);
        }

        /// <summary>
        /// Returns an object that contains the property described by the specified property descriptor.
        /// </summary>
        /// <param name="pd">A PropertyDescriptor that represents the property whose owner is to be found. </param>
        /// <returns>Handle object</returns>
        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return m_handle;
        }

        #endregion

        /// <summary>
        /// overrides ToString method
        /// </summary>
        /// <returns>The name of the handle object</returns>
        public override string ToString()
        {
            return this.Name;
        } 
        #endregion
    }
}
