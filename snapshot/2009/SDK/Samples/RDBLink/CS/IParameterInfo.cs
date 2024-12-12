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
using Autodesk.Revit.Enums;
using Autodesk.Revit.Parameters;
using Autodesk.Revit;
using System.Collections.Generic;

namespace Revit.SDK.Samples.RDBLink.CS
{
    /// <summary>
    /// Contains information of a custom parameter
    /// </summary>
    public interface IParameterInfo
    {
        #region Properties
        /// <summary>
        /// Gets or sets whether the parameter is a project parameter or shared parameter
        /// </summary>
        bool ParameterIsProject { get; set;}

        /// <summary>
        /// Gets or sets whether the parameter is binded to type or instance
        /// </summary>
        bool ParameterForType { get; set;}

        /// <summary>
        /// Gets or sets the parameter name
        /// </summary>
        string ParameterName { get; set;}

        /// <summary>
        /// Gets or sets the parameter group
        /// </summary>
        BuiltInParameterGroup ParameterGroup { get; set;}

        /// <summary>
        /// Gets or sets the parameter type
        /// </summary>
        ParameterType ParameterType { get; set;}

        /// <summary>
        /// Gets or sets the parameter categories binded
        /// </summary>
        CategorySet Categories { get; set;}

        /// <summary>
        /// Gets or sets the parameter binding
        /// </summary>
        ElementBinding Binding { get; set;}

        /// <summary>
        /// Gets or sets the parameter definition
        /// </summary>
        Definition Definition { get; set;} 
        #endregion
    }

    /// <summary>
    /// Used to create a new custom parameter
    /// </summary>
    public class ParameterInfo : IParameterInfo
    {
        #region Fields
        /// <summary>
        /// Whether the parameter is a project parameter or shared parameter
        /// </summary>
        bool m_parameterIsProject;

        /// <summary>
        /// Whether the parameter is binded to type or instance
        /// </summary>
        bool m_parameterForType;

        /// <summary>
        /// Parameter name
        /// </summary>
        string m_parameterName;

        /// <summary>
        /// Parameter group
        /// </summary>
        BuiltInParameterGroup m_parameterGroup;

        /// <summary>
        /// Parameter type
        /// </summary>
        ParameterType m_parameterType;

        /// <summary>
        /// Parameter categories binded
        /// </summary>
        CategorySet m_categories;

        /// <summary>
        /// Parameter binding
        /// </summary>
        ElementBinding m_binding;

        /// <summary>
        /// Parameter definition
        /// </summary>
        Definition m_definition; 
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets whether the parameter is a project parameter or shared parameter
        /// </summary>
        public bool ParameterIsProject
        {
            get { return m_parameterIsProject; }
            set { m_parameterIsProject = value; }
        }

        /// <summary>
        /// Gets or sets whether the parameter is binded to type or instance
        /// </summary>
        public bool ParameterForType
        {
            get { return m_parameterForType; }
            set { m_parameterForType = value; }
        }

        /// <summary>
        /// Gets or sets the parameter name
        /// </summary>
        public string ParameterName
        {
            get { return m_parameterName; }
            set { m_parameterName = value; }
        }

        /// <summary>
        /// Gets or sets the parameter group
        /// </summary>
        public BuiltInParameterGroup ParameterGroup
        {
            get { return m_parameterGroup; }
            set { m_parameterGroup = value; }
        }

        /// <summary>
        /// Gets or sets the parameter type
        /// </summary>
        public ParameterType ParameterType
        {
            get { return m_parameterType; }
            set { m_parameterType = value; }
        }

        /// <summary>
        /// Gets or sets the parameter categories binded
        /// </summary>
        public CategorySet Categories
        {
            get { return m_categories; }
            set { m_categories = value; }
        }

        /// <summary>
        /// Gets or sets the parameter binding
        /// </summary>
        public ElementBinding Binding
        {
            get { return m_binding; }
            set { m_binding = value; }
        }

        /// <summary>
        /// Gets or sets the parameter definition
        /// </summary>
        public Definition Definition
        {
            get { return m_definition; }
            set { m_definition = value; }
        } 
        #endregion

        #region Constructors
        /// <summary>
        /// Initialize some variables
        /// </summary>
        public ParameterInfo()
        {
            m_parameterGroup = BuiltInParameterGroup.INVALID;
            m_parameterType = ParameterType.Text;
        } 
        #endregion
    };

    /// <summary>
    /// ParameterInfo binded with Definition and ElementBinding
    /// </summary>
    public class ParameterInfoBinded : IParameterInfo
    {
        #region Fields
        /// <summary>
        /// Parameter binding
        /// </summary>
        ElementBinding m_binding;

        /// <summary>
        /// Parameter definition
        /// </summary>
        Definition m_definition; 
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets whether the parameter is a project parameter or shared parameter
        /// </summary>
        public bool ParameterIsProject
        {
            get { return m_definition is ExternalDefinition; }
            set { }
        }

        /// <summary>
        /// Gets or sets whether the parameter is binded to type or instance
        /// </summary>
        public bool ParameterForType
        {
            get { return m_binding is TypeBinding; }
            set { }
        }

        /// <summary>
        /// Gets or sets the parameter name
        /// </summary>
        public string ParameterName
        {
            get { return m_definition.Name; }
            set { /*m_definition.Name = value;*/ }
        }


        /// <summary>
        /// Gets or sets the parameter group
        /// </summary>
        public BuiltInParameterGroup ParameterGroup
        {
            get { return m_definition.ParameterGroup; }
            set { /*m_definition.ParameterGroup = value;*/ }
        }

        /// <summary>
        /// Gets or sets the parameter type
        /// </summary>
        public ParameterType ParameterType
        {
            get { return m_definition.ParameterType; }
            set { /*m_definition.ParameterType = value;*/ }
        }

        /// <summary>
        /// Gets or sets the parameter categories binded
        /// </summary>
        public CategorySet Categories
        {
            get { return m_binding.Categories; }
            set { m_binding.Categories = value; }
        }

        /// <summary>
        /// Gets or sets the parameter binding
        /// </summary>
        public ElementBinding Binding
        {
            get { return m_binding; }
            set { m_binding = value; }
        }

        /// <summary>
        /// Gets or sets the parameter definition
        /// </summary>
        public Definition Definition
        {
            get { return m_definition; }
            set { m_definition = value; }
        } 
        #endregion

        #region Constructors
        /// <summary>
        /// Initialize Binding and Definition
        /// </summary>
        /// <param name="definition">definition</param>
        /// <param name="binding">binding</param>
        public ParameterInfoBinded(Definition definition, ElementBinding binding)
        {
            m_definition = definition;
            m_binding = binding;
        }
        #endregion
    };
}
