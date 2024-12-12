#region Header
//
// CmdNewSpotElevation.cs - insert a new spot elevation on top surface of beam
//
// Copyright (C) 2010-2011 by Jeremy Tammik,
// Autodesk Inc. All rights reserved.
//
#endregion // Header

#region Namespaces
using System.Collections.Generic;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
#endregion // Namespaces

namespace BuildingCoder
{
  [Transaction( TransactionMode.Automatic )]
  class CmdNewSpotElevation : IExternalCommand
  {
    /// <summary>
    /// Simulate VSTA macro Application class member variable:
    /// </summary>
    Autodesk.Revit.Creation.Application Create;

    /// <summary>
    /// Return a view with the given name in the document.
    /// </summary>
    private View FindView( Document doc, string name )
    {
      // todo: check whether this includes derived types,
      // which is what we need here. in revit 2009, we used
      // TypeFilter filter = Create.Filter.NewTypeFilter(
      //   typeof( View ), true );

      return Util.GetFirstElementOfTypeNamed(
        doc, typeof( View ), name ) as View;
    }

    /// <summary>
    /// Return a reference to the topmost face of the given element.
    /// </summary>
    private Reference FindTopMostReference( Element e )
    {
      Document doc = e.Document;

      SubTransaction t = new SubTransaction( doc );

      XYZ viewDirection = Create.NewXYZ( 0, 0, -1 );

      View3D view3D = doc.Create.NewView3D(
        viewDirection );

      BoundingBoxXYZ elemBoundingBox
        = e.get_BoundingBox( view3D );

      XYZ max = elemBoundingBox.Max;

      XYZ minAtMaxElevation = Create.NewXYZ(
        elemBoundingBox.Min.X, elemBoundingBox.Min.Y, max.Z );

      XYZ centerOfTopOfBox = minAtMaxElevation
        .Add( max ).Divide( 2 );

      centerOfTopOfBox += 10 * XYZ.BasisZ;

      //ReferenceArray references
      //  = doc.FindReferencesByDirection(
      //    centerOfTopOfBox, viewDirection, view3D ); // 2011

      IList<ReferenceWithContext> references
        = doc.FindReferencesWithContextByDirection(
          centerOfTopOfBox, viewDirection, view3D ); // 2012

      double closest = double.PositiveInfinity;

      Reference ret = null;

      //foreach( Reference r in references )
      //{
      //  // 'Autodesk.Revit.DB.Reference.Element' is
      //  // obsolete: Property will be removed. Use
      //  // Document.GetElement(Reference) instead.
      //  //Element re = r.Element; // 2011

      //  Element re = doc.GetElement( r ); // 2012

      //  if( re.Id.IntegerValue == e.Id.IntegerValue
      //    && r.ProximityParameter < closest )
      //  {
      //    ret = r;
      //    closest = r.ProximityParameter;
      //  }
      //}

      foreach( ReferenceWithContext r in references )
      {
        Element re = doc.GetElement( 
          r.GetReference() ); // 2012

        if( re.Id.IntegerValue == e.Id.IntegerValue
          && r.Proximity < closest )
        {
          ret = r.GetReference();
          closest = r.Proximity;
        }
      }

      t.RollBack();

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

      //ElementId instanceId = Create.NewElementId();
      //instanceId.IntegerValue = 230298;

      ElementId instanceId = new ElementId( 230298 );

      FamilyInstance beam = doc.get_Element(
        instanceId ) as FamilyInstance;

      if( null == beam )
      {
        Util.ErrorMsg( "Beam 230298 not found." );
        return false;
      }

      Reference topReference 
        = FindTopMostReference( beam );

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
    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      UIApplication app = commandData.Application;
      Document doc = app.ActiveUIDocument.Document;
      Create = app.Application.Create;

      return NewSpotElevation( doc )
        ? Result.Succeeded
        : Result.Failed;
    }
  }
}
