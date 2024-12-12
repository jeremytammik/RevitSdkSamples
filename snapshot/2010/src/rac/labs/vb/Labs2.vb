#Region "Header"
' Revit API .NET Labs
'
' Copyright (C) 2006-2009 by Autodesk, Inc.
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
Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.IO
Imports Autodesk.Revit
Imports Autodesk.Revit.Geometry
Imports Autodesk.Revit.Elements
'Imports Autodesk.Revit.Enums
Imports Autodesk.Revit.Parameters
Imports Autodesk.Revit.Structural.Enums
Imports Microsoft.VisualBasic
'Imports XYZ = Autodesk.Revit.Geometry.XYZ
Imports XYZ2 = Autodesk.Revit.Geometry.XYZ
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

                Dim corners As List(Of XYZ2) = New List(Of XYZ2)(4)
                corners.Add(New XYZ2(0, 0, 0))
                corners.Add(New XYZ2(width, 0, 0))
                corners.Add(New XYZ2(width, depth, 0))
                corners.Add(New XYZ2(0, depth, 0))
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
                Dim midpoint As XYZ2 = LabUtils.Midpoint(corners(0), corners(1))
                Dim p As XYZ2 = LabUtils.Midpoint(corners(0), midpoint)
                Dim q As XYZ2 = LabUtils.Midpoint(midpoint, corners(1))
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

            Dim app As Application = commandData.Application
            Dim doc As Document = app.ActiveDocument

            ' Typical .NET error-checking (should be done everywhere, but will be omitted
            ' for clarity in some of the following labs unless we expect exceptions)
            Dim sw As StreamWriter
            Try
                sw = New StreamWriter(FilePath)
            Catch ex As Exception
                MsgBox("Cannot open " & FilePath & "; Exception=" & ex.Message)
                Return IExternalCommand.Result.Failed
            End Try

            ' *ALL* elements are bundled together and accessible via Document's ElementIterator
            Dim sLine As String
            Dim e As Element
            Dim it As ElementIterator = doc.Elements
            'Dim it As IEnumerator = doc.Elements ' this would also be sufficient

            Do While (it.MoveNext)

                ' current Element
                e = it.Current

                ' Element Id
                sLine = "Id=" & e.Id.Value.ToString

                ' Element class (System.Type)
                sLine += "; Class=" & e.GetType.Name

                ' Element Category (not implemented for all classes!)
                Dim sCatName As String = ""
                If TypeOf e Is Family Then
                    Dim f As Family = e
                    If Not f.FamilyCategory Is Nothing Then
                        sCatName = f.FamilyCategory.Name
                    End If
                End If
                If 0 = sCatName.Length And Not e.Category Is Nothing Then
                    sCatName = e.Category.Name
                End If
                If 0 = sCatName.Length Then
                    sCatName = "?"
                End If
                sLine += "; Category=" & sCatName

                ' Element Name (different meaning for different classes, but mostly implemented "logically")
                ' Later, we'll see that more precise info on elements can be obtained in class-specific ways...
                Dim sName As String

                sName = e.Name
                sLine += "; Name=" & sName

                ' write the Line
                sw.WriteLine(sLine)

            Loop

            sw.Close()
            MsgBox("Elements list has been created in " & FilePath & "!")
            Return IExternalCommand.Result.Failed

        End Function
    End Class

#End Region

#Region "Lab2_2_ModelElements"
    ''' <summary>
    ''' List all model elements.
    ''' </summary>
    Public Class Lab2_2_ModelElements
        Implements IExternalCommand

        Dim _bicPreviewLegendComponent As BuiltInCategory = BuiltInCategory.OST_PreviewLegendComponents

        Public Function Execute( _
            ByVal commandData As ExternalCommandData, _
            ByRef message As String, _
            ByVal elements As ElementSet) _
        As IExternalCommand.Result _
        Implements IExternalCommand.Execute

            Dim app As Application = commandData.Application
            Dim doc As Document = app.ActiveDocument

            Dim it As ElementIterator = doc.Elements
            Dim s As String = String.Empty
            Dim count As Integer = 0

            Dim opt As Geometry.Options = app.Create.NewGeometryOptions
            'opt.DetailLevel = Geometry.Options.DetailLevels.Fine

            Dim iBic As Integer = _bicPreviewLegendComponent

            Do While (it.MoveNext())
                Dim e As Element = it.Current

                ' This single line would probably work if all system 
                ' families were exposed as HostObjects, but they are not yet:
                'If TypeOf e Is FamilyInstance Or TypeOf e Is HostObject Then

                ' up until evit 2009 we used the following:
                '
                'If Not (TypeOf e Is Symbol) _
                '    And Not (TypeOf e Is FamilyBase) _
                '    And Not (e.Category Is Nothing) _
                '    Then
                '
                ' but in 2010, this also admits legend component elements,
                ' which previously did not have any geometry.
                ' to eliminate these, we can either use 
                '
                ' !(e is Symbol)
                ' && !(e is FamilyBase)
                ' && (null != e.Category)
                ' && (_CategoryNameLegendComponents != e.Category.Name)
                ' && (null != e.get_Geometry(opt))
                '
                ' or simply check that the level is not null
                ' and altogether skip the checks on the object type:
                ' simply check that the level is not null and
                ' altogether skip the checks on the object type;
                ' nope, this does not work, it unfortunately misses 
                ' certain model elements which do not have a level 
                ' asssigned:
                '
                ' (null != e.Category)
                ' && (null != e.Level)
                ' && (null != e.get_Geometry(opt))
                '
                If Not (TypeOf e Is Symbol) _
                    And Not (TypeOf e Is FamilyBase) _
                    And Not (e.Category Is Nothing) _
                    And Not (iBic = e.Category.Id.Value) Then

                    Dim geo As Geometry.Element = e.Geometry(opt)
                    If Not (geo Is Nothing) Then
                        count = count + 1
                        s += vbCrLf & "  Category=" & e.Category.Name & "; Id=" & e.Id.Value.ToString
                    End If
                End If

            Loop

            s = "There are " & count & " model elements:" & s

            MsgBox(s)

            Return IExternalCommand.Result.Failed

        End Function
    End Class

#End Region

#Region "Lab2_3_AllWallsAndDoorFamilyInstances"
    ''' <summary>
    ''' List all walls and family instances.
    ''' </summary>
    Public Class Lab2_3_AllWallsAndDoorFamilyInstances
        Implements IExternalCommand

        Public Function Execute( _
            ByVal commandData As ExternalCommandData, _
            ByRef message As String, _
            ByVal elements As ElementSet) _
        As IExternalCommand.Result Implements IExternalCommand.Execute

            Dim app As Application = commandData.Application
            Dim doc As Document = app.ActiveDocument

            ' get all Walls

            Dim walls As New List(Of Element)
            Dim f As Filter = app.Create.Filter.NewTypeFilter(GetType(Wall))
            Dim n As Integer = doc.Elements(f, walls)

            Dim wall As Wall
            Dim s As String = "All Walls in the model:"
            For Each wall In walls
                s += vbCrLf & "  Id=" & wall.Id.Value.ToString _
                    & "; Kind=" & wall.WallType.Kind.ToString _
                    & "; Type=" & wall.WallType.Name
            Next
            MsgBox(s)

            ' get all Doors (you can change it to e.g. Furniture or Windows...)

            Dim bic As BuiltInCategory = BuiltInCategory.OST_Doors
            Dim catName As String = doc.Settings.Categories.Item(bic).Name

            Dim doors As New System.Collections.Generic.List(Of Element)
            Dim filterType As Filter = app.Create.Filter.NewTypeFilter(GetType(FamilyInstance))
            Dim filterCategory As Filter = app.Create.Filter.NewCategoryFilter(bic)
            f = app.Create.Filter.NewLogicAndFilter(filterCategory, filterType)
            Dim nRetVal As Integer = app.ActiveDocument.Elements(f, doors)

            Dim door As FamilyInstance
            s = "All " & catName & " instances in the model:"
            For Each door In doors
                ' For FamilyInstances, element's Name property returns Type name:
                s += vbCrLf & "  Id=" & door.Id.Value.ToString & "; Type=" & door.Name
            Next
            MsgBox(s)

            Return IExternalCommand.Result.Failed

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
                Dim filterCategory As Filter = app.Create.Filter.NewCategoryFilter(BuiltInCategory.OST_Doors)
                Dim filterType As Filter = app.Create.Filter.NewTypeFilter(GetType(FamilyInstance))
                Dim f As Filter = app.Create.Filter.NewLogicAndFilter(filterCategory, filterType)
                Dim doors As List(Of Element) = New List(Of Element)()
                Dim n As Integer = doc.Elements(f, doors)

                'Move the doors up 0.2 feet:
                Dim xyzVector As XYZ2
                xyzVector = New XYZ2(0, 0, 0.2)

                'Edit all doors in current document.
                Dim door As FamilyInstance = Nothing
                For Each door In doors
                    'door = TryCast(e, FamilyInstance)
                    'If door IsNot Nothing Then

                    doc.Move(door, xyzVector)

                    ' Widen the doors by one foot by changing parameter value:

                    Dim par As Parameter = door.Symbol.Parameter(BuiltInParameter.WINDOW_WIDTH)
                    If par IsNot Nothing Then
                        Dim width As Double = par.AsDouble()
                        width += 1.0
                        par.Set(width)
                    End If
                Next door
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

