#region Header
// Revit API .NET Labs
//
// Copyright (C) 2007-2008 by Autodesk, Inc.
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
using System.Collections;
using System.Collections.Generic;
using Autodesk;
using Autodesk.Revit;
using Autodesk.Revit.Parameters;
using Autodesk.Revit.Structural;
using Autodesk.Revit.Utility;
using Autodesk.Revit.Elements;
using CmdResult = Autodesk.Revit.IExternalCommand.Result;
#endregion // Namespaces

namespace RstLabs
{
  #region Lab2_1_ListStructuralColumns
  /// <summary>
  /// Lab 2-1 - List all structural columns in the model.
  /// </summary>
  public class Lab2_1_ListStructuralColumns : IExternalCommand
  {
    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      try
      {
        Application app = commandData.Application;
        //
        // Get all Structural COLUMNS - we can use a generic utility.
        // In 8.1 we had to hard-code the category name which then works only in 
        // specific locale (EN or DE or IT etc.):
        // ElementSet columns = GetStandardFamilyInstancesForACategory( app, "Structural Columns" )
        // From 9.0 onwards, there is category enum, which works in ANY locale:
        //
        BuiltInCategory bicSc = BuiltInCategory.OST_StructuralColumns;
        List<Element> columns = RacUtils.GetAllStandardFamilyInstancesForACategory( app, bicSc );
        string sMsg = "There are " + columns.Count.ToString() + " structural column elements:";

        foreach( FamilyInstance fi in columns )
        {
          sMsg += "\r\n  " + RstUtils.StructuralElementDescription( fi );
        }
        RacUtils.InfoMsg( sMsg );
        return CmdResult.Succeeded;
      }
      catch( Exception ex )
      {
        message = ex.Message;
        return CmdResult.Failed;
      }
    }
  }
  #endregion // Lab2_1_ListStructuralColumns

  #region Lab2_2_ListStructuralFraming
  /// <summary>
  /// Lab 2-2 - List all structural framing elements in the model.
  /// </summary>
  public class Lab2_2_ListStructuralFraming : IExternalCommand
  {
    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;
      // get all structural framing elements - similar to Lab 2-1:
      BuiltInCategory bicSf = BuiltInCategory.OST_StructuralFraming;
      List<Element> frmEls = RacUtils.GetAllStandardFamilyInstancesForACategory( app, bicSf );
      string sMsg = "There are " + frmEls.Count.ToString() + " structural framing elements:";
      foreach( FamilyInstance fi in frmEls )
      {
        // INSTANCE_STRUCT_USAGE_TEXT_PARAM works only in 8.1 and not in 9.
        // so better use dedicated class property StructuralUsage which works in both. 
        sMsg += "\r\n  " + RstUtils.StructuralElementDescription( fi );
      }
      RacUtils.InfoMsg( sMsg );
      return CmdResult.Succeeded;
    }
  }
  #endregion // Lab2_2_ListStructuralFraming

  #region Lab2_3_Foundations
  /// <summary>
  /// Lab 2-3 - List all structural foundation elements in the model.
  /// </summary>
  public class Lab2_3_Foundations : IExternalCommand
  {
    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;
      //
      // Get all standard Structural FOUNDATION elements - again the same. 
      // Note that this:
      // a) excludes "Wall Foundation" System Type under "Structural Foundations" 
      // category in the Browser - these belong to the *Continuous Footing* 
      // system family, see later lab.
      // b) excludes "Foundation Slab" System Type under "Structural Foundations" 
      // category in the Browser - these are internally implemented as 
      // Revit *Floor* system family, see later lab
      //
      BuiltInCategory bic = BuiltInCategory.OST_StructuralFoundation;
      List<Element> struFnds = RacUtils.GetAllStandardFamilyInstancesForACategory( app, bic );
      string sMsg = "There are " + struFnds.Count.ToString()
        + " structural foundation (standard families only) elements :";

      foreach( FamilyInstance fi in struFnds )
      {
        sMsg += "\r\n  " + RstUtils.StructuralElementDescription( fi );
      }
      RacUtils.InfoMsg( sMsg );
      return CmdResult.Succeeded;
    }
  }
  #endregion // Lab2_3_Foundations

  #region Lab2_4_StandardFamilyInstances
  /// <summary>
  /// Lab 2-4 - List all standard family instances with an analytical model.
  /// This presents an alternative way from Labs 2-1, 2-2 and 2-3 to retrieve 
  /// all structural family instances.
  /// </summary>
  public class Lab2_4_StandardFamilyInstances : IExternalCommand
  {
    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;
      //
      // all the three previous labs retrieved standard family instances,
      // so we can alternatively get them by using something like this:
      //
      Filter filter = app.Create.Filter.NewTypeFilter( typeof( FamilyInstance ) );
      List<Element> instances = new List<Element>();
      app.ActiveDocument.get_Elements( filter, instances );
      string sMsg = "All structural family instances (generic check):";
      //string categoryName;
      //int i = 0;
      foreach( FamilyInstance fi in instances )
      {
        //
        // note that instead of looping through and checking for a 
        // non-null analytical model, we might also be able to use some 
        // other criterion that can be fed straight into the Revit API 
        // filtering mechanism, such as structural usage:
        //
        if( null != fi.AnalyticalModel )
        {
          sMsg += "\r\n  " + RstUtils.StructuralElementDescription( fi );
        }
      }
      RacUtils.InfoMsg( sMsg );
      return CmdResult.Succeeded;
    }
  }
  #endregion // Lab2_4_StandardFamilyInstances

  #region Lab2_5_StructuralSystemFamilyInstances
  /// <summary>
  /// Lab 2-5 - Retrieve structural system family instances: wall, floor, continuous footing.
  /// </summary>
  public class Lab2_5_StructuralSystemFamilyInstances : IExternalCommand
  {
    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;

      // get all structural walls elements using a dedicated helper 
      // that checks for all walls of structural usage:
      ElementSet struWalls = RstUtils.GetAllStructuralWalls( app );
      string sMsg = "There are " + struWalls.Size.ToString() 
        + " structural wall elements:";

      foreach( Wall w in struWalls )
      {
        // WALL_STRUCTURAL_USAGE_TEXT_PARAM works only in 8.1 and not from 9,
        // so better use dedicated class property StructuralUsage which works 
        // in both:
        //sMsg = sMsg + "\r\n  Id="  + w.Id.Value.ToString()
        //  + ", Type=" + w.WallType.Name
        //  + ", Struct.Usage=" + w.StructuralUsage.ToString()
        //  + ", Analytical Type=" + w.AnalyticalModel.GetType().Name;
        sMsg += "\r\n  " + RstUtils.StructuralElementDescription( w );
      }
      RacUtils.InfoMsg( sMsg );

      // get all structural floor elements using a dedicated helper 
      // that checks for all floors of structural usage.
      // note: from RS3 onwards, these include not only standard
      // floors, but also "Foundation Slab" instances from the 
      // "Structural Foundations" category:
      ElementSet struFloors = RstUtils.GetAllStructuralFloors( app );
      sMsg = "There are " + struFloors.Size + " structural floor elements:";

      foreach( Floor fl in struFloors )
      {
        //sMsg = sMsg + "\r\n  Id=" + fl.Id.Value.ToString()
        //  + ", Category=" + fl.Category.Name
        //  + ", Type=" + fl.FloorType.Name
        //  + ", Analytical Type=" + fl.AnalyticalModel.GetType().Name;
        sMsg += "\r\n  " + RstUtils.StructuralElementDescription( fl );
      }
      RacUtils.InfoMsg( sMsg );

      // get all structural continuous footing elements.
      // note: from RS3, these are "Wall Foundation" instances from 
      // the "Structural Foundations" category:
      ElementSet contFootings = RstUtils.GetAllStructuralContinuousFootings( app );
      sMsg = "There are " + contFootings.Size.ToString() 
        + " structural continuous footing (or wall foundation) elements:";

      foreach( ContFooting cf in contFootings )
      {
        sMsg += "\r\n  Id=" + cf.Id.Value.ToString()
          + " Type=" + cf.FootingType.Name
          + " Analytical Type=" + cf.AnalyticalModel.GetType().Name;
        sMsg += "\r\n  " + RstUtils.StructuralElementDescription( cf );
      }
      RacUtils.InfoMsg( sMsg );
      return CmdResult.Succeeded;
    }
  }
  #endregion // Lab2_5_StructuralSystemFamilyInstances
}
