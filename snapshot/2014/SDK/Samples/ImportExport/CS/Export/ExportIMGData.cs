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
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Forms;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.ImportExport.CS
{
    class ExportIMGData : ExportDataWithViews
    {
        #region Class Member Variables

        /// <summary>
        /// String list of image type
        /// </summary>
        private List<String> m_imageType;

        #endregion

        #region Class Member Methods
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="commandData">Revit command data</param>
        /// <param name="exportFormat">Format to export</param>
        public ExportIMGData(ExternalCommandData commandData, ExportFormat exportFormat)
            : base(commandData, exportFormat)
        {
            Initialize();
        }

        /// <summary>
        /// Initialize the variables
        /// </summary>
        private void Initialize()
        {
            //Image type
            m_imageType = new List<String>();
            m_imageType.Add("(*.bmp)");
            m_imageType.Add("(*.jpeg)");
            m_imageType.Add("(*.png)");
            m_imageType.Add("(*.tga)");
            m_imageType.Add("(*.tif)");

            StringBuilder tmp = new StringBuilder();
            tmp.Append(m_imageType[0] + "|*.bmp|");
            tmp.Append(m_imageType[1] + "|*.jpeg|");
            tmp.Append(m_imageType[2] + "|*.png|");
            tmp.Append(m_imageType[3] + "|*.tga|");
            tmp.Append(m_imageType[4] + "|*.tif|");

            m_filter = tmp.ToString().TrimEnd('|');
            m_title = "Export IMG";
        }


        /// <summary>
        /// Collect the parameters and export
        /// </summary>
        /// <returns></returns>
        public override bool Export()
        {
            base.Export();
            return true;
        }

        #endregion
    }
}
