#region Header
//
// CmdNewSpotElevation.cs - insert a new spot elevation on top surface of beam
//
// Copyright (C) 2010 by Jeremy Tammik,
// Autodesk Inc. All rights reserved.
//
#endregion // Header

#region Namespaces
using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Geometry;
using CmdResult = Autodesk.Revit.IExternalCommand.Result;
using Element = Autodesk.Revit.Element;
#endregion // Namespaces

namespace BuildingCoder
{
  class CmdNewSpotElevation : IExternalCommand
  {
    /// <summary>
    /// Simulate VSTA macro Application class member variable:
    /// </summary>
    Autodesk.Revit.Creation.Application Create; // = app.Create;

    /// <summary>
    /// Return a view with the given name in the document.
    /// </summary>
    private View FindView( Document doc, string name )
    {
      TypeFilter filter = Create.Filter.NewTypeFilter(
        typeof( View ), true );

      ElementIterator iter = doc.get_Elements( filter );
      View ret = null;

      iter.Reset();
      while( iter.MoveNext() )
      {
        Element e = iter.Current as Element;
        
        if( e.Name == name )
        {
          ret = e as View;
          break;
        }
      }
      return ret;
    }

    /// <summary>
    /// Return a reference to the topmost face of the given element.
    /// </summary>
    private Reference FindTopMostReference( Element e )
    {
      Document doc = e.Document;

      doc.BeginTransaction();

      XYZ viewDirection = Create.NewXYZ( 0, 0, -1 );

      View3D view3D = doc.Create.NewView3D( 
        viewDirection );

      BoundingBoxXYZ elemBoundingBox 
        = e.get_BoundingBox( view3D );

      XYZ max = elemBoundingBox.Max;

      XYZ minAtMaxElevation = elemBoundingBox.Min;

      minAtMaxElevation.Z = max.Z;

      XYZ centerOfTopOfBox = minAtMaxElevation
        .Add( max ).Divide( 2 );

      centerOfTopOfBox.Z = centerOfTopOfBox.Z + 10;

      ReferenceArray references 
        = doc.FindReferencesByDirection( 
          centerOfTopOfBox, viewDirection, view3D );

      double closest = double.PositiveInfinity;

      Reference ret = null;

      foreach( Reference r in references )
      {
        if( r.Element.Id.Value == e.Id.Value 
          && r.ProximityParameter < closest )
        {
          ret = r;
          closest = r.ProximityParameter;
        }
      }
      doc.AbortTransaction();

      return ret;
    }

    /// <summary>
    /// Create three new spot elevations on the top surface of a beam,
    /// at its midpoint and both endpoints.
    /// </summary>
    bool NewSpotElevation( Document doc )
    {
      //Document doc = ActiveDocument; // for VSTA macro version

      View westView = FindView( doc, "West" );

      if( null == westView )
      {
        Util.ErrorMsg( "No view found named 'West'." );
        return false;
      }

      // define the hard coded element id of beam:

      ElementId instanceId = Create.NewElementId();

      instanceId.Value = 230298; 

      FamilyInstance beam = doc.get_Element( 
        ref instanceId ) as FamilyInstance;

      if( null == beam )
      {
        Util.ErrorMsg( "Beam 230298 not found." );
        return false;
      }

      Reference topReference = FindTopMostReference( beam );

      LocationCurve lCurve = beam.Location 
        as LocationCurve;

      //doc.BeginTransaction(); // for VSTA macro version

      for( int beamIndex = 0; beamIndex < 3; ++beamIndex )
      {
        XYZ lCurvePnt = lCurve.Curve.Evaluate( 
          0.5 * beamIndex, true );

        XYZ bendPnt = lCurvePnt.Add( 
          Create.NewXYZ( 0, 1, 4 ) );

        XYZ endPnt = lCurvePnt.Add( 
          Create.NewXYZ( 0, 2, 4 ) );

        // NewSpotElevation arguments:
        //
        // View view, Reference reference, 
        // XYZ origin, XYZ bend, XYZ end, XYZ refPt, 
        // bool hasLeader

        SpotDimension sd = doc.Create.NewSpotElevation( 
          westView, topReference, lCurvePnt, bendPnt, 
          endPnt, lCurvePnt, true );
      }
      //doc.EndTransaction(); // for VSTA macro version

      return true;
    }

    /// <summary>
    /// Exrernal command mainline method for non-VSTA solution.
    /// </summary>
    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;
      Document doc = app.ActiveDocument;
      Create = app.Create;

      return NewSpotElevation( doc )
        ? CmdResult.Succeeded
        : CmdResult.Failed;
    }
  }
}
