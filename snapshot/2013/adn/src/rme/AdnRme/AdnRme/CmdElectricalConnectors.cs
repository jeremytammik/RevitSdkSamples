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
  [Transaction( TransactionMode.ReadOnly )]
  class CmdElectricalConnectors : IExternalCommand
  {
    #region Test code
#if TEST_CODE
    /// <summary>
    /// Get the connected connector of one connector
    /// </summary>
    /// <param name="connector">The connector to be analyzed</param>
    /// <returns>The connected connector</returns>
    static Connector GetConnectedConnector( Connector connector )
    {
      Connector connectedConnector = null;
      ConnectorSet allRefs = connector.AllRefs;
      foreach( Connector c in allRefs )
      {
        // ignore non-EndConn connectors and connectors of the current element:
        if( ConnectorType.EndConn != c.ConnectorType
          || c.Owner.Id.Equals( connector.Owner.Id ) )
        {
          continue;
        }
        connectedConnector = c;
        break;
      }
      return connectedConnector;
    }

    static void PopulateChildren( 
      MapParentToChildren mapParentToChildren, 
      Element parent )
    {
      Connector c2 = null;
      ElementId id = parent.Id;
      FamilyInstance fi = parent as FamilyInstance;
      ElectricalSystem eq = parent as ElectricalSystem;
      Debug.Assert( null != fi || null != eq, "expected element to be family instance or electrical system" );
      ConnectorSet connectors = (null == eq)
        ? fi.MEPModel.ConnectorManager.Connectors
        : eq.ConnectorManager.Connectors;
      foreach( Connector c in connectors )
      {
        Debug.Assert( c.Owner.Id.Equals( id ), "expected connector owner to be this element" );
        //if( c.IsConnected ) // only valid for PhysicalConn
        //MEPSystem mepSystem = c.MEPSystem; // null for electrical connector
        //if( null == mepSystem ) // || !mepSystem.Id.IntegerValue.Equals( m_system.Id.IntegerValue )
        //{
        //  continue;
        //}
        //c2 = GetConnectedConnector( c );

        ConnectorSet refs = c.AllRefs;
        Debug.Assert( 1 == refs.Size, "expected one single connected connector" );
        foreach( Connector tmp in refs )
        {
          c2 = tmp;
        }
        if( null != c2 )
        {
          Element e = c2.Owner;
          Debug.Assert( null != e, "expected valid connector owner" );
          Debug.Assert( e is FamilyInstance || e is ElectricalSystem, "expected electrical connector owner to be family instance or electrical equipment" );
          mapParentToChildren.Add( id, e );
        }
      }
    }
#endif // TEST_CODE
    #endregion // Test code

    public Result Execute(
      ExternalCommandData commandData,
      ref String message,
      ElementSet elements )
    {
      UIApplication app = commandData.Application;
      Document doc = app.ActiveUIDocument.Document;
      //
      // retrieve electrical equipment:
      //
      List<Element> equipment = Util.GetElectricalEquipment( doc );
      int n = equipment.Count;
      Debug.WriteLine( string.Format( 
        "Retrieved {0} electrical equipment instance{1}{2}",
        n, Util.PluralSuffix( n ), Util.DotOrColon( n ) ) );
      //
      // determine which equipment has parents;
      // the remaining ones are root nodes:
      //
      Dictionary<ElementId, ElementId> equipmentParents 
        = new Dictionary<ElementId, ElementId>();

      foreach( FamilyInstance fi in equipment )
      {
        foreach( Connector c in fi.MEPModel.ConnectorManager.Connectors )
        {
          ConnectorSet refs = c.AllRefs;
          foreach( Connector c2 in refs )
          {
            Debug.Assert( null != c2.Owner, 
              "expected valid connector owner" );

            Debug.Assert( c2.Owner is ElectricalSystem, 
              "expected panel element to be electrical system" );

            ElectricalSystem eq = c2.Owner as ElectricalSystem;
            foreach( Element e2 in eq.Elements )
            {
              Debug.Assert( e2 is FamilyInstance, 
                "expected electrical system element to be family instance" );

              if( !e2.Id.Equals( fi.Id ) )
              {
                if( equipment.Exists( 
                  delegate( Element e ) 
                  { return e.Id.Equals( e2.Id ); } ) )
                {
                  equipmentParents[e2.Id] = eq.Id;
                }
              }
            }
          }
        }
      }

      //n = equipment.RemoveAll( delegate( Element e ) { return subequipment.Exists( delegate( Element e2 ) { return e2.Id.Equals( e.Id ); } ); } );

      //
      // populate parent to children mapping:
      //
      ElementId nullId = ElementId.InvalidElementId;
      MapParentToChildren mapParentToChildren = new MapParentToChildren();

      foreach( FamilyInstance fi in equipment )
      {
        //ElementId parentId = equipmentParents.ContainsKey( fi.Id ) ? equipmentParents[fi.Id] : nullId;
        //mapParentToChildren.Add( parentId, fi );

        //
        // handle root nodes;
        // non-roots are handled below a children:
        //
        if( !equipmentParents.ContainsKey( fi.Id ) )
        {
          mapParentToChildren.Add( nullId, fi );
        }
        
        foreach( Connector c in fi.MEPModel.ConnectorManager.Connectors )
        {
          ConnectorSet refs = c.AllRefs;
          foreach( Connector c2 in refs )
          {
            Debug.Assert( null != c2.Owner, 
              "expected valid connector owner" );

            Debug.Assert( c2.Owner is ElectricalSystem, 
              "expected panel element to be electrical system" );

            ElectricalSystem eq = c2.Owner as ElectricalSystem;
            mapParentToChildren.Add( fi.Id, eq );
            foreach( Element e2 in eq.Elements )
            {
              Debug.Assert( e2 is FamilyInstance, 
                "expected electrical system element to be family instance" );

              if( !e2.Id.Equals( fi.Id ) )
              {
                mapParentToChildren.Add( eq.Id, e2 );
              }
            }
          }
        }
      }

      #region Test code
#if TEST_CODE
      //
      // retrieve electrical systems:
      //
      List<Element> systems = new List<Element>();
      doc.get_Elements( typeof( ElectricalSystem ), systems );
      n = systems.Count;
      Debug.WriteLine( string.Format( "Retrieved {0} electrical system{1}{2}",
        n, Util.PluralSuffix( n ), Util.DotOrColon( n ) ) );
      //
      // iterate over all electrical systems and recursively add 
      // connected ecomponents starting from the system base equipment:
      //
      /*
      Dictionary<string, ElectricalSystem> systemDict = new Dictionary<string, ElectricalSystem>( n );
      foreach( ElectricalSystem s in systems )
      {
        systemDict[s.Name] = s;
      }
      List<string> keys = new List<string>( systemDict.Keys );
      keys.Sort();
      foreach( string key in keys )
      {
        FamilyInstance b = systemDict[key].BaseEquipment;
        if( null != b )
        {
          mapParentToChildren.Add( nullId, b ); // root node
          PopulateChildren( mapParentToChildren, b );
        }
      }
      foreach( ElectricalSystem s in systems )
      {
        FamilyInstance b = s.BaseEquipment;
        if( null != b )
        {
          mapParentToChildren.Add( nullId, b ); // root node
          PopulateChildren( mapParentToChildren, b );
        }
      }
#endif // TEST_CODE
      #endregion // Test code

      //
      // get the electrical equipment category id:
      //
      Categories categories = doc.Settings.Categories;
      ElementId electricalEquipmentCategoryId = categories.get_Item( 
        BuiltInCategory.OST_ElectricalEquipment ).Id;

      //
      // display hierarchical structure in tree view:
      //
      CmdInspectElectricalForm2 dialog 
        = new CmdInspectElectricalForm2( 
          mapParentToChildren, electricalEquipmentCategoryId, equipment );

      dialog.Show();
      return Result.Failed;
    }
  }
}
