//
// (C) Copyright 2003-2015 by Autodesk, Inc.
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
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Element = Autodesk.Revit.DB.Element;
using GeometryElement = Autodesk.Revit.DB.GeometryElement;
using Instance = Autodesk.Revit.DB.Instance;

namespace Revit.SDK.Samples.DWGFamilyCreation.CS
{
    /// <summary>
    /// To add an external command to Autodesk Revit 
    /// the developer should implement an object that 
    /// supports the IExternalCommand interface.
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class Command : IExternalCommand
    {
        /// <summary>
        /// Revit application
        /// </summary>
        Autodesk.Revit.UI.UIApplication m_app;
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
        public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData,
        ref string message, Autodesk.Revit.DB.ElementSet elements)
        {
            try
            {
                m_app = commandData.Application;
                m_doc = commandData.Application.ActiveUIDocument.Document;
                if (null == m_doc)
                {
                    message = "There is no active document.";
                    return Autodesk.Revit.UI.Result.Failed;
                }

                if (!m_doc.IsFamilyDocument)
                {
                    message = "Current document is not a family document.";
                    return Autodesk.Revit.UI.Result.Failed;
                }

                // Get the view where the dwg file will be imported
                View view = GetView();
                if (null == view)
                {
                    message = "Opened wrong template file, please use the provided family template file.";
                    return Autodesk.Revit.UI.Result.Failed;
                }

                // The dwg file which will be imported
                string AssemblyDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                string DWGFile = "Desk.dwg";
                string DWGFullPath = Path.Combine(AssemblyDirectory, DWGFile);

                Transaction transaction = new Transaction(m_doc, "DWGFamilyCreation");
                transaction.Start();
                // Import the dwg file into current family document
                DWGImportOptions options = new DWGImportOptions();
                options.Placement = Autodesk.Revit.DB.ImportPlacement.Origin;
                options.OrientToView = true;
                ElementId elementId = null;
                m_doc.Import(DWGFullPath, options, view, out elementId);

                // Add type parameters to the family
                AddParameters(DWGFile);
                transaction.Commit();
            }
            catch (Exception ex)
            {
                message = ex.ToString();
                return Autodesk.Revit.UI.Result.Failed;
            }

            return Autodesk.Revit.UI.Result.Succeeded;
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
            FamilyParameter paraFileName = familyMgr.AddParameter("DWGFileName", Autodesk.Revit.DB.BuiltInParameterGroup.INVALID,
                Autodesk.Revit.DB.ParameterType.Text, false);
            familyMgr.Set(paraFileName, DWGFileName);

            // Add parameter 2: ImportTime
            String time = DateTime.Now.ToString("yyyy-MM-dd");
            FamilyParameter paraImportTime = familyMgr.AddParameter("ImportTime", Autodesk.Revit.DB.BuiltInParameterGroup.INVALID,
                Autodesk.Revit.DB.ParameterType.Text, false);
            familyMgr.Set(paraImportTime, time);
        }

        /// <summary>
        /// Get the view where the dwg file will be imported
        /// </summary>
        /// <returns>The view where the dwg file will be imported</returns>
        private View GetView()
        {
            View view = null;
            List<Autodesk.Revit.DB.Element> views = new List<Autodesk.Revit.DB.Element>();
            FilteredElementCollector collector = new FilteredElementCollector(m_app.ActiveUIDocument.Document);
            views.AddRange(collector.OfClass(typeof(View)).ToElements());
            foreach (View v in views)
            {
                if (!v.IsTemplate && v.ViewType == Autodesk.Revit.DB.ViewType.FloorPlan && v.Name == "Ref. Level")
                {
                    view = v;
                    break;
                }
            }

            return view;
        }
    }
}
