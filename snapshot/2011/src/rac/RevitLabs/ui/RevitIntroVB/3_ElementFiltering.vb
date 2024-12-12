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
'Imports Autodesk.Revit.ApplicationServices
'Imports Autodesk.Revit.Attributes '' specific this if you want to save typing for attributes. e.g., 

#End Region

#Region "Description"
''  Revit Intro Lab 3 
''
''  In this lab, you will learn how to filter elements 
''  in the previous lab, we have learned how an element is represnted in the revit database. 
''  we learned how to retrieve information, and identify the kind of elements. 
''  in this lab, we'll take a look how to filter element from the database. 
''  Disclaimer: minimum error checking to focus on the main topic. 
'' 
'' MH: my scale(77) 
''-------1---------2---------3---------4---------5---------6---------7-------
'' 
''-------1---------2---------3----------4---------5--------6---------
'' 
#End Region

''  ElementFiltering - 
''
<Transaction(TransactionMode.Automatic)> _
<Regeneration(RegenerationOption.Manual)> _
Public Class ElementFiltering
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
        Dim rvtUIApp As UIApplication = commandData.Application
        Dim rvtUIDoc As UIDocument = rvtUIApp.ActiveUIDocument
        m_rvtApp = rvtUIApp.Application
        m_rvtDoc = rvtUIDoc.Document

        ''  (1) In eailer lab, CommandData command, we learned how to access to the wallType. i.e., 
        ''  here we'll take a look at more on the topic of accessing to elements in the interal rvt project database. 
        ListFamilyTypes()

        ''  (2) list instances of specific object class. 
        ListInstances()

        ''  (3) find a specific family type. 
        FindFamilyType()

        ''  (4) find specific instances, including filtering by parameters.
        FindInstance()

        ''  we are done. 
        Return Result.Succeeded

    End Function

    ''  list the family types 
    ''
    Public Sub ListFamilyTypes()

        '' 
        ''  (1) get a list of family types available in the current rvt project. 
        '' 
        ''   For system family types, there is a designated properties that allows us to directly access to the types. 
        ''   e.g., rvtDoc.WallTypes 
        '' 
        Dim wallTypes As WallTypeSet = m_rvtDoc.WallTypes

        ''  show it. 
        Dim sWallTypes As String = "Wall Types (by rvtDoc.WallTypes): " + _
            wallTypes.Size.ToString + vbCr + vbCr
        For Each wType As WallType In wallTypes
            sWallTypes = sWallTypes + wType.Kind.ToString + " : " + wType.Name + vbCr
        Next
        TaskDialog.Show("Revit Intro Lab", sWallTypes)


        ''  (1.1) same idea applies to other system family, such as Floors, Roofs. 
        ''
        Dim floorTypes As FloorTypeSet = m_rvtDoc.FloorTypes

        ''  show it. 
        Dim sFloorTypes = "Floor Types (by rvtDoc.FloorTypes): " + _
            floorTypes.Size.ToString + vbCr + vbCr
        For Each fType As FloorType In floorTypes
            '' Family name is not in the property for floor. so use BuiltInParameter here. 
            Dim param As Parameter = fType.Parameter(BuiltInParameter.SYMBOL_FAMILY_NAME_PARAM)
            If param IsNot Nothing Then
                sFloorTypes = sFloorTypes + param.AsString
            End If
            sFloorTypes = sFloorTypes + " : " + fType.Name + vbCr
        Next
        TaskDialog.Show("Revit Intro Lab", sFloorTypes)


        ''  (1.2a) another approach is to use a filter. here is an example with wall type. 
        '' 
        Dim wallTypeCollector1 = New FilteredElementCollector(m_rvtDoc)
        wallTypeCollector1.WherePasses(New ElementClassFilter(GetType(WallType)))
        Dim wallTypes1 As IList(Of Element) = wallTypeCollector1.ToElements

        ''  using a helper funtion to display the result here. See code below. 
        ShowElementList(wallTypes1, "Wall Types (by Filter): ") '' use helper function. 

        ''  (1.2b) the following are the same as two lines above. 
        ''  these alternative forms are provided for convenience. 
        ''  using OfClass() 
        ''
        'Dim wallTypeCollector2 = New FilteredElementCollector(m_rvtDoc)
        'wallTypeCollector2.OfClass(GetType(WallType))

        ''  (1.2c) the following are the same as above. For convenience. 
        ''  using short cut this time. 
        '' 
        'Dim wallTypeCollector3 = New FilteredElementCollector(m_rvtDoc).OfClass(GetType(WallType))

        '' 
        '' (2) Listing for component family types.
        '' 
        ''  for component family. it is slightly different. 
        ''  There is no designate property in the document class. 
        ''  you always need to use a filtering. 
        ''  for example, doors and windows. 
        ''  remember for component family, you will need to check element type and category 
        ''  this time using OfClass(). 
        '' 
        Dim doorTypeCollector = New FilteredElementCollector(m_rvtDoc)
        doorTypeCollector.OfClass(GetType(FamilySymbol))
        doorTypeCollector.OfCategory(BuiltInCategory.OST_Doors)
        Dim doorTypes As IList(Of Element) = doorTypeCollector.ToElements

        ShowElementList(doorTypes, "Door Types (by Filter): ")

    End Sub

    ''  To get a list of instances of a specific family type, you will need to use filters. 
    ''  The same idea that we learned for family types applies for instances as well. 
    ''
    Sub ListInstances()

        ''  list all the wall instances 
        Dim wallCollector = _
            New FilteredElementCollector(m_rvtDoc).OfClass(GetType(Wall))
        Dim wallList As IList(Of Element) = wallCollector.ToElements

        ShowElementList(wallList, "Wall Instances: ")

        ''  list all the door instances 
        Dim doorCollector = New FilteredElementCollector(m_rvtDoc). _
            OfClass(GetType(FamilyInstance))
        doorCollector.OfCategory(BuiltInCategory.OST_Doors)
        Dim doorList As IList(Of Element) = doorCollector.ToElements

        ShowElementList(doorList, "Door Instance: ")

    End Sub

    '' 
    ''  Looks at a way to get to the more specific family types with a given name.  
    ''
    '' 
    Sub FindFamilyType()

        ''  In this exercise, we will look for the following family types for wall and door 
        ''  Hard coding for similicity.  modify here if you want to try out with other family types. 

        ''  Constant to this function.         
        ''  this is for wall. e.g., "Basic Wall: Generic - 200mm"
        Const wallFamilyName As String = "Basic Wall"
        Const wallTypeName As String = "Generic - 200mm"
        Const wallFamilyAndTypeName As String = wallFamilyName + ": " + wallTypeName

        ''  this is for door. e.g., "M_Single-Flush: 0915 x 2134mm 
        Const doorFamilyName As String = "M_Single-Flush"
        Const doorTypeName As String = "0915 x 2134mm"
        Const doorFamilyAndTypeName As String = doorFamilyName + ": " + doorTypeName

        ''  keep messages to the user in this function.
        Dim msg As String = "Find Family Type - All -: " + vbCr + vbCr

        ''  (1) get a specific system family type. e.g., wall type. 
        ''  There are a few different ways to do this.

        ''  (1.1) first version uses LINQ query.   
        '' 
        Dim wallType1 As Element = FindFamilyType_Wall_v1(wallFamilyName, wallTypeName)

        ''  show the result.   
        msg = msg + ShowFamilyTypeAndId("Find wall family type (using LINQ): ", _
                                        wallFamilyAndTypeName, wallType1) + vbCr

        ''
        ''  (1.2) Another way is to use iterator.  (cf. look for example, Developer guide 87) 
        '' 
        Dim wallType2 As Element = FindFamilyType_Wall_v2(wallFamilyName, wallTypeName)

        msg = msg + ShowFamilyTypeAndId("Find wall family type (using iterator): ", _
                                wallFamilyAndTypeName, wallType2) + vbCr

        ''  (2) get a specific component family type. e.g., door type.  
        '' 
        ''  (2.1) similar approach as (1.1) using LINQ.
        ''
        Dim doorType1 As Element = FindFamilyType_Door_v1(doorFamilyName, doorTypeName)

        msg = msg + ShowFamilyTypeAndId("Find door type (using LINQ): ", _
                        doorFamilyAndTypeName, doorType1) + vbCr

        ''  (2.2) get a specific door type. the second approach.   
        ''  another approach will be to look up from Family, then from Family.Symbols property.  
        ''  This gets more complicated although it is logical approach.  
        '' 
        Dim doorType2 As Element = FindFamilyType_Door_v2(doorFamilyName, doorTypeName)

        msg = msg + ShowFamilyTypeAndId("Find door type (using Family): ", _
                doorFamilyAndTypeName, doorType2) + vbCr

        ''  (3) here is more generic form. defining a more generalized function below.  
        ''
        ''  (3.1) for the wall type 
        '' 
        Dim wallType3 As Element = _
        FindFamilyType(m_rvtDoc, GetType(WallType), wallFamilyName, wallTypeName)

        msg = msg + ShowFamilyTypeAndId("Find wall type (using generic function): ", _
                                        wallFamilyAndTypeName, wallType3) + vbCr

        ''  (3.2) for the door type.  
        '' 
        Dim doorType3 As Element = _
        FindFamilyType(m_rvtDoc, GetType(FamilySymbol), doorFamilyName, doorTypeName, BuiltInCategory.OST_Doors)

        msg = msg + ShowFamilyTypeAndId("Find door type (using generic function): ", _
                                        doorFamilyAndTypeName, doorType3) + vbCr

        ''  Finally, show the result all together
        TaskDialog.Show("Revit Intro Lab", msg)

    End Sub

    ''  Find a specific family type for a wall with a given family and type names.   
    ''  This version uses LINQ query. 
    ''
    Function FindFamilyType_Wall_v1(ByVal wallFamilyName As String, ByVal wallTypeName As String) As Element

        ''  narrow down a collector with class. 
        Dim wallTypeCollector1 = New FilteredElementCollector(m_rvtDoc)
        wallTypeCollector1.OfClass(GetType(WallType))

        ''  LINQ query 
        Dim wallTypeElems1 = _
            From element In wallTypeCollector1 _
            Where element.Name.Equals(wallTypeName) _
            Select element

        ''  get the result. 
        Dim wallType1 As Element = Nothing '' result will go here. 
        ''  (1) directly accessing from the query result. 
        If wallTypeElems1.Count > 0 Then
            wallType1 = wallTypeElems1.First
        End If

        ''  (2) if you want to get the result as a list of element, here is how.  
        'Dim wallTypeList1 As IList(Of Element) = wallTypeElems1.ToList()
        'If wallTypeList1.Count > 0 Then
        '    wallType1 = wallTypeList1(0) ' found it. 
        'End If

        Return wallType1

    End Function

    ''  Find a specific family type for a wall, which is a system family.  
    ''  This version uses iteration.  (cf. look for example, Developer guide 87)
    ''
    Function FindFamilyType_Wall_v2(ByVal wallFamilyName As String, ByVal wallTypeName As String) As Element

        ''  first, narrow down the collector by Class 
        Dim wallTypeCollector2 = New FilteredElementCollector(m_rvtDoc).OfClass(GetType(WallType))

        ''  use iterator 
        Dim wallTypeItr As FilteredElementIterator = wallTypeCollector2.GetElementIterator
        wallTypeItr.Reset()
        Dim wallType2 As Element = Nothing
        While wallTypeItr.MoveNext
            Dim wType As WallType = wallTypeItr.Current
            ''  we check two names for the match: type name and family name. 
            If (wType.Name = wallTypeName) And _
            (wType.Parameter(BuiltInParameter.SYMBOL_FAMILY_NAME_PARAM).AsString.Equals(wallFamilyName)) Then
                wallType2 = wType ''  we found it. 
                Exit While
            End If
        End While

        Return wallType2

    End Function

    ''  Find a specific family type for a door, which is a component family.  
    ''  This version uses LINQ. 
    ''
    Function FindFamilyType_Door_v1(ByVal doorFamilyName As String, ByVal doorTypeName As String) As Element

        ''  narrow down the collection with class and category. 
        Dim doorFamilyCollector1 = New FilteredElementCollector(m_rvtDoc)
        doorFamilyCollector1.OfClass(GetType(FamilySymbol))
        doorFamilyCollector1.OfCategory(BuiltInCategory.OST_Doors)

        ''  parse the collection for the given name
        ''  using LINQ query here.   
        Dim doorTypeElems = _
            From element In doorFamilyCollector1 _
            Where element.Name.Equals(doorTypeName) And _
            element.Parameter(BuiltInParameter.SYMBOL_FAMILY_NAME_PARAM).AsString.Equals(doorFamilyName) _
            Select element

        ''  get the result. 
        Dim doorType1 As Element = Nothing
        '' (1) directly accessing from the query result 
        'If doorTypeElems.Count > 0 Then '' we should have only one with the given name. minimum error checking.   
        '    doorType1 = doorTypeElems(0) ' found it. 
        'End If

        '' (2) if we want to get the list of element, here is how.  
        Dim doorTypeList As IList(Of Element) = doorTypeElems.ToList()
        If doorTypeList.Count > 0 Then '' we should have only one with the given name. minimum error checking.   
            doorType1 = doorTypeList(0) ' found it. 
        End If

        Return doorType1

    End Function

    ''  Find a specific family type for a door.  
    ''  another approach will be to look up from Family, then from Family.Symbols property.  
    ''  This gets more complicated although it is logical approach. 
    ''
    Function FindFamilyType_Door_v2(ByVal doorFamilyName As String, ByVal doorTypeName As String) As Element

        ''  (1) find the family with the given name. 
        '' 
        Dim familyCollector = New FilteredElementCollector(m_rvtDoc)
        familyCollector.OfClass(GetType(Family))

        ''  use the iterator 
        Dim doorFamily As Family = Nothing
        Dim familyItr As FilteredElementIterator = familyCollector.GetElementIterator
        'familyItr.Reset()
        While (familyItr.MoveNext)
            Dim fam As Family = familyItr.Current
            ''  check name and categoty 
            If (fam.Name = doorFamilyName) And _
            (fam.FamilyCategory.Id.IntegerValue = BuiltInCategory.OST_Doors) Then
                ''  we found the family. 
                doorFamily = fam
                Exit While
            End If
        End While

        ''  (2) find the type with the given name. 
        '' 
        Dim doorType2 As Element = Nothing '' id of door type we are looking for. 
        If doorFamily IsNot Nothing Then
            '' if we have a family, then proceed with finding a type under Symbols property.  
            Dim doorFamilySymbolSet As FamilySymbolSet = doorFamily.Symbols

            '' iterate through the set of family symbols. 
            Dim doorTypeItr As FamilySymbolSetIterator = doorFamilySymbolSet.ForwardIterator
            While doorTypeItr.MoveNext
                Dim dType As FamilySymbol = doorTypeItr.Current
                If (dType.Name = doorTypeName) Then
                    doorType2 = dType '' found it.
                    Exit While
                End If
            End While
        End If

        Return doorType2

    End Function

    ''
    ''  Find specific instances, including filtering by parameters. 
    ''
    Sub FindInstance()

        ''  Constant to this function. (we may want to change the value here.)         
        ''  this is for wall. e.g., "Basic Wall: Generic - 200mm"
        Const wallFamilyName As String = "Basic Wall"
        Const wallTypeName As String = "Generic - 200mm"
        Const wallFamilyAndTypeName As String = wallFamilyName + ": " + wallTypeName

        ''  this is for door. e.g., "M_Single-Flush: 0915 x 2134mm 
        Const doorFamilyName As String = "M_Single-Flush"
        Const doorTypeName As String = "0915 x 2134mm"
        Const doorFamilyAndTypeName As String = doorFamilyName + ": " + doorTypeName


        ''  (1) find walls with a specific type
        '' 
        ''  find a specific family type. use the function we defined earlier.   
        Dim idWallType As ElementId = FindFamilyType(m_rvtDoc, GetType(WallType), wallFamilyName, wallTypeName).Id
        ''  find instances of the given family type. 
        Dim walls As IList(Of Element) = FindInstancesOfType(GetType(Wall), idWallType)

        ''  show it. 
        Dim msgWalls As String = "Instances of wall with type: " + wallFamilyAndTypeName + vbCr
        ShowElementList(walls, msgWalls)


        ''  (2) find a specific door. same idea. 
        Dim idDoorType As ElementId = _
        FindFamilyType(m_rvtDoc, GetType(FamilySymbol), doorFamilyName, doorTypeName, BuiltInCategory.OST_Doors).Id
        Dim doors As IList(Of Element) = _
        FindInstancesOfType(GetType(FamilyInstance), idDoorType, BuiltInCategory.OST_Doors)

        Dim msgDoors As String = "Instances of door with type: " + doorFamilyAndTypeName + vbCr
        ShowElementList(doors, msgDoors)

        ''  (3) apply the same idea to the supporting element, such as level. 
        ''  In this case, we simply check the name. 
        ''  This becomes handy when you are creating an object on a certain level,  
        ''  for example, when we create a wall. 
        ''  We will use this in the lab 5 when we create a simple house.  
        '' 
        Dim level1 As Level = FindElement(m_rvtDoc, GetType(Level), "Level 1")

        Dim msgLevel1 As String = "Level1: " + vbCr + ElementToString(level1) + vbCr
        TaskDialog.Show("Revit Intro Lab", msgLevel1)

        ''  (4) finally, let's see how to use parameter filter 
        ''  Let's try to get a wall whose length is larger than 60 feet. 

        Dim longWalls As IList(Of Element) = FindLongWalls()

        Dim msgLongWalls As String = "Long walls: " + vbCr
        ShowElementList(longWalls, msgLongWalls)

    End Sub

    ''  Helper function: find a list of element with given class, family type and category (optional). 
    ''
    Function FindInstancesOfType(ByVal targetType As Type, _
                                 ByVal idType As ElementId, _
                                 Optional ByVal targetCategory As BuiltInCategory = Nothing) _
                                 As IList(Of Element)

        ''  first, narrow down to the elements of the given type and category 
        '' 
        Dim collector = New FilteredElementCollector(m_rvtDoc).OfClass(targetType)
        If Not (targetCategory = Nothing) Then
            collector.OfCategory(targetCategory)
        End If

        ''  parse the collection for the given family type id.
        ''  using LINQ query here.
        Dim elems = _
            From element In collector _
            Where element.Parameter(BuiltInParameter.SYMBOL_ID_PARAM).AsElementId.Equals(idType) _
            Select element

        ''  put the result as a list of element fo accessibility. 
        Return elems.ToList()

    End Function

    ''
    ''  Optional - example of parameter filter. 
    ''  find walls whose length is longer than a certain length. e.g., 60 feet 
    ''  This could get more complex than looping through in terms of writing a code. 
    ''  See page 82 of Developer guide. 
    ''
    Function FindLongWalls() As IList(Of Element)

        ''  constant for this function. 
        Const kWallLength As Double = 60.0 '' 60 feet. hard coding for simplicity. 

        ''  first, narrow down to the elements of the given type and category 
        Dim collector = New FilteredElementCollector(m_rvtDoc).OfClass(GetType(Wall))

        ''  define a filter by parameter 
        ''  1st arg - value provider 
        Dim lengthParam As BuiltInParameter = BuiltInParameter.CURVE_ELEM_LENGTH
        Dim iLengthParam As Integer = lengthParam
        Dim paramValueProvider = New ParameterValueProvider(New ElementId(iLengthParam))
        ''  2nd - evaluator 
        Dim evaluator As New FilterNumericGreater
        ''  3rd - rule value 
        Dim ruleVal As Double = kWallLength
        ''  4th - epsilon 
        Const eps As Double = 0.000001
        ''  define a rule 
        Dim filterRule = New FilterDoubleRule(paramValueProvider, evaluator, ruleVal, eps)
        ''  create a new filter 
        Dim paramFilter = New ElementParameterFilter(filterRule)
        ''  go through the filter 
        Dim elems As IList(Of Element) = collector.WherePasses(paramFilter).ToElements
        Return elems

    End Function

#Region "Helper Functions"
    ''====================================================================
    ''  Helper Functions 
    ''====================================================================
    ''
    ''  Helper function: find an element of the given type, name, and category(optional) 
    ''  You can use this, for example, to find a specific wall and window family with the given name. 
    ''  e.g., 
    ''  FindFamilyType(m_rvtDoc, GetType(WallType), "Basic Wall", "Generic - 200mm") 
    ''  FindFamilyType(m_rvtDoc, GetType(FamilySymbol), "M_Single-Flush", "0915 x 2134mm",  BuiltInCategory.OST_Doors) 
    '' 
    '' 
    Public Shared Function FindFamilyType(ByVal rvtDoc As Document, _
            ByVal targetType As Type, ByVal targetFamilyName As String, _
            ByVal targetTypeName As String, _
            Optional ByVal targetCategory As BuiltInCategory = Nothing) _
            As Element

        ''  first, narrow down to the elements of the given type and category 
        Dim collector = New FilteredElementCollector(rvtDoc).OfClass(targetType)
        If Not (targetCategory = Nothing) Then
            collector.OfCategory(targetCategory)
        End If

        ''  parse the collection for the given names
        ''  using LINQ query here.
        Dim targetElems = _
            From element In collector _
            Where element.Name.Equals(targetTypeName) And _
            element.Parameter(BuiltInParameter.SYMBOL_FAMILY_NAME_PARAM). _
            AsString.Equals(targetFamilyName) _
            Select element

        ''  put the result as a list of element fo accessibility. 
        Dim elems As IList(Of Element) = targetElems.ToList()

        ''  return the result. 
        If elems.Count > 0 Then
            Return elems(0)
        End If

        Return Nothing

    End Function

    '' 
    ''  Helper function: find a list of element with given Class, Name and Category (optional). 
    ''
    Public Shared Function FindElements(ByVal rvtDoc As Document, _
                          ByVal targetType As Type, ByVal targetName As String, _
                          Optional ByVal targetCategory As BuiltInCategory = Nothing) As IList(Of Element)

        ''  first, narrow down to the elements of the given type and category 
        Dim collector = New FilteredElementCollector(rvtDoc).OfClass(targetType)
        If Not (targetCategory = Nothing) Then
            collector.OfCategory(targetCategory)
        End If

        ''  parse the collection for the given names
        ''  using LINQ query here.
        Dim elems = _
            From element In collector _
            Where element.Name.Equals(targetName) _
            Select element

        ''  put the result as a list of element for accessibility. 
        Return elems.ToList()

    End Function

    ''  ------------------------------------------------------------------
    ''  Helper function: searches elements with given Class, Name and Category (optional),  
    ''  and returns the first in the elements found. 
    ''  This gets handy when trying to find, for example, Level. 
    ''  e.g., FindElement(m_rvtDoc, GetType(Level), "Level 1")
    ''
    Public Shared Function FindElement(ByVal rvtDoc As Document, _
                         ByVal targetType As Type, ByVal targetName As String, _
                         Optional ByVal targetCategory As BuiltInCategory = Nothing) As Element

        ''  find a list of elements using the overloaded method. 
        Dim elems As IList(Of Element) = FindElements(rvtDoc, targetType, targetName, targetCategory)

        ''  return the first one from the result. 
        If elems.Count > 0 Then
            Return elems(0)
        End If

        Return Nothing

    End Function

    ''
    ''  Helper function: to show the result of finding a family type. 
    ''
    Function ShowFamilyTypeAndId(ByVal header As String, ByVal familyAndTypeName As String, _
                                 ByVal familyType As ElementType) As String

        ''  show the result.
        Dim msg As String = header + vbCr + familyAndTypeName + " >> Id = "

        If familyType IsNot Nothing Then
            msg = msg + familyType.Id.ToString + vbCr
        End If

        '' uncomment this if you want to show each result. 
        'TaskDialog.Show("Revit Intro Lab", msg)

        Return msg

    End Function
    ''
    ''  Helper function to display info from a list of elements passed onto. 
    '' 
    Sub ShowElementList(ByVal elems As IList(Of Element), ByVal header As String)

        Dim s As String = header + "(" + elems.Count.ToString + ")" + vbCr + vbCr
        s = s + " - Class - Category - Name (or Family: Type Name) - Id - " + vbCr
        For Each elem As Element In elems
            s = s + ElementToString(elem)
        Next
        TaskDialog.Show("Revit Intro Lab", s)

    End Sub

    ''  Helper Funtion: summarize an element information as a line of text, 
    ''  which is composed of: class, category, name and id. 
    ''  name will be "Family: Type" if a given element is ElementType. 
    ''  Intended for quick viewing of list of element, for example.  
    ''
    Function ElementToString(ByVal elem As Element) As String

        If elem Is Nothing Then
            Return "none"
        End If

        Dim name As String = ""

        If TypeOf elem Is ElementType Then
            Dim param As Parameter = elem.Parameter(BuiltInParameter.SYMBOL_FAMILY_AND_TYPE_NAMES_PARAM)
            If param IsNot Nothing Then
                name = param.AsString
            End If
        Else
            name = elem.Name
        End If

        Return elem.GetType.Name + "; " + elem.Category.Name + "; " _
        + name + "; " + elem.Id.IntegerValue.ToString + vbCr

    End Function

#End Region

End Class