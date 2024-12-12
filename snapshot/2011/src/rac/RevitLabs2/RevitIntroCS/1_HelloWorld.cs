#region "Imports"

using System; 
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes; // specify this if you want to save typing for attributes. e.g.

#endregion

#region "Description"
//' Revit Intro Lab - 1 
//' 
//' In this lab, you will learn how to "hook" your add-on program to Revit. 
//' This command defines a minimum external command. 
//' You will learn: 
//' 
//' Explain about addin manifest. How to create GUID. 
//' Hello World in VB.NET is form page 360 of Developer Guide. 
//' 
#endregion

// Hello World #1 - A minimum Revit external command. 
// 
[Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Automatic)]
[Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
public class HelloWorld : IExternalCommand
{
    public Autodesk.Revit.UI.Result Execute(
        Autodesk.Revit.UI.ExternalCommandData commandData, 
        ref string message, 
        Autodesk.Revit.DB.ElementSet elements)
    {
        Autodesk.Revit.UI.TaskDialog.Show("My Dialog Title", "Hello World!");

        return Result.Succeeded;
    }
}

// Hello World #2 - simplified without full namespace. 
// 
[Transaction(TransactionMode.Automatic)]
[Regeneration(RegenerationOption.Manual)]
public class HelloWorldSimple : IExternalCommand
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        TaskDialog.Show("My Dialog Title", "Hello World Simple!");

        return Result.Succeeded;
    }
}

//' Hello World #3 - minimum external application 
//' diference: IExternalApplication instead of IExternalCommand. in addin manifest. 
//' Use addin type "Application", use <Name/> instead of <Text/>. 
//' 

[Transaction(TransactionMode.Automatic)]
[Regeneration(RegenerationOption.Manual)]
public class HelloWorldApp : IExternalApplication
{
    //' OnShutdown() - called when Revit ends. 
    //' 
    public Autodesk.Revit.UI.Result OnShutdown(Autodesk.Revit.UI.UIControlledApplication application)
    {
        return Result.Succeeded;
    }

    //' OnStartup() - called when Revit starts. 
    //' 
    public Autodesk.Revit.UI.Result OnStartup(Autodesk.Revit.UI.UIControlledApplication application)
    {
        TaskDialog.Show("My Dialog Title", "Hello World from App!");

        return Result.Succeeded;
    }
}

//' Command Arguments 
//' 
[Transaction(TransactionMode.Automatic)]
[Regeneration(RegenerationOption.Manual)]
public class CommandData : IExternalCommand
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        //' The first argument, commandData, is the top most object model. 
        //' Just as an example, print out a few data accessed from commandData 
        //' 
        string versionName = commandData.Application.Application.VersionName;
        string documentTitle = commandData.Application.ActiveUIDocument.Document.Title;
        TaskDialog.Show("Revit Intro Lab", "Version Name = " + versionName + "\n" + "Document Title = " + documentTitle);

        //' print out a list of wall types available in the current rvt project. 
        //' 
        WallTypeSet wallTypes = 
            commandData.Application.ActiveUIDocument.Document.WallTypes;
        string s = "";
        foreach (WallType wType in wallTypes)
        {
            s = s + wType.Name + "\n";
        }

        //' show the result. 
        TaskDialog.Show("Revit Intro Lab", "Wall Types:" + "\n" + s);

        //' Second argument - no coding. just explain. 
        //' Third argument - no coding necessary. but just explain. 

        return Result.Succeeded;
    }
}