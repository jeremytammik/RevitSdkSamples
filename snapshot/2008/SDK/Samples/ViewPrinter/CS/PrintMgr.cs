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
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using Autodesk.Revit;
using Autodesk.Revit.Elements;

using RView = Autodesk.Revit.Elements.View;

namespace Revit.SDK.Samples.ViewPrinter.CS
{
    /// <summary>
    /// This is the most important class of the sample project. It will show you how to print view,
    /// provides access to all elements of specified type within the Document, and how to open file
    /// on disk.
    /// </summary>
    public class PrintMgr
    {
        private ExternalCommandData m_commandData;
        private Document m_activeDoc;
        private ViewSet m_printableViews;
        private ViewSet m_selectedViews;

        /// <summary>
        /// The Project Information of the current project.
        /// </summary>
        /// <remarks>This should be update once project changed.</remarks>
        /// <value>Return the Project Information of the current project.</value>
        public string ProjectInfo
        {
            get
            {
                ProjectInfo projInfo = m_activeDoc.ProjectInformation;
                return "IssueDate - " + projInfo.IssueDate + "\r\n"
                + "Status - " + projInfo.Status + "\r\n"
                + "ClientName - " + projInfo.ClientName + "\r\n"
                + "Address - " + projInfo.Address + "\r\n"
                + "Name - " + projInfo.Name + "\r\n"
                + "Number - " + projInfo.Number;
            }
        }

        /// <summary>
        /// All printable views in current project.
        /// </summary>
        public ViewSet PrintableViews
        {
            get
            {
                return m_printableViews;
            }
        }

        /// <summary>
        /// All selected views to be print.
        /// </summary>
        public ViewSet SelectedViews
        {
            get
            {
                return m_selectedViews;
            }
        }

        /// <summary>
        /// Hiden constructor, use forbbiden.
        /// </summary>
        private PrintMgr()
        {
        }        

        /// <summary>
        /// public constructor,
        /// </summary>
        /// <param name="commandData">A ExternalCommandData object which contains reference to 
        /// Application and View needed by external command.</param>
        public PrintMgr(ExternalCommandData commandData)
        {
            m_printableViews = new ViewSet();
            m_selectedViews = new ViewSet();
            m_commandData = commandData;

            UpdateProject(null);
        }

        /// <summary>
        /// Open file, and retrieve all printable views in current document.
        /// </summary>
        /// <param name="fileName">The file to be opened. null or empty means current project is ok.</param>
        /// <returns></returns>
        public Document UpdateProject(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                m_activeDoc = m_commandData.Application.ActiveDocument;
            }
            else
            {
                m_activeDoc = m_commandData.Application.OpenDocumentFile(fileName);
            }

            ElementIterator itor = m_activeDoc.get_Elements(typeof(RView));
            itor.Reset();
            m_printableViews.Clear();

            while (itor.MoveNext())
            {
                RView view = itor.Current as RView;
                if (null != view && view.CanBePrinted)
                {
                    m_printableViews.Insert(view);
                }
            }
            return m_activeDoc;
        }

        /// <summary>
        /// Print this view with default print setting and default view template.
        /// If the view can not be printed successfully then an exception will be thrown out.
        /// </summary>
        /// <param name="printableView">A view which property CanBePrinted is true.</param>
        static public void Print(RView printableView)
        {
            if (null == printableView)
            {
                throw new ArgumentNullException();
            }
            printableView.Print();
        }
    }
}
