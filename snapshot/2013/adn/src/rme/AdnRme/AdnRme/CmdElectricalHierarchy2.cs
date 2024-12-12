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
using TreeNode = System.Windows.Forms.TreeNode;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.UI;
#endregion // Namespaces

namespace AdnRme
{
  #region MapParentToChildren
  /// <summary>
  /// Map tree node parents to their children.
  /// 
  /// The various possible combinations of parent and child 
  /// element types include:
  /// 
  ///   null --> root panels
  ///   panel --> systems
  ///   system --> circuit elements, further panels, ...
  /// 
  /// This map is populated as follows: as we iterate over all the relevant
  /// electrical equipment, systems, and circuit objects. Each object
  /// attempts to identify its parent, i.e. its destination parent node
  /// in the tree and registers itself in the parent's list of elements.
  /// We attempted to use the parent element itself as a key, but that 
  /// does not work, so we reverted to using the parent element id as 
  /// a key instead.
  /// </summary>
  public class MapParentToChildren : Dictionary<ElementId, List<Element>>
  {
    /// <summary>
    /// Add a new parent, or a new child to a parent, ensuring that a new container 
    /// list is created if this parent has not yet been registered.
    /// </summary>
    /// <param name="parentId">Parent element id</param>
    /// <param name="child">New child element</param>
    public void Add( ElementId parentId, Element child )
    {
      if( !this.ContainsKey( parentId ) )
      {
        this.Add( parentId, new List<Element>() );
      }
      if( null != child )
      {
        this[parentId].Add( child );
      }
    }
  }

  #region Using Element as Key
  /// <summary>
  /// I tried to use the Element instance itself as a key into this map,
  /// but apparently the comparison does not always work correctly, so I 
  /// had to revert to using the element id instead.
  /// </summary>
  public class MapParentToChildren2 : Dictionary<Element, List<Element>>
  {
    Element _root;

    public MapParentToChildren2( Element root )
    {
      _root = root;
    }

    public Element Root
    {
      get
      {
        return _root;
      }
    }

    public void Add( Element parent, Element child )
    {
      if( !this.ContainsKey( parent ) )
      {
        this.Add( parent, new List<Element>() );
      }
      this[parent].Add( child );
    }
  }
  #endregion // Using Element as Key
  #endregion // MapParentToChildren

  #region CmdElectricalHierarchy2
  /// <summary>
  /// Inspect the electrical system.
  /// 
  /// Analyse the electrical system connection graph and display it in 
  /// tree view in a a modeless dialogue, i.e. it remains visible after 
  /// the command has completed.
  /// 
  /// This presents a more direct approach than the first implementation 
  /// in CmdElectricalSystemBrowser. In this implementation, we directly 
  /// build the tree hierarchy in a MapParentToChildren dictionary from 
  /// the element relationships. Also, we use element ids wherever 
  /// possible, instead of key strings of the form 
  /// "panel name : circuit or system name".
  /// 
  /// Note: this sample was written before the introduction of the 
  /// Connector and ConnectionManager classes. The complex determination 
  /// of the connection hierarchy based on parameter values performed 
  /// here can be much simplified using the connectors, as demonstrated 
  /// by CmdElectricalConnectors.
  /// </summary>
  [Transaction( TransactionMode.ReadOnly )]
  class CmdElectricalHierarchy2 : IExternalCommand
  {
    public Result Execute(
      ExternalCommandData commandData,
      ref String message,
      ElementSet elements )
    {
      try
      {
        //
        // dictionary defining tree view info displayed in modeless
        // dialogue mapping parent node to all its circuit elements:
        // null --> root panels
        // panel  --> systems
        // system --> circuit elements, panels, ...
        // 
        MapParentToChildren mapParentToChildren = new MapParentToChildren();
        ElementId electricalEquipmentCategoryId = ElementId.InvalidElementId;
        List<Element> equipment;
        {
          //
          // run the analysis in its own scope, so the wait cursor
          // disappears before we display the modeless dialogue:
          //
          WaitCursor waitCursor = new WaitCursor();
          UIApplication app = commandData.Application;
          Document doc = app.ActiveUIDocument.Document;
          ElementId nullId = ElementId.InvalidElementId;
          //
          // retrieve electrical equipment instances:
          //
          equipment = Util.GetElectricalEquipment( doc );
          int n = equipment.Count;
          Debug.WriteLine( string.Format( "Retrieved {0} electrical equipment instance{1}{2}",
            n, Util.PluralSuffix( n ), Util.DotOrColon( n ) ) );
          Dictionary<string, FamilyInstance> mapPanel = new Dictionary<string, FamilyInstance>();
          foreach( FamilyInstance e in equipment )
          {
            //
            // ensure that every panel shows up in the list, 
            // even if it does not have children:
            //
            mapParentToChildren.Add( e.Id, null );
            mapPanel[e.Name] = e;
            MEPModel mepModel = e.MEPModel;
            ElectricalSystemSet systems2 = mepModel.ElectricalSystems;
            string panelAndSystem = string.Empty;
            if( null == systems2 )
            {
              panelAndSystem = CmdElectricalSystemBrowser.Unassigned; // this is a root node
            }
            else
            {
              Debug.Assert( 1 == systems2.Size, "expected equipment to belong to one single panel and system" );
              foreach( ElectricalSystem system in systems2 )
              {
                if( 0 < panelAndSystem.Length )
                {
                  panelAndSystem += ", ";
                }
                panelAndSystem += system.PanelName + ":" + system.Name + ":" + system.Id.IntegerValue.ToString();
              }
            }
            Debug.WriteLine( "  " + Util.ElementDescriptionAndId( e ) + " " + panelAndSystem );
          }
          //
          // retrieve electrical systems:
          // these are also returned by Util.GetCircuitElements(), by the way, 
          // since they have the parameters RBS_ELEC_CIRCUIT_PANEL_PARAM and
          // RBS_ELEC_CIRCUIT_NUMBER that we use to identify those.
          //
          FilteredElementCollector c = new FilteredElementCollector( doc );
          IList<Element> systems = c.OfClass( typeof( ElectricalSystem ) ).ToElements();
          n = systems.Count;
          Debug.WriteLine( string.Format( "Retrieved {0} electrical system{1}{2}",
            n, Util.PluralSuffix( n ), Util.DotOrColon( n ) ) );
          foreach( ElectricalSystem system in systems )
          {
            string panelName = system.PanelName;
            if( 0 == panelName.Length )
            {
              panelName = CmdElectricalSystemBrowser.Unassigned; // will not appear in tree
            }
            else
            {
              //
              // todo: is there a more direct way to identify 
              // what panel a system belongs to? this seems error
              // prone ... what if a panel name occurs multiple times?
              // how do we identify which one to use?
              //
              FamilyInstance panel = mapPanel[panelName];
              mapParentToChildren.Add( panel.Id, system );
            }
            string panelAndSystem = panelName + ":" + system.Name + ":" + system.Id.IntegerValue.ToString();
            Debug.WriteLine( "  " + Util.ElementDescriptionAndId( system ) + " " + panelAndSystem );
            Debug.Assert( system.ConnectorManager.Owner.Id.Equals( system.Id ), "expected electrical system's connector manager owner to be system itself" );
          }
          //
          // now we have the equipment and systems, 
          // we can build the non-leaf levels of the tree:
          //
          foreach( FamilyInstance e in equipment )
          {
            MEPModel mepModel = e.MEPModel;
            ElectricalSystemSet systems2 = mepModel.ElectricalSystems;
            if( null == systems2 )
            {
              mapParentToChildren.Add( nullId, e ); // root node
            }
            else
            {
              Debug.Assert( 1 == systems2.Size, "expected equipment to belong to one single panel and system" );
              foreach( ElectricalSystem system in systems2 )
              {
                mapParentToChildren.Add( system.Id, e );
              }
            }
          }
          //
          // list all circuit elements:
          //
          BuiltInParameter bipPanel = BuiltInParameter.RBS_ELEC_CIRCUIT_PANEL_PARAM;
          BuiltInParameter bipCircuit = BuiltInParameter.RBS_ELEC_CIRCUIT_NUMBER;
          IList<Element> circuitElements = Util.GetCircuitElements( doc );
          n = circuitElements.Count;
          Debug.WriteLine( string.Format( "Retrieved {0} circuit element{1}...",
            n, Util.PluralSuffix( n ) ) );
          n = 0;
          foreach( Element e in circuitElements )
          {
            if( e is ElectricalSystem )
            {
              ++n;
            }
            else
            {
              string circuitName = e.get_Parameter( bipCircuit ).AsString();
              string panelName = e.get_Parameter( bipPanel ).AsString();
              string key = panelName + ":" + circuitName;
              string panelAndSystem = string.Empty;
              FamilyInstance inst = e as FamilyInstance;
              Debug.Assert( null != inst, "expected all circuit elements to be family instances" );
              MEPModel mepModel = inst.MEPModel;
              ElectricalSystemSet systems2 = mepModel.ElectricalSystems;
              Debug.Assert( null != systems2, "expected circuit element to belong to an electrical system" );

              // this fails in "2341_MEP - 2009 Central.rvt", says martin:
              //
              // a circuit element can belong to several systems ... imagine 
              // a piece of telephone equipment which hooks up to a phone line 
              // and also requires power ... so i removed this assertion:
              //
              //Debug.Assert( 1 == systems2.Size, "expected circuit element to belong to one single system" );

              foreach( ElectricalSystem system in systems2 )
              {
                if( 0 < panelAndSystem.Length )
                {
                  panelAndSystem += ", ";
                }
                panelAndSystem += system.PanelName + ":" + system.Name + ":" + system.Id.IntegerValue.ToString();
                Debug.Assert( system.PanelName == panelName, "expected same panel name in parameter and electrical system" );
                // this fails in "2341_MEP - 2009 Central.rvt", says martin:
                //Debug.Assert( system.Name == circuitName, "expected same name in circuit parameter and system" );
                mapParentToChildren.Add( system.Id, e );
              }
              Debug.WriteLine( string.Format( "  {0} panel:circuit {1}", Util.ElementDescriptionAndId( e ), panelAndSystem ) );
            }
          }
          Debug.WriteLine( string.Format( "{0} circuit element{1} were the electrical systems.",
            n, Util.PluralSuffix( n ) ) );
          //
          // get the electrical equipment category id:
          //
          Categories categories = doc.Settings.Categories;
          electricalEquipmentCategoryId = categories.get_Item( BuiltInCategory.OST_ElectricalEquipment ).Id;
        }
        //
        // we have assembled the entire required tree view structure, so let us display it:
        //
        CmdInspectElectricalForm2 dialog = new CmdInspectElectricalForm2( mapParentToChildren, electricalEquipmentCategoryId, equipment );
        dialog.Show();
        return Result.Succeeded;
      }
      catch( Exception ex )
      {
        message = ex.Message;
        return Result.Failed;
      }
    }
  }
  #endregion // CmdElectricalHierarchy2
}
