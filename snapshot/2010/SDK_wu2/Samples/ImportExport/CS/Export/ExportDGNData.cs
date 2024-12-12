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
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Autodesk.Revit;
using Autodesk.Revit.Elements;

namespace Revit.SDK.Samples.ImportExport.CS
{
    /// <summary>
    /// Data class which stores the main information for exporting dgn format
    /// </summary>
    public class ExportDGNData : ExportDataWithViews
    {
        #region Class Memeber Variables
        /// <summary>
        /// String list of Layer Settings used in UI
        /// </summary>
        List<String> m_layerMapping;
        /// <summary>
        /// String list of layer settings values defined in Revit 
        /// </summary>
        List<String> m_enumLayerMapping;
        /// <summary>
        /// Layer setting option to export
        /// </summary>
        private String m_exportLayerMapping;
        /// <summary>
        /// Template file
        /// </summary>
        private String m_templateFile;
        #endregion

        #region Class Properties
        /// <summary>
        /// Layer setting option to export
        /// </summary>
        public String ExportLayerMapping
        {
            get
            {
                return m_exportLayerMapping;
            }
            set
            {
                m_exportLayerMapping = value;
            }
        }

        /// <summary>
        /// String collection of Layer Settings used in UI
        /// </summary>
        public ReadOnlyCollection<String> LayerMapping
        {
            get
            {
                return new ReadOnlyCollection<String>(m_layerMapping);
            }
        }

        /// <summary>
        /// String collection of layer settings values defined in Revit  
        /// </summary>
        public ReadOnlyCollection<String> EnumLayerMapping
        {
            get
            {
                return new ReadOnlyCollection<String>(m_enumLayerMapping);
            }
        }

        /// <summary>
        /// Template file
        /// </summary>
        public String TemplateFile
        {
            get
            {
                return m_templateFile;
            }
            set
            {
                m_templateFile = value;
            }
        }
        #endregion

        #region Class Member Methods
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="commandData">Revit command data</param>
        /// <param name="exportFormat">Format to export</param>
        public ExportDGNData(ExternalCommandData commandData, ExportFormat exportFormat)
            : base(commandData, exportFormat)
        {
            Initialize();
        }

        /// <summary>
        /// Initialize the variables
        /// </summary>
        private void Initialize()
        {
            //Layer Settings:
            m_layerMapping = new List<String>();
            m_enumLayerMapping = new List<String>();
            m_layerMapping.Add("AIA - American Institute of Architects standard");
            m_enumLayerMapping.Add("AIA");
            m_layerMapping.Add("ISO13567 - ISO standard 13567");
            m_enumLayerMapping.Add("ISO13567");
            m_layerMapping.Add("CP83 - Singapore standard 83");
            m_enumLayerMapping.Add("CP83");
            m_layerMapping.Add("BS1192 - British standard 1192");
            m_enumLayerMapping.Add("BS1192");

            m_filter = "Microstation DGN Files |*.dgn";
            m_title = "Export DGN";
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
            DGNExportOptions dgnExportOptions = new DGNExportOptions();
            dgnExportOptions.LayerMapping = m_exportLayerMapping;
            dgnExportOptions.TemplateFile = m_templateFile;

            //Export
            exported = m_activeDoc.Export(m_exportFolder, m_exportFileName, views, dgnExportOptions);

            return exported;
        }
        #endregion
    }
}
