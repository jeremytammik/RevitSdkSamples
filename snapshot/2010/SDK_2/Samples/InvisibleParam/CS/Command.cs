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
using System.IO;
using System.Reflection;

using Autodesk.Revit;
using Autodesk.Revit.Parameters;


namespace Revit.SDK.Samples.InvisibleParam.CS
{
    /// <summary>
    /// This command show how to use parameter file to store invisible and 
    /// visible parameters.
    /// </summary>
    public class Command : IExternalCommand
    {

        #region IExternalCommand Members
        /// <summary>
        /// Implement this method as an external command for Revit.
        /// </summary>
        /// <param name="commandData">An object that is passed to the external application
        /// which contains data related to the command,
        /// such as the application object and active view.</param>
        /// <param name="message">A message that can be set by the external application
        /// which will be displayed if a failure or cancellation is returned by
        /// the external command.</param>
        /// <param name="elements">A set of elements to which the external application
        /// can add elements that are to be highlighted in case of failure or cancellation.</param>
        /// <returns>Return the status of the external command.
        /// A result of Succeeded means that the API external method functioned as expected.
        /// Cancelled can be used to signify that the user cancelled the external operation 
        /// at some point. Failure should be returned if the application is unable to proceed with
        /// the operation.</returns>
        public IExternalCommand.Result Execute(
            ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                //Create a clear file as parameter file.
                String path = Assembly.GetExecutingAssembly().Location;
                int index = path.LastIndexOf("\\");
                String newPath = path.Substring(0,index);
                newPath += "\\RevitParameters.txt";
                if(File.Exists(newPath))
                {
                    File.Delete(newPath);
                }
                FileStream fs = File.Create(newPath);
                fs.Close();               

                //cache application handle
                Application revitApp = commandData.Application;
                //prepare shared parameter file
                commandData.Application.Options.SharedParametersFilename = newPath;

                //Open shared parameter file
                DefinitionFile parafile = revitApp.OpenSharedParameterFile();

                //get walls category
                Category wallCat = revitApp.ActiveDocument.Settings.Categories.get_Item(BuiltInCategory.OST_Walls);
                CategorySet categories = revitApp.Create.NewCategorySet();
                categories.Insert(wallCat);

                InstanceBinding binding = revitApp.Create.NewInstanceBinding(categories);

                //Create a group
                DefinitionGroup apiGroup = parafile.Groups.Create("APIGroup");

                //Create a visible "VisibleParam" of text type.
                Definition visibleParamDef = apiGroup.Definitions.Create
                    ("VisibleParam", ParameterType.Text, true);
                                        ;
                BindingMap bindingMap = revitApp.ActiveDocument.ParameterBindings;
                bindingMap.Insert(visibleParamDef, binding);

                //Create a invisible "InvisibleParam" of text type.
                Definition invisibleParamDef = apiGroup.Definitions.Create
                    ("InvisibleParam", ParameterType.Text, false);
                bindingMap.Insert(invisibleParamDef, binding);
            }
            catch(Exception e)
            {
                message = e.ToString();
                return IExternalCommand.Result.Cancelled;
            }
            return IExternalCommand.Result.Succeeded;
        }

        #endregion
    }
}
