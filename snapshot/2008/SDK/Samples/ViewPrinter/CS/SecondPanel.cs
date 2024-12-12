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
using System.Collections;
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
    /// The next page should display all the printable views in the project that they chose.
    /// A tree view should be used to allow them to select which views they wish to print 
    /// </summary>
    public partial class SecondPanel : UserControl
    {
        /// <summary>
        /// An object operate with Revit.
        /// </summary>
        private PrintMgr m_viewPrinter;

        /// <summary>
        /// public constructor.
        /// </summary>
        public SecondPanel()
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
        /// Select which views they wish to print and allow to check the node's child nodes.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="check"></param>
        private void CheckNode(TreeNode node, bool check)
        {
            if (0 < node.Nodes.Count)//has children.
            {
                foreach (TreeNode t in node.Nodes)
                {
                    t.Checked = check;
                    CheckNode(t, check);
                }
            }
            else//No children
            {
                if (node.Checked)//Add to selection.
                {
                    foreach (RView view in m_viewPrinter.PrintableViews)
                    {
                        if (node.Name.Equals(view.Id.Value.ToString()))
                        {
                            m_viewPrinter.SelectedViews.Insert(view);
                            break;
                        }
                    }
                }
                else//Delete from selection.
                {
                    foreach (RView view in m_viewPrinter.SelectedViews)
                    {
                        if (node.Name.Equals(view.Id.Value.ToString()))
                        {
                            m_viewPrinter.SelectedViews.Erase(view);
                            break;
                        }
                    }
                }
            }

            //Update parent state with children.
            if (null == node.Parent)
            {
                return;
            }

            foreach (TreeNode treeNode in node.Parent.Nodes)
            {
                if (treeNode.Checked)
                {
                    if (!node.Parent.Checked)
                    {
                        node.Parent.Checked = true;
                    }

                    return;
                }
            }

            node.Parent.Checked = false;
        }

        /// <summary>
        /// Select which views they wish to print and allow to check the node's child nodes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void viewToPrintTreeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Action == TreeViewAction.ByMouse)
            {
                CheckNode(e.Node, e.Node.Checked);
            }
        }

        /// <summary>
        /// Simulate the tree view as project browser in Revit UI.
        /// </summary>
        public void FormatViewTree()
        {
            ViewSet views = m_viewPrinter.PrintableViews;

            viewToPrintTreeView.Nodes.Clear();
            m_viewPrinter.SelectedViews.Clear();
            viewToPrintTreeView.BeginUpdate();
            viewToPrintTreeView.Nodes.Add("Views (printable)");
            foreach (RView view in views)
            {
                string category = view.ViewType.ToString();

                if (null == category)
                {
                    category = "UnKnown";
                }

                bool isCategoried = false;
                foreach (TreeNode tn in viewToPrintTreeView.Nodes[0].Nodes)
                {
                    if (category.Equals(tn.Name))
                    {
                        TreeNode viewNode = new TreeNode(view.Id.Value.ToString());
                        viewNode.Name = view.Id.Value.ToString();
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
                viewToPrintTreeView.Nodes[0].Nodes.Add(typeNode);
                TreeNode childViewNode = new TreeNode(view.Id.Value.ToString());
                childViewNode.Name = view.Id.Value.ToString();
                childViewNode.Text = view.Name;
                typeNode.Nodes.Add(childViewNode);
            }
            viewToPrintTreeView.ExpandAll();
            viewToPrintTreeView.EndUpdate();
        }
    }
}
