#region "Imports"

using System;
using System.Linq; 
using System.Collections.Generic; 
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes; // specify this if you want to save typing for attributes. e.g.
using Autodesk.Revit.ApplicationServices; // Application class

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
#endregion 

//' ElementFiltering - 
//' 
[Transaction(TransactionMode.Automatic)] 
[Regeneration(RegenerationOption.Manual)] 
public class AsdkElementFiltering : IExternalCommand 
{ 
    //' member variables 
    Application m_rvtApp; 
    Document m_rvtDoc; 
    
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements) 
    { 
        //' get the access to the top most objects. Notice that we have UI abd DB application and Document. 
        //' (we are notusing them all here. This is to show two versions.) 
        //' 
        UIApplication rvtUIApp = commandData.Application; 
        UIDocument rvtUIDoc = rvtUIApp.ActiveUIDocument; 
        m_rvtApp = rvtUIApp.Application; 
        m_rvtDoc = rvtUIDoc.Document; 
        
        //' (1) In eailer lab, CommandData command, we learned how to access to the wallType. i.e., 
        //' 
        ShowElementTypes(); 
        
        //ShowElements() 
        
        //' (2) now get a specific element. 
        //' 
        GetElement(); 
          
        //' we are done. 
            
        return Result.Succeeded; 
    } 
    
    //' shows basic information about the given element. 
    //' Note: we are intentionally including both element and element type here 
    //' to be able to compare the output on the same dialog. 
    //' Compare, for example, category in element and element type. 
    //' 
    public void ShowElementTypes() 
    { 
        //' (1) get a list of element types available in the current rvt project. 
        //' For system family types, there is a designated properties that allows us to directly access to the types. 
        //' 
        WallTypeSet wallTypes = m_rvtDoc.WallTypes; 
        string s = ""; 
        foreach (WallType wType in wallTypes) 
        { 
            s = s + wType.Name + "\n"; 
        } 
        TaskDialog.Show("Revit Intro Lab", "Wall Types (rvtDoc.WallTypes): " + wallTypes.Size.ToString() + "\n" + s); 
        
        //' same idea applies to other system family, such as Floors, Roofs. 
        //' 
        FloorTypeSet floorTypes = m_rvtDoc.FloorTypes; 
        s = ""; 
        foreach (FloorType fType in floorTypes) 
        { 
            s = s + fType.Name + "\n"; 
        } 
        TaskDialog.Show("Revit Intro Lab", "Floor Types (rvtDoc.FloorTypes): " + floorTypes.Size.ToString() + "\n" + s); 
        
        //' another approach is to use a filter. here is an example with wallType. 
        //' 
        s = ""; 
        var collector = new FilteredElementCollector(m_rvtDoc); 
        collector.WherePasses(new ElementClassFilter(typeof(WallType))); 
        IList<Element> wallTypes2 = collector.ToElements(); 
        foreach (Element elem in wallTypes2) 
        { 
            s = s + elem.Name + "\n"; 
        } 
        TaskDialog.Show("Revit Intro Lab", "Wall Types (by Filer): " + wallTypes2.Count.ToString() + "\n" + s); 
        
        //' parse the collection for the given name 
        //' using LINQ query here. 
        //' 
        //Dim targetElems = From element In collector Where element.Name.Equals(targetName) Select element 
        //Dim elems As List(Of Element) = targetElems.ToList() 
        
        //If elems.Count > 0 Then '' we should have only one with the given name. 
        // Return elems(0) 
        //End If 
        
        //' show the list. 
        
        //' (2) for component family. it is slightly more complex. you will need to use a filtering. 
        //' for example, doors - remember for component family, you will need to check element type and category 
        //' 
        s = ""; 
        collector = new FilteredElementCollector(m_rvtDoc); 
        collector.WherePasses(new ElementClassFilter(typeof(ElementType))); 
        collector.OfCategory(BuiltInCategory.OST_Doors); 
        
        IList<Element> doorTypes = collector.ToElements(); 
        foreach (Element elem in doorTypes) 
        { 
            FamilySymbol dType = (FamilySymbol)elem; 
            s = s + dType.Family.Name + " : "; 
            s = s + dType.Name + "\n"; 
        } 
        TaskDialog.Show("Revit Intro Lab", "Door Types (by Filer): " + doorTypes.Count.ToString() + "\n" + s); 
        
        //' same rule applies to the other component family. 
        //' here is in more generic form. 
        //' 
        ShowFamilyTypes(typeof(WallType), null, "Wall Types: "); 
            
        ShowFamilyTypes(typeof(FamilySymbol), BuiltInCategory.OST_Doors, "Door Types: "); 
    } 
    
    //' generic function to collect elements with the given type and category. 
    //' e.g., wall types can be obtained by targetType as WallTypes. 
    //' door types by FamilySymbol & BuiltInCategory.OST_Doors. 
    
    public void ShowFamilyTypes(Type targetType, Nullable<BuiltInCategory> targetCategory, [System.Runtime.InteropServices.DefaultParameterValueAttribute("")] string header) 
    { 
        //' filter by Type and Category. 
        var collector = new FilteredElementCollector(m_rvtDoc); 
        collector.WherePasses(new ElementClassFilter(targetType)); 
        //' e.g., WallType, FamilySymbol 
        if (!targetCategory.HasValue) 
        { 
            //' e.g., BuiltInCategory.OST_Doors. 
            collector.OfCategory(targetCategory.Value); 
        } 
        
        //' show it in a dialog. 
        string s = ""; 
        IList<Element> types = collector.ToElements(); 
        foreach (Element elem in types) 
        { 
            if ((elem) is FamilySymbol) 
            { 
                FamilySymbol dType = (FamilySymbol)elem; 
                s = s + dType.Family.Name + " : "; 
            } 
            s = s + elem.Name + "\n"; 
        } 
        
        //' show it. 
            
        TaskDialog.Show("Revit Intro Lab", header + types.Count.ToString() + "\n" + s); 
    } 
    
    //' get a specific element 
    //' 
    public void GetElement() 
    { 
        //' (1) get a specific wall type. 
        string targetName = "Generic - 200mm"; 
        
        var collector = new FilteredElementCollector(m_rvtDoc); 
        collector.WherePasses(new ElementClassFilter(typeof(WallType))); 
        
        //' parse the collection for the given name 
        //' using LINQ query here. 
        //' 
        var targetElems = 
            from element in collector 
            where element.Name.Equals(targetName) 
            select element; 
        IList<Element> elems = targetElems.ToList(); 
        
        if (elems.Count > 0) 
        { 
            //' we should have only one with the given name. 
            //Return elems(0) 
            TaskDialog.Show("Revit Intro Lab", targetName + ", element id = " + elems[0].Id.IntegerValue.ToString() + "\n"); 
        } 
        
        //' (2) get a specific door type 
        var targetFamilyName = "M_Single-Flush"; 
        var targetFamilyTypeName = "0915 x 2134mm"; 
        
        collector = new FilteredElementCollector(m_rvtDoc); 
        collector.WherePasses(new ElementClassFilter(typeof(FamilySymbol))); 
        
        //' parse the collection for the given name 
        //' using LINQ query here. 
        //' 
        targetElems = 
            from element in collector 
            where element.Name.Equals(targetFamilyTypeName) 
            select element; 
        elems = targetElems.ToList(); 
        
        if (elems.Count > 0) 
        { 
            //' we should have only one with the given name. 
            //Return elems(0) 
            TaskDialog.Show("Revit Intro Lab", targetFamilyTypeName + ", element id = " + elems[0].Id.IntegerValue.ToString() + "\n");  
        } 
    } 
    
    //' ================================================================================== 
    //' helper function: find an element of the given type and the name. 
    //' You can use this, for example, to find Reference or Level with the given name. 
    //' ================================================================================== 
    public Element FindElement(Type targetType, string targetName) 
    { 
        //' get the elements of the given type 
        //' 
        var collector = new FilteredElementCollector(m_rvtDoc); 
        collector.WherePasses(new ElementClassFilter(targetType)); 
        
        //' parse the collection for the given name 
        //' using LINQ query here. 
        //' 
        var targetElems = 
            from element in collector 
            where element.Name.Equals(targetName) 
            select element; 
        IList<Element> elems = targetElems.ToList(); 
        
        if (elems.Count > 0) 
        { 
            //' we should have only one with the given name. 
            return elems[0]; 
        } 
        
        //' cannot find it. 
            
        return null; 
    }   
} 