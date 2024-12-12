//
// (C) Copyright 2003-2019 by Autodesk, Inc.
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
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.ImportExport.CS
{
    /// <summary>
    /// Data class which stores the main information for exporting dwg format
    /// </summary>
    public class ExportDWGData : ExportDataWithViews
    {
        #region Class Member Variables
        /// <summary>
        /// Data class ExportOptionsData
        /// </summary>
        private ExportBaseOptionsData m_exportOptionsData;

        /// <summary>
        /// String list of AutoCAD versions
        /// </summary>
        private List<String> m_fileVersion;

        /// <summary>
        /// List of Autodesk.Revit.DB.ACADVersion defined in Revit
        /// </summary>
        private List<Autodesk.Revit.DB.ACADVersion> m_enumFileVersion;

        /// <summary>
        /// File version option to export
        /// </summary>
        private Autodesk.Revit.DB.ACADVersion m_exportFileVersion;
        #endregion

        #region Class Properties
        /// <summary>
        /// Data class ExportOptionsData
        /// </summary>
        public ExportBaseOptionsData ExportOptionsData
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
        /// Collection of Autodesk.Revit.DB.ACADVersion defined in Revit
        /// </summary>
        public ReadOnlyCollection<Autodesk.Revit.DB.ACADVersion> EnumFileVersion
        {            
            get 
            {
                return new ReadOnlyCollection<Autodesk.Revit.DB.ACADVersion>(m_enumFileVersion); 
            }
        }

        /// <summary>
        /// File version option to export
        /// </summary>
        public Autodesk.Revit.DB.ACADVersion ExportFileVersion
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
            m_exportOptionsData = new ExportBaseOptionsData();

            Initialize();
        }

        /// <summary>
        /// Initialize the variables
        /// </summary>
        private void Initialize()
        {
            //AutoCAD versions
            m_fileVersion = new List<String>();
            m_enumFileVersion = new List<Autodesk.Revit.DB.ACADVersion>();
            m_fileVersion.Add("AutoCAD 2013 DWG Files (*.dwg)");
            m_enumFileVersion.Add(Autodesk.Revit.DB.ACADVersion.R2013);
            m_fileVersion.Add("AutoCAD 2010 DWG Files (*.dwg)");
            m_enumFileVersion.Add(Autodesk.Revit.DB.ACADVersion.R2010);
            m_fileVersion.Add("AutoCAD 2007 DWG Files (*.dwg)");
            m_enumFileVersion.Add(Autodesk.Revit.DB.ACADVersion.R2007);


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
            //parameter :  views
            IList<ElementId> views = new List<ElementId>();
            if (m_currentViewOnly)
            {
                views.Add(m_activeDoc.ActiveView.Id);
            }
            else
            {
                foreach (View view in m_selectViewsData.SelectedViews)
                {
                    views.Add(view.Id);
                }
            }

            // Default values
            m_exportFileVersion = ACADVersion.R2010;
            //parameter : DWGExportOptions dwgExportOptions
            DWGExportOptions dwgExportOptions = new DWGExportOptions();
            dwgExportOptions.ExportingAreas = m_exportOptionsData.ExportAreas;
            dwgExportOptions.ExportOfSolids = m_exportOptionsData.ExportSolid;
            dwgExportOptions.FileVersion = m_exportFileVersion;
            dwgExportOptions.LayerMapping = m_exportOptionsData.ExportLayerMapping;
            dwgExportOptions.LineScaling = m_exportOptionsData.ExportLineScaling;
            dwgExportOptions.MergedViews = m_exportOptionsData.ExportMergeFiles;
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
