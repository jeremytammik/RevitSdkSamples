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
using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
#endregion // Namespaces

namespace RstLabs
{
  #region Lab2_1_ListStructuralColumns
  /// <summary>
  /// Lab 2-1 - List all structural columns in the model.
  /// </summary>
  [Transaction( TransactionMode.Automatic )]
  [Regeneration( RegenerationOption.Manual )]
  public class Lab2_1_ListStructuralColumns : IExternalCommand
  {
    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      try
      {
        UIApplication app = commandData.Application;
        Document doc = app.ActiveUIDocument.Document;
        //
        // Get all Structural COLUMNS - we can use a generic utility.
        // In 8.1 we had to hard-code the category name which then works only in
        // specific locale (EN or DE or IT etc.):
        // ElementSet columns = GetStandardFamilyInstancesForACategory( app, "Structural Columns" )
        // From 9.0 onwards, there is category enum, which works in ANY locale:
        //
        BuiltInCategory bicSc = BuiltInCategory.OST_StructuralColumns;
        IList<Element> columns = RacUtils.GetAllStandardFamilyInstancesForACategory( doc, bicSc );
        string msg = "There are " + columns.Count.ToString() + " structural column elements:";

        foreach( FamilyInstance fi in columns )
        {
          msg += "\r\n  " + RstUtils.StructuralElementDescription( fi );
        }
        RacUtils.InfoMsg( msg );
        return Result.Succeeded;
      }
      catch( Exception ex )
      {
        message = ex.Message;
        return Result.Failed;
      }
    }
  }
  #endregion // Lab2_1_ListStructuralColumns

  #region Lab2_2_ListStructuralFraming
  /// <summary>
  /// Lab 2-2 - List all structural framing elements in the model.
  /// </summary>
  [Transaction( TransactionMode.Automatic )]
  [Regeneration( RegenerationOption.Manual )]
  public class Lab2_2_ListStructuralFraming : IExternalCommand
  {
    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      UIApplication app = commandData.Application;
      Document doc = app.ActiveUIDocument.Document;

      // get all structural framing elements - similar to Lab 2-1:

      BuiltInCategory bicSf = BuiltInCategory.OST_StructuralFraming;
      IList<Element> frmEls = RacUtils.GetAllStandardFamilyInstancesForACategory( doc, bicSf );

      string msg = "There are " + frmEls.Count.ToString() + " structural framing elements:";

      foreach( FamilyInstance fi in frmEls )
      {
        // INSTANCE_STRUCT_USAGE_TEXT_PARAM works only in 8.1 and not in 9. From 2010, it works well.
        // so better use dedicated class property StructuralUsage which works in both.

        msg += "\r\n  " + RstUtils.StructuralElementDescription( fi );
      }
      RacUtils.InfoMsg( msg );
      return Result.Succeeded;
    }
  }
  #endregion // Lab2_2_ListStructuralFraming

  #region Lab2_3_Foundations
  /// <summary>
  /// Lab 2-3 - List all structural foundation elements in the model.
  /// </summary>
  [Transaction( TransactionMode.Automatic )]
  [Regeneration( RegenerationOption.Manual )]
  public class Lab2_3_Foundations : IExternalCommand
  {
    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      UIApplication app = commandData.Application;
      Document doc = app.ActiveUIDocument.Document;
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
      IList<Element> struFnds = RacUtils.GetAllStandardFamilyInstancesForACategory( doc, bic );

      string msg = "There are " + struFnds.Count.ToString()
        + " structural foundation (standard families only) elements :";

      foreach( Element elem in struFnds )
      {
        if( elem is FamilyInstance )
        {
          FamilyInstance fi = elem as FamilyInstance;
          msg += "\r\n  " + RstUtils.StructuralElementDescription( fi );
        }
      }
      RacUtils.InfoMsg( msg );
      return Result.Succeeded;
    }
  }
  #endregion // Lab2_3_Foundations

  #region Lab2_4_StandardFamilyInstances
  /// <summary>
  /// Lab 2-4 - List all standard family instances with an analytical model.
  /// This presents an alternative way from Labs 2-1, 2-2 and 2-3 to retrieve
  /// all structural family instances.
  /// </summary>
  [Transaction( TransactionMode.Automatic )]
  [Regeneration( RegenerationOption.Manual )]
  public class Lab2_4_StandardFamilyInstances : IExternalCommand
  {
    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      UIApplication app = commandData.Application;
      Document doc = app.ActiveUIDocument.Document;

      // This commented-out code works for Revit 2010:
      //
      //Filter filter = app.Create.Filter.NewTypeFilter( typeof( FamilyInstance ) );
      //List<Element> instances = new List<Element>();
      //app.ActiveDocument.get_Elements( filter, instances );

      // From 2011, the way to iterate elements is changed. 
      // FilteredElementCollector is used to do this:

      IList<Element> instances = null;
      FilteredElementCollector collector = new FilteredElementCollector( doc );
      instances = collector.OfClass( typeof( FamilyInstance ) ).ToElements();
      string msg = "All structural family instances (generic check):";

      foreach( FamilyInstance fi in instances )
      {
        //
        // Note that instead of looping through and checking for a
        // non-null analytical model, we might also be able to use some
        // other criterion that can be fed straight into the Revit API
        // filtering mechanism, such as structural usage:
        //
        if( null != fi.GetAnalyticalModel() )
        {
          msg += "\r\n  " + RstUtils.StructuralElementDescription( fi );
        }
      }
      RacUtils.InfoMsg( msg );
      return Result.Succeeded;
    }
  }
  #endregion // Lab2_4_StandardFamilyInstances

  #region Lab2_5_StructuralSystemFamilyInstances
  /// <summary>
  /// Lab 2-5 - Retrieve structural system family instances: wall, floor, continuous footing.
  /// </summary>
  [Transaction( TransactionMode.Automatic )]
  [Regeneration( RegenerationOption.Manual )]
  public class Lab2_5_StructuralSystemFamilyInstances : IExternalCommand
  {
    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      UIApplication app = commandData.Application;
      Document doc = app.ActiveUIDocument.Document;

      // get all structural walls elements using a dedicated helper
      // that checks for all walls of structural usage:

      List<Element> struWalls = RstUtils.GetAllStructuralWalls( doc );
      string msg = "There are " + struWalls.Count.ToString()
        + " structural wall elements:";

      foreach( Wall w in struWalls )
      {
        // WALL_STRUCTURAL_USAGE_TEXT_PARAM works only in 8.1 and not from 9,
        // so better use dedicated class property StructuralUsage which works
        // in both:
        //msg = msg + "\r\n  Id="  + w.Id.IntegerValue.ToString()
        //  + ", Type=" + w.WallType.Name
        //  + ", Struct.Usage=" + w.StructuralUsage.ToString()
        //  + ", Analytical Type=" + w.AnalyticalModel.GetType().Name;

        msg += "\r\n  " + RstUtils.StructuralElementDescription( w );
      }
      RacUtils.InfoMsg( msg );

      // get all structural floor elements using a dedicated helper
      // that checks for all floors of structural usage.
      // note: from RS3 onwards, these include not only standard
      // floors, but also "Foundation Slab" instances from the
      // "Structural Foundations" category:

      List<Element> struFloors = RstUtils.GetAllStructuralFloors( doc );
      msg = "There are " + struFloors.Count + " structural floor elements:";

      foreach( Floor fl in struFloors )
      {
        //msg = msg + "\r\n  Id=" + fl.Id.IntegerValue.ToString()
        //  + ", Category=" + fl.Category.Name
        //  + ", Type=" + fl.FloorType.Name
        //  + ", Analytical Type=" + fl.AnalyticalModel.GetType().Name;
        msg += "\r\n  " + RstUtils.StructuralElementDescription( fl );
      }
      RacUtils.InfoMsg( msg );

      // get all structural continuous footing elements.
      // note: from RS3, these are "Wall Foundation" instances from
      // the "Structural Foundations" category:

      List<Element> contFootings = RstUtils.GetAllStructuralContinuousFootings( doc );
      msg = "There are " + contFootings.Count.ToString()
        + " structural continuous footing (or wall foundation) elements:";

      foreach( ContFooting cf in contFootings )
      {
        msg += "\r\n  Id=" + cf.Id.IntegerValue.ToString()
          + " Type=" + cf.FootingType.Name
          + "\r\n  " + RstUtils.StructuralElementDescription( cf );
      }
      RacUtils.InfoMsg( msg );
      return Result.Succeeded;
    }
  }
  #endregion // Lab2_5_StructuralSystemFamilyInstances
}
