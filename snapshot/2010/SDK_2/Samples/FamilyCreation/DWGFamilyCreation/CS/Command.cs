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
using System.Collections.Generic;
using System.Text;

using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Geometry;
using Element = Autodesk.Revit.Element;
using GeometryElement = Autodesk.Revit.Geometry.Element;
using Instance = Autodesk.Revit.Geometry.Instance;

namespace Revit.SDK.Samples.DWGFamilyCreation.CS
{
    /// <summary>
    /// To add an external command to Autodesk Revit 
    /// the developer should implement an object that 
    /// supports the IExternalCommand interface.
    /// </summary>
    public class Command : IExternalCommand
    {
        /// <summary>
        /// Revit application
        /// </summary>
        Autodesk.Revit.Application m_app;
        /// <summary>
        /// Revit document
        /// </summary>
        Document m_doc;

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
        public IExternalCommand.Result Execute(ExternalCommandData commandData,
        ref string message, ElementSet elements)
        {
            try
            {
                m_app = commandData.Application;
                m_doc = m_app.ActiveDocument;
                if (null == m_doc)
                {
                    message = "There is no active document.";
                    return IExternalCommand.Result.Failed;
                }

                if (!m_doc.IsFamilyDocument)
                {
                    message = "Current document is not a family document.";
                    return IExternalCommand.Result.Failed;
                }

                // Get the view where the dwg file will be imported
                View view = GetView();
                if (null == view)
                {
                    message = "Opened wrong template file, please use the provided family template file.";
                    return IExternalCommand.Result.Failed;
                }

                // The dwg file which will be imported
                string AssemblyDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                string DWGFile = "Desk.dwg";
                string DWGFullPath = Path.Combine(AssemblyDirectory, DWGFile);

                // Import the dwg file into current family document
                DWGImportOptions options = new DWGImportOptions();
                options.Placement = Autodesk.Revit.Enums.ImportPlacement.Origin;
                options.OrientToView = true;
                options.View = view;
                Element element = null;
                m_doc.Import(DWGFullPath, options, out element);

                // Add type parameters to the family
                AddParameters(DWGFile);
            }
            catch (Exception ex)
            {
                message = ex.ToString();
                return IExternalCommand.Result.Failed;
            }

            return IExternalCommand.Result.Succeeded;
        }

        /// <summary>
        /// Add type parameters to the family
        /// </summary>
        /// <param name="DWGFileName">Name of imported dwg file</param>
        private void AddParameters(string DWGFileName)
        {
            // Get the family manager
            FamilyManager familyMgr = m_doc.FamilyManager;

            // Add parameter 1: DWGFileName
            familyMgr.NewType("DWGFamilyCreation");
            FamilyParameter paraFileName = familyMgr.AddParameter("DWGFileName", Autodesk.Revit.Parameters.BuiltInParameterGroup.INVALID,
                Autodesk.Revit.Parameters.ParameterType.Text, false);
            familyMgr.Set(paraFileName, DWGFileName);

            // Add parameter 2: ImportTime
            String time = DateTime.Now.ToString("yyyy-MM-dd");
            FamilyParameter paraImportTime = familyMgr.AddParameter("ImportTime", Autodesk.Revit.Parameters.BuiltInParameterGroup.INVALID,
                Autodesk.Revit.Parameters.ParameterType.Text, false);
            familyMgr.Set(paraImportTime, time);
        }

        /// <summary>
        /// Get the view where the dwg file will be imported
        /// </summary>
        /// <returns>The view where the dwg file will be imported</returns>
        private View GetView()
        {
            View view = null;

            TypeFilter viewFilter = m_app.Create.Filter.NewTypeFilter(typeof(View), true);
            IList<Element> views = new List<Element>();
            m_doc.get_Elements(viewFilter, views);

            foreach (View v in views)
            {
                if (v.ViewType == Autodesk.Revit.Enums.ViewType.FloorPlan && v.Name == "Ref. Level")
                {
                    view = v;
                    break;
                }
            }

            return view;
        }
    }
}
