using System;
using System.Collections;
using System.Collections.Generic;

using Autodesk;
using Autodesk.Revit;
using Autodesk.Revit.Parameters;
using Autodesk.Revit.Structural;
using Autodesk.Revit.Utility;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Symbols;
using System.Windows.Forms;

//  Utils from standard RevitAPI class (some will be reused here)

public class RacUtils
{
  public static ElementSet ConvertElementListToSet( Autodesk.Revit.Application revitApp, List<Element> listElement )
  {
    ElementSet elemSet = revitApp.Create.NewElementSet();
    foreach( Element elem in listElement )
    {
      elemSet.Insert( elem );
    }
    return elemSet;
  }

  //  Helper to get all geometrical elements
  static ElementSet GetAllModelElements(Autodesk.Revit.Application revitApp)
  {
    ElementSet elems = revitApp.Create.NewElementSet();
    IEnumerator iter = revitApp.ActiveDocument.Elements;
    while (iter.MoveNext())
    {
      Autodesk.Revit.Element elem = iter.Current as Autodesk.Revit.Element;
      //  This single line would probably work if all system families were exposed as HostObjects, but they are not yet
      // If TypeOf elem Is FamilyInstance Or TypeOf elem Is HostObject Then
      if (!((elem is Symbol)
            || (elem is FamilyBase)))
      {
        if (!(elem.Category == null))
        {
          Autodesk.Revit .Geometry.Options opt = revitApp.Create.NewGeometryOptions();
          opt.DetailLevel = Autodesk.Revit .Geometry.Options.DetailLevels.Medium;
          Autodesk.Revit .Geometry.Element geo = elem.get_Geometry(opt);
          if (!(geo == null))
          {
            elems.Insert(elem);
          }
        }
      }
    }
    return elems;
  }

  //  Helper to get all Walls
  static ElementSet GetAllWalls(Autodesk.Revit.Application revitApp)
  {
    //The following commented code is for Revit 2008 and previous version. It works in Revit 2009 too. However this method has a low performance.
    //There is a new feature named Element filter in Revit 2009 API, which can improve the performance.

    //ElementSet elems = revitApp.Create.NewElementSet();
    //IEnumerator iter = revitApp.ActiveDocument.Elements;
    //while (iter.MoveNext())
    //{
    //  Autodesk.Revit.Element elem = iter.Current as Autodesk.Revit.Element;
    //  //  For Wall (one of the Host objects), there is a specific class!
    //  if ((elem is Wall))
    //  {
    //    elems.Insert(elem);
    //  }
    //}
    //return elems;

    //getting all LoadNatures via element filter that provided in Revit 2009 API. This is quicker and simplerr.
    List<Element> listWalls = new List<Element>();
    Filter filterType = revitApp.Create.Filter.NewTypeFilter(typeof(Wall));
    revitApp.ActiveDocument.get_Elements(filterType, listWalls);
    return ConvertElementListToSet(revitApp, listWalls);
  }

  public static ElementSet GetAllStandardFamilyInstancesForACategory(Autodesk.Revit.Application revitApp, string catName)
  {
    //The following commented code is for Revit 2008 and previous version. It works in Revit 2009 too. However this method has a low performance.
    //There is a new feature named Element filter in Revit 2009 API, which can improve the performance.
    //ElementSet elems = revitApp.Create.NewElementSet();
    //IEnumerator iter = revitApp.ActiveDocument.Elements;
    //while (iter.MoveNext())
    //{
    //  Element elem = (Element)iter.Current;
    //  if (elem is FamilyInstance)
    //  {
    //    try
    //    {
    //      if (elem.Category.Name.Equals(catName))
    //      {
    //        elems.Insert(elem);
    //      }
    //    }
    //    catch
    //    {
    //    }
    //  }
    //}

    //
    //getting all specific elements via element filter that provided in Revit 2009 API. This is quicker and simplerr.
    //
    List<Element> listFamilyInstances = new List<Element>();
    Category cat = revitApp .ActiveDocument .Settings .Categories .get_Item (catName);
    Filter filterCategory = revitApp.Create.Filter.NewCategoryFilter(cat);
    Filter filterType = revitApp.Create.Filter.NewTypeFilter(typeof(FamilyInstance));
    Filter filterAnd = revitApp.Create.Filter.NewLogicAndFilter(filterCategory, filterType);
    revitApp.ActiveDocument.get_Elements(filterAnd, listFamilyInstances);
    return ConvertElementListToSet(revitApp, listFamilyInstances);
  }

  //  Helper to get specified Type for specified Family as FamilySymbol object
  //   (in theory, we should also check for the correct *Category Name*)
  public static FamilySymbol GetFamilySymbol(Autodesk.Revit.Application revitApp,Document doc, string familyName, string typeName)
  {
    //The following commented code is for Revit 2008 and previous version. It works in Revit 2009 too. However this method has a low performance.
    //There is a new feature named Element filter in Revit 2009 API, which can improve the performance.

    //ElementIterator iter = doc.Elements;
    //while (iter.MoveNext())
    //{
    //  Element elem = (Element) iter.Current;

    //
    //getting all specific elements via element filter that provided in Revit 2009 API. This is quicker and simplerr.
    //
    Filter filterFamily = revitApp.Create.Filter.NewFamilyFilter( familyName );
    List<Element> listFamilies = new List<Element>();
    doc.get_Elements( filterFamily, listFamilies );
    foreach( Element elem in listFamilies )
    {
      if (elem is Family)
      {
        Family fam = (Family) elem;
        if (fam.Name.Equals(familyName))
        {
          foreach (FamilySymbol sym in fam.Symbols)
          {
            if (sym.Name.Equals(typeName))
            {
              return sym;
            }
          }
        }
      }
    }
    return null;
  }

  public static string GetParamAsString( Parameter param )
  {
    switch (param.StorageType)
    {
      case StorageType.None:
        return "?NONE?";

      case StorageType.Integer:
        return param.AsInteger().ToString();

      case StorageType.Double:
        return param.AsDouble().ToString();

      case StorageType.String:
        return param.AsString();

      case StorageType.ElementId:
        return param.AsElementId().Value.ToString();
    }
    return "?ELSE?";
  }

  //Helper to get *specific* parameter by name
  public static Parameter GetElemParam(Autodesk.Revit .Element elem, string name)
  {
    // The following commented code is for Revit 2008 and previous version. It works in Revit 2009 too.
    // Revit 2009 API provide a overload method to get the Parameter via the parameter name string. It is much faster.
    //ParameterSet parameters = elem.Parameters;
    //foreach (Parameter parameter in parameters)
    //{
    //  if (parameter.Definition.Name == name)
    //  {
    //    return parameter;
    //  }
    //}
    //return null;

    // We use the new added overload method to retrieve the Parameter via the name string. This only works in Revit 2009 onward.
    Parameter para  = elem.get_Parameter (name);
    return para;
  }

  // Helpers for the Shared Parameters:
  // -------------------------------------
  // Shared Params FILE
  public static DefinitionFile GetSharedParamsFile(Autodesk.Revit.Application revitApp)
  {
    DefinitionFile sharedParametersFile;
    try
    {
      string sharedParamsFileName = revitApp.Options.SharedParametersFilename;
    }
    catch
    {
      MessageBox.Show("No Shared params file set !?");
      return null;
    }
    try
    {
      sharedParametersFile = revitApp.OpenSharedParameterFile();
    }
    catch
    {
      MessageBox.Show("Cannnot open Shared Params file !?");
      sharedParametersFile = null;
    }
    return sharedParametersFile;
  }

  // Shared Params GROUP
  public static DefinitionGroup GetOrCreateSharedParamsGroup(DefinitionFile sharedParametersFile, string groupName)
  {
    DefinitionGroup msProjectGroup = sharedParametersFile.Groups.get_Item(groupName);
    if (msProjectGroup == null)
    {
      try
      {
        msProjectGroup = sharedParametersFile.Groups.Create(groupName);
      }
      catch
      {
        msProjectGroup = null;
      }
    }
    return msProjectGroup;
  }

  // Shared Params DEFINITION
  public static Definition GetOrCreateSharedParamsDefinition(DefinitionGroup defGroup, ParameterType defType, string defName, bool visible)
  {
    Definition definition = defGroup.Definitions.get_Item(defName);
    if (definition == null)
    {
      try
      {
        definition = defGroup.Definitions.Create(defName, defType, visible);
      }
      catch
      {

        definition = null;

      }
    }
    return definition;
  }

  // Get GUID for a given shared param name
  public static Guid SharedParamGUID(Autodesk.Revit.Application revitApp, string defGroup, string defName)
  {
    Guid guid = Guid.Empty;
    try
    {
      ExternalDefinition externalDefinition = (ExternalDefinition) revitApp.OpenSharedParameterFile().Groups.get_Item(defGroup).Definitions.get_Item(defName);
      guid = externalDefinition.GUID;
    }
    catch
    {

    }
    return guid;
  }

  // Helper to get all Groups
  public static ElementSet GetAllGroups(Autodesk.Revit.Application revitApp)
  {
    //The following commented code is for Revit 2008 and previous version. It works in Revit 2009 too. However this method has a low performance.
    //There is a new feature named Element filter in Revit 2009 API, which can improve the performance.
    //ElementSet elems = revitApp.Create.NewElementSet();
    //IEnumerator iter = revitApp.ActiveDocument.Elements;
    //while (iter.MoveNext())
    //{
    //  Element elem = (Element)iter.Current;
    //  if (elem is Group)
    //  {
    //    elems.Insert(elem);
    //  }
    //}
    //return elems;

    //
    //getting all Groups via element filter that provided in Revit 2009 API. This is quicker and simpler.
    //
    List<Element> listGroups = new List<Element>();
    Filter filterType = revitApp.Create.Filter.NewTypeFilter(typeof(Group));
    revitApp.ActiveDocument.get_Elements(filterType, listGroups);
    return ConvertElementListToSet(revitApp, listGroups);

  }

  // Helper to get all Group Types
  public static ElementSet GetAllGroupTypes(Autodesk.Revit.Application revitApp)
  {
    //The following commented code is for Revit 2008 and previous version. It works in Revit 2009 too. However this method has a low performance.
    //There is a new feature named Element filter in Revit 2009 API, which can improve the performance.
    //ElementSet elems = revitApp.Create.NewElementSet();
    //IEnumerator iter = revitApp.ActiveDocument.Elements;
    //while (iter.MoveNext())
    //{
    //  Element elem = (Element) iter.Current;
    //  if (elem is GroupType)
    //  {
    //    elems.Insert(elem);
    //  }
    //}
    //return elems;

    //
    //getting all GroupTypes via element filter that provided in Revit 2009 API. This is quicker and simpler.
    //
    List<Element> listGroupTypes = new List<Element>();
    Filter filterType = revitApp.Create.Filter.NewTypeFilter(typeof(GroupType));
    revitApp.ActiveDocument.get_Elements(filterType, listGroupTypes);
    return ConvertElementListToSet(revitApp, listGroupTypes);
  }

  // Helper to get all *Model* Group Types
  public static ElementSet GetAllModelGroupTypes(Autodesk.Revit.Application revitApp)
  {
    ElementSet elems = revitApp.Create.NewElementSet();
    //The following commented code is for Revit 2008 and previous version. It works in Revit 2009 too. However this method has a low performance.
    //There is a new feature named Element filter in Revit 2009 API, which can improve the performance.

    //IEnumerator iter = revitApp.ActiveDocument.Elements;
    //while (iter.MoveNext())
    //{
    //  Element elem = (Element) iter.Current;
    List<Element> listGroupTypes = new List<Element>();
    Filter filterType = revitApp.Create.Filter.NewTypeFilter(typeof(GroupType));
    revitApp.ActiveDocument.get_Elements(filterType, listGroupTypes);
    foreach (Element elem in listGroupTypes )
    {
      if (elem is GroupType)
      {
        GroupType gt = (GroupType) elem;
        try
        {
          if (gt.get_Parameter(BuiltInParameter.SYMBOL_FAMILY_NAME_PARAM).AsString().Equals("Model Group"))
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
