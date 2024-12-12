//
// (C) Copyright 2003-2014 by Autodesk, Inc.
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
using Autodesk.Revit.DB;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB.Structure;
using System.Reflection;
using System.IO;

namespace Revit.SDK.Samples.MultiplanarRebar.CS
{
    /// <summary>
    /// This is an utility class used to create shared parameter in Revit Document.
    /// It simplifies the process of shared parameters creation.
    /// </summary>
    class SharedParameterUtil
    {
        /// <summary>
        /// Get existed or create a new shared parameters with the given name and Revit DB Document.
        /// </summary>
        /// <param name="name">Shared parameter name</param>
        /// <param name="revitDoc">Revit DB Document</param>
        /// <returns>ElementId of get or created shared parameter</returns>
        public static ElementId GetOrCreateDef(string name, Document revitDoc)
        {
            ExternalDefinition ed = GetOrCreateDef(name, revitDoc.Application);
            return RebarShapeParameters.GetOrCreateElementIdForExternalDefinition(revitDoc, ed);
        }

        /// <summary>
        /// Get existed or create a new shared parameters with the given name and Revit DB Application.
        /// </summary>
        /// <param name="name">Shared parameter name</param>
        /// <param name="revitApp">Revit DB Application</param>
        /// <returns>ExternalDefinition of get or created shared parameter</returns>
        public static ExternalDefinition GetOrCreateDef(string name, Application revitApp)
        {
            return GetOrCreateDef(name, "Rebar Shape", revitApp);
        }
 
        /// <summary>
        /// Get existed or create a new shared parameters with the given name, group and Revit DB Application.
        /// </summary>
        /// <param name="name">Shared parameter name</param>
        /// <param name="groupName">Shared parameter group name</param>
        /// <param name="revitApp">Revit DB Application</param>
        /// <returns>ExternalDefinition of get or created shared parameter</returns>
        public static ExternalDefinition GetOrCreateDef(string name, string groupName, Application revitApp)
        {
            DefinitionFile parameterFile = GetSharedParameterFile(revitApp);

            DefinitionGroup group = parameterFile.Groups.get_Item(groupName);
            if (group == null)
                group = parameterFile.Groups.Create(groupName);

            ExternalDefinition Bdef = group.Definitions.get_Item(name) as ExternalDefinition;
            if (Bdef == null)
            {
               ExternalDefinitonCreationOptions externalDefinitonCreationOptions = new ExternalDefinitonCreationOptions(name, ParameterType.ReinforcementLength);
               Bdef = group.Definitions.Create(externalDefinitonCreationOptions) as ExternalDefinition;
            }

            return Bdef;
        }

        /// <summary>
        /// Get shared parameter DefinitionFile of given Revit DB Application.
        /// </summary>
        /// <param name="revitApp">Revit DB Application</param>
        /// <returns>DefinitionFile of Revit DB Application</returns>
        public static DefinitionFile GetSharedParameterFile(Application revitApp)
        {
            DefinitionFile file = null;

            int count = 0;
            // A count is to avoid infinite loop
            while (null == file && count < 100)
            {
                file = revitApp.OpenSharedParameterFile();
                if (file == null)
                {
                    // If Shared parameter file does not exist, then create a new one.
                    string shapeFile = 
                        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) 
                        + "\\MultiplanarParameterFiles.txt";                        

                    // Fill Schema data of Revit shared parameter file.
                    // If no this schema data, OpenSharedParameterFile may alway return null.
                    System.Text.StringBuilder contents = new System.Text.StringBuilder();
                    contents.AppendLine("# This is a Revit shared parameter file.");
                    contents.AppendLine("# Do not edit manually.");
                    contents.AppendLine("*META	VERSION	MINVERSION");
                    contents.AppendLine("META	2	1");
                    contents.AppendLine("*GROUP	ID	NAME");
                    contents.AppendLine("*PARAM	GUID	NAME	DATATYPE	DATACATEGORY	GROUP	VISIBLE");

                    // Write Schema data of Revit shared parameter file.
                    File.WriteAllText(shapeFile, contents.ToString());

                    // Set Revit shared parameter file
                    revitApp.SharedParametersFilename = shapeFile;
                }

                // To avoid infinite loop.
                ++count;
            }            

            return file;
        }
    }

}
