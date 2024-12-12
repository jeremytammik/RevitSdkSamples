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

Namespace RstLabs
    'Revit Structure utilities
    Public Class RstUtils

        Public Shared Function GetAllLoadNatures(ByVal revitApp As Revit.Application) As ElementSet

            'The following commented code is for Revit 2008 and previous version. It works in Revit 2009 and afterward version too. However this method has a low performance.
            'There is a new feature named Element filter in Revit 2009 API, which can improve the performance.

            'Dim natures As ElementSet = revitApp.Create.NewElementSet
            'Dim iter As IEnumerator = revitApp.ActiveDocument.Elements
            'Do While (iter.MoveNext())
            '    Dim elem As Revit.Element = iter.Current
            '    If TypeOf elem Is LoadNature Then
            '        natures.Insert(elem)
            '    End If
            'Loop
            'Return natures

            '
            'getting all LoadNatures via element filter that provided in Revit 2009 API. This is quicker and simpler.
            '
            Dim listLoadNatures As List(Of Autodesk.Revit.Element) = New List(Of Revit.Element)
            Dim filterType As Revit.Filter = revitApp.Create.Filter.NewTypeFilter(GetType(LoadNature))
            Dim bRetVal As Boolean = revitApp.ActiveDocument.Elements(filterType, listLoadNatures)
            Return RacUtils.ConvertElementListToSet(revitApp, listLoadNatures)

        End Function

        Public Shared Function GetAllLoadCases(ByVal revitApp As Revit.Application) As ElementSet

            'The following commented code is for Revit 2008 and previous version. It works in Revit 2009 and afterward version too. However this method has a low performance.
            'There is a new feature named Element filter in Revit 2009 API, which can improve the performance.

            'Dim cases As ElementSet = revitApp.Create.NewElementSet

            'Dim iter As IEnumerator = revitApp.ActiveDocument.Elements
            'Do While (iter.MoveNext())
            '    Dim elem As Revit.Element = iter.Current
            '    If TypeOf elem Is LoadCase Then
            '        cases.Insert(elem)
            '    End If
            'Loop

            'Return cases

            '
            'getting all LoadCases via element filter that provided in Revit 2009 API. This is quicker and simpler.
            '
            Dim listLoadCases As List(Of Autodesk.Revit.Element) = New List(Of Revit.Element)
            Dim filterType As Revit.Filter = revitApp.Create.Filter.NewTypeFilter(GetType(LoadCase))
            Dim bRetVal As Boolean = revitApp.ActiveDocument.Elements(filterType, listLoadCases)
            Return RacUtils.ConvertElementListToSet(revitApp, listLoadCases)

        End Function

        Public Shared Function GetAllLoadCombinations(ByVal revitApp As Revit.Application) As ElementSet

            'The following commented code is for Revit 2008 and previous version. It works in Revit 2009 and afterward version too. However this method has a low performance.
            'There is a new feature named Element filter in Revit 2009 API, which can improve the performance.

            'Dim combinations As ElementSet = revitApp.Create.NewElementSet

            'Dim iter As IEnumerator = revitApp.ActiveDocument.Elements
            'Do While (iter.MoveNext())
            '    Dim elem As Revit.Element = iter.Current
            '    If TypeOf elem Is LoadCombination Then
            '        combinations.Insert(elem)
            '    End If
            'Loop

            'Return combinations

            '
            'getting all LoadCombinations via element filter that provided in Revit 2009 API. This is quicker and simpler.
            '
            Dim listLoadCombinations As List(Of Autodesk.Revit.Element) = New List(Of Revit.Element)
            Dim filterType As Revit.Filter = revitApp.Create.Filter.NewTypeFilter(GetType(LoadCombination))
            Dim bRetVal As Boolean = revitApp.ActiveDocument.Elements(filterType, listLoadCombinations)
            Return RacUtils.ConvertElementListToSet(revitApp, listLoadCombinations)


        End Function

        Public Shared Function GetAllLoadUsages(ByVal revitApp As Revit.Application) As ElementSet

            'The following commented code is for Revit 2008 and previous version. It works in Revit 2009 and afterward version too. However this method has a low performance.
            'There is a new feature named Element filter in Revit 2009 API, which can improve the performance.

            'Dim usages As ElementSet = revitApp.Create.NewElementSet

            'Dim iter As IEnumerator = revitApp.ActiveDocument.Elements
            'Do While (iter.MoveNext())
            '    Dim elem As Revit.Element = iter.Current
            '    If TypeOf elem Is LoadUsage Then
            '        usages.Insert(elem)
            '    End If
            'Loop

            'Return usages

            '
            'getting all LoadUsages via element filter that provided in Revit 2009 API. This is quicker and simpler.
            '
            Dim listLoadUsages As List(Of Autodesk.Revit.Element) = New List(Of Revit.Element)
            Dim filterType As Revit.Filter = revitApp.Create.Filter.NewTypeFilter(GetType(LoadUsage))
            Dim bRetVal As Boolean = revitApp.ActiveDocument.Elements(filterType, listLoadUsages)
            Return RacUtils.ConvertElementListToSet(revitApp, listLoadUsages)

        End Function

        Public Shared Function GetAllLoads(ByVal revitApp As Revit.Application) As ElementSet

            'The following commented code is for Revit 2008 and previous version. It works in Revit 2009 and afterward version too. However this method has a low performance.
            'There is a new feature named Element filter in Revit 2009 API, which can improve the performance.

            'Dim loads As ElementSet = revitApp.Create.NewElementSet

            'Dim iter As IEnumerator = revitApp.ActiveDocument.Elements
            'Do While (iter.MoveNext())
            '    Dim elem As Revit.Element = iter.Current
            '    If TypeOf elem Is LoadBase Then
            '        loads.Insert(elem)
            '    End If
            'Loop

            'Return loads

            '
            'getting all Loads via element filter that provided in Revit 2009 API. This is quicker and simpler.
            '
            Dim listLoadBases As List(Of Autodesk.Revit.Element) = New List(Of Revit.Element)
            Dim filterType As Revit.Filter = revitApp.Create.Filter.NewTypeFilter(GetType(LoadBase), True)
            Dim bRetVal As Boolean = revitApp.ActiveDocument.Elements(filterType, listLoadBases)
            Return RacUtils.ConvertElementListToSet(revitApp, listLoadBases)

        End Function

        ' More efficient if we loop only once and sort at the time
        Public Shared Sub GetAllSpecificLoads(ByVal revitApp As Revit.Application, _
                                              ByRef pointLoads As ElementSet, _
                                              ByRef lineLoads As ElementSet, _
                                              ByRef areaLoads As ElementSet)

            'The following commented code is for Revit 2008 and previous version. It works in Revit 2009 and afterward version too. However this method has a low performance.
            'There is a new feature named Element filter in Revit 2009 API, which can improve the performance.

            'Dim iter As IEnumerator = revitApp.ActiveDocument.Elements
            'Do While (iter.MoveNext())

            '
            'The following code is using the new feature of Element filter to improve the performance
            '
            Dim listLoadBases As List(Of Autodesk.Revit.Element) = New List(Of Revit.Element)
            Dim filterType As Revit.Filter = revitApp.Create.Filter.NewTypeFilter(GetType(LoadBase), True)
            Dim bRetVal As Boolean = revitApp.ActiveDocument.Elements(filterType, listLoadBases)
            For Each elem As Revit.Element In listLoadBases
                If TypeOf elem Is PointLoad Then
                    pointLoads.Insert(elem)
                ElseIf TypeOf elem Is LineLoad Then
                    lineLoads.Insert(elem)
                ElseIf TypeOf elem Is AreaLoad Then
                    areaLoads.Insert(elem)
                End If
            Next

        End Sub


        Public Shared Function GetAllLoadSymbols(ByVal revitApp As Revit.Application) As ElementSet

            Dim symbols As ElementSet = revitApp.Create.NewElementSet
            'The following commented code is for Revit 2008 and previous version. It works in Revit 2009 and afterward version too. However this method has a low performance.
            'There is a new feature named Element filter in Revit 2009 API, which can improve the performance.

            'Dim catLoads As Category = revitApp.ActiveDocument.Settings.Categories.Item(BuiltInCategory.OST_Loads)
            'Dim iter As IEnumerator = revitApp.ActiveDocument.Elements
            'Do While (iter.MoveNext())
            '    Dim elem As Revit.Element = iter.Current
            '    If TypeOf elem Is Symbol Then
            '        Try
            '            Dim cat As Category = elem.Category
            '            ' new
            '            'If cat.Name = "Structural Loads" Then
            '            If cat.Equals(catLoads) Then
            '                symbols.Insert(elem)
            '            End If
            '        Catch
            '        End Try
            '    End If
            'Loop

            'Dim symbols As ElementSet = revitApp.Create.NewElementSet
            'Revit 2009 added new classes to represent Load symbols, such as LoadTypeBase, PointLoadType,LineLoadType and AreaLoadType.
            'to find out all load types, we can compare the class instead of comparing Category.
            'This kind of iterate elements in database has low performance.
            'Dim iter As IEnumerator = revitApp.ActiveDocument.Elements
            'Do While (iter.MoveNext())
            '    Dim loadType As LoadTypeBase = iter.Current
            '    If TypeOf loadType Is LoadTypeBase Then
            '        symbols.Insert(loadType)
            '    End If

            'Loop

            'Return symbols

            '
            'The following code is using Element filter to improve the performance. It works in Revit 2009 and afterward version
            '
            Dim listLoadTypeBases As List(Of Autodesk.Revit.Element) = New List(Of Revit.Element)
            Dim filterType As Revit.Filter = revitApp.Create.Filter.NewTypeFilter(GetType(LoadTypeBase), True)
            Dim bRetVal As Boolean = revitApp.ActiveDocument.Elements(filterType, listLoadTypeBases)
            Return RacUtils.ConvertElementListToSet(revitApp, listLoadTypeBases)


        End Function

        Public Shared Function GetPointLoadSymbols(ByVal revitApp As Revit.Application) As ElementSet

            Dim symbols As ElementSet = revitApp.Create.NewElementSet

            'The following commented code is for Revit 2008 and previous version. It works in Revit 2009 and afterward version too. However this method has a low performance.
            'There is a new feature named Element filter in Revit 2009 API, which can improve the performance.

            ' And use the new method to determine Point type.  So the following codes are commented.
            ' This code work for Revit 2008. It can also work for Revit 2009 too.

            'Dim iter As IEnumerator = revitApp.ActiveDocument.Elements
            'Do While (iter.MoveNext())
            '    Dim elem As Revit.Element = iter.Current
            '    If TypeOf elem Is Symbol Then
            '        Try
            '            If elem.Category.Name = "Structural Loads" Then
            '                Try ' this is failing now in RS3/4 - was OK in RS2 !? :-( - IS OK IN 2008
            '                    If elem.Parameter(BuiltInParameter.SYMBOL_FAMILY_NAME_PARAM).AsString.Equals("Point Loads") Then
            '                        symbols.Insert(elem)
            '                    End If
            '                Catch ex As Exception
            '                End Try
            '            End If
            '        Catch
            '        End Try
            '    End If
            'Loop

            'Revit 2009 added new classes to represent Point load type, it is PointLoadType.
            'to find out point load types, we can compare the class PointLoadType instead of comparing Category.
            'This method to iterate the whole document to find the specific element has lowwer performance.
            'Dim iter As IEnumerator = revitApp.ActiveDocument.Elements(GetType(PointLoadType))
            'Do While (iter.MoveNext())
            '    Dim elem As Revit.Element = iter.Current
            '    If TypeOf elem Is PointLoadType Then
            '        symbols.Insert(elem)
            '    End If
            'Loop
            'Return symbols

            '
            'The following code is using the new feature of Element filter to improve the performance. It works in Revit 2009 and afterward version
            '
            Dim listLoadTypeBases As List(Of Autodesk.Revit.Element) = New List(Of Revit.Element)
            Dim filterType As Revit.Filter = revitApp.Create.Filter.NewTypeFilter(GetType(PointLoadType), True)
            Dim bRetVal As Boolean = revitApp.ActiveDocument.Elements(filterType, listLoadTypeBases)
            Return RacUtils.ConvertElementListToSet(revitApp, listLoadTypeBases)

        End Function

        ' Helper to get all STRUCTURAL Walls
        Shared Function GetAllStructuralWalls(ByVal revitApp As Revit.Application) As ElementSet

            Dim elems As ElementSet = revitApp.Create.NewElementSet

            'The following commented code is for Revit 2008 and previous version. It works in Revit 2009 and afterward version too. However this method has a low performance.
            'There is a new feature named Element filter in Revit 2009 API, which can improve the performance.

            'Dim iter As IEnumerator = revitApp.ActiveDocument.Elements
            'Do While (iter.MoveNext())
            'Dim elem As Revit.Element = iter.Current

            '
            'The following code is using Element filter to improve the performance. It works in Revit 2009 and afterward version
            '
            Dim listWalls As List(Of Autodesk.Revit.Element) = New List(Of Revit.Element)
            Dim filterType As Revit.Filter = revitApp.Create.Filter.NewTypeFilter(GetType(Wall))
            Dim bRetVal As Boolean = revitApp.ActiveDocument.Elements(filterType, listWalls)
            For Each elem As Revit.Element In listWalls
                ' For Wall (one of the Host objects), there is a specific class!
                If TypeOf elem Is Wall Then
                    Dim w As Wall = elem

                    '' We could check if the Wall is anything but Non-bearing in one of the two following ways:...

                    ' this works only in 8.1, not in 9/9.1!
                    'If Not w.Parameter(BuiltInParameter.WALL_STRUCTURAL_USAGE_TEXT_PARAM).AsString.Equals("Non-bearing") Then
                    '    '...
                    'End If
                    ''OR better

                    ' this works both in 8.1 and 9/9.1!
                    'If Not w.Parameter(BuiltInParameter.WALL_STRUCTURAL_USAGE_PARAM).AsInteger = 0 Then
                    '    '...
                    'End If

                    '... but it's more generic and precise to make sure that Analytical Model exists (in theory, one
                    '    can set the wall to bearing and still uncheck Analytical)
                    Dim anaMod As AnalyticalModel
                    Try
                        anaMod = w.AnalyticalModel
                        If Not anaMod Is Nothing Then
                            elems.Insert(elem)
                        End If
                    Catch
                    End Try
                End If
            Next

            Return elems

        End Function

        ' Helper to get all STRUCTURAL Floors
        Shared Function GetAllStructuralFloors(ByVal revitApp As Revit.Application) As ElementSet

            Dim elems As ElementSet = revitApp.Create.NewElementSet
            'The following commented code is for Revit 2008 and previous version. It works in Revit 2009 and afterward version too. However this method has a low performance.
            'There is a new feature named Element filter in Revit 2009 API, which can improve the performance.

            'Dim iter As IEnumerator = revitApp.ActiveDocument.Elements
            'Do While (iter.MoveNext())
            '    Dim elem As Revit.Element = iter.Current

            '
            'The following code is using Element filter to improve the performance. It works in Revit 2009 and afterward version
            '
            Dim listFloors As List(Of Autodesk.Revit.Element) = New List(Of Revit.Element)
            Dim filterType As Revit.Filter = revitApp.Create.Filter.NewTypeFilter(GetType(Floor))
            Dim bRetVal As Boolean = revitApp.ActiveDocument.Elements(filterType, listFloors)
            For Each elem As Revit.Element In listFloors
                If TypeOf elem Is Floor Then
                    Dim f As Floor = elem
                    Dim anaMod As AnalyticalModel
                    Try
                        anaMod = f.AnalyticalModel
                        If Not anaMod Is Nothing Then
                            ' For floors, looks like we need to have additional check: for non-Structural Floors
                            '    anaMod is NOT Nothing, but it IS empty!
                            Dim floorAnaMod As AnalyticalModelFloor = anaMod
                            If floorAnaMod.Curves.Size > 0 Then
                                elems.Insert(elem)
                            End If
                        End If
                    Catch
                    End Try
                End If
            Next

            Return elems

        End Function

        ' Helper to get all STRUCTURAL ContinuousFootings
        Shared Function GetAllStructuralContinuousFootings(ByVal revitApp As Revit.Application) As ElementSet

            Dim elems As ElementSet = revitApp.Create.NewElementSet
            'The following commented code is for Revit 2008 and previous version. It works in Revit 2009 and afterward version too. However this method has a low performance.
            'There is a new feature named Element filter in Revit 2009 API, which can improve the performance.

            'Dim iter As IEnumerator = revitApp.ActiveDocument.Elements
            'Do While (iter.MoveNext())
            '    Dim elem As Revit.Element = iter.Current
            Dim listContFootings As List(Of Autodesk.Revit.Element) = New List(Of Revit.Element)
            Dim filterType As Revit.Filter = revitApp.Create.Filter.NewTypeFilter(GetType(ContFooting))
            Dim bRetVal As Boolean = revitApp.ActiveDocument.Elements(filterType, listContFootings)
            For Each elem As Revit.Element In listContFootings
                If TypeOf elem Is ContFooting Then
                    Dim cf As ContFooting = elem
                    Dim anaMod As AnalyticalModel
                    Try
                        anaMod = cf.AnalyticalModel
                        If Not anaMod Is Nothing Then
                            elems.Insert(elem)
                        End If
                    Catch
                    End Try
                End If
            Next

            Return elems

        End Function

    End Class

End Namespace