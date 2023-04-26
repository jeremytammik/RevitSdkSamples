//
// (C) Copyright 2003-2019 by Autodesk, Inc.
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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Revit.SDK.Samples.ImportExport.CS
{
   /// <summary>
   /// Data class which stores information of lower priority for exporting PDF format.
   /// </summary>
   public partial class ExportPDFOptionsForm : Form
   {
      /// <summary>
      /// ExportPDFData object
      /// </summary>
      private ExportPDFData m_data;

    /// <summary>
    /// ExportPDFOptionsForm constructor
    /// </summary>
    public ExportPDFOptionsForm(ExportPDFData data)
      {
         m_data = data;
         InitializeComponent();
         Initialize();
      }

      /// <summary>
      /// Initialize controls
      /// </summary>
      private void Initialize()
      {
         checkBoxCombineViews.Checked = m_data.Combine;
      }

      private void buttonOK_Click(object sender, EventArgs e)
      {
         m_data.Combine = checkBoxCombineViews.Checked;
      }
   }
}
