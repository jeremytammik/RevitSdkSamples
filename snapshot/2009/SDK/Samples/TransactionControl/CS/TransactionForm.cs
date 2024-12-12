//
// (C) Copyright 2003-2008 by Autodesk, Inc.
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted,
// provided that the above copyright notify appears in all copies and
// that both that copyright notify and the limited warranty and
// restricted rights notify below appear in all supporting
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

using Autodesk.Revit;
using Autodesk.Revit.Elements;

namespace Revit.SDK.Samples.TransactionControl.CS
{
    using SystemColor = System.Drawing.Color;

    /// <summary>
    /// A Form used to deal with transaction and create, move or delete a wall
    /// </summary>
    public partial class TransactionForm : Form
    {
        /// <summary>
        /// an enum used to indicate which operation is executed
        /// </summary>
        private enum OperationType
        {
            BeginTransaction,
            EndTransaction,
            AbortTransaction,
            APIOperation
        }

        private ExternalCommandData m_commandData;  //a reference to the external command data
        private TreeNode m_parentNode;  //a reference to tree node where sub node will be added 
        private int m_activeTransCount = 0; //the number of transactions which is not over
        private int m_totalTransCount = 0;  //the number of all transactions or method "BeginTransaction" is called
        private SystemColor m_beginTransColor = SystemColor.Green;  //fore color of tree node before a transaction is over
        private SystemColor m_endTransColor = SystemColor.Black;    //fore color of tree node after ending a transaction
        private SystemColor m_abortTransColor = SystemColor.Gray;   //fore color of tree node after aborting a transaction
        private SystemColor m_normalColor = SystemColor.Blue;   //fore color of tree node which is not executed during a transaction

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="commandData">the external command data</param>
        public TransactionForm(ExternalCommandData commandData)
        {
            InitializeComponent();
            m_commandData = commandData;

            m_parentNode = new TreeNode("Flow");
            this.transactionsTreeView.Nodes.Add(m_parentNode);

            UpdateButtonsStatus();
        }

        /// <summary>
        /// Begin a transaction, append transaction node to tree view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void beginTransButton_Click(object sender, EventArgs e)
        {
            m_commandData.Application.ActiveDocument.BeginTransaction();
            AddNode(OperationType.BeginTransaction);
            UpdateButtonsStatus();
        }

        /// <summary>
        /// End a transaction
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void endTransButton_Click(object sender, EventArgs e)
        {
            if (m_activeTransCount < 1)
            {
                return;
            }
            m_commandData.Application.ActiveDocument.EndTransaction();
            AddNode(OperationType.EndTransaction);
            UpdateButtonsStatus();
        }

        /// <summary>
        /// Abort a transaction
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void abortTransButton_Click(object sender, EventArgs e)
        {
            if (m_activeTransCount < 1)
            {
                return;
            }
            m_commandData.Application.ActiveDocument.AbortTransaction();
            AddNode(OperationType.AbortTransaction);
            UpdateButtonsStatus();
        }

        /// <summary>
        /// Create a wall, append a node to tree view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void createWallbutton_Click(object sender, EventArgs e)
        {
            try
            {
                using (CreateWallForm createWallForm = new CreateWallForm(m_commandData))
                {
                    createWallForm.ShowDialog();
                    if (DialogResult.OK == createWallForm.DialogResult)
                    {
                        AddNode(OperationType.APIOperation, "Create a wall");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Create wall failed: " + ex.Message);
            }
        }

        /// <summary>
        /// Move a wall, append a node to tree view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void moveWallButton_Click(object sender, EventArgs e)
        {
            try
            {
                Document document = m_commandData.Application.ActiveDocument;
                // iterate elements to find a wall to move
                ElementIterator iter = document.get_Elements(typeof(Wall));
                iter.Reset();
                while (iter.MoveNext())
                {
                    if (null != iter.Current)
                    {
                        Autodesk.Revit.Geometry.XYZ translationVec = 
                            new Autodesk.Revit.Geometry.XYZ(10, 10, 0);
                        document.Move(iter.Current as Wall, translationVec);
                        AddNode(OperationType.APIOperation, "Move a wall");
                        return;
                    }
                }
                //if no wall was found, show a message box to notify user
                MessageBox.Show("No wall exists.");
            }
            catch
            {
                //if an Exception was thrown, show a message box to notify user
                MessageBox.Show("Move wall failed.");
            }
        }

        /// <summary>
        /// Delete a wall, append a node to tree view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteWallButton_Click(object sender, EventArgs e)
        {
            try
            {
                Document document = m_commandData.Application.ActiveDocument;
                ElementIterator iter = document.get_Elements(typeof(Wall));
                iter.Reset();
                // iterate elements to find a wall to delete
                while (iter.MoveNext())
                {
                    if (null != iter.Current)
                    {
                        //show a message box for user to confirm
                        if (DialogResult.Yes ==
                            MessageBox.Show("Delete a wall?", "Warning", MessageBoxButtons.YesNo))
                        {
                            document.Delete(iter.Current as Wall);
                            AddNode(OperationType.APIOperation, "Delete a wall");
                        }
                        return;
                    }
                }
                //if did not find a wall, show a message box to notify user
                MessageBox.Show("No wall exists.");
            }
            catch
            {
                //if an Exception is thrown, show a message box to notify user
                MessageBox.Show("Delete wall failed.");
            }
        }

        /// <summary>
        /// End the transactions that are not over and close this dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void okButton_Click(object sender, EventArgs e)
        {
            //if some transactions are not over, notify user to deal with
            if (0 != m_activeTransCount)
            {
                DialogResult dialogResult = MessageBox.Show("Some transactions are not over. Do you want to end all of them?", "Warning", MessageBoxButtons.YesNoCancel);
                if(DialogResult.Cancel == dialogResult)
                {
                    return;
                }

                if (DialogResult.Yes == dialogResult)
                {
                    while (0 != m_activeTransCount)
                    {
                        m_commandData.Application.ActiveDocument.EndTransaction();
                        m_activeTransCount--;
                    }
                }                
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// If there are transactions, enable endButton and abortButton
        /// otherwise disable them.
        /// </summary>
        private void UpdateButtonsStatus()
        {
            endTransButton.Enabled = abortTransButton.Enabled = m_activeTransCount > 0;
        }

        /// <summary>
        /// add node to tree view control
        /// </summary>
        /// <param name="type">indicate operation type</param>
        private void AddNode(OperationType type)
        {
            AddNode(type, null);
        }

        /// <summary>
        /// add node to tree view control
        /// </summary>
        /// <param name="type">indicate operation type</param>
        /// <param name="info">tree node text</param>
        private void AddNode(OperationType type, string info)
        {
            //add tree node according to operation type 
            if (type == OperationType.BeginTransaction)
            {
                m_activeTransCount++;
                m_totalTransCount++;
                TreeNode childNode = 
                    new TreeNode("Transaction(" + m_totalTransCount.ToString() + ")");
                childNode.ForeColor = m_beginTransColor;
                m_parentNode.Nodes.Add(childNode);
                m_parentNode.Expand();
                m_parentNode = childNode;
            }
            else if (type == OperationType.EndTransaction)
            {
                m_activeTransCount--;
                UpdateTreeNode(m_parentNode, type);
                m_parentNode.Expand();
                //current transaction is over,so change m_parentNode to its parent
                m_parentNode = m_parentNode.Parent;

            }
            else if (type == OperationType.AbortTransaction)
            {
                m_activeTransCount--;
                UpdateTreeNode(m_parentNode, type);
                m_parentNode.Expand();
                //current transaction is over,so change m_parentNode to its parent
                m_parentNode = m_parentNode.Parent;
            }
            else
            {
                string childNodeText = null;
                SystemColor childNodeColor;

                //if variable "info" is null, use default value 
                if (info == null)
                {
                    childNodeText = "Operation";
                }
                else
                {
                    childNodeText = info;
                }

                //if m_activeTransCount is not zero, the operation creating wall,
                //moving wall or deleting wall must occur during a transaction
                if (m_activeTransCount == 0)
                {
                    childNodeColor = m_normalColor;
                }
                else
                {
                    childNodeColor = m_beginTransColor;
                }

                TreeNode childNode = new TreeNode(childNodeText);
                childNode.ForeColor = childNodeColor;
                m_parentNode.Nodes.Add(childNode);
                m_parentNode.Expand();
            }
        }

        /// <summary>
        /// Update the color of tree node
        /// </summary>
        /// <param name="parentNode">indicate child nodes of which will be updated</param>
        /// <param name="type">operation type</param>
        private void UpdateTreeNode(TreeNode parentNode, OperationType type)
        {
            if (type == OperationType.BeginTransaction)
            {
                UpdateTreeNode(parentNode, m_beginTransColor);
            }
            else if (type == OperationType.EndTransaction)
            {
                UpdateTreeNode(parentNode, m_endTransColor);
            }
            else if (type == OperationType.AbortTransaction)
            {
                UpdateTreeNode(parentNode, m_abortTransColor);
            }
        }

        /// <summary>
        /// Update the color of tree node
        /// </summary>
        /// <param name="parentNode">indicate child nodes of which will be updated</param>
        /// <param name="color">Color to change</param>
        private void UpdateTreeNode(TreeNode parentNode, SystemColor color)
        {
            parentNode.ForeColor = color;
            foreach (TreeNode childNode in parentNode.Nodes)
            {
                UpdateTreeNode(childNode, color);
            }
        }
    }


}