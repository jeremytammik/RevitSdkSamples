//
// (C) Copyright 2003-2014 by Autodesk, Inc.
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

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.ImportExport.CS
{
    /// <summary>
    /// Base data class which stores the common information for exporting view related format
    /// </summary>
    public class ExportDataWithViews : ExportData
    {
        #region Class Member Variables
        /// <summary>
        /// Data class SelectViewsData
        /// </summary>
        protected SelectViewsData m_selectViewsData;

        /// <summary>
        /// Views to export
        /// </summary>
        private ViewSet m_exportViews;

        /// <summary>
        /// Whether to export current view only
        /// </summary>
        protected bool m_currentViewOnly;
        #endregion

        #region Class Properties
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
        #endregion

        #region Class Member Methods
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="commandData">Revit command data</param>
        /// <param name="exportFormat">Format to export</param>
        public ExportDataWithViews(ExternalCommandData commandData, ExportFormat exportFormat)
            : base(commandData, exportFormat)
        {
            m_selectViewsData = new SelectViewsData(commandData);

            Initialize();
        }

        /// <summary>
        /// Initialize the variables
        /// </summary>
        private void Initialize()
        {
            //Views to export
            m_exportViews = new ViewSet();
        }
        #endregion
    }
}
