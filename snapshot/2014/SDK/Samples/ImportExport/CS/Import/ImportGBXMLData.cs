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

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.ImportExport.CS
{
    /// <summary>
    /// Data class which stores the information for importing GBXML format
    /// </summary>
    class ImportGBXMLData : ImportData
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="commandData">Revit command data</param>
        /// <param name="importFormat">Format to import</param>
        public ImportGBXMLData(ExternalCommandData commandData, ImportFormat importFormat)
            : base(commandData, importFormat)
        {
            m_filter = "XML Documents (*.xml)|*.xml";
            m_title = "Import GBXML";
        }

        /// <summary>
        /// Collect the parameters and export
        /// </summary>
        /// <returns></returns>
        public override bool Import()
        {
            bool imported = false;

            //parameter: GBXMLImportOptions
            GBXMLImportOptions options = new GBXMLImportOptions();

            //Import
            Transaction t = new Transaction(m_activeDoc);
            t.SetName("Import GBXML");
            t.Start();
            imported = m_activeDoc.Import(m_importFileFullName, options);
            t.Commit();

            return imported;
        }
    }
}


