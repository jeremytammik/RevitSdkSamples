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
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Autodesk.Revit;
using Autodesk.Revit.Elements;

namespace Revit.SDK.Samples.ImportAndExportForDWG.CS
{
    /// <summary>
    /// Data class which stores the main information for export
    /// </summary>
    public class ExportData
    {
        #region Class Memeber Variables
        // Revit command data
        private ExternalCommandData m_commandData;

        // Data class ExportOptionsData
        private ExportOptionsData m_exportOptionsData;

        // Data class SelectViewsData
        private SelectViewsData m_selectViewsData;

        // String list of AutoCAD versions
        private List<String> m_fileVersion;

        // List of Autodesk.Revit.Enums.ACADVersion defined in Revit
        private List<Autodesk.Revit.Enums.ACADVersion> m_enumFileVersion;

        // File version option to export
        private Autodesk.Revit.Enums.ACADVersion m_exportFileVersion;

        // File Name or Prefix to be used
        private String m_exportFileName;

        // Directory to store the exported file
        private String m_exportFolder;

        // Whether to create separate files for each view/sheet
        private Boolean m_exportMergeFiles;

        // Views to export
        private ViewSet m_exportViews;

        // Whether to export current view only
        private bool m_currentViewOnly;

        // Whether current view is a 3D view
        bool m_is3DView;
        #endregion

        #region Class Properties
        /// <summary>
        /// Revit command data
        /// </summary>
        public ExternalCommandData CommandData
        {
            get 
            { 
                return m_commandData; 
            }
        }

        /// <summary>
        /// Data class ExportOptionsData
        /// </summary>
        public ExportOptionsData ExportOptionsData
        {
            get 
            { 
                return m_exportOptionsData; 
            }
            set 
            { 
                m_exportOptionsData = value; 
            }
        }
  
        /// <summary>
        /// Data class SelectViewsData
        /// </summary>
        public SelectViewsData SelectViewsData
        {
            get 
            { 
                return m_selectViewsData; 
            }
            set 
            { 
                m_selectViewsData = value; 
            }
        }

        /// <summary>
        /// String collection of AutoCAD versions
        /// </summary>
        public ReadOnlyCollection<String> FileVersion
        {
            get 
            { 
                return new ReadOnlyCollection<String>(m_fileVersion); 
            }
        }

        /// <summary>
        /// Collection of Autodesk.Revit.Enums.ACADVersion defined in Revit
        /// </summary>
        public ReadOnlyCollection<Autodesk.Revit.Enums.ACADVersion> EnumFileVersion
        {            
            get 
            {
                return new ReadOnlyCollection<Autodesk.Revit.Enums.ACADVersion>(m_enumFileVersion); 
            }
        }

        /// <summary>
        /// File version option to export
        /// </summary>
        public Autodesk.Revit.Enums.ACADVersion ExportFileVersion
        {
            get 
            { 
                return m_exportFileVersion; 
            }
            set 
            { 
                m_exportFileVersion = value; 
            }
        }        

        /// <summary>
        /// File Name or Prefix to be used
        /// </summary>
        public String ExportFileName
        {
            get 
            { 
                return m_exportFileName; 
            }
            set 
            { 
                m_exportFileName = value; 
            }
        }  

        /// <summary>
        /// Directory to store the exported file
        /// </summary>
        public String ExportFolder
        {
            get 
            { 
                return m_exportFolder; 
            }
            set 
            { 
                m_exportFolder = value; 
            }
        }

        /// <summary>
        /// Whether to create separate files for each view/sheet
        /// </summary>
        public Boolean ExportMergeFiles
        {
            get 
            { 
                return m_exportMergeFiles; 
            }
            set 
            { 
                m_exportMergeFiles = value; 
            }
        }    

        /// <summary>
        /// Views to export
        /// </summary>
        public ViewSet ExportViews
        {
            get 
            { 
                return m_exportViews; 
            }
            set 
            { 
                m_exportViews = value; 
            }
        } 

        /// <summary>
        /// Whether to export current view only
        /// </summary>
        public bool CurrentViewOnly
        {
            get 
            { 
                return m_currentViewOnly; 
            }
            set 
            { 
                m_currentViewOnly = value; 
            }
        }

        /// <summary>
        /// Whether current view is a 3D view
        /// </summary>
        public bool Is3DView
        {
            get
            {
                return m_is3DView;
            }
            set
            {
                m_is3DView = value;
            }
        }  
        #endregion


        #region Class Member Methods
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="commandData"></param>
        public ExportData(ExternalCommandData commandData)
        {
            m_commandData = commandData;
            m_exportOptionsData = new ExportOptionsData(commandData);
            m_selectViewsData = new SelectViewsData(commandData);

            Initialize();
        }

        /// <summary>
        /// Initialize the variables
        /// </summary>
        private void Initialize()
        {
            //AutoCAD versions
            m_fileVersion = new List<String>();
            m_enumFileVersion = new List<Autodesk.Revit.Enums.ACADVersion>();
            m_fileVersion.Add("AutoCAD 2007 DWG Files (*.dwg)");
            m_enumFileVersion.Add(Autodesk.Revit.Enums.ACADVersion.R2007);
            m_fileVersion.Add("AutoCAD 2004 DWG Files (*.dwg)");
            m_enumFileVersion.Add(Autodesk.Revit.Enums.ACADVersion.R2004);
            m_fileVersion.Add("AutoCAD 2000 DWG Files (*.dwg)");
            m_enumFileVersion.Add(Autodesk.Revit.Enums.ACADVersion.R2000);

            //The directory into which the file will be exported
            String dllFilePath = Assembly.GetExecutingAssembly().Location;
            m_exportFolder = dllFilePath.Substring(0, dllFilePath.LastIndexOf("\\"));

            //The file name to be used by export
            String docTitle = m_commandData.Application.ActiveDocument.Title;
            String viewName = m_commandData.Application.ActiveDocument.ActiveView.Name;
            String viewType = m_commandData.Application.ActiveDocument.ActiveView.ViewType.ToString();
            m_exportFileName = docTitle + "-" + viewType + "-" + viewName + ".dwg";

            //Views to export
            m_exportViews = new ViewSet();

            //CurrentViewOnly
            m_currentViewOnly = true;

            //Whether current active view is 3D view
            if (m_commandData.Application.ActiveDocument.ActiveView.ViewType == Autodesk.Revit.Enums.ViewType.ThreeD)
            {
                m_is3DView = true;
            }
            else
            {
                m_is3DView = false;
            }
        }

        /// <summary>
        /// Collect the parameters and export
        /// </summary>
        /// <returns></returns>
        public bool Export()
        {
            //parameter : String folder
            String folder = m_exportFolder;
            //parameter : String name
            String name = m_exportFileName;
            if (folder == null || name == null)
            {
                throw new NullReferenceException();
            }
            //parameter : ViewSet views
            ViewSet views = new ViewSet();
            if (m_currentViewOnly)
            {
                views.Insert(m_commandData.Application.ActiveDocument.ActiveView);
            }
            else
            {
                views = m_selectViewsData.SelectedViews;
            }

            //parameter : DWGExportOptions dwgExportOptions
            DWGExportOptions dwgExportOptions = new DWGExportOptions();
            dwgExportOptions.ExportingAreas = m_exportOptionsData.ExportAreas;
            dwgExportOptions.ExportOfSolids = m_exportOptionsData.ExportSolid;
            dwgExportOptions.FileVersion = m_exportFileVersion;
            dwgExportOptions.LayerMapping = m_exportOptionsData.ExportLayerMapping;
            dwgExportOptions.LineScaling = m_exportOptionsData.ExportLineScaling;
            dwgExportOptions.MergedViews = m_exportMergeFiles;
            dwgExportOptions.PropOverrides = m_exportOptionsData.ExportLayersAndProperties;
            dwgExportOptions.SharedCoords = m_exportOptionsData.ExportCoorSystem;
            dwgExportOptions.TargetUnit = m_exportOptionsData.ExportUnit;

            //Export
            Document doc = m_commandData.Application.ActiveDocument;
            bool exported = doc.Export( folder, name, views, dwgExportOptions );

            return exported;
        }
        #endregion
    }
}
