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
using Arc = Autodesk.Revit.Geometry.Arc;
using Curve = Autodesk.Revit.Geometry.Curve;
using Line = Autodesk.Revit.Geometry.Line;
using XYZ = Autodesk.Revit.Geometry.XYZ;
using Autodesk.Revit.Parameters;
using Autodesk.Revit.Structural;
using Autodesk.Revit.Utility;
using CmdResult = Autodesk.Revit.IExternalCommand.Result;
#endregion // Namespaces

namespace RstLabs
{
  #region Lab1_1_LoadGrouping
  /// <summary>
  /// Lab 1-1 - List all load grouping objects, 
  /// i.e. load natures, cases, and combinations.
  /// </summary>
  public class Lab1_1_LoadGrouping : IExternalCommand
  {
    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;

      // load natures
      List<Element> ldNatures = RstUtils.GetAllLoadNatures( app );
      string sMsg = "There are " + ldNatures.Count.ToString() + " LoadNature objects in the model:";
      foreach( LoadNature nature in ldNatures )
      {
        sMsg += "\r\n  " + nature.Name + ", Id=" + nature.Id.Value.ToString();
      }
      RacUtils.InfoMsg( sMsg );

      // load cases
      List<Element> ldCases = RstUtils.GetAllLoadCases( app );
      sMsg = "There are " + ldCases.Count.ToString() + " LoadCase objects in the model:";
      foreach( LoadCase ldCase in ldCases )
      {
        // Category is NOT implemented in API for Loadcase class in 2008 and 2009,
        // though the category enums are there (OST_LoadCasesXxx)
        string catName = (null == ldCase.Category) ? "?" : ldCase.Category.Name;
        sMsg += "\r\n  " + ldCase.Name + ", Id=" + ldCase.Id.Value.ToString()
          + ", Category=" + catName;
        // there seems to be no way to get LoadCase's LoadNature in API
      }
      RacUtils.InfoMsg( sMsg );

      // load usages (optionally used for combinations)
      List<Element> ldUsages = RstUtils.GetAllLoadUsages( app );
      sMsg = "There are " + ldUsages.Count.ToString() + " LoadUsage objects in the model:";
      foreach( LoadUsage ldUsage in ldUsages )
      {
        sMsg += "\r\n  " + ldUsage.Name + ", Id=" + ldUsage.Id.Value.ToString();
      }
      RacUtils.InfoMsg( sMsg );

      // load combinations
      List<Element> combs = RstUtils.GetAllLoadCombinations( app );
      sMsg = "There are " + combs.Count.ToString() + " LoadCombination objects in the model:";
      foreach( LoadCombination comb in combs )
      {
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
          + ", Id=" + comb.Id.Value.ToString()
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
      RacUtils.InfoMsg( sMsg );
      // most of these objects are also createable, see the
      // Autodesk.Revit.Creation.Document.NewLoad*** methods. 
      // To call them, use something like:
      // app.ActiveDocument.Create.NewLoadCase(...)
      // app.ActiveDocument.Create.NewLoadCombination(...)
      // app.ActiveDocument.Create.NewLoadNature(...)
      // app.ActiveDocument.Create.NewLoadUsage(...)
      return CmdResult.Succeeded;
    }
  }
  #endregion // Lab1_1_LoadGrouping

  #region Lab1_2_Loads
  /// <summary>
  /// Lab 1-2 - Demonstrate access to load objects and list all loads.
  /// </summary>
  public class Lab1_2_Loads : IExternalCommand
  {
    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;
      List<Element> loads = RstUtils.GetAllLoads( app );
      string sMsg = "There are " + loads.Count.ToString() + " load objects in the model:";
      string sHost;

      foreach( LoadBase load in loads )
      {
        sHost = ( null == load.HostElement )
          ? string.Empty
          : string.Format( ", Host={0}:{1}", load.HostElement.Name, load.HostElement.Id.Value );
        sMsg += "\r\n  " + load.GetType().Name
          + ", Id=" + load.Id.Value.ToString()
          + sHost;
      }
      RacUtils.InfoMsg( sMsg );
      // We could loop the above collection, but better code 
      // another helper for specific loads directly
      ElementSet pointLoads = app.Create.NewElementSet();
      ElementSet lineLoads = app.Create.NewElementSet();
      ElementSet areaLoads = app.Create.NewElementSet();
      RstUtils.GetAllSpecificLoads( app, ref pointLoads, ref lineLoads, ref areaLoads );

      // POINT
      sMsg = pointLoads.Size.ToString() + " POINT Loads:";
      foreach( PointLoad ptLd in pointLoads )
      {
        // The following are all specific READ-ONLY properties:
        XYZ F = ptLd.Force;
        XYZ M = ptLd.Moment;
        XYZ p = ptLd.Point;
        string nameLoadCase = ptLd.LoadCaseName;
        string nameLoadCategory = ptLd.LoadCategoryName; // bug in 8.1 - returns nothing - still the same in 2009
        string nameLoadNature = ptLd.LoadNatureName;
        sMsg += "\r\n  Id=" + ptLd.Id.Value.ToString()
          + ": F=" + RacUtils.PointString( F )
          + ", M=" + RacUtils.PointString( M )
          + ", Pt=" + RacUtils.PointString( p )
          + ", LoadCase=" + nameLoadCase
          + ", LoadCat=" + nameLoadCategory
          + ", LoadNature=" + nameLoadNature;
      }
      RacUtils.InfoMsg( sMsg );

      // LINE
      sMsg = lineLoads.Size + " LINE Loads:";
      foreach( LineLoad lnLd in lineLoads )
      {
        // The following are *some* READ-ONLY properties:
        XYZ F1 = lnLd.Force1;
        XYZ F2 = lnLd.Force2;
        // can do similar for Moment1, Moment2...
        XYZ ptStart = lnLd.StartPoint;
        // similar for EndPoint
        // LoadxxxName same as for PointLoad (implemented in the base class)
        sMsg += "\r\n  Id=" + lnLd.Id.Value.ToString()
          + ": F1= " + RacUtils.PointString( F1 )
          + ", F2= " + RacUtils.PointString( F2 )
          + ", Start= " + RacUtils.PointString( ptStart )
          + ", Uniform=" + lnLd.UniformLoad
          + ", Projected=" + lnLd.ProjectedLoad;
      }
      RacUtils.InfoMsg( sMsg );

      // AREA
      sMsg = areaLoads.Size.ToString() + " AREA Loads:";
      foreach( AreaLoad arLd in areaLoads )
      {
        // The following are *some* READ-ONLY properties:
        XYZ F1 = arLd.Force1;
        int numLoops = arLd.NumLoops;
        string s = ", Nr. of Loops=" + numLoops.ToString()
          + ", Nr. of Curves=" + arLd.get_NumCurves(0).ToString();
        for( int i = 1; i < numLoops; ++i ) // if more than single loop
        {
          s += "," + arLd.get_NumCurves( i );
        }
        sMsg += "\r\n  Id=" + arLd.Id.Value.ToString()
          + ": F1= " + RacUtils.PointString(F1) + s;

        // For curves geometry, see later commands...
      }
      RacUtils.InfoMsg( sMsg );
      return CmdResult.Succeeded;
    }
  }
  #endregion // Lab1_2_Loads

  #region Lab1_3_ModifySelectedLoads
  /// <summary>
  /// Lab 1-3 - More detailed info and modification of selected loads.
  /// </summary>
  public class Lab1_3_ModifySelectedLoads : IExternalCommand
  {
    Document _doc;

    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;
      _doc = app.ActiveDocument;
      ElementSetIterator iter = _doc.Selection.Elements.ForwardIterator();
      while( iter.MoveNext() )
      {
        Element elem = iter.Current as Element;
        if( elem is PointLoad )
        {
          ListAndModifyPointLoad( elem as PointLoad );
        }
        else if( elem is LineLoad )
        {
          ListAndModifyLineLoad( elem as LineLoad, app );
        }
        else if( elem is AreaLoad )
        {
          ListAndModifyAreaLoad( elem as AreaLoad, app );
        }
      }
      return CmdResult.Succeeded;
    }

    public void ListAndModifyPointLoad( PointLoad ptLd )
    {
      // One can access some parameters as read-only class properties/methods,
      // but to be able to change them and to access all, we need to go via Parameters:
      string sMsg = "POINT Load Id=" + ptLd.Id.Value.ToString();
      try
      {
        // List 2 Params via their display name (locale-dependent!)
        sMsg += "\r\n  Is Reaction (via Display Name)= "
          + RacUtils.GetParameterValue( RacUtils.GetElemParam( ptLd, "Is Reaction" ) );
        sMsg += "\r\n  Load Case (via Display Name)= "
          + RacUtils.GetParameterValue2( RacUtils.GetElemParam( ptLd, "Load Case" ), _doc );

        // Better use BUILT_IN_PARAMS, but have to guess-and-try-and-error to find correct ones!
        // The following 2 give the same as above:
        sMsg += "\r\n  LOAD_IS_REACTION BuiltInParameter= "
          + RacUtils.GetParameterValue( ptLd.get_Parameter( BuiltInParameter.LOAD_IS_REACTION ) );

        sMsg += "\r\n  LOAD_CASE_ID BuiltInParameter= "
          + RacUtils.GetParameterValue2( ptLd.get_Parameter( BuiltInParameter.LOAD_CASE_ID ), _doc );

        // The following display one hasn't got one-to-one corresponding Built-in, but basically the same:
        sMsg += "\r\n  Orient to (via Display Name)= "
          + RacUtils.GetParameterValue( RacUtils.GetElemParam( ptLd, "Orient to" ) );
        sMsg += "\r\n  LOAD_USE_LOCAL_COORDINATE_SYSTEM BuiltInParameter= "
          + RacUtils.GetParameterValue( ptLd.get_Parameter( BuiltInParameter.LOAD_USE_LOCAL_COORDINATE_SYSTEM ) );
        sMsg += "\r\n  LOAD_USE_LOCAL_COORDINATE_SYSTEM_HOSTED BuiltInParameter= "
          + RacUtils.GetParameterValue( ptLd.get_Parameter( BuiltInParameter.LOAD_USE_LOCAL_COORDINATE_SYSTEM_HOSTED ) );

        // Scale Fz by factor 2
        Parameter paramFz = ptLd.get_Parameter( "Fz" );
        double Fz_old = paramFz.AsDouble();
        paramFz.Set( 2.0 * Fz_old );
        sMsg +=  "\r\n  Fz:  OLD=" + Fz_old.ToString() + " NEW=" + paramFz.AsDouble().ToString();

        // If created by API
        //try // this param was working in RS2, but not accessible in RS3!
        {
          sMsg += "\r\n  LOAD_IS_CREATED_BY_API BuiltInParameter= "
            + RacUtils.GetParameterValue( ptLd.get_Parameter( BuiltInParameter.LOAD_IS_CREATED_BY_API ) );
        }
        //try
        {
          sMsg += "\r\n  ELEM_TYPE_PARAM BuiltInParameter= "
            + RacUtils.GetParameterValue( ptLd.get_Parameter( BuiltInParameter.ELEM_TYPE_PARAM ) );
        }
      }
      catch( Exception ex )
      {
        RacUtils.InfoMsg( "Error in ListAndModifyPointLoad: " + ex.Message );
      }
      finally
      {
        RacUtils.InfoMsg( sMsg );
      }
    }

    public void ListAndModifyLineLoad( LineLoad lnLd, Application app )
    {
      // One can access some parameters as read-only class properties/methods,
      // but to be able to change them and to access all, we need to go via Parameters:
      string sMsg = "LINE Load Id=" + lnLd.Id.Value.ToString();
      try
      {
        // show how to access the same param via both display name and BUILT_IN_PARAMS
        sMsg += "\r\n  Load Case (via Display Name)= "
          + RacUtils.GetParameterValue( RacUtils.GetElemParam( lnLd, "Load Case" ) );
        sMsg += "\r\n  LOAD_CASE_ID BuiltInParameter= "
          + RacUtils.GetParameterValue2( lnLd.get_Parameter( BuiltInParameter.LOAD_CASE_ID ), _doc );

        // Scale Fz1 by factor 2, but this time get it as BUILT_IN_PARAMS
        Parameter paramFZ1 = lnLd.get_Parameter( BuiltInParameter.LOAD_LINEAR_FORCE_FZ1 );
        double FZ1_old = paramFZ1.AsDouble();
        paramFZ1.Set( 2.0 * FZ1_old );
        sMsg += "\r\n  FZ1:  OLD=" + FZ1_old.ToString() + " NEW=" + paramFZ1.AsDouble().ToString();

        // Check if the load is attached to a host - did not work in previous versions.
        // There was no way to determine if an Line/Area load was hosted.
        // Since Revit 2008, you can determine this more easily using the 
        // generic load property LoadBase.HostElement, as demonstrated in 
        // Lab1_2_Loads:
        Parameter hostIdParam = lnLd.get_Parameter( BuiltInParameter.HOST_ID_PARAM );
        if( null != hostIdParam )
        {
          ElementId hostId = hostIdParam.AsElementId();
          Element elem = app.ActiveDocument.get_Element( ref hostId );
          sMsg += "\r\n  HOST element ID=" + elem.Id.Value.ToString() + " Class=" + elem.GetType().Name
            + " Name=" + elem.Name;
        }
        else
        {
          sMsg += "\r\n  NO HOST element";
        }

        // test possible values for Line Load:
        // ALL 3 are 0 if "Project"
        // ALL 3 are 1 if either "Workplane" or "Host Workplane"
        sMsg += "\r\n  Orient to (via Display Name)= "
          + RacUtils.GetParameterValue( RacUtils.GetElemParam( lnLd, "Orient to" ) );
        sMsg += "\r\n  LOAD_USE_LOCAL_COORDINATE_SYSTEM BuiltInParameter= "
          + RacUtils.GetParameterValue( lnLd.get_Parameter( BuiltInParameter.LOAD_USE_LOCAL_COORDINATE_SYSTEM ) );
        sMsg += "\r\n  LOAD_USE_LOCAL_COORDINATE_SYSTEM_HOSTED BuiltInParameter= "
          + RacUtils.GetParameterValue( lnLd.get_Parameter( BuiltInParameter.LOAD_USE_LOCAL_COORDINATE_SYSTEM_HOSTED ) );
        sMsg += "\r\n  LOAD_IS_CREATED_BY_API BuiltInParameter= "
          + RacUtils.GetParameterValue(lnLd.get_Parameter(BuiltInParameter.LOAD_IS_CREATED_BY_API));
      }
      catch( Exception ex )
      {
        RacUtils.InfoMsg( "Error in ListAndModifyLineLoad: " + ex.Message );
      }
      finally
      {
        RacUtils.InfoMsg( sMsg );
      }
    }

    public void ListAndModifyAreaLoad( AreaLoad arLd, Application app )
    {
      // One can access some parameters as read-only class properties/methods,
      // but to be able to change them and to access all, we need to go via Parameters:
      string sMsg = "AREA Load Id=" + arLd.Id.Value.ToString();
      try
      {
        // Again, show how to access the same param via both display name and BUILT_IN_PARAMS
        sMsg += "\r\n  Load Case (via Display Name)= "
          + RacUtils.GetParameterValue( RacUtils.GetElemParam( arLd, "Load Case" ) );
        sMsg += "\r\n  LOAD_CASE_ID BuiltInParameter= "
          + RacUtils.GetParameterValue2( arLd.get_Parameter( BuiltInParameter.LOAD_CASE_ID ), _doc );

        // Scale Fz1 by factor 2
        Parameter paramFZ1 = arLd.get_Parameter(BuiltInParameter.LOAD_AREA_FORCE_FZ1);
        double FZ1_old = paramFZ1.AsDouble();
        paramFZ1.Set((double)(2.0 * FZ1_old));
        sMsg += "\r\n  FZ1:  OLD=" + RacUtils.RealString( FZ1_old ) 
          + " NEW=" + RacUtils.RealString( paramFZ1.AsDouble() ) + "\r\n";

        // Specifically for AREA Load, there can be more than a single Loop:
        int numLoops = arLd.NumLoops;
        // Loop all Loops
        sMsg +=  "\r\n  Number Of Loops = " + numLoops.ToString();
        for (int i = 0; i < numLoops; ++i)
        {
          int numCurves = arLd.get_NumCurves(i);
          sMsg += "\r\n  Loop " + (i + 1) + " has " + numCurves.ToString() + " curves:";
          for (int j = 0; j < numCurves; j++)
          {
            Curve crv = arLd.get_Curve(i, j);
            if (crv is Line)
            {
              Line line = (Line)crv;
              XYZ ptS = line.get_EndPoint(0);
              XYZ ptE = line.get_EndPoint(1);
              sMsg += "\r\n    Curve " + (j + 1) + " is a LINE:"
                + RacUtils.PointString(ptS) + " ; "
                + RacUtils.PointString(ptE);
            }
            else if (crv is Arc)
            {
              Arc arc = (Arc)crv;
              XYZ ptS = arc.get_EndPoint(0);
              XYZ ptE = arc.get_EndPoint(1);
              double r = arc.Radius;
              sMsg += "\r\n    Curve " + (j + 1) + " is an ARC:"
                + RacUtils.PointString(ptS) + " ; "
                + RacUtils.PointString(ptE)
                + " ; R=" + r.ToString();
            }
          }
        }
        int numRefPts = arLd.NumRefPoints;
        sMsg +=  "\r\n  Number of Ref. Points = " + numRefPts.ToString();

        for( int i = 0; i < numRefPts; ++i )
        {
          XYZ p = arLd.get_RefPoint(i);
          sMsg += "\r\n  RefPt " + i.ToString() + " = " + RacUtils.PointString( p );
        }
      }
      catch( Exception ex )
      {
        RacUtils.InfoMsg("Error in ListAndModifyAreaLoad: " + ex.Message);
      }
      finally
      {
        RacUtils.InfoMsg( sMsg );
      }
    }
  }
  #endregion // Lab1_3_ModifySelectedLoads

  #region Lab1_4_LoadSymbols
  /// <summary>
  /// Lab 1-4 - List load symbols.
  /// </summary>
  public class Lab1_4_LoadSymbols : IExternalCommand
  {
    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;
      List<Element> symbs = RstUtils.GetAllLoadSymbols( app );
      string sMsg = "There are " + symbs.Count.ToString()
        + " load symbol objects in the model:";

      foreach( Symbol ls in symbs )
      {
        // There are no dedicated classes like PointLoadSymbol, LineLoadSymbol or AreaLoadSymbol,
        // so we need to check the Family name Param!
        string famName = "?";
        //try
        {
          // this worked in RS2, but failing now in RS3, works fine in 2008
          famName = ls.get_Parameter(BuiltInParameter.SYMBOL_FAMILY_NAME_PARAM).AsString();
        }
        //catch
        //{
        //}
        sMsg += "\r\n  " + ls.Name + ", Id=" + ls.Id.Value.ToString() + ", Family=" + famName;
      }
      RacUtils.InfoMsg( sMsg );

      // We better make utilities (or alternatively single one for all three,
      // like for loads) that will get particular symbols. For example, to
      // retrieve POINT load symbols:
      List<Element> ptLdSymbs = RstUtils.GetPointLoadSymbols( app );
      sMsg = "Point load symbols directly:";
      foreach( Symbol ls1 in ptLdSymbs )
      {
        // Get one specific param for this symbol (can also change it if needed, like for load objects)
        string scaleF = RacUtils.GetParameterValue( ls1.get_Parameter( BuiltInParameter.LOAD_ATTR_FORCE_SCALE_FACTOR ) );
        sMsg +=  "\r\n  " + ls1.Name + ", Id=" + ls1.Id.Value.ToString() + ", Force Scale=" + scaleF;
      }
      RacUtils.InfoMsg( sMsg );
      return CmdResult.Succeeded;
    }
  }
  #endregion // Lab1_4_LoadSymbols

  #region Lab1_5_CreateNewPointLoads
  /// <summary>
  /// Lab 1-5 - Create new point loads.
  /// </summary>
  public class Lab1_5_CreateNewPointLoads : IExternalCommand
  {
    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;
      // Loads are not standard Families and FamilySymbols:
      // Id=42618; Class=Symbol; Category=Structural Loads; Name=Point Load 1
      // Id=42623; Class=Symbol; Category=Structural Loads; Name=Line Load 1
      // Id=42632; Class=Symbol; Category=Structural Loads; Name=Area Load 1
      // Id=126296; Class=Symbol; Category=Structural Loads; Name=MS load
      // Id=129986; Class=Symbol; Category=Structural Loads; Name=MS AL 2
      // Hence cannot use anything like NewFamilyInstance ...
      // ... but there are dedicated methods:
      Autodesk.Revit.Creation.Document creationDoc = app.ActiveDocument.Create;

      // REACTION (will be read-only in UI)
      XYZ p = new XYZ( 0, 0, 0 );
      XYZ f = new XYZ( 0, 0, 14593.9 ); // equals 1 kip
      XYZ m = new XYZ( 0, 0, 0 );
      PointLoad newPointLoadReaction = creationDoc.NewPointLoad( p, f, m, true, null, null );
      // This one will have default symbol and no load case

      // EXTERNAL Force
      PointLoad newPointLoadExternal = creationDoc.NewPointLoad( new XYZ(5, 5, 0), new XYZ(0, 0, -30000), new XYZ(0, 0, 0), false, null, null );

      // Ask user to select SYMBOL for the new Load (loop questions to avoid custom forms)
      List<Element> ptLdSymbs = RstUtils.GetPointLoadSymbols( app );
      bool gotSymb = false;
      while( !gotSymb )
      {
        foreach( Symbol sym in ptLdSymbs )
        {
          switch( WinForms.MessageBox.Show( "Use point load symbol " + sym.Name + "?",
             "Select symbol for the new load", WinForms.MessageBoxButtons.YesNoCancel ) )
          {
            case WinForms.DialogResult.Cancel:
              return CmdResult.Cancelled;
            case WinForms.DialogResult.Yes:
              // either of these works to change the Type/Symbol
              ElementId id = sym.Id;
              newPointLoadExternal.get_Parameter( BuiltInParameter.ELEM_TYPE_PARAM ).Set( ref id );
              // newPointLoadExternal.Parameter(BuiltInParameter.SYMBOL_ID_PARAM).Set(sym.Id)
              gotSymb = true;
              break;
          }
          if( gotSymb ) { break; }
        }
      }

      // Ask user to select LOAD CASE for the new Load (loop questions to avoid custom forms)
      List<Element> ldCases = RstUtils.GetAllLoadCases( app );
      bool gotCase = false;
      while( !gotCase )
      {
        foreach( LoadCase ldCase in ldCases )
        {
          switch( WinForms.MessageBox.Show( "Assign to load case " + ldCase.Name + "?",
             "Select load case for the new load", WinForms.MessageBoxButtons.YesNoCancel ) )
          {
            case WinForms.DialogResult.Cancel:
              return CmdResult.Cancelled;
            case WinForms.DialogResult.Yes:
              ElementId ldCaseid = ldCase.Id;
              newPointLoadExternal.get_Parameter(BuiltInParameter.LOAD_CASE_ID).Set(ref ldCaseid);
              gotCase = true;
              break;
          }
          if( gotCase ) { break; }
        }
      }
      return CmdResult.Succeeded;
    }
  }
  #endregion // Lab1_5_CreateNewPointLoads
}
