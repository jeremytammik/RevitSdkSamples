#region Copyright
//
// Copyright (C) 2010-2012 by Autodesk, Inc.
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
#endregion // Copyright

using System;
using System.Collections.Generic;
using System.Linq;

#region Revit Namespaces

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.ApplicationServices; 
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.Attributes;
 
#endregion

namespace IntroCs
{

[Autodesk.Revit.Attributes.Transaction
    (Autodesk.Revit.Attributes.TransactionMode.Manual)]
[Autodesk.Revit.Attributes.Regeneration
    (Autodesk.Revit.Attributes.RegenerationOption.Manual)]
public class HelloWorldCmd 
                    : Autodesk.Revit.UI.IExternalCommand
{
    public Autodesk.Revit.UI.Result Execute(
        Autodesk.Revit.UI.ExternalCommandData commandData,
        ref string message,
        Autodesk.Revit.DB.ElementSet elements)
    {
        Autodesk.Revit.UI.TaskDialog.Show(
        "My Dialog Title",
        "Hello World!");

        return Autodesk.Revit.UI.Result.Succeeded;
    }
}
      
/// <summary>
/// Hello World #3 - minimum external application 
/// Difference :  
/// IExternalApplication instead of IExternalCommand. 
/// In addin manifest, use addin type "Application" and 
/// Name instead of Text tag. 
/// </summary>
public class HelloWorldApp : IExternalApplication
{
    // OnStartup() - called when Revit starts. 
    public Result OnStartup(UIControlledApplication app)
    {
        TaskDialog.Show
                    (
                        "My Dialog Title", 
                        "Hello World from App!"
                    );
        return Result.Succeeded;
    }

    // OnShutdown() - called when Revit ends. 
    public Result OnShutdown(UIControlledApplication app)
    {
        return Result.Succeeded;
    }
}
   
/// <summary>
/// Command Arguments 
/// Take a look at the command arguments. 
/// commandData is the topmost object and 
/// provides the entry point to the Revit model. 
/// </summary>
[Autodesk.Revit.Attributes.Transaction
    (Autodesk.Revit.Attributes.TransactionMode.Manual)]
[Autodesk.Revit.Attributes.Regeneration
    (Autodesk.Revit.Attributes.RegenerationOption.Manual)]
public class CommandData : IExternalCommand
{
    public Result Execute(
        ExternalCommandData commandData,
        ref string message,
        ElementSet elements)
    {
        // The first argument, commandData, provides 
        // access to the top most object model. 
        // You will get the necessary information from 
        // commandData. 
        // To see what's in there, print out a few data 
        // accessed from commandData 
        // 
        // Exercise: Place a break point at commandData 
        // and drill down the data. 

        UIApplication uiApp = commandData.Application;
        Application rvtApp = uiApp.Application;
        UIDocument uiDoc = uiApp.ActiveUIDocument;
        Document rvtDoc = uiDoc.Document; 

        // Print out a few information that you can get 
        // from commandData 
        string versionName = rvtApp.VersionName;
        string documentTitle = rvtDoc.Title;

        TaskDialog.Show(
        "Revit Intro Lab",
        "Version Name = " + versionName
        + "\nDocument Title = " + documentTitle);

        // Print out a list of wall types available in 
        // the current rvt project:

        WallTypeSet wallTypes = rvtDoc.WallTypes;

        string s = "";
        foreach (WallType wallType in wallTypes)
        {
        s += wallType.Name + "\r\n";
        }

        // Show the result:

        TaskDialog.Show(
        "Revit Intro Lab",
        "Wall Types (in main instruction):\n\n" + s);

        // 2nd and 3rd arguments are when the 
        // command fails. 
        // 2nd - set a message to the user. 
        // 3rd - set elements to highlight. 

        return Result.Succeeded;
    }
}

[Autodesk.Revit.Attributes.Transaction
    (Autodesk.Revit.Attributes.TransactionMode.Manual)]
[Autodesk.Revit.Attributes.Regeneration
    (Autodesk.Revit.Attributes.RegenerationOption.Manual)]
public class SelectedElements : IExternalCommand
{
public Result Execute(
    ExternalCommandData commandData,
    ref string message,
    ElementSet elements)
{
    // Get the access to the top most objects. 
    // Notice that we have UI and DB versions for 
    // application and Document. 
    // (We list them both here to show two versions.) 

    UIApplication uiApp = commandData.Application;
    Application app = uiApp.Application;
    UIDocument uiDoc = uiApp.ActiveUIDocument;
    Document doc = uiDoc.Document;

    Autodesk.Revit.UI.Selection.SelElementSet 
                selElementSet = uiDoc.Selection.Elements;
    foreach(Element e in selElementSet)
    {
        // Let's see what kind of element we got. 
        string s = "You picked:"
        + "\r\nClass name = " + e.GetType().Name
        + "\r\nCategory = " + e.Category.Name
        + "\r\nElement id = " + e.Id.ToString();

        // And check its type info. 
        ElementId elemTypeId = e.GetTypeId(); 

        ElementType elemType = 
                (ElementType)doc.GetElement(elemTypeId); 

        s += "\r\nIts ElementType:"
        + " Class name = " + elemType.GetType().Name
        + " Category = " + elemType.Category.Name
        + " Element type id = " + elemType.Id.ToString();

        // Show what we got. 
        TaskDialog.Show("Basic Element Info", s);
    }

    return Result.Succeeded;
}
}

[Autodesk.Revit.Attributes.Transaction
    (Autodesk.Revit.Attributes.TransactionMode.Manual)]
[Autodesk.Revit.Attributes.Regeneration
    (Autodesk.Revit.Attributes.RegenerationOption.Manual)]
public class FilteredElements : IExternalCommand
{
    public Result Execute(
    ExternalCommandData commandData,
    ref string message,
    ElementSet elements)
    {
        // Get the access to the top most objects. 
        // Notice that we have UI and DB versions for 
        // application and Document. 
        // (We list them both here to show two versions.) 

        UIApplication uiApp = commandData.Application;
        Application app = uiApp.Application;
        UIDocument uiDoc = uiApp.ActiveUIDocument;
        Document doc = uiDoc.Document;
                      

        // First, narrow down to the wall elements

        FilteredElementCollector wallTypeCollector 
                    = new FilteredElementCollector(doc);
        // Method : 1
        wallTypeCollector 
            = wallTypeCollector.WherePasses
                (
                new ElementClassFilter(typeof(WallType))
                );
          
        // Method : 2
        //wallTypeCollector 
        //      = wallTypeCollector.OfClass(typeof(Wall));

        // Constant for this function. 
        // 60 feet. hard coding for simplicity.
        const double kWallLength = 60.0;   

        // Define a filter by parameter 
        // 1st arg - value provider 
        BuiltInParameter lengthParam 
                    = BuiltInParameter.CURVE_ELEM_LENGTH;
        int iLengthParam = (int)lengthParam;
        var paramValueProvider 
            = new ParameterValueProvider
                (
                    new ElementId(iLengthParam)
                );
        
        // 2nd - evaluator 
        FilterNumericGreater evaluator 
                            = new FilterNumericGreater();

        // 3rd - rule value 
        double ruleVal = kWallLength;

        // 4th - epsilon 
        const double eps = 1E-06;

        // Define a rule 
        var filterRule 
            = new FilterDoubleRule(
                                    paramValueProvider, 
                                    evaluator, 
                                    ruleVal, 
                                    eps
                                    );

        // Create a new filter 
        var paramFilter 
                = new ElementParameterFilter(filterRule);

        // Go through the filter 
        IList<Element> elems 
            = wallTypeCollector.WherePasses
                (
                    paramFilter
                ).ToElements();
        foreach (Element e in elems)
        {
            // We now have the list of Long walls
        }

        return Result.Succeeded;
    }

}

[Autodesk.Revit.Attributes.Transaction
    (Autodesk.Revit.Attributes.TransactionMode.Manual)]
[Autodesk.Revit.Attributes.Regeneration
    (Autodesk.Revit.Attributes.RegenerationOption.Manual)]
public class AllElements : IExternalCommand
{
    public Result Execute(
    ExternalCommandData commandData,
    ref string message,
    ElementSet elements)
    {
        UIApplication uiApp = commandData.Application;
        Application app = uiApp.Application;
        UIDocument uiDoc = uiApp.ActiveUIDocument;
        Document doc = uiDoc.Document;

        FilteredElementCollector collector1
        = new FilteredElementCollector(doc)
            .WhereElementIsElementType();

        FilteredElementCollector collector2
        = new FilteredElementCollector(doc)
            .WhereElementIsNotElementType();

        collector1.UnionWith(collector2);

        // Loop over the elements and list their data:
        foreach (Element e in collector1)
        {
        }
        return Result.Succeeded;
    }
}

[Autodesk.Revit.Attributes.Transaction
    (Autodesk.Revit.Attributes.TransactionMode.Manual)]
[Autodesk.Revit.Attributes.Regeneration
    (Autodesk.Revit.Attributes.RegenerationOption.Manual)]
public class ElementIdentification : IExternalCommand
{
    public Result Execute(
    ExternalCommandData commandData,
    ref string message,
    ElementSet elements)
    {
        UIApplication uiApp = commandData.Application;
        Application app = uiApp.Application;
        UIDocument uiDoc = uiApp.ActiveUIDocument;
        Document doc = uiDoc.Document;

        //GetAllWalls(doc);
        //GetAllDoors(doc);
        //return Result.Succeeded;

        Reference r = uiDoc.Selection.PickObject
                                    (
                                        ObjectType.Element,
                                        "Pick an element"
                                    );

        // We have picked something. 
        Element e = uiDoc.Document.GetElement(r);

        // An instance of a system family has a designated 
        // class. You can use it identify the type of element. 
        // e.g., walls, floors, roofs. 

        string s = "";

        if (e is Wall)
        {
            s = "Wall";
        }
        else if (e is Floor)
        {
            s = "Floor";
        }
        else if (e is RoofBase)
        {
            s = "Roof";
        }
        else if (e is FamilyInstance)
        {
            // An instance of a component family is all 
            // FamilyInstance. 
            // We'll need to further check its category. 
            // e.g., Doors, Windows, Furnitures. 
            if (e.Category.Id.IntegerValue 
                            == (int)BuiltInCategory.OST_Doors)
            {
                s = "Door";
            }
            else if (e.Category.Id.IntegerValue 
                          == (int)BuiltInCategory.OST_Windows)
            {
                s = "Window";
            }
            else if (e.Category.Id.IntegerValue 
                        == (int)BuiltInCategory.OST_Furniture)
            {
                s = "Furniture";
            }
            else
            {
                // e.g. Plant 
                s = "Component family instance";
            }
        }
        // Check the base class. e.g., CeilingAndFloor. 
        else if (e is HostObject)
        {
            s = "System family instance";
        }
        else
        {
            s = "Other";
        }

        s = "You have picked: " + s;

        TaskDialog.Show("Identify Element", s);

        return Result.Succeeded;
    }

    public static IList<Element> GetAllWalls(Document doc)
    {
        FilteredElementCollector wallTypeCollector 
                        = new FilteredElementCollector(doc);

        wallTypeCollector 
                    = wallTypeCollector.OfClass(typeof(Wall));

        // We now have the list of walls
        IList<Element> walls = wallTypeCollector.ToElements();

        String msg = String.Empty;
        foreach (Element e in walls)
        {
            Wall wall = e as Wall;
            msg = msg + String.Format
                            (   "{0}{1} - {2}", 
                                Environment.NewLine, 
                                wall.Id.IntegerValue.ToString(), 
                                wall.WallType.Name
                            );
        }
        TaskDialog.Show("All walls in the model", msg);

        return walls;
    }

    public static IList<Element> GetAllDoors(Document doc)
    {
        FilteredElementCollector collector 
                        = new FilteredElementCollector(doc);
            
        collector = collector.OfClass
                                    (typeof(FamilyInstance));

        collector = collector.OfCategory
                                  (BuiltInCategory.OST_Doors);

        // We now have the list of doors
        IList<Element> doors = collector.ToElements();

        String msg = String.Empty;
        foreach (Element e in doors)
        {
            ElementId elemTypeId = e.GetTypeId();
            ElementType elemType = 
                     (ElementType)doc.GetElement(elemTypeId);
            Parameter param 
                = elemType.get_Parameter
                (BuiltInParameter.SYMBOL_FAMILY_AND_TYPE_NAMES_PARAM);

            if (param != null)
            {
                msg = msg + String.Format
                            (
                                "{0}{1} - {2}{3}", 
                                Environment.NewLine,
                                e.Id.IntegerValue.ToString(), 
                                elemType.Name, 
                                param.AsString()
                            );
            }
        }
        TaskDialog.Show("All doors in the model", msg);

        return doors;
    }
}

[Autodesk.Revit.Attributes.Transaction
    (Autodesk.Revit.Attributes.TransactionMode.Manual)]
[Autodesk.Revit.Attributes.Regeneration
    (Autodesk.Revit.Attributes.RegenerationOption.Manual)]
public class ElementCreation : IExternalCommand
{
    const double _mmToFeet = 0.0032808399;

    public Result Execute(
    ExternalCommandData commandData,
    ref string message,
    ElementSet elements)
    {
        UIApplication uiApp = commandData.Application;
        Application app = uiApp.Application;
        UIDocument uiDoc = uiApp.ActiveUIDocument;
        Document doc = uiDoc.Document;

        // First, narrow down to the elements of the given type and category 
        var collector 
            = new FilteredElementCollector(doc)
                                      .OfClass(typeof(Level));

        // Parse the collection for the given names 
        // Using LINQ query here. 
        var elems =
            from element in collector
            where element.Name.Equals("Level 1")
            select element;

        // Put the result as a list of element for 
        // accessibility. 

        IList<Element> levels = elems.ToList();
        Level level1 = levels[0] as Level;

        // Parse the collection for the given names 
        // Using LINQ query here. 
        elems =
            from element in collector
            where element.Name.Equals("Level 2")
            select element;

        // Put the result as a list of element for 
        // accessibility. 

        levels = elems.ToList();
        Level level2 = levels[0] as Level;

        // Hard coding the size of the house for simplicity 
        double widthInmm = 10000.0;
        double depthInmm = 5000.0;

        double widthInft = widthInmm * _mmToFeet;
        double depthInft = depthInmm * _mmToFeet;

        XYZ pt1 
            = new XYZ(-widthInft / 2.0, -depthInft / 2.0, 0.0);

        XYZ pt2 
            = new XYZ(widthInft / 2.0, -depthInft / 2.0, 0.0);

        // Define a base curve from two points. 
        Line baseCurve = app.Create.NewLineBound(pt1, pt2);
        // Create a wall using the one of overloaded methods. 

        bool isStructural = true;

        // 2012
        //Wall aWall 
        // = _doc.Create.NewWall(baseCurve, level1, isStructural); 

        // since 2013
        Wall aWall = Wall.Create(
                                    doc,
                                    baseCurve, 
                                    level1.Id, 
                                    isStructural
                                ); 

        // Set the Top Constraint to Level 2 
        aWall.get_Parameter(BuiltInParameter.WALL_HEIGHT_TYPE)
                                              .Set(level2.Id);

        // This is important. 
        // we need these lines to have shrinkwrap working. 
        doc.Regenerate();
        doc.AutoJoinElements();

        return Result.Succeeded;
    }
}

[Autodesk.Revit.Attributes.Transaction
    (Autodesk.Revit.Attributes.TransactionMode.Manual)]
[Autodesk.Revit.Attributes.Regeneration
    (Autodesk.Revit.Attributes.RegenerationOption.Manual)]
public class ElementModification : IExternalCommand
{
    const double _mmToFeet = 0.0032808399;

    public Result Execute(
    ExternalCommandData commandData,
    ref string message,
    ElementSet elements)
    {
        UIApplication uiApp = commandData.Application;
        Application app = uiApp.Application;
        UIDocument uiDoc = uiApp.ActiveUIDocument;
        Document doc = uiDoc.Document;

        Reference r = uiDoc.Selection.PickObject
                      (
                        ObjectType.Element, 
                        "Pick a wall, please"
                      );

        // We have picked something. 
        Element e = doc.GetElement(r);

        if (!(e is Wall))
        {
            message = "Please select a wall.";
            return Result.Failed;
        }

        Wall aWall = e as Wall;

        LocationCurve wallLocation 
                            = (LocationCurve)aWall.Location;

        XYZ pt1 = wallLocation.Curve.get_EndPoint(0);
        XYZ pt2 = wallLocation.Curve.get_EndPoint(1);

        // Hard coding the displacement value for simplicity 
        // here. We can also use the "ElementTransformUtils" 
        // class to transform the element
        double dtInmm = 1000.0;
        double dtInft = dtInmm * _mmToFeet;
        XYZ newPt1 
            = new XYZ(pt1.X - dtInft, pt1.Y - dtInft, pt1.Z);

        XYZ newPt2 
            = new XYZ(pt2.X - dtInft, pt2.Y - dtInft, pt2.Z);

        // Create a new line bound. 
        Line newWallLine 
                    = app.Create.NewLineBound(newPt1, newPt2);

        // Finally change the curve. 
        wallLocation.Curve = newWallLine;

        //XYZ v = new XYZ(dtInft, dtInft, 0.0);
        //ElementTransformUtils.MoveElement(doc, e.Id, v); 

        // Message to the user. 
        TaskDialog.Show
            (
                "ElementModification - wall", 
                "Location: start point moved -1000.0 mm in X-direction\r\n"
            );

        doc.Regenerate();
        return Result.Succeeded;
    }
}

[Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
[Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
public class BuiltInParameters : IExternalCommand
{
    public Result Execute(
    ExternalCommandData commandData,
    ref string message,
    ElementSet elements)
    {
        UIApplication uiApp = commandData.Application;
        Application app = uiApp.Application;
        UIDocument uiDoc = uiApp.ActiveUIDocument;
        Document doc = uiDoc.Document;

        Reference r = uiDoc.Selection.PickObject
            (
                ObjectType.Element, 
                "Pick a wall, please"
            );

        // We have picked something. 
        Element e = doc.GetElement(r);

        string s = string.Empty;

        foreach (Parameter param in e.Parameters)
        {
            string name = param.Definition.Name;
            // To get the value, we need to pause the param 
            // depending on the storage type 
            // see the helper function below 
            string val = ParameterToString(param);
            s += "\r\n" + name + " = " + val;
        }

        TaskDialog.Show("Element Parameters: ", s);

        RetrieveParameter
        (   e, 
            "Element Parameter (using BuiltInParameter and Name): "
        );

        return Result.Succeeded;
    }

    /// <summary>
    /// Helper function: return a string form of a given 
    /// parameter.
    /// </summary>
    public static string ParameterToString(Parameter param)
    {
        string val = "none";

        if (param == null)
        {
            return val;
        }

        // To get to the parameter value, we need to pause it 
        // depending on its storage type 

        switch (param.StorageType)
        {
            case StorageType.Double:
                double dVal = param.AsDouble();
                val = dVal.ToString();
                break;

            case StorageType.Integer:
                int iVal = param.AsInteger();
                val = iVal.ToString();
                break;

            case StorageType.String:
                string sVal = param.AsString();
                val = sVal;
                break;

            case StorageType.ElementId:
                ElementId idVal = param.AsElementId();
                val = idVal.IntegerValue.ToString();
                break;

            case StorageType.None:
                break;
        }
        return val;
    }

    /// <summary>
    /// Examples of retrieving a specific parameter individually 
    /// (hard coded for simplicity; This function works best 
    /// with walls and doors).
    /// </summary>
    public void RetrieveParameter(Element e, string header)
    {
        string s = string.Empty;

        // As an experiment, let's pick up some arbitrary 
        // parameters. 
        // Comments - most of instance has this parameter 

        // (1) by BuiltInParameter. 
        Parameter param =
            e.get_Parameter
            (
                BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS
            );

        if (param != null)
        {
            s += "Comments (by BuiltInParameter) = " 
                    + ParameterToString(param) + "\n";
        }

        TaskDialog.Show(header, s);

        // (2) by name. (Mark - most of instance has this parameter.) 
        // if you use this method, it will language specific. 
        param = e.get_Parameter("Mark");
        if (param != null)
        {
            s += "Mark (by Name) = " 
                    + ParameterToString(param) + "\n";
        }

        // Though the first one is the most commonly used, 
        // other possible methods are: 
        // (3) by definition 
        // param = e.Parameter(Definition) 
        // (4) and for shared parameters, you can also use GUID. 
        // parameter = Parameter(GUID) 

        // The following should be in most of type parameter 

        param = 
            e.get_Parameter
            (
                BuiltInParameter.ALL_MODEL_TYPE_COMMENTS
            );
        if (param != null)
        {
            s += "Type Comments (by BuiltInParameter) = " 
                            + ParameterToString(param) + "\n";
        }

        param = e.get_Parameter("Fire Rating");
        if (param != null)
        {
            s += "Fire Rating (by Name) = " 
                            + ParameterToString(param) + "\n";
        }

        // Using the BuiltInParameter, you can sometimes 
        // access one that is not in the parameters set. 
        // Note: this works only for element type. 

        param = 
            e.get_Parameter
            (
                BuiltInParameter.SYMBOL_FAMILY_AND_TYPE_NAMES_PARAM
            );
        if (param != null)
        {
            s += "SYMBOL_FAMILY_AND_TYPE_NAMES_PARAM (only by BuiltInParameter) = "
                + ParameterToString(param) + "\n";
        }

        param =
            e.get_Parameter
            (
                BuiltInParameter.SYMBOL_FAMILY_NAME_PARAM
            );
        if (param != null)
        {
            s += "SYMBOL_FAMILY_NAME_PARAM (only by BuiltInParameter) = "
                + ParameterToString(param) + "\n";
        }

        // Show it. 
        TaskDialog.Show(header, s);
    }
} // class

} // namespace
