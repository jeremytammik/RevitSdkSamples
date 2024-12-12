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

using Autodesk.Revit.Elements;
using RView = Autodesk.Revit.Elements.View;

namespace Revit.SDK.Samples.ViewPrinter.CS
{
    /// <summary>
    /// The third page should be a confirmation of what they wish to do with a Print button
    /// </summary>
    public partial class ThirdPanel : UserControl
    {
        /// <summary>
        /// An object operate with Revit.
        /// </summary>
        private PrintMgr m_viewPrinter;

        /// <summary>
        /// public constructor.
        /// </summary>
        public ThirdPanel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialize the object PrintMgr.
        /// </summary>
        /// <param name="printMgr"></param>
        public void InitializePrintMgr(PrintMgr printMgr)
        {
            m_viewPrinter = printMgr;
        }

        /// <summary>
        /// Allows users to select a printer and choose which portions of the document to print.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void settingButton_Click(object sender, EventArgs e)
        {
            PrintDialog prtDlg = new PrintDialog();
            prtDlg.ShowDialog();
        }

        /// <summary>
        /// Print the selection views with default view template and default print settings.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void printButton_Click(object sender, EventArgs e)
        {
            confirmTreeView.Visible = false;            
            printingProgressBar.Visible = true;
            this.Update();
            printingProgressBar.Value = 0;
            printingProgressBar.Maximum = m_viewPrinter.SelectedViews.Size;

            foreach (RView view in m_viewPrinter.SelectedViews)
            {
                PrintMgr.Print(view);
                printingProgressBar.PerformStep();
            }

            printingProgressBar.Visible = false;
            confirmTreeView.Visible = true;                   
        }

        /// <summary>
        /// Simulate the tree view as project browser in Revit UI.
        /// </summary>
        public void FormatViewTree()
        {
            ViewSet views = m_viewPrinter.SelectedViews;

            confirmTreeView.Nodes.Clear();
            confirmTreeView.BeginUpdate();
            confirmTreeView.Nodes.Add("Views (selected)");
            foreach (RView view in views)
            {
                string category = view.ViewType.ToString();

                if (null == category)
                {
                    category = "UnKnown";
                }

                bool isCategoried = false;
                foreach (TreeNode tn in confirmTreeView.Nodes[0].Nodes)
                {
                    if (category.Equals(tn.Name))
                    {
                        TreeNode viewNode = new TreeNode(view.Id.Value.ToString());
                        viewNode.Text = view.Name;
                        tn.Nodes.Add(viewNode);
                        isCategoried = true;
                        break;
                    }
                }

                if (isCategoried)
                {
                    continue;
                }
                TreeNode typeNode = new TreeNode(category);
                typeNode.Name = category;
                confirmTreeView.Nodes[0].Nodes.Add(typeNode);
                TreeNode childViewNode = new TreeNode(view.Id.Value.ToString());
                childViewNode.Text = view.Name;
                typeNode.Nodes.Add(childViewNode);
            }
            confirmTreeView.ExpandAll();
            confirmTreeView.EndUpdate();
        }
    }
}
