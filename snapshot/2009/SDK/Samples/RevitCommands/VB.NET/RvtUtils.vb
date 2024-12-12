'
' (C) Copyright 2003-2008 by Autodesk, Inc.
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
' MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE. AUTODESK, INC.
' DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
' UNINTERRUPTED OR ERROR FREE.
'
' Use, duplication, or disclosure by the U.S. Government is subject to
' restrictions set forth in FAR 52.227-19 (Commercial Computer
' Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
' (Rights in Technical Data and Computer Software), as applicable.
'


Imports System

Imports Autodesk
Imports Autodesk.Revit
Imports Autodesk.Revit.Geometry

'
'  Utilities 
'
Public Class RvtUtils

    '==================================================================
    '
    '  Show Element Information 
    '
    '==================================================================
    '  List all the parameters of the given element. 
    ' 
    Public Shared Sub ListParameters(ByVal elem As Autodesk.Revit.Element)

        ListInstanceParameters(elem)
        ListTypeParameters(elem)

    End Sub

    '------------------------------------------------------------------
    '  List Instance Parameters
    '
    Public Shared Sub ListInstanceParameters(ByVal elem As Autodesk.Revit.Element)

        '  header 
        Dim str As String = "  --  Instance Paramaters  --  " & vbCr & vbCr

        '  get params
        Dim params As ParameterSet = elem.Parameters
        str = str + ParameterSetToString(elem.Document, params)

        '  show it
        MsgBox(str)

    End Sub

    '------------------------------------------------------------------
    '  List Type Parameters
    '
    Public Shared Sub ListTypeParameters(ByVal elem As Autodesk.Revit.Element)

        '  header
        Dim str As String = "  --  Type Paramaters  --  " & vbCr

        ' get the type. 
        Dim type As Autodesk.Revit.Symbol = elem.ObjectType

        If Not (type Is Nothing) Then
            '  get family and type name.  Note: this is not the part of parameter set below. 
            Dim paramFamilyTypeName As Autodesk.Revit.Parameter = type.Parameter( _
            Parameters.BuiltInParameter.SYMBOL_FAMILY_AND_TYPE_NAMES_PARAM)
            str = str + "Family Type Name: " + ParameterToString(elem.Document, _
            paramFamilyTypeName) + vbCr + vbCr

            '  get parameter set. 
            Dim params As ParameterSet = type.Parameters
            str = str & ParameterSetToString(elem.Document, params)
        Else
            str = str & "none"
        End If

        '  show it. 
        MsgBox(str)

    End Sub

    '------------------------------------------------------------------
    '  Return a string form of a given parameter set  
    '
    Public Shared Function ParameterSetToString(ByVal rvtDoc As Autodesk.Revit.Document, _
    ByVal params As ParameterSet) As String

        Dim str As String = ""

        '  does the element have parameters? if not, simply return. 
        If (rvtDoc Is Nothing) Or (params Is Nothing) Or params.IsEmpty Then
            str = "none" & vbCr
            Return str
        End If

        '  we have some parameters. show it. 
        Dim param As Autodesk.Revit.Parameter
        For Each param In params
            Dim name As String = param.Definition.Name
            str = str & name & ": " & ParameterToString(rvtDoc, param) & vbCr
        Next

        Return str

    End Function

    '------------------------------------------------------------------
    '  Return a string form of a given parameter 
    '
    Public Shared Function ParameterToString(ByVal rvtDoc As Autodesk.Revit.Document, _
    ByVal param As Autodesk.Revit.Parameter) As String

        Dim val As String = "none"

        If (rvtDoc Is Nothing) Or (param Is Nothing) Then
            Return val
        End If

        Dim type As Autodesk.Revit.Parameters.StorageType = param.StorageType

        ' take the internal type of the parameter and convert it into a string
        Select Case type

            Case Parameters.StorageType.Double
                val = param.AsDouble

            Case Parameters.StorageType.ElementId
                ' get the element and use its name.
                Dim paraElem As Autodesk.Revit.Element = rvtDoc.Element(param.AsElementId)
                If Not (paraElem Is Nothing) Then
                    val = paraElem.Name
                End If

            Case Parameters.StorageType.Integer
                val = param.AsInteger

            Case Parameters.StorageType.String
                val = param.AsString
            Case Parameters.StorageType.None
                val = param.AsValueString
            Case Else

        End Select

        Return val

    End Function


    '----------------------------------------------------------------------------
    '  find a parameter of a given parameter definition name. (workaround when elem.paramater()
    ' does find one.) 
    '
    Public Shared Function FindParameter(ByVal elem As Autodesk.Revit.Element, _
    ByVal targetName As String) As Autodesk.Revit.Parameter

        Dim params As ParameterSet = elem.Parameters

        '  does the element have parameters? if not, simply return. 
        If params.IsEmpty Then
            Return Nothing
        End If

        '  we have some parameters. show it. 
        Dim param As Autodesk.Revit.Parameter
        For Each param In params
            Dim name As String = param.Definition.Name
            If name = targetName Then
                ' we found it. 
                Return param
            End If
        Next

        Return Nothing

    End Function

    '============================================================================
    ' 
    '  List geometry information of the given element. 
    ' 
    '============================================================================
    Public Shared Sub ListGeometry(ByVal rvtApp As Autodesk.Revit.Application, _
    ByVal elem As Autodesk.Revit.Element)

        '  header.
        Dim str As String = "  --  Geometry  --  " & vbCr

        '  set the geometry option.
        Dim opt As Autodesk.Revit.Geometry.Options
        opt = rvtApp.Create.NewGeometryOptions
        opt.DetailLevel = Autodesk.Revit.Geometry.Options.DetailLevels.Fine

        '  does the element have geometry data? 
        Dim geomElem As Autodesk.Revit.Geometry.Element = elem.Geometry(opt)
        If geomElem Is Nothing Then
            MsgBox(str & "no data")
            Return
        End If

        str = GeometryElementToString(geomElem)

        MsgBox(str)

    End Sub

    '------------------------------------------------------------------
    Public Shared Function GeometryElementToString( _
    ByVal geomElem As Autodesk.Revit.Geometry.Element) As String

        Dim geomObjs As Autodesk.Revit.Geometry.GeometryObjectArray = geomElem.Objects
        Dim geomObj As GeometryObject

        Dim str As String = ""
        str = str & "Total number of GeometryObject: " & geomObjs.Size.ToString & vbCr

        For Each geomObj In geomObjs

            If TypeOf geomObj Is Autodesk.Revit.Geometry.Solid Then  '  ex. wall

                Dim solid As Autodesk.Revit.Geometry.Solid = geomObj
                str = str & GeometrySolidToString(solid)

            ElseIf TypeOf geomObj Is Autodesk.Revit.Geometry.Instance Then ' ex. door/window

                str = str & "  -- Geometry.Instance -- " & vbCr
                Dim geomInstance As Autodesk.Revit.Geometry.Instance = geomObj
                Dim geoElem As Autodesk.Revit.Geometry.Element = geomInstance.SymbolGeometry()

                str = str & GeometryElementToString(geoElem)

            ElseIf TypeOf geomObj Is Autodesk.Revit.Geometry.Curve Then ' ex. 

                Dim curv As Autodesk.Revit.Geometry.Curve = geomObj
                str = str & GeometryCurveToString(curv)

            ElseIf TypeOf geomObj Is Autodesk.Revit.Geometry.Mesh Then ' ex. 

                Dim mesh As Autodesk.Revit.Geometry.Mesh = geomObj
                str = str & GeometryMeshToString(mesh)

            Else
                str = str & "  *** unkown geometry type" & geomObj.GetType.ToString

            End If

        Next

        Return str

    End Function

    '------------------------------------------------------------------
    Public Shared Function GeometrySolidToString(ByVal solid _
    As Autodesk.Revit.Geometry.Solid) As String

        Dim str As String = "  -- Geometry.Solid -- " + vbCr
        Dim faces As Autodesk.Revit.Geometry.FaceArray = solid.Faces

        str = str + "Total number of faces: " + faces.Size.ToString & vbCr

        Dim face As Autodesk.Revit.Geometry.Face
        Dim iface As Integer = 0

        For Each face In faces
            iface = iface + 1

            str = str + "Face " & iface & "/" & faces.Size.ToString & vbCr

            Dim edgeLoops As Autodesk.Revit.Geometry.EdgeArrayArray = face.EdgeLoops
            Dim iLoop As Integer = 0

            Dim edgeLoop As Autodesk.Revit.Geometry.EdgeArray
            For Each edgeLoop In edgeLoops
                iLoop = iLoop + 1
                str = str & "    EdgeLoop[" & iLoop.ToString & "] "
                Dim edge As Autodesk.Revit.Geometry.Edge

                For Each edge In edgeLoop
                    Dim pts As Autodesk.Revit.Geometry.XYZArray = edge.Tessellate
                    str = str + PointArrayToString(pts)

                Next
                str = str & vbCr
            Next

        Next

        Return str

    End Function

    '------------------------------------------------------------------
    Public Shared Function GeometryCurveToString(ByVal curv As Autodesk.Revit.Geometry.Curve) _
    As String

        Dim str As String = "  -- Geometry.Curve -- " + vbCr
        Dim pts As Autodesk.Revit.Geometry.XYZArray = curv.Tessellate
        str = str + PointArrayToString(pts)

        Return str

    End Function

    '------------------------------------------------------------------
    Public Shared Function GeometryMeshToString(ByVal mesh As Autodesk.Revit.Geometry.Mesh) _
    As String

        Dim str As String = "  -- Geometry.Mesh -- " + vbCr
        Dim pts As Autodesk.Revit.Geometry.XYZArray = mesh.Vertices
        str = str + PointArrayToString(pts)

        Return str

    End Function

    '------------------------------------------------------------------
    Public Shared Function PointArrayToString(ByVal pts As Autodesk.Revit.Geometry.XYZArray) _
    As String

        Dim str As String = ""
        Dim pt As Autodesk.Revit.Geometry.XYZ
        For Each pt In pts
            str = str + PointToString(pt)
        Next
        Return str

    End Function

    '------------------------------------------------------------------
    Public Shared Function PointToString(ByVal pt As Autodesk.Revit.Geometry.XYZ) As String

        Dim str As String = "(" & pt.X.ToString("F3") & ", " & pt.Y.ToString("F3") & ", " _
        & pt.Z.ToString("F3") & ") "
        Return str

    End Function

    '============================================================================
    ' 
    '  List location of the given element. 
    ' 
    '============================================================================

    Public Shared Sub ListLocation(ByVal elem As Autodesk.Revit.Element)

        ' header
        Dim str As String = "  --  Location  -- " & vbCr

        ' do we have a location data? 
        Dim loc As Location = elem.Location
        If (loc Is Nothing) Then
            MsgBox(str & "no data")
            Return
        End If

        '  we have location data.  show it.

        ' location type is LocationCurve 
        If TypeOf loc Is LocationCurve Then
            Dim crv As LocationCurve = loc
            Dim startPt As Autodesk.Revit.Geometry.XYZ = crv.Curve.EndPoint(0)
            Dim endPt As Autodesk.Revit.Geometry.XYZ = crv.Curve.EndPoint(1)
            str = str & "Start Point: " & startPt.X & ", " & startPt.Y & ", " & startPt.Z & vbCr
            str = str & "End Point: " & endPt.X & ", " & endPt.Y & ", " & endPt.Z

            ' location type is LocationPoint 
        ElseIf TypeOf loc Is LocationPoint Then
            Dim locPt As LocationPoint = loc
            Dim pt As Autodesk.Revit.Geometry.XYZ = locPt.Point
            str = str & "Point: " & pt.X & ", " & pt.Y & ", " & pt.Z & vbCr

            ' Others 
        Else
            str = str & "this is not LocationCurve nor LocationPoint"
        End If

        '  show it.
        MsgBox(str)

    End Sub


    '==========================================================================================
    '
    '  Show a list of elements
    ' 
    '==========================================================================================

    Public Shared Sub ListElements(ByVal elemSet As Autodesk.Revit.ElementSet)

        Dim sHeader As String = "  --  Elements (" & elemSet.Size().ToString & ")" & vbCr

        Dim i As Integer = 0
        Dim str As String = ""
        Dim elem As Autodesk.Revit.Element

        For Each elem In elemSet
            str = str & elem.GetType.ToString & " " & elem.Name & vbCr
            i = i + 1
            If i = 100 Then
                MsgBox(sHeader + str)
                i = 0
                str = ""
            End If
        Next

        If i > 0 Then
            MsgBox(str)
        End If

    End Sub

    '-------------------------------------------------------------------------
    '  Helper Function

    Public Shared Function ElementSummaryStr(ByVal elem As Autodesk.Revit.Element) As String

        Dim str As String = ""
        Dim param As Autodesk.Revit.Parameter

        ' if elem is symbol/type, then show family name and symbol/type name. 
        If TypeOf elem Is Autodesk.Revit.Symbol Then

            ' get family name 
            param = elem.Parameter(Parameters.BuiltInParameter.ALL_MODEL_FAMILY_NAME)
            Dim familyName As String = ParameterToString(elem.Document, param)

            ' get symbol name.
            param = elem.Parameter(Parameters.BuiltInParameter.SYMBOL_NAME_PARAM)
            Dim symbolName As String = ParameterToString(elem.Document, param)

            '  want to show it.
            str = familyName + ": " + symbolName

        Else
            ' elem is non symbol or instance. check the view, for example, next. 
            If Not (elem.Category Is Nothing) Then
                If elem.Category.Name = "Viewports" Then

                    ' view name 
                    param = elem.Parameter(Parameters.BuiltInParameter.VIEWPORT_VIEW_NAME)
                    Dim viewName As String = ParameterToString(elem.Document, param)

                    ' view type 
                    param = elem.Parameter(Parameters.BuiltInParameter.VIEW_TYPE)
                    Dim viewName2 As String = ParameterToString(elem.Document, param)

                    ' show it. 
                    str = str + viewName2 + ": " + viewName
                End If

            End If

        End If

        Return str

    End Function

    '-------------------------------------------------------------------------
    '  ListElementMap
    '
    Public Shared Sub ListElementMap(ByVal elemMap As Autodesk.Revit.Collections.Map)

        Dim sHeader As String = "  --  Element Map (" & elemMap.Size().ToString & ") -- " _
        & vbCrLf & vbCrLf

        'Dim i As Integer = 0
        Dim str As String = ""

        Dim iter As Autodesk.Revit.Collections.MapIterator = elemMap.ForwardIterator

        Do While (iter.MoveNext())

            '  the Revit map iterator provides access to the key as well as the values
            Dim key As String = iter.Key
            Dim elemSet As Autodesk.Revit.ElementSet = iter.Current
            str = str + key + " (" + elemSet.Size.ToString + ") " + " - "

            '  look at each element set 
            Dim i As Integer = 0
            Dim elem As Autodesk.Revit.Element
            For Each elem In elemSet
                If i = 0 Then
                    str = str + elem.GetType.ToString + vbCrLf + "        "
                End If
                i = i + 1
                str = str + ElementSummaryStr(elem) + ", "

            Next
            str = str + vbCrLf

        Loop

        ' finally show it.
        MsgBox(sHeader + str)

    End Sub


    '-------------------------------------------------------------------------
    '  List categories in the project 
    '
    Public Shared Sub ListCategories(ByVal rvtApp As Autodesk.Revit.Application)

        Dim str As String = "  -- Categories -- " + vbCrLf

        Dim cats As Autodesk.Revit.Categories = rvtApp.ActiveDocument.Settings.Categories()
        Dim cat As Autodesk.Revit.Category
        For Each cat In cats
            str = str + cat.Name + ", "
        Next
        MsgBox(str)

    End Sub

    '==========================================================================================
    '
    '  Element Collection 
    '
    '==========================================================================================
    '  makes a collection of instances in the active document.  
    '
    Public Shared Function CollectInstances(ByVal rvtApp As Autodesk.Revit.Application) _
    As Autodesk.Revit.ElementSet

        '  a container of elements we want to collect. 
        Dim resultSet As Autodesk.Revit.ElementSet = rvtApp.Create.NewElementSet()

        '  get a set of elements in the current document. 
        Dim itr As Autodesk.Revit.ElementIterator = rvtApp.ActiveDocument.Elements

        '  geometry option. 
        Dim opt As Autodesk.Revit.Geometry.Options = rvtApp.Create.NewGeometryOptions
        opt.DetailLevel = Autodesk.Revit.Geometry.Options.DetailLevels.Medium

        '  go over the elements and show all the geometry elements. 
        Dim elem As Autodesk.Revit.Element
        Do While (itr.MoveNext)
            elem = itr.Current

            If Not (TypeOf elem Is Autodesk.Revit.Symbol) Then
                resultSet.Insert(elem)
            End If

        Loop

        ' finishing up.
        Return resultSet

    End Function

    '------------------------------------------------------------------------------------------
    '  makes a collection of symbols/types in the active document.  
    '
    Public Shared Function CollectSymbols(ByVal rvtApp As Autodesk.Revit.Application) _
    As Autodesk.Revit.ElementSet

        '  a container of elements we want to collect. 
        Dim resultSet As Autodesk.Revit.ElementSet = rvtApp.Create.NewElementSet()

        '  get a set of elements in the current document. 
        Dim itr As Autodesk.Revit.ElementIterator = rvtApp.ActiveDocument.Elements

        '  geometry option. 
        Dim opt As Autodesk.Revit.Geometry.Options = rvtApp.Create.NewGeometryOptions
        opt.DetailLevel = Autodesk.Revit.Geometry.Options.DetailLevels.Medium

        '  go over the elements and show all the geometry elements. 
        Dim elem As Autodesk.Revit.Element
        Do While (itr.MoveNext)
            elem = itr.Current

            If (TypeOf elem Is Autodesk.Revit.Symbol) Then
                resultSet.Insert(elem)
            End If

        Loop

        ' finishing up.
        Return resultSet

    End Function

    '------------------------------------------------------------------------------------------
    '  makes a collection of symbols/types in the active document.  
    '
    Public Shared Function CollectElements(ByVal rvtApp As Autodesk.Revit.Application, _
    ByVal flag As Boolean) As Autodesk.Revit.ElementSet

        '  a container of elements we want to collect. 
        Dim resultSet As Autodesk.Revit.ElementSet = rvtApp.Create.NewElementSet()

        '  get a set of elements in the current document. 
        Dim itr As Autodesk.Revit.ElementIterator = rvtApp.ActiveDocument.Elements

        '  geometry option. 
        Dim opt As Autodesk.Revit.Geometry.Options = rvtApp.Create.NewGeometryOptions
        opt.DetailLevel = Autodesk.Revit.Geometry.Options.DetailLevels.Medium

        '  go over the elements and show all the geometry elements. 
        Dim elem As Autodesk.Revit.Element
        Do While (itr.MoveNext)
            elem = itr.Current

            If (TypeOf elem Is Autodesk.Revit.Symbol) Then
                resultSet.Insert(elem)
            End If

        Loop

        ' finishing up.
        Return resultSet

    End Function


    '------------------------------------------------------------------------------------------
    '  get the collection of elements with geometry information. (e.g., wall, door, window.) 
    '
    Public Shared Function CollectElementsWithGeometry(ByVal rvtApp As Autodesk.Revit.Application) _
    As Autodesk.Revit.ElementSet

        '  a container of elements we want to collect. 
        Dim resultSet As Autodesk.Revit.ElementSet = rvtApp.Create.NewElementSet()

        '  get a set of elements in the current document. 
        Dim itr As Autodesk.Revit.ElementIterator = rvtApp.ActiveDocument.Elements

        '  geometry option. 
        Dim opt As Autodesk.Revit.Geometry.Options = rvtApp.Create.NewGeometryOptions
        opt.DetailLevel = Autodesk.Revit.Geometry.Options.DetailLevels.Medium

        '  go over the elements and collect only the geometry elements. 
        Dim elem As Autodesk.Revit.Element
        Do While (itr.MoveNext)
            elem = itr.Current

            If Not (TypeOf elem Is Autodesk.Revit.Symbol) Then
                If Not (elem.Category Is Nothing) Then
                    Dim geo As Autodesk.Revit.Geometry.Element = elem.Geometry(opt)
                    If Not (geo Is Nothing) Then
                        resultSet.Insert(elem)
                    End If
                End If
            End If

        Loop

        ' finishing up.
        Return resultSet

    End Function


    '------------------------------------------------------------------------------------------
    '  get the collection of elements without geometry information. 
    '
    Public Shared Function CollectElementsWithNoGeometry( _
    ByVal rvtApp As Autodesk.Revit.Application) As Autodesk.Revit.ElementSet

        '  a container of elements we want to collect. 
        Dim resultSet As Autodesk.Revit.ElementSet = rvtApp.Create.NewElementSet()

        '  get a set of elements in the current document. 
        Dim itr As Autodesk.Revit.ElementIterator = rvtApp.ActiveDocument.Elements

        '  geometry option. 
        Dim opt As Autodesk.Revit.Geometry.Options = rvtApp.Create.NewGeometryOptions
        opt.DetailLevel = Autodesk.Revit.Geometry.Options.DetailLevels.Medium

        '  go over the elements and show all the geometry elements. 
        Dim elem As Autodesk.Revit.Element
        Do While (itr.MoveNext)
            elem = itr.Current
            If Not (TypeOf elem Is Autodesk.Revit.Symbol) Then
                If Not (elem.Category Is Nothing) Then
                    Dim geo As Autodesk.Revit.Geometry.Element = elem.Geometry(opt)
                    If (geo Is Nothing) Then
                        resultSet.Insert(elem)
                    End If
                End If
            End If
        Loop

        ' finishing up.
        Return resultSet

    End Function

    '------------------------------------------------------------------------------------------
    '  get the collection of viewport. 
    '
    Public Shared Function CollectViewports(ByVal rvtApp As Autodesk.Revit.Application) _
    As Autodesk.Revit.ElementSet

        '  a container of elements we want to collect. 
        Dim resultSet As Autodesk.Revit.ElementSet = rvtApp.Create.NewElementSet()

        '  get a set of elements in the current document. 
        Dim itr As Autodesk.Revit.ElementIterator = rvtApp.ActiveDocument.Elements

        '  go over the elements and collect Viewports. 
        Dim elem As Autodesk.Revit.Element
        Do While (itr.MoveNext)
            elem = itr.Current

            If Not (elem.Category Is Nothing) Then

                If elem.Category.Name = "Viewports" Then
                    resultSet.Insert(elem)
                End If
            End If

        Loop

        ' finishing up.
        Return resultSet

    End Function

    '=========================================================================================
    '  checks the type of element.
    Public Shared Function isSymbol(ByVal elem As Autodesk.Revit.Element) As Boolean

        If (TypeOf elem Is Autodesk.Revit.Symbol) Then
            Return True
        End If
        Return False

    End Function

    '------------------------------------------------------------------------
    ' checks if the elem is a family. 
    Public Shared Function isFamily(ByVal elem As Autodesk.Revit.Element) As Boolean

        If (TypeOf elem Is Autodesk.Revit.Elements.Family) Then
            Return True
        End If
        Return False

    End Function

    '------------------------------------------------------------------------
    '  checks if the element's category is defined. 
    Public Shared Function hasCategory(ByVal elem As Autodesk.Revit.Element) As Boolean

        If elem.Category Is Nothing Then
            Return False
        End If
        Return True

    End Function

    '------------------------------------------------------------------------
    '  checks if the element has geometry information. 
    Public Shared Function hasGeometry(ByVal rvtApp As Autodesk.Revit.Application, ByVal elem As _
    Autodesk.Revit.Element) As Boolean

        '  geometry option. 
        Dim opt As Autodesk.Revit.Geometry.Options = rvtApp.Create.NewGeometryOptions
        opt.DetailLevel = Autodesk.Revit.Geometry.Options.DetailLevels.Medium

        If elem.Geometry(opt) Is Nothing Then
            Return False
        End If
        Return True

    End Function

    '------------------------------------------------------------------------
    Public Enum CollectionType As Integer
        All
        Symbol
        NonSymbol
        Category
        NonCategory
        Geometry
        NonGeometry
        Family
        NonFamily
    End Enum

    '------------------------------------------------------------------------
    '  checks if the element is a given element kind. 
    Public Shared Function isTypeOf(ByVal rvtApp As Autodesk.Revit.Application, _
    ByVal elem As Autodesk.Revit.Element, ByVal type As Integer) As Boolean

        If type = CollectionType.All Then  ' save all
            Return True

        ElseIf type = CollectionType.Symbol Then ' Symbol  
            If (isSymbol(elem)) Then
                Return True
            End If

        ElseIf type = CollectionType.NonSymbol Then ' Non-Symbol 
            If Not (isSymbol(elem)) Then
                Return True
            End If

        ElseIf type = CollectionType.Category Then ' Category  
            If (hasCategory(elem)) Then
                Return True
            End If

        ElseIf type = CollectionType.NonCategory Then ' Non-Category 
            If Not (hasCategory(elem)) Then
                Return True
            End If

        ElseIf type = CollectionType.Geometry Then ' Geometry 
            If (hasGeometry(rvtApp, elem)) Then
                Return True
            End If

        ElseIf type = CollectionType.NonGeometry Then ' No-Geometry
            If Not (hasGeometry(rvtApp, elem)) Then
                Return True
            End If

        ElseIf type = CollectionType.Family Then
            If (isFamily(elem)) Then
                Return True
            End If

        End If

        Return False

    End Function

    '----------------------------------------------------------------------------------------------------
    '  Collect all the elements in the active document. 
    '
    Public Shared Function CollectElements(ByVal rvtApp As Autodesk.Revit.Application, _
    ByVal type As CollectionType) As Autodesk.Revit.ElementSet

        '  a container of the elements we want to collect. 
        Dim resultSet As Autodesk.Revit.ElementSet = rvtApp.Create.NewElementSet()

        '  get elements in the document. 
        Dim itr As Autodesk.Revit.ElementIterator = rvtApp.ActiveDocument.Elements

        '  go over the elements and collect them in a element set. 
        Dim elem As Autodesk.Revit.Element

        Do While (itr.MoveNext)
            elem = itr.Current
            If isTypeOf(rvtApp, elem, type) Then
                resultSet.Insert(elem)
            End If
        Loop

        ' finishing up.
        Return resultSet

    End Function

    '------------------------------------------------------------------------
    ' override function (2/2) 
    Public Shared Function CollectElements(ByVal rvtApp As Autodesk.Revit.Application, _
    ByVal elemSet As Autodesk.Revit.ElementSet, ByVal type As CollectionType) _
    As Autodesk.Revit.ElementSet

        '  a container of the elements we want to collect. 
        Dim resultSet As Autodesk.Revit.ElementSet = rvtApp.Create.NewElementSet()

        '  go over the elements and collect them in a element set. 
        Dim elem As Autodesk.Revit.Element

        For Each elem In elemSet
            If isTypeOf(rvtApp, elem, type) Then
                resultSet.Insert(elem)
            End If
        Next

        ' finishing up.
        Return resultSet

    End Function

    '------------------------------------------------------------------------
    '  collect all the elements in an ElementSet 
    ' 
    Public Shared Function CollectElements(ByVal rvtApp As Autodesk.Revit.Application) _
    As Autodesk.Revit.ElementSet

        '  a container of the elements we want to collect. 
        Dim resultSet As Autodesk.Revit.ElementSet = rvtApp.Create.NewElementSet()

        '  get elements in the document. 
        Dim itr As Autodesk.Revit.ElementIterator = rvtApp.ActiveDocument.Elements

        '  go over the elements and collect them in a element set. 
        Dim elem As Autodesk.Revit.Element

        Do While (itr.MoveNext)
            elem = itr.Current
            resultSet.Insert(elem)
        Loop

        Return resultSet

    End Function


    '------------------------------------------------------------------------
    '  collect elements of the given element type. 
    '
    Public Shared Function CollectElementsOfType(ByVal rvtApp As Autodesk.Revit.Application, _
    ByVal targetType As Type) As Autodesk.Revit.ElementSet

        '  a container of the elements we want to collect. 
        Dim resultSet As Autodesk.Revit.ElementSet = rvtApp.Create.NewElementSet()

        '  get elements in the document. 
        Dim itr As Autodesk.Revit.ElementIterator = rvtApp.ActiveDocument.Elements

        '  go over the elements and collect them in a element set. 
        Dim elem As Autodesk.Revit.Element

        Do While (itr.MoveNext)
            elem = itr.Current
            If elem.GetType Is targetType Then
                resultSet.Insert(elem)
            End If
        Loop

        Return resultSet

    End Function

    '-------------------------------------------------------------------------
    '  collect elements of the given element type and name.  (this may be only one?) 
    '
    Public Shared Function CollectElementsOfTypeName(ByVal rvtApp As Autodesk.Revit.Application, _
    ByVal targetType As Type, ByVal targetName As String) As Autodesk.Revit.ElementSet

        '  a container of the elements we want to collect. 
        Dim resultSet As Autodesk.Revit.ElementSet = rvtApp.Create.NewElementSet()

        '  get elements in the document. 
        Dim itr As Autodesk.Revit.ElementIterator = rvtApp.ActiveDocument.Elements

        '  go over the elements and collect them in a element set. 
        Dim elem As Autodesk.Revit.Element

        Do While (itr.MoveNext)
            elem = itr.Current
            If elem.GetType Is targetType Then
                If elem.Name = targetName Then
                    resultSet.Insert(elem)
                End If
            End If
        Loop

        Return resultSet

    End Function

    '-------------------------------------------------------------------------
    ' searches the entire Revit project and sorts the elements based upon category. 
    ' Revit Symbols (Types) are ignored as we are only interested in instances of elements.
    '
    Public Shared Function CreateInstanceMap(ByVal rvtApp As Autodesk.Revit.Application) _
    As Autodesk.Revit.Collections.Map

        Return CreateNonSymbolCategoryMap(rvtApp)

    End Function

    Public Shared Function CreateNonSymbolCategoryMap(ByVal rvtApp As Autodesk.Revit.Application) _
    As Autodesk.Revit.Collections.Map

        '  a container of the elements map we want to collect. 
        Dim elemMap As Autodesk.Revit.Collections.Map = rvtApp.Create.NewMap()

        '  document object
        Dim rvtDoc As Autodesk.Revit.Document = rvtApp.ActiveDocument

        '  loop through the entire revit project 
        Dim iter As IEnumerator = rvtDoc.Elements()
        Dim elem As Autodesk.Revit.Element

        Do While (iter.MoveNext())

            elem = iter.Current
            If Not (TypeOf elem Is Autodesk.Revit.Symbol) Then

                ' retrieve the category of the element
                Dim cat As Autodesk.Revit.Category = elem.Category

                If Not (cat Is Nothing) Then

                    Dim elemSet As Autodesk.Revit.ElementSet
                    ' if this is a category that we have seen before, 
                    'then add this element to that set,
                    ' otherwise create a new set and add the element to it.
                    If elemMap.Contains(cat.Name) Then
                        elemSet = elemMap.Item(cat.Name)
                    Else
                        elemSet = rvtApp.Create.NewElementSet()
                        elemMap.Insert(cat.Name, elemSet)
                    End If
                    elemSet.Insert(elem)

                End If

            End If

        Loop

        Return elemMap

    End Function

    '-------------------------------------------------------------------------
    '  create map by category.  
    '
    Public Shared Function CreateCategoryMap(ByVal rvtApp As Autodesk.Revit.Application, _
    ByVal elements As Autodesk.Revit.ElementSet) As Autodesk.Revit.Collections.Map

        '  a container of the elements map we want to collect. 
        Dim elemMap As Autodesk.Revit.Collections.Map = rvtApp.Create.NewMap()

        '  loop through the element set  
        Dim elem As Autodesk.Revit.Element

        For Each elem In elements

            ' retrieve the category of the element
            Dim cat As Autodesk.Revit.Category = elem.Category

            If Not (cat Is Nothing) Then

                Dim elemSet As Autodesk.Revit.ElementSet
                ' if this is a category that we have seen before, then add this element to that set,
                ' otherwise create a new set and add the element to it.
                If elemMap.Contains(cat.Name) Then
                    elemSet = elemMap.Item(cat.Name)
                Else
                    elemSet = rvtApp.Create.NewElementSet()
                    elemMap.Insert(cat.Name, elemSet)
                End If
                elemSet.Insert(elem)

            End If

        Next

        Return elemMap

    End Function
    '-------------------------------------------------------------------------
    '  Collect elemens whose type is not Symbol or Type. 
    '
    Public Shared Function CollectElementsNonSymbol(ByVal rvtApp As Autodesk.Revit.Application) _
    As Autodesk.Revit.ElementSet

        '  a container of the elements we want to collect. 
        Dim elemSet As Autodesk.Revit.ElementSet = rvtApp.Create.NewElementSet

        '  document object
        Dim rvtDoc As Autodesk.Revit.Document = rvtApp.ActiveDocument

        '  loop through the entire revit project 
        Dim iter As IEnumerator = rvtDoc.Elements()
        Dim elem As Autodesk.Revit.Element

        Do While (iter.MoveNext())

            elem = iter.Current
            If Not (TypeOf elem Is Autodesk.Revit.Symbol) Then
                elemSet.Insert(elem)
            End If

        Loop

        Return elemSet

    End Function

    '-------------------------------------------------------------------------
    '  Non Symbol & No Category 
    '
    Public Shared Function CollectElementsNonSymbolNoCategory( _
    ByVal rvtApp As Autodesk.Revit.Application) As Autodesk.Revit.ElementSet

        '  a container of the elements we want to collect. 
        Dim elemSet As Autodesk.Revit.ElementSet = rvtApp.Create.NewElementSet

        '  document object
        Dim rvtDoc As Autodesk.Revit.Document = rvtApp.ActiveDocument

        '  loop through the entire revit project 
        Dim iter As IEnumerator = rvtDoc.Elements()
        Dim elem As Autodesk.Revit.Element

        Do While (iter.MoveNext())

            elem = iter.Current
            If Not (TypeOf elem Is Autodesk.Revit.Symbol) Then

                Dim cat As Autodesk.Revit.Category = elem.Category
                ' no category. 
                If (cat Is Nothing) Then
                    elemSet.Insert(elem)
                End If

            End If

        Loop

        Return elemSet

    End Function


    '==========================================================================
    '
    '  Element Search 
    '
    '==========================================================================

    '  find an element with the givem name.  find only the first one. 
    '
    Public Shared Function FindElementOfName(ByVal elemSet As Autodesk.Revit.ElementSet, _
    ByVal targetName As String) As Autodesk.Revit.Element

        Dim elem As Autodesk.Revit.Element
        For Each elem In elemSet
            If elem.Name = targetName Then
                Return elem ' return the first one we find. 
            End If
        Next

        Return Nothing ' failed to find one. 

    End Function

    '-------------------------------------------------------------------------
    ' 
    Public Shared Function FindElementFromId(ByVal rvtApp As Autodesk.Revit.Application, _
    ByVal targetId As Autodesk.Revit.ElementId) As Autodesk.Revit.Element

        Dim elem As Autodesk.Revit.Element = rvtApp.ActiveDocument.Element(targetId)
        Return elem

    End Function

    '==========================================================================
    '
    '  RGB/Integer conversion helper. 
    '
    '==========================================================================

    Public Shared Function IntegerToRGBString(ByVal ival As Integer) As String

        Dim bs As Byte() = Nothing
        IntegerToRGB(ival, bs)
        Dim str As String
        str = " (RGB: " + bs(0).ToString + " " + bs(1).ToString + " " + bs(2).ToString + ")"
        Return str

    End Function

    '--------------------------------------------------------------------------
    Public Shared Sub IntegerToRGB(ByVal ival As Integer, ByRef bs As Byte())

        bs = System.BitConverter.GetBytes(ival)

    End Sub


End Class
