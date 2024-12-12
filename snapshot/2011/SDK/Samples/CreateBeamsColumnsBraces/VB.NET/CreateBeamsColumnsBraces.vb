'
' (C) Copyright 2003-2010 by Autodesk, Inc.
'
' Permission to use, copy, modify, and distribute this software in
' object code form for any purpose and without fee is hereby granted
' provided that the above copyright notice appears in all copies and
' that both that copyright notice and the limited warranty and
' restricted rights notice below appear in all supporting
' documentation.
'
' AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL ITS FAULTS.
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
Imports System.IO
Imports System.Collections
Imports System.Windows.Forms

Imports Autodesk.Revit
Imports Autodesk.Revit.Collections
Imports Autodesk.Revit.DB.Events
Imports Autodesk.Revit.DB
Imports Autodesk.Revit.UI
Imports Autodesk.Revit.DB.Structure
Imports Autodesk.Revit.Creation

' Create Beams, Columns & Braces according to user's input information
<Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Automatic)> _
<Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Automatic)> _
<Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)> _
Public Class Command
    Implements IExternalCommand 'ToDo: Add Implements Clauses for implementation methods of these interface(s)
    Private m_revit As Autodesk.Revit.UI.UIApplication = Nothing

    Private m_columnMaps As New ArrayList                'list of columns' type
    Private m_beamMaps As New ArrayList                  'list of beams' type
    Private m_braceMaps As New ArrayList                 'list of braces' type
    Private levels As New SortedList                     'list of list sorted by their elevations
    Private m_matrixUV(,) As Autodesk.Revit.DB.UV  '2D coordinates of matrix

    ''' <summary>
    ''' list of all type of columns
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property ColumnMaps() As ArrayList
        Get
            Return m_columnMaps
        End Get
    End Property

    ''' <summary>
    ''' list of all type of beams
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property BeamMaps() As ArrayList
        Get
            Return m_beamMaps
        End Get
    End Property

    ''' <summary>
    ''' list of all type of braces
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property BraceMaps() As ArrayList
        Get
            Return m_braceMaps
        End Get
    End Property

    ''' <summary>
    ''' Implement this method as an external command for Revit.
    ''' </summary>
    ''' <param name="revit">An object that is passed to the external application 
    ''' which contains data related to the command, 
    ''' such as the application object and active view.</param>
    ''' <param name="message">A message that can be set by the external application 
    ''' which will be displayed if a failure or cancellation is returned by 
    ''' the external command.</param>
    ''' <param name="elements">A set of elements to which the external application 
    ''' can add elements that are to be highlighted in case of failure or cancellation.</param>
    ''' <returns>Return the status of the external command. 
    ''' A result of Succeeded means that the API external method functioned as expected. 
    ''' Cancelled can be used to signify that the user cancelled the external operation 
    ''' at some point. Failure should be returned if the application is unable to proceed with 
    ''' the operation.</returns>
    Public Function Execute(ByVal revit As Autodesk.Revit.UI.ExternalCommandData, ByRef message As String, ByVal elements As Autodesk.Revit.DB.ElementSet) As Autodesk.Revit.UI.Result Implements Autodesk.Revit.UI.IExternalCommand.Execute
        Try
            m_revit = revit.Application

            'if initialize failed return Result.Failed
            Dim initializeOK As Boolean = Initialize()
            If Not initializeOK Then
                Return Autodesk.Revit.UI.Result.Failed
            End If

            Dim displayForm As New CreateBeamsColumnsBracesForm(Me)
            Using (displayForm)
                If displayForm.ShowDialog() <> DialogResult.OK Then
                    Return Autodesk.Revit.UI.Result.Cancelled
                End If
            End Using

            Return Autodesk.Revit.UI.Result.Succeeded
        Catch ex As Exception
            message = ex.Message
            Return Autodesk.Revit.UI.Result.Failed
        End Try

    End Function

    ''' <summary>
    ''' check the number of floors is less than the number of levels, 
    ''' create beams, columns abd braces according to selected types
    ''' </summary>
    ''' <param name="columnObject">type of column</param>
    ''' <param name="beamObject">type of beam</param>
    ''' <param name="braceObject">type of brace</param>
    ''' <param name="floorNumber">number of floor</param>
    ''' <returns>number of floors is less than the number of levels and create successfully then return true</returns>
    ''' <remarks></remarks>
    Public Function AddInstance(ByVal columnObject As Object, ByVal beamObject As Object, ByVal braceObject As Object, ByVal floorNumber As Integer) As Boolean
        'whether floor number less than levels number
        If floorNumber >= levels.Count Then
            MessageBox.Show("The number of levels must be added.", "Revit")
            Return False
        End If

        Dim columnSymbol As Autodesk.Revit.DB.FamilySymbol = Nothing

        If TypeOf columnObject Is Autodesk.Revit.DB.FamilySymbol Then
            columnSymbol = columnObject
        End If

        Dim beamSymbol As Autodesk.Revit.DB.FamilySymbol = Nothing
        If TypeOf beamObject Is Autodesk.Revit.DB.FamilySymbol Then
            beamSymbol = beamObject
        End If

        Dim braceSymbol As Autodesk.Revit.DB.FamilySymbol = Nothing
        If TypeOf braceObject Is Autodesk.Revit.DB.FamilySymbol Then
            braceSymbol = braceObject
        End If

        'any symbol is null then the command failed
        If columnSymbol Is Nothing OrElse beamSymbol Is Nothing OrElse braceSymbol Is Nothing Then
            Return False
        End If

        Try

            For k As Integer = 0 To floorNumber - 1 'iterate levels from lower one to higher
                Dim baseLevel As Autodesk.Revit.DB.Level = levels.GetByIndex(k)
                Dim topLevel As Autodesk.Revit.DB.Level = levels.GetByIndex((k + 1))

                'place column of this level
                Dim point2D As Autodesk.Revit.DB.UV
                For Each point2D In m_matrixUV
                    PlaceColumn(point2D, columnSymbol, baseLevel, topLevel)
                Next point2D

                Dim matrixXSize As Integer = m_matrixUV.GetLength(0) 'length of matrix's x range
                Dim matrixYSize As Integer = m_matrixUV.GetLength(1) 'length of matrix's y range
                'iterate coordinate both in x direction and y direction and create beams and braces
                For j As Integer = 0 To matrixYSize - 1

                    For i As Integer = 0 To matrixXSize - 1
                        'create beams and braces in x direction
                        If i <> matrixXSize - 1 Then
                            PlaceBrace(m_matrixUV(i, j), m_matrixUV(i + 1, j), baseLevel, topLevel, braceSymbol, True)
                        End If
                        'create beams and braces in y direction
                        If j <> matrixYSize - 1 Then
                            PlaceBrace(m_matrixUV(i, j), m_matrixUV(i, j + 1), baseLevel, topLevel, braceSymbol, False)
                        End If
                    Next i
                Next j

                For j As Integer = 0 To matrixYSize - 1

                    For i As Integer = 0 To matrixXSize - 1
                        'create beams and braces in x direction
                        If i <> matrixXSize - 1 Then
                            PlaceBeam(m_matrixUV(i, j), m_matrixUV(i + 1, j), baseLevel, topLevel, beamSymbol)
                        End If
                        'create beams and braces in y direction
                        If j <> matrixYSize - 1 Then
                            PlaceBeam(m_matrixUV(i, j), m_matrixUV(i, j + 1), baseLevel, topLevel, beamSymbol)
                        End If
                    Next i
                Next j
            Next k
        Catch
            Return False
        End Try

        Return True
    End Function

    ''' <summary>
    ''' generate 2D coordinates of matrix according to parameters
    ''' </summary>
    ''' <param name="xNumber">Number of Columns in the X direction</param>
    ''' <param name="yNumber">Number of Columns in the Y direction</param>
    ''' <param name="distance">Distance between columns</param>
    ''' <remarks></remarks>
    Public Sub CreateMatrix(ByVal xNumber As Integer, ByVal yNumber As Integer, ByVal distance As Double)
        m_matrixUV = New Autodesk.Revit.DB.UV(xNumber - 1, yNumber - 1) {}

        Dim i As Integer
        For i = 0 To xNumber - 1
            Dim j As Integer
            For j = 0 To yNumber - 1
                m_matrixUV(i, j) = New Autodesk.Revit.DB.UV(i * distance, j * distance)
            Next j
        Next i
    End Sub

    ''' <summary>
    ''' iterate all the symbols of levels, columns, beams and braces 
    ''' </summary>
    ''' <returns>A value that signifies if the initialization was successful for true or failed for false</returns>
    ''' <remarks></remarks>
    Private Function Initialize() As Boolean
        Try
            'get elements in the document which type == Level or type ==  Family
            Dim filter1 As Autodesk.Revit.DB.ElementClassFilter
            Dim filter2 As Autodesk.Revit.DB.ElementClassFilter
            filter1 = New Autodesk.Revit.DB.ElementClassFilter(GetType(Autodesk.Revit.DB.Level))
            filter2 = New Autodesk.Revit.DB.ElementClassFilter(GetType(Autodesk.Revit.DB.Family))
            Dim orFilter As Autodesk.Revit.DB.LogicalOrFilter
            orFilter = New Autodesk.Revit.DB.LogicalOrFilter(filter1, filter2)
            Dim collector As Autodesk.Revit.DB.FilteredElementCollector
            collector = New Autodesk.Revit.DB.FilteredElementCollector(m_revit.ActiveUIDocument.Document)
            collector.WherePasses(orFilter)
            Dim i As IEnumerator
            i = collector.GetElementIterator

            i.Reset()
            Dim moreElement As Boolean = i.MoveNext()
            While moreElement
                Dim o As Object = i.Current

                'add level to list
                Dim level As Autodesk.Revit.DB.Level
                level = Nothing
                If TypeOf o Is Autodesk.Revit.DB.Level Then
                    level = CType(o, Autodesk.Revit.DB.Level)
                End If '

                If Not (level Is Nothing) Then
                    levels.Add(level.Elevation, level)
                    GoTo nextLoop
                End If

                Dim f As Autodesk.Revit.DB.Family = Nothing
                If TypeOf o Is Autodesk.Revit.DB.Family Then
                    f = o
                End If

                If f Is Nothing Then
                    GoTo nextLoop
                End If

                Dim symbol As Object
                For Each symbol In f.Symbols
                    Dim familyType As Autodesk.Revit.DB.FamilySymbol = symbol '

                    If familyType Is Nothing Then
                        GoTo nextLoop
                    End If
                    If familyType.Category Is Nothing Then
                        GoTo nextLoop
                    End If

                    'add symbols of beams and braces to lists 
                    Dim categoryName As String = familyType.Category.Name
                    If "Structural Framing" = categoryName Then
                        m_beamMaps.Add(New SymbolMap(familyType))
                        m_braceMaps.Add(New SymbolMap(familyType))
                    ElseIf "Structural Columns" = categoryName Then
                        m_columnMaps.Add(New SymbolMap(familyType))
                    End If
                Next symbol
nextLoop:
                moreElement = i.MoveNext()
            End While
        Catch
            Return False
        End Try
        Return True
    End Function

    ''' <summary>
    ''' create column of certain type in certain position
    ''' </summary>
    ''' <param name="point2D">2D coordinate of the col umn</param>
    ''' <param name="columnType">type of column</param>
    ''' <param name="baseLevel">the base level of the column</param>
    ''' <param name="topLevel">the top level of the column</param>
    ''' <remarks></remarks>
    Private Sub PlaceColumn(ByVal point2D As Autodesk.Revit.DB.UV, ByVal columnType As Autodesk.Revit.DB.FamilySymbol, ByVal baseLevel As Autodesk.Revit.DB.Level, ByVal topLevel As Autodesk.Revit.DB.Level)
        'create column of certain type in certain level and start point 
        Dim point As New Autodesk.Revit.DB.XYZ(point2D.U, point2D.V, 0)
        Dim structuralType As Autodesk.Revit.DB.Structure.StructuralType
        structuralType = Autodesk.Revit.DB.Structure.StructuralType.Column
        Dim column As Autodesk.Revit.DB.FamilyInstance = m_revit.ActiveUIDocument.Document.Create.NewFamilyInstance(point, columnType, topLevel, structuralType)

        'set baselevel & toplevel of the column
        If Not (column Is Nothing) Then
            Dim baseLevelParameter As Parameter = column.Parameter(Autodesk.Revit.DB.BuiltInParameter.FAMILY_BASE_LEVEL_PARAM)
            Dim topLevelParameter As Parameter = column.Parameter(Autodesk.Revit.DB.BuiltInParameter.FAMILY_TOP_LEVEL_PARAM)
            Dim topOffsetParameter As Parameter = column.Parameter(BuiltInParameter.FAMILY_TOP_LEVEL_OFFSET_PARAM)
            Dim baseOffsetParameter As Parameter = column.Parameter(BuiltInParameter.FAMILY_BASE_LEVEL_OFFSET_PARAM)

            If Not (baseLevelParameter Is Nothing) Then
                Dim baseLevelId As Autodesk.Revit.DB.ElementId
                baseLevelId = baseLevel.Id
                baseLevelParameter.Set(baseLevelId)
            End If

            If Not (topLevelParameter Is Nothing) Then
                Dim topLevelId As Autodesk.Revit.DB.ElementId
                topLevelId = topLevel.Id
                topLevelParameter.Set(topLevelId)
            End If

            If Not (topOffsetParameter Is Nothing) Then
                topOffsetParameter.Set(0.0)
            End If

            If Not (baseOffsetParameter Is Nothing) Then
                baseOffsetParameter.Set(0.0)
            End If
        End If
    End Sub

    ''' <summary>
    ''' create beam of certain type in certain position
    ''' </summary>
    ''' <param name="point2D1">one point of the location line in 2D</param>
    ''' <param name="point2D2">another point of the location line in 2D</param>
    ''' <param name="baseLevel">the base level of the beam</param>
    ''' <param name="topLevel">the top level of the beam</param>
    ''' <param name="beamType">type of beam</param>
    ''' <remarks></remarks>
    Private Sub PlaceBeam(ByVal point2D1 As Autodesk.Revit.DB.UV, ByVal point2D2 As Autodesk.Revit.DB.UV, ByVal baseLevel As Autodesk.Revit.DB.Level, ByVal topLevel As Autodesk.Revit.DB.Level, ByVal beamType As Autodesk.Revit.DB.FamilySymbol)
        Dim height As Double = topLevel.Elevation
        Dim startPoint As New Autodesk.Revit.DB.XYZ(point2D1.U, point2D1.V, height)
        Dim endPoint As New Autodesk.Revit.DB.XYZ(point2D2.U, point2D2.V, height)

        Dim structuralType As Autodesk.Revit.DB.Structure.StructuralType = Autodesk.Revit.DB.Structure.StructuralType.Beam
        Dim beam As Autodesk.Revit.DB.FamilyInstance = m_revit.ActiveUIDocument.Document.Create.NewFamilyInstance(startPoint, beamType, topLevel, structuralType)

        Dim beamCurve As LocationCurve = beam.Location '

        If Not (beamCurve Is Nothing) Then
            Dim line As Line = m_revit.Application.Create.NewLineBound(startPoint, endPoint)
            beamCurve.Curve = line
        End If
    End Sub

    ''' <summary>
    ''' create brace of certain type in certain position between two adjacent columns
    ''' </summary>
    ''' <param name="point2D1">one point of the location line in 2D</param>
    ''' <param name="point2D2">another point of the location line in 2D</param>
    ''' <param name="baseLevel">the base level of the brace</param>
    ''' <param name="topLevel">the top level of the brace</param>
    ''' <param name="braceType">type of beam</param>
    ''' <param name="isXDirection">whether the location line is in x direction</param>
    ''' <remarks></remarks>
    Private Sub PlaceBrace(ByVal point2D1 As Autodesk.Revit.DB.UV, ByVal point2D2 As Autodesk.Revit.DB.UV, ByVal baseLevel As Autodesk.Revit.DB.Level, ByVal topLevel As Autodesk.Revit.DB.Level, ByVal braceType As Autodesk.Revit.DB.FamilySymbol, ByVal isXDirection As Boolean)
        'get the start points and end points of location lines of two braces
        Dim topHeight As Double = topLevel.Elevation
        Dim baseHeight As Double = baseLevel.Elevation
        Dim middleElevation As Double = (topHeight + baseHeight) / 2
        Dim middleHeight As Double = (topHeight + baseHeight) / 2
        Dim startPoint As New Autodesk.Revit.DB.XYZ(point2D1.U, point2D1.V, middleElevation)
        Dim endPoint As New Autodesk.Revit.DB.XYZ(point2D2.U, point2D2.V, middleElevation)
        Dim middlePoint As Autodesk.Revit.DB.XYZ
        If isXDirection Then
            middlePoint = New Autodesk.Revit.DB.XYZ((point2D1.U + point2D2.U) / 2, point2D2.V, topHeight)
        Else
            middlePoint = New Autodesk.Revit.DB.XYZ(point2D2.U, (point2D1.V + point2D2.V) / 2, topHeight)
        End If

        'create two brace and set their location line
        Dim structuralType As Autodesk.Revit.DB.Structure.StructuralType = Autodesk.Revit.DB.Structure.StructuralType.Brace
        Dim levelId As Autodesk.Revit.DB.ElementId = topLevel.Id
        Dim startLevelId As Autodesk.Revit.DB.ElementId = baseLevel.Id
        Dim endLevelId As Autodesk.Revit.DB.ElementId = topLevel.Id

        Dim firstBrace As Autodesk.Revit.DB.FamilyInstance = m_revit.ActiveUIDocument.Document.Create.NewFamilyInstance(startPoint, braceType, structuralType)
        Dim braceCurve1 As LocationCurve = firstBrace.Location '

        If Not (braceCurve1 Is Nothing) Then
            Dim line As Line = m_revit.Application.Create.NewLineBound(startPoint, middlePoint)
            braceCurve1.Curve = line
        End If

        Dim referenceLevel1 As Parameter = firstBrace.Parameter(BuiltInParameter.INSTANCE_REFERENCE_LEVEL_PARAM)
        If Not (referenceLevel1 Is Nothing) Then
            referenceLevel1.Set(levelId)
        End If

        Dim secondBrace As Autodesk.Revit.DB.FamilyInstance = m_revit.ActiveUIDocument.Document.Create.NewFamilyInstance(endPoint, braceType, baseLevel, structuralType)
        Dim braceCurve2 As LocationCurve = secondBrace.Location '

        If Not (braceCurve2 Is Nothing) Then
            Dim line As Line = m_revit.Application.Create.NewLineBound(endPoint, middlePoint)
            braceCurve2.Curve = line
        End If

        Dim referenceLevel2 As Parameter = secondBrace.Parameter(BuiltInParameter.INSTANCE_REFERENCE_LEVEL_PARAM)
        If Not (referenceLevel2 Is Nothing) Then
            referenceLevel2.Set(levelId)
        End If
    End Sub
End Class

''' <summary>
''' assistant class contains the symbol and its name
''' </summary>
''' <remarks></remarks>
Public Class SymbolMap
    Private m_symbolName As String = ""
    Private m_symbol As Autodesk.Revit.DB.FamilySymbol = Nothing

    ''' <summary>
    ''' constructor without parameter is forbidden
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub New()
    End Sub

    ''' <summary>
    ''' constructor
    ''' </summary>
    ''' <param name="symbol">family symbol</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal symbol As Autodesk.Revit.DB.FamilySymbol)
        m_symbol = symbol
        Dim familyName As String = ""
        If Not (symbol.Family Is Nothing) Then
            familyName = symbol.Family.Name
        End If
        m_symbolName = familyName + " : " + symbol.Name
    End Sub

    Public ReadOnly Property SymbolName() As String
        Get
            Return m_symbolName
        End Get
    End Property

    Public ReadOnly Property ElementType() As Autodesk.Revit.DB.FamilySymbol
        Get
            Return m_symbol
        End Get
    End Property
End Class
