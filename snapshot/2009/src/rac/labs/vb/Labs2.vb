#Region "Header"
' Revit API .NET Labs
'
' Copyright (C) 2006-2008 by Autodesk, Inc.
'
' Permission to use, copy, modify, and distribute this software
' for any purpose and without fee is hereby granted, provided
' that the above copyright notice appears in all copies and
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

#Region "Namespaces"
Imports System.Enum
Imports Autodesk.Revit
Imports Autodesk.Revit.Geometry
Imports Autodesk.Revit.Elements
Imports Autodesk.Revit.Structural.Enums
Imports Autodesk.Revit.Geometry.XYZ
Imports LabsVb
#End Region

Namespace Labs

#Region "Lab2_0_CreateLittleHouse"
    ''' <summary>
    ''' Create some sample elements.
    ''' We create a simple building consisting of four walls, 
    ''' a door, two windows, a floor, a roof, a room and a room tag.
    ''' </summary>
    Public Class Lab2_0_CreateLittleHouse
        Implements IExternalCommand


        Public Function Execute( _
            ByVal commandData As ExternalCommandData, _
            ByRef message As String, _
            ByVal elements As ElementSet) _
        As IExternalCommand.Result Implements IExternalCommand.Execute

            Try
                Dim waitCursor As New Labs.WaitCursor
                Dim app As Application = commandData.Application
                Dim doc As Document = app.ActiveDocument
                Dim creApp As Autodesk.Revit.Creation.Application = app.Create
                Dim creDoc As Autodesk.Revit.Creation.Document = doc.Create
                '
                ' determine the four corners of the rectangular house:
                '
                Dim width As Double = 7 * LabConstants.MeterToFeet
                Dim depth As Double = 4 * LabConstants.MeterToFeet

                Dim corners As List(Of Geometry.XYZ) = New List(Of Geometry.XYZ)(4)
                corners.Add(New Geometry.XYZ(0, 0, 0))
                corners.Add(New Geometry.XYZ(width, 0, 0))
                corners.Add(New Geometry.XYZ(width, depth, 0))
                corners.Add(New Geometry.XYZ(0, depth, 0))
                '
                ' determine the levels where the walls will be located:
                '
                Dim levelBottom As Level = Nothing
                Dim levelTop As Level = Nothing
                If Not LabUtils.GetBottomAndTopLevels(app, levelBottom, levelTop) Then
                    message = "Unable to determine wall bottom and top levels"
                    Return IExternalCommand.Result.Failed
                End If
                Debug.Print(String.Format("Drawing walls on '{0}' up to '{1}'", _
                  levelBottom.Name, levelTop.Name))
                '
                ' create the walls:
                '   
                Dim topLevelParam As BuiltInParameter = BuiltInParameter.WALL_HEIGHT_TYPE
                Dim topLevelId As ElementId = levelTop.Id
                Dim walls As List(Of Wall) = New List(Of Wall)(4)
                Dim i As Integer
                For i = 0 To 4 - 1 Step +1
                    Dim line As Line = creApp.NewLineBound(corners(i), corners(IIf(3 = i, 0, i + 1)))

                    Dim wall As Wall = creDoc.NewWall(line, levelBottom, False)
                    Dim param As Parameter = wall.Parameter(topLevelParam)
                    param.Set(topLevelId)
                    walls.Add(wall)
                Next
                '
                ' add door and windows to the first wall:
                '
                Dim doorSymbols As List(Of Element) = LabUtils.GetAllFamilySymbols(app, BuiltInCategory.OST_Doors)
                Dim windowSymbols As List(Of Element) = LabUtils.GetAllFamilySymbols(app, BuiltInCategory.OST_Windows)
                Debug.Assert(0 < doorSymbols.Count, "expected at least one door symbol to be loaded into project")
                Debug.Assert(0 < windowSymbols.Count, "expected at least one window symbol to be loaded into project")
                Dim door As FamilySymbol = CType(doorSymbols(0), FamilySymbol)
                Dim window As FamilySymbol = CType(windowSymbols(0), FamilySymbol)
                Dim midpoint As Geometry.XYZ = LabUtils.Midpoint(corners(0), corners(1))
                Dim p As Geometry.XYZ = LabUtils.Midpoint(corners(0), midpoint)
                Dim q As Geometry.XYZ = LabUtils.Midpoint(midpoint, corners(1))
                'double h = 1 * LabConstants.MeterToFeet;
                Dim h As Double = levelBottom.Elevation + 0.3 * (levelTop.Elevation - levelBottom.Elevation)
                p.Z = h
                q.Z = h
                Dim inst As FamilyInstance = creDoc.NewFamilyInstance( _
                  midpoint, door, walls(0), levelBottom, StructuralType.NonStructural)
                inst = creDoc.NewFamilyInstance(p, window, walls(0), levelBottom, StructuralType.NonStructural)
                inst = creDoc.NewFamilyInstance(q, window, walls(0), levelBottom, StructuralType.NonStructural)
                '
                ' determine wall thickness and grow the profile out by half the wall thickness, 
                ' so the floor and roof do not stop halfway through the wall:
                '
                Dim thickness As Double = walls(0).WallType.CompoundStructure.Layers.Item(0).Thickness
                Dim w As Double = 0.5 * thickness
                corners(0).X -= w
                corners(0).Y -= w
                corners(1).X += w
                corners(1).Y -= w
                corners(2).X += w
                corners(2).Y += w
                corners(3).X -= w
                corners(3).Y += w
                Dim profile As New CurveArray()
                For i = 0 To 4 - 1 Step +1
                    Dim line As Line = creApp.NewLineBound(corners(i), corners(IIf(3 = i, 0, i + 1)))
                    profile.Append(line)
                Next
                '
                ' add a floor, a roof, a room and a room tag:
                '
                Dim structural As Boolean = False
                Dim floor As Floor = creDoc.NewFloor(profile, structural)
                '
                ' add a roof. 
                '
                Dim roofTypes As List(Of Element) = LabUtils.GetAllTypes(app, GetType(RoofType), BuiltInCategory.OST_Roofs)
                Debug.Assert(0 < roofTypes.Count, "expected at least one roof type to be loaded into project")
                Dim roofType As RoofType = CType(roofTypes(0), RoofType)

                Dim footPrintToModelCurvesMapping As ElementIdSet = New ElementIdSet
                Dim roof As FootPrintRoof = creDoc.NewFootPrintRoof(profile, levelTop, roofType, footPrintToModelCurvesMapping)
                Dim slopeAngle As Double = 30 * LabConstants.DegreesToRadians

                For Each id As ElementId In footPrintToModelCurvesMapping
                    Dim id2 As ElementId = id
                    Dim line As ModelCurve = doc.Element(id2)
                    roof.DefinesSlope(line) = True
                    roof.SlopeAngle(line) = slopeAngle
                Next
                '
                ' create a room.
                '
                Dim room As Room = creDoc.NewRoom( _
                    levelBottom, New Autodesk.Revit.Geometry.UV(0.5 * width, 0.5 * depth))
                '
                ' add a room tag.
                '
                Dim roomTag As RoomTag = creDoc.NewRoomTag( _
                    room, New Autodesk.Revit.Geometry.UV(0.5 * width, 0.7 * depth), Nothing)
                'LabUtils.InfoMsg( "Little house was created successfully." );
                Return IExternalCommand.Result.Succeeded

            Catch ex As Exception
                message = ex.Message
                Return IExternalCommand.Result.Failed
            End Try
        End Function
    End Class

#End Region

#Region "Lab2_1_Elements"
    ''' <summary>
    ''' List all document elements.
    ''' </summary>
    Public Class Lab2_1_Elements
        Implements IExternalCommand

        Public Function Execute( _
            ByVal commandData As ExternalCommandData, _
            ByRef message As String, _
            ByVal elements As ElementSet) _
        As IExternalCommand.Result _
        Implements IExternalCommand.Execute

            ' Typical .NET error-checking (should be done everywhere, but will be omitted
            ' for clarity in some of the following labs unless we expect exceptions)
            Dim sw As StreamWriter
            Try
                sw = New StreamWriter(gsFilePathLab2_1)
            Catch e As Exception
                MsgBox("Cannot open " & gsFilePathLab2_1 & "; Exception=" & e.Message)
                Return IExternalCommand.Result.Failed
            End Try

            ' *ALL* elements are bundled together and accessible via Document's ElementIterator
            Dim sLine As String
            Dim elem As Revit.Element
            Dim iter As ElementIterator = commandData.Application.ActiveDocument.Elements
            Do While (iter.MoveNext)

                ' current Element
                elem = iter.Current

                ' Element Id
                sLine = "Id=" & elem.Id.Value.ToString

                ' Element class (System.Type)
                sLine += "; Class=" & elem.GetType.Name

                ' Element Category (not implemented for all classes!)
                Dim sCatName As String = ""
                If TypeOf elem Is Family Then
                    Dim f As Family = elem
                    If Not f.FamilyCategory Is Nothing Then
                        sCatName = f.FamilyCategory.Name
                    End If
                End If
                If 0 = sCatName.Length And Not elem.Category Is Nothing Then
                    sCatName = elem.Category.Name
                End If
                If 0 = sCatName.Length Then
                    sCatName = "?"
                End If
                sLine += "; Category=" & sCatName

                ' Element Name (different meaning for different classes, but mostly implemented "logically")
                ' Later, we'll see that more precise info on elements can be obtained in class-specific ways...
                Dim sName As String

                sName = elem.Name
                sLine += "; Name=" & sName

                ' write the Line
                sw.WriteLine(sLine)

            Loop

            sw.Close()
            MsgBox("Elements list has been created in " & gsFilePathLab2_1 & "!")
            Return IExternalCommand.Result.Succeeded

        End Function
    End Class

#End Region

#Region "Lab2_2_ModelElements"
    ''' <summary>
    ''' List all model elements.
    ''' </summary>
    Public Class Lab2_2_ModelElements
        Implements IExternalCommand

        Public Function Execute( _
            ByVal commandData As ExternalCommandData, _
            ByRef message As String, _
            ByVal elements As ElementSet) _
        As IExternalCommand.Result _
        Implements IExternalCommand.Execute

            ' Call the utility
            Dim modelElems As ElementSet = LabUtils.GetAllModelElements(commandData.Application)

            ' List all elems
            Dim sMsg As String = "There are " & modelElems.Size & " model elements:"
            Dim elem As Revit.Element
            For Each elem In modelElems
                sMsg += vbCrLf & "  Category=" & elem.Category.Name & "; Id=" & elem.Id.Value.ToString
            Next
            MsgBox(sMsg)

            Return IExternalCommand.Result.Succeeded

        End Function
    End Class

#End Region

#Region "Lab2_3_AllWallsAndFamilyInstances"
    ''' <summary>
    ''' List all walls and family instances.
    ''' </summary>
    Public Class Lab2_3_AllWallsAndFamilyInstances
        Implements IExternalCommand

        Public Function Execute( _
            ByVal commandData As ExternalCommandData, _
            ByRef message As String, _
            ByVal elements As ElementSet) _
        As IExternalCommand.Result Implements IExternalCommand.Execute

            Dim app As Revit.Application = commandData.Application

            ' get all Walls
            Dim walls As List(Of Element) = LabUtils.GetAllWalls(app)
            Dim wall As Wall
            Dim sMsg As String = "All Walls in the model:"
            For Each wall In walls
                sMsg += vbCrLf & "  Id=" & wall.Id.Value.ToString & "; Kind=" & wall.WallType.Kind.ToString & "; Type=" & wall.WallType.Name
            Next
            MsgBox(sMsg)

            ' get all Doors (you can change it to eg Furniture or Windows...)

            'NOTE: Before 9.0, one had to hard-code the name which wouldn't work in other locales!
            'Dim catName As String = "Doors"
            ' From 9.0, there is enumeration which should work with ALL locales
            Dim catName As String = app.ActiveDocument.Settings.Categories.Item(BuiltInCategory.OST_Doors).Name

            Dim familyInstances As List(Of Revit.Element) = LabUtils.GetAllStandardFamilyInstancesForACategory(app, BuiltInCategory.OST_Doors)
            Dim inst As FamilyInstance
            sMsg = "All " & catName & " instances in the model:"
            For Each inst In familyInstances
                ' For FamilyInstances, Element's Name property returns Type name. More on this later...
                sMsg += vbCrLf & "  Id=" & inst.Id.Value.ToString & "; Type=" & inst.Name
            Next
            MsgBox(sMsg)

            Return IExternalCommand.Result.Succeeded

        End Function
    End Class

#End Region

#Region "Lab2_4_EditFamilyInstance"
    ''' <summary>
    ''' Edit all doors in the current project.
    ''' Move the doors up 0.2 feet via Document.Move() method.
    ''' Widen the door 1 foot by changing the Parameter value.
    ''' </summary>
    Public Class Lab2_4_EditFamilyInstance
        Implements IExternalCommand

        Public Function Execute( _
                ByVal commandData As ExternalCommandData, _
                ByRef msg As String, _
                ByVal els As ElementSet) _
                As IExternalCommand.Result _
                Implements IExternalCommand.Execute

            Dim app As Application = commandData.Application
            Dim doc As Document = app.ActiveDocument
            Dim ss As ElementSet = doc.Selection.Elements

            'Find the destination door in the model by Filter
            Try
                Dim filterDoor As Filter = app.Create.Filter.NewCategoryFilter(BuiltInCategory.OST_Doors)
                Dim filterFamilyInstance As Filter = app.Create.Filter.NewTypeFilter(GetType(FamilyInstance))
                Dim filterAnd As Filter = app.Create.Filter.NewLogicAndFilter(filterDoor, filterFamilyInstance)
                Dim listDoors As List(Of Element) = New List(Of Element)()
                Dim bRet As Boolean = doc.Elements(filterAnd, listDoors)
                'Edit all doors in current document.
                Dim door As FamilyInstance = Nothing
                For Each elem As Element In listDoors
                    door = TryCast(elem, FamilyInstance)
                    If door IsNot Nothing Then
                        'Move up the door 0.2 feet.
                        Dim xyzVector As Autodesk.Revit.Geometry.XYZ
                        xyzVector = New Autodesk.Revit.Geometry.XYZ(0, 0, 0.2)
                        doc.Move(door, xyzVector)
                        'Widen the door by changing Parameter value.
                        Dim par As Parameter = door.Symbol.Parameter(BuiltInParameter.WINDOW_WIDTH)
                        If par IsNot Nothing Then
                            Dim width As Double = par.AsDouble()
                            width += 1.0
                            par.Set(width)
                        End If
                    End If
                Next elem
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try

            Return IExternalCommand.Result.Succeeded

        End Function
    End Class

#End Region

#Region "Lab2_5_Categories"
    ''' <summary>
    ''' List all built-in categories and the entire category tree.
    ''' Some of the results:
    ''' - not all built-in categories have a corresponding document category.
    ''' - not all document categories have a corresponding built-in category.
    ''' There are 645 built-in categories.
    ''' 419 of them have associated document categories.
    ''' 199 of these are top level parents.
    ''' These lead to 444 top-level and children categories.
    ''' </summary>
    Public Class Lab2_5_Categories
        Implements IExternalCommand

        Dim _doc As Document
        Const _indentDepth As Integer = 2
        Dim _bicForCategory As Hashtable

        ''' <summary>
        ''' Check whether the given category is one of the built-in ones.
        ''' This implementation is slow in 2008, and very very very slow in 2009,
        ''' so it has been replaced by the _bicForCategory hash table solution below.
        ''' </summary>
        ''' <param name="c"></param>
        ''' <returns>True if the given category is one of the built-in ones.</returns>
        Private Function IsBuiltInCategory1(ByVal c As Category) As Boolean

            Dim rc As Boolean = False
            Dim a As BuiltInCategory
            For Each a In System.Enum.GetValues(GetType(BuiltInCategory))
                Dim c2 As Category = _doc.Settings.Categories.Item(a)
                If c.Equals(c2) = True Then
                    rc = True
                    Exit For
                End If
            Next
            Return rc
        End Function

        ''' <summary>
        ''' Check whether the given category is one of the built-in ones.
        ''' </summary>
        ''' <param name="c"></param>
        ''' <returns>True if the given category is one of the built-in ones.</returns>
        Private Function IsBuiltInCategory(ByVal c As Category) As Boolean
            Return _bicForCategory.ContainsKey(c)
        End Function

        Private Function ListCategoryAndSubCategories(ByVal c As Category, ByVal level As Integer) As Integer

            Dim n As Integer = 1
            Dim indent As String = New String(" ", level * _indentDepth)
            Dim subCat As Category
            For Each subCat In c.SubCategories
                n += ListCategoryAndSubCategories(subCat, level + 1)
            Next
            Return n

        End Function

        Public Function Execute( _
            ByVal commandData As ExternalCommandData, _
            ByRef message As String, _
            ByVal elements As ElementSet) _
            As IExternalCommand.Result _
            Implements IExternalCommand.Execute

            Dim waitCursor As New Labs.WaitCursor
            Dim app As Application = commandData.Application
            _doc = app.ActiveDocument
            Dim categories As Categories = _doc.Settings.Categories
            Dim bics As Array = System.Enum.GetValues(GetType(BuiltInCategory))
            _bicForCategory = New Hashtable(bics.GetLength(0))
            '_categoryForBic = new Hashtable( bics.GetLength( 0 ) );
            Dim bic As BuiltInCategory
            Dim c As Category
            For Each bic In bics
                c = categories.Item(bic)
                If c Is Nothing Then
                    Debug.WriteLine(String.Format("Built-in category '{0}' has a null category.", bic.ToString()))
                Else
                    '_categoryForBic.Add( bic, c );
                    _bicForCategory.Add(c, bic)
                End If
            Next
            '
            ' list and count all the built-in categoreies:
            '
            Dim nBuiltInCategories As Integer = 0
            Dim nDocumentCategories As Integer = 0
            Dim indent As String = New String(" "c, _indentDepth)
            Dim topLevelCategories As CategorySet = app.Create.NewCategorySet()
            Debug.WriteLine("\nBuilt-in categories:")
            Dim a As BuiltInCategory
            For Each a In System.Enum.GetValues(GetType(BuiltInCategory))
                c = categories.Item(a)
                If Not (c Is Nothing) Then
                    nDocumentCategories = nDocumentCategories + 1
                    If c.Parent Is Nothing Then
                        topLevelCategories.Insert(c)
                    End If
                End If

                Dim tempStr As String = ""
                If Not c Is Nothing Then
                    tempStr = ": " + c.Name
                End If
                Debug.WriteLine(indent + a.ToString() + tempStr)
                nBuiltInCategories = nBuiltInCategories + 1
            Next
            Debug.WriteLine(String.Format("There are {0} built-in categories. {1} of them have associated document categories. {2} are top level parents.", nBuiltInCategories, nDocumentCategories, topLevelCategories.Size))
            '
            ' list the entire category hierarchy:
            '
            Dim nPrinted As Integer = 0
            Debug.WriteLine("\nDocument categories:")
            For Each c In topLevelCategories
                nPrinted += ListCategoryAndSubCategories(c, 1)
            Next
            Debug.WriteLine(String.Format("{0} top-level and children categories printed.", nPrinted))
            Return IExternalCommand.Result.Succeeded

        End Function
    End Class
#End Region

End Namespace

