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
using WinForms = System.Windows.Forms;
using Autodesk;
using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Geometry;
using Autodesk.Revit.Parameters;
using Autodesk.Revit.Structural;
using Autodesk.Revit.Utility;
using CmdResult = Autodesk.Revit.IExternalCommand.Result;
#endregion // Namespaces

namespace RstLabs
{
  public class Lab1_1 : IExternalCommand
  {
    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;
      // Load NATURES
      List<Autodesk.Revit.Element> ldNatures = RstUtils.GetAllLoadNatures(app);
      string sMsg = "There are " + ldNatures.Count.ToString() + " LoadNature objects in the model:";
      foreach (LoadNature nature in ldNatures)
      {
        sMsg += "\r\n  " + nature.Name + ", Id=" + nature.Id.Value.ToString();
      }
      RacUtils.InfoMsg(sMsg);
      // Load CASES
      List<Autodesk.Revit.Element> ldCases = RstUtils.GetAllLoadCases(app);
      sMsg = "There are " + ldCases.Count.ToString() + " LoadCase objects in the model:" + "\r\n";
      foreach (LoadCase ldCase in ldCases)
      {
        string catName = "";
        try
        {
          // . Category NOT implemented in API for Loadcase class :-( ,
          // though the category enums are there (OST_LoadCasesXxx)
          // todo: check this is true in 2009
          catName = ldCase.Category.Name;
        }
        catch
        {
        }
        sMsg += "  " + ldCase.Name + ", Id=" + ldCase.Id.Value.ToString()
             + ", Category=" + catName + "\r\n";
        // there seems to be no way to get LoadCase's LoadNature in API
      }
      RacUtils.InfoMsg(sMsg);

      // Load USAGES (optionally used for Combinations)
      List<Autodesk.Revit.Element> ldUsages = RstUtils.GetAllLoadUsages(app);
      sMsg = "There are " + ldUsages.Count.ToString() + " LoadUsage objects in the model:";

      foreach (LoadUsage ldUsage in ldUsages)
      {
        sMsg += "\r\n  " + ldUsage.Name + ", Id=" + ldUsage.Id.Value.ToString();
      }
      RacUtils.InfoMsg(sMsg);

      // Load COMBINATIONS
      List<Autodesk.Revit.Element> combs = RstUtils.GetAllLoadCombinations(app);
      sMsg = "There are " + combs.Count.ToString() + " LoadCombination objects in the model:" + "\r\n";
      foreach (LoadCombination comb in combs)
      {
        // Combinaton properties
        string usageNames = "[NONE]";
        for (int i = 0; i < comb.NumberOfUsages; i++)
        {
          if ((i == 0))
          {
            usageNames = comb.get_UsageName(i);
          }
          else
          {
            usageNames = (usageNames + (";" + comb.get_UsageName(i)));
          }
        }
        sMsg += "\r\n  " + comb.Name
             + ",  Id=" + comb.Id.Value.ToString()
             + "  Type=" + comb.CombinationType
             + "  TypeIndex=" + comb.CombinationTypeIndex
             + "  State=" + comb.CombinationState
             + "  StateIndex=" + comb.CombinationStateIndex
             + "  Usages=" + usageNames;

        sMsg += "\r\n  Number of components = " + comb.NumberOfComponents + ":";
        // Loop all components' propeties
        for( int i = 0; i < comb.NumberOfComponents; ++i )
        {
          sMsg += "\r\n  Comp.name=" + comb.get_CombinationCaseName( i )
                + "  Comp.nature=" + comb.get_CombinationNatureName( i )
                + "  Factor=" + comb.get_Factor( i );
        }
      }
      RacUtils.InfoMsg(sMsg);
      // NOTE: Unlike RS2, from RS3 most of this objects are also create-able, see:
      // Autodesk.Revit.Creation.Document.NewLoad*** methods. To call them, use something like:
      // app.ActiveDocument.Create.NewLoadCase(...)
      // app.ActiveDocument.Create.NewLoadCombination(...)
      // app.ActiveDocument.Create.NewLoadNature(...)
      // app.ActiveDocument.Create.NewLoadUsage(...)
      return CmdResult.Succeeded;
    }
  }

  // List all Loads
  public class Lab1_2 : IExternalCommand
  {

    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;
      List<Autodesk.Revit.Element> loads = RstUtils.GetAllLoads(app);
      string sMsg = "There are " + loads.Count.ToString() + " Load objects in the model:"
                   + "\r\n";
      string sHost;

      foreach (LoadBase load in loads)
      {
        try
        {
          sHost = ", Host Id=" + load.HostElement.Id.Value.ToString();
        }
        catch
        {
          sHost = "";
        }
        sMsg += "\r\n  " + load.GetType().Name
              + ", Id=" + load.Id.Value.ToString()
              + sHost;
      }
      RacUtils.InfoMsg(sMsg);
      // We could loop the above collection, but better code another helper for Specific Loads directly
      ElementSet pointLoads = app.Create.NewElementSet();
      ElementSet lineLoads = app.Create.NewElementSet();
      ElementSet areaLoads = app.Create.NewElementSet();
      RstUtils.GetAllSpecificLoads(app, ref pointLoads, ref lineLoads, ref areaLoads);
      // POINT
      sMsg = pointLoads.Size + " POINT Loads:";

      foreach (PointLoad ptLd in pointLoads)
      {
        // The following are all specific READ-ONLY properties:
        XYZ F = ptLd.Force;
        XYZ M = ptLd.Moment;
        XYZ pt = ptLd.Point;
        string nameLoadCase = ptLd.LoadCaseName;
        string nameLoadCategory = ptLd.LoadCategoryName;
        // current bug in 8.1 - returns nothing
        string nameLoadNature = ptLd.LoadNatureName;
        sMsg += "\r\nId=" + ptLd.Id.Value.ToString()
              + ": F=" + RacUtils.PointString( F )
              + " M=" + RacUtils.PointString( M )
              + " Pt=" + RacUtils.PointString( pt )
              + " LoadCase=" + nameLoadCase
              + " LoadCat=" + nameLoadCategory
              + " LoadNature=" + nameLoadNature;
      }
      RacUtils.InfoMsg(sMsg);
      // LINE
      sMsg = lineLoads.Size + " LINE Loads:";

      foreach (LineLoad lnLd in lineLoads)
      {
        // The following are *some* READ-ONLY properties:
        XYZ F1 = lnLd.Force1;
        XYZ F2 = lnLd.Force2;
        // can do similar for Moment1, Moment2...
        XYZ ptStart = lnLd.StartPoint;
        // similar for EndPoint
        // LoadxxxName same as for PointLoad (implemented in the base class)
        sMsg += "\r\nId=" + lnLd.Id.Value.ToString()
              + ": F1= " + RacUtils.PointString( F1 )
               + ": F2= " + RacUtils.PointString( F2 )
               + " Start= " + RacUtils.PointString( ptStart )
               + " Uniform=" + lnLd.UniformLoad
               + " Projected=" + lnLd.ProjectedLoad;
      }
      RacUtils.InfoMsg(sMsg);
      // AREA
      sMsg = areaLoads.Size.ToString() + " AREA Loads:" + "\r\n";

      foreach (AreaLoad arLd in areaLoads)
      {
        // The following are *some* READ-ONLY properties:
        XYZ F1 = arLd.Force1;
        int numLoops = arLd.NumLoops;
        string s2 = "Num.of Loops=" + numLoops.ToString()
               + " Num.of Curves=" + arLd.get_NumCurves(0).ToString();

        // if more than single Loop
        for (int i = 1; i < numLoops; i++)
        {
          s2 = (s2 + ("," + arLd.get_NumCurves(i)));
        }
        sMsg += "Id=" + arLd.Id.Value.ToString()
              + ": F1= " + F1.X + "," + F1.Y + "," + F1.Z
              + s2 + "\r\n";
        // For curves geometry, see later commands...
      }
      RacUtils.InfoMsg(sMsg);
      return CmdResult.Succeeded;
    }
  }

  // More detailed info for *selected* loads
  public class Lab1_3 : IExternalCommand
  {
    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;

      ElementSetIterator iter = app.ActiveDocument.Selection.Elements.ForwardIterator();
      while (iter.MoveNext())
      {
        Autodesk.Revit.Element elem = iter.Current as Autodesk.Revit.Element;
        if ((elem is PointLoad))
        {
          ListAndModifyPointLoad(((PointLoad)(elem)));
        }
        else if ((elem is LineLoad))
        {
          ListAndModifyLineLoad(((LineLoad)(elem)), app);
        }
        else if ((elem is AreaLoad))
        {
          ListAndModifyAreaLoad(((AreaLoad)(elem)), app);
        }
      }
      return CmdResult.Succeeded;
    }
    public void ListAndModifyPointLoad(PointLoad ptLd)
    {
      // One can access some parameters as read-only class properties/methods,
      // but to be able to change them and to access all, we need to go via Parameters:
      string sMsg = "POINT Load Id=" + ptLd.Id.Value.ToString() + "\r\n";
      try
      {
        // List 2 Params via their display name (locale-dependent!)
        sMsg += "  Is Reaction (via Display Name)= "
          + RacUtils.GetParamAsString(RacUtils.GetElemParam(ptLd, "Is Reaction")) + "\r\n";
        sMsg += "  Load Case (via Display Name)= "
          + RacUtils.GetParamAsString(RacUtils.GetElemParam(ptLd, "Load Case")) + "\r\n";

        // Better use BUILT_IN_PARAMS, but have to guess-and-try-and-error to find correct ones!
        // The following 2 give the same as above:
        sMsg += "  LOAD_IS_REACTION BuiltInParameter= "
          + RacUtils.GetParamAsString(ptLd.get_Parameter(BuiltInParameter.LOAD_IS_REACTION))
          + "\r\n";
        sMsg += "  LOAD_CASE_ID BuiltInParameter= "
          + RacUtils.GetParamAsString(ptLd.get_Parameter(BuiltInParameter.LOAD_CASE_ID))
          + "\r\n";

        // The following Display one hasn't got one-to-one corresponding Built-in, but basically the same:
        sMsg += "  Orient to (via Display Name)= "
          + RacUtils.GetParamAsString(RacUtils.GetElemParam(ptLd, "Orient to")) + "\r\n";
        sMsg += "  LOAD_USE_LOCAL_COORDINATE_SYSTEM BuiltInParameter= "
          + RacUtils.GetParamAsString(ptLd.get_Parameter(BuiltInParameter.LOAD_USE_LOCAL_COORDINATE_SYSTEM))
          + "\r\n";
        try
        {
          sMsg += "  LOAD_USE_LOCAL_COORDINATE_SYSTEM_HOSTED BuiltInParameter= "
            + RacUtils.GetParamAsString(ptLd.get_Parameter(BuiltInParameter.LOAD_USE_LOCAL_COORDINATE_SYSTEM_HOSTED))
            + "\r\n";
        }
        catch
        {
        }

        // Scale Fz by factor 2
        Parameter paramFz = RacUtils.GetElemParam(ptLd, "Fz");
        double Fz_old = paramFz.AsDouble();
        paramFz.Set((double)(2.0 * Fz_old));
        sMsg +=  "  Fz:  OLD=" + Fz_old.ToString() + " NEW=" + paramFz.AsDouble().ToString()
          + "\r\n";

        // If created by API
        try // this param was working in RS2, but not accessible in RS3!
        {
          sMsg += "  LOAD_IS_CREATED_BY_API BuiltInParameter= "
            + RacUtils.GetParamAsString(ptLd.get_Parameter(BuiltInParameter.LOAD_IS_CREATED_BY_API))
            + "\r\n";
        }
        catch
        {

        }
        try
        {
          sMsg += "  ELEM_TYPE_PARAM BuiltInParameter= "
            + RacUtils.GetParamAsString(ptLd.get_Parameter(BuiltInParameter.ELEM_TYPE_PARAM))
            + "\r\n";

        }
        catch
        {

        }
      }
      catch (Exception ex)
      {
        RacUtils.InfoMsg("Error in ListAndModifyPointLoad: " + ex.Message);
      }
      finally
      {
        RacUtils.InfoMsg(sMsg);
      }
    }

    public void ListAndModifyLineLoad(LineLoad lnLd, Application app)
    {
      // One can access some parameters as read-only class properties/methods,
      // but to be able to change them and to access all, we need to go via Parameters:
      string sMsg = "LINE Load Id=" + lnLd.Id.Value.ToString() + "\r\n";
      try
      {
        // Again, show how to access the same param via both display name and BUILT_IN_PARAMS
        sMsg += "  Load Case (via Display Name)= "
          + RacUtils.GetParamAsString(RacUtils.GetElemParam(lnLd, "Load Case")) + "\r\n";
        sMsg +=  "  LOAD_CASE_ID BuiltInParameter= "
          + RacUtils.GetParamAsString(lnLd.get_Parameter(BuiltInParameter.LOAD_CASE_ID)) + "\r\n";

        // Scale Fz1 by factor 2, but this time get it as BUILT_IN_PARAMS
        Parameter paramFZ1 = lnLd.get_Parameter(BuiltInParameter.LOAD_LINEAR_FORCE_FZ1);
        double FZ1_old = paramFZ1.AsDouble();
        paramFZ1.Set((double)(2.0 * FZ1_old));
        sMsg += "  FZ1:  OLD=" + FZ1_old.ToString() + " NEW=" + paramFZ1.AsDouble().ToString() + "\r\n";


        // Check if the Load is attach to a host - DOESN'T WORK! ALWAYS Nothing...
        // There seems to be NO way to determine if an Line/Area LOAD is HOSTED
        // In Revit 2008, use the generic load property LoadBase.HostElement
        Parameter hostIdParam = lnLd.get_Parameter(BuiltInParameter.HOST_ID_PARAM);
        if (hostIdParam != null)
        {
          ElementId hostId = hostIdParam.AsElementId();
          Autodesk.Revit.Element elem = app.ActiveDocument.get_Element(ref hostId);
          sMsg +=  "  HOST element ID=" + elem.Id.Value.ToString() + " Class=" + elem.GetType().Name
            + " Name=" + elem.Name + "\r\n";
        }
        else
        {
          sMsg +=  "  NO HOST element\r\n";
        }


        // test possible values for Line Load:
        // ALL 3 are 0 if "Project"
        // ALL 3 are 1 if either "Workplane" or "Host Workplane"
        sMsg +=  "  Orient to (via Display Name)= "
          + RacUtils.GetParamAsString(RacUtils.GetElemParam(lnLd, "Orient to")) + "\r\n";
        sMsg +=  "  LOAD_USE_LOCAL_COORDINATE_SYSTEM BuiltInParameter= "
          + RacUtils.GetParamAsString(lnLd.get_Parameter(BuiltInParameter.LOAD_USE_LOCAL_COORDINATE_SYSTEM))
          + "\r\n";
        try
        {
          sMsg +=  "  LOAD_USE_LOCAL_COORDINATE_SYSTEM_HOSTED BuiltInParameter= "
            + RacUtils.GetParamAsString(lnLd.get_Parameter(BuiltInParameter.LOAD_USE_LOCAL_COORDINATE_SYSTEM_HOSTED))
            + "\r\n";
        }
        catch
        {

        }

        // If created by API
        try
        {
          sMsg +=  "  LOAD_IS_CREATED_BY_API BuiltInParameter= "
            + RacUtils.GetParamAsString(lnLd.get_Parameter(BuiltInParameter.LOAD_IS_CREATED_BY_API))
            + "\r\n";
        }
        catch
        {

        }
      }
      catch (Exception ex)
      {
        RacUtils.InfoMsg("Error in ListAndModifyLineLoad: " + ex.Message);

      }
      finally
      {
        RacUtils.InfoMsg(sMsg);
      }
    }

    public void ListAndModifyAreaLoad(AreaLoad arLd, Application app)
    {
      // One can access some parameters as read-only class properties/methods,
      // but to be able to change them and to access all, we need to go via Parameters:
      string sMsg = "AREA Load Id=" + arLd.Id.Value.ToString() + "\r\n";
      try
      {
        // Again, show how to access the same param via both display name and BUILT_IN_PARAMS
        sMsg +=  "  Load Case (via Display Name)= "
          + RacUtils.GetParamAsString(RacUtils.GetElemParam(arLd, "Load Case")) + "\r\n";
        sMsg +=  "  LOAD_CASE_ID BuiltInParameter= "
          + RacUtils.GetParamAsString(arLd.get_Parameter(BuiltInParameter.LOAD_CASE_ID)) + "\r\n";

        // Scale Fz1 by factor 2
        Parameter paramFZ1 = arLd.get_Parameter(BuiltInParameter.LOAD_AREA_FORCE_FZ1);
        double FZ1_old = paramFZ1.AsDouble();
        paramFZ1.Set((double)(2.0 * FZ1_old));
        sMsg +=  "  FZ1:  OLD=" + FZ1_old.ToString() + " NEW=" + paramFZ1.AsDouble().ToString()
          + "\r\n";


        // Specifically for AREA Load, there can be more than a single Loop:
        int numLoops = arLd.NumLoops;
        // Loop all Loops
        sMsg +=  "\r\n  Number Of Loops = " + numLoops.ToString() + "\r\n";
        for (int i = 0; i < numLoops ; i++)
        {
          int numCurves = arLd.get_NumCurves(i);
          sMsg +=  "  Loop " + (i + 1) + " has " + numCurves.ToString() + " curves:\r\n";
          for (int j = 0; j < numCurves ; j++)
          {
            Curve crv = arLd.get_Curve(i, j);
            if (crv is Line)
            {
              Line line = (Line)crv;
              XYZ ptS = line.get_EndPoint(0);
              XYZ ptE = line.get_EndPoint(1);
              sMsg +=  "    Curve " + (j + 1) + " is a LINE:"
                + ptS.X.ToString() + ", " + ptS.Y.ToString() + ", " + ptS.Z.ToString() + " ; "
                + ptE.X.ToString() + ", " + ptE.Y.ToString() + ", " + ptE.Z.ToString() + "\r\n";
            }
            else if (crv is Arc)
            {
              Arc arc = (Arc)crv;
              XYZ ptS = arc.get_EndPoint(0);
              XYZ ptE = arc.get_EndPoint(1);
              double r = arc.Radius;
              sMsg +=  "    Curve " + (j + 1) + " is an ARC:"
                + ptS.X.ToString() + ", " + ptS.Y.ToString() + ", " + ptS.Z.ToString() + " ; "
                + ptE.X.ToString() + ", " + ptE.Y.ToString() + ", " + ptE.Z.ToString()
                + " ; R=" + r.ToString() + "\r\n";
            }
          }
        }
        int numRefPts = arLd.NumRefPoints;
        sMsg +=  "\r\n  Number Of Ref.Points = " + numRefPts.ToString() + "\r\n";

        for (int i = 0; i < numRefPts ; i++)
        {
          XYZ pt = arLd.get_RefPoint(i);
          sMsg +=  "  RefPt " + (i + 1) + " = "
            + pt.X.ToString() + ", " + pt.Y.ToString() + ", " + pt.Z.ToString() + "\r\n";
        }
      }
      catch (Exception ex)
      {
        RacUtils.InfoMsg("Error in ListAndModifyAreaLoad: " + ex.Message);
      }
      finally
      {
        RacUtils.InfoMsg(sMsg);
      }
    }

  }

  // List all Load Symbols
  public class Lab1_4 : IExternalCommand
  {
    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;
      List<Autodesk.Revit.Element> symbs = RstUtils.GetAllLoadSymbols(app);
      string sMsg = "There are " + symbs.Count.ToString()
        + " Load Symbol objects in the model:" + "\r\n";

      foreach (Symbol ls in symbs)
      {
        // There are no dedicated classes like PointLoadSymbol, LineLoadSymbol or AreaLoadSymbol,
        // so we need to check the Family name Param!
        string famName = "?";
        try
        {
          // this worked in RS2, but failing now in RS3, works fine in 2008
          famName = ls.get_Parameter(BuiltInParameter.SYMBOL_FAMILY_NAME_PARAM).AsString();
        }
        catch
        {
        }

        sMsg +=  "  " + ls.Name + ", Id=" + ls.Id.Value.ToString()
               + " Family=" + famName + "\r\n";

      }
      RacUtils.InfoMsg(sMsg);

      // We better make utilities (or alternatively single one for all 3 like for Loads) that will get particular Symbols.
      // For example, only for POINT Load Symbols:
      List<Autodesk.Revit.Element> ptLdSymbs = RstUtils.GetPointLoadSymbols(app);
      sMsg = ("Point Load Symbols directly:" + "\r\n");
      foreach (Symbol ls1 in ptLdSymbs)
      {
        // Get one specific param for this symbol (can also change it if needed, like for load objects)
        string scaleF = RacUtils.GetParamAsString(ls1.get_Parameter(BuiltInParameter.LOAD_ATTR_FORCE_SCALE_FACTOR));
        sMsg +=  "  " + ls1.Name + ", Id=" + ls1.Id.Value.ToString()
               + " Force Scale=" + scaleF + "\r\n";
      }
      RacUtils.InfoMsg(sMsg);

      return CmdResult.Succeeded;

    }
  }

  // Create NEW Load
  public class Lab1_5 : IExternalCommand
  {
    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;
      // Loads are not standard Families and FamilySymbols!!!
      // Id=42618; Class=Symbol; Category=Structural Loads; Name=Point Load 1
      // Id=42623; Class=Symbol; Category=Structural Loads; Name=Line Load 1
      // Id=42632; Class=Symbol; Category=Structural Loads; Name=Area Load 1
      // Id=126296; Class=Symbol; Category=Structural Loads; Name=MS load
      // Id=129986; Class=Symbol; Category=Structural Loads; Name=MS AL 2
      // ' Hence cannot use anything like NewFamilyInstance...
      // ...but there are DEDICATED METHODS:
      Autodesk.Revit.Creation.Document creationDoc = app.ActiveDocument.Create;
      // REACTION (will be read-only in UI)
      XYZ pt = new XYZ(0, 0, 0);
      XYZ f = new XYZ(0, 0, 14593.9);
      // eq.to 1 kip
      XYZ m = new XYZ(0, 0, 0);
      // Dim newPointLoadReaction As PointLoad = creationDoc.NewPointLoad(pt, f, m, True) ' 2008
      PointLoad newPointLoadReaction = creationDoc.NewPointLoad(pt, f, m, true, null, null);
      // 2009 ... todo: test this!
      // This one will have default Symbol and no Load case
      // EXTERNAL Force
      // Dim newPointLoadExternal As PointLoad
      // = creationDoc.NewPointLoad(New XYZ(5, 5, 0), New XYZ(0, 0, -30000), New XYZ(0, 0, 0), False) ' 2008
      PointLoad newPointLoadExternal = creationDoc.NewPointLoad(new XYZ(5, 5, 0)
                   , new XYZ(0, 0, -30000), new XYZ(0, 0, 0), false, null, null);
      // 2009 ... todo: test this!
      // doesn't work any longer in RS3!!!
      // seems ok again in 2008
      // ' Ask user to select SYMBOL for the new Load (loop questions to avoid custom forms)
      List<Autodesk.Revit.Element> ptLdSymbs = RstUtils.GetPointLoadSymbols(app);
      bool gotSymb = false;
      while (!gotSymb)
      {
        foreach (Symbol sym in ptLdSymbs)
        {
          switch( WinForms.MessageBox.Show( ( "Use PointLoad Symbol " + ( sym.Name + "?" ) ),
             "Select Symbol for the New Load", WinForms.MessageBoxButtons.YesNoCancel ) )
          {
            case WinForms.DialogResult.Cancel:
              return CmdResult.Cancelled;
            case WinForms.DialogResult.Yes:
              // either of these works to change the Type/Symbol
              ElementId sysId = sym.Id;
              newPointLoadExternal.get_Parameter(BuiltInParameter.ELEM_TYPE_PARAM).Set(ref sysId);
              // newPointLoadExternal.Parameter(BuiltInParameter.SYMBOL_ID_PARAM).Set(sym.Id)
              gotSymb = true;
              break; // TODO: Warning!!! Review that break works as 'Exit For' as it is inside another 'breakable' statement:Switch
          }
          if (gotSymb == true)
            break;
        }
      }
      // Ask user to select LOAD CASE for the new Load (loop questions to avoid custom forms)
      // Load CASES
      List<Autodesk.Revit.Element> ldCases = RstUtils.GetAllLoadCases(app);
      bool gotCase = false;
      while (!gotCase)
      {
        foreach (LoadCase ldCase in ldCases)
        {
          switch( WinForms.MessageBox.Show( ( "Assign to Load Case " + ( ldCase.Name + "?" ) ),
             "Select Load Case for the New Load", WinForms.MessageBoxButtons.YesNoCancel ) )
          {
            case WinForms.DialogResult.Cancel:
              return CmdResult.Cancelled;
            case WinForms.DialogResult.Yes:
              ElementId ldCaseid = ldCase.Id;
              newPointLoadExternal.get_Parameter(BuiltInParameter.LOAD_CASE_ID).Set(ref ldCaseid);
              gotCase = true;
              break; // TODO: Warning!!! Review that break works as 'Exit For' as it is inside another 'breakable' statement:Switch
          }
          if (gotCase == true)
            break;
        }
      }
      return CmdResult.Succeeded;
    }
  }
}