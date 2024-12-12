//
// (C) Copyright 2003-2014 by Autodesk, Inc.
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
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.TransactionControl.CS
{
    using SystemColor = System.Drawing.Color;

    /// <summary>
    /// A Form used to deal with transaction and create, move or delete a wall
    /// </summary>
    public partial class TransactionForm : System.Windows.Forms.Form
    {
        /// <summary>
        /// An enum used to indicate which operation is executed
        /// </summary>
        private enum OperationType
        {
            StartTransactionGroup,
            RollbackTransactionGroup,
            CommitTransactionGroup,
            StartTransaction,
            CommitTransaction,
            RollbackTransaction,
            ObjectModification,
            ObjectDeletion
        }

        /// <summary>
        /// A reference to the external command data.
        /// </summary>
        private ExternalCommandData m_commandData;
        /// <summary>
        /// The active document
        /// </summary>
        private Document m_document;
        /// <summary>
        /// The root node of the operation tree
        /// </summary>
        private TreeNode m_rootNode;
        /// <summary>
        /// A reference to active transaction group node where sub transaction node will be added 
        /// </summary>
        private TreeNode m_transGroupNode;
        /// <summary>
        /// A reference to the active sub transaction node
        /// </summary>
        private TreeNode m_transNode;
        /// <summary>
        /// fore color of tree node before a transaction is over
        /// </summary>
        private SystemColor m_startedColor = SystemColor.Green;
        /// <summary>
        /// fore color of tree node after ending a transaction
        /// </summary>
        private SystemColor m_committedColor = SystemColor.Black;
        /// <summary>
        /// fore color of tree node after aborting a transaction
        /// </summary>
        private SystemColor m_rolledbackColor = SystemColor.Gray;
        /// <summary>
        /// fore color of a tree node of a created or modified element
        /// </summary>
        private SystemColor m_normalColor = SystemColor.Blue;
        /// <summary>
        /// fore color of a tree node of a deleted element
        /// </summary>
        private SystemColor m_deletedColor = SystemColor.Red;
        /// <summary>
        /// The currently active transaction group
        /// </summary>
        private TransactionGroup m_transactionGroup = null;
        /// The main, hidden outer transaction group
        /// </summary>
        private TransactionGroup m_mainTtransactionGroup = null;
        /// <summary>
        /// The active transaction
        /// </summary>
        private Transaction m_transaction = null;
        /// <summary>
        /// The number of transactions
        /// </summary>
        private int m_transCount = 0;
        /// <summary>
        /// The number of transaction groups
        /// </summary>
        private int m_transGroupCount = 0;
        /// <summary>
        /// The last created wall in the active transaction
        /// </summary>
        private Wall m_lastCreatedWall = null;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="commandData">the external command data</param>
        public TransactionForm(ExternalCommandData commandData)
        {
            m_commandData = commandData;
            m_document = m_commandData.Application.ActiveUIDocument.Document;
            if (m_document == null)
            {
                TaskDialog.Show("Revit", "There is no active document.");
            }

            InitializeComponent();

            // created the root node
            m_rootNode = new TreeNode("Command history");
            this.transactionsTreeView.Nodes.Add(m_rootNode);

            // set availability of form buttons
            UpdateButtonsStatus();

            // start the main transaction group (will be hidden to the user)
            m_mainTtransactionGroup = new TransactionGroup(m_document);
            m_mainTtransactionGroup.Start();

        }

        /// <summary>
        /// Begin a transaction, append transaction node to tree view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void startTransButton_Click(object sender, EventArgs e)
        {
            int transNo = m_transCount + 1;
            m_transaction = new Transaction(m_document, "Transaction " + transNo.ToString());
            if (m_transaction.Start() == TransactionStatus.Started)
            {
                m_transCount++;
                AddNode(OperationType.StartTransaction);
                m_lastCreatedWall = null;
                UpdateButtonsStatus();
            }
            else
            {
                m_transaction = null;
                TaskDialog.Show("Revit", "Starting transaction failed");
            }
        }

        /// <summary>
        /// Commit a transaction
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void commitTransButton_Click(object sender, EventArgs e)
        {
            if ((m_transaction != null) && (m_transaction.GetStatus() == TransactionStatus.Started))
            {
                m_transaction.Commit();
                if (m_transaction.HasEnded())
                {
                    // in theory, committing a transaction can turn out to a be roll-back instead
                    // depending on how model failures, if any, are resolved by the user
                    if (m_transaction.GetStatus() == TransactionStatus.Committed)
                        AddNode(OperationType.CommitTransaction);
                    else
                        AddNode(OperationType.RollbackTransaction);

                    m_transaction = null;
                    m_lastCreatedWall = null;
                    UpdateButtonsStatus();
                }
                else
                {
                    TaskDialog.Show("Revit", "Committing transaction failed");
                }
            }
        }

        /// <summary>
        /// Rollback a transaction
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rollbackTransButton_Click(object sender, EventArgs e)
        {
            if ((m_transaction != null) && (m_transaction.GetStatus() == TransactionStatus.Started))
            {
                if (m_transaction.RollBack() == TransactionStatus.RolledBack)
                {
                    m_transaction = null;
                    m_lastCreatedWall = null;
                    AddNode(OperationType.RollbackTransaction);
                    UpdateButtonsStatus();
                }
                else
                {
                    TaskDialog.Show("Revit", "Rolling Back transaction failed");
                }
            }
        }

        /// <summary>
        /// Create a wall, append a node to tree view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void createWallbutton_Click(object sender, EventArgs e)
        {
            // a sub-transaction is not necessary in this case
            // it is used for illustration purposes only
            using (SubTransaction subTransaction = new SubTransaction(m_document))
            {
                // if not handled explicitly, the sub-transaction will be rolled back when leaving this block
                try
                {
                    if (subTransaction.Start() == TransactionStatus.Started)
                    {
                        using (CreateWallForm createWallForm = new CreateWallForm(m_commandData))
                        {
                            createWallForm.ShowDialog();
                            if (DialogResult.OK == createWallForm.DialogResult)
                            {
                                updateModel(true);  // immediately update the view to see the changes

                                if (subTransaction.Commit() == TransactionStatus.Committed)
                                {
                                    m_lastCreatedWall = createWallForm.CreatedWall;
                                    AddNode(OperationType.ObjectModification, "Created wall " + m_lastCreatedWall.Id.IntegerValue.ToString());
                                    UpdateButtonsStatus();
                                    return;
                                }
                            }
                            else
                            {
                                subTransaction.RollBack();
                                return;
                            }
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    TaskDialog.Show("Revit", "Exception when creating a wall: " + ex.Message);
                }
            }
            TaskDialog.Show("Revit", "Creating wall failed");
        }

        /// <summary>
        /// Move a wall, append a node to tree view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void moveWallButton_Click(object sender, EventArgs e)
        {
            if (m_lastCreatedWall == null)
                return;

            // a sub-transaction is not necessary in this case
            // it is used for illustration purposes only
            using (SubTransaction subTransaction = new SubTransaction(m_document))
            {
                // if not handled explicitly, the sub-transaction will be rolled back when leaving this block
                try
                {
                    if (subTransaction.Start() == TransactionStatus.Started)
                    {
                        Autodesk.Revit.DB.XYZ translationVec = new Autodesk.Revit.DB.XYZ(10, 10, 0);
                        ElementTransformUtils.MoveElement(m_document, m_lastCreatedWall.Id, translationVec);
                        updateModel(true);  // immediately update the view to see the changes

                        if (subTransaction.Commit() == TransactionStatus.Committed)
                        {
                            AddNode(OperationType.ObjectModification, "Moved wall " + m_lastCreatedWall.Id.IntegerValue.ToString());
                            return;
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    TaskDialog.Show("Revit", "Exception when moving a wall: " + ex.Message);
                }
            }
            TaskDialog.Show("Revit", "Moving wall failed.");
        }

        /// <summary>
        /// Delete a wall, append a node to tree view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteWallButton_Click(object sender, EventArgs e)
        {
            if (m_lastCreatedWall == null)
                return;

            // a sub-transaction is not necessary in this case
            // it is used for illustration purposes only
            using (SubTransaction subTransaction = new SubTransaction(m_document))
            {
                // if not handled explicitly, the sub-transaction will be rolled back when leaving this block
                try
                {
                    String wallId = m_lastCreatedWall.Id.IntegerValue.ToString();

                    if (TaskDialogResult.No ==
                        TaskDialog.Show("Warning", "Do you really want to delete wall with id " + wallId + "?",
                     TaskDialogCommonButtons.Yes | TaskDialogCommonButtons.No))
                    {
                        return;
                    }

                    if (subTransaction.Start() == TransactionStatus.Started)
                    {
                        m_document.Delete(m_lastCreatedWall.Id);
                        updateModel(false);  // immediately update the view to see the changes

                        if (subTransaction.Commit() == TransactionStatus.Committed)
                        {
                            AddNode(OperationType.ObjectDeletion, "Deleted wall " + wallId);
                            m_lastCreatedWall = null;
                            UpdateButtonsStatus();
                            return;
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    TaskDialog.Show("Revit", "Exception when deleting a wall: " + ex.Message);
                }
            }
            TaskDialog.Show("Revit", "Deleting wall failed.");
        }

        /// <summary>
        /// Start transaction group button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStartTransGroup_Click(object sender, EventArgs e)
        {
            m_transGroupCount++;
            m_transactionGroup = new TransactionGroup(m_document, "Transaction Group " + m_transGroupCount.ToString());
            m_transactionGroup.Start();

            AddNode(OperationType.StartTransactionGroup);

            UpdateButtonsStatus();
        }

        /// <summary>
        /// Commit transaction group button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCommitTransGroup_Click(object sender, EventArgs e)
        {
            if (m_transactionGroup != null)
            {
                m_transactionGroup.Commit();
                AddNode(OperationType.CommitTransactionGroup);
                m_transaction = null;
                m_lastCreatedWall = null;
                UpdateButtonsStatus();
            }
        }

        /// <summary>
        /// Rollback transaction group button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRollbackTransGroup_Click(object sender, EventArgs e)
        {
            if (m_transactionGroup != null)
            {
                m_transactionGroup.RollBack();
                AddNode(OperationType.RollbackTransactionGroup);
                m_transaction = null;
                m_lastCreatedWall = null;
                UpdateButtonsStatus();
            }
        }

        /// <summary>
        /// Accept the changes and and close this form
        /// </summary>
        /// <remarks>If any transaction group or the active transaction is still open
        /// give the user the option to either commit them or roll them back first.</remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void okButton_Click(object sender, EventArgs e)
        {
            //if any transaction or transaction group is not finished, notify user to deal with it
            if (null != m_transaction || null != m_transactionGroup)
            {
                TaskDialogResult dialogResult =
                   TaskDialog.Show("Warning", "Some transaction groups or the active transaction is not committed."
                   + " Choose Yes to commit them, or No to roll them back.", TaskDialogCommonButtons.Yes | TaskDialogCommonButtons.No | TaskDialogCommonButtons.Cancel);

                if (TaskDialogResult.Cancel == dialogResult)
                {
                    return;
                }
                else if (TaskDialogResult.Yes == dialogResult)
                {
                    if (m_transaction != null && (m_transaction.GetStatus() == TransactionStatus.Started))
                    {
                        m_transaction.Commit();
                    }
                    HandleNestedTransactionGroups(OperationType.CommitTransactionGroup);
                }
                else
                {
                    if (m_transaction != null && (m_transaction.GetStatus() == TransactionStatus.Started))
                    {
                        m_transaction.RollBack();
                    }
                    HandleNestedTransactionGroups(OperationType.RollbackTransactionGroup);
                }
            }

            // silently commit the main (hidden) transaction group
            if ((null != m_mainTtransactionGroup) && (m_mainTtransactionGroup.GetStatus() == TransactionStatus.Started))
            {
                m_mainTtransactionGroup.SetName("SDK Transaction Sample");
                m_mainTtransactionGroup.Assimilate();
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// Rollback all changes and close this form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancelButton_Click(object sender, EventArgs e)
        {
            if (m_transCount > 0)
            {
                if (TaskDialogResult.No ==
                   TaskDialog.Show("Warning", "By canceling this dialog, all modifications to the model will be discarded."
                    + " Do you want to proceed?", TaskDialogCommonButtons.Yes | TaskDialogCommonButtons.No))
                {
                    this.DialogResult = DialogResult.None;
                    return;
                }
            }

            // if there is still an active transaction, roll it back now, silently
            if ((null != m_transaction) && (m_transaction.GetStatus() == TransactionStatus.Started))
            {
                m_transaction.RollBack();
            }

            // if there are still transaction groups open, roll them back now, silently
            if ((null != m_transactionGroup) && (m_transactionGroup.GetStatus() == TransactionStatus.Started))
            {
                HandleNestedTransactionGroups(OperationType.RollbackTransactionGroup);
            }

            // silently roll back the main (hidden) transaction group
            if ((null != m_mainTtransactionGroup) && (m_mainTtransactionGroup.GetStatus() == TransactionStatus.Started))
            {
                m_mainTtransactionGroup.RollBack();
            }

            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// Commit or rollback nested transaction groups
        /// </summary>
        /// <param name="operationType">The operation type determines whether to commit or to rollback transaction groups</param>
        private void HandleNestedTransactionGroups(OperationType operationType)
        {
            while (m_transactionGroup != null)
            {
                if (operationType.Equals(OperationType.CommitTransactionGroup))
                {
                    m_transactionGroup.Commit();
                }
                else
                {
                    m_transactionGroup.RollBack();
                }

                if (m_transactionGroup.HasEnded())
                {
                    m_transGroupNode = m_transGroupNode.Parent;
                    if (m_transGroupNode.Equals(m_rootNode))
                    {
                        m_transactionGroup = null;
                    }
                    else
                    {
                        m_transactionGroup = m_transGroupNode.Tag as TransactionGroup;
                    }
                }
                else
                {
                    throw new System.ApplicationException("Could not end a transaction group");
                }
            }
        }

        /// <summary>
        /// Update form buttons depending on the status of existing transaction and groups
        /// </summary>
        private void UpdateButtonsStatus()
        {
            // there can be only one active transaction at any given time 
            btnStartTrans.Enabled = (m_transaction == null);

            // only active transaction can be committed 
            btnCommitTrans.Enabled = ((m_transaction != null) && (m_transaction.GetStatus() == TransactionStatus.Started));

            // the same rule applies to rolling back a transaction
            btnRollbackTrans.Enabled = btnCommitTrans.Enabled;

            // transaction group cannot be started inside an active transaction
            btnStartTransGroup.Enabled = (m_transaction == null);

            // transaction group cannot be committed while still inside an active transaction
            // also, the group must be active in order to be committed
            btnCommitTransGroup.Enabled = ((m_transaction == null) && (m_transactionGroup != null) && (m_transactionGroup.GetStatus() == TransactionStatus.Started));

            // the same rule applies to rolling back a transaction group
            btnRollbackTransGroup.Enabled = btnCommitTransGroup.Enabled;

            // wall can be created only in an active transaction
            btnCreateWall.Enabled = btnCommitTrans.Enabled;

            // the same rule applies to deleting a wall, plus, naturally, the wall must exist
            btnDeleteWall.Enabled = (btnCreateWall.Enabled && (m_lastCreatedWall != null));

            // the same applies to moving a wall
            btnMoveWall.Enabled = btnDeleteWall.Enabled;
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
            if (type == OperationType.StartTransactionGroup)
            {
                if (m_transGroupNode == null)
                {
                    m_transGroupNode = new TreeNode(m_transactionGroup.GetName());
                    int index = m_rootNode.Nodes.Add(m_transGroupNode);
                    m_rootNode.Nodes[index].Tag = m_transactionGroup;
                    m_rootNode.Expand();
                    UpdateTreeNode(m_transGroupNode, type);
                }
                else
                {
                    TreeNode newTransGroupNode = new TreeNode(m_transactionGroup.GetName());
                    int index = m_transGroupNode.Nodes.Add(newTransGroupNode);
                    m_transGroupNode.Nodes[index].Tag = m_transactionGroup;
                    m_transGroupNode.Expand();
                    m_transGroupNode = newTransGroupNode;
                    m_transGroupNode.Expand();
                    UpdateTreeNode(m_transGroupNode, type);
                }
                m_transNode = null;
                m_transaction = null;
            }
            else if (type == OperationType.RollbackTransactionGroup)
            {
                UpdateTreeNode(m_transGroupNode, type);
                if (m_transGroupNode.Parent.Equals(m_rootNode))
                {
                    m_rootNode.Expand();
                    m_transactionGroup = null;
                    m_transGroupNode = null;
                }
                else
                {
                    m_transGroupNode = m_transGroupNode.Parent;
                    m_transGroupNode.Expand();
                    m_transactionGroup = m_transGroupNode.Tag as TransactionGroup;
                }
                m_transNode = null;
                m_transaction = null;
            }
            else if (type == OperationType.CommitTransactionGroup)
            {
                UpdateTreeNode(m_transGroupNode, type);
                if (m_transGroupNode.Parent.Equals(m_rootNode))
                {
                    m_rootNode.Expand();
                    m_transactionGroup = null;
                    m_transGroupNode = null;
                }
                else
                {
                    m_transGroupNode.Expand();
                    m_transGroupNode = m_transGroupNode.Parent;
                    m_transactionGroup = m_transGroupNode.Tag as TransactionGroup;
                }
                m_transNode = null;
                m_transaction = null;
            }
            else if (type == OperationType.StartTransaction)
            {
                m_transNode = new TreeNode(m_transaction.GetName());
                m_transNode.ForeColor = m_startedColor;
                TreeNode node = m_transGroupNode == null ? m_rootNode : m_transGroupNode;
                node.Nodes.Add(m_transNode);
                node.Expand();
                UpdateTreeNode(m_transNode, type);
            }
            else if (type == OperationType.CommitTransaction)
            {
                UpdateTreeNode(m_transNode, type);
                TreeNode node = m_transGroupNode == null ? m_rootNode : m_transGroupNode;
                node.Expand();
                m_transNode = null;
            }
            else if (type == OperationType.RollbackTransaction)
            {
                UpdateTreeNode(m_transNode, type);
                TreeNode node = m_transGroupNode == null ? m_rootNode : m_transGroupNode;
                node.Expand();
                m_transNode = null;
            }
            else
            {
                string childNodeText = null;

                if (String.IsNullOrEmpty(info))
                {
                    childNodeText = "Operation";
                }
                else
                {
                    childNodeText = info;
                }

                TreeNode childNode = new TreeNode(childNodeText);
                if (type == OperationType.ObjectDeletion)
                    childNode.ForeColor = m_deletedColor;
                else
                    childNode.ForeColor = m_normalColor;
                m_transNode.Nodes.Add(childNode);
                m_transNode.Expand();
            }
        }

        /// <summary>
        /// Update the color of a tree node
        /// </summary>
        /// <param name="parentNode">indicate child nodes of which will be updated</param>
        /// <param name="type">operation type</param>
        private void UpdateTreeNode(TreeNode parentNode, OperationType type)
        {
            switch (type)
            {
                case OperationType.StartTransaction:
                case OperationType.StartTransactionGroup:
                    UpdateTreeNode(parentNode, m_startedColor);
                    break;

                case OperationType.RollbackTransaction:
                case OperationType.RollbackTransactionGroup:
                    UpdateTreeNode(parentNode, m_rolledbackColor);
                    break;

                case OperationType.CommitTransaction:
                case OperationType.CommitTransactionGroup:
                    UpdateTreeNode(parentNode, m_committedColor);
                    break;

                default:
                    UpdateTreeNode(parentNode, m_normalColor);
                    break;
            }
        }

        /// <summary>
        /// Update the color of tree node including all its sub-nodes
        /// </summary>
        /// <param name="parentNode">the node of which color will be updated</param>
        /// <param name="color">Color to change</param>
        private void UpdateTreeNode(TreeNode parentNode, SystemColor color)
        {
            parentNode.ForeColor = color;
            foreach (TreeNode childNode in parentNode.Nodes)
            {
                if (childNode.ForeColor == m_rolledbackColor)
                {
                    continue;
                }
                UpdateTreeNode(childNode, color);
            }
        }

        /// <summary>
        /// Updates the model and refreshes the active view
        /// </summary>
        /// <param name="autoJoin">Whether to also perform auto-join or not</param>
        private void updateModel(bool autoJoin)
        {
            // in order to be able to see changes to the model before 
            // the current transaction is committed, we have to regenerate
            // the model manually.
            m_document.Regenerate();

            // auto-joining is optional, but may be necessary to see connection details.
            if (autoJoin)
            {
                m_document.AutoJoinElements();
            }

            // to see the changes immediately, we need to refresh the view
            m_commandData.Application.ActiveUIDocument.RefreshActiveView();
        }
    }
}