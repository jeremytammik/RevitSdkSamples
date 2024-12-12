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
//using System.Windows.Forms;
using Autodesk;
using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Symbols;
using Autodesk.Revit.Parameters;
using Autodesk.Revit.Structural;
using Autodesk.Revit.Utility;
#endregion // Namespaces

namespace RstLabs
{
  // Revit Structure utilities
  public class RstUtils
  {
    public static ElementSet GetAllLoadNatures(Application revitApp)
    {
      //The following commented code is for Revit 2008 and previous version. It works in Revit 2009 too. However this method has a low performance.
      //There is a new feature named Element filter in Revit 2009 API, which can improve the performance.
      /*
      ElementSet natures = revitApp.Create.NewElementSet();
      IEnumerator iter = revitApp.ActiveDocument.Elements;
      while (iter.MoveNext())
      {
        Autodesk.Revit.Element elem = iter.Current as Autodesk.Revit.Element;
        if (elem is LoadNature)
        {
          natures.Insert(elem);
        }
      }
      return natures;
      */

      //getting all LoadNatures via element filter that provided in Revit 2009 API. This is quicker and simpler than the previous method.
      List<Element> listLoadNatures = new List<Element>();
      Filter filterType = revitApp.Create.Filter.NewTypeFilter(typeof(LoadNature));
      revitApp.ActiveDocument.get_Elements(filterType, listLoadNatures);
      return RacUtils.ConvertElementListToSet(revitApp, listLoadNatures);
    }

    public static ElementSet GetAllLoadCases(Application revitApp)
    {
      //The following commented code is for Revit 2008 and previous version. It works in Revit 2009 too. However this method has a low performance.
      //There is a new feature named Element filter in Revit 2009 API, which can improve the performance.
      /*
      ElementSet cases = revitApp.Create.NewElementSet();
      IEnumerator iter = revitApp.ActiveDocument.Elements;
      while (iter.MoveNext())
      {
        Autodesk.Revit.Element elem = iter.Current as Autodesk.Revit.Element;
        if (elem is LoadCase)
        {
          cases.Insert(elem);
        }
      }
      return cases;
      */

      //getting all LoadCases via element filter that provided in Revit 2009 API. This is quicker and simpler.
      List<Element> listLoadCases = new List<Element>();
      Filter filterType = revitApp.Create.Filter.NewTypeFilter(typeof(LoadCase));
      revitApp.ActiveDocument.get_Elements(filterType, listLoadCases);
      return RacUtils.ConvertElementListToSet(revitApp, listLoadCases);
    }

    public static ElementSet GetAllLoadCombinations(Application revitApp)
    {
      //The following commented code is for Revit 2008 and previous version. It works in Revit 2009 too. However this method has a low performance.
      //There is a new feature named Element filter in Revit 2009 API, which can improve the performance.
      /*
      ElementSet combinations = revitApp.Create.NewElementSet();
      IEnumerator iter = revitApp.ActiveDocument.Elements;
      while (iter.MoveNext())
      {
        Autodesk.Revit.Element elem = iter.Current as Autodesk.Revit.Element;
        if (elem is LoadCombination)
        {
          combinations.Insert(elem);
        }
      }
      return combinations;
      */

      //The following code uses the element filter method that provided in Revit 2009 API
      List<Element> listLoadCombinations = new List<Element>();
      Filter filterType = revitApp.Create.Filter.NewTypeFilter(typeof(LoadCombination));
      revitApp.ActiveDocument.get_Elements(filterType, listLoadCombinations);
      return RacUtils.ConvertElementListToSet(revitApp, listLoadCombinations);
    }

    public static ElementSet GetAllLoadUsages(Application revitApp)
    {
      //The following commented code is for Revit 2008 and previous version. It works in Revit 2009 too. However this method has a low performance.
      //There is a new feature named Element filter in Revit 2009 API, which can improve the performance.
      /*
      ElementSet usages = revitApp.Create.NewElementSet();
      IEnumerator iter = revitApp.ActiveDocument.Elements;
      while (iter.MoveNext())
      {
        Autodesk.Revit.Element elem = iter.Current as Autodesk.Revit.Element;
        if (elem is LoadUsage)
        {
          usages.Insert(elem);
        }
      }
      return usages;
       */

      //The following code uses the element filter method that provided in Revit 2009 API
      List<Element> listLoadUsages = new List<Element>();
      Filter filterType = revitApp.Create.Filter.NewTypeFilter(typeof(LoadUsage));
      revitApp.ActiveDocument.get_Elements(filterType, listLoadUsages);
      return RacUtils.ConvertElementListToSet(revitApp, listLoadUsages);

    }

    public static ElementSet GetAllLoads(Application revitApp)
    {
      //The following commented code is for Revit 2008 and previous version. It works in Revit 2009 too. However this method has a low performance.
      //There is a new feature named Element filter in Revit 2009 API, which can improve the performance.
      /*
      ElementSet loads = revitApp.Create.NewElementSet();
      IEnumerator iter = revitApp.ActiveDocument.Elements;
      while (iter.MoveNext())
      {
        Autodesk.Revit.Element elem = iter.Current as Autodesk.Revit.Element;
        if (elem is LoadBase)
        {
          loads.Insert(elem);
        }
      }
      return loads;
      */

      //The following code uses the element filter method that provided in Revit 2009 API
      List<Element> listLoadBases = new List<Element>();
      Filter filterType = revitApp.Create.Filter.NewTypeFilter(typeof(LoadBase), true);
      revitApp.ActiveDocument.get_Elements(filterType, listLoadBases);
      return RacUtils.ConvertElementListToSet(revitApp, listLoadBases);
    }

    //  More efficient if we loop only once and sort at the time
    public static void GetAllSpecificLoads(Application revitApp, ref ElementSet pointLoads, ref ElementSet lineLoads, ref ElementSet areaLoads)
    {
      //The following commented code is for Revit 2008 and previous version. It works in Revit 2009 too. However this method has a low performance.
      //There is a new feature named Element filter in Revit 2009 API, which can improve the performance.
      /*
      IEnumerator iter = revitApp.ActiveDocument.Elements;
      while (iter.MoveNext())
      {
        Autodesk.Revit.Element elem = iter.Current as Autodesk.Revit.Element;
        if (elem is PointLoad)
        {
          pointLoads.Insert(elem);
        }
        else if (elem is LineLoad)
        {
          lineLoads.Insert(elem);
        }
        else if (elem is AreaLoad)
        {
          areaLoads.Insert(elem);
        }
      }
      */

      //The following code uses the element filter method that provided in Revit 2009 API
      Filter filterTypeLoad = revitApp.Create.Filter.NewTypeFilter(typeof(LoadBase), true);

      List<Element> listLoad = new List<Element>();
      revitApp.ActiveDocument.get_Elements(filterTypeLoad, listLoad);
      foreach (Element elem in listLoad)
      {
        if (elem is PointLoad)
        {
          pointLoads.Insert(elem);
        }
        else if (elem is LineLoad)
        {
          lineLoads.Insert(elem);
        }
        else if (elem is AreaLoad)
        {
          areaLoads.Insert(elem);
        }
      }
    }

    public static ElementSet GetAllLoadSymbols(Application revitApp)
    {
      //The following commented code is for Revit 2008 and previous version. It works in Revit 2009 too. However this method has a low performance.
      //There is a new feature named Element filter in Revit 2009 API, which can improve the performance.
      /*
      ElementSet symbols = revitApp.Create.NewElementSet();
      // Revit 2009 added new classes to represent Load symbols, such as LoadTypeBase, PointLoadType,LineLoadType and AreaLoadType.
      // to find out all load types, we can compare the class instead of comparing Category.

      IEnumerator iter = revitApp.ActiveDocument.Elements;
      while ((iter.MoveNext()))
      {
        LoadTypeBase loadType = iter.Current as LoadTypeBase;
        if (loadType is LoadTypeBase)
        {
          symbols.Insert(loadType);
        }
      }
      return symbols;
      */

      //
      //The following code uses the element filter method that provided in Revit 2009 API
      //
      Filter filterLoadSymbol = revitApp.Create.Filter.NewTypeFilter(typeof(LoadTypeBase), true);

      List<Element> listLoadSymbol = new List<Element>();
      revitApp.ActiveDocument.get_Elements(filterLoadSymbol, listLoadSymbol);
      return RacUtils.ConvertElementListToSet(revitApp, listLoadSymbol);
    }

    public static ElementSet GetPointLoadSymbols(Application revitApp)
    {
      //The following commented code is for Revit 2008 and previous version. It works in Revit 2009 too. However this method has a low performance.
      //There is a new feature named Element filter in Revit 2009 API, which can improve the performance.
      /*
      ElementSet symbols = revitApp.Create.NewElementSet();

      ElementIterator iter = revitApp.ActiveDocument.get_Elements(typeof(PointLoadType));
      while ((iter.MoveNext()))
      {
        Element elem = iter.Current as Element;
        if (elem is PointLoadType)
        {
          symbols.Insert(elem);
        }
      }
      return symbols;
      */

      //
      //The following code uses the element filter method that provided in Revit 2009 API
      //
      Filter filterPointLoadSymbol = revitApp.Create.Filter.NewTypeFilter(typeof(PointLoadType));
      List<Element> listPointLoadSymbol = new List<Element>();
      revitApp.ActiveDocument.get_Elements(filterPointLoadSymbol, listPointLoadSymbol);
      return RacUtils.ConvertElementListToSet(revitApp, listPointLoadSymbol);
    }

    // Helper to get all STRUCTURAL Walls
    public static ElementSet GetAllStructuralWalls(Application revitApp)
    {
      //The following commented code is for Revit 2008 and previous version. It works in Revit 2009 too. However this method has a low performance.
      //There is a new feature named Element filter in Revit 2009 API, which can improve the performance.

      //ElementSet elems = revitApp.Create.NewElementSet();
      //IEnumerator iter = revitApp.ActiveDocument.Elements;
      //while ((iter.MoveNext()))
      //{
      //  Autodesk.Revit.Element elem = iter.Current as Autodesk.Revit.Element;

      //
      //The following code uses the element filter method that provided in Revit 2009 API
      //
      ElementSet elems = revitApp.Create.NewElementSet();
      Filter filterWall = revitApp.Create.Filter.NewTypeFilter(typeof(Wall));

      List<Element> listWall = new List<Element>();
      revitApp.ActiveDocument.get_Elements(filterWall, listWall);
      foreach (Element elem in listWall)
      {
        // For Wall (one of the Host objects), there is a specific class!
        if (elem is Wall)
        {
          Wall w = elem as Wall;

          //' We could check if the Wall is anything but Non-bearing in one of the two following ways:...

          // this works only in 8.1, not in 9/9.1!
          //If Not w.Parameter(BuiltInParameter.WALL_STRUCTURAL_USAGE_TEXT_PARAM).AsString.Equals("Non-bearing") Then
          // '...
          //End If
          //'OR better

          // this works both in 8.1 and 9/9.1!
          //If Not w.Parameter(BuiltInParameter.WALL_STRUCTURAL_USAGE_PARAM).AsInteger = 0 Then
          // '...
          //End If

          //... but it's more generic and precise to make sure that Analytical Model exists (in theory, one
          // can set the wall to bearing and still uncheck Analytical)
          AnalyticalModel anaMod;
          try
          {
            anaMod = w.AnalyticalModel;
            if ((anaMod != null))
            {
              elems.Insert(elem);
            }
          }
          catch
          {
          }
        }
      }
      return elems;
    }

    // Helper to get all STRUCTURAL Floors
    public static ElementSet GetAllStructuralFloors(Application revitApp)
    {
      ElementSet elems = revitApp.Create.NewElementSet();
      //The following commented code is for Revit 2008 and previous version. It works in Revit 2009 too. However this method has a low performance.
      //There is a new feature named Element filter in Revit 2009 API, which can improve the performance.
      //IEnumerator iter = revitApp.ActiveDocument.Elements;
      //while ((iter.MoveNext()))
      //  elem = iter.Current as Element

      //
      //The following code uses the element filter method that provided in Revit 2009 API
      //
      Filter filterFloors = revitApp.Create.Filter.NewTypeFilter(typeof(Floor));

      List<Element> listFloor = new List<Element>();
      revitApp.ActiveDocument.get_Elements(filterFloors, listFloor);
      foreach (Element elem in listFloor)
      {
        if (elem is Floor)
        {
          Floor f = elem as Floor;
          AnalyticalModel anaMod;
          try
          {
            anaMod = f.AnalyticalModel;
            if ((anaMod != null))
            {
              // For floors, looks like we need to have additional check: for non-Structural Floors
              // anaMod is NOT Nothing, but it IS empty!
              AnalyticalModelFloor floorAnaMod = anaMod as AnalyticalModelFloor;
              if (floorAnaMod.Curves.Size > 0)
              {
                elems.Insert(elem);
              }
            }
          }
          catch
          {
          }
        }
      }
      return elems;
    }

    //  Helper to get all STRUCTURAL ContinuousFootings
    public static ElementSet GetAllStructuralContinuousFootings(Application revitApp)
    {
      ElementSet elems = revitApp.Create.NewElementSet();
      //The following commented code is for Revit 2008 and previous version. It works in Revit 2009 too. However this method has a low performance.
      //There is a new feature named Element filter in Revit 2009 API, which can improve the performance.
      //IEnumerator iter = revitApp.ActiveDocument.Elements;
      //while (iter.MoveNext())

      //
      //The following code uses the element filter method that provided in Revit 2009 API
      //
      Filter ContFooting = revitApp.Create.Filter.NewTypeFilter(typeof(ContFooting));
      List<Element> listContFooting = new List<Element>();
      revitApp.ActiveDocument.get_Elements(ContFooting, listContFooting);
      foreach (Element elem in listContFooting)
      {
        if (elem is ContFooting)
        {
          ContFooting cf = elem as ContFooting;
          AnalyticalModel anaMod;
          try
          {
            anaMod = cf.AnalyticalModel;
            if (!(anaMod == null))
            {
              elems.Insert(elem);
            }
          }
          catch
          {
          }
        }
      }
      return elems;
    }
  }
}
