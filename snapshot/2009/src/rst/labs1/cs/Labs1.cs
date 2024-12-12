using System;
using System.Collections;
using System.Collections.Generic ;

using Autodesk;
using Autodesk.Revit;
using Autodesk.Revit.Parameters;
using Autodesk.Revit.Structural;
using Autodesk.Revit.Utility;
using Autodesk.Revit.Elements;

using System.Windows.Forms;

using Autodesk.Revit.Geometry;


public class Lab1_1 : IExternalCommand
{
  public Autodesk.Revit.IExternalCommand.Result Execute(Autodesk.Revit.ExternalCommandData commandData, ref string message, Autodesk.Revit.ElementSet elements)
  {
    Autodesk.Revit.Application app = commandData.Application;
    //  Load NATURES
    ElementSet  ldNatures = RstUtils.GetAllLoadNatures(app);
    string sMsg = "There are " + ldNatures.Size.ToString()  + " LoadNature objects in the model:";
    foreach (LoadNature nature in ldNatures)
    {
      sMsg += "\r\n  " + nature.Name + ", Id=" + nature.Id.Value.ToString();
    }
    MessageBox.Show(sMsg);
    //  Load CASES
    ElementSet ldCases = RstUtils.GetAllLoadCases(app);
    sMsg = "There are " + ldCases.Size.ToString() + " LoadCase objects in the model:" + "\r\n";
    foreach (LoadCase ldCase in ldCases)
    {
      string catName = "";
      try
      {
        //  . Category NOT implemented in API for Loadcase class :-( , though the category enums are there (OST_LoadCasesXxx)
        catName = ldCase.Category.Name;
      }
      catch
      {
      }

      sMsg = sMsg + "  " + ldCase.Name + ", Id=" + ldCase.Id.Value.ToString()
               + ", Category=" + catName + "\r\n";

       //  there seems to be no way to get LoadCase's LoadNature in API
    }
    MessageBox.Show(sMsg);

    //  Load USAGES (optionally used for Combinations)
    ElementSet ldUsages = RstUtils.GetAllLoadUsages(app);
    sMsg = "There are "+ ldUsages.Size + " LoadUsage objects in the model:" + "\r\n";

    foreach (LoadUsage ldUsage in ldUsages)
    {
      sMsg = sMsg + "  "+ ldUsage.Name+ ", Id="+ ldUsage.Id.Value.ToString()  + "\r\n";
    }
    MessageBox.Show(sMsg);

    //  Load COMBINATIONS
    ElementSet combs = RstUtils.GetAllLoadCombinations(app);
    sMsg = "There are " + combs.Size + " LoadCombination objects in the model:" + "\r\n";
    foreach (LoadCombination comb in combs)
    {
      //  Combinaton properties
      string usageNames = "[NONE]";
      for (int iUse = 0; (iUse <= (comb.NumberOfUsages - 1)); iUse++)
      {
        if ((iUse == 0))
        {
          usageNames = comb.get_UsageName(iUse);
        }
        else
        {
          usageNames = (usageNames + (";" + comb.get_UsageName(iUse)));
        }
      }
      sMsg = sMsg + "\r\n" + "  "+ comb.Name
           + ",  Id=" + comb.Id.Value.ToString()
           + "  Type=" + comb.CombinationType
           + "  TypeIndex=" + comb.CombinationTypeIndex
           + "  State=" + comb.CombinationState
           + "  StateIndex=" + comb.CombinationStateIndex
           + "  Usages=" + usageNames+ "\r\n";

      sMsg = sMsg + "  Number of components = "
            + comb.NumberOfComponents + ":" + "\r\n";
      //  Loop all components' propeties
      int iComp;
      for (iComp = 0; (iComp <= (comb.NumberOfComponents - 1)); iComp++)
      {
        sMsg = sMsg + "  Comp.name=" + comb.get_CombinationCaseName(iComp)
              + "  Comp.nature=" + comb.get_CombinationNatureName(iComp)
              + "  Factor=" + comb.get_Factor(iComp)
              + "\r\n";
      }
    }
    MessageBox.Show(sMsg);
    //  NOTE: Unlike RS2, from RS3 most of this objects are also create-able, see:
    //  Autodesk.Revit.Creation.Document.NewLoad*** methods. To call them, use something like:
    //   app.ActiveDocument.Create.NewLoadCase(...)
    //   app.ActiveDocument.Create.NewLoadCombination(...)
    //   app.ActiveDocument.Create.NewLoadNature(...)
    //   app.ActiveDocument.Create.NewLoadUsage(...)
    return IExternalCommand.Result.Succeeded; // added by joe
  }
}

//  List all Loads
public class Lab1_2 : IExternalCommand
{

  public Autodesk.Revit.IExternalCommand.Result Execute(Autodesk.Revit.ExternalCommandData commandData, ref string message, Autodesk.Revit.ElementSet elements) {
    Autodesk.Revit.Application app = commandData.Application;
    ElementSet loads = RstUtils.GetAllLoads(app);
    string sMsg = "There are "
          + loads.Size + " Load objects in the model:" + "\r\n";
    string sHost;

    foreach (LoadBase load in loads)
    {
      try {
        sHost = ", Host Id=" + load.HostElement.Id.Value.ToString();
      }
      catch
      {
        sHost = "";
      }
      sMsg = sMsg + "  "  + load.GetType().Name
            + ", Id="  + load.Id.Value.ToString()
            + sHost + "\r\n";
    }
    MessageBox.Show(sMsg);
    // We could loop the above collection, but better code another helper for Specific Loads directly
    ElementSet pointLoads = app.Create.NewElementSet();
    ElementSet lineLoads = app.Create.NewElementSet();
    ElementSet areaLoads = app.Create.NewElementSet();
    RstUtils.GetAllSpecificLoads(app, ref pointLoads, ref lineLoads, ref areaLoads);
    //  POINT
    sMsg = pointLoads.Size + " POINT Loads:" + "\r\n";

    foreach (PointLoad ptLd in pointLoads) {
      //  The following are all specific READ-ONLY properties:
      XYZ F = ptLd.Force;
      XYZ M = ptLd.Moment;
      XYZ pt = ptLd.Point;
      string nameLoadCase = ptLd.LoadCaseName;
      string nameLoadCategory = ptLd.LoadCategoryName;
      //  current bug in 8.1 - returns nothing
      string nameLoadNature = ptLd.LoadNatureName;
      sMsg = sMsg + "Id="
            + ptLd.Id.Value.ToString() + ": F= "  + F.X
            + ","
            + F.Y + ","
            + F.Z + " M= "
            + M.X + ","
            + M.Y + ","
            + M.Z + " Pt= "
            + pt.X + ","
            + pt.Y + ","
            + pt.Z
            + " LoadCase=" + nameLoadCase
            + " LoadCat="  + nameLoadCategory
            + " LoadNature="  + nameLoadNature
            + "\r\n";
    }
    MessageBox.Show(sMsg);
    //  LINE
    sMsg = lineLoads.Size + " LINE Loads:" + "\r\n";

    foreach (LineLoad lnLd in lineLoads) {
      //  The following are *some* READ-ONLY properties:
      XYZ F1 = lnLd.Force1;
      XYZ F2 = lnLd.Force2;
      //  can do similar for Moment1, Moment2...
      XYZ ptStart = lnLd.StartPoint;
      //  similar for EndPoint
      //  LoadxxxName same as for PointLoad (implemented in the base class)
      sMsg = sMsg + "Id="
            + lnLd.Id.Value.ToString() + ": F1= "  + F1.X + ","  + F1.Y + ","  + F1.Z
            + ": F2= " + F2.X + "," + F2.Y + "," + F2.Z
            + " Start= "  + ptStart.X + ","  + ptStart.Y + ","
            + ptStart.Z + " Uniform="
            + lnLd.UniformLoad + " Projected="
            + lnLd.ProjectedLoad + "\r\n";
    }
    MessageBox.Show(sMsg);
    //  AREA
    sMsg = areaLoads.Size + " AREA Loads:" + "\r\n";

    foreach (AreaLoad arLd in areaLoads) {
      //  The following are *some* READ-ONLY properties:
      XYZ F1 = arLd.Force1;
      int numLoops = arLd.NumLoops;
      string s2 = "Num.of Loops=" + numLoops
             + " Num.of Curves=" + arLd.get_NumCurves(0);
      int iLoop;
      //  if more than single Loop
      for (iLoop = 1; (iLoop  <= (numLoops - 1)); iLoop++) {
        s2 = (s2 + ("," + arLd.get_NumCurves(iLoop)));
      }
      sMsg = sMsg + "Id="  + arLd.Id.Value.ToString()
            + ": F1= "  + F1.X + "," + F1.Y + ","  + F1.Z
            + s2 + "\r\n";
      //  For curves geometry, see later commands...
    }
    MessageBox.Show(sMsg);
    return IExternalCommand.Result.Succeeded;
  }
}

//  More detailed info for *selected* loads
public class Lab1_3 : IExternalCommand
{

  public Autodesk.Revit.IExternalCommand.Result Execute(Autodesk.Revit.ExternalCommandData commandData, ref string message, Autodesk.Revit.ElementSet elements)
  {
    Autodesk.Revit.Application app = commandData.Application;

    ElementSetIterator iter = app.ActiveDocument.Selection.Elements.ForwardIterator();
    while (iter.MoveNext())
    {
      Autodesk.Revit.Element elem = iter.Current as Autodesk.Revit.Element;  //
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
    return IExternalCommand.Result.Succeeded;
  }
  public void ListAndModifyPointLoad(PointLoad ptLd)
  {
    string sMsg = "POINT Load Id=" + ptLd.Id.Value.ToString() + "\r\n";
    try
    {
      sMsg = sMsg + "  Is Reaction (via Display Name)= " + RacUtils.GetParamAsString(RacUtils.GetElemParam(ptLd, "Is Reaction")) + "\r\n";
      sMsg = sMsg + "  Load Case (via Display Name)= " + RacUtils.GetParamAsString(RacUtils.GetElemParam(ptLd, "Load Case")) + "\r\n";
      sMsg = sMsg + "  LOAD_IS_REACTION BuiltInParameter= " + RacUtils.GetParamAsString(ptLd.get_Parameter(BuiltInParameter.LOAD_IS_REACTION)) + "\r\n";
      sMsg = sMsg + "  LOAD_CASE_ID BuiltInParameter= " + RacUtils.GetParamAsString(ptLd.get_Parameter(BuiltInParameter.LOAD_CASE_ID)) + "\r\n";
      sMsg = sMsg + "  Orient to (via Display Name)= " + RacUtils.GetParamAsString(RacUtils.GetElemParam(ptLd, "Orient to")) + "\r\n";
      sMsg = sMsg + "  LOAD_USE_LOCAL_COORDINATE_SYSTEM BuiltInParameter= " + RacUtils.GetParamAsString(ptLd.get_Parameter(BuiltInParameter.LOAD_USE_LOCAL_COORDINATE_SYSTEM)) + "\r\n";
      try
      {
        sMsg = sMsg + "  LOAD_USE_LOCAL_COORDINATE_SYSTEM_HOSTED BuiltInParameter= " + RacUtils.GetParamAsString(ptLd.get_Parameter(BuiltInParameter.LOAD_USE_LOCAL_COORDINATE_SYSTEM_HOSTED)) + "\r\n";
      }
      catch
      {
      }
      Parameter paramFz = RacUtils.GetElemParam(ptLd, "Fz");
      double Fz_old = paramFz.AsDouble();
      paramFz.Set((double)(2.0 * Fz_old));
      sMsg = sMsg + "  Fz:  OLD=" + Fz_old.ToString() + " NEW=" + paramFz.AsDouble().ToString() + "\r\n";
      try
      {
        sMsg = sMsg + "  LOAD_IS_CREATED_BY_API BuiltInParameter= " + RacUtils.GetParamAsString(ptLd.get_Parameter(BuiltInParameter.LOAD_IS_CREATED_BY_API)) + "\r\n";
      }
      catch
      {

      }
      try
      {
        sMsg = sMsg + "  ELEM_TYPE_PARAM BuiltInParameter= " + RacUtils.GetParamAsString(ptLd.get_Parameter(BuiltInParameter.ELEM_TYPE_PARAM)) + "\r\n";
      }
      catch
      {

      }
    }
    catch (Exception ex)
    {
      MessageBox.Show("Error in ListAndModifyPointLoad: " + ex.Message);
    }
    finally
    {
      MessageBox.Show(sMsg);
    }
  }

  public void ListAndModifyLineLoad(LineLoad lnLd, Autodesk.Revit.Application app)
  {
    string sMsg = "LINE Load Id=" + lnLd.Id.Value.ToString() + "\r\n";
    try
    {
      sMsg = sMsg + "  Load Case (via Display Name)= " + RacUtils.GetParamAsString(RacUtils.GetElemParam(lnLd, "Load Case")) + "\r\n";
      sMsg = sMsg + "  LOAD_CASE_ID BuiltInParameter= " + RacUtils.GetParamAsString(lnLd.get_Parameter(BuiltInParameter.LOAD_CASE_ID)) + "\r\n";
      Parameter paramFZ1 = lnLd.get_Parameter(BuiltInParameter.LOAD_LINEAR_FORCE_FZ1);
      double FZ1_old = paramFZ1.AsDouble();
      paramFZ1.Set((double)(2.0 * FZ1_old));
      sMsg = sMsg + "  FZ1:  OLD=" + FZ1_old.ToString() + " NEW=" + paramFZ1.AsDouble().ToString() + "\r\n";
      Parameter hostIdParam = lnLd.get_Parameter(BuiltInParameter.HOST_ID_PARAM);
      if (hostIdParam != null)
      {
        ElementId hostId = hostIdParam.AsElementId();
        Autodesk.Revit.Element elem = app.ActiveDocument.get_Element(ref hostId);
        sMsg = sMsg + "  HOST element ID=" + elem.Id.Value.ToString() + " Class=" + elem.GetType().Name + " Name=" + elem.Name + "\r\n";
      }
      else
      {
        sMsg = sMsg + "  NO HOST element\r\n";
      }
      sMsg = sMsg + "  Orient to (via Display Name)= " + RacUtils.GetParamAsString(RacUtils.GetElemParam(lnLd, "Orient to")) + "\r\n";
      sMsg = sMsg + "  LOAD_USE_LOCAL_COORDINATE_SYSTEM BuiltInParameter= " + RacUtils.GetParamAsString(lnLd.get_Parameter(BuiltInParameter.LOAD_USE_LOCAL_COORDINATE_SYSTEM)) + "\r\n";
      try
      {
        sMsg = sMsg + "  LOAD_USE_LOCAL_COORDINATE_SYSTEM_HOSTED BuiltInParameter= " + RacUtils.GetParamAsString(lnLd.get_Parameter(BuiltInParameter.LOAD_USE_LOCAL_COORDINATE_SYSTEM_HOSTED)) + "\r\n";
      }
      catch
      {

      }
      try
      {
        sMsg = sMsg + "  LOAD_IS_CREATED_BY_API BuiltInParameter= " + RacUtils.GetParamAsString(lnLd.get_Parameter(BuiltInParameter.LOAD_IS_CREATED_BY_API)) + "\r\n";
      }
      catch
      {

      }
    }
    catch (Exception ex)
    {
      MessageBox.Show("Error in ListAndModifyLineLoad: " + ex.Message);

    }
    finally
    {
      MessageBox.Show(sMsg);
    }
  }

  public void ListAndModifyAreaLoad(AreaLoad arLd, Autodesk.Revit.Application app)
  {
    string sMsg = "AREA Load Id=" + arLd.Id.Value.ToString() + "\r\n";
    try
    {
      sMsg = sMsg + "  Load Case (via Display Name)= " + RacUtils.GetParamAsString(RacUtils.GetElemParam(arLd, "Load Case")) + "\r\n";
      sMsg = sMsg + "  LOAD_CASE_ID BuiltInParameter= " + RacUtils.GetParamAsString(arLd.get_Parameter(BuiltInParameter.LOAD_CASE_ID)) + "\r\n";
      Parameter paramFZ1 = arLd.get_Parameter(BuiltInParameter.LOAD_AREA_FORCE_FZ1);
      double FZ1_old = paramFZ1.AsDouble();
      paramFZ1.Set((double) (2.0 * FZ1_old));
      sMsg = sMsg + "  FZ1:  OLD=" + FZ1_old.ToString() + " NEW=" + paramFZ1.AsDouble().ToString() + "\r\n";
      int numLoops = arLd.NumLoops;
      sMsg = sMsg + "\r\n  Number Of Loops = " + numLoops.ToString() + "\r\n";
      for (int iLoop = 0; iLoop <= numLoops - 1; iLoop++)
      {
        int numCurves = arLd.get_NumCurves(iLoop);
        sMsg = sMsg + "  Loop " + (iLoop + 1) + " has " + numCurves.ToString() + " curves:\r\n";
        for (int iCurve = 0; iCurve <= numCurves - 1; iCurve++)
        {
          Curve crv = arLd.get_Curve(iLoop, iCurve);
          if (crv is Line)
          {
            Line line = (Line) crv;
            XYZ ptS = line.get_EndPoint(0);
            XYZ ptE = line.get_EndPoint(1);
            sMsg = sMsg + "    Curve " +  (iCurve + 1) + " is a LINE:" + ptS.X.ToString() + ", " + ptS.Y.ToString() + ", " + ptS.Z.ToString() + " ; " + ptE.X.ToString() + ", " + ptE.Y.ToString() + ", " + ptE.Z.ToString() + "\r\n";
          }
          else if (crv is Arc)
          {
            Arc arc = (Arc) crv;
            XYZ ptS = arc.get_EndPoint(0);
            XYZ ptE = arc.get_EndPoint(1);
            double r = arc.Radius;
            sMsg = sMsg + "    Curve " + (iCurve + 1) + " is an ARC:" + ptS.X.ToString() + ", " + ptS.Y.ToString() + ", " + ptS.Z.ToString() + " ; " + ptE.X.ToString() + ", " + ptE.Y.ToString() + ", " + ptE.Z.ToString() + " ; R=" + r.ToString() + "\r\n";
          }
        }
      }
      int numRefPts = arLd.NumRefPoints;
      sMsg = sMsg + "\r\n  Number Of Ref.Points = " + numRefPts.ToString() + "\r\n";

      for (int iRefPt = 0; iRefPt <= numRefPts - 1; iRefPt++)
      {
        XYZ pt = arLd.get_RefPoint(iRefPt);
        sMsg = sMsg + "  RefPt " +  (iRefPt + 1) + " = " + pt.X.ToString() + ", " + pt.Y.ToString() + ", " + pt.Z.ToString() + "\r\n";
      }
    }
    catch (Exception ex)
    {
      MessageBox.Show("Error in ListAndModifyAreaLoad: " + ex.Message);
    }
    finally
    {
      MessageBox.Show(sMsg);
    }
  }

}

//  List all Load Symbols
public class Lab1_4 : IExternalCommand
{
  public Autodesk.Revit.IExternalCommand.Result Execute(Autodesk.Revit.ExternalCommandData commandData, ref string message, Autodesk.Revit.ElementSet elements)
  {
    Autodesk.Revit.Application app = commandData.Application;
    ElementSet symbs = RstUtils.GetAllLoadSymbols(app);
    string sMsg = "There are "  + symbs.Size + " Load Symbol objects in the model:" + "\r\n";

    foreach (Symbol ls in symbs)
    {
      //  There are no dedicated classes like PointLoadSymbol, LineLoadSymbol or AreaLoadSymbol, so
      //  we need to check the Family name Param!
      string famName = "?";
      try {
        //  this worked in RS2, but failing now in RS3, works fine in 2008
        famName = ls.get_Parameter(BuiltInParameter.SYMBOL_FAMILY_NAME_PARAM).AsString();
      }
      catch
      {
      }

      sMsg = sMsg + "  "+ ls.Name + ", Id="+ ls.Id.Value.ToString()
             + " Family="+ famName  + "\r\n";

    }
    MessageBox.Show(sMsg);

    //  We better make utilities (or alternatively single one for all 3 like for Loads) that will get particular Symbols.
    //  For example, only for POINT Load Symbols:
    ElementSet ptLdSymbs = RstUtils.GetPointLoadSymbols(app);
    sMsg = ("Point Load Symbols directly:" + "\r\n");
    foreach (Symbol ls1 in ptLdSymbs)
    {
      //  Get one specific param for this symbol (can also change it if needed, like for load objects)
      string scaleF = RacUtils.GetParamAsString(ls1.get_Parameter(BuiltInParameter.LOAD_ATTR_FORCE_SCALE_FACTOR));
      sMsg = sMsg + "  "+ ls1.Name + ", Id="+ ls1.Id.Value.ToString()
             + " Force Scale="+ scaleF + "\r\n";
    }
    MessageBox.Show(sMsg);

    return IExternalCommand.Result.Succeeded;

  }
}

//  Create NEW Load
public class Lab1_5 : IExternalCommand
{

  public Autodesk.Revit.IExternalCommand.Result Execute(Autodesk.Revit.ExternalCommandData commandData, ref string message, Autodesk.Revit.ElementSet elements) {
    Autodesk.Revit.Application app = commandData.Application;
    //  Loads are not standard Families and FamilySymbols!!!
    // Id=42618; Class=Symbol; Category=Structural Loads; Name=Point Load 1
    // Id=42623; Class=Symbol; Category=Structural Loads; Name=Line Load 1
    // Id=42632; Class=Symbol; Category=Structural Loads; Name=Area Load 1
    // Id=126296; Class=Symbol; Category=Structural Loads; Name=MS load
    // Id=129986; Class=Symbol; Category=Structural Loads; Name=MS AL 2
    // ' Hence cannot use anything like NewFamilyInstance...
    // ...but there are DEDICATED METHODS:
    Autodesk.Revit .Creation.Document creationDoc = app.ActiveDocument.Create;
    //  REACTION (will be read-only in UI)
    XYZ pt = new XYZ(0, 0, 0);
    XYZ f = new XYZ(0, 0, 14593.9);
    //  eq.to 1 kip
    XYZ m = new XYZ(0, 0, 0);
    // Dim newPointLoadReaction As PointLoad = creationDoc.NewPointLoad(pt, f, m, True) ' 2008
    PointLoad newPointLoadReaction = creationDoc.NewPointLoad(pt, f, m, true, null, null);
    //  2009 ... todo: test this!
    //  This one will have default Symbol and no Load case
    //  EXTERNAL Force
    // Dim newPointLoadExternal As PointLoad = creationDoc.NewPointLoad(New XYZ(5, 5, 0), New XYZ(0, 0, -30000), New XYZ(0, 0, 0), False) ' 2008
    PointLoad newPointLoadExternal = creationDoc.NewPointLoad(new XYZ(5, 5, 0), new XYZ(0, 0, -30000), new XYZ(0, 0, 0), false, null, null);
    //  2009 ... todo: test this!
    //  doesn't work any longer in RS3!!!
    //  seems ok again in 2008
    // ' Ask user to select SYMBOL for the new Load (loop questions to avoid custom forms)
    ElementSet ptLdSymbs = RstUtils.GetPointLoadSymbols(app);
    bool gotSymb = false;
    while (!gotSymb)
    {
      foreach (Symbol sym in ptLdSymbs)
      {
        switch (MessageBox.Show(("Use PointLoad Symbol "
                + (sym.Name + "?")), "Select Symbol for the New Load", MessageBoxButtons.YesNoCancel))
        {
          case DialogResult.Cancel:
            return IExternalCommand.Result.Cancelled;
          case DialogResult.Yes:
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
    //  Ask user to select LOAD CASE for the new Load (loop questions to avoid custom forms)
    //  Load CASES
    ElementSet ldCases = RstUtils.GetAllLoadCases(app);
    bool gotCase = false;
    while (!gotCase) {

      foreach (LoadCase ldCase in ldCases)
      {
        switch (MessageBox.Show(("Assign to Load Case "
                + (ldCase.Name + "?")), "Select Load Case for the New Load", MessageBoxButtons.YesNoCancel))
        {
          case DialogResult.Cancel:
            return IExternalCommand.Result.Cancelled;
          case DialogResult.Yes:
            ElementId ldCaseid = ldCase.Id;
            newPointLoadExternal.get_Parameter(BuiltInParameter.LOAD_CASE_ID).Set(ref ldCaseid);
            gotCase = true;
            break; // TODO: Warning!!! Review that break works as 'Exit For' as it is inside another 'breakable' statement:Switch
        }
        if (gotCase == true)
          break;
      }
    }
    return IExternalCommand.Result.Succeeded;
  }
}
