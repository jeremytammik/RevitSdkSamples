#region Copyright
//
// (C) Copyright 2010 by Autodesk, Inc.
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted,
// provided that the above copyright notice appears in all copies and
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
//
// Migrated to C# by Adam Nagy 
// 
#endregion // Copyright

#region "Imports" 
//' Import the following name spaces in the project properties/references. 
//' Note: VB.NET has a slighly different way of recognizing name spaces than C#. 
//' if you explicitely set them in each .vb file, you will need to specify full name spaces. 

using System; 
using Autodesk.Revit.DB; 
using Autodesk.Revit.UI; 
using Autodesk.Revit.ApplicationServices; 
using Autodesk.Revit.Attributes; //'' specific this if you want to save typing for attributes. e.g., 
using System.Collections.Generic;
using System.Linq; 

#endregion 

#region "Description" 
//' Revit Intro Lab 3 
//' 
//' In this lab, you will learn how to filter elements 
//' in the previous lab, we have learned how an element is represnted in the revit database. 
//' we learned how to retrieve information, and identify the kind of elements. 
//' in this lab, we'll take a look how to filter element from the database. 
//' Disclaimer: minimum error checking to focus on the main topic. 
//' 
//' MH: my scale(77) 
//'-------1---------2---------3---------4---------5---------6---------7------- 
//' 
//'-------1---------2---------3----------4---------5--------6--------- 
//' 
#endregion 

namespace RevitIntroCS
{
  //' ElementFiltering - 
  //' 
  [Transaction(TransactionMode.Automatic)]
  [Regeneration(RegenerationOption.Manual)]
  public class ElementFiltering : IExternalCommand
  {
    //' member variables 
    Application m_rvtApp;
    Document m_rvtDoc;

    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
      //' Get the access to the top most objects. 
      UIApplication rvtUIApp = commandData.Application;
      UIDocument rvtUIDoc = rvtUIApp.ActiveUIDocument;
      m_rvtApp = rvtUIApp.Application;
      m_rvtDoc = rvtUIDoc.Document;

      //' (1) In eailer lab, CommandData command, we learned how to access to the wallType. i.e., 
      //' here we'll take a look at more on the topic of accessing to elements in the interal rvt project database. 
      ListFamilyTypes();

      //' (2) list instances of specific object class. 
      ListInstances();

      //' (3) find a specific family type. 
      FindFamilyType();

      //' (4) find specific instances, including filtering by parameters. 
      FindInstance();

      //' we are done. 

      return Result.Succeeded;
    }

    //' list the family types 
    //' 
    public void ListFamilyTypes()
    {
      //' 
      //' (1) get a list of family types available in the current rvt project. 
      //' 
      //' For system family types, there is a designated properties that allows us to directly access to the types. 
      //' e.g., rvtDoc.WallTypes 
      //' 
      WallTypeSet wallTypes = m_rvtDoc.WallTypes;

      //' show it. 
      string sWallTypes = "Wall Types (by rvtDoc.WallTypes): " + wallTypes.Size.ToString() + "\n" + "\n";
      foreach (WallType wType in wallTypes)
      {
        sWallTypes = sWallTypes + wType.Kind.ToString() + " : " + wType.Name + "\n";
      }
      TaskDialog.Show("Revit Intro Lab", sWallTypes);


      //' (1.1) same idea applies to other system family, such as Floors, Roofs. 
      //' 
      FloorTypeSet floorTypes = m_rvtDoc.FloorTypes;

      //' show it. 
      var sFloorTypes = "Floor Types (by rvtDoc.FloorTypes): " + floorTypes.Size.ToString() + "\n" + "\n";
      foreach (FloorType fType in floorTypes)
      {
        //' Family name is not in the property for floor. so use BuiltInParameter here. 
        Parameter param = fType.get_Parameter(BuiltInParameter.SYMBOL_FAMILY_NAME_PARAM);
        if (param != null)
        {
          sFloorTypes = sFloorTypes + param.AsString();
        }
        sFloorTypes = sFloorTypes + " : " + fType.Name + "\n";
      }
      TaskDialog.Show("Revit Intro Lab", sFloorTypes);


      //' (1.2a) another approach is to use a filter. here is an example with wall type. 
      //' 
      var wallTypeCollector1 = new FilteredElementCollector(m_rvtDoc);
      wallTypeCollector1.WherePasses(new ElementClassFilter(typeof(WallType)));
      IList<Element> wallTypes1 = wallTypeCollector1.ToElements();

      //' using a helper funtion to display the result here. See code below. 
      ShowElementList(wallTypes1, "Wall Types (by Filter): ");
      //' use helper function. 

      //' (1.2b) the following are the same as two lines above. 
      //' these alternative forms are provided for convenience. 
      //' using OfClass() 
      //' 
      //FilteredElementCollector wallTypeCollector2 = new FilteredElementCollector(m_rvtDoc);
      //wallTypeCollector2.OfClass(typeof(WallType)); 

      //' (1.2c) the following are the same as above. For convenience. 
      //' using short cut this time. 
      //' 
      //FilteredElementCollector wallTypeCollector3 = new FilteredElementCollector(m_rvtDoc).OfClass(typeof(WallType)); 

      //' 
      //' (2) Listing for component family types. 
      //' 
      //' for component family. it is slightly different. 
      //' There is no designate property in the document class. 
      //' you always need to use a filtering. 
      //' for example, doors and windows. 
      //' remember for component family, you will need to check element type and category 
      //' this time using OfClass(). 
      //' 
      var doorTypeCollector = new FilteredElementCollector(m_rvtDoc);
      doorTypeCollector.OfClass(typeof(FamilySymbol));
      doorTypeCollector.OfCategory(BuiltInCategory.OST_Doors);
      IList<Element> doorTypes = doorTypeCollector.ToElements();

      ShowElementList(doorTypes, "Door Types (by Filter): ");
    }

    //' To get a list of instances of a specific family type, you will need to use filters. 
    //' The same idea that we learned for family types applies for instances as well. 
    //' 
    public void ListInstances()
    {
      //' list all the wall instances 
      var wallCollector = new FilteredElementCollector(m_rvtDoc).OfClass(typeof(Wall));
      IList<Element> wallList = wallCollector.ToElements();

      ShowElementList(wallList, "Wall Instances: ");

      //' list all the door instances 
      var doorCollector = new FilteredElementCollector(m_rvtDoc).OfClass(typeof(FamilyInstance));
      doorCollector.OfCategory(BuiltInCategory.OST_Doors);
      IList<Element> doorList = doorCollector.ToElements();

      ShowElementList(doorList, "Door Instance: ");
    }

    //' 
    //' Looks at a way to get to the more specific family types with a given name. 
    //' 
    //' 
    public void FindFamilyType()
    {
      //' In this exercise, we will look for the following family types for wall and door 
      //' Hard coding for similicity. modify here if you want to try out with other family types. 

      //' Constant to this function. 
      //' this is for wall. e.g., "Basic Wall: Generic - 200mm" 
      const string wallFamilyName = "Basic Wall";
      const string wallTypeName = "Generic - 200mm";
      const string wallFamilyAndTypeName = wallFamilyName + ": " + wallTypeName;

      //' this is for door. e.g., "M_Single-Flush: 0915 x 2134mm 
      const string doorFamilyName = "M_Single-Flush";
      const string doorTypeName = "0915 x 2134mm";
      const string doorFamilyAndTypeName = doorFamilyName + ": " + doorTypeName;

      //' keep messages to the user in this function. 
      string msg = "Find Family Type - All -: " + "\n" + "\n";

      //' (1) get a specific system family type. e.g., wall type. 
      //' There are a few different ways to do this. 

      //' (1.1) first version uses LINQ query. 
      //' 
      ElementType wallType1 = (ElementType)FindFamilyType_Wall_v1(wallFamilyName, wallTypeName);

      //' show the result. 
      msg = msg + ShowFamilyTypeAndId("Find wall family type (using LINQ): ", wallFamilyAndTypeName, wallType1) + "\n";

      //' 
      //' (1.2) Another way is to use iterator. (cf. look for example, Developer guide 87) 
      //' 
      ElementType wallType2 = (ElementType)FindFamilyType_Wall_v2(wallFamilyName, wallTypeName);

      msg = msg + ShowFamilyTypeAndId("Find wall family type (using iterator): ", wallFamilyAndTypeName, wallType2) + "\n";

      //' (2) get a specific component family type. e.g., door type. 
      //' 
      //' (2.1) similar approach as (1.1) using LINQ. 
      //' 
      ElementType doorType1 = (ElementType)FindFamilyType_Door_v1(doorFamilyName, doorTypeName);

      msg = msg + ShowFamilyTypeAndId("Find door type (using LINQ): ", doorFamilyAndTypeName, doorType1) + "\n";

      //' (2.2) get a specific door type. the second approach. 
      //' another approach will be to look up from Family, then from Family.Symbols property. 
      //' This gets more complicated although it is logical approach. 
      //' 
      ElementType doorType2 = (ElementType)FindFamilyType_Door_v2(doorFamilyName, doorTypeName);

      msg = msg + ShowFamilyTypeAndId("Find door type (using Family): ", doorFamilyAndTypeName, doorType2) + "\n";

      //' (3) here is more generic form. defining a more generalized function below. 
      //' 
      //' (3.1) for the wall type 
      //' 
      ElementType wallType3 = (ElementType)FindFamilyType(m_rvtDoc, typeof(WallType), wallFamilyName, wallTypeName, null);

      msg = msg + ShowFamilyTypeAndId("Find wall type (using generic function): ", wallFamilyAndTypeName, wallType3) + "\n";

      //' (3.2) for the door type. 
      //' 
      ElementType doorType3 = (ElementType)FindFamilyType(m_rvtDoc, typeof(FamilySymbol), doorFamilyName, doorTypeName, BuiltInCategory.OST_Doors);

      msg = msg + ShowFamilyTypeAndId("Find door type (using generic function): ", doorFamilyAndTypeName, doorType3) + "\n";

      //' Finally, show the result all together 

      TaskDialog.Show("Revit Intro Lab", msg);
    }

    //' Find a specific family type for a wall with a given family and type names. 
    //' This version uses LINQ query. 
    //' 
    public Element FindFamilyType_Wall_v1(string wallFamilyName, string wallTypeName)
    {
      //' narrow down a collector with class. 
      var wallTypeCollector1 = new FilteredElementCollector(m_rvtDoc);
      wallTypeCollector1.OfClass(typeof(WallType));

      //' LINQ query 
      var wallTypeElems1 =
          from element in wallTypeCollector1
          where element.Name.Equals(wallTypeName)
          select element;

      //' get the result. 
      Element wallType1 = null;
      //' result will go here. 
      //' (1) directly accessing from the query result. 
      if (wallTypeElems1.Count<Element>() > 0)
      {
        wallType1 = wallTypeElems1.First<Element>();
      }

      //' (2) if you want to get the result as a list of element, here is how. 
      //Dim wallTypeList1 As IList(Of Element) = wallTypeElems1.ToList() 
      //If wallTypeList1.Count > 0 Then 
      // wallType1 = wallTypeList1(0) ' found it. 
      //End If 

      return wallType1;
    }

    //' Find a specific family type for a wall, which is a system family. 
    //' This version uses iteration. (cf. look for example, Developer guide 87) 
    //' 
    public Element FindFamilyType_Wall_v2(string wallFamilyName, string wallTypeName)
    {
      //' first, narrow down the collector by Class 
      var wallTypeCollector2 = new FilteredElementCollector(m_rvtDoc).OfClass(typeof(WallType));

      //' use iterator 
      FilteredElementIterator wallTypeItr = wallTypeCollector2.GetElementIterator();
      wallTypeItr.Reset();
      Element wallType2 = null;
      while (wallTypeItr.MoveNext())
      {
        WallType wType = (WallType)wallTypeItr.Current;
        //' we check two names for the match: type name and family name. 
        if ((wType.Name == wallTypeName) & (wType.get_Parameter(BuiltInParameter.SYMBOL_FAMILY_NAME_PARAM).AsString().Equals(wallFamilyName)))
        {
          wallType2 = wType;
          //' we found it. 
          break; // TODO: might not be correct. Was : Exit While 
        }
      }

      return wallType2;
    }

    //' Find a specific family type for a door, which is a component family. 
    //' This version uses LINQ. 
    //' 
    public Element FindFamilyType_Door_v1(string doorFamilyName, string doorTypeName)
    {
      //' narrow down the collection with class and category. 
      var doorFamilyCollector1 = new FilteredElementCollector(m_rvtDoc);
      doorFamilyCollector1.OfClass(typeof(FamilySymbol));
      doorFamilyCollector1.OfCategory(BuiltInCategory.OST_Doors);

      //' parse the collection for the given name 
      //' using LINQ query here. 
      var doorTypeElems =
          from element in doorFamilyCollector1
          where element.Name.Equals(doorTypeName) &&
          element.get_Parameter(BuiltInParameter.SYMBOL_FAMILY_NAME_PARAM).AsString().Equals(doorFamilyName)
          select element;

      //' get the result. 
      Element doorType1 = null;
      //' (1) directly accessing from the query result 
      //If doorTypeElems.Count > 0 Then '' we should have only one with the given name. minimum error checking. 
      // doorType1 = doorTypeElems(0) ' found it. 
      //End If 

      //' (2) if we want to get the list of element, here is how. 
      IList<Element> doorTypeList = doorTypeElems.ToList();
      if (doorTypeList.Count > 0)
      {
        //' we should have only one with the given name. minimum error checking. 
        // found it. 
        doorType1 = doorTypeList[0];
      }

      return doorType1;
    }

    //' Find a specific family type for a door. 
    //' another approach will be to look up from Family, then from Family.Symbols property. 
    //' This gets more complicated although it is logical approach. 
    //' 
    public Element FindFamilyType_Door_v2(string doorFamilyName, string doorTypeName)
    {
      //' (1) find the family with the given name. 
      //' 
      var familyCollector = new FilteredElementCollector(m_rvtDoc);
      familyCollector.OfClass(typeof(Family));

      //' use the iterator 
      Family doorFamily = null;
      FilteredElementIterator familyItr = familyCollector.GetElementIterator();
      //familyItr.Reset() 
      while ((familyItr.MoveNext()))
      {
        Family fam = (Family)familyItr.Current;
        //' check name and categoty 
        if ((fam.Name == doorFamilyName) & (fam.FamilyCategory.Id.IntegerValue == (int)BuiltInCategory.OST_Doors))
        {
          //' we found the family. 
          doorFamily = fam;
          break; // TODO: might not be correct. Was : Exit While 
        }
      }

      //' (2) find the type with the given name. 
      //' 
      Element doorType2 = null;
      //' id of door type we are looking for. 
      if (doorFamily != null)
      {
        //' if we have a family, then proceed with finding a type under Symbols property. 
        FamilySymbolSet doorFamilySymbolSet = doorFamily.Symbols;

        //' iterate through the set of family symbols. 
        FamilySymbolSetIterator doorTypeItr = doorFamilySymbolSet.ForwardIterator();
        while (doorTypeItr.MoveNext())
        {
          FamilySymbol dType = (FamilySymbol)doorTypeItr.Current;
          if ((dType.Name == doorTypeName))
          {
            doorType2 = dType;
            //' found it. 
            break; // TODO: might not be correct. Was : Exit While 
          }
        }
      }

      return doorType2;
    }

    //' 
    //' Find specific instances, including filtering by parameters. 
    //' 
    public void FindInstance()
    {
      //' Constant to this function. (we may want to change the value here.) 
      //' this is for wall. e.g., "Basic Wall: Generic - 200mm" 
      const string wallFamilyName = "Basic Wall";
      const string wallTypeName = "Generic - 200mm"; //"Generic - 8\""; // "Generic - 200mm" 
      const string wallFamilyAndTypeName = wallFamilyName + ": " + wallTypeName;

      //' this is for door. e.g., "M_Single-Flush: 0915 x 2134mm 
      const string doorFamilyName = "M_Single-Flush"; // "Single-Flush"; // "M_Single-Flush" 
      const string doorTypeName = "0915 x 2134mm"; // "30\" x 80\""; // "0915 x 2134mm" 
      const string doorFamilyAndTypeName = doorFamilyName + ": " + doorTypeName;

      //' (1) find walls with a specific type 
      //' 
      //' find a specific family type. use the function we defined earlier. 
      ElementId idWallType = FindFamilyType(m_rvtDoc, typeof(WallType), wallFamilyName, wallTypeName, null).Id;
      //' find instances of the given family type. 
      IList<Element> walls = FindInstancesOfType(typeof(Wall), idWallType, null);

      //' show it. 
      string msgWalls = "Instances of wall with type: " + wallFamilyAndTypeName + "\n";
      ShowElementList(walls, msgWalls);

      //' (2) find a specific door. same idea. 
      ElementId idDoorType = FindFamilyType(m_rvtDoc, typeof(FamilySymbol), doorFamilyName, doorTypeName, BuiltInCategory.OST_Doors).Id;
      IList<Element> doors = FindInstancesOfType(typeof(FamilyInstance), idDoorType, BuiltInCategory.OST_Doors);

      string msgDoors = "Instances of door with type: " + doorFamilyAndTypeName + "\n";
      ShowElementList(doors, msgDoors);

      //' (3) apply the same idea to the supporting element, such as level. 
      //' In this case, we simply check the name. 
      //' This becomes handy when you are creating an object on a certain level, 
      //' for example, when we create a wall. 
      //' We will use this in the lab 5 when we create a simple house. 
      //' 
      Level level1 = (Level)FindElement(m_rvtDoc, typeof(Level), "Level 1", null);

      string msgLevel1 = "Level1: " + "\n" + ElementToString(level1) + "\n";
      TaskDialog.Show("Revit Intro Lab", msgLevel1);

      //' (4) finally, let's see how to use parameter filter 
      //' Let's try to get a wall whose length is larger than 60 feet. 

      IList<Element> longWalls = FindLongWalls();

      string msgLongWalls = "Long walls: " + "\n";

      ShowElementList(longWalls, msgLongWalls);
    }

    //' Helper function: find a list of element with given class, family type and category (optional). 
    //' 
    public IList<Element> FindInstancesOfType(Type targetType, ElementId idType, Nullable<BuiltInCategory> targetCategory)
    {
      //' first, narrow down to the elements of the given type and category 
      //' 
      var collector = new FilteredElementCollector(m_rvtDoc).OfClass(targetType);
      if (targetCategory.HasValue)
      {
        collector.OfCategory(targetCategory.Value);
      }

      //' parse the collection for the given family type id. 
      //' using LINQ query here. 
      var elems =
          from element in collector
          where element.get_Parameter(BuiltInParameter.SYMBOL_ID_PARAM).AsElementId().Equals(idType)
          select element;

      //' put the result as a list of element fo accessibility. 

      return elems.ToList();
    }

    //' 
    //' Optional - example of parameter filter. 
    //' find walls whose length is longer than a certain length. e.g., 60 feet 
    //' This could get more complex than looping through in terms of writing a code. 
    //' See page 82 of Developer guide. 
    //' 
    public IList<Element> FindLongWalls()
    {
      //' constant for this function. 
      const double kWallLength = 60.0;
      //' 60 feet. hard coding for simplicity. 

      //' first, narrow down to the elements of the given type and category 
      var collector = new FilteredElementCollector(m_rvtDoc).OfClass(typeof(Wall));

      //' define a filter by parameter 
      //' 1st arg - value provider 
      BuiltInParameter lengthParam = BuiltInParameter.CURVE_ELEM_LENGTH;
      int iLengthParam = (int)lengthParam;
      var paramValueProvider = new ParameterValueProvider(new ElementId(iLengthParam));
      //' 2nd - evaluator 
      FilterNumericGreater evaluator = new FilterNumericGreater();
      //' 3rd - rule value 
      double ruleVal = kWallLength;
      //' 4th - epsilon 
      const double eps = 1E-06;
      //' define a rule 
      var filterRule = new FilterDoubleRule(paramValueProvider, evaluator, ruleVal, eps);
      //' create a new filter 
      var paramFilter = new ElementParameterFilter(filterRule);
      //' go through the filter 
      IList<Element> elems = collector.WherePasses(paramFilter).ToElements();

      return elems;
    }

    #region "Helper Functions"
    //'==================================================================== 
    //' Helper Functions 
    //'==================================================================== 
    //' 
    //' Helper function: find an element of the given type, name, and category(optional) 
    //' You can use this, for example, to find a specific wall and window family with the given name. 
    //' e.g., 
    //' FindFamilyType(m_rvtDoc, GetType(WallType), "Basic Wall", "Generic - 200mm") 
    //' FindFamilyType(m_rvtDoc, GetType(FamilySymbol), "M_Single-Flush", "0915 x 2134mm", BuiltInCategory.OST_Doors) 
    //' 
    //' 
    public static Element FindFamilyType(Document rvtDoc, Type targetType, string targetFamilyName, string targetTypeName, Nullable<BuiltInCategory> targetCategory)
    {
      //' first, narrow down to the elements of the given type and category 
      var collector = new FilteredElementCollector(rvtDoc).OfClass(targetType);
      if (targetCategory.HasValue)
      {
        collector.OfCategory(targetCategory.Value);
      }

      //' parse the collection for the given names 
      //' using LINQ query here. 
      var targetElems =
          from element in collector
          where element.Name.Equals(targetTypeName) &&
          element.get_Parameter(BuiltInParameter.SYMBOL_FAMILY_NAME_PARAM).
          AsString().Equals(targetFamilyName)
          select element;

      //' put the result as a list of element fo accessibility. 
      IList<Element> elems = targetElems.ToList();

      //' return the result. 
      if (elems.Count > 0)
      {
        return elems[0];
      }

      return null;
    }

    //' 
    //' Helper function: find a list of element with given Class, Name and Category (optional). 
    //' 
    public static IList<Element> FindElements(Document rvtDoc, Type targetType, string targetName, Nullable<BuiltInCategory> targetCategory)
    {
      //' first, narrow down to the elements of the given type and category 
      var collector = new FilteredElementCollector(rvtDoc).OfClass(targetType);
      if (targetCategory.HasValue)
      {
        collector.OfCategory(targetCategory.Value);
      }

      //' parse the collection for the given names 
      //' using LINQ query here. 
      var elems =
          from element in collector
          where element.Name.Equals(targetName)
          select element;

      //' put the result as a list of element for accessibility. 

      return elems.ToList();
    }

    //' ------------------------------------------------------------------ 
    //' Helper function: searches elements with given Class, Name and Category (optional), 
    //' and returns the first in the elements found. 
    //' This gets handy when trying to find, for example, Level. 
    //' e.g., FindElement(m_rvtDoc, GetType(Level), "Level 1") 
    //' 
    public static Element FindElement(Document rvtDoc, Type targetType, string targetName, Nullable<BuiltInCategory> targetCategory)
    {
      //' find a list of elements using the overloaded method. 
      IList<Element> elems = FindElements(rvtDoc, targetType, targetName, targetCategory);

      //' return the first one from the result. 
      if (elems.Count > 0)
      {
        return elems[0];
      }

      return null;
    }

    //' 
    //' Helper function: to show the result of finding a family type. 
    //' 
    public string ShowFamilyTypeAndId(string header, string familyAndTypeName, ElementType familyType)
    {
      //' show the result. 
      string msg = header + "\n" + familyAndTypeName + " >> Id = ";

      if (familyType != null)
      {
        msg = msg + familyType.Id.ToString() + "\n";
      }

      //' uncomment this if you want to show each result. 
      //TaskDialog.Show("Revit Intro Lab", msg) 

      return msg;
    }
    //' 
    //' Helper function to display info from a list of elements passed onto. 
    //' 
    public void ShowElementList(IList<Element> elems, string header)
    {
      string s = header + "(" + elems.Count.ToString() + ")" + "\n" + "\n";
      s = s + " - Class - Category - Name (or Family: Type Name) - Id - " + "\n";
      foreach (Element elem in elems)
      {
        s = s + ElementToString(elem);
      }

      TaskDialog.Show("Revit Intro Lab", s);
    }

    //' Helper Funtion: summarize an element information as a line of text, 
    //' which is composed of: class, category, name and id. 
    //' name will be "Family: Type" if a given element is ElementType. 
    //' Intended for quick viewing of list of element, for example. 
    //' 
    public string ElementToString(Element elem)
    {
      if (elem == null)
      {
        return "none";
      }

      string name = "";

      if (elem is ElementType)
      {
        Parameter param = elem.get_Parameter(BuiltInParameter.SYMBOL_FAMILY_AND_TYPE_NAMES_PARAM);
        if (param != null)
        {
          name = param.AsString();
        }
      }
      else
      {
        name = elem.Name;
      }

      return elem.GetType().Name + "; " + elem.Category.Name + "; " + name + "; " + elem.Id.IntegerValue.ToString() + "\n";
    }

    #endregion
  }
}