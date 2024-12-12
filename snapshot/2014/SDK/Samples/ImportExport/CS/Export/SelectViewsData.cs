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
using System.Collections;
using System.Collections.Generic;
using System.Text;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.ImportExport.CS
{
    /// <summary>
    /// Data class which stores views information for export
    /// </summary>
    public class SelectViewsData
    {
        #region Class Member Variables
        /// <summary>
        /// Revit command data
        /// </summary>
        private ExternalCommandData m_commandData;

        /// <summary>
        /// All printable views(except sheets)
        /// </summary>
        private ViewSet m_printableViews;

        /// <summary>
        /// All printable sheets
        /// </summary>
        private ViewSet m_printableSheets;

        /// <summary>
        /// All selected views in UI
        /// </summary>
        private ViewSet m_selectedViews;

        /// <summary>
        /// Whether contain 3D view in selected views
        /// </summary>
        private bool m_contain3DView;
        #endregion

        #region Class Properties
        /// <summary>
        /// All printable views(except sheets)
        /// </summary>
        public ViewSet PrintableViews
        {
            get 
            { 
                return m_printableViews; 
            }
            set 
            { 
                m_printableViews = value; 
            }
        }

        /// <summary>
        /// All printable sheets
        /// </summary>
        public ViewSet PrintableSheets
        {
            get 
            { 
                return m_printableSheets; 
            }
            set 
            { 
                m_printableSheets = value;
            }
        }      

        /// <summary>
        /// All selected views in UI
        /// </summary>
        public ViewSet SelectedViews
        {
            get 
            {
                return m_selectedViews;
            }
            set 
            { 
                m_selectedViews = value;
            }
        }

        /// <summary>
        /// Whether contain 3D view in selected views
        /// </summary>
        public bool Contain3DView
        {
            get 
            { 
                return m_contain3DView; 
            }
            set 
            { 
                m_contain3DView = value; 
            }
        }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="commandData"></param>
        public SelectViewsData(ExternalCommandData commandData)
        {
            m_commandData = commandData;
            m_printableViews = new ViewSet();
            m_printableSheets = new ViewSet();
            m_selectedViews = new ViewSet();

            GetAllPrintableViews();
        }

        /// <summary>
        /// Get all printable views and sheets
        /// </summary>
        private void GetAllPrintableViews()
        {
            FilteredElementCollector collector = new FilteredElementCollector(m_commandData.Application.ActiveUIDocument.Document);
            FilteredElementIterator itor = collector.OfClass(typeof(View)).GetElementIterator();
            itor.Reset();
            m_printableViews.Clear();
            m_printableSheets.Clear();

            while (itor.MoveNext())
            {
                View view = itor.Current as View;
                // skip view templates because they're invisible in project browser, invalid for print
                if (null == view || view.IsTemplate || !view.CanBePrinted)
                {
                    continue;
                }
                else if(view.ViewType == Autodesk.Revit.DB.ViewType.DrawingSheet)
                {
                    m_printableSheets.Insert(view);
                }
                else
                {
                    m_printableViews.Insert(view);
                }
            }
        }
    }
}
