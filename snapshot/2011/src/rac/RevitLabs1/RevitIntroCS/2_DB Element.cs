#region "Imports"

using System; 
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes; // specify this if you want to save typing for attributes. e.g.
using Autodesk.Revit.UI.Selection; // for selection
using Autodesk.Revit.ApplicationServices; // Application class

#endregion

#region "Description" 
//' Revit Intro Lab - 2 
//' 
//' In this lab, you will learn how an element is represended in Revit. 
//' Disclaimer: minimum error checking to focus on the main topic. 
//' 
#endregion 

//' DBElement - identifying element 
//' 
[Transaction(TransactionMode.Automatic)] 
[Regeneration(RegenerationOption.Manual)] 
public class AsdkDBElement : IExternalCommand 
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
        
        //' (1) select an object on a screen. (We'll come back to the selection in the UI Lab later.) 
        Reference refPick = 
            rvtUIDoc.Selection.PickObject(ObjectType.Element, "Pick an element"); 
        
        //' we have picked something. 
        Element elem = refPick.Element; 
        
        //' (2) let's see what kind of element we got. 
        //' Key properties that we need to check are: Class, Category and if an element is ElementType or not. 
        //' 
        ShowBasicElementInfo(elem); 
        
        // '' now check and see the basic info of the element type 
        //Dim elemTypeId As ElementId = elem.GetTypeId 
        //Dim elemType As ElementType = m_rvtDoc.Element(elemTypeId) 
        //ShowBasicElementInfo(elemType) 
        
        //' (3) now, we are going to identify each major types of element. 
        IdentifyElement(elem); 
        
        //' Now look at other properties - important ones are parameters, locations and geometry. 
        
        //' (4) first parameters. 
        //' 
        ShowParameters(elem, "Element Parameters: "); 
        
        //' check to see its type parameter as well 
        //' 
        ElementId elemTypeId = elem.GetTypeId(); 
        ElementType elemType = (ElementType)m_rvtDoc.get_Element(elemTypeId); 
        ShowParameters(elemType, "Type Parameters: "); 
        
        //' okay. we saw a set or parameters for a given element or element type. 
        //' how can we access to each parameters. For example, how can we get the value of "length" information? 
        //' here is how: 
        
        // '' select a wall object on a screen. 
        //ref = rvtUIDoc.Selection.PickObject(Selection.ObjectType.Element, "Pick a wall element") 
        
        // '' we have picked something. 
        //elem = ref.Element 
        
        RetrieveParameter(elem, "Element Parameter (by Name and BuiltInParameter): "); 
        //' the same logic applies to the type parameter. 
        RetrieveParameter(elemType, "Type Parameter (by Name and BuiltInParameter): "); 
        
        //' (5) location 
        ShowLocation(elem); 
        
        //' (6) geometry - the last piece. (Optional) 
        ShowGeometry(elem); 
        
        //' these are the common proerties. 
        //' there may be more properties specific to the given element class, 
        //' such as Wall.Width, .Flipped and Orientation. Expore using RevitLookup and RevitAPI.chm. 
        //' 
        
        //' we are done. 
            
        return Result.Succeeded; 
    } 
    
    //' shows basic information about the given element. 
    //' Note: we are intentionally including both element and element type here 
    //' to be able to compare the output on the same dialog. 
    //' Compare, for example, category in element and element type. 
    //' 
    public void ShowBasicElementInfo(Element elem) 
    { 
        //' let's see what kind of element we got. 
        //' 
        string s = "You Picked:" + "\n"; 
        
        s = s + " Class name = " + elem.GetType().Name + "\n"; 
        s = s + " Category = " + elem.Category.Name + "\n"; 
        s = s + " Element id = " + elem.Id.ToString() + "\n" + "\n"; 
        
        //' and, check its type info. 
        //' 
        //Dim elemType As ElementType = elem.ObjectType '' this is obsolete. 
        ElementId elemTypeId = elem.GetTypeId(); 
        ElementType elemType = (ElementType)m_rvtDoc.get_Element(elemTypeId); 
        
        s = s + "Its ElementType:" + "\n"; 
        s = s + " Class name = " + elemType.GetType().Name + "\n"; 
        s = s + " Category = " + elemType.Category.Name + "\n"; 
        s = s + " Element type id = " + elemType.Id.ToString() + "\n"; 
        
        //' finally show it. 
            
        TaskDialog.Show("Revit Intro Lab", s); 
    } 
    
    //' identify the type of the element known to the UI. 
    //' 
    public void IdentifyElement(Element elem) 
    { 
        
        //' An instance of a system family has a designated class. 
        //' You can use it identify the type of element. 
        //' e.g., walls, floors, roofs. 
        //' 
        string s = ""; 
        
        if (elem is Wall) 
        { 
            s = "Wall"; 
        } 
        else if (elem is Floor) 
        { 
            s = "Floor"; 
        } 
        else if (elem is RoofBase) 
        { 
            s = "Roof"; 
        } 
        else if (elem is FamilyInstance) 
        { 
            //' An instance of a component family is all FamilyInstance. 
            //' We'll need to further check its category. 
            //' e.g., Doors, Windows, Furnitures. 
            if (elem.Category.Id.IntegerValue == 
                (int)BuiltInCategory.OST_Doors) 
            { 
                s = "Door"; 
            } 
            else if (elem.Category.Id.IntegerValue == 
                (int)BuiltInCategory.OST_Windows) 
            { 
                s = "Window"; 
            } 
            else if (elem.Category.Id.IntegerValue == 
                (int)BuiltInCategory.OST_Furniture) { 
                s = "Furniture"; 
            } 
            else 
            { 
                    //' e.g. Plant 
                s = "Component family instance";  
            } 
        } 
        else if (elem is HostObject) 
        { 
            //' check the base class. e.g., CeilingAndFloor. 
            s = "System family instance"; 
        } 
        else 
        { 
            s = "Other"; 
        } 
        
        s = "You have picked: " + s; 
        //' show it. 
            
        TaskDialog.Show("Revit Intro Lab", s); 
    } 
    
    #region "ShowParameters" 
    //' show the parameter values of the element 
    //' 
    public void ShowParameters(Element elem, [System.Runtime.InteropServices.DefaultParameterValueAttribute("")] string title) 
    { 
        
        ParameterSet paramSet = elem.Parameters; 
        string s = title + "\n" + "\n"; 
        
        foreach (Parameter param in paramSet) 
        { 
            string name = param.Definition.Name; 
            
            //' to get the value, we need to pause the param depending on the strage type 
            string val = ParameterToString(param); 
            
            s = s + name + " = " + val + "\n"; 
        } 
        
            
        TaskDialog.Show("Revit Intro Lab", s); 
    } 
    
    //' Helper function: return a string from of the given parameter. 
    //' 
    public static string ParameterToString(Parameter param) 
    { 
        
        string val = ""; 
        
        if (param == null) 
        { 
            return val; 
        } 
        
        //' to get to the parameter value, we need to pause it depending on its strage type 
        //' 
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
            default: 
                
                break; 
        } 
        
        return val; 
    } 
    
    #endregion 
    
    //' example of retrieving a specific parameter indivisually. 
    //' (harding coding for simplicity. This function works best with walls and doors.) 
    //' 
    public void RetrieveParameter(Element elem, [System.Runtime.InteropServices.DefaultParameterValueAttribute("")] string title) 
    { 
        string s = title + "\n" + "\n"; 
        
        //' as an experiment, let's pick up some arbitrary parameters. 
        //' comments - most of instance has this parameter 
        
        //' (1) by BuiltInParameter. 
        Parameter param = elem.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS); 
        if (param != null) 
        { 
            s = s + "Comments (by BuiltInParameter) = " + ParameterToString(param) + "\n"; 
        } 
        
        //' (2) by name. (Mark - most of instance has this parameter.) if you use this method, it will language specific. 
        param = elem.get_Parameter("Mark"); 
        if (param != null) 
        {
            s = s + "Mark (by Name) = " + ParameterToString(param) + "\n"; 
        } 
        
        //' though the first one is the most commonly used, other possible methods are: 
        //' (3) by definition 
        //param = elem.Parameter(Definition) 
        //' (4) and for shared parameters, you can also use GUID. 
        //parameter = Parameter(GUID) 
        
        
        //' the following should be in most of type parameter 
        //' 
        param = elem.get_Parameter(BuiltInParameter.ALL_MODEL_TYPE_COMMENTS); 
        if (param != null)
        { 
            s = s + "Type Comments (by BuiltInParameter) = " + ParameterToString(param) + "\n"; 
        } 
        
        param = elem.get_Parameter("Fire Rating"); 
        if (param != null) 
        { 
            s = s + "Fire Rating (by Name) = " + ParameterToString(param) + "\n" + "\n"; 
        } 
        
        //' using the BuiltInParameter, you can sometimes access one that is not in the parameters set. 
        //' Note: this works only for element type. 
        //' 
        param = elem.get_Parameter(BuiltInParameter.SYMBOL_FAMILY_AND_TYPE_NAMES_PARAM); 
        if (param != null) 
        { 
            s = s + "Family Type Name (by BuiltInParameter) = " + ParameterToString(param) + "\n"; 
        } 
        
        //' show it. 
            
        TaskDialog.Show("Revit Intro Lab", s); 
    } 
    
    #region "ReportLocation" 
    
    //' show the location information of the given element. 
    //' location can be LocationPoint (e.g., furniture), and LocationCurve (e.g., wall). 
    //' 
    public void ShowLocation(Element elem) 
    { 
        
        string s = "Location Information: " + "\n" + "\n"; 
        Location loc = elem.Location; 
        
        if (loc is LocationPoint) 
        {  
            //' (1) we have a location point 
            LocationPoint locPoint = (LocationPoint)loc; 
            XYZ pt = locPoint.Point; 
            double r = locPoint.Rotation; 
            
            s = s + "LocationPoint" + "\n"; 
            s = s + "Point = " + PointToString(pt) + "\n"; 
                
            s = s + "Rotation = " + r.ToString() + "\n"; 
        } 
        else if (loc is LocationCurve)
        { 
            //' (2) we have a location curve 
            LocationCurve locCurve = (LocationCurve)loc; 
            Curve crv = locCurve.Curve; 
            
            s = s + "LocationCurve" + "\n"; 
            s = s + "EndPoint(0)/Start Point = " + PointToString(crv.get_EndPoint(0)) + "\n"; 
            s = s + "EndPoint(1)/End point = " + PointToString(crv.get_EndPoint(1)) + "\n"; 
                
            s = s + "Length = " + crv.Length.ToString() + "\n"; 
        } 
        
        //' show it. 
            
        TaskDialog.Show("Revit Intro Lab", s); 
    } 
    
    
    
    //' Helper Function: returns XYZ in a string form. 
    //' 
    public static string PointToString(XYZ pt) 
    { 
        if (pt == null) 
        { 
            return ""; 
        } 
            
        return "(" + pt.X.ToString("F2") + ", " + pt.Y.ToString("F2") + ", " + pt.Z.ToString("F2") + ")"; 
    } 
    
    #endregion 
    
    #region "ReportGeometry" 
    
    //' This is lengthy. So Optional: 
    //' show the geometry information of the given element. Here is how to access it. 
    //' you can go through by RevitLookup, instead. 
    //' 
    public void ShowGeometry(Element elem) 
    { 
        string s = "Geometry Information: " + "\n" + "\n"; 
        
        //' first, set a geometry option 
        Options opt = m_rvtApp.Create.NewGeometryOptions(); 
        opt.DetailLevel = DetailLevels.Fine; 
        
        //' does the element have the geometry data? 
        GeometryElement geomElem = elem.get_Geometry(opt); 
        if (geomElem == null) 
        { 
            TaskDialog.Show("Revit Intro Lab", s + "no data"); 
            return; 
        } 
        
        //' get the geometry information from the geom elem. 
        //' geometry informaion can easily go into depth. 
        //' here we look at at top level. use RevitLookup for complee dril down. 
        //' 
        s = GeometryElementToString(geomElem); 
        
        //' show it. 
            
        TaskDialog.Show("Revit Intro Lab", s); 
    } 
    
    public static string GeometryElementToString(GeometryElement geomElem) 
    { 
        
        GeometryObjectArray geomObjs = geomElem.Objects; 
        
        string str = "Total number of GeometryObject: " + geomObjs.Size.ToString() + "\n"; 
        
        foreach (GeometryObject geomObj in geomObjs) 
        { 
            if (geomObj is Solid) 
            { 
                // ex. wall 
                
                Solid solid = (Solid)geomObj; 
                //str = str & GeometrySolidToString(solid) 
                    
                str = str + "Solid" + "\n"; 
            } 
            else if (geomObj is GeometryInstance) 
            { 
                // ex. door/window 
                
                str = str + " -- Geometry.Instance -- " + "\n"; 
                GeometryInstance geomInstance = (GeometryInstance)geomObj; 
                GeometryElement geoElem = geomInstance.SymbolGeometry; 
                    
                str = str + GeometryElementToString(geoElem); 
            } 
            else if (geomObj is Curve)
            { 
                // ex. 
                
                Curve curv = (Curve)geomObj; 
                //str = str & GeometryCurveToString(curv) 
                    
                str = str + "Curve" + "\n"; 
            } 
            else if (geomObj is Mesh) 
            { 
                // ex. 
                
                Mesh mesh = (Mesh)geomObj; 
                //str = str & GeometryMeshToString(mesh) 
                    
                str = str + "Mesh" + "\n"; 
            } 
            else 
            { 
                str = str + " *** unkown geometry type" +  geomObj.GetType().Name; 
            } 
        } 
        
        return str; 
    } 
    
    #endregion 
} 