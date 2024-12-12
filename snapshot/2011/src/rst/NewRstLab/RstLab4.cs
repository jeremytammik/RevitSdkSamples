#region Header
// Revit Structure API Labs
//
// Copyright (C) 2010 by Autodesk, Inc.
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
  [Regeneration( RegenerationOption.Manual )]
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

      //  create a newrebarshape. 
      //
      RebarShape oRebarShape = doc.Create.NewRebarShape();
      oRebarShape.Name = RstUtils.msRebarShapeName;

      //  the real meat of information is under rebarshape definition. 
      //
      RebarShapeDefinitionBySegments shapeDef = 
          oRebarShape.NewDefinitionBySegments( 5 );  

      //  add parameters with default value. 
      ExternalDefinition def = GetOrCreateSharedParameter( "A" );
      Parameter pA = shapeDef.AddParameter( def, 280 );

      def = GetOrCreateSharedParameter( "B" );
      Parameter pB = shapeDef.AddParameter( def, 453 );

      def = GetOrCreateSharedParameter( "C" );
      Parameter pC = shapeDef.AddParameter( def, 560 );

      def = GetOrCreateSharedParameter( "D" );
      Parameter pD = shapeDef.AddParameter( def, 453 );

      def = GetOrCreateSharedParameter( "E" );
      Parameter pE = shapeDef.AddParameter( def, 280 );

      def = GetOrCreateSharedParameter( "H" );
      Parameter pH = shapeDef.AddParameter( def, 320 );

      def = GetOrCreateSharedParameter( "K" );
      Parameter pK = shapeDef.AddParameter( def, 320 );

      def = GetOrCreateSharedParameter( "O" );
      // here is the one with a formula. search valid formula syntax in product help.
      Parameter pO = shapeDef.AddFormulaParameter( def, "2*K+C" ); 

      // add constraints
      //Length constraint for each segment
      shapeDef.AddConstraintParallelToSegment( 0, pA, false, false );  
      shapeDef.AddConstraintParallelToSegment( 1, pB, false, false );
      shapeDef.AddConstraintParallelToSegment( 2, pC, false, false );
      shapeDef.AddConstraintParallelToSegment( 3, pD, false, false );
      shapeDef.AddConstraintParallelToSegment( 4, pE, false, false );

      shapeDef.SetSegmentFixedDirection( 0, 0.0, -1.0 );
      shapeDef.SetSegmentFixedDirection( 2, 1.0, 0.0 );
      shapeDef.SetSegmentFixedDirection( 4, 0.0, 1.0 );

      //set segment direction for variable-direction segment
      shapeDef.AddConstraintToSegment( 1, pH, 0.0, -1.0, 1, false, false ); 
      shapeDef.AddConstraintToSegment( 1, pK, 1.0, 0.0, -1, false, false );
      shapeDef.AddConstraintToSegment( 3, pK, 1.0, 0.0, 1, false, false );
      shapeDef.AddConstraintToSegment( 3, pH, 0.0, 1.0, -1, false, false );

      // second argument: 1 left, -1 right turn. 
      shapeDef.AddBendDefaultRadius( 1, 1, RebarShapeBendAngle.Acute ); 
      shapeDef.AddBendDefaultRadius( 2, 1, RebarShapeBendAngle.Acute );
      shapeDef.AddBendDefaultRadius( 3, 1, RebarShapeBendAngle.Acute );
      shapeDef.AddBendDefaultRadius( 4, 1, RebarShapeBendAngle.Acute );

      // this will add a read-only (gary out) dimention.  
      shapeDef.AddListeningDimensionBendToBend( pO, 1.0, 0.0, 0, 0, 4, 1 ); 

      // set hooks (optional).  
      oRebarShape.set_HookAngle( 0, 180 );
      oRebarShape.set_HookAngle( 1, 180 );
      oRebarShape.set_HookOrientation( 0, 
          Autodesk.Revit.DB.Structure.RebarHookOrientation.Left );
      oRebarShape.set_HookOrientation( 1, 
          Autodesk.Revit.DB.Structure.RebarHookOrientation.Left );

      // finally, call commit. without this, it won't show up in the browser. 
      try
      {
        shapeDef.Commit();
      }
      catch( System.Exception ex )
      {
        RstUtils.InfoMsg( "Failed to commit rebar def: " + ex.ToString() );
      }

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
  ///1.	Create rebar for a selected column using the rebar shape we created in command RstLab4_1.
  ///2.	Change the layout of the rebar along the full column.    
  /// </summary>
  #region RstLab4_2_CreateRebar
  [Transaction( TransactionMode.Manual )]
  [Regeneration( RegenerationOption.Manual )]
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
      column = RstUtils.GetFamilyInstanceBySelection( doc, uiApp,builtCat,
          "Please select a concrete column" );

      //judge if it is made of concrete.
      MaterialSet materials = column.Materials;
      MaterialConcrete matConcrete = null;
      foreach( Material mat in materials )
      {
        matConcrete = mat as MaterialConcrete; //cast it to concrete material
        break;
      }
      if( null == matConcrete )
      {
        messages = "The selected column is not concrete column";
        trans.RollBack();
        return Result.Failed;
      }


      //Get the RebarShape and RebarBarType
      RebarShape barShape;
      RebarBarType barType;
      RstUtils.GetRebarShape( RstUtils.msRebarShapeName, "#10", 
          out barShape, out barType, doc );
      //create rebar for column
      XYZ vectorX = new XYZ( 1, 0, 0 );
      XYZ vectorY = new XYZ( 0, 1, 0 );

      LocationPoint columnPt = column.Location as LocationPoint;
      XYZ originPt = columnPt.Point;

      try
      {
        //Get the rebar's origin and two direction vecter.
        GeometrySupport geometryData = new GeometrySupport( column );
        List<Autodesk.Revit.DB.XYZ> profilePoints = geometryData.ProfilePoints;
        Autodesk.Revit.DB.XYZ origin = profilePoints[0];
        Autodesk.Revit.DB.XYZ yVec = profilePoints[1] - origin;
        Autodesk.Revit.DB.XYZ xVec = profilePoints[3] - origin;

        Rebar createdRebar = doc.Create.NewRebar( 
            barShape, barType, column, origin, xVec, yVec );
        doc.Regenerate();

        //layout the rebar to fit the column's section.
        createdRebar.ScaleToBox( origin, xVec, yVec );
        double barSpacing = 0.1;
        int barNum = ( int ) ( geometryData.DrivingLength / barSpacing );
        createdRebar.SetLayoutAsNumberWithSpacing(
            barNum, barSpacing, true, true, true );
      }
      catch( Exception ex )
      {
        RstUtils.InfoMsg( "Failed to create rebar for the selected column." 
            + ex.Message );
      }

      doc.Regenerate();

      trans.Commit();
      return Result.Succeeded;
    }
  }
  #endregion //RstLab4_2_CreateRebar
}
