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
  // List all STRUCTURAL elements in the model
  // -----------------------------------------
  // Structural COLUMNS and FRAMING elements
  public class Lab2_1 : IExternalCommand
  {
    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;
      // Get all Structural COLUMNS - we can use generic utility
      // In 8.1 we had to hard-code the category name which then works only in specific locale (EN or DE or IT etc)...
      // Dim columns As ElementSet = RacUtils.GetAllStandardFamilyInstancesForACategory(app, "Structural Columns")
      // ...but from 9.0 there is new category enum, so this should work in ANY locale:
      Category catStructuralColumns = app.ActiveDocument.Settings.Categories.get_Item(BuiltInCategory.OST_StructuralColumns);
      List<Element> columns = RacUtils.GetAllStandardFamilyInstancesForACategory(app, catStructuralColumns.Name); // todo: use category enum, not name
      string sMsg = "There are " + columns.Count.ToString() + " Structural COLUMNS elements:" + "\r\n";

      // " Struct.Usage=" & col.StructuralUsage.ToString & _ ' only beam and brace allow strctural usage in 2008
      foreach (FamilyInstance col in columns)
      {
        sMsg = sMsg + "  Id=" + col.Id.Value.ToString()
               + " Type=" + col.Symbol.Name
               + " Struct.Type=" + col.StructuralType.ToString()
               + " Analytical Type=" + col.AnalyticalModel.GetType().Name
               + "\r\n";
      }
      RacUtils.InfoMsg(sMsg);
      // Get all Structural FRAMING elements - again the same
      Category catStructuralFraming = app.ActiveDocument.Settings.Categories.get_Item(BuiltInCategory.OST_StructuralFraming);
      List<Element> frmEls = RacUtils.GetAllStandardFamilyInstancesForACategory(app, catStructuralFraming.Name);
      sMsg = "There are " + frmEls.Count.ToString() + " Structural FRAMING elements:" + "\r\n";

      foreach (FamilyInstance frmEl in frmEls)
      {
        // INSTANCE_STRUCT_USAGE_TEXT_PARAM works only in 8.1 and not in 9!...
        // sMsg += "  Id=" & frmEl.Id.Value.ToString & " Type=" & frmEl.Symbol.Name & _
        // " Struct.Usage=" & RacUtils.GetParamAsString(frmEl.Parameter(BuiltInParameter.INSTANCE_STRUCT_USAGE_TEXT_PARAM)) & _
        // " Analytical Type=" & frmEl.AnalyticalModel.GetType.Name & vbCrLf
        // ..so better use dedicated class' property StructuralUsage which works in both. Also check StructuralType
        sMsg = sMsg + "  Id=" + frmEl.Id.Value.ToString()
               + " Type=" + frmEl.Symbol.Name
               + " Struct.Usage=" + frmEl.StructuralUsage.ToString()
               + " Struct.Type=" + frmEl.StructuralType.ToString()
               + " Analytical Type=" + frmEl.AnalyticalModel.GetType().Name
               + "\r\n";
      }
      RacUtils.InfoMsg(sMsg);
      return CmdResult.Succeeded;
    }
  }

  // Structural FOUNDATION elements and any standard family in alternative way
  public class Lab2_2 : IExternalCommand
  {
    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;
      // Get all standard Structural FOUNDATION elements - again the same. Note that this:
      // a)  excludes "Wall Foundation" System Type under "Structural Foundations" category in the Browser - these belong to *Continuous Footing* system family, see next Lab
      // b)  excludes "Foundation Slab" System Type under "Structural Foundations" category in the Browser - these are internally implemented as Revit *Floor* system family, see next Lab
      Category catStructuralFoundation = app.ActiveDocument.Settings.Categories.get_Item(BuiltInCategory.OST_StructuralFoundation);
      List<Element> struFnds = RacUtils.GetAllStandardFamilyInstancesForACategory(app, catStructuralFoundation.Name);
      string sMsg = "There are " + struFnds.Count.ToString()
        + " Structural FOUNDATION (standard families only) elements :" + "\r\n";

      // " Struct.Usage=" & found.StructuralUsage.ToString & _only Beam and Brace support Structural Usage!
      foreach (FamilyInstance found in struFnds)
      {
        sMsg = sMsg + "  Id=" + found.Id.Value.ToString()
               + " Type=" + found.Symbol.Name
               + " Struct.Type=" + found.StructuralType.ToString()
               + " Analytical Type=" + found.AnalyticalModel.GetType().Name
               + "\r\n";
      }
      RacUtils.InfoMsg(sMsg);
      // NOTE: All of the previous 3 are *standard* Family Instances,
      // so we could alternatively get them by using something like:
      sMsg = "ALL Structural FAMILY INSTANCES (generic check):" + "\r\n";
      string categoryName;
      int i = 0;
      // The following commented code is for Revit 2008 and previous version. It works in Revit 2009 too.
      // However this method has a low performance.
      // There is a new feature named Element filter in Revit 2009 API, which can improve the performance.
      //IEnumerator iter = app.ActiveDocument.Elements;
      //while (iter.MoveNext())
      //{
      // Autodesk.Revit.Element elem = iter.Current as Autodesk.Revit .Element ;

      // The following code uses the element filter method that provided in Revit 2009 API
      Filter filterType = app.Create.Filter.NewTypeFilter(typeof(FamilyInstance));
      List<Element> listFamilyInstances = new List<Element>();
      app.ActiveDocument.get_Elements(filterType, listFamilyInstances);
      foreach (Element elem in listFamilyInstances)
      {
        if ((elem is FamilyInstance))
        {
          FamilyInstance fi = elem as FamilyInstance;
          AnalyticalModel anaMod = fi.AnalyticalModel;
          if (!(anaMod == null))
          {
            i++;
            categoryName = "?";
            try
            {
              categoryName = fi.Category.Name;
            }
            catch
            {
            }

            sMsg = sMsg
                   + i + ": Category=" + categoryName
                   + "  Struct.Type=" + fi.StructuralType.ToString()
                   + "  Id= " + fi.Id.Value.ToString()
                   + "\r\n";
          }
        }
      }
      RacUtils.InfoMsg(sMsg);

      return CmdResult.Succeeded;
    }
  }

  // Structural System Families: WALL, FLOOR, CONTINUOUS FOOTING
  public class Lab2_3 : IExternalCommand
  {
    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;
      // Get all Structural WALLS elements - dedicated helper that checks for all Walls of Structural usage
      ElementSet struWalls = RstUtils.GetAllStructuralWalls(app);
      string sMsg = "There are " + struWalls.Size.ToString() + " Structural WALLS elements:" + "\r\n";
      foreach (Wall w in struWalls)
      {
        // WALL_STRUCTURAL_USAGE_TEXT_PARAM works only in 8.1 and not from 9!...
        // sMsg += "  Id=" & w.Id.Value.ToString & " Type=" & w.WallType.Name & _
        // " Struct.Usage=" & RacUtils.GetParamAsString(w.Parameter(BuiltInParameter.WALL_STRUCTURAL_USAGE_TEXT_PARAM)) & _
        // " Analytical Type=" & w.AnalyticalModel.GetType.Name & vbCrLf
        // ..so better use dedicated class' property StructuralUsage which works in both
        sMsg = sMsg + "  Id="  + w.Id.Value.ToString()
              + " Type=" + w.WallType.Name
              + " Struct.Usage=" + w.StructuralUsage.ToString()
              + " Analytical Type=" + w.AnalyticalModel.GetType().Name
              + "\r\n";
      }
      RacUtils.InfoMsg(sMsg);
      // Get all Structural FLOOR elements - dedicated helper that checks for all Floors of Structural usage
      // NOTE: From RS3, these include not only standard Floors, but also "Foundation Slab" instances from "Structural Foundations" category
      ElementSet struFloors = RstUtils.GetAllStructuralFloors(app);
      sMsg = "There are "
            + struFloors.Size + " Structural FLOOR elements:" + "\r\n";

      foreach (Floor fl in struFloors)
      {
        sMsg = sMsg + "  Id=" + fl.Id.Value.ToString()
               + "  Category=" + fl.Category.Name
               + "  Type=" + fl.FloorType.Name
               + "  Analytical Type=" + fl.AnalyticalModel.GetType().Name
               + "\r\n";
      }
      RacUtils.InfoMsg(sMsg);
      // Get all Structural CONTINUOUS FOOTING elements - dedicated helper
      // NOTE: From RS3, these are "Wall Foundation" instances from "Structural Foundations" category
      ElementSet contFootings = RstUtils.GetAllStructuralContinuousFootings(app);
      sMsg = "There are " + contFootings.Size.ToString()
        + " Structural CONTINUOUS FOOTING (or Wall Foundations) elements:" + "\r\n";

      foreach (ContFooting cf in contFootings)
      {
        sMsg = sMsg + "  Id=" + cf.Id.Value.ToString()
               + " Type=" + cf.FootingType.Name
               + " Analytical Type=" + cf.AnalyticalModel.GetType().Name
               + "\r\n";
      }
      RacUtils.InfoMsg(sMsg);
      return CmdResult.Succeeded;
    }
  }
}