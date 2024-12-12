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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Revit.SDK.Samples.ImportExport.CS
{
    /// <summary>
    /// Dialog provides options for exporting dxf format
    /// </summary>
    public partial class ExportDXFOptionsForm : ExportBaseOptionsForm
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="exportOptionsData"></param>
        /// <param name="contain3DView"></param>
        public ExportDXFOptionsForm(ExportBaseOptionsData exportOptionsData, bool contain3DView)
            : base(exportOptionsData, contain3DView)
        {
            this.Text = "Export DXF Options";
        }
    }
}
