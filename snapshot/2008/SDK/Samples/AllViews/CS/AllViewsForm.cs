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
using System.Collections;

namespace Revit.SDK.Samples.AllViews.CS
{
    /// <summary>
    /// This is a dialog should appear that contains the following:
    /// A tree view represents all the views' names.
    /// A list of all title blocks.
    /// An edit box for the sheet's name.
    /// </summary>
    public partial class AllViewsForm : Form
    {
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="data"></param>
        public AllViewsForm(ViewsMgr data)
        {
            m_data = data;
            InitializeComponent();
        }

        private void AllViewsForm_Load(object sender, EventArgs e)
        {
            allViewsTreeView.Nodes.Add(m_data.AllViewsNames);
            allViewsTreeView.TopNode.Expand();

            foreach (string s in m_data.AllTitleBlocksNames)
            {
                titleBlocksListBox.Items.Add(s);
            }
        }

        private void oKButton_Click(object sender, EventArgs e)
        {
            m_data.SelectViews();
            m_data.SheetName = sheetNameTextBox.Text;

            if (1 == titleBlocksListBox.SelectedItems.Count)
            {
                string titleBlock = titleBlocksListBox.SelectedItems[0].ToString();
                m_data.ChooseTitleBlock(titleBlock);
            }
        }

        #region CheckTreeNode
        private void CheckNode(TreeNode node, bool check)
        {
            if (0 < node.Nodes.Count)
            {
                if (node.Checked)
                {
                    node.Expand();
                }
                else
                {
                    node.Collapse();
                }

                foreach (TreeNode t in node.Nodes)
                {
                    t.Checked = check;
                    CheckNode(t, check);
                }
            }
        }

        private void allViewsTreeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            CheckNode(e.Node, e.Node.Checked);
        } 
        #endregion

        /// <summary>
        /// Select title block to generate sheet.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void titleBlocksListBox_MouseClick(object sender, MouseEventArgs e)
        {
            int idx = titleBlocksListBox.SelectedIndex;
            if (0 < idx)
            {
                titleBlocksListBox.SetSelected(idx, true);
            }            
        }

    }
}