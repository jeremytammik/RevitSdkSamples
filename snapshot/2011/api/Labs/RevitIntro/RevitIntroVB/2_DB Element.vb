#Region "Copyright"
''
'' (C) Copyright 2010 by Autodesk, Inc.
''
'' Permission to use, copy, modify, and distribute this software in
'' object code form for any purpose and without fee is hereby granted,
'' provided that the above copyright notice appears in all copies and
'' that both that copyright notice and the limited warranty and
'' restricted rights notice below appear in all supporting
'' documentation.
''
'' AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
'' AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
'' MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC.
'' DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
'' UNINTERRUPTED OR ERROR FREE.
''
'' Use, duplication, or disclosure by the U.S. Government is subject to
'' restrictions set forth in FAR 52.227-19 (Commercial Computer
'' Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
'' (Rights in Technical Data and Computer Software), as applicable.
''
'' Written by M.Harada 
''
#End Region

#Region "Imports"
'' Import the following name spaces in the project properties/references. 
'' Note: VB.NET has a slighly different way of recognizing name spaces than C#. 
'' if you explicitely set them in each .vb file, you will need to specify full name spaces. 

'Imports System
'Imports Autodesk.Revit.DB
'Imports Autodesk.Revit.UI
'Imports Autodesk.Revit.ApplicationServices  '' Application class
'Imports Autodesk.Revit.Attributes '' specific this if you want to save typing for attributes. e.g., 
'Imports Autodesk.Revit.UI.Selection '' for selection 

#End Region

#Region "Description"
''  Revit Intro Lab - 2 
''  
''  In this lab, you will learn how an element is represended in Revit. 
''  Disclaimer: minimum error checking to focus on the main topic. 
'' 
#End Region

''  DBElement - identifying element 
''
<Transaction(TransactionMode.Automatic)> _
<Regeneration(RegenerationOption.Manual)> _
Public Class DBElement
    Implements IExternalCommand

    ''  member variables 
    Dim m_rvtApp As Application
    Dim m_rvtDoc As Document

    Public Function Execute(ByVal commandData As ExternalCommandData, _
                            ByRef message As String, _
                            ByVal elements As ElementSet) _
                            As Result _
                            Implements IExternalCommand.Execute

        ''  Get the access to the top most objects.
        ''  Notice that we have UI and DB versions for application and Document.
        ''  (We list them both here to show two versions.)  
        '' 
        Dim rvtUIApp As UIApplication = commandData.Application
        Dim rvtUIDoc As UIDocument = rvtUIApp.ActiveUIDocument
        m_rvtApp = rvtUIApp.Application
        m_rvtDoc = rvtUIDoc.Document

        ''  (1) select an object on a screen. (We'll come back to the selection in the UI Lab later.) 
        Dim ref As Reference = _
            rvtUIDoc.Selection.PickObject(ObjectType.Element, "Pick an element")

        ''  we have picked something. 
        Dim elem As Element = ref.Element

        ''  (2) let's see what kind of element we got. 
        ''  Key properties that we need to check are: Class, Category and if an element is ElementType or not. 
        ''  
        ShowBasicElementInfo(elem)

        ' ''  now check and see the basic info of the element type 
        'Dim elemTypeId As ElementId = elem.GetTypeId
        'Dim elemType As ElementType = m_rvtDoc.Element(elemTypeId)
        'ShowBasicElementInfo(elemType)

        ''  (3) now, we are going to identify each major types of element. 
        IdentifyElement(elem)

        ''  Now look at other properties - important ones are parameters, locations and geometry. 

        ''  (4) first parameters. 
        '' 
        ShowParameters(elem, "Element Parameters: ")

        ''  check to see its type parameter as well
        '' 
        Dim elemTypeId As ElementId = elem.GetTypeId
        Dim elemType As ElementType = m_rvtDoc.Element(elemTypeId)
        ShowParameters(elemType, "Type Parameters: ")

        ''  okay. we saw a set or parameters for a given element or element type.
        ''  how can we access to each parameters. For example, how can we get the value of "length" information?
        ''  here is how: 

        ' ''  select a wall object on a screen. 
        'ref = rvtUIDoc.Selection.PickObject(Selection.ObjectType.Element, "Pick a wall element")

        ' ''  we have picked something. 
        'elem = ref.Element

        RetrieveParameter(elem, "Element Parameter (by Name and BuiltInParameter): ")
        ''  the same logic applies to the type parameter. 
        RetrieveParameter(elemType, "Type Parameter (by Name and BuiltInParameter): ")

        ''  (5) location
        ShowLocation(elem)

        ''  (6) geometry - the last piece. (Optional) 
        ShowGeometry(elem)

        ''  these are the common proerties. 
        ''  there may be more properties specific to the given element class,  
        ''  such as Wall.Width, .Flipped and Orientation. Expore using RevitLookup and RevitAPI.chm. 
        ''

        ''  we are done. 
        Return Result.Succeeded

    End Function

    ''-------1---------2---------3----------4---------5--------6---------7---
    ''-----------------------------------------------------------------------  
    ''  ShowBasicElementInfo() 
    '' 
    ''  Show hows basic information about the given element. 
    ''  Note: we are intentionally including both element and element type 
    ''  here to compare the output on the same dialog. 
    ''  Compare, for example, the categories of element and element type.  
    ''-----------------------------------------------------------------------
    Public Sub ShowBasicElementInfo(ByVal elem As Element)

        ''  let's see what kind of element we got. 
        Dim s As String = "You picked:" + vbCr
        s = s + "  Class name = " + elem.GetType.Name + vbCr
        s = s + "  Category = " + elem.Category.Name + vbCr
        s = s + "  Element id = " + elem.Id.ToString + vbCr + vbCr

        ''  and check its type info. 
        ''Dim elemType As ElementType = elem.ObjectType  '' Note: this is obsolete.
        Dim elemTypeId As ElementId = elem.GetTypeId
        Dim elemType As ElementType = m_rvtDoc.Element(elemTypeId)
        s = s + "Its ElementType:" + vbCr
        s = s + "  Class name = " + elemType.GetType.Name + vbCr
        s = s + "  Category = " + elemType.Category.Name + vbCr
        s = s + "  Element type id = " + elemType.Id.ToString + vbCr

        ''  show what we got. 
        TaskDialog.Show("Revit Intro Lab", s)

    End Sub

    ''  identify the type of the element known to the UI. 
    ''
    Public Sub IdentifyElement(ByVal elem As Element)

        ''  An instance of a system family has a designated class. 
        ''  You can use it identify the type of element. 
        ''  e.g., walls, floors, roofs. 
        '' 
        Dim s As String = ""

        If TypeOf elem Is Wall Then
            s = "Wall"
        ElseIf TypeOf elem Is Floor Then
            s = "Floor"
        ElseIf TypeOf elem Is RoofBase Then
            s = "Roof"
        ElseIf TypeOf elem Is FamilyInstance Then
            ''  An instance of a component family is all FamilyInstance.
            ''  We'll need to further check its category. 
            ''  e.g., Doors, Windows, Furnitures. 
            If elem.Category.Id.IntegerValue = _
                BuiltInCategory.OST_Doors Then
                s = "Door"
            ElseIf elem.Category.Id.IntegerValue = _
                BuiltInCategory.OST_Windows Then
                s = "Window"
            ElseIf elem.Category.Id.IntegerValue = _
                BuiltInCategory.OST_Furniture Then
                s = "Furniture"
            Else
                s = "Component family instance"  '' e.g. Plant 
            End If

            '' check the base class. e.g., CeilingAndFloor. 
        ElseIf TypeOf elem Is HostObject Then
            s = "System family instance"
        Else
            s = "Other"
        End If

        s = "You have picked: " + s
        ''  show it. 
        TaskDialog.Show("Revit Intro Lab", s)

    End Sub

    '' 
    ''  show the parameter values of the element 
    ''
    Public Sub ShowParameters(ByVal elem As Element, Optional ByVal header As String = "")

        Dim s As String = header + vbCr + vbCr
        Dim params As ParameterSet = elem.Parameters

        For Each param As Parameter In params
            Dim name As String = param.Definition.Name
            ''  to get the value, we need to pause the param depending on the strage type
            ''  see the helper function below 
            Dim val As String = ParameterToString(param)
            s = s + name + " = " + val + vbCr
        Next

        TaskDialog.Show("Revit Intro Lab", s)

    End Sub

    ''
    ''  Helper function: return a string from of a given parameter.  
    ''
    Public Shared Function ParameterToString(ByVal param As Parameter) As String

        Dim val As String = "none"

        If param Is Nothing Then
            Return val
        End If

        ''  to get to the parameter value, we need to pause it depending on its strage type
        '' 
        Select Case param.StorageType
            Case StorageType.Double
                Dim dVal As Double = param.AsDouble
                val = dVal.ToString

            Case StorageType.Integer
                Dim iVal As Integer = param.AsInteger
                val = iVal.ToString()

            Case StorageType.String
                Dim sVal As String = param.AsString
                val = sVal

            Case StorageType.ElementId
                Dim idVal As ElementId = param.AsElementId
                val = idVal.IntegerValue.ToString

            Case StorageType.None
            Case Else

        End Select

        Return val

    End Function

    ''  examples of retrieving a specific parameter indivisually.  
    ''  (harding coding for simplicity. This function works best with walls and doors.) 
    ''
    Public Sub RetrieveParameter(ByVal elem As Element, Optional ByVal header As String = "")

        Dim s As String = header + vbCr + vbCr

        ''  as an experiment, let's pick up some arbitrary parameters. 
        ''  comments - most of instance has this parameter 

        ''  (1) by BuiltInParameter.
        Dim param As Parameter = elem.Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS)
        If param IsNot Nothing Then
            s = s + "Comments (by BuiltInParameter) =  " + ParameterToString(param) + vbCr
        End If

        ''  (2) by name.  (Mark - most of instance has this parameter.) if you use this method, it will language specific. 
        param = elem.Parameter("Mark")
        If param IsNot Nothing Then
            s = s + "Mark (by Name) = " + ParameterToString(param) + vbCr
        End If

        ''  though the first one is the most commonly used, other possible methods are: 
        ''  (3) by definition 
        'param = elem.Parameter(Definition) 
        ''  (4) and for shared parameters, you can also use GUID. 
        'parameter = Parameter(GUID) 


        ''  the following should be in most of type parameter 
        '' 
        param = elem.Parameter(BuiltInParameter.ALL_MODEL_TYPE_COMMENTS)
        If param IsNot Nothing Then
            s = s + "Type Comments (by BuiltInParameter) = " + ParameterToString(param) + vbCr
        End If

        param = elem.Parameter("Fire Rating")
        If param IsNot Nothing Then
            s = s + "Fire Rating (by Name) = " + ParameterToString(param) + vbCr
        End If

        ''  using the BuiltInParameter, you can sometimes access one that is not in the parameters set. 
        ''  Note: this works only for element type. 
        ''  [MH3i: To Do - check c# version. 4/26. ]
        param = elem.Parameter(BuiltInParameter.SYMBOL_FAMILY_AND_TYPE_NAMES_PARAM)
        If param IsNot Nothing Then
            s = s + "SYMBOL_FAMILY_AND_TYPE_NAMES_PARAM (only by BuiltInParameter) = " + _
            ParameterToString(param) + vbCr
        End If

        param = elem.Parameter(BuiltInParameter.SYMBOL_FAMILY_NAME_PARAM)
        If param IsNot Nothing Then
            s = s + "SYMBOL_FAMILY_NAME_PARAM (only by BuiltInParameter) = " + _
            ParameterToString(param) + vbCr
        End If

        '' show it. 
        TaskDialog.Show("Revit Intro Lab", s)

    End Sub

    ''  show the location information of the given element. 
    ''  location can be LocationPoint (e.g., furniture), and LocationCurve (e.g., wall). 
    ''
    Public Sub ShowLocation(ByVal elem As Element)

        Dim s As String = "Location Information: " + vbCr + vbCr
        Dim loc As Location = elem.Location

        If TypeOf loc Is LocationPoint Then

            '' (1) we have a location point
            '' 
            Dim locPoint As LocationPoint = loc
            Dim pt As XYZ = locPoint.Point
            Dim r As Double = locPoint.Rotation

            s = s + "LocationPoint" + vbCr
            s = s + "Point = " + PointToString(pt) + vbCr
            s = s + "Rotation = " + r.ToString + vbCr

        ElseIf TypeOf loc Is LocationCurve Then

            '' (2) we have a location curve 
            '' 
            Dim locCurve As LocationCurve = loc
            Dim crv As Curve = locCurve.Curve

            s = s + "LocationCurve" + vbCr
            s = s + "EndPoint(0)/Start Point = " + PointToString(crv.EndPoint(0)) + vbCr
            s = s + "EndPoint(1)/End point = " + PointToString(crv.EndPoint(1)) + vbCr
            s = s + "Length = " + crv.Length.ToString + vbCr
            '' check join type [MH3i: To Do - add in C# 4/26]
            s = s + "JoinType(0) = " + locCurve.JoinType(0).ToString + vbCr
            s = s + "JoinType(1) = " + locCurve.JoinType(1).ToString + vbCr

        End If

        '' show it 
        TaskDialog.Show("Revit Intro Lab", s)

    End Sub

    ''  Helper Function: returns XYZ in a string form.  
    ''
    Public Shared Function PointToString(ByVal pt As XYZ) As String

        If pt Is Nothing Then
            Return ""
        End If

        Return "(" + pt.X.ToString("F2") + ", " + pt.Y.ToString("F2") + ", " + pt.Z.ToString("F2") + ")"

    End Function

    ''  This is lengthy. So Optional: 
    ''  show the geometry information of the given element. Here is how to access it. 
    ''  you can go through by RevitLookup, instead. 
    ''
    Public Sub ShowGeometry(ByVal elem As Element)

        Dim s As String = "Geometry Information: " + vbCr + vbCr

        ''  first, set a geometry option
        Dim opt As Options = m_rvtApp.Create.NewGeometryOptions
        opt.DetailLevel = DetailLevels.Fine

        ''  does the element have the geometry data? 
        Dim geomElem As GeometryElement = elem.Geometry(opt)
        If geomElem Is Nothing Then
            TaskDialog.Show("Revit Intro Lab", s + "no data")
            Return
        End If

        ''  get the geometry information from the geom elem. 
        ''  geometry informaion can easily go into depth. 
        ''  here we look at at top level. use RevitLookup for complee dril down. 
        '' 
        s = GeometryElementToString(geomElem)

        '' show it. 
        TaskDialog.Show("Revit Intro Lab", s)

    End Sub

    ''  Helper Function: parse the geometry element by geometry type. 
    ''  see ReviCommands in the SDK sample for complete implementation. 
    ''
    Public Shared Function GeometryElementToString(ByVal geomElem As GeometryElement) As String

        Dim geomObjs As GeometryObjectArray = geomElem.Objects

        Dim str As String = "Total number of GeometryObject: " & geomObjs.Size.ToString & vbCr

        For Each geomObj As GeometryObject In geomObjs

            If TypeOf geomObj Is Solid Then  '  ex. wall

                Dim solid As Solid = geomObj
                'str = str & GeometrySolidToString(solid)
                str = str + "Solid" + vbCr

            ElseIf TypeOf geomObj Is GeometryInstance Then ' ex. door/window

                str = str & "  -- Geometry.Instance -- " & vbCr
                Dim geomInstance As GeometryInstance = geomObj
                Dim geoElem As GeometryElement = geomInstance.SymbolGeometry()
                str = str & GeometryElementToString(geoElem)

            ElseIf TypeOf geomObj Is Curve Then ' ex. 

                Dim curv As Curve = geomObj
                'str = str & GeometryCurveToString(curv)
                str = str + "Curve" + vbCr

            ElseIf TypeOf geomObj Is Mesh Then ' ex. 

                Dim mesh As Mesh = geomObj
                'str = str & GeometryMeshToString(mesh)
                str = str + "Mesh" + vbCr

            Else
                str = str & "  *** unkown geometry type" & geomObj.GetType.ToString

            End If

        Next

        Return str

    End Function

End Class