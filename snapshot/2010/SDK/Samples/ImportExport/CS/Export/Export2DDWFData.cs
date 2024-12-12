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
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;

using Autodesk.Revit;
using Autodesk.Revit.Elements;

namespace Revit.SDK.Samples.ImportExport.CS
{
    /// <summary>
    /// Data class which stores the information for exporting 2D DWF format
    /// </summary>
    public class Export2DDWFData : ExportDataWithViews
    {
        #region Class Memeber Variables
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
        List<Autodesk.Revit.Enums.DWFImageQuality> m_ImageQualities;
        /// <summary>
        /// Selected image quality for DWF export
        /// </summary>
        Autodesk.Revit.Enums.DWFImageQuality m_dwfImageQuality;
        /// <summary>
        /// Selected image format for DWF export
        /// </summary>
        Autodesk.Revit.Enums.DWFImageFormat m_dwfImageFormat;
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
        public Autodesk.Revit.Enums.DWFImageFormat DwfImageFormat
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
        public Autodesk.Revit.Enums.DWFImageQuality DwfImageQuality
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
        public List<Autodesk.Revit.Enums.DWFImageQuality> ImageQualities
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
        public Export2DDWFData(ExternalCommandData commandData, ExportFormat exportFormat)
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
            m_ImageQualities = new List<Autodesk.Revit.Enums.DWFImageQuality>();
            m_ImageQualities.Add(Autodesk.Revit.Enums.DWFImageQuality.Low);
            m_ImageQualities.Add(Autodesk.Revit.Enums.DWFImageQuality.Medium);
            m_ImageQualities.Add(Autodesk.Revit.Enums.DWFImageQuality.High);

            // Export 2D DWF
            if (m_exportFormat == ExportFormat.DWF2D)
            {
                m_filter = "DWF Files |*.dwf";
                m_title = "Export 2D DWF";
            }
            // Export 2D DWFx
            else
            {
                m_filter = "DWFx Files |*.dwfx";
                m_title = "Export 2D DWFx";
            }
        }

        /// <summary>
        /// Collect the parameters and export
        /// </summary>
        /// <returns></returns>
        public override bool Export()
        {
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
            if (m_exportFormat == ExportFormat.DWFx2D)
            {
                DWFX2DExportOptions options = new DWFX2DExportOptions();
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
                DWF2DExportOptions options = new DWF2DExportOptions();
                options.ExportObjectData = m_exportObjectData;
                options.ExportingAreas = m_exportAreas;
                options.MergedViews = m_exportMergeFiles;
                options.ImageFormat = m_dwfImageFormat;
                options.ImageQuality = m_dwfImageQuality;
                exported = m_activeDoc.Export(m_exportFolder, m_exportFileName, views, options);
            }

            return exported;
        }

        /// <summary>
        /// Modified by Joe Ye for for case 
        /// 1255511 [Problem with API Sample Import-Export putting - after file name].
        /// </summary>
        public bool Export_1255511()
        {
          bool exported = false;
          base.Export();

          //parameter : ViewSet views
          ViewSet views = new ViewSet();
          if( m_currentViewOnly )
          {
            views.Insert( m_activeDoc.ActiveView );
          }
          else
          {
            views = m_selectViewsData.SelectedViews;
          }

          // Export DWFx
          if( m_exportFormat == ExportFormat.DWFx2D )
          {
            DWFX2DExportOptions options = new DWFX2DExportOptions();
            options.ExportObjectData = m_exportObjectData;
            options.ExportingAreas = m_exportAreas;
            options.MergedViews = m_exportMergeFiles;
            options.ImageFormat = m_dwfImageFormat;
            options.ImageQuality = m_dwfImageQuality;

            if( m_exportMergeFiles == false )
            {
              string strTempFolder = "C:/tmp";

              exported = m_activeDoc.Export( strTempFolder, 
                m_exportFileName, views, options );

              File.Copy( 
                strTempFolder + "/" + m_exportFileName + "-.DWFx", 
                m_exportFolder + "/" + m_exportFileName + ".DWFx" );

              // optionally delete the intermediate file
              //File.Delete(strTempFolder + "/" + m_exportFileName + "-.DWFx");
            }
            else
            {
              exported = m_activeDoc.Export( m_exportFolder, 
                m_exportFileName, views, options );
            }
          }
          // Export DWF
          else
          {
            DWF2DExportOptions options = new DWF2DExportOptions();
            options.ExportObjectData = m_exportObjectData;
            options.ExportingAreas = m_exportAreas;
            options.MergedViews = m_exportMergeFiles;
            options.ImageFormat = m_dwfImageFormat;
            options.ImageQuality = m_dwfImageQuality;
            if( m_exportMergeFiles == false )
            {
              string strTempFolder = "C:/tmp";

              exported = m_activeDoc.Export( strTempFolder, 
                m_exportFileName, views, options );

              File.Copy( 
                strTempFolder + "/" + m_exportFileName + "-.DWF", 
                m_exportFolder + "/" + m_exportFileName + ".DWF" );

              // optionally delete the intermediate file
              //File.Delete( strTempFolder + "/" + m_exportFileName + "-.DWF");
            }
            else
            {
              exported = m_activeDoc.Export( m_exportFolder, 
                m_exportFileName, views, options );
            }
          }
          return exported;
        }

        #endregion
    }
}
