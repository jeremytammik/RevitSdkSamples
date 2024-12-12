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
using Autodesk.Revit.UI;
#endregion // Namespaces

namespace AdnRme
{
  public partial class CmdInspectElectricalForm : WinForm
  {
    ElementId _electricalEquipmentCategoryId;

    #region Loads
    /// <summary>
    /// Store the loads associated with a panel or circuit:
    /// 
    /// Provide at the circuit and panel level the total connected load by load type… 
    /// RME provides four load types: HVAC, lighting, receptacle, and other.
    /// The project browser only shows a total. If we could subdivide that 
    /// information based on the category, that would be interesting.
    /// 
    /// RBS_ELEC_PANEL_TOTALLOAD_HVAC_PARAM	HVAC Total Connected
    /// RBS_ELEC_PANEL_TOTALLOAD_LIGHT_PARAM	Lighting Total Connected
    /// RBS_ELEC_PANEL_TOTALLOAD_POWER_PARAM	Power Total Connected
    /// RBS_ELEC_PANEL_TOTALLOAD_OTHER_PARAM	Other Total Connected
    /// RBS_ELEC_PANEL_TOTALLOAD_PARAM	Total Connected
    /// </summary>
    class Loads
    {
      public string Hvac;
      public string Light;
      public string Power;
      public string Other;
      public string Total;

      static string GetStringFromParam( Element e, BuiltInParameter bip )
      {
        Parameter p = e.get_Parameter( bip );
        return ( null == p ) ? null
          : ( StorageType.String == p.StorageType ) ? p.AsString() 
          : p.AsValueString();
      }

      public Loads( Element e )
      {
        Hvac = GetStringFromParam( e, BuiltInParameter.RBS_ELEC_PANEL_TOTALLOAD_HVAC_PARAM );
        Light = GetStringFromParam( e, BuiltInParameter.RBS_ELEC_PANEL_TOTALLOAD_LIGHT_PARAM );
        Power = GetStringFromParam( e, BuiltInParameter.RBS_ELEC_PANEL_TOTALLOAD_POWER_PARAM );
        Other = GetStringFromParam( e, BuiltInParameter.RBS_ELEC_PANEL_TOTALLOAD_OTHER_PARAM );
        Total = GetStringFromParam( e, BuiltInParameter.RBS_ELEC_PANEL_TOTALLOAD_PARAM );
      }

      public bool IsValid
      {
        get
        {
          return null != Hvac;
        }
      }

      public override string ToString()
      {
        return string.Format( "HVAC {0} Light {1} Power {2} Other {3} Total {4}",
          Hvac, Light, Power, Other, Total );
      }

      public void AddNodes( TreeNode tn )
      {
        TreeNode loadNode = tn.Nodes.Add( "Loads" );
        loadNode.Nodes.Add( "HVAC " + Hvac );
        loadNode.Nodes.Add( "Light " + Light );
        loadNode.Nodes.Add( "Power " + Power );
        loadNode.Nodes.Add( "Other " + Other );
        loadNode.Nodes.Add( "Total " + Total );
      }
    }

    public static void AddLoadNodes( TreeNode tn, Element e )
    {
      Loads loads = new Loads( e );
      if( loads.IsValid )
      {
        loads.AddNodes( tn );
      }
    }
    #endregion // Loads

    #region Panel and System Accessors
    static string[] GetPanelAndSystemPair( string panelAndSystem )
    {
      string[] a = panelAndSystem.Split( ':' );
      Debug.Assert( 2 == a.GetLength( 0 ), "expected panelAndSystem string in the form panel:system" );
      return a;
    }

    static string GetPanelName( string panelAndSystem )
    {
      return GetPanelAndSystemPair( panelAndSystem )[0];
    }

    static string GetSystemName( string panelAndSystem )
    {
      return GetPanelAndSystemPair( panelAndSystem )[1];
    }
    #endregion // Panel and System Accessors

    #region PopulateLikeSystemBrowser
    void PopulateLikeSystemBrowser( IDictionary<string, List<Element>> map )
    {
      TreeNode tn;
      List<string> keys = new List<string>( map.Keys );
      keys.Sort( new CmdElectricalSystemBrowser.PanelSystemComparer() );
      foreach( string panelAndSystem in keys )
      {
        string[] pair = GetPanelAndSystemPair( panelAndSystem );
        string panelName = pair[0];
        string systemName = pair[1];
        tn = tv.Nodes.ContainsKey( panelName )
          ? tv.Nodes.Find( panelName, false )[0]
          : tv.Nodes.Add( panelName, panelName );
        tn = tn.Nodes.ContainsKey( systemName )
          ? tn.Nodes.Find( systemName, false )[0]
          : tn.Nodes.Add( systemName, systemName );
        List<Element> elements = map[panelAndSystem];
        List<string> a = new List<string>( elements.Count );
        foreach( Element e in elements )
        {
          a.Add( Util.BrowserDescription( e ) );
        }
        a.Sort();
        foreach( string s in a )
        {
          tn.Nodes.Add( s );
        }
      }
    }
    #endregion // PopulateLikeSystemBrowser

    #region PopulateFullHierarchy
    static bool PanelNameMatches( string panelName, string panelAndSystem )
    {
      return GetPanelName( panelAndSystem ).Equals( panelName );
    }

    bool FindUnassigned( string panelAndSystem )
    {
      return PanelNameMatches( CmdElectricalSystemBrowser.Unassigned, panelAndSystem );
    }

    bool FindPanelBasedOnName( string panelAndSystem )
    {
      return PanelNameMatches( _panelNameToFind, panelAndSystem );
    }

    void PopulateSystemNodes( TreeNode tn, string panelAndSystem )
    {
      List<Element> elements = _map[panelAndSystem];
      foreach( Element e in elements )
      {
        //
        // for language independence, never compare categories by name.
        // in Revit 2009, you cannot compare categories themselves reliably 
        // using category.Equals(), so you should compare the category 
        // element id instead:
        //
        if( e.Category.Id.Equals( _electricalEquipmentCategoryId ) )
        {
          tn.NodeFont = new System.Drawing.Font( this.Font, FontStyle.Bold );
          AddLoadNodes( tn, _map[panelAndSystem][0] );
          PopulatePanelSystemNodes( tn.Nodes, e.Name, e );
        }
        else
        {
          TreeNode elementNode = tn.Nodes.Add( Util.BrowserDescription( e ) );
        }
      }
    }

    private void PopulatePanelSystemNodes( TreeNodeCollection tnc, string panelName, Element e )
    {
      TreeNode tn = tnc.Add( panelName, panelName );
      AddLoadNodes( tn, e );
      _panelNameToFind = panelName;
      List<string> systemsForPanel = _keys.FindAll( FindPanelBasedOnName );
      systemsForPanel.Sort( new CmdElectricalSystemBrowser.PanelSystemComparer() );
      foreach( string panelAndSystem in systemsForPanel )
      {
        TreeNode systemNode = tn.Nodes.Add( GetSystemName( panelAndSystem ) );
        PopulateSystemNodes( systemNode, panelAndSystem );
      }
    }

    IDictionary<string, List<Element>> _map;
    string _panelNameToFind;
    List<string> _keys;

    void PopulateFullHierarchy( IDictionary<string, List<Element>> map )
    {
      _map = map;
      _keys = new List<string>( map.Keys );
      List<string> rootPanels = _keys.FindAll( FindUnassigned );
      foreach( string rootPanel in rootPanels )
      {
        List<Element> es = _map[rootPanel];
        Debug.Assert( 1 == es.Count, "expected only one element in root panel element list" );
        PopulatePanelSystemNodes( tv.Nodes, GetSystemName( rootPanel ), es[0] );
      }
    }
    #endregion // PopulateFullHierarchy

    #region Constructor
    /// <summary>
    /// Constructor used to populate from dictionary mapping key in the form
    /// "panel name : circuit number" to a list of child elements.
    /// </summary>
    public CmdInspectElectricalForm(
      IDictionary<string, List<Element>> map,
      ElementId electricalEquipmentCategoryId,
      bool populateFullHierarchy )
    {
      _electricalEquipmentCategoryId = electricalEquipmentCategoryId;
      InitializeComponent();
      tv.BeginUpdate();

      if( populateFullHierarchy )
      {
        //
        // to dispaly the full hierarchical tree structure:
        //
        // I would like to see this tree view more reflective of the 
        // distribution system, e.g. panel MDP is not connected to 
        // anything upstream, so this should be the root node.
        // Further, MDP circuit 1,3,5 has panel H-2 connected to it, 
        // so the entire tree of H-2 should reside as a child of the 
        // 1,3,5 node of MDP.  
        // Likewise, H-2 circuit 1,3,5 has transformer T2 connected, 
        // so this should be shown as a child of this node, etc…
        //
        PopulateFullHierarchy( map );
      }
      else
      {

        //
        // we could implement sorting on the tree view itself, 
        // but since we already worked out sorting algorithms
        // for our dictionaries and maps in the main command
        // for printing to the debug output window, we may as 
        // well reuse those and leave the tree view unsorted:
        //
        PopulateLikeSystemBrowser( map );
      }
      tv.EndUpdate();
    }
    #endregion // Constructor

    #region Obsolete CmdElectricalHierarchy2
#if _OBSOLETE
    #region PopulateFromMapParentToChildren
    void PopulateFromMapParentToChildren( TreeNodeCollection tnc, ElementId parentId, MapParentToChildren map )
    {
      List<Element> children = map[parentId];
      string key, text;
      bool isEquipment, isCircuit;
      foreach( Element e in children )
      {
        isCircuit = e is ElectricalSystem;
        isEquipment = e.Category.Id.Equals( _electricalEquipmentCategoryId );
        key = Util.ElementDescriptionAndId( e );
        text = (isEquipment || isCircuit) ? e.Name : Util.BrowserDescription( e );
        TreeNode tn = tnc.Add( key, text );
        AddLoadNodes( tn, e );
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
            if( null != p && (null == p.NodeFont || !p.NodeFont.Bold ) )
            {
              p.NodeFont = new System.Drawing.Font( this.Font, FontStyle.Bold );
            }
          }
          PopulateFromMapParentToChildren( tn.Nodes, e.Id, map );
        }
      }
    }
    #endregion // PopulateFromMapParentToChildren

    #region Constructor
    /// <summary>
    /// Constructor used to populate from a dictionary mapping element ids 
    /// to a list of child elements.
    /// </summary>
    public CmdInspectElectricalForm( 
      MapParentToChildren map,
      ElementId electricalEquipmentCategoryId )
    {
      Debug.Assert( false, "this is no longer used" );
      _electricalEquipmentCategoryId = electricalEquipmentCategoryId;
      InitializeComponent();
      tv.BeginUpdate();
      PopulateFromMapParentToChildren( tv.Nodes, new ElementId(), map );
      tv.EndUpdate();
    }
    #endregion // Constructor
#endif // _OBSOLETE
    #endregion // Obsolete CmdElectricalHierarchy2

    #region Aborted attempt at using tooltip to display loads
#if USING_TOOLTIP
    int _currentNodeIndex;

    private void tv_MouseMove( object sender, MouseEventArgs e )
    {
      TreeNode tn = tv.GetNodeAt( e.X, e.Y );
      if( null != tn )
      {
        if( tn.Index != _currentNodeIndex )
        {
          _currentNodeIndex = tn.Index;
          object tag = tn.Tag;
          toolTip1.SetToolTip( tv, null == tag ? "jt" : tag.ToString() );
        }
      }
    }
#endif // USING_TOOLTIP
    #endregion // Aborted attempt at using tooltip to display loads
  }
}
