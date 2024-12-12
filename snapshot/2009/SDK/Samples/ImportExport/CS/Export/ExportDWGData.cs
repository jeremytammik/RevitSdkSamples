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
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Autodesk.Revit;
using Autodesk.Revit.Elements;

namespace Revit.SDK.Samples.ImportExport.CS
{
    /// <summary>
    /// Data class which stores the main information for exporting dwg format
    /// </summary>
    public class ExportDWGData : ExportDataWithViews
    {
        #region Class Memeber Variables
        // Data class ExportOptionsData
        private ExportDWGOptionsData m_exportOptionsData;

        // String list of AutoCAD versions
        private List<String> m_fileVersion;

        // List of Autodesk.Revit.Enums.ACADVersion defined in Revit
        private List<Autodesk.Revit.Enums.ACADVersion> m_enumFileVersion;

        // File version option to export
        private Autodesk.Revit.Enums.ACADVersion m_exportFileVersion;
        #endregion

        #region Class Properties
        /// <summary>
        /// Data class ExportOptionsData
        /// </summary>
        public ExportDWGOptionsData ExportOptionsData
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
        #endregion

        #region Class Member Methods
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="commandData">Revit command data</param>
        /// <param name="exportFormat">Format to export</param>
        public ExportDWGData(ExternalCommandData commandData, ExportFormat exportFormat)
            : base(commandData, exportFormat)
        {
            m_exportOptionsData = new ExportDWGOptionsData();

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

            StringBuilder tmp = new StringBuilder();
            foreach (String version in m_fileVersion)
            {
                tmp.Append(version + "|*.dwg|");
            }
            m_filter = tmp.ToString().TrimEnd('|');
            m_title = "Export DWG";
        }

        /// <summary>
        /// Collect the parameters and export
        /// </summary>
        /// <returns></returns>
        public override bool Export()
        {
            base.Export();

            bool exported = false;
            //parameter : ViewSet views
            ViewSet views = new ViewSet();
            if (m_currentViewOnly)
            {
                views.Insert(m_activeDoc.ActiveView);
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
            exported = m_activeDoc.Export(m_exportFolder, m_exportFileName, views, dwgExportOptions);

            return exported;
        }
        #endregion
    }
}
