#Region "Copyright"
'
' Copyright (C) 2010-2012 by Autodesk, Inc.
'
' Permission to use, copy, modify, and distribute this software in
' object code form for any purpose and without fee is hereby granted,
' provided that the above copyright notice appears in all copies and
' that both that copyright notice and the limited warranty and
' restricted rights notice below appear in all supporting
' documentation.
'
' AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
' AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
' MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC.
' DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
' UNINTERRUPTED OR ERROR FREE.
'
' Use, duplication, or disclosure by the U.S. Government is subject to
' restrictions set forth in FAR 52.227-19 (Commercial Computer
' Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
' (Rights in Technical Data and Computer Software), as applicable.
#End Region

Imports System.Collections.Generic
Imports System.Linq

#Region "Revit Namespaces"

Imports Autodesk.Revit.DB
Imports Autodesk.Revit.UI
Imports Autodesk.Revit.UI.Selection
Imports Autodesk.Revit.Attributes
Imports Autodesk.Revit.ApplicationServices

#End Region

<Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Automatic)> _
Public Class HelloWorldCmd
    Implements Autodesk.Revit.UI.IExternalCommand

    Public Function Execute _
                ( _
                    ByVal commandData As Autodesk.Revit.UI.ExternalCommandData, _
                    ByRef message As String, _
                    ByVal elements As Autodesk.Revit.DB.ElementSet _
                ) As Autodesk.Revit.UI.Result _
            Implements Autodesk.Revit.UI.IExternalCommand.Execute

        Autodesk.Revit.UI.TaskDialog.Show("My Dialog Title", "Hello World!")

        Return Autodesk.Revit.UI.Result.Succeeded
    End Function
End Class

''' <summary>
''' Hello World #3 - minimum external application 
''' Difference: IExternalApplication instead of IExternalCommand. 
''' In addin manifest, use addin type "Application" and Name instead of Text tag. 
''' </summary>
Public Class HelloWorldApp
    Implements Autodesk.Revit.UI.IExternalApplication
    ' OnStartup() - called when Revit starts. 

    Public Function OnStartup(ByVal app As UIControlledApplication) As Result Implements Autodesk.Revit.UI.IExternalApplication.OnStartup
        TaskDialog.Show("My Dialog Title", "Hello World from App!")

        Return Result.Succeeded
    End Function

    ' OnShutdown() - called when Revit ends. 

    Public Function OnShutdown(ByVal app As UIControlledApplication) As Result Implements Autodesk.Revit.UI.IExternalApplication.OnShutdown
        Return Result.Succeeded
    End Function
End Class

''' <summary>
''' Command Arguments 
''' Take a look at the command arguments. 
''' commandData is the topmost object and 
''' provides the entry point to the Revit model. 
''' </summary>
<Transaction(TransactionMode.Automatic)> _
Public Class CommandData
    Implements Autodesk.Revit.UI.IExternalCommand
    Public Function Execute(ByVal commandData As ExternalCommandData, ByRef message As String, ByVal elements As ElementSet) As Result Implements Autodesk.Revit.UI.IExternalCommand.Execute
        ' The first argument, commandData, provides access to the top most object model. 
        ' You will get the necessary information from commandData. 
        ' To see what's in there, print out a few data accessed from commandData 
        ' 
        ' Exercise: Place a break point at commandData and drill down the data. 

        Dim uiApp As UIApplication = commandData.Application
        Dim rvtApp As Application = uiApp.Application
        Dim uiDoc As UIDocument = uiApp.ActiveUIDocument
        Dim rvtDoc As Document = uiDoc.Document

        ' Print out a few information that you can get from commandData 
        Dim versionName As String = rvtApp.VersionName
        Dim documentTitle As String = rvtDoc.Title

        TaskDialog.Show("Revit Intro Lab", "Version Name = " & versionName & vbLf & "Document Title = " & documentTitle)

        ' Print out a list of wall types available in the current rvt project:

        Dim wallTypes As WallTypeSet = rvtDoc.WallTypes

        Dim s As String = ""
        For Each wallType As WallType In wallTypes
            s += wallType.Name + vbCr & vbLf
        Next

        ' Show the result:

        TaskDialog.Show("Revit Intro Lab", "Wall Types (in main instruction):" & vbLf & vbLf & s)

        ' 2nd and 3rd arguments are when the command fails. 
        ' 2nd - set a message to the user. 
        ' 3rd - set elements to highlight. 

        Return Result.Succeeded
    End Function
End Class

<Transaction(TransactionMode.Automatic)> _
Public Class SelectedElements
    Implements Autodesk.Revit.UI.IExternalCommand
    Public Function Execute(ByVal commandData As ExternalCommandData, ByRef message As String, ByVal elements As ElementSet) As Result Implements Autodesk.Revit.UI.IExternalCommand.Execute
        ' Get the access to the top most objects. 
        ' Notice that we have UI and DB versions for application and Document. 
        ' (We list them both here to show two versions.) 

        Dim uiApp As UIApplication = commandData.Application
        Dim app As Application = uiApp.Application
        Dim uiDoc As UIDocument = uiApp.ActiveUIDocument
        Dim doc As Document = uiDoc.Document

        Dim selElementSet As Autodesk.Revit.UI.Selection.SelElementSet = uiDoc.Selection.Elements
        For Each e As Element In selElementSet
            ' Let's see what kind of element we got. 
            Dim s As String = (("You picked:" & vbCr & vbLf & "Class name = ") + e.[GetType]().Name & vbCr & vbLf & "Category = ") + e.Category.Name & vbCr & vbLf & "Element id = " & e.Id.ToString()

            ' And check its type info. 
            Dim elemTypeId As ElementId = e.GetTypeId()

            Dim elemType As ElementType = DirectCast(doc.GetElement(elemTypeId), ElementType)

            s += ((vbCr & vbLf & "Its ElementType:" & " Class name = ") + elemType.[GetType]().Name & " Category = ") + elemType.Category.Name & " Element type id = " & elemType.Id.ToString()

            ' Show what we got. 
            TaskDialog.Show("Basic Element Info", s)
        Next

        Return Result.Succeeded
    End Function
End Class

<Transaction(TransactionMode.Automatic)> _
Public Class FilteredElements
    Implements Autodesk.Revit.UI.IExternalCommand
    Public Function Execute(ByVal commandData As ExternalCommandData, ByRef message As String, ByVal elements As ElementSet) As Result Implements Autodesk.Revit.UI.IExternalCommand.Execute
        ' Get the access to the top most objects. 
        ' Notice that we have UI and DB versions for application and Document. 
        ' (We list them both here to show two versions.) 

        Dim uiApp As UIApplication = commandData.Application
        Dim app As Application = uiApp.Application
        Dim uiDoc As UIDocument = uiApp.ActiveUIDocument
        Dim doc As Document = uiDoc.Document


        ' First, narrow down to the wall elements

        Dim wallTypeCollector As New FilteredElementCollector(doc)
        ' Method : 1
        wallTypeCollector = wallTypeCollector.WherePasses(New ElementClassFilter(GetType(WallType)))

        ' Method : 2
        'wallTypeCollector = wallTypeCollector.OfClass(typeof(Wall));


        ' Constant for this function. 
        Const kWallLength As Double = 60.0
        ' 60 feet. hard coding for simplicity. 
        ' Define a filter by parameter 
        ' 1st arg - value provider 
        Dim lengthParam As BuiltInParameter = BuiltInParameter.CURVE_ELEM_LENGTH
        Dim iLengthParam As Integer = CInt(lengthParam)
        Dim paramValueProvider = New ParameterValueProvider(New ElementId(iLengthParam))

        ' 2nd - evaluator 
        Dim evaluator As New FilterNumericGreater()

        ' 3rd - rule value 
        Dim ruleVal As Double = kWallLength

        ' 4th - epsilon 
        Const eps As Double = 0.000001

        ' Define a rule 
        Dim filterRule = New FilterDoubleRule(paramValueProvider, evaluator, ruleVal, eps)

        ' Create a new filter 
        Dim paramFilter = New ElementParameterFilter(filterRule)

        ' Go through the filter 
        Dim elems As IList(Of Element) = wallTypeCollector.WherePasses(paramFilter).ToElements()
        ' We now have the list of Long walls
        For Each e As Element In elems
        Next

        Return Result.Succeeded
    End Function

End Class

<Transaction(TransactionMode.Automatic)> _
Public Class AllElements
    Implements Autodesk.Revit.UI.IExternalCommand
    Public Function Execute(ByVal commandData As ExternalCommandData, ByRef message As String, ByVal elements As ElementSet) As Result Implements Autodesk.Revit.UI.IExternalCommand.Execute
        Dim uiApp As UIApplication = commandData.Application
        Dim app As Application = uiApp.Application
        Dim uiDoc As UIDocument = uiApp.ActiveUIDocument
        Dim doc As Document = uiDoc.Document

        Dim collector1 As FilteredElementCollector = New FilteredElementCollector(doc).WhereElementIsElementType()

        Dim collector2 As FilteredElementCollector = New FilteredElementCollector(doc).WhereElementIsNotElementType()

        collector1.UnionWith(collector2)

        ' Loop over the elements and list their data:
        For Each e As Element In collector1
        Next
        Return Result.Succeeded
    End Function
End Class

<Transaction(TransactionMode.Automatic)> _
Public Class ElementIdentification
    Implements Autodesk.Revit.UI.IExternalCommand
    Public Function Execute(ByVal commandData As ExternalCommandData, ByRef message As String, ByVal elements As ElementSet) As Result Implements Autodesk.Revit.UI.IExternalCommand.Execute
        Dim uiApp As UIApplication = commandData.Application
        Dim app As Application = uiApp.Application
        Dim uiDoc As UIDocument = uiApp.ActiveUIDocument
        Dim doc As Document = uiDoc.Document

        'GetAllWalls(doc);
        'GetAllDoors(doc);
        'return Result.Succeeded;

        Dim r As Reference = uiDoc.Selection.PickObject(ObjectType.Element, "Pick an element")

        ' We have picked something. 
        Dim e As Element = uiDoc.Document.GetElement(r)

        ' An instance of a system family has a designated class. 
        ' You can use it identify the type of element. 
        ' e.g., walls, floors, roofs. 

        Dim s As String = ""

        If TypeOf e Is Wall Then
            s = "Wall"
        ElseIf TypeOf e Is Floor Then
            s = "Floor"
        ElseIf TypeOf e Is RoofBase Then
            s = "Roof"
        ElseIf TypeOf e Is FamilyInstance Then
            ' An instance of a component family is all FamilyInstance. 
            ' We'll need to further check its category. 
            ' e.g., Doors, Windows, Furnitures. 
            If e.Category.Id.IntegerValue = CInt(BuiltInCategory.OST_Doors) Then
                s = "Door"
            ElseIf e.Category.Id.IntegerValue = CInt(BuiltInCategory.OST_Windows) Then
                s = "Window"
            ElseIf e.Category.Id.IntegerValue = CInt(BuiltInCategory.OST_Furniture) Then
                s = "Furniture"
            Else
                ' e.g. Plant 
                s = "Component family instance"
            End If
            ' Check the base class. e.g., CeilingAndFloor. 
        ElseIf TypeOf e Is HostObject Then
            s = "System family instance"
        Else
            s = "Other"
        End If

        s = "You have picked: " & s

        TaskDialog.Show("Identify Element", s)

        Return Result.Succeeded
    End Function

    Public Shared Function GetAllWalls(ByVal doc As Document) As IList(Of Element)
        Dim wallTypeCollector As New FilteredElementCollector(doc)
        wallTypeCollector = wallTypeCollector.OfClass(GetType(Wall))

        ' We now have the list of walls
        Dim walls As IList(Of Element) = wallTypeCollector.ToElements()

        Dim msg As [String] = [String].Empty
        For Each e As Element In walls
            Dim wall As Wall = TryCast(e, Wall)
            msg = msg & [String].Format("{0}{1} - {2}", Environment.NewLine, wall.Id.IntegerValue.ToString(), wall.WallType.Name)
        Next
        TaskDialog.Show("All walls in the model", msg)

        Return walls
    End Function

    Public Shared Function GetAllDoors(ByVal doc As Document) As IList(Of Element)
        Dim collector As New FilteredElementCollector(doc)

        collector = collector.OfClass(GetType(FamilyInstance))
        collector = collector.OfCategory(BuiltInCategory.OST_Doors)

        ' We now have the list of doors
        Dim doors As IList(Of Element) = collector.ToElements()

        Dim msg As [String] = [String].Empty
        For Each e As Element In doors
            Dim elemTypeId As ElementId = e.GetTypeId()
            Dim elemType As ElementType = DirectCast(doc.GetElement(elemTypeId), ElementType)
            Dim param As Parameter = elemType.Parameter(BuiltInParameter.SYMBOL_FAMILY_AND_TYPE_NAMES_PARAM)
            If param IsNot Nothing Then
                msg = msg & [String].Format("{0}{1} - {2}{3}", Environment.NewLine, e.Id.IntegerValue.ToString(), elemType.Name, param.AsString())
            End If
        Next
        TaskDialog.Show("All doors in the model", msg)

        Return doors
    End Function
End Class

<Transaction(TransactionMode.Automatic)> _
Public Class ElementCreation
    Implements Autodesk.Revit.UI.IExternalCommand
    Const _mmToFeet As Double = 0.0032808399

    Public Function Execute(ByVal commandData As ExternalCommandData, ByRef message As String, ByVal elements As ElementSet) As Result Implements Autodesk.Revit.UI.IExternalCommand.Execute
        Dim uiApp As UIApplication = commandData.Application
        Dim app As Application = uiApp.Application
        Dim uiDoc As UIDocument = uiApp.ActiveUIDocument
        Dim doc As Document = uiDoc.Document

        ' First, narrow down to the elements of the given type and category 
        Dim collector = New FilteredElementCollector(doc).OfClass(GetType(Level))

        ' Parse the collection for the given names 
        ' Using LINQ query here. 
        Dim elems = From element In collector Where element.Name.Equals("Level 1") Select element

        ' Put the result as a list of element for accessibility. 

        Dim levels As IList(Of Element) = elems.ToList()
        Dim level1 As Level = TryCast(levels(0), Level)


        ' Parse the collection for the given names 
        ' Using LINQ query here. 
        elems = From element In collector Where element.Name.Equals("Level 2") Select element

        ' Put the result as a list of element for accessibility. 

        levels = elems.ToList()
        Dim level2 As Level = TryCast(levels(0), Level)

        ' Hard coding the size of the house for simplicity 
        Dim widthInmm As Double = 10000.0
        Dim depthInmm As Double = 5000.0

        Dim widthInft As Double = widthInmm * _mmToFeet
        Dim depthInft As Double = depthInmm * _mmToFeet

        Dim pt1 As New XYZ(-widthInft / 2.0, -depthInmm / 2.0, 0.0)
        Dim pt2 As New XYZ(widthInft / 2.0, -depthInmm / 2.0, 0.0)

        ' Define a base curve from two points. 
        Dim baseCurve As Line = app.Create.NewLineBound(pt1, pt2)
        ' Create a wall using the one of overloaded methods. 

        'Wall aWall = _doc.Create.NewWall(baseCurve, level1, isStructural); // 2012

        Dim isStructural As Boolean = True
        Dim aWall As Wall = Wall.Create(doc, baseCurve, level1.Id, isStructural)
        ' since 2013
        ' Set the Top Constraint to Level 2 
        aWall.Parameter(BuiltInParameter.WALL_HEIGHT_TYPE).[Set](level2.Id)

        ' This is important. we need these lines to have shrinkwrap working. 

        doc.Regenerate()
        doc.AutoJoinElements()

        Return Result.Succeeded
    End Function
End Class

<Transaction(TransactionMode.Automatic)> _
Public Class ElementModification
    Implements Autodesk.Revit.UI.IExternalCommand
    Const _mmToFeet As Double = 0.0032808399

    Public Function Execute(ByVal commandData As ExternalCommandData, ByRef message As String, ByVal elements As ElementSet) As Result Implements Autodesk.Revit.UI.IExternalCommand.Execute
        Dim uiApp As UIApplication = commandData.Application
        Dim app As Application = uiApp.Application
        Dim uiDoc As UIDocument = uiApp.ActiveUIDocument
        Dim doc As Document = uiDoc.Document

        Dim r As Reference = uiDoc.Selection.PickObject(ObjectType.Element, "Pick a wall, please")
        ' We have picked something. 
        Dim e As Element = doc.GetElement(r)

        If Not (TypeOf e Is Wall) Then
            message = "Please select a wall."
            Return Result.Failed
        End If

        Dim aWall As Wall = TryCast(e, Wall)

        Dim wallLocation As LocationCurve = DirectCast(aWall.Location, LocationCurve)

        Dim pt1 As XYZ = wallLocation.Curve.EndPoint(0)
        Dim pt2 As XYZ = wallLocation.Curve.EndPoint(1)

        ' Hard coding the displacement value for simplicity here. 
        ' We can also use the "ElementTransformUtils" class to transform the element
        Dim dtInmm As Double = 1000.0
        Dim dtInft As Double = dtInmm * _mmToFeet
        Dim newPt1 As New XYZ(pt1.X - dtInft, pt1.Y - dtInft, pt1.Z)
        Dim newPt2 As New XYZ(pt2.X - dtInft, pt2.Y - dtInft, pt2.Z)

        ' Create a new line bound. 
        Dim newWallLine As Line = app.Create.NewLineBound(newPt1, newPt2)

        ' Finally change the curve. 
        wallLocation.Curve = newWallLine

        'XYZ v = new XYZ(dtInft, dtInft, 0.0);
        'ElementTransformUtils.MoveElement(doc, e.Id, v); 

        ' Message to the user. 
        TaskDialog.Show("ElementModification - wall", "Location: start point moved -1000.0 in X-direction" & vbCr & vbLf)

        doc.Regenerate()
        Return Result.Succeeded
    End Function
End Class

<Transaction(TransactionMode.Automatic)> _
Public Class BuiltInParameters
    Implements Autodesk.Revit.UI.IExternalCommand
    Public Function Execute(ByVal commandData As ExternalCommandData, ByRef message As String, ByVal elements As ElementSet) As Result Implements Autodesk.Revit.UI.IExternalCommand.Execute
        Dim uiApp As UIApplication = commandData.Application
        Dim app As Application = uiApp.Application
        Dim uiDoc As UIDocument = uiApp.ActiveUIDocument
        Dim doc As Document = uiDoc.Document

        Dim r As Reference = uiDoc.Selection.PickObject(ObjectType.Element, "Pick a wall, please")
        ' We have picked something. 
        Dim e As Element = doc.GetElement(r)

        Dim s As String = String.Empty

        For Each param As Parameter In e.Parameters
            Dim name As String = param.Definition.Name
            ' To get the value, we need to pause the param depending on the storage type 
            ' see the helper function below 
            Dim val As String = ParameterToString(param)
            s += vbCr & vbLf & name & " = " & val
        Next

        TaskDialog.Show("Element Parameters: ", s)

        RetrieveParameter(e, "Element Parameter (using BuiltInParameter and Name): ")

        Return Result.Succeeded
    End Function

    ''' <summary>
    ''' Helper function: return a string form of a given parameter.
    ''' </summary>
    Public Shared Function ParameterToString(ByVal param As Parameter) As String
        Dim val As String = "none"

        If param Is Nothing Then
            Return val
        End If

        ' To get to the parameter value, we need to pause it depending on its storage type 

        Select Case param.StorageType
            Case StorageType.[Double]
                Dim dVal As Double = param.AsDouble()
                val = dVal.ToString()
                Exit Select

            Case StorageType.[Integer]
                Dim iVal As Integer = param.AsInteger()
                val = iVal.ToString()
                Exit Select

            Case StorageType.[String]
                Dim sVal As String = param.AsString()
                val = sVal
                Exit Select

            Case StorageType.ElementId
                Dim idVal As ElementId = param.AsElementId()
                val = idVal.IntegerValue.ToString()
                Exit Select

            Case StorageType.None
                Exit Select
        End Select
        Return val
    End Function

    ''' <summary>
    ''' Examples of retrieving a specific parameter indivisually 
    ''' (hard coded for simplicity; This function works best 
    ''' with walls and doors).
    ''' </summary>
    Public Sub RetrieveParameter(ByVal e As Element, ByVal header As String)
        Dim s As String = String.Empty

        ' As an experiment, let's pick up some arbitrary parameters. 
        ' Comments - most of instance has this parameter 

        ' (1) by BuiltInParameter. 
        Dim param As Parameter = e.Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS)
        If param IsNot Nothing Then
            s += "Comments (by BuiltInParameter) = " & ParameterToString(param) & vbLf
        End If

        TaskDialog.Show(header, s)

        ' (2) by name. (Mark - most of instance has this parameter.) if you use this method, it will language specific. 
        param = e.Parameter("Mark")
        If param IsNot Nothing Then
            s += "Mark (by Name) = " & ParameterToString(param) & vbLf
        End If

        ' Though the first one is the most commonly used, other possible methods are: 
        ' (3) by definition 
        ' param = e.Parameter(Definition) 
        ' (4) and for shared parameters, you can also use GUID. 
        ' parameter = Parameter(GUID) 

        ' The following should be in most of type parameter 

        param = e.Parameter(BuiltInParameter.ALL_MODEL_TYPE_COMMENTS)
        If param IsNot Nothing Then
            s += "Type Comments (by BuiltInParameter) = " & ParameterToString(param) & vbLf
        End If

        param = e.Parameter("Fire Rating")
        If param IsNot Nothing Then
            s += "Fire Rating (by Name) = " & ParameterToString(param) & vbLf
        End If

        ' Using the BuiltInParameter, you can sometimes access one that is not in the parameters set. 
        ' Note: this works only for element type. 

        param = e.Parameter(BuiltInParameter.SYMBOL_FAMILY_AND_TYPE_NAMES_PARAM)
        If param IsNot Nothing Then
            s += "SYMBOL_FAMILY_AND_TYPE_NAMES_PARAM (only by BuiltInParameter) = " & ParameterToString(param) & vbLf
        End If

        param = e.Parameter(BuiltInParameter.SYMBOL_FAMILY_NAME_PARAM)
        If param IsNot Nothing Then
            s += "SYMBOL_FAMILY_NAME_PARAM (only by BuiltInParameter) = " & ParameterToString(param) & vbLf
        End If

        ' Show it. 

        TaskDialog.Show(header, s)
    End Sub
End Class
