//
// (C) Copyright 2003-2013 by Autodesk, Inc.
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
using System.Reflection;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.ImportExport.CS
{
    /// <summary>
    /// Data class which stores the information for exporting 2D DWF format
    /// </summary>
    public class ExportDWFData : ExportDataWithViews
    {
        #region Class Member Variables
        /// <summary>
        /// Whether export object data
        /// </summary>
        private bool m_exportObjectData;
        /// <summary>
        /// Whether export areas
        /// </summary>
        private bool m_exportAreas;
        /// <summary>
        /// Whether to create separate files for each view/sheet
        /// </summary>
        private bool m_exportMergeFiles;
        /// <summary>
        /// List of image quality for DWF export
        /// </summary>
        List<Autodesk.Revit.DB.DWFImageQuality> m_ImageQualities;
        /// <summary>
        /// Selected image quality for DWF export
        /// </summary>
        Autodesk.Revit.DB.DWFImageQuality m_dwfImageQuality;
        /// <summary>
        /// Selected image format for DWF export
        /// </summary>
        Autodesk.Revit.DB.DWFImageFormat m_dwfImageFormat;
        #endregion

        #region Class Properties
        /// <summary>
        /// Whether export object data
        /// </summary>
        public bool ExportObjectData
        {
            get 
            { 
                return m_exportObjectData; 
            }
            set 
            { 
                m_exportObjectData = value; 
            }
        }

        /// <summary>
        /// Whether to create separate files for each view/sheet
        /// </summary>
        public bool ExportMergeFiles
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
        /// Whether export areas
        /// </summary>
        public bool ExportAreas
        {
            get
            {
                return m_exportAreas;
            }
            set
            {
                m_exportAreas = value;
            }
        }

        /// <summary>
        /// Selected image format for DWF export
        /// </summary>
        public Autodesk.Revit.DB.DWFImageFormat DwfImageFormat
        {
            get 
            { 
                return m_dwfImageFormat; 
            }
            set 
            { 
                m_dwfImageFormat = value; 
            }
        }

        /// <summary>
        /// Selected image quality for DWF export
        /// </summary>
        public Autodesk.Revit.DB.DWFImageQuality DwfImageQuality
        {
            get 
            { 
                return m_dwfImageQuality; 
            }
            set 
            { 
                m_dwfImageQuality = value; 
            }
        }

        /// <summary>
        /// List of image quality for DWF export
        /// </summary>
        public List<Autodesk.Revit.DB.DWFImageQuality> ImageQualities
        {
            get 
            { 
                return m_ImageQualities; 
            }
            set 
            { 
                m_ImageQualities = value; 
            }
        }
        #endregion

        #region Class Member Methods
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="commandData">Revit command data</param>
        /// <param name="exportFormat">Format to export</param>
        public ExportDWFData(ExternalCommandData commandData, ExportFormat exportFormat)
            : base(commandData, exportFormat)
        {
            Initialize();
        }

        /// <summary>
        /// Initialize the variables
        /// </summary>
        private void Initialize()
        {
            m_exportObjectData = true;
            m_exportAreas = false;
            m_exportMergeFiles = true;
            m_ImageQualities = new List<Autodesk.Revit.DB.DWFImageQuality>();
            m_ImageQualities.Add(Autodesk.Revit.DB.DWFImageQuality.Low);
            m_ImageQualities.Add(Autodesk.Revit.DB.DWFImageQuality.Medium);
            m_ImageQualities.Add(Autodesk.Revit.DB.DWFImageQuality.High);

            // Export DWF
            if (m_exportFormat == ExportFormat.DWF)
            {
                m_filter = "DWF Files |*.dwf";
                m_title = "Export DWF";
            }
            // Export DWFx
            else
            {
                m_filter = "DWFx Files |*.dwfx";
                m_title = "Export DWFx";
            }
        }

        /// <summary>
        /// Collect the parameters and export
        /// </summary>
        /// <returns></returns>
        public override bool Export()
        {
            Transaction transaction = new Transaction(m_activeDoc, "Export_To_DWF");
            transaction.Start();
            bool exported = false;
            base.Export();

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

            // Export DWFx
            if (m_exportFormat == ExportFormat.DWFx)
            {
                DWFXExportOptions options = new DWFXExportOptions();
                options.ExportObjectData = m_exportObjectData;
                options.ExportingAreas = m_exportAreas;
                options.MergedViews = m_exportMergeFiles;
                options.ImageFormat = m_dwfImageFormat;
                options.ImageQuality = m_dwfImageQuality;
                exported = m_activeDoc.Export(m_exportFolder, m_exportFileName, views, options);
            }
            // Export DWF
            else
            {
                DWFExportOptions options = new DWFExportOptions();
                options.ExportObjectData = m_exportObjectData;
                options.ExportingAreas = m_exportAreas;
                options.MergedViews = m_exportMergeFiles;
                options.ImageFormat = m_dwfImageFormat;
                options.ImageQuality = m_dwfImageQuality;
                exported = m_activeDoc.Export(m_exportFolder, m_exportFileName, views, options);
            }
            transaction.Commit();

            return exported;
        }
        #endregion
    }
}
