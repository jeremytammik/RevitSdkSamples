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
using System.IO;

using Autodesk.Revit;
using Autodesk.Revit.Elements;

namespace Revit.SDK.Samples.ImportExport.CS
{
    /// <summary>
    /// Base data class which stores the basic information for export
    /// </summary>
    public class ExportData
    {
        #region Class Memeber Variables
        /// <summary>
        /// Revit command data
        /// </summary>
        protected ExternalCommandData m_commandData;

        /// <summary>
        /// Active document
        /// </summary>
        protected Document m_activeDoc;

        /// <summary>
        /// File Name or Prefix to be used
        /// </summary>
        protected String m_exportFileName;

        /// <summary>
        /// Directory to store the exported file
        /// </summary>
        protected String m_exportFolder;

        /// <summary>
        /// Whether current view is a 3D view
        /// </summary>
        protected bool m_is3DView;

        /// <summary>
        /// The format to be exported
        /// </summary>
        protected ExportFormat m_exportFormat;

        /// <summary>
        /// The filter which will be used in file saving dialog
        /// </summary>
        protected String m_filter;

        /// <summary>
        /// The title of exporting dialog
        /// </summary>
        protected String m_title;
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

        /// <summary>
        /// The format to be exported
        /// </summary>
        public ExportFormat ExportFormat
        {
            get 
            { 
                return m_exportFormat; 
            }
            set 
            { 
                m_exportFormat = value; 
            }
        }

        /// <summary>
        /// The filter which will be used in file saving dialog
        /// </summary>
        public String Filter
        {
            get 
            { 
                return m_filter; 
            }
        }

        /// <summary>
        /// The title of exporting dialog
        /// </summary>
        public String Title
        {
            get 
            { 
                return m_title; 
            }
        }
        #endregion


        #region Class Member Methods
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="commandData">Revit command data</param>
        /// <param name="exportFormat">Format to export</param>
        public ExportData(ExternalCommandData commandData, ExportFormat exportFormat)
        {
            m_commandData = commandData;
            m_activeDoc = commandData.Application.ActiveDocument;
            m_exportFormat = exportFormat;
            m_filter = String.Empty;
            Initialize();
        }

        /// <summary>
        /// Initialize the variables
        /// </summary>
        private void Initialize()
        {
            //The directory into which the file will be exported
            String dllFilePath = Assembly.GetExecutingAssembly().Location;
            m_exportFolder = Path.GetDirectoryName(dllFilePath);

            //The file name to be used by export
            String docTitle = m_activeDoc.Title;
            String viewName = m_activeDoc.ActiveView.Name;
            String viewType = m_activeDoc.ActiveView.ViewType.ToString();
            m_exportFileName = docTitle + "-" + viewType + "-" + viewName + "." + getExtension().ToString();

            //Whether current active view is 3D view
            if (m_activeDoc.ActiveView.ViewType == Autodesk.Revit.Enums.ViewType.ThreeD)
            {
                m_is3DView = true;
            }
            else
            {
                m_is3DView = false;
            }
        }

        /// <summary>
        /// Get the extension of the file to export
        /// </summary>
        /// <returns></returns>
        private String getExtension()
        {
            String extension = null;
            switch (m_exportFormat)
            {
                case ExportFormat.DWG:
                    extension = "dwg";
                    break;
                case ExportFormat.DWF2D:
                case ExportFormat.DWF3D:
                    extension = "dwf";
                    break;
                case ExportFormat.DWFx2D:
                case ExportFormat.DWFx3D:
                    extension = "dwfx";
                    break;
                case ExportFormat.GBXML:
                    extension = "xml";
                    break;
                case ExportFormat.FBX:
                    extension = "fbx";
                    break;
                default:
                    break;
            }

            return extension;
        }

        /// <summary>
        /// Collect the parameters and export
        /// </summary>
        /// <returns></returns>
        public virtual bool Export()
        {
            if (m_exportFolder == null || m_exportFileName == null)
            {
                throw new NullReferenceException();
            }

            return true;
        }
        #endregion
    }
}