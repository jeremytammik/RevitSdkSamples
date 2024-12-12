
//
// (C) Copyright 2003-2011 by Autodesk, Inc.
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
    /// Data class which stores the information for importing image format
    /// </summary>
    class ImportImageData : ImportData
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="commandData">Revit command data</param>
        /// <param name="importFormat">Format to import</param>
        public ImportImageData(ExternalCommandData commandData, ImportFormat importFormat)
            : base(commandData, importFormat)
        {
            m_filter = "All Image Files (*.bmp, *.gif, *.jpg, *.jpeg, *.png, *.tif)|*.bmp;*.gif;*.jpg;*.jpeg;*.png;*.tif";
            m_title = "Import Image";
        }

        /// <summary>
        /// Collect the parameters and export
        /// </summary>
        /// <returns></returns>
        public override bool Import()
        {
            bool imported = false;

            //parameter: ImageImportOptions
            ImageImportOptions options = new ImageImportOptions();
            options.Placement = Autodesk.Revit.DB.BoxPlacement.Center;
            //parameter: Element
            Element element = null;

            //Import
            Transaction t = new Transaction(m_activeDoc);
            t.SetName("Import");
            t.Start();
            imported = m_activeDoc.Import(m_importFileFullName, options, CommandData.Application.ActiveUIDocument.Document.ActiveView, out element);
            t.Commit();

            return imported;
        }
    }
}
