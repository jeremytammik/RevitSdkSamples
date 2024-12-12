#region Header
//
// CmdNewLineLoad.cs - create a new structural line load element
//
// Copyright (C) 2009-2010 by Jeremy Tammik,
// Autodesk Inc. All rights reserved.
//
#endregion // Header

#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit;
using Autodesk.Revit.Elements;
//using Autodesk.Revit.Geometry;
using Autodesk.Revit.Structural;
using Autodesk.Revit.Symbols;
using Curve = Autodesk.Revit.Geometry.Curve;
using Line = Autodesk.Revit.Geometry.Line;
using Plane = Autodesk.Revit.Geometry.Plane;
using XYZ = Autodesk.Revit.Geometry.XYZ;
using XYZArray = Autodesk.Revit.Geometry.XYZArray;
using CmdResult = Autodesk.Revit.IExternalCommand.Result;
//using RvtElement = Autodesk.Revit.Element;
#endregion // Namespaces

// 1251154 [NewLineLoad]

namespace BuildingCoder
{
  class CmdNewLineLoad : IExternalCommand
  {
    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;
      Document doc = app.ActiveDocument;

      Autodesk.Revit.Creation.Application ca
        = app.Create;

      Autodesk.Revit.Creation.Document cd
        = doc.Create;

      Autodesk.Revit.Creation.Filter cf
        = ca.Filter;

      // determine line load symbol to use:

      List<Element> symbols = new List<Element>();
      Filter f1 = cf.NewTypeFilter( typeof( LineLoadType ) );
      int n = doc.get_Elements( f1, symbols );

      LineLoadType loadSymbol 
        = symbols[0] as LineLoadType;

      // sketch plane and arrays of forces and moments:

      Plane plane = ca.NewPlane( XYZ.BasisZ, XYZ.Zero );
      SketchPlane skplane = cd.NewSketchPlane( plane );

      XYZ forceA = new XYZ( 0, 0, 5 );
      XYZ forceB = new XYZ( 0, 0, 10 );
      XYZArray forces = new XYZArray();
      forces.Append( forceA );
      forces.Append( forceB );

      XYZ momentA = new XYZ( 0, 0, 0 );
      XYZ momentB = new XYZ( 0, 0, 0 );
      XYZArray moments = new XYZArray();
      moments.Append( momentA );
      moments.Append( momentB );

      BuiltInCategory bic 
        = BuiltInCategory.OST_StructuralFraming;

      f1 = cf.NewTypeFilter( typeof( FamilyInstance ) );
      Filter f2 = cf.NewCategoryFilter( bic );
      Filter f3 = cf.NewLogicAndFilter( f1, f2 );

      List<Element> beams = new List<Element>();

      n = doc.get_Elements( f3, beams );

      XYZ p1 = new XYZ( 0, 0, 0 );
      XYZ p2 = new XYZ( 3, 0, 0 );
      XYZArray points = new XYZArray();
      points.Append( p1 );
      points.Append( p2 );

      // create a new unhosted line load on points:

      LineLoad lineLoadNoHost = cd.NewLineLoad( 
        points, forces, moments, 
        false, false, false, 
        loadSymbol, skplane );

      Debug.Print( "Unhosted line load works." );

      // create new line loads on beam:

      foreach( Element e in beams )
      {
        try
        {
          LineLoad lineLoad = cd.NewLineLoad(
            e, forces, moments,
            false, false, false,
            loadSymbol, skplane );

          Debug.Print( "Hosted line load on beam works." );
        }
        catch( Exception ex )
        {
          Debug.Print( "Hosted line load on beam fails: "
            + ex.Message );
        }

        FamilyInstance i = e as FamilyInstance;

        AnalyticalModelFrame amFrame 
          = i.AnalyticalModel as AnalyticalModelFrame;

        foreach( Curve curve in amFrame.Curves )
        {
          try
          {
            LineLoad lineLoad = cd.NewLineLoad( 
              curve.Reference, forces, moments, 
              false, false, false, 
              loadSymbol, skplane );

            Debug.Print( "Hosted line load on "
              + "AnalyticalModelFrame curve works." );
          }
          catch( Exception ex )
          {
            Debug.Print( "Hosted line load on "
              + "AnalyticalModelFrame curve fails: " 
              + ex.Message );
          }
        }
      }
      return CmdResult.Succeeded;
    }
  }
}
