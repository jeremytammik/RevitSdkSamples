#region Header
// Revit Structure API Labs
//
// Copyright (C) 2010-2013 by Autodesk, Inc.
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
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
#endregion // Header

#region Namespaces
using System;
using System.Collections.Generic;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
#endregion // Namespaces

namespace NewRstLab
{
  /// <summary>
  /// This command achieves:
  /// Create a 5 segments U shape rebar shape.    
  /// </summary>
  #region RstLab4_1_CreateRebarShape
  [Transaction( TransactionMode.Manual )]
  public class RstLab4_1_CreateRebarShape : IExternalCommand
  {
    Application m_app;
    public Result Execute(
        ExternalCommandData commandData,
        ref string messages,
        ElementSet elements )
    {
      UIApplication uiApp = commandData.Application;
      Application app = uiApp.Application;
      m_app = app;
      Document doc = uiApp.ActiveUIDocument.Document;

      Transaction trans = new Transaction( doc, "Lab4_1" );
      trans.Start();


      //  5 segment sample from SDK (see the illustration in the SDK) Metric.  
      //        pO
      //  A |       | E  _
      //   B \_____/ D   _ H 
      //        C
      //          | |
      //           K

      RebarShapeDefinitionBySegments shapeDef = new RebarShapeDefinitionBySegments( doc, 5 );

      //  the real meat of information is under rebarshape definition. 
      //

      //  add parameters with default value. 
      ExternalDefinition def = GetOrCreateSharedParameter( "A" );
      ElementId idA = RebarShapeParameters.GetOrCreateElementIdForExternalDefinition( doc, def );
      shapeDef.AddParameter( idA, 280 );

      def = GetOrCreateSharedParameter( "B" );
      ElementId idB = RebarShapeParameters.GetOrCreateElementIdForExternalDefinition( doc, def );
      shapeDef.AddParameter( idB, 453 );

      def = GetOrCreateSharedParameter( "C" );
      ElementId idC = RebarShapeParameters.GetOrCreateElementIdForExternalDefinition( doc, def );
      shapeDef.AddParameter( idC, 560 );

      def = GetOrCreateSharedParameter( "D" );
      ElementId idD = RebarShapeParameters.GetOrCreateElementIdForExternalDefinition( doc, def );
      shapeDef.AddParameter( idD, 453 );

      def = GetOrCreateSharedParameter( "E" );
      ElementId idE = RebarShapeParameters.GetOrCreateElementIdForExternalDefinition( doc, def );
      shapeDef.AddParameter( idE, 280 );

      def = GetOrCreateSharedParameter( "H" );
      ElementId idH = RebarShapeParameters.GetOrCreateElementIdForExternalDefinition( doc, def );
      shapeDef.AddParameter( idH, 320 );

      def = GetOrCreateSharedParameter( "K" );
      ElementId idK = RebarShapeParameters.GetOrCreateElementIdForExternalDefinition( doc, def );
      shapeDef.AddParameter( idK, 320 );

      def = GetOrCreateSharedParameter( "O" );
      // here is the one with a formula. search valid formula syntax in product help.
      ElementId idO = RebarShapeParameters.GetOrCreateElementIdForExternalDefinition( doc, def );
      shapeDef.AddFormulaParameter( idO, "2*K+C" );

      /* public void AddConstraintParallelToSegment(
          int iSegment,
          Parameter param,
          bool measureToOutsideOfBend0,
          bool measureToOutsideOfBend1 ) */

      // add constraints
      // Length constraint for each segment
      shapeDef.AddConstraintParallelToSegment( 0, idA, false, false );
      shapeDef.AddConstraintParallelToSegment( 1, idB, false, false );
      shapeDef.AddConstraintParallelToSegment( 2, idC, false, false );
      shapeDef.AddConstraintParallelToSegment( 3, idD, false, false );
      shapeDef.AddConstraintParallelToSegment( 4, idE, false, false );

      shapeDef.SetSegmentFixedDirection( 0, 0.0, -1.0 );
      shapeDef.SetSegmentFixedDirection( 2, 1.0, 0.0 );
      shapeDef.SetSegmentFixedDirection( 4, 0.0, 1.0 );

      // set segment direction for variable-direction segment
      shapeDef.AddConstraintToSegment( 1, idH, 0.0, -1.0, 1, false, false );
      shapeDef.AddConstraintToSegment( 1, idK, 1.0, 0.0, -1, false, false );
      shapeDef.AddConstraintToSegment( 3, idK, 1.0, 0.0, 1, false, false );
      shapeDef.AddConstraintToSegment( 3, idH, 0.0, 1.0, -1, false, false );

      // second argument: 1 left, -1 right turn. 
      shapeDef.AddBendDefaultRadius( 1, 1, RebarShapeBendAngle.Acute ); // 2013
      shapeDef.AddBendDefaultRadius( 2, 1, RebarShapeBendAngle.Acute ); // 2013
      shapeDef.AddBendDefaultRadius( 3, 1, RebarShapeBendAngle.Acute ); // 2013
      shapeDef.AddBendDefaultRadius( 4, 1, RebarShapeBendAngle.Acute ); // 2013

      // this will add a read-only (gary out) dimention.  
      shapeDef.AddListeningDimensionBendToBend( idO, 1.0, 0.0, 0, 0, 4, 1 );

      //  create a newrebarshape. 
      //
      RebarShape oRebarShape = RebarShape.Create( doc, shapeDef, null, RebarStyle.Standard, StirrupTieAttachmentType.InteriorFace,
          180, RebarHookOrientation.Left,
          180, RebarHookOrientation.Left,
          0 );
      oRebarShape.Name = RstUtils.msRebarShapeName;

      //// finally, call commit. without this, it won't show up in the browser. 
      //try
      //{

      //    shapeDef.Commit();
      //}
      //catch (System.Exception ex)
      //{
      //    RstUtils.InfoMsg("Failed to commit rebar def: " + ex.ToString());
      //}

      trans.Commit();

      Boolean result = shapeDef.CheckDefaultParameterValues( 0, 0 );
      if( true == result )
      {
        RstUtils.InfoMsg( RstUtils.msRebarShapeName + " is created successfully" );
      }
      else
      {
        RstUtils.InfoMsg( "Failed to create RebarShape" );
      }

      return Result.Succeeded;
    }

    public ExternalDefinition GetOrCreateSharedParameter( string sParamName )
    {
      return SharedParameterHelper.GetOrCreateSharedParameter( m_app, sParamName );
    }
  }
  #endregion //RstLab4_1_CreateRebarShape


  /// <summary>
  /// This command achieves:
  /// 1. Create rebar for a selected column using the rebar shape we created in command RstLab4_1.
  /// 2. Change the layout of the rebar along the full column.    
  /// </summary>
  #region RstLab4_2_CreateRebar
  [Transaction( TransactionMode.Manual )]
  public class RstLab4_2_CreateRebar : IExternalCommand
  {

    Application m_app;
    public Result Execute(
        ExternalCommandData commandData,
        ref string messages,
        ElementSet elements )
    {
      UIApplication uiApp = commandData.Application;
      Application app = uiApp.Application;
      m_app = app;
      Document doc = uiApp.ActiveUIDocument.Document;

      Transaction trans = new Transaction( doc, "Lab4_2" );
      trans.Start();

      //select a structural column
      FamilyInstance column = null;
      BuiltInCategory builtCat = BuiltInCategory.OST_StructuralColumns;
      column = RstUtils.GetFamilyInstanceBySelection( doc, uiApp, builtCat,
          "Please select a vertical concrete column" );

      // Judge if it is made of concrete

      //MaterialSet materials = column.Materials;

      //MaterialConcrete matConcrete = null;
      
      //foreach( Material mat in materials )
      //{
      //  // cast it to concrete material
      //  matConcrete = mat as MaterialConcrete; 
      //  break;
      //}
      //if( null == matConcrete )
      //{
      //  messages = "The selected column is not concrete";
      //  trans.RollBack();
      //  return Result.Failed;
      //}

      if( StructuralMaterialType.Concrete 
        != column.StructuralMaterialType )
      {
        messages = "The selected column is not concrete";
        trans.RollBack();
        return Result.Failed;
      }

      // Get the RebarShape and RebarBarType

      RebarShape barShape;
      RebarBarType barType;
      RstUtils.GetRebarShape( RstUtils.msRebarShapeName, 
        "#10", out barShape, out barType, doc );

      // create rebar for column

      XYZ vectorX = XYZ.BasisX;
      XYZ vectorY = XYZ.BasisY;

      //LocationPoint columnPt = column.Location as LocationPoint;
      //XYZ originPt = columnPt.Point;

      try
      {
        //Get the rebar's origin and two direction vecter.
        GeometrySupport geometryData = new GeometrySupport( column );
        List<Autodesk.Revit.DB.XYZ> profilePoints = geometryData.ProfilePoints;
        Autodesk.Revit.DB.XYZ origin = profilePoints[0];
        Autodesk.Revit.DB.XYZ yVec = profilePoints[1] - origin;
        Autodesk.Revit.DB.XYZ xVec = profilePoints[3] - origin;

        // no longer doc.Create.NewRebar:

        Rebar createdRebar = Rebar.CreateFromRebarShape( doc,
            barShape, barType, column, origin, xVec, yVec );

        doc.Regenerate();

        // layout the rebar to fit the column's section.

        createdRebar.ScaleToBox( origin, xVec, yVec );

        double barSpacing = 0.1;

        int barNum = (int) ( geometryData.DrivingLength / barSpacing );

        createdRebar.SetLayoutAsNumberWithSpacing(
            barNum, barSpacing, true, true, true );

        trans.Commit();

        return Result.Succeeded;
      }
      catch( Exception ex )
      {
        RstUtils.InfoMsg( "Failed to create rebar for the selected column: "
          + ex.Message );

        trans.RollBack();

        return Result.Failed;
      }
    }
  }
  #endregion //RstLab4_2_CreateRebar
}
