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

namespace Revit.SDK.Samples.ViewPrinter.CS
{
    /// <summary>
    /// User interface
    /// </summary>
    public partial class ViewPrinterForm : Form
    {
        /// <summary>
        /// An object operate with Revit.
        /// </summary>
        private PrintMgr m_viewPrinter;

        /// <summary>
        /// The page of wizard.
        /// </summary>
        private int m_page;

        /// <summary>
        /// Public constructor.
        /// </summary>
        /// <param name="viewPrinter"></param>
        public ViewPrinterForm(PrintMgr viewPrinter)
        {
            m_viewPrinter = viewPrinter;
            InitializeComponent();
            firstPanel.InitializePrintMgr(m_viewPrinter);
            secondPanel.InitializePrintMgr(m_viewPrinter);
            thirdPanel.InitializePrintMgr(m_viewPrinter);
        }

        /// <summary>
        /// Turn to last page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lastButton_Click(object sender, EventArgs e)
        {
            switch (m_page)
            {
                case 1:
                    {
                        firstPanel.Visible = true;
                        lastButton.Enabled = false;

                        secondPanel.Visible = false;
                        thirdPanel.Visible = false;
                        break;
                    }
                case 2:
                    {
                        firstPanel.Visible = false;
                        secondPanel.Visible = true;

                        thirdPanel.Visible = false;
                        nextButton.Enabled = true;
                        break;
                    }
                default:
                    {
                        throw new InvalidOperationException();
                    }
            }

            m_page = m_page - 1;
        }

        /// <summary>
        /// Turn to next page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nextButton_Click(object sender, EventArgs e)
        {
            switch (m_page)
            {
                case 0:
                    {
                        firstPanel.Visible = false;
                        lastButton.Enabled = true;

                        secondPanel.Visible = true;
                        secondPanel.FormatViewTree();
                        thirdPanel.Visible = false;

                        break;
                    }
                case 1:
                    {
                        firstPanel.Visible = false;
                        secondPanel.Visible = false;

                        thirdPanel.Visible = true;
                        thirdPanel.FormatViewTree();
                        nextButton.Enabled = false;
                        break;
                    }
                case 2:
                    {
                        throw new InvalidOperationException();
                    }
                default:
                    {
                        throw new InvalidOperationException();
                    }
            }

            m_page = m_page + 1;
        }
    }
}