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
  /// List all structural columns in the model.
  /// </summary>
  #region RstLab2_1_ListStructuralColumns
  [Transaction( TransactionMode.Manual )]
  public class RstLab2_1_ListStructuralColumns : IExternalCommand
  {
    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {

      UIApplication app = commandData.Application;
      Document doc = app.ActiveUIDocument.Document;
      Transaction trans = new Transaction( doc, "Lab2_1" );
      trans.Start();
      try
      {
        //
        // Get all Structural COLUMNS - we can use a generic utility.              
        BuiltInCategory bicSc = BuiltInCategory.OST_StructuralColumns;
        IList<Element> columns = RstUtils.GetAllStandardFamilyInstancesForACategory( doc, bicSc );
        string sMsg = "There are " + columns.Count.ToString() + " structural columns.";

        foreach( FamilyInstance fi in columns )
        {
          sMsg += "\r\n  " + RstUtils.StructuralElementDescription( fi );
        }
        RstUtils.InfoMsg( sMsg );
        trans.Commit();
        return Result.Succeeded;
      }
      catch( Exception ex )
      {
        message = ex.Message;
        trans.RollBack();
        return Result.Failed;
      }
    }
  }
  #endregion // RstLab2_1_ListStructuralColumns

  // Note
  // We can get beam, brace element in the same way as lab2_1_listStructuralColumns.         
  // Replace category OST_StructuralColumns with OST_StructuralFramings.


  /// <summary>
  /// This command achieves:
  /// List all structural wall and their details  
  /// </summary>
  #region RstLab2_2_ListStructuralWalls
  [Transaction( TransactionMode.Manual )]
  public class RstLab2_2_ListStructuralWalls : IExternalCommand
  {
    public Result Execute(
        ExternalCommandData commandData,
        ref string messages,
        ElementSet elements )
    {
      UIApplication uiApp = commandData.Application;
      Application app = uiApp.Application;
      Document doc = uiApp.ActiveUIDocument.Document;

      Transaction trans = new Transaction( doc, "RstLab2_2" );
      trans.Start();

      IList<Element> listWalls = RstUtils.GetElementOfClass( doc, typeof( Wall ) );
      //Above listWalls includes structural wall and architecture wall. 
      //Filter out architecture wall by Wall.StructuralUsage property.

      List<Wall> walls = new List<Wall>();
      foreach( Wall wall in listWalls )
      {
        //
        // Note that instead of looping through and checking for a
        // NonBearing wall, we might also be able to use some
        // other criterion that can be fed straight into the Revit API
        // filtering mechanism, such as GetAnalyticalModel.
        // StructuralUsage has better performance than GetAnalyticalModel method.
        if( StructuralWallUsage.NonBearing != wall.StructuralUsage )
        {
          walls.Add( wall );
        }
      }

      string sMsg = null;
      sMsg = string.Format( 
          "There are {0} structural walls in the model", walls.Count );

      foreach( Wall wall in walls )
      {
        sMsg += "\r\n" + RstUtils.StructuralElementDescription( wall );
      }
      trans.Commit();
      RstUtils.InfoMsg( sMsg );

      return Result.Succeeded;
    }

  }
  #endregion  // RstLab2_2_ListStructuralWalls

  // Note
  // Lab2_2_ListStructuralWall demonstrates how to get all structural walls. 
  // Structural floor, structrual ContinuousFooting can be gotten in the same way,
  // you just need to replace the class Wall with Floor/ContFooting. 

  /// <summary>
  /// This command achieves:
  /// List all point, line and area boundary conditions
  /// </summary>
  #region RstLab2_3_ListBoundaryConditions
  [Transaction( TransactionMode.Manual )]
  public class RstLab2_3_ListBoundaryConditions : IExternalCommand
  {
    public Result Execute(
        ExternalCommandData commandData,
        ref string messages,
        ElementSet elements )
    {
      UIApplication uiApp = commandData.Application;
      Application app = uiApp.Application;
      Document doc = uiApp.ActiveUIDocument.Document;

      Transaction trans = new Transaction( doc, "Lab2_3" );
      trans.Start();

      IList<Element> listBCs = null;
      listBCs = RstUtils.GetElementOfClass(doc, typeof(BoundaryConditions));
      //lilstBCs contains point, line and area three types of BoundaryConditions
      string sMsg = null;
      foreach( BoundaryConditions bc in listBCs )
      {
        sMsg += "\nBoundary condition Id = " + bc.Id.IntegerValue.ToString();
        sMsg += string.Format( "\n The host element:{0}, id = {1}",
                bc.HostElement.Name, bc.HostElement.Id.IntegerValue.ToString() );
        Parameter param = null;
        param = bc.get_Parameter(BuiltInParameter.BOUNDARY_CONDITIONS_TYPE);
        //Get the host element

        switch( param.AsInteger() )
        {
          case 0:
            XYZ point = bc.Point;
            sMsg += "\nThis bc is a Point Boundary Conditions.";
            sMsg += "\nLocation point: (" + RstUtils.PointString( point ) + ")";
            sMsg += "\n";
            break;
          case 1:
            sMsg += "\nThis bc is a Line Boundary Conditions.";
            Curve curve = bc.get_Curve( 0 );
            // Get curve start point
            sMsg += "\nLocation Line: start point: ("
                + RstUtils.PointString( curve.get_EndPoint( 0 ) ) + ")";
            // Get curve end point
            sMsg += ";  end point:(" + 
                    RstUtils.PointString( curve.get_EndPoint( 1 ) ) + ")";
            sMsg += "\n";
            break;
          case 2:
            sMsg += "\nThis bc is an Area Boundary Conditions.";
            sMsg += string.Format( "\nIt has {0} support edges", bc.NumCurves );
            for( int i = 0; i < bc.NumCurves; i++ )
            {
              Autodesk.Revit.DB.Curve areaCurve = bc.get_Curve( i );
              // Get curve start point
              sMsg += "\nCurve start point:(" + 
                  RstUtils.PointString( areaCurve.get_EndPoint( 0 ) ) + ")";
              // Get curve end point
              sMsg += "; Curve end point:(" + 
                  RstUtils.PointString( areaCurve.get_EndPoint( 1 ) ) + ")";
            }
            sMsg += "\n";
            break;
          default:
            break;
        }
      }
      trans.Commit();
      RstUtils.InfoMsg( sMsg );
      return Result.Succeeded;
    }
  }
  #endregion  // RstLab2_3_ListBoundaryConditions
}
