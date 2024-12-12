#region Header
//
// (C) Copyright 2011 by Autodesk, Inc.
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
#endregion // Header

#region Namespaces
using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.DB.Mechanical;
#endregion // Namespaces

namespace MepPlaceholders
{
  /// <summary>
  /// Demonstrate NewMechanicalSystem and 
  /// Duct.CreatePlaceholder methods in conjunction 
  /// with NewElbowFitting and NewCrossFitting.
  /// </summary>
  [Transaction( TransactionMode.Manual )]
  public class CreatePlaceholders : IExternalCommand
  {
    static string RealString( double a )
    {
      return a.ToString( "0.##" );
    }

    static string PointString( XYZ p )
    {
      return string.Format( "({0},{1},{2})",
        RealString( p.X ), RealString( p.Y ),
        RealString( p.Z ) );
    }

    /// <summary>
    /// Helper class to encapsulate an equipment element
    /// with its supply air connector and its location
    /// and direction.
    /// </summary>
    class EquipmentElement
    {
      public FamilyInstance FamilyInstance;
      public Connector SupplyAirConnector;
      public XYZ ConnectionPoint;
      public XYZ ConnectionDirection;

      public EquipmentElement(
        FamilyInstance familyInstance,
        Connector supplyAirConnector,
        XYZ connectionPoint,
        XYZ connectionDirection )
      {
        FamilyInstance = familyInstance;
        SupplyAirConnector = supplyAirConnector;
        ConnectionPoint = connectionPoint;
        ConnectionDirection = connectionDirection;
      }
    }

    /// <summary>
    /// Return the duct connector at the given location,
    /// and the other connector as well.
    /// </summary>
    /// <param name="duct">A duct, which always has two connections</param>
    /// <param name="location">The location of one of the two duct connectors</param>
    /// <param name="otherConnector">The other connector</param>
    /// <returns>The connector at the given location</returns>
    static Connector GetDuctConnectorAt(
      Duct duct,
      XYZ location,
      out Connector otherConnector )
    {
      otherConnector = null;

      Connector targetConnector = null;

      ConnectorManager cm = duct.ConnectorManager;

      foreach( Connector c in cm.Connectors )
      {
        if( c.Origin.IsAlmostEqualTo( location ) )
        {
          targetConnector = c;
        }
        else
        {
          otherConnector = c;
        }
      }
      return targetConnector;
    }

    static void ExecuteMepPlaceholders( Document doc )
    {
      // Find duct type to use

      Func<Element, bool> isRectangularRadiusDuctType 
        = dt => dt.Name.Contains( "Radius Elbows / Tees" );

      Element ductType 
        = new FilteredElementCollector( doc )
          .OfClass( typeof( DuctType ) )
          .First<Element>( isRectangularRadiusDuctType );

      // Find Level 1 to place ductwork

      Func<Level, bool> isLevel1 
        = level => level.Name.Equals( "Level 1" );

      Level level1 = new FilteredElementCollector( doc )
        .OfClass( typeof( Level ) )
        .Cast<Level>()
        .First<Level>( isLevel1 );

      // Find all mechanical equipment elements

      List<BuiltInCategory> cats 
        = new List<BuiltInCategory>( 2 );

      cats.Add( BuiltInCategory.OST_MechanicalEquipment );
      cats.Add( BuiltInCategory.OST_DuctTerminal );

      FilteredElementCollector equipment 
        = new FilteredElementCollector( doc )
          .OfClass( typeof( FamilyInstance ) )
          .WherePasses( new ElementMulticategoryFilter( 
            cats ) );

      List<EquipmentElement> equipmentElements 
        = new List<EquipmentElement>();

      foreach( FamilyInstance fi in equipment )
      {
        // find connectors

        ConnectorManager cm 
          = fi.MEPModel.ConnectorManager;

        ConnectorSet cs = cm.Connectors;

        foreach( Connector c in cs )
        {
          if( Domain.DomainHvac == c.Domain 
            && DuctSystemType.SupplyAir == c.DuctSystemType )
          {
            // connector point and direction

            XYZ p = c.Origin; 
            XYZ v = c.CoordinateSystem.BasisZ;

            equipmentElements.Add( 
              new EquipmentElement( fi, c, p, v ) );
          }
        }
      }

      // Determine a waypoint for the cross fitting
      // from the XY centre point of all elements
      // bounding boxes and a given height eleven 
      // feet over the floor:

      double[] min = new double[] { 
        double.PositiveInfinity, 
        double.PositiveInfinity };

      double[] max = new double[] { 
        double.NegativeInfinity, 
        double.NegativeInfinity };

      foreach( EquipmentElement e in equipmentElements )
      {
        BoundingBoxXYZ box 
          = e.FamilyInstance.get_BoundingBox( null );

        for( int i = 0; i < 2; ++i )
        {
          min[i] = box.Min[i] < min[i] ? box.Min[i] : min[i];
          max[i] = box.Max[i] > max[i] ? box.Max[i] : max[i];
        }
      }

      double verticalLocation = level1.Elevation + 11.0;

      XYZ wayPoint = new XYZ( 
        ( min[0] + max[0] ) / 2, 
        ( min[1] + max[1] ) / 2, 
        verticalLocation );

      Debug.Print( "Waypoint found at {0}", 
        PointString( wayPoint ) );

      TransactionGroup tGroup 
        = new TransactionGroup( doc );

      tGroup.Start( "Auto-route placeholders" );

      // Create a new MEP mechanical system 
      // element from all connectors:

      Transaction t = new Transaction( doc );

      t.Start( "Create system" );

      Connector baseConnector = null;
      ConnectorSet newSystemCS = new ConnectorSet();

      foreach( EquipmentElement e in equipmentElements )
      {
        if( e.FamilyInstance.Category.Id.Equals(
          new ElementId( BuiltInCategory.OST_MechanicalEquipment ) ) )
        {
          baseConnector = e.SupplyAirConnector;
        }
        else
        {
          newSystemCS.Insert( e.SupplyAirConnector );
        }
      }
      doc.Create.NewMechanicalSystem( baseConnector, 
        newSystemCS, DuctSystemType.SupplyAir );

      t.Commit();

      bool xFirst = true;

      List<Connector> wayPointConnectors 
        = new List<Connector>();

      foreach( EquipmentElement e in equipmentElements )
      {
        Connector nextConnector;

        // if connector direction is vertical, 
        // add duct to reach target elevation

        if( !e.ConnectionDirection.IsAlmostEqualTo( XYZ.BasisZ ) )
        {
          throw new NotImplementedException( 
            "Not implemented for initially non-vertical connectors" );
        }

        t.Start( "Create placeholder duct" );
        
        XYZ secondPoint = new XYZ( e.ConnectionPoint.X, 
          e.ConnectionPoint.Y, wayPoint.Z );

        Duct duct = Duct.CreatePlaceholder( doc,
          ductType.Id, level1.Id, e.ConnectionPoint, 
          secondPoint );

        t.Commit();

        t.Start( "Connect duct" );

        Connector targetConnector = GetDuctConnectorAt( 
          duct, e.ConnectionPoint, out nextConnector );

        targetConnector.ConnectTo( e.SupplyAirConnector );

        t.Commit();

        // all connections should make a right 
        // hand turn into the waypoint

        XYZ nextConnectorPoint = nextConnector.Origin;
        XYZ nextWayPoint = null;
        if( xFirst )
        {
          nextWayPoint = new XYZ( wayPoint.X, 
            nextConnectorPoint.Y, wayPoint.Z );
        }
        else
        {
          nextWayPoint = new XYZ( nextConnectorPoint.X, 
            wayPoint.Y, wayPoint.Z );
        }

        t.Start( "Create placeholder duct" );
        
        Duct nextDuct = Duct.CreatePlaceholder( doc, 
          ductType.Id, level1.Id, nextConnectorPoint, 
          nextWayPoint );

        t.Commit();
        
        t.Start( "Add fitting" );
        
        Connector nextNextConnector;
        
        Connector nextTargetConnector 
          = GetDuctConnectorAt( nextDuct, 
            nextConnectorPoint, out nextNextConnector );

        doc.Create.NewElbowFitting( nextConnector, 
          nextTargetConnector );

        t.Commit();

        t.Start( "Create placeholder duct" );

        nextDuct = Duct.CreatePlaceholder( doc, 
          ductType.Id, level1.Id, 
          nextNextConnector.Origin, wayPoint );

        t.Commit();

        t.Start( "Add fitting" );

        Connector lastConnector;

        Connector nextNextTargetConnector 
          = GetDuctConnectorAt( nextDuct, 
            nextNextConnector.Origin, 
            out lastConnector );

        doc.Create.NewElbowFitting( nextNextConnector, 
          nextNextTargetConnector );

        wayPointConnectors.Add( lastConnector );

        t.Commit();

        xFirst = !xFirst;
      }

      if( wayPointConnectors.Count != 4 )
      {
        throw new Exception( 
          "Unexpected number of connectors" );
      }

      t.Start( "Add cross fitting" );

      doc.Create.NewCrossFitting( 
        wayPointConnectors[0], wayPointConnectors[2], 
        wayPointConnectors[1], wayPointConnectors[3] );

      t.Commit();

      tGroup.Assimilate();
    }

    public Result Execute(
      ExternalCommandData revit,
      ref string message,
      ElementSet elements )
    {
      UIApplication uiapp = revit.Application;
      Application app = uiapp.Application;

      if( ProductType.MEP != app.Product )
      {
        message = "Please run this command in Revit MEP.";
        return Result.Failed;
      }

      Document doc = uiapp.ActiveUIDocument.Document;

      ExecuteMepPlaceholders( doc );

      return Result.Succeeded;
    }
  }

  /// <summary>
  /// Convert placeholder elements to real ducts.
  /// </summary>
  [Transaction( TransactionMode.Manual )]
  public class ConvertPlaceholders : IExternalCommand
  {
    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Document doc = commandData.View.Document;

      Transaction t = new Transaction( doc );
      t.Start( "Convert placeholder network" );

      FilteredElementCollector ductCollector
        = new FilteredElementCollector( doc )
          .OfClass( typeof( Duct ) );

      Func<Duct, bool> isPlaceholder 
        = duct => duct.IsPlaceholder;

      IEnumerable<Duct> ducts = ductCollector
        .OfType<Duct>()
        .Where<Duct>( isPlaceholder );

      ICollection<ElementId> ductIds = ducts
        .Select<Duct, ElementId>( duct => duct.Id )
        .ToList<ElementId>();

      MechanicalUtils.ConvertDuctPlaceholders( 
        doc, ductIds );

      t.Commit();

      return Result.Succeeded;
    }
  }

  /// <summary>
  /// Set cable tray shape.
  /// </summary>
  [Transaction( TransactionMode.Manual )]
  public class SetCableTrayShape : IExternalCommand
  {
    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Document doc = commandData.Application
        .ActiveUIDocument.Document;

      // Get the trays

      FilteredElementCollector trays 
        = new FilteredElementCollector( doc )
          .OfCategory( BuiltInCategory.OST_CableTray )
          .WhereElementIsNotElementType();

      // Get the ladder tray type

      FilteredElementCollector trayTypes 
        = new FilteredElementCollector( doc )
          .OfClass( typeof( CableTrayType ) );

      Element ladderType = trayTypes.First<Element>( 
        e => e.Name.Equals( "Ladder Cable Tray" ) );

      // Set all trays type to ladder

      foreach( Element tray in trays )
      {
        Transaction trans = new Transaction( doc, "Edit Type" );
        trans.Start();
        tray.ChangeTypeId( ladderType.Id );
        trans.Commit();
      }
      return Result.Succeeded;
    }
  }
}
