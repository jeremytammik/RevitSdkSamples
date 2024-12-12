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
  /// Commands in this Lab1 show how to access object related to load group, load type and load.
  /// </summary>
  #region RstLab1_1_LoadCase
  [Transaction( TransactionMode.Manual )]
  public class RstLab1_1_LoadCase : IExternalCommand
  {
    public Result Execute(
        ExternalCommandData commandData,
        ref string messages,
        ElementSet elements )
    {
      UIApplication uiApp = commandData.Application;
      Application app = uiApp.Application;
      Document doc = uiApp.ActiveUIDocument.Document;

      Transaction trans = new Transaction( doc, "Lab1_1" );
      trans.Start();

      //List LoadCase in current model            
      IList<Element> lists = RstUtils.GetElementOfClass(
          doc, typeof( LoadCase ) );
      string sMsg = null;
      sMsg = string.Format( "There are {0} LoadCase objects in this model.\n They are :\n",
          lists.Count );
      foreach( Element elem in lists )
      {
        LoadCase ln = elem as LoadCase;
        sMsg += ln.Name + "\n";
      }
      RstUtils.InfoMsg( sMsg );
      //In the same way to get LoadCase objects, you can also get LoadNature objects, LoadUsage objects. 
      //Just replace the class name LoadCase with LoadCase or LoadUsage.

      //Create a new LoadUsage object. We can create other load group objects in the similar way.
      string sName = "MainTainLoad";
      Boolean bExist = false;
      IList<Element> listLU = RstUtils.GetElementOfClass(
          doc, typeof( LoadUsage ) );
      foreach( Element elem in listLU )
      {
        if( elem.Name.Equals( sName ) )
        {
          bExist = true;
          break;
        }
      }
      if( !bExist )
      {
        LoadUsage lu = doc.Create.NewLoadUsage( "MaintainLoad" );
        if( null != lu )
        {
          sMsg = "LoadUsage MainTainLoad was created";
        }
      }
      else
      {
        sMsg = "LoadUsage MainTainLoad existed. ";

      }
      RstUtils.InfoMsg( sMsg );

      //Find the first LoadCombination
      IList<Element> listLC = RstUtils.GetElementOfClass(
          doc, typeof( LoadCombination ) );
      LoadCombination lc1 = null;
      //Show the properties of this load combination
      sMsg = "There are " + listLC.Count.ToString()
          + " LoadCombination objects in the model:";
      foreach( LoadCombination comb in listLC )
      {
        //Get the first load combination for later usage.
        if( null != lc1 )
          lc1 = comb;
        // combinaton properties
        string usageNames = ( 0 == comb.NumberOfUsages )
          ? "[NONE]"
          : comb.get_UsageName( 0 );
        for( int i = 1; i < comb.NumberOfUsages; ++i )
        {
          usageNames += "+";
          usageNames += comb.get_UsageName( i );
        }
        sMsg += "\r\n\r\n  " + comb.Name
          + ", Id=" + comb.Id.IntegerValue.ToString()
          + ", Type=" + comb.CombinationType
          + ", TypeIndex=" + comb.CombinationTypeIndex
          + ", State=" + comb.CombinationState
          + ", StateIndex=" + comb.CombinationStateIndex
          + ", Usages=" + usageNames;

        sMsg += "\r\n    Number of components = " + comb.NumberOfComponents + ":";
        // loop all component properties
        for( int i = 0; i < comb.NumberOfComponents; ++i )
        {
          sMsg += "\r\n    Comp.name=" + comb.get_CombinationCaseName( i )
            + "  Comp.nature=" + comb.get_CombinationNatureName( i )
            + "  Factor=" + comb.get_Factor( i );
        }
      }
      RstUtils.InfoMsg( sMsg );

      trans.Commit();

      return Result.Succeeded;
    }
  }

  #endregion //RstLab1_1_LoadCase

  ////            
  // Loads sample in Revit SDK shows how to crete LoadCombination. 
  // We don't show it here.
  ////

  /// <summary>
  /// This command achieves:
  /// 1. Get a point load type,then create a point load with this load type.
  /// 2. list all point loads, 
  /// 3. change the force value for one of the point load.
  /// </summary>
  #region RstLab1_2_LoadsOperation
  [Transaction( TransactionMode.Manual )]
  public class RstLab1_2_LoadsOperation : IExternalCommand
  {
    public Result Execute(
        ExternalCommandData commandData,
        ref string messages,
        ElementSet elements )
    {
      UIApplication uiApp = commandData.Application;
      Application app = uiApp.Application;
      Document doc = uiApp.ActiveUIDocument.Document;

      Transaction trans = new Transaction( doc, "RstLab1_2" );
      trans.Start();

      try
      {
        string sMsg = null;
        double dKip = 14593.9;  // 1 kip = 14593.9 

        // List Point load types, save first load type for later usage.

        FilteredElementCollector collector = new FilteredElementCollector( doc );
        collector.OfClass( typeof( PointLoadType ) );
        PointLoadType loadType = collector.FirstElement() as PointLoadType;

        // create a point load using the PointLoadType obtained previously

        XYZ xyzPt = uiApp.ActiveUIDocument.Selection.PickPoint(
            "Please pick a point to create a point load" );

        XYZ xyzForce = new XYZ( 0, 0, dKip );
        XYZ xyzMoment = new XYZ( dKip, 0, 0 );

        PointLoad plNew = doc.Create.NewPointLoad(
            xyzPt, xyzForce, xyzMoment, true, loadType, null );
        
        doc.Regenerate();

        sMsg = string.Format( "Point load:F={0}, M={1} was created successfully",
            RstUtils.PointString( xyzForce ), RstUtils.PointString( xyzMoment ) );

        RstUtils.InfoMsg( sMsg );
        sMsg = null;

        // Get PointLoad object. 

        IList<Element> listLoads = null;
        listLoads = RstUtils.GetElementOfClass( doc, typeof( PointLoad ) );
        PointLoad ptLoad1 = null;
        
        sMsg = string.Format( "There are {0} point loads in the model",
            listLoads.Count );

        foreach( PointLoad ptLd in listLoads )
        {
          if( null == ptLoad1 )
            ptLoad1 = ptLd;
        
          // The following are all specific READ-ONLY properties:
          XYZ F = ptLd.Force;
          XYZ M = ptLd.Moment;
          XYZ p = ptLd.Point;
          
          string nameLoadCase = ptLd.LoadCaseName;
          
          //  in 8.1 - returns nothing - still the same in 2011

          string nameLoadCategory = ptLd.LoadCategoryName;
          string nameLoadNature = ptLd.LoadNatureName;
          
          sMsg += "\r\n  Id=" + ptLd.Id.IntegerValue.ToString()
            + ": F=" + RstUtils.PointString( F )
            + ", M=" + RstUtils.PointString( M )
            + ", Pt=" + RstUtils.PointString( p )
            + ", LoadCase=" + nameLoadCase
            + ", LoadCat=" + nameLoadCategory
            + ", LoadNature=" + nameLoadNature;

        }
        RstUtils.InfoMsg( sMsg );
        sMsg = null;

        // change the force of ptLoad1 by modifying paramter value

        if( null != ptLoad1 )
        {
          Parameter paramFx = ptLoad1.get_Parameter( BuiltInParameter.LOAD_FORCE_FX );
          Parameter paramFy = ptLoad1.get_Parameter( BuiltInParameter.LOAD_FORCE_FY );
          Parameter paramFz = ptLoad1.get_Parameter( BuiltInParameter.LOAD_FORCE_FZ );
          double dFx = paramFx.AsDouble();
          double dFy = paramFy.AsDouble();
          double dfz = paramFz.AsDouble();

          paramFx.Set( dKip );
          paramFy.Set( 2 * dKip );
          paramFz.Set( 3 * dKip );
          doc.Regenerate();

          sMsg = string.Format( "Point load's original force is {0},{1},{2}.\n",
              dFx, dFy, dfz );
        
          sMsg += string.Format( "It was just changed to {0},{1},{2}",
              dKip, 2 * dKip, 3 * dKip );
          
          RstUtils.InfoMsg( sMsg );
        }
        trans.Commit();

        return Result.Succeeded;
      }
      catch( Exception ex )
      {
        RstUtils.InfoMsg( ex.Message );
        trans.RollBack();
        return Result.Failed;
      }
    }
  }
  #endregion //RstLab1_2_LoadsOperation
}
