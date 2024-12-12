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
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Revit.SDK.Samples.ViewPrinter.CS
{
    /// <summary>
    /// The first page of the wizard should ask the user if they want to print from
    /// the currently open project or from a file on disk. Selecting file from disk should 
    /// open a .rvt file browser and allow the user to pick a revit project.
    /// </summary>
    public partial class FirstPanel : UserControl
    {
        /// <summary>
        /// An object operate with Revit.
        /// </summary>
        private PrintMgr m_viewPrinter;

        /// <summary>
        /// public constructor.
        /// </summary>
        public FirstPanel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialize the object PrintMgr.
        /// </summary>
        /// <param name="printMgr"></param>
        public void InitializePrintMgr(PrintMgr printMgr)
        {
            if (null == printMgr)
            {
                throw new ArgumentNullException();
            }

            m_viewPrinter = printMgr;
            proToPrintTextBox.Text = m_viewPrinter.ProjectInfo;
        }

        /// <summary>
        /// ask the user if they want to print from
        /// the currently open project or from a file on disk
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void currentProRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            proOnDiskTextBox.Enabled = proOnDiskRadioButton.Checked;
            proOnDiskButton.Enabled = proOnDiskRadioButton.Checked;
        }

        /// <summary>
        /// open a .rvt file browser and allow the user to pick a revit project.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void proOnDiskButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "rvt files (*.rvt)|*.rvt";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                proOnDiskTextBox.Text = openFileDialog.FileName;
                m_viewPrinter.UpdateProject(proOnDiskTextBox.Text);
                proToPrintTextBox.Text = m_viewPrinter.ProjectInfo;
            }
        }
    }
}
