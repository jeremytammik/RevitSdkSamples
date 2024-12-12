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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Revit.SDK.Samples.ImportAndExportForDWG.CS
{
    /// <summary>
    /// Provide a dialog which lets user choose the operation(export or import)
    /// </summary>
    public partial class MainForm : Form
    {
        MainData m_mainData;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mainData"></param>
        public MainForm(MainData mainData)
        {
            m_mainData = mainData;
            InitializeComponent();
        }

        /// <summary>
        /// Show the export/import dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (radioButtonExport.Checked == true)
            {
                ExportData exportData = new ExportData(m_mainData.CommandData);
                using (ExportForm exportForm = new ExportForm(exportData))
                {
                    exportForm.ShowDialog();
                }
            }
            else
            {
                ImportData importData = new ImportData(m_mainData.CommandData);
                using (ImportForm importForm = new ImportForm(importData))
                {
                    importForm.ShowDialog();
                }
            }            
        }
    }
}