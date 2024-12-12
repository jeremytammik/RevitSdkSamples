//
// (C) Copyright 2003-2008 by Autodesk, Inc.
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
using Autodesk.Revit.Symbols;
using System.Collections;
using Autodesk.Revit.Geometry;
using System.Drawing.Drawing2D;

namespace Revit.SDK.Samples.NewHostedSweep.CS
{
    /// <summary>
    /// This form is intent to fetch edges for hosted sweep creation or modification.
    /// It contains a picture box for geometry preview and a tree view to list all the edges
    /// which hosted sweep can be created on.
    /// If the user mouse-over an edge where a hosted sweep can be created, the edge will be 
    /// highlighted in yellow. If user clicks on the highlighted edge, the edge will 
    /// be marked as selected in red color. Click it again to un-select, the color will turn back. 
    /// Edge selection from preview box will be reflected in edge list and vice versa. 
    /// The geometry displayed in the picture box can be rotated with left mouse or 
    /// arrow keys (up, down, left and right) and zoomed with right mouse.
    /// </summary>
    public partial class EdgeFetchForm : Form
    {
        #region Fields and Constructors
        
        /// <summary>
        /// Contains all the data need to fetch edges.
        /// </summary>
        private CreationData m_creationData;

        /// <summary>
        /// Flag to indicate whether or not we should cancel expand or collapse
        /// the tree-node which contains children.
        /// </summary>
        private bool m_cancelExpandOrCollapse;

        /// <summary>
        /// Active element displayed in the preview.
        /// </summary>
        private Autodesk.Revit.Element m_activeElem;

        /// <summary>
        /// Yield rotation and scale transformation for current geometry display.
        /// </summary>
        private TrackBall m_trackBall;

        /// <summary>
        /// Move the Graphics origin to preview center and flip its y-Axis.
        /// </summary>
        private Matrix m_centerMatrix;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public EdgeFetchForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Customize constructor.
        /// </summary>
        /// <param name="creationData"></param>
        public EdgeFetchForm(CreationData creationData)
            : this()
        {
            m_creationData = creationData;
            treeViewHost.StateImageList = imageListForCheckBox;
            m_trackBall = new TrackBall();
        }
        #endregion

        #region Initialize Methods

        /// <summary>
        /// Initialize the combo box data source with Autodesk.Revit.Symbols.
        /// e.g. FasciaTypes, GutterTypes, SlabEdgeTypes, and so on.
        /// </summary>
        private void InitializeTypes()
        {
            List<object> objects = new List<object>();
            object selected = null;
            foreach (object obj in m_creationData.Creator.AllTypes)
            {
                objects.Add(obj);
                if (m_creationData.Symbol != null)
                {
                    if ((obj as Autodesk.Revit.Symbol).Id.Value == m_creationData.Symbol.Id.Value)
                    {
                        selected = obj;
                    }
                }
            }
            comboBoxTypes.DataSource = objects;
            comboBoxTypes.DisplayMember = "Name";

            if (selected != null)
                comboBoxTypes.SelectedItem = selected;
        }

        /// <summary>
        /// Initialize the TreeView: create a tree according to geometry edges 
        /// and set each node's check status to unchecked.
        /// </summary>
        private void InitializeTree()
        {
            HostedSweepCreator creator = m_creationData.Creator;

            TreeNode rootNode = new TreeNode();
            rootNode.StateImageIndex = (int)CheckState.Unchecked;
            foreach (KeyValuePair<Autodesk.Revit.Element, List<Edge>> pair in creator.SupportEdges)
            {
                Autodesk.Revit.Element elem = pair.Key;
                TreeNode elemNode = new TreeNode("[Id:" + elem.Id.Value + "] " + elem.Name);
                elemNode.StateImageIndex = (int)CheckState.Unchecked;
                rootNode.Nodes.Add(elemNode);
                elemNode.Tag = elem;
                int i = 1;
                foreach (Edge edge in pair.Value)
                {
                    TreeNode edgeNode = new TreeNode("Edge " + i);
                    edgeNode.StateImageIndex = (int)CheckState.Unchecked;
                    edgeNode.Tag = edge;
                    elemNode.Nodes.Add(edgeNode);
                    ++i;
                }
            }
            rootNode.Text = "Roofs";
            if (creator is SlabEdgeCreator)
            {
                rootNode.Text = "Floors";
            }            
            treeViewHost.Nodes.Add(rootNode);
            treeViewHost.TopNode.Expand();
        }

        /// <summary>
        /// Initialize element geometry.
        /// </summary>
        private void InitializeElementGeometry()
        {
            foreach (ElementGeometry elemGeom in m_creationData.Creator.ElemGeomDic.Values)
            {
                elemGeom.InitializeTransform(pictureBoxPreview.Width * 0.8, pictureBoxPreview.Height * 0.8);
                elemGeom.ResetEdgeStates();
            }
        }

        /// <summary>
        /// Initialize tree check states according to edges which hosted sweep can be created on.
        /// </summary>
        private void InitializeTreeCheckStates()
        {
            if (m_creationData.EdgesForHostedSweep.Count == 0) return;

            // Initialize edge binding selection state
            foreach(Edge edge in m_creationData.EdgesForHostedSweep)
            {
                foreach(ElementGeometry elemGeom in m_creationData.Creator.ElemGeomDic.Values)
                {
                    if(elemGeom.EdgeBindingDic.ContainsKey(edge))
                    {                        
                        elemGeom.EdgeBindingDic[edge].IsSelected = true;
                    }
                }
            }

            // Initialize tree node selection state
            // check on all the edges on which we created hostd sweeps
            TreeNode root = treeViewHost.Nodes[0];
            foreach(TreeNode elemNode in root.Nodes)
            {
                foreach(TreeNode edgeNode in elemNode.Nodes)
                {
                    Edge edge = edgeNode.Tag as Edge;
                    if(m_creationData.EdgesForHostedSweep.IndexOf(edge) != -1)
                    {
                        edgeNode.StateImageIndex = (int)CheckState.Checked;
                        UpdateParent(edgeNode);
                    }
                }
            }
        }

        /// <summary>
        /// Initialize text properties of this form.
        /// </summary>
        private void InitializeText()
        {
            this.Text = "Pick edges for " + m_creationData.Creator.Name;
            this.label.Text = "Select a type for " + m_creationData.Creator.Name;
            this.groupBoxEdges.Text = "All edges for " + m_creationData.Creator.Name;
        }

        /// <summary>
        /// Initialize something related to the geometry preview.
        /// </summary>
        private void InitializePreview()
        {
            m_centerMatrix = new Matrix(1, 0, 0, -1,
               (float)pictureBoxPreview.Width / 2.0f, (float)pictureBoxPreview.Height / 2.0f);
            this.KeyPreview = true;

            foreach (Autodesk.Revit.Element elem in m_creationData.Creator.ElemGeomDic.Keys)
            {
                m_activeElem = elem;
                break;
            }
        }        
        #endregion

        #region Auxiliary Methods

        /// <summary>
        /// Extract the checked edges in the whole tree to CreationData.EdgesForHostedSweep.
        /// </summary>
        private void ExtractCheckedEdgesAndSelectedSymbol()
        {
            m_creationData.EdgesForHostedSweep.Clear();
            TreeNode rootNode = treeViewHost.Nodes[0];
            foreach (TreeNode hostNode in rootNode.Nodes)
            {
                foreach (TreeNode edgeNode in hostNode.Nodes)
                {
                    
                    if (edgeNode.StateImageIndex == (int)CheckState.Checked)
                        m_creationData.EdgesForHostedSweep.Add(edgeNode.Tag as Edge);
                }
            }
            m_creationData.Symbol = comboBoxTypes.SelectedItem as Autodesk.Revit.Symbol;
        }

        /// <summary>
        /// Update tree node check status, it will impact its children and parents' status.
        /// </summary>
        /// <param name="node">Tree node to update</param>
        /// <param name="state">CheckState value</param>
        private void UpdateNodeCheckStatus(TreeNode node, CheckState state)
        {
            node.StateImageIndex = (int)state;
            if(node.Tag != null && node.Tag is Edge && m_activeElem != null)
            {
                Edge edge = node.Tag as Edge;
                Autodesk.Revit.Element elem = node.Parent.Tag as Autodesk.Revit.Element;
                ElementGeometry elemGeom = m_creationData.Creator.ElemGeomDic[elem];
                elemGeom.EdgeBindingDic[edge].IsSelected =
                    (node.StateImageIndex == (int)CheckState.Checked);
            }
            UpdateChildren(node);
            UpdateParent(node);
        }

        /// <summary>
        /// Recursively update tree children's status to match its parent status.
        /// </summary>
        /// <param name="node">Parent node whose children will be updated</param>
        private void UpdateChildren(TreeNode node)
        {
            foreach (TreeNode child in node.Nodes)
            {
                if (child.StateImageIndex != node.StateImageIndex)
                {
                    child.StateImageIndex = node.StateImageIndex;

                    if(m_activeElem != null && child.Tag != null && child.Tag is Edge)
                    {
                        Edge edge  = child.Tag as Edge;
                        Autodesk.Revit.Element elem = child.Parent.Tag as Autodesk.Revit.Element;
                        ElementGeometry elemGeom = m_creationData.Creator.ElemGeomDic[elem];
                        elemGeom.EdgeBindingDic[edge].IsSelected = 
                            (child.StateImageIndex == (int)CheckState.Checked);
                    }
                }
                UpdateChildren(child);
            }
        }

        /// <summary>
        /// Recursively update tree parent's status to match its children status.
        /// </summary>
        /// <param name="node">Child whose parents will be updated</param>
        private void UpdateParent(TreeNode node)
        {
            TreeNode parent = node.Parent;
            if (parent == null) return;
            foreach (TreeNode brother in parent.Nodes)
            {
                if (brother.StateImageIndex != node.StateImageIndex)
                {
                    parent.StateImageIndex = (int)CheckState.Indeterminate;
                    UpdateParent(parent);
                    return;
                }
            }
            parent.StateImageIndex = node.StateImageIndex;
            UpdateParent(parent);
        }

        /// <summary>
        /// Switch geometry displayed in the preview according to the tree node.
        /// </summary>
        /// <param name="node">Tree node to active</param>
        private void ActiveNode(TreeNode node)
        {
            if (node.Tag == null) return;

            if (node.Tag is Autodesk.Revit.Element)
                m_activeElem = node.Tag as Autodesk.Revit.Element;
            else if (node.Tag is Edge)
            {
                m_activeElem = node.Parent.Tag as Autodesk.Revit.Element;
            }
        }

        /// <summary>
        /// Clear Highlighted status of all highlighted edges.
        /// </summary>
        private void ClearAllHighLight()
        {
            if (m_activeElem == null) return;
            ElementGeometry elemGeom = m_creationData.Creator.ElemGeomDic[m_activeElem];

            foreach (Edge edge in m_creationData.Creator.SupportEdges[m_activeElem])
            {
                elemGeom.EdgeBindingDic[edge].IsHighLighted = false;
            }
        }

        /// <summary>
        /// Get the related tree node of an edit
        /// </summary>
        /// <param name="edge">Given edge to find its tree-node</param>
        /// <returns>Tree-node matched with the given edge</returns>
        private TreeNode GetEdgeTreeNode(Edge edge)
        {
            TreeNode result = null;
            TreeNode root = treeViewHost.Nodes[0];
            Stack<TreeNode> todo = new Stack<TreeNode>();
            todo.Push(root);
            while (todo.Count > 0)
            {
                TreeNode node = todo.Pop();
                if (node.Tag != null && node.Tag is Edge && (node.Tag as Edge) == edge)
                {
                    result = node;
                    break;
                }

                foreach (TreeNode tmpNode in node.Nodes)
                {
                    todo.Push(tmpNode);
                }
            }
            return result;
        }

        #endregion

        #region Event handles

        /// <summary>
        /// Extract checked edges and verify there are edges 
        /// checked in the treeView, if there aren't edges to be checked, complain
        /// about it with a message box, otherwise close this from.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOK_Click(object sender, EventArgs e)
        {
            ExtractCheckedEdgesAndSelectedSymbol();
            if (m_creationData.EdgesForHostedSweep.Count == 0)
            {
                MessageBox.Show("At least one edge should be selected!");
                return;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// Close this form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// This form Load event handle, all the initializations will be in here.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EdgeFetch_Load(object sender, EventArgs e)
        {
            InitializeTypes();
            InitializeTree();
            InitializeElementGeometry();
            InitializeTreeCheckStates();
            InitializeText();
            InitializePreview();
        }

        /// <summary>
        /// Suppress the default behaviors that double-click
        /// on a tree-node which contains children will make the tree-node collapse or expand.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeViewHost_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            if (m_cancelExpandOrCollapse)
                e.Cancel = true;
            m_cancelExpandOrCollapse = false;
        }

        /// <summary>
        /// Suppress the default behaviors that double-click
        /// on a tree-node which contains children will make the tree-node collapse or expand.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeViewHost_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (m_cancelExpandOrCollapse)
                e.Cancel = true;
            m_cancelExpandOrCollapse = false;
        } 

        /// <summary>
        /// Draw the geometry.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBoxPreview_Paint(object sender, PaintEventArgs e)
        {
            if (m_activeElem == null) return;

            ElementGeometry elemGeo = null;
            if (!m_creationData.Creator.ElemGeomDic.TryGetValue(m_activeElem, out elemGeo)) return;

            e.Graphics.Transform = m_centerMatrix;
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

            elemGeo.Draw(e.Graphics);
        }

        /// <summary>
        /// Initialize the track ball.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBoxPreview_MouseDown(object sender, MouseEventArgs e)
        {
            m_trackBall.OnMouseDown(pictureBoxPreview.Width, pictureBoxPreview.Height, e);
        }

        /// <summary>
        /// Rotate or zoom the displayed geometry, or highlight the edge under the mouse location.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBoxPreview_MouseMove(object sender, MouseEventArgs e)
        {
            if (m_activeElem == null) return;

            m_trackBall.OnMouseMove(e);
            if (e.Button == MouseButtons.Left)
            {
                m_creationData.Creator.ElemGeomDic[m_activeElem].Rotation *= m_trackBall.Rotation;
                pictureBoxPreview.Refresh();
            }
            else if (e.Button == MouseButtons.Right)
            {
                m_creationData.Creator.ElemGeomDic[m_activeElem].Scale *= m_trackBall.Scale;
                pictureBoxPreview.Refresh();
            }

            if (e.Button == MouseButtons.None)
            {
                ClearAllHighLight();
                pictureBoxPreview.Refresh();
                Matrix mat = (Matrix)m_centerMatrix.Clone();
                mat.Invert();
                PointF[] pts = new PointF[1] { e.Location };
                mat.TransformPoints(pts);
                ElementGeometry elemGeom = m_creationData.Creator.ElemGeomDic[m_activeElem];
                foreach (Edge edge in m_creationData.Creator.SupportEdges[m_activeElem])
                {
                    if (elemGeom.EdgeBindingDic.ContainsKey(edge))
                    {
                        if (elemGeom.EdgeBindingDic[edge].HighLight(pts[0].X, pts[0].Y))
                        {
                            pictureBoxPreview.Refresh();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Select or unselect edge
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBoxPreview_MouseClick(object sender, MouseEventArgs e)
        {
            if (m_activeElem == null || e.Button != MouseButtons.Left) return;

            ElementGeometry elemGeom = m_creationData.Creator.ElemGeomDic[m_activeElem];
            foreach (Edge edge in m_creationData.Creator.SupportEdges[m_activeElem])
            {
                if (elemGeom.EdgeBindingDic.ContainsKey(edge))
                {
                    if (elemGeom.EdgeBindingDic[edge].IsHighLighted)
                    {
                        bool isSelect = elemGeom.EdgeBindingDic[edge].IsSelected;
                        elemGeom.EdgeBindingDic[edge].IsHighLighted = false;
                        elemGeom.EdgeBindingDic[edge].IsSelected = !isSelect;
                            
                        TreeNode node = GetEdgeTreeNode(edge);

                        CheckState state = isSelect ? CheckState.Unchecked : CheckState.Checked;
                        UpdateNodeCheckStatus(node, state);
                        pictureBoxPreview.Refresh();
                        treeViewHost.Refresh();
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Highlight the edge in the preview
        /// if mouse-over an edge tree-node.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeViewHost_NodeMouseHover(object sender, TreeNodeMouseHoverEventArgs e)
        {
            TreeNode node = e.Node;
            treeViewHost.SelectedNode = e.Node;
            ClearAllHighLight();            
            ActiveNode(node);
            pictureBoxPreview.Refresh();
            if (m_activeElem == null || node.Tag == null || !(node.Tag is Edge)) return;

            ElementGeometry elemGeom = m_creationData.Creator.ElemGeomDic[m_activeElem];
            elemGeom.EdgeBindingDic[node.Tag as Edge].IsHighLighted = true;
            pictureBoxPreview.Refresh();
        }

        /// <summary>
        /// Use arrow keys to rotate the display of geometry.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EdgeFetch_KeyDown(object sender, KeyEventArgs e)
        {
            m_trackBall.OnKeyDown(e);
            switch (e.KeyCode)
            {
                case Keys.Up:
                case Keys.Down:
                case Keys.Left:
                case Keys.Right:
                    m_creationData.Creator.ElemGeomDic[m_activeElem].Rotation *= m_trackBall.Rotation;
                    pictureBoxPreview.Refresh();
                    break;
                default: break;
            }
        }

        /// <summary>
        /// Suppress the key input.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeViewHost_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
        }

        /// <summary>
        /// Select or un-select the key-node
        /// if down the left mouse button in the area of check-box or label.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeViewHost_MouseDown(object sender, MouseEventArgs e)
        {
            TreeViewHitTestInfo hitInfo = treeViewHost.HitTest(e.Location);

            if(e.Button == MouseButtons.Left && 
                (hitInfo.Location == TreeViewHitTestLocations.StateImage ||
                hitInfo.Location == TreeViewHitTestLocations.Label))
            {
                // mouse down in area of state image or label.                
                TreeNode node = hitInfo.Node;
                if(node.Nodes.Count > 0)
                    // cancel the expand or collapse of node which has children.
                    m_cancelExpandOrCollapse = true;
                
                // active the node.
                ActiveNode(node);

                // select or un-select the node.
                CheckState checkState = (CheckState)((node.StateImageIndex + 1) % 2);
                UpdateNodeCheckStatus(node, checkState);

                pictureBoxPreview.Refresh();
            }
        }
        #endregion
    }
}
