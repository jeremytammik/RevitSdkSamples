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
using System.IO;
using Autodesk.Revit.Parameters;
using Autodesk.Revit;
using Autodesk.Revit.Enums;

namespace Revit.SDK.Samples.RDBLink.CS
{
    /// <summary>
    /// Provides functions to create custom parameters
    /// </summary>
    public class ParameterCreation
    {
        #region Fields
        /// <summary>
        /// Revit Application
        /// </summary>
        Application m_revitApp; 
        #endregion

        #region Constructors
        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="revitApp">Revit Application</param>
        public ParameterCreation(Application revitApp)
        {
            m_revitApp = revitApp;
        } 
        #endregion

        #region Methods
        /// <summary>
        /// Create a custom parameter
        /// </summary>
        /// <param name="parameterInfo">IParameterInfo stores the information to create parameter</param>
        /// <returns>true if created successfully, otherwise false</returns>
        public bool CreateUserDefinedParameter(ParameterInfo parameterInfo)
        {
            // currently project parameter can not be created via API
            if (parameterInfo.ParameterIsProject) return false;

            m_revitApp.ActiveDocument.BeginTransaction();

            // get a group
            DefinitionGroup group = GetSharedGroup();

            // get a definition from the group
            Definition definition = group.Definitions.get_Item(parameterInfo.ParameterName);
            if (definition == null)
            {
                // if the definition does not exist, create one
                definition = group.Definitions.Create(parameterInfo.ParameterName, parameterInfo.ParameterType);
            }

            // if the parameter is of type, create a type binding object, otherwise create an instance one.
            ElementBinding binding = null;
            if (parameterInfo.ParameterForType)
            {
                binding = m_revitApp.Create.NewTypeBinding(parameterInfo.Categories);
            }
            else
            {
                binding = m_revitApp.Create.NewInstanceBinding(parameterInfo.Categories);
            }

            // insert the binding and definition to current document
            bool insertSuccess = m_revitApp.ActiveDocument.ParameterBindings.Insert(definition, binding, parameterInfo.ParameterGroup);
            if (!insertSuccess) return false;

            m_revitApp.ActiveDocument.EndTransaction();
            return true;
        }

        /// <summary>
        /// Get a group to store shared parameters
        /// </summary>
        /// <returns>Group contains shared parameters</returns>
        private DefinitionGroup GetSharedGroup()
        {
            // get shared parameters file
            string SharedParametersFilename = GetSharedParametersFile();
            m_revitApp.Options.SharedParametersFilename = SharedParametersFilename;

            // open the file
            DefinitionFile definitionFile = m_revitApp.OpenSharedParameterFile();
            if (definitionFile == null)
                return null;

            // get groups
            DefinitionGroups groups = definitionFile.Groups;

            // get a specific group
            DefinitionGroup group = null;
            group = groups.get_Item("RDBParameters");

            // if the group does not exist, create one
            if (null == group)
            {
                groups.Create("RDBParameters");
                group = groups.get_Item("RDBParameters");
            }
            return group;
        }

        /// <summary>
        /// Whether a shared parameter file exist, if not, create one
        /// </summary>
        /// <returns>Full path of the file</returns>
        private string GetSharedParametersFile()
        {
            string path = Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
            string fullPath = path + "\\RDBLink_SharedParameters.txt";

            if (!File.Exists(fullPath))
            {
                FileStream file = File.Create(fullPath);
                file.Close();
            }
            return fullPath;
        } 
        #endregion
    }
}
