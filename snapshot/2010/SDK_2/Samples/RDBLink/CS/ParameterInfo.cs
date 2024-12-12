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
using Autodesk.Revit.Enums;
using Autodesk.Revit.Parameters;
using Autodesk.Revit;
using System.Collections.Generic;

namespace Revit.SDK.Samples.RDBLink.CS
{
    /// <summary>
    /// Contains custom parameter information, used to create a new custom parameter
    /// </summary>
    public class ParameterInfo
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
        #endregion

        #region Constructors
        /// <summary>
        /// Initialize variables
        /// </summary>
        public ParameterInfo()
        {
            m_parameterGroup = BuiltInParameterGroup.INVALID;
            m_parameterType = ParameterType.Text;
        }

        /// <summary>
        /// Initialize variables
        /// </summary>
        /// <param name="definition">Definition</param>
        /// <param name="binding">ElementBinding</param>
        public ParameterInfo(Definition definition, ElementBinding binding)
        {
            m_parameterIsProject = definition is ExternalDefinition;
            m_parameterForType = binding is TypeBinding;
            m_parameterName = definition.Name;
            m_parameterGroup = definition.ParameterGroup;
            m_parameterType = definition.ParameterType;
            m_categories = binding.Categories;
        }
        #endregion
    };
}
