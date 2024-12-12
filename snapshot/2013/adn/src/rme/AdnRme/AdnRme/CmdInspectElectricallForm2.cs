#region Header
// Revit MEP API sample application
//
// Copyright (C) 2007-2013 by Jeremy Tammik, Autodesk, Inc.
//
// Permission to use, copy, modify, and distribute this software
// for any purpose and without fee is hereby granted, provided
// that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  
// AUTODESK, INC. DOES NOT WARRANT THAT THE OPERATION OF THE 
// PROGRAM WILL BE UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject
// to restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
#endregion // Header

#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using WinForm = System.Windows.Forms.Form;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.UI;
#endregion // Namespaces

namespace AdnRme
{
  /// <summary>
  /// Form displaying electrical system elements in a tree view
  /// according to their connection hierarchy, and a list view
  /// of electrical equipment providing immediate access to jump 
  /// directly into the appropriate location in the tree view.
  /// </summary>
  public partial class CmdInspectElectricalForm2 : WinForm
  {
    ElementId _electricalEquipmentCategoryId;
    Dictionary<ElementId, int> _done = new Dictionary<ElementId, int>();
    ElectricalElementComparer _comparer;

    static public string ElectricalElementText( Element e, ElementId electricalEquipmentCategoryId )
    {
      bool isCircuit = e is ElectricalSystem;
      bool isEquipment = e.Category.Id.Equals( electricalEquipmentCategoryId );
      return ( isEquipment || isCircuit ) ? e.Name : Util.BrowserDescription( e );
    }

    #region PopulateFromMapParentToChildren
    void PopulateFromMapParentToChildren(
      TreeNodeCollection tnc, 
      ElementId parentId, 
      MapParentToChildren map )
    {
      if( map.ContainsKey( parentId ) )
      {
        List<Element> children = map[parentId];
        children.Sort( _comparer );
        string key, text;
        bool isEquipment; // isCircuit
        foreach( Element e in children )
        {
          if( !_done.ContainsKey( e.Id ) )
          {
            _done[e.Id] = 1;
            //isCircuit = e is ElectricalSystem;
            isEquipment = e.Category.Id.Equals( _electricalEquipmentCategoryId );
            key = Util.ElementDescriptionAndId( e );
            //text = ( isEquipment || isCircuit ) ? e.Name : Util.BrowserDescription( e );
            text = ElectricalElementText( e, _electricalEquipmentCategoryId );
            TreeNode tn = tnc.Add( key, text );
            CmdInspectElectricalForm.AddLoadNodes( tn, e );
            if( map.ContainsKey( e.Id ) )
            {
              //
              // highlight circuit e.g. electrical system node that has 
              // electrical equipment connected to it; also avoid setting
              // it to bold multiple times over:
              //
              if( isEquipment )
              {
                TreeNode p = tn.Parent;
                listBox1.Items.Add( new PanelTreeNodeHelper( e, tn ) );
                if( null != p && ( null == p.NodeFont || !p.NodeFont.Bold ) )
                {
                  p.NodeFont = new System.Drawing.Font( this.Font, FontStyle.Bold );
                }
              }
              PopulateFromMapParentToChildren( tn.Nodes, e.Id, map );
            }
          }
        }
      }
    }
    #endregion // PopulateFromMapParentToChildren

    #region Constructor
    /// <summary>
    /// Constructor used to populate from a dictionary mapping element ids 
    /// to a list of child elements.
    /// </summary>
    public CmdInspectElectricalForm2(
      MapParentToChildren map,
      ElementId electricalEquipmentCategoryId,
      IList<Element> electricalEquipment )
    {
      _electricalEquipmentCategoryId = electricalEquipmentCategoryId;
      _comparer = new ElectricalElementComparer( electricalEquipmentCategoryId );
      InitializeComponent();
      tv.BeginUpdate();
      PopulateFromMapParentToChildren( tv.Nodes, ElementId.InvalidElementId, map );
      tv.EndUpdate();
      listBox1.SelectedIndex = 0;
    }
    #endregion // Constructor

    #region Selected equipment changed
    private void listBox1_SelectedIndexChanged( object sender, EventArgs e )
    {
      PanelTreeNodeHelper ptnh = listBox1.SelectedItem as PanelTreeNodeHelper;
      if( null != ptnh )
      {
        TreeNode tn = ptnh.TreeNode;
        tv.CollapseAll();
        tv.SelectedNode = tn;
        tv.Select();
        tn.Expand();
      }
    }
    #endregion // Selected equipment changed
  }

  /// <summary>
  /// Helper class for string comparison of child elements for sorting the tree node order.
  /// </summary>
  class ElectricalElementComparer : IComparer<Element>
  {
    ElementId _electricalEquipmentCategoryId;

    public ElectricalElementComparer( ElementId electricalEquipmentCategoryId )
    {
      _electricalEquipmentCategoryId = electricalEquipmentCategoryId;
    }

    public int Compare( Element x, Element y )
    {
      string sx = CmdInspectElectricalForm2.ElectricalElementText( x, _electricalEquipmentCategoryId );
      string sy = CmdInspectElectricalForm2.ElectricalElementText( y, _electricalEquipmentCategoryId );
      return sx.CompareTo( sy );
    }
  }
}
