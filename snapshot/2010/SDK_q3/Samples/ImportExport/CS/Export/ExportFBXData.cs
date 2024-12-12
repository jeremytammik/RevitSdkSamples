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

using Autodesk.Revit;
using Autodesk.Revit.Elements;

namespace Revit.SDK.Samples.ImportExport.CS
{
    /// <summary>
    /// Data class which stores the information for exporting fbx format
    /// </summary>
    class ExportFBXData : ExportData
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="commandData">Revit command data</param>
        /// <param name="exportFormat">Format to export</param>
        public ExportFBXData(ExternalCommandData commandData, ExportFormat exportFormat)
            : base(commandData, exportFormat)
        {
            m_filter = "FBX Files |*.fbx";
            m_title = "Export FBX";
        }

        /// <summary>
        /// Export FBX format
        /// </summary>
        /// <returns></returns>
        public override bool Export()
        {
            base.Export();

            bool exported = false;
            //parameter : ViewSet views
            ViewSet views = new ViewSet();
            views.Insert(m_activeDoc.ActiveView);

            FBXExportOptions options = new FBXExportOptions();
            exported = m_activeDoc.Export(m_exportFolder, m_exportFileName, views, options);

            return exported;
        }
    }
}
